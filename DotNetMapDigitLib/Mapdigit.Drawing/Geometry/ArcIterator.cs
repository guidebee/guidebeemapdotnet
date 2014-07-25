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
using Mapdigit.Util;

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
    ///  A utility class to iterate over the path segments of an arc
    /// through the IPathIterator interface.
    /// </summary>
    internal class ArcIterator : PathIterator
    {
        private readonly double _x;
        private readonly double _y;
        private readonly double _w;
        private readonly double _h;
        private readonly double _angStRad;
        private readonly double _increment;
        private readonly double _cv;
        private readonly AffineTransform _affine;
        private int _index;
        private readonly int _arcSegs;
        private readonly int _lineSegs;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="ArcIterator"/> class.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="at">At.</param>
        internal ArcIterator(Arc a, AffineTransform at)
        {
            _w = a.IntWidth/2.0;
            _h = a.IntHeight/2.0;
            _x = a.IntX + _w;
            _y = a.IntY + _h;
            _angStRad = -MathEx.ToRadians(a.AngleStart);
            _affine = at;
            double ext = -a.AngleExtent;
            if (ext >= 360.0 || ext <= -360)
            {
                _arcSegs = 4;
                _increment = Math.PI/2;
                // btan(Math.Pi / 2);
                _cv = 0.5522847498307933;
                if (ext < 0)
                {
                    _increment = -_increment;
                    _cv = -_cv;
                }
            }
            else
            {
                _arcSegs = (int) MathEx.Ceil(MathEx.Abs(ext)/90.0);
                _increment = MathEx.ToRadians(ext/_arcSegs);
                _cv = Btan(_increment);
                if (_cv == 0)
                {
                    _arcSegs = 0;
                }
            }
            switch (a.ArcType)
            {
                case Arc.Open:
                    _lineSegs = 0;
                    break;
                case Arc.Chord:
                    _lineSegs = 1;
                    break;
                case Arc.Pie:
                    _lineSegs = 2;
                    break;
            }
            if (_w < 0 || _h < 0)
            {
                _arcSegs = _lineSegs = -1;
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
            return _index > _arcSegs + _lineSegs;
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
        }


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
                throw new IndexOutOfRangeException("arc iterator out of bounds");
            }
            double angle = _angStRad;
            if (_index == 0)
            {
                coords[0] = (int) (_x + MathEx.Cos(angle)*_w + .5);
                coords[1] = (int) (_y + MathEx.Sin(angle)*_h + .5);
                if (_affine != null)
                {
                    _affine.Transform(coords, 0, coords, 0, 1);
                }
                return SegMoveto;
            }
            if (_index > _arcSegs)
            {
                if (_index == _arcSegs + _lineSegs)
                {
                    return SegClose;
                }
                coords[0] = (int) (_x + .5);
                coords[1] = (int) (_y + .5);
                if (_affine != null)
                {
                    _affine.Transform(coords, 0, coords, 0, 1);
                }
                return SegLineto;
            }
            angle += _increment*(_index - 1);
            double relx = MathEx.Cos(angle);
            double rely = MathEx.Sin(angle);
            coords[0] = (int) (_x + (relx - _cv*rely)*_w + .5);
            coords[1] = (int) (_y + (rely + _cv*relx)*_h + .5);
            angle += _increment;
            relx = MathEx.Cos(angle);
            rely = MathEx.Sin(angle);
            coords[2] = (int) (_x + (relx + _cv*rely)*_w + .5);
            coords[3] = (int) (_y + (rely - _cv*relx)*_h + .5);
            coords[4] = (int) (_x + relx*_w + .5);
            coords[5] = (int) (_y + rely*_h + .5);
            if (_affine != null)
            {
                _affine.Transform(coords, 0, coords, 0, 3);
            }
            return SegCubicto;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// btan computes the length (k) of the control segments at
        /// the beginning and end of a cubic bezier that approximates
        /// a segment of an arc with extent less than or equal to
        /// 90 degrees.  This length (k) will be used to generate the
        /// 2 bezier control points for such a segment.
        ///
        ///   Assumptions:
        ///     a) arc is centered on 0,0 with radius of 1.0
        ///     b) arc extent is less than 90 degrees
        ///     c) control points should preserve tangent
        ///     d) control segments should have equal length
        ///
        ///   Initial data:
        ///     start angle: ang1
        ///     end angle:   ang2 = ang1 + extent
        ///     start point: P1 = (x1, y1) = (cos(ang1), sin(ang1))
        ///     end point:   P4 = (x4, y4) = (cos(ang2), sin(ang2))
        ///
        ///   Control points:
        ///     P2 = (x2, y2)
        ///     | x2 = x1 - k * sin(ang1) = cos(ang1) - k * sin(ang1)
        ///     | y2 = y1 + k * cos(ang1) = sin(ang1) + k * cos(ang1)
        ///
        ///     P3 = (x3, y3)
        ///     | x3 = x4 + k * sin(ang2) = cos(ang2) + k * sin(ang2)
        ///     | y3 = y4 - k * cos(ang2) = sin(ang2) - k * cos(ang2)
        ///
        /// The formula for this length (k) can be found using the
        /// following derivations:
        ///
        ///   Midpoints:
        ///     a) bezier (t = 1/2)
        ///        bPm = P1 * (1-t)^3 +
        ///              3 * P2 * t * (1-t)^2 +
        ///              3 * P3 * t^2 * (1-t) +
        ///              P4 * t^3 =
        ///            = (P1 + 3P2 + 3P3 + P4)/8
        ///
        ///     b) arc
        ///        aPm = (cos((ang1 + ang2)/2), sin((ang1 + ang2)/2))
        ///
        ///   Let angb = (ang2 - ang1)/2; angb is half of the angle
        ///   between ang1 and ang2.
        ///
        ///   Solve the equation bPm == aPm
        ///
        ///     a) For xm coord:
        ///        x1 + 3*x2 + 3*x3 + x4 = 8*cos((ang1 + ang2)/2)
        ///
        ///        cos(ang1) + 3*cos(ang1) - 3*k*sin(ang1) +
        ///        3*cos(ang2) + 3*k*sin(ang2) + cos(ang2) =
        ///        = 8*cos((ang1 + ang2)/2)
        ///
        ///        4*cos(ang1) + 4*cos(ang2) + 3*k*(sin(ang2) - sin(ang1)) =
        ///        = 8*cos((ang1 + ang2)/2)
        ///
        ///        8*cos((ang1 + ang2)/2)*cos((ang2 - ang1)/2) +
        ///        6*k*sin((ang2 - ang1)/2)*cos((ang1 + ang2)/2) =
        ///        = 8*cos((ang1 + ang2)/2)
        ///
        ///        4*cos(angb) + 3*k*sin(angb) = 4
        ///
        ///        k = 4 / 3 * (1 - cos(angb)) / sin(angb)
        ///
        ///     b) For ym coord we derive the same formula.
        ///
        /// Since this formula can generate "NaN" values for small
        /// angles, we will derive a safer form that does not involve
        /// dividing by very small values:
        ///     (1 - cos(angb)) / sin(angb) =
        ///     = (1 - cos(angb))*(1 + cos(angb)) / sin(angb)*(1 + cos(angb)) =
        ///     = (1 - cos(angb)^2) / sin(angb)*(1 + cos(angb)) =
        ///     = sin(angb)^2 / sin(angb)*(1 + cos(angb)) =
        ///     = sin(angb) / (1 + cos(angb))
        /// </summary>
        /// <param name="increment">The increment.</param>
        /// <returns></returns>
        private static double Btan(double increment)
        {
            increment /= 2.0;
            return 4.0/3.0*MathEx.Sin(increment)/(1.0 + MathEx.Cos(increment));
        }
    }
}