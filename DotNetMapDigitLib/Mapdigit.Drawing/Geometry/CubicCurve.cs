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
using Mapdigit.Util;

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
    /// A cubic parametric curve segment.
    /// </summary>
    public class CubicCurve : IShape
    {

        /// <summary>
        /// The X coordinate of the start point
        /// of the cubic curve segment.
        /// </summary>
        public double X1;

        /// <summary>
        /// The Y coordinate of the start point
        /// of the cubic curve segment.
        /// </summary>
        public double Y1;

        /// <summary>
        /// The X coordinate of the first control point
        /// of the cubic curve segment.
        /// </summary>
        public double Ctrlx1;


        /// <summary>
        /// 
        /// The Y coordinate of the first control point
        /// of the cubic curve segment.
        /// </summary>
        public double Ctrly1;


        /// <summary>
        /// The X coordinate of the second control point
        /// of the cubic curve segment.
        /// </summary>
        public double Ctrlx2;


        /// <summary>
        /// The Y coordinate of the second control point
        /// of the cubic curve segment.
        /// </summary>
        public double Ctrly2;


        /// <summary>
        /// The X coordinate of the end point
        /// of the cubic curve segment.
        /// </summary>
        public double X2;


        /// <summary>
        /// The Y coordinate of the end point
        /// of the cubic curve segment.
        /// </summary>
        public double Y2;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="CubicCurve"/> class.
        /// </summary>
        public CubicCurve()
        {
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs and initializes a CubicCurve from the specified double coordinates.
        /// </summary>
        /// <param name="x1">the X coordinate for the start point
        ///            of the resulting CubicCurve</param>
        /// <param name="y1">the Y coordinate for the start point
        ///            of the resulting CubicCurve</param>
        /// <param name="ctrlx1">the X coordinate for the first control point
        ///                of the resulting CubicCurve</param>
        /// <param name="ctrly1">the Y coordinate for the first control point
        ///                of the resulting CubicCurve</param>
        /// <param name="ctrlx2">the X coordinate for the second control point
        ///                of the resulting CubicCurve</param>
        /// <param name="ctrly2">the Y coordinate for the second control point
        ///                of the resulting CubicCurve</param>
        /// <param name="x2">the X coordinate for the end point
        ///            of the resulting CubicCurve</param>
        /// <param name="y2">the Y coordinate for the end point
        ///            of the resulting CubicCurve</param>
        public CubicCurve(double x1, double y1,
                          double ctrlx1, double ctrly1,
                          double ctrlx2, double ctrly2,
                          double x2, double y2)
        {
            SetCurve(x1, y1, ctrlx1, ctrly1, ctrlx2, ctrly2, x2, y2);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the X coordinate of the start point
        /// </summary>
        /// <returns>the X coordinate of the start point of the
        ///          CubicCurve.</returns>
        public int GetX1()
        {
            return (int) (X1 + .5);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the Y coordinate of the start point.
        /// </summary>
        /// <returns>the Y coordinate of the start point of the
        /// the CubicCurve.</returns>
        public int GetY1()
        {
            return (int) (Y1 + .5);
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
        /// <returns>a Point that is the start point of
        ///          the CubicCurve.</returns>
        public Point GetP1()
        {
            return new Point((int) (X1 + .5), (int) (Y1 + .5));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the X coordinate of the first control point.
        /// </summary>
        /// <returns>the X coordinate of the first control point of the
        ///          CubicCurve.</returns>
        public int GetCtrlX1()
        {
            return (int) (Ctrlx1 + .5);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the Y coordinate of the first control point.
        /// </summary>
        /// <returns>the Y coordinate of the first control point of the
        ///          CubicCurve</returns>
        public int GetCtrlY1()
        {
            return (int) (Ctrly1 + .5);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the first control point.
        /// </summary>
        /// <returns> a Point that is the first control point of
        ///         the CubicCurve.</returns>
        public Point GetCtrlP1()
        {
            return new Point((int) (Ctrlx1 + .5), (int) (Ctrly1 + .5));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the X coordinate of the second control point.
        /// </summary>
        /// <returns>the X coordinate of the second control point of the
        ///          CubicCurve.</returns>
        public int GetCtrlX2()
        {
            return (int) (Ctrlx2 + .5);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the Y coordinate of the second control point.
        /// </summary>
        /// <returns>the Y coordinate of the second control point of the
        ///         CubicCurve.</returns>
        public int GetCtrlY2()
        {
            return (int) (Ctrly2 + .5);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the second control point.
        /// </summary>
        /// <returns>a Point that is the second control point of
        ///         the CubicCurve.</returns>
        public Point GetCtrlP2()
        {
            return new Point((int) (Ctrlx2 + .5), (int) (Ctrly2 + .5));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the X coordinate of the end point.
        /// </summary>
        /// <returns>the X coordinate of the end point of the
        ///         CubicCurve.</returns>
        public int GetX2()
        {
            return (int) (X2 + .5);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the Y coordinate of the end point.
        /// </summary>
        /// <returns>the Y coordinate of the end point of the
        ///         CubicCurve.</returns>
        public int GetY2()
        {
            return (int) (Y2 + .5);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///Returns the end point.
        /// </summary>
        /// <returns> a Point that is the end point of
        ///         the CubicCurve.</returns>
        public Point GetP2()
        {
            return new Point((int) (X2 + .5), (int) (Y2 + .5));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the location of the end points and control points of this curve
        /// to the specified double coordinates.
        /// </summary>
        /// <param name="x1">the X coordinate used to set the start point
        ///           of this CubicCurve</param>
        /// <param name="y1">the Y coordinate used to set the start point
        ///            of this CubicCurve</param>
        /// <param name="ctrlx1">the X coordinate used to set the first control point
        ///                of this CubicCurve</param>
        /// <param name="ctrly1">the Y coordinate used to set the first control point
        ///                of this CubicCurve</param>
        /// <param name="ctrlx2">the X coordinate used to set the second control point
        ///                of this CubicCurve</param>
        /// <param name="ctrly2">the Y coordinate used to set the second control point
        ///                of this CubicCurve</param>
        /// <param name="x2">the X coordinate used to set the end point
        ///            of this CubicCurve</param>
        /// <param name="y2">the Y coordinate used to set the end point
        ///            of this CubicCurve</param>
        public void SetCurve(double x1, double y1,
                             double ctrlx1, double ctrly1,
                             double ctrlx2, double ctrly2,
                             double x2, double y2)
        {
            X1 = x1;
            Y1 = y1;
            Ctrlx1 = ctrlx1;
            Ctrly1 = ctrly1;
            Ctrlx2 = ctrlx2;
            Ctrly2 = ctrly2;
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
        /// Returns an integer Rectangle that completely encloses the
        /// IShape.  
        /// </summary>
        /// <remarks>
        /// remmeber that there is no guarantee that the
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
                double left = MathEx.Min(MathEx.Min(X1, X2),
                                         MathEx.Min(Ctrlx1, Ctrlx2));
                double top = MathEx.Min(MathEx.Min(Y1, Y2),
                                        MathEx.Min(Ctrly1, Ctrly2));
                double right = MathEx.Max(MathEx.Max(X1, X2),
                                          MathEx.Max(Ctrlx1, Ctrlx2));
                double bottom = MathEx.Max(MathEx.Max(Y1, Y2),
                                           MathEx.Max(Ctrly1, Ctrly2));
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
        /// Sets the location of the end points and control points of this curve
        /// to the double coordinates at the specified offset in the specified array.
        /// </summary>
        /// <param name="coords">a double array containing coordinates.</param>
        /// <param name="offset"> the index of coords from which to begin
        ///          setting the end points and control points of this curve
        /// 	to the coordinates contained in coords</param>
        public void SetCurve(double[] coords, int offset)
        {
            SetCurve(coords[offset + 0], coords[offset + 1],
                     coords[offset + 2], coords[offset + 3],
                     coords[offset + 4], coords[offset + 5],
                     coords[offset + 6], coords[offset + 7]);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Sets the location of the end points and control points of this curve
        ///  to the specified Point coordinates.
        /// </summary>
        /// <param name="p1">the first specified Point used to set the
        /// 		start point of this curve.</param>
        /// <param name="cp1">the second specified Point used to set the
        /// 		first control point of this curve</param>
        /// <param name="cp2">the third specified Point used to set the
        /// 		second control point of this curve.</param>
        /// <param name="p2">the fourth specified Point used to set the
        /// 		end point of this curve</param>
        public void SetCurve(Point p1, Point cp1, Point cp2, Point p2)
        {
            SetCurve(p1.X, p1.Y, cp1.X, cp1.Y,
                     cp2.X, cp2.Y, p2.X, p2.Y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the location of the end points and control points of this curve
        /// to the coordinates of the Point objects at the specified
        /// offset in the specified array.
        /// </summary>
        /// <param name="pts">an array of Point objects</param>
        /// <param name="offset">the index of pts from which to begin setting
        ///           the end points and control points of this curve to the
        /// 	points contained in pts</param>
        public void SetCurve(Point[] pts, int offset)
        {
            SetCurve(pts[offset + 0].X, pts[offset + 0].Y,
                     pts[offset + 1].X, pts[offset + 1].Y,
                     pts[offset + 2].X, pts[offset + 2].Y,
                     pts[offset + 3].X, pts[offset + 3].Y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the location of the end points and control points of this curve
        /// to the same as those in the specified CubicCurve.
        /// </summary>
        /// <param name="c">the specified CubicCurve</param>
        public void SetCurve(CubicCurve c)
        {
            SetCurve(c.GetX1(), c.GetY1(), c.GetCtrlX1(), c.GetCtrlY1(),
                     c.GetCtrlX2(), c.GetCtrlY2(), c.GetX2(), c.GetY2());
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the square of the flatness of the cubic curve specified
        ///  by the indicated control points. 
        /// </summary>
        /// <remarks>
        /// The flatness is the maximum distance
        /// of a control point from the line connecting the end points.
        /// </remarks>
        /// <param name="x1">the X coordinate that specifies the start point
        ///            of a CubicCurve</param>
        /// <param name="y1">the Y coordinate that specifies the start point
        ///            of a CubicCurve</param>
        /// <param name="ctrlx1">the X coordinate that specifies the first control point
        ///                of a CubicCurve</param>
        /// <param name="ctrly1">the Y coordinate that specifies the first control point
        ///                of a CubicCurve.</param>
        /// <param name="ctrlx2"> the X coordinate that specifies the second control point
        ///                of a CubicCurve.</param>
        /// <param name="ctrly2">the Y coordinate that specifies the second control point
        ///                of a CubicCurve.</param>
        /// <param name="x2">the X coordinate that specifies the end point
        ///            of a CubicCurve.</param>
        /// <param name="y2">the Y coordinate that specifies the end point
        ///            of a CubicCurve</param>
        /// <returns>the square of the flatness of the CubicCurve
        /// 		represented by the specified coordinates.</returns>
        public static int GetFlatnessSq(int x1, int y1,
                                        int ctrlx1, int ctrly1,
                                        int ctrlx2, int ctrly2,
                                        int x2, int y2)
        {
            return MathEx.Max(Line.PtSegDistSq(x1, y1, x2, y2, ctrlx1, ctrly1),
                              Line.PtSegDistSq(x1, y1, x2, y2, ctrlx2, ctrly2));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the flatness of the cubic curve specified
        /// by the indicated control points. 
        /// </summary>
        /// <remarks>
        /// The flatness is the maximum distance
        /// of a control point from the line connecting the end points.
        /// </remarks>
        /// <param name="x1">the X coordinate that specifies the start point
        ///            of a CubicCurve</param>
        /// <param name="y1">the Y coordinate that specifies the start point
        ///            of a CubicCurve</param>
        /// <param name="ctrlx1">the X coordinate that specifies the first control point
        ///                of a CubicCurve</param>
        /// <param name="ctrly1">the Y coordinate that specifies the first control point
        ///                of a CubicCurve.</param>
        /// <param name="ctrlx2"> the X coordinate that specifies the second control point
        ///                of a CubicCurve.</param>
        /// <param name="ctrly2">the Y coordinate that specifies the second control point
        ///                of a CubicCurve.</param>
        /// <param name="x2">the X coordinate that specifies the end point
        ///            of a CubicCurve.</param>
        /// <param name="y2">the Y coordinate that specifies the end point
        ///            of a CubicCurve</param>
        /// <returns>the flatness of the CubicCurve
        ///  represented by the specified coordinates.</returns>
        public static int GetFlatness(int x1, int y1,
                                      int ctrlx1, int ctrly1,
                                      int ctrlx2, int ctrly2,
                                      int x2, int y2)
        {
            return (int) (MathEx.Sqrt(GetFlatnessSq(x1, y1, ctrlx1, ctrly1,
                                                    ctrlx2, ctrly2, x2, y2)) + .5);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the square of the flatness of the cubic curve specified
        /// by the control points stored in the indicated array at the
        /// indicated index. 
        /// </summary>
        /// <remarks>
        /// The flatness is the maximum distance
        /// of a control point from the line connecting the end points.
        /// </remarks>
        /// <param name="coords">an array containing coordinates.</param>
        /// <param name="offset">the index of coords from which to begin
        ///          getting the end points and control points of the curve.</param>
        /// <returns></returns>
        public static int GetFlatnessSq(int[] coords, int offset)
        {
            return GetFlatnessSq(coords[offset + 0], coords[offset + 1],
                                 coords[offset + 2], coords[offset + 3],
                                 coords[offset + 4], coords[offset + 5],
                                 coords[offset + 6], coords[offset + 7]);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the flatness of the cubic curve specified
        /// by the control points stored in the indicated array at the
        /// indicated index.  
        /// </summary>
        /// <remarks>
        /// The flatness is the maximum distance
        /// of a control point from the line connecting the end points.
        /// </remarks>
        /// <param name="coords">an array containing coordinates.</param>
        /// <param name="offset">the index of coords from which to begin
        ///           getting the end points and control points of the curve</param>
        /// <returns>the flatness of the CubicCurve
        /// 	specified by the coordinates in coords at
        /// 	the specified offset.</returns>
        public static int GetFlatness(int[] coords, int offset)
        {
            return GetFlatness(coords[offset + 0], coords[offset + 1],
                               coords[offset + 2], coords[offset + 3],
                               coords[offset + 4], coords[offset + 5],
                               coords[offset + 6], coords[offset + 7]);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the square of the flatness of this curve.  The flatness is the
        /// maximum distance of a control point from the line connecting the
        /// end points.
        /// </summary>
        /// <returns>the square of the flatness of this curve.</returns>
        public int GetFlatnessSq()
        {
            return GetFlatnessSq(GetX1(), GetY1(), GetCtrlX1(), GetCtrlY1(),
                                 GetCtrlX2(), GetCtrlY2(), GetX2(), GetY2());
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the flatness of this curve.  The flatness is the
        /// maximum distance of a control point from the line connecting the
        /// end points.
        /// </summary>
        /// <returns>the flatness of this curve</returns>
        public int GetFlatness()
        {
            return GetFlatness(GetX1(), GetY1(), GetCtrlX1(), GetCtrlY1(),
                               GetCtrlX2(), GetCtrlY2(), GetX2(), GetY2());
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Subdivides this cubic curve and stores the resulting two
        /// subdivided curves into the left and right curve parameters.
        /// </summary>
        /// <remarks>
        /// Either or both of the left and right objects may be the same
        /// as this object or null.
        /// </remarks>
        /// <param name="left">left the cubic curve object for storing for the left or
        /// first half of the subdivided curve</param>
        /// <param name="right">right the cubic curve object for storing for the right or
        /// second half of the subdivided curve</param>
        public void Subdivide(CubicCurve left, CubicCurve right)
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
        /// Subdivides the cubic curve specified by the src parameter
        /// and stores the resulting two subdivided curves into the
        /// left and right curve parameters.
        /// </summary>
        /// <remarks>
        /// Either or both of the left and right objects
        /// may be the same as the src object or null.
        /// </remarks>
        /// <param name="src">the cubic curve to be subdivided.</param>
        /// <param name="left">the cubic curve object for storing the left or
        /// first half of the subdivided curve</param>
        /// <param name="right"> the cubic curve object for storing the right or
        /// second half of the subdivided curve.</param>
        public static void Subdivide(CubicCurve src,
                                     CubicCurve left,
                                     CubicCurve right)
        {
            double x1 = src.GetX1();
            double y1 = src.GetY1();
            double ctrlx1 = src.GetCtrlX1();
            double ctrly1 = src.GetCtrlY1();
            double ctrlx2 = src.GetCtrlX2();
            double ctrly2 = src.GetCtrlY2();
            double x2 = src.GetX2();
            double y2 = src.GetY2();
            double centerx = (ctrlx1 + ctrlx2)/2.0;
            double centery = (ctrly1 + ctrly2)/2.0;
            ctrlx1 = (x1 + ctrlx1)/2.0;
            ctrly1 = (y1 + ctrly1)/2.0;
            ctrlx2 = (x2 + ctrlx2)/2.0;
            ctrly2 = (y2 + ctrly2)/2.0;
            double ctrlx12 = (ctrlx1 + centerx)/2.0;
            double ctrly12 = (ctrly1 + centery)/2.0;
            double ctrlx21 = (ctrlx2 + centerx)/2.0;
            double ctrly21 = (ctrly2 + centery)/2.0;
            centerx = (ctrlx12 + ctrlx21)/2.0;
            centery = (ctrly12 + ctrly21)/2.0;
            if (left != null)
            {
                left.SetCurve(x1, y1, ctrlx1, ctrly1,
                              ctrlx12, ctrly12, centerx, centery);
            }
            if (right != null)
            {
                right.SetCurve(centerx, centery, ctrlx21, ctrly21,
                               ctrlx2, ctrly2, x2, y2);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Subdivides the cubic curve specified by the coordinates
        /// stored in the src array at indices srcoff
        /// through (srcoff+7) and stores the
        /// resulting two subdivided curves into the two result arrays at the
        /// corresponding indices.
        /// </summary>
        /// <remarks>
        /// Either or both of the left and right
        /// arrays may be null or a reference to the same array
        /// as the src array.
        /// remember that the last point in the first subdivided curve is the
        /// same as the first point in the second subdivided curve. Thus,
        /// it is possible to pass the same array for left
        /// and right and to use offsets, such as rightoff
        /// equals (leftoff + 6), in order
        /// to avoid allocating extra storage for this common point.
        /// </remarks>
        /// <param name="src">the array holding the coordinates for the source curve.</param>
        /// <param name="srcoff">the offset into the array of the beginning of the
        /// the 6 source coordinates</param>
        /// <param name="left">the array for storing the coordinates for the first
        /// half of the subdivided curve.</param>
        /// <param name="leftoff"> the offset into the array of the beginning of the
        /// the 6 left coordinates.</param>
        /// <param name="right">the array for storing the coordinates for the second
        ///  half of the subdivided curve.</param>
        /// <param name="rightoff">the offset into the array of the beginning of the
        /// the 6 right coordinates.</param>
        public static void Subdivide(double[] src, int srcoff,
                                     double[] left, int leftoff,
                                     double[] right, int rightoff)
        {
            double x1 = src[srcoff + 0];
            double y1 = src[srcoff + 1];
            double ctrlx1 = src[srcoff + 2];
            double ctrly1 = src[srcoff + 3];
            double ctrlx2 = src[srcoff + 4];
            double ctrly2 = src[srcoff + 5];
            double x2 = src[srcoff + 6];
            double y2 = src[srcoff + 7];
            if (left != null)
            {
                left[leftoff + 0] = x1;
                left[leftoff + 1] = y1;
            }
            if (right != null)
            {
                right[rightoff + 6] = x2;
                right[rightoff + 7] = y2;
            }
            x1 = (x1 + ctrlx1)/2.0;
            y1 = (y1 + ctrly1)/2.0;
            x2 = (x2 + ctrlx2)/2.0;
            y2 = (y2 + ctrly2)/2.0;
            double centerx = (ctrlx1 + ctrlx2)/2.0;
            double centery = (ctrly1 + ctrly2)/2.0;
            ctrlx1 = (x1 + centerx)/2.0;
            ctrly1 = (y1 + centery)/2.0;
            ctrlx2 = (x2 + centerx)/2.0;
            ctrly2 = (y2 + centery)/2.0;
            centerx = (ctrlx1 + ctrlx2)/2.0;
            centery = (ctrly1 + ctrly2)/2.0;
            if (left != null)
            {
                left[leftoff + 2] = x1;
                left[leftoff + 3] = y1;
                left[leftoff + 4] = ctrlx1;
                left[leftoff + 5] = ctrly1;
                left[leftoff + 6] = centerx;
                left[leftoff + 7] = centery;
            }
            if (right != null)
            {
                right[rightoff + 0] = centerx;
                right[rightoff + 1] = centery;
                right[rightoff + 2] = ctrlx2;
                right[rightoff + 3] = ctrly2;
                right[rightoff + 4] = x2;
                right[rightoff + 5] = y2;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Solves the cubic whose coefficients are in the eqn
        /// array and places the non-complex roots back into the same array,
        /// returning the number of roots.  
        /// </summary>
        /// <remarks>
        /// The solved cubic is represented
        /// by the equation:
        /// <pre>
        ///     eqn = {c, b, a, d}
        ///     dx^3 + ax^2 + bx + c = 0
        /// </pre>
        /// A return value of -1 is used to distinguish a constant equation
        /// that might be always 0 or never 0 from an equation that has no
        /// zeroes.
        /// </remarks>
        /// <param name="eqn">an array containing coefficients for a cubic</param>
        /// <returns>the number of roots, or -1 if the equation is a constant.</returns>
        public static int SolveCubic(double[] eqn)
        {
            return SolveCubic(eqn, eqn);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Solve the cubic whose coefficients are in the eqn
        /// array and place the non-complex roots into the res
        /// array, returning the number of roots.
        /// </summary>
        /// <remarks>
        /// The cubic solved is represented by the equation:
        ///     eqn = {c, b, a, d}
        ///     dx^3 + ax^2 + bx + c = 0
        /// A return value of -1 is used to distinguish a constant equation,
        /// which may be always 0 or never 0, from an equation which has no
        /// zeroes.
        /// </remarks>
        /// <param name="eqn">the specified array of coefficients to use to solve
        ///         the cubic equation</param>
        /// <param name="res">Tthe array that contains the non-complex roots
        ///        resulting from the solution of the cubic equation</param>
        /// <returns>the number of roots, or -1 if the equation is a constant</returns>
        public static int SolveCubic(double[] eqn, double[] res)
        {
            // From Numerical Recipes, 5.6, Quadratic and Cubic Equations
            double d = eqn[3];
            if (d == 0.0)
            {
                // The cubic has degenerated to quadratic (or line or ...).
                return QuadCurve.SolveQuadratic(eqn, res);
            }
            double a = eqn[2]/d;
            double b = eqn[1]/d;
            double c = eqn[0]/d;
            int roots = 0;
            double q = (a*a - 3.0*b)/9.0;
            double r = (2.0*a*a*a - 9.0*a*b + 27.0*c)/54.0;
            double r2 = r*r;
            double q3 = q*q*q;
            a = a/3.0;
            if (r2 < q3)
            {
                double theta = MathEx.Acos(r/MathEx.Sqrt(q3));
                q = -2.0*MathEx.Sqrt(q);
                if (res == eqn)
                {
                    // Copy the eqn so that we don't clobber it with the
                    // roots.  This is needed so that fixRoots can do its
                    // work with the original equation.
                    eqn = new double[4];
                    Array.Copy(res, 0, eqn, 0, 4);
                }
                res[roots++] = q*MathEx.Cos(theta/3.0) - a;
                res[roots++] = q*MathEx.Cos((theta + MathEx.Pi*2.0)/3.0) - a;
                res[roots++] = q*MathEx.Cos((theta - MathEx.Pi*2.0)/3.0) - a;
                FixRoots(res, eqn);
            }
            else
            {
                bool neg = (r < 0.0);
                double s = MathEx.Sqrt(r2 - q3);
                if (neg)
                {
                    r = -r;
                }
                double pow = MathEx.Pow(r + s, 1.0/3.0);
                if (!neg)
                {
                    pow = -pow;
                }
                double d1 = (pow == 0.0) ? 0.0 : (q/pow);
                res[roots++] = (pow + d1) - a;
            }
            return roots;
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
            if (!(x*0.0 + y*0.0 == 0.0))
            {
                /* Either x or y was infinite or NaN.
                 * A NaN always produces a negative response to any test
                 * and Infinity values cannot be "inside" any path so
                 * they should return false as well.
                 */
                return false;
            }
            // We count the "Y" crossings to determine if the point is
            // inside the curve bounded by its closing line.
            double xp1 = GetX1();
            double yp1 = GetY1();
            double xp2 = GetX2();
            double yp2 = GetY2();
            int crossings =
                (Curve.PointCrossingsForLine(x, y, xp1, yp1, xp2, yp2) +
                 Curve.PointCrossingsForCubic(x, y,
                                              xp1, yp1,
                                              GetCtrlX1(), GetCtrlY1(),
                                              GetCtrlX2(), GetCtrlY2(),
                                              xp2, yp2, 0));
            return ((crossings & 1) == 1);
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
            return Contains(p.X, p.Y);
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
            // Trivially reject non-existant rectangles
            if (w <= 0 || h <= 0)
            {
                return false;
            }

            // Trivially accept if either endpoint is inside the rectangle
            // (not on its border since it may end there and not go inside)
            // Record where they lie with respect to the rectangle.
            //     -1 => left, 0 => inside, 1 => right
            double xp1 = GetX1();
            double yp1 = GetY1();
            int x1Tag = GetTag(xp1, x, x + w);
            int y1Tag = GetTag(yp1, y, y + h);
            if (x1Tag == Inside && y1Tag == Inside)
            {
                return true;
            }
            double xp2 = GetX2();
            double yp2 = GetY2();
            int x2Tag = GetTag(xp2, x, x + w);
            int y2Tag = GetTag(yp2, y, y + h);
            if (x2Tag == Inside && y2Tag == Inside)
            {
                return true;
            }

            double ctrlxp1 = GetCtrlX1();
            double ctrlyp1 = GetCtrlY1();
            double ctrlxp2 = GetCtrlX2();
            double ctrlyp2 = GetCtrlY2();
            int ctrlx1Tag = GetTag(ctrlxp1, x, x + w);
            int ctrly1Tag = GetTag(ctrlyp1, y, y + h);
            int ctrlx2Tag = GetTag(ctrlxp2, x, x + w);
            int ctrly2Tag = GetTag(ctrlyp2, y, y + h);

            // Trivially reject if all points are entirely to one side of
            // the rectangle.
            if (x1Tag < Inside && x2Tag < Inside &&
                ctrlx1Tag < Inside && ctrlx2Tag < Inside)
            {
                return false; // All points left
            }
            if (y1Tag < Inside && y2Tag < Inside &&
                ctrly1Tag < Inside && ctrly2Tag < Inside)
            {
                return false; // All points above
            }
            if (x1Tag > Inside && x2Tag > Inside &&
                ctrlx1Tag > Inside && ctrlx2Tag > Inside)
            {
                return false; // All points right
            }
            if (y1Tag > Inside && y2Tag > Inside &&
                ctrly1Tag > Inside && ctrly2Tag > Inside)
            {
                return false; // All points below
            }

            // Test for endpoints on the edge where either the segment
            // or the curve is headed "inwards" from them
            // remember: These tests are a superset of the fast endpoint tests
            //       above and thus repeat those tests, but take more time
            //       and cover more cases
            if (Inwards(x1Tag, x2Tag, ctrlx1Tag) &&
                Inwards(y1Tag, y2Tag, ctrly1Tag))
            {
                // First endpoint on border with either edge moving inside
                return true;
            }
            if (Inwards(x2Tag, x1Tag, ctrlx2Tag) &&
                Inwards(y2Tag, y1Tag, ctrly2Tag))
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
            // but the 4 points are not all on one side of the rectangle.
            // Therefore the curve cannot be contained inside the rectangle,
            // but the rectangle might be contained inside the curve, or
            // the curve might intersect the boundary of the rectangle.

            double[] eqn = new double[4];
            double[] res = new double[4];
            if (!yoverlap)
            {
                // Both y coordinates for the closing segment are above or
                // below the rectangle which means that we can only intersect
                // if the curve crosses the top (or bottom) of the rectangle
                // in more than one place and if those crossing locations
                // span the horizontal range of the rectangle.
                FillEqn(eqn, (y1Tag < Inside ? y : y + h), yp1, ctrlyp1, ctrlyp2, yp2);
                int num = SolveCubic(eqn, res);
                num = EvalCubic(res, num, true, true, null,
                                xp1, ctrlxp1, ctrlxp2, xp2);
                // odd counts imply the crossing was out of [0,1] bounds
                // otherwise there is no way for that part of the curve to
                // "return" to meet its endpoint
                return (num == 2 &&
                        GetTag(res[0], x, x + w)*GetTag(res[1], x, x + w) <= 0);
            }

            // Y ranges overlap.  Now we examine the X ranges

            if (!xoverlap)
            {
                // Both x coordinates for the closing segment are left of
                // or right of the rectangle which means that we can only
                // intersect if the curve crosses the left (or right) edge
                // of the rectangle in more than one place and if those
                // crossing locations span the vertical range of the rectangle.
                FillEqn(eqn, (x1Tag < Inside ? x : x + w), xp1, ctrlxp1, ctrlxp2, xp2);
                int num = SolveCubic(eqn, res);
                num = EvalCubic(res, num, true, true, null,
                                yp1, ctrlyp1, ctrlyp2, yp2);
                // odd counts imply the crossing was out of [0,1] bounds
                // otherwise there is no way for that part of the curve to
                // "return" to meet its endpoint
                return (num == 2 &&
                        GetTag(res[0], y, y + h)*GetTag(res[1], y, y + h) <= 0);
            }

            // The X and Y ranges of the endpoints overlap the X and Y
            // ranges of the rectangle, now find out how the endpoint
            // line segment intersects the Y range of the rectangle
            double dx = xp2 - xp1;
            double dy = yp2 - yp1;
            double k = yp2*xp1 - xp2*yp1;
            int c1Tag = y1Tag == Inside ? x1Tag : GetTag((k + dx*(y1Tag < Inside ? y : y + h))/dy, x, x + w);
            int c2Tag = y2Tag == Inside ? x2Tag : GetTag((k + dx*(y2Tag < Inside ? y : y + h))/dy, x, x + w);
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
            // where both endpoints are to one side, except that we may
            // get one or three intersections of the curve with the vertical
            // side of the rectangle.  This is because the endpoint segment
            // accounts for the other intersection in an even pairing.  Thus,
            // with the endpoint crossing we end up with 2 or 4 total crossings.
            //
            // (Remember there is overlap in both the X and Y ranges which
            //  means that the segment itself must cross at least one vertical
            //  edge of the rectangle - in particular, the "near vertical side"
            //  - leaving an odd number of intersections for the curve.)
            //
            // Now we calculate the y tags of all the intersections on the
            // "near vertical side" of the rectangle.  We will have one with
            // the endpoint segment, and one or three with the curve.  If
            // any pair of those vertical intersections overlap the Y range
            // of the rectangle, we have an intersection.  Otherwise, we don't.

            // c1tag = vertical intersection class of the endpoint segment
            //
            // Choose the y tag of the endpoint that was not on the same
            // side of the rectangle as the subsegment calculated above.
            // remember that we can "steal" the existing Y tag of that endpoint
            // since it will be provably the same as the vertical intersection.
            c1Tag = ((c1Tag*x1Tag <= 0) ? y1Tag : y2Tag);

            // Now we have to calculate an array of solutions of the curve
            // with the "near vertical side" of the rectangle.  Then we
            // need to sort the tags and do a pairwise range test to see
            // if either of the pairs of crossings spans the Y range of
            // the rectangle.
            //
            // remember that the c2tag can still tell us which vertical edge
            // to test against.
            FillEqn(eqn, (c2Tag < Inside ? x : x + w), xp1, ctrlxp1, ctrlxp2, xp2);
            {
                int num = SolveCubic(eqn, res);
                num = EvalCubic(res, num, true, true, null, yp1, ctrlyp1, ctrlyp2, yp2);

                // Now put all of the tags into a bucket and sort them.  There
                // is an intersection iff one of the pairs of tags "spans" the
                // Y range of the rectangle.
                int[] tags = new int[num + 1];
                for (int i = 0; i < num; i++)
                {
                    tags[i] = GetTag(res[i], y, y + h);
                }
                tags[num] = c1Tag;
                Array.Sort(tags);
                return ((num >= 1 && tags[0]*tags[1] <= 0) ||
                        (num >= 3 && tags[2]*tags[3] <= 0));
            }
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
            return Intersects(r.IntX, r.IntY, r.IntWidth, r.IntHeight);
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
            if (w <= 0 || h <= 0)
            {
                return false;
            }
            // Assertion: Cubic curves closed by connecting their
            // endpoints form either one or two convex halves with
            // the closing line segment as an edge of both sides.
            if (!(Contains(x, y) &&
                  Contains(x + w, y) &&
                  Contains(x + w, y + h) &&
                  Contains(x, y + h)))
            {
                return false;
            }
            // Either the rectangle is entirely inside one of the convex
            // halves or it crosses from one to the other, in which case
            // it must intersect the closing line segment.
            Rectangle rect = new Rectangle(x, y, w, h);
            return !rect.IntersectsLine(GetX1(), GetY1(), GetX2(), GetY2());
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
            return Contains(r.IntX, r.IntY, r.IntWidth, r.IntHeight);
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
            return new CubicIterator(this, at);
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

        private const int Below = -2;
        private const int LowEdge = -1;
        private const int Inside = 0;
        private const int HighEdge = 1;
        private const int Above = 2;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This pruning step is necessary since solveCubic uses the
        /// cosine function to calculate the roots when there are 3
        /// of them.  Since the cosine method can have an error of
        /// +/- 1E-14 we need to make sure that we don't make any
        /// bad decisions due to an error.
        ///
        /// If the root is not near one of the endpoints, then we will
        /// only have a slight inaccuracy in calculating the x intercept
        /// which will only cause a slightly wrong answer for some
        /// points very close to the curve.  While the results in that
        /// case are not as accurate as they could be, they are not
        /// disastrously inaccurate either.
        ///
        /// On the other hand, if the error happens near one end of
        /// the curve, then our processing to reject values outside
        /// of the t=[0,1] range will fail and the results of that
        /// failure will be disastrous since for an entire horizontal
        /// range of test points, we will either overcount or undercount
        /// the crossings and get a wrong answer for all of them, even
        /// when they are clearly and obviously inside or outside the
        /// curve.
        ///
        /// To work around this problem, we try a couple of Newton-Raphson
        /// iterations to see if the true root is closer to the endpoint
        /// or further away.  If it is further away, then we can stop
        /// since we know we are on the right side of the endpoint.  If
        /// we change direction, then either we are now being dragged away
        /// from the endpoint in which case the first condition will cause
        /// us to stop, or we have passed the endpoint and are headed back.
        /// In the second case, we simply evaluate the slope at the
        /// endpoint itself and place ourselves on the appropriate side
        /// of it or on it depending on that result.
        /// </summary>
        /// <param name="res">The res.</param>
        /// <param name="eqn">The eqn.</param>
        private static void FixRoots(double[] res, double[] eqn)
        {
            const double epsilon = 1E-5;
            for (int i = 0; i < 3; i++)
            {
                double t = res[i];
                if (MathEx.Abs(t) < epsilon)
                {
                    res[i] = FindZero(t, 0, eqn);
                }
                else if (MathEx.Abs(t - 1) < epsilon)
                {
                    res[i] = FindZero(t, 1, eqn);
                }
            }
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Fill an array with the coefficients of the parametric equation
        /// in t, ready for solving against val with solveCubic.
        /// We currently have:
        /// <pre>
        ///   val = P(t) = C1(1-t)^3 + 3CP1 t(1-t)^2 + 3CP2 t^2(1-t) + C2 t^3
        ///              = C1 - 3C1t + 3C1t^2 - C1t^3 +
        ///                3CP1t - 6CP1t^2 + 3CP1t^3 +
        ///                3CP2t^2 - 3CP2t^3 +
        ///                C2t^3
        ///            0 = (C1 - val) +
        ///                (3CP1 - 3C1) t +
        ///                (3C1 - 6CP1 + 3CP2) t^2 +
        ///                (C2 - 3CP2 + 3CP1 - C1) t^3
        ///            0 = C + Bt + At^2 + Dt^3
        ///     C = C1 - val
        ///     B = 3///CP1 - 3///C1
        ///     A = 3///CP2 - 6///CP1 + 3///C1
        ///     D = C2 - 3///CP2 + 3///CP1 - C1
        /// </pre>
        /// </summary>
        /// <param name="eqn">The eqn.</param>
        /// <param name="val">The val.</param>
        /// <param name="c1">The c1.</param>
        /// <param name="cp1">The CP1.</param>
        /// <param name="cp2">The CP2.</param>
        /// <param name="c2">The c2.</param>
        private static void FillEqn(double[] eqn, double val,
                                    double c1, double cp1, double cp2, double c2)
        {
            eqn[0] = c1 - val;
            eqn[1] = (cp1 - c1)*3.0;
            eqn[2] = (cp2 - cp1 - cp1 + c1)*3.0;
            eqn[3] = c2 + (cp1 - cp2)*3.0 - c1;
            return;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Evaluate the t values in the first num slots of the vals[] array
        /// and place the evaluated values back into the same array.  Only
        /// evaluate t values that are within the range [0, 1], including
        /// the 0 and 1 ends of the range iff the include0 or include1
        /// booleans are true.  If an "inflection" equation is handed in,
        /// then any points which represent a point of inflection for that
        /// cubic equation are also ignored.
        /// </summary>
        /// <param name="vals">The vals.</param>
        /// <param name="num">The num.</param>
        /// <param name="include0">if set to true [include0].</param>
        /// <param name="include1">if set to true [include1].</param>
        /// <param name="inflect">The inflect.</param>
        /// <param name="c1">The c1.</param>
        /// <param name="cp1">The CP1.</param>
        /// <param name="cp2">The CP2.</param>
        /// <param name="c2">The c2.</param>
        /// <returns></returns>
        private static int EvalCubic(double[] vals, int num,
                                     bool include0,
                                     bool include1,
                                     double[] inflect,
                                     double c1, double cp1,
                                     double cp2, double c2)
        {
            int j = 0;
            for (int i = 0; i < num; i++)
            {
                double t = vals[i];
                if ((include0 ? t >= 0 : t > 0) &&
                    (include1 ? t <= 1 : t < 1) &&
                    (inflect == null ||
                     inflect[1] + (2*inflect[2] + 3*inflect[3]*t)*t != 0))
                {
                    double u = 1 - t;
                    vals[j++] = c1*u*u*u + 3*cp1*t*u*u + 3*cp2*t*t*u + c2*t*t*t;
                }
            }
            return j;
        }

        ////////////////////////////////////////////////////////////////////////////
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

        ////////////////////////////////////////////////////////////////////////////
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


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Solves the eqn.
        /// </summary>
        /// <param name="eqn">The eqn.</param>
        /// <param name="order">The order.</param>
        /// <param name="t">The t.</param>
        /// <returns></returns>
        private static double SolveEqn(double[] eqn, int order, double t)
        {
            double v = eqn[order];
            while (--order >= 0)
            {
                v = v*t + eqn[order];
            }
            return v;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Finds the zero.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <param name="target">The target.</param>
        /// <param name="eqn">The eqn.</param>
        /// <returns></returns>
        private static double FindZero(double t, double target, double[] eqn)
        {
            double[] slopeqn = {eqn[1], 2*eqn[2], 3*eqn[3]};
            double origdelta = 0;
            double origt = t;
            while (true)
            {
                double slope = SolveEqn(slopeqn, 2, t);
                if (slope == 0)
                {
                    // At a local minima - must return
                    return t;
                }
                double y = SolveEqn(eqn, 3, t);
                if (y == 0)
                {
                    // Found it! - return it
                    return t;
                }
                // assert(slope != 0 && y != 0);
                double delta = -(y/slope);
                // assert(delta != 0);
                if (origdelta == 0)
                {
                    origdelta = delta;
                }
                if (t < target)
                {
                    if (delta < 0)
                    {
                        return t;
                    }
                }
                else if (t > target)
                {
                    if (delta > 0)
                    {
                        return t;
                    }
                }
                else
                {
                    /* t == target */
                    return (delta > 0
                                ? (target + double.MinValue)
                                : (target - double.MinValue));
                }
                double newt = t + delta;
                if (t == newt)
                {
                    // The deltas are so small that we aren't moving...
                    return t;
                }
                if (delta*origdelta < 0)
                {
                    // We have reversed our path.
                    int tag = (origt < t
                                   ? GetTag(target, origt, t)
                                   : GetTag(target, t, origt));
                    if (tag != Inside)
                    {
                        // Local minima found away from target - return the middle
                        return (origt + t)/2;
                    }
                    // Local minima somewhere near target - move to target
                    // and let the slope determine the resulting t.
                    t = target;
                }
                else
                {
                    t = newt;
                }
            }
        }
    }
}