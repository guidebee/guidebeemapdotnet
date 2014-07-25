//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 25SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System;

//--------------------------------- PACKAGE ------------------------------------

namespace Mapdigit.Drawing.Geometry
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 25SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// A utility class to iterate over the path segments of an ellipse
    /// through the IPathIterator interface.
    /// </summary>
    internal class EllipseIterator : PathIterator
    {
        private readonly double _x;
        private readonly double _y;
        private readonly double _w;
        private readonly double _h;
        private readonly AffineTransform _affine;
        private int _index;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="EllipseIterator"/> class.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <param name="at">At.</param>
        internal EllipseIterator(RectangularShape e, AffineTransform at)
        {
            _x = e.IntX;
            _y = e.IntY;
            _w = e.IntWidth;
            _h = e.IntHeight;
            _affine = at;
            if (_w < 0 || _h < 0)
            {
                _index = 6;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Returns the winding rule for determining the interior of the path.
        /// </summary>
        /// <returns>the winding rule</returns>
        public override int WindingRule
        {
            get { return WindNonZero; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tests if the iteration is complete.
        /// </summary>
        /// <returns>
        /// 	true if all the segments have
        /// been read; false otherwise.
        /// </returns>
        public override bool IsDone()
        {
            return _index > 5;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Moves the iterator to the next segment of the path forwards
        /// along the primary direction of traversal as long as there are
        /// more points in that direction.
        /// </summary>
        public override void Next()
        {
            _index++;
        } // ArcIterator.btan(Math.Pi/2)

        public const double CtrlVal = 0.5522847498307933;

        /*
         * ctrlpts contains the control points for a set of 4 cubic
         * bezier curves that approximate a circle of radius 0.5
         * centered at 0.5, 0.5
         */
        private const double Pcv = 0.5 + CtrlVal*0.5;
        private const double Ncv = 0.5 - CtrlVal*0.5;

        private static readonly double[][] Ctrlpts = new[]
                                                         {
                                                             new[] {1.0, Pcv, Pcv, 1.0, 0.5, 1.0},
                                                             new[] {Ncv, 1.0, 0.0, Pcv, 0.0, 0.5},
                                                             new[] {0.0, Ncv, Ncv, 0.0, 0.5, 0.0},
                                                             new[] {Pcv, 0.0, 1.0, Ncv, 1.0, 0.5}
                                                         };

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the coordinates and type of the current path segment in
        /// the iteration.
        /// The return value is the path-segment type:
        /// SegMoveto, SegLineto, SegQuadto, SegCubicto, or SegClose.
        /// A long array of length 6 must be passed in and can be used to
        /// store the coordinates of the point(s).
        /// Each point is stored as a pair of long x,y coordinates.
        /// SegMoveto and SegLineto types returns one point,
        /// SegQuadto returns two points,
        /// SegCubicto returns 3 points
        /// and SegClose does not return any points.
        /// </summary>
        /// <param name="coords">an array that holds the data returned from
        /// this method</param>
        /// <returns>
        /// the path-segment type of the current path segment
        /// </returns>
        public override int CurrentSegment(int[] coords)
        {
            if (IsDone())
            {
                throw new IndexOutOfRangeException("ellipse iterator out of bounds");
            }
            if (_index == 5)
            {
                return SegClose;
            }
            if (_index == 0)
            {
                double[] ctrls = Ctrlpts[3];
                coords[0] = (int) (_x + ctrls[4]*_w + .5);
                coords[1] = (int) (_y + ctrls[5]*_h + .5);
                if (_affine != null)
                {
                    _affine.Transform(coords, 0, coords, 0, 1);
                }
                return SegMoveto;
            }
            {
                double[] ctrls = Ctrlpts[_index - 1];
                coords[0] = (int) (_x + ctrls[0]*_w + .5);
                coords[1] = (int) (_y + ctrls[1]*_h + .5);
                coords[2] = (int) (_x + ctrls[2]*_w + .5);
                coords[3] = (int) (_y + ctrls[3]*_h + .5);
                coords[4] = (int) (_x + ctrls[4]*_w + .5);
                coords[5] = (int) (_y + ctrls[5]*_h + .5);
                if (_affine != null)
                {
                    _affine.Transform(coords, 0, coords, 0, 3);
                }
            }
            return SegCubicto;
        }
    }
}