//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 27SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System;
using System.Collections;
using System.IO;
using System.Threading;
using Mapdigit.Ajax;
using Mapdigit.Rms;
using Mapdigit.Util;

//--------------------------------- PACKAGE -----------------------------------
namespace Mapdigit.Gis.Service.Google
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 27SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// This class is used to store driving directions results
    /// </summary>
    internal class GoogleMapService : DigitalMapService
    {

        /**
         * map tile urls.
         */
        public static Hashtable MapTypeUrls = new Hashtable();

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleMapService"/> class.
        /// </summary>
        public GoogleMapService()
        {
            localSearch = new GLocalSearch();
            geocoder = new GClientGeocoder();
            reverseGeocoder = new GReverseClientGeocoder();
            directionQuery = new GDirections();
        }
       

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Updates the map tile URL.
        /// </summary>
        public static void UpdateMapServiceUrl()
        {
            try
            {
                RestoreMapServiceUrls();
                Request.Get(_queryUrl + "version", null, null, _mapServiceUrlQuery, "version");
                _syncObject.WaitOne(5000,false);
            }
            catch (Exception e)
            {
                Log.P(e.Message);
            }
        }


        internal static bool _usingJson = true;//use JSON or KML


        /**
         * record store name
         */
        private const string MapServiceUrlRecordstoreName = "Guidebee_ServiceUrl";
        /**
         * record store
         */
        private static RecordStore _mapDataRecordStore;

        

        internal static string GetGoogleKey()
        {
            return MapKeyRepository.GetKey(MapKey.MapkeyTypeGoogle);
        }

        internal static string _chineseFullName = "中华人民共和国";


        /**
         * map tile urls.
         */
        internal static Hashtable _mapServicesUrls = new Hashtable();
        internal static string _geocoderChina = "GEOCODERCHINA";
        internal static string _geoCoderString = "GEOCODER";
        internal static string _direction = "DIRECTION";
        internal static string _localsearch = "LOCALSEARCH";
        internal static string _reversegeocoder = "REVERSEGEOCODER";
        internal static string _reverseGeocoderChina = "REVERSEGEOCODERCHINA";
        internal static int _version;
        internal static MapServiceUrlQuery _mapServiceUrlQuery = new MapServiceUrlQuery();
        internal static string _queryUrl = "http://www.mapdigit.com/guidebeemap/config.php?q=";
        internal static AutoResetEvent _syncObject = new AutoResetEvent(false);

        private static byte[] ToByteArray(string mapType, string url)
        {
            try
            {
                MemoryStream baos = new MemoryStream();
                BinaryWriter dos = new BinaryWriter(baos);
                dos.Write(mapType);
                dos.Write(url);
                dos.Close();
                baos.Close();
                return baos.GetBuffer();
            }
            catch (IOException)
            {

            }

            return null;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Saves the map urls.
        /// </summary>
        internal static void SaveMapServiceUrls()
        {

            try
            {
                RecordStore.DeleteRecordStore(MapServiceUrlRecordstoreName);
            }
            catch (RecordStoreException)
            {

            }
            try
            {

                lock (MapTypeUrls)
                {
                    _mapDataRecordStore = RecordStore.OpenRecordStore(MapServiceUrlRecordstoreName, true);
                    byte[] version = new byte[1];
                    version[0] = (byte)_version;
                    _mapDataRecordStore.AddRecord(version, 0, 1);
                    IEnumerator emu = MapTypeUrls.GetEnumerator();
                    while (emu.MoveNext())
                    {
                        DictionaryEntry dictionaryEntry = (DictionaryEntry)emu.Current;
                        int mapTypeIndex = (int)dictionaryEntry.Key;
                        string url = (string)MapTypeUrls[mapTypeIndex];
                        byte[] recordDate = ToByteArray(mapTypeIndex.ToString(), url);
                        if (recordDate != null)
                        {
                            _mapDataRecordStore.AddRecord(recordDate, 0, recordDate.Length);
                        }

                    }

                }
                _mapDataRecordStore.CloseRecordStore();
                _mapDataRecordStore = null;

            }
            catch (RecordStoreException)
            {

            }

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Restores the map urls.
        /// </summary>
        internal static void RestoreMapServiceUrls()
        {
            {
                try
                {
                    lock (MapTypeUrls)
                    {
                        MapTypeUrls.Clear();
                        if (_mapDataRecordStore == null)
                        {
                            _mapDataRecordStore = RecordStore.OpenRecordStore(MapServiceUrlRecordstoreName, false);

                            int numOfRecords = _mapDataRecordStore.GetNumRecords();
                            if (numOfRecords > 0)
                            {
                                byte[] recordDate = _mapDataRecordStore.GetRecord(1);
                                _version = recordDate[0];
                            }
                            for (int i = 1; i < numOfRecords; i++)
                            {
                                byte[] recordDate = _mapDataRecordStore.GetRecord(i + 1);
                                MemoryStream bais = new MemoryStream(recordDate);
                                BinaryReader dis = new BinaryReader(bais);
                                try
                                {
                                    string mapType = dis.ReadString();
                                    int mapTypeIndex = int.Parse(mapType);
                                    string url = dis.ReadString();
                                    MapTypeUrls.Add(mapTypeIndex, url);
                                    bais.Close();
                                    dis.Close();
                                }
                                catch (IOException)
                                {

                                }

                            }
                            _mapDataRecordStore.CloseRecordStore();
                            _mapDataRecordStore = null;
                        }
                    }

                }
                catch (RecordStoreException)
                {

                }
            }

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the map service URL.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns></returns>
        internal static string GetMapServiceUrl(string serviceType)
        {
            lock (MapTypeUrls)
            {
                return (string)MapTypeUrls[serviceType];
            }
        }

    }

    class MapServiceUrlQuery : IRequestListener
    {

        public void ReadProgress(object context, int bytes, int total)
        {
        }

        public void WriteProgress(object context, int bytes, int total)
        {
        }

        public void Done(object context, Response response)
        {
            //lock (GoogleMapService.syncObject)
            {
                if ((string)context == "version")
                {
                    if (response != null)
                    {
                        try
                        {
                            string ver = response.GetResult().GetAsString("Version");
                            int verNo = int.Parse(ver);
                            if (verNo > GoogleMapService._version || verNo == 0)
                            {
                                GoogleMapService._version = verNo;
                                Request.Get(GoogleMapService._queryUrl + "serviceurl", null, null, GoogleMapService._mapServiceUrlQuery, "serviceurl");
                            }
                            else
                            {
                                GoogleMapService._syncObject.Set();

                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                else if ((string)context == "serviceurl")
                {
                    if (response != null)
                    {
                        Result result = response.GetResult();
                        int count = result.GetSizeOfArray("serviceUrls");
                        lock (GoogleMapService.MapTypeUrls)
                        {
                            GoogleMapService.MapTypeUrls.Clear();
                            for (int i = 0; i < count; i++)
                            {
                                string mapType = result.GetAsString("serviceUrls[" + i + "].type");
                                string mapTileUrl = result.GetAsString("serviceUrls[" + i + "].URL");
                                GoogleMapService.MapTypeUrls.Add(mapType, mapTileUrl);
                            }
                        }

                        GoogleMapService.SaveMapServiceUrls();
                    }
                    GoogleMapService._syncObject.Set();

                }
            }
        }

    }

}
