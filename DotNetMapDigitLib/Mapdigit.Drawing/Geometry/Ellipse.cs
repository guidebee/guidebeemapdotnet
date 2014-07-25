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
    ///  The Ellipse class defines an ellipse.
    /// </summary>
    public class Ellipse : RectangularShape
    {

        /// <summary>
        /// The X coordinate of the upper-left corner of the
        /// framing rectangle of this Ellipse.
        /// </summary>
        public double X;

        /// <summary>
        /// The Y coordinate of the upper-left corner of the
        /// framing rectangle of this Ellipse.
        /// </summary>
        public double Y;

        /// <summary>
        /// The overall width of this Ellipse.
        /// </summary>
        public double Width;

        /// <summary>
        /// The overall height of the Ellipse.
        /// </summary>
        public double Height;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipse"/> class.
        /// </summary>
        public Ellipse()
        {
        }

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs and initializes an Ellipse from the
        /// specified coordinates.
        /// </summary>
        /// <param name="x">the X coordinate of the upper-left corner
        ///         of the framing rectangle.</param>
        /// <param name="y">the Y coordinate of the upper-left corner
        ///         of the framing rectangle</param>
        /// <param name="w">the width of the framing rectangle.</param>
        /// <param name="h">the height of the framing rectangle.</param>
        public Ellipse(double x, double y, double w, double h)
        {
            SetFrame((int) (x + .5),
                     (int) (y + .5), (int) (w + .5),
                     (int) (h + .5));
        }

        /////////////////////////////////////////////////////////////////////////////
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

        /////////////////////////////////////////////////////////////////////////////
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

        /////////////////////////////////////////////////////////////////////////////
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

        /////////////////////////////////////////////////////////////////////////////
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

        /////////////////////////////////////////////////////////////////////////////
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

        /////////////////////////////////////////////////////////////////////////////
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
        public override sealed void SetFrame(int x, int y, int w, int h)
        {
            X = x;
            Y = y;
            Width = w;
            Height = h;
        }

        /////////////////////////////////////////////////////////////////////////////
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
            return (normx*normx + normy*normy) < 0.25;
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
        public override bool Intersects(int x, int y, int w, int h)
        {
            if (w <= 0.0 || h <= 0.0)
            {
                return false;
            }
            // Normalize the rectangular coordinates compared to the ellipse
            // having a center at 0,0 and a radius of 0.5.
            double ellw = IntWidth;
            if (ellw <= 0.0)
            {
                return false;
            }
            double normx0 = (x - IntX)/ellw - 0.5;
            double normx1 = normx0 + w/ellw;
            double ellh = IntHeight;
            if (ellh <= 0.0)
            {
                return false;
            }
            double normy0 = (y - IntY)/ellh - 0.5;
            double normy1 = normy0 + h/ellh;
            // find nearest x (left edge, right edge, 0.0)
            // find nearest y (top edge, bottom edge, 0.0)
            // if nearest x,y is inside circle of radius 0.5, then intersects
            double nearx, neary;
            if (normx0 > 0.0)
            {
                // center to left of X extents
                nearx = normx0;
            }
            else if (normx1 < 0.0)
            {
                // center to right of X extents
                nearx = normx1;
            }
            else
            {
                nearx = 0.0;
            }
            if (normy0 > 0.0)
            {
                // center above Y extents
                neary = normy0;
            }
            else if (normy1 < 0.0)
            {
                // center below Y extents
                neary = normy1;
            }
            else
            {
                neary = 0.0;
            }
            return (nearx*nearx + neary*neary) < 0.25;
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
        public override bool Contains(int x, int y, int w, int h)
        {
            return (Contains(x, y) &&
                    Contains(x + w, y) &&
                    Contains(x, y + h) &&
                    Contains(x + w, y + h));
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
            return new EllipseIterator(this, at);
        }

        /////////////////////////////////////////////////////////////////////////////
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
            long bits = Util.Utils.DoubleToInt64Bits(IntX);
            bits += Util.Utils.DoubleToInt64Bits(IntY)*37;
            bits += Util.Utils.DoubleToInt64Bits(IntWidth)*43;
            bits += Util.Utils.DoubleToInt64Bits(IntHeight)*47;
            return (((int) bits) ^ ((int) (bits >> 32)));
        }

        /////////////////////////////////////////////////////////////////////////////
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
        ///  to this instance; otherwise, false.
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
            if (obj is Ellipse)
            {
                Ellipse e2D = (Ellipse) obj;
                return ((IntX == e2D.IntX) &&
                        (IntY == e2D.IntY) &&
                        (IntWidth == e2D.IntWidth) &&
                        (IntHeight == e2D.IntHeight));
            }
            return false;
        }
    }
}