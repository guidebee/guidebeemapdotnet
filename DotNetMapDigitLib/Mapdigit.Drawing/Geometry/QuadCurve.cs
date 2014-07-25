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
    ///  A quadratic parametric curve segment
    /// </summary>
    public class QuadCurve : IShape
    {

        /// <summary>
        /// The X coordinate of the start point of the quadratic curve segment.
        /// </summary>
        public double X1;

        /// <summary>
        /// The Y coordinate of the start point of the quadratic curve segment.
        /// </summary>
        public double Y1;

        /// <summary>
        ///  The X coordinate of the control point of the quadratic curve segment.
        /// </summary>
        public double Ctrlx;

        /// <summary>
        ///  The Y coordinate of the control point of the quadratic curve segment.
        /// </summary>
        /// 
        public double Ctrly;

        /// <summary>
        ///  The X coordinate of the end point of the quadratic curve segment.
        /// </summary>
        public double X2;

        /// <summary>
        ///  The Y coordinate of the end point of the quadratic curve segment.
        /// </summary>
        public double Y2;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="QuadCurve"/> class.
        /// </summary>
        public QuadCurve()
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs and initializes a QuadCurve from the
        /// specified double coordinates.
        /// </summary>
        /// <param name="x1">the X coordinate of the start point.</param>
        /// <param name="y1">the Y coordinate of the start point.</param>
        /// <param name="ctrlx">the X coordinate of the control point.</param>
        /// <param name="ctrly">the Y coordinate of the control point.</param>
        /// <param name="x2">the X coordinate of the end point</param>
        /// <param name="y2">the Y coordinate of the end point.</param>
        public QuadCurve(double x1, double y1,
                         double ctrlx, double ctrly,
                         double x2, double y2)
        {
            SetCurve(x1, y1, ctrlx, ctrly, x2, y2);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Returns the X coordinate of the start poin
        /// </summary>
        public int IntX1
        {
            get { return (int) (X1 + .5); }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the Y coordinate of the start point
        /// </summary>
        public int IntY1
        {
            get { return (int) (Y1 + .5); }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the start point.
        /// </summary>
        public Point P1
        {
            get { return new Point((int) (X1 + .5), (int) (Y1 + .5)); }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the X coordinate of the control point
        /// </summary>
        public int IntCtrlX
        {
            get { return (int) (Ctrlx + .5); }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the Y coordinate of the control point
        /// </summary>
        public int IntCtrlY
        {
            get { return (int) (Ctrly + .5); }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the control point.
        /// </summary>
        public Point CtrlPt
        {
            get { return new Point((int) (Ctrlx + .5), (int) (Ctrly + .5)); }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the X coordinate of the end point
        /// </summary>
        public int IntX2
        {
            get { return (int) (X2 + .5); }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the Y coordinate of the end point
        /// </summary>
        public int IntY2
        {
            get { return (int) (Y2 + .5); }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the end point.
        /// </summary>
        public Point P2
        {
            get { return new Point((int) (X2 + .5), (int) (Y2 + .5)); }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the location of the end points and control point of this curve
        /// to the specified double coordinates.
        /// </summary>
        /// <param name="x1">the X coordinate of the start point.</param>
        /// <param name="y1">the Y coordinate of the start point.</param>
        /// <param name="ctrlx">T the X coordinate of the control point.</param>
        /// <param name="ctrly"> the Y coordinate of the control point.</param>
        /// <param name="x2">the X coordinate of the end point.</param>
        /// <param name="y2">the Y coordinate of the end point.</param>
        public void SetCurve(double x1, double y1,
                             double ctrlx, double ctrly,
                             double x2, double y2)
        {
            X1 = x1;
            Y1 = y1;
            Ctrlx = ctrlx;
            Ctrly = ctrly;
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
        /// Returns an integer Rectangle that completely encloses the IShape. 
        /// </summary>
        /// <remarks>
        ///  remember that there is no guarantee that the
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
                double left = Math.Min(Math.Min(X1, X2), Ctrlx);
                double top = Math.Min(Math.Min(Y1, Y2), Ctrly);
                double right = Math.Max(Math.Max(X1, X2), Ctrlx);
                double bottom = Math.Max(Math.Max(Y1, Y2), Ctrly);
                return new Rectangle((int) (left + .5),
                                     (int) (top + .5),
                                     (int) (right - left + .5),
                                     (int) (bottom - top + .5));
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the location of the end points and control points of this
        /// QuadCurve to the double coordinates at
        /// the specified offset in the specified array.
        /// </summary>
        /// <param name="coords">the array containing coordinate values.</param>
        /// <param name="offset">the index into the array from which to start
        /// getting the coordinate values and assigning them to this
        /// 	QuadCurve.</param>
        public void SetCurve(double[] coords, int offset)
        {
            SetCurve(coords[offset + 0], coords[offset + 1],
                     coords[offset + 2], coords[offset + 3],
                     coords[offset + 4], coords[offset + 5]);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the location of the end points and control point of this
        /// QuadCurve to the specified Point
        /// coordinates.
        /// </summary>
        /// <param name="p1">the start point</param>
        /// <param name="cp">the control point.</param>
        /// <param name="p2">the end point.</param>
        public void SetCurve(Point p1, Point cp, Point p2)
        {
            SetCurve(p1.X, p1.Y,
                     cp.X, cp.Y,
                     p2.X, p2.Y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the location of the end points and control points of this
        /// QuadCurve to the coordinates of the
        /// Point objects at the specified offset in
        /// the specified array.
        /// </summary>
        /// <param name="pts">an array containing Point that define
        /// 		coordinate values.</param>
        /// <param name="offset"> the index into pts from which to start
        /// 	getting the coordinate values and assigning them to this
        /// 	QuadCurve.</param>
        public void SetCurve(Point[] pts, int offset)
        {
            SetCurve(pts[offset + 0].X, pts[offset + 0].Y,
                     pts[offset + 1].X, pts[offset + 1].Y,
                     pts[offset + 2].X, pts[offset + 2].Y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the location of the end points and control point of this
        /// QuadCurve to the same as those in the specified
        /// QuadCurve.
        /// </summary>
        /// <param name="c">the specified QuadCurve</param>
        public void SetCurve(QuadCurve c)
        {
            SetCurve(c.IntX1, c.IntY1,
                     c.IntCtrlX, c.IntCtrlY,
                     c.IntX2, c.IntY2);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the square of the flatness, or maximum distance of a
        /// control point from the line connecting the end points, of the
        /// quadratic curve specified by the indicated control points.
        /// </summary>
        /// <param name="x1">the X coordinate of the start point</param>
        /// <param name="y1">the Y coordinate of the start point</param>
        /// <param name="ctrlx">the X coordinate of the control point</param>
        /// <param name="ctrly">the Y coordinate of the control point.</param>
        /// <param name="x2">the X coordinate of the end point.</param>
        /// <param name="y2">the Y coordinate of the end point.</param>
        /// <returns>the square of the flatness of the quadratic curve
        /// 		defined by the specified coordinates.</returns>
        public static int GetFlatnessSq(int x1, int y1,
                                        int ctrlx, int ctrly,
                                        int x2, int y2)
        {
            return Line.PtSegDistSq(x1, y1, x2, y2, ctrlx, ctrly);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the flatness, or maximum distance of a
        /// control point from the line connecting the end points, of the
        /// quadratic curve specified by the indicated control points.
        /// </summary>
        /// <param name="x1">the X coordinate of the start point</param>
        /// <param name="y1">the Y coordinate of the start point</param>
        /// <param name="ctrlx">the X coordinate of the control point</param>
        /// <param name="ctrly">the Y coordinate of the control point.</param>
        /// <param name="x2">the X coordinate of the end point.</param>
        /// <param name="y2">the Y coordinate of the end point.</param>
        /// <returns>the flatness of the quadratic curve defined by the
        /// 		specified coordinates.</returns>
        public static int GetFlatness(int x1, int y1,
                                      int ctrlx, int ctrly,
                                      int x2, int y2)
        {
            return Line.PtSegDist(x1, y1, x2, y2, ctrlx, ctrly);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the square of the flatness, or maximum distance of a
        /// control point from the line connecting the end points, of the
        /// quadratic curve specified by the control points stored in the
        /// indicated array at the indicated index.
        /// </summary>
        /// <param name="coords">an array containing coordinate values.</param>
        /// <param name="offset">the index into coords from which to
        /// 		to start getting the values from the array.</param>
        /// <returns>the flatness of the quadratic curve that is defined by the
        /// 		values in the specified array at the specified index.</returns>
        public static int GetFlatnessSq(int[] coords, int offset)
        {
            return Line.PtSegDistSq(coords[offset + 0], coords[offset + 1],
                                    coords[offset + 4], coords[offset + 5],
                                    coords[offset + 2], coords[offset + 3]);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the flatness, or maximum distance of a
        /// control point from the line connecting the end points, of the
        /// quadratic curve specified by the control points stored in the
        /// indicated array at the indicated index.
        /// </summary>
        /// <param name="coords">an array containing coordinate values.</param>
        /// <param name="offset">the index into coords from which to
        /// 		to start getting the values from the array.</param>
        /// <returns>the flatness of a quadratic curve defined by the
        /// 	specified array at the specified offset.</returns>
        public static int GetFlatness(int[] coords, int offset)
        {
            return Line.PtSegDist(coords[offset + 0], coords[offset + 1],
                                  coords[offset + 4], coords[offset + 5],
                                  coords[offset + 2], coords[offset + 3]);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Returns the square of the flatness, or maximum distance of a
        /// control point from the line connecting the end points, of this
        /// QuadCurve.
        /// </summary>
        /// <returns>the square of the flatness of this
        /// QuadCurve.</returns>
        public double GetFlatnessSq()
        {
            return Line.PtSegDistSq(IntX1, IntY1,
                                    IntX2, IntY2,
                                    IntCtrlX, IntCtrlY);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the flatness, or maximum distance of a
        /// control point from the line connecting the end points, of this
        /// QuadCurve.
        /// </summary>
        /// <returns>the flatness of this QuadCurve.</returns>
        public double GetFlatness()
        {
            return Line.PtSegDist(IntX1, IntY1,
                                  IntX2, IntY2,
                                  IntCtrlX, IntCtrlY);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Subdivides this QuadCurve and stores the resulting
        /// two subdivided curves into the left and
        /// right curve parameters.
        /// </summary>
        /// <remarks>
        /// Either or both of the left and right
        /// objects can be the same as this QuadCurve or
        /// null.
        /// </remarks>
        /// <param name="left">the QuadCurve object for storing the
        /// left or first half of the subdivided curve</param>
        /// <param name="right">the QuadCurve object for storing the
        /// right or second half of the subdivided curve</param>
        public void Subdivide(QuadCurve left, QuadCurve right)
        {
            Subdivide(this, left, right);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Subdivides the quadratic curve specified by the src
        /// parameter and stores the resulting two subdivided curves into the
        /// left and right curve parameters.
        /// </summary>
        /// <remarks>
        /// Either or both of the left and right
        /// objects can be the same as the src object or
        /// null.
        /// </remarks>
        /// <param name="src">the quadratic curve to be subdivided.</param>
        /// <param name="left">the QuadCurve object for storing the
        /// 		left or first half of the subdivided curve.</param>
        /// <param name="right"> the QuadCurve object for storing the
        /// 	right or second half of the subdivided curve</param>
        public static void Subdivide(QuadCurve src,
                                     QuadCurve left,
                                     QuadCurve right)
        {
            double x1 = src.IntX1;
            double y1 = src.IntY1;
            double ctrlx = src.IntCtrlX;
            double ctrly = src.IntCtrlY;
            double x2 = src.IntX2;
            double y2 = src.IntY2;
            double ctrlx1 = (x1 + ctrlx)/2.0;
            double ctrly1 = (y1 + ctrly)/2.0;
            double ctrlx2 = (x2 + ctrlx)/2.0;
            double ctrly2 = (y2 + ctrly)/2.0;
            ctrlx = (ctrlx1 + ctrlx2)/2.0;
            ctrly = (ctrly1 + ctrly2)/2.0;
            if (left != null)
            {
                left.SetCurve(x1, y1, ctrlx1, ctrly1, ctrlx, ctrly);
            }
            if (right != null)
            {
                right.SetCurve(ctrlx, ctrly, ctrlx2, ctrly2, x2, y2);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Subdivides the quadratic curve specified by the coordinates
        /// stored in the src array at indices
        /// srcoff through srcoff+5
        /// and stores the resulting two subdivided curves into the two
        /// result arrays at the corresponding indices.
        /// </summary>
        /// <remarks>
        /// Either or both of the left and right
        /// arrays can be null or a reference to the same array
        /// and offset as the src array.
        /// Remember that the last point in the first subdivided curve is the
        /// same as the first point in the second subdivided curve.  Thus,
        /// it is possible to pass the same array for left and
        /// right and to use offsets such that
        /// rightoff equals leftoff + 4 in order
        /// to avoid allocating extra storage for this common point.
        /// </remarks>
        /// <param name="src">the array holding the coordinates for the source curve.</param>
        /// <param name="srcoff"> the offset into the array of the beginning of the
        ///  the 6 source coordinates.</param>
        /// <param name="left"> the array for storing the coordinates for the first
        ///  half of the subdivided curve</param>
        /// <param name="leftoff">the offset into the array of the beginning of the
        ///  the 6 left coordinates</param>
        /// <param name="right"> the array for storing the coordinates for the second
        ///  half of the subdivided curve</param>
        /// <param name="rightoff">the offset into the array of the beginning of the
        /// the 6 right coordinates.</param>
        public static void Subdivide(double[] src, int srcoff,
                                     double[] left, int leftoff,
                                     double[] right, int rightoff)
        {
            double x1 = src[srcoff + 0];
            double y1 = src[srcoff + 1];
            double ctrlx = src[srcoff + 2];
            double ctrly = src[srcoff + 3];
            double x2 = src[srcoff + 4];
            double y2 = src[srcoff + 5];
            if (left != null)
            {
                left[leftoff + 0] = x1;
                left[leftoff + 1] = y1;
            }
            if (right != null)
            {
                right[rightoff + 4] = x2;
                right[rightoff + 5] = y2;
            }
            x1 = (x1 + ctrlx)/2.0;
            y1 = (y1 + ctrly)/2.0;
            x2 = (x2 + ctrlx)/2.0;
            y2 = (y2 + ctrly)/2.0;
            ctrlx = (x1 + x2)/2.0;
            ctrly = (y1 + y2)/2.0;
            if (left != null)
            {
                left[leftoff + 2] = x1;
                left[leftoff + 3] = y1;
                left[leftoff + 4] = ctrlx;
                left[leftoff + 5] = ctrly;
            }
            if (right != null)
            {
                right[rightoff + 0] = ctrlx;
                right[rightoff + 1] = ctrly;
                right[rightoff + 2] = x2;
                right[rightoff + 3] = y2;
            }
        }

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Solves the quadratic whose coefficients are in the eqn
        /// array and places the non-complex roots back into the same array,
        /// returning the number of roots. 
        /// </summary>
        /// <remarks>
        ///  The quadratic solved is represented
        /// by the equation:
        /// <pre>
        ///     eqn = {C, B, A};
        ///     ax^2 + bx + c = 0
        /// </pre>
        /// A return value of -1 is used to distinguish a constant
        /// equation, which might be always 0 or never 0, from an equation that
        /// has no zeroes.
        /// </remarks>
        /// <param name="eqn">the array that contains the quadratic coefficients</param>
        /// <returns> the number of roots, or -1 if the equation is
        /// 		a constant</returns>
        public static int SolveQuadratic(double[] eqn)
        {
            return SolveQuadratic(eqn, eqn);
        }

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Solves the quadratic whose coefficients are in the eqn
        /// array and places the non-complex roots into the res
        /// array, returning the number of roots.
        /// </summary>
        /// <remarks>
        /// The quadratic solved is represented by the equation:
        /// <pre>
        ///     eqn = {C, B, A};
        ///     ax^2 + bx + c = 0
        /// </pre>
        /// A return value of -1 is used to distinguish a constant
        /// equation, which might be always 0 or never 0, from an equation that
        /// has no zeroes.
        ///</remarks>
        /// <param name="eqn">the specified array of coefficients to use to solve
        ///        the quadratic equation</param>
        /// <param name="res">the array that contains the non-complex roots
        ///         resulting from the solution of the quadratic equation.</param>
        /// <returns>the number of roots, or -1 if the equation is
        /// 	a constant</returns>
        public static int SolveQuadratic(double[] eqn, double[] res)
        {
            double a = eqn[2];
            double b = eqn[1];
            double c = eqn[0];
            int roots = 0;
            if (a == 0.0)
            {
                // The quadratic parabola has degenerated to a line.
                if (b == 0.0)
                {
                    // The line has degenerated to a constant.
                    return -1;
                }
                res[roots++] = -c/b;
            }
            else
            {
                // From Numerical Recipes, 5.6, Quadratic and Cubic Equations
                double d = b*b - 4.0*a*c;
                if (d < 0.0)
                {
                    // If d < 0.0, then there are no roots
                    return 0;
                }
                d = Math.Sqrt(d);
                // For accuracy, calculate one root using:
                //     (-b +/- d) / 2a
                // and the other using:
                //     2c / (-b +/- d)
                // Choose the sign of the +/- so that b+d gets larger in magnitude
                if (b < 0.0)
                {
                    d = -d;
                }
                double q = (b + d)/-2.0;
                // We already tested a for being 0 above
                res[roots++] = q/a;
                if (q != 0.0)
                {
                    res[roots++] = c/q;
                }
            }
            return roots;
        }

        /////////////////////////////////////////////////////////////////////////////
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
            double xp1 = IntX1;
            double yp1 = IntY1;
            double xc = IntCtrlX;
            double yc = IntCtrlY;
            double xp2 = IntX2;
            double yp2 = IntY2;

            /*
             * We have a convex shape bounded by quad curve Pc(t)
             * and ine Pl(t).
             *
             *     P1 = (x1, y1) - start point of curve
             *     P2 = (X2, y2) - end point of curve
             *     Pc = (xc, yc) - control point
             *
             *     Pq(t) = P1*(1 - t)^2 + 2*Pc*t*(1 - t) + P2*t^2 =
             *           = (P1 - 2*Pc + P2)*t^2 + 2*(Pc - P1)*t + P1
             *     Pl(t) = P1*(1 - t) + P2*t
             *     t = [0:1]
             *
             *     P = (x, y) - point of interest
             *
             * Let's look at second derivative of quad curve equation:
             *
             *     Pq''(t) = 2 * (P1 - 2 * Pc + P2) = Pq''
             *     It's constant vector.
             *
             * Let's draw a line through P to be parallel to this
             * vector and find the intersection of the quad curve
             * and the line.
             *
             * Pq(t) is point of intersection if system of equations
             * below has the solution.
             *
             *     L(s) = P + Pq''*s == Pq(t)
             *     Pq''*s + (P - Pq(t)) == 0
             *
             *     | xq''*s + (x - xq(t)) == 0
             *     | yq''*s + (y - yq(t)) == 0
             *
             * This system has the solution if rank of its matrix equals to 1.
             * That is, determinant of the matrix should be zero.
             *
             *     (y - yq(t))*xq'' == (x - xq(t))*yq''
             *
             * Let's solve this equation with 't' variable.
             * Also let kx = x1 - 2*xc + X2
             *          ky = y1 - 2*yc + y2
             *
             *     t0q = (1/2)*((x - x1)*ky - (y - y1)*kx) /
             *                 ((xc - x1)*ky - (yc - y1)*kx)
             *
             * Let's do the same for our line Pl(t):
             *
             *     t0l = ((x - x1)*ky - (y - y1)*kx) /
             *           ((X2 - x1)*ky - (y2 - y1)*kx)
             *
             * It's easy to check that t0q == t0l. This fact means
             * we can compute t0 only one time.
             *
             * In case t0 < 0 or t0 > 1, we have an intersections outside
             * of shape bounds. So, P is definitely out of shape.
             *
             * In case t0 is inside [0:1], we should calculate Pq(t0)
             * and Pl(t0). We have three points for now, and all of them
             * lie on one line. So, we just need to detect, is our point
             * of interest between points of intersections or not.
             *
             * If the denominator in the t0q and t0l equations is
             * zero, then the points must be collinear and so the
             * curve is degenerate and encloses no area.  Thus the
             * result is false.
             */
            double kx = xp1 - 2*xc + xp2;
            double ky = yp1 - 2*yc + yp2;
            double dx = x - xp1;
            double dy = y - yp1;
            double dxl = xp2 - xp1;
            double dyl = yp2 - yp1;

            double t0 = (dx*ky - dy*kx)/(dxl*ky - dyl*kx);
            if (t0 < 0 || t0 > 1)
            {
                return false;
            }

            double xb = kx*t0*t0 + 2*(xc - xp1)*t0 + xp1;
            double yb = ky*t0*t0 + 2*(yc - yp1)*t0 + yp1;
            double xl = dxl*t0 + xp1;
            double yl = dyl*t0 + yp1;

            return (x >= xb && x < xl) ||
                   (x >= xl && x < xb) ||
                   (y >= yb && y < yl) ||
                   (y >= yl && y < yb);
        }

        /////////////////////////////////////////////////////////////////////////////
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
            return Contains(p.X, p.Y);
        }

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Fill an array with the coefficients of the parametric equation
        /// in t, ready for solving against val with solveQuadratic.
        /// </summary>
        /// <remarks>
        /// We currently have:
        /// <code>
        ///     val = Py(t) = C1*(1-t)^2 + 2*CP*t*(1-t) + C2*t^2
        ///                 = C1 - 2*C1*t + C1*t^2 + 2*CP*t - 2*CP*t^2 + C2*t^2
        ///                 = C1 + (2*CP - 2*C1)*t + (C1 - 2*CP + C2)*t^2
        ///               0 = (C1 - val) + (2*CP - 2*C1)*t + (C1 - 2*CP + C2)*t^2
        ///               0 = C + Bt + At^2
        ///     C = C1 - val
        ///     B = 2*CP - 2*C1
        ///     A = C1 - 2*CP + C2
        /// </code>
        /// </remarks>
        /// <param name="eqn">The eqn.</param>
        /// <param name="val">The val.</param>
        /// <param name="c1">The c1.</param>
        /// <param name="cp">The cp.</param>
        /// <param name="c2">The c2.</param>
        private static void FillEqn(double[] eqn, double val,
                                    double c1, double cp, double c2)
        {
            eqn[0] = c1 - val;
            eqn[1] = cp + cp - c1 - c1;
            eqn[2] = c1 - cp - cp + c2;
            return;
        }

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Evaluate the t values in the first num slots of the vals[] array
        /// and place the evaluated values back into the same array. 
        /// </summary>
        /// <remarks>
        /// Only evaluate t values that are within the range [0, 1], including
        /// the 0 and 1 ends of the range iff the include0 or include1
        /// booleans are true.  If an "inflection" equation is handed in,
        /// then any points which represent a point of inflection for that
        /// quadratic equation are also ignored.
        /// </remarks>
        /// <param name="vals">The vals.</param>
        /// <param name="num">The num.</param>
        /// <param name="include0">if set to <c>true</c> [include0].</param>
        /// <param name="include1">if set to <c>true</c> [include1].</param>
        /// <param name="inflect">The inflect.</param>
        /// <param name="c1">The c1.</param>
        /// <param name="ctrl">The CTRL.</param>
        /// <param name="c2">The c2.</param>
        /// <returns></returns>
        private static int EvalQuadratic(double[] vals, int num,
                                         bool include0,
                                         bool include1,
                                         double[] inflect,
                                         double c1, double ctrl, double c2)
        {
            int j = 0;
            for (int i = 0; i < num; i++)
            {
                double t = vals[i];
                if ((include0 ? t >= 0 : t > 0) &&
                    (include1 ? t <= 1 : t < 1) &&
                    (inflect == null ||
                     inflect[1] + 2*inflect[2]*t != 0))
                {
                    double u = 1 - t;
                    vals[j++] = c1*u*u + 2*ctrl*t*u + c2*t*t;
                }
            }
            return j;
        }

        private const int Below = -2;
        private const int LowEdge = -1;
        private const int Inside = 0;
        private const int HighEdge = 1;
        private const int Above = 2;

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determine where coord lies with respect to the range from
        /// low to high.  It is assumed that low is less than high.  The return
        /// value is one of the 5 values BELOW, LOWEDGE, INSIDE, HIGHEDGE,
        /// or ABOVE.
        /// </summary>
        /// <param name="coord">The coord.</param>
        /// <param name="low">The low.</param>
        /// <param name="high">The high.</param>
        /// <returns></returns>
        private static int GetTag(double coord, double low, double high)
        {
            if (coord <= low)
            {
                return (coord < low ? Below : LowEdge);
            }
            if (coord >= high)
            {
                return (coord > high ? Above : HighEdge);
            }
            return Inside;
        }

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determine if the pttag represents a coordinate that is already
        /// in its test range, or is on the border with either of the two
        /// opttags representing another coordinate that is "towards the
        /// inside" of that test range.  In other words, are either of the
        /// two "opt" points "drawing the pt inward"?
        /// </summary>
        /// <param name="pttag">The pttag.</param>
        /// <param name="opt1Tag">The opt1 tag.</param>
        /// <param name="opt2Tag">The opt2 tag.</param>
        /// <returns></returns>
        private static bool Inwards(int pttag, int opt1Tag, int opt2Tag)
        {
            switch (pttag)
            {
                default:
                    return false;
                case LowEdge:
                    return (opt1Tag >= Inside || opt2Tag >= Inside);
                case Inside:
                    return true;
                case HighEdge:
                    return (opt1Tag <= Inside || opt2Tag <= Inside);
            }
        }

        /////////////////////////////////////////////////////////////////////////////
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
        /// The Area class performs
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
            // Trivially reject non-existant rectangles
            if (w <= 0 || h <= 0)
            {
                return false;
            }

            // Trivially accept if either endpoint is inside the rectangle
            // (not on its border since it may end there and not go inside)
            // Record where they lie with respect to the rectangle.
            //     -1 => left, 0 => inside, 1 => right
            double xp1 = IntX1;
            int x1Tag = GetTag(xp1, x, x + w);
            int y1Tag = GetTag(Y1, y, y + h);
            if (x1Tag == Inside && y1Tag == Inside)
            {
                return true;
            }
            double xp2 = IntX2;
            double yp2 = IntY2;
            int x2Tag = GetTag(xp2, x, x + w);
            int y2Tag = GetTag(yp2, y, y + h);
            if (x2Tag == Inside && y2Tag == Inside)
            {
                return true;
            }
            double ctrlpx = IntCtrlX;
            double ctrlpy = IntCtrlY;
            int ctrlxtag = GetTag(ctrlpx, x, x + w);
            int ctrlytag = GetTag(ctrlpy, y, y + h);

            // Trivially reject if all points are entirely to one side of
            // the rectangle.
            if (x1Tag < Inside && x2Tag < Inside && ctrlxtag < Inside)
            {
                return false; // All points left
            }
            if (y1Tag < Inside && y2Tag < Inside && ctrlytag < Inside)
            {
                return false; // All points above
            }
            if (x1Tag > Inside && x2Tag > Inside && ctrlxtag > Inside)
            {
                return false; // All points right
            }
            if (y1Tag > Inside && y2Tag > Inside && ctrlytag > Inside)
            {
                return false; // All points below
            }

            // Test for endpoints on the edge where either the segment
            // or the curve is headed "inwards" from them
            // Remember: These tests are a superset of the fast endpoint tests
            //       above and thus repeat those tests, but take more time
            //       and cover more cases
            if (Inwards(x1Tag, x2Tag, ctrlxtag) &&
                Inwards(y1Tag, y2Tag, ctrlytag))
            {
                // First endpoint on border with either edge moving inside
                return true;
            }
            if (Inwards(x2Tag, x1Tag, ctrlxtag) &&
                Inwards(y2Tag, y1Tag, ctrlytag))
            {
                // Second endpoint on border with either edge moving inside
                return true;
            }

            // Trivially accept if endpoints span directly across the rectangle
            bool xoverlap = (x1Tag*x2Tag <= 0);
            bool yoverlap = (y1Tag*y2Tag <= 0);
            if (x1Tag == Inside && x2Tag == Inside && yoverlap)
            {
                return true;
            }
            if (y1Tag == Inside && y2Tag == Inside && xoverlap)
            {
                return true;
            }

            // We now know that both endpoints are outside the rectangle
            // but the 3 points are not all on one side of the rectangle.
            // Therefore the curve cannot be contained inside the rectangle,
            // but the rectangle might be contained inside the curve, or
            // the curve might intersect the boundary of the rectangle.

            double[] eqn = new double[3];
            double[] res = new double[3];
            if (!yoverlap)
            {
                // Both Y coordinates for the closing segment are above or
                // below the rectangle which means that we can only intersect
                // if the curve crosses the top (or bottom) of the rectangle
                // in more than one place and if those crossing locations
                // span the horizontal range of the rectangle.
                FillEqn(eqn, (y1Tag < Inside ? y : y + h), Y1, ctrlpy, yp2);
                return (SolveQuadratic(eqn, res) == 2 &&
                        EvalQuadratic(res, 2, true, true, null,
                                      xp1, ctrlpx, xp2) == 2 &&
                        GetTag(res[0], x, x + w)*GetTag(res[1], x, x + w) <= 0);
            }

            // Y ranges overlap.  Now we examine the X ranges
            if (!xoverlap)
            {
                // Both X coordinates for the closing segment are left of
                // or right of the rectangle which means that we can only
                // intersect if the curve crosses the left (or right) edge
                // of the rectangle in more than one place and if those
                // crossing locations span the vertical range of the rectangle.
                FillEqn(eqn, (x1Tag < Inside ? x : x + w), xp1, ctrlpx, xp2);
                return (SolveQuadratic(eqn, res) == 2 &&
                        EvalQuadratic(res, 2, true, true, null,
                                      Y1, ctrlpy, yp2) == 2 &&
                        GetTag(res[0], y, y + h)*GetTag(res[1], y, y + h) <= 0);
            }

            // The X and Y ranges of the endpoints overlap the X and Y
            // ranges of the rectangle, now find out how the endpoint
            // line segment intersects the Y range of the rectangle
            double dx = xp2 - xp1;
            double dy = yp2 - Y1;
            double k = yp2*xp1 - xp2*Y1;
            int c1Tag, c2Tag;
            if (y1Tag == Inside)
            {
                c1Tag = x1Tag;
            }
            else
            {
                c1Tag = GetTag((k + dx*(y1Tag < Inside ? y : y + h))/dy, x, x + w);
            }
            if (y2Tag == Inside)
            {
                c2Tag = x2Tag;
            }
            else
            {
                c2Tag = GetTag((k + dx*(y2Tag < Inside ? y : y + h))/dy, x, x + w);
            }
            // If the part of the line segment that intersects the Y range
            // of the rectangle crosses it horizontally - trivially accept
            if (c1Tag*c2Tag <= 0)
            {
                return true;
            }

            // Now we know that both the X and Y ranges intersect and that
            // the endpoint line segment does not directly cross the rectangle.
            //
            // We can almost treat this case like one of the cases above
            // where both endpoints are to one side, except that we will
            // only get one intersection of the curve with the vertical
            // side of the rectangle.  This is because the endpoint segment
            // accounts for the other intersection.
            //
            // (Remember there is overlap in both the X and Y ranges which
            //  means that the segment must cross at least one vertical edge
            //  of the rectangle - in particular, the "near vertical side" -
            //  leaving only one intersection for the curve.)
            //
            // Now we calculate the y tags of the two intersections on the
            // "near vertical side" of the rectangle.  We will have one with
            // the endpoint segment, and one with the curve.  If those two
            // vertical intersections overlap the Y range of the rectangle,
            // we have an intersection.  Otherwise, we don't.

            // c1tag = vertical intersection class of the endpoint segment
            //
            // Choose the y tag of the endpoint that was not on the same
            // side of the rectangle as the subsegment calculated above.
            // Remember that we can "steal" the existing Y tag of that endpoint
            // since it will be provably the same as the vertical intersection.
            c1Tag = ((c1Tag*x1Tag <= 0) ? y1Tag : y2Tag);

            // c2tag = vertical intersection class of the curve
            //
            // We have to calculate this one the straightforward way.
            // Remember that the c2tag can still tell us which vertical edge
            // to test against.
            FillEqn(eqn, (c2Tag < Inside ? x : x + w), xp1, ctrlpx, xp2);
            int num = SolveQuadratic(eqn, res);

            // Remember: We should be able to assert(num == 2); since the
            // X range "crosses" (not touches) the vertical boundary,
            // but we pass num to evalQuadratic for completeness.
            EvalQuadratic(res, num, true, true, null, Y1, ctrlpy, yp2);

            // Remember: We can assert(num evals == 1); since one of the
            // 2 crossings will be out of the [0,1] range.
            c2Tag = GetTag(res[0], y, y + h);

            // Finally, we have an intersection if the two crossings
            // overlap the Y range of the rectangle.
            return (c1Tag*c2Tag <= 0);
        }

        /////////////////////////////////////////////////////////////////////////////
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
        /// The Area class performs
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
            return Intersects(r.IntX, r.IntY, r.IntWidth, r.IntHeight);
        }

        /////////////////////////////////////////////////////////////////////////////
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
        /// The Area class performs
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
            if (w <= 0 || h <= 0)
            {
                return false;
            }
            // Assertion: Quadratic curves closed by connecting their
            // endpoints are always convex.
            return (Contains(x, y) &&
                    Contains(x + w, y) &&
                    Contains(x + w, y + h) &&
                    Contains(x, y + h));
        }

        /////////////////////////////////////////////////////////////////////////////
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
        /// The Area class performs
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
            return Contains(r.IntX, r.IntY, r.IntWidth, r.IntHeight);
        }

        /////////////////////////////////////////////////////////////////////////////
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
        /// If an optional AffineTransform
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
            return new QuadIterator(this, at);
        }

        /////////////////////////////////////////////////////////////////////////////
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
            return new FlatteningPathIterator(GetPathIterator(at), flatness);
        }
    }
}