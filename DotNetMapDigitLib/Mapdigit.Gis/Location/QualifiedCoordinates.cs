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
using System;

//--------------------------------- PACKAGE ------------------------------------
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
    /// The QualifiedCoordinates class represents coordinates as
    /// latitude-longitude-altitude values that are associated with an accuracy
    /// value.
    /// </summary>
    public class QualifiedCoordinates : Coordinates
    {
        /**
         * The horizontal accuracy of this location result in meters. 
         * Double.Nan can be used to indicate that the accuracy is not 
         * known. Must be greateror equal to 0.
         */
        private double _horizontalAccuracy;

        /**
         * The vertical accuracy of this location result in meters. 
         * Double.Nan can be used to indicate that the accuracy is not
         * known. Must be greater or equal to 0
         */
        private double _verticalAccuracy;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a new QualifiedCoordinates object with the values
        /// specified. The latitude and longitude parameters are expressed in degrees
        /// using floating point values. The degrees are in decimal values (rather
        /// than minutes/seconds).
        /// </summary>
        /// <remarks>
        /// The coordinate values always apply to the WGS84 datum.
        /// 
        /// The Double.Nan value can be used for altitude to indicate 
        /// that altitude is not known.
        /// </remarks>
        /// <param name="latitude">the latitude of the location. Valid range: [-90.0,
        ///        90.0]. Positive values indicate northern latitude and negative
        ///        values southern latitude.</param>
        /// <param name="longitude">the longitude of the location. Valid range: [-180.0,
        ///        180.0). Positive values indicate eastern longitude and negative
        ///        values western longitude</param>
        /// <param name="altitude">the altitude of the location in meters, defined as
        ///        height above WGS84 ellipsoid. Double.Nan can be used 
        ///        to indicate that altitude is not known.</param>
        /// <param name="horizontalAccuracy">the horizontal accuracy of this location
        ///        result in meters. Double.Nan can be used to indicate 
        ///        that the accuracy is not known. Must be greater or equal to 0.</param>
        /// <param name="verticalAccuracy">the vertical accuracy of this location result
        ///        in meters. Double.Nan can be used to indicate that the
        ///        accuracy is not known. Must be greater or equal to 0.</param>
        public QualifiedCoordinates(double latitude, double longitude,
                double altitude, double horizontalAccuracy, double verticalAccuracy)
            : base(latitude, longitude, altitude)
        {

            HorizontalAccuracy = horizontalAccuracy;
            VerticalAccuracy = verticalAccuracy;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the horizontal accuracy of the location in meters (1-sigma
        /// standard deviation). 
        /// </summary>
        /// <remarks>
        /// A value of Double.Nan means the 
        /// horizontal accuracy could not be determined.
        /// 
        /// The horizontal accuracy is the RMS (root mean square) of east accuracy
        /// (latitudinal error in meters, 1-sigma standard deviation), north accuracy
        /// (longitudinal error in meters, 1-sigma).
        /// </remarks>
        public double HorizontalAccuracy
        {
            get { return _horizontalAccuracy; }
            set
            {
                if (Double.IsNaN(value) || (value >= 0.0F))
                {
                    _horizontalAccuracy = value;
                }
                else
                {
                    throw new ArgumentException("Horizontal accuracy " +
                                                "(" + value + ") is invalid.");
                }
            }
        }

      
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// the accuracy of the location in meters in vertical direction
        /// (orthogonal to ellipsoid surface, 1-sigma standard deviation). A value of
        /// Double.Nan means the vertical accuracy could not be 
        /// determined.
        /// </summary>
        public double VerticalAccuracy
        {
            get { return _verticalAccuracy; }
            set
            {
                if (Double.IsNaN(value) || (value >= 0.0F))
                {
                    _verticalAccuracy = value;
                }
                else
                {
                    throw new ArgumentException
                        ("Vertical accuracy (" + value + ") is invalid.");
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Provides a string representation of the qualified coordinates.
        /// </summary>
        /// <returns>
        /// A string such as "79.32N 169.8E 25.7m ?.8mH ?.7mV" where the
        ///         words are the latitude, longitude, altitude (in meters),
        ///         horizontal accuracy (in meters), and vertical accuracy (in
        ///         meters).
        /// </returns>
        public override string ToString()
        {
            // Get the regular coordinates as a string.
            string s = base.ToString();

            // Append the horizontal accuracy.
            if (Double.IsNaN(_horizontalAccuracy) == false)
            {
                s += "? " + _horizontalAccuracy + "mH";
            }

            // Append the vertical accuracy.
            if (Double.IsNaN(_verticalAccuracy) == false)
            {
                s += "? " + _verticalAccuracy + "mV";
            }

            return s;
        }
    }
}
