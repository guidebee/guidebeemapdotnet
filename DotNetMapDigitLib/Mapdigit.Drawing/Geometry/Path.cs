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
using System.Text;
using Mapdigit.Drawing.Geometry.Parser;

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
    /// The Path class provides a simple, yet flexible
    /// shape which represents an arbitrary geometric path.
    /// </summary>
    /// <remarks>
    /// It can fully represent any path which can be iterated by the
    /// IPathIterator interface including all of its segment
    /// types and winding rules and it implements all of the
    /// basic hit testing methods of theIShape interface.
    /// 
    /// Path provides exactly those facilities required for
    /// basic construction and management of a geometric path and
    /// implementation of the above interfaces with little added
    /// interpretation.
    /// If it is useful to manipulate the interiors of closed
    /// geometric shapes beyond simple hit testing then the
    /// Area class provides additional capabilities
    /// specifically targeted at closed figures.
    /// While both classes nominally implement the IShape
    /// interface, they differ in purpose and together they provide
    /// two useful views of a geometric shape where Path
    /// deals primarily with a trajectory formed by path segments
    /// and Area deals more with interpretation and manipulation
    /// of enclosed regions of 2D geometric space.
    /// 
    /// The IPathIterator interface has more detailed descriptions
    /// of the types of segments that make up a path and the winding rules
    /// that control how to determine which regions are inside or outside
    /// the path.
    /// </remarks>
    public class Path : IShape
    {
        ///<summary>
        /// An even-odd winding rule for determining the interior of a path.
        ///</summary>
        public const int WindEvenOdd = PathIterator.WindEvenOdd;

        ///<summary>
        /// A non-zero winding rule for determining the interior of a path.
        ///</summary>
        public const int WindNonZero = PathIterator.WindNonZero;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses Path object from a path data input string that contains the moveto
        /// , line, curve (both cubic and quadratic Beziers) and closepath
        /// instructions. 
        /// </summary>
        /// <remarks>
        /// For example, "M 100 100 L 300 100 L 200 300 z". . 
        /// </remarks>
        /// <param name="input">Tpath input string.</param>
        /// <returns>path object</returns>
        public static Path FromString(string input)
        {
            lock (PathParser)
            {
                return PathParser.ParsePath(input);
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="shape"></param>
        ///<returns></returns>
        public static String ToSvg(IShape shape)
        {
            PathIterator pathIterator = shape.GetPathIterator(null);
            StringBuilder svgString = new StringBuilder("<path d='");
            int[] coords = new int[6];
            int type;
            while (!pathIterator.IsDone())
            {
                type = pathIterator.CurrentSegment(coords);
                switch (type)
                {
                    case PathIterator.SegClose:
                        svgString.Append("Z ");
                        break;
                    case PathIterator.SegCubicto:
                        svgString.Append("C ");
                        svgString.Append(coords[0] + " ");
                        svgString.Append(coords[1] + " ");
                        svgString.Append(coords[2] + " ");
                        svgString.Append(coords[3] + " ");
                        svgString.Append(coords[4] + " ");
                        svgString.Append(coords[5]);
                        break;
                    case PathIterator.SegLineto:
                        svgString.Append("L ");
                        svgString.Append(coords[0] + " ");
                        svgString.Append(coords[1]);
                        break;
                    case PathIterator.SegMoveto:
                        svgString.Append("M ");
                        svgString.Append(coords[0] + " ");
                        svgString.Append(coords[1]);
                        break;
                    case PathIterator.SegQuadto:
                        svgString.Append("Q ");
                        svgString.Append(coords[0] + " ");
                        svgString.Append(coords[1] + " ");
                        svgString.Append(coords[2] + " ");
                        svgString.Append(coords[3]);
                        break;
                }

                pathIterator.Next();

            }
            svgString.Append("' />");
            return svgString.ToString();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a new int precision Path object
        /// from an arbitraryIShape object, transformed by an
        /// AffineTransform object.
        /// </summary>
        /// <remarks>
        /// All of the initial geometry and the winding rule for this path are
        /// taken from the specified IShape object and transformed
        /// by the specified AffineTransform object.
        /// </remarks>
        /// <param name="s"> the specified IShape object</param>
        /// <param name="at">the specified AffineTransform object</param>
        public Path(IShape s, AffineTransform at)
        {
            if (s is Path)
            {
                Path p2D = (Path) s;
                WindingRule = p2D._windingRule;
                _numTypes = p2D._numTypes;
                _pointTypes = new byte[p2D._pointTypes.Length];
                Array.Copy(p2D._pointTypes, _pointTypes, _pointTypes.Length);

                _numCoords = p2D._numCoords;
                _intCoords = p2D.CloneCoords(at);
            }
            else
            {
                PathIterator pi = s.GetPathIterator(at);
                WindingRule = pi.WindingRule;
                _pointTypes = new byte[InitSize];
                _intCoords = new int[InitSize*2];
                Append(pi, false);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a new empty int precision Path object
        /// with a default winding rule of WindNonZero.
        /// </summary>
        public Path()
            : this(WindNonZero, InitSize)
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a new empty int precision Path object
        /// with the specified winding rule to control operations that
        /// require the interior of the path to be defined.
        /// </summary>
        /// <param name="rule">the winding rule</param>
        public Path(int rule)
            : this(rule, InitSize)
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a new Path object from the given
        /// specified initial values.
        /// </summary>
        /// <remarks>
        /// This method is only intended for internal use and should
        /// not be made public if the other constructors for this class
        /// are ever exposed.
        /// </remarks>
        /// <param name="rule">the winding rule</param>
        /// <param name="initialTypes">the size to make the initial array to
        ///                     store the path segment types</param>
        public Path(int rule, int initialTypes)
        {
            WindingRule = rule;
            _pointTypes = new byte[initialTypes];
            _intCoords = new int[initialTypes*2];
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a new int precision Path object
        /// from an arbitraryIShape object.
        /// </summary>
        /// <remarks>
        /// All of the initial geometry and the winding rule for this path are
        /// taken from the specified IShape object.
        /// </remarks>
        /// <param name="s">The s.</param>
        public Path(IShape s)
            : this(s, null)
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates a new object of the same class as this object.
        /// </summary>
        /// <returns>a clone of this instance.</returns>
        public object Clone()
        {
            return new Path(this);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Closes the current subpath by drawing a straight line back to
        /// the coordinates of the last moveTo. 
        /// </summary>
        /// <remarks>
        ///  If the path is already closed then this method has no effect.
        /// </remarks>
        public void ClosePath()
        {
            if (_numTypes == 0 || _pointTypes[_numTypes - 1] != SegClose)
            {
                NeedRoom(true, 0);
                _pointTypes[_numTypes++] = SegClose;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Appends the geometry of the specified IShape object to the
        /// path, possibly connecting the new geometry to the existing path
        /// segments with a line segment.
        /// </summary>
        /// <remarks>
        /// If the {connectparameter is true and the
        /// path is not empty then any initial moveTo in the
        /// geometry of the appended IShape
        /// is turned into a lineTo segment.
        /// If the destination coordinates of such a connecting lineTo
        /// segment match the ending coordinates of a currently open
        /// subpath then the segment is omitted as superfluous.
        /// The winding rule of the specified IShape is ignored
        /// and the appended geometry is governed by the winding
        /// rule specified for this path.
        /// </remarks>
        /// <param name="s">the IShape whose geometry is appended
        ///           to this path</param>
        /// <param name="connect">a boolean to control whether or not to turn an initial
        ///                 moveTo segment into a lineTo segment
        ///                to connect the new geometry to the existing path</param>
        public void Append(IShape s, bool connect)
        {
            Append(s.GetPathIterator(null), connect);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// the winding rule.
        /// </summary>
        public int WindingRule
        {
            get { return _windingRule; }
            set
            {
                if (value != WindEvenOdd && value != WindNonZero)
                {
                    throw new ArgumentException("winding rule must be " +
                                                "WindEvenOdd or " +
                                                "WindNonZero");
                }
                _windingRule = value;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the coordinates most recently added to the end of the path
        /// as a Point object.
        /// </summary>
        public Point CurrentPoint
        {
            get
            {
                int index = _numCoords;
                if (_numTypes < 1 || index < 1)
                {
                    return null;
                }
                if (_pointTypes[_numTypes - 1] == SegClose)
                {
                    loop:
                    for (int i = _numTypes - 2; i > 0; i--)
                    {
                        switch (_pointTypes[i])
                        {
                            case SegMoveto:
                                goto loop;
                            case SegLineto:
                                index -= 2;
                                break;
                            case SegQuadto:
                                index -= 4;
                                break;
                            case SegCubicto:
                                index -= 6;
                                break;
                            case SegClose:
                                break;
                        }
                    }
                }
                return GetPoint(index - 2);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Resets the path to empty.
        /// </summary>
        /// <remarks>
        ///  The append position is set back to the
        /// beginning of the path and all coordinates and point types are
        /// forgotten.
        /// </remarks> 
        public void Reset()
        {
            _numTypes = _numCoords = 0;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a new IShape representing a transformed version
        /// of this Path.
        /// </summary>
        /// <remarks>
        /// remember that the exact type and coordinate precision of the return
        /// value is not specified for this method.
        /// The method will return a IShape that contains no less precision
        /// for the transformed geometry than this Path currently
        /// maintains, but it may contain no more precision either.
        /// If the tradeoff of precision vs. 
        /// </remarks>
        /// <param name="at">the AffineTransform used to transform a
        ///            new IShape.</param>
        /// <returns>a new IShape, transformed with the specified
        ///          AffineTransform.</returns>
        public IShape CreateTransformedShape(AffineTransform at)
        {
            Path p2D = (Path) Clone();
            if (at != null)
            {
                p2D.Transform(at);
            }
            return p2D;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tests if the specified coordinates are inside the closed
        /// boundary of the specified IPathIterator.
        /// </summary>
        /// <remarks>
        /// This method provides a basic facility for implementors of
        /// theIShape interface to implement support for the
        /// contains(int, int)method.
        /// </remarks>
        /// <param name="pi">the specified IPathIterator</param>
        /// <param name="x">the specified X coordinate</param>
        /// <param name="y">the specified Y coordinate</param>
        /// <returns>
        /// 	true if the specified coordinates are inside the
        ///     specified IPathIterator; false otherwise
        /// </returns>
        public static bool Contains(PathIterator pi, int x, int y)
        {
            if (x*0 + y*0 == 0)
            {
                /* N * 0 is 0 only if N is finite.
                 * Here we know that both x and y are finite.
                 */
                int mask = (pi.WindingRule == WindNonZero ? -1 : 1);
                int cross = Curve.PointCrossingsForPath(pi, x, y);
                return ((cross & mask) != 0);
            }
            /* Either x or y was infinite or NaN.
                 * A NaN always produces a negative response to any test
                 * and Infinity values cannot be "inside" any path so
                 * they should return false as well.
                 */
            return false;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tests if the specified Point is inside the closed
        /// boundary of the specified IPathIterator.
        /// </summary>
        /// <remarks>
        /// This method provides a basic facility for implementors of
        /// theIShape interface to implement support for the
        /// contains(Point)method.
        /// </remarks>
        /// <param name="pi">the specified IPathIterator</param>
        /// <param name="p">the specified Point</param>
        /// <returns>
        /// 	true if the specified coordinates are inside the
        ///      specified IPathIterator; false otherwise
        /// </returns>
        public static bool Contains(PathIterator pi, Point p)
        {
            return Contains(pi, p.X, p.Y);
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
            if (x*0 + y*0 == 0)
            {
                /* N * 0 is 0 only if N is finite.
                 * Here we know that both x and y are finite.
                 */
                if (_numTypes < 2)
                {
                    return false;
                }
                int mask = (_windingRule == WindNonZero ? -1 : 1);
                return ((PointCrossings(x, y) & mask) != 0);
            }
            /* Either x or y was infinite or NaN.
                 * A NaN always produces a negative response to any test
                 * and Infinity values cannot be "inside" any path so
                 * they should return false as well.
                 */
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
            return Contains(p.X, p.Y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tests if the specified rectangular area is entirely inside the
        /// closed boundary of the specified IPathIterator.
        /// </summary>
        /// <remarks>
        /// This method provides a basic facility for implementors of
        /// theIShape interface to implement support for the
        /// contains(int, int, int, int) method.
        /// 
        /// This method object may conservatively return false in
        /// cases where the specified rectangular area intersects a
        /// segment of the path, but that segment does not represent a
        /// boundary between the interior and exterior of the path.
        /// Such segments could lie entirely within the interior of the
        /// path if they are part of a path with a WindNonZero
        /// winding rule or if the segments are retraced in the reverse
        /// direction such that the two sets of segments cancel each
        /// other out without any exterior area falling between them.
        /// To determine whether segments represent true boundaries of
        /// the interior of the path would require extensive calculations
        /// involving all of the segments of the path and the winding
        /// rule and are thus beyond the scope of this implementation.
        /// </remarks>
        /// <param name="pi">the specified IPathIterator</param>
        /// <param name="x">the specified X coordinate</param>
        /// <param name="y">the specified Y coordinate.</param>
        /// <param name="w">the width of the specified rectangular area.</param>
        /// <param name="h">the height of the specified rectangular area</param>
        /// <returns>
        /// 	true if the specified IPathIterator contains
        ///      the specified rectangluar area; false otherwise.
        /// </returns>
        public static bool Contains(PathIterator pi,
                                    int x, int y, int w, int h)
        {
            if (Double.IsNaN(x + w) || Double.IsNaN(y + h))
            {
                /* [xy]+[wh] is NaN if any of those values are NaN,
                 * or if adding the two together would produce NaN
                 * by virtue of adding opposing Infinte values.
                 * Since we need to add them below, their sum must
                 * not be NaN.
                 * We return false because NaN always produces a
                 * negative response to tests
                 */
                return false;
            }
            if (w <= 0 || h <= 0)
            {
                return false;
            }
            int mask = (pi.WindingRule == WindNonZero ? -1 : 2);
            int crossings = Curve.RectCrossingsForPath(pi, x, y, x + w, y + h);
            return (crossings != Curve.RectIntersects &&
                    (crossings & mask) != 0);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tests if the specified Rectangle is entirely inside the
        /// closed boundary of the specified IPathIterator.
        /// </summary>
        /// <remarks>
        /// This method provides a basic facility for implementors of
        /// theIShape interface to implement support for the
        /// contains(Rectangle)method.
        /// 
        /// This method object may conservatively return false in
        /// cases where the specified rectangular area intersects a
        /// segment of the path, but that segment does not represent a
        /// boundary between the interior and exterior of the path.
        /// Such segments could lie entirely within the interior of the
        /// path if they are part of a path with a WindNonZero
        /// winding rule or if the segments are retraced in the reverse
        /// direction such that the two sets of segments cancel each
        /// other out without any exterior area falling between them.
        /// To determine whether segments represent true boundaries of
        /// the interior of the path would require extensive calculations
        /// involving all of the segments of the path and the winding
        /// rule and are thus beyond the scope of this implementation.
        /// </remarks>
        /// <param name="pi">the specified IPathIterator</param>
        /// <param name="r">a specified Rectangle</param>
        /// <returns>
        /// 	true if the specified IPathIterator contains
        ///      the specified Rectangle; false otherwise.
        /// </returns>
        public static bool Contains(PathIterator pi, Rectangle r)
        {
            return Contains(pi, r.X, r.Y, r.Width, r.Height);
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
        /// The IShape.contains()method allows a IShape
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
            if (Double.IsNaN(x + w) || Double.IsNaN(y + h))
            {
                /* [xy]+[wh] is NaN if any of those values are NaN,
                 * or if adding the two together would produce NaN
                 * by virtue of adding opposing Infinte values.
                 * Since we need to add them below, their sum must
                 * not be NaN.
                 * We return false because NaN always produces a
                 * negative response to tests
                 */
                return false;
            }
            if (w <= 0 || h <= 0)
            {
                return false;
            }
            int mask = (_windingRule == WindNonZero ? -1 : 2);
            int crossings = RectCrossings(x, y, x + w, y + h);
            return (crossings != Curve.RectIntersects &&
                    (crossings & mask) != 0);
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
        /// The IShape.contains()method allows a IShape
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
            return Contains(r.X, r.Y, r.Width, r.Height);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tests if the interior of the specified IPathIterator
        /// intersects the interior of a specified set of rectangular
        /// coordinates.
        /// </summary>
        /// <remarks>
        /// This method provides a basic facility for implementors of
        /// theIShape interface to implement support for the
        /// intersects(int, int, int, int) method.
        /// 
        /// This method object may conservatively return true in
        /// cases where the specified rectangular area intersects a
        /// segment of the path, but that segment does not represent a
        /// boundary between the interior and exterior of the path.
        /// Such a case may occur if some set of segments of the
        /// path are retraced in the reverse direction such that the
        /// two sets of segments cancel each other out without any
        /// interior area between them.
        /// To determine whether segments represent true boundaries of
        /// the interior of the path would require extensive calculations
        /// involving all of the segments of the path and the winding
        /// rule and are thus beyond the scope of this implementation.
        /// </remarks>
        /// <param name="pi">the specified IPathIterator.</param>
        /// <param name="x">the specified X coordinate.</param>
        /// <param name="y">the specified Y coordinate.</param>
        /// <param name="w">the width of the specified rectangular coordinates</param>
        /// <param name="h">the height of the specified rectangular coordinates.</param>
        /// <returns>
        ///  true if the specified IPathIterator and
        ///         the interior of the specified set of rectangular
        ///         coordinates intersect each other; false otherwise.
        /// </returns>
        public static bool Intersects(PathIterator pi,
                                      int x, int y, int w, int h)
        {
            if (Double.IsNaN(x + w) || Double.IsNaN(y + h))
            {
                /* [xy]+[wh] is NaN if any of those values are NaN,
                 * or if adding the two together would produce NaN
                 * by virtue of adding opposing Infinte values.
                 * Since we need to add them below, their sum must
                 * not be NaN.
                 * We return false because NaN always produces a
                 * negative response to tests
                 */
                return false;
            }
            if (w <= 0 || h <= 0)
            {
                return false;
            }
            int mask = (pi.WindingRule == WindNonZero ? -1 : 2);
            int crossings = Curve.RectCrossingsForPath(pi, x, y, x + w, y + h);
            return (crossings == Curve.RectIntersects ||
                    (crossings & mask) != 0);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tests if the interior of the specified IPathIterator
        /// intersects the interior of a specified Rectangle.
        /// </summary>
        /// <remarks>
        /// This method provides a basic facility for implementors of
        /// theIShape interface to implement support for the
        /// intersects(Rectangle) method.
        /// 
        /// This method object may conservatively return true in
        /// cases where the specified rectangular area intersects a
        /// segment of the path, but that segment does not represent a
        /// boundary between the interior and exterior of the path.
        /// Such a case may occur if some set of segments of the
        /// path are retraced in the reverse direction such that the
        /// two sets of segments cancel each other out without any
        /// interior area between them.
        /// To determine whether segments represent true boundaries of
        /// the interior of the path would require extensive calculations
        /// involving all of the segments of the path and the winding
        /// rule and are thus beyond the scope of this implementation.
        /// </remarks>
        /// <param name="pi">the specified IPathIterator</param>
        /// <param name="r">the specified Rectangle</param>
        /// <returns>true if the specified IPathIterator and
        ///         the interior of the specified Rectangle
        ///         intersect each other; false otherwise.</returns>
        public static bool Intersects(PathIterator pi, Rectangle r)
        {
            return Intersects(pi, r.X, r.Y, r.Width, r.Height);
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
            if (Double.IsNaN(x + w) || Double.IsNaN(y + h))
            {
                /* [xy]+[wh] is NaN if any of those values are NaN,
                 * or if adding the two together would produce NaN
                 * by virtue of adding opposing Infinte values.
                 * Since we need to add them below, their sum must
                 * not be NaN.
                 * We return false because NaN always produces a
                 * negative response to tests
                 */
                return false;
            }
            if (w <= 0 || h <= 0)
            {
                return false;
            }
            int mask = (_windingRule == WindNonZero ? -1 : 2);
            int crossings = RectCrossings(x, y, x + w, y + h);
            return (crossings == Curve.RectIntersects ||
                    (crossings & mask) != 0);
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
            return Intersects(r.X, r.Y, r.Width, r.Height);
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
        public PathIterator GetPathIterator(AffineTransform at,
                                            int flatness)
        {
            return new FlatteningPathIterator(GetPathIterator(at), flatness);
        }

        /*
         * Support fields and methods for serializing the subclasses.
         */
        //    private const byte SERIAL_STORAGE_FLT_ARRAY = 0x30;
        //    private const byte SERIAL_STORAGE_DBL_ARRAY = 0x31;
        //
        //    private const byte SERIAL_SEG_FLT_MOVETO    = 0x40;
        //    private const byte SERIAL_SEG_FLT_LINETO    = 0x41;
        //    private const byte SERIAL_SEG_FLT_QUADTO    = 0x42;
        //    private const byte SERIAL_SEG_FLT_CUBICTO   = 0x43;
        //
        //    private const byte SERIAL_SEG_DBL_MOVETO    = 0x50;
        //    private const byte SERIAL_SEG_DBL_LINETO    = 0x51;
        //    private const byte SERIAL_SEG_DBL_QUADTO    = 0x52;
        //    private const byte SERIAL_SEG_DBL_CUBICTO   = 0x53;
        //
        //    private const byte SERIAL_SEG_CLOSE         = 0x60;
        //    private const byte SERIAL_PATH_END          = 0x61;

        private abstract class Iterator : PathIterator
        {
            internal int _typeIdx;
            internal int _pointIdx;
            internal readonly Path _path;
            internal readonly int[] _curvecoords = new[] {2, 2, 4, 6, 0};

            protected Iterator(Path path)
            {
                _path = path;
            }

            public override int WindingRule
            {
                get { return _path.WindingRule; }
            }

            public override bool IsDone()
            {
                return (_typeIdx >= _path._numTypes);
            }

            public override void Next()
            {
                int type = _path._pointTypes[_typeIdx++];
                _pointIdx += _curvecoords[type];
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Adds a point to the path by moving to the specified
        /// coordinates specified in double precision.
        /// </summary>
        /// <param name="x"> the specified X coordinate</param>
        /// <param name="y">the specified Y coordinate</param>
        public void MoveTo(int x, int y)
        {
            if (_numTypes > 0 && _pointTypes[_numTypes - 1] == SegMoveto)
            {
                _intCoords[_numCoords - 2] = x;
                _intCoords[_numCoords - 1] = y;
            }
            else
            {
                NeedRoom(false, 2);
                _pointTypes[_numTypes++] = SegMoveto;
                _intCoords[_numCoords++] = x;
                _intCoords[_numCoords++] = y;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Adds a point to the path by drawing a straight line from the
        /// current coordinates to the new specified coordinates
        /// specified in double precision.
        /// </summary>
        /// <param name="x"> the specified X coordinate</param>
        /// <param name="y">the specified Y coordinate</param>
        public void LineTo(int x, int y)
        {
            NeedRoom(true, 2);
            _pointTypes[_numTypes++] = SegLineto;
            _intCoords[_numCoords++] = x;
            _intCoords[_numCoords++] = y;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Adds a curved segment, defined by two new points, to the path by
        /// drawing a Quadratic curve that intersects both the current
        /// coordinates and the specified coordinates (x2,y2),
        /// using the specified point (x1,y1) as a quadratic
        /// parametric control point.
        /// All coordinates are specified in double precision.
        /// </summary>
        /// <param name="x1">the X coordinate of the quadratic control point.</param>
        /// <param name="y1">the Y coordinate of the quadratic control point.</param>
        /// <param name="x2">the X coordinate of the final end point.</param>
        /// <param name="y2">the Y coordinate of the final end point.</param>
        public void QuadTo(int x1, int y1,
                           int x2, int y2)
        {
            NeedRoom(true, 4);
            _pointTypes[_numTypes++] = SegQuadto;
            _intCoords[_numCoords++] = x1;
            _intCoords[_numCoords++] = y1;
            _intCoords[_numCoords++] = x2;
            _intCoords[_numCoords++] = y2;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Adds a curved segment, defined by three new points, to the path by
        /// drawing a Bezier curve that intersects both the current
        /// coordinates and the specified coordinates (x3,y3),
        /// using the specified points (x1,y1) and (x2,y2) as
        /// Bezier control points.
        /// All coordinates are specified in double precision.
        /// </summary>
        /// <param name="x1">the X coordinate of the first Bezier control point.</param>
        /// <param name="y1">the Y coordinate of the first Bezier control point.</param>
        /// <param name="x2">the X coordinate of the second Bezier control point.</param>
        /// <param name="y2">the Y coordinate of the secondBezier control point.</param>
        /// <param name="x3">the X coordinate of the final end point.</param>
        /// <param name="y3">the Y coordinate of the final end point.</param>
        public void CurveTo(int x1, int y1,
                            int x2, int y2,
                            int x3, int y3)
        {
            NeedRoom(true, 6);
            _pointTypes[_numTypes++] = SegCubicto;
            _intCoords[_numCoords++] = x1;
            _intCoords[_numCoords++] = y1;
            _intCoords[_numCoords++] = x2;
            _intCoords[_numCoords++] = y2;
            _intCoords[_numCoords++] = x3;
            _intCoords[_numCoords++] = y3;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Appends the geometry of the specified
        /// IPathIterator object
        /// to the path, possibly connecting the new geometry to the existing
        /// path segments with a line segment.
        /// </summary>
        /// <remarks>
        /// If the connectparameter is true and the
        /// path is not empty then any initial moveTo in the
        /// geometry of the appended IShape is turned into a
        /// lineTo segment.
        /// If the destination coordinates of such a connecting lineTo
        /// segment match the ending coordinates of a currently open
        /// subpath then the segment is omitted as superfluous.
        /// The winding rule of the specified IShape is ignored
        /// and the appended geometry is governed by the winding
        /// rule specified for this path.
        /// </remarks>
        /// <param name="pi">the IPathIterator whose geometry is appended to
        ///           this path</param>
        /// <param name="connect">ia boolean to control whether or not to turn an initial
        ///                moveTo segment into a lineTo segment
        ///                to connect the new geometry to the existing path</param>
        public void Append(PathIterator pi, bool connect)
        {
            int[] coords = new int[6];
            while (!pi.IsDone())
            {
                switch (pi.CurrentSegment(coords))
                {
                    case SegMoveto:
                        if (!connect || _numTypes < 1 || _numCoords < 1)
                        {
                            MoveTo(coords[0], coords[1]);
                            break;
                        }
                        if (_pointTypes[_numTypes - 1] != SegClose &&
                            _intCoords[_numCoords - 2] == coords[0] &&
                            _intCoords[_numCoords - 1] == coords[1])
                        {
                            // Collapse out initial moveto/lineto
                            break;
                        }
                        LineTo(coords[0], coords[1]);
                        break;
                    case SegLineto:
                        LineTo(coords[0], coords[1]);
                        break;
                    case SegQuadto:
                        QuadTo(coords[0], coords[1],
                               coords[2], coords[3]);
                        break;
                    case SegCubicto:
                        CurveTo(coords[0], coords[1],
                                coords[2], coords[3],
                                coords[4], coords[5]);
                        break;
                    case SegClose:
                        ClosePath();
                        break;
                }
                pi.Next();
                connect = false;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Transforms the geometry of this path using the specified
        /// AffineTransform.
        /// </summary>
        /// <remarks>
        /// The geometry is transformed in place, which permanently changes the
        /// boundary defined by this object.
        /// </remarks>
        /// <param name="at">the AffineTransform used to transform the area</param>
        public void Transform(AffineTransform at)
        {
            at.Transform(_intCoords, 0, _intCoords, 0, _numCoords/2);
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
                int x1, y1, x2, y2;
                int i = _numCoords;
                if (i > 0)
                {
                    y1 = y2 = _intCoords[--i];
                    x1 = x2 = _intCoords[--i];
                    while (i > 0)
                    {
                        int y = _intCoords[--i];
                        int x = _intCoords[--i];
                        if (x < x1)
                        {
                            x1 = x;
                        }
                        if (y < y1)
                        {
                            y1 = y;
                        }
                        if (x > x2)
                        {
                            x2 = x;
                        }
                        if (y > y2)
                        {
                            y2 = y;
                        }
                    }
                }
                else
                {
                    x1 = y1 = x2 = y2 = 0;
                }
                return new Rectangle(x1, y1, x2 - x1, y2 - y1);
            }
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
        /// IShape outline.  If an optional AffineTransform
        /// is specified, the coordinates returned in the iteration are
        /// transformed accordingly.
        /// </summary>
        /// <remarks>
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
            if (at == null)
            {
                return new CopyIterator(this);
            }
            return new TxIterator(this, at);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// To a SVG string.
        /// </summary>
        /// <param name="shape">The shape.</param>
        /// <returns>a SVG string</returns>
        public static string ToSVG(IShape shape)
        {
            PathIterator pathIterator = shape.GetPathIterator(null);
            StringBuilder svgString = new StringBuilder("<path d='");
            int[] coords = new int[6];
            int type;
            while (!pathIterator.IsDone())
            {
                type = pathIterator.CurrentSegment(coords);
                switch (type)
                {
                    case PathIterator.SegClose:
                        svgString.Append("Z ");
                        break;
                    case PathIterator.SegCubicto:
                        svgString.Append("C ");
                        svgString.Append(coords[0] + " ");
                        svgString.Append(coords[1] + " ");
                        svgString.Append(coords[2] + " ");
                        svgString.Append(coords[3] + " ");
                        svgString.Append(coords[4] + " ");
                        svgString.Append(coords[5]);
                        break;
                    case PathIterator.SegLineto:
                        svgString.Append("L ");
                        svgString.Append(coords[0] + " ");
                        svgString.Append(coords[1]);
                        break;
                    case PathIterator.SegMoveto:
                        svgString.Append("M ");
                        svgString.Append(coords[0] + " ");
                        svgString.Append(coords[1]);
                        break;
                    case PathIterator.SegQuadto:
                        svgString.Append("Q ");
                        svgString.Append(coords[0] + " ");
                        svgString.Append(coords[1] + " ");
                        svgString.Append(coords[2] + " ");
                        svgString.Append(coords[3]);
                        break;
                }

                pathIterator.Next();
            }
            svgString.Append("' />");
            return svgString.ToString();
        } // For c simplicity, copy these constants to our namespace
        // and cast them to byte constants for easy storage.
        private const byte SegMoveto = PathIterator.SegMoveto;
        private const byte SegLineto = PathIterator.SegLineto;
        private const byte SegQuadto = PathIterator.SegQuadto;
        private const byte SegCubicto = PathIterator.SegCubicto;
        private const byte SegClose = PathIterator.SegClose;
        internal byte[] _pointTypes;
        private int _numTypes;
        private int _numCoords;
        private int _windingRule;
        private const int InitSize = 20;
        private const int ExpandMax = 500;
        private int[] _intCoords;
        private static readonly PathParser PathParser = new PathParser();

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Points the crossings.
        /// </summary>
        /// <param name="px">The px.</param>
        /// <param name="py">The py.</param>
        /// <returns></returns>
        private int PointCrossings(int px, int py)
        {
            int movx, movy;
            int[] coords = _intCoords;
            int curx = movx = coords[0];
            int cury = movy = coords[1];
            int crossings = 0;
            int ci = 2;
            for (int i = 1; i < _numTypes; i++)
            {
                int endx;
                int endy;
                switch (_pointTypes[i])
                {
                    case PathIterator.SegMoveto:
                        if (cury != movy)
                        {
                            crossings +=
                                Curve.PointCrossingsForLine(px, py,
                                                            curx, cury,
                                                            movx, movy);
                        }
                        movx = curx = coords[ci++];
                        movy = cury = coords[ci++];
                        break;
                    case PathIterator.SegLineto:
                        crossings +=
                            Curve.PointCrossingsForLine(px, py,
                                                        curx, cury,
                                                        endx = coords[ci++],
                                                        endy = coords[ci++]);
                        curx = endx;
                        cury = endy;
                        break;
                    case PathIterator.SegQuadto:
                        crossings +=
                            Curve.PointCrossingsForQuad(px, py,
                                                        curx, cury,
                                                        coords[ci++],
                                                        coords[ci++],
                                                        endx = coords[ci++],
                                                        endy = coords[ci++],
                                                        0);
                        curx = endx;
                        cury = endy;
                        break;
                    case PathIterator.SegCubicto:
                        crossings +=
                            Curve.PointCrossingsForCubic(px, py,
                                                         curx, cury,
                                                         coords[ci++],
                                                         coords[ci++],
                                                         coords[ci++],
                                                         coords[ci++],
                                                         endx = coords[ci++],
                                                         endy = coords[ci++],
                                                         0);
                        curx = endx;
                        cury = endy;
                        break;
                    case PathIterator.SegClose:
                        if (cury != movy)
                        {
                            crossings +=
                                Curve.PointCrossingsForLine(px, py,
                                                            curx, cury,
                                                            movx, movy);
                        }
                        curx = movx;
                        cury = movy;
                        break;
                }
            }
            if (cury != movy)
            {
                crossings +=
                    Curve.PointCrossingsForLine(px, py,
                                                curx, cury,
                                                movx, movy);
            }
            return crossings;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Clones the coords.
        /// </summary>
        /// <param name="at">At.</param>
        /// <returns></returns>
        private int[] CloneCoords(AffineTransform at)
        {
            int[] ret;
            if (at == null)
            {
                ret = new int[_intCoords.Length];
                Array.Copy(_intCoords, ret, ret.Length);
            }
            else
            {
                ret = new int[_intCoords.Length];
                at.Transform(_intCoords, 0, ret, 0, _numCoords/2);
            }
            return ret;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the point.
        /// </summary>
        /// <param name="coordindex">The coordindex.</param>
        /// <returns></returns>
        private Point GetPoint(int coordindex)
        {
            Point pt = new Point();
            pt.X = _intCoords[coordindex];
            pt.Y = _intCoords[coordindex + 1];
            return pt;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Needs the room.
        /// </summary>
        /// <param name="needMove">if set to true [need move].</param>
        /// <param name="newCoords">The new coords.</param>
        private void NeedRoom(bool needMove, int newCoords)
        {
            if (needMove && _numTypes == 0)
            {
                throw new IllegalPathStateException("missing initial moveto " +
                                                    "in path definition");
            }
            int size = _pointTypes.Length;
            if (_numTypes >= size)
            {
                int grow = size;
                if (grow > ExpandMax)
                {
                    grow = ExpandMax;
                }
                byte[] temp = new byte[size];
                Array.Copy(_pointTypes, temp, temp.Length);
                _pointTypes = new byte[size + grow];
                Array.Copy(temp, _pointTypes, temp.Length);
            }
            size = _intCoords.Length;
            if (_numCoords + newCoords > size)
            {
                int grow = size;
                if (grow > ExpandMax*2)
                {
                    grow = ExpandMax*2;
                }
                if (grow < newCoords)
                {
                    grow = newCoords;
                }
                int[] temp = new int[size];
                Array.Copy(_intCoords, temp, temp.Length);
                _intCoords = new int[size + grow];
                Array.Copy(temp, _intCoords, temp.Length);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Rects the crossings.
        /// </summary>
        /// <param name="rxmin">The rxmin.</param>
        /// <param name="rymin">The rymin.</param>
        /// <param name="rxmax">The rxmax.</param>
        /// <param name="rymax">The rymax.</param>
        /// <returns></returns>
        private int RectCrossings(int rxmin, int rymin,
                                  int rxmax, int rymax)
        {
            int[] coords = _intCoords;
            int movx, movy;
            int curx = movx = coords[0];
            int cury = movy = coords[1];
            int crossings = 0;
            int ci = 2;
            for (int i = 1;
                 crossings != Curve.RectIntersects && i < _numTypes;
                 i++)
            {
                int endx;
                int endy;
                switch (_pointTypes[i])
                {
                    case PathIterator.SegMoveto:
                        if (curx != movx || cury != movy)
                        {
                            crossings =
                                Curve.RectCrossingsForLine(crossings,
                                                           rxmin, rymin,
                                                           rxmax, rymax,
                                                           curx, cury,
                                                           movx, movy);
                        }
                        // Count should always be a multiple of 2 here.
                        // assert((crossings & 1) != 0);
                        movx = curx = coords[ci++];
                        movy = cury = coords[ci++];
                        break;
                    case PathIterator.SegLineto:
                        endx = coords[ci++];
                        endy = coords[ci++];
                        crossings =
                            Curve.RectCrossingsForLine(crossings,
                                                       rxmin, rymin,
                                                       rxmax, rymax,
                                                       curx, cury,
                                                       endx, endy);
                        curx = endx;
                        cury = endy;
                        break;
                    case PathIterator.SegQuadto:
                        crossings =
                            Curve.RectCrossingsForQuad(crossings,
                                                       rxmin, rymin,
                                                       rxmax, rymax,
                                                       curx, cury,
                                                       coords[ci++],
                                                       coords[ci++],
                                                       endx = coords[ci++],
                                                       endy = coords[ci++],
                                                       0);
                        curx = endx;
                        cury = endy;
                        break;
                    case PathIterator.SegCubicto:
                        crossings =
                            Curve.RectCrossingsForCubic(crossings,
                                                        rxmin, rymin,
                                                        rxmax, rymax,
                                                        curx, cury,
                                                        coords[ci++],
                                                        coords[ci++],
                                                        coords[ci++],
                                                        coords[ci++],
                                                        endx = coords[ci++],
                                                        endy = coords[ci++],
                                                        0);
                        curx = endx;
                        cury = endy;
                        break;
                    case PathIterator.SegClose:
                        if (curx != movx || cury != movy)
                        {
                            crossings =
                                Curve.RectCrossingsForLine(crossings,
                                                           rxmin, rymin,
                                                           rxmax, rymax,
                                                           curx, cury,
                                                           movx, movy);
                        }
                        curx = movx;
                        cury = movy;
                        // Count should always be a multiple of 2 here.
                        // assert((crossings & 1) != 0);
                        break;
                }
            }
            if (crossings != Curve.RectIntersects &&
                (curx != movx || cury != movy))
            {
                crossings =
                    Curve.RectCrossingsForLine(crossings,
                                               rxmin, rymin,
                                               rxmax, rymax,
                                               curx, cury,
                                               movx, movy);
            }
            // Count should always be a multiple of 2 here.
            // assert((crossings & 1) != 0);
            return crossings;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        private class CopyIterator : Iterator
        {
            private readonly int[] _intCoords;

            internal CopyIterator(Path p2Dd)
                : base(p2Dd)
            {
                _intCoords = p2Dd._intCoords;
            }

            public override int CurrentSegment(int[] coords)
            {
                int type = _path._pointTypes[_typeIdx];
                int numCoords = _curvecoords[type];
                if (numCoords > 0)
                {
                    for (int i = 0; i < numCoords; i++)
                    {
                        coords[i] = _intCoords[_pointIdx + i];
                    }
                }
                return type;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        private class TxIterator : Iterator
        {
            private readonly int[] _intCoords;
            private readonly AffineTransform _affine;

            internal TxIterator(Path p2Dd, AffineTransform at)
                : base(p2Dd)
            {
                _intCoords = p2Dd._intCoords;
                _affine = at;
            }

            public override int CurrentSegment(int[] coords)
            {
                int type = _path._pointTypes[_typeIdx];
                int numCoords = _curvecoords[type];
                if (numCoords > 0)
                {
                    _affine.Transform(_intCoords, _pointIdx,
                                      coords, 0, numCoords/2);
                }
                return type;
            }
        }
    }
}