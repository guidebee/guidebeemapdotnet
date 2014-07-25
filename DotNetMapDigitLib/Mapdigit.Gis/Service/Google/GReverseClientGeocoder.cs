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
using Mapdigit.Ajax;
using Mapdigit.Gis.Geometry;
using Mapdigit.Gis.Raster;
using Mapdigit.Network;
using Mapdigit.Util;

//--------------------------------- PACKAGE ------------------------------------
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
    internal sealed class GReverseClientGeocoder : IReverseGeocoder
    {

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="GReverseClientGeocoder"/> class.
        /// </summary>
        public GReverseClientGeocoder()
        {
            _reverseAddressQuery = new ReverseAddressQuery();
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
        /// <param name="china">query china or not.</param>
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
        /// <param name="key">The key.</param>
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
        /// Sends a request to Google servers to reverse geocode the specified address
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="listener">The listener.</param>
        public void GetLocations(string address, IReverseGeocodingListener listener)
        {
            _listener = listener;
            _searchAddress = address;
            SetGoogleKey(GoogleMapService.GetGoogleKey());
            MapPoint mapPoint = (MapPoint)_addressCache[address];

            if (mapPoint == null)
            {

                string input;
                if (_isChina)
                {
                    input = GoogleMapService.GetMapServiceUrl(GoogleMapService._reverseGeocoderChina);
                }
                else
                {
                    input = GoogleMapService.GetMapServiceUrl(GoogleMapService._reversegeocoder);
                }
                if (input != null)
                {
                    string queryUrl = ReplaceMetaString(address, input);
                    Request.Get(queryUrl, null, null, _reverseAddressQuery, this);
                }
                else
                {

                    ArrayList argList = new ArrayList();
                    argList.Add(new Arg("ll", address));
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
                    SearchOptions searchOptions = DigitalMapService.GetSearchOptions();
                    if (searchOptions.LanguageId.Length != 0 && !_isChina)
                    {
                        argList.Add(new Arg("hl", searchOptions.LanguageId));
                    }
                    argList.Add(new Arg("key", _queryKey));
                    Arg[] args = new Arg[argList.Count + 1];
                    argList.CopyTo(args);
                    args[argList.Count] = null;
                    if (!_isChina)
                    {

                        Request.Get(SearchBase, args, null, _reverseAddressQuery, this);
                    }
                    else
                    {

                        Request.Get(SearchBaseChina, args, null, _reverseAddressQuery, this);
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


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sends a request to Google servers to reverse geocode the specified address
        /// </summary>
        /// <param name="mapType">Type of the map.</param>
        /// <param name="address">The address.</param>
        /// <param name="listener">The listener.</param>
        public void GetLocations(int mapType, string address, IReverseGeocodingListener listener)
        {
            _isChina = mapType == MapType.MicrosoftChina || mapType == MapType.GoogleChina
                    || mapType == MapType.MapabcChina || mapType == MapType.GenericMaptypeChina;
            SetChina(_isChina);

            GetLocations(address, listener);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28DEC2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sends a request to Google servers to reverse geocode the specified address
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="listener">The listener.</param>
        public void GetLocations(GeoLatLng address, IReverseGeocodingListener listener)
        {
            GetLocations(address.Longitude + "," + address.Latitude, listener);
        }
        private const string SearchBase = "http://maps.google.com/maps/geo";
        private const string SearchBaseChina = "http://ditu.google.cn/maps/geo";
        private readonly Hashtable _addressCache = new Hashtable();
        private string _searchAddress;
        private IReverseGeocodingListener _listener;
        private readonly ReverseAddressQuery _reverseAddressQuery;
        private string _queryKey = "ABQIAAAAi44TY0V29QjeejKd2l3ipRTRERdeAiwZ9EeJWta3L_JZVS0bOBQlextEji5FPvXs8mXtMbELsAFL0w";
        private static bool _isChina;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Replaces the meta string.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        private string ReplaceMetaString(string address, string input)
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

        private class ReverseAddressQuery : IRequestListener
        {

            private static void SearchResponse(GReverseClientGeocoder reverseGeoCoder, Response response)
            {
                MapPoint[] mapPoints = null;
                Exception ex = response.GetException();
                if (ex != null || response.GetCode() != HttpConnection.HttpOk)
                {
                    if (ex is OutOfMemoryException)
                    {
                        Log.P("Dont have enough memory", Log.Error);
                        if (reverseGeoCoder._listener != null)
                        {
                            reverseGeoCoder._listener.Done(null, null);
                        }
                    }
                    else
                    {
                        Log.P("Error connecting to search service", Log.Error);
                        if (reverseGeoCoder._listener != null)
                        {
                            reverseGeoCoder._listener.Done(reverseGeoCoder._searchAddress, null);
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
                        if (reverseGeoCoder._addressCache.Count > 24)
                        {
                            IEnumerator enumerator = reverseGeoCoder._addressCache.GetEnumerator();
                            object[] aboutToDeleted = new object[12];
                            for (int i = 0; i < aboutToDeleted.Length; i++)
                            {
                                DictionaryEntry dictionaryEntry = (DictionaryEntry)enumerator.Current;
                                aboutToDeleted[i] = dictionaryEntry.Key;
                                enumerator.MoveNext();
                            }

                            for (int j = 0; j < 12; j++)
                            {
                                reverseGeoCoder._addressCache.Remove(aboutToDeleted[j]);
                            }


                        }
                        reverseGeoCoder._addressCache[mapPoints[0].Name]= mapPoints[0];
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
                if (reverseGeoCoder._listener != null)
                {
                    reverseGeoCoder._listener.Done(reverseGeoCoder._searchAddress, mapPoints);
                }

            }

            public void ReadProgress(object context, int bytes, int total)
            {
                if (context is GReverseClientGeocoder)
                {
                    GReverseClientGeocoder reverseGeoCoder = (GReverseClientGeocoder)context;
                    if (reverseGeoCoder._listener != null)
                    {
                        reverseGeoCoder._listener.ReadProgress(bytes, total);
                    }
                }
            }

            public void WriteProgress(object context, int bytes, int total)
            {
            }

            public void Done(object context, Response response)
            {
                if (context is GReverseClientGeocoder)
                {
                    GReverseClientGeocoder reverseGeoCoder = (GReverseClientGeocoder)context;
                    SearchResponse(reverseGeoCoder, response);

                }
            }
        }
    }


}
