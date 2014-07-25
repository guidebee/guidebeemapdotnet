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
using System.IO;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Ajax.Json
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 24SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// JSONStringer provides a quick and convenient way of producing JSON text.
    /// The texts produced strictly conform to JSON syntax rules. No whitespace is
    /// added, so the results are ready for transmission or storage. Each instance of
    /// JSONStringer can produce one JSON text.
    /// 
    /// A JSONStringer instance provides a value method for appending
    /// values to the text, and a key
    /// method for adding keys before values in objects. There are array
    /// and endArray methods that make and bound array values, and
    /// object and endObject methods which make and bound
    /// object values. All of these methods return the JSONWriter instance,
    /// permitting cascade style. For example, <pre>
    ///  myString = new JSONStringer()
    ///      .object()
    ///          .key("JSON")
    ///          .value("Hello, World!")
    ///     .endObject()
    /// .toString();</pre> which produces the string <pre>
    ///  {"JSON":"Hello, World!"}</pre>
    /// 
    /// The first method called must be array or object.
    /// There are no methods for adding commas or colons. JSONStringer adds them for
    /// you. Objects and arrays can be nested up to 20 levels deep.
    /// 
    /// This can sometimes be easier than using a JSONObject to build a string.
    /// </summary>
    public class JSONStringer : JSONWriter
    {

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Make a fresh JSONStringer. It can be used to build one JSON text.
        /// </summary>
        public JSONStringer(): base(new StringWriter())
        {

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Return the JSON text. This method is used to obtain the product of the
        /// JSONStringer instance. It will return null if there was a
        /// problem in the construction of the JSON text (such as the calls to
        /// array were not properly balanced with calls to
        /// endArray).
        /// </summary>
        /// <returns>
        /// The JSON text
        /// </returns>
        public override string ToString()
        {
            return _mode == 'd' ? _writer.ToString() : null;
        }
    }
}
