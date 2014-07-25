//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 14OCT2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System;
using Mapdigit.Drawing.Geometry;
using Mapdigit.Drawing.Core;

//--------------------------------- PACKAGE ------------------------------------

namespace Mapdigit.Drawing
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 14OCT2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// This Graphics2D class provides more sophisticated control over geometry,
    /// coordinate transformations, color management, and text layout.
    /// </summary>
    /// <remarks>
    ///  This is the fundamental class for rendering 2-dimensional shapes, text and images
    /// </remarks>
    public sealed class Graphics2D
    {
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructor. create a graphics object with given width and height
        /// </summary>
        /// <param name="width">the width of the graphics 2d object..</param>
        /// <param name="height">the height of the graphics 2d object.</param>
        public Graphics2D(int width, int height)
        {
            _graphicsFp = new GraphicsFP(width, height);
            _defaultPen = new Pen(Color.Black);
            _defaultBrush = new SolidBrush(Color.White);
            _graphicsWidth = width;
            _graphicsHeight = height;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Resets this graphics with default pen, color,brush etc.
        /// </summary>
        public void Reset()
        {
            _graphicsFp.SetMatrix(new MatrixFP());
            _defaultPen = new Pen(Color.Black);
            _defaultBrush = new SolidBrush(Color.White);
            _graphicsFp.SetClip(0, 0, _graphicsWidth, _graphicsHeight);
            SetGraphicsFpPenAttribute(_defaultPen);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// get the height of the graphics object.
        /// </summary>
        public int Width
        {
            get { return _graphicsWidth; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// get the height of the graphics object.
        /// </summary>
        public int Height
        {
            get { return _graphicsHeight; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the shape's outline with given pen.
        /// </summary>
        /// <param name="pen">pen used to draw the shape, only the pen's width have
        /// the effect on the shape's outline.</param>
        /// <param name="shape">the input shape.</param>
        /// <returns>the outline shape if draw with given pen.</returns>
        public static IShape GetOutline(Pen pen, IShape shape)
        {
            if (pen != null)
            {
                PenFP penFP = new PenFP(pen._color.Argb)
                                  {
                                      EndCap = pen._cap,
                                      StartCap = pen._cap,
                                      LineJoin = pen._join,
                                      Width = pen._width << SingleFP.DecimalBits,
                                      Brush =
                                          pen._brush != null
                                              ? pen._brush._wrappedBrushFp
                                              : new SolidBrushFP(pen._color._value)
                                  };
                if (pen._dash != null)
                {
                    penFP.DashArray = new int[pen._dash.Length - pen._dashPhase];
                    for (int i = 0; i < pen._dash.Length - pen._dashPhase; i++)
                    {
                        penFP.DashArray[i] =
                            pen._dash[i - pen._dashPhase]
                            << SingleFP.DecimalBits;
                    }
                }
                else
                {
                    penFP.DashArray = null;
                }

                PathIterator pathIterator = shape.GetPathIterator(null);
                int[] coords = new int[6];
                GraphicsPathFP graphicsPathFP = new GraphicsPathFP();
                PointFP pointFP1 = new PointFP();
                PointFP pointFPCtl1 = new PointFP();
                PointFP pointFPCtl2 = new PointFP();

                while (!pathIterator.IsDone())
                {
                    int type = pathIterator.CurrentSegment(coords);
                    switch (type)
                    {
                        case PathIterator.SegMoveto:
                            pointFP1.Reset(coords[0] << SingleFP.DecimalBits,
                                           coords[1] << SingleFP.DecimalBits);
                            graphicsPathFP.AddMoveTo(pointFP1);
                            break;
                        case PathIterator.SegClose:
                            graphicsPathFP.AddClose();
                            break;
                        case PathIterator.SegLineto:
                            pointFP1.Reset(coords[0] << SingleFP.DecimalBits,
                                           coords[1] << SingleFP.DecimalBits);
                            graphicsPathFP.AddLineTo(pointFP1);
                            break;
                        case PathIterator.SegQuadto:
                            pointFPCtl1.Reset(coords[0] << SingleFP.DecimalBits,
                                              coords[1] << SingleFP.DecimalBits);
                            pointFP1.Reset(coords[2] << SingleFP.DecimalBits,
                                           coords[3] << SingleFP.DecimalBits);
                            graphicsPathFP.AddQuadTo(pointFPCtl1, pointFP1);
                            break;
                        case PathIterator.SegCubicto:
                            pointFPCtl1.Reset(coords[0] << SingleFP.DecimalBits,
                                              coords[1] << SingleFP.DecimalBits);
                            pointFPCtl2.Reset(coords[2] << SingleFP.DecimalBits,
                                              coords[3] << SingleFP.DecimalBits);
                            pointFP1.Reset(coords[4] << SingleFP.DecimalBits,
                                           coords[5] << SingleFP.DecimalBits);
                            graphicsPathFP.AddCurveTo(pointFPCtl1, pointFPCtl2, pointFP1);
                            break;
                    }
                    pathIterator.Next();
                }

                if (penFP.DashArray != null)
                {
                    PenFP newlineStyle = new PenFP(penFP.Brush, penFP.Width,
                                                   PenFP.LinecapButt, PenFP.LinecapButt, PenFP.LinejoinMiter)
                                             {DashArray = penFP.DashArray};

                    GraphicsPathDasherFP dasher = new GraphicsPathDasherFP(graphicsPathFP,
                                                                           newlineStyle.DashArray, 0);
                    GraphicsPathFP newPath = dasher.GetDashedGraphicsPath();
                    graphicsPathFP = newPath;
                }
                GraphicsPathFP outline = graphicsPathFP.CalcOutline(penFP);
                return Union(outline.ToPath());
            }
            return null;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Strokes the outline of a IShape using the settings of the current
        /// Graphics2D context.
        /// </summary>
        /// <param name="pen">the pen used to stroke the shape.</param>
        /// <param name="shape"> the IShape to be rendered.</param>
        public void Draw(Pen pen, IShape shape)
        {
            SetGraphicsFpPenAttribute(pen);
            PathIterator pathIterator = shape.GetPathIterator(null);
            int[] coords = new int[6];
            GraphicsPathFP graphicsPathFP = new GraphicsPathFP();
            PointFP pointFP1 = new PointFP();
            PointFP pointFPCtl1 = new PointFP();
            PointFP pointFPCtl2 = new PointFP();

            while (!pathIterator.IsDone())
            {
                int type = pathIterator.CurrentSegment(coords);
                switch (type)
                {
                    case PathIterator.SegMoveto:
                        pointFP1.Reset(coords[0] << SingleFP.DecimalBits,
                                       coords[1] << SingleFP.DecimalBits);
                        graphicsPathFP.AddMoveTo(pointFP1);
                        break;
                    case PathIterator.SegClose:
                        graphicsPathFP.AddClose();
                        break;
                    case PathIterator.SegLineto:
                        pointFP1.Reset(coords[0] << SingleFP.DecimalBits,
                                       coords[1] << SingleFP.DecimalBits);
                        graphicsPathFP.AddLineTo(pointFP1);
                        break;
                    case PathIterator.SegQuadto:
                        pointFPCtl1.Reset(coords[0] << SingleFP.DecimalBits,
                                          coords[1] << SingleFP.DecimalBits);
                        pointFP1.Reset(coords[2] << SingleFP.DecimalBits,
                                       coords[3] << SingleFP.DecimalBits);
                        graphicsPathFP.AddQuadTo(pointFPCtl1, pointFP1);
                        break;
                    case PathIterator.SegCubicto:
                        pointFPCtl1.Reset(coords[0] << SingleFP.DecimalBits,
                                          coords[1] << SingleFP.DecimalBits);
                        pointFPCtl2.Reset(coords[2] << SingleFP.DecimalBits,
                                          coords[3] << SingleFP.DecimalBits);
                        pointFP1.Reset(coords[4] << SingleFP.DecimalBits,
                                       coords[5] << SingleFP.DecimalBits);
                        graphicsPathFP.AddCurveTo(pointFPCtl1, pointFPCtl2, pointFP1);
                        break;
                }
                pathIterator.Next();
            }
            lock (_graphicsFp)
            {
                _graphicsFp.DrawPath(graphicsPathFP);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Strokes the outline of a IShape using the settings of the current
        /// Graphics2D context.
        /// </summary>
        /// <param name="brush">the brush used to fill the shape.</param>
        /// <param name="shape">the IShape to be rendered.</param>
        public void Fill(Brush brush, IShape shape)
        {
            if (brush != null)
            {
                _graphicsFp.SetBrush(brush._wrappedBrushFp);
                _defaultBrush = brush;
            }

            PathIterator pathIterator = shape.GetPathIterator(null);
            int[] coords = new int[6];
            GraphicsPathFP graphicsPathFP = new GraphicsPathFP();
            PointFP pointFP1 = new PointFP();
            PointFP pointFPCtl1 = new PointFP();
            PointFP pointFPCtl2 = new PointFP();

            while (!pathIterator.IsDone())
            {
                int type = pathIterator.CurrentSegment(coords);
                switch (type)
                {
                    case PathIterator.SegMoveto:
                        pointFP1.Reset(coords[0] << SingleFP.DecimalBits,
                                       coords[1] << SingleFP.DecimalBits);
                        graphicsPathFP.AddMoveTo(pointFP1);
                        break;
                    case PathIterator.SegClose:
                        graphicsPathFP.AddClose();
                        break;
                    case PathIterator.SegLineto:
                        pointFP1.Reset(coords[0] << SingleFP.DecimalBits,
                                       coords[1] << SingleFP.DecimalBits);
                        graphicsPathFP.AddLineTo(pointFP1);
                        break;
                    case PathIterator.SegQuadto:
                        pointFPCtl1.Reset(coords[0] << SingleFP.DecimalBits,
                                          coords[1] << SingleFP.DecimalBits);
                        pointFP1.Reset(coords[2] << SingleFP.DecimalBits,
                                       coords[3] << SingleFP.DecimalBits);
                        graphicsPathFP.AddQuadTo(pointFPCtl1, pointFP1);
                        break;
                    case PathIterator.SegCubicto:
                        pointFPCtl1.Reset(coords[0] << SingleFP.DecimalBits,
                                          coords[1] << SingleFP.DecimalBits);
                        pointFPCtl2.Reset(coords[2] << SingleFP.DecimalBits,
                                          coords[3] << SingleFP.DecimalBits);
                        pointFP1.Reset(coords[4] << SingleFP.DecimalBits,
                                       coords[5] << SingleFP.DecimalBits);
                        graphicsPathFP.AddCurveTo(pointFPCtl1, pointFPCtl2, pointFP1);
                        break;
                }
                pathIterator.Next();
            }
            lock (_graphicsFp)
            {
                _graphicsFp.FillPath(graphicsPathFP);
            }
        }

        ///<summary>
        /// Draws the specified characters using the current font and color.
        /// The offset and length parameters must specify a valid range of characters
        /// within the character array data. The offset parameter must be within the
        /// range [0..(data.length)], inclusive
        ///</summary>
        ///<param name="font">font object</param>
        ///<param name="fontSize"> the size of the font</param>
        ///<param name="data">the array of characters to be drawn</param>
        ///<param name="x">x coordinate of the anchor point.</param>
        ///<param name="y">y coordinate of the anchor point.</param>
        public void DrawChars(FontEx font, int fontSize, char[] data,
           int x, int y)
        {
            DrawChars(font, fontSize, data, 0, data.Length, x, y, FontEx.TextDirLr);
        }

        ///<summary>
        /// Draws the specified characters using the current font and color.
        /// The offset and length parameters must specify a valid range of characters
        /// within the character array data. The offset parameter must be within the
        /// range [0..(data.length)], inclusive
        ///</summary>
        ///<param name="font">the font object</param>
        ///<param name="fontSize">the size of the font</param>
        ///<param name="data">the array of characters to be drawn.</param>
        ///<param name="offset">the start offset in the data</param>
        ///<param name="length">the number of characters to be drawn</param>
        ///<param name="x">x coordinate of the anchor point.</param>
        ///<param name="y">y coordinate of the anchor point.</param>
        public void DrawChars(FontEx font, int fontSize, char[] data, int offset,
            int length, int x, int y)
        {
            DrawChars(font, fontSize, data, offset, length, x, y, FontEx.TextDirLr);
        }

        ///<summary>
        /// Draws the specified characters using the current font and color.
        /// The offset and length parameters must specify a valid range of characters
        /// within the character array data. The offset parameter must be within the
        /// range [0..(data.length)], inclusive
        ///</summary>
        ///<param name="font">the font object</param>
        ///<param name="fontSize">the size of the font</param>
        ///<param name="data">the array of characters to be drawn.</param>
        ///<param name="offset">the start offset in the data</param>
        ///<param name="length">the number of characters to be drawn</param>
        ///<param name="x">x coordinate of the anchor point.</param>
        ///<param name="y">y coordinate of the anchor point.</param>
        ///<param name="tdir">text direction</param>
        public void DrawChars(FontEx font, int fontSize, char[] data, int offset,
           int length, int x, int y, int tdir)
        {
            DrawChars(font, DefaultBrush, DefaultPen, fontSize, data, offset,
                    length, x, y, tdir);
        }

        ///<summary>
        /// Draws the specified characters using the current font and color.
        /// The offset and length parameters must specify a valid range of characters
        /// within the character array data. The offset parameter must be within the
        /// range [0..(data.length)], inclusive
        ///</summary>
        ///<param name="font">the font object</param>
        ///<param name="pen">pen object</param>
        ///<param name="fontSize">the size of the font</param>
        ///<param name="data">the array of characters to be drawn.</param>
        ///<param name="offset">the start offset in the data</param>
        ///<param name="length">the number of characters to be drawn</param>
        ///<param name="x">x coordinate of the anchor point.</param>
        ///<param name="y">y coordinate of the anchor point.</param>
        ///<param name="brush">brush object</param>
        ///<param name="tdir">text direction</param>
        public void DrawChars(FontEx font, Brush brush, Pen pen, int fontSize,
           char[] data, int offset,
           int length, int x, int y, int tdir)
        {
            AffineTransform transfrom = new AffineTransform();
            transfrom.Translate(x, y);
            IShape[] shapes = font.GetGlyphArray(fontSize, data, offset, length,
                    tdir, transfrom);
            for (int j = 0; j < shapes.Length; j++)
            {
                if (shapes[j] != null)
                {
                    if (brush != null)
                    {
                        Fill(brush, shapes[j]);
                    }
                    if (pen != null)
                    {
                        Draw(pen, shapes[j]);
                    }
                }
            }

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws a line between the points (x1, y1) and (x2, y2).
        /// </summary>
        /// <param name="pen">pen used to draw the line.</param>
        /// <param name="x1">the first point's x coordinate.</param>
        /// <param name="y1">the first point's y coordinate.</param>
        /// <param name="x2">the second point's x coordinate.</param>
        /// <param name="y2">the second point's y coordinate.</param>
        public void DrawLine(Pen pen, int x1, int y1, int x2, int y2)
        {
            SetGraphicsFpPenAttribute(pen);
            lock (_graphicsFp)
            {
                _graphicsFp.DrawLine(x1 << SingleFP.DecimalBits, y1 << SingleFP.DecimalBits,
                                     x2 << SingleFP.DecimalBits, y2 << SingleFP.DecimalBits);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws a line between the points pt1 and pt2.
        /// </summary>
        /// <param name="pen">pen used to draw the line</param>
        /// <param name="pt1">the first point.</param>
        /// <param name="pt2">the second point.</param>
        public void DrawLine(Pen pen, Point pt1, Point pt2)
        {
            DrawLine(pen, pt1.X, pt1.Y, pt2.X, pt2.Y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// the default pen of the graphics
        /// </summary>
        public Pen DefaultPen
        {
            set
            {
                _defaultPen = value;
                SetGraphicsFpPenAttribute(value);
            }
            get { return _defaultPen; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set the default pen and brush together of the graphics
        /// </summary>
        /// <param name="pen">default pen to be used by the graphcis if any draw method's
        ///  pen set to null.</param>
        /// <param name="brush">default brush to be used by the graphics.</param>
        public void SetPenAndBrush(Pen pen, Brush brush)
        {
            DefaultPen = pen;
            if (brush != null)
            {
                lock (_graphicsFp)
                {
                    _graphicsFp.SetBrush(brush._wrappedBrushFp);
                }
                _defaultBrush = brush;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// the default brush of the graphics
        /// </summary>
        public Brush DefaultBrush
        {
            set
            {
                if (value != null)
                {
                    lock (_graphicsFp)
                    {
                        _graphicsFp.SetBrush(value._wrappedBrushFp);
                    }
                    _defaultBrush = value;
                }
            }
            get { return _defaultBrush; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draw a rectangle with given pen
        /// </summary>
        /// <param name="pen"> pen used to draw the rectangle.</param>
        /// <param name="rectangle">rectangle to be drawn..</param>
        public void DrawRectangle(Pen pen, Rectangle rectangle)
        {
            Draw(pen, rectangle);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Fill a rectangle with given brush
        /// </summary>
        /// <param name="brush"> brush used to fill the rectangle..</param>
        /// <param name="rectangle">rectangle to be filled..</param>
        public void FillRectangle(Brush brush, Rectangle rectangle)
        {
            Fill(brush, rectangle);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the outline of an oval. The result is a circle or ellipse that fits
        /// within the rectangle specified by the x, y, width, and height arguments.
        /// </summary>
        /// <param name="pen">pen used to draw the oval.</param>
        /// <param name="x">the x coordinate of the upper left corner of the oval to be drawn.</param>
        /// <param name="y">the y coordinate of the upper left corner of the oval to be drawn.</param>
        /// <param name="width">the width of the oval to be drawn.</param>
        /// <param name="height">the height of the oval to be drawn.</param>
        public void DrawOval(Pen pen, int x, int y, int width, int height)
        {
            SetGraphicsFpPenAttribute(pen);
            lock (_graphicsFp)
            {
                _graphicsFp.DrawOval(x << SingleFP.DecimalBits,
                                     y << SingleFP.DecimalBits,
                                     (x + width) << SingleFP.DecimalBits,
                                     (y + height) << SingleFP.DecimalBits);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Fills an oval bounded by the specified rectangle with the current color.
        /// </summary>
        /// <param name="brush">the brush used to fill the oval.</param>
        /// <param name="x">the x coordinate of the upper left corner of the oval to be filled.</param>
        /// <param name="y">the y coordinate of the upper left corner of the oval to be filled.</param>
        /// <param name="width">the width of the oval to be filled.</param>
        /// <param name="height">the height of the oval to be filled.</param>
        public void FillOval(Brush brush, int x, int y, int width, int height)
        {
            DefaultBrush = brush;
            lock (_graphicsFp)
            {
                _graphicsFp.FillOval(x << SingleFP.DecimalBits,
                                     y << SingleFP.DecimalBits,
                                     (x + width) << SingleFP.DecimalBits,
                                     (y + height) << SingleFP.DecimalBits);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the polyline.
        /// </summary>
        /// <param name="pen">the pen used to draw the polyline.</param>
        /// <param name="polyline">the polyline to be drawn.</param>
        public void DrawPolyline(Pen pen, Polyline polyline)
        {
            Draw(pen, polyline);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the polygon.
        /// </summary>
        /// <param name="pen">the pen used to draw the polygon.</param>
        /// <param name="polygon">the polygon to be drawn.</param>
        public void DrawPolygon(Pen pen, Polygon polygon)
        {
            Draw(pen, polygon);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Fills the polygon.
        /// </summary>
        /// <param name="brush">the brush used to fill the polygon.</param>
        /// <param name="polygon">the polygon to be filled.</param>
        public void FillPolygon(Brush brush, Polygon polygon)
        {
            Fill(brush, polygon);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// the current transformation matrix from user space to device space.
        /// </summary>
        public AffineTransform AffineTransform
        {
            set
            {
                lock (_graphicsFp)
                {
                    _graphicsFp.SetMatrix(Utils.ToMatrixFP(value));
                }
            }
            get { return Utils.ToMatrix(_graphicsFp.GetMatrix()); }
        }

       
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// the current clip of this graphcis object.
        /// </summary>
        /// <returns>the current clip  rectangle.</returns>
        public Rectangle ClipRect
        {
            get
            {
                return new Rectangle(_graphicsFp.GetClipX(), _graphicsFp.GetClipY(),
                                     _graphicsFp.GetClipWidth(),
                                     _graphicsFp.GetClipHeight());
            }
            set
            {
                SetClip(value.X, value.Y, value.Width, value.Height);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set current clip of this graphcis object.
        /// </summary>
        /// <param name="x">the x coordinate of the top left point..</param>
        /// <param name="y">the y coordinate of the top left point..</param>
        /// <param name="width">the widht of the clip rectangle..</param>
        /// <param name="height">the height of the clip rectangle.</param>
        public void SetClip(int x, int y, int width, int height)
        {
            lock (_graphicsFp)
            {
                _graphicsFp.SetClip(x, y, width, height);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the specified Image object at the specified location and with the
        /// specified size.
        /// </summary>
        /// <param name="imageRgb">Image object to draw.</param>
        /// <param name="width">Width of the portion of the source image to draw.</param>
        /// <param name="height">Height of the portion of the source image to draw.</param>
        /// <param name="dstX">x-coordinate of the upper-left corner of the drawn image.</param>
        /// <param name="dstY">y-coordinate of the upper-left corner of the drawn image</param>
        public void DrawImage(int[] imageRgb, int width, int height,
                              int dstX, int dstY)
        {
            Rectangle rect1 = ClipRect;
            Rectangle rect2 = new Rectangle(dstX,
                                            dstY,
                                            width + dstX,
                                            height + dstY);
            Rectangle rect = rect1.Intersection(rect2);
            if (!rect.IsEmpty())
            {
                width = rect.Width;
                height = rect.Height;

                int[] destBuffer = Argb;
                int desWidth = _graphicsWidth;
                int i;
                for (i = 0; i < height; i++)
                {
                    Array.Copy(imageRgb, i*width, destBuffer,
                               dstX + (i + dstY)*desWidth, width);
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the specified Image object at the specified location and with the
        /// specified size.
        /// </summary>
        /// <param name="imageRgb">The image RGB.</param>
        /// <param name="width">Width of the portion of the source image to draw..</param>
        /// <param name="height">Height of the portion of the source image to draw.</param>
        /// <param name="dstX">x-coordinate of the upper-left corner of the drawn image.</param>
        /// <param name="dstY">y-coordinate of the upper-left corner of the drawn image</param>
        /// <param name="srcX">x-coordinate of the upper-left corner of the source image</param>
        /// <param name="srcY">y-coordinate of the upper-left corner of the source image.</param>
        /// <param name="dstWidth">Width of the destination image.</param>
        /// <param name="dstHeight">Height of the destination image.</param>
        public void DrawImage(int[] imageRgb, int width, int height,
                              int dstX, int dstY, int srcX, int srcY, int dstWidth, int dstHeight)
        {
            int[] tempRgb = imageRgb;
            dstWidth = Math.Min(dstWidth, width - srcX);
            dstHeight = Math.Min(dstHeight, height - srcY);
            if (dstWidth != width || dstHeight != height)
            {
                tempRgb = new int[dstWidth*dstHeight];
                for (int i = 0; i < dstHeight; i++)
                {
                    Array.Copy(imageRgb, (i + srcY)*width + srcX, tempRgb,
                               i*dstWidth, dstWidth);
                }
            }
            DrawImage(tempRgb, dstWidth, dstHeight, dstX, dstY);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the specified Image object at the specified location and with the
        /// specified size.
        /// </summary>
        /// <param name="imageRgb">Image object to draw.</param>
        /// <param name="width">Width of the portion of the source image to draw..</param>
        /// <param name="height">Height of the portion of the source image to draw.</param>
        /// <param name="dstX">x-coordinate of the upper-left corner of the drawn image.</param>
        /// <param name="dstY">y-coordinate of the upper-left corner of the drawn image</param>
        /// <param name="transpency">specify the transparent color of the image.</param>
        public void DrawImage(int[] imageRgb, int width, int height,
                              int dstX, int dstY,
                              int transpency)
        {
            Rectangle rect1 = ClipRect;
            Rectangle rect2 = new Rectangle(dstX,
                                            dstY,
                                            width + dstX,
                                            height + dstY);
            Rectangle rect = rect1.Intersection(rect2);
            if (!rect.IsEmpty())
            {
                int[] destBuffer = Argb;
                int desWidth = _graphicsWidth;
                for ( int i = 0; i < width; i++)
                {
                    int j;
                    for (j = 0; j < height; j++)
                    {
                        if (((dstX + i) < _graphicsWidth) && ((dstY + j) < _graphicsHeight) &&
                            dstX + i >= 0 && dstY + j >= 0)
                        {
                            if ((imageRgb[i + j*width] & 0x00ffffff)
                                != (transpency & 0x00ffffff))
                            {
                                destBuffer[dstX + i + (j + dstY)*desWidth]
                                    = imageRgb[i + j*width];
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
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the transparent Image object at the specified location and with the
        /// specified size
        /// </summary>
        /// <param name="imageRgb">Image object to draw.</param>
        /// <param name="width">Width of the portion of the source image to draw..</param>
        /// <param name="height">Height of the portion of the source image to draw.</param>
        /// <param name="dstX">x-coordinate of the upper-left corner of the drawn image.</param>
        /// <param name="dstY">y-coordinate of the upper-left corner of the drawn image</param>
        /// <param name="transpency">specify the transparent color of the image.</param>
        /// <param name="alpha">The new alpha.</param>
        public void DrawImage(int[] imageRgb, int width, int height,
                              int dstX, int dstY,
                              int transpency, int alpha)
        {
            Rectangle rect1 = ClipRect;
            Rectangle rect2 = new Rectangle(dstX,
                                            dstY,
                                            width + dstX,
                                            height + dstY);
            Rectangle rect = rect1.Intersection(rect2);
            if (!rect.IsEmpty())
            {
                int[] destBuffer = Argb;
                int desWidth = _graphicsWidth;
                for (int i = 0; i < width; i++)
                {
                    int j;
                    for (j = 0; j < height; j++)
                    {
                        if (((dstX + i) < _graphicsWidth)
                            && ((dstY + j) < _graphicsHeight) && dstX + i >= 0 && dstY + j >= 0)
                        {
                            if ((imageRgb[i + j*width] & 0x00ffffff)
                                != (transpency & 0x00ffffff))
                            {
                                destBuffer[dstX + i + (j + dstY)*desWidth]
                                    = Merge(destBuffer[dstX + i + (j + dstY)*desWidth],
                                            imageRgb[i + j*width] & ((alpha & 0xff) << 24 | 0xFFFFFF));
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
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Clear the graphicis object with given color.
        /// </summary>
        /// <param name="color">the color used to clear the graphics.</param>
        public void Clear(Color color)
        {
            Clear(color._value);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Clear the graphics content with given color.
        /// </summary>
        /// <param name="color">the clear color.</param>
        public void Clear(int color)
        {
            lock (_graphicsFp)
            {
                _graphicsFp.Clear(color);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the content of this image as ARGB array.
        /// </summary>
        public int[] Argb
        {
            get
            {
                lock (_graphicsFp)
                {
                    return _graphicsFp.GetRGB();
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the <see cref="System.Int32"/> with the specified x.
        /// </summary>
        /// <value></value>
        public int this[int x, int y]
        {
            get { return _graphicsFp.GetRGB()[y*_graphicsWidth + x]; }
        }

        /**
         * the wraped graphicsFP object.
         */
        private readonly GraphicsFP _graphicsFp;
        /**
         * default pen for drawing.
         */
        private Pen _defaultPen;
        /**
         * default brush for filling.
         */
        private Brush _defaultBrush;


        /// <summary>
        /// graphics width
        /// </summary>
        private readonly int _graphicsWidth;

        /// <summary>
        /// graphics height.
        /// </summary>
        private readonly int _graphicsHeight;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Union the shape into single path.
        /// </summary>
        /// <param name="shape">shape the shape object..</param>
        /// <returns>a single path</returns>
        private static IShape Union(IShape shape)
        {
            PathIterator pathIterator = shape.GetPathIterator(null);
            Area area = new Area();
            int[] coords = new int[6];
            Path path = new Path();
            while (!pathIterator.IsDone())
            {
                int type = pathIterator.CurrentSegment(coords);
                switch (type)
                {
                    case PathIterator.SegClose:
                        path.ClosePath();
                        area.Add(new Area(path));
                        path = new Path();
                        break;
                    case PathIterator.SegCubicto:
                        path.CurveTo(coords[0], coords[1], coords[2], coords[3],
                                     coords[4], coords[5]);
                        break;
                    case PathIterator.SegLineto:
                        path.LineTo(coords[0], coords[1]);
                        break;
                    case PathIterator.SegMoveto:
                        path.MoveTo(coords[0], coords[1]);
                        break;
                    case PathIterator.SegQuadto:
                        path.QuadTo(coords[0], coords[1], coords[2], coords[3]);
                        break;
                }

                pathIterator.Next();
            }
            return area;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sset graphics pen attribute.
        /// </summary>
        /// <param name="pen">The pen.</param>
        private void SetGraphicsFpPenAttribute(Pen pen)
        {
            if (pen != null)
            {
                _defaultPen = pen;
                PenFP penFP = _graphicsFp.GetPen();
                penFP.EndCap = pen._cap;
                penFP.StartCap = pen._cap;
                penFP.LineJoin = pen._join;
                penFP.Width = pen._width << SingleFP.DecimalBits;
                penFP.Brush = pen._brush != null ? pen._brush._wrappedBrushFp : new SolidBrushFP(pen._color._value);
                if (pen._dash != null)
                {
                    penFP.DashArray = new int[pen._dash.Length - pen._dashPhase];
                    for (int i = 0; i < pen._dash.Length - pen._dashPhase; i++)
                    {
                        penFP.DashArray[i] =
                            pen._dash[i - pen._dashPhase]
                            << SingleFP.DecimalBits;
                    }
                }
                else
                {
                    penFP.DashArray = null;
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Merges the two colors.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <returns></returns>
        private static int Merge(int color1, int color2)
        {
            int a2 = (color2 >> 24) & 0xFF;
            if (a2 == 0xFF || color1 == 0x0)
            {
                return color2;
            }
            if (a2 == 0)
            {
                return color1;
            }
            int a1 = 0xFF - ((color1 >> 24) & 0xFF);
            int a3 = 0xFF - a2;
            int b1 = color1 & 0xFF;
            int g1 = (color1 >> 8) & 0xFF;
            int r1 = (color1 >> 16) & 0xFF;
            int b2 = color2 & 0xFF;
            int g2 = (color2 >> 8) & 0xFF;
            int r2 = (color2 >> 16) & 0xFF;

            int ca = (0xFF*0xFF - a1*a3) >> 8;
            int cr = (r1*a3 + r2*a2) >> 8;
            int cg = (g1*a3 + g2*a2) >> 8;
            int cb = (b1*a3 + b2*a2) >> 8;
            return ca << 24 | cr << 16 | cg << 8 | cb;
        }
    }
}