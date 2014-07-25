//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 15OCT2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System;
using System.Text;
using Mapdigit.Gis.Geometry;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Gis
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 15OCT2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <Summary>
    /// This class is used to store driving directions results
    /// </Summary>
    public sealed class MapDirection : MapObject
    {

        /// <summary>
        /// Waypoints along the direction.
        /// </summary>
        public MapPoint[] GeoCodes;

        /// <summary>
        /// total Distance in meters.
        /// </summary>
        public double Distance;

        /// <summary>
        /// total Duration in seconds.
        /// </summary>
        public double Duration;

        /// <summary>
        ///  the Polyline of this direction.
        /// </summary>
        public GeoPolyline Polyline;

        /// <summary>
        /// total Routes included in this direction.
        /// </summary>
        public MapRoute[] Routes;

        /// <summary>
        /// Summary information about this direction.
        /// </summary>
        public string Summary;

        /// <summary>
        /// Response status of this query.
        /// </summary>
        public int Status;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public MapDirection()
        {
            MapObjectType = TypeDirection;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapDirection"/> class.
        /// </summary>
        /// <param name="mapDirection">The map direction copied from.</param>
        public MapDirection(MapDirection mapDirection)
        {
            MapObjectType = TypeDirection;
            Distance = mapDirection.Distance;
            Duration = mapDirection.Duration;
            GeoCodes = new MapPoint[mapDirection.GeoCodes.Length];
            for (int i = 0; i < GeoCodes.Length; i++)
            {
                GeoCodes[i] = new MapPoint(mapDirection.GeoCodes[i]);
            }
            Name = mapDirection.Name;
            ObjectNote = mapDirection.ObjectNote;
            MapInfoId = mapDirection.MapInfoId;
            Status = mapDirection.Status;
            Summary = mapDirection.Summary;
            Highlighted = mapDirection.Highlighted;
            Polyline = new GeoPolyline(mapDirection.Polyline);
            Routes = new MapRoute[mapDirection.Routes.Length];
            for (int i = 0; i < Routes.Length; i++)
            {
                Routes[i] = new MapRoute();
                Routes[i].Distance = mapDirection.Routes[i].Distance;
                Routes[i].Duration = mapDirection.Routes[i].Duration;
                Routes[i].EndGeocode = new MapPoint(mapDirection.Routes[i].EndGeocode);
                Routes[i].StartGeocode = new MapPoint(mapDirection.Routes[i].StartGeocode);
                Routes[i].Summary = mapDirection.Routes[i].Summary;
                Routes[i].Steps = new MapStep[mapDirection.Routes[i].Steps.Length];
                for (int j = 0; j < Routes[i].Steps.Length; j++)
                {
                    Routes[i].Steps[j] = new MapStep();
                    Routes[i].Steps[j].Bearing = mapDirection.Routes[i].Steps[j].Bearing;
                    Routes[i].Steps[j].Bounds = new GeoLatLngBounds(mapDirection.Routes[i].Steps[j].Bounds);
                    Routes[i].Steps[j].CacheAccessTime = DateTime.Now;
                    Routes[i].Steps[j].Description = mapDirection.Routes[i].Steps[j].Description;
                    Routes[i].Steps[j].DescriptionEnglish = mapDirection.Routes[i].Steps[j].DescriptionEnglish;
                    Routes[i].Steps[j].CalculatedDirectionType = mapDirection.Routes[i].Steps[j].CalculatedDirectionType;
                    Routes[i].Steps[j].Distance = mapDirection.Routes[i].Steps[j].Distance;
                    Routes[i].Steps[j].FirstLatLng = new GeoLatLng(mapDirection.Routes[i].Steps[j].FirstLatLng);
                    Routes[i].Steps[j].FirstLocationIndex = mapDirection.Routes[i].Steps[j].FirstLocationIndex;
                    Routes[i].Steps[j].LastLatLng = new GeoLatLng(mapDirection.Routes[i].Steps[j].LastLatLng);
                    Routes[i].Steps[j].LastLocationIndex = mapDirection.Routes[i].Steps[j].LastLocationIndex;
                    Routes[i].Steps[j].MapInfoId = mapDirection.Routes[i].Steps[j].MapInfoId;
                    Routes[i].Steps[j].Name = mapDirection.Routes[i].Steps[j].Name;
                    Routes[i].Steps[j].DirectionCommandElements = new MapDirectionCommandElement[mapDirection.Routes[i].Steps[j].DirectionCommandElements.Length];
                    for (int k = 0; k < Routes[i].Steps[j].DirectionCommandElements.Length; k++)
                    {
                        if (mapDirection.Routes[i].Steps[j].DirectionCommandElements[k] != null)
                        {
                            Routes[i].Steps[j].DirectionCommandElements[k] = new MapDirectionCommandElement(mapDirection.Routes[i].Steps[j].DirectionCommandElements[k]);
                        }
                    }
                }

            }


        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Create a new MapRoute object.
        /// </summary>
        /// <returns>a new MapRoute object</returns>
        public static MapRoute NewRoute()
        {
            return new MapRoute();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// calculate map step direction based on geographical shape of the route.
        /// </summary>
        public void CalculateMapStepDirections()
        {
            for (int i = 0; i < Routes.Length; i++)
            {
                MapRoute mapRoute = Routes[i];
                GeoLatLng pt1, pt2, pt3;
                int nextIndex;
                int ptIndex;
                if (mapRoute.Steps.Length > 1)
                {
                    ptIndex = mapRoute.Steps[0].FirstLocationIndex;
                    nextIndex = 1;
                    pt1 = Polyline.GetVertex(ptIndex);
                    pt2 = Polyline.GetVertex(ptIndex + nextIndex);
                    while (pt1.X == pt2.X && pt1.Y == pt2.Y)
                    {
                        nextIndex++;
                        pt1 = Polyline.GetVertex(ptIndex + nextIndex);
                    }
                    double headAngle = GeoLatLng.AzimuthTo(pt1, pt2);
                    if (headAngle < 0)
                    {
                        headAngle += 360;
                    }
                    mapRoute.Steps[0].CalculatedDirectionType.Type =
                            GetMapHeadDirectionCommandTypeByBearing(headAngle);
                    mapRoute.Steps[0].Bearing = (int)(headAngle + 0.5);
                    if (string.IsNullOrEmpty(mapRoute.Steps[0].CurrentRoadName))
                    {
                        if (mapRoute.Steps[0].DirectionCommandElements
                                [MapDirectionCommandElement.FromRoadName] != null)
                        {
                            mapRoute.Steps[0].CurrentRoadName = mapRoute.Steps[0].DirectionCommandElements
                                [MapDirectionCommandElement.FromRoadName].Description;
                        }
                        else if (mapRoute.Steps[0].DirectionCommandElements
                                     [MapDirectionCommandElement.ToRoadName] != null)
                        {
                            mapRoute.Steps[0].CurrentRoadName = mapRoute.Steps[0].DirectionCommandElements
                                [MapDirectionCommandElement.ToRoadName].Description;
                        }
                    }
                    for (int j = 1; j < mapRoute.Steps.Length; j++)
                    {

                        nextIndex = 1;
                        ptIndex = mapRoute.Steps[j].FirstLocationIndex;
                        pt2 = Polyline.GetVertex(ptIndex);
                        pt1 = Polyline.GetVertex(ptIndex - nextIndex);
                        while (pt1.X == pt2.X && pt1.Y == pt2.Y)
                        {
                            nextIndex++;
                            pt1 = Polyline.GetVertex(ptIndex - nextIndex);
                        }
                        nextIndex = 1;
                        pt3 = Polyline.GetVertex(ptIndex + nextIndex);
                        while (pt3.X == pt2.X && pt3.Y == pt2.Y)
                        {
                            nextIndex++;
                            pt3 = Polyline.GetVertex(ptIndex + nextIndex);
                        }
                        double bearing = GeoLatLng.GetBearing(pt1, pt2, pt3);
                        mapRoute.Steps[j].Bearing = (int) (bearing + 0.5);
                        mapRoute.Steps[j].CalculatedDirectionType.Type =
                            GetMapDirectionCommandTypeByBearing(bearing);

                        if (string.IsNullOrEmpty(mapRoute.Steps[j].CurrentRoadName))
                        {
                            if (mapRoute.Steps[j].DirectionCommandElements
                                    [MapDirectionCommandElement.ToRoadName] != null)
                            {
                                mapRoute.Steps[j].CurrentRoadName = mapRoute.Steps[j]
                                    .DirectionCommandElements
                                    [MapDirectionCommandElement.ToRoadName].Description;
                            }
                            else if (mapRoute.Steps[j].DirectionCommandElements
                                         [MapDirectionCommandElement.FromRoadName] != null)
                            {
                                mapRoute.Steps[j].CurrentRoadName = mapRoute.Steps[j]
                                    .DirectionCommandElements
                                    [MapDirectionCommandElement.FromRoadName].Description;

                            }
                            else if (mapRoute.Steps[j - 1].DirectionCommandElements
                                         [MapDirectionCommandElement.ToRoadName] != null)
                            {
                                mapRoute.Steps[j].CurrentRoadName = mapRoute.Steps[j - 1]
                                    .DirectionCommandElements
                                    [MapDirectionCommandElement.ToRoadName].Description;
                            }
                            else
                            {
                                string routeName = mapRoute.Steps[j - 1].CurrentRoadName;
                                mapRoute.Steps[j].CurrentRoadName = routeName;
                            }
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(mapRoute.Steps[0].CurrentRoadName))
                    {
                        if (mapRoute.Steps.Length == 1)
                        {
                            mapRoute.Steps[0].CalculatedDirectionType.Type
                                = MapDirectionCommandType.CommandInvalid;
                            if (mapRoute.Steps[0].DirectionCommandElements
                                    [MapDirectionCommandElement.FromRoadName] != null)
                            {
                                mapRoute.Steps[0].CurrentRoadName
                                    = mapRoute.Steps[0].DirectionCommandElements
                                        [MapDirectionCommandElement.FromRoadName].Description;
                            }
                            else if (mapRoute.Steps[0].DirectionCommandElements
                                         [MapDirectionCommandElement.ToRoadName] != null)
                            {
                                mapRoute.Steps[0].CurrentRoadName
                                    = mapRoute.Steps[0].DirectionCommandElements
                                        [MapDirectionCommandElement.ToRoadName].Description;
                            }
                        }
                    }

                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the map step by the point index in the polyline.
        /// </summary>
        /// <param name="pointIndex"> point index in the polyline</param>
        /// <returns>the map step object contains the point</returns>
        public MapStep GetMapStepIndexByPointIndex(int pointIndex)
        {
            GeoPoint result = GetMapRouteStepIndexByPointIndex(pointIndex);
            if (result != null)
            {
                return Routes[(int)result.X].Steps[(int)result.Y];
            }
            return null;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the map route step by the point index in the polyline.
        /// </summary>
        /// <param name="pointIndex">point index in the polyline.</param>
        /// <returns>GeoPoint object, where x is the route index, y is the step index</returns>
        public GeoPoint GetMapRouteStepIndexByPointIndex(int pointIndex)
        {
            for (int i = 0; i < Routes.Length; i++)
            {
                MapRoute mapRoute = Routes[i];
                for (int j = 0; j < mapRoute.Steps.Length; j++)
                {
                    MapStep mapStep = mapRoute.Steps[j];
                    if (mapStep.FirstLocationIndex <= pointIndex &&
                            mapStep.LastLocationIndex > pointIndex)
                    {
                        GeoPoint geoPoint = new GeoPoint(i, j);
                        return geoPoint;
                    }
                }
            }
            return null;

        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// get total map step counts for this direction.
        /// </summary>
        /// <returns>total map step count</returns>
        public int GetMapStepCount()
        {
            int totalStep = 0;
            for (int i = 0; i < Routes.Length; i++)
            {
                totalStep += Routes[i].Steps.Length;
            }
            return totalStep;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// return the first map step.
        /// </summary>
        /// <returns>the first map Step;</returns>
        public MapStep FirstMapStep()
        {
            _currentRouteIndex = 0;
            _currentMapStepIndex = 0;
            return Routes[_currentRouteIndex].Steps[_currentMapStepIndex];
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// return the last map step.
        /// </summary>
        /// <returns>the last map step.</returns>
        public MapStep LastMapStep()
        {
            _currentRouteIndex = Routes.Length - 1;
            _currentMapStepIndex = Routes[_currentRouteIndex].Steps.Length - 1;
            return Routes[_currentRouteIndex].Steps[_currentMapStepIndex];

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// return next map step.
        /// </summary>
        /// <returns>next map step or null if reaches the end of the direction.</returns>
        public MapStep NextMapStep()
        {
            if (_currentMapStepIndex < Routes[_currentRouteIndex].Steps.Length - 1)
            {
                _currentMapStepIndex++;
            }
            else if (_currentRouteIndex < Routes.Length - 1)
            {
                _currentRouteIndex++;
                _currentMapStepIndex = 0;
            }
            else
            {
                return null;
            }
            return Routes[_currentRouteIndex].Steps[_currentMapStepIndex];
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// return prev map step.
        /// </summary>
        /// <returns>prve map step or null if reaches the start of the direction.</returns>
        public MapStep PrevMapStep()
        {
            if (_currentMapStepIndex > 0)
            {
                _currentMapStepIndex--;
            }
            else if (_currentRouteIndex > 0)
            {
                _currentRouteIndex--;
                _currentMapStepIndex = Routes[_currentRouteIndex].Steps.Length - 1;
            }
            else
            {
                return null;
            }
            return Routes[_currentRouteIndex].Steps[_currentMapStepIndex];
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// get map step at given index
        /// </summary>
        /// <param name="index">the index of the map step.</param>
        /// <returns>map step object or null if out of boundary</returns>
        public MapStep GetMapStepAt(int index)
        {
            if (index >= 0 && index < GetMapStepCount())
            {
                int mapStepIndex = 0;
                for (int i = 0; i < Routes.Length; i++)
                {
                    for (int j = 0; j < Routes[i].Steps.Length; j++)
                    {
                        mapStepIndex++;
                        if (mapStepIndex == index)
                        {
                            _currentMapStepIndex = j;
                            _currentRouteIndex = i;
                            return Routes[_currentRouteIndex].Steps[_currentMapStepIndex];
                        }
                    }
                }
            }
            return null;

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// jump to given map step on the direction.
        /// </summary>
        /// <param name="mapStep">The map step.</param>
        public void JumpToStep(MapStep mapStep)
        {
            for (int i = 0; i < Routes.Length; i++)
            {
                for (int j = 0; j < Routes[i].Steps.Length; j++)
                {

                    if (Routes[i].Steps[j] == mapStep)
                    {
                        _currentMapStepIndex = j;
                        _currentRouteIndex = i;
                        return;
                    }
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Routes.Length; i++)
            {
                MapRoute mapRoute = Routes[i];
                for (int j = 0; j < mapRoute.Steps.Length; j++)
                {
                    MapStep mapStep = mapRoute.Steps[j];
                    sb.Append(mapStep.ToString());

                }
            }
            return sb.ToString();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the remaining distance of this route.
        /// </summary>
        /// <param name="location"> current location</param>
        /// <returns>the distance (in meters) ,-1 invalid input</returns>
        public GeoPoint GetRemainingDistance(GeoLatLng location)
        {
            GeoPoint finalResult = new GeoPoint();
            GeoPoint result = Polyline.IndexOfClosestdistanceToPoly(location);
            int nearestPoint = (int)result.Y;
            GeoPoint result1 = GetMapRouteStepIndexByPointIndex(nearestPoint);
            int routeIndex = (int)result1.X;
            int stepIndex = (int)result1.Y;
            double remainingDistance = GetRemainingDistance(location, routeIndex, stepIndex);
            finalResult.X = remainingDistance;
            for (int i = stepIndex + 1; i < Routes[routeIndex].Steps.Length; i++)
            {
                remainingDistance += Routes[routeIndex].Steps[i].Distance;
            }
            for (int i = routeIndex + 1; i < Routes.Length; i++)
            {
                for (int j = 0; j < Routes[i].Steps.Length; j++)
                {
                    remainingDistance += Routes[i].Steps[j].Distance;
                }
            }
            finalResult.Y = remainingDistance;
            return finalResult;


        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the remaining distance along given step.
        /// </summary>
        /// <param name="location"> current location.</param>
        /// <param name="routeIndex">the index of the map route</param>
        /// <param name="stepIndex">Indexof the map step.</param>
        /// <returns>the distance (in meters)></returns>
        private double GetRemainingDistance(GeoLatLng location, int routeIndex,
                int stepIndex)
        {
            if (routeIndex > Routes.Length - 1 ||
                    stepIndex > Routes[routeIndex].Steps.Length - 1)
            {
                throw new ArgumentException("Index out of boundary");
            }
            double remainingDistance;
            MapStep mapStep = Routes[routeIndex].Steps[stepIndex];
            int startIndex = mapStep.FirstLocationIndex;
            int endIndex = mapStep.LastLocationIndex;
            GeoLatLng pt = Polyline.GetVertex(startIndex);
            double nearestDistance = pt.DistanceFrom(location);
            int nearestIndex = startIndex;
            for (int i = startIndex + 1; i < endIndex; i++)
            {
                pt = Polyline.GetVertex(i);
                double dis = pt.DistanceFrom(location);
                if (dis < nearestDistance)
                {
                    nearestIndex = i;
                    nearestDistance = dis;
                }
            }

            //if(result.X<5)
            {
                if (nearestIndex + 1 < endIndex)
                {
                    GeoLatLng pt1 = Polyline.GetVertex(nearestIndex);
                    GeoLatLng pt2 = Polyline.GetVertex(nearestIndex + 1);
                    double bearing = GeoLatLng.GetBearing(pt1, location, pt2);
                    if ((bearing >= 0 && bearing <= 5) ||
                            (bearing >= 355 && bearing <= 360))
                    {//the location is on the right side of pt1
                        nearestIndex += 1;
                    }
                    pt1 = Polyline.GetVertex(nearestIndex);
                    remainingDistance = GeoLatLng.Distance(location, pt1) * 1000;
                    for (int i = nearestIndex + 1; i < endIndex; i++)
                    {
                        pt2 = Polyline.GetVertex(i);
                        remainingDistance += GeoLatLng.Distance(pt1, pt2) * 1000;
                        pt1 = pt2;
                    }
                }
                else
                {
                    GeoLatLng pt1 = location;
                    GeoLatLng pt2 = Polyline.GetVertex(endIndex);
                    remainingDistance = GeoLatLng.Distance(pt1, pt2) * 1000;
                }
            }

            return remainingDistance;

        }

        private int _currentRouteIndex;
        private int _currentMapStepIndex;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get head command type based on bearing
        /// </summary>
        /// <param name="bearing">the bearing the routes</param>
        /// <returns>the head command type</returns>
        private static int GetMapHeadDirectionCommandTypeByBearing(double bearing)
        {
            int dirType = GetMapDirectionCommandTypeByBearing(bearing);
            int retType = MapDirectionCommandType.CommandInvalid;
            switch (dirType)
            {
                case MapDirectionCommandType.CommandNoTurn:
                    retType = MapDirectionCommandType.CommandHeadDirectionN;
                    break;
                case MapDirectionCommandType.CommandBearRight:
                    retType = MapDirectionCommandType.CommandHeadDirectionNe;
                    break;
                case MapDirectionCommandType.CommandTurnRight:
                    retType = MapDirectionCommandType.CommandHeadDirectionE;
                    break;
                case MapDirectionCommandType.CommandSharpRight:
                    retType = MapDirectionCommandType.CommandHeadDirectionSe;
                    break;
                case MapDirectionCommandType.CommandUTurn:
                    retType = MapDirectionCommandType.CommandHeadDirectionS;
                    break;
                case MapDirectionCommandType.CommandSharpLeft:
                    retType = MapDirectionCommandType.CommandHeadDirectionSw;
                    break;
                case MapDirectionCommandType.CommandTurnLeft:
                    retType = MapDirectionCommandType.CommandHeadDirectionW;
                    break;
                case MapDirectionCommandType.CommandBearLeft:
                    retType = MapDirectionCommandType.CommandHeadDirectionNw;
                    break;

            }
            return retType;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get direction type based on bearing of the route.
        /// </summary>
        /// <param name="bearing">bearing of the route step</param>
        /// <returns>the direction command type</returns>
        private static int GetMapDirectionCommandTypeByBearing(double bearing)
        {
            int type = MapDirectionCommandType.CommandInvalid;
            if ((bearing >= 350 && bearing < 360) || (bearing >= 0 && bearing < 10))
            {
                type = MapDirectionCommandType.CommandNoTurn;
            }
            else if (bearing >= 10 && bearing < 40)
            {
                type = MapDirectionCommandType.CommandBearRight;
            }
            else if (bearing >= 40 && bearing < 112.5)
            {
                type = MapDirectionCommandType.CommandTurnRight;
            }
            else if (bearing >= 112.5 && bearing < 175)
            {
                type = MapDirectionCommandType.CommandSharpRight;
            }
            else if (bearing >= 175 && bearing < 185)
            {
                type = MapDirectionCommandType.CommandUTurn;
            }
            else if (bearing >= 185 && bearing < 247.5)
            {
                type = MapDirectionCommandType.CommandSharpLeft;
            }
            else if (bearing >= 247.5 && bearing < 320)
            {
                type = MapDirectionCommandType.CommandTurnLeft;
            }
            else if (bearing >= 320 && bearing < 350)
            {
                type = MapDirectionCommandType.CommandBearLeft;
            }
            return type;
        }


    }



}
