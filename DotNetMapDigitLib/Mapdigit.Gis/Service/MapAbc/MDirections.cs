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

//--------------------------------- PACKAGE -----------------------------------
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
    /// This class is used to obtain driving directions results.
    /// </summary>
    internal sealed class MDirections : IDirectionQuery
    {

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 30DEC2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// default constructor.
        /// </summary>
        public MDirections()
        {
            _directionQuery = new DirectionQuery();

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This method issues a new directions query. The query parameter is
        /// a string containing any valid directions query,
        /// e.g. "from: Seattle to: San Francisco" or
        /// "from: Toronto to: Ottawa to: New York".
        /// </summary>
        /// <param name="waypoints">the directions query string.</param>
        /// <param name="listener">the routing listener.</param>
        public void GetDirection(GeoLatLng[] waypoints, IRoutingListener listener)
        {
            string queryString = "";
            if (waypoints != null && waypoints.Length > 1)
            {

                for (int i = 0; i < waypoints.Length - 1; i++)
                {

                    queryString +=
                            +waypoints[i].Longitude
                            + "," + waypoints[i].Latitude + ",";
                }

                queryString +=
                        +waypoints[waypoints.Length - 1].Longitude
                        + "," + waypoints[waypoints.Length - 1].Latitude;
                GetDirection(queryString, listener);
            }

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This method issues a new directions query. The query parameter is
        /// a string containing any valid directions query,
        /// e.g. "from: Seattle to: San Francisco" or
        /// "from: Toronto to: Ottawa to: New York".
        /// </summary>
        /// <param name="query">the directions query string.</param>
        /// <param name="listener">the routing listener.</param>
        public void GetDirection(string query, IRoutingListener listener)
        {

            _listener = listener;
            _routeQuery = query;
            _queryKey = MapAbcMapService.GetMapAbcKey();
            string[] lists = query.Split(new[] { ',' });
            {
                ArrayList argList = new ArrayList();
                argList.Add(new Arg("highLight", "false"));
                argList.Add(new Arg("enc", "utf-8"));
                argList.Add(new Arg("ver", MapAbcMapService._mapabcServiceVer));
                argList.Add(new Arg("config", "R"));
                if (MapAbcMapService._usingJson)
                {
                    argList.Add(new Arg("resType", "JSON"));
                }
                else
                {
                    argList.Add(new Arg("resType", "XML"));
                }
                for (int i = 0; i < lists.Length / 2; i++)
                {
                    argList.Add(new Arg("x" + (i + 1), lists[i * 2]));
                    argList.Add(new Arg("y" + (i + 1), lists[i * 2 + 1]));
                }
                argList.Add(new Arg("routeType", "0"));
                argList.Add(new Arg("per", "150"));
                argList.Add(new Arg("xs", ""));
                argList.Add(new Arg("ys", ""));
                argList.Add(new Arg("a_k", _queryKey));
                Arg[] args = new Arg[argList.Count + 1];
                argList.CopyTo(args);
                args[argList.Count] = null;
                Request.Get(SearchBaseChina, args, null, _directionQuery, this);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This method issues a new directions query. The query parameter is
        /// a string containing any valid directions query,
        /// e.g. "from: Seattle to: San Francisco" or
        /// "from: Toronto to: Ottawa to: New York".
        /// </summary>
        /// <param name="mapType">map type.</param>
        /// <param name="query">the directions query string.</param>
        /// <param name="listener">the routing listener.</param>
        public void GetDirection(int mapType, string query, IRoutingListener listener)
        {

            GetDirection(query, listener);
        }


        private const string SearchBaseChina = "http://search1.mapabc.com/sisserver";
        private string _queryKey = "b0a7db0b3a30f944a21c3682064dc70ef5b738b062f6479a5eca39725798b1ee300bd8d5de3a4ae3";
        private readonly DirectionQuery _directionQuery;
        private MapDirection _mapDirection ;
        private IRoutingListener _listener;
        private string _routeQuery;


        class DirectionQuery : IRequestListener
        {


            private static void SearchResponse(MDirections mDirection, Response response)
            {

                Exception ex = response.GetException();
                ArrayList level16CoordsVector = new ArrayList();
                if (ex != null || response.GetCode() != HttpConnection.HttpOk)
                {
                    if (ex is OutOfMemoryException)
                    {
                        Log.P("Dont have enough memory", Log.Error);
                        if (mDirection._listener != null)
                        {
                            mDirection._listener.Done(null, null);
                        }
                    }
                    else
                    {
                        Log.P("Error connecting to search service", Log.Error);
                        if (mDirection._listener != null)
                        {
                            mDirection._listener.Done(mDirection._routeQuery, null);
                        }
                    }

                    return;
                }
                try
                {
                    Result result = response.GetResult();
                    mDirection._mapDirection.Name = "";
                    mDirection._mapDirection.Status = 200;
                    string level16Coords = result.GetAsString("coors");
                    string[] level16CoordsList = level16Coords.Split(new[] { ',' });

                    ArrayList level8CoordsVector = new ArrayList();
                    int currentPointIndex = 0;
                    for (int i = 0; i < level16CoordsList.Length / 2; i++)
                    {
                        double lng = Double.Parse(level16CoordsList[2 * i]);
                        double lat = Double.Parse(level16CoordsList[2 * i + 1]);
                        GeoLatLng latLng = new GeoLatLng(lat, lng);
                        level16CoordsVector.Add(latLng);
                    }
                    int numOfSteps = result.GetSizeOfArray("segmengList");
                    mDirection._mapDirection.Routes = new MapRoute[1];
                    mDirection._mapDirection.Routes[0] = MapDirection.NewRoute();
                    mDirection._mapDirection.Routes[0].Steps = new MapStep[numOfSteps];
                    for (int i = 0; i < numOfSteps; i++)
                    {
                        string stepString = "segmengList[" + i + "]";
                        mDirection._mapDirection.Routes[0].Steps[i] = MapRoute.NewStep();
                        mDirection._mapDirection.Routes[0].Steps[i].Description = result.GetAsString(stepString + ".textInfo");
                        mDirection._mapDirection.Routes[0].Steps[i].CurrentRoadName = result.GetAsString(stepString + ".roadName");
                        mDirection._mapDirection.Routes[0].Steps[i].Distance = result.GetAsDouble(stepString + ".roadLength");
                        string duration = result.GetAsString(stepString + ".driveTime");
                        int minIndex = duration.IndexOf("分钟");//feizhong
                        if (minIndex > 0)
                        {
                            try
                            {
                                double durationSeconds = Double.Parse(duration.Substring(0, minIndex));
                                mDirection._mapDirection.Routes[0].Steps[i].Duration = (int)(durationSeconds * 60);
                            }
                            catch (Exception )
                            {
                            }
                        }
                        mDirection._mapDirection.Distance += mDirection._mapDirection.Routes[0].Steps[i].Distance;
                        mDirection._mapDirection.Duration += mDirection._mapDirection.Routes[0].Steps[i].Duration;
                        int offset = 0;
                        if (i != 0) offset = 1;
                        string level8Coords = result.GetAsString(stepString + ".coor");
                        string[] level8CoordsList = level8Coords.Split(new[] { ',' });
                        mDirection._mapDirection.Routes[0].Steps[i].FirstLocationIndex = currentPointIndex;
                        currentPointIndex += (level8CoordsList.Length / 2 - 1) - offset;
                        mDirection._mapDirection.Routes[0].Steps[i].LastLocationIndex = currentPointIndex;
                        for (int j = offset; j < level8CoordsList.Length / 2; j++)
                        {
                            double lng = Double.Parse(level8CoordsList[2 * j]);
                            double lat = Double.Parse(level8CoordsList[2 * j + 1]);
                            GeoLatLng latLng = new GeoLatLng(lat, lng);
                            level8CoordsVector.Add(latLng);
                        }
                        mDirection._mapDirection.Routes[0].Steps[i].FirstLatLng
                                = (GeoLatLng)level8CoordsVector
                                [(mDirection._mapDirection.Routes[0].Steps[i].FirstLocationIndex)];
                        mDirection._mapDirection.Routes[0].Steps[i].LastLatLng
                                = (GeoLatLng)level8CoordsVector
                                [(mDirection._mapDirection.Routes[0].Steps[i].LastLocationIndex)];
                        string actionCommand = result.GetAsString(stepString + ".action");
                        mDirection._mapDirection.Routes[0].Steps[i].DirectionCommandElements
                                = MDirectionCommandAnalyzer.Analyse(actionCommand, mDirection._mapDirection.Routes[0].Steps[i].CurrentRoadName);

                    }
                    //now need to create the map direction Polyline.
                    GeoLatLng[] latLngs = new GeoLatLng[level8CoordsVector.Count];
                    level8CoordsVector.CopyTo(latLngs);
                    mDirection._mapDirection.Polyline = new GeoPolyline(latLngs, 0x00FF00, 1, 1);
                    mDirection._mapDirection.Polyline.NumLevels = 2;
                    //fill the gecode for map direciton.
                    mDirection._mapDirection.GeoCodes = new MapPoint[2];
                    mDirection._mapDirection.GeoCodes[0] = new MapPoint();
                    mDirection._mapDirection.GeoCodes[0].Name = "Start";
                    mDirection._mapDirection.GeoCodes[0].Point = (latLngs[0]);
                    mDirection._mapDirection.GeoCodes[1] = new MapPoint();
                    mDirection._mapDirection.GeoCodes[1].Name = "End";
                    mDirection._mapDirection.GeoCodes[1].Point = (latLngs[latLngs.Length - 1]);
                    mDirection._mapDirection.Routes[0].StartGeocode = mDirection._mapDirection.GeoCodes[0];
                    mDirection._mapDirection.Routes[0].EndGeocode = mDirection._mapDirection.GeoCodes[1];
                    mDirection._mapDirection.Summary = "全长" + (((int)(mDirection._mapDirection.Distance / 1000) * 100) / 100) + "公里";
                    mDirection._mapDirection.Summary += " 预计驾驭时间" + (int)(mDirection._mapDirection.Duration / 60) + "分钟";
                    mDirection._mapDirection.Routes[0].Summary = mDirection._mapDirection.Summary;
                    mDirection._mapDirection.Routes[0].Distance = mDirection._mapDirection.Distance;
                    mDirection._mapDirection.Routes[0].Duration = mDirection._mapDirection.Duration;

                }
                catch (OutOfMemoryException )
                {
                    if (mDirection._listener != null)
                    {
                        mDirection._listener.Done(null, null);
                    }
                    return;

                }
                catch (Exception )
                {
                    if (mDirection._listener != null)
                    {
                        mDirection._listener.Done(mDirection._routeQuery, null);
                    }
                    return;

                }

                if (mDirection._listener != null)
                {
                    try
                    {
                        MapDirection mapDirection = mDirection._mapDirection;
                        GeoPolyline polyline = mapDirection.Polyline;
                        if (polyline.GetVertexCount() > 1)
                        {
                            GeoLatLng latLngTemp = polyline.GetVertex(0);
                            mapDirection.Bounds = new GeoLatLngBounds(latLngTemp, latLngTemp);

                            for (int i = 0; i < mapDirection.Routes.Length; i++)
                            {
                                MapRoute mapRoute = mapDirection.Routes[i];
                                latLngTemp = polyline.GetVertex(mapRoute.Steps[0].FirstLocationIndex);
                                mapRoute.Bounds = new GeoLatLngBounds(latLngTemp, latLngTemp);
                                for (int j = 0; j < mapRoute.Steps.Length; j++)
                                {
                                    latLngTemp = polyline.GetVertex(mapRoute.Steps[j].FirstLocationIndex);
                                    MapStep mapStep = mapRoute.Steps[j];
                                    mapStep.Bounds = new GeoLatLngBounds(latLngTemp, latLngTemp);
                                    for (int k = mapStep.FirstLocationIndex; k <= mapStep.LastLocationIndex; k++)
                                    {
                                        GeoLatLng latLng = polyline.GetVertex(k);
                                        mapStep.Bounds.Add(latLng.Longitude, latLng.Latitude);
                                        mapRoute.Bounds.Add(latLng.Longitude, latLng.Latitude);
                                        mapDirection.Bounds.Add(latLng.Longitude, latLng.Latitude);
                                        polyline.SetLevel(k, 2);
                                        for (int p = 0; p < level16CoordsVector.Count; p++)
                                        {
                                            GeoLatLng lngLat16 = (GeoLatLng)level16CoordsVector[p];
                                            if (((latLng.X - lngLat16.X) * (latLng.X - lngLat16.X) +
                                                (latLng.Y - lngLat16.Y) * (latLng.Y - lngLat16.Y)) < 0.0001)
                                            {
                                                polyline.SetLevel(k, 16);
                                                break;
                                            }
                                        }
                                    }

                                }

                            }
                        }
                        mapDirection.CalculateMapStepDirections();

                        mDirection._listener.Done(mDirection._routeQuery, mapDirection);
                    }
                    catch (OutOfMemoryException )
                    {
                        mDirection._listener.Done(null, null);
                    }
                }


            }

            public void ReadProgress(Object context, int bytes, int total)
            {
                MDirections mDirection = (MDirections)context;
                if (mDirection._listener != null)
                {
                    mDirection._listener.ReadProgress(bytes, total);
                }
            }

            public void WriteProgress(Object context, int bytes, int total)
            {
            }

            public void Done(Object context, Response response)
            {
                MDirections mDirection = (MDirections)context;
                mDirection._mapDirection = new MapDirection();
                SearchResponse(mDirection, response);
            }
        }
    }

}
