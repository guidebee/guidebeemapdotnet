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
    /// A line segment specified with int coordinates.
    /// </summary>
    public class Line : IShape
    {
        /// <summary>
        /// The X coordinate of the start point of the line segment.
        /// </summary>
        public int X1;

        /// <summary>
        /// The Y coordinate of the start point of the line segment.
        /// </summary>
        public int Y1;

        /// <summary>
        /// The X coordinate of the end point of the line segment.
        /// </summary>
        public int X2;

        /// <summary>
        /// The Y coordinate of the end point of the line segment.
        /// </summary>
        public int Y2;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="Line"/> class.
        /// </summary>
        public Line()
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs and initializes a Line from the
        /// specified coordinates.
        /// </summary>
        /// <param name="x1">the X coordinate of the start point.</param>
        /// <param name="y1">the Y coordinate of the start point.</param>
        /// <param name="x2">the X coordinate of the end point.</param>
        /// <param name="y2">the Y coordinate of the end point.</param>
        public Line(int x1, int y1, int x2, int y2)
        {
            SetLine(x1, y1, x2, y2);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs and initializes a Line from the
        /// specified Point objects.
        /// </summary>
        /// <param name="p1">the start Point of this line segment.</param>
        /// <param name="p2">the end Point of this line segment.</param>
        public Line(Point p1, Point p2)
        {
            SetLine(p1, p2);
        }

       
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the start Point of this Line.
        /// </summary>
        public Point P1
        {
            get { return new Point(X1, Y1); }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the end Point of this Line.
        /// </summary>
        public Point P2
        {
            get { return new Point(X2, Y2); }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the location of the end points of this Line to
        /// the specified coordinates.
        /// </summary>
        /// <param name="x1">the X coordinate of the start point</param>
        /// <param name="y1">the Y coordinate of the start point</param>
        /// <param name="x2">the X coordinate of the end point.</param>
        /// <param name="y2">the Y coordinate of the end point.</param>
        public void SetLine(int x1, int y1, int x2, int y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the location of the end points of this Line to
        /// the specified Point coordinates.
        /// </summary>
        /// <param name="p1">the start Point of the line segment.</param>
        /// <param name="p2">the end Point of the line segment.</param>
        public void SetLine(Point p1, Point p2)
        {
            SetLine(p1.X, p1.Y, p2.X, p2.Y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the location of the end points of this Line to
        /// the same as those end points of the specified Line.
        /// </summary>
        /// <param name="l">the specified Line.</param>
        public void SetLine(Line l)
        {
            SetLine(l.X1, l.Y1, l.X2, l.Y2);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns an integer Rectangle that completely encloses the
        /// IShape.  
        /// </summary>
        /// <remarks>
        /// remember that there is no guarantee that the
        /// returned Rectangle is the smallest bounding box that
        /// encloses the IShape, only that the IShape
        /// lies entirely within the indicated  Rectangle.  The
        /// returned Rectangle might also fail to completely
        /// enclose the IShape if the IShape overflows
        /// the limited range of the integer data type.  The
        /// getBounds method generally returns a
        /// tighter bounding box due to its greater flexibility in
        /// representation.
        /// </remarks>
        /// <returns>
        /// an integer Rectangle that completely encloses
        /// the IShape.
        /// </returns>
        public Rectangle Bounds
        {
            get
            {
                int x, y, w, h;
                if (X1 < X2)
                {
                    x = X1;
                    w = X2 - X1;
                }
                else
                {
                    x = X2;
                    w = X1 - X2;
                }
                if (Y1 < Y2)
                {
                    y = Y1;
                    h = Y2 - Y1;
                }
                else
                {
                    y = Y2;
                    h = Y1 - Y2;
                }
                return new Rectangle(x, y, w, h);
            }
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns an indicator of where the specified point
        /// (px,py) lies with respect to the line segment from
        /// (x1,y1) to (x2,y2).
        /// </summary>
        /// <remarks>
        /// The return value can be either 1, -1, or 0 and indicates
        /// in which direction the specified line must pivot around its
        /// first end point, (x1,y1), in order to point at the
        /// specified point (px,py).
        /// A return value of 1 indicates that the line segment must
        /// turn in the direction that takes the positive X axis towards
        /// the negative Y axis.  In the default coordinate system used by
        /// Java 2D, this direction is counterclockwise.
        /// A return value of -1 indicates that the line segment must
        /// turn in the direction that takes the positive X axis towards
        /// the positive Y axis.  In the default coordinate system, this
        /// direction is clockwise.
        /// A return value of 0 indicates that the point lies
        /// exactly on the line segment.  remember that an indicator value
        /// of 0 is rare and not useful for determining colinearity
        /// because of floating point rounding issues.
        /// If the point is colinear with the line segment, but
        /// not between the end points, then the value will be -1 if the point
        /// lies "beyond (x1,y1)" or 1 if the point lies
        /// "beyond (x2,y2)".
        /// </remarks>
        /// <param name="x1">the X coordinate of the start point of the
        ///            specified line segment.</param>
        /// <param name="y1">the Y coordinate of the start point of the
        ///            specified line segment</param>
        /// <param name="x2">the X coordinate of the end point of the
        ///            specified line segment</param>
        /// <param name="y2">the Y coordinate of the end point of the
        ///            specified line segment.</param>
        /// <param name="px">the X coordinate of the specified point to be
        ///            compared with the specified line segment.</param>
        /// <param name="py">the Y coordinate of the specified point to be
        ///            compared with the specified line segment.</param>
        /// <returns></returns>
        public static int RelativeCcw(int x1, int y1,
                                      int x2, int y2,
                                      int px, int py)
        {
            x2 -= x1;
            y2 -= y1;
            px -= x1;
            py -= y1;
            int ccw = px*y2 - py*x2;
            if (ccw == 0)
            {
                // The point is colinear, classify based on which side of
                // the segment the point falls on.  We can calculate a
                // relative Value using the projection of px,py onto the
                // segment - a negative Value indicates the point projects
                // outside of the segment in the direction of the particular
                // endpoint used as the origin for the projection.
                ccw = px*x2 + py*y2;
                if (ccw > 0)
                {
                    // Reverse the projection to be relative to the original X2,y2
                    // X2 and y2 are simply negated.
                    // px and py need to have (X2 - x1) or (y2 - y1) subtracted
                    //    from them (based on the original values)
                    // Since we really want to get a positive answer when the
                    //    point is "beyond (X2,y2)", then we want to calculate
                    //    the inverse anyway - thus we leave X2 & y2 negated.
                    px -= x2;
                    py -= y2;
                    ccw = px*x2 + py*y2;
                    if (ccw < 0)
                    {
                        ccw = 0;
                    }
                }
            }
            return (ccw < 0) ? -1 : ((ccw > 0) ? 1 : 0);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns an indicator of where the specified point
        /// (px,py) lies with respect to this line segment.
        /// </summary>
        /// <remarks>
        /// See the method comments of
        /// relativeCCW(int, int, int, int, int, int)
        /// to interpret the return value.
        /// </remarks>
        /// <param name="px">the X coordinate of the specified point
        ///            to be compared with this Line</param>
        /// <param name="py">the Y coordinate of the specified point
        ///            to be compared with this Line</param>
        /// <returns>an integer that indicates the position of the specified
        ///          coordinates with respect to this Line</returns>
        public int RelativeCcw(int px, int py)
        {
            return RelativeCcw(X1, Y1, X2, Y2, px, py);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns an indicator of where the specified Point
        /// lies with respect to this line segment.
        /// </summary>
        /// <remarks>
        /// See the method comments of
        /// relativeCCW(int, int, int, int, int, int)
        /// to interpret the return value.
        /// </remarks>
        /// <param name="p">the specified Point to be compared
        ///           with this Line</param>
        /// <returns>an integer that indicates the position of the specified
        ///         Point with respect to this Line</returns>
        public int RelativeCcw(Point p)
        {
            return RelativeCcw(X1, Y1, X2, Y2,
                               p.X, p.Y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tests if the line segment from (x1,y1) to
        /// (x2,y2) intersects the line segment from (x3,y3)
        /// to (x4,y4).
        /// </summary>
        /// <param name="x1">the X coordinate of the start point of the first
        ///            specified line segment</param>
        /// <param name="y1">the Y coordinate of the start point of the first
        ///            specified line segment</param>
        /// <param name="x2">the X coordinate of the end point of the first
        ///            specified line segment</param>
        /// <param name="y2">the Y coordinate of the end point of the first
        ///            specified line segment.</param>
        /// <param name="x3">the X coordinate of the start point of the second
        ///            specified line segment</param>
        /// <param name="y3">the Y coordinate of the start point of the second
        ///            specified line segment</param>
        /// <param name="x4">the X coordinate of the end point of the second
        ///           specified line segment</param>
        /// <param name="y4">the Y coordinate of the end point of the second
        ///            specified line segment</param>
        /// <returns>true if the first specified line segment
        /// 			and the second specified line segment intersect
        /// 			each other; false otherwise.</returns>
        public static bool LinesIntersect(int x1, int y1,
                                          int x2, int y2,
                                          int x3, int y3,
                                          int x4, int y4)
        {
            return ((RelativeCcw(x1, y1, x2, y2, x3, y3)*
                     RelativeCcw(x1, y1, x2, y2, x4, y4) <= 0)
                    && (RelativeCcw(x3, y3, x4, y4, x1, y1)*
                        RelativeCcw(x3, y3, x4, y4, x2, y2) <= 0));
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tests if the line segment from (x1,y1) to (x2,y2) intersects this line segment.
        /// </summary>
        /// <param name="x1">the X coordinate of the start point of the
        ///            specified line segment</param>
        /// <param name="y1">the Y coordinate of the start point of the
        ///            specified line segment</param>
        /// <param name="x2">the X coordinate of the end point of the
        ///            specified line segment</param>
        /// <param name="y2">the Y coordinate of the end point of the
        ///            specified line segment</param>
        /// <returns>true,if this line segment and the specified line segment
        /// intersect each other; false otherwise.</returns>
        public bool IntersectsLine(int x1, int y1, int x2, int y2)
        {
            return LinesIntersect(x1, y1, x2, y2,
                                  X1, Y1, X2, Y2);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tests if the specified line segment intersects this line segment.
        /// </summary>
        /// <param name="l">the specified Line</param>
        /// <returns>true if this line segment and the specified line
        /// 		segment intersect each other;
        /// 		false otherwise.</returns>
        public bool IntersectsLine(Line l)
        {
            return LinesIntersect(l.X1, l.Y1, l.X2, l.Y2,
                                  X1, Y1, X2, Y2);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the square of the distance from a point to a line segment.
        /// The distance measured is the distance between the specified
        /// point and the closest point between the specified end points.
        /// </summary>
        /// <remarks>
        /// If the specified point intersects the line segment in between the
        /// end points, this method returns 0.
        /// </remarks>
        /// <param name="x1">the X coordinate of the start point of the
        ///            specified line segment.</param>
        /// <param name="y1">the Y coordinate of the start point of the
        ///            specified line segment</param>
        /// <param name="x2">the X coordinate of the end point of the
        ///            specified line segment.</param>
        /// <param name="y2">the Y coordinate of the end point of the
        ///            specified line segment</param>
        /// <param name="px">the X coordinate of the specified point being
        ///            measured against the specified line segment.</param>
        /// <param name="py">the Y coordinate of the specified point being
        ///            measured against the specified line segment.</param>
        /// <returns>a int value that is the square of the distance from the
        /// 	specified point to the specified line segment.</returns>
        public static int PtSegDistSq(int x1, int y1,
                                      int x2, int y2,
                                      int px, int py)
        {
            // Adjust vectors relative to x1,y1
            // X2,y2 becomes relative vector from x1,y1 to end of segment
            x2 -= x1;
            y2 -= y1;
            // px,py becomes relative vector from x1,y1 to test point
            px -= x1;
            py -= y1;
            int dotprod = px*x2 + py*y2;
            int projlenSq;
            if (dotprod <= 0)
            {
                // px,py is on the side of x1,y1 away from X2,y2
                // distance to segment is length of px,py vector
                // "length of its (clipped) projection" is now 0.0
                projlenSq = 0;
            }
            else
            {
                // switch to backwards vectors relative to X2,y2
                // X2,y2 are already the negative of x1,y1=>X2,y2
                // to get px,py to be the negative of px,py=>X2,y2
                // the dot product of two negated vectors is the same
                // as the dot product of the two normal vectors
                px = x2 - px;
                py = y2 - py;
                dotprod = px*x2 + py*y2;
                if (dotprod <= 0)
                {
                    // px,py is on the side of X2,y2 away from x1,y1
                    // distance to segment is length of (backwards) px,py vector
                    // "length of its (clipped) projection" is now 0.0
                    projlenSq = 0;
                }
                else
                {
                    // px,py is between x1,y1 and X2,y2
                    // dotprod is the length of the px,py vector
                    // projected on the X2,y2=>x1,y1 vector times the
                    // length of the X2,y2=>x1,y1 vector
                    projlenSq = dotprod*dotprod/(x2*x2 + y2*y2);
                }
            }
            // Distance to line is now the length of the relative point
            // vector minus the length of its projection onto the line
            // (which is zero if the projection falls outside the range
            //  of the line segment).
            int lenSq = px*px + py*py - projlenSq;
            if (lenSq < 0)
            {
                lenSq = 0;
            }
            return lenSq;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the distance from a point to a line segment.
        /// The distance measured is the distance between the specified
        /// point and the closest point between the specified end points.
        /// </summary>
        /// <remarks>
        /// If the specified point intersects the line segment in between the
        /// end points, this method returns 0.
        /// </remarks>
        /// <param name="x1">the X coordinate of the start point of the
        ///            specified line segment.</param>
        /// <param name="y1">the Y coordinate of the start point of the
        ///            specified line segment</param>
        /// <param name="x2">the X coordinate of the end point of the
        ///            specified line segment.</param>
        /// <param name="y2">the Y coordinate of the end point of the
        ///            specified line segment</param>
        /// <param name="px">the X coordinate of the specified point being
        ///            measured against the specified line segment.</param>
        /// <param name="py">the Y coordinate of the specified point being
        ///            measured against the specified line segment.</param>
        /// <returns>a int value that is the distance from the specified point
        /// 			to the specified line segment.</returns>
        public static int PtSegDist(int x1, int y1,
                                    int x2, int y2,
                                    int px, int py)
        {
            long dis = PtSegDistSq(x1, y1, x2, y2, px, py);
            dis <<= MathFP.DefaultPrecision;
            return MathFP.ToInt(MathFP.Sqrt(dis));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the distance from a point to this line segment.
        /// The distance measured is the distance between the specified
        /// point and the closest point between the current line's end points.
        /// </summary>
        /// <remarks>
        /// If the specified point intersects the line segment in between the
        /// end points, this method returns 0.0.
        /// </remarks>
        /// <param name="px">the X coordinate of the specified point being
        ///            measured against this line segment.</param>
        /// <param name="py">the Y coordinate of the specified point being
        ///            measured against this line segment.</param>
        /// <returns>a int value that is the distance from the specified
        /// 			point to the current line segment.</returns>
        public int PtSegDist(int px, int py)
        {
            return PtSegDist(X1, Y1, X2, Y2, px, py);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the distance from a Point to this line segment.
        /// </summary>
        /// <remarks>
        /// The distance measured is the distance between the specified
        /// point and the closest point between the current line's end points.
        /// If the specified point intersects the line segment in between the
        /// end points, this method returns 0.
        /// </remarks>
        /// <param name="pt">the specified Point being measured
        /// 	against this line segment</param>
        /// <returns>a int value that is the distance from the specified
        /// Point to the current line segment.</returns>
        public int PtSegDist(Point pt)
        {
            return PtSegDist(X1, Y1, X2, Y2,
                             pt.X, pt.Y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the square of the distance from a point to this line segment.
        /// The distance measured is the distance between the specified
        /// point and the closest point between the current line's end points.
        /// </summary>
        /// <remarks>
        /// If the specified point intersects the line segment in between the
        /// end points, this method returns 0.0.
        /// </remarks>
        /// <param name="px">the X coordinate of the specified point being
        ///           measured against this line segment</param>
        /// <param name="py">the Y coordinate of the specified point being
        ///           measured against this line segment</param>
        /// <returns>a int value that is the square of the distance from the
        /// 	specified point to the current line segment.</returns>
        public int PtSegDistSq(int px, int py)
        {
            return PtSegDistSq(X1, Y1, X2, Y2, px, py);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the square of the distance from a Point to
        /// this line segment.
        /// </summary>
        /// <remarks>
        /// The distance measured is the distance between the specified
        /// point and the closest point between the current line's end points.
        /// If the specified point intersects the line segment in between the
        /// end points, this method returns 0.0.
        /// </remarks>
        /// <param name="pt">the specified Point being measured against
        ///          this line segment..</param>
        /// <returns>a int value that is the square of the distance from the
        /// specified Point to the current	line segment.</returns>
        public int PtSegDistSq(Point pt)
        {
            return PtSegDistSq(X1, Y1, X2, Y2,
                               pt.X, pt.Y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the square of the distance from a point to a line.
        /// </summary>
        /// <remarks>
        /// The distance measured is the distance between the specified
        /// point and the closest point on the infinitely-extended line
        /// defined by the specified coordinates.  If the specified point
        /// intersects the line, this method returns 0.
        /// </remarks>
        /// <param name="x1">the X coordinate of the start point of the specified line.</param>
        /// <param name="y1">the Y coordinate of the start point of the specified line</param>
        /// <param name="x2">the X coordinate of the end point of the specified line</param>
        /// <param name="y2">the Y coordinate of the end point of the specified line.</param>
        /// <param name="px">the X coordinate of the specified point being
        ///            measured against the specified line</param>
        /// <param name="py">the Y coordinate of the specified point being
        ///            measured against the specified line</param>
        /// <returns>a int value that is the square of the distance from the
        /// 			specified point to the specified line.</returns>
        public static int PtLineDistSq(int x1, int y1,
                                       int x2, int y2,
                                       int px, int py)
        {
            // Adjust vectors relative to x1,y1
            // X2,y2 becomes relative vector from x1,y1 to end of segment
            x2 -= x1;
            y2 -= y1;
            // px,py becomes relative vector from x1,y1 to test point
            px -= x1;
            py -= y1;
            int dotprod = px*x2 + py*y2;
            // dotprod is the length of the px,py vector
            // projected on the x1,y1=>X2,y2 vector times the
            // length of the x1,y1=>X2,y2 vector
            int projlenSq = dotprod*dotprod/(x2*x2 + y2*y2);
            // Distance to line is now the length of the relative point
            // vector minus the length of its projection onto the line
            int lenSq = px*px + py*py - projlenSq;
            if (lenSq < 0)
            {
                lenSq = 0;
            }
            return lenSq;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the square of the distance from a specified
        /// Point to this line.
        /// </summary>
        /// <remarks>
        /// The distance measured is the distance between the specified
        /// point and the closest point on the infinitely-extended line
        /// defined by this Line.  If the specified point
        /// intersects the line, this method returns 0.0.
        /// </remarks>
        /// <param name="pt">the specified Point being measured
        ///            against this line</param>
        /// <returns>a int value that is the square of the distance from a
        /// specified Point to the current	line.</returns>
        public int PtLineDistSq(Point pt)
        {
            return PtLineDistSq(X1, Y1, X2, Y2,
                                pt.X, pt.Y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the square of the distance from a point to this line.
        /// </summary>
        /// <remarks>
        /// The distance measured is the distance between the specified
        /// point and the closest point on the infinitely-extended line
        /// defined by this Line.  If the specified point
        /// intersects the line, this method returns 0.
        /// </remarks>
        /// <param name="px">the X coordinate of the specified point being
        ///            measured against this line.</param>
        /// <param name="py">the Y coordinate of the specified point being
        ///           measured against this line.</param>
        /// <returns>a int value that is the square of the distance from a
        /// 		specified point to the current line.</returns>
        public int PtLineDistSq(int px, int py)
        {
            return PtLineDistSq(X1, Y1, X2, Y2, px, py);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the distance from a point to a line.
        /// </summary>
        /// <remarks>
        /// The distance measured is the distance between the specified
        /// point and the closest point on the infinitely-extended line
        /// defined by the specified coordinates.  If the specified point
        /// intersects the line, this method returns 0.
        /// </remarks>
        /// <param name="x1">the X coordinate of the start point of the specified line</param>
        /// <param name="y1">the Y coordinate of the start point of the specified line.</param>
        /// <param name="x2">the X coordinate of the end point of the specified line.</param>
        /// <param name="y2">the Y coordinate of the end point of the specified line</param>
        /// <param name="px">the X coordinate of the specified point being
        ///            measured against the specified line</param>
        /// <param name="py">the Y coordinate of the specified point being
        ///            measured against the specified line</param>
        /// <returns>a int value that is the distance from the specified
        /// 			 point to the specified line.</returns>
        public static int PtLineDist(int x1, int y1,
                                     int x2, int y2,
                                     int px, int py)
        {
            long dis = PtLineDistSq(x1, y1, x2, y2, px, py);
            dis <<= MathFP.DefaultPrecision;
            return MathFP.ToInt(MathFP.Sqrt(dis));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the distance from a point to this line.
        /// </summary>
        /// <remarks>
        /// The distance measured is the distance between the specified
        /// point and the closest point on the infinitely-extended line
        /// defined by this Line.  If the specified point
        /// intersects the line, this method returns 0.
        /// </remarks>
        /// <param name="px">the X coordinate of the specified point being
        ///            measured against this line</param>
        /// <param name="py">the Y coordinate of the specified point being
        ///            measured against this line</param>
        /// <returns>a int value that is the distance from a specified point
        /// 			to the current line.</returns>
        public int PtLineDist(int px, int py)
        {
            return PtLineDist(X1, Y1, X2, Y2, px, py);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the distance from a Point to this line.
        /// </summary>
        /// <remarks>
        /// The distance measured is the distance between the specified
        /// point and the closest point on the infinitely-extended line
        /// defined by this Line.  If the specified point
        /// intersects the line, this method returns 0.
        /// </remarks>
        /// <param name="pt">the specified Point being measured</param>
        /// <returns>a int value that is the distance from a specified
        /// 		Point to the current line.</returns>
        public int PtLineDist(Point pt)
        {
            return PtLineDist(X1, Y1, X2, Y2,
                              pt.X, pt.Y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tests if the specified coordinates are inside the boundary of the
        /// IShape.
        /// </summary>
        /// <param name="x">the specified X coordinate to be tested</param>
        /// <param name="y">the specified Y coordinate to be tested.</param>
        /// <returns>
        /// 	true if the specified coordinates are inside
        /// the IShape boundary; false
        /// otherwise.
        /// </returns>
        public bool Contains(int x, int y)
        {
            return false;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tests if a specified Point is inside the boundary
        /// of the IShape.
        /// </summary>
        /// <param name="p">the specified Point to be tested</param>
        /// <returns>
        /// 	true if the specified Point is
        /// inside the boundary of the IShape;
        /// false otherwise.
        /// </returns>
        public bool Contains(Point p)
        {
            return false;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tests if the interior of the IShape entirely contains the
        /// specified Rectangle.
        /// </summary>
        /// <remarks>
        /// The IShape.contains() method allows a IShape
        /// implementation to conservatively return false when:
        /// <ul>
        /// 		<li>
        /// the intersect method returns true and
        /// </li>
        /// the calculations to determine whether or not the
        /// IShape entirely contains the Rectangle
        /// are prohibitively expensive.
        /// </ul>
        /// This means that for some Shapes this method might
        /// return false even though the IShape contains
        /// the Rectangle.
        /// The com.mapdigit.drawing.geometry.Area class performs
        /// more accurate geometric computations than most
        /// IShape objects and therefore can be used if a more precise
        /// answer is required.
        /// </remarks>
        /// <param name="r">The specified Rectangle</param>
        /// <returns>
        /// 	true if the interior of the IShape
        /// entirely contains the Rectangle;
        /// false otherwise or, if the IShape
        /// contains the Rectangle and the
        /// intersects method returns true
        /// and the containment calculations would be too expensive to
        /// perform.
        /// </returns>
        public bool Contains(Rectangle r)
        {
            return false;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tests if the interior of the IShape entirely contains
        /// the specified rectangular area. 
        /// </summary>
        /// <remarks>
        ///  All coordinates that lie inside
        /// the rectangular area must lie within the IShape for the
        /// entire rectanglar area to be considered contained within the
        /// IShape.
        /// The IShape.contains() method allows a IShape
        /// implementation to conservatively return false when:
        /// <ul>
        /// 		<li>
        /// the intersect method returns true and
        /// </li>
        /// the calculations to determine whether or not the
        /// IShape entirely contains the rectangular area are
        /// prohibitively expensive.
        /// </ul>
        /// This means that for some Shapes this method might
        /// return false even though the IShape contains
        /// the rectangular area.
        /// The com.mapdigit.drawing.geometry.Area class performs
        /// more accurate geometric computations than most
        /// IShape objects and therefore can be used if a more precise
        /// answer is required.
        /// </remarks>
        /// <param name="x">X coordinate of the upper-left corner
        /// of the specified rectangular area</param>
        /// <param name="y">the Y coordinate of the upper-left corner
        /// of the specified rectangular area</param>
        /// <param name="w">Tthe width of the specified rectangular area.</param>
        /// <param name="h">the height of the specified rectangular area.</param>
        /// <returns>
        /// 	true if the interior of the IShape
        /// entirely contains the specified rectangular area;
        /// false otherwise or, if the IShape
        /// contains the rectangular area and the
        /// intersects method returns true
        /// and the containment calculations would be too expensive to
        /// perform.
        /// </returns>
        public bool Contains(int x, int y, int w, int h)
        {
            return false;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tests if the interior of the IShape intersects the
        /// interior of a specified rectangular area.
        /// </summary>
        /// <remarks>
        /// The rectangular area is considered to intersect the IShape
        /// if any point is contained in both the interior of the
        /// IShape and the specified rectangular area.
        /// The IShape.intersects() method allows a IShape
        /// implementation to conservatively return true when:
        /// <ul>
        /// 		<li>
        /// there is a high probability that the rectangular area and the
        /// IShape intersect, but
        /// </li>
        /// the calculations to accurately determine this intersection
        /// are prohibitively expensive.
        /// </ul>
        /// This means that for some Shapes this method might
        /// return true even though the rectangular area does not
        /// intersect the IShape.
        /// The com.mapdigit.drawing.geometry.Area class performs
        /// more accurate computations of geometric intersection than most
        /// IShape objects and therefore can be used if a more precise
        /// answer is required.
        /// </remarks>
        /// <param name="x">the X coordinate of the upper-left corner
        /// of the specified rectangular area</param>
        /// <param name="y">the Y coordinate of the upper-left corner
        /// of the specified rectangular area</param>
        /// <param name="w">Tthe width of the specified rectangular area</param>
        /// <param name="h">the height of the specified rectangular area</param>
        /// <returns>
        /// 	true if the interior of the IShape and
        /// the interior of the rectangular area intersect, or are
        /// both highly likely to intersect and intersection calculations
        /// would be too expensive to perform; false
        /// </returns>
        public bool Intersects(int x, int y, int w, int h)
        {
            return Intersects(new Rectangle(x, y, w, h));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tests if the interior of the IShape intersects the
        /// interior of a specified Rectangle.
        /// </summary>
        /// <remarks>
        /// The IShape.intersects() method allows a IShape
        /// implementation to conservatively return true when:
        /// <ul>
        /// 		<li>
        /// there is a high probability that the Rectangle and the
        /// IShape intersect, but
        /// </li>
        /// the calculations to accurately determine this intersection
        /// are prohibitively expensive.
        /// </ul>
        /// This means that for some Shapes this method might
        /// return true even though the Rectangle does not
        /// intersect the IShape.
        /// The com.mapdigit.drawing.geometry.Area class performs
        /// more accurate computations of geometric intersection than most
        /// IShape objects and therefore can be used if a more precise
        /// answer is required.
        /// </remarks>
        /// <param name="r">r the specified Rectangle.</param>
        /// <returns>
        /// 	true if the interior of the IShape and
        /// the interior of the specified Rectangle
        /// intersect, or are both highly likely to intersect and intersection
        /// calculations would be too expensive to perform; false
        /// otherwise.
        /// </returns>
        public bool Intersects(Rectangle r)
        {
            return r.IntersectsLine(X1, Y1, X2, Y2);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns an iterator object that iterates along the
        /// IShape boundary and provides access to the geometry of the
        /// IShape outline. 
        /// </summary>
        /// <remarks>
        ///  If an optional AffineTransform
        /// is specified, the coordinates returned in the iteration are
        /// transformed accordingly.
        /// Each call to this method returns a fresh IPathIterator
        /// object that traverses the geometry of the IShape object
        /// independently from any other IPathIterator objects in use
        /// at the same time.
        /// It is recommended, but not guaranteed, that objects
        /// implementing the IShape interface isolate iterations
        /// that are in process from any changes that might occur to the original
        /// object's geometry during such iterations.
        /// </remarks>
        /// <param name="at">an optional AffineTransform to be applied to the
        /// coordinates as they are returned in the iteration, or
        /// null if untransformed coordinates are desired</param>
        /// <returns>
        /// a new IPathIterator object, which independently
        /// traverses the geometry of the IShape.
        /// </returns>
        public PathIterator GetPathIterator(AffineTransform at)
        {
            return new LineIterator(this, at);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns an iterator object that iterates along the IShape
        /// boundary and provides access to a flattened view of the
        /// IShape outline geometry.
        /// </summary>
        /// <remarks>
        /// Only SegMoveto, SegLineto, and SegClose point types are
        /// returned by the iterator.
        /// If an optional AffineTransform is specified,
        /// the coordinates returned in the iteration are transformed
        /// accordingly.
        /// The amount of subdivision of the curved segments is controlled
        /// by the flatness parameter, which specifies the
        /// maximum distance that any point on the unflattened transformed
        /// curve can deviate from the returned flattened path segments.
        /// remember that a limit on the accuracy of the flattened path might be
        /// silently imposed, causing very small flattening parameters to be
        /// treated as larger values.  This limit, if there is one, is
        /// defined by the particular implementation that is used.
        /// Each call to this method returns a fresh IPathIterator
        /// object that traverses the IShape object geometry
        /// independently from any other IPathIterator objects in use at
        /// the same time.
        /// It is recommended, but not guaranteed, that objects
        /// implementing the IShape interface isolate iterations
        /// that are in process from any changes that might occur to the original
        /// object's geometry during such iterations.
        /// </remarks>
        /// <param name="at">an optional AffineTransform to be applied to the
        /// coordinates as they are returned in the iteration, or
        /// null if untransformed coordinates are desired</param>
        /// <param name="flatness">the maximum distance that the line segments used to
        /// approximate the curved segments are allowed to deviate
        /// from any point on the original curve</param>
        /// <returns>
        /// a new IPathIterator that independently traverses
        /// a flattened view of the geometry of the  IShape.
        /// </returns>
        public PathIterator GetPathIterator(AffineTransform at, int flatness)
        {
            return new LineIterator(this, at);
        }
    }
}