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
using System.Text;
using Mapdigit.Ajax;
using Mapdigit.Gis.Geometry;
using Mapdigit.Gis.Raster;
using Mapdigit.Network;
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
    /// This class is used to communicate directly with Google servers to obtain
    /// geocodes for user specified addresses. In addition, a geocoder maintains
    /// its own cache of addresses, which allows repeated queries to be answered
    /// without a round trip to the server.
    /// </summary>
    internal sealed class GClientGeocoder : IGeocoder
    {

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="GClientGeocoder"/> class.
        /// </summary>
        public GClientGeocoder()
        {
            _addressQuery = new AddressQuery();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set google china or not.
        /// </summary>
        /// <param name="china">iquery china or not.</param>
        public void SetChina(bool china)
        {
            _isChina = china;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the google key.
        /// </summary>
        /// <param name="key">google query key.</param>
        public void SetGoogleKey(string key)
        {
            _queryKey = key;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sends a request to Google servers to geocode the specified address
        /// </summary>
        /// <param name="address">address to query</param>
        /// <param name="listener">callback when query is done.</param>
        public void GetLocations(string address, IGeocodingListener listener)
        {
            _listener = listener;
            _searchAddress = address;
            try
            {
                address = Html2Text.Encodeutf8(_encoding.GetBytes(address));
            }
            catch (Exception) { }
            SetGoogleKey(GoogleMapService.GetGoogleKey());
            MapPoint mapPoint = (MapPoint)_addressCache[_searchAddress];
            if (mapPoint == null)
            {

                string input;
                if (_isChina)
                {
                    input = GoogleMapService.GetMapServiceUrl(GoogleMapService._geocoderChina);
                }
                else
                {
                    input = GoogleMapService.GetMapServiceUrl(GoogleMapService._geoCoderString);

                }
                if (input != null)
                {
                    string queryUrl = ReplaceMetaString(address, input);
                    Request.Get(queryUrl, null, null, _addressQuery, this);
                }
                else
                {

                    ArrayList argList = new ArrayList();
                    argList.Add(new Arg("q", address));
                    if (GoogleMapService._usingJson)
                    {
                        argList.Add(new Arg("output", "json"));
                    }
                    else
                    {
                        argList.Add(new Arg("output", "kml"));
                    }
                    argList.Add(new Arg("oe", "utf8"));
                    argList.Add(new Arg("ie", "utf8"));
                    argList.Add(new Arg("key", _queryKey));

                    SearchOptions searchOptions = DigitalMapService.GetSearchOptions();
                    if (searchOptions.LanguageId.Length != 0 && !_isChina)
                    {
                        argList.Add(new Arg("hl", searchOptions.LanguageId));
                    }
                    Arg[] args = new Arg[argList.Count + 1];
                    argList.CopyTo(args);
                    args[argList.Count] = null;
                    if (!_isChina)
                    {

                        Request.Get(SearchBase, args, null, _addressQuery, this);
                    }
                    else
                    {

                        Request.Get(SearchBaseChina, args, null, _addressQuery, this);
                    }
                }
            }
            else
            {
                MapPoint[] mapPoints = new MapPoint[1];
                mapPoints[0] = mapPoint;
                listener.Done(mapPoint.Name, mapPoints);
            }
        }

        private static string ReplaceMetaString(string address, string input)
        {

            string[] pattern = new[]{
                "{ADDRESS}",
                "{GOOGLE_KEY}",
                "{LANG}",
                " "
            };

            string lang = "";
            SearchOptions searchOptions = DigitalMapService.GetSearchOptions();
            if (searchOptions.LanguageId.Length != 0 && !_isChina)
            {
                lang = "&hl=" + searchOptions.LanguageId;
            }

            string[] replace = new[]{
                address,
                _queryKey,
                lang,
                "+"
            };

            string url = Utils.Replace(pattern, replace, input);
            return url;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sends a request to Google servers to geocode the specified address
        /// </summary>
        /// <param name="mapType">the map type</param>
        /// <param name="address">address to query</param>
        /// <param name="listener">callback when query is done.</param>
        public void GetLocations(int mapType, string address, IGeocodingListener listener)
        {
            _isChina = mapType == MapType.MicrosoftChina || mapType == MapType.GoogleChina
                    || mapType == MapType.MapabcChina || mapType == MapType.GenericMaptypeChina;
            SetChina(_isChina);
            GetLocations(address, listener);
        }
        private const string SearchBase = "http://maps.google.com/maps/geo";
        private const string SearchBaseChina = "http://ditu.google.cn/maps/geo";
        private readonly Hashtable _addressCache = new Hashtable();
        private string _searchAddress;
        private IGeocodingListener _listener;
        private readonly AddressQuery _addressQuery;
        private static string _queryKey = "ABQIAAAAi44TY0V29QjeejKd2l3ipRTRERdeAiwZ9EeJWta3L_JZVS0bOBQlextEji5FPvXs8mXtMbELsAFL0w";
        private static bool _isChina;
        private readonly UTF8Encoding _encoding = new UTF8Encoding();


        private class AddressQuery : IRequestListener
        {



            private static void SearchResponse(GClientGeocoder geoCoder, Response response)
            {
                MapPoint[] mapPoints = null;
                Exception ex = response.GetException();
                if (ex != null || response.GetCode() != HttpConnection.HttpOk)
                {
                    if (ex is OutOfMemoryException)
                    {
                        Log.P("Dont have enough memory", Log.Error);
                        if (geoCoder._listener != null)
                        {
                            geoCoder._listener.Done(null, null);
                        }
                    }
                    else
                    {
                        Log.P("Error connecting to search service", Log.Error);
                        if (geoCoder._listener != null)
                        {
                            geoCoder._listener.Done(geoCoder._searchAddress, null);
                        }
                    }

                    return;
                }
                try
                {
                    Result result = response.GetResult();
                    string prefix = "";
                    if (!GoogleMapService._usingJson)
                    {
                        prefix = "kml.Response.";
                    }
                    int resultCount = result.GetSizeOfArray(prefix + "Placemark");
                    if (resultCount >= 0)
                    {
                        if (resultCount == 0)
                        {
                            mapPoints = new MapPoint[1];
                            int i = 0;
                            mapPoints[i] = new MapPoint();
                            mapPoints[i].Name = result.GetAsString(prefix + "Placemark.address");
                            if (_isChina)
                            {
                                if (mapPoints[i].Name.StartsWith(GoogleMapService._chineseFullName))
                                {
                                    mapPoints[i].Name = mapPoints[i].Name.Substring(GoogleMapService._chineseFullName.Length);
                                }
                            }
                            string location = result.GetAsString(prefix + "Placemark.Point.coordinates");
                            GeoLatLng latLng = MapLayer.FromStringToLatLng(location);
                            mapPoints[i].SetPoint(latLng);


                        }
                        else
                        {
                            mapPoints = new MapPoint[resultCount];
                            for (int i = 0; i < resultCount; i++)
                            {
                                mapPoints[i] = new MapPoint();
                                mapPoints[i].Name = result.GetAsString(prefix + "Placemark[" + i + "].address");
                                if (_isChina)
                                {
                                    if (mapPoints[i].Name.StartsWith(GoogleMapService._chineseFullName))
                                    {
                                        mapPoints[i].Name = mapPoints[i].Name.Substring(GoogleMapService._chineseFullName.Length);
                                    }
                                }
                                string location = result.GetAsString(prefix + "Placemark[" + i + "].Point.coordinates");
                                GeoLatLng latLng = MapLayer.FromStringToLatLng(location);
                                mapPoints[i].SetPoint(latLng);

                            }
                        }
                        if (geoCoder._addressCache.Count > 24)
                        {

                            IEnumerator enumerator = geoCoder._addressCache.GetEnumerator();
                            object[] aboutToDeleted = new object[12];
                            for (int i = 0; i < aboutToDeleted.Length; i++)
                            {
                                DictionaryEntry dictionaryEntry = (DictionaryEntry)enumerator.Current;
                                aboutToDeleted[i] = dictionaryEntry.Key;
                                enumerator.MoveNext();
                            }

                            for (int j = 0; j < 12; j++)
                            {
                                geoCoder._addressCache.Remove(aboutToDeleted[j]);
                            }


                        }
                        geoCoder._addressCache[mapPoints[0].Name]=mapPoints[0];
                    }

                }
                catch (OutOfMemoryException)
                {
                    GC.Collect();
                }
                catch (Exception rex)
                {
                    Log.P("Error extracting result information:" + rex.Message, Log.Error);

                }
                if (geoCoder._listener != null)
                {
                    geoCoder._listener.Done(geoCoder._searchAddress, mapPoints);
                }

            }

            public void ReadProgress(object context, int bytes, int total)
            {
                if (context is GClientGeocoder)
                {
                    GClientGeocoder geoCoder = (GClientGeocoder)context;
                    if (geoCoder._listener != null)
                    {
                        geoCoder._listener.ReadProgress(bytes, total);
                    }
                }
            }

            public void WriteProgress(object context, int bytes, int total)
            {
            }

            public void Done(object context, Response response)
            {
                if (context is GClientGeocoder)
                {
                    GClientGeocoder geoCoder = (GClientGeocoder)context;
                    SearchResponse(geoCoder, response);
                }
            }

        }
    }


}
