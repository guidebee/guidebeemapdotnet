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

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Drawing.Geometry
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 14OCT2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// The PathIterator interface provides the mechanism  for objects that 
    /// implement the  IShape interface to return the geometry of their boundary 
    /// by allowing a caller to retrieve the path of that boundary a segment at a
    /// time. 
    /// </summary>
    /// <remarks>
    ///  This interface allows these objects to retrieve the path of
    /// their boundary a segment at a time by using 1st through 3rd order
    /// Bezier curves, which are lines and quadratic or cubic
    /// Bezier splines.
    /// Multiple subpaths can be expressed by using a "MOVETO" segment to
    /// create a discontinuity in the geometry to move from the end of
    /// one subpath to the beginning of the next.
    /// Each subpath can be closed manually by ending the last segment in
    /// the subpath on the same coordinate as the beginning "MOVETO" segment
    /// for that subpath or by using a "CLOSE" segment to append a line
    /// segment from the last point back to the first.
    /// Be aware that manually closing an outline as opposed to using a
    /// "CLOSE" segment to close the path might result in different line
    /// style decorations being used at the end points of the subpath.
    ///</remarks>
    public abstract class PathIterator
    {

        /// <summary>
        /// The winding rule constant for specifying an even-odd rule
        /// for determining the interior of a path.
        /// </summary>
        /// <remarks>
        /// The even-odd rule specifies that a point lies inside the
        /// path if a ray drawn in any direction from that point to
        /// infinity is crossed by path segments an odd number of times.
        /// </remarks>
        public const int WindEvenOdd = 0;


        /// <summary>
        /// The winding rule constant for specifying a non-zero rule
        /// for determining the interior of a path.
        /// </summary>
        /// <remarks>
        /// The non-zero rule specifies that a point lies inside the
        /// path if a ray drawn in any direction from that point to
        /// infinity is crossed by path segments a different number
        /// of times in the counter-clockwise direction than the
        /// clockwise direction.
        /// </remarks>
        public const int WindNonZero = 1;


        /// <summary>
        /// The segment type constant for a point that specifies the
        /// starting location for a new subpath.
        /// </summary>
        public const int SegMoveto = 0;


        /// <summary>
        /// The segment type constant for a point that specifies the
        /// end point of a line to be drawn from the most recently
        /// specified point.
        /// </summary>
        public const int SegLineto = 1;


        /// <summary>
        /// The segment type constant for the pair of points that specify
        /// a quadratic parametric curve to be drawn from the most recently
        /// specified point.
        /// </summary>
        /// <remarks>
        /// The curve is interpolated by solving the parametric control
        /// equation in the range (t=[0..1]) using
        /// the most recently specified (current) point (CP),
        /// the first control point (P1),
        /// and the final interpolated control point (P2).
        /// The parametric control equation for this curve is:
        /// <pre>
        ///          P(t) = B(2,0)*CP + B(2,1)*P1 + B(2,2)*P2
        ///          0 &lt;= t &lt;= 1
        ///
        ///        B(n,m) = mth coefficient of nth degree Bernstein polynomial
        ///               = C(n,m) * t^(m) * (1 - t)^(n-m)
        ///        C(n,m) = Combinations of n things, taken m at a time
        ///               = n! / (m! * (n-m)!)
        /// </pre>
        /// </remarks>
        public const int SegQuadto = 2;


        /// <summary>
        /// The segment type constant for the set of 3 points that specify
        /// a cubic parametric curve to be drawn from the most recently
        /// specified point.
        /// </summary>
        /// <remarks>
        /// The curve is interpolated by solving the parametric control
        /// equation in the range (t=[0..1]) using
        /// the most recently specified (current) point (CP),
        /// the first control point (P1),
        /// the second control point (P2),
        /// and the final interpolated control point (P3).
        /// The parametric control equation for this curve is:
        /// <pre>
        ///          P(t) = B(3,0)*CP + B(3,1)*P1 + B(3,2)*P2 + B(3,3)*P3
        ///          0 &lt;= t &lt;= 1
        ///
        ///        B(n,m) = mth coefficient of nth degree Bernstein polynomial
        ///               = C(n,m) * t^(m) * (1 - t)^(n-m)
        ///        C(n,m) = Combinations of n things, taken m at a time
        ///               = n! / (m! * (n-m)!)
        /// </pre>
        /// This form of curve is commonly known as a Bezier curve.
        /// </remarks>
        public const int SegCubicto = 3;


        /// <summary>
        /// The segment type constant that specifies that
        /// the preceding subpath should be closed by appending a line segment
        /// back to the point corresponding to the most recent SegMoveto.
        /// </summary>
        public const int SegClose = 4;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the winding rule for determining the interior of the path.
        /// </summary>
        public abstract int WindingRule { get; }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tests if the iteration is complete.
        /// </summary>
        /// <returns>
        /// 	true if all the segments have 
        ///  been read; false otherwise.
        /// </returns>
        public abstract bool IsDone();

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Moves the iterator to the next segment of the path forwards
        /// along the primary direction of traversal as long as there are
        /// more points in that direction.
        /// </summary>
        public abstract void Next();

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the coordinates and type of the current path segment in
        /// the iteration.
        /// </summary>
        /// <remarks>
        /// The return value is the path-segment type:
        /// SegMoveto, SegLineto, SegQuadto, SegCubicto, or SegClose.
        /// A long array of length 6 must be passed in and can be used to
        /// store the coordinates of the point(s).
        /// Each point is stored as a pair of long x,y coordinates.
        /// SegMoveto and SegLineto types returns one point,
        /// SegQuadto returns two points,
        /// SegCubicto returns 3 points
        /// and SegClose does not return any points.
        /// </remarks>
        /// <param name="coords">an array that holds the data returned from
        /// this method</param>
        /// <returns> the path-segment type of the current path segment</returns>
        public abstract int CurrentSegment(int[] coords);
    }
}