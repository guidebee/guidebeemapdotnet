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
    /// NEMA Data record for GPGGA sentence.
    /// </summary>
    public class NmeaGPGGADataRecord : NmeaDataRecord
    {

        /// <summary>
        /// Time stamp.
        /// </summary>
        public DateTime TimeStamp;

        /// <summary>
        /// Latitude.
        /// </summary>
        public double Latitude;

        /// <summary>
        /// Longitude.
        /// </summary>
        public double Longitude;

        /// <summary>
        /// Receiver mode.
        /// </summary>
        public int ReceiverMode;

        /// <summary>
        /// number Of Satellites used in fix.
        /// </summary>
        public int NumberOfSatellites;

        /// <summary>
        /// Horizontal dilution of precision.
        /// </summary>
        public double Hdop;

        /// <summary>
        /// Altitude.
        /// </summary>
        public double Altitude;


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="NmeaGPGGADataRecord"/> class.
        /// </summary>
        public NmeaGPGGADataRecord()
        {
            RecordType = TypeGPGGA;
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
                    "Quality indicator:" + ReceiverMode + "\n" +
                    "Number of Satellite:" + NumberOfSatellites + "\n" +
                    "HDOP:" + Hdop + "\n";
        }
    }
}
