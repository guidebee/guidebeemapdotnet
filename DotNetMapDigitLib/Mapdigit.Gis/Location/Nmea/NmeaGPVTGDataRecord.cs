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
    /// NEMA Data record for GPVTG sentence
    /// </summary>
    public class NmeaGPVTGDataRecord : NmeaDataRecord
    {

        /// <summary>
        /// Course ,true .
        /// </summary>
        public double Course;

        /// <summary>
        /// course, magnetic.
        /// </summary>
        public double CourseMagnetic;

        /// <summary>
        /// speed in knot.
        /// </summary>
        public double SpeedKnot;

        /// <summary>
        /// speed in km.
        /// </summary>
        public double SpeedKm;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="NmeaGPVTGDataRecord"/> class.
        /// </summary>
        public NmeaGPVTGDataRecord()
        {
            RecordType = TypeGPVTG;
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
            return "Course:" + Course + "\n" +
                   "Magnetic Course:" + CourseMagnetic + "\n" +
                   "SpeedKm" + SpeedKm + "\n" +
                   "SpeedKnot:" + SpeedKnot + "\n";
        }
    }
}
