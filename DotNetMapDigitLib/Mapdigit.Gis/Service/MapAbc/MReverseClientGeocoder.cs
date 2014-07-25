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
    internal sealed class MReverseClientGeocoder : IReverseGeocoder
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
        /// <param name="cityCode">city code.</param>
        /// <param name="address">The address.</param>
        /// <param name="listener">The listener.</param>
        public void GetLocations(int cityCode, string address, IReverseGeocodingListener listener)
        {
            _listener = listener;
            _searchAddress = address;
            bool isLngLat = true;

            try
            {
                string tmpAddress = address;
                string strLngLat = "[" + tmpAddress + ",0]";
                MapLayer.FromStringToLatLng(strLngLat);
                _searchAddress = tmpAddress;

            }
            catch (Exception )
            {
                isLngLat = false;
            }
            if (isLngLat)
            {
                _queryKey = MapAbcMapService.GetMapAbcKey();
                MapPoint mapPoint = (MapPoint)_addressCache[address];
                if (mapPoint == null)
                {

                    ArrayList argList = new ArrayList();
                    argList.Add(new Arg("highLight", "false"));
                    argList.Add(new Arg("enc", "utf-8"));
                    argList.Add(new Arg("ver", MapAbcMapService._mapabcServiceVer));
                    if (cityCode == -1 || cityCode < 10)
                    {
                        argList.Add(new Arg("cityCode", "total"));
                    }
                    else
                    {
                        argList.Add(new Arg("cityCode", cityCode.ToString()));
                    }
                    if (MapAbcMapService._usingJson)
                    {
                        argList.Add(new Arg("resType", "JSON"));
                    }
                    else
                    {
                        argList.Add(new Arg("resType", "XML"));
                    }

                    argList.Add(new Arg("config", "SPAS"));
                    string[] lngLat = _searchAddress.Split(new[] { ',' });
                    SearchOptions routeOptions = DigitalMapService.GetSearchOptions();
                    string xmlRequest = ReplaceMetaString(lngLat[0], lngLat[1], routeOptions.NumberOfSearchResult.ToString());
                    argList.Add(new Arg("spatialXml", xmlRequest));

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

        }

        private readonly Hashtable _addressCache = new Hashtable();
        private string _searchAddress ;
        private IReverseGeocodingListener _listener ;
        private readonly ReverseAddressQuery _addressQuery = new ReverseAddressQuery();
        private const string SearchBaseChina = "http://search1.mapabc.com/sisserver";
        private string _queryKey = "b0a7db0b3a30f944a21c3682064dc70ef5b738b062f6479a5eca39725798b1ee300bd8d5de3a4ae3";
        private const string SpatialXml = "%3C?xml%20version=%221.0%22%20encoding" +
                "=%22gb2312%22?%3E%0D%0A%3Cspatial_request%20method=" +
                "%22searchPoint%22%3E%3Cx%3E{LONGITUDE}%3C/x%3E%3Cy%3E{LATITUDE}%3C/y%3E%3Cxs/%3E%3Cys/%3E%3C" +
                "poiNumber%3E{NUMBER}%3C/poiNumber%3E%3Crange%3ENaN%3C/range%3E%3Cpattern%3E1%3C/pattern%3E" +
                "%3CroadLevel%3E0%3C/roadLevel%3E%3Cexkey/%3E%3C/spatial_request%3E%0D%0A";



        private static string ReplaceMetaString(string longitude, string latitude, string number)
        {

            string[] pattern = new[]{
            "{LONGITUDE}",
            "{LATITUDE}",
            "{NUMBER}",
        };


            string[] replace = new[]{
            longitude,
            latitude,
            number,

        };

            string url = Utils.Replace(pattern, replace, SpatialXml);
            return url;
        }

        private class ReverseAddressQuery : IRequestListener
        {

            public void ReadProgress(Object context, int bytes, int total)
            {
                if (context is MReverseClientGeocoder)
                {
                    MReverseClientGeocoder geoCoder = (MReverseClientGeocoder)context;
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
                if (context is MReverseClientGeocoder)
                {
                    MReverseClientGeocoder geoCoder = (MReverseClientGeocoder)context;
                    SearchResponse(geoCoder, response);
                }
            }

            private void SearchResponse(MReverseClientGeocoder geoCoder, Response response)
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
                    string prefix = "SpatialBean.";
                    if (!MapAbcMapService._usingJson)
                    {
                        prefix = "";
                    }
                    int poiListCount = result.GetSizeOfArray(prefix + "poiList");
                    int roadListCount = result.GetSizeOfArray(prefix + "roadList");
                    int resultCount = poiListCount + roadListCount;
                    if (resultCount > 0)
                    {

                        mapPoints = new MapPoint[resultCount];
                        //get poi list.
                        if (poiListCount > 0)
                        {
                            for (int i = 0; i < poiListCount; i++)
                            {
                                mapPoints[i] = new MapPoint();

                                mapPoints[i].Name = result.GetAsString(prefix + "poiList[" + i + "].Name");
                                string latitude = result.GetAsString(prefix + "poiList[" + i + "].y");
                                string longitude = result.GetAsString(prefix + "poiList[" + i + "].x");
                                string location = "[" + longitude + "," + latitude + ",0]";
                                GeoLatLng latLng = MapLayer.FromStringToLatLng(location);
                                string address = result.GetAsString(prefix + "poiList[" + i + "].address");
                                string tel = result.GetAsString(prefix + "poiList[" + i + "].tel");
                                mapPoints[i].ObjectNote = (address + " " + tel);
                                mapPoints[i].Point = (latLng);

                            }
                        }
                        //road list
                        if (roadListCount > 0)
                        {

                            string latitude = result.GetAsString(prefix + "District.y");
                            string longitude = result.GetAsString(prefix + "District.x");
                            string location = "[" + longitude + "," + latitude + ",0]";
                            GeoLatLng latLng = MapLayer.FromStringToLatLng(location);
                            for (int i = 0; i < roadListCount; i++)
                            {
                                mapPoints[i + poiListCount] = new MapPoint();
                                mapPoints[i + poiListCount].Name = result.GetAsString(prefix + "roadList[" + i + "].Name");
                                string address = result.GetAsString(prefix + "roadList[" + i + "].distance");
                                mapPoints[i + poiListCount].ObjectNote = (address + "m");
                                mapPoints[i + poiListCount].Point = (latLng);

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
