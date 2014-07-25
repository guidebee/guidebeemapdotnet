//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 02OCT2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using BinaryReader = System.IO.BinaryReader;
using Mapdigit.Util;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Gis.Vector.MapFile
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 02OCT2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// string data section of the map file.
    /// </summary>
    internal class StringData : Section
    {

        public int FieldCount;
        public int[] MapInfoId;
        public int[] FieldId;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="StringData"/> class.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="size">The size.</param>
        public StringData(BinaryReader reader, long offset, long size)
            : base(reader, offset, size)
        {

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the record.
        /// </summary>
        /// <param name="off">The offset.</param>
        /// <returns></returns>
        public string GetRecord(long off)
        {
            DataReader.Seek(reader, off);
            string ret = DataReader.ReadString(reader);
            return ret;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get one string value.
        /// </summary>
        /// <param name="off">The offset.</param>
        /// <returns></returns>
        public string GetMapInfoIdAndField(long off)
        {
            DataReader.Seek(reader, off);
            string ret = DataReader.ReadString(reader);
            FieldCount = DataReader.ReadShort(reader);
            MapInfoId = new int[FieldCount];
            FieldId = new int[FieldCount];
            for (int i = 0; i < FieldCount; i++)
            {
                MapInfoId[i] = DataReader.ReadInt(reader);
                FieldId[i] = reader.ReadByte();
            }
            return ret;
        }
    }

}
