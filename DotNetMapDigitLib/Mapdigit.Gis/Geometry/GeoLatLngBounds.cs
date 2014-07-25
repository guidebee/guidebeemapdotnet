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
    /// GeoLatLngBounds is a bound in geographical coordinates longitude and latitude.
    /// remember: the positive of North is from top to bottom instead of from bottom to 
    /// top internally.
    /// </summary>
    public class GeoLatLngBounds : GeoBounds
    {

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
        public GeoLatLngBounds()
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
        /// Constructs a new GeoBounds, initialized to match 
        /// the values of the specified GeoBounds.
        /// </summary>
        /// <param name="r">the GeoBounds from which to copy initial values
        ///          to a newly constructed GeoBounds.</param>
        public GeoLatLngBounds(GeoBounds r)
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
        /// <param name="x">the specified X coordinate</param>
        /// <param name="y">the specified Y coordinate</param>
        /// <param name="width">the width of the GeoBounds</param>
        /// <param name="height">the height of the GeoBounds.</param>
        public GeoLatLngBounds(double x, double y, double width, double height)
            : base(x, y, width, height)
        {

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
        /// <param name="height">the height of the GeoBounds.</param>
        public GeoLatLngBounds(int width, int height)
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
        /// specified by the GeoPoint argument, and
        /// whose width and height are specified by the 
        /// GeoSize argument.
        /// </summary>
        /// <param name="p">a GeoPoint that is the upper-left corner of 
        /// the GeoBounds</param>
        /// <param name="size">size a GeoSize, representing the 
        /// width and height of the GeoBounds</param>
        public GeoLatLngBounds(GeoPoint p, GeoSize size)
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
        public GeoLatLngBounds(GeoPoint p)
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
        public GeoLatLngBounds(GeoSize size)
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
        ///  Constructs a rectangle from the points at its south-west and north-east 
        /// corners.
        /// </summary>
        /// <param name="sw">south-west point of the rectangle</param>
        /// <param name="ne">north-east point of the rectangle</param>
        public GeoLatLngBounds(GeoPoint sw, GeoPoint ne)
            : this(sw.X, sw.Y, ne.X - sw.X, ne.Y - sw.Y)
        {

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Returns true if the geographical coordinates of the point lie within 
        /// this rectangle
        /// </summary>
        /// <param name="latlng">the given point.</param>
        /// <returns>
        /// 	if the geographical coordinates of the point lie within 
        /// this rectangle
        /// </returns>
        public bool ContainsLatLng(GeoLatLng latlng)
        {
            return Contains(latlng.X, latlng.Y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tests if the interior of the IShape intersects the interior of a 
        /// specified rectangular area.
        /// </summary>
        /// <param name="other"> the given rectangle.</param>
        /// <returns>true if the interior of the IShape and the interior of the 
        ///  rectangular area intersect.</returns>
        public bool Intersects(GeoLatLngBounds other)
        {
            return Intersects(other.X, other.Y, other.Width, other.Height);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tests if the interior of the IShape entirely contains the specified 
        /// rectangular area. 
        /// </summary>
        /// <param name="other"> the given rectangle.</param>
        /// <returns>
        /// 	true if the interior of the IShape entirely contains the 
        ///  specified rectangular area; 
        /// </returns>
        public bool ContainsBounds(GeoLatLngBounds other)
        {
            return Contains(other.X, other.Y, other.Width, other.Height);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Enlarges this rectangle such that it contains the given point
        /// </summary>
        /// <param name="latlng">the new GeoLatLng to add to this rectangle.</param>
        public void Extend(GeoLatLng latlng)
        {
            Add(latlng.X, latlng.Y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the point at the south-west corner of the rectangle.
        /// </summary>
        public GeoLatLng SouthWest
        {
            get { return new GeoLatLng(Y, X); }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the point at the north-east corner of the rectangle.
        /// </summary>
        public GeoLatLng NorthEast
        {
            get { return new GeoLatLng(Y + Height, X + Width); }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a GLatLng whose cordinates represent the size of this rectangle.
        /// </summary>
        /// <returns>the point whose cordinates represent the size of this rectangle</returns>
        public GeoLatLng ToSpan()
        {
            return new GeoLatLng(Width, Height);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns true if this rectangle extends from the south pole to the north pole
        /// </summary>
        /// <returns>
        /// 	true if this rectangle extends from the south pole to the north pole
        /// </returns>
        public bool IsFullLat()
        {
            return (Y == -90 && Height == 180);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns true if this rectangle extends fully around the earth in the
        /// longitude direction.
        /// </summary>
        /// <returns>
        /// 	true if this rectangle extends fully around the earth in the
        ///  longitude direction.
        /// </returns>
        public bool IsFullLng()
        {
            return (Y == -180 && Height == 360);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns true if this rectangle is empty.
        /// </summary>
        /// <returns>true,it's empty.</returns>
        public override bool IsEmpty()
        {
            return (Width <= 0) || (Height <= 0);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the point at the center of the rectangle.
        /// </summary>
        public GeoLatLng Center
        {
            get
            {
                GeoPoint pt1 = MapLayer.FromLatLngToPixel(SouthWest, 15);
                GeoPoint pt2 = MapLayer.FromLatLngToPixel(NorthEast, 15);
                GeoPoint pt = new GeoPoint
                                  {
                                      X = pt1.X + (pt2.X - pt1.X)/2,
                                      Y = pt1.Y + (pt2.Y - pt1.Y)/2
                                  };
                return MapLayer.FromPixelToLatLng(pt, 15);
            }
        }
    }

}
