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
    /// This class is used to communicate directly with cloudmade servers to obtain
    /// geocodes for user specified addresses. In addition, a geocoder maintains
    /// its own cache of addresses, which allows repeated queries to be answered
    /// without a round trip to the server.
    /// </summary>
    internal sealed class CLocalSearch : ILocalSearch
    {

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sends a request to cloudmade servers to geocode the specified address
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="start">The start index.</param>
        /// <param name="center">The center.</param>
        /// <param name="bound">The bound.</param>
        /// <param name="listener">The listener.</param>
        public void GetLocations(string address, int start, GeoLatLng center,
                GeoBounds bound, IGeocodingListener listener)
        {
            GetLocations(-1, address, start, center, bound, listener);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sends a request to cloudemade servers to geocode the specified address
        /// </summary>
        /// <param name="mapType">Type of the map.</param>
        /// <param name="address">The address.</param>
        /// <param name="start">The start index.</param>
        /// <param name="center">The center.</param>
        /// <param name="bound">The bound.</param>
        /// <param name="listener">The listener.</param>
        public void GetLocations(int mapType, string address, int start,
                GeoLatLng center, GeoBounds bound, IGeocodingListener listener)
        {
            _listener = listener;
            _searchAddress = address;
            try
            {
                address = Html2Text.Encodeutf8(_encoding.GetBytes(address));
            }
            catch (Exception) { }
            _queryKey = CloudMadeMapService.GetCloudMadeKey();
            MapPoint mapPoint = (MapPoint)_addressCache[_searchAddress];
            if (mapPoint == null)
            {

                string searchBase = ReplaceMetaString(SearchBase);
                ArrayList argList = new ArrayList();
                argList.Add(new Arg("query", address));
                SearchOptions routeOptions = DigitalMapService.GetSearchOptions();
                argList.Add(new Arg("results ", routeOptions.NumberOfSearchResult.ToString()));
                argList.Add(new Arg("bbox", bound.GetMinY() + "," + bound.GetMinX() + "," + bound.GetMaxY() + "," + bound.GetMaxX()));
                argList.Add(new Arg("return_location", "true"));
                argList.Add(new Arg("skip ", start.ToString()));
                Arg[] args = new Arg[argList.Count + 1];
                argList.CopyTo(args);
                args[argList.Count] = null;
                Request.Get(searchBase, args, null, _addressQuery, this);


            }
            else
            {
                MapPoint[] mapPoints = new MapPoint[1];
                mapPoints[0] = mapPoint;
                listener.Done(mapPoint.Name, mapPoints);
            }
        }



        private readonly Hashtable _addressCache = new Hashtable();
        private string _searchAddress;
        private IGeocodingListener _listener;
        private readonly LocalAddressQuery _addressQuery = new LocalAddressQuery();
        private const string SearchBase = "http://geocoding.cloudmade.com/{CLOUDMADE_KEY}/geocoding/v2/find.js";
        private string _queryKey = "8ee2a50541944fb9bcedded5165f09d9";
        private readonly UTF8Encoding _encoding = new UTF8Encoding();

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
        private class LocalAddressQuery : IRequestListener
        {

            public void ReadProgress(Object context, int bytes, int total)
            {
                if (context is CLocalSearch)
                {
                    CLocalSearch geoCoder = (CLocalSearch)context;
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
                if (context is CLocalSearch)
                {
                    CLocalSearch geoCoder = (CLocalSearch)context;
                    SearchResponse(geoCoder, response);
                }
            }

            private static void SearchResponse(CLocalSearch geoCoder, Response response)
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
                            mapPoints[i].Name = result.GetAsString(prefix + "features[" + i + "].properties.name");
                            double[] latLngArray = result.GetAsDoubleArray(prefix + "features[" + i + "].centroid.coordinates");
                            string address = result.GetAsString(prefix + "features[" + i + "].location.road");
                            address += "," + result.GetAsString(prefix + "features[" + i + "].location.city");
                            address += "," + result.GetAsString(prefix + "features[" + i + "].location.county");

                            mapPoints[i].ObjectNote = address.Replace("''''", ",");
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
