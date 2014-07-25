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
    /// A Rectangle specifies an area in a coordinate space that is
    /// enclosed by the Rectangle object's upper-left point
    /// (x,y)
    /// in the coordinate space, its width, and its height.
    /// </summary>
    /// <remarks>
    /// 
    /// A Rectangle object's width and
    /// height are public fields. The constructors
    /// that create a Rectangle, and the methods that can modify
    /// one, do not prevent setting a negative value for width or height.
    /// 
    /// A Rectangle whose width or height is exactly zero has location
    /// along those axes with zero dimension, but is otherwise considered empty.
    /// The isEmpty method will return true for such a Rectangle.
    /// Methods which test if an empty Rectangle contains or intersects
    /// a point or rectangle will always return false if either dimension is zero.
    /// Methods which combine such a Rectangle with a point or rectangle
    /// will include the location of the Rectangle on that axis in the
    /// result as if the add(Point) method were being called.
    /// A Rectangle whose width or height is negative has neither
    /// location nor dimension along those axes with negative dimensions.
    /// Such a Rectangle is treated as non-existant along those axes.
    /// Such a Rectangle is also empty with respect to containment
    /// calculations and methods which test if it contains or intersects a
    /// point or rectangle will always return false.
    /// Methods which combine such a Rectangle with a point or rectangle
    /// will ignore the Rectangle entirely in generating the result.
    /// If two Rectangle objects are combined and each has a negative
    /// dimension, the result will have at least one negative dimension.
    /// 
    /// Methods which affect only the location of a Rectangle will
    /// operate on its location regardless of whether or not it has a negative
    /// or zero dimension along either axis.
    /// 
    /// Remember that a Rectangle constructed with the default no-argument
    /// constructor will have dimensions of 0x0 and therefore be empty.
    /// That Rectangle will still have a location of (0,0) and
    /// will contribute that location to the union and add operations.
    /// Code attempting to accumulate the bounds of a set of points should
    /// therefore initially construct the Rectangle with a specifically
    /// negative width and height or it should use the first point in the set
    /// to construct the Rectangle.
    /// For example:
    /// <pre>
    ///     Rectangle bounds = new Rectangle(0, 0, -1, -1);
    ///     for (int i = 0; i &lt; points.length; i++) {
    ///         bounds.add(points[i]);
    ///     }
    /// </pre>
    /// or if we know that the points array contains at least one point:
    /// <pre>
    ///     Rectangle bounds = new Rectangle(points[0]);
    ///     for (int i = 1; i &lt; points.length; i++) {
    ///         bounds.add(points[i]);
    ///     }
    /// </pre>
    /// 
    /// This class uses 32-bit integers to store its location and dimensions.
    /// Frequently operations may produce a result that exceeds the range of
    /// a 32-bit integer.
    /// The methods will calculate their results in a way that avoids any
    /// 32-bit overflow for intermediate results and then choose the best
    /// representation to store the final results back into the 32-bit fields
    /// which hold the location and dimensions.
    /// The location of the result will be stored into the xand
    /// yfields by clipping the true result to the nearest 32-bit value.
    /// The values stored into the width and height dimension
    /// fields will be chosen as the 32-bit values that encompass the largest
    /// part of the true result as possible.
    /// Generally this means that the dimension will be clipped independently
    /// to the range of 32-bit integers except that if the location had to be
    /// moved to store it into its pair of 32-bit fields then the dimensions
    /// will be adjusted relative to the "best representation" of the location.
    /// If the true result had a negative dimension and was therefore
    /// non-existant along one or both axes, the stored dimensions will be
    /// negative numbers in those axes.
    /// If the true result had a location that could be represented within
    /// the range of 32-bit integers, but zero dimension along one or both
    /// axes, then the stored dimensions will be zero in those axes.
    ///
    ///</remarks>
    public class Rectangle : RectangularShape
    {

        /// <summary>
        /// The bitmask that indicates that a point lies to the left of
        /// this Rectangle.
        /// </summary>
        public const int OutLeft = 1;


        /// <summary>
        ///  The bitmask that indicates that a point lies above
        /// this Rectangle.
        /// </summary>
        public const int OutTop = 2;


        /// <summary>
        /// The bitmask that indicates that a point lies to the right of
        /// this Rectangle.
        /// </summary>
        public const int OutRight = 4;


        /// <summary>
        /// The bitmask that indicates that a point lies below
        /// this Rectangle.
        /// </summary>
        public const int OutBottom = 8;


        /// <summary>
        /// The X coordinate of the upper-left corner of the Rectangle.
        /// </summary>
        public int X;


        /// <summary>
        /// The Y coordinate of the upper-left corner of the Rectangle.
        /// </summary>
        public int Y;


        /// <summary>
        /// The width of the Rectangle.
        /// </summary>
        public int Width;


        /// <summary>
        /// The height of the Rectangle.
        /// </summary>
        public int Height;

        private readonly Dimension _size;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> class.
        /// </summary>
        public Rectangle()
            : this(0, 0, 0, 0)
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a new Rectangle, initialized to match
        /// the values of the specified Rectangle.
        /// </summary>
        /// <param name="r">the Rectangle from which to copy initial values
        ///           to a newly constructed Rectangle</param>
        public Rectangle(Rectangle r)
            : this(r.X, r.Y, r.Width, r.Height)
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a new Rectangle whose upper-left corner is
        /// specified as
        /// (x,y) and whose width and height
        /// are specified by the arguments of the same name.
        /// </summary>
        /// <param name="x">the specified X coordinate.</param>
        /// <param name="y">the specified Y coordinate.</param>
        /// <param name="width">the width of the Rectangle.</param>
        /// <param name="height">the height of the Rectangle.</param>
        public Rectangle(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            _size = new Dimension(width, height);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a new Rectangle whose upper-left corner
        /// is at (0,0) in the coordinate space, and whose width and
        /// height are specified by the arguments of the same name.
        /// </summary>
        /// <param name="width">the width of the Rectangle.</param>
        /// <param name="height">the height of the Rectangle.</param>
        public Rectangle(int width, int height)
            : this(0, 0, width, height)
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a new Rectangle with given two points.
        /// </summary>
        /// <param name="p1">one corner point of the rectangle.</param>
        /// <param name="p2">one corner point of the rectangle.</param>
        public Rectangle(Point p1, Point p2)
            : this(Math.Min(p1.X, p2.X),
                   Math.Min(p1.Y, p2.Y),
                   Math.Abs(p1.X - p2.X),
                   Math.Abs(p1.Y - p2.Y))
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates a new instance of Rectangle at position (x, y) and with
        ///  predefine dimension
        /// </summary>
        /// <param name="x">x the x coordinate of the rectangle</param>
        /// <param name="y">y the y coordinate of the rectangle</param>
        /// <param name="d">d the Dimension of the rectangle</param>
        public Rectangle(int x, int y, Dimension d)
            : this(x, y, d.Width, d.Height)
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a new Rectangle whose upper-left corner is
        /// specified by the Point argument, and
        /// whose width and height are specified by the
        /// Dimension argument.
        /// </summary>
        /// <param name="p">a Point that is the upper-left corner of
        /// the Rectangle</param>
        /// <param name="d">a Dimension, representing the
        /// width and height of the Rectangle</param>
        public Rectangle(Point p, Dimension d)
            : this(p.X, p.Y, d.Width, d.Height)
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a new Rectangle whose upper-left corner is the
        /// specified Point, and whose width and height are both zero.
        /// </summary>
        /// <param name="p">a Point that is the top left corner
        /// of the Rectangle.</param>
        public Rectangle(Point p)
            : this(p.X, p.Y, 0, 0)
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a new Rectangle whose top left corner is
        /// (0,0) and whose width and height are specified
        /// by the Dimension argument.
        /// </summary>
        /// <param name="d">a Dimension, specifying width and height</param>
        public Rectangle(Dimension d)
            : this(0, 0, d.Width, d.Height)
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets this Rectangle to be the same as the specified Rectangle.
        /// </summary>
        /// <param name="r">the specified Rectangle</param>
        public void SetRect(Rectangle r)
        {
            SetRect(r.IntX, r.IntY, r.IntWidth, r.IntHeight);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the bounds of this Rectangle to the integer bounds
        /// which encompass the specified x, y, width,
        /// and height.
        /// </summary>
        /// <remarks>
        /// If the parameters specify a Rectangle that exceeds the
        /// maximum range of integers, the result will be the best
        /// representation of the specified Rectangle intersected
        /// with the maximum integer bounds.
        /// </remarks>
        /// <param name="x">the X coordinate of the upper-left corner of
        ///                   the specified rectangle</param>
        /// <param name="y">the Y coordinate of the upper-left corner of
        ///                  the specified rectangle</param>
        /// <param name="width">the width of the specified rectangle</param>
        /// <param name="height">the new height of the specified rectangle.</param>
        public void SetRect(int x, int y, int width, int height)
        {
            int newx, newy, neww, newh;

            if (x > int.MaxValue/2)
            {
                // Too far in positive X direction to represent...
                // We cannot even reach the left side of the specified
                // rectangle even with both x & width set to MAX_VALUE.
                // The intersection with the "maximal integer rectangle"
                // is non-existant so we should use a width < 0.
                // REMIND: Should we try to determine a more "meaningful"
                // adjusted Value for neww than just "-1"?
                newx = int.MaxValue/2;
                neww = -1;
            }
            else
            {
                newx = Clip(x, false);
                if (width >= 0)
                {
                    width += x - newx;
                }
                neww = Clip(width, width >= 0);
            }

            if (y > int.MaxValue)
            {
                // Too far in positive Y direction to represent...
                newy = int.MaxValue/2;
                newh = -1;
            }
            else
            {
                newy = Clip(y, false);
                if (height >= 0)
                {
                    height += y - newy;
                }
                newh = Clip(height, height >= 0);
            }

            Reshape(newx, newy, neww, newh);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tests if the specified line segment intersects the interior of this
        /// Rectangle.
        /// </summary>
        /// <param name="x1">the X coordinate of the start point of the specified
        ///            line segment</param>
        /// <param name="y1">the Y coordinate of the start point of the specified
        ///            line segment</param>
        /// <param name="x2">the X coordinate of the end point of the specified
        ///            line segment.</param>
        /// <param name="y2">the Y coordinate of the end point of the specified
        ///            line segment</param>
        /// <returns>true if the specified line segment intersects
        /// the interior of this Rectangle; false
        /// otherwise.</returns>
        public bool IntersectsLine(int x1, int y1, int x2, int y2)
        {
            int out1, out2;
            if ((out2 = Outcode(x2, y2)) == 0)
            {
                return true;
            }
            while ((out1 = Outcode(x1, y1)) != 0)
            {
                if ((out1 & out2) != 0)
                {
                    return false;
                }
                if ((out1 & (OutLeft | OutRight)) != 0)
                {
                    int xp = IntX;
                    if ((out1 & OutRight) != 0)
                    {
                        xp += IntWidth;
                    }
                    y1 = y1 + (xp - x1)*(y2 - y1)/(x2 - x1);
                    x1 = xp;
                }
                else
                {
                    int yp = IntY;
                    if ((out1 & OutBottom) != 0)
                    {
                        yp += IntHeight;
                    }
                    x1 = x1 + (yp - y1)*(x2 - x1)/(y2 - y1);
                    y1 = yp;
                }
            }
            return true;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tests if the specified line segment intersects the interior of this
        /// Rectangle.
        /// </summary>
        /// <param name="l">the specified Line to test for intersection
        /// with the interior of this Rectangle.</param>
        /// <returns> true if the specified Line
        /// intersects the interior of this Rectangle;
        /// false otherwise.</returns>
        public bool IntersectsLine(Line l)
        {
            return IntersectsLine(l.X1, l.Y1, l.X2, l.Y2);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determines where the specified Point lies with
        /// respect to this Rectangle.
        /// </summary>
        /// <remarks>
        /// This method computes a binary OR of the appropriate mask values
        /// indicating, for each side of this Rectangle,
        /// whether or not the specified Point is on the same
        /// side of the edge as the rest of this Rectangle.
        /// </remarks>
        /// <param name="p">the specified Point</param>
        /// <returns>the logical OR of all appropriate out codes.</returns>
        public int Outcode(Point p)
        {
            return Outcode(p.X, p.Y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the location and size of the framing rectangle of this
        /// IShape to the specified rectangular values.
        /// </summary>
        /// <param name="x">the X coordinate of the upper-left corner of the
        /// specified rectangular shape</param>
        /// <param name="y">the Y coordinate of the upper-left corner of the
        /// specified rectangular shape</param>
        /// <param name="w">the width of the specified rectangular shape</param>
        /// <param name="h">the height of the specified rectangular shape</param>
        public override void SetFrame(int x, int y, int w, int h)
        {
            SetRect(x, y, w, h);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determines whether [contains] [the specified r X].
        /// </summary>
        /// <param name="rX">The r X.</param>
        /// <param name="rY">The r Y.</param>
        /// <returns>
        /// 	<c>true</c> if [contains] [the specified r X]; otherwise, <c>false</c>.
        /// </returns>
        public override bool Contains(int rX, int rY)
        {
            return Inside(rX, rY);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Intersectses the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public override bool Intersects(int x, int y, int width, int height)
        {
            int tw = _size.Width;
            int th = _size.Height;
            return Intersects(X, Y, tw, th, x, y, width, height);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Intersectses the specified rect.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <returns></returns>
        public override bool Intersects(Rectangle rect)
        {
            return Intersects(rect.IntX, rect.IntY,
                              rect.GetSize().Width, rect.GetSize().Height);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Helper method allowing us to determine if two coordinate sets intersect. This saves
        /// us the need of creating a rectangle object for a quick calculation
        /// </summary>
        /// <param name="tx">x of first rectangl</param>
        /// <param name="ty">y of first rectangle.</param>
        /// <param name="tw">width of first rectangle.</param>
        /// <param name="th">height of first rectangle</param>
        /// <param name="x">x of second rectangle</param>
        /// <param name="y">y of second rectangle</param>
        /// <param name="width">width width of second rectangle.</param>
        /// <param name="height">height height of second rectangle.</param>
        /// <returns>true if the rectangles intersect</returns>
        public static bool Intersects(int tx, int ty, int tw, int th, int x, int y, int width, int height)
        {
            int rw = width;
            int rh = height;
            if (rw <= 0 || rh <= 0 || tw <= 0 || th <= 0)
            {
                return false;
            }
            int rx = x;
            int ry = y;
            rw += rx;
            rh += ry;
            tw += tx;
            th += ty;
            return ((rw < rx || rw > tx) &&
                    (rh < ry || rh > ty) &&
                    (tw < tx || tw > rx) &&
                    (th < ty || th > ry));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determines whether [contains] [the specified r X].
        /// </summary>
        /// <param name="rX">The r X.</param>
        /// <param name="rY">The r Y.</param>
        /// <param name="rWidth">Width of the r.</param>
        /// <param name="rHeight">Height of the r.</param>
        /// <returns>
        /// 	<c>true</c> if [contains] [the specified r X]; otherwise, <c>false</c>.
        /// </returns>
        public override bool Contains(int rX, int rY, int rWidth, int rHeight)
        {
            return X <= rX && Y <= rY && X + IntWidth >= rX + rWidth &&
                   Y + IntHeight >= rY + rHeight;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Intersects the pair of specified source Rectangle
        /// objects and puts the result into the specified destination
        /// Rectangle object. 
        /// </summary>
        /// <remarks>
        ///  One of the source rectangles
        /// can also be the destination to avoid creating a third Rectangle
        /// object, but in this case the original points of this source
        /// rectangle will be overwritten by this method.
        /// </remarks>
        /// <param name="src1"> the first of a pair of Rectangle
        /// objects to be intersected with each other</param>
        /// <param name="src2">the second of a pair of Rectangle
        ///  objects to be intersected with each other.</param>
        /// <param name="dest">the Rectangle that holds the
        /// results of the intersection of src1 and
        /// src2.</param>
        public static void Intersect(Rectangle src1,
                                     Rectangle src2,
                                     Rectangle dest)
        {
            int x1 = Math.Max(src1.IntMinX, src2.IntMinX);
            int y1 = Math.Max(src1.IntMinY, src2.IntMinY);
            int x2 = Math.Min(src1.IntMaxX, src2.IntMaxX);
            int y2 = Math.Min(src1.IntMaxY, src2.IntMaxY);
            dest.SetFrame(x1, y1, x2 - x1, y2 - y1);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Unions the pair of source Rectangle objects
        /// and puts the result into the specified destination
        /// Rectangle object.  
        /// </summary>
        /// <remarks>
        /// One of the source rectangles
        /// can also be the destination to avoid creating a third Rectangle
        /// object, but in this case the original points of this source
        /// rectangle will be overwritten by this method.
        /// </remarks>
        /// <param name="src1"> the first of a pair of Rectangle
        /// objects to be intersected with each other</param>
        /// <param name="src2">the second of a pair of Rectangle
        ///  objects to be intersected with each other.</param>
        /// <param name="dest">the Rectangle that holds the
        /// results of the intersection of src1 and
        /// src2.</param>
        public static void Union(Rectangle src1,
                                 Rectangle src2,
                                 Rectangle dest)
        {
            int x1 = Math.Min(src1.IntMinX, src2.IntMinX);
            int y1 = Math.Min(src1.IntMinY, src2.IntMinY);
            int x2 = Math.Max(src1.IntMaxX, src2.IntMaxX);
            int y2 = Math.Max(src1.IntMaxY, src2.IntMaxY);
            dest.SetFrameFromDiagonal(x1, y1, x2, y2);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Adds a point, specified by the int precision arguments
        /// newx and newy, to this Rectangle.  
        /// </summary>
        /// <remarks>
        /// The resulting Rectangle
        /// is the smallest Rectangle that
        /// contains both the original Rectangle and the
        /// specified point.
        /// 
        /// After adding a point, a call to contains with the
        /// added point as an argument does not necessarily return
        /// true. The contains method does not
        /// return true for points on the right or bottom
        /// edges of a rectangle. Therefore, if the added point falls on
        /// the left or bottom edge of the enlarged rectangle,
        /// contains returns false for that point.
        /// </remarks>
        /// <param name="newx">the X coordinate of the new point.</param>
        /// <param name="newy">the Y coordinate of the new point.</param>
        public void Add(int newx, int newy)
        {
            int x1 = Math.Min(IntMinX, newx);
            int x2 = Math.Max(IntMaxX, newx);
            int y1 = Math.Min(IntMinY, newy);
            int y2 = Math.Max(IntMaxY, newy);
            SetRect(x1, y1, x2 - x1, y2 - y1);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Adds the Point object pt to this  Rectangle.
        /// </summary>
        /// <remarks>
        /// The resulting Rectangle is the smallest
        /// Rectangle that contains both the original
        /// Rectangle and the specified Point.
        /// After adding a point, a call to contains with the
        /// added point as an argument does not necessarily return
        /// true. The contains
        /// method does not return true for points on the right
        /// or bottom edges of a rectangle. Therefore, if the added point falls
        /// on the left or bottom edge of the enlarged rectangle,
        /// contains returns false for that point.
        /// </remarks>
        /// <param name="pt"> pt the new Point to add to this
        /// Rectangle.</param>
        public void Add(Point pt)
        {
            Add(pt.X, pt.Y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Adds a Rectangle object to this Rectangle.  
        /// </summary>
        /// <remarks>
        /// The resulting Rectangle
        /// is the union of the two Rectangle objects.
        /// </remarks>
        /// <param name="r">the Rectangle to add to this
        /// Rectangle.</param>
        public void Add(Rectangle r)
        {
            int x1 = Math.Min(IntMinX, r.IntMinX);
            int x2 = Math.Max(IntMaxX, r.IntMaxX);
            int y1 = Math.Min(IntMinY, r.IntMinY);
            int y2 = Math.Max(IntMaxY, r.IntMaxY);
            SetRect(x1, y1, x2 - x1, y2 - y1);
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
        public override PathIterator GetPathIterator(AffineTransform at)
        {
            return new RectIterator(this, at);
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
        public override PathIterator GetPathIterator(AffineTransform at, int flatness)
        {
            return new RectIterator(this, at);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms 
        /// and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            int bits = (int) (X & 0xFFFF0000 + Y & 0x0000FFFF);

            return bits;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to
        ///  this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            if (obj is Rectangle)
            {
                Rectangle r2D = (Rectangle) obj;
                return ((X == r2D.X) &&
                        (X == r2D.Y) &&
                        (Width == r2D.Width) &&
                        (Height == r2D.Height));
            }
            return false;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the X coordinate of the upper-left corner of
        /// the framing rectangle.
        /// </summary>
        public override int IntX
        {
            get { return X; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the Y coordinate of the upper-left corner of
        /// the framing rectangle .
        /// </summary>
        public override int IntY
        {
            get { return Y; }
        }
     
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the width of the framing rectangle.
        /// </summary>
        public override int IntWidth
        {
            get { return Width; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the height of the framing rectangle.
        /// </summary>
        public override int IntHeight
        {
            get { return Height; }
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
        public override Rectangle Bounds
        {
            get { return new Rectangle(X, Y, Width, Height); }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the bounding Rectangle of this Rectangle
        /// to match the specified Rectangle.
        /// </summary>
        /// <remarks>
        /// This method is included for completeness, to parallel the
        /// setBounds method of Component.
        /// </remarks>
        /// <param name="r"> the specified Rectangle</param>
        public void SetBounds(Rectangle r)
        {
            SetFrame(r.X, r.Y, r.Width, r.Height);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the bounding Rectangle of this Rectangle to the specified
        /// x, y, width, and height.
        /// </summary>
        /// <remarks>
        /// This method is included for completeness, to parallel the
        /// setBounds method of Component.
        /// </remarks>
        /// <param name="x">the new X coordinate for the upper-left
        ///                     corner of this Rectangle</param>
        /// <param name="y">the new Y coordinate for the upper-left
        ///                     corner of this Rectangle</param>
        /// <param name="width">the new width for this Rectangle</param>
        /// <param name="height">the new height for this Rectangle</param>
        public void SetBounds(int x, int y, int width, int height)
        {
            Reshape(x, y, width, height);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the location of this Rectangle.
        /// </summary>
        /// <remarks>
        /// This method is included for completeness, to parallel the
        /// getLocation method of Component.
        /// </remarks>
        /// <returns>the Point that is the upper-left corner of
        /// 			this Rectangle.</returns>
        public Point GetLocation()
        {
            return new Point(X, Y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Moves this Rectangle to the specified location.
        /// </summary>
        /// <remarks>
        /// This method is included for completeness, to parallel the
        /// setLocation method of Component.
        /// </remarks>
        /// <param name="p">the Point specifying the new location
        ///                 for this Rectangle</param>
        public void SetLocation(Point p)
        {
            SetLocation(p.X, p.Y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Moves this Rectangle to the specified location.
        /// </summary>
        /// <remarks>
        /// This method is included for completeness, to parallel the
        /// setLocation method of Component.
        /// </remarks>
        /// <param name="x"> the X coordinate of the new location</param>
        /// <param name="y"> the Y coordinate of the new location</param>
        public void SetLocation(int x, int y)
        {
            Move(x, y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Translates this Rectangle the indicated distance,
        /// to the right along the X coordinate axis, and
        /// downward along the Y coordinate axis.
        /// </summary>
        /// <param name="dx">the distance to move this Rectangle
        ///                  along the X axis</param>
        /// <param name="dy">the distance to move this Rectangle
        ///                  along the Y axis</param>
        public void Translate(int dx, int dy)
        {
            int oldv = X;
            int newv = oldv + dx;
            if (dx < 0)
            {
                // moving leftward
                if (newv > oldv)
                {
                    // negative overflow
                    // Only adjust width if it was valid (>= 0).
                    if (Width >= 0)
                    {
                        // The right edge is now conceptually at
                        // newv+width, but we may move newv to prevent
                        // overflow.  But we want the right edge to
                        // remain at its new location in spite of the
                        // clipping.  Think of the following adjustment
                        // conceptually the same as:
                        // width += newv; newv = MIN_VALUE; width -= newv;
                        Width += newv - int.MinValue;
                        // width may go negative if the right edge went past
                        // MIN_VALUE, but it cannot overflow since it cannot
                        // have moved more than MIN_VALUE and any non-negative
                        // number + MIN_VALUE does not overflow.
                    }
                    newv = int.MinValue;
                }
            }
            else
            {
                // moving rightward (or staying still)
                if (newv < oldv)
                {
                    // positive overflow
                    if (Width >= 0)
                    {
                        // Conceptually the same as:
                        // width += newv; newv = MAX_VALUE; width -= newv;
                        Width += newv - int.MaxValue;
                        // With large widths and large displacements
                        // we may overflow so we need to check it.
                        if (Width < 0)
                        {
                            Width = int.MaxValue;
                        }
                    }
                    newv = int.MaxValue;
                }
            }
            X = newv;

            oldv = Y;
            newv = oldv + dy;
            if (dy < 0)
            {
                // moving upward
                if (newv > oldv)
                {
                    // negative overflow
                    if (Height >= 0)
                    {
                        Height += newv - int.MinValue;
                        // See above comment about no overflow in this case
                    }
                    newv = int.MinValue;
                }
            }
            else
            {
                // moving downward (or staying still)
                if (newv < oldv)
                {
                    // positive overflow
                    if (Height >= 0)
                    {
                        Height += newv - int.MaxValue;
                        if (Height < 0)
                        {
                            Height = int.MaxValue;
                        }
                    }
                    newv = int.MaxValue;
                }
            }
            Y = newv;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the size of this Rectangle, represented by
        /// the returned Dimension.
        /// </summary>
        /// <remarks>
        /// This method is included for completeness, to parallel the
        /// getSize method of Component.
        /// </remarks>
        /// <returns>a Dimension, representing the size of
        ///             this Rectangle.</returns>
        public Dimension GetSize()
        {
            return _size;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the size of this Rectangle to match the
        /// specified Dimension.
        /// </summary>
        /// <remarks>
        /// This method is included for completeness, to parallel the
        /// setSize method of Component.
        /// </remarks>
        /// <param name="d"> the new size for the Dimension object</param>
        public void SetSize(Dimension d)
        {
            SetSize(d.Width, d.Height);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the size of this Rectangle to the specified
        /// width and height.
        /// </summary>
        /// <remarks>
        /// This method is included for completeness, to parallel the
        /// setSize method of Component.
        /// </remarks>
        /// <param name="width">the new width for this Rectangle</param>
        /// <param name="height">the new height for this Rectangle</param>
        public void SetSize(int width, int height)
        {
            Resize(width, height);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tests if a specified Point is inside the boundary of the IShape.
        /// </summary>
        /// <param name="p">the specified Point to be tested</param>
        /// <returns>
        /// 	true if the specified Point is
        /// inside the boundary of the IShape;
        /// false otherwise.
        /// </returns>
        public override bool Contains(Point p)
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
        /// Determines whether [contains] [the specified rect].
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <returns>
        /// 	<c>true</c> if [contains] [the specified rect]; otherwise, <c>false</c>.
        /// </returns>
        public override bool Contains(Rectangle rect)
        {
            return Contains(rect.X, rect.Y, rect._size.Width, rect._size.Height);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Computes the intersection of this Rectangle with the
        /// specified Rectangle.
        /// </summary>
        /// <remarks>
        ///  Returns a new Rectangle
        /// that represents the intersection of the two rectangles.
        /// If the two rectangles do not intersect, the result will be
        /// an empty rectangle.
        /// </remarks>
        /// <param name="r"> the specified Rectangle</param>
        /// <returns> the largest Rectangle contained in both the
        ///            specified Rectangle and in
        ///		  this Rectangle; or if the rectangles
        ///            do not intersect, an empty rectangle.</returns>
        public Rectangle Intersection(Rectangle r)
        {
            int tx1 = X;
            int ty1 = Y;
            int rx1 = r.X;
            int ry1 = r.Y;
            int tx2 = tx1;
            tx2 += Width;
            int ty2 = ty1;
            ty2 += Height;
            int rx2 = rx1;
            rx2 += r.Width;
            int ry2 = ry1;
            ry2 += r.Height;
            if (tx1 < rx1)
            {
                tx1 = rx1;
            }
            if (ty1 < ry1)
            {
                ty1 = ry1;
            }
            if (tx2 > rx2)
            {
                tx2 = rx2;
            }
            if (ty2 > ry2)
            {
                ty2 = ry2;
            }
            tx2 -= tx1;
            ty2 -= ty1;
            // tx2,ty2 will never overflow (they will never be
            // larger than the smallest of the two source w,h)
            // they might underflow, though...
            if (tx2 < int.MinValue)
            {
                tx2 = int.MinValue;
            }
            if (ty2 < int.MinValue)
            {
                ty2 = int.MinValue;
            }
            return new Rectangle(tx1, ty1, tx2, ty2);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Computes the union of this Rectangle with the
        /// specified Rectangle.
        /// </summary>
        /// <remarks>
        ///  Returns a new Rectangle that represents the union of the two rectangles.
        /// 
        /// If either Rectangle has any dimension less than zero
        /// the rules for non-existant rectangles apply.
        /// If only one has a dimension less than zero, then the result
        /// will be a copy of the other Rectangle.
        /// If both have dimension less than zero, then the result will
        /// have at least one dimension less than zero.
        /// 
        /// If the resulting Rectangle would have a dimension
        /// too large to be expressed as an int, the result
        /// will have a dimension of  Integer.MAX_VALUE along
        /// that dimension.
        /// </remarks>
        /// <param name="r">the specified Rectangle</param>
        /// <returns>the smallest Rectangle containing both
        ///		  the specified Rectangle and this
        ///		  Rectangle.</returns>
        public Rectangle Union(Rectangle r)
        {
            int tx2 = Width;
            int ty2 = Height;
            if ((tx2 | ty2) < 0)
            {
                // This rectangle has negative dimensions...
                // If r has non-negative dimensions then it is the answer.
                // If r is non-existant (has a negative dimension), then both
                // are non-existant and we can return any non-existant rectangle
                // as an answer.  Thus, returning r meets that criterion.
                // Either way, r is our answer.
                return new Rectangle(r);
            }
            int rx2 = r.Width;
            int ry2 = r.Height;
            if ((rx2 | ry2) < 0)
            {
                return new Rectangle(this);
            }
            int tx1 = X;
            int ty1 = Y;
            tx2 += tx1;
            ty2 += ty1;
            int rx1 = r.X;
            int ry1 = r.Y;
            rx2 += rx1;
            ry2 += ry1;
            if (tx1 > rx1)
            {
                tx1 = rx1;
            }
            if (ty1 > ry1)
            {
                ty1 = ry1;
            }
            if (tx2 < rx2)
            {
                tx2 = rx2;
            }
            if (ty2 < ry2)
            {
                ty2 = ry2;
            }
            tx2 -= tx1;
            ty2 -= ty1;
            // tx2,ty2 will never underflow since both original rectangles
            // were already proven to be non-empty
            // they might overflow, though...
            if (tx2 > int.MaxValue)
            {
                tx2 = int.MaxValue;
            }
            if (ty2 > int.MaxValue)
            {
                ty2 = int.MaxValue;
            }
            return new Rectangle(tx1, ty1, tx2, ty2);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Resizes the Rectangle both horizontally and vertically.
        /// </summary>
        /// <remarks>
        /// This method modifies the Rectangle so that it is
        /// h units larger on both the left and right side,
        /// and v units larger at both the top and bottom.
        /// 
        /// The new Rectangle has (x - h, y - v)
        /// as its upper-left corner,
        /// width of  (width + 2h),
        /// and a height of (height + 2v).
        /// 
        /// If negative values are supplied for h and
        /// v, the size of the Rectangle
        /// decreases accordingly.
        /// The  grow method will check for integer overflow
        /// and underflow, but does not check whether the resulting
        /// values of width and height grow
        /// from negative to non-negative or shrink from non-negative
        /// to negative.
        /// </remarks>
        /// <param name="h">the horizontal expansion.</param>
        /// <param name="v">the vertical expansion.</param>
        public void Grow(int h, int v)
        {
            int x0 = X;
            int y0 = Y;
            int x1 = Width;
            int y1 = Height;
            x1 += x0;
            y1 += y0;

            x0 -= h;
            y0 -= v;
            x1 += h;
            y1 += v;

            if (x1 < x0)
            {
                // Non-existant in X direction
                // Final width must remain negative so subtract x0 before
                // it is clipped so that we avoid the risk that the clipping
                // of x0 will reverse the ordering of x0 and x1.
                x1 -= x0;
                if (x1 < int.MinValue)
                {
                    x1 = int.MinValue;
                }
                if (x0 < int.MinValue)
                {
                    x0 = int.MinValue;
                }
                else if (x0 > int.MaxValue)
                {
                    x0 = int.MaxValue;
                }
            }
            else
            {
                // (x1 >= x0)
                // Clip x0 before we subtract it from x1 in case the clipping
                // affects the representable area of the rectangle.
                if (x0 < int.MinValue)
                {
                    x0 = int.MinValue;
                }
                else if (x0 > int.MaxValue)
                {
                    x0 = int.MaxValue;
                }
                x1 -= x0;
                // The only way x1 can be negative now is if we clipped
                // x0 against MIN and x1 is less than MIN - in which case
                // we want to leave the width negative since the result
                // did not intersect the representable area.
                if (x1 < int.MinValue)
                {
                    x1 = int.MinValue;
                }
                else if (x1 > int.MaxValue)
                {
                    x1 = int.MaxValue;
                }
            }

            if (y1 < y0)
            {
                // Non-existant in Y direction
                y1 -= y0;
                if (y1 < int.MinValue)
                {
                    y1 = int.MinValue;
                }
                if (y0 < int.MinValue)
                {
                    y0 = int.MinValue;
                }
                else if (y0 > int.MaxValue)
                {
                    y0 = int.MaxValue;
                }
            }
            else
            {
                // (y1 >= y0)
                if (y0 < int.MinValue)
                {
                    y0 = int.MinValue;
                }
                else if (y0 > int.MaxValue)
                {
                    y0 = int.MaxValue;
                }
                y1 -= y0;
                if (y1 < int.MinValue)
                {
                    y1 = int.MinValue;
                }
                else if (y1 > int.MaxValue)
                {
                    y1 = int.MaxValue;
                }
            }

            Reshape(x0, y0, x1, y1);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determines whether the RectangularShape is empty.
        /// When the RectangularShape is empty, it encloses no
        /// area.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance is empty; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsEmpty()
        {
            return (Width <= 0) || (Height <= 0);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set the rectangle to empty.
        /// </summary>
        public void SetEmpty()
        {
            X = Y = Width = Height = 0;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determines where the specified coordinates lie with respect
        /// to this Rectangle.
        /// </summary>
        /// <remarks>
        /// This method computes a binary OR of the appropriate mask values
        /// indicating, for each side of this Rectangle,
        /// whether or not the specified coordinates are on the same side
        /// of the edge as the rest of this Rectangle.
        /// </remarks>
        /// <param name="x">the specified X coordinate</param>
        /// <param name="y">the specified Y coordinate</param>
        /// <returns>the logical OR of all appropriate out codes.</returns>
        public int Outcode(int x, int y)
        {
            int outValue = 0;
            if (Width <= 0)
            {
                outValue |= OutLeft | OutRight;
            }
            else if (x < X)
            {
                outValue |= OutLeft;
            }
            else if (x > X + (long) Width)
            {
                outValue |= OutRight;
            }
            if (Height <= 0)
            {
                outValue |= OutTop | OutBottom;
            }
            else if (y < Y)
            {
                outValue |= OutTop;
            }
            else if (y > Y + (long) Height)
            {
                outValue |= OutBottom;
            }
            return outValue;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a new Rectangle object representing the
        /// intersection of this Rectangle with the specified
        /// Rectangle.
        /// </summary>
        /// <param name="r">the Rectangle to be intersected with
        /// this Rectangle</param>
        /// <returns>the largest Rectangle contained in both
        /// 		the specified Rectangle and in this
        ///		Rectangle.</returns>
        public Rectangle CreateIntersection(Rectangle r)
        {
            return Intersection(r);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a new Rectangle object representing the
        /// union of this Rectangle with the specified
        /// Rectangle.
        /// </summary>
        /// <param name="r">the Rectangle to be combined with
        /// this Rectangle</param>
        /// <returns>the smallest Rectangle containing both
        /// the specified Rectangle and this
        /// Rectangle.</returns>
        public Rectangle CreateUnion(Rectangle r)
        {
            return Union(r);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "RECT " + "[x=" + X + ",y=" + Y + ",width=" + Width + ",height=" + Height + "]";
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Return best integer representation for v, clipped to integer
        /// range and floor-ed or ceiling-ed, depending on the boolean.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <param name="doceil">if set to <c>true</c> [doceil].</param>
        /// <returns></returns>
        private static int Clip(int v, bool doceil)
        {
            if (v <= int.MinValue)
            {
                return int.MinValue;
            }
            if (v >= int.MaxValue)
            {
                return int.MaxValue;
            }
            return (int) (doceil ? Math.Ceiling((double) v) : Math.Floor((double) v));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the bounding Rectangle of this
        /// Rectangle to the specified
        /// x, y, width,
        /// and height.
        /// </summary>
        /// <param name="x">the new X coordinate for the upper-left
        ///                     corner of this Rectangle</param>
        /// <param name="y">the new Y coordinate for the upper-left
        ///                    corner of this Rectangle</param>
        /// <param name="width">the new width for this Rectangle.</param>
        /// <param name="height">the new height for this Rectangle.</param>
        private void Reshape(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            _size.Width = width;
            _size.Height = Height;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Moves this Rectangle to the specified location.
        /// </summary>
        /// <param name="x">the X coordinate of the new location</param>
        /// <param name="y">the Y coordinate of the new location</param>
        private void Move(int x, int y)
        {
            X = x;
            Y = y;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Sets the size of this Rectangle to the specified
        /// width and height.
        /// </summary>
        /// <param name="width">the new width for this Rectangle</param>
        /// <param name="height">the new height for this Rectangle</param>
        private void Resize(int width, int height)
        {
            Width = width;
            Height = height;
            _size.Width = width;
            _size.Height = Height;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Checks whether or not this Rectangle contains the
        /// point at the specified location (X,Y).
        /// </summary>
        /// <param name="x">the specified X coordinate.</param>
        /// <param name="y">the specified Y coordinate.</param>
        /// <returns>true if the point
        ///            (X,Y) is inside this
        ///		  Rectangle;
        ///            false otherwise.</returns>
        private bool Inside(int x, int y)
        {
            int w = Width;
            int h = Height;
            if ((w | h) < 0)
            {
                // At least one of the dimensions is negative...
                return false;
            }
            // Remember: if either dimension is zero, tests below must return false...
            int xp = X;
            int yp = Y;
            if (x < xp || y < yp)
            {
                return false;
            }
            w += xp;
            h += yp;
            //    overflow || intersect
            return ((w < xp || w > x) &&
                    (h < yp || h > y));
        }
    }
}