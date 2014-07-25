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
    /// This class is factory for creating new Connection objects. 
    /// </summary>
    public class Connector
    {
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Opens the url.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static HttpConnection Open(string name)
        {
            return new HttpConnectionImpl(name);
        }
    }

    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 24SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// internal class implements HttpConnection interface..
    /// </summary>
    class HttpConnectionImpl : HttpConnection
    {
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpConnectionImpl"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        public HttpConnectionImpl(string url)
        {
            _httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            _httpWebRequest.Proxy = null;
           // _httpWebRequest.Proxy.Credentials = CredentialCache.DefaultCredentials;
        }

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
        public override void SetRequestMethod(string method)
        {
            _httpWebRequest.Method = method;
        }

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
        public override void SetRequestProperty(string key, string value)
        {
            _httpWebRequest.Headers.Add(key,value);
        }

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
        public override Stream OpenOutputStream()
        {
            if (_outputStream == null)
            {
                _outputStream = _httpWebRequest.GetRequestStream();
            }
            return _outputStream;
        }

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
        /// <returns>
        /// the HTTP Status-Code or -1 if no status code can be discerned
        /// </returns>
        public override int GetResponseCode()
        {
            if (_httpWebResponse == null)
            {
                _httpWebResponse = (HttpWebResponse)_httpWebRequest.GetResponse();
            }
            return (int)_httpWebResponse.StatusCode;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Closes this connection.
        /// </summary>
        public override void Close()
        {
            if (_outputStream != null)
            {
                _outputStream.Close();
                _outputStream = null;
            }
            if (_inputStream != null)
            {
                _inputStream.Close();
                _inputStream = null;
            }
        }

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
        /// <returns>
        /// the value of the named field, parsed as an integer. The def value
        /// is returned if the field is missing or malformed
        /// </returns>
        public override int GetHeaderFieldInt(string name, int def)
        {
            string value = _httpWebResponse.Headers[name];
            try
            {
                if (value != null)
                {
                    int retValue = int.Parse(value);
                    return retValue;
                }
            }
            catch(Exception )
            {
                
            }
            return def;
        }

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
        public override Stream OpenInputStream()
        {
            if (_httpWebResponse == null)
            {
                _httpWebResponse = (HttpWebResponse)_httpWebRequest.GetResponse();
            }
            if (_inputStream == null)
            {
                _inputStream = _httpWebResponse.GetResponseStream();
            }
            return _inputStream;
        }

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
        /// <returns>
        /// the key of the nth header field or null if the array index is
        /// out of range
        /// </returns>
        public override string GetHeaderFieldKey(int i)
        {
            if (i < _httpWebResponse.Headers.Count)
            {
                return _httpWebResponse.Headers.GetKey(i);
            }
            return null;
        }

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
        /// <returns>
        /// the value of the nth header field or null if the array index is
        /// out of range. An empty String is returned if the field does not have a value
        /// </returns>
        public override string GetHeaderField(int i)
        {
            if (i < _httpWebResponse.Headers.Count)
            {
                return _httpWebResponse.Headers.Get(i);
            }
            return null;
        }

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
        /// <returns>
        /// the value of the named header field, or null if there is no
        /// such field in the header
        /// </returns>
        public override string getHeaderField(string name)
        {
            return _httpWebResponse.Headers.Get(name);
        }

        private readonly HttpWebRequest _httpWebRequest;
        private HttpWebResponse _httpWebResponse;
        private Stream _outputStream;
        private Stream _inputStream;
    }
}
