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
    /// NEMA Data record for GPGSA sentence.
    /// </summary>
    public class NmeaGPGSADataRecord : NmeaDataRecord
    {

        /// <summary>
        /// Mode M or A.
        /// </summary>
        public bool ManualMode;

        /// <summary>
        /// Mode 1 -fix not avaiable, 2 2D ,3 3D.
        /// </summary>
        public int OperationMode;

        /// <summary>
        /// PRN numbers of saltellite used in solution.
        /// </summary>
        public int[] PrNs = new int[12];

        /// <summary>
        /// PDOP.
        /// </summary>
        public double Pdop;

        /// <summary>
        /// HDOP.
        /// </summary>
        public double Hdop;

        /// <summary>
        /// VDOP.
        /// </summary>
        public double Vdop;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="NmeaGPGSADataRecord"/> class.
        /// </summary>
        public NmeaGPGSADataRecord()
        {
            RecordType = TypeGPGSA;
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
            string ret = "Selection Mode:" + ManualMode + "\n" +
                    "Operation Mode:" + OperationMode + "\n" +
                    "HDOP:" + Hdop + "\n" +
                    "PDOP:" + Pdop + "\n" +
                    "VDOP:" + Vdop + "\n" +
                    "PRNs:";

            for (int i = 0; i < PrNs.Length; i++)
            {
                ret += PrNs[i] + ",";
            }
            ret += "\n";
            return ret;
        }
    }
}
