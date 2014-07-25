//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 24SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Drawing.Core
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 24SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///   Encapsulates a 2D drawing surface.
    /// </summary>
    internal sealed class GraphicsFP
    {
        /**
         * draw mode , XOR
         */
        public const int ModeXor = GraphicsPathRendererFP.ModeXor;

        /**
         * draw mode. nothing.
         */
        public const int ModeZero = GraphicsPathRendererFP.ModeZero;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsFP"/> class.
        /// </summary>
        public GraphicsFP()
        {
            InitBlock();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Constructor. create a graphics object with given width and height.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public GraphicsFP(int width, int height)
        {
            InitBlock();
            Resize(width, height);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the content of this image as ARGB array.
        /// </summary>
        /// <returns>the ARGB array of the image content</returns>
        public int[] GetRGB()
        {
            return _renderer._buffer;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the brush object of the graphics.
        /// </summary>
        /// <returns>the brush object</returns>
        public BrushFP GetBrush()
        {
            return _fillStyle;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set a new brush for this graphics object.
        /// </summary>
        /// <param name="value">value a new brush.</param>
        public void SetBrush(BrushFP value)
        {
            _fillStyle = value;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the pen object for this graphics.
        /// </summary>
        /// <returns>the pen object</returns>
        public PenFP GetPen()
        {
            return _lineStyle;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// set the new pen for this graphics object.
        /// </summary>
        /// <param name="value">a new pen object</param>
        public void SetPen(PenFP value)
        {
            _lineStyle = value;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the paint mode.
        /// </summary>
        /// <returns>the paint mode.</returns>
        public int GetPaintMode()
        {
            return _paintMode;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set the paint mode for this graphics.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetPaintMode(int value)
        {
            _paintMode = value;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the graphics drawing matrix.
        /// </summary>
        /// <returns>the drawing matrix</returns>
        public MatrixFP GetMatrix()
        {
            return _matrix;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set the graphics matrix.
        /// </summary>
        /// <param name="value">value the new matrix</param>
        public void SetMatrix(MatrixFP value)
        {
            _matrix = value == null ? null : new MatrixFP(value);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// resize the graphics object.
        /// </summary>
        /// <param name="width">the new width of the graphics.</param>
        /// <param name="height">the new height of the graphics object.</param>
        public void Resize(int width, int height)
        {
            _renderer.Reset(width, height, width);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Clear the graphics content with given color.
        /// </summary>
        /// <param name="color">The color.</param>
        public void Clear(int color)
        {
            _renderer.Clear(color);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the line.
        /// </summary>
        /// <param name="ffX1">the x coord of the first point of the line.</param>
        /// <param name="ffY1">the y coord of the first point of the line</param>
        /// <param name="ffX2">the x coord of the second point of the line</param>
        /// <param name="ffY2">the y coord of the second point of the line</param>
        public void DrawLine(int ffX1, int ffY1, int ffX2, int ffY2)
        {
            DrawPath(GraphicsPathFP.CreateLine(ffX1, ffY1, ffX2, ffY2));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the polyline.
        /// </summary>
        /// <param name="points"> the coordinates  of the polyline</param>
        public void DrawPolyline(PointFP[] points)
        {
            DrawPath(GraphicsPathFP.CreatePolyline(points));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the polygon.
        /// </summary>
        /// <param name="points">the coordinates  of the polygon</param>
        public void DrawPolygon(PointFP[] points)
        {
            DrawPath(GraphicsPathFP.CreatePolygon(points));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the curves.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="numberOfSegments">The number of segments.</param>
        /// <param name="ffFactor">The ff factor.</param>
        public void DrawCurves(PointFP[] points, int offset, int numberOfSegments,
                               int ffFactor)
        {
            DrawPath(GraphicsPathFP.CreateSmoothCurves(points,
                                                       offset, numberOfSegments, ffFactor, false));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the closed curves.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="numberOfSegments">The number of segments.</param>
        /// <param name="ffFactor">The ff factor.</param>
        public void DrawClosedCurves(PointFP[] points, int offset,
                                     int numberOfSegments, int ffFactor)
        {
            DrawPath(GraphicsPathFP.CreateSmoothCurves(points, offset,
                                                       numberOfSegments, ffFactor, true));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the round rect.
        /// </summary>
        /// <param name="ffXmin">The ff xmin.</param>
        /// <param name="ffYmin">The ff ymin.</param>
        /// <param name="ffXmax">The ff xmax.</param>
        /// <param name="ffYmax">The ff ymax.</param>
        /// <param name="ffRx">The ff rx.</param>
        /// <param name="ffRy">The ff ry.</param>
        public void DrawRoundRect(int ffXmin, int ffYmin, int ffXmax,
                                  int ffYmax, int ffRx, int ffRy)
        {
            DrawPath(GraphicsPathFP.CreateRoundRect(ffXmin, ffYmin, ffXmax,
                                                    ffYmax, ffRx, ffRy));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the rect.
        /// </summary>
        /// <param name="ffXmin">The ff xmin.</param>
        /// <param name="ffYmin">The ff ymin.</param>
        /// <param name="ffXmax">The ff xmax.</param>
        /// <param name="ffYmax">The ff ymax.</param>
        public void DrawRect(int ffXmin, int ffYmin, int ffXmax, int ffYmax)
        {
            DrawPath(GraphicsPathFP.CreateRect(ffXmin, ffYmin, ffXmax, ffYmax));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the oval.
        /// </summary>
        /// <param name="ffXmin">The ff xmin.</param>
        /// <param name="ffYmin">The ff ymin.</param>
        /// <param name="ffXmax">The ff xmax.</param>
        /// <param name="ffYmax">The ff ymax.</param>
        public void DrawOval(int ffXmin, int ffYmin, int ffXmax, int ffYmax)
        {
            DrawPath(GraphicsPathFP.CreateOval(ffXmin, ffYmin, ffXmax, ffYmax));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the arc.
        /// </summary>
        /// <param name="ffXmin">The ff xmin.</param>
        /// <param name="ffYmin">The ff ymin.</param>
        /// <param name="ffXmax">The ff xmax.</param>
        /// <param name="ffYmax">The ff ymax.</param>
        /// <param name="ffStartangle">The ff startangle.</param>
        /// <param name="ffSweepangle">The ff sweepangle.</param>
        public void DrawArc(int ffXmin, int ffYmin, int ffXmax, int ffYmax,
                            int ffStartangle, int ffSweepangle)
        {
            DrawPath(GraphicsPathFP.CreateArc(ffXmin, ffYmin, ffXmax, ffYmax,
                                              ffStartangle, ffSweepangle, false));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the pie.
        /// </summary>
        /// <param name="ffXmin">The ff xmin.</param>
        /// <param name="ffYmin">The ff ymin.</param>
        /// <param name="ffXmax">The ff xmax.</param>
        /// <param name="ffYmax">The ff ymax.</param>
        /// <param name="ffStartangle">The ff startangle.</param>
        /// <param name="ffSweepangle">The ff sweepangle.</param>
        public void DrawPie(int ffXmin, int ffYmin, int ffXmax, int ffYmax,
                            int ffStartangle, int ffSweepangle)
        {
            DrawPath(GraphicsPathFP.CreateArc(ffXmin, ffYmin, ffXmax, ffYmax,
                                              ffStartangle, ffSweepangle, true));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the path.
        /// </summary>
        /// <param name="path">The path.</param>
        public void DrawPath(GraphicsPathFP path)
        {
            if (_lineStyle.DashArray != null)
            {
                var newlineStyle = new PenFP(_lineStyle.Brush, _lineStyle.Width,
                                             PenFP.LinecapButt, PenFP.LinecapButt, PenFP.LinejoinMiter)
                                       {DashArray = _lineStyle.DashArray};

                var dasher = new GraphicsPathDasherFP(path,
                                                      newlineStyle.DashArray, 0);
                var newPath = dasher.GetDashedGraphicsPath();
                _renderer.DrawPath(newPath.CalcOutline(newlineStyle), _matrix,
                                   _lineStyle.Brush, GraphicsPathRendererFP.ModeZero);
            }
            else
            {
                _renderer.DrawPath(path.CalcOutline(_lineStyle), _matrix,
                                   _lineStyle.Brush, GraphicsPathRendererFP.ModeZero);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Fills the closed curves.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="numberOfSegments">The number of segments.</param>
        /// <param name="ffFactor">The ff factor.</param>
        public void FillClosedCurves(PointFP[] points, int offset,
                                     int numberOfSegments, int ffFactor)
        {
            FillPath(GraphicsPathFP.CreateSmoothCurves(points, offset,
                                                       numberOfSegments, ffFactor, true));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Fills the polygon.
        /// </summary>
        /// <param name="points">The points.</param>
        public void FillPolygon(PointFP[] points)
        {
            FillPath(GraphicsPathFP.CreatePolygon(points));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Fills the round rect.
        /// </summary>
        /// <param name="ffXmin">The ff xmin.</param>
        /// <param name="ffYmin">The ff ymin.</param>
        /// <param name="ffXmax">The ff xmax.</param>
        /// <param name="ffYmax">The ff ymax.</param>
        /// <param name="ffRx">The ff rx.</param>
        /// <param name="ffRy">The ff ry.</param>
        public void FillRoundRect(int ffXmin, int ffYmin, int ffXmax,
                                  int ffYmax, int ffRx, int ffRy)
        {
            FillPath(GraphicsPathFP.CreateRoundRect(ffXmin, ffYmin, ffXmax,
                                                    ffYmax, ffRx, ffRy));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Fills the rect.
        /// </summary>
        /// <param name="ffXmin">The ff xmin.</param>
        /// <param name="ffYmin">The ff ymin.</param>
        /// <param name="ffXmax">The ff xmax.</param>
        /// <param name="ffYmax">The ff ymax.</param>
        public void FillRect(int ffXmin, int ffYmin, int ffXmax, int ffYmax)
        {
            var path = GraphicsPathFP.CreateRect(ffXmin, ffYmin,
                                                 ffXmax, ffYmax);
            path.AddClose();
            FillPath(path);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Fills the oval.
        /// </summary>
        /// <param name="ffXmin">The ff xmin.</param>
        /// <param name="ffYmin">The ff ymin.</param>
        /// <param name="ffXmax">The ff xmax.</param>
        /// <param name="ffYmax">The ff ymax.</param>
        public void FillOval(int ffXmin, int ffYmin, int ffXmax, int ffYmax)
        {
            var path = GraphicsPathFP.CreateOval(ffXmin,
                                                 ffYmin, ffXmax, ffYmax);
            path.AddClose();
            FillPath(path);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Fills the pie.
        /// </summary>
        /// <param name="ffXmin">The ff xmin.</param>
        /// <param name="ffYmin">The ff ymin.</param>
        /// <param name="ffXmax">The ff xmax.</param>
        /// <param name="ffYmax">The ff ymax.</param>
        /// <param name="ffStartangle">The ff startangle.</param>
        /// <param name="ffSweepangle">The ff sweepangle.</param>
        public void FillPie(int ffXmin, int ffYmin, int ffXmax, int ffYmax,
                            int ffStartangle, int ffSweepangle)
        {
            FillPath(GraphicsPathFP.CreateArc(ffXmin, ffYmin, ffXmax, ffYmax,
                                              ffStartangle, ffSweepangle, true));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Fills the path.
        /// </summary>
        /// <param name="path">The path.</param>
        public void FillPath(GraphicsPathFP path)
        {
            _renderer.DrawPath(path, _matrix, _fillStyle, _paintMode);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the height of the clip.
        /// </summary>
        /// <returns></returns>
        public int GetClipHeight()
        {
            return _renderer._clipHeight;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the width of the clip.
        /// </summary>
        /// <returns></returns>
        public int GetClipWidth()
        {
            return _renderer._clipWidth;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the clip X.
        /// </summary>
        /// <returns></returns>
        public int GetClipX()
        {
            return _renderer._clipX;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the clip Y.
        /// </summary>
        /// <returns></returns>
        public int GetClipY()
        {
            return _renderer._clipY;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the clip.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public void SetClip(int x,
                            int y,
                            int width,
                            int height)
        {
            _renderer.SetClip(x, y, width, height);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Finalizes the buffer.
        /// </summary>
        /// <param name="color">The color.</param>
        public void FinalizeBuffer(int color)
        {
            _renderer.FinalizeBuffer(color);
        }

        private PenFP _lineStyle;
        private BrushFP _fillStyle;
        private readonly GraphicsPathRendererFP _renderer = new GraphicsPathRendererFP();
        private int _paintMode;
        private MatrixFP _matrix;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// init the pen, brush, paint mode.
        /// </summary>
        private void InitBlock()
        {
            _lineStyle = new PenFP(0x0, SingleFP.One);
            _fillStyle = new SolidBrushFP(0x0);
            _paintMode = ModeXor;
        }
    }
}