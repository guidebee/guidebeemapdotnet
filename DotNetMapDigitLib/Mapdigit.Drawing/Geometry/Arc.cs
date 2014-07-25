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
    /// This class defines an arc .
    /// </summary>
    public class Arc : RectangularShape
    {

        ///<summary>
        /// The closure type for an open arc with no path segments
        /// connecting the two ends of the arc segment.
        ///</summary>
        public const int Open = 0;

        /// <summary>
        ///  The closure type for an arc closed by drawing a straight
        /// line segment from the start of the arc segment to the end of the
        /// arc segment.
        /// </summary>
        public const int Chord = 1;

        /// <summary>
        /// The closure type for an arc closed by drawing straight line
        /// segments from the start of the arc segment to the center
        /// of the full ellipse and from that point to the end of the arc segment.
        /// </summary>
        public const int Pie = 2;

        /// <summary>
        ///  The X coordinate of the upper-left corner of the framing
        /// rectangle of the arc.
        /// </summary>
        public double X;

        /// <summary>
        /// The Y coordinate of the upper-left corner of the framing
        /// rectangle of the arc.
        /// </summary>
        public double Y;

        /// <summary>
        ///  The overall width of the full ellipse of which this arc is
        /// a partial section (not considering the angular extents).
        /// </summary>
        public double Width;

        /// <summary>
        /// The overall height of the full ellipse of which this arc is
        /// a partial section (not considering the angular extents).
        /// </summary>
        public double Height;

        ///<summary>
        /// The starting angle of the arc in degrees.
        ///</summary>
        public double Start;

        ///<summary>
        /// The angular extent of the arc in degrees.
        ///</summary>
        public double Extent;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="Arc"/> class.
        /// </summary>
        public Arc()
            : this(Open)
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="Arc"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public Arc(int type)
        {
            ArcType = type;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a new arc, initialized to the specified location,
        /// size, angular extents, and closure type.
        /// </summary>
        /// <param name="ellipseBounds">The framing rectangle that defines the
        /// outer boundary of the full ellipse of which this arc is a
        /// partial section..</param>
        /// <param name="start">The starting angle of the arc in degrees.</param>
        /// <param name="extent">The angular extent of the arc in degrees.</param>
        /// <param name="type">The closure type for the arc.</param>
        public Arc(RectangularShape ellipseBounds,
                   double start, double extent, int type)
            : this(type)
        {
            X = ellipseBounds.IntX;
            Y = ellipseBounds.IntY;
            Width = ellipseBounds.IntWidth;
            Height = ellipseBounds.IntHeight;
            Start = start;
            Extent = extent;
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
        /// <returns>
        /// the X coordinate of the upper-left corner of
        /// the framing rectangle.
        /// </returns>
        public override int IntX
        {
            get { return (int) (X + .5); }
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
        /// <returns>
        /// the Y coordinate of the upper-left corner of
        /// the framing rectangle
        /// </returns>
        public override int IntY
        {
            get { return (int) (Y + .5); }
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
        /// <returns>the width of the framing rectangle</returns>
        public override int IntWidth
        {
            get { return (int) (Width + .5); }
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
        /// <returns>the height of the framing rectangle</returns>
        public override int IntHeight
        {
            get { return (int) (Height + .5); }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// the start angle.
        /// </summary>
        public double AngleStart
        {
            get { return Start; }
            set { Start = value; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// the extent angle.
        /// </summary>
        public double AngleExtent
        {
            get { return Extent; }
            set { Extent = value; }
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
        /// 	true if this instance is empty; otherwise, false.
        /// </returns>
        public override bool IsEmpty()
        {
            return (Width <= 0.0 || Height <= 0.0);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the location, size, angular extents, and closure type of
        /// this arc to the specified double values.
        /// </summary>
        /// <param name="x">The X coordinate of the upper-left corner of the arc</param>
        /// <param name="y">The Y coordinate of the upper-left corner of the arc.</param>
        /// <param name="w">The overall width of the full ellipse of which
        ///           this arc is a partial section.</param>
        /// <param name="h">The overall height of the full ellipse of which
        ///           this arc is a partial section.</param>
        /// <param name="angSt">The starting angle of the arc in degrees.</param>
        /// <param name="angExt">The angular extent of the arc in degrees.</param>
        /// <param name="closure">The closure type for the arc:
        ///  Open, Chord, or Pie.</param>
        public void SetArc(double x, double y, double w, double h,
                           double angSt, double angExt, int closure)
        {
            ArcType = closure;
            X = x;
            Y = y;
            Width = w;
            Height = h;
            Start = angSt;
            Extent = angExt;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a Rectangle of the appropriate precision
        /// to hold the parameters calculated to be the framing rectangle
        /// of this arc.
        /// </summary>
        /// <param name="x">The X coordinate of the upper-left corner of the
        ///  framing rectangle</param>
        /// <param name="y">The Y coordinate of the upper-left corner of the
        ///  framing rectangle.</param>
        /// <param name="w">The width of the framing rectangle.</param>
        /// <param name="h">The height of the framing rectangle.</param>
        /// <returns></returns>
        protected static Rectangle MakeBounds(double x, double y,
                                              double w, double h)
        {
            return new Rectangle((int) (x + .5),
                                 (int) (y + .5),
                                 (int) (w + .5),
                                 (int) (h + .5));
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Constructs a new arc, initialized to the specified location,
        ///  size, angular extents, and closure type.
        /// </summary>
        /// <param name="x">The X coordinate of the upper-left corner
        ///           of the arc's framing rectangle</param>
        /// <param name="y">The Y coordinate of the upper-left corner
        ///           of the arc's framing rectangle..</param>
        /// <param name="w">The overall width of the full ellipse of which this
        ///           arc is a partial section.</param>
        /// <param name="h">The overall height of the full ellipse of which this
        ///           arc is a partial section.</param>
        /// <param name="start">The starting angle of the arc in degrees.</param>
        /// <param name="extent">The angular extent of the arc in degree.</param>
        /// <param name="type">The closure type for the arc:
        /// Open, Chord, or Pie.</param>
        public Arc(double x, double y, double w, double h,
                   double start, double extent, int type)
            : this(type)
        {
            X = x;
            Y = y;
            Width = w;
            Height = h;
            Start = start;
            Extent = extent;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// the arc closure type of the arc: Open,Chord,Pie
        /// </summary>
        public int ArcType
        {
            get { return _type; }
            set
            {
                if (value < Open || value > Pie)
                {
                    throw new ArgumentException("invalid type for Arc: " + value);
                }
                _type = value;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the starting point of the arc. 
        /// </summary>
        /// <remarks>
        /// This point is the  intersection of the ray from the center defined by the
        /// starting angle and the elliptical boundary of the arc.
        /// </remarks>
        public Point StartPoint
        {
            get
            {
                double angle = MathEx.ToRadians(-AngleStart);
                double xp = IntX + (MathEx.Cos(angle)*0.5 + 0.5)*IntWidth;
                double yp = IntY + (MathEx.Sin(angle)*0.5 + 0.5)*IntHeight;
                return new Point((int) (xp + .5), (int) (yp + .5));
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the ending point of the arc.  
        /// </summary>
        /// <remarks>
        /// This point is the intersection of the ray from the center defined by the
        /// starting angle plus the angular extent of the arc and the
        /// elliptical boundary of the arc.
        /// </remarks>
        public Point EndPoint
        {
            get
            {
                double angle = MathEx.ToRadians(-AngleStart - AngleExtent);
                double xp = IntX + (MathEx.Cos(angle)*0.5 + 0.5)*IntWidth;
                double yp = IntY + (MathEx.Sin(angle)*0.5 + 0.5)*IntHeight;
                return new Point((int) (xp + .5), (int) (yp + .5));
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the location, size, angular extents, and closure type of
        /// this arc to the specified values.
        /// </summary>
        /// <param name="loc">The <CODE>Point</CODE> representing the coordinates of
        /// the upper-left corner of the arc.</param>
        /// <param name="size"> The <CODE>Dimension</CODE> representing the width
        /// and height of the full ellipse of which this arc is
        /// a partial section.</param>
        /// <param name="angSt">The starting angle of the arc in degrees.</param>
        /// <param name="angExt">The angular extent of the arc in degrees.</param>
        /// <param name="closure">The closure type for the arc:
        /// Open, Chord, or Pie.</param>
        public void SetArc(Point loc, Dimension size,
                           double angSt, double angExt, int closure)
        {
            SetArc(loc.X, loc.X, size.Width, size.Height,
                   angSt, angExt, closure);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the location, size, angular extents, and closure type of
        /// this arc to the specified values.
        /// </summary>
        /// <param name="rect">rect The framing rectangle that defines the
        /// outer boundary of the full ellipse of which this arc is a
        /// partial section.</param>
        /// <param name="angSt">The starting angle of the arc in degrees.</param>
        /// <param name="angExt">The angular extent of the arc in degrees.</param>
        /// <param name="closure">The closure type for the arc:
        /// Open, Chord, or Pie.</param>
        public void SetArc(Rectangle rect, double angSt, double angExt,
                           int closure)
        {
            SetArc(rect.IntX, rect.IntY, rect.IntWidth, rect.IntHeight,
                   angSt, angExt, closure);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets this arc to be the same as the specified arc.
        /// </summary>
        /// <param name="a">a The <CODE>Arc</CODE> to use to set the arc's values</param>
        public void SetArc(Arc a)
        {
            SetArc(a.IntX, a.IntY, a.IntWidth, a.IntHeight,
                   a.AngleStart, a.AngleExtent, a._type);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the position, bounds, angular extents, and closure type of
        /// this arc to the specified values. 
        /// </summary>
        /// <remarks>
        /// The arc is defined by a center point and a radius rather than a framing
        ///  rectangle for the full ellipse.
        /// </remarks>
        /// <param name="x">The X coordinate of the center of the arc</param>
        /// <param name="y">The Y coordinate of the center of the arc..</param>
        /// <param name="radius"> The radius of the arc</param>
        /// <param name="angSt">The starting angle of the arc in degrees</param>
        /// <param name="angExt"> The angular extent of the arc in degrees.</param>
        /// <param name="closure">The closure type for the arc:
        ///  Open, Chord, or Pie.</param>
        public void SetArcByCenter(double x, double y, double radius,
                                   double angSt, double angExt, int closure)
        {
            SetArc(x - radius, y - radius, radius*2.0, radius*2.0,
                   angSt, angExt, closure);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the position, bounds, and angular extents of this arc to the
        /// specified value. 
        /// </summary>
        /// <remarks>
        /// The starting angle of the arc is tangent to the
        /// line specified by points (p1, p2), the ending angle is tangent to
        /// the line specified by points (p2, p3), and the arc has the
        /// specified radius.
        /// </remarks>
        /// <param name="p1">The first point that defines the arc. The starting
        /// angle of the arc is tangent to the line specified by points (p1, p2).</param>
        /// <param name="p2">he second point that defines the arc. The starting
        /// angle of the arc is tangent to the line specified by points (p1, p2).
        /// The ending angle of the arc is tangent to the line specified by
        /// points (p2, p3).</param>
        /// <param name="p3"> The third point that defines the arc. The ending angle
        /// of the arc is tangent to the line specified by points (p2, p3).</param>
        /// <param name="radius">The radius of the arc.</param>
        public void SetArcByTangent(Point p1, Point p2, Point p3,
                                    double radius)
        {
            double ang1 = MathEx.Atan2(p1.Y - p2.Y,
                                       p1.X - p2.X);
            double ang2 = MathEx.Atan2(p3.Y - p2.Y,
                                       p3.X - p2.X);
            double diff = ang2 - ang1;
            if (diff > MathEx.Pi)
            {
                ang2 -= MathEx.Pi*2.0;
            }
            else if (diff < -MathEx.Pi)
            {
                ang2 += MathEx.Pi*2.0;
            }
            double bisect = (ang1 + ang2)/2.0;
            double theta = MathEx.Abs(ang2 - bisect);
            double dist = radius/MathEx.Sin(theta);
            double xp = p2.X + dist*MathEx.Cos(bisect);
            double yp = p2.Y + dist*MathEx.Sin(bisect);
            // REMIND: This needs some work...
            if (ang1 < ang2)
            {
                ang1 -= MathEx.Pi/2.0;
                ang2 += MathEx.Pi/2.0;
            }
            else
            {
                ang1 += MathEx.Pi/2.0;
                ang2 -= MathEx.Pi/2.0;
            }
            ang1 = MathEx.ToDegrees(-ang1);
            ang2 = MathEx.ToDegrees(-ang2);
            diff = ang2 - ang1;
            if (diff < 0)
            {
                diff += 360;
            }
            else
            {
                diff -= 360;
            }
            SetArcByCenter(xp, yp, radius, ang1, diff, _type);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the starting angle of this arc to the angle that the
        /// specified point defines relative to the center of this arc.
        /// The angular extent of the arc will remain the same.
        /// </summary>
        /// <param name="p">The <CODE>Point</CODE> that defines the starting angle.</param>
        public void SetAngleStart(Point p)
        {
            // Bias the dx and dy by the height and width of the oval.
            double dx = IntHeight*(p.X - IntCenterX);
            double dy = IntWidth*(p.Y - IntCenterY);
            AngleStart = -MathEx.ToDegrees(MathEx.Atan2(dy, dx));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the starting angle and angular extent of this arc using two
        /// sets of coordinates.
        /// </summary>
        /// <remarks>
        ///  The first set of coordinates is used to determine the angle of the 
        /// starting point relative to the arc's center. The second set of coordinates
        ///  is used to determine the angle of the end point relative to the arc's center.
        /// The arc will always be non-empty and extend counterclockwise
        /// from the first point around to the second point.
        /// </remarks>
        /// <param name="x1">The X coordinate of the arc's starting point</param>
        /// <param name="y1">The Y coordinate of the arc's starting point.</param>
        /// <param name="x2">The X coordinate of the arc's ending point.</param>
        /// <param name="y2">The Y coordinate of the arc's ending point.</param>
        public void SetAngles(double x1, double y1, double x2, double y2)
        {
            double xp = IntCenterX;
            double yp = IntCenterY;
            double w = IntWidth;
            double h = IntHeight;
            // remember: reversing the Y equations negates the angle to adjust
            // for the upside down coordinate system.
            // Also we should bias atans by the height and width of the oval.
            double ang1 = MathEx.Atan2(w*(yp - y1), h*(x1 - xp));
            double ang2 = MathEx.Atan2(w*(yp - y2), h*(x2 - xp));
            ang2 -= ang1;
            if (ang2 <= 0.0)
            {
                ang2 += MathEx.Pi*2.0;
            }
            AngleStart = MathEx.ToDegrees(ang1);
            AngleExtent = MathEx.ToDegrees(ang2);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the starting angle and angular extent of this arc using
        /// two points. The first point is used to determine the angle of
        /// the starting point relative to the arc's center.
        /// </summary>
        /// <remarks>
        /// The second point is used to determine the angle of the end point
        /// relative to the arc's center.
        /// The arc will always be non-empty and extend counterclockwise
        /// from the first point around to the second point.
        /// </remarks>
        /// <param name="p1">The <CODE>Point</CODE> that defines the arc's
        /// starting point.</param>
        /// <param name="p2">The <CODE>Point</CODE> that defines the arc's
        /// ending point.</param>
        public void SetAngles(Point p1, Point p2)
        {
            SetAngles(p1.X, p1.Y, p2.X, p2.Y);
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
            SetArc(x, y, w, h, AngleStart, AngleExtent, _type);
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
        public override Rectangle Bounds
        {
            get
            {
                if (IsEmpty())
                {
                    return MakeBounds(IntX, IntY, IntWidth, IntHeight);
                }
                double x1, y1, x2, y2;
                if (ArcType == Pie)
                {
                    x1 = y1 = x2 = y2 = 0.0;
                }
                else
                {
                    x1 = y1 = 1.0;
                    x2 = y2 = -1.0;
                }
                double angle = 0.0;
                for (int i = 0; i < 6; i++)
                {
                    if (i < 4)
                    {
                        // 0-3 are the four quadrants
                        angle += 90.0;
                        if (!ContainsAngle(angle))
                        {
                            continue;
                        }
                    }
                    else if (i == 4)
                    {
                        // 4 is start angle
                        angle = AngleStart;
                    }
                    else
                    {
                        // 5 is end angle
                        angle += AngleExtent;
                    }
                    double rads = MathEx.ToRadians(-angle);
                    double xe = MathEx.Cos(rads);
                    double ye = MathEx.Sin(rads);
                    x1 = MathEx.Min(x1, xe);
                    y1 = MathEx.Min(y1, ye);
                    x2 = MathEx.Max(x2, xe);
                    y2 = MathEx.Max(y2, ye);
                }
                double w = IntWidth;
                double h = IntHeight;
                x2 = (x2 - x1)*0.5*w;
                y2 = (y2 - y1)*0.5*h;
                x1 = IntX + (x1*0.5 + 0.5)*w;
                y1 = IntY + (y1*0.5 + 0.5)*h;
                return MakeBounds(x1, y1, x2, y2);
            }
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determines whether or not the specified angle is within the
        /// angular extents of the arc.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <returns>
        /// 	true, if the arc contains the angle,
        ///  false,if the arc doesn't contain the angle
        /// </returns>
        public bool ContainsAngle(double angle)
        {
            double angExt = AngleExtent;
            bool backwards = (angExt < 0.0);
            if (backwards)
            {
                angExt = -angExt;
            }
            if (angExt >= 360.0)
            {
                return true;
            }
            angle = NormalizeDegrees(angle) - NormalizeDegrees(AngleStart);
            if (backwards)
            {
                angle = -angle;
            }
            if (angle < 0.0)
            {
                angle += 360.0;
            }


            return (angle >= 0.0) && (angle < angExt);
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
        public override bool Contains(int x, int y)
        {
            // Normalize the coordinates compared to the ellipse
            // having a center at 0,0 and a radius of 0.5.
            double ellw = IntWidth;
            if (ellw <= 0.0)
            {
                return false;
            }
            double normx = (x - IntX)/ellw - 0.5;
            double ellh = IntHeight;
            if (ellh <= 0.0)
            {
                return false;
            }
            double normy = (y - IntY)/ellh - 0.5;
            double distSq = (normx*normx + normy*normy);
            if (distSq >= 0.25)
            {
                return false;
            }
            double angExt = MathEx.Abs(AngleExtent);
            if (angExt >= 360.0)
            {
                return true;
            }
            bool inarc = ContainsAngle(-MathEx.ToDegrees(MathEx.Atan2(normy,
                                                                      normx)));
            if (_type == Pie)
            {
                return inarc;
            }
            // Chord and Open behave the same way
            if (inarc)
            {
                if (angExt >= 180.0)
                {
                    return true;
                }
                // point must be outside the "pie triangle"
            }
            else
            {
                if (angExt <= 180.0)
                {
                    return false;
                }
                // point must be inside the "pie triangle"
            }
            // The point is inside the pie triangle iff it is on the same
            // side of the line connecting the ends of the arc as the center.
            double angle = MathEx.ToRadians(-AngleStart);
            double x1 = MathEx.Cos(angle);
            double y1 = MathEx.Sin(angle);
            angle += MathEx.ToRadians(-AngleExtent);
            double x2 = MathEx.Cos(angle);
            double y2 = MathEx.Sin(angle);
            bool inside = (Line.RelativeCcw((int) (x1 + .5),
                                            (int) (y1 + .5),
                                            (int) (x2 + .5),
                                            (int) (y2 + .5),
                                            (int) (2*normx + .5),
                                            (int) (2*normy + .5))*
                           Line.RelativeCcw((int) (x1 + .5),
                                            (int) (y1 + .5),
                                            (int) (x2 + .5),
                                            (int) (y2 + .5), 0, 0) >= 0);
            return inarc ? !inside : inside;
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
        /// there is a high probability that the rectangular area and the
        /// IShape intersect, but
        /// the calculations to accurately determine this intersection
        /// are prohibitively expensive.
        /// This means that for some Shapes this method might
        /// return true even though the rectangular area does not
        /// intersect the IShape.
        /// The com.mapdigit.drawing.geometry.Area Area class performs
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
        public override bool Intersects(int x, int y, int w, int h)
        {
            double aw = IntWidth;
            double ah = IntHeight;

            if (w <= 0 || h <= 0 || aw <= 0 || ah <= 0)
            {
                return false;
            }
            double ext = AngleExtent;
            if (ext == 0)
            {
                return false;
            }

            double ax = IntX;
            double ay = IntY;
            double axw = ax + aw;
            double ayh = ay + ah;
            double xw = x + w;
            double yh = y + h;

            // check bbox
            if (x >= axw || y >= ayh || xw <= ax || yh <= ay)
            {
                return false;
            }

            // extract necessary data
            double axc = IntCenterX;
            double ayc = IntCenterY;
            Point sp = StartPoint;
            Point ep = EndPoint;
            double sx = sp.X;
            double sy = sp.Y;
            double ex = ep.X;
            double ey = ep.Y;

            /*
             * Try to catch rectangles that intersect arc in areas
             * outside of rectagle with left top corner coordinates
             * (Min(center x, start point x, end point x),
             *  Min(center y, start point y, end point y))
             * and rigth bottom corner coordinates
             * (Max(center x, start point x, end point x),
             *  Max(center y, start point y, end point y)).
             * So we'll check axis segments outside of rectangle above.
             */
            if (ayc >= y && ayc <= yh)
            {
                // 0 and 180
                if ((sx < xw && ex < xw && axc < xw &&
                     axw > x && ContainsAngle(0)) ||
                    (sx > x && ex > x && axc > x &&
                     ax < xw && ContainsAngle(180)))
                {
                    return true;
                }
            }
            if (axc >= x && axc <= xw)
            {
                // 90 and 270
                if ((sy > y && ey > y && ayc > y &&
                     ay < yh && ContainsAngle(90)) ||
                    (sy < yh && ey < yh && ayc < yh &&
                     ayh > y && ContainsAngle(270)))
                {
                    return true;
                }
            }

            /*
             * For Pie we should check intersection with pie slices;
             * also we should do the same for arcs with extent is greater
             * than 180, because we should cover case of rectangle, which
             * situated between center of arc and chord, but does not
             * intersect the chord.
             */
            Rectangle rect = new Rectangle((int) (x + .5),
                                           (int) (y + .5),
                                           (int) (w + .5),
                                           (int) (h + .5));
            if (_type == Pie || MathEx.Abs(ext) > 180)
            {
                // for Pie: try to find intersections with pie slices
                if (rect.IntersectsLine((int) (axc + .5),
                                        (int) (ayc + .5),
                                        (int) (sx + .5),
                                        (int) (sy + .5)) ||
                    rect.IntersectsLine((int) (axc + .5),
                                        (int) (ayc + .5),
                                        (int) (ex + .5),
                                        (int) (ey + .5)))
                {
                    return true;
                }
            }
            else
            {
                // for Chord and Open: try to find intersections with chord
                if (rect.IntersectsLine((int) (sx + .5),
                                        (int) (sy + .5), (int) (ex + .5),
                                        (int) (ey + .5)))
                {
                    return true;
                }
            }

            // finally check the rectangle corners inside the arc
            if (Contains(x, y) || Contains(x + w, y) ||
                Contains(x, y + h) || Contains(x + w, y + h))
            {
                return true;
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
        /// Tests if the interior of the IShape entirely contains
        /// the specified rectangular area.  
        /// </summary>
        /// <remarks>
        /// All coordinates that lie inside
        /// the rectangular area must lie within the IShape for the
        /// entire rectanglar area to be considered contained within the
        /// IShape.
        /// The IShape.contains()method allows a IShape}
        /// implementation to conservatively return false when:
        /// the intersect method returns true and
        /// the calculations to determine whether or not the
        /// IShape entirely contains the rectangular area are
        /// prohibitively expensive.
        /// This means that for some Shapes this method might
        /// return false even though the IShape} contains
        /// the rectangular area.
        /// The com.mapdigit.drawing.geometry.Area Area class performs
        /// more accurate geometric computations than most
        /// IShape} objects and therefore can be used if a more precise
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
        public override bool Contains(int x, int y, int w, int h)
        {
            return Contains(x, y, w, h, null);
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
        /// The IShape.contains() method allows a IShape}
        /// implementation to conservatively return false when:
        /// the intersect method returns true and
        /// the calculations to determine whether or not the
        /// IShape entirely contains the Rectangle
        /// are prohibitively expensive.
        /// This means that for some Shapes this method might
        /// return false even though the IShape} contains
        /// the Rectangle.
        /// The com.mapdigit.drawing.geometry.Area Area class performs
        /// more accurate geometric computations than most
        /// IShape} objects and therefore can be used if a more precise
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
        public override bool Contains(Rectangle r)
        {
            return Contains(r.IntX, r.IntY, r.IntWidth, r.IntHeight, r);
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
            return new ArcIterator(this, at);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a hash c for this instance.
        /// </summary>
        /// <returns>
        /// A hash c for this instance, suitable for use in hashing algorithms 
        /// and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            long bits = Util.Utils.DoubleToInt64Bits(IntX);
            bits += Util.Utils.DoubleToInt64Bits(IntY)*37;
            bits += Util.Utils.DoubleToInt64Bits(IntWidth)*43;
            bits += Util.Utils.DoubleToInt64Bits(IntHeight)*47;
            bits += Util.Utils.DoubleToInt64Bits(AngleStart)*53;
            bits += Util.Utils.DoubleToInt64Bits(AngleExtent)*59;
            bits += ArcType*61;
            return (((int) bits) ^ ((int) (bits >> 32)));
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
        /// 	true if the specified <see cref="System.Object"/> is equal to
        ///  this instance; otherwise, false.
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
            if (obj is Arc)
            {
                Arc a2D = (Arc) obj;
                return ((IntX == a2D.IntX) &&
                        (IntY == a2D.IntY) &&
                        (IntWidth == a2D.IntWidth) &&
                        (IntHeight == a2D.IntHeight) &&
                        (AngleStart == a2D.AngleStart) &&
                        (AngleExtent == a2D.AngleExtent) &&
                        (ArcType == a2D.ArcType));
            }
            return false;
        }

        private int _type;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Normalizes the specified angle into the range -180 to 180.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <returns></returns>
        internal static double NormalizeDegrees(double angle)
        {
            if (angle > 180.0)
            {
                if (angle <= (180.0 + 360.0))
                {
                    angle = angle - 360.0;
                }
                else
                {
                    angle = MathEx.IEEERemainder(angle, 360.0);
                    // IEEERemainder can return -180 here for some input values...
                    if (angle == -180.0)
                    {
                        angle = 180.0;
                    }
                }
            }
            else if (angle <= -180.0)
            {
                if (angle > (-180.0 - 360.0))
                {
                    angle = angle + 360.0;
                }
                else
                {
                    angle = MathEx.IEEERemainder(angle, 360.0);
                    // IEEERemainder can return -180 here for some input values...
                    if (angle == -180.0)
                    {
                        angle = 180.0;
                    }
                }
            }
            return angle;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determines whether [contains] [the specified x].
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="w">The w.</param>
        /// <param name="h">The h.</param>
        /// <param name="origrect">The origrect.</param>
        /// <returns>
        /// 	true if [contains] [the specified x]; otherwise, false.
        /// </returns>
        private bool Contains(int x, int y, int w, int h,
                              Rectangle origrect)
        {
            if (!(Contains(x, y) &&
                  Contains(x + w, y) &&
                  Contains(x, y + h) &&
                  Contains(x + w, y + h)))
            {
                return false;
            }
            // If the shape is convex then we have done all the testing
            // we need.  Only Pie arcs can be concave and then only if
            // the angular extents are greater than 180 degrees.
            if (_type != Pie || MathEx.Abs(AngleExtent) <= 180.0)
            {
                return true;
            }
            // For a Pie shape we have an additional test for the case where
            // the angular extents are greater than 180 degrees and all four
            // rectangular corners are inside the shape but one of the
            // rectangle edges spans across the "missing wedge" of the arc.
            // We can test for this case by checking if the rectangle intersects
            // either of the pie angle segments.
            if (origrect == null)
            {
                origrect = new Rectangle((int) (x + .5),
                                         (int) (y + .5),
                                         (int) (w + .5),
                                         (int) (h + .5));
            }
            double halfW = IntWidth/2.0;
            double halfH = IntHeight/2.0;
            double xc = IntX + halfW;
            double yc = IntY + halfH;
            double angle = MathEx.ToRadians(-AngleStart);
            double xe = xc + halfW*MathEx.Cos(angle);
            double ye = yc + halfH*MathEx.Sin(angle);
            if (origrect.IntersectsLine((int) (xc + .5),
                                        (int) (yc + .5),
                                        (int) (xe + .5),
                                        (int) (ye + .5)))
            {
                return false;
            }
            angle += MathEx.ToRadians(-AngleExtent);
            xe = xc + halfW*MathEx.Cos(angle);
            ye = yc + halfH*MathEx.Sin(angle);
            return !origrect.IntersectsLine((int) (xc + .5),
                                            (int) (yc + .5),
                                            (int) (xe + .5),
                                            (int) (ye + .5));
        }
    }
}