//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 28SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------

//--------------------------------- PACKAGE ------------------------------------
using System;
using Mapdigit.Util;

namespace Mapdigit.Gis.Location
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 28SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// The Coordinates class represents coordinates as
    /// latitude-longitude-altitude values. 
    /// </summary>
    /// <remarks>
    /// The latitude and longitude values are
    /// expressed in degrees using floating point values. The degrees are in decimal
    /// values (rather than minutes/seconds). The coordinates are given using the
    /// WGS84 datum.
    /// 
    /// This class also provides convenience methods for converting between a string
    /// coordinate representation and the double representation used in this
    /// class.
    /// </remarks>
    public class Coordinates
    {

        /// <summary>
        /// Identifier for string coordinate representation Degrees, Minutes, Seconds
        /// and decimal fractions of a second.
        /// </summary>
        public const int DdMmSs = 1;

        /// <summary>
        /// 
        /// Identifier for string coordinate representation Degrees, Minutes, decimal
        /// fractions of a minute.
        /// </summary>
        public const int DdMm = 2;


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a new Coordinates object with the values 
        /// specified.
        /// </summary>
        /// <remarks>
        /// The latitude and longitude parameters are expressed in degrees using
        /// floating point values. The degrees are in decimal values (rather than
        /// minutes/seconds).
        /// 
        /// The coordinate values always apply to the WGS84 datum.
        /// 
        /// The Double.NaN value can be used for altitude to indicate 
        /// that altitude is not known.
        ///</remarks>
        /// <param name="latitude">the latitude of the location. Valid range: [-90.0,
        ///        90.0]. Positive values indicate northern latitude and negative
        ///        values southern latitude.</param>
        /// <param name="longitude">the longitude of the location. Valid range: [-180.0,
        ///        180.0). Positive values indicate eastern longitude and negative
        ///        values western longitude.</param>
        /// <param name="altitude">the altitude of the location in meters, defined as
        ///        height above WGS84 ellipsoid. Double.Nan can be used 
        ///        to indicate that altitude is not known.</param>
        public Coordinates(double latitude, double longitude, double altitude)
        {
            Latitude = latitude;
            Longitude = longitude;
            Altitude = altitude;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// the latitude component of this coordinate. Positive values
        /// indicate northern latitude and negative values southern latitude.
        /// The latitude is given in WGS84 datum.
        /// </summary>
        public double Latitude
        {
            get { return _latitude; }
            set
            {
                if (double.IsNaN(value) || (value < -90.0 || value >= 90.0))
                {
                    throw new ArgumentException
                        ("Latitude (" + value + ") is invalid.");
                }
                _latitude = value;
            }
        }

        
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the geodetic altitude for this point.
        /// </summary>
        /// <value>the altitude of the location in meters, defined as
        ///        height above the WGS84 ellipsoid. 0.0 means a location at the
        ///        ellipsoid surface, negative values mean the location is below the
        ///        ellipsoid surface, Double.Nan that no altitude is not
        ///        available</value>
        public double Altitude
        {
            set { _altitude = value; }
            get { return _altitude; }
        }

        
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the geodetic longitude for this point. Longitude is given as a
        ///  double expressing the longitude in degrees in the WGS84 datum.
        /// </summary>
        /// <value>the longitude of the location in degrees. Valid range:
        ///         [-180.0, 180.0)</value>
        public double Longitude
        {
            set
            {
                if (double.IsNaN(value) || (value < -180.0
                                            || value >= 180.0))
                {
                    throw new ArgumentException
                        ("Longitude (" + value + ") is invalid.");
                }
                _longitude = value;
            }
            get { return _longitude; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Calculates the azimuth between the two points according to the ellipsoid
        /// model of WGS84. The azimuth is relative to true north.
        /// </summary>
        /// <remarks>
        ///  The Coordinates object on which this method is called is considered the 
        /// origin for the calculation and the Coordinates object passed as a parameter is the
        /// destination which the azimuth is calculated to. When the origin is the
        /// North pole and the destination is not the North pole, this method returns
        /// 180.0. When the origin is the South pole and the destination is not the
        /// South pole, this method returns 0.0. If the origin is equal to the
        /// destination, this method returns 0.0. The implementation shall calculate
        /// the result as exactly as it can. However, it is required that the result
        /// is within 1 degree of the correct result.
        /// </remarks>
        /// <param name="to">the Coordinates of the destination</param>
        /// <returns>the azimuth to the destination in degrees. Result is within the
        ///          range [0.0 ,360.0).</returns>
        public double AzimuthTo(Coordinates to)
        {
            if (to == null)
            {
                throw new ArgumentException
                        ("azimuthTo does not accept a null parameter.");
            }

            // Convert from degrees to radians.
            double lat1 = MathEx.ToRadians(_latitude);
            double lon1 = MathEx.ToRadians(_longitude);
            double lat2 = MathEx.ToRadians(to._latitude);
            double lon2 = MathEx.ToRadians(to._longitude);

            // Formula for computing the course between two points.
            // It is explained in detail here:
            //   http://williams.best.vwh.net/avform.htm
            //   http://www.movable-type.co.uk/scripts/LatLong.html
            // course = atan2(
            //            sin(lon2-lon1)*cos(lat2),                               
            //            cos(lat1)*sin(lat2)-sin(lat1)*cos(lat2)*cos(lon2-lon1)) 

            double deltaLon = lon2 - lon1;
            double cosLat2 = MathEx.Cos(lat2);
            double c1 = MathEx.Sin(deltaLon) * cosLat2;
            double c2 = MathEx.Cos(lat1) * MathEx.Sin(lat2) -
                    MathEx.Sin(lat1) * cosLat2 * MathEx.Cos(deltaLon);
            double courseInRadians = MathEx.Atan2(c1, c2);

            double course = MathEx.ToDegrees(courseInRadians);
            course = (360.0 + course) % 360.0;  // Normalize to [0,360)
            return course;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// /// Calculates the geodetic distance between the two points according to the
        /// ellipsoid model of WGS84. Altitude is neglected from calculations.
        /// </summary>
        /// <remarks>
        /// The implementation shall calculate this as exactly as it can. However, it
        /// is required that the result is within 0.35% of the correct result.
        /// </remarks>
        /// <param name="to"> the Coordinates of the destination</param>
        /// <returns>the distance to the destination in meters</returns>
        public double Distance(Coordinates to)
        {
            if (to == null)
            {
                throw new ArgumentException
                        ("distance does not accept a null parameter.");
            }

            // Convert from degrees to radians.
            double lat1 = MathEx.ToRadians(_latitude);
            double lon1 = MathEx.ToRadians(_longitude);
            double lat2 = MathEx.ToRadians(to._latitude);
            double lon2 = MathEx.ToRadians(to._longitude);

            // Use the Haversine formula for greater accuracy when measuring
            // short distances.  It is explained in detail here:
            //   http://williams.best.vwh.net/avform.htm
            //   http://www.movable-type.co.uk/scripts/LatLong.html
            // d = 2*asin(sqrt(
            //          (sin((lat1-lat2)/2))^2 +    // d2
            //          cos(lat1)*cos(lat2) *       // d3
            //          (sin((lon1-lon2)/2))^2) )   // d5

            double d1 = MathEx.Sin((lat1 - lat2) / 2.0);
            double d2 = d1 * d1;
            double d3 = MathEx.Cos(lat1) * MathEx.Cos(lat2);
            double d4 = MathEx.Sin((lon1 - lon2) / 2.0);
            double d5 = d4 * d4;
            double d6 = d2 + d3 * d5;
            double distanceInRadians = 2.0 * MathEx.Asin(Math.Sqrt(d6));

            double distance = MetersPerRadian * distanceInRadians;
            return distance;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
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
            long hash = 7;
            hash = 53 * hash + Utils.DoubleToInt64Bits(_altitude)
                    ^ (Utils.DoubleToInt64Bits(_altitude) >> 32);
            hash = 53 * hash + (int)(Utils.DoubleToInt64Bits(_latitude)
                    ^ (Utils.DoubleToInt64Bits(_latitude) >> 32));
            hash = 53 * hash + (int)(Utils.DoubleToInt64Bits(_longitude)
                    ^ (Utils.DoubleToInt64Bits(_longitude) >> 32));
            return (int)hash;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to 
        /// this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object other)
        {
            // This is an allowable difference to account for floating point 
            //imprecisions.
            const double tolerance = 0.000001;

            if (other == null)
            {
                return false;
            }

            if ((other is Coordinates) == false)
            {
                return false;
            }

            // Otherwise it is a Coordinates object.
            Coordinates c = (Coordinates)other;

            if ((_latitude < c._latitude - tolerance) ||
                    (_latitude > c._latitude + tolerance))
            {
                return false;
            }

            if ((_longitude < c._longitude - tolerance) ||
                    (_longitude > c._longitude + tolerance))
            {
                return false;
            }

            if (double.IsNaN(_altitude) &&
                    (double.IsNaN(c._altitude) == false))
            {
                return false;
            }
            if ((double.IsNaN(_altitude) == false) &&
                    double.IsNaN(c._altitude))
            {
                return false;
            }
            if (double.IsNaN(_altitude) && double.IsNaN(c._altitude))
            {
                return true;
            }
            if ((_altitude < c._altitude - tolerance) ||
                    (_altitude > c._altitude + tolerance))
            {
                return false;
            }

            // If we got here the two coordinates are equal.
            return true;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            string s;

            // Add the latitude.
            if (_latitude >= 0.0)
            {
                s = _latitude.ToString();
                s += "N ";
            }
            else
            {
                s = (-1 * _latitude).ToString();
                s += "S ";
            }

            // Add the longitude.
            if (_longitude >= 0.0)
            {
                s += _longitude.ToString();
                s += "E ";
            }
            else
            {
                s += (-1 * _longitude).ToString();
                s += "W ";
            }

            // Add the altitude.
            if (double.IsNaN(_altitude) == false)
            {
                s += (" " + _altitude + "m");
            }

            return s;
        }

        /**
         * This is the earth's mean radius in meters.  Using the mean gives the most
         * accurate results for distances measured with any bearing.
         * 
         * In truth the earth is not a perfect sphere.  The radius of the equator
         * is 6,378,137 and the polar radius is 6,356,752.3142.  The FAI's
         * definition of 6,371,000 lies between them.
         */
        private const double MetersPerRadian = 6371000;


        /**
         * The altitude of the location in meters, defined as height above WGS84
         * ellipsoid. double.Nan can be used to indicate that altitude
         * is not
         * known.
         */
        private double _altitude;

        /**
         * The latitude of the location. Valid range: [-90.0, 90.0]. Positive values
         * indicate northern latitude and negative values southern latitude.
         */
        private double _latitude;

        /**
         * The longitude of the location. Valid range: [-180.0, 180.0). Positive
         * values indicate eastern longitude and negative values western longitude.
         */
        private double _longitude;
    }
}
