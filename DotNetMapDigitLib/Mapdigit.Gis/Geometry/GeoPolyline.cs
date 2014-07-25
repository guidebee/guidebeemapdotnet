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

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Gis.Geometry
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 27SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Polyline on map.
    /// </summary>
    public sealed class GeoPolyline
    {


        /// <summary>
        /// Zoom factor
        /// </summary>
        public int ZoomFactor;

        /// <summary>
        /// total zoom level, default 18
        /// </summary>
        public int NumLevels;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoPolyline"/> class.
        /// </summary>
        public GeoPolyline()
        {
            _latlngs = null;
            _levels = null;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoPolyline"/> class.
        /// </summary>
        /// <param name="pline">pline object copied from</param>
        public GeoPolyline(GeoPolyline pline)
        {
            if (pline._latlngs != null)
            {
                _latlngs = new GeoLatLng[pline._latlngs.Length];
                Array.Copy(_latlngs, 0, _latlngs, 0, _latlngs.Length);
                _levels = new int[pline._levels.Length];
                for (int i = 0; i < _levels.Length; i++)
                {
                    _levels[i] = pline._levels[i];
                }
                _bounds = new GeoLatLngBounds(pline._bounds);
            }
            _color = pline._color;
            _weight = pline._weight;
            _opacity = pline._opacity;
            ZoomFactor = pline.ZoomFactor;
            NumLevels = pline.NumLevels;
            _visible = pline._visible;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates a polyline from an array of vertices.  The weight is the width of
        /// the line in pixels. The opacity is given as a number between 0 and 1.
        /// The line will be antialiased and semitransparent.
        /// </summary>
        /// <param name="latlngs"> array of points.</param>
        /// <param name="color">the color of the polyline.</param>
        /// <param name="weight">the width of the polyline.</param>
        /// <param name="opacity"> the opacity of the polyline.</param>
        public GeoPolyline(GeoLatLng[] latlngs, int color, int weight,
                double opacity)
        {
            if (latlngs != null)
            {
                _latlngs = new GeoLatLng[latlngs.Length];
                Array.Copy(latlngs, 0, _latlngs, 0, latlngs.Length);
                _levels = new int[latlngs.Length];
                for (int i = 0; i < _levels.Length; i++)
                {
                    _levels[i] = 16;
                }
                double maxlat = 0, minlat = 0, maxlon = 0, minlon = 0;
                GeoLatLng[] points = _latlngs;
                for (int i = 0; i < points.Length; i++)
                {

                    // determin bounds (Max/Min Lat/lon)
                    if (i == 0)
                    {
                        maxlat = minlat = points[i].Latitude;
                        maxlon = minlon = points[i].Longitude;
                    }
                    else
                    {
                        if (points[i].Latitude > maxlat)
                        {
                            maxlat = points[i].Latitude;
                        }
                        else if (points[i].Latitude < minlat)
                        {
                            minlat = points[i].Latitude;
                        }
                        else if (points[i].Longitude > maxlon)
                        {
                            maxlon = points[i].Longitude;
                        }
                        else if (points[i].Longitude < minlon)
                        {
                            minlon = points[i].Longitude;
                        }
                    }
                }
                GeoLatLng sw = new GeoLatLng(minlat, minlon);
                GeoLatLng ne = new GeoLatLng(maxlat, maxlon);
                _bounds = new GeoLatLngBounds(sw, ne);
            }
            _color = color;
            _weight = weight;
            _opacity = opacity;
            ZoomFactor = 1;
            NumLevels = 0;
            _visible = true;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates a polyline from encoded strings of aggregated points and levels.
        /// zoomFactor and  numLevels  these two values determine the precision
        /// of the levels within an encoded polyline.
        /// </summary>
        /// <param name="color">the color of the polyline</param>
        /// <param name="weight">width of the line in pixels.</param>
        /// <param name="opacity">the opacity of the polyline.</param>
        /// <param name="points">a string containing the encoded latitude and longitude
        ///   coordinates.</param>
        /// <param name="zoomFactor">the magnification between adjacent sets of zoom levels
        ///  in the encoded levels string.</param>
        /// <param name="levels">a string containing the encoded polyline zoom level groups</param>
        /// <param name="numLevels">the number of zoom levels contained in the encoded
        ///  levels string.</param>
        /// <returns>Geo polyline object</returns>
        public static GeoPolyline FromEncoded(int color, int weight, double opacity,
                string points, int zoomFactor, string levels, int numLevels)
        {
            ArrayList trk = PolylineEncoder.CreateDecodings(points);
            GeoLatLng[] array = new GeoLatLng[trk.Count];
            var temp = trk.ToArray();
            for (int i = 0; i < temp.Length; i++)
            {
                array[i] = (GeoLatLng)temp[i];
            }
            GeoPolyline polyline = new GeoPolyline(array, color, weight, opacity);
            polyline._levels = PolylineEncoder.DecodeLevel(levels);
            polyline.ZoomFactor = zoomFactor;
            polyline.NumLevels = numLevels;
            return polyline;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the distance from GeoPoint (x,y) to the closest line
        /// segment in the Poly (int[] xpts, int[] ypts).
        /// 
        /// This procedure assumes that xpts.length == ypts.length.
        /// </summary>
        /// <param name="ptx"> X points of the polygon</param>
        /// <param name="pty">Y points of the polygon</param>
        /// <param name="xpts">x location of the point</param>
        /// <param name="ypts">Ty location of the point</param>
        /// <param name="isPolygon"> polyline or polygon</param>
        /// <returns></returns>
        public static double DistanceToPoly(double ptx, double pty, double[] xpts,
                double[] ypts, bool isPolygon)
        {
            return IndexOfClosestdistanceToPoly(ptx, pty, xpts, ypts, isPolygon).X;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the distance from GeoPoint (x,y) to the closest line
        /// segment in the Poly (int[] xpts, int[] ypts).
        /// 
        /// This procedure assumes that xpts.length == ypts.length.
        /// </summary>
        /// <param name="ptx">X points of the polygon</param>
        /// <param name="pty">Y points of the polygon</param>
        /// <param name="xpts">x location of the point</param>
        /// <param name="ypts">y location of the point</param>
        /// <param name="isPolygon"> polyline or polygon</param>
        /// <returns> GeoPoint whose x is the closes distance, y is the index of the poly</returns>
        public static GeoPoint IndexOfClosestdistanceToPoly(double ptx, double pty,
                double[] xpts,
                double[] ypts, bool isPolygon)
        {
            GeoPoint retValue = new GeoPoint();
            if (xpts.Length == 0)
            {
                retValue.X = double.PositiveInfinity;
                retValue.Y = 0;
            }
            if (xpts.Length == 1)
            {
                retValue.X = Distance(xpts[0], ypts[0], ptx, pty);
                retValue.Y = 0;
            }

            double temp, distance = double.PositiveInfinity;
            int i, j;

            for (i = 0, j = 1; j < xpts.Length; i++, j++)
            {
                temp = DistanceToLine(xpts[i],
                        ypts[i],
                        xpts[j],
                        ypts[j],
                        ptx,
                        pty);
                if (temp < distance)
                {
                    distance = temp;
                    retValue.X = distance;
                    retValue.Y = i;
                }
            }

            // connect
            if (isPolygon)
            {
                temp = DistanceToLine(xpts[i],
                        ypts[i],
                        xpts[0],
                        ypts[0],
                        ptx,
                        pty);
                if (temp < distance)
                {
                    distance = temp;
                    retValue.X = distance;
                    retValue.Y = i;
                }
            }
            return retValue;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the distance from GeoPoint (x,y) to the closest line
        /// segment in the Poly (int[] xpts, int[] ypts).
        /// 
        /// This procedure assumes that xpts.length == ypts.length.
        /// </summary>
        /// <param name="latLng">location of the point</param>
        /// <returns> GeoPoint whose x is the closes distance, y is the index of the poly</returns>
        public GeoPoint IndexOfClosestdistanceToPoly(GeoLatLng latLng)
        {
            double[] xpts = new double[_latlngs.Length];
            double[] ypts = new double[_latlngs.Length];
            for (int i = 0; i < _latlngs.Length; i++)
            {
                xpts[i] = _latlngs[i].X;
                ypts[i] = _latlngs[i].Y;
            }
            return IndexOfClosestdistanceToPoly(latLng.X, latLng.Y, xpts, ypts, false);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns n or n+1 points along a line.
        /// </summary>
        /// <param name="pt1">The PT1.</param>
        /// <param name="pt2">The PT2.</param>
        /// <param name="n">The n.</param>
        /// <param name="includeLast">if set to <c>true</c> [include_last].</param>
        /// <returns></returns>
        public static GeoPoint[] LineSegments(GeoPoint pt1, GeoPoint pt2, int n,
                                                 bool includeLast)
        {

            GeoPoint v = new GeoPoint(pt2.X - pt1.X, pt2.Y - pt1.Y);
            int end = includeLast ? n + 1 : n;
            GeoPoint[] retVal = new GeoPoint[end];
            double inc = 1f / (double)n;
            double t = inc;

            retVal[0] = pt1;
            for (int i = 1; i < end; i++, t += inc)
            {
                retVal[i] = new GeoPoint(pt1.X + (v.X * t), pt1.Y
                        + (v.Y * t));
            }
            return retVal;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Find the closest point on a line from a given pt.
        /// </summary>
        /// <param name="pt1">start point of the line</param>
        /// <param name="pt2">end point of the line.</param>
        /// <param name="pt">distance to this point.</param>
        /// <param name="segmentClamp">is segment or not</param>
        /// <returns>the closet point on the line</returns>
        public static GeoPoint GetClosetPoint(GeoPoint pt1, GeoPoint pt2,
                GeoPoint pt, bool segmentClamp)
        {
            GeoPoint p1 = new GeoPoint(pt.X - pt1.X, pt.Y - pt1.Y);
            GeoPoint p2 = new GeoPoint(pt2.X - pt1.X, pt2.Y - pt1.Y);
            double ab2 = p2.X * p2.X + p2.Y * p2.Y;
            double apAb = p1.X * p2.X + p1.Y * p2.Y;
            double t = apAb / ab2;
            if (segmentClamp)
            {
                if (t < 0.0f)
                {
                    t = 0.0f;
                }
                else if (t > 1.0f)
                {
                    t = 1.0f;
                }

            }
            GeoPoint closestPt = new GeoPoint(pt1.X + p2.X * t, pt1.Y + p2.Y * t);
            return closestPt;

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Distance to closest endpoint.
        /// </summary>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static double DistanceToEndpoint(int x1, int y1, int x2,
                                                       int y2, int x, int y)
        {

            return Math.Min(Distance(x1, y1, x, y), Distance(x2, y2, x, y));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Computes the distance from a point to a line segment.
        /// </summary>
        /// <remarks>
        /// Variable usage as follows:
        /// 
        /// <ul>
        /// <li>x12 x distance from the first endpoint to the second.</li>
        /// <li>y12 y distance from the first endpoint to the second.</li>
        /// <li>x13 x distance from the first endpoint to point being
        /// tested.</li>
        /// <li>y13 y distance from the first endpoint to point being
        /// tested.</li>
        /// <li>x23 x distance from the second endpoint to point being
        /// tested.</li>
        /// <li>y23 y distance from the second endpoint to point being
        /// tested.</li>
        /// <li>D12 Length of the line segment.</li>
        /// <li>pp distance along the line segment to the intersection of
        /// the perpendicular from the point to line extended.</li>
        /// </ul>
        ///
        /// Procedure:
        /// 
        ///
        /// Compute D12, the length of the line segment. Compute pp, the
        /// distance to the perpendicular. If pp is negative, the
        /// intersection is before the start of the line segment, so return
        /// the distance from the start point. If pp exceeds the length of
        /// the line segment, then the intersection is beyond the end point
        /// so return the distance of the point from the end point.
        /// Otherwise, return the absolute value of the length of the
        /// perpendicular line. The sign of the length of the perpendicular
        /// line indicates whether the point lies to the right or left of
        /// the line as one travels from the start point to the end point.
        /// </remarks>
        /// <param name="x1">line x coord1</param>
        /// <param name="y1">line y coord1</param>
        /// <param name="x2">line x coord2</param>
        /// <param name="y2">line y coord2</param>
        /// <param name="x">point x coord</param>
        /// <param name="y">point y coord</param>
        /// <returns>double distance to line segment</returns>
        public static double DistanceToLine(double x1, double y1, double x2, double y2,
                                                   double x, double y)
        {

            // algorithm courtesy of Ray 1/16/98
            double x12 = x2 - x1;
            double y12 = y2 - y1;
            double x13 = x - x1;
            double y13 = y - y1;
            double d12 = Math.Sqrt(x12 * x12 + y12 * y12);
            if (d12 == 0)
            {
                return Math.Sqrt(x13 * x13 + y13 * y13);
            }
            double pp = (x12 * x13 + y12 * y13) / d12;
            if (pp < 0.0)
            {
                return Math.Sqrt(x13 * x13 + y13 * y13);
            }
            if (pp > d12)
            {
                double x23 = x - x2;
                double y23 = y - y2;
                return Math.Sqrt(x23 * x23 + y23 * y23);
            }
            return Math.Abs(((x12 * y13 - y12 * x13) / d12));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 2D distance formula.
        /// </summary>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        /// <returns></returns>
        public static double Distance(double x1, double y1, double x2, double y2)
        {
            double xdiff = x2 - x1;
            double ydiff = y2 - y1;
            return Math.Sqrt((xdiff * xdiff + ydiff * ydiff));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Distances the specified x1.
        /// </summary>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        /// <returns></returns>
        public static double Distance(int x1, int y1, int x2, int y2)
        {
            int xdiff = x2 - x1;
            int ydiff = y2 - y1;
            return Math.Sqrt(xdiff * xdiff + ydiff * ydiff);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the number of vertices in the polyline.
        /// </summary>
        /// <returns>the number of vertices in the polyline.</returns>
        public int GetVertexCount()
        {
            if (_latlngs != null)
            {
                return _latlngs.Length;
            }
            return 0;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the vertex with the given index in the polyline.
        /// </summary>
        /// <param name="index">the index of the point.</param>
        /// <returns>the vertex with the given index in the polyline</returns>
        public GeoLatLng GetVertex(int index)
        {
            if (_latlngs != null)
            {
                return _latlngs[index];
            }
            return null;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 03JAN2009  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        // <summary>
        /// Set the level with the given index in the polyline.
        /// </summary>
        /// <param name="index">the index of the point..</param>
        /// <returns>the level with the given index in the polyline.</returns>
        public void SetLevel(int index, int level)
        {
            if (_levels != null)
            {
                _levels[index] = level;
            }
        }
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the level with the given index in the polyline.
        /// </summary>
        /// <param name="index">the index of the point..</param>
        /// <returns>the level with the given index in the polyline.</returns>
        public int GetLevel(int index)
        {
            if (_levels != null)
            {
                return _levels[index];
            }
            return 0;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the length (in meters) of the polyline along the surface of a
        /// spherical Earth
        /// </summary>
        /// <returns>the length (in meters) of the polyline</returns>
        public int GetLength()
        {
            int len = 0;
            if (_latlngs != null)
            {
                double length = 0;
                GeoLatLng pt1 = new GeoLatLng(_latlngs[0].Latitude, _latlngs[0].Longitude);
                for (int i = 1; i < _latlngs.Length; i++)
                {
                    GeoLatLng pt2 = new GeoLatLng(_latlngs[i].Latitude, _latlngs[i].Longitude);
                    length += pt1.DistanceFrom(pt2);
                    pt1 = pt2;
                }
                len = (int)(length * 1000.0 + 0.5);
            }
            return len;

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the length (in meters) of the polyline along the surface of a
        /// spherical Earth
        /// </summary>
        /// <param name="startIndex">start point index.</param>
        /// <param name="endIndex">end point idnex(not included).</param>
        /// <returns> the length (in meters) of the polyline between two point</returns>
        public int GetLength(int startIndex, int endIndex)
        {
            int len = 0;
            if (_latlngs != null && startIndex >= 0 && endIndex < _latlngs.Length &&
                    startIndex < endIndex)
            {
                double length = 0;
                GeoLatLng pt1 = new GeoLatLng(_latlngs[startIndex].Latitude, _latlngs[startIndex].Longitude);
                for (int i = startIndex + 1; i < endIndex; i++)
                {
                    GeoLatLng pt2 = new GeoLatLng(_latlngs[i].Latitude, _latlngs[i].Longitude);
                    length += pt1.DistanceFrom(pt2);
                    pt1 = pt2;
                }
                len = (int)(length * 1000.0 + 0.5);
            }
            return len;

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the bounds for this polyline.
        /// </summary>
        /// <returns>the bounds for this polyline.</returns>
        public GeoLatLngBounds GetBounds()
        {
            return _bounds;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Hides this polyline.
        /// </summary>
        public void Hide()
        {
            _visible = false;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Show this polyline.
        /// </summary>
        public void Show()
        {
            _visible = true;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns true if the polyline is currently hidden. Otherwise returns false
        /// </summary>
        /// <returns>
        /// 	true if the polyline is currently hidden.
        /// </returns>
        public bool IsHidden()
        {
            return !_visible;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns true if GeoPolyline.hide() is supported
        /// </summary>
        /// <returns>always true.</returns>
        public bool SupportsHide()
        {
            return true;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// set the array of points which consist of the line.
        /// </summary>
        /// <param name="points">points array of points</param>
        public void SetPoints(GeoLatLng[] points)
        {
            _latlngs = points;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// get the array of points which consist of the line.
        /// </summary>
        /// <returns>the array of points which consist of the line.</returns>
        public GeoLatLng[] GetPoints()
        {
            return _latlngs;
        }

        /**
         * array store points in the polyline
         */
        private GeoLatLng[] _latlngs;
        /**
         * Color of the polyline
         */
        private readonly int _color;
        /**
         * Width of the polyline
         */
        private readonly int _weight;
        /**
         * Opacity of the polyline
         */
        private readonly double _opacity;

        /**
         * level for each point.
         */
        private int[] _levels;
        /**
         * the bounds of the polyline
         */
        private readonly GeoLatLngBounds _bounds;
        /**
         * visible or not
         */
        private bool _visible = true;
    }

}
