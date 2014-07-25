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
using Mapdigit.Gis.Vector.MapFile;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Gis.Vector
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 02OCT2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Define one database table.
    /// </summary>
    public sealed class DataTable
    {

        /// <summary>
        /// Data table definition.
        /// </summary>
        public DataField[] Fields;


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="DataTable"/> class.
        /// </summary>
        /// <param name="tabularData">The tabular data.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="recordCount">The record count.</param>
        internal DataTable(TabularData tabularData, DataField[] fields, int recordCount)
        {
            _tabularData = tabularData;
            Fields = fields;
            _recordCount = recordCount;
            _currentIndex = 1;
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
        /// <returns></returns>
        public DataRowValue GetRecord(int mapInfoId)
        {
            _currentIndex = mapInfoId;
            return _tabularData.GetRecord(mapInfoId);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the record count.
        /// </summary>
        /// <returns></returns>
        public int GetRecordCount()
        {
            return _recordCount;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Moves to the first record.
        /// </summary>
        /// <returns></returns>
        public DataRowValue MoveFirst()
        {
            _currentIndex = 1;
            return GetRecord(_currentIndex);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Moves to the last record.
        /// </summary>
        /// <returns></returns>
        public DataRowValue MoveLast()
        {
            _currentIndex = _recordCount;
            return GetRecord(_currentIndex);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Moves to the previous record.
        /// </summary>
        /// <returns></returns>
        public DataRowValue MovePrevious()
        {
            _currentIndex--;
            if (_currentIndex == 0)
            {
                throw new IOException("Passed the first record!");
            }
            return ReadOneRecord();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Moves to the next record.
        /// </summary>
        /// <returns></returns>
        public DataRowValue MoveNext()
        {
            _currentIndex++;
            if (_currentIndex > _recordCount)
            {
                throw new IOException("Passed the last the record!");
            }
            return ReadOneRecord();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the column index of given column name.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>the index of the column(field) or -1 if not found.</returns>
        public int GetFieldIndex(string fieldName)
        {
            int ret = -1;
            for (int i = 0; i < Fields.Length; i++)
            {
                if (Fields[i].GetName().ToLower().Equals(fieldName.ToLower()))
                {
                    ret = i;
                    break;
                }
            }
            return ret;
        }

        /**
         * current index id.
         */
        private int _currentIndex;
        /**
         * Tabular data
         */
        private readonly TabularData _tabularData;
        /**
         * total record Count;
         */
        private readonly int _recordCount;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Reads the one record.
        /// </summary>
        /// <returns></returns>
        private DataRowValue ReadOneRecord()
        {
            return GetRecord(_currentIndex);
        }
    }

}
