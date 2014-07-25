//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 29SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Gis.Location.Nmea
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 29SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// NEMA Data record for GPRMC sentence.
    /// </summary>
    public class NmeaGPRMCDataRecord : NmeaDataRecord
    {

        /// <summary>
        /// Time stamp.
        /// </summary>
        public DateTime TimeStamp;

        /// <summary>
        /// Status.
        /// </summary>
        public bool Status;

        /// <summary>
        ///  Latitude
        /// </summary>
        public double Latitude;

        /// <summary>
        /// Longitude.
        /// </summary>
        public double Longitude;

        /// <summary>
        /// speed over ground
        /// </summary>
        public double Speed;

        /// <summary>
        /// course over ground, degree true
        /// </summary>
        public double Course;

        /// <summary>
        /// Magnetic variation
        /// </summary>
        public double MagneticCourse;


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="NmeaGPRMCDataRecord"/> class.
        /// </summary>
        public NmeaGPRMCDataRecord()
        {
            RecordType = TypeGPRMC;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "Time:" + TimeStamp + "\n" +
                    "Latitude:" + Latitude + "\n" +
                    "Longitude:" + Longitude + "\n" +
                    "Course:" + Course + "\n" +
                    "Magnetic Course:" + MagneticCourse + "\n" +
                    "Speed:" + Speed + "\n" +
                    "status:" + Status + "\n";

        }
    }


}
