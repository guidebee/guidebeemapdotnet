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
    /// the class uses line to draw a sketch for a given path.
    /// </summary>
    internal class GraphicsPathSketchFP : IGraphicsPathIteratorFP
    {
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Currents the point.
        /// </summary>
        /// <returns>Get the current point.</returns>
        public PointFP CurrentPoint()
        {
            return _currPoint;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the start point
        /// </summary>
        /// <returns>the start point</returns>
        public PointFP StartPoint()
        {
            return _startPoint;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Begins the path.
        /// </summary>
        public virtual void Begin()
        {
            _started = false;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Ends the path.
        /// </summary>
        public virtual void End()
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Moves to given point.
        /// </summary>
        /// <param name="point">The point.</param>
        public virtual void MoveTo(PointFP point)
        {
            if (!_started)
            {
                _startPoint.Reset(point);
                _started = true;
            }
            _currPoint.Reset(point);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// draw a line from current point to given point.
        /// </summary>
        /// <param name="point">The point.</param>
        public virtual void LineTo(PointFP point)
        {
            _currPoint.Reset(point);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draw quadic curve with given control points.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="point">The point.</param>
        public virtual void QuadTo(PointFP control, PointFP point)
        {
            // Compute forward difference values for a quadratic
            // curve of type A*(1-t)^2 + 2*B*t*(1-t) + C*t^2

            var f = new PointFP(_currPoint);
            var tmp = new PointFP((_currPoint.X - control.X*2 + point.X)
                                  /Subdivide2, (_currPoint.Y - control.Y*2 + point.Y)
                                               /Subdivide2);
            var ddf = new PointFP(tmp.X*2, tmp.Y*2);
            var df = new PointFP(tmp.X + (control.X - _currPoint.X)*2
                                         /Subdivide, tmp.Y + (control.Y - _currPoint.Y)*2/Subdivide);

            for (int c = 0; c < Subdivide - 1; c++)
            {
                f.Add(df);
                df.Add(ddf);
                LineTo(f);
            }

            // We specify the last point manually since
            // we obtain rounding errors during the
            // forward difference computation.
            LineTo(point);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draw curve with given control points.
        /// </summary>
        /// <param name="control1">The control1.</param>
        /// <param name="control2">The control2.</param>
        /// <param name="point">The point.</param>
        public virtual void CurveTo(PointFP control1, PointFP control2, PointFP point)
        {
            var tmp1 = new PointFP(_currPoint.X - control1.X*2 + control2.X,
                                   _currPoint.Y - control1.Y*2 + control2.Y);
            var tmp2 = new PointFP((control1.X - control2.X)*3 - _currPoint.X
                                   + point.X, (control1.Y - control2.Y)*3 - _currPoint.Y + point.Y);

            var f = new PointFP(_currPoint);
            var df = new PointFP((control1.X - _currPoint.X)*3/Subdivide
                                 + tmp1.X*3/Subdivide2 + tmp2.X/Subdivide3,
                                 (control1.Y - _currPoint.Y)*3/Subdivide + tmp1.Y*3
                                                                           /Subdivide2 + tmp2.Y/Subdivide3);
            var ddf = new PointFP(tmp1.X*6/Subdivide2 + tmp2.X*6
                                                        /Subdivide3, tmp1.Y*6/Subdivide2 + tmp2.Y*6/Subdivide3);
            var dddf = new PointFP(tmp2.X*6
                                   /Subdivide3, tmp2.Y*6/Subdivide3);

            for (var c = 0; c < Subdivide - 1; c++)
            {
                f.Add(df);
                df.Add(ddf);
                ddf.Add(dddf);
                LineTo(f);
            }

            LineTo(point);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Closes the path.
        /// </summary>
        public virtual void Close()
        {
            // Connect start point with end point
            LineTo(_startPoint);
            _started = false;
        }

        protected const int Subdivide = 24;
        protected const int Subdivide2 = Subdivide*Subdivide;
        protected const int Subdivide3 = Subdivide2*Subdivide;
        protected PointFP _startPoint = new PointFP();
        protected PointFP _currPoint = new PointFP();
        protected bool _started;
    }
}