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
    /// The IShape interface provides definitions for objects
    /// that represent some form of geometric shape. 
    /// </summary>
    /// <remarks>
    ///  The IShape is described by a IPathIterator object, which can 
    /// express the outline of the IShape as well as a rule for determining
    /// how the outline divides the 2D plane into interior and exterior
    /// points.  Each IShape object provides callbacks to get the
    /// bounding box of the geometry, determine whether points or
    /// rectangles lie partly or entirely within the interior
    /// of the IShape, and retrieve a IPathIterator
    /// object that describes the trajectory path of the IShape
    /// outline.
    /// 
    /// <b>Definition of insideness:</b>
    /// A point is considered to lie inside a
    /// IShape if and only if:
    /// <ul>
    /// <li> it lies completely
    /// </li>
    /// inside theIShape boundary <i>or</i>
    /// <li>
    /// it lies exactly on the IShape boundary <i>and</i> the
    /// space immediately adjacent to the
    /// point in the increasing X direction is
    /// entirely inside the boundary <i>or</i>
    /// </li>
    /// <li>
    /// it lies exactly on a horizontal boundary segment <b>and</b> the
    /// space immediately adjacent to the point in the
    /// increasing Y direction is inside the boundary.
    /// </li>
    /// </ul>
    /// The contains and intersects methods
    /// consider the interior of a IShape to be the area it
    /// encloses as if it were filled.  This means that these methods
    /// consider
    /// unclosed shapes to be implicitly closed for the purpose of
    /// determining if a shape contains or intersects a rectangle or if a
    /// shape contains a point.
    /// </remarks>
    public interface IShape
    {
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
        /// <returns>an integer Rectangle that completely encloses
        /// the IShape.</returns>
        Rectangle Bounds { get; }

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
        ///     the IShape boundary; false
        ///   otherwise.
        /// </returns>
        bool Contains(int x, int y);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Tests if a specified Point is inside the boundary
        /// of the IShape.
        /// </summary>
        /// <param name="p">the specified Point to be tested</param>
        /// <returns>
        /// 	true if the specified Point is
        ///          inside the boundary of the IShape;
        ///    false otherwise.
        /// </returns>
        bool Contains(Point p);

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
        /// <li>
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
        ///        of the specified rectangular area</param>
        /// <param name="y">the Y coordinate of the upper-left corner
        ///          of the specified rectangular area</param>
        /// <param name="w">Tthe width of the specified rectangular area.</param>
        /// <param name="h">the height of the specified rectangular area.</param>
        /// <returns>
        /// 	 true if the interior of the IShape
        ///  		entirely contains the specified rectangular area;
        ///  		false otherwise or, if the IShape
        /// 		contains the rectangular area and the
        /// 		intersects method returns true
        ///  		and the containment calculations would be too expensive to
        ///  		perform.
        /// </returns>
        bool Contains(int x, int y, int w, int h);

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
        /// <li>
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
        /// <param name="r"> The specified Rectangle</param>
        /// <returns>
        /// 	 true if the interior of the IShape
        ///          entirely contains the Rectangle;
        ///          false otherwise or, if the IShape
        ///          contains the Rectangle and the
        ///          intersects method returns true
        ///          and the containment calculations would be too expensive to
        ///          perform.
        /// </returns>
        bool Contains(Rectangle r);

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
        /// <li>
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
        ///           of the specified rectangular area</param>
        /// <param name="y">the Y coordinate of the upper-left corner
        ///           of the specified rectangular area</param>
        /// <param name="w">Tthe width of the specified rectangular area</param>
        /// <param name="h">the height of the specified rectangular area</param>
        /// <returns>
        /// true if the interior of the IShape and
        /// 		the interior of the rectangular area intersect, or are
        /// 		both highly likely to intersect and intersection calculations
        /// 		would be too expensive to perform; false
        /// </returns>
        bool Intersects(int x, int y, int w, int h);

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
        /// <li>
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
        /// true if the interior of the IShape and
        /// 		the interior of the specified Rectangle
        ///		intersect, or are both highly likely to intersect and intersection
        ///		calculations would be too expensive to perform; false
        /// 		otherwise.
        /// </returns>
        bool Intersects(Rectangle r);

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
        ///  		coordinates as they are returned in the iteration, or
        /// 		null if untransformed coordinates are desired</param>
        /// <returns>a new IPathIterator object, which independently
        /// 		traverses the geometry of the IShape.</returns>
        PathIterator GetPathIterator(AffineTransform at);

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
        /// <p />
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
        ///  		coordinates as they are returned in the iteration, or
        /// 		null if untransformed coordinates are desired</param>
        /// <param name="flatness">the maximum distance that the line segments used to
        ///           approximate the curved segments are allowed to deviate
        ///           from any point on the original curve</param>
        /// <returns>a new IPathIterator that independently traverses
        /// a flattened view of the geometry of the  IShape.</returns>
        PathIterator GetPathIterator(AffineTransform at, int flatness);
    }
}