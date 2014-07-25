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
using Mapdigit.Ajax.Json;
using Mapdigit.Gis.Geometry;
using Mapdigit.Network;
using Mapdigit.Util;

//--------------------------------- PACKAGE -----------------------------------
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
    /// This class is used to obtain driving directions results.
    /// </summary>
    internal sealed class CDirections : IDirectionQuery
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
        public CDirections()
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
            _queryKey = CloudMadeMapService.GetCloudMadeKey();
            string[] lists = query.Split(new[] { ',' });
            if (lists.Length > 3)
            {
                ArrayList argList = new ArrayList();
                string startPoint = lists[1] + "," + lists[0];
                string endPoint = lists[3] + "," + lists[2];
                if (lists.Length / 2 > 2)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 2; i < lists.Length / 2; i++)
                    {
                        sb.Append(lists[i * 2 + 1]);
                        sb.Append(",");
                        sb.Append(lists[i * 2]);
                        sb.Append(",");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    argList.Add(new Arg("transit_points", sb.ToString()));

                }
                SearchOptions routeOptions = DigitalMapService.GetSearchOptions();
                string routeType;
                switch (routeOptions.RoutingType)
                {
                    case SearchOptions.RouteTypeCommuting:
                        routeType = "bicycle";
                        break;

                    case SearchOptions.RouteTypeWalking:
                        routeType = "foot";
                        break;
                    default:
                        routeType = "car";
                        break;


                }
                argList.Add(new Arg("lang", routeOptions.LanguageId.Substring(0, 2)));
                switch (routeOptions.RouteUnit)
                {
                    case SearchOptions.RouteUnitKm:
                        argList.Add(new Arg("units", "km"));
                        break;
                    case SearchOptions.RouteUnitMile:
                        argList.Add(new Arg("units", "mile"));
                        break;
                }

                Arg[] args = new Arg[argList.Count + 1];
                argList.CopyTo(args);
                args[argList.Count] = null;
                string searchBase = ReplaceMetaString(startPoint, endPoint, routeType, "");
                Request.Get(searchBase, args, null, _directionQuery, this);
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
        private const string SearchBase = "http://routes.cloudmade.com/{CLOUDMADE_KEY}/api/0.3/{START_POINT},{END_POINT}/{ROUTE_TYPE}{ROUTE_TYPE_MODIFIER}.js";
        private string _queryKey = "8ee2a50541944fb9bcedded5165f09d9";
        private readonly DirectionQuery _directionQuery;
        private MapDirection _mapDirection;
        private IRoutingListener _listener;
        private string _routeQuery;

        private string ReplaceMetaString(string startPoint,
                string endPoint, string routeType, string routeTypeModifier)
        {

            string[] pattern = new[]{
            "{CLOUDMADE_KEY}",
            "{START_POINT}",
            "{END_POINT}",
            "{ROUTE_TYPE}",
            "{ROUTE_TYPE_MODIFIER}",
            " ",};



            string[] replace = new[]{
            _queryKey,
            startPoint,
            endPoint,
            routeType,
            routeTypeModifier,
            "+"
        };

            string url = Utils.Replace(pattern, replace, SearchBase);
            return url;
        }

        class DirectionQuery : IRequestListener
        {
            ////////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS ------------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ----------------------
            // 04JAN2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Convert string to Latitude,Longitude pair, the input string has this format
            /// [Latitude,longitude] for example  [-31.948275,115.857562]
            /// </summary>
            /// <param name="location">location string</param>
            /// <returns> the geographical coordinates.</returns>
            private static GeoLatLng FromStringToLatLng(string location)
            {
                location = location.Trim();
                location = location.Substring(1, location.Length - 2);
                int commaIndex = location.IndexOf(",");
                string latitude = location.Substring(0, commaIndex);
                string longitude = location.Substring(commaIndex + 1);
                double lat = Double.Parse(latitude);
                double lng = Double.Parse(longitude);
                return new GeoLatLng(lat, lng);
            }

            private static void FillMapStepInfo(MapStep mapStep, object[] instructions)
            {
                string[] lists = new String[instructions.Length];
                for (int i = 0; i < lists.Length; i++)
                {
                    lists[i] = instructions[i].ToString();
                }
                mapStep.Description = lists[0];
                mapStep.Distance = Double.Parse(lists[1]);
                mapStep.FirstLocationIndex = Int32.Parse(lists[2]);
                mapStep.Duration = Double.Parse(lists[3]);
                mapStep.Bearing = 0;
                string command = "C";
                if (lists.Length > 7)
                {
                    mapStep.Bearing = (int)Double.Parse(lists[8]);
                    command = lists[7];
                }
                mapStep.DirectionCommandElements = CDirectionCommandAnalyzer.Analyse(command, "");

            }

            private static void SearchResponse(CDirections cDirection, Response response)
            {

                Exception ex = response.GetException();
                if (ex != null || response.GetCode() != HttpConnection.HttpOk)
                {
                    if (ex is OutOfMemoryException)
                    {
                        Log.P("Dont have enough memory", Log.Error);
                        if (cDirection._listener != null)
                        {
                            cDirection._listener.Done(null, null);
                        }
                    }
                    else
                    {
                        Log.P("Error connecting to search service", Log.Error);
                        if (cDirection._listener != null)
                        {
                            cDirection._listener.Done(cDirection._routeQuery, null);
                        }
                    }

                    return;
                }
                try
                {
                    Result result = response.GetResult();
                    cDirection._mapDirection.Name = "";
                    int status = result.GetAsInteger("status");
                    if (status == 0)
                    {
                        cDirection._mapDirection.Status = 200;
                    }
                    else
                    {
                        cDirection._mapDirection.Status = 400;
                    }
                    cDirection._mapDirection.Distance = result.GetAsInteger("route_summary.total_distance");
                    cDirection._mapDirection.Duration = result.GetAsInteger("route_summary.total_time");
                    cDirection._mapDirection.Summary = "Total Distance:" + cDirection._mapDirection.Distance + "m" + " Estimated time:" + cDirection._mapDirection.Duration + "s";
                    int totalPoint = result.GetSizeOfArray("route_geometry");
                    if (totalPoint > 0)
                    {
                        ArrayList level8CoordsVector = new ArrayList();
                        string pointPath = "route_geometry";
                        JSONArray jsonArray = result.GetAsArray(pointPath);
                        for (int i = 0; i < totalPoint; i++)
                        {
                            string geometry = jsonArray.GetJSONArray(i).ToString();

                            GeoLatLng latLng = FromStringToLatLng(geometry);
                            level8CoordsVector.Add(latLng);
                        }
                        GeoLatLng[] latLngs = new GeoLatLng[level8CoordsVector.Count];
                        level8CoordsVector.CopyTo(latLngs);
                        cDirection._mapDirection.Polyline = new GeoPolyline(latLngs, 0x00FF00, 1, 1);
                        cDirection._mapDirection.Polyline.NumLevels = 2;
                        //fill the gecode for map direciton.
                        cDirection._mapDirection.GeoCodes = new MapPoint[2];
                        cDirection._mapDirection.GeoCodes[0] = new MapPoint();
                        cDirection._mapDirection.GeoCodes[0].Name = "Start";
                        cDirection._mapDirection.GeoCodes[0].Point = (latLngs[0]);
                        cDirection._mapDirection.GeoCodes[1] = new MapPoint();
                        cDirection._mapDirection.GeoCodes[1].Name = "End";
                        cDirection._mapDirection.GeoCodes[1].Point = (latLngs[latLngs.Length - 1]);

                    }


                    int numOfSteps = result.GetSizeOfArray("route_instructions");
                    cDirection._mapDirection.Routes = new MapRoute[1];
                    cDirection._mapDirection.Routes[0] = MapDirection.NewRoute();
                    cDirection._mapDirection.Routes[0].Steps = new MapStep[numOfSteps];
                    cDirection._mapDirection.Routes[0].StartGeocode = cDirection._mapDirection.GeoCodes[0];
                    cDirection._mapDirection.Routes[0].EndGeocode = cDirection._mapDirection.GeoCodes[1];
                    cDirection._mapDirection.Routes[0].Summary = cDirection._mapDirection.Summary;
                    cDirection._mapDirection.Routes[0].Distance = cDirection._mapDirection.Distance;
                    cDirection._mapDirection.Routes[0].Duration = cDirection._mapDirection.Duration;
                    JSONArray routeInstruction = result.GetAsArray("route_instructions");
                    for (int i = 0; i < numOfSteps; i++)
                    {
                        cDirection._mapDirection.Routes[0].Steps[i] = MapRoute.NewStep();
                        JSONArray jsonArray = routeInstruction.GetJSONArray(i);
                        Object[] objects = new Object[jsonArray.Length()];
                        for (int k = 0; k < objects.Length; k++)
                        {
                            objects[k] = jsonArray.Get(k);
                        }
                        FillMapStepInfo(cDirection._mapDirection.Routes[0].Steps[i],
                                objects);
                    }


                }
                catch (OutOfMemoryException )
                {
                    if (cDirection._listener != null)
                    {
                        cDirection._listener.Done(null, null);
                    }
                    return;

                }
                catch (Exception )
                {
                    if (cDirection._listener != null)
                    {
                        cDirection._listener.Done(cDirection._routeQuery, null);
                    }
                    return;

                }
                if (cDirection._listener != null)
                {
                    try
                    {
                        MapDirection mapDirection = cDirection._mapDirection;

                        for (int i = 0; i < mapDirection.Routes.Length; i++)
                        {
                            MapRoute mapRoute = mapDirection.Routes[i];
                            for (int j = 0; j < mapRoute.Steps.Length - 1; j++)
                            {
                                MapStep mapStep = mapRoute.Steps[j];
                                mapStep.FirstLatLng = mapDirection.Polyline.GetVertex(mapStep.FirstLocationIndex);
                                mapStep.LastLocationIndex = mapRoute.Steps[j + 1].FirstLocationIndex;
                                mapStep.LastLatLng = mapDirection.Polyline.GetVertex(mapRoute.Steps[j + 1].FirstLocationIndex);
                            }
                            mapRoute.Steps[mapRoute.Steps.Length - 1].LastLocationIndex =
                                    mapDirection.Polyline.GetVertexCount() - 1;
                            mapRoute.Steps[mapRoute.Steps.Length - 1].LastLatLng =
                                    mapDirection.Polyline.GetVertex(mapDirection.Polyline.GetVertexCount() - 1);
                        }

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
                                        polyline.SetLevel(k, 16);

                                    }

                                }

                            }
                        }
                        cDirection._listener.Done(cDirection._routeQuery, mapDirection);
                    }
                    catch (OutOfMemoryException)
                    {
                        cDirection._listener.Done(null, null);
                    }
                }

            }

            public void ReadProgress(Object context, int bytes, int total)
            {
                CDirections cDirection = (CDirections)context;
                if (cDirection._listener != null)
                {
                    cDirection._listener.ReadProgress(bytes, total);
                }
            }

            public void WriteProgress(Object context, int bytes, int total)
            {
            }

            public void Done(Object context, Response response)
            {
                CDirections cDirection = (CDirections)context;
                cDirection._mapDirection = new MapDirection();
                SearchResponse(cDirection, response);
            }
        }
    }

}
