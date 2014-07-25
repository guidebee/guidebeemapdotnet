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
using Mapdigit.Ajax.Json;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Ajax
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 24SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Http result object (JSONObject or JSONArray).
    /// </summary>
    public class Result : JSONPath
    {

        /**
         * text/javascript content type.
         */
        public const string JsContentType = "text/javascript";

        /**
         * application/json content type.
         */
        public const string JSONContentType = "application/json";

        /**
         * text/plain content type.
         */
        public const string PlainTextContentType = "text/plain";

        /**
         * text/xml content type.
         */
        public const string TextXmlContentType = "text/xml";

        /**
         * text/html content type.
         */
        public const string TextHtmlContentType = "text/html";

        /**
         * application/xml content type.
         */
        public const string ApplicationXmlContentType = "application/xml";

        /**
         * application/kml content type.
         */
        public const string ApplicationKmlContentType = "application/vnd.google-earth.kml+xml";


        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// return the hash code.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return _isArray ? _array.GetHashCode() : _json.GetHashCode();
        }

        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object other)
        {
            if (other is Result)
            {
                return _isArray ? _array.Equals(other) : _json.Equals(other);
            }
            return false;
        }

        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            try
            {
                return _isArray ? _array.ToString(2) :
                    _json.ToString(2);
            }
            catch (Exception)
            {
                return _json.ToString();
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the boolean value based on the path.
        /// </summary>
        /// <param name="path">the path string.</param>
        /// <returns>the boolean values.</returns>
        public override bool GetAsBoolean(string path)
        {
            return _isArray ? _array.GetAsBoolean(path) : _json.GetAsBoolean(path);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the integer value based on the path.
        /// </summary>
        /// <param name="path">the path string.</param>
        /// <returns>the integer values.</returns>
        public override int GetAsInteger(string path)
        {
            return _isArray ? _array.GetAsInteger(path) : _json.GetAsInteger(path);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the long value based on the path.
        /// </summary>
        /// <param name="path">path the path string.</param>
        /// <returns>the long values.</returns>
        public override long GetAsLong(string path)
        {
            return _isArray ? _array.GetAsLong(path) : _json.GetAsLong(path);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the double value based on the path.
        /// </summary>
        /// <param name="path">the path string</param>
        /// <returns>the double values</returns>
        public override double GetAsDouble(string path)
        {
            return _isArray ? _array.GetAsDouble(path) : _json.GetAsDouble(path);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the String value based on the path.
        /// </summary>
        /// <param name="path">the path string.</param>
        /// <returns>the string values.</returns>
        public override string GetAsString(string path)
        {
            return _isArray ? _array.GetAsString(path) : _json.GetAsString(path);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the size of array.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>the size of a given arrary.</returns>
        public override int GetSizeOfArray(string path)
        {
            return _isArray ? _array.GetSizeOfArray(path) : _json.GetSizeOfArray(path);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets as string array.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>a string array</returns>
        public override string[] GetAsStringArray(string path)
        {
            return _isArray ? _array.GetAsStringArray(path) : _json.GetAsStringArray(path);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets as integer array.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>a integer array.</returns>
        public override int[] GetAsIntegerArray(string path)
        {
            return _isArray ? _array.GetAsIntegerArray(path)
                    : _json.GetAsIntegerArray(path);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets as double array.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>a double array.</returns>
        public override double[] GetAsDoubleArray(string path)
        {
            return _isArray ? _array.GetAsDoubleArray(path)
                    : _json.GetAsDoubleArray(path);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets as json array.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>a json array.</returns>
        public override JSONArray GetAsArray(string path)
        {
            return _isArray ? _array.GetAsArray(path)
                    : _json.GetAsArray(path);
        }

        private readonly JSONObject _json;
        private readonly JSONArray _array;
        private readonly bool _isArray;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// construct a Result from input string.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns></returns>
        internal static Result FromContent(string content, string contentType)
        {

            if (content == null)
            {
                throw new ArgumentException("content cannot be null");
            }

            if (JsContentType.Equals(contentType) ||
                TextHtmlContentType.Equals(contentType) ||
                JSONContentType.Equals(contentType) ||
                // some sites return JSON with the plain text content type
                PlainTextContentType.Equals(contentType))
            {
                try
                {
                    return content.StartsWith("[") ?
                        new Result(new JSONArray(content)) :
                        new Result(new JSONObject(content));
                }
                catch (Exception ex)
                {
                    throw new JSONException(ex.Message);
                }
            }

            if (TextXmlContentType.Equals(contentType) ||
                 ApplicationXmlContentType.Equals(contentType) ||
                 ApplicationKmlContentType.Equals(contentType) ||
                // default to XML if content type is not specified
                 contentType == null)
            {
                try
                {
                    return new Result(JSONObject.FromXmlString(content));
                }
                catch (Exception ex)
                {
                    throw new JSONException(ex.Message);
                }
            }
            throw new JSONException("Unsupported content-type: " + contentType);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// </summary>
        /// <param name="obj">The obj.</param>
        private Result(JSONObject obj)
        {
            if (obj == null)
            {
                throw new ArgumentException("json object cannot be null");
            }
            _isArray = false;
            _json = obj;
            _array = null;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// </summary>
        /// <param name="obj">The obj.</param>
        private Result(JSONArray obj)
        {
            if (obj == null)
            {
                throw new ArgumentException("json object cannot be null");
            }
            _isArray = true;
            _json = null;
            _array = obj;
        }

    }

}
