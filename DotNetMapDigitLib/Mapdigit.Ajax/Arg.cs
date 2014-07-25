//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 20SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Ajax
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 20SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Arg defines HTTP header/value pair.
    /// </summary>
    public sealed class Arg
    {
        // some commonly used http header names
        /// <summary>
        /// Http header authorization.
        /// </summary>
        public const string Authorization = "authorization";


        /// <summary>
        /// Http header content-length.
        /// </summary>
        public const string ContentLength = "content-length";


        /// <summary>
        /// Http header content-type.
        /// </summary>
        public const string ContentType = "content-type";


        /// <summary>
        /// Http header content-disposition.
        /// </summary>
        public const string ContentDisposition = "content-disposition";


        /// <summary>
        /// Http header content-transfer-encoding.
        /// </summary>
        public const string ContentTransferEncoding= "content-transfer-encoding";


        /// <summary>
        /// Http header last-modified.
        /// </summary>
        public const string LastModified = "last-modified";


        /// <summary>
        /// Http header if-modified-since.
        /// </summary>
        public const string IfModifiedSince = "if-modified-since";


        /// <summary>
        /// Http header etag.
        /// </summary>
        public const string Etag = "etag";


        /// <summary>
        /// Http header if-none-match.
        /// </summary>
        public const string IfNoneMatch = "if-none-match";

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 20SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="Arg"/> class.
        /// </summary>
        /// <param name="k">http header name.</param>
        /// <param name="v">http header value.</param>
        public Arg(string k, string v)
        {
            if (string.IsNullOrEmpty(k))
            {
                throw new ArgumentException("invalid key");
            }
            _key = k;
            _value = v;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 20SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// return the http header name.
        /// </summary>
        /// <returns>the http header name</returns>
        public string GetKey()
        {
            return _key;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 20SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// return the http header value.
        /// </summary>
        /// <returns>return the http header value</returns>
        public string GetValue()
        {
            return _value;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 20SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms 
        /// and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return _value == null ? _key.GetHashCode()
                    : _key.GetHashCode() ^ _value.GetHashCode();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 20SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to 
        /// this instance.
        /// </summary>
        /// <param name="other">The <see cref="System.Object"/> to compare with this 
        /// instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal 
        /// to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object other)
        {
            if (other is Arg)
            {
                var oa = ((Arg)other);
                return _value == null ? _key.Equals(oa._key) :
                    _key.Equals(oa._key) && _value.Equals(oa._value);
            }
            return false;
        }


        /// <summary>
        /// the http header key.
        /// </summary>
        private readonly string _key;


        /// <summary>
        /// the http header value.
        /// </summary>
        private readonly string _value;

    }
}
