//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 28SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System;
using System.Collections;
using System.IO;
using System.Threading;
using Mapdigit.Ajax;
using Mapdigit.Rms;
using Mapdigit.Util;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Gis.Raster
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 28SEP2010  James Shen                 	          Initial Creation
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Defines the map types and relations between map types.
    /// </summary>
    public static class MapType
    {

        /// <summary>
        /// Google Road Maps
        /// </summary>
        public const int GoogleMap = 0;

        /// <summary>
        /// Google Satellite Images
        /// </summary>
        public const int GoogleSatellite = 1;

        /// <summary>
        /// Google Satellite Images with Road Maps Overlayed
        /// </summary>
        public const int GoogleHybrid = 2;

        /// <summary>
        ///  Google Road Maps (China)
        /// </summary>
        public const int GoogleChina = 3;

        /// <summary>
        /// Yahoo Road Maps
        /// </summary>
        public const int YahooMap = 4;

        /// <summary>
        /// Yahoo Satellite Images
        /// </summary>
        public const int YahooSatellite = 5;

        /// <summary>
        /// Yahoo Satellite Images with Road Maps Overlayed
        /// </summary>
        public const int YahooHybrid = 6;

        /// <summary>
        /// Yahoo India Road Maps
        /// </summary>
        public const int YahooIndiaMap = 7;

        /// <summary>
        /// Yahoo Satellite Images with India Road Maps Overlayed
        /// </summary>
        public const int YahooIndiaHybrid = 8;

        /// <summary>
        /// Ask.com Road Maps
        /// </summary>
        public const int AskdotcomMap = 9;

        /// <summary>
        /// Ask.com Satellite Images
        /// </summary>
        public const int AskdotcomSatellite = 10;

        /// <summary>
        /// Ask.com Satellite Images with Labels
        /// </summary>
        public const int AskdotcomHybrid = 11;

        /// <summary>
        /// Microsoft Road Maps
        /// </summary>
        public const int MicrosoftMap = 12;

        /// <summary>
        ///  Microsoft Satellite Maps
        /// </summary>
        public const int MicrosoftSatellite = 13;

        /// <summary>
        /// Microsoft Satellite Images with Road Maps Overlayed
        /// </summary>
        public const int MicrosoftHybrid = 14;

        /// <summary>
        /// Microsoft Live China
        /// </summary>
        public const int MicrosoftChina = 15;

        /// <summary>
        /// Nokia normal map
        /// </summary>
        public const int NokiaMap = 16;

        /// <summary>
        ///  Map abc china
        /// </summary>
        public const int MapabcChina = 17;

        /// <summary>
        /// Google terren
        /// </summary>
        public const int GoogleTerren = 18;

        /// <summary>
        ///  OpenStreetMap.org Maps
        /// </summary>
        public const int OpenstreetMap = 19;

        /// <summary>
        /// Open satellite Maps
        /// </summary>
        public const int OpenSatellitetMap = 20;

        /// <summary>
        /// Open cycle Maps
        /// </summary>
        public const int OpenCycleMap = 21;

        /// <summary>
        ///  OSMA Maps
        /// </summary>
        public const int OsmaMap = 22;

        /// <summary>
        /// Microsoft terren
        /// </summary>
        public const int MicrosoftTerren = 23;

        /// <summary>
        ///  max map type
        /// </summary>
        public const int MaxmapType = MicrosoftTerren;

        /// <summary>
        /// Generic map type ,used to extension.
        /// </summary>
        public const int GenericMaptype1 = 190;

        /// <summary>
        ///  Generic map type ,used to extension.
        /// </summary>
        public const int GenericMaptype2 = 191;

        /// <summary>
        ///  Generic map type ,used to extension.
        /// </summary>
        public const int GenericMaptype3 = 192;

        /// <summary>
        ///  Generic map type ,used to extension.
        /// </summary>
        public const int GenericMaptype4 = 193;

        /// <summary>
        /// Generic map type ,used to extension.
        /// </summary>
        public const int GenericMaptype5 = 194;

        /// <summary>
        /// Generic map type ,used to extension.
        /// </summary>
        public const int GenericMaptype6 = 195;

        /// <summary>
        /// Generic map type ,used to extension.
        /// </summary>
        public const int GenericMaptype7 = 196;

        /// <summary>
        ///  Generic map type ,used to extension.
        /// </summary>
        public const int GenericMaptypeChina = 197;

        /// <summary>
        /// Routing direction map.
        /// </summary>
        public const int RoutingDirection = 198;

        /// <summary>
        /// Mapinfo Vector map type
        /// </summary>
        public const int MapinfoVectorMap = 199;

        /// <summary>
        ///  for each map type, what consists each map type.
        /// some map type like hybrid may consistes two map types, the satellites
        /// and the hybrid itself. 
        /// </summary>
        public static readonly Hashtable MapSequences = new Hashtable();

        /// <summary>
        /// map type names and it's index
        /// </summary>
        public static Hashtable MapTypeNames = new Hashtable();

        /// <summary>
        ///  map tile urls.
        /// </summary>
        public static Hashtable MapTypeUrls = new Hashtable();

        ///<summary>
        /// empty tile urls.
        ///</summary>
        public static string EmptyTileUrl="guidebee://emptytileurl";


        static MapType()
        {
            MapSequences.Add((GoogleMap), new[] { GoogleMap });
            MapSequences.Add((GoogleSatellite), new[] { MicrosoftSatellite });
            MapSequences.Add((GoogleHybrid), new[] { MicrosoftSatellite, GoogleHybrid });
            MapSequences.Add((GoogleChina), new[] { GoogleChina });
            MapSequences.Add((GoogleTerren), new[] { GoogleTerren });
            MapSequences.Add((YahooMap), new[] { YahooMap });
            MapSequences.Add((YahooSatellite), new[] { YahooSatellite });
            MapSequences.Add((YahooHybrid), new[] { YahooSatellite, YahooHybrid });
            MapSequences.Add((YahooIndiaMap), new[] { YahooIndiaMap });
            MapSequences.Add((YahooIndiaHybrid), new[] { YahooIndiaHybrid });
            MapSequences.Add((AskdotcomMap), new[] { AskdotcomMap });
            MapSequences.Add((AskdotcomSatellite), new[] { AskdotcomSatellite });
            MapSequences.Add((AskdotcomHybrid), new[] { AskdotcomSatellite, AskdotcomHybrid });
            MapSequences.Add((MicrosoftMap), new[] { MicrosoftMap });
            MapSequences.Add((MicrosoftSatellite), new[] { MicrosoftSatellite });
            MapSequences.Add((MicrosoftHybrid), new[] { MicrosoftHybrid });
            MapSequences.Add((MicrosoftChina), new[] { MicrosoftChina });
            MapSequences.Add((MicrosoftTerren), new[] { MicrosoftTerren });
            MapSequences.Add((OpenstreetMap), new[] { OpenstreetMap });
            MapSequences.Add((OpenSatellitetMap), new[] { OpenSatellitetMap });
            MapSequences.Add((OpenCycleMap), new[] { OpenCycleMap });
            MapSequences.Add((OsmaMap), new[] { OsmaMap });
            MapSequences.Add((NokiaMap), new[] { NokiaMap });
            MapSequences.Add((MapabcChina), new[] { MapabcChina });
            MapSequences.Add((RoutingDirection), new[] { RoutingDirection });
            MapSequences.Add((MapinfoVectorMap), new[] { MapinfoVectorMap });
            MapSequences.Add((GenericMaptype1), new[] { GenericMaptype1 });
            MapSequences.Add((GenericMaptype2), new[] { GenericMaptype2 });
            MapSequences.Add((GenericMaptype3), new[] { GenericMaptype3 });
            MapSequences.Add((GenericMaptype4), new[] { GenericMaptype4 });
            MapSequences.Add((GenericMaptype5), new[] { GenericMaptype5 });
            MapSequences.Add((GenericMaptypeChina), new[] { GenericMaptypeChina });
            MapSequences.Add((GenericMaptype6), new[] { GenericMaptype6 });
            MapSequences.Add((GenericMaptype7), new[] { GenericMaptype6, GenericMaptype7 });


            MapTypeNames.Add("GOOGLEMAP", (GoogleMap));
            MapTypeNames.Add("GOOGLESATELLITE", (GoogleSatellite));
            MapTypeNames.Add("GOOGLEHYBRID", (GoogleHybrid));
            MapTypeNames.Add("GOOGLECHINA", (GoogleChina));
            MapTypeNames.Add("GOOGLETERREN", (GoogleTerren));
            MapTypeNames.Add("YAHOOMAP", (YahooMap));
            MapTypeNames.Add("YAHOOSATELLITE", (YahooSatellite));
            MapTypeNames.Add("YAHOOHYBRID", (YahooHybrid));
            MapTypeNames.Add("YAHOOINDIAMAP", (YahooIndiaMap));
            MapTypeNames.Add("YAHOOINDIAHYBRID", (YahooIndiaHybrid));
            MapTypeNames.Add("MICROSOFTMAP", (MicrosoftMap));
            MapTypeNames.Add("MICROSOFTSATELLITE", (MicrosoftSatellite));
            MapTypeNames.Add("MICROSOFTHYBRID", (MicrosoftHybrid));
            MapTypeNames.Add("MICROSOFTCHINA", (MicrosoftChina));
            MapTypeNames.Add("MICROSOFTTERREN", (MicrosoftTerren));
            MapTypeNames.Add("OPENSTREETMAP", (OpenstreetMap));
            MapTypeNames.Add("OPENSATELLITETMAP", (OpenSatellitetMap));
            MapTypeNames.Add("OPENCYCLEMAP", (OpenCycleMap));
            MapTypeNames.Add("OSMAMAP", (OsmaMap));
            MapTypeNames.Add("NOKIAMAP", (NokiaMap));
            MapTypeNames.Add("MAPABCCHINA", (MapabcChina));

            MapTypeNames.Add("GENERIC_MAPTYPE_1", (GenericMaptype1));
            MapTypeNames.Add("GENERIC_MAPTYPE_2", (GenericMaptype2));
            MapTypeNames.Add("GENERIC_MAPTYPE_3", (GenericMaptype3));
            MapTypeNames.Add("GENERIC_MAPTYPE_4", (GenericMaptype4));
            MapTypeNames.Add("GENERIC_MAPTYPE_5", (GenericMaptype5));
            MapTypeNames.Add("GENERIC_MAPTYPE_6", (GenericMaptype6));
            MapTypeNames.Add("GENERIC_MAPTYPE_7", (GenericMaptype7));
            MapTypeNames.Add("GENERIC_MAPTYPE_CHINA", (GenericMaptypeChina));


        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Replaces the meta string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="zoomLevel">The zoom level.</param>
        /// <returns></returns>
        private static string ReplaceMetaString(string input, int x, int y, int zoomLevel)
        {
            int digit = ((x + y) % 4);
            zoomLevel = Numzoomlevels - zoomLevel;
            string[] pattern = new[]{
            "{GOOGLE_DIGIT}",
            "{X}",
            "{Y}",
            "{ZOOM}",
            "{GALILEO}",
            "{MS_DIGIT}",
            "{QUAD}",
            "{YAHOO_DIGIT}",
            "{YAHOO_Y}",
            "{YAHOO_ZOOM}",
            "{YAHOO_ZOOM_2}",
            "{OAM_ZOOM}",
            "{NOKIA_ZOOM}"
        };

            string quad = "";
            for (int i = zoomLevel - 1; i >= 0; i--)
            {
                quad = quad + (((((y >> i) & 1) << 1) + ((x >> i) & 1)));
            }
            string[] replace = new[]{
            digit.ToString(),//"{GOOGLE_DIGIT}"
            x.ToString(),//"{X}"
            y.ToString(),//"{Y}"
            zoomLevel.ToString(),//"{ZOOM}"
            "Galileo".Substring(0,(3 * x + y) % 8),//"{GALILEO}"
            (((y & 1) << 1) + (x & 1)).ToString(),//"{MS_DIGIT}"
            quad,//"{QUAD}"
            (1 + ((x + y) % 3)).ToString(),//"{YAHOO_DIGIT}"
            (((1 << (zoomLevel)) >> 1) - 1 - y).ToString(),//"{YAHOO_Y}"
            (zoomLevel + 1).ToString(),//"{YAHOO_ZOOM}"
            (Numzoomlevels - zoomLevel + 1).ToString(),//"{YAHOO_ZOOM_2}"
            (Numzoomlevels - zoomLevel).ToString(),//"{OAM_ZOOM}"
            zoomLevel.ToString()//"{NOKIA_ZOOM}"
        };
            string url = Utils.Replace(pattern, replace, input);
            return url;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the tile URL.
        /// </summary>
        /// <param name="mtype">The mtype.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="zoomLevel">The zoom level.</param>
        /// <returns></returns>
        public static string GetTileUrl(int mtype, int x, int y, int zoomLevel)
        {
            string retUrl = "";
            if (customMapType != null)
            {
                retUrl = customMapType.GetTileUrl(mtype, x, y, Numzoomlevels - zoomLevel);
            }
            if (!string.IsNullOrEmpty(retUrl))
            {
                return retUrl;
            }
            string metaUrl;
            lock (MapTypeUrls)
            {
                metaUrl = (string) MapTypeUrls[mtype];
            }
            if (metaUrl != null)
            {
                return ReplaceMetaString(metaUrl, x, y, zoomLevel);
            }
            return GetTileInternalUrl(mtype, x, y, zoomLevel);
        }
        static ICustomMapType customMapType ;
        internal const int Numzoomlevels = 17;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the tile internal URL.
        /// </summary>
        /// <param name="mtype">The mtype.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="zoomLevel">The zoom level.</param>
        /// <returns></returns>
        private static string GetTileInternalUrl(int mtype, int x, int y, int zoomLevel)
        {

            string url = "";
            mtype = mtype % (MaxmapType + 1);
            string galileo = "Galileo".Substring(0, (3 * x + y) % 8);
            switch (mtype)
            {
                case GoogleTerren:
                    url = "http://mt" + ((x + y) % 4) + ".google.com/vt/lyrs=t@108,r@138";
                    url += "&hl=en&s=" + galileo;
                    url += "&x=" + x + "&y=" + y + "&z=" + (Numzoomlevels - zoomLevel);
                    break;
                case MapabcChina:
                    url = "http://emap" + ((x + y) % 4) + ".mapabc.com/mapabc/maptile?v=";
                    url += "w2.99";
                    url += "&x=" + x + "&y=" + y + "&zoom=" + zoomLevel;
                    break;
                case GoogleChina:
                    url = "http://mt" + ((x + y) % 4) + ".google.cn/vt/lyrs=m@138&hl=zh-cn&gl=cn";
                    url += "&s=" + galileo;
                    url += "&x=" + x + "&y=" + y + "&zoom=" + zoomLevel;
                    break;
                case GoogleMap:
                    url = "http://mt" + ((x + y) % 4) + ".google.com/vt/lyrs=m@138&hl=en";
                    url += "&s=" + galileo;
                    url += "&x=" + x + "&y=" + y + "&z=" + (Numzoomlevels - zoomLevel);
                    break;
                case GoogleHybrid:
                    url = "http://mt" + ((x + y) % 4) + ".google.com/vt/lyrs=h@138&hl=en";
                    url += "&s=" + galileo;
                    url += "&x=" + x + "&y=" + y + "&z=" + (Numzoomlevels - zoomLevel);
                    break;

                case GoogleSatellite:
                    {
                        url = "http://khm" + ((x + y) % 4) + ".google.com/kh/v=58";
                        url += "&s=" + galileo;
                        url += "&x=" + x + "&y=" + y + "&z=" + (Numzoomlevels - zoomLevel);
                        break;
                    }
                case YahooMap:
                    url = "http://maps" + (1 + ((x + y) % 3)) + ".yimg.com/hx/";
                    url += "tl?v=4.3";
                    url += "&.intl=en&x=" + x + "&y=" + (((1 << (Numzoomlevels - zoomLevel)) >> 1) - 1 - y)
                            + "&z=" + (Numzoomlevels - zoomLevel + 1) + "&r=1";
                    break;
                case YahooSatellite:
                    url = "http://maps" + (1 + ((x + y) % 3)) + ".yimg.com/ae/ximg?";
                    url += "v=1.9&t=a&s=256";
                    url += "&.intl=en&x=" + x + "&y=" + (((1 << (Numzoomlevels - zoomLevel)) >> 1) - 1 - y)
                            + "&z=" + (Numzoomlevels - zoomLevel + 1) + "&r=1";
                    break;

                case YahooHybrid:
                    url = "http://maps" + (1 + ((x + y) % 3)) + ".yimg.com/hx/";
                    url += "tl?v=4.3&t=h";
                    url += "&.intl=en&x=" + x + "&y=" + (((1 << (Numzoomlevels - zoomLevel)) >> 1) - 1 - y)
                            + "&z=" + (Numzoomlevels - zoomLevel + 1) + "&r=1";
                    break;


                case YahooIndiaHybrid:
                case YahooIndiaMap:
                    url = "http://maps.yimg.com/hw/tile?locale=en&imgtype=png&yimgv=1.2&v=4.1";
                    url += "&x=" + x + "&y=" + (((1 << (Numzoomlevels - zoomLevel)) >> 1) - 1 - y)
                            + "&z=" + (zoomLevel + 1);
                    break;
                case MicrosoftHybrid:
                case MicrosoftMap:
                case MicrosoftSatellite:
                    url = "http://ecn.t";
                    url += (((y & 1) << 1) + (x & 1))
                            + ".tiles.virtualearth.net/tiles/";
                    url += (mtype == MicrosoftMap)
                            ? "r"
                            : (mtype == MicrosoftSatellite)
                            ? "a" : "h";
                    for (int i = Numzoomlevels - zoomLevel - 1; i >= 0; i--)
                    {
                        url = url + (((((y >> i) & 1) << 1) + ((x >> i) & 1)));
                    }
                    url += ".png?g=556";
                    break;
                case MicrosoftChina:
                    url = "http://r";
                    url += (((y & 1) << 1) + (x & 1))
                            + ".tiles.ditu.live.com/tiles/";
                    url += "r";
                    for (int i = Numzoomlevels - zoomLevel - 1; i >= 0; i--)
                    {
                        url = url + (((((y >> i) & 1) << 1) + ((x >> i) & 1)));
                    }
                    url += ".png?g=54";
                    break;
                case MicrosoftTerren:
                    url = "http://ecn.t";
                    url += (((y & 1) << 1) + (x & 1))
                            + ".tiles.virtualearth.net/tiles/";
                    url += "r";
                    for (int i = Numzoomlevels - zoomLevel - 1; i >= 0; i--)
                    {
                        url = url + (((((y >> i) & 1) << 1) + ((x >> i) & 1)));
                    }
                    url += ".png?g=556&mkt=en-us&shading=hill&n=z";
                    break;
                case AskdotcomHybrid:
                case AskdotcomMap:
                case AskdotcomSatellite:
                    url = (zoomLevel > 6)
                            ? "http://mapstatic"
                            : "http://mapcache";
                    url += ((x + y) % 4 + 1) + ".ask.com/";
                    url += (mtype == AskdotcomMap)
                            ? "map"
                            : (mtype == AskdotcomSatellite)
                            ? "sat" : "mapt";
                    url += "/" + (zoomLevel + 2) + "/";
                    url += (x - ((1 << (Numzoomlevels - zoomLevel)) >> 1))
                            + "/" + (y - ((1 << (Numzoomlevels - zoomLevel)) >> 1));
                    url += "?partner=&tc=28";
                    break;
                case OpenstreetMap:
                    url = "http://tile.openstreetmap.org/"
                            + (Numzoomlevels - zoomLevel)
                            + "/" + x + "/" + y + ".png";
                    break;
                case NokiaMap:
                    url = "http://maptile.svc.nokia.com.edgesuite.net/maptiler/maptile/0.1.22.103/normal.day/"
                            + (Numzoomlevels - zoomLevel)
                            + "/" + x + "/" + y + "/256/png";
                    break;
                case OpenSatellitetMap:
                    url = "http://tile.openaerialmap.org/tiles/?v=mgm&;layer=openaerialmap-900913";
                    url += "&x=" + x + "&y=" + y
                            + "&z=" + zoomLevel;
                    break;
                case OpenCycleMap:
                    url = "http://andy.sandbox.cloudmade.com/tiles/cycle/";
                    url += (Numzoomlevels - zoomLevel)
                            + "/" + x + "/" + y + ".png";
                    break;
                case OsmaMap:
                    url = "http://tah.openstreetmap.org/Tiles/tile/";
                    url += (Numzoomlevels - zoomLevel)
                            + "/" + x + "/" + y + ".png";
                    break;
            }
            return url;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 12AUG2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /**
         * Manually set the map tile url template
         * @param mType map type.
         * @param urlTemplate url tempalte
         */
        public static void SetCustomMapTileUrl(ICustomMapType customMapTypeUrl)
        {
            customMapType = customMapTypeUrl;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 12AUG2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the map tile URL.
        /// </summary>
        /// <param name="mType">Type of the m.</param>
        /// <param name="urlTemplate">The URL template.</param>
        public static void SetMapTileUrl(int mType, string urlTemplate)
        {
            lock (MapTypeUrls)
            {
                MapTypeUrls.Add(mType, urlTemplate);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 12SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Updates the map tile URL.
        /// </summary>
        public static void UpdateMapTileUrl()
        {
            try
            {
                lock (_syncObject)
                {
                    RestoreMapUrls();
                    Request.Get(_queryUrl + "version", null, null, _mapTileUrlQuery, "version");

                }
                _syncObject.WaitOne(5000, false);
            }
            catch (Exception e)
            {
                Log.P(e.Message);
            }
        }
        /**
         * record store name
         */
        private const string MapTileUrlRecordstoreName = "Guidebee_ServiceUrl";
        /**
         * record store
         */
        private static RecordStore _mapDataRecordStore;
        internal static MapTileUrlQuery _mapTileUrlQuery = new MapTileUrlQuery();
        internal static string _queryUrl = "http://www.mapdigit.com/guidebeemap/config.php?q=";
        internal static AutoResetEvent _syncObject = new AutoResetEvent(false);
        internal static int _version;

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
        internal static void SaveMapUrls()
        {

            try
            {
                RecordStore.DeleteRecordStore(MapTileUrlRecordstoreName);
            }
            catch (Exception)
            {

            }
            try
            {

                lock (MapTypeUrls)
                {
                    _mapDataRecordStore = RecordStore.OpenRecordStore(MapTileUrlRecordstoreName, true);
                    byte[] version = new byte[1];
                    version[0] = (byte)_version;
                    _mapDataRecordStore.AddRecord(version, 0, 1);
                    IEnumerator emu = MapTypeUrls.GetEnumerator();
                    while (emu.MoveNext())
                    {
                        DictionaryEntry dictionaryEntry = (DictionaryEntry) emu.Current;
                        int mapTypeIndex = (int)dictionaryEntry.Key;
                        string url = (string)MapTypeUrls[mapTypeIndex];
                        if (!string.IsNullOrEmpty(url))
                        {
                            byte[] recordDate = ToByteArray(mapTypeIndex.ToString(), url);
                            if (recordDate != null)
                            {
                                _mapDataRecordStore.AddRecord(recordDate, 0, recordDate.Length);
                            }
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
        internal static void RestoreMapUrls()
        {
            {
                try
                {
                    lock (MapTypeUrls)
                    {
                        MapTypeUrls.Clear();
                        if (_mapDataRecordStore == null)
                        {
                            _mapDataRecordStore = RecordStore.OpenRecordStore(MapTileUrlRecordstoreName, false);

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
                                    if (!string.IsNullOrEmpty(mapType))
                                    {
                                        int mapTypeIndex = int.Parse(mapType);
                                        string url = dis.ReadString();
                                        MapTypeUrls.Add(mapTypeIndex, url);
                                    }
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
                catch (Exception)
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

    class MapTileUrlQuery : IRequestListener
    {

        public void ReadProgress(object context, int bytes, int total)
        {
        }

        public void WriteProgress(object context, int bytes, int total)
        {
        }

        public void Done(object context, Response response)
        {
            //lock (MapType.syncObject)
            {
                if ((string)context == "version")
                {
                    if (response != null)
                    {
                        try
                        {
                            string ver = response.GetResult().GetAsString("Version");
                            int verNo = int.Parse(ver);
                            if (verNo > MapType._version || verNo == 0)
                            {
                                MapType._version = verNo;
                                Request.Get(MapType._queryUrl + "maptileurl", null, null, MapType._mapTileUrlQuery, "maptileurl");
                            }
                            else
                            {
                                MapType._syncObject.Set();
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                else if ((string)context == "maptileurl")
                {
                    if (response != null)
                    {
                        Result result = response.GetResult();
                        int count = result.GetSizeOfArray("maptileUrls");
                        lock (MapType.MapTypeUrls)
                        {
                            MapType.MapTypeUrls.Clear();
                            for (int i = 0; i < count; i++)
                            {
                                string mapType = result.GetAsString("maptileUrls[" + i + "].type");
                                string mapTileUrl = result.GetAsString("maptileUrls[" + i + "].URL");
                                object mapTypeIndex = MapType.MapTypeNames[mapType];
                                MapType.MapTypeUrls.Add(mapTypeIndex, mapTileUrl);
                            }
                        }

                        MapType.SaveMapUrls();
                    }
                    MapType._syncObject.Set();

                }
            }
        }

    }

}
