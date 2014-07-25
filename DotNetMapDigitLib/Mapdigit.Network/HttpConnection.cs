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
using System.IO;
using System.Net;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Network
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 24SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// This interface defines the necessary methods and constants for an HTTP
    /// connection.
    /// </summary>
    public abstract class HttpConnection
    {
        /**
         * HTTP Get method.
        */
        public const string Get = "GET";

        /**
         * HTTP POST method.
         */
        public const string Post = "POST";

        /**
         * 200: The request has succeeded.
         */
        public const int HttpOk = (int)HttpStatusCode.OK;

        /**
         * 404: The server has not found anything matching the Request-URI.
         * No indication is given of whether the condition is temporary or permanent.
         */
        public const int HttpNotFound = (int)HttpStatusCode.NotFound;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set the method for the URL request, one of:  GET, POST, HEAD
        /// </summary>
        /// <param name="method">the HTTP method</param>
        public abstract void SetRequestMethod(string method);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the general request property. If a property with the key already
        /// exists, overwrite its value with the new value.
        /// </summary>
        /// <param name="key">the keyword by which the request is known 
        /// (e.g., "accept").</param>
        /// <param name="value">the value associated with it.</param>
        public abstract void SetRequestProperty(string key, string value);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Open and return an output stream for a connection..
        /// </summary>
        /// <returns></returns>
        public abstract Stream OpenOutputStream();

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the HTTP response status code. It parses responses like:
        /// HTTP/1.0 200 OK
        /// HTTP/1.0 401 Unauthorized
        /// and extracts the ints 200 and 401 respectively. from the response
        /// (i.e., the response is not valid HTTP).
        /// </summary>
        /// <returns>the HTTP Status-Code or -1 if no status code can be discerned</returns>
        public abstract int GetResponseCode();

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Closes this connection.
        /// </summary>
        public abstract void Close();

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the value of the named field parsed as a number.
        /// </summary>
        /// <param name="name">the name of the header field.</param>
        /// <param name="def">the default value</param>
        /// <returns>the value of the named field, parsed as an integer. The def value
        /// is returned if the field is missing or malformed</returns>
        public abstract int GetHeaderFieldInt(string name, int def);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Opens the input stream for a connection. 
        /// </summary>
        /// <returns>An input stream</returns>
        public abstract Stream OpenInputStream();

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets a header field key by index.
        /// </summary>
        /// <param name="i">the index of the header field</param>
        /// <returns>the key of the nth header field or null if the array index is
        /// out of range</returns>
        public abstract string GetHeaderFieldKey(int i);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets a header field value by index.
        /// </summary>
        /// <param name="i">the index of the header field</param>
        /// <returns>the value of the nth header field or null if the array index is
        /// out of range. An empty String is returned if the field does not have a value</returns>
        public abstract string GetHeaderField(int i);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the value of the named header field.
        /// </summary>
        /// <param name="name">name of a header field.</param>
        /// <returns>the value of the named header field, or null if there is no
        /// such field in the header</returns>
        public abstract string getHeaderField(string name);

    }
}
