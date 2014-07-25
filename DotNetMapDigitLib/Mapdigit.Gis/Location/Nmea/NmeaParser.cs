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
using System.Globalization;
using System.Text;

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
    /// NEMA 0183 parser.
    /// </summary>
    internal class NmeaParser
    {

        /**
         *   GPS DOP and active satellites.
         */
        public const string DollarSignGPGSA = "$GPGSA";

        /**
         * Global positioning system fixed data.
         */
        public const string DollarSignGPGGA = "$GPGGA";

        /**
         * Recommended minimum specific GPS/Transit data.
         */
        public const string DollarSignGPRMC = "$GPRMC";

        /**
         * Satellites in view.
         */
        public const string DollarSignGPGSV = "$GPGSV";

        /**
         * Location Fix.
         */
        public const string DollarSignGPGLL = "$GPGLL";

        /**
         * ground speed.
         */
        public const string DollarSignGPVTG = "$GPVTG";

        /**
         * Size of the string buffer. This should be a little more than the size of
         * the byte array plus 80 (the max size of an Nmea sentence).
         */
        public const short OutputBufferMaxSize = 2048;

        /**
         * The maximum size of a sentence according to the Nmea standards is 82. We
         * will use 128 to be safe.
         */
        public const short MaxSentenceSize = 128;

        /**
         * Sentence characters
         */
        public const char SentenceStart = '$';

        /**
         * checksum start.
         */
        public const char ChecksumStart = '*';

        /**
         * sentence end character.
         */
        public const char SentenceEnd = '\n';

        /**
         * parameter delimter.
         */
        public const char Delimiter = ',';

        /**
         * There was not enough to process
         */
        public const short TypeNothingToProcess = -1;

        /**
         * Parsed Nmea data records.
         */
        public ArrayList NmeaDataRecords = new ArrayList();

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Append the input and parse it. The size of the input must always be
        /// less than OUTPUT_BUFFER_MAX_SIZE.
        /// </summary>
        /// <param name="input">the input to parse in bytes.</param>
        /// <param name="size">the size of the input.</param>
        /// <returns>a integer to indicate which sentences were parsed</returns>
        public int Parse(byte[] input, int size)
        {
            Append(input, size);
            return DoParse();
        }

        /**
         * The current data read from the GPS device
         */
        private readonly byte[] _data = new byte[OutputBufferMaxSize];

        /**
         * The length of data
         */
        private int _dataLength;

        /**
         * temp hashtable used to determin all GSV sentences have been parsed.
         */
        private readonly ArrayList _gpgsvTable = new ArrayList();

        /**
         * temporaily stored satellites in view.
         */
        private readonly NmeaGPGSVDataRecord _satellitesInfo = new NmeaGPGSVDataRecord();


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Append the input to the input buffer. The size of the input must
        /// always be less than OUTPUT_BUFFER_MAX_SIZE.
        /// </summary>
        /// <param name="input">the input in bytes.</param>
        /// <param name="size">the size of the input</param>
        private void Append(byte[] input, int size)
        {

            // Allocate a new buffer if we got so much data that it will
            // contain all our information and the buffer from before can
            // be discarded to save the overhead of processing it.
            if (_dataLength + size >= _data.Length)
            {
                Flush();
            }

            // Append input to data left over from a past append().
            int start = 0;
            int length = size;

            if (size > _data.Length)
            {
                start = size - _data.Length;
                length = size - start;
            }

            Array.Copy(input, start, _data, _dataLength, length);
            _dataLength += size;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Flush the buffer.
        /// </summary>
        private void Flush()
        {
            _dataLength = 0;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parse the data from the Bluetooth GPS device into NMEA sentences.
        /// </summary>
        /// <returns>a integer to indicate which sentences were parsed</returns>
        private int DoParse()
        {

            int parsedSentenceTypes = NmeaDataRecord.TypeNone;

            // If there is hardly anything in the buffer, there won't be a
            // Nmea sentence, so don't bother processing it.
            if (_dataLength < 16)
            {
                return NmeaDataRecord.TypeNone;
            }

            // Set the current index to be the
            int currentIndex = _dataLength - 1;

            // True if the current sentence is the last sentence in the buffer
            bool isLastSentence = true;

            // Index of the start of the last sentence
            int lastSentenceStart = -1;

            // While there are characters left to process
            while (currentIndex > 0)
            {
                // Find the start of the last Nmea sentence.
                int sentenceStart =
                        LastIndexOf(_data, SentenceStart, currentIndex);

                // Did we find the start of a sentence?
                if (sentenceStart != -1)
                {
                    // We found the start of a sentence, look for the end.
                    int sentenceEnd =
                            IndexOf(_data, SentenceEnd, sentenceStart, _dataLength);

                    // Did we find the sentence end?
                    if (sentenceEnd != -1)
                    {
                        // Look for the first delimitter to get the sentence type.
                        // (i.e. string.IndexOf(Delimiter, sentenceStart))
                        int sentenceTypeEnd =
                                IndexOf(_data, Delimiter, sentenceStart, sentenceEnd);

                        // If we found the type end and the sentence end is within
                        // this sentence, then process the sentence. By checking t
                        // hat the sentence end is less than the current index then 
                        // we handle the the case that we have a buffer left of
                        // "$GPRMC,45667,V,4354.788"
                        // and the first chunch of the new chars does not end the
                        // same sentence
                        // but instead starts a new one.
                        if ((sentenceTypeEnd != -1) &&
                                (sentenceEnd <= currentIndex))
                        {
                            try
                            {
                                string type = Encoding.UTF8.GetString(_data, sentenceStart,
                                               sentenceTypeEnd - sentenceStart);

                                if ((type.Equals(DollarSignGPRMC)) &&
                                        ((parsedSentenceTypes & NmeaDataRecord.TypeGPRMC) == 0))
                                {
                                    parsedSentenceTypes = parsedSentenceTypes |
                                            ProcessSentence(_data,
                                            sentenceStart, sentenceEnd, NmeaDataRecord.TypeGPRMC);
                                }
                                else if ((type.Equals(DollarSignGPGGA)) &&
                                        ((parsedSentenceTypes & NmeaDataRecord.TypeGPGGA) == 0))
                                {
                                    parsedSentenceTypes = parsedSentenceTypes |
                                            ProcessSentence(_data,
                                            sentenceStart, sentenceEnd, NmeaDataRecord.TypeGPGGA);
                                }
                                else if ((type.Equals(DollarSignGPGSA)) &&
                                      ((parsedSentenceTypes & NmeaDataRecord.TypeGPGSA) == 0))
                                {
                                    parsedSentenceTypes = parsedSentenceTypes |
                                            ProcessSentence(_data,
                                            sentenceStart, sentenceEnd, NmeaDataRecord.TypeGPGSA);
                                }
                                else if ((type.Equals(DollarSignGPGSV)) &&
                                      ((parsedSentenceTypes & NmeaDataRecord.TypeGPGSV) == 0))
                                {
                                    parsedSentenceTypes = parsedSentenceTypes |
                                            ProcessSentence(_data,
                                            sentenceStart, sentenceEnd, NmeaDataRecord.TypeGPGSV);
                                }
                                else if ((type.Equals(DollarSignGPGLL)) &&
                                      ((parsedSentenceTypes & NmeaDataRecord.TypeGPGLL) == 0))
                                {
                                    parsedSentenceTypes = parsedSentenceTypes |
                                            ProcessSentence(_data,
                                            sentenceStart, sentenceEnd, NmeaDataRecord.TypeGPGLL);
                                }
                                else if ((type.Equals(DollarSignGPVTG)) &&
                                      ((parsedSentenceTypes & NmeaDataRecord.TypeGPVTG) == 0))
                                {
                                    parsedSentenceTypes = parsedSentenceTypes |
                                            ProcessSentence(_data,
                                            sentenceStart, sentenceEnd, NmeaDataRecord.TypeGPVTG);
                                }
                            }
                            catch (Exception)
                            {
                                // We are kind of screwed at this point so just return
                                // what we have and flush the buffer.
                                Flush();
                                return parsedSentenceTypes;
                            }
                            // move the current position
                            currentIndex = sentenceStart - 1;
                            // Check if we have a complete record. If so we do
                            // not need to keep working with this buffer
                            // Check if we have a complete record. If so we do
                            // not need to keep working with this buffer
                            if (parsedSentenceTypes == NmeaDataRecord.AllTypesMask)
                            {
                                break;
                            }

                        }
                        else
                        {
                            // This sentence is junk, so just skip it
                            currentIndex = sentenceStart - 1;
                        }
                    }
                    else
                    {
                        // If this is the last sentence in the buffer, then keep the
                        // index of the start so that we do not delete it.
                        if (isLastSentence)
                        {
                            lastSentenceStart = sentenceStart;
                        }

                        currentIndex = sentenceStart - 1;


                    }
                }
                else
                {
                    break;
                }

                // Once we have completed an iteration, set the last sentence flag
                // to false
                isLastSentence = false;
            } // while

            // Throw away everything that has already been parsed.
            if (lastSentenceStart < 0)
            {
                // Processed everything.  No partial sentence left at the end.
                Flush();
            }
            else
            {
                // Keep the partial last sentence.
                _dataLength -= lastSentenceStart;
                Array.Copy(_data, lastSentenceStart, _data, 0, _dataLength);
            }

            // If we parsed any of the sentences that we care about, put the record
            // in the buffer.
            if (parsedSentenceTypes != 0)
            {
                // Put the record in the record buffer
                //setRecordBuffer( record );
                // Create a new record from the existing record
                //record = new GPSRecord(record);
            }

            return parsedSentenceTypes;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Looks for the first occurance of a byte b in 
        /// array between 
        /// [fromIndex, stopIndex).
        /// </summary>
        /// <param name="array"> is the data to scan</param>
        /// <param name="b">is the byte to match.</param>
        /// <param name="fromIndex">is the first index into array to check</param>
        /// <param name="stopIndex">is one past the last index into array to check.</param>
        /// <returns>The first index where b was found; -1 if it was not
        ///  found.</returns>
        private static int IndexOf(byte[] array, char b, int fromIndex,
                int stopIndex)
        {
            for (int position = fromIndex; position < stopIndex; position++)
            {
                if (array[position] == b)
                {
                    return position;
                }
            }

            // If we made it here, b was not found.
            return -1;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Looks for the last occurance of a byte b in
        /// array going
        /// backwards from fromIndex.
        /// </summary>
        /// <param name="array">is the data to scan.</param>
        /// <param name="b">is the byte to match</param>
        /// <param name="fromIndex">is the first index into array to check</param>
        /// <returns>The last index where b was found; -1 if it was not
        /// found.</returns>
        private static int LastIndexOf(byte[] array, char b, int fromIndex)
        {
            for (int position = fromIndex; position >= 0; position--)
            {
                if (array[position] == b)
                {
                    return position;
                }
            }

            // If we made it here, b was not found.
            return -1;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// check the checksum is correct or not.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        private static bool CheckSum(byte[] buffer)
        {
            string nmeaSentence = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            int intstarpos = nmeaSentence.IndexOf('*');
            if (intstarpos >= 0)
            {
                // we have a checksum so check it...
                string strChecksum = nmeaSentence.Substring(intstarpos + 1);
                // remove checksum from end of string
                string strData = nmeaSentence.Substring(0, nmeaSentence.Length
                        - strChecksum.Length - 1);
                int intor = 0;
                //go from first character upto last *
                for (int i = 1; (i < strData.Length); i++)
                {
                    intor = intor ^ strData[i];
                }

                int y;

                try
                {
                    y = int.Parse(strChecksum, NumberStyles.HexNumber);
                }
                catch (Exception)
                {
                    return false;
                }
                if (intor != y)
                {
                    // debug for checksum failures
                    intor += 0;
                }
                return (intor == y);
            }
            return true;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Process the sentence of the specified type.  The sentence is
        /// every ASCII character stored in data between
        /// offset and stop.
        /// </summary>
        /// <param name="data">contains the NMEA sentence to process</param>
        /// <param name="offset">is the index that starts the NMEA sentence within 
        /// data.</param>
        /// <param name="stop">is the index of the final character in the sentenc.</param>
        /// <param name="type">the sentence type.</param>
        /// <returns>the type of the setence processed. If the sentence cannot be
        ///         processed this returns 0.</returns>
        private short ProcessSentence(byte[] data, int offset, int stop, short type)
        {
            // If the sentence is greater than the max size just discard it
            short retType = NmeaDataRecord.TypeNone;
            if (stop - offset <= MaxSentenceSize)
            {
                //get rid of \r\n
                int length = stop - offset + 1;
                if (data[stop] == 10)
                {
                    length--;
                }
                if (data[stop - 1] == 13)
                {
                    length--;
                }
                byte[] buffer = new byte[length];
                Array.Copy(data, offset, buffer, 0, buffer.Length);
                if (CheckSum(buffer))
                {
                    string nmeaSentence = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    switch (type)
                    {
                        case NmeaDataRecord.TypeGPRMC:
                            retType = ProcessGPRMC(nmeaSentence);
                            break;
                        case NmeaDataRecord.TypeGPGGA:
                            retType = ProcessGPGGA(nmeaSentence);
                            break;
                        case NmeaDataRecord.TypeGPGSA:
                            retType = ProcessGPGSA(nmeaSentence);
                            break;
                        case NmeaDataRecord.TypeGPGSV:
                            retType = ProcessGPGSV(nmeaSentence);
                            break;
                        case NmeaDataRecord.TypeGPGLL:
                            retType = ProcessGPGLL(nmeaSentence);
                            break;
                        case NmeaDataRecord.TypeGPVTG:
                            retType = ProcessGPVTG(nmeaSentence);
                            break;
                        default:
                            break;
                    }
                }
            }

            return retType;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// latitude in ddmm.mmmmmm
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        private static double LatitudeToDouble(string latitude, string direction)
        {
            double ret = 0.0;
            try
            {
                string strDegree = latitude.Substring(0, 2);
                string strMinutes = latitude.Substring(2, latitude.Length);
                double dblDegree = double.Parse(strDegree);
                double dblMinute = double.Parse(strMinutes) / 60.0;
                ret = dblDegree + dblMinute;
                if (direction.Equals("S", StringComparison.OrdinalIgnoreCase))
                {
                    ret = -ret;
                }
            }
            catch (Exception)
            {
            }
            return ret;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// longitude in dddmm.mmmmmm
        /// </summary>
        /// <param name="longitude">The longitude.</param>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        private static double LongitudeToDouble(string longitude, string direction)
        {
            double ret = 0.0;
            try
            {
                string strDegree = longitude.Substring(0, 3);
                string strMinutes = longitude.Substring(3, longitude.Length);
                double dblDegree = double.Parse(strDegree);
                double dblMinute = double.Parse(strMinutes) / 60.0;
                ret = dblDegree + dblMinute;
                if (direction.Equals("W", StringComparison.OrdinalIgnoreCase))
                {
                    ret = -ret;
                }
            }
            catch (Exception)
            {
            }
            return ret;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Processes the GPRMC.
        ///  RMC Recommended Minimum Navigation Information
        ///        12
        ///        1 2 3 4 5 6 7 8 9 10 11|
        ///        | | | | | | | | | | | |
        ///        $--RMC,hhmmss.ss,A,llll.ll,a,yyyyy.yy,a,x.x,x.x,xxxx,x.x,a*hh
        ///        1) Time (UTC)
        ///        2) Status, V = Navigation receiver warning
        ///        3) Latitude
        ///        4) N or S
        ///        5) Longitude
        ///        6) E or W
        ///        7) Speed over ground, knots
        ///        8) Track made good, degrees true
        ///        9) Date, ddmmyy
        ///        10) Magnetic Variation, degrees
        ///        11) E or W
        ///        12) Checksum
        /// </summary>
        /// <param name="nmeaSentence">The nmea sentence.</param>
        /// <returns></returns>
        private short ProcessGPRMC(string nmeaSentence)
        {
            NmeaGPRMCDataRecord dataRecord = new NmeaGPRMCDataRecord();

            //UTC
            int commaIndex1 = nmeaSentence.IndexOf(Delimiter);
            int commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            //string strUtc = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            //Status
            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            string strStatus = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            //Latitude
            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            string strLatitude = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);
            //Direction
            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            string strLatitudeDir = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            //Longitude
            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            string strLongitude = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);
            //Direction
            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            string strLongitudeDir = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            //Speed
            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            string strSpeed = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            //Course
            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            string strCourse = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            //Date
            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            //string strDate = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            //Magnetic
            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            string strMagnetic = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            //MagneticDir
            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            if (commaIndex2 == -1)
            {
                commaIndex2 = nmeaSentence.IndexOf(ChecksumStart, commaIndex1 + 1);
            }
            string strMagneticDir = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            //int utchours = int.Parse(strUTC.Substring(0, 2));
            //int utcminutes = int.Parse(strUTC.Substring(2, 4));
            //int utcseconds = int.Parse(strUTC.Substring(4, 6));
            //int day = int.Parse(strDate.Substring(0, 2));
            //int month = int.Parse(strDate.Substring(2, 4));
            // available for this century
            //int year = int.Parse(strDate.Substring(4, 6)) + 2000;
            //        Calendar calendar=Calendar.getInstance();
            //        calendar.set(calendar.YEAR,year);
            //        calendar.set(calendar.MONTH,month);
            //        calendar.set(calendar.DAY_OF_MONTH,day);
            //        calendar.set(calendar.HOUR,utchours);
            //        calendar.set(calendar.MINUTE,utcminutes);
            //        calendar.set(calendar.SECOND,utcseconds);
            dataRecord.TimeStamp = DateTime.Now;

            if (strStatus.Equals("A", StringComparison.OrdinalIgnoreCase))
            {
                dataRecord.Status = true;
            }
            else
            {
                dataRecord.Status = false;
            }

            dataRecord.Latitude = LatitudeToDouble(strLatitude, strLatitudeDir);
            dataRecord.Longitude = LongitudeToDouble(strLongitude, strLongitudeDir);
            if (strSpeed.Length > 0)
            {
                dataRecord.Speed = double.Parse(strSpeed);
            }
            if (strCourse.Length > 0)
            {
                dataRecord.Course = double.Parse(strCourse);
            }
            if (strMagnetic.Length > 0)
            {
                dataRecord.MagneticCourse = double.Parse(strMagnetic);
                if (strMagneticDir.Equals("W", StringComparison.OrdinalIgnoreCase))
                {
                    dataRecord.MagneticCourse = -dataRecord.MagneticCourse;
                }
            }
            NmeaDataRecords.Add(dataRecord);



            return NmeaDataRecord.TypeGPRMC;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Processes the GPGGA.
        /// GGA Global Positioning System Fix Data. Time, Position and fix related data
        ///    for a GPS receiver
        ///                        11
        ///    1 2 3 4 5 6 7 8 9 10 | 12 13 14 15
        ///    | | | | | | | | | | | | | | |
        ///    $--GGA,hhmmss.ss,llll.ll,a,yyyyy.yy,a,x,xx,x.x,x.x,M,x.x,M,x.x,xxxx*hh
        ///    1) Time (UTC)
        ///    2) Latitude
        ///    3) N or S (North or South)
        ///    4) Longitude
        ///    5) E or W (East or West)
        ///    6) GPS Quality Indicator,
        ///    0 - fix not available,
        ///    1 - GPS fix,
        ///    2 - Differential GPS fix
        ///    7) Number of satellites in view, 00 - 12
        ///    8) Horizontal Dilution of precision
        ///    9) Antenna Altitude above/below mean-sea-level (geoid)
        ///    10) Units of antenna altitude, meters
        ///    11) Geoidal separation, the difference between the WGS-84 earth
        ///    ellipsoid and mean-sea-level (geoid), "-" means mean-sea-level below ellipsoid
        ///    12) Units of geoidal separation, meters
        ///    13) Age of differential GPS data, time in seconds since last SC104
        ///    type 1 or 9 update, null field when DGPS is not used
        ///    14) Differential reference station ID, 0000-1023
        ///    15) Checksum
        /// </summary>
        /// <param name="nmeaSentence">The nmea sentence.</param>
        /// <returns></returns>
        private short ProcessGPGGA(string nmeaSentence)
        {
            NmeaGPGGADataRecord dataRecord = new NmeaGPGGADataRecord();
            //GMT
            int commaIndex1 = nmeaSentence.IndexOf(Delimiter);
            int commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            //string strGmt = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);


            //Latitude
            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            string strLatitude = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);
            //Direction
            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            string strLatitudeDir = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            //Longitude
            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            string strLongitude = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);
            //Direction
            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            string strLongitudeDir = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            //ReceiveMode
            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            string strMode = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            //Number of satellite
            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            string strNumberOfSatellites = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            //HDOP
            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            string strHdop = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            //Altitude
            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            string strAltitude = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            //AltitudeUnit
            //commaIndex1 = commaIndex2;
            //commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            //string strAltitudeUnit = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            //Geoidal Separation
            //commaIndex1 = commaIndex2;
            //commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            //string strGeoidalSeparation = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            //Separation Unit
            //commaIndex1 = commaIndex2;
            //commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            //string strSeparationUnit = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            //Age of differential corrections in seconds
            //commaIndex1 = commaIndex2;
            //commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            //string strAOD = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            //Differential reference Station ID
            //commaIndex1 = commaIndex2;
            //commaIndex2 = nmeaSentence.IndexOf(CHECKSUM_START, commaIndex1 + 1);
            //string strStationID = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            //int utchours = int.Parse(strGmt.Substring(0, 2));
            //int utcminutes = int.Parse(strGmt.Substring(2, 4));
            //int utcseconds = int.Parse(strGmt.Substring(4, 6));
            //        Calendar calendar=Calendar.getInstance();
            //        calendar.set(calendar.HOUR,utchours);
            //        calendar.set(calendar.MINUTE,utcminutes);
            //        calendar.set(calendar.SECOND,utcseconds);
            dataRecord.TimeStamp = DateTime.Now;
            dataRecord.Latitude = LatitudeToDouble(strLatitude, strLatitudeDir);
            dataRecord.Longitude = LongitudeToDouble(strLongitude, strLongitudeDir);
            if (strMode.Length > 0)
            {
                dataRecord.ReceiverMode = int.Parse(strMode);
            }
            if (strNumberOfSatellites.Length > 0)
            {
                dataRecord.NumberOfSatellites = int.Parse(strNumberOfSatellites);
            }
            if (strAltitude.Length > 0)
            {
                dataRecord.Altitude = double.Parse(strAltitude);
            }
            if (strHdop.Length > 0)
            {
                dataRecord.Hdop = double.Parse(strHdop);
            }
            NmeaDataRecords.Add(dataRecord);
            return NmeaDataRecord.TypeGPGGA;
        }



        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Processes the GPGSA.
        /// GSA GPS DOP and active satellites
        ///    1 2 3 14 15 16 17 18
        ///    | | | | | | | |
        ///    $--GSA,a,a,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x.x,x.x,x.x*hh
        ///    1) Selection mode
        ///    2) Mode
        ///    3) ID of 1st satellite used for fix
        ///    4) ID of 2nd satellite used for fix
        ///    ...
        ///    14) ID of 12th satellite used for fix
        ///    15) PDOP in meters
        ///    16) HDOP in meters
        ///    17) VDOP in meters
        ///    18) Checksum
        /// </summary>
        /// <param name="nmeaSentence">The nmea sentence.</param>
        /// <returns></returns>
        private short ProcessGPGSA(string nmeaSentence)
        {
            NmeaGPGSADataRecord dataRecord = new NmeaGPGSADataRecord();
            //Manual or Automatic Mode
            int commaIndex1 = nmeaSentence.IndexOf(Delimiter);
            int commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            string strMaMode = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            //2D3D Mode
            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            string str2D3DMode = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            string[] strPrn = new string[12];

            for (int i = 0; i < 12; i++)
            {
                //PRN Numbers
                commaIndex1 = commaIndex2;
                commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
                strPrn[i] = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);
            }

            //PDOP
            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            string strPdop = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            //HDOP
            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            string strHdop = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            //VDOP
            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            if (commaIndex2 == -1)
            {
                commaIndex2 = nmeaSentence.IndexOf(ChecksumStart, commaIndex1 + 1);
            }
            string strVdop = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            if (strMaMode.Equals("M", StringComparison.OrdinalIgnoreCase))
            {
                dataRecord.ManualMode = true;
            }
            if (str2D3DMode.Length > 0)
            {
                dataRecord.OperationMode = int.Parse(str2D3DMode);
            }
            for (int i = 0; i < 12; i++)
            {
                if (strPrn[i].Length > 0)
                {
                    dataRecord.PrNs[i] = int.Parse(strPrn[i]);
                }
            }
            if (strPdop.Length > 0)
            {
                dataRecord.Pdop = double.Parse(strPdop);
            }
            if (strHdop.Length > 0)
            {
                dataRecord.Hdop = double.Parse(strHdop);
            }
            if (strVdop.Length > 0)
            {
                dataRecord.Vdop = double.Parse(strVdop);
            }
            NmeaDataRecords.Add(dataRecord);
            return NmeaDataRecord.TypeGPGSA;
        }



        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Processes the GPGSV.
        /// GSV Satellites in view
        ///    1 2 3 4 5 6 7 n
        ///    | | | | | | | |
        ///    $--GSV,x,x,x,x,x,x,x,...*hh
        ///    1) total number of messages
        ///    2) message number
        ///    3) satellites in view
        ///    4) satellite number
        ///    5) elevation in degrees
        ///    6) azimuth in degrees to true
        ///    7) SNR in dB
        ///    more satellite infos like 4)-7)
        ///    n) Checksum
        /// </summary>
        /// <param name="nmeaSentence">The nmea sentence.</param>
        /// <returns></returns>
        private short ProcessGPGSV(string nmeaSentence)
        {

            //Total number of message

            int commaIndex1 = nmeaSentence.IndexOf(Delimiter);
            int commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            string strNumberOfMessage = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            int numberOfMessages = int.Parse(strNumberOfMessage);

            int intPosOfStar = nmeaSentence.IndexOf(ChecksumStart);
            //Message no
            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            string strMessageNo = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            int messageNo = (int.Parse(strMessageNo));

            if (_gpgsvTable.Contains(messageNo))
            {
                return NmeaDataRecord.TypeNone;
            }
            if (_gpgsvTable.Count == 0)
            {
                _satellitesInfo.Satellites.Clear();
            }
            _gpgsvTable.Add(messageNo);
            //Total Satellite no
            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            string strTotalSatellte = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);
            if (strTotalSatellte.Length > 0)
            {
                _satellitesInfo.NumberOfSatelltes = int.Parse(strTotalSatellte);
            }
            while (commaIndex2 < intPosOfStar)
            {
                //PRN no
                commaIndex1 = commaIndex2;
                commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
                string strPrn = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

                // elevation ,degree ,maximum 90
                commaIndex1 = commaIndex2;
                commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
                string strElevation = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

                // Azimuth ,degree ,true 000 to 359
                commaIndex1 = commaIndex2;
                commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
                string strAzimuth = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

                // SNR (C/Nno) 00-99 db
                commaIndex1 = commaIndex2;
                commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
                if (commaIndex2 == -1)
                {
                    commaIndex2 = nmeaSentence.IndexOf(ChecksumStart, commaIndex1 + 1);
                }
                string strSnr = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

                Satellite satellite = new Satellite();
                if (strPrn.Length > 0)
                {
                    satellite.Id = int.Parse(strPrn);
                }
                if (strElevation.Length > 0)
                {
                    satellite.Elevation = int.Parse(strElevation);
                }
                if (strAzimuth.Length > 0)
                {
                    satellite.Azimuth = int.Parse(strAzimuth);
                }
                if (strSnr.Length > 0)
                {
                    satellite.Snr = int.Parse(strSnr);
                }
                _satellitesInfo.Satellites.Add(satellite);

            }

            if (_gpgsvTable.Count == numberOfMessages)
            {
                _gpgsvTable.Clear();
                NmeaDataRecords.Add(_satellitesInfo);
                return NmeaDataRecord.TypeGPGSV;
            }
            return NmeaDataRecord.TypeNone;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Processes the GPGLL.
        /// GLL Geographic Position Latitude/Longitude
        ///    1 2 3 4 5 6 7
        ///    | | | | | | |
        ///    $--GLL,llll.ll,a,yyyyy.yy,a,hhmmss.ss,A*hh
        ///    1) Latitude
        ///    2) N or S (North or South)
        ///    3) Longitude
        ///    4) E or W (East or West)
        ///    5) Time (UTC)
        ///    6) Status A - Data Valid, V - Data Invalid
        ///    7) Checksum
        /// </summary>
        /// <param name="nmeaSentence">The nmea sentence.</param>
        /// <returns></returns>
        private short ProcessGPGLL(string nmeaSentence)
        {

            NmeaGPGLLDataRecord dataRecord = new NmeaGPGLLDataRecord();
            //Latitude
            int commaIndex1 = nmeaSentence.IndexOf(Delimiter);
            int commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            string strLatitude = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            //Latitude Dir
            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            string strLatitudeDir = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            //Longitude
            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            string strLongitude = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            //Longitude Dir
            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            string strLongitudeDir = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            //UTC
            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            //string strUTC = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);


            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            if (commaIndex2 == -1)
            {
                commaIndex2 = nmeaSentence.IndexOf(ChecksumStart, commaIndex1 + 1);
            }
            string strStatus = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);
            //int utchours = int.Parse(strUTC.Substring(0, 2));
            //int utcminutes = int.Parse(strUTC.Substring(2, 4));
            //int utcseconds = int.Parse(strUTC.Substring(4, 6));
            //        Calendar calendar=Calendar.getInstance();
            //        calendar.set(calendar.HOUR,utchours);
            //        calendar.set(calendar.MINUTE,utcminutes);
            //        calendar.set(calendar.SECOND,utcseconds);
            dataRecord.TimeStamp = DateTime.Now;
            dataRecord.Latitude = LatitudeToDouble(strLatitude, strLatitudeDir);
            dataRecord.Longitude = LongitudeToDouble(strLongitude, strLongitudeDir);
            if (strStatus.Equals("A", StringComparison.OrdinalIgnoreCase))
            {
                dataRecord.Status = true;
            }
            NmeaDataRecords.Add(dataRecord);
            return NmeaDataRecord.TypeGPGLL;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Processes the GPVTG.
        /// VTG Track Made Good and Ground Speed
        ///    1 2 3 4 5 6 7 8 9
        ///    | | | | | | | | |
        ///    $--VTG,x.x,T,x.x,M,x.x,N,x.x,K*hh
        ///    1) Track Degrees
        ///    2) T = True
        ///    3) Track Degrees
        ///    4) M = Magnetic
        ///    5) Speed Knots
        ///    6) N = Knots
        ///    7) Speed Kilometers Per Hour
        ///    8) K = Kilometres Per Hour
        ///    9) Checksum
        /// </summary>
        /// <param name="nmeaSentence">The nmea sentence.</param>
        /// <returns></returns>
        private short ProcessGPVTG(string nmeaSentence)
        {

            NmeaGPVTGDataRecord dataRecord = new NmeaGPVTGDataRecord();
            //Course degree, true
            int commaIndex1 = nmeaSentence.IndexOf(Delimiter);
            int commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            string strCourse = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            //Indicates true course
            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            //string strTrueCourse = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            //Course ,degree , magenetic
            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            string strMagnetic = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            //indicates magnetic course
            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            //string strMagneticCourse = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            //Speed Knot
            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            string strSpeedKnot = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            //Speed Knot unit
            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            //string strSpeedKnotUnit = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            //Speed Km
            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            string strSpeedKm = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            commaIndex1 = commaIndex2;
            commaIndex2 = nmeaSentence.IndexOf(Delimiter, commaIndex1 + 1);
            if (commaIndex2 == -1)
            {
                //commaIndex2 = nmeaSentence.IndexOf(CHECKSUM_START, commaIndex1 + 1);
            }
            //string strSpeedKmUnit = nmeaSentence.Substring(commaIndex1 + 1, commaIndex2);

            if (strCourse.Length > 0)
            {
                dataRecord.Course = double.Parse(strCourse);
            }

            if (strMagnetic.Length > 0)
            {
                dataRecord.CourseMagnetic = double.Parse(strMagnetic);
            }

            if (strSpeedKnot.Length > 0)
            {
                dataRecord.SpeedKnot = double.Parse(strSpeedKnot);
            }

            if (strSpeedKm.Length > 0)
            {
                dataRecord.SpeedKm = double.Parse(strSpeedKm);
            }
            NmeaDataRecords.Add(dataRecord);
            return NmeaDataRecord.TypeGPVTG;
        }
    }
}
