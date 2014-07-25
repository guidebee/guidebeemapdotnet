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
using System.Collections;

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
    /// An Area object stores and manipulates a
    /// resolution-independent description of an enclosed area of
    /// 2-dimensional space.
    /// </summary>
    /// <remarks>
    /// Area objects can be transformed and can perform
    /// various Constructive Area Geometry (CAG) operations when combined
    /// with other Area objects.
    /// The CAG operations include area
    /// add addition, subtract subtraction,
    /// intersect intersection, and exclusiveOr exclusive or.
    /// See the linked method documentation for examples of the various
    /// operations.
    /// 
    /// The Area class implements the IShape
    /// interface and provides full support for all of its hit-testing
    /// and path iteration facilities, but an Area is more
    /// specific than a generalized path in a number of ways:
    /// <ul>
    /// <li>Only closed paths and sub-paths are stored.
    ///     Area objects constructed from unclosed paths
    ///     are implicitly closed during construction as if those paths
    ///     had been filled by the Graphics2D.fill method.
    /// </li>
    /// <li>The interiors of the individual stored sub-paths are all
    ///     non-empty and non-overlapping.  Paths are decomposed during
    ///     construction into separate component non-overlapping parts,
    ///     empty pieces of the path are discarded, and then these
    ///     non-empty and non-overlapping properties are maintained
    ///     through all subsequent CAG operations.  Outlines of different
    ///     component sub-paths may touch each other, as long as they
    ///     do not cross so that their enclosed areas overlap.
    /// </li>
    /// <li>The geometry of the path describing the outline of the
    ///     Area resembles the path from which it was
    ///     constructed only in that it describes the same enclosed
    ///     2-dimensional area, but may use entirely different types
    ///     and ordering of the path segments to do so.
    /// </li>
    /// </ul>
    /// Interesting issues which are not always obvious when using
    /// the Area include:
    /// <ul>
    /// <li>Creating an Area from an unclosed (open)
    ///     IShape results in a closed outline in the
    ///     Area object.
    /// </li>
    /// <li>Creating an Area from a IShape
    ///     which encloses no area (even when "closed") produces an
    ///     empty Area.  A common example of this issue
    ///     is that producing an Area from a line will
    ///     be empty since the line encloses no area.  An empty
    ///     Area will iterate no geometry in its
    ///     IPathIterator objects.
    /// </li>
    /// <li>A self-intersecting IShape may be split into
    ///     two (or more) sub-paths each enclosing one of the
    ///     non-intersecting portions of the original path.
    /// </li>
    /// <li>An Area may take more path segments to
    ///     describe the same geometry even when the original
    ///     outline is simple and obvious.  The analysis that the
    ///     Area class must perform on the path may
    ///     not reflect the same concepts of "simple and obvious"
    ///     as a human being perceives.
    /// </li>
    /// </ul>
    /// </remarks>
    public class Area : IShape
    {
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="Area"/> class.
        /// </summary>
        public Area()
        {
            _curves = EmptyCurves;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// The Area class creates an area geometry from the specified IShapeobject. 
        /// </summary>
        /// <remarks>
        ///  The geometry is explicitly
        /// closed, if the IShape is not already closed.  The
        /// fill rule (even-odd or winding) specified by the geometry of the
        /// IShape is used to determine the resulting enclosed area.
        /// </remarks>
        /// <param name="s">the IShape from which the area is constructed</param>
        public Area(IShape s)
        {
            if (s is Area)
            {
                _curves = ((Area) s)._curves;
            }
            else
            {
                _curves = PathToCurves(s.GetPathIterator(null));
            }
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Adds the shape of the specified Area to the shape of this Area.
        /// </summary>
        /// <remarks>
        /// The resulting shape of this Area will include
        /// the union of both shapes, or all areas that were contained
        /// in either this or the specified Area.
        /// <pre>
        ///     // Example:
        ///     Area a1 = new Area([triangle 0,0 =&gt; 8,0 =&gt; 0,8]);
        ///     Area a2 = new Area([triangle 0,0 =&gt; 8,0 =&gt; 8,8]);
        ///     a1.add(a2);
        ///
        ///        a1(before)     +         a2         =     a1(after)
        ///
        ///     ################     ################     ################
        ///     ##############         ##############     ################
        ///     ############             ############     ################
        ///     ##########                 ##########     ################
        ///     ########                     ########     ################
        ///     ######                         ######     ######    ######
        ///     ####                             ####     ####        ####
        ///     ##                                 ##     ##            ##
        /// </pre>
        /// </remarks>
        /// <param name="rhs">the Area to be added to the
        ///          current shape</param>
        public void Add(Area rhs)
        {
            _curves = new AreaOp.AddOp().Calculate(_curves, rhs._curves);
            InvalidateBounds();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Subtracts the shape of the specified Area from the shape of this Area.
        /// </summary>
        /// <remarks>
        /// The resulting shape of this Area will include
        /// areas that were contained only in this Area
        /// and not in the specified Area.
        /// <pre>
        ///     // Example:
        ///     Area a1 = new Area([triangle 0,0 =&gt; 8,0 =&gt; 0,8]);
        ///     Area a2 = new Area([triangle 0,0 =&gt; 8,0 =&gt; 8,8]);
        ///     a1.subtract(a2);
        ///
        ///        a1(before)     -         a2         =     a1(after)
        ///
        ///     ################     ################
        ///     ##############         ##############     ##
        ///     ############             ############     ####
        ///     ##########                 ##########     ######
        ///     ########                     ########     ########
        ///     ######                         ######     ######
        ///     ####                             ####     ####
        ///     ##                                 ##     ##
        /// </pre>
        /// </remarks>
        /// <param name="rhs">the Area to be subtracted from the
        /// 		current shape</param>
        public void Subtract(Area rhs)
        {
            _curves = new AreaOp.SubOp().Calculate(_curves, rhs._curves);
            InvalidateBounds();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the shape of this Area to the intersection of
        /// its current shape and the shape of the specified Area.
        /// </summary>
        /// <remarks>
        /// The resulting shape of this Area will include
        /// only areas that were contained in both this Area
        /// and also in the specified Area.
        /// <pre>
        ///     // Example:
        ///     Area a1 = new Area([triangle 0,0 =&gt; 8,0 =&gt; 0,8]);
        ///     Area a2 = new Area([triangle 0,0 =&gt; 8,0 =&gt; 8,8]);
        ///     a1.intersect(a2);
        ///
        ///      a1(before)   intersect     a2         =     a1(after)
        ///
        ///     ################     ################     ################
        ///     ##############         ##############       ############
        ///     ############             ############         ########
        ///     ##########                 ##########           ####
        ///     ########                     ########
        ///     ######                         ######
        ///     ####                             ####
        ///     ##                                 ##
        /// </pre>
        /// </remarks>
        /// <param name="rhs">the Area to be intersected with this
        /// 	Area</param>
        public void Intersect(Area rhs)
        {
            _curves = new AreaOp.IntOp().Calculate(_curves, rhs._curves);
            InvalidateBounds();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the shape of this Area to be the combined area
        /// of its current shape and the shape of the specified Area,
        /// minus their intersection.
        /// </summary>
        /// <remarks>
        /// The resulting shape of this Area will include
        /// only areas that were contained in either this Area
        /// or in the specified Area, but not in both.
        /// <pre>
        ///     // Example:
        ///     Area a1 = new Area([triangle 0,0 =&gt; 8,0 =&gt; 0,8]);
        ///     Area a2 = new Area([triangle 0,0 =&gt; 8,0 =&gt; 8,8]);
        ///     a1.exclusiveOr(a2);
        ///
        ///        a1(before)    xor        a2         =     a1(after)
        ///
        ///     ################     ################
        ///     ##############         ##############     ##            ##
        ///     ############             ############     ####        ####
        ///     ##########                 ##########     ######    ######
        ///     ########                     ########     ################
        ///     ######                         ######     ######    ######
        ///     ####                             ####     ####        ####
        ///     ##                                 ##     ##            ##
        /// </pre>
        /// </remarks>
        /// <param name="rhs">the Area to be exclusive ORed with this
        /// 		Area.</param>
        public void ExclusiveOr(Area rhs)
        {
            _curves = new AreaOp.XorOp().Calculate(_curves, rhs._curves);
            InvalidateBounds();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Removes all of the geometry from this Area and
        /// restores it to an empty area.
        /// </summary>
        public void Reset()
        {
            _curves = new ArrayList();
            InvalidateBounds();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tests whether this Area object encloses any area.
        /// </summary>
        /// <returns>
        /// 	true if this Area object
        ///  represents an empty area; false otherwise.
        /// </returns>
        public bool IsEmpty()
        {
            return (_curves.Count == 0);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tests whether this Area consists entirely of
        ///  straight edged polygonal geometry.
        /// </summary>
        /// <returns>
        /// 	 true if the geometry of this
        ///  Area consists entirely of line segments;
        ///  false otherwise.
        /// </returns>
        public bool IsPolygonal()
        {
            IEnumerator enumerator = _curves.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (((Curve) enumerator.Current).GetOrder() > 1)
                {
                    return false;
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
        /// Tests whether this Area is rectangular in shape.
        /// </summary>
        /// <returns>
        /// 	true if the geometry of this
        ///  Area is rectangular in shape; false
        ///  otherwise.
        /// </returns>
        public bool IsRectangular()
        {
            int size = _curves.Count;
            if (size == 0)
            {
                return true;
            }
            if (size > 3)
            {
                return false;
            }
            var c1 = (Curve) _curves[1];
            var c2 = (Curve) _curves[2];
            if (c1.GetOrder() != 1 || c2.GetOrder() != 1)
            {
                return false;
            }
            if (c1.GetXTop() != c1.GetXBot() || c2.GetXTop() != c2.GetXBot())
            {
                return false;
            }
            if (c1.GetYTop() != c2.GetYTop() || c1.GetYBot() != c2.GetYBot())
            {
                // One might be able to prove that this is impossible...
                return false;
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
        /// Tests whether this Area is comprised of a single
        /// closed subpath.  This method returns true if the
        /// path contains 0 or 1 subpaths, or false if the path
        /// contains more than 1 subpath.  The subpaths are counted by the
        /// number of SegMoveto segments
        /// that appear in the path.
        /// </summary>
        /// <returns>
        /// 	 true if the Area is comprised
        ///  of a single basic geometry; false otherwise.
        /// </returns>
        public bool IsSingular()
        {
            if (_curves.Count < 3)
            {
                return true;
            }
            IEnumerator enumerator = _curves.GetEnumerator();
            enumerator.MoveNext(); // First Order0 "moveto"
            while (enumerator.MoveNext())
            {
                if (((Curve) enumerator.Current).GetOrder() == 0)
                {
                    return false;
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
        /// Returns an integer Rectangle that completely encloses the IShape.  
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
            get { return GetCachedBounds().Bounds; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tests whether the geometries of the two Area objects are equal.
        /// This method will return false if the argument is null.
        /// </summary>
        /// <param name="other">the Area to be compared to this
        /// 		Area</param>
        /// <returns>true if the two geometries are equal;
        /// 		false otherwise.</returns>
        public bool Equals(Area other)
        {
            // REMIND: A *much* simpler operation should be possible...
            // Should be able to do a curve-wise comparison since all Areas
            // should evaluate their curves in the same top-down order.
            if (other == this)
            {
                return true;
            }
            if (other == null)
            {
                return false;
            }
            ArrayList c = new AreaOp.XorOp().Calculate(_curves, other._curves);
            return c.Count == 0;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Transforms the geometry of this Area using the specified
        /// AffineTransform.  The geometry is transformed in place, which
        /// permanently changes the enclosed area defined by this object.
        /// </summary>
        /// <param name="t">the transformation used to transform the area</param>
        public void Transform(AffineTransform t)
        {
            if (t == null)
            {
                throw new NullReferenceException("transform must not be null");
            }
            // REMIND: A simpler operation can be performed for some types
            // of transform.
            _curves = PathToCurves(GetPathIterator(t));
            InvalidateBounds();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates a new Area object that contains the same
        /// geometry as this Area transformed by the specified
        /// AffineTransform.  This Area object
        /// is unchanged.
        /// </summary>
        /// <param name="t">the specified AffineTransform used to transform
        ///            the new Area</param>
        /// <returns>a new Area object representing the transformed
        ///            geometry.</returns>
        public Area CreateTransformedArea(AffineTransform t)
        {
            Area a = new Area(this);
            a.Transform(t);
            return a;
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
            if (!GetCachedBounds().Contains(x, y))
            {
                return false;
            }
            IEnumerator enumerator = _curves.GetEnumerator();
            int crossings = 0;
            while (enumerator.MoveNext())
            {
                var c = (Curve) enumerator.Current;
                crossings += c.CrossingsFor(x, y);
            }
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
        /// Tests if the interior of the IShape entirely contains
        /// the specified rectangular area. 
        /// </summary>
        /// <remarks>
        ///  All coordinates that lie inside
        /// the rectangular area must lie within the IShape for the
        /// entire rectanglar area to be considered contained within the
        /// IShape.
        /// The  IShape.contains() method allows a IShape
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
            if (w < 0 || h < 0)
            {
                return false;
            }
            if (!GetCachedBounds().Contains(x, y, w, h))
            {
                return false;
            }
            Crossings c = Crossings.FindCrossings(_curves, x, y, x + w, y + h);
            return (c.Covers(y, y + h));
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
        /// The  IShape.contains() method allows a IShape
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
            if (w < 0 || h < 0)
            {
                return false;
            }
            if (!GetCachedBounds().Intersects(x, y, w, h))
            {
                return false;
            }
            Crossings c = Crossings.FindCrossings(_curves, x, y, x + w, y + h);
            return (c == null || !c.IsEmpty());
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
            return new AreaIterator(_curves, at);
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
            return new FlatteningPathIterator(GetPathIterator(at), flatness);
        }

        private static readonly ArrayList EmptyCurves = new ArrayList();
        private ArrayList _curves;
        private Rectangle _cachedBounds;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Pathes to curves.
        /// </summary>
        /// <param name="pi">The pi.</param>
        /// <returns></returns>
        private static ArrayList PathToCurves(PathIterator pi)
        {
            ArrayList curves = new ArrayList();
            int windingRule = pi.WindingRule;
            // coords array is big enough for holding:
            //     coordinates returned from currentSegment (6)
            //     OR
            //         two subdivided quadratic curves (2+4+4=10)
            //         AND
            //             0-1 horizontal splitting parameters
            //             OR
            //             2 parametric equation derivative coefficients
            //     OR
            //         three subdivided cubic curves (2+6+6+6=20)
            //         AND
            //             0-2 horizontal splitting parameters
            //             OR
            //             3 parametric equation derivative coefficients
            var coords = new int[23];
            double movx = 0, movy = 0;
            double curx = 0, cury = 0;
            while (!pi.IsDone())
            {
                double newx;
                double newy;
                switch (pi.CurrentSegment(coords))
                {
                    case PathIterator.SegMoveto:
                        Curve.InsertLine(curves, curx, cury, movx, movy);
                        curx = movx = coords[0];
                        cury = movy = coords[1];
                        Curve.InsertMove(curves, movx, movy);
                        break;
                    case PathIterator.SegLineto:
                        newx = coords[0];
                        newy = coords[1];
                        Curve.InsertLine(curves, curx, cury, newx, newy);
                        curx = newx;
                        cury = newy;
                        break;
                    case PathIterator.SegQuadto:
                        {
                            newx = coords[2];
                            newy = coords[3];
                            var dblCoords = new double[coords.Length];
                            for (int i = 0; i < coords.Length; i++)
                            {
                                dblCoords[i] = coords[i];
                            }
                            Curve.InsertQuad(curves, curx, cury, dblCoords);
                            curx = newx;
                            cury = newy;
                        }
                        break;
                    case PathIterator.SegCubicto:
                        {
                            newx = coords[4];
                            newy = coords[5];
                            var dblCoords = new double[coords.Length];
                            for (int i = 0; i < coords.Length; i++)
                            {
                                dblCoords[i] = coords[i];
                            }
                            Curve.InsertCubic(curves, curx, cury, dblCoords);
                            curx = newx;
                            cury = newy;
                        }
                        break;
                    case PathIterator.SegClose:
                        Curve.InsertLine(curves, curx, cury, movx, movy);
                        curx = movx;
                        cury = movy;
                        break;
                }
                pi.Next();
            }
            Curve.InsertLine(curves, curx, cury, movx, movy);
            AreaOp op;
            if (windingRule == PathIterator.WindEvenOdd)
            {
                op = new AreaOp.EoWindOp();
            }
            else
            {
                op = new AreaOp.NzWindOp();
            }
            return op.Calculate(curves, EmptyCurves);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Invalidates the bounds.
        /// </summary>
        private void InvalidateBounds()
        {
            _cachedBounds = null;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the cached bounds.
        /// </summary>
        /// <returns></returns>
        private Rectangle GetCachedBounds()
        {
            if (_cachedBounds != null)
            {
                return _cachedBounds;
            }
            Rectangle r = new Rectangle();
            if (_curves.Count > 0)
            {
                Curve c = (Curve) _curves[0];
                // First point is always an order 0 curve (moveto)
                r.SetRect((int) (c.GetX0() + .5),
                          (int) (c.GetY0() + .5), 0, 0);
                for (int i = 1; i < _curves.Count; i++)
                {
                    ((Curve) _curves[i]).Enlarge(r);
                }
            }
            return (_cachedBounds = r);
        }
    }

    ////////////////////////////////////////////////////////////////////////////
    //--------------------------------- REVISIONS ------------------------------
    // Date       Name                 Tracking #         Description
    // ---------  -------------------  -------------      ----------------------
    // 14OCT2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Area interator.
    /// </summary>
    internal class AreaIterator : PathIterator
    {
        private readonly AffineTransform _transform;
        private readonly ArrayList _curves;
        private int _index;
        private Curve _prevcurve;
        private Curve _thiscurve;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="AreaIterator"/> class.
        /// </summary>
        /// <param name="curves">The curves.</param>
        /// <param name="at">At.</param>
        public AreaIterator(ArrayList curves, AffineTransform at)
        {
            _curves = curves;
            _transform = at;
            if (curves.Count >= 1)
            {
                _thiscurve = (Curve) curves[0];
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Returns the winding rule for determining the interior of the path.
        /// </summary>
        /// <returns>the winding rule</returns>
        public override int WindingRule
        {
            get
            {
                // REMIND: Which is better, EVEN_ODD or NON_ZERO?
                //         The paths calculated could be classified either way.
                //return WindEvenOdd;
                return WindNonZero;
            }
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
            return (_prevcurve == null && _thiscurve == null);
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
            if (_prevcurve != null)
            {
                _prevcurve = null;
            }
            else
            {
                _prevcurve = _thiscurve;
                _index++;
                if (_index < _curves.Count)
                {
                    _thiscurve = (Curve) _curves[_index];
                    if (_thiscurve.GetOrder() != 0 &&
                        _prevcurve.GetX1() == _thiscurve.GetX0() &&
                        _prevcurve.GetY1() == _thiscurve.GetY0())
                    {
                        _prevcurve = null;
                    }
                }
                else
                {
                    _thiscurve = null;
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
            int segtype;
            int numpoints;
            if (_prevcurve != null)
            {
                // Need to finish off junction between curves
                if (_thiscurve == null || _thiscurve.GetOrder() == 0)
                {
                    return SegClose;
                }
                coords[0] = (int) (_thiscurve.GetX0() + .5);
                coords[1] = (int) (_thiscurve.GetY0() + .5);
                segtype = SegLineto;
                numpoints = 1;
            }
            else if (_thiscurve == null)
            {
                throw new IndexOutOfRangeException("area iterator out of bounds");
            }
            else
            {
                double[] dblCoords = new double[coords.Length];

                segtype = _thiscurve.GetSegment(dblCoords);
                numpoints = _thiscurve.GetOrder();
                if (numpoints == 0)
                {
                    numpoints = 1;
                }
                for (int i = 0; i < coords.Length; i++)
                {
                    coords[i] = (int) (dblCoords[i] + .5);
                }
            }
            if (_transform != null)
            {
                _transform.Transform(coords, 0, coords, 0, numpoints);
            }
            return segtype;
        }
    }
}