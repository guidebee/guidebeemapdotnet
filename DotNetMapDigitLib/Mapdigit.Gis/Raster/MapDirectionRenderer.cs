//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 28SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System;
using System.Collections;
using Mapdigit.Drawing;
using Mapdigit.Gis.Drawing;
using Mapdigit.Drawing.Geometry;
using Mapdigit.Gis.Geometry;
using Mapdigit.Gis.Vector;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Gis.Raster
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 28SEP2010  James Shen                 	          Initial Creation
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Vector map render, each time, the renderer draw one map tile.
    /// </summary>
    internal class MapDirectionRenderer : MapTileAbstractReader
    {
        /**
         * transparent color value.
         */
        private const int Transparency = 0x00FFFFFF;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapDirectionRenderer"/> class.
        /// </summary>
        public MapDirectionRenderer()
        {

            if (MapConfiguration._routePen != null)
            {
                _routePen = MapConfiguration._routePen;
            }
            else
            {
                _routePen = new Pen(new Color(0x7F00FF00, false), 4);
            }
            _mapDirectionLayer = new MapDirectionLayer(MapLayer.MapTileWidth,
                    MapLayer.MapTileWidth, this);
            if (MapConfiguration._startIconBrush != null)
            {
                _startRouteBrush = MapConfiguration._startIconBrush;
            }
            else
            {
                _startRouteBrush = new SolidBrush(Color.Blue);
            }
            if (MapConfiguration._endIconBrush != null)
            {
                _endRouteBrush = MapConfiguration._endIconBrush;
            }
            else
            {
                _endRouteBrush = new SolidBrush(Color.Red);
            }
            if (MapConfiguration._middleIconBrush != null)
            {
                _middleRouteBrush = MapConfiguration._middleIconBrush;
            }
            else
            {
                _middleRouteBrush = new SolidBrush(Color.Orange);
            }
            _linePen = new Pen(new Color(0, false), 2);

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the route pen.
        /// </summary>
        /// <param name="routePen">The route pen.</param>
        public void SetRoutePen(Pen routePen)
        {
            if (routePen != null)
            {
                _routePen = routePen;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the map direction.
        /// </summary>
        /// <param name="newDirection">The new direction.</param>
        public void SetMapDirection(MapDirection newDirection)
        {
            _mapDirectionLayer.SetMapDirection(newDirection);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the map directions.
        /// </summary>
        /// <param name="newDirections">The new directions.</param>
        public void SetMapDirections(MapDirection[] newDirections)
        {
            _mapDirectionLayer.SetMapDirections(newDirections);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the map direction.
        /// </summary>
        /// <returns>the map direction</returns>
        public MapDirection GetMapDirection()
        {
            return _mapDirectionLayer.GetMapDirection();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the map directions.
        /// </summary>
        /// <returns>the map directions</returns>
        public MapDirection[] GetMapDirections()
        {
            return _mapDirectionLayer.GetMapDirections();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get image at given location. when the reader is done, imageArray shall
        /// store the image byte array. imageArraySize is the actually data size.
        /// isImagevalid indicate the data is valid or not. normally this shall be
        /// an async call.
        /// </summary>
        /// <param name="mtype">the map type of the map tile.</param>
        /// <param name="x">the x index of the map tile.</param>
        /// <param name="y">the y index of the map tile.</param>
        /// <param name="zoomLevel">the zoom level of the map tile.</param>
        public override void GetImage(int mtype, int x, int y, int zoomLevel)
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the image at given x,y zoom level
        /// </summary>
        /// <param name="x">x index of the map tile</param>
        /// <param name="y">y index of the map tile</param>
        /// <param name="zoomLevel">zoom level of the map tile</param>
        /// <returns>the given image</returns>
        public IImage GetImage(int x, int y, int zoomLevel)
        {
            MapDirection mapDirection = GetMapDirection();
            if (mapDirection != null)
            {
                try
                {
                    const int shiftWidth = 4;
                    GeoPoint pt1 = new GeoPoint(x * MapLayer.MapTileWidth - shiftWidth,
                            y * MapLayer.MapTileWidth - shiftWidth);
                    GeoPoint pt2 = new GeoPoint((x + 1) * MapLayer.MapTileWidth + shiftWidth,
                            (y + 1) * MapLayer.MapTileWidth + shiftWidth);
                    GeoLatLng latLng1 = MapLayer.FromPixelToLatLng(pt1, zoomLevel);
                    GeoLatLng latLng2 = MapLayer.FromPixelToLatLng(pt2, zoomLevel);
                    double minY = Math.Min(latLng1.Latitude, latLng2.Latitude);
                    double maxY = Math.Max(latLng1.Latitude, latLng2.Latitude);
                    double minX = Math.Min(latLng1.Longitude, latLng2.Longitude);
                    double maxX = Math.Max(latLng1.Longitude, latLng2.Longitude);
                    GeoLatLngBounds geoBounds = new GeoLatLngBounds(minX, minY,
                            maxX - minX, maxY - minY);
                    GeoLatLng centerPt = geoBounds.Center;
                    _mapDirectionLayer.SetCenter(centerPt, zoomLevel);
                    _mapDirectionLayer._screenBounds = geoBounds;
                    _mapDirectionLayer.Paint(null);
                    IsImagevalid = true;
                    return MapLayer.GetAbstractGraphicsFactory()
                        .CreateImage(_mapDirectionLayer.GetRGB(),
                                     MapLayer.MapTileWidth, MapLayer.MapTileWidth);
                }
                catch (Exception)
                {

                }
            }
            return null;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// a way app can cancel the reading process.
        /// </summary>
        public override void CancelRead()
        {
        }

        private readonly MapDirectionLayer _mapDirectionLayer;

        private Pen _routePen;
        private readonly Pen _linePen;
        private readonly Brush _startRouteBrush;
        private readonly Brush _middleRouteBrush;
        private readonly Brush _endRouteBrush;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This a map layer delicated to draw routing result.
        /// </summary>
        private class MapDirectionLayer : MapLayer
        {
            private readonly MapDirectionRenderer _mapDirectionRenderer;

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Initializes a new instance of the <see cref="MapDirectionLayer"/> class.
            /// </summary>
            /// <param name="width">The width.</param>
            /// <param name="height">The height.</param>
            /// <param name="mapDirectionRenderer">The map direction renderer.</param>
            public MapDirectionLayer(int width, int height, MapDirectionRenderer mapDirectionRenderer)
                : base(width, height)
            {
                _mapDirectionRenderer = mapDirectionRenderer;
                _mapDrawingTileWidth = MapTileWidth / MapConfiguration._mapDirectionRenderBlocks;
                if (!MapConfiguration._lowMemoryMode)
                {
                    _routeGraphics2D = new Graphics2D(_mapDrawingTileWidth,
                        _mapDrawingTileWidth);
                }

            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Sets the map direction.
            /// </summary>
            /// <param name="newDirection">The new direction.</param>
            public void SetMapDirection(MapDirection newDirection)
            {
                if (newDirection != null)
                {
                    _currentMapDirections = new MapDirection[1];
                    _currentMapDirections[0] = newDirection;
                }
                else
                {
                    _currentMapDirections = null;
                }
            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Sets the map directions.
            /// </summary>
            /// <param name="newDirections">The new directions.</param>
            public void SetMapDirections(MapDirection[] newDirections)
            {
                _currentMapDirections = newDirections;
            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Gets the map direction.
            /// </summary>
            /// <returns></returns>
            public MapDirection GetMapDirection()
            {
                if (_currentMapDirections != null)
                {
                    return _currentMapDirections[0];
                }
                return null;
            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Gets the map directions.
            /// </summary>
            /// <returns></returns>
            public MapDirection[] GetMapDirections()
            {
                return _currentMapDirections;
            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Gets the RGB.
            /// </summary>
            /// <returns></returns>
            internal int[] GetRGB()
            {
                return _routeGraphics2D.Argb;
            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Draw the map layer to an graphics.
            /// </summary>
            /// <param name="graphics">The graphics.</param>
            public override void Paint(IGraphics graphics)
            {
                Paint(graphics, 0, 0);
            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Draw the map layer to an graphics
            /// </summary>
            /// <param name="graphics">the graphics object where the map is drawn..</param>
            /// <param name="offsetX">The offset X.</param>
            /// <param name="offsetY">The offset Y.</param>
            public override void Paint(IGraphics graphics, int offsetX, int offsetY)
            {
                Paint(offsetX, offsetY, MapTileWidth, MapTileWidth);
            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Paints the specified offset X.
            /// </summary>
            /// <param name="offsetX">The offset X.</param>
            /// <param name="offsetY">The offset Y.</param>
            /// <param name="width">The width.</param>
            /// <param name="height">The height.</param>
            private void Paint(int offsetX, int offsetY, int width, int height)
            {
                if (_currentMapDirections != null)
                {
                    lock (syncObject)
                    {
                        DrawRouteCanvas(offsetX, offsetY, width, height);
                    }

                }
            }

            internal GeoLatLngBounds _screenBounds;
            private readonly Graphics2D _routeGraphics2D;
            private volatile MapDirection[] _currentMapDirections;
            /**
             * SutherlandHodgman clip pline and region.
             */
            private SutherlandHodgman _sutherlandHodgman;

            /**
             * When draw the map tile, the default map tile width is 64X64
             * using 64X64 istread of 256X256 mainly for saving memory usage in
             * memeory constrained devices. it can change to a bigger value if the
             * memory is not issue.
             */
            private readonly int _mapDrawingTileWidth;

            private const int RouteIconWidth = 12;


            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// check if need to show on map.
            /// </summary>
            /// <param name="numLevel">The num level.</param>
            /// <param name="zoomLevel">The zoom level.</param>
            /// <returns></returns>
            private static int NeedShowLevel(int numLevel, int zoomLevel)
            {
                const int totalZoomLevel = 16;
                int mapGrade = (totalZoomLevel - zoomLevel) / numLevel - 1;
                return mapGrade;

            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Draws the route canvas.
            /// </summary>
            /// <param name="x">The x.</param>
            /// <param name="y">The y.</param>
            /// <param name="width">The width.</param>
            /// <param name="height">The height.</param>
            private void DrawRouteCanvas(int x, int y,
                    int width, int height)
            {
                if (_currentMapDirections != null)
                {
                    try
                    {
                        DrawRouteImage(_currentMapDirections, x, y, width, height);
                        DrawRouteIcons(_currentMapDirections, x, y, width, height);
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Draws the route image.
            /// </summary>
            /// <param name="mapDirections">The map directions.</param>
            /// <param name="x">The x.</param>
            /// <param name="y">The y.</param>
            /// <param name="width">The width.</param>
            /// <param name="height">The height.</param>
            private void DrawRouteImage(MapDirection[] mapDirections, int x, int y,
                   int width, int height)
            {


                _sutherlandHodgman = new SutherlandHodgman(_screenBounds);
                _routeGraphics2D.Clear(Transparency);
                for (int k = 0; k < mapDirections.Length; k++)
                {
                    MapDirection mapDirection = mapDirections[k];
                    ArrayList polyline = new ArrayList();
                    int minLevel = NeedShowLevel(mapDirection.Polyline.NumLevels, GetZoom());
                    int level;
                    for (int i = 0; i < mapDirection.Polyline.GetVertexCount(); i++)
                    {
                        level = mapDirection.Polyline.GetLevel(i);
                        if (level >= minLevel)
                        {
                            polyline.Add(mapDirection.Polyline.GetVertex(i));
                        }
                    }
                    ArrayList clippedPts = _sutherlandHodgman.ClipPline(polyline);

                    GeoLatLng pt2, pt;
                    GeoPoint newPt1;
                    GeoPoint newPt2;

                    GeoPoint drawPt1 = new GeoPoint(0, 0), drawPt2 = new GeoPoint(0, 0);
                    int steps = 1;
                    int numOfTiles = MapTileWidth / _mapDrawingTileWidth;
                    Rectangle drawArea = new Rectangle();
                    Rectangle intersectRect = new Rectangle(0, 0, width, height);
                    int xIndex, yIndex;
                    for (xIndex = 0; xIndex < numOfTiles; xIndex++)
                    {
                        for (yIndex = 0; yIndex < numOfTiles; yIndex++)
                        {
                            bool hasPt1 = false;
                            GeoLatLng pt1 = null;

                            drawArea.X = xIndex * _mapDrawingTileWidth;
                            drawArea.Y = yIndex * _mapDrawingTileWidth;
                            drawArea.Width = drawArea.Height = _mapDrawingTileWidth;
                            drawArea = intersectRect.Intersection(drawArea);
                            int totalPointSize = clippedPts.Count;
                            if (!drawArea.IsEmpty())
                            {
                                _routeGraphics2D.SetClip(0, 0,
                                        drawArea.Width, drawArea.Height);
                                try
                                {
                                    for (int j = 0; j < totalPointSize; j += steps)
                                    {
                                        pt = (GeoLatLng)clippedPts[j];
                                        level = minLevel;
                                        if (hasPt1 == false)
                                        {
                                            if (level >= minLevel)
                                            {
                                                {
                                                    {
                                                        hasPt1 = true;
                                                        pt1 = pt;
                                                        continue;
                                                    }
                                                }
                                            }
                                        }
                                        if (hasPt1)
                                        {
                                            if (level >= minLevel)
                                            {
                                                pt2 = pt;
                                                newPt1 = FromLatLngToMapPixel(pt1);
                                                newPt2 = FromLatLngToMapPixel(pt2);
                                                newPt1.X -= x + xIndex * _mapDrawingTileWidth;
                                                newPt1.Y -= y + yIndex * _mapDrawingTileWidth;
                                                newPt2.X -= x + xIndex * _mapDrawingTileWidth;
                                                newPt2.Y -= y + yIndex * _mapDrawingTileWidth;
                                                drawPt1.X = (int)newPt1.X;
                                                drawPt1.Y = (int)newPt1.Y;
                                                drawPt2.X = (int)newPt2.X;
                                                drawPt2.Y = (int)newPt2.Y;

                                                if ((drawPt1.Distance(drawPt2) > 0))
                                                {
                                                    _routeGraphics2D.DrawLine(_mapDirectionRenderer._routePen, (int)drawPt1.X,
                                                            (int)drawPt1.Y,
                                                            (int)drawPt2.X, (int)drawPt2.Y);
                                                    pt1 = pt2;
                                                    if (_mapDirectionRenderer.readListener != null)
                                                    {
                                                        _mapDirectionRenderer.readListener.ReadProgress(j,
                                                                totalPointSize);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception)
                                {

                                }
                            }


                        }
                    }
                    int[] rgb = _routeGraphics2D.Argb;
                    ModifyAlpha(rgb, 160, Transparency);
                }

            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Modifies the alpha.
            /// </summary>
            /// <param name="arr">The arr.</param>
            /// <param name="alpha">The alpha.</param>
            /// <param name="removeColor">Color of the remove.</param>
            private void ModifyAlpha(int[] arr, byte alpha, int removeColor)
            {
                removeColor = removeColor & 0xffffff;
                int w = _mapDrawingTileWidth;
                int h = _mapDrawingTileWidth;
                int size = w * h;
                int alphaInt = (int)((alpha << 24) & 0xff000000L);
                for (int iter = 0; iter < size; iter++)
                {
                    if ((arr[iter] & 0xff000000) != 0)
                    {
                        arr[iter] = (arr[iter] & 0xffffff) | alphaInt;
                        if (removeColor == (0xffffff & arr[iter]))
                        {
                            arr[iter] = 0;
                        }
                    }
                }

            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Draws the route icon.
            /// </summary>
            /// <param name="x">The x.</param>
            /// <param name="y">The y.</param>
            /// <param name="brush">The brush.</param>
            private void DrawRouteIcon(int x, int y, Brush brush)
            {
                int[] xPoints = new int[4];
                int[] yPoints = new int[4];
                int width = RouteIconWidth / 2;
                xPoints[0] = x; yPoints[0] = y - width;
                xPoints[1] = x - width; yPoints[1] = y;
                xPoints[2] = x; yPoints[2] = y + width;
                xPoints[3] = x + width; yPoints[3] = y;
                Polygon polygon = new Polygon(xPoints, yPoints, 4);
                _routeGraphics2D.FillPolygon(brush, polygon);
                _routeGraphics2D.DrawPolygon(_mapDirectionRenderer._linePen, polygon);

            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Draws the route icons.
            /// </summary>
            /// <param name="mapDirections">The map directions.</param>
            /// <param name="x">The x.</param>
            /// <param name="y">The y.</param>
            /// <param name="width">The width.</param>
            /// <param name="height">The height.</param>
            private void DrawRouteIcons(MapDirection[] mapDirections, int x, int y,
                   int width, int height)
            {

                for (int k = 0; k < mapDirections.Length; k++)
                {
                    MapDirection mapDirection = mapDirections[k];
                    GeoPoint newPt;
                    GeoLatLng pt;
                    _routeGraphics2D.SetClip(0, 0, width, height);
                    for (int j = 0; j < mapDirection.Routes.Length; j++)
                    {
                        for (int i = 0; i < mapDirection.Routes[j].Steps.Length - 1; i++)
                        {
                            MapStep mapStep = mapDirection.Routes[j].Steps[i];
                            pt = mapStep.LastLatLng;
                            if (_screenBounds.ContainsLatLng(pt))
                            {
                                newPt = FromLatLngToMapPixel(pt);
                                newPt.X -= x;
                                newPt.Y -= y;
                                DrawRouteIcon((int)newPt.X, (int)newPt.Y, _mapDirectionRenderer._middleRouteBrush);
                            }
                        }
                    }
                    pt = mapDirection.Polyline.GetVertex(0);
                    newPt = FromLatLngToMapPixel(pt);
                    newPt.X -= x;
                    newPt.Y -= y;
                    DrawRouteIcon((int)newPt.X, (int)newPt.Y, _mapDirectionRenderer._startRouteBrush);
                    pt = mapDirection.Polyline.GetVertex(mapDirection.Polyline.GetVertexCount() - 1);
                    newPt = FromLatLngToMapPixel(pt);
                    newPt.X -= x;
                    newPt.Y -= y;
                    DrawRouteIcon((int)newPt.X, (int)newPt.Y, _mapDirectionRenderer._endRouteBrush);
                }

            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Draws the map canvas.
            /// </summary>
            protected override void DrawMapCanvas()
            {
            }

        }

    }

}
