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
using Mapdigit.Network;

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
    /// Http response object. 
    /// </summary>
    public sealed class Response
    {

        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /**
         * Default constructor.
         */
        internal Response()
        {
            _result = null;
        }

        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the http result object.
        /// </summary>
        /// <returns>the result object</returns>
        public Result GetResult()
        {
            return _result;
        }

        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the http content type.
        /// </summary>
        /// <returns>the content type</returns>
        public string GetContentType()
        {
            return _contentType;
        }

        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the http charset.
        /// </summary>
        /// <returns>the charset</returns>
        public string GetCharset()
        {
            return _charset;
        }

        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the http response status code.
        /// </summary>
        /// <returns>the status code</returns>
        public int GetCode()
        {
            return _responseCode;
        }

        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the http response headers.
        /// </summary>
        /// <returns>the header array.</returns>
        public Arg[] GetHeaders()
        {
            return _headers;
        }

        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the raw contents
        /// </summary>
        /// <returns>raw contents.</returns>
        public string GetRawContent()
        {
            return _rawContent;
        }

        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the raw content array.
        /// </summary>
        /// <returns></returns>
        public byte[] GetRawContentArray()
        {
            return _rawArray;
        }

        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get any exception related to this http request/response.
        /// </summary>
        /// <returns>the exception object.</returns>
        public Exception GetException()
        {
            return _ex;
        }

        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        public void SetException(Exception ex)
        {
            _ex = ex;
        }

        //package domain variables.
        internal Result _result;
        internal string _rawContent;
        internal byte[] _rawArray;
        internal int _responseCode = HttpConnection.HttpNotFound;
        internal Exception _ex;
        internal string _contentType;
        internal string _charset;
        internal Arg[] _headers;

    }
}
