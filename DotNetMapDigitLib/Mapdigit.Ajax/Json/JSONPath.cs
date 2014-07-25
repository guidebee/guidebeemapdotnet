//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 23SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Ajax.Json
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 23SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// JSONPath defines a simple expression language which works a lot like a very 
    /// small subset of XPath. - the expression syntax uses the dot character 
    /// for sub-elements and square brackets for arrays. Some sample expressions are,
    /// for example - "photos.photo[1].title", "[0].location", "[1].status.text", etc 
    /// </summary>
    public abstract class JSONPath
    {

        /**
         * Dot character used as the separator.
         */
        public const char Separator = '.';

        /**
         * Array start character.
         */
        public const char ArrayStart = '[';

        /**
         * Array end character.
         */
        public const char ArrayEnd = ']';

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 23SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the boolean value based on the path.
        /// </summary>
        /// <param name="path">the path string.</param>
        /// <returns>the boolean values.</returns>
        public abstract bool GetAsBoolean(string path);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 23SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the integer value based on the path.
        /// </summary>
        /// <param name="path">the path string.</param>
        /// <returns>the integer values.</returns>
        public abstract int GetAsInteger(string path);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 23SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the long value based on the path.
        /// </summary>
        /// <param name="path">path the path string.</param>
        /// <returns>the long values.</returns>
        public abstract long GetAsLong(string path);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 23SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the double value based on the path.
        /// </summary>
        /// <param name="path">the path string</param>
        /// <returns>the double values</returns>
        public abstract double GetAsDouble(string path);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 23SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the String value based on the path.
        /// </summary>
        /// <param name="path">the path string.</param>
        /// <returns>the string values.</returns>
        public abstract string GetAsString(string path);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 23SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the size of array.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>the size of a given arrary.</returns>
        public abstract int GetSizeOfArray(string path);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 23SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets as string array.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>a string array</returns>
        public abstract string[] GetAsStringArray(string path);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 23SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets as integer array.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>a integer array.</returns>
        public abstract int[] GetAsIntegerArray(string path);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 23SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets as double array.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>a double array.</returns>
        public abstract double[] GetAsDoubleArray(string path);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 23SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets as json array.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>a json array.</returns>
        public abstract JSONArray GetAsArray(string path);

    }
}
