//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 26SEP2010  James Shen                 	          Code review
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
    // 26SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// A utility class to iterate over the path segments of an rounded rectangle
    /// through the IPathIterator interface.
    /// </summary>
    internal class RoundRectIterator : PathIterator
    {
        private readonly double _x;
        private readonly double _y;
        private readonly double _w;
        private readonly double _h;
        private readonly double _aw;
        private readonly double _ah;
        private readonly AffineTransform _affine;
        private int _index;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="RoundRectIterator"/> class.
        /// </summary>
        /// <param name="rr">The rr.</param>
        /// <param name="at">At.</param>
        internal RoundRectIterator(RoundRectangle rr, AffineTransform at)
        {
            _x = rr.IntX;
            _y = rr.IntY;
            _w = rr.IntWidth;
            _h = rr.IntHeight;
            _aw = Math.Min(_w, Math.Abs(rr.ArcWidth));
            _ah = Math.Min(_h, Math.Abs(rr.ArcHeight));
            _affine = at;
            if (_aw < 0 || _ah < 0)
            {
                // Don't draw anything...
                _index = Ctrlpts.Length;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
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
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tests if the iteration is complete.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if all the segments have
        /// been read; <c>false</c> otherwise.
        /// </returns>
        public override bool IsDone()
        {
            return _index >= Ctrlpts.Length;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Moves the iterator to the next segment of the path forwards
        /// along the primary direction of traversal as long as there are
        /// more points in that direction.
        /// </summary>
        public override void Next()
        {
            _index++;
        }

        private const double Angle = Math.PI/4.0;
        private static readonly double A = 1.0 - Math.Cos(Angle);
        private static readonly double B = Math.Tan(Angle);
        private static readonly double C = Math.Sqrt(1.0 + B*B) - 1 + A;
        private static readonly double Cv = 4.0/3.0*A*B/C;
        private static readonly double Acv = (1.0 - Cv)/2.0;

        // For each array:
        //     4 values for each point {v0, v1, v2, v3}:
        //         point = (x + v0 * w + v1 * arcWidth,
        //                  y + v2 * h + v3 * arcHeight);
        private static readonly double[][] Ctrlpts = new[]
                                                         {
                                                             new[] {0.0, 0.0, 0.0, 0.5},
                                                             new[] {0.0, 0.0, 1.0, -0.5},
                                                             new[]
                                                                 {
                                                                     0.0, 0.0, 1.0, -Acv,
                                                                     0.0, Acv, 1.0, 0.0,
                                                                     0.0, 0.5, 1.0, 0.0
                                                                 },
                                                             new[] {1.0, -0.5, 1.0, 0.0},
                                                             new[]
                                                                 {
                                                                     1.0, -Acv, 1.0, 0.0,
                                                                     1.0, 0.0, 1.0, -Acv,
                                                                     1.0, 0.0, 1.0, -0.5
                                                                 },
                                                             new[] {1.0, 0.0, 0.0, 0.5},
                                                             new[]
                                                                 {
                                                                     1.0, 0.0, 0.0, Acv,
                                                                     1.0, -Acv, 0.0, 0.0,
                                                                     1.0, -0.5, 0.0, 0.0
                                                                 },
                                                             new[] {0.0, 0.5, 0.0, 0.0},
                                                             new[]
                                                                 {
                                                                     0.0, Acv, 0.0, 0.0,
                                                                     0.0, 0.0, 0.0, Acv,
                                                                     0.0, 0.0, 0.0, 0.5
                                                                 },
                                                             new double[] {},
                                                         };

        private static readonly int[] Types = {
                                                  SegMoveto,
                                                  SegLineto, SegCubicto,
                                                  SegLineto, SegCubicto,
                                                  SegLineto, SegCubicto,
                                                  SegLineto, SegCubicto,
                                                  SegClose,
                                              };

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
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
                throw new IndexOutOfRangeException("roundrect iterator out of bounds");
            }
            double[] ctrls = Ctrlpts[_index];
            int nc = 0;
            for (int i = 0; i < ctrls.Length; i += 4)
            {
                coords[nc++] = (int) (_x + ctrls[i + 0]*_w + ctrls[i + 1]*_aw + .5);
                coords[nc++] = (int) (_y + ctrls[i + 2]*_h + ctrls[i + 3]*_ah + .5);
            }
            if (_affine != null)
            {
                _affine.Transform(coords, 0, coords, 0, nc/2);
            }
            return Types[_index];
        }
    }
}