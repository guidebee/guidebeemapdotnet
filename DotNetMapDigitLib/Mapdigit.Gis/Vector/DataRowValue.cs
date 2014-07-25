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
using System;

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
    /// Defines a row of a database table.
    /// </summary>
    public sealed class DataRowValue
    {

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="DataRowValue"/> class.
        /// </summary>
        /// <param name="fieldValues">The field values.</param>
        public DataRowValue(string[] fieldValues)
        {
            _fieldValues = fieldValues;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the value of the specified column as a String.
        /// </summary>
        /// <param name="ordinal"> The zero-based column ordinal.</param>
        /// <returns>The value of the specified column as a String</returns>
        public string GetString(int ordinal)
        {
            if (ordinal >= 0 && ordinal < _fieldValues.Length)
            {
                return _fieldValues[ordinal];
            }
            return null;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the value of the specified column as a integer.
        /// </summary>
        /// <param name="ordinal">The zero-based column ordinal</param>
        /// <returns>The value of the specified column as a integer</returns>
        public int GetInt(int ordinal)
        {
            try
            {
                if (ordinal >= 0 && ordinal < _fieldValues.Length)
                {
                    return int.Parse(_fieldValues[ordinal]);
                }
            }
            catch (Exception)
            {
                //ingore the exception.
            }

            return 0;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the value of the specified column as a short.
        /// </summary>
        /// <param name="ordinal"> The zero-based column ordinal..</param>
        /// <returns>The value of the specified column as a short</returns>
        public short GetShort(int ordinal)
        {
            try
            {
                if (ordinal >= 0 && ordinal < _fieldValues.Length)
                {
                    return short.Parse(_fieldValues[ordinal]);
                }
            }
            catch (Exception)
            {
                //ingore the exception.
            }

            return 0;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the value of the specified column as a double.
        /// </summary>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <returns>The value of the specified column as a double</returns>
        public double GetDouble(int ordinal)
        {
            try
            {
                if (ordinal >= 0 && ordinal < _fieldValues.Length)
                {
                    return double.Parse(_fieldValues[ordinal]);
                }
            }
            catch (Exception)
            {
                //ingore the exception.
            }

            return 0.0;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the value of the specified column as a string(Date).
        /// </summary>
        /// <param name="ordinal">The zero-based column ordinal..</param>
        /// <returns>The value of the specified column as a string</returns>
        public string GetDate(int ordinal)
        {
            try
            {
                if (ordinal >= 0 && ordinal < _fieldValues.Length)
                {
                    return _fieldValues[ordinal];
                }
            }
            catch (Exception)
            {
                //ingore the exception.
            }

            return null;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the boolean.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns></returns>
        public bool GetBoolean(int ordinal)
        {
            try
            {
                if (ordinal >= 0 && ordinal < _fieldValues.Length)
                {
                    if (byte.Parse(_fieldValues[ordinal]) != 0)
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                //ingore the exception.
            }

            return false;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            string retStr = "";
            for (int i = 0; i < _fieldValues.Length - 1; i++)
            {
                if (IsNumber(_fieldValues[i]))
                {
                    retStr += _fieldValues[i] + ",";
                }
                else
                {
                    retStr += "\"" + _fieldValues[i] + "\"" + ",";
                }
            }
            if (IsNumber(_fieldValues[_fieldValues.Length - 1]))
            {
                retStr += _fieldValues[_fieldValues.Length - 1];
            }
            else
            {
                retStr += "\"" + _fieldValues[_fieldValues.Length - 1] + "\"";
            }
            return retStr;
        }


        /**
         * internal store all field values in string format.
         */
        private readonly string[] _fieldValues;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determines whether the specified STR value is number.
        /// </summary>
        /// <param name="strValue">The STR value.</param>
        /// <returns>
        /// 	<c>true</c> if the specified STR value is number; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsNumber(string strValue)
        {
            try
            {
                double.Parse(strValue);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

}
