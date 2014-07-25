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
    /// string index section of the map file.
    /// </summary>
    internal class StringIndex : Section
    {

        /**
         * the offset of the record.
         */
        public int RecordOffset;
        /**
         * the lenght of the record.
         */
        public int RecordLength;
        /**
         * the size of the record index record.
         */
        public const int RECORDSIZE = 6;

        /**
         * total record count;
         */
        public int RecordCount;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="StringIndex"/> class.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="size">The size.</param>
        public StringIndex(BinaryReader reader, long offset, long size)
            : base(reader, offset, size)
        {

            RecordCount = (int)(size / RECORDSIZE);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///Get one record (index) of give record ID.
        /// </summary>
        /// <param name="recordId">The record ID.</param>
        public void GetRecord(int recordId)
        {
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
        /// Get previous record (index).
        /// </summary>
        /// <returns></returns>
        public bool MovePrevious()
        {
            bool ret = false;
            _currentIndex--;
            if (_currentIndex < 0)
            {
                ret = true;
            }
            else
            {
                ReadOneRecord();
            }
            return ret;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///Get next record (index).
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            bool ret = false;
            _currentIndex++;
            if (_currentIndex >= RecordCount)
            {
                ret = true;
            }
            else
            {
                ReadOneRecord();
            }
            return ret;
        }

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
            DataReader.Seek(reader, offset + _currentIndex * RECORDSIZE);
            RecordOffset = DataReader.ReadInt(reader);
            RecordLength = DataReader.ReadInt(reader);
        }
    }

}
