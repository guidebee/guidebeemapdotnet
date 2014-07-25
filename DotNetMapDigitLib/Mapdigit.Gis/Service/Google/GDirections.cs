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
    /// This class is used to obtain driving directions results.
    /// </summary>
    internal sealed class GDirections : IDirectionQuery
    {

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Set google china or not.
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
        /// Initializes a new instance of the <see cref="GDirections"/> class.
        /// </summary>
        public GDirections()
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
        /// <param name="query">the directions query string.</param>
        /// <param name="listener">the routing listener.</param>
        public void GetDirection(string query, IRoutingListener listener)
        {

            _listener = listener;
            _routeQuery = query;
            try
            {
                query = Html2Text.Encodeutf8(_encoding.GetBytes(query));
            }
            catch (Exception) { }
            SetGoogleKey(GoogleMapService.GetGoogleKey());
            string input = GoogleMapService.GetMapServiceUrl(GoogleMapService._direction);
            if (input != null)
            {
                string queryUrl = ReplaceMetaString(query, input);
                Request.Get(queryUrl, null, null, _directionQuery, this);
            }
            else
            {

                ArrayList argList = new ArrayList();
                argList.Add(new Arg("q", query));
                argList.Add(new Arg("output", "js"));
                argList.Add(new Arg("oe", "utf8"));
                argList.Add(new Arg("key", _queryKey));
                SearchOptions routeOptions = DigitalMapService.GetSearchOptions();
                string dirOption = "";
                if (routeOptions.AvoidHighway)
                {
                    dirOption = "h";
                }
                if (routeOptions.AvoidTollway)
                {
                    dirOption += "t";
                }
                switch (routeOptions.RoutingType)
                {
                    case SearchOptions.RouteTypeCommuting:
                        break;
                    case SearchOptions.RouteTypeDriving:
                        break;
                    case SearchOptions.RouteTypeWalking:
                        dirOption = "w";
                        break;

                }
                if (dirOption.Length != 0)
                {
                    argList.Add(new Arg("dirflg", dirOption));
                }
                if (!_needSecondQuery)
                {
                    argList.Add(new Arg("hl", "en-US"));
                }
                else
                {
                    argList.Add(new Arg("hl", routeOptions.LanguageId));
                }
                Arg[] args = new Arg[argList.Count + 1];
                argList.CopyTo(args);
                args[argList.Count] = null;
                if (!_isChina)
                {

                    Request.Get(SearchBase, args, null, _directionQuery, this);
                }
                else
                {

                    Request.Get(SearchBaseChina, args, null, _directionQuery, this);
                }
            }
        }

        private string ReplaceMetaString(string query, string input)
        {

            string[] pattern = new[]{
            "{QUERY}",
            "{GOOGLE_KEY}",
            "{OPTION}",
            " ",};

            string option = "";
            SearchOptions routeOptions = DigitalMapService.GetSearchOptions();
            string dirOption = "";
            if (routeOptions.AvoidHighway)
            {
                dirOption = "h";
            }
            if (routeOptions.AvoidTollway)
            {
                dirOption += "t";
            }
            switch (routeOptions.RoutingType)
            {
                case SearchOptions.RouteTypeCommuting:
                    break;
                case SearchOptions.RouteTypeDriving:
                    break;
                case SearchOptions.RouteTypeWalking:
                    dirOption = "w";
                    break;

            }
            if (dirOption.Length != 0)
            {
                option = "&dirflg=" + dirOption;
            }

            string[] replace = new[]{
            query,
            _queryKey,
            option,
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
        /// This method issues a new directions query. The query parameter is
        /// a string containing any valid directions query,
        /// e.g. "from: Seattle to: San Francisco" or
        /// "from: Toronto to: Ottawa to: New York".
        /// </summary>
        /// <param name="mapType">Type of the map.</param>
        /// <param name="query">the directions query string</param>
        /// <param name="listener">the routing listener.</param>
        public void GetDirection(int mapType, string query, IRoutingListener listener)
        {
            _isChina = mapType == MapType.MicrosoftChina ||
                       mapType == MapType.GoogleChina || mapType == MapType.MapabcChina;
            SetChina(_isChina);
            GetDirection(query, listener);
        }

        private const string SearchBase = "http://maps.google.com/maps/nav";
        //private static  string SearchBaseChina =  "http://ditu.google.cn/maps/nav";
        private const string SearchBaseChina = "http://maps.google.com/maps/nav";
        private string _queryKey = "ABQIAAAAi44TY0V29QjeejKd2l3ipRTRERdeAiwZ9EeJWta3L_JZVS0bOBQlextEji5FPvXs8mXtMbELsAFL0w";
        readonly DirectionQuery _directionQuery;
        MapDirection _mapDirection;
        MapDirection _mapDirectionEnglish;
        IRoutingListener _listener;
        string _routeQuery;
        bool _isChina;
        private bool _needSecondQuery;
        private bool _returnedResult;
        private readonly UTF8Encoding _encoding = new UTF8Encoding();

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
        /// <param name="waypoints">Type of the map.</param>
        /// <param name="listener">the routing listener.</param>
        public void GetDirection(GeoLatLng[] waypoints, IRoutingListener listener)
        {
            if (waypoints != null && waypoints.Length > 1)
            {
                string queryString = "from:" + "@"
                        + waypoints[0].Latitude
                        + "," + waypoints[0].Longitude;
                for (int i = 1; i < waypoints.Length; i++)
                {

                    queryString += " to:" + "@"
                            + waypoints[i].Latitude
                            + "," + waypoints[i].Longitude;
                }
                GetDirection(queryString, listener);
            }

        }

        class DirectionQuery : IRequestListener
        {
            readonly Html2Text _html2Text = new Html2Text();



            private void SearchResponse(GDirections gDirection, Response response)
            {

                Exception ex = response.GetException();
                if (ex != null || response.GetCode() != HttpConnection.HttpOk)
                {
                    if (ex is OutOfMemoryException)
                    {
                        Log.P("Dont have enough memory", Log.Error);
                        if (gDirection._listener != null)
                        {
                            gDirection._listener.Done(null, null);
                        }
                    }
                    else
                    {
                        Log.P("Error connecting to search service", Log.Error);
                        if (gDirection._listener != null)
                        {
                            gDirection._listener.Done(gDirection._routeQuery, null);
                        }
                    }

                    return;
                }
                try
                {
                    Result result = response.GetResult();
                    MapDirection currentMapDirection;
                    if (!gDirection._needSecondQuery)
                    {
                        gDirection._mapDirectionEnglish = new MapDirection();
                        currentMapDirection = gDirection._mapDirectionEnglish;
                    }
                    else
                    {
                        gDirection._mapDirection = new MapDirection();
                        currentMapDirection = gDirection._mapDirection;

                    }

                    currentMapDirection.Name = result.GetAsString("name");
                    currentMapDirection.Status = result.GetAsInteger("Status.code");
                    currentMapDirection.Duration = result.GetAsInteger("Directions.Duration.seconds");
                    currentMapDirection.Distance = result.GetAsInteger("Directions.Distance.meters");
                    currentMapDirection.Summary = _html2Text.Convert(result.GetAsString("Directions.summaryHtml"));
                    if (!gDirection._needSecondQuery)
                    {
                        string points = result.GetAsString("Directions.Polyline.points");
                        string levels = result.GetAsString("Directions.Polyline.levels");
                        int zoomFactor = result.GetAsInteger("Directions.Polyline.zoomFactor");
                        int numLevels = result.GetAsInteger("Directions.Polyline.numLevels");
                        currentMapDirection.Polyline = GeoPolyline.FromEncoded(0x00FF00, 4, 1, points,
                                zoomFactor, levels, numLevels);
                    }
                    int numOfGeocodes = result.GetSizeOfArray("Placemark");
                    if (numOfGeocodes > 0)
                    {
                        currentMapDirection.GeoCodes = new MapPoint[numOfGeocodes];
                        for (int i = 0; i < numOfGeocodes; i++)
                        {
                            currentMapDirection.GeoCodes[i] = new MapPoint();
                            currentMapDirection.GeoCodes[i].Name = result.GetAsString("Placemark[" + i + "].address");
                            string location = result.GetAsString("Placemark[" + i + "].Point.coordinates");
                            GeoLatLng latLng = MapLayer.FromStringToLatLng(location);
                            currentMapDirection.GeoCodes[i].Point = (latLng);

                        }
                    }

                    int numOfRoutes = result.GetSizeOfArray("Directions.Routes");
                    if (numOfRoutes > 0)
                    {
                        currentMapDirection.Routes = new MapRoute[numOfRoutes];
                        for (int i = 0; i < numOfRoutes; i++)
                        {
                            string routeString = "Directions.Routes[" + i + "]";
                            currentMapDirection.Routes[i] = MapDirection.NewRoute();
                            currentMapDirection.Routes[i].Summary = _html2Text.Convert(result.GetAsString(routeString + ".summaryHtml"));
                            currentMapDirection.Routes[i].Distance = result.GetAsInteger(routeString + ".Distance.meters");
                            currentMapDirection.Routes[i].Duration = result.GetAsInteger(routeString + ".Duration.seconds");
                            string lastLatLng = result.GetAsString(routeString + ".End.coordinates");
                            currentMapDirection.Routes[i].LastLatLng = MapLayer.FromStringToLatLng(lastLatLng);
                            int numOfSteps = result.GetSizeOfArray(routeString + ".Steps");
                            if (numOfSteps > 0)
                            {
                                currentMapDirection.Routes[i].Steps = new MapStep[numOfSteps];
                                for (int j = 0; j < numOfSteps; j++)
                                {
                                    string stepString = routeString + ".Steps[" + j + "]";
                                    currentMapDirection.Routes[i].Steps[j] = MapRoute.NewStep();
                                    currentMapDirection.Routes[i].Steps[j].Description = _html2Text.Convert(result.GetAsString(stepString + ".descriptionHtml"));
                                    currentMapDirection.Routes[i].Steps[j].DescriptionEnglish = currentMapDirection.Routes[i].Steps[j].Description;
                                    currentMapDirection.Routes[i].Steps[j].Distance = result.GetAsInteger(stepString + ".Distance.meters");
                                    currentMapDirection.Routes[i].Steps[j].Duration = result.GetAsInteger(stepString + ".Duration.seconds");
                                    currentMapDirection.Routes[i].Steps[j].FirstLocationIndex = result.GetAsInteger(stepString + ".polylineIndex");
                                    string firstLocation = result.GetAsString(stepString + ".Point.coordinates");
                                    currentMapDirection.Routes[i].Steps[j].FirstLatLng = MapLayer.FromStringToLatLng(firstLocation);
                                    if (!gDirection._needSecondQuery)
                                    {
                                        currentMapDirection.Routes[i].Steps[j].DirectionCommandElements = GDirectionCommandAnalyzer.Analyse(currentMapDirection.Routes[i].Steps[j].Description);
                                    }

                                }
                            }

                        }
                    }

                }
                catch (OutOfMemoryException)
                {
                    if (gDirection._listener != null)
                    {
                        gDirection._listener.Done(null, null);
                    }
                    return;

                }
                catch (Exception)
                {
                    if (gDirection._listener != null)
                    {
                        gDirection._listener.Done(gDirection._routeQuery, null);
                    }
                    return;

                }
                if (gDirection._listener != null)
                {
                    try
                    {
                        MapDirection currentMapDirection;
                        if (!gDirection._needSecondQuery)
                        {
                            currentMapDirection = gDirection._mapDirectionEnglish;
                        }
                        else
                        {
                            currentMapDirection = gDirection._mapDirection;
                        }

                        if (!gDirection._needSecondQuery)
                        {

                            if (currentMapDirection.GeoCodes.Length == currentMapDirection.Routes.Length + 1)
                            {
                                for (int i = 0; i < currentMapDirection.Routes.Length; i++)
                                {
                                    currentMapDirection.Routes[i].StartGeocode = currentMapDirection.GeoCodes[i];
                                    currentMapDirection.Routes[i].EndGeocode = currentMapDirection.GeoCodes[i + 1];
                                }
                            }

                            for (int i = 0; i < currentMapDirection.Routes.Length; i++)
                            {
                                MapRoute mapRoute = currentMapDirection.Routes[i];
                                for (int j = 0; j < mapRoute.Steps.Length - 1; j++)
                                {
                                    MapStep mapStep = mapRoute.Steps[j];

                                    mapStep.LastLocationIndex = mapRoute.Steps[j + 1].FirstLocationIndex;
                                    mapStep.LastLatLng = currentMapDirection.Polyline.GetVertex(mapRoute.Steps[j + 1].FirstLocationIndex);
                                }
                                mapRoute.Steps[mapRoute.Steps.Length - 1].LastLocationIndex =
                                        currentMapDirection.Polyline.GetVertexCount() - 1;
                                mapRoute.Steps[mapRoute.Steps.Length - 1].LastLatLng =
                                        currentMapDirection.Polyline.GetVertex(currentMapDirection.Polyline.GetVertexCount() - 1);
                            }
                            GeoPolyline polyline = currentMapDirection.Polyline;
                            if (polyline.GetVertexCount() > 1)
                            {
                                GeoLatLng latLngTemp = polyline.GetVertex(0);
                                currentMapDirection.Bounds = new GeoLatLngBounds(latLngTemp, latLngTemp);

                                for (int i = 0; i < currentMapDirection.Routes.Length; i++)
                                {
                                    MapRoute mapRoute = currentMapDirection.Routes[i];
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
                                            currentMapDirection.Bounds.Add(latLng.Longitude, latLng.Latitude);
                                        }

                                    }

                                }
                            }


                            currentMapDirection.CalculateMapStepDirections();
                            //check last step ,need to check if it has destination on the left/right
                            int numOfRoutes = currentMapDirection.Routes.Length;
                            int stepLength = currentMapDirection.Routes[numOfRoutes - 1].Steps.Length;
                            MapStep lastMapStep = currentMapDirection.Routes[numOfRoutes - 1].Steps[stepLength - 1];
                            MapStep newStep = GDirectionCommandAnalyzer.AnalyseLastStep(lastMapStep);
                            if (newStep != null)
                            {
                                MapStep[] oldSteps = currentMapDirection.Routes[numOfRoutes - 1].Steps;
                                currentMapDirection.Routes[numOfRoutes - 1].Steps = new MapStep[stepLength + 1];
                                Array.Copy(oldSteps, 0, currentMapDirection.Routes[numOfRoutes - 1].Steps,
                                       0, stepLength);
                                currentMapDirection.Routes[numOfRoutes - 1].Steps[stepLength] = newStep;
                            }
                        }

                        //this is the actual return.
                        SearchOptions routeOptions = DigitalMapService.GetSearchOptions();
                        if (routeOptions.LanguageId.ToLower().StartsWith("en-us") && !gDirection._returnedResult)
                        {
                            gDirection._needSecondQuery = false;

                        }
                        else
                        {
                            gDirection._needSecondQuery = true;

                        }

                        if (!gDirection._needSecondQuery || gDirection._returnedResult)
                        {
                            if (gDirection._needSecondQuery && gDirection._mapDirection != null)
                            {
                                gDirection._mapDirection.Polyline = gDirection._mapDirectionEnglish.Polyline;
                                gDirection._mapDirection.Bounds = gDirection._mapDirectionEnglish.Bounds;

                                //copy the map step's direction for english one.
                                for (int i = 0; i < gDirection._mapDirection.Routes.Length; i++)
                                {
                                    MapRoute mapRoute = gDirection._mapDirection.Routes[i];
                                    MapRoute mapRouteEnglish = gDirection._mapDirectionEnglish.Routes[i];
                                    mapRoute.Bounds = mapRouteEnglish.Bounds;
                                    mapRoute.StartGeocode = mapRouteEnglish.StartGeocode;
                                    mapRoute.EndGeocode = mapRouteEnglish.EndGeocode;
                                    MapStep[] oldSteps = mapRoute.Steps;
                                    mapRoute.Steps = new MapStep[mapRouteEnglish.Steps.Length];
                                    Array.Copy(mapRouteEnglish.Steps, 0, mapRoute.Steps,
                                           0, mapRouteEnglish.Steps.Length);

                                    for (int j = 0; j < oldSteps.Length; j++)
                                    {
                                        MapStep mapStep = oldSteps[j];
                                        MapStep mapStepEnglish = mapRoute.Steps[j];
                                        mapStepEnglish.Description = mapStep.Description;
                                        
                                    }
                                    if(oldSteps.Length+1==mapRoute.Steps.Length)
                                    {
                                        mapRoute.Steps[mapRoute.Steps.Length - 1].DescriptionEnglish
                                            = mapRoute.Steps[mapRoute.Steps.Length - 1].Description;
                                        mapRoute.Steps[mapRoute.Steps.Length - 1].Description = "";
                                    }

                                }
                            }
                            if (!gDirection._needSecondQuery)
                            {
                                gDirection._mapDirection = gDirection._mapDirectionEnglish;
                            }
                            gDirection._listener.Done(gDirection._routeQuery, gDirection._mapDirection);
                            gDirection._needSecondQuery = false;
                            gDirection._returnedResult = false;
                        }
                        else
                        {
                            gDirection.GetDirection(gDirection._routeQuery, gDirection._listener);
                        }
                        gDirection._returnedResult = true;
                    }
                    catch (OutOfMemoryException)
                    {
                        gDirection._listener.Done(null, null);
                    }
                }

            }

            public void ReadProgress(object context, int bytes, int total)
            {
                GDirections gDirection = (GDirections)context;
                if (gDirection._listener != null)
                {
                    gDirection._listener.ReadProgress(bytes, total);
                }
            }

            public void WriteProgress(object context, int bytes, int total)
            {
            }

            public void Done(object context, Response response)
            {

                GDirections gDirection = (GDirections)context;
                SearchResponse(gDirection, response);
            }
        }
    }

}
