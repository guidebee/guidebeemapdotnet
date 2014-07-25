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
    /// GeoLatLng is a point in geographical coordinates longitude and latitude.
    /// </summary>
    public class GeoLatLng : GeoPoint
    {

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoLatLng"/> class.
        /// </summary>
        /// Constructs and initializes a point at the origin (0,0) of the coordinate space.
        public GeoLatLng()
        {

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoLatLng"/> class.
        /// </summary>
        /// <param name="p">a point.</param>
        public GeoLatLng(GeoLatLng p)
            : this(p.Latitude, p.Longitude)
        {

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Constructs and initializes a point at the specified 
        /// (lat,lng) location in the coordinate space. 
        /// </summary>
        /// <param name="lat">the latitude coordinate.</param>
        /// <param name="lng">the longitute coordinate.</param>
        public GeoLatLng(double lat, double lng)
            : this(lat, lng, true)
        {

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs and initializes a point at the specified 
        /// (lat,lng) location in the coordinate space. 
        /// </summary>
        /// <param name="lat">the latitude coordinate.</param>
        /// <param name="lng">the longitute coordinate.</param>
        /// <param name="unbounded">whether the point of bounded or not</param>
        public GeoLatLng(double lat, double lng, bool unbounded)
        {
            double lat1 = lat;
            double lng1 = lng;
            if (!unbounded)
            {

                lng1 = lng1 - (((int)(lng1)) / 360) * 360;

                if (lng1 < 0)
                {
                    lng1 += 360;
                }
                if (lat1 < -90)
                {
                    lat1 = -90;
                }
                else if (-90 <= lat1 && lat1 < 90)
                {
                    //lat1 = lat1;
                }
                else if (90 <= lat1)
                {
                    lat1 = 90;
                }
                if (0 <= lng1 && lng1 < 180)
                {
                    //lng1 = lng1;
                }
                else
                {
                    lng1 = lng1 - 360;
                }
            }
            SetLocation(lng1, lat1);

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the latitude coordinate in degrees, as a number between -90 and 
        /// +90. If the unbounded flag was set in the constructor,
        /// this coordinate can be outside this interval.
        /// </summary>
        public double Latitude
        {
            get { return Y; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the longitude coordinate in degrees, as a number between -180 and 
        /// +180. If the unbounded flag was set in the constructor,
        /// this coordinate can be outside this interval.
        /// </summary>
        public double Longitude
        {
            get { return X; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the latitude coordinate in radians, as a number between -Pi/2 
        /// and +Pi/2. If the unbounded flag was set in the constructor, 
        /// this coordinate can be outside this interval.
        /// </summary>
        /// <returns>the latitude coordinate in radians</returns>
        public double LatRadians()
        {
            return MathEx.ToRadians(Y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the longitude coordinate in radians, as a number between -Pi 
        /// and +Pi. If the unbounded flag was set in the constructor, 
        /// this coordinate can be outside this interval.
        /// </summary>
        /// <returns> the longitude coordinate in radians</returns>
        public double LngRadians()
        {
            return MathEx.ToRadians(X);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determines whether or not two points are equal. Two instances of
        /// GeoLatLng are equal if the values of their 
        /// x and y member fields, representing
        /// their position in the coordinate space, are the same.
        /// </summary>
        /// <param name="obj">an object to be compared with this GeoLatLng</param>
        /// <returns>
        /// 	true ,if the object to be compared is
        ///         an instance of GeoLatLng and has
        ///         the same values; false otherwise.
        /// </returns>
        public new bool Equals(object obj)
        {
            if (obj is GeoLatLng)
            {
                GeoLatLng p2D = (GeoLatLng)obj;
                return (X == p2D.X) && (Y == p2D.Y);
            }
            return base.Equals(obj);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the distance, in meters, from this point to the given point. 
        /// By default, this distance is calculated given the default equatorial 
        /// earth radius of 6378137 meters. The earth is approximated as a sphere,
        /// hence the distance could be off as much as 0.3%,
        /// especially in the polar extremes. 
        /// </summary>
        /// <param name="pt1">the first point</param>
        /// <param name="pt2">the other point.</param>
        /// <returns>the distance, in kilo meters.</returns>
        public static double Distance(GeoLatLng pt1, GeoLatLng pt2)
        {
            GreateCircleCalculator cal = new GreateCircleCalculator(
                    GreateCircleCalculator.EarthModelWgs84,
                    GreateCircleCalculator.UnitKm);
            return cal.CalculateDistance(pt1, pt2);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the distance, in meters, from this point to the given point. 
        /// By default, this distance is calculated given the default equatorial 
        /// earth radius of 6378137 meters. The earth is approximated as a sphere,
        /// hence the distance could be off as much as 0.3%,
        /// especially in the polar extremes. 
        /// </summary>
        /// <param name="other">the other point</param>
        /// <returns>the distance, in Kilo meters.</returns>
        public double DistanceFrom(GeoLatLng other)
        {
            GreateCircleCalculator cal = new GreateCircleCalculator(
                    GreateCircleCalculator.EarthModelWgs84,
                    GreateCircleCalculator.UnitKm);
            return cal.CalculateDistance(this, other);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Calculate bearing
        /// </summary>
        /// <param name="pt1">first location.</param>
        /// <param name="pt2">2nd location.</param>
        /// <param name="pt3">3rd location.</param>
        /// <returns>the bearing angle (from 0 to 360).</returns>
        public static double GetBearing(GeoLatLng pt1, GeoLatLng pt2, GeoLatLng pt3)
        {
            double course1 = AzimuthTo(pt1, pt2);
            double course2 = AzimuthTo(pt2, pt3);
            double bearing = course2 - course1;
            if (bearing < 0) bearing += 360;
            return bearing;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Calculates the azimuth between the two points according to the ellipsoid
        /// model of WGS84. The azimuth is relative to true north. The Coordinates
        /// object on which this method is called is considered the origin for the
        /// calculation and the Coordinates object passed as a parameter is the
        /// destination which the azimuth is calculated to. When the origin is the
        /// North pole and the destination is not the North pole, this method returns
        /// 180.0. When the origin is the South pole and the destination is not the
        /// South pole, this method returns 0.0. If the origin is equal to the
        /// destination, this method returns 0.0. The implementation shall calculate
        /// the result as exactly as it can. However, it is required that the result
        /// is within 1 degree of the correct result.
        /// </summary>
        /// <param name="from">the Coordinates of the origin</param>
        /// <param name="to">the Coordinates of the destination</param>
        /// <returns>the azimuth to the destination in degrees. Result is within the
        ///         range [0.0 ,360.0).</returns>
        public static double AzimuthTo(GeoLatLng from, GeoLatLng to)
        {
            if (to == null || from == null)
            {
                throw new ArgumentException
                        ("AzimuthTo does not accept a null parameter.");
            }

            // Convert from degrees to radians.
            double lat1 = MathEx.ToRadians(from.Latitude);
            double lon1 = MathEx.ToRadians(from.Longitude);
            double lat2 = MathEx.ToRadians(to.Latitude);
            double lon2 = MathEx.ToRadians(to.Longitude);

            // Formula for computing the course between two points.
            // It is explained in detail here:
            //   http://williams.best.vwh.net/avform.htm
            //   http://www.movable-type.co.uk/scripts/LatLong.html
            // course = atan2(
            //            sin(lon2-lon1)*cos(lat2),
            //            cos(lat1)*sin(lat2)-sin(lat1)*cos(lat2)*cos(lon2-lon1))

            double deltaLon = lon2 - lon1;
            double cosLat2 = Math.Cos(lat2);
            double c1 = Math.Sin(deltaLon) * cosLat2;
            double c2 = Math.Cos(lat1) * Math.Sin(lat2) -
                    Math.Sin(lat1) * cosLat2 * Math.Cos(deltaLon);
            double courseInRadians = MathEx.Atan2(c1, c2);

            double course = MathEx.ToDegrees(courseInRadians);
            course = (360.0 + course) % 360.0;  // Normalize to [0,360)
            return course;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a string that represents this point in a format suitable 
        /// for use as a URL parameter value, separated by a comma, without 
        /// whitespace. By default, precision is returned to 6 digits, 
        /// which corresponds to a resolution to 4 inches/ 11 centimeters. 
        /// An optional precision parameter allows you to specify a lower 
        /// precision to reduce server load.
        /// </summary>
        /// <param name="precision">the precision of the output</param>
        /// <returns>string that represents this point</returns>
        public string ToUrlValue(int precision)
        {
            long multiple = 1;
            double lat = Y;
            double lng = X;
            if (precision < 0)
            {
                precision = 6;
            }
            for (int i = 0; i < precision; i++)
            {
                multiple *= 10;
            }
            lat = (((int)(lat * multiple + 0.5)) / ((double)multiple));
            lng = (((int)(lng * multiple + 0.5)) / ((double)multiple));
            return lat + "," + lng;
        }
    }

}
