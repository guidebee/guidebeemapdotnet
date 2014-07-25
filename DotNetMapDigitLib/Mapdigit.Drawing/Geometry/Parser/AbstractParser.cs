//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 24SEP2010  James Shen                 	          Code review
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
    // 24SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// AbstractParser is the base class for parsers found
    /// in this package. <br />
    /// All parsers work on a String and the AbstractParser
    /// keeps a reference to that string along with the current position
    /// (@see #currentPos) and current character (@see current). <br />
    /// The key methods for this class are read which reads the next
    /// character in the parsed string, setString which sets the string
    /// to be parsed, and the utility methods skipCommaSpaces,
    /// skipSpaces and SkipSpacesCommaSpaces which can
    /// be used by descendants to skip common separators.
    /// <br />
    /// For an implementation example, see TransformListParser.
    /// </summary>
    internal abstract class AbstractParser
    {
        /**
         * The current position in the string
         */
        protected int _currentPos;
        /**
         * The String being parsed
         */
        protected string _inputString;
        /**
         * The current character being parsed
         * This is accessible by sub-classes
         */
        protected int _current;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// the next character. Returns -1 when the
        /// end of the String has been reached.
        /// </summary>
        /// <returns>the next character</returns>
        protected int Read()
        {
            if (_currentPos < _inputString.Length)
            {
                return _inputString[_currentPos++];
            }
            return -1;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets this parser's String. This also resets the current position to 0
        /// </summary>
        /// <param name="str">the string this parser should parse</param>
        protected void SetString(string str)
        {
            if (str == null)
            {
                throw new ArgumentException();
            }

            _inputString = str;
            _currentPos = 0;
            _current = -1;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Skips the spaces.
        /// </summary>
        protected void SkipSpaces()
        {
            while (_current == 0x20 || _current == 0x09 || _current == 0x0D || _current == 0x0A)
            {
                _current = Read();
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Skips the whitespaces and an optional comma.
        /// </summary>
        protected void SkipCommaSpaces()
        {
            SkipSepSpaces(',');
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Skips the whitespaces and an optional char.
        /// </summary>
        /// <param name="sep">seperator to skip in addition to spaces.</param>
        protected void SkipSepSpaces(char sep)
        {
            while (_current == sep || _current == 0x20 || _current == 0x09
                   || _current == 0x0D || _current == 0x0A)
            {
                _current = Read();
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Skips wsp*,wsp* .
        /// </summary>
        protected void SkipSpacesCommaSpaces()
        {
            SkipSpaces();

            if (_current != ',')
            {
                throw new ArgumentException();
            }

            _current = Read();
            SkipSpaces();
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tests if the current substring (i.e. the substring beginning at the
        /// current position) starts with the specified prefix.  If the current
        /// substring starts with the specified prefix, the current character will
        /// be updated to point to the character immediately following the last
        /// character in the prefix; otherwise, the currentPos will
        /// not be affected.  For example, if the string being parsed is
        ///  "timingAttr", and the current character is 'A':
        /// <pre>
        /// CurrentStartsWith("Att") returns true, current == 'r'
        /// CurrentStartsWith("Attr") returns true, current == -1
        /// CurrentStartsWith("Attx") returns false, current == 'A'
        /// </pre>
        /// </summary>
        /// <param name="str">str the prefix to be tested</param>
        /// <returns>true if the current substring starts with the
        /// specified prefix.  The result is false if
        /// currentPos is non-positive, or if the current substring
        /// does not start with the specified prefix.</returns>
        protected bool CurrentStartsWith(string str)
        {
            if (_currentPos <= 0)
            {
                return false;
            }
            if (_inputString.Substring(_currentPos - 1).StartsWith(str))
            {
                _currentPos += str.Length - 1;
                _current = Read();
                return true;
            }
            return false;
        }
    }
}