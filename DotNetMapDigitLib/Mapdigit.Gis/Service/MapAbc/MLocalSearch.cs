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
namespace Mapdigit.Gis.Service.MapAbc
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 27SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// This class is used to communicate directly with Mapabc servers to obtain
    /// geocodes for user specified addresses. In addition, a geocoder maintains
    /// its own cache of addresses, which allows repeated queries to be answered
    /// without a round trip to the server.
    /// </summary>
    internal sealed class MLocalSearch : ILocalSearch
    {

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sends a request to Mapabc servers to geocode the specified address
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
        /// Sends a request to mapabc servers to geocode the specified address
        /// </summary>
        /// <param name="cityCode">city code.</param>
        /// <param name="address">The address.</param>
        /// <param name="start">The start index.</param>
        /// <param name="center">The center.</param>
        /// <param name="bound">The bound.</param>
        /// <param name="listener">The listener.</param>
        public void GetLocations(int cityCode, string address, int start,
                GeoLatLng center, GeoBounds bound, IGeocodingListener listener)
        {
            _listener = listener;
            _searchAddress = address;
            try
            {
                address = Html2Text.Encodeutf8(_encoding.GetBytes(address));
            }
            catch (Exception) { }
            _queryKey = MapAbcMapService.GetMapAbcKey();
            MapPoint mapPoint = (MapPoint)_addressCache[address];
            if (mapPoint == null)
            {

                ArrayList argList = new ArrayList();
                argList.Add(new Arg("highLight", "false"));
                argList.Add(new Arg("enc", "utf-8"));
                argList.Add(new Arg("ver", MapAbcMapService._mapabcServiceVer));
                argList.Add(new Arg("cityCode", "total"));

                argList.Add(new Arg("config", "BELSBN"));
                argList.Add(new Arg("centerPoiXY", center.ToString()));
                argList.Add(new Arg("searchName", address));
                SearchOptions routeOptions = DigitalMapService.GetSearchOptions();
                argList.Add(new Arg("number ", routeOptions.NumberOfSearchResult.ToString()));
                argList.Add(new Arg("searchType", ""));
                int distance = (int)(1000 * GeoLatLng.Distance(center, new GeoLatLng(bound.GetCenterY(), bound.GetCenterX())));
                argList.Add(new Arg("range", distance.ToString()));
                int batchId = (start / routeOptions.NumberOfSearchResult) + 1;
                argList.Add(new Arg("batch", batchId.ToString()));
                if (MapAbcMapService._usingJson)
                {
                    argList.Add(new Arg("resType", "JSON"));
                }
                else
                {
                    argList.Add(new Arg("resType", "XML"));
                }
                argList.Add(new Arg("a_k", _queryKey));
                Arg[] args = new Arg[argList.Count + 1];
                argList.CopyTo(args);
                args[argList.Count] = null;
                Request.Get(SearchBaseChina, args, null, _addressQuery, this);


            }
            else
            {
                MapPoint[] mapPoints = new MapPoint[1];
                mapPoints[0] = mapPoint;
                listener.Done(mapPoint.Name, mapPoints);
            }
        }

        private readonly Hashtable _addressCache = new Hashtable();
        private string _searchAddress ;
        private IGeocodingListener _listener;
        private readonly LocalAddressQuery _addressQuery = new LocalAddressQuery();
        private const string SearchBaseChina = "http://search1.mapabc.com/sisserver";
        private string _queryKey = "b0a7db0b3a30f944a21c3682064dc70ef5b738b062f6479a5eca39725798b1ee300bd8d5de3a4ae3";
        private readonly UTF8Encoding _encoding = new UTF8Encoding();

        private class LocalAddressQuery : IRequestListener
        {

            public void ReadProgress(Object context, int bytes, int total)
            {
                if (context is MLocalSearch)
                {
                    MLocalSearch geoCoder = (MLocalSearch)context;
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
                if (context is MLocalSearch)
                {
                    MLocalSearch geoCoder = (MLocalSearch)context;
                    SearchResponse(geoCoder, response);
                }
            }

            private void SearchResponse(MLocalSearch geoCoder, Response response)
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
                    if (!MapAbcMapService._usingJson)
                    {
                        prefix = "";
                    }
                    int resultCount = result.GetSizeOfArray(prefix + "poilist");
                    if (resultCount >= 0)
                    {
                        if (resultCount == 0)
                        {
                            mapPoints = new MapPoint[1];
                            int i = 0;
                            mapPoints[i] = new MapPoint();
                            mapPoints[i].Name = result.GetAsString(prefix + "poilist.Name");
                            string latitude = result.GetAsString(prefix + "poilist.y");
                            string longitude = result.GetAsString(prefix + "poilist.x");
                            string location = "[" + longitude + "," + latitude + ",0]";
                            GeoLatLng latLng = MapLayer.FromStringToLatLng(location);
                            string address = result.GetAsString(prefix + "poilist.address");
                            string tel = result.GetAsString(prefix + "poilist.tel");
                            mapPoints[i].ObjectNote = (address + " " + tel);
                            mapPoints[i].Point = (latLng);


                        }
                        else
                        {
                            mapPoints = new MapPoint[resultCount];
                            for (int i = 0; i < resultCount; i++)
                            {
                                mapPoints[i] = new MapPoint();

                                mapPoints[i].Name = result.GetAsString(prefix + "poilist[" + i + "].Name");
                                string latitude = result.GetAsString(prefix + "poilist[" + i + "].y");
                                string longitude = result.GetAsString(prefix + "poilist[" + i + "].x");
                                string location = "[" + longitude + "," + latitude + ",0]";
                                GeoLatLng latLng = MapLayer.FromStringToLatLng(location);
                                string address = result.GetAsString(prefix + "poilist[" + i + "].address");
                                string tel = result.GetAsString(prefix + "poilist[" + i + "].tel");
                                mapPoints[i].ObjectNote = (address + " " + tel);
                                mapPoints[i].Point = (latLng);

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
                        geoCoder._addressCache[mapPoints[0].Name] = mapPoints[0];

                    }
                }
                catch (OutOfMemoryException )
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
