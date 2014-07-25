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
    /// The Polygon class encapsulates a description of a
    /// closed, two-dimensional region within a coordinate space. This
    /// region is bounded by an arbitrary number of line segments, each of
    /// which is one side of the polygon. 
    /// </summary>
    /// <remarks>
    /// Internally, a polygon comprises of a list of (x,y)
    /// coordinate pairs, where each pair defines a <i>vertex</i> of the
    /// polygon, and two successive pairs are the endpoints of a
    /// line that is a side of the polygon. The first and final
    /// pairs of (x,y) points are joined by a line segment
    /// that closes the polygon.  This Polygon is defined with
    /// an even-odd winding rule.  See
    /// WindEvenOdd
    /// for a definition of the even-odd winding rule.
    /// This class's hit-testing methods, which include the
    /// contains, intersects and inside
    /// methods, use the <i>insideness</i> definition described in the
    /// IShape class comments.
    /// </remarks>
    public class Polygon : IShape
    {

        /// <summary>
        /// The total number of points. 
        /// </summary>
        /// <remarks>
        ///  The Value of npoints
        /// represents the number of valid points in this Polygon
        /// and might be less than the number of elements in
        /// xpoints or ypoints.
        /// This Value can be NULL.
        /// </remarks>
        public int NumOfPoints;


        /// <summary>
        /// The array of X coordinates.  
        /// </summary>
        /// <remarks>
        /// The number of elements in
        /// this array might be more than the number of X coordinates
        /// in this Polygon.  The extra elements allow new points
        /// to be added to this Polygon without re-creating this
        /// array.  The Value of npoints is equal to the
        /// number of valid points in this Polygon.
        /// </remarks>
        public int[] XPoints;


        /// <summary>
        /// The array of Y coordinates.  
        /// </summary>
        /// <remarks>
        /// The number of elements in
        /// this array might be more than the number of Y coordinates
        /// in this Polygon.  The extra elements allow new points
        /// to be added to this Polygon without re-creating this
        /// array.  The Value of npoints is equal to the
        /// number of valid points in this Polygon.
        /// </remarks>
        public int[] YPoints;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parse a polygon from a string
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>polygon object</returns>
        public static Polygon FromString(string input)
        {
            lock (NumberListParser)
            {
                float[] coords = NumberListParser.ParseNumberList(input);
                int length = coords.Length/2;
                if (length >= 2)
                {
                    int[] xpoints = new int[length];
                    int[] ypoints = new int[length];
                    for (int i = 0; i < length; i++)
                    {
                        xpoints[i] = (int) coords[i*2];
                        ypoints[i] = (int) coords[i*2 + 1];
                    }
                    return new Polygon(xpoints, ypoints, length);
                }
                return null;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="Polygon"/> class.
        /// </summary>
        public Polygon()
        {
            XPoints = new int[MinLength];
            YPoints = new int[MinLength];
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs and initializes a Polygon from the specified
        /// parameters.
        /// </summary>
        /// <param name="xpoints"> an array of X coordinates.</param>
        /// <param name="ypoints">an array of Y coordinates.</param>
        /// <param name="npoints">the total number of points in the
        /// 			Polygon</param>
        public Polygon(int[] xpoints, int[] ypoints, int npoints)
        {
            if (npoints > xpoints.Length || npoints > ypoints.Length)
            {
                throw new IndexOutOfRangeException("npoints > xpoints.length || " +
                                                   "npoints > ypoints.length");
            }
            if (npoints < 0)
            {
                throw new ArgumentException("npoints < 0");
            }
            NumOfPoints = npoints;
            XPoints = new int[xpoints.Length];
            Array.Copy(xpoints, XPoints, xpoints.Length);
            YPoints = new int[ypoints.Length];
            Array.Copy(ypoints, YPoints, ypoints.Length);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Resets this Polygon object to an empty polygon.
        /// </summary>
        /// <remarks>
        /// The coordinate arrays and the data in them are left untouched
        /// but the number of points is reset to zero to mark the old
        /// vertex data as invalid and to start accumulating new vertex
        /// data at the beginning.
        /// All internally-cached data relating to the old vertices
        /// are discarded.
        /// Remember that since the coordinate arrays from before the reset
        /// are reused, creating a new empty Polygon might
        /// be more memory efficient than resetting the current one if
        /// the number of vertices in the new polygon data is significantly
        /// smaller than the number of vertices in the data from before the
        /// reset.
        /// </remarks>
        public void Reset()
        {
            NumOfPoints = 0;
            _bounds = null;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Invalidates or flushes any internally-cached data that depends
        /// on the vertex coordinates of this Polygon.
        /// </summary>
        /// <remarks>
        /// This method should be called after any direct manipulation
        /// of the coordinates in the xpoints or
        /// ypoints arrays to avoid inconsistent results
        /// from methods such as getBounds or contains
        /// that might cache data from earlier computations relating to
        /// the vertex coordinates.
        /// </remarks>
        public void Invalidate()
        {
            _bounds = null;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Translates the vertices of the Polygon by
        /// deltaX along the x axis and by
        /// deltaY along the y axis.
        /// </summary>
        /// <param name="deltaX">the amount to translate along the X axis.</param>
        /// <param name="deltaY">the amount to translate along the Y axis.</param>
        public void Translate(int deltaX, int deltaY)
        {
            for (int i = 0; i < NumOfPoints; i++)
            {
                XPoints[i] += deltaX;
                YPoints[i] += deltaY;
            }
            if (_bounds != null)
            {
                _bounds.Translate(deltaX, deltaY);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Appends the specified coordinates to this Polygon.
        /// </summary>
        /// <remarks>
        /// If an operation that calculates the bounding box of this
        /// Polygon has already been performed, such as
        /// getBounds or contains, then this
        /// method updates the bounding box.
        /// </remarks>
        /// <param name="pt"> a point to be added..</param>
        public void AddPoint(Point pt)
        {
            AddPoint(pt.X, pt.Y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Appends the specified coordinates to this Polygon.
        /// </summary>
        /// <remarks>
        /// If an operation that calculates the bounding box of this
        /// Polygon has already been performed, such as
        /// getBounds or contains, then this
        /// method updates the bounding box.
        /// </remarks>
        /// <param name="x">the specified X coordinate.</param>
        /// <param name="y">the specified Y coordinate.</param>
        public void AddPoint(int x, int y)
        {
            if (NumOfPoints >= XPoints.Length || NumOfPoints >= YPoints.Length)
            {
                int newLength = NumOfPoints*2;
                // Make sure that newLength will be greater than MinLength and
                // aligned to the power of 2
                if (newLength < MinLength)
                {
                    newLength = MinLength;
                }

                int[] temp = new int[XPoints.Length];
                Array.Copy(XPoints, temp, XPoints.Length);
                XPoints = new int[newLength];
                Array.Copy(temp, XPoints, temp.Length);

                temp = new int[YPoints.Length];
                Array.Copy(YPoints, temp, YPoints.Length);
                YPoints = new int[newLength];
                Array.Copy(temp, YPoints, temp.Length);
            }
            XPoints[NumOfPoints] = x;
            YPoints[NumOfPoints] = y;
            NumOfPoints++;
            if (_bounds != null)
            {
                UpdateBounds(x, y);
            }
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
            get { return GetBoundingBox(); }
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
        /// Determines whether the specified coordinates are contained in this
        ///  Polygon.
        /// </summary>
        /// <param name="x">the specified X coordinate to be tested.</param>
        /// <param name="y">the specified Y coordinate to be tested.</param>
        /// <returns>true if this Polygon contains
        ///          the specified coordinates (x,y);
        ///         false otherwise.</returns>
        public bool Inside(int x, int y)
        {
            return Contains(x, y);
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
            if (NumOfPoints <= 2 || !GetBoundingBox().Contains(x, y))
            {
                return false;
            }
            int hits = 0;

            int lastx = XPoints[NumOfPoints - 1];
            int lasty = YPoints[NumOfPoints - 1];
            int curx, cury;

            // Walk the edges of the polygon
            for (int i = 0; i < NumOfPoints; lastx = curx, lasty = cury, i++)
            {
                curx = XPoints[i];
                cury = YPoints[i];

                if (cury == lasty)
                {
                    continue;
                }

                int leftx;
                if (curx < lastx)
                {
                    if (x >= lastx)
                    {
                        continue;
                    }
                    leftx = curx;
                }
                else
                {
                    if (x >= curx)
                    {
                        continue;
                    }
                    leftx = lastx;
                }

                double test1, test2;
                if (cury < lasty)
                {
                    if (y < cury || y >= lasty)
                    {
                        continue;
                    }
                    if (x < leftx)
                    {
                        hits++;
                        continue;
                    }
                    test1 = x - curx;
                    test2 = y - cury;
                }
                else
                {
                    if (y < lasty || y >= cury)
                    {
                        continue;
                    }
                    if (x < leftx)
                    {
                        hits++;
                        continue;
                    }
                    test1 = x - lastx;
                    test2 = y - lasty;
                }

                if (test1 < (test2/(lasty - cury)*(lastx - curx)))
                {
                    hits++;
                }
            }

            return ((hits & 1) != 0);
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
        /// The IShape.intersects()method allows a IShape
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
            if (NumOfPoints <= 0 || !GetBoundingBox().Intersects(x, y, w, h))
            {
                return false;
            }

            Crossings cross = GetCrossings(x, y, x + w, y + h);
            return (cross == null || !cross.IsEmpty());
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
        /// The IShape.intersects()method allows a IShape
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
            return Intersects(r.X, r.Y, r.Width, r.Height);
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
            if (NumOfPoints <= 0 || !GetBoundingBox().Intersects(x, y, w, h))
            {
                return false;
            }

            Crossings cross = GetCrossings(x, y, x + w, y + h);
            return (cross != null && cross.Covers(y, y + h));
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
            return Contains(r.X, r.Y, r.Width, r.Height);
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
            return new PolygonPathIterator(this, at);
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
            return GetPathIterator(at);
        }


        /**
         * The bounds of this Polygon.
         * This Value can be null.
         */
        private Rectangle _bounds;

        /*
         * Default length for xpoints and ypoints.
         */
        private const int MinLength = 4;

        private static readonly NumberListParser NumberListParser = new NumberListParser();

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Calculates the bounding box of the points passed to the constructor.
        /// Sets bounds to the result.
        /// </summary>
        /// <param name="xpoints">array of <i>x</i> coordinates</param>
        /// <param name="ypoints">array of <i>y</i> coordinates.</param>
        /// <param name="npoints">the total number of points.</param>
        private void CalculateBounds(int[] xpoints, int[] ypoints, int npoints)
        {
            int boundsMinX = int.MaxValue;
            int boundsMinY = int.MaxValue;
            int boundsMaxX = int.MinValue;
            int boundsMaxY = int.MinValue;

            for (int i = 0; i < npoints; i++)
            {
                int x = xpoints[i];
                boundsMinX = Math.Min(boundsMinX, x);
                boundsMaxX = Math.Max(boundsMaxX, x);
                int y = ypoints[i];
                boundsMinY = Math.Min(boundsMinY, y);
                boundsMaxY = Math.Max(boundsMaxY, y);
            }
            _bounds = new Rectangle(boundsMinX, boundsMinY,
                                    boundsMaxX - boundsMinX,
                                    boundsMaxY - boundsMinY);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Resizes the bounding box to accomodate the specified coordinates.
        /// </summary>
        /// <param name="x">The x coordinates.</param>
        /// <param name="y">The y coordinates.</param>
        private void UpdateBounds(int x, int y)
        {
            if (x < _bounds.X)
            {
                _bounds.Width = _bounds.Width + (_bounds.X - x);
                _bounds.X = x;
            }
            else
            {
                _bounds.Width = Math.Max(_bounds.Width, x - _bounds.X);
                // bounds.x = bounds.x;
            }

            if (y < _bounds.Y)
            {
                _bounds.Height = _bounds.Height + (_bounds.Y - y);
                _bounds.Y = y;
            }
            else
            {
                _bounds.Height = Math.Max(_bounds.Height, y - _bounds.Y);
                // bounds.y = bounds.y;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the bounds of this Polygon.
        /// </summary>
        /// <returns>the bounds of this Polygon.</returns>
        private Rectangle GetBoundingBox()
        {
            if (NumOfPoints == 0)
            {
                return new Rectangle();
            }
            if (_bounds == null)
            {
                CalculateBounds(XPoints, YPoints, NumOfPoints);
            }
            if (_bounds != null) return _bounds.Bounds;
            return null;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the crossings.
        /// </summary>
        /// <param name="xlo">The xlo.</param>
        /// <param name="ylo">The ylo.</param>
        /// <param name="xhi">The xhi.</param>
        /// <param name="yhi">The yhi.</param>
        /// <returns></returns>
        private Crossings GetCrossings(int xlo, int ylo,
                                       int xhi, int yhi)
        {
            Crossings cross = new Crossings.EvenOdd(xlo, ylo, xhi, yhi);
            int lastx = XPoints[NumOfPoints - 1];
            int lasty = YPoints[NumOfPoints - 1];
            int curx, cury;

            // Walk the edges of the polygon
            for (int i = 0; i < NumOfPoints; i++)
            {
                curx = XPoints[i];
                cury = YPoints[i];
                if (cross.AccumulateLine(lastx, lasty, curx, cury))
                {
                    return null;
                }
                lastx = curx;
                lasty = cury;
            }

            return cross;
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
        private class PolygonPathIterator : PathIterator
        {
            private readonly Polygon _poly;
            private readonly AffineTransform _transform;
            private int _index;

            public PolygonPathIterator(Polygon pg, AffineTransform at)
            {
                _poly = pg;
                _transform = at;
                if (pg.NumOfPoints == 0)
                {
                    // Prevent a spurious SegClose segment
                    _index = 1;
                }
            }

            /**
             * Returns the winding rule for determining the interior of the
             * path.
             * @return an integer representing the current winding rule.
             */

            public override int WindingRule
            {
                get { return WindEvenOdd; }
            }

            /**
             * Tests if there are more points to read.
             * @return true if there are more points to read;
             *          false otherwise.
             */

            public override bool IsDone()
            {
                return _index > _poly.NumOfPoints;
            }

            /**
             * Moves the iterator forwards, along the primary direction of
             * traversal, to the next segment of the path when there are
             * more points in that direction.
             */

            public override void Next()
            {
                _index++;
            }

            /**
             * Returns the coordinates and type of the current path segment in
             * the iteration.
             * The return Value is the path segment type:
             * SegMoveto, SegLineto, or SegClose.
             * A int array of length 2 must be passed in and
             * can be used to store the coordinates of the point(s).
             * Each point is stored as a pair of int x,y
             * coordinates.  SegMoveto and SegLineto types return one
             * point, and SegClose does not return any points.
             * @param coords a int array that specifies the
             * coordinates of the point(s)
             * @return an integer representing the type and coordinates of the
             * 		current path segment.
             */

            public override int CurrentSegment(int[] coords)
            {
                if (_index >= _poly.NumOfPoints)
                {
                    return SegClose;
                }
                coords[0] = _poly.XPoints[_index];
                coords[1] = _poly.YPoints[_index];
                if (_transform != null)
                {
                    _transform.Transform(coords, 0, coords, 0, 1);
                }
                return (_index == 0 ? SegMoveto : SegLineto);
            }
        }
    }
}