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
    /// tabular data section of the map file.
    /// </summary>
    internal sealed class TabularData : Section
    {

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="TabularData"/> class.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="size">The size.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="stringData">The string data.</param>
        /// <param name="stringIndex">Index of the string.</param>
        public TabularData(BinaryReader reader, long offset, long size,
                DataField[] fields, StringData stringData, StringIndex stringIndex)
            : base(reader, offset, size)
        {
            _fields = fields;
            _stringData = stringData;
            _stringIndex = stringIndex;
            int numberOfField = fields.Length;
            _recordSize = 0;
            for (int i = 0; i < numberOfField; i++)
            {
                switch (fields[i].GetFieldType())
                {
                    case DataField.TypeChar://char
                        _recordSize += 4;
                        break;
                    case DataField.TypeInteger://int
                        _recordSize += 4;
                        break;
                    case DataField.TypeSmallint://shot
                        _recordSize += 2;
                        break;
                    case DataField.TypeFloat://float
                        _recordSize += 8;
                        break;
                    case DataField.TypeDecimal://float
                        _recordSize += 8;
                        break;
                    case DataField.TypeDate://date
                        _recordSize += 4;
                        break;
                    case DataField.TypeLogical://bool
                        _recordSize += 1;
                        break;
                }
            }
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
            int recordId = mapInfoId - 1;
            if (mapInfoId < 1)
            {
                throw new IOException("MapInfo ID starts from 1");
            }
            DataReader.Seek(reader, offset + recordId * _recordSize);
            string[] fieldValues = new string[_fields.Length];

            int intValue;
            int shortValue;
            int stringId;
            byte boolValue;
            double doubleValue;
            for (int i = 0; i < fieldValues.Length; i++)
            {
                switch (_fields[i].GetFieldType())
                {
                    case DataField.TypeChar://char
                    case DataField.TypeDate://date
                        stringId = DataReader.ReadInt(reader);
                        fieldValues[i] = stringId.ToString();
                        break;
                    case DataField.TypeInteger://int
                        intValue = DataReader.ReadInt(reader);
                        fieldValues[i] = intValue.ToString();
                        break;
                    case DataField.TypeSmallint://short
                        shortValue = DataReader.ReadShort(reader);
                        fieldValues[i] = shortValue.ToString();
                        break;
                    case DataField.TypeDecimal://short
                    case DataField.TypeFloat://float
                        doubleValue = DataReader.ReadDouble(reader);
                        fieldValues[i] = doubleValue.ToString();
                        break;
                    case DataField.TypeLogical://bool
                        boolValue = reader.ReadByte();
                        fieldValues[i] = boolValue.ToString();
                        break;
                }
            }

            //read string data.
            for (int i = 0; i < fieldValues.Length; i++)
            {
                switch (_fields[i].GetFieldType())
                {
                    case DataField.TypeChar://char
                    case DataField.TypeDate://date
                        stringId = int.Parse(fieldValues[i]);
                        if (stringId != -1)
                        {
                            _stringIndex.GetRecord(stringId);
                            fieldValues[i] = _stringData.GetRecord(_stringIndex.RecordOffset);
                        }
                        else
                        {
                            fieldValues[i] = "";
                        }
                        break;
                }
            }
            var ret = new DataRowValue(fieldValues);
            return ret;
        }

        /**
         * table field defintion.
         */
        private readonly DataField[] _fields;
        /**
         * the lenght of one record.
         */
        private readonly int _recordSize;
        /**
         * string data section object.
         */
        private readonly StringData _stringData;
        /**
         * string index section object.
         */
        private readonly StringIndex _stringIndex;
    }
}

