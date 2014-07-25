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
    /// Defines a filed of a database table.
    /// </summary>
    public sealed class DataField
    {

        /// <summary>
        /// Data type is char(string).
        /// </summary>
        public const byte TypeChar = 1;
        /// <summary>
        /// Data type is integer(4 bytes).
        /// </summary>
        public const byte TypeInteger = 2;
        /// <summary>
        /// Data type is short (2 bytes).
        /// </summary>
        public const byte TypeSmallint = 3;
        /// <summary>
        /// Data type is double.
        /// </summary>
        public const byte TypeDecimal = 4;
        /// <summary>
        /// Data type is float.
        /// </summary>
        public const byte TypeFloat = 5;
        /// <summary>
        /// Data type is date.
        /// </summary>
        public const byte TypeDate = 6;
        /// <summary>
        /// Data type is boolean.
        /// </summary>
        public const byte TypeLogical = 7;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="DataField"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="width">The width.</param>
        /// <param name="precision">The precision.</param>
        public DataField(string name, byte type, int width, short precision)
        {
            _fieldName = name;
            _fieldType = type;
            _fieldWidth = width;
            _fieldPrecision = precision;
            if (_fieldType == 0)
            {
                _fieldType = TypeChar;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return _fieldName;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the type of the field.
        /// </summary>
        /// <returns></returns>
        public byte GetFieldType()
        {
            return _fieldType;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <returns></returns>
        public int GetWidth()
        {
            return _fieldWidth;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the precision.
        /// </summary>
        /// <returns></returns>
        public short GetPrecision()
        {
            return _fieldPrecision;
        }

        /**
         * the name of the field.
         */
        private readonly string _fieldName;
        /**
         * the type of the field.
         */
        private readonly byte _fieldType;
        /**
         * the Width of the fields.
         */
        private readonly int _fieldWidth;
        /**
         * the precision of the field.
         */
        private readonly short _fieldPrecision;
    }

}
