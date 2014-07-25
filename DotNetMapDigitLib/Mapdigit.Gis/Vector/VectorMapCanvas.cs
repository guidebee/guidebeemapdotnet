//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 02OCT2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System.Collections;
using Mapdigit.Drawing;
using Mapdigit.Drawing.Geometry;
using Mapdigit.Gis.Drawing;
using Mapdigit.Gis.Geometry;
using Color = Mapdigit.Drawing.Color;
using Pen = Mapdigit.Drawing.Pen;
using Rectangle = Mapdigit.Drawing.Geometry.Rectangle;
using SolidBrush = Mapdigit.Drawing.SolidBrush;
using TextureBrush = Mapdigit.Drawing.TextureBrush;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Gis.Vector
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 02OCT2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// vector map canvas
    /// </summary>
    internal class VectorMapCanvas : VectorMapAbstractCanvas
    {

        /// <summary>
        /// Shared graphics2D instance used to drawing map objects.
        /// </summary>
        private static Graphics2D _sharedGraphics2D;


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the graphics2 D instance.
        /// </summary>
        private static void GetGraphics2DInstance()
        {
            lock (GraphicsMutex)
            {
                if (_sharedGraphics2D == null)
                {
                    _sharedGraphics2D = new Graphics2D(MapLayer.MapTileWidth,
                            MapLayer.MapTileWidth);
                }
            }
            return;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="VectorMapCanvas"/> class.
        /// </summary>
        public VectorMapCanvas()
        {
            GetGraphics2DInstance();
            _textImage = MapLayer.GetAbstractGraphicsFactory()
                    .CreateImage(MapLayer.MapTileWidth,
                    ImagePaternWidth);
            _textGraphics = _textImage.GetGraphics();
            _textGraphics.SetColor(0xC0C0FF);
            _textGraphics.FillRect(0, 0, _textImage.GetWidth(), _textImage.GetHeight());
            _fontTranspency = _textImage.GetRGB()[0];
            _imagePattern = MapLayer.GetAbstractGraphicsFactory()
                    .CreateImage(ImagePaternWidth,
                    ImagePaternWidth);

            _imagePatternGraphics = _imagePattern.GetGraphics();
            
            mapSize.X = 0; mapSize.Y = 0;
            mapSize.MaxX = MapLayer.MapTileWidth;
            mapSize.MaxY = MapLayer.MapTileWidth;
            mapSize.Width = MapLayer.MapTileWidth;
            mapSize.Height = MapLayer.MapTileWidth;
        }



        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the RGB.
        /// </summary>
        /// <returns></returns>
        public override int[] GetRGB()
        {
            if (_sharedGraphics2D != null)
            {
                return _sharedGraphics2D.Argb;
            }
            return null;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the point.
        /// </summary>
        /// <param name="mapPoint">The map point.</param>
        private void DrawPoint(MapPoint mapPoint)
        {
            GeoPoint screenPt = FromLatLngToMapPixel(mapPoint.Point);
            SolidBrush brush = new SolidBrush(new Color(mapPoint.SymbolType.Color,false));
            _sharedGraphics2D.FillRectangle(brush, new Rectangle((int)screenPt.X - 2,
                    (int)screenPt.Y - 2, 4, 4));

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the pline.
        /// </summary>
        /// <param name="mapPen">The map pen.</param>
        /// <param name="pline">The pline.</param>
        private void DrawPline(MapPen mapPen, GeoPolyline pline)
        {
            ArrayList clippedPts = sutherlandHodgman.ClipPline(pline.GetPoints());
            GeoPoint[] screenPts = FromLatLngToMapPixel(clippedPts);
            if (screenPts.Length > 1)
            {
                {
                    int penWidth = mapPen.Width;
                    if (mapPen.Pattern > 62)
                    {
                        penWidth = mapPen.Width * 2;
                    }
                    Pen pen = new Pen(new Color(mapPen.Color,false), penWidth);
                    _sharedGraphics2D.DefaultPen = pen;
                    int[] xpoints = new int[screenPts.Length];
                    int[] ypoints = new int[screenPts.Length];
                    for (int i = 0; i < screenPts.Length; i++)
                    {
                        xpoints[i] = (int)screenPts[i].X;
                        ypoints[i] = (int)screenPts[i].Y;

                    }
                    Polyline polyline = new Polyline
                                            {
                                                XPoints = xpoints,
                                                YPoints = ypoints,
                                                NumOfPoints = xpoints.Length
                                            };

                    _sharedGraphics2D.DrawPolyline(null, polyline);
                }

            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the region.
        /// </summary>
        /// <param name="mapPen">The map pen.</param>
        /// <param name="mapBrush">The map brush.</param>
        /// <param name="region">The region.</param>
        private void DrawRegion(MapPen mapPen, MapBrush mapBrush, GeoPolygon region)
        {
            Pen pen = new Pen(new Color(mapPen.Color,false), mapPen.Width);
            TextureBrush brush = GetImagePatternBrush(mapBrush);
            ArrayList clippedPts = sutherlandHodgman.ClipRegion(region.GetPoints());
            GeoPoint[] screenPts = FromLatLngToMapPixel(clippedPts);

            if (screenPts.Length > 2)
            {
                {
                    int[] xpoints = new int[screenPts.Length];
                    int[] ypoints = new int[screenPts.Length];
                    for (int i = 0; i < screenPts.Length; i++)
                    {
                        xpoints[i] = (int)screenPts[i].X;
                        ypoints[i] = (int)screenPts[i].Y;

                    }
                    Polygon polygon = new Polygon
                                          {
                                              XPoints = xpoints,
                                              YPoints = ypoints,
                                              NumOfPoints = xpoints.Length
                                          };

                    if (mapBrush.Pattern == 2)
                    {
                        _sharedGraphics2D.SetPenAndBrush(pen, brush);
                        _sharedGraphics2D.DrawPolygon(null, polygon);
                        _sharedGraphics2D.FillPolygon(null, polygon);
                    }
                    else
                    {
                        _sharedGraphics2D.DefaultPen = pen;
                        _sharedGraphics2D.DrawPolygon(null, polygon);
                    }
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the text.
        /// </summary>
        /// <param name="mapText">The map text.</param>
        private void DrawText(MapText mapText)
        {
            {
                fontColor = mapText.ForeColor;
                DrawString(font, mapText.TextString,
                        (int)mapText.Point.X,
                        (int)mapText.Point.Y);
            }
        }



        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the image pattern brush.
        /// </summary>
        /// <param name="brush">The brush.</param>
        /// <returns></returns>
        private TextureBrush GetImagePatternBrush(MapBrush brush)
        {
            switch (brush.Pattern)
            {
                case 1:
                    break;
                case 2:
                    _imagePatternGraphics.SetColor(brush.ForeColor);
                    _imagePatternGraphics.FillRect( 0, 0, ImagePaternWidth,
                            ImagePaternWidth);
                    break;
                case 3:
                case 19:
                case 20:
                case 21:
                case 22:
                case 23:
                    _imagePatternGraphics.SetColor(brush.BackColor);
                    _imagePatternGraphics.FillRect( 0, 0, ImagePaternWidth,
                            ImagePaternWidth);

                    for (int i = 0; i < 4; i++)
                    {
                        _imagePatternGraphics.DrawLine(0, i * 4, ImagePaternWidth, i * 4);
                    }
                    break;
                case 4:
                case 24:
                case 25:
                case 26:
                case 27:
                case 28:
                    _imagePatternGraphics.SetColor(brush.BackColor);
                    _imagePatternGraphics.FillRect( 0, 0, ImagePaternWidth,
                            ImagePaternWidth);
                    _imagePatternGraphics.SetColor(brush.ForeColor);
                    for (int i = 0; i < 4; i++)
                    {
                        _imagePatternGraphics.DrawLine(i * 4, 0, i * 4, ImagePaternWidth);
                    }
                    break;
                case 5:
                case 29:
                case 30:
                case 31:
                case 32:
                case 33:
                    _imagePatternGraphics.SetColor(brush.BackColor);
                    _imagePatternGraphics.FillRect( 0, 0, ImagePaternWidth,
                           ImagePaternWidth);
                    _imagePatternGraphics.SetColor(brush.ForeColor);
                    for (int i = 0; i < 8; i++)
                    {
                        _imagePatternGraphics.DrawLine(0, i * 4, i * 4, 0);
                    }
                    break;
                case 6:
                case 34:
                case 35:
                case 36:
                case 37:
                case 38:
                    _imagePatternGraphics.SetColor(brush.BackColor);
                    _imagePatternGraphics.FillRect(0, 0, ImagePaternWidth,
                           ImagePaternWidth);
                    _imagePatternGraphics.SetColor(brush.ForeColor);
                    for (int i = 0; i < 8; i++)
                    {
                        _imagePatternGraphics.DrawLine(0, ImagePaternWidth - i * 4, i * 4, 0);
                    }
                    break;
                case 15:
                case 16:
                case 17:
                case 18:
                case 48:
                case 49:
                case 50:
                case 51:
                case 53:
                    _imagePatternGraphics.SetColor(brush.BackColor);
                    _imagePatternGraphics.FillRect(0, 0, ImagePaternWidth,
                           ImagePaternWidth);
                    _imagePatternGraphics.SetColor(brush.ForeColor);
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            _imagePatternGraphics.FillRect(i * 4, j * 4, 1, 1);
                        }
                    }
                    break;
                default:
                    _imagePatternGraphics.SetColor(brush.BackColor);
                    _imagePatternGraphics.FillRect(0, 0, ImagePaternWidth,
                           ImagePaternWidth);
                    _imagePatternGraphics.SetColor(brush.ForeColor);
                    for (int i = 0; i < 4; i++)
                    {
                        _imagePatternGraphics.DrawLine(0, i * 4, ImagePaternWidth, i * 4);
                        _imagePatternGraphics.DrawLine(i * 4, 0, i * 4, ImagePaternWidth);
                    }


                    break;

            }

            int[] rgbData = _imagePattern.GetRGB();

            TextureBrush textureBrush = new TextureBrush(rgbData, ImagePaternWidth, ImagePaternWidth);


            return textureBrush;
        }



        

        /**
         * pattern Image;
         */
        private readonly IImage _imagePattern;
        /**
         * draw the image pattern.
         */
        private readonly IGraphics _imagePatternGraphics;
        /**
         * used to Show text on the image.
         */
        private readonly ArrayList _mapNameHolder = new ArrayList();


        /**
         * image used to draw char with system fonts.
         */
        private readonly IImage _textImage;

        /**
         * graphics used to draw char with system fonts.
         */
        private readonly IGraphics _textGraphics;


        private readonly int _fontTranspency;
        /**
         * defautl imagePattern size;
         */
        private const int ImagePaternWidth = 24;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the map object.
        /// </summary>
        /// <param name="mapObject">The map object.</param>
        /// <param name="drawBoundary">The draw boundary.</param>
        /// <param name="zoomLevel">The zoom level.</param>
        public override void DrawMapObject(MapObject mapObject, GeoLatLngBounds drawBoundary,
                int zoomLevel)
        {
            GeoLatLng drawPt = new GeoLatLng();
            sutherlandHodgman = new SutherlandHodgman(drawBoundary);
            mapZoomLevel = zoomLevel;
            mapCenterPt.X = drawBoundary.GetCenterX();
            mapCenterPt.Y = drawBoundary.GetCenterY();
            bool pointFound = false;
            switch (mapObject.MapObjectType)
            {
                case MapObject.TypeNone:
                    break;
                case MapObject.TypePoint:
                    {
                        MapPoint mapPoint = (MapPoint)mapObject;
                        DrawPoint(mapPoint);
                        drawPt.X = mapPoint.Point.X;
                        drawPt.Y = mapPoint.Point.Y;
                        pointFound = true;
                    }
                    break;
                case MapObject.TypeMultiPoint:
                    {
                        MapMultiPoint mapMultiPoint = (MapMultiPoint)mapObject;
                        for (int i = 0; i < mapMultiPoint.Points.Length; i++)
                        {
                            MapPoint mapPoint = new MapPoint
                                                    {
                                                        SymbolType = mapMultiPoint.SymbolType,
                                                        Point = new GeoLatLng(mapMultiPoint.Points[i])
                                                    };
                            DrawPoint(mapPoint);
                        }
                        for (int i = 0; i < mapMultiPoint.Points.Length; i++)
                        {
                            if (drawBoundary.Contains(mapMultiPoint.Points[i]))
                            {
                                drawPt.X = mapMultiPoint.Points[i].X;
                                drawPt.Y = mapMultiPoint.Points[i].Y;
                                pointFound = true;
                                break;
                            }
                        }

                    }
                    break;
                case MapObject.TypePline:
                    {
                        MapPline mapPline = (MapPline)mapObject;
                        DrawPline(mapPline.PenStyle, mapPline.Pline);
                        for (int i = 0; i < mapPline.Pline.GetVertexCount(); i++)
                        {
                            if (drawBoundary.Contains(mapPline.Pline.GetVertex(i)))
                            {
                                drawPt.X = mapPline.Pline.GetVertex(i).X;
                                drawPt.Y = mapPline.Pline.GetVertex(i).Y;
                                pointFound = true;
                                break;
                            }
                        }
                    }
                    break;
                case MapObject.TypeMultiPline:
                    {
                        MapMultiPline mapMultiPline = (MapMultiPline)mapObject;
                        for (int i = 0; i < mapMultiPline.Plines.Length; i++)
                        {
                            DrawPline(mapMultiPline.PenStyle,
                                    mapMultiPline.Plines[i]);
                            for (int j = 0; j < mapMultiPline.Plines[i].GetVertexCount(); j++)
                            {
                                if (drawBoundary.Contains(mapMultiPline.Plines[i].GetVertex(j)))
                                {
                                    drawPt.X = mapMultiPline.Plines[i].GetVertex(j).X;
                                    drawPt.Y = mapMultiPline.Plines[i].GetVertex(j).Y;
                                    pointFound = true;
                                    break;
                                }
                            }
                        }
                    }
                    break;
                case MapObject.TypeReginRegion:
                    {
                        MapRegion mapRegion = (MapRegion)mapObject;
                        DrawRegion(mapRegion.PenStyle, mapRegion.BrushStyle,
                                mapRegion.Region);
                        drawPt.X = mapRegion.CenterPt.X;
                        drawPt.Y = mapRegion.CenterPt.Y;
                        pointFound = true;
                    }
                    break;
                case MapObject.TypeMultiRegion:
                    {
                        MapMultiRegion mapMultiRegion = (MapMultiRegion)mapObject;
                        for (int i = 0; i < mapMultiRegion.Regions.Length; i++)
                        {
                            DrawRegion(mapMultiRegion.PenStyle,
                                    mapMultiRegion.BrushStyle,
                                    mapMultiRegion.Regions[i]);

                        }
                        drawPt.X = mapMultiRegion.CenterPt.X;
                        drawPt.Y = mapMultiRegion.CenterPt.Y;
                        pointFound = true;
                    }
                    break;
                case MapObject.TypeCollection:
                    {
                        MapCollection mapCollection = (MapCollection)mapObject;
                        if (mapCollection.MultiRegion != null)
                        {
                            MapMultiRegion mapMultiRegion = mapCollection.MultiRegion;
                            for (int i = 0; i < mapMultiRegion.Regions.Length; i++)
                            {
                                DrawRegion(mapMultiRegion.PenStyle,
                                        mapMultiRegion.BrushStyle,
                                        mapMultiRegion.Regions[i]);
                            }
                        }
                        if (mapCollection.MultiPline != null)
                        {
                            MapMultiPline mapMultiPline = mapCollection.MultiPline;
                            for (int i = 0; i < mapMultiPline.Plines.Length; i++)
                            {
                                DrawPline(mapMultiPline.PenStyle,
                                        mapMultiPline.Plines[i]);
                            }
                        }
                        if (mapCollection.MultiPoint != null)
                        {
                            MapMultiPoint mapMultiPoint = mapCollection.MultiPoint;
                            for (int i = 0; i < mapMultiPoint.Points.Length; i++)
                            {
                                MapPoint mapPoint = new MapPoint
                                                        {
                                                            SymbolType = mapMultiPoint.SymbolType,
                                                            Point = new GeoLatLng(mapMultiPoint.Points[i])
                                                        };
                                DrawPoint(mapPoint);
                            }
                        }
                        pointFound = true;
                        drawPt.X = mapCollection.Bounds.X + mapCollection.Bounds.Width / 2;
                        drawPt.Y = mapCollection.Bounds.Y + mapCollection.Bounds.Height / 2;

                    }
                    break;
                case MapObject.TypeText:
                    {
                        MapText mapText = (MapText)mapObject;
                        drawPt.X = mapText.Point.X;
                        drawPt.Y = mapText.Point.Y;
                        pointFound = true;
                    }
                    break;
            }
            if (!mapObject.Name.ToLower().Equals("unknown") && pointFound)
            {
                MapText mapName = new MapText {Font = font};
                mapName.SetForeColor(fontColor);
                mapName.TextString = mapObject.Name;
                GeoPoint screenPt = FromLatLngToMapPixel(drawPt);
                mapName.Point.X = screenPt.X;
                mapName.Point.Y = screenPt.Y;
                mapName.Bounds.X = mapName.Point.X;
                mapName.Bounds.Y = mapName.Point.Y;
                if (font != null)
                {
                    mapName.Bounds.Height = ImagePaternWidth;
                    mapName.Bounds.Width = font.CharsWidth(mapObject.Name.ToCharArray(), 0,
                            mapObject.Name.ToCharArray().Length);

                }
                AddMapName(mapName);

            }

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Clears the canvas.
        /// </summary>
        /// <param name="color">The color.</param>
        public override void ClearCanvas(int color)
        {
            if (_sharedGraphics2D != null)
            {
                _sharedGraphics2D.Clear(new Color(color));
                _mapNameHolder.Clear();
            }
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the map text.
        /// </summary>
        public override void DrawMapText()
        {
            for (int i = 0; i < _mapNameHolder.Count; i++)
            {
                MapText mapText = (MapText)_mapNameHolder[i];
                if (font != null)
                {
                    DrawText(mapText);
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Adds the name of the map.
        /// </summary>
        /// <param name="mapText">The map text.</param>
        private void AddMapName(MapText mapText)
        {
            GeoLatLngBounds mapTextBounds = mapText.Bounds;
            for (int i = 0; i < _mapNameHolder.Count; i++)
            {
                GeoLatLngBounds storedMapTextBounds =
                        ((MapText)_mapNameHolder[i]).Bounds;
                if (storedMapTextBounds.Intersects(mapTextBounds))
                {
                    return;
                }
            }
            if (mapSize.Contains(mapTextBounds))
            {
                _mapNameHolder.Add(mapText);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the string.
        /// </summary>
        /// <param name="fontType">The font.</param>
        /// <param name="str">The STR.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        private void DrawString(IFont fontType, string str, int x, int y)
        {
            lock (_textGraphics)
            {
                _textGraphics.SetColor(_fontTranspency);
                _textGraphics.FillRect(0, 0, _textImage.GetWidth(), _textImage.GetHeight());
                _textGraphics.SetFont(fontType);
                _textGraphics.SetColor(fontColor);
                _textGraphics.DrawString(str, 0, 0);
                _sharedGraphics2D.DrawImage(_textImage.GetRGB(), MapLayer.MapTileWidth,
                        ImagePaternWidth, x, y, _fontTranspency);
            }
        }


    }

}
