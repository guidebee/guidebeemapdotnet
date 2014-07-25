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
using Mapdigit.Network;
using Mapdigit.Util;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Gis.Service.CloudMade
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 27SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// This class is used to communicate directly with cloudMade servers to obtain
    /// geocodes for user specified addresses. In addition, a geocoder maintains
    /// its own cache of addresses, which allows repeated queries to be answered
    /// without a round trip to the server.
    /// </summary>
    internal sealed class CReverseClientGeocoder : IReverseGeocoder
    {

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
            GetLocations(-1, address, listener);
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
        public void GetLocations(GeoLatLng address, IReverseGeocodingListener listener)
        {
            GetLocations(address.Longitude + "," + address.Latitude, listener);
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
            _listener = listener;

            _queryKey = CloudMadeMapService.GetCloudMadeKey();
            bool isLngLat = true;

            try
            {
                string tmpAddress = address;
                string strLngLat = "[" + tmpAddress + ",0]";
                MapLayer.FromStringToLatLng(strLngLat);
                _searchAddress = tmpAddress;

            }
            catch (Exception)
            {
                isLngLat = false;
            }
            string searchBase = ReplaceMetaString(SearchBase);
            if (isLngLat)
            {

                ArrayList argList = new ArrayList();
                argList.Add(new Arg("around", _searchAddress));
                SearchOptions routeOptions = DigitalMapService.GetSearchOptions();
                argList.Add(new Arg("results ", routeOptions.NumberOfSearchResult.ToString()));
                argList.Add(new Arg("distance", "closest"));
                argList.Add(new Arg("return_location", "true"));
                Arg[] args = new Arg[argList.Count + 1];
                argList.CopyTo(args);
                args[argList.Count] = null;
                Request.Get(searchBase, args, null, _addressQuery, this);

            }

        }
        private readonly Hashtable _addressCache = new Hashtable();
        private string _searchAddress;
        private IReverseGeocodingListener _listener;
        private readonly ReverseAddressQuery _addressQuery = new ReverseAddressQuery();
        private const string SearchBase = "http://geocoding.cloudmade.com/{CLOUDMADE_KEY}/geocoding/v2/find.js";
        private string _queryKey = "8ee2a50541944fb9bcedded5165f09d9";



        private string ReplaceMetaString(string input)
        {

            string[] pattern = new[]{
            "{CLOUDMADE_KEY}",
            " "
        };

            string[] replace = new[]{
            _queryKey,
            "+"
        };

            string url = Utils.Replace(pattern, replace, input);
            return url;
        }



        private class ReverseAddressQuery : IRequestListener
        {

            public void ReadProgress(Object context, int bytes, int total)
            {
                if (context is CReverseClientGeocoder)
                {
                    CReverseClientGeocoder geoCoder = (CReverseClientGeocoder)context;
                    if (geoCoder._listener != null)
                    {
                        geoCoder._listener.ReadProgress(bytes, total);
                    }
                }
            }

            public void WriteProgress(Object context, int bytes, int total)
            {
            }

            public void Done(Object context, Response response)
            {
                if (context is CReverseClientGeocoder)
                {
                    CReverseClientGeocoder geoCoder = (CReverseClientGeocoder)context;
                    SearchResponse(geoCoder, response);
                }
            }

            private static void SearchResponse(CReverseClientGeocoder geoCoder, Response response)
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

                    int resultCount = result.GetSizeOfArray(prefix + "features");
                    if (resultCount > 0)
                    {

                        mapPoints = new MapPoint[resultCount];
                        for (int i = 0; i < resultCount; i++)
                        {
                            mapPoints[i] = new MapPoint();
                            mapPoints[i].Name = result.GetAsString(prefix + "features[" + i + "].properties.addr:housenumber");
                            mapPoints[i].Name += "," + result.GetAsString(prefix + "features[" + i + "].properties.addr:street");
                            double[] latLngArray = result.GetAsDoubleArray(prefix + "features[" + i + "].centroid.coordinates");
                            string address = result.GetAsString(prefix + "features[" + i + "].location.county");
                            address += "," + result.GetAsString(prefix + "features[" + i + "].location.country");
                            mapPoints[i].ObjectNote = (address);
                            mapPoints[i].Point = (new GeoLatLng(latLngArray[0], latLngArray[1]));

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
                        geoCoder._addressCache[mapPoints[0].Name] = mapPoints[0];

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
        }
    }


}
