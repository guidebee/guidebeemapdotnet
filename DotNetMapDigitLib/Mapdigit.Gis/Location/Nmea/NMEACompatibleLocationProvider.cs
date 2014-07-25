//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 29SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Threading;
using Mapdigit.Util;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Gis.Location.Nmea
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 29SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// A NMEACompatibleLocationProvider represents a NEMA0183 compatible devcie
    /// generating Locations.
    /// </summary>
    /// <remarks>
    /// Applications obtain NMEACompatibleLocationProvider instances (classes
    /// implementing the actual functionality by extending this abstract class)
    /// by calling the one of the factory methods.
    /// </remarks>
    public class NmeaCompatibleLocationProvider : LocationProvider
    {
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Start lister to an NMEA data input stream
        /// </summary>
        /// <param name="input">the NMEA input</param>
        public void StartListening(Stream input)
        {
            _nmeaInputStream = input;

            if (input != null)
            {
                _stopWorker = true;
                if (_nmeaDataWorkerThread != null)
                {
                    try
                    {
                        _nmeaDataWorkerThread.Join();
                    }
                    catch (Exception)
                    {

                    }
                }
                _stopWorker = false;
                _nmeaDataWorkerThread = new Thread(new NmeaDataWorker(this).Run);
                _nmeaDataWorkerThread.Name = "NmeaDataWorker";
                _nmeaDataWorkerThread.Priority = ThreadPriority.Lowest;
                _nmeaDataWorkerThread.Start();
            }
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Stops the listening.
        /// </summary>
        public void StopListening()
        {
            _stopWorker = true;
            if (_nmeaDataWorkerThread != null)
            {
                try
                {
                    _nmeaDataWorkerThread.Join();
                }
                catch (Exception)
                {

                }
            }
            _nmeaDataWorkerThread = null;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the nmea data listener.
        /// </summary>
        /// <param name="listener">The listener.</param>
        public void SetNmeaDataListener(INmeaDataListener listener)
        {
            nmeaDataListener = listener;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        public void Parse(string input)
        {
            byte[] array = Encoding.UTF8.GetBytes(input);
            Parse(array, array.Length);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="len">The len.</param>
        public void Parse(byte[] input, int len)
        {
            lock (syncObject)
            {
                //if need one fix or periodically update.
                bool bNeedFix = false;
                long currentTime = (long)(DateTime.Now - _startTime).TotalMilliseconds;
                if (getOneFix)
                {
                    bNeedFix = true;
                }
                else if (LocationListener != null)
                {

                    if (currentTime - previousFixtime > locationInterval * 1000)
                    {
                        bNeedFix = true;
                    }
                }
                if (bNeedFix || nmeaDataListener != null)
                {
                    _nmeaParser.Parse(input, len);
                    ProcessNmeaDataRecords();
                    previousFixtime = currentTime;
                }

            }
        }

        /**
         * Nmea Parser
         */
        private readonly NmeaParser _nmeaParser = new NmeaParser();


        /// <summary>
        /// the Nmea data listener.
        /// </summary>
        protected INmeaDataListener nmeaDataListener;


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Processes the nmea data records.
        /// </summary>
        private void ProcessNmeaDataRecords()
        {
            ArrayList dataRecords = _nmeaParser.NmeaDataRecords;
            for (int i = 0; i < dataRecords.Count; i++)
            {
                NmeaDataRecord dataRecord = (NmeaDataRecord)
                    dataRecords[i];
                switch (dataRecord.RecordType)
                {
                    case NmeaDataRecord.TypeGPGGA:
                        {
                            NmeaGPGGADataRecord gpggaDataRecord =
                              (NmeaGPGGADataRecord)dataRecord;
                            CurrentLocation.Latitude = gpggaDataRecord.Latitude;
                            CurrentLocation.Longitude = gpggaDataRecord.Longitude;
                            CurrentLocation.Altitude = gpggaDataRecord.Altitude;
                            CurrentLocation.TimeStamp = gpggaDataRecord.TimeStamp;
                            CurrentLocation.Hdop = gpggaDataRecord.Hdop;
                            if (gpggaDataRecord.ReceiverMode == 0)
                            {
                                CurrentState = TemporarilyUnavailable;
                            }
                            else
                            {
                                CurrentState = Available;
                            }
                        }
                        break;
                    case NmeaDataRecord.TypeGPRMC:
                        {
                            NmeaGPRMCDataRecord gprmcDataRecord =
                              (NmeaGPRMCDataRecord)dataRecord;
                            CurrentLocation.Latitude = gprmcDataRecord.Latitude;
                            CurrentLocation.Longitude = gprmcDataRecord.Longitude;
                            CurrentLocation.Speed = gprmcDataRecord.Speed;
                            CurrentLocation.TimeStamp = gprmcDataRecord.TimeStamp;
                            CurrentLocation.Course = gprmcDataRecord.Course;
                            CurrentLocation.Status = gprmcDataRecord.Status;
                        }
                        break;
                    case NmeaDataRecord.TypeGPVTG:
                        {
                            NmeaGPVTGDataRecord gpvtgDataRecord =
                              (NmeaGPVTGDataRecord)dataRecord;
                            CurrentLocation.Speed = gpvtgDataRecord.SpeedKnot;
                            CurrentLocation.Course = gpvtgDataRecord.Course;

                        }
                        break;
                    case NmeaDataRecord.TypeGPGLL:
                        {
                            NmeaGPGLLDataRecord gpgllDataRecord =
                              (NmeaGPGLLDataRecord)dataRecord;
                            CurrentLocation.Latitude = gpgllDataRecord.Latitude;
                            CurrentLocation.Longitude = gpgllDataRecord.Longitude;
                            CurrentLocation.TimeStamp = gpgllDataRecord.TimeStamp;
                            CurrentLocation.Status = gpgllDataRecord.Status;
                            if (!gpgllDataRecord.Status)
                            {
                                CurrentState = TemporarilyUnavailable;
                            }
                            else
                            {
                                CurrentState = Available;
                            }

                        }
                        break;
                    case NmeaDataRecord.TypeGPGSA:
                        {
                            NmeaGPGSADataRecord gpgsaDataRecord =
                              (NmeaGPGSADataRecord)dataRecord;
                            CurrentLocation.Pdop = gpgsaDataRecord.Pdop;
                            CurrentLocation.Vdop = gpgsaDataRecord.Vdop;
                            CurrentLocation.Hdop = gpgsaDataRecord.Hdop;
                        }
                        break;
                    case NmeaDataRecord.TypeGPGSV:

                        break;


                }
                if (nmeaDataListener != null)
                {
                    nmeaDataListener.NmeaDataReceived(this, dataRecord);
                }
            }

            _nmeaParser.NmeaDataRecords.Clear();

            CurrentLocation.Speed *= 1.609;
            if (getOneFix)
            {
                syncObject.Set();

            }
            if (LocationListener != null)
            {
                LocationListener.LocationUpdated(this, CurrentLocation);
            }

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// reset the location provider
        /// </summary>
        public override void Reset()
        {
            StopListening();
            if (_nmeaInputStream != null)
            {
                try
                {
                    _nmeaInputStream.Close();
                }
                catch (IOException)
                {

                }
            }
        }

        private volatile bool _stopWorker;
        private Stream _nmeaInputStream;
        private Thread _nmeaDataWorkerThread;
        private readonly DateTime _startTime = new DateTime(1971, 1, 1);



        private class NmeaDataWorker
        {
            private readonly NmeaCompatibleLocationProvider _nmeaCompatibleLocationProvider;

            public NmeaDataWorker(NmeaCompatibleLocationProvider nmeaCompatibleLocationProvider)
            {
                _nmeaCompatibleLocationProvider = nmeaCompatibleLocationProvider;
            }
            private readonly byte[] _buffer = new byte[1024];
            public void Run()
            {
                Log.P(Thread.CurrentThread.Name + " started");
                while (!_nmeaCompatibleLocationProvider._stopWorker && _nmeaCompatibleLocationProvider._nmeaInputStream != null)
                {
                    try
                    {

                        if (_nmeaCompatibleLocationProvider._nmeaInputStream.Length > 0)
                        {
                            int len = _nmeaCompatibleLocationProvider._nmeaInputStream.Read(_buffer, 0, _buffer.Length);
                            if (_nmeaCompatibleLocationProvider.nmeaDataListener != null || _nmeaCompatibleLocationProvider.LocationListener != null)
                            {
                                _nmeaCompatibleLocationProvider.Parse(_buffer, len);
                            }
                        } Thread.Sleep(1000);
                    }
                    catch (Exception)
                    {

                    }
                }
                Log.P(Thread.CurrentThread.Name + " stopped");
            }

        }

    }

}
