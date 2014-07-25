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
    /// NEMA Data record for each NMEA sentence.
    /// </summary>
    public abstract class NmeaDataRecord
    {
        /// <summary>
        /// None type
        /// </summary>
        public const short TypeNone = 0;

        /// <summary>
        /// Type GPRMC.
        /// </summary>
        public const short TypeGPRMC = 1;

        /// <summary>
        /// Type GPGGA.
        /// </summary>
        public const short TypeGPGGA = 2;

        /// <summary>
        /// Type GPGSA.
        /// </summary>
        public const short TypeGPGSA = 4;

        /// <summary>
        /// Type GPRMC.
        /// </summary>
        public const short TypeGPGSV = 8;

        /// <summary>
        /// Type GPGGA.
        /// </summary>
        public const short TypeGPGLL = 16;

        /// <summary>
        /// Type GPGSA.
        /// </summary>
        public const short TypeGPVTG = 32;

        /// <summary>
        /// TYPE_GPRMC | TYPE_GPGGA | TypeGPGSA |
        /// TYPE_GPGSV | TYPE_GPGLL | TYPE_GPVTG
        /// </summary>
        public const short AllTypesMask = 63;

        /// <summary>
        /// record Type.
        /// </summary>
        public short RecordType = TypeNone;
    }


}
