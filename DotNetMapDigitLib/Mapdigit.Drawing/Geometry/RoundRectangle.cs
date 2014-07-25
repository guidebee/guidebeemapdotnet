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
    /// The RoundRectangle class defines a rectangle with rounded
    /// corners all specified in long coordinates.
    /// </summary>
    public class RoundRectangle : RectangularShape
    {
        /// <summary>
        /// The X coordinate of this RoundRectangle.
        /// </summary>
        public double X;

        /// <summary>
        /// The Y coordinate of this RoundRectangle.
        /// </summary>
        public double Y;

        /// <summary>
        /// The width of this RoundRectangle.
        /// </summary>
        public double Width;

        /// <summary>
        /// The height of this RoundRectangle.
        /// </summary>
        public double Height;


        /// <summary>
        /// The width of the arc that rounds off the corners.
        /// </summary>
        public double ArcWidth;

        /// <summary>
        /// The height of the arc that rounds off the corners.
        /// </summary>
        public double ArcHeight;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="RoundRectangle"/> class.
        /// </summary>
        public RoundRectangle()
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs and initializes a RoundRectangle
        /// from the specified double coordinates.
        /// </summary>
        /// <param name="x">the X coordinate of the newly
        ///           constructed RoundRectangle</param>
        /// <param name="y">the Y coordinate of the newly
        ///           constructed RoundRectangle.</param>
        /// <param name="w">the width to which to set the newly
        ///           constructed RoundRectangle.</param>
        /// <param name="h">the height to which to set the newly
        ///           constructed RoundRectangle.</param>
        /// <param name="arcw">the width of the arc to use to round off the
        ///             corners of the newly constructed RoundRectangle</param>
        /// <param name="arch">the height of the arc to use to round off the
        ///             corners of the newly constructed
        ///             RoundRectangle</param>
        public RoundRectangle(double x, double y, double w, double h,
                              double arcw, double arch)
        {
            SetRoundRect(x, y, w, h, arcw, arch);
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
        /// Determines whether the RectangularShape is empty.
        /// When the RectangularShape is empty, it encloses no
        /// area.
        /// </summary>
        /// <returns>
        /// 	true if this instance is empty; otherwise, false.
        /// </returns>
        public override bool IsEmpty()
        {
            return (Width <= 0.0f) || (Height <= 0.0f);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the location, size, and corner radii of this
        /// RoundRectangle to the specified
        /// double values.
        /// </summary>
        /// <param name="x">X coordinate to which to set the
        ///          location of this RoundRectangle</param>
        /// <param name="y">the Y coordinate to which to set the
        ///           location of this RoundRectangle</param>
        /// <param name="w">the width to which to set this
        ///          RoundRectangle</param>
        /// <param name="h">the height to which to set this
        ///           RoundRectangle.</param>
        /// <param name="arcw">the width to which to set the arc of this
        ///                  RoundRectangle.</param>
        /// <param name="arch">the height to which to set the arc of this
        ///                   RoundRectangle</param>
        public void SetRoundRect(double x, double y, double w, double h,
                                 double arcw, double arch)
        {
            X = x;
            Y = y;
            Width = w;
            Height = h;
            ArcWidth = arcw;
            ArcHeight = arch;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets this RoundRectangle to be the same as the
        /// specified RoundRectangle.
        /// </summary>
        /// <param name="rr">he specified RoundRectangle.</param>
        public void SetRoundRect(RoundRectangle rr)
        {
            X = rr.IntX;
            Y = rr.IntY;
            Width = rr.IntWidth;
            Height = rr.IntHeight;
            ArcWidth = rr.ArcWidth;
            ArcHeight = rr.ArcHeight;
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
            get
            {
                return new Rectangle(
                    (int) (X + .5),
                    (int) (Y + .5),
                    (int) (Width + .5),
                    (int) (Height + .5));
            }
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
            SetRoundRect(x, y, w, h, ArcWidth, ArcHeight);
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
            if (IsEmpty())
            {
                return false;
            }
            double rrx0 = IntX;
            double rry0 = IntY;
            double rrx1 = rrx0 + IntWidth;
            double rry1 = rry0 + IntHeight;
            // Check for trivial rejection - point is outside bounding rectangle
            if (x < rrx0 || y < rry0 || x >= rrx1 || y >= rry1)
            {
                return false;
            }
            double aw = Math.Min(IntWidth, Math.Abs(ArcWidth)) / 2.0;
            double ah = Math.Min(IntHeight, Math.Abs(ArcHeight)) / 2.0;
            // Check which corner point is in and do circular containment
            // test - otherwise simple acceptance
            if (x >= (rrx0 += aw) && x < (rrx0 = rrx1 - aw))
            {
                return true;
            }
            if (y >= (rry0 += ah) && y < (rry0 = rry1 - ah))
            {
                return true;
            }
            double xp = (x - rrx0)/aw;
            double yp = (y - rry0)/ah;
            return (xp*xp + yp*yp <= 1.0);
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
        public override bool Intersects(int x, int y, int w, int h)
        {
            if (IsEmpty() || w <= 0 || h <= 0)
            {
                return false;
            }
            double rrx0 = IntX;
            double rry0 = IntY;
            double rrx1 = rrx0 + IntWidth;
            double rry1 = rry0 + IntHeight;
            // Check for trivial rejection - bounding rectangles do not intersect
            if (x + w <= rrx0 || x >= rrx1 || y + h <= rry0 || y >= rry1)
            {
                return false;
            }
            double aw = Math.Min(IntWidth, Math.Abs(ArcWidth)) / 2.0;
            double ah = Math.Min(IntHeight, Math.Abs(ArcHeight)) / 2.0;
            int x0Class = Classify(x, rrx0, rrx1, aw);
            int x1Class = Classify(x + w, rrx0, rrx1, aw);
            int y0Class = Classify(y, rry0, rry1, ah);
            int y1Class = Classify(y + h, rry0, rry1, ah);
            // Trivially accept if any point is inside inner rectangle
            if (x0Class == 2 || x1Class == 2 || y0Class == 2 || y1Class == 2)
            {
                return true;
            }
            // Trivially accept if either edge spans inner rectangle
            if ((x0Class < 2 && x1Class > 2) || (y0Class < 2 && y1Class > 2))
            {
                return true;
            }
            // Since neither edge spans the center, then one of the corners
            // must be in one of the rounded edges.  We detect this case if
            // a [xy]0class is 3 or a [xy]1class is 1.  One of those two cases
            // must be true for each direction.
            // We now find a "nearest point" to test for being inside a rounded
            // corner.
            double xp = x;
            double yp = y;
            xp = (x1Class == 1) ? (xp + w - (rrx0 + aw)) : (xp - (rrx1 - aw));
            yp = (y1Class == 1) ? (yp + h - (rry0 + ah)) : (yp - (rry1 - ah));
            xp = xp/aw;
            yp = yp/ah;
            return (xp*xp + yp*yp <= 1.0);
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
        public override bool Contains(int x, int y, int w, int h)
        {
            if (IsEmpty() || w <= 0 || h <= 0)
            {
                return false;
            }
            return (Contains(x, y) &&
                    Contains(x + w, y) &&
                    Contains(x, y + h) &&
                    Contains(x + w, y + h));
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
        public override PathIterator GetPathIterator(AffineTransform at)
        {
            return new RoundRectIterator(this, at);
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
            bits += Util.Utils.DoubleToInt64Bits(ArcWidth) * 53;
            bits += Util.Utils.DoubleToInt64Bits(ArcHeight) * 59;
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
        /// 	true if the specified <see cref="System.Object"/> is equal 
        /// to this instance; otherwise, false.
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
            if (obj is RoundRectangle)
            {
                RoundRectangle rr2D = (RoundRectangle) obj;
                return ((IntX == rr2D.IntX) &&
                        (IntY == rr2D.IntY) &&
                        (IntWidth == rr2D.IntWidth) &&
                        (IntHeight == rr2D.IntHeight) &&
                        (ArcWidth == rr2D.ArcWidth) &&
                        (ArcHeight == rr2D.ArcHeight));
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
        /// Classifies the specified coord.
        /// </summary>
        /// <param name="coord">The coord.</param>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <param name="arcsize">The arcsize.</param>
        /// <returns></returns>
        private static int Classify(double coord, double left, double right,
                                    double arcsize)
        {
            if (coord < left)
            {
                return 0;
            }
            if (coord < left + arcsize)
            {
                return 1;
            }
            if (coord < right - arcsize)
            {
                return 2;
            }
            if (coord < right)
            {
                return 3;
            }
            return 4;
        }
    }
}