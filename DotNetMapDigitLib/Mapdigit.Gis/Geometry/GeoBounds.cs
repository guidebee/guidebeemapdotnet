//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 27SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System;
using Mapdigit.Util;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Gis.Geometry
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 27SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// GeoBounds is a rectangular area of the map in pixel coordinates.
    /// </summary>
    public class GeoBounds
    {

        /// <summary>
        /// The bitmask that indicates that a point lies to the left of
        /// this GeoBounds.
        /// </summary>
        public const int OutLeft = 1;

        /// <summary>
        /// The bitmask that indicates that a point lies above
        /// this GeoBounds.
        /// </summary>
        public const int OutTop = 2;


        /// <summary>
        /// 
        /// The bitmask that indicates that a point lies to the right of
        /// this GeoBounds.
        /// </summary>
        public const int OutRight = 4;


        /// <summary>
        /// The bitmask that indicates that a point lies below
        /// this GeoBounds.
        /// </summary>
        public const int OutBottom = 8;

        /// <summary>
        /// The X coordinate of this GeoBounds.
        /// </summary>
        public double X;

        /// <summary>
        /// The Y coordinate of this GeoBounds.
        /// </summary>
        public double Y;

        /// <summary>
        /// The Width of this GeoBounds.
        /// </summary>
        public double Width;

        /// <summary>
        /// The Height of this GeoBounds.
        /// </summary>
        public double Height;

        /// <summary>
        /// The X coordinate of the left edge of the rectangle.
        /// </summary>
        public double MinX;

        /// <summary>
        /// The Y coordinate of the top edge of the rectangle.
        /// </summary>
        public double MinY;

        /// <summary>
        /// The X coordinate of the right edge of the rectangle.
        /// </summary>
        public double MaxX;

        /// <summary>
        /// The Y coordinate of the bottom edge of the rectangle.
        /// </summary>
        public double MaxY;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a new GeoBounds whose upper-left corner 
        /// is at (0,0) in the coordinate space, and whose width and 
        /// height are both zero. 
        /// </summary>
        public GeoBounds()
            : this(0, 0, 0, 0)
        {

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// check if the geo bound is a empty rectangle.
        /// </summary>
        /// <returns>
        /// 	true,it's empty.
        /// </returns>
        public virtual bool IsEmpty()
        {
            return (Width <= 0.0) || (Height <= 0.0);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Reset the geo bound with new position and size.
        /// </summary>
        /// <param name="x">the x coordinate.</param>
        /// <param name="y">the y coordinate.</param>
        /// <param name="w">the width.</param>
        /// <param name="h">the height.</param>
        public void SetRect(double x, double y, double w, double h)
        {
            X = x;
            Y = y;
            Width = w;
            Height = h;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Reset the geo bound with same position and size with given geo bound.
        /// </summary>
        /// <param name="r">the geo bound to copy from.</param>
        public void SetRect(GeoBounds r)
        {
            X = r.X;
            Y = r.Y;
            Width = r.Width;
            Height = r.Height;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Check give (x,y)'s relative postion to this geo bound
        /// </summary>
        /// <param name="x"> x coordinate of the point.</param>
        /// <param name="y">y coordinate of the point.</param>
        /// <returns>relative position of the point.</returns>
        public int Outcode(double x, double y)
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
            else if (x > X + Width)
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
            else if (y > Y + Height)
            {
                outValue |= OutBottom;
            }
            return outValue;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Create the intersection rectangle between this rectangle and r rectangle.
        /// </summary>
        /// <param name="r"> the other rectangle.</param>
        /// <returns>the intersection rectangle.</returns>
        public GeoBounds CreateIntersection(GeoBounds r)
        {
            GeoBounds dest = new GeoBounds();
            Intersect(this, r, dest);
            return dest;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// create the union rectangle of the two rectangles.
        /// </summary>
        /// <param name="r">the other rectangle.</param>
        /// <returns>union rectangle of the two rectangles</returns>
        public GeoBounds CreateUnion(GeoBounds r)
        {
            GeoBounds dest = new GeoBounds();
            Union(this, r, dest);
            return dest;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a rectangle that contains all the given points.
        /// </summary>
        /// <param name="points">an array of points.</param>
        public GeoBounds(GeoPoint[] points)
            : this()
        {

            if (points == null)
            {
                SetRect(0, 0, 0, 0);
            }
            if (points != null)
            {
                int count = points.Length;
                switch (count)
                {
                    case 0:
                        SetRect(0, 0, 0, 0);
                        break;
                    case 1:
                        SetRect(points[0].X, points[0].Y, 0, 0);
                        break;
                    case 2:
                        {
                            double x1 = Math.Min(points[0].X, points[1].X);
                            double x2 = Math.Max(points[0].X, points[1].X);
                            double y1 = Math.Min(points[0].Y, points[1].Y);
                            double y2 = Math.Max(points[0].Y, points[1].Y);
                            SetRect(x1, y1, x2 - x1, y2 - y1);
                        }
                        break;
                    default:
                        {
                            double x1 = Math.Min(points[0].X, points[1].X);
                            double x2 = Math.Max(points[0].X, points[1].X);
                            double y1 = Math.Min(points[0].Y, points[1].Y);
                            double y2 = Math.Max(points[0].Y, points[1].Y);
                            SetRect(x1, y1, x2 - x1, y2 - y1);
                        }
                        for (int i = 2; i < count; i++)
                        {
                            Add(points[i].X, points[i].Y);
                        }
                        break;
                }
            }
            MinX = X;
            MinY = Y;
            MaxX = X + Width;
            MaxY = Y + Height;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Constructs a new GeoBounds, initialized to match 
        /// the values of the specified GeoBounds.
        /// </summary>
        /// <param name="r">the GeoBounds from which to copy initial values
        ///           to a newly constructed GeoBounds</param>
        public GeoBounds(GeoBounds r)
            : this(r.X, r.Y, r.Width, r.Height)
        {

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a new GeoBounds whose upper-left corner is 
        /// specified as
        /// (x,y) and whose width and height 
        /// are specified by the arguments of the same name. 
        /// </summary>
        /// <param name="x">the specified X coordinate.</param>
        /// <param name="y">the specified Y coordinate</param>
        /// <param name="width">the width of the GeoBounds</param>
        /// <param name="height">the height of the GeoBounds</param>
        public GeoBounds(double x, double y, double width, double height)
        {
            SetRect(x, y, width, height);
            MinX = X;
            MinY = Y;
            MaxX = X + Width;
            MaxY = Y + Height;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a new GeoBounds whose upper-left corner 
        /// is at (0,0) in the coordinate space, and whose width and 
        /// height are specified by the arguments of the same name. 
        /// </summary>
        /// <param name="width">the width of the GeoBounds</param>
        /// <param name="height">the height of the GeoBounds</param>
        public GeoBounds(double width, double height)
            : this(0, 0, width, height)
        {

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a new GeoBounds whose upper-left corner is 
        /// specified by theGeoPoint argument, and
        /// whose width and height are specified by the 
        /// GeoSize argument. 
        /// </summary>
        /// <param name="p">p a GeoPoint that is the upper-left corner of 
        /// the GeoBounds</param>
        /// <param name="size"> a GeoSize, representing the 
        /// width and height of the GeoBounds</param>
        public GeoBounds(GeoPoint p, GeoSize size)
            : this(p.X, p.Y, size.Width, size.Height)
        {

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a new GeoBounds whose upper-left corner is the  
        /// specified GeoPoint, and whose width and height are both zero. 
        /// </summary>
        /// <param name="p"> a GeoPoint that is the top left corner 
        /// of the GeoBounds</param>
        public GeoBounds(GeoPoint p)
            : this(p.X, p.Y, 0, 0)
        {

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a new GeoBounds whose top left corner is  
        /// (0,0) and whose width and height are specified  
        /// by the GeoSize argument. 
        /// </summary>
        /// <param name="size">a GeoSize, specifying width and height</param>
        public GeoBounds(GeoSize size)
            : this(0, 0, size.Width, size.Height)
        {

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the pixel coordinates of the center of the rectangular area.
        /// </summary>
        /// <returns>the center point of the GeoBounds.</returns>
        public GeoPoint Mid()
        {
            GeoPoint point = new GeoPoint((MinX + MaxX) / 2, (MinY + MaxY) / 2);
            return point;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Returns the pixel coordinates of the upper left corner of the rectangular
        /// area.
        /// </summary>
        /// <returns>the  upper left corner of the rectangular area.</returns>
        public GeoPoint Min()
        {
            GeoPoint point = new GeoPoint(MinX, MinY);
            return point;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Returns the pixel coordinates of the lower right corner of the 
        /// rectangular area.
        /// </summary>
        /// <returns>upper lower right of the rectangular area</returns>
        public GeoPoint Max()
        {
            GeoPoint point = new GeoPoint(MaxX, MaxY);
            return point;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tests if the specified line segment intersects the interior of this
        ///  GeoBounds.
        /// </summary>
        /// <param name="x1">the X coordinate of the start point of the specified
        ///            line segment</param>
        /// <param name="y1">the Y coordinate of the start point of the specified
        ///            line segment</param>
        /// <param name="x2">the X coordinate of the end point of the specified
        ///            line segment</param>
        /// <param name="y2">the Y coordinate of the end point of the specified
        ///            line segment.</param>
        /// <returns> true if the specified line segment intersects
        /// the interior of this GeoBounds; false
        /// otherwise.</returns>
        public bool IntersectsLine(double x1, double y1, double x2, double y2)
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
                    double tempX = X;
                    if ((out1 & OutRight) != 0)
                    {
                        tempX += Width;
                    }
                    y1 = y1 + (tempX - x1) * (y2 - y1) / (x2 - x1);
                    x1 = tempX;
                }
                else
                {
                    double tempY = Y;
                    if ((out1 & OutBottom) != 0)
                    {
                        tempY += Height;
                    }
                    x1 = x1 + (tempY - y1) * (x2 - x1) / (y2 - y1);
                    y1 = tempY;
                }
            }
            return true;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determines where the specified GeoPoint lies with
        /// respect to this GeoBounds.
        /// This method computes a binary OR of the appropriate mask values
        /// indicating, for each side of this GeoBounds,
        /// whether or not the specified GeoPoint is on the same
        /// side of the edge as the rest of this GeoBounds.
        /// </summary>
        /// <param name="p">the specified GeoPoint</param>
        /// <returns>the logical OR of all appropriate out codes.</returns>
        public int Outcode(GeoPoint p)
        {
            return Outcode(p.X, p.Y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the location and size of the outer bounds of this
        /// GeoBounds to the specified rectangular values.*
        /// </summary>
        /// <param name="x">the X coordinate of the upper-left corner
        ///           of this GeoBounds</param>
        /// <param name="y">the Y coordinate of the upper-left corner
        ///           of this GeoBounds</param>
        /// <param name="w">the width of this GeoBounds</param>
        /// <param name="h">the height of this GeoBounds</param>
        public void SetFrame(double x, double y, double w, double h)
        {
            SetRect(x, y, w, h);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// check if this rectangle contains given point.
        /// </summary>
        /// <param name="x">the x coordinate of the given point.</param>
        /// <param name="y">the y coordinate of the given point.</param>
        /// <returns>
        /// 	true if this rectangle contains given point.
        /// </returns>
        public bool Contains(double x, double y)
        {
            double x0 =X;
            double y0 = Y;
            return (x >= x0 &&
                    y >= y0 &&
                    x < x0 + Width &&
                    y < y0 + Height);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// check if this rectangle intersects with given rectangle.
        /// </summary>
        /// <param name="x">the x coordinate of the other rectangle.</param>
        /// <param name="y">the y coordinate of the other rectangle.</param>
        /// <param name="w">the width of the other rectangle.</param>
        /// <param name="h">the height of the other rectangle.</param>
        /// <returns>true, if they intersect.</returns>
        public bool Intersects(double x, double y, double w, double h)
        {
            if (IsEmpty() || w <= 0 || h <= 0)
            {
                return false;
            }
            double x0 = X;
            double y0 = Y;
            return (x + w > x0 &&
                    y + h > y0 &&
                    x < x0 + Width &&
                    y < y0 + Height);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// check if this rectangle contains given rectangle.
        /// </summary>
        /// <param name="x">the x coordinate of the other rectangle.</param>
        /// <param name="y">the y coordinate of the other rectangle.</param>
        /// <param name="w">the width of the other rectangle.</param>
        /// <param name="h">the height of the other rectangle.</param>
        /// <returns>
        /// 	true, if it totally contains other rectangle.
        /// </returns>
        public bool Contains(double x, double y, double w, double h)
        {
            if (IsEmpty() || w <= 0 || h <= 0)
            {
                return false;
            }
            double x0 = X;
            double y0 = Y;
            return (x >= x0 &&
                    y >= y0 &&
                    (x + w) <= x0 + Width &&
                    (y + h) <= y0 + Height);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Intersects the pair of specified source GeoBounds
        /// objects and puts the result into the specified destination
        /// GeoBounds object.  One of the source rectangles
        /// can also be the destination to avoid creating a third GeoBounds
        /// object, but in this case the original points of this source
        /// rectangle will be overwritten by this method.
        /// </summary>
        /// <param name="src1"> the first of a pair of GeoBounds
        ///  objects to be intersected with each other</param>
        /// <param name="src2"> the second of a pair of GeoBounds
        /// objects to be intersected with each other</param>
        /// <param name="dest">dest the GeoBounds that holds the
        /// results of the intersection of src1 and
        /// src2</param>
        public static void Intersect(GeoBounds src1,
                GeoBounds src2,
                GeoBounds dest)
        {
            double x1 = Math.Max(src1.GetMinX(), src2.GetMinX());
            double y1 = Math.Max(src1.GetMinY(), src2.GetMinY());
            double x2 = Math.Min(src1.GetMaxX(), src2.GetMaxX());
            double y2 = Math.Min(src1.GetMaxY(), src2.GetMaxY());
            dest.SetFrame(x1, y1, x2 - x1, y2 - y1);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Unions the pair of source GeoBounds objects
        /// and puts the result into the specified destination
        /// GeoBounds object.  One of the source rectangles
        /// can also be the destination to avoid creating a third GeoBounds
        /// object, but in this case the original points of this source
        /// rectangle will be overwritten by this method.
        /// </summary>
        /// <param name="src1">the first of a pair of GeoBounds
        /// objects to be combined with each other</param>
        /// <param name="src2">the second of a pair of GeoBounds
        /// objects to be combined with each other</param>
        /// <param name="dest">dest the GeoBounds that holds the
        /// results of the union of src1 and
        /// src2</param>
        public static void Union(GeoBounds src1,
                GeoBounds src2,
                GeoBounds dest)
        {
            double x1 = Math.Min(src1.GetMinX(), src2.GetMinX());
            double y1 = Math.Min(src1.GetMinY(), src2.GetMinY());
            double x2 = Math.Max(src1.GetMaxX(), src2.GetMaxX());
            double y2 = Math.Max(src1.GetMaxY(), src2.GetMaxY());
            dest.SetFrameFromDiagonal(x1, y1, x2, y2);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the smallest X coordinate of the framing
        /// rectangle of the IShape in double
        /// precision.
        /// </summary>
        /// <returns>the smallest X coordinate of the framing
        /// 		rectangle</returns>
        public double GetMinX()
        {
            return X;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the smallest Y coordinate of the framing
        /// rectangle of the IShape in double
        /// precision.
        /// </summary>
        /// <returns>the smallest Y coordinate of the framing
        /// 	rectangle</returns>
        public double GetMinY()
        {
            return Y;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the largest X coordinate of the framing
        /// rectangle of the IShape in double
        /// precision.
        /// </summary>
        /// <returns>the largest X coordinate of the framing
        ///  		rectangle</returns>
        public double GetMaxX()
        {
            return X + Width;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the largest Y coordinate of the framing
        /// rectangle of the IShape in double
        /// precision.
        /// </summary>
        /// <returns>the largest Y coordinate of the framing
        /// 		rectangle</returns>
        public double GetMaxY()
        {
            return Y + Height;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the X coordinate of the center of the framing
        /// rectangle of the IShape in double
        /// precision.
        /// </summary>
        /// <returns>the X coordinate of the center of the framing rectangle</returns>
        public double GetCenterX()
        {
            return X + Width / 2.0;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the Y coordinate of the center of the framing
        /// rectangle of the IShape in double
        /// precision.
        /// </summary>
        /// <returns>the Y coordinate of the center of the framing rectangle</returns>
        public double GetCenterY()
        {
            return Y + Height / 2.0;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the framing GeoBounds
        /// that defines the overall shape of this object.
        /// </summary>
        /// <returns> a GeoBounds</returns>
        public GeoBounds GetFrame()
        {
            return new GeoBounds(X,Y, Width, Height);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the location and size of the framing rectangle of this
        /// IShape to the specified GeoPoint and
        /// GeoSize, respectively.  The framing rectangle is used
        /// by the subclasses of RectangularShape to define
        /// their geometry.
        /// </summary>
        /// <param name="loc">the specified GeoPoint.</param>
        /// <param name="size">the specified GeoSize</param>
        public void SetFrame(GeoPoint loc, GeoSize size)
        {
            SetFrame(loc.X, loc.Y, size.Width, size.Height);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the framing rectangle of this IShape to
        /// be the specified GeoBounds.  The framing rectangle is
        /// used by the subclasses of RectangularShape to define
        /// their geometry.
        /// </summary>
        /// <param name="r">the specified GeoBounds</param>
        public void SetFrame(GeoBounds r)
        {
            SetFrame(r.X, r.Y, r.Width, r.Height);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the diagonal of the framing rectangle of this IShape
        /// based on the two specified coordinates.  The framing rectangle is
        /// used by the subclasses of RectangularShape to define
        /// their geometry.
        /// </summary>
        /// <param name="x1">the X coordinate of the start point of the specified diagonal</param>
        /// <param name="y1">the Y coordinate of the start point of the specified diagonal</param>
        /// <param name="x2">the X coordinate of the end point of the specified diagonal</param>
        /// <param name="y2">the Y coordinate of the end point of the specified diagonal</param>
        public void SetFrameFromDiagonal(double x1, double y1,
                double x2, double y2)
        {
            if (x2 < x1)
            {
                double t = x1;
                x1 = x2;
                x2 = t;
            }
            if (y2 < y1)
            {
                double t = y1;
                y1 = y2;
                y2 = t;
            }
            SetFrame(x1, y1, x2 - x1, y2 - y1);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the diagonal of the framing rectangle of this IShape
        /// based on two specified GeoPoint objects.  The framing
        /// rectangle is used by the subclasses of RectangularShape
        /// to define their geometry.
        /// </summary>
        /// <param name="p1">the start GeoPoint of the specified diagonal</param>
        /// <param name="p2">the end GeoPoint of the specified diagonal</param>
        public void SetFrameFromDiagonal(GeoPoint p1, GeoPoint p2)
        {
            SetFrameFromDiagonal(p1.X, p1.Y, p2.X, p2.Y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the framing rectangle of this IShape
        /// based on the specified center point coordinates and corner point
        /// coordinates.  The framing rectangle is used by the subclasses of
        /// RectangularShape to define their geometry.
        /// </summary>
        /// <param name="centerX">the X coordinate of the specified center point</param>
        /// <param name="centerY">the Y coordinate of the specified center point</param>
        /// <param name="cornerX">the X coordinate of the specified corner point</param>
        /// <param name="cornerY">the Y coordinate of the specified corner point</param>
        public void SetFrameFromCenter(double centerX, double centerY,
                double cornerX, double cornerY)
        {
            double halfW = Math.Abs(cornerX - centerX);
            double halfH = Math.Abs(cornerY - centerY);
            SetFrame(centerX - halfW, centerY - halfH, halfW * 2.0, halfH * 2.0);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the framing rectangle of this IShape based on a
        /// specified center GeoPoint and corner
        /// GeoPoint.  The framing rectangle is used by the subclasses
        /// of RectangularShape to define their geometry.
        /// </summary>
        /// <param name="center"> the specified center GeoPoint</param>
        /// <param name="corner">the specified corner GeoPoint</param>
        public void SetFrameFromCenter(GeoPoint center, GeoPoint corner)
        {
            SetFrameFromCenter(center.X, center.Y,
                    corner.X, corner.Y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// check if this rectangle contains given point.
        /// </summary>
        /// <param name="p">the point to be checked.</param>
        /// <returns>
        /// 	true,it contains given point.
        /// </returns>
        public bool Contains(GeoPoint p)
        {
            return Contains(p.X, p.Y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// check if this rectangle intersects given rectangle.
        /// </summary>
        /// <param name="r">the rectangle to be checked.</param>
        /// <returns>true, it intersects given rectangle.</returns>
        public bool Intersects(GeoBounds r)
        {
            return Intersects(r.X, r.Y, r.Width, r.Height);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// check if this rectangle contains given rectangle.
        /// </summary>
        /// <param name="r">the rectangle to be checked.</param>
        /// <returns>
        /// 	true, it totally contains given rectangle.
        /// </returns>
        public bool Contains(GeoBounds r)
        {
            return Contains(r.X, r.Y, r.Width, r.Height);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// return a new geo bounds of this rectangle.
        /// </summary>
        /// <returns>a new copy of this geo bound</returns>
        public GeoBounds GetBounds()
        {
            double tempWidth = Width;
            double tempHeight = Height;
            if (tempWidth < 0 || tempHeight < 0)
            {
                return new GeoBounds();
            }
            double tempX = X;
            double tempY = Y;
            double x1 = Math.Floor(tempX);
            double y1 = Math.Floor(tempY);
            double x2 = Math.Ceiling(tempX + tempWidth);
            double y2 = Math.Ceiling(tempY + tempHeight);
            return new GeoBounds((int)x1, (int)y1,
                    (int)(x2 - x1), (int)(y2 - y1));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Adds a point, specified by the double precision arguments
        /// newx and newy, to this
        /// GeoBounds.  The resulting GeoBounds
        /// is the smallest GeoBounds that
        /// contains both the original GeoBounds and the
        /// specified point.
        /// 
        /// After adding a point, a call to contains with the
        /// added point as an argument does not necessarily return
        /// true. The contains method does not
        /// return true for points on the right or bottom
        /// edges of a rectangle. Therefore, if the added point falls on
        /// the left or bottom edge of the enlarged rectangle,
        /// contains returns false for that point.
        /// </summary>
        /// <param name="newx">the X coordinate of the new point.</param>
        /// <param name="newy">the Y coordinate of the new point</param>
        public void Add(double newx, double newy)
        {
            double x1 = Math.Min(GetMinX(), newx);
            double x2 = Math.Max(GetMaxX(), newx);
            double y1 = Math.Min(GetMinY(), newy);
            double y2 = Math.Max(GetMaxY(), newy);
            SetRect(x1, y1, x2 - x1, y2 - y1);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Adds the GeoPoint object pt to this
        /// GeoBounds.
        /// The resulting GeoBounds is the smallest
        /// GeoBounds that contains both the original
        /// GeoBounds and the specified GeoPoint.
        /// 
        /// After adding a point, a call to contains with the
        /// added point as an argument does not necessarily return
        /// true. The contains
        /// method does not return true for points on the right
        /// or bottom edges of a rectangle. Therefore, if the added point falls
        /// on the left or bottom edge of the enlarged rectangle,
        /// contains returns false for that point.
        /// </summary>
        /// <param name="pt">the new GeoPoint to add to this
        ///  GeoBounds.</param>
        public void Add(GeoPoint pt)
        {
            Add(pt.X, pt.Y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Adds a GeoBounds object to this
        /// GeoBounds.  The resulting GeoBounds
        /// is the union of the two GeoBounds objects.
        /// </summary>
        /// <param name="r">the GeoBounds to add to this
        /// GeoBounds.</param>
        public void Add(GeoBounds r)
        {
            double x1 = Math.Min(GetMinX(), r.GetMinX());
            double x2 = Math.Max(GetMaxX(), r.GetMaxX());
            double y1 = Math.Min(GetMinY(), r.GetMinY());
            double y2 = Math.Max(GetMaxY(), r.GetMaxY());
            SetRect(x1, y1, x2 - x1, y2 - y1);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms
        ///  and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            long bits = Utils.DoubleToInt64Bits(X);
            bits += Utils.DoubleToInt64Bits(Y) * 37;
            bits += Utils.DoubleToInt64Bits(Width) * 43;
            bits += Utils.DoubleToInt64Bits(Height) * 47;
            return (((int)bits) ^ ((int)(bits >> 32)));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal
        ///  to this instance; otherwise, <c>false</c>.
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
            if (obj is GeoBounds)
            {
                GeoBounds r2D = (GeoBounds)obj;
                return ((X == r2D.X) &&
                        (Y == r2D.Y) &&
                        (Width == r2D.Width) &&
                        (Height == r2D.Height));
            }
            return false;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the bounding GeoSize of this GeoSize
        /// to match the specified GeoSize.
        /// 
        /// This method is included for completeness, to parallel the
        /// setBounds method of Component.
        /// </summary>
        /// <param name="r">the specified GeoSize</param>
        public void SetBounds(GeoBounds r)
        {
            SetBounds(r.X, r.Y, r.Width, r.Height);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the bounding GeoSize of this
        /// GeoSize to the specified
        /// x, y, width,
        /// and height.
        /// 
        /// This method is included for completeness, to parallel the
        /// setBounds method of Component.
        /// </summary>
        /// <param name="x">new X coordinate for the upper-left
        ///                     corner of this GeoSize</param>
        /// <param name="y">the new Y coordinate for the upper-left
        ///                     corner of this GeoSize</param>
        /// <param name="width">the new width for this GeoSize</param>
        /// <param name="height">the new height for this GeoSize</param>
        public void SetBounds(double x, double y, double width, double height)
        {
            Reshape(x, y, width, height);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the bounding GeoSize of this
        /// GeoSize to the specified
        /// x, y, width,
        /// and height.
        /// </summary>
        /// <param name="x">the new X coordinate for the upper-left
        ///                     corner of this GeoSize</param>
        /// <param name="y">the new Y coordinate for the upper-left
        ///                     corner of this GeoSize</param>
        /// <param name="width">the new width for this GeoSize</param>
        /// <param name="height">the new height for this GeoSize</param>
        private void Reshape(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Check whether the current rectangle entirely contains other rectangle.
        /// </summary>
        /// <param name="other"> the other rectangle.</param>
        /// <returns>
        /// 	true if the passed rectangular area is entirely contained 
        /// in this rectangular area.
        /// </returns>
        public bool ContainsBounds(GeoBounds other)
        {
            SetBounds(MinX, MinY, MaxX - MinX, MaxY - MinY);
            return Contains(other.MinX, other.MinY,
                    other.MaxX - other.MinX,
                    other.MaxY - other.MinY);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Check whether the current rectangle contains given point.
        /// </summary>
        /// <param name="point">the point object</param>
        /// <returns>
        /// 	true if the rectangular area (inclusively) contains the pixel 
        ///  coordinates.
        /// </returns>
        public bool ContainsPoint(GeoPoint point)
        {
            SetBounds(MinX, MinY, MaxX - MinX, MaxY - MinY);
            return Contains(point.X, point.Y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Enlarges this box so that the point is also contained in this box.
        /// </summary>
        /// <param name="point">the point object.</param>
        public void Extend(GeoPoint point)
        {
            SetBounds(MinX, MinY, MaxX - MinX, MaxY - MinY);
            Add(point.X, point.Y);
            MinX = X;
            MinY = Y;
            MaxX = X + Width;
            MaxY = Y + Height;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return MinX + "," + MinY + "," + MaxX + "," + MaxY;
        }
    }

}
