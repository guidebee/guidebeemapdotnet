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
using System.IO;
using Mapdigit.Util;
using BinaryReader = System.IO.BinaryReader;

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
    /// Record index section of the map file.
    /// </summary>
    internal class RecordIndex : Section
    {

        /**
         * The type of the map object.
         */
        public byte MapObjectType;
        /**
         * the offset of the record.
         */
        public int RecordOffset;
        /**
         * the lenght of the record.
         */
        public int RecordLength;
        /**
         *  the MinX of the MBR.
         */
        public int MinX;
        /**
         *  the MinY of the MBR.
         */
        public int MinY;
        /**
         *  the MaxX of the MBR.
         */
        public int MaxX;
        /**
         *  the MaxY of the MBR.
         */
        public int MaxY;
        /**
         * Map object param1 (depends on map object type).
         */
        public int Param1;
        /**
         * Map object param2 (depends on map object type).
         */
        public int Param2;
        /**
         * Map object param3 (depends on map object type).
         */
        public int Param3;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordIndex"/> class.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// constructor.
        public RecordIndex(BinaryReader reader, long offset, long size)
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
        /// Get one record (index) of give mapInfo ID.
        /// </summary>
        /// <param name="mapInfoId">The map info ID.</param>
        public void GetRecord(int mapInfoId)
        {
            int recordId = mapInfoId - 1;
            if (mapInfoId < 1)
            {
                throw new IOException("MapInfo ID starts from 1");
            }
            _currentIndex = recordId;
            ReadOneRecord();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Get next record (index).
        /// </summary>
        public void MovePrevious()
        {
            _currentIndex--;
            ReadOneRecord();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///Get previous record (index).
        /// </summary>
        public void MoveNext()
        {
            _currentIndex++;
            ReadOneRecord();
        }

        /**
         * the size of the record index record.
         */
        private const int Recordsize = 37;
        /**
         * current index id.
         */
        private int _currentIndex;


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Reads the one record.
        /// </summary>
        private void ReadOneRecord()
        {
            DataReader.Seek(reader, offset + _currentIndex * Recordsize);
            MapObjectType = reader.ReadByte();
            RecordOffset = DataReader.ReadInt(reader);
            RecordLength = DataReader.ReadInt(reader);
            MinX = DataReader.ReadInt(reader);
            MinY = DataReader.ReadInt(reader);
            MaxX = DataReader.ReadInt(reader);
            MaxY = DataReader.ReadInt(reader);
            Param1 = DataReader.ReadInt(reader);
            Param2 = DataReader.ReadInt(reader);
            Param3 = DataReader.ReadInt(reader);
        }
    }

}
