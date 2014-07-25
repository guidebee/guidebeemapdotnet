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
    internal class GLocalSearch : ILocalSearch
    {
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="GLocalSearch"/> class.
        /// </summary>
        public GLocalSearch()
        {
            _addressQuery = new LocalAddressQuery();
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
        /// Set google query key.
        /// </summary>
        /// <param name="key">key google query key.</param>
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
        /// <param name="address">The address.</param>
        /// <param name="start">The start index.</param>
        /// <param name="center">The center.</param>
        /// <param name="bound">The bound.</param>
        /// <param name="listener">The listener.</param>
        public void GetLocations(string address, int start, GeoLatLng center,
            GeoBounds bound, IGeocodingListener listener)
        {
            _listener = listener;
            _searchAddress = address;
            try
            {
                address = Html2Text.Encodeutf8(_encoding.GetBytes(address));
            }
            catch (Exception) { }
            MapPoint mapPoint = (MapPoint)_addressCache[address];
            SetGoogleKey(GoogleMapService.GetGoogleKey());

            if (mapPoint == null)
            {
                string input = GoogleMapService.GetMapServiceUrl(GoogleMapService._localsearch);
                if (input != null)
                {
                    string queryUrl = ReplaceMetaString(address, start, center, bound, input);
                    Request.Get(queryUrl, null, null, _addressQuery, this);
                }
                else
                {

                    ArrayList argList = new ArrayList();
                    argList.Add(new Arg("q", address));
                    argList.Add(new Arg("v", LocalSearchVer));
                    argList.Add(new Arg("output", "json"));
                    argList.Add(new Arg("oe", "utf8"));
                    argList.Add(new Arg("ie", "utf8"));
                    argList.Add(new Arg("start", start.ToString()));
                    argList.Add(new Arg("sll", center.Latitude + "," + center.Longitude));
                    argList.Add(new Arg("sspn", bound.Width + "," + bound.Height));
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


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sends a request to Google servers to geocode the specified address
        /// </summary>
        /// <param name="mapType">Type of the map.</param>
        /// <param name="address">The address.</param>
        /// <param name="start">The start index.</param>
        /// <param name="center">The center.</param>
        /// <param name="bound">The bound.</param>
        /// <param name="listener">The listener.</param>
        public void GetLocations(int mapType, string address, int start, GeoLatLng center,
            GeoBounds bound, IGeocodingListener listener)
        {
            _isChina = mapType == MapType.MicrosoftChina || mapType == MapType.GoogleChina
                    || mapType == MapType.MapabcChina || mapType == MapType.GenericMaptypeChina;
            SetChina(_isChina);
            GetLocations(address, start, center, bound, listener);

        }

        private const string SearchBase = "http://ajax.googleapis.com/ajax/services/search/local";
        private const string SearchBaseChina = "http://ajax.googleapis.com/ajax/services/search/local";
        private const string LocalSearchVer = "1.0";
        private readonly Hashtable _addressCache = new Hashtable();
        private static readonly Html2Text Html2Text = new Html2Text();
        private string _searchAddress;
        private IGeocodingListener _listener;
        private readonly LocalAddressQuery _addressQuery;
        private string _queryKey = "ABQIAAAAi44TY0V29QjeejKd2l3ipRTRERdeAiwZ9EeJWta3L_JZVS0bOBQlextEji5FPvXs8mXtMbELsAFL0w";
        private bool _isChina;
        private readonly UTF8Encoding _encoding = new UTF8Encoding();

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
        /// <param name="start">The start.</param>
        /// <param name="center">The center.</param>
        /// <param name="bound">The bound.</param>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        private string ReplaceMetaString(string address, int start, GeoLatLng center,
                GeoBounds bound, string input)
        {

            string[] pattern = new[]{
                "{QUERY}",
                "{GOOGLE_KEY}",
                "{START}",
                "{CENTER}",
                "{BOUND}",
                "{LANG}",
                " ",};

            string lang = "";

            SearchOptions searchOptions = DigitalMapService.GetSearchOptions();
            if (searchOptions.LanguageId.Length != 0 && !_isChina)
            {
                lang = "&hl=" + searchOptions.LanguageId;
            }

            string[] replace = new[]{
            address,
            _queryKey,
            start.ToString(),
            center.Latitude + "," + center.Longitude,
            bound.Width + "," + bound.Height,
            lang,
            "+"
        };

            string url = Utils.Replace(pattern, replace, input);
            return url;
        }


        private class LocalAddressQuery : IRequestListener
        {
            private static void SearchResponse(GLocalSearch localSearch, Response response)
            {
                MapPoint[] mapPoints = null;
                Exception ex = response.GetException();
                if (ex != null || response.GetCode() != HttpConnection.HttpOk)
                {
                    if (ex is OutOfMemoryException)
                    {
                        Log.P("Dont have enough memory", Log.Error);
                        if (localSearch._listener != null)
                        {
                            localSearch._listener.Done(null, null);
                        }
                    }
                    else
                    {
                        Log.P("Error connecting to search service", Log.Error);
                        if (localSearch._listener != null)
                        {
                            localSearch._listener.Done(localSearch._searchAddress, null);
                        }
                    }

                    return;
                }
                try
                {
                    Result result = response.GetResult();
                    string statusCode = result.GetAsString("responseStatus");
                    if (statusCode.EndsWith("200"))
                    {
                        int resultCount = result.GetSizeOfArray("responseData.results");
                        if (resultCount > 0)
                        {
                            mapPoints = new MapPoint[resultCount];
                            for (int i = 0; i < resultCount; i++)
                            {
                                mapPoints[i] = new MapPoint();
                                string address = result.GetAsString("responseData.results[" + i + "].title") + "(" + result.GetAsString("responseData.results[" + i + "].streetAddress") + "," + result.GetAsString("responseData.results[" + i + "].city") + ")";
                                mapPoints[i].Name = Html2Text.Convert(address);
                                string lat = result.GetAsString("responseData.results[" + i + "].lat");
                                string lng = result.GetAsString("responseData.results[" + i + "].lng");
                                string location = "[" + lng + "," + lat + ",0]";
                                GeoLatLng latLng = MapLayer.FromStringToLatLng(location);
                                mapPoints[i].SetPoint(latLng);
                                int phoneNumberCount = result.GetSizeOfArray("responseData.results[" + i + "].phoneNumbers");
                                string note = result.GetAsString("responseData.results[" + i + "].titleNoFormatting") + "\r\n";
                                for (int j = 0; j < phoneNumberCount; j++)
                                {
                                    string phoneNo = result.GetAsString("responseData.results[" + i + "].phoneNumbers[" + j + "].number");
                                    note += phoneNo + " ";
                                }
                                mapPoints[i].ObjectNote = note;

                            }
                            if (localSearch._addressCache.Count > 24)
                            {
                                IEnumerator enumerator = localSearch._addressCache.GetEnumerator();
                                object[] aboutToDeleted = new object[12];
                                for (int i = 0; i < aboutToDeleted.Length; i++)
                                {
                                    aboutToDeleted[i] = enumerator.Current;
                                    enumerator.MoveNext();
                                }

                                for (int j = 0; j < 12; j++)
                                {
                                    localSearch._addressCache.Remove(aboutToDeleted[j]);
                                }
                            }
                            localSearch._addressCache[mapPoints[0].Name]= mapPoints[0];
                        }
                    }

                }
                catch (Exception rex)
                {
                    Log.P("Error extracting result information:" + rex.Message, Log.Error);

                }
                if (localSearch._listener != null)
                {
                    localSearch._listener.Done(localSearch._searchAddress, mapPoints);
                }

            }

            public void ReadProgress(object context, int bytes, int total)
            {
                if (context is GLocalSearch)
                {
                    GLocalSearch geoCoder = (GLocalSearch)context;
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
                if (context is GLocalSearch)
                {
                    GLocalSearch geoCoder = (GLocalSearch)context;
                    SearchResponse(geoCoder, response);
                }
            }
        }
    }
}
