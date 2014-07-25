//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 25SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System;

//--------------------------------- PACKAGE ------------------------------------

namespace Mapdigit.Drawing.Geometry.Parser
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 25SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// The NumberListParser class converts attributes
    /// conforming to the SVG Tiny definition of coordinate or number
    /// list (see <a href="http://www.w3.org/TR/SVG11/types.html#BasicDataTypes">
    /// Basic Data Types</a>).
    /// </summary>
    internal class NumberListParser : NumberParser
    {
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses the number list.
        /// </summary>
        /// <param name="listStr">the string containing the list of numbers</param>
        /// <param name="sep">he separator between number values</param>
        /// <returns> An array of numbers</returns>
        public float[] ParseNumberList(string listStr, char sep)
        {
            SetString(listStr);

            _current = Read();
            SkipSpaces();

            bool requireMore = false;
            float[] numbers = null;
            int cur = 0;
            for (;;)
            {
                if (_current != -1)
                {
                    float v = ParseNumber(false);
                    if (numbers == null)
                    {
                        numbers = new float[1];
                    }
                    else if (numbers.Length <= cur)
                    {
                        float[] tmpNumbers = new float[numbers.Length*2];
                        Array.Copy(numbers, 0, tmpNumbers, 0, numbers.Length);
                        numbers = tmpNumbers;
                    }
                    numbers[cur++] = v;
                }
                else
                {
                    if (!requireMore)
                    {
                        break;
                    }
                    throw new ArgumentException();
                }
                SkipSpaces();
                requireMore = (_current == sep);
                SkipSepSpaces(sep);
            }

            if (numbers != null && cur != numbers.Length)
            {
                float[] tmpNumbers = new float[cur];
                Array.Copy(numbers, 0, tmpNumbers, 0, cur);
                numbers = tmpNumbers;
            }

            return numbers;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses the number list.
        /// </summary>
        /// <param name="listStr">listStr the string containing the list of numbers</param>
        /// <returns> An array of numbers</returns>
        public float[] ParseNumberList(string listStr)
        {
            return ParseNumberList(listStr, ',');
        }
    }
}