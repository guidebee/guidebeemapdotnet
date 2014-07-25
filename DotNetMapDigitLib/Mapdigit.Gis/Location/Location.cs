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
    /// The Location class represents the standard set of basic location information.
    /// </summary>
    /// <remarks>
    /// This includes the timestamped coordinates, accuracy, speed, course, and 
    /// information about the positioning method used for the location.
    /// </remarks>
    public class Location
    {
        /// <summary>
        /// Time stamp.
        /// </summary>
        public DateTime TimeStamp;

        /// <summary>
        ///  Latitude.
        /// </summary>
        public double Latitude;

        /// <summary>
        /// Longitude
        /// </summary>
        public double Longitude;

        /// <summary>
        /// Altitude
        /// </summary>
        public double Altitude;

        /// <summary>
        /// Returns the terminal's current ground speed in meters per second (m/s) 
        /// at the time of measurement. The speed is always a non-negative value. 
        /// </summary>
        /// <remarks>
        /// Remember that unlike the coordinates, speed does not have an associated 
        /// accuracy because the methods used to determine the speed typically are 
        /// not able to indicate the accuracy. 
        /// </remarks>
        public double Speed;

        /// <summary>
        /// course over ground, degree true.
        /// </summary>
        public double Course;

        /// <summary>
        /// PDOP
        /// </summary>
        public double Pdop;

        /// <summary>
        /// HDOP
        /// </summary>
        public double Hdop;

        /// <summary>
        /// VDOP
        /// </summary>
        public double Vdop;

        /// <summary>
        /// Status
        /// </summary>
        public bool Status;



        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the coordinates of this location and their accuracy. 
        /// </summary>
        public QualifiedCoordinates QualifiedCoordinates
        {
            get
            {
                QualifiedCoordinates qualifiedCoordinates =
                    new QualifiedCoordinates(Latitude,
                                             Longitude, Altitude, Hdop, Vdop);
                return qualifiedCoordinates;
            }
        }

        
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the timestamp at which the data was collected. This timestamp 
        /// should represent the point in time when the measurements were made. 
        /// </summary>
        /// <remarks>
        /// Implementations make best effort to set the timestamp as close to this 
        /// point in time as possible. The time returned is the time of the local 
        /// clock in the terminal in milliseconds using the same clock and same time 
        /// representation as System.currentTimeMillis(). 
        /// </remarks>
        /// <returns>a timestamp representing the time.</returns>
        public long TotalMillisecond
        {
            get { return (long) (TimeStamp - _startTime).TotalMilliseconds; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns whether this Location instance represents a valid location with 
        /// coordinates or an invalid one where all the data, especially the latitude
        /// and longitude coordinates, may not be present.
        /// </summary>
        /// <returns>
        /// 	a boolean value with true indicating that this Location instance 
        /// is valid.
        /// </returns>
        public bool IsValid()
        {
            return Status;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Copy from anthoer location object
        /// </summary>
        /// <param name="location">location object to copy from.</param>
        public void Copy(Location location)
        {
            Hdop = location.Hdop;
            Pdop = location.Pdop;
            Vdop = location.Vdop;
            Altitude = location.Altitude;
            Course = location.Course;
            Latitude = location.Latitude;
            Longitude = location.Longitude;
            Speed = location.Speed;
            Status = location.Status;
            TimeStamp = location.TimeStamp;

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>
        /// A string that represents this instance.
        /// </returns>
        public override string ToString()
        {
            string strRet = "";
            strRet += "Latitude=" + Latitude + "\n";
            strRet += "Longitude=" + Longitude + "\n";
            strRet += "Altitude=" + Altitude + "\n";
            strRet += "Speed=" + Speed + "\n";
            strRet += "Course=" + Course + "\n";
            strRet += "Datetime=" + TimeStamp + "\n";
            return strRet;
        }

        private readonly DateTime _startTime = new DateTime(1971, 1, 1);
    }
}
