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
    /// Polygon on map.
    /// </summary>
    public sealed class GeoPolygon
    {

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoPolygon"/> class.
        /// </summary>
        public GeoPolygon()
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
        /// Initializes a new instance of the <see cref="GeoPolygon"/> class.
        /// </summary>
        /// <param name="polygon">polygon object copied from.</param>
        public GeoPolygon(GeoPolygon polygon)
        {
            if (polygon._latlngs != null)
            {
                _latlngs = new GeoLatLng[polygon._latlngs.Length];
                Array.Copy(_latlngs, 0, _latlngs, 0, _latlngs.Length);
                _levels = new int[polygon._levels.Length];
                for (int i = 0; i < _levels.Length; i++)
                {
                    _levels[i] = polygon._levels[i];
                }
                _bounds = new GeoLatLngBounds(polygon._bounds);
            }
            _strokeColor = polygon._strokeColor;
            _strokeOpacity = polygon._strokeOpacity;
            _strokeWeight = polygon._strokeWeight;
            _fillColor = polygon._fillColor;
            _fillOpacity = polygon._fillOpacity;
            _zoomFactor = polygon._zoomFactor;
            _numLevels = polygon._numLevels;
            _visible = polygon._visible;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates a polygon from an array of vertices.  The weight is the width of
        /// the line in pixels. The opacity is given as a number between 0 and 1.
        /// The line will be antialiased and semitransparent.
        /// </summary>
        /// <param name="latlngs"> array of points.</param>
        /// <param name="strokeColor">the color of the polygon stroke.</param>
        /// <param name="strokeWeight">the width of the polygon stroke.</param>
        /// <param name="strokeOpacity">the opacity of the polygon stroke.</param>
        /// <param name="fillColor">the inner color of the polygon.</param>
        /// <param name="fillOpacity">the inner opacity of the polygon.</param>
        public GeoPolygon(GeoLatLng[] latlngs, int strokeColor, int strokeWeight,
                double strokeOpacity, int fillColor, double fillOpacity)
        {
            if (latlngs != null)
            {
                _latlngs = new GeoLatLng[latlngs.Length];
                Array.Copy(latlngs, 0, _latlngs, 0, latlngs.Length);
                _levels = new int[latlngs.Length];
                for (int i = 0; i < _levels.Length; i++)
                {
                    _levels[i] = 0;
                }
                double maxlat = 0, minlat = 0, maxlon = 0, minlon = 0;
                GeoLatLng[] points = _latlngs;
                for (int i = 0; i < points.Length; i++)
                {

                    // determin _bounds (Max/Min Lat/lon)
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
                GeoLatLng sw, ne;
                sw = new GeoLatLng(minlat, minlon);
                ne = new GeoLatLng(maxlat, maxlon);
                _bounds = new GeoLatLngBounds(sw, ne);
            }
            _strokeColor = strokeColor;
            _strokeOpacity = strokeOpacity;
            _strokeWeight = strokeWeight;
            _fillColor = fillColor;
            _fillOpacity = fillOpacity;
            _zoomFactor = 1;
            _numLevels = 0;
            _visible = true;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates a polygon from encoded strings of aggregated points and levels.
        /// zoomFactor and  numLevels  these two values determine the precision
        /// of the levels within an encoded polygon.
        /// </summary>
        /// <param name="strokeColor">the color of the polygon.</param>
        /// <param name="strokeWeight">width of the line in pixels</param>
        /// <param name="strokeOpacity">the opacity of the polygon.</param>
        /// <param name="fillColor">the inner color of the polygon.</param>
        /// <param name="fillOpacity">the inner opacity of the polygon.</param>
        /// <param name="points">a string containing the encoded latitude and longitude
        ///  coordinates.</param>
        /// <param name="zoomFactor"> the magnification between adjacent sets of zoom levels
        /// in the encoded levels string</param>
        /// <param name="levels">a string containing the encoded polygon zoom level groups</param>
        /// <param name="numLevels">the number of zoom levels contained in the encoded levels string.</param>
        /// <returns>Geo polygon object</returns>
        public static GeoPolygon FromEncoded(int strokeColor, int strokeWeight,
                double strokeOpacity, int fillColor, double fillOpacity,
                string points, int zoomFactor, string levels, int numLevels)
        {
            ArrayList trk = PolylineEncoder.CreateDecodings(points);
            GeoLatLng[] array = new GeoLatLng[trk.Count];
            var temp = trk.ToArray();
            for (int i = 0; i < temp.Length; i++)
            {
                array[i] = (GeoLatLng)temp[i];
            }
            GeoPolygon polygon = new GeoPolygon(array, strokeColor, strokeWeight, strokeOpacity,
                    fillColor, fillOpacity);
            polygon._levels = PolylineEncoder.DecodeLevel(levels);
            polygon._zoomFactor = zoomFactor;
            polygon._numLevels = numLevels;
            return polygon;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the number of vertices in the polygon.
        /// </summary>
        /// <returns>the number of vertices in the polygon.</returns>
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
        /// Returns the vertex with the given index in the polygon.
        /// </summary>
        /// <param name="index">the index of the point.</param>
        /// <returns>the vertex with the given index in the polygon</returns>
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
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the bounds for this polygon.
        /// </summary>
        /// <returns>the bounds for this polygon</returns>
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
        /// Hides the polygon. 
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
        /// Shows the polygon.
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
        /// Returns true if the polygon is currently hidden. Otherwise returns false.
        /// </summary>
        /// <returns>
        /// 	true if the polygon is currently hidden.
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
        /// Returns true if GeoPolygon.hide() is supported 
        /// </summary>
        /// <returns>always is true</returns>
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
        /// <param name="points">array of points</param>
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
        ///  get the array of points which consist of the line.
        /// </summary>
        /// <returns>the points stored in the line</returns>
        public GeoLatLng[] GetPoints()
        {
            return _latlngs;
        }

        /**
         * array store points in the polygon
         */
        private GeoLatLng[] _latlngs;

        /**
         * stroke color of the polygon
         */
        private readonly int _strokeColor;

        /**
         * stroke Width of the polygon
         */
        private readonly int _strokeWeight;

        /**
         * stroke opacity of the polygon
         */
        private readonly double _strokeOpacity;

        /**
         * fill color
         */
        private readonly int _fillColor;

        /**
         * fill opacity of the polygon
         */
        private readonly double _fillOpacity;

        /**
         * Zoom factor
         */
        private int _zoomFactor;

        /**
         * total zoom level, default 18
         */
        private int _numLevels;

        /**
         * level for each point.
         */
        private int[] _levels;

        /**
         * the _bounds of the polyline
         */
        private readonly GeoLatLngBounds _bounds;

        /**
         * _visible or not
         */
        private bool _visible = true;

    }

}
