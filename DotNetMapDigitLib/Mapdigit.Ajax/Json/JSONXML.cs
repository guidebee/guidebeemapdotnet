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
using System.Text;

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
    /// This provides static methods to convert an JSONXML text into a JSONObject,
    /// and to covert a JSONObject into an JSONXML text.
    /// </summary>
    internal class JSONXML
    {

        /** 
         * The Character '&'. 
         */
        public const char Amp = '&';

        /** 
         * The Character '''. 
         */
        public const char Apos = '\\';

        /** 
         * The Character '!'. 
         */
        public const char Bang = '!';

        /** 
         * The Character '='. 
         */
        public const char Eq = '=';

        /** 
         * The Character '>'. 
         */
        public const char Gt = '>';

        /** 
         * The Character '<'. 
         */
        public const char Lt = '<';

        /** 
         * The Character '?'. 
         */
        public const char Quest = '?';

        /** 
         * The Character '"'. 
         */
        public const char Quot = '"';

        /** 
         * The Character '/'. 
         */
        public const char Slash = '/';

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Replace special characters with JSONXML escapes:
        /// <pre>
        /// &amp; <small>(ampersand)</small> is replaced by &amp;amp;
        /// &lt; <small>(less than)</small> is replaced by &amp;lt;
        /// &gt; <small>(greater than)</small> is replaced by &amp;gt;
        /// &quot; <small>(double quote)</small> is replaced by &amp;quot;
        /// </pre>
        /// </summary>
        /// <param name="str">The string to be escaped</param>
        /// <returns>The escaped string</returns>
        public static string Escape(string str)
        {
            var sb = new StringBuilder();
            for (int i = 0, len = str.Length; i < len; i++)
            {
                var c = str[i];
                switch (c)
                {
                    case '&':
                        sb.Append("&amp;");
                        break;
                    case '<':
                        sb.Append("&lt;");
                        break;
                    case '>':
                        sb.Append("&gt;");
                        break;
                    case '"':
                        sb.Append("&quot;");
                        break;
                    default:
                        sb.Append(c);
                        break;

                }
            }
            return sb.ToString();
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Convert a well-formed (but not necessarily valid) JSONXML string into a
        /// JSONObject. Some information may be lost in this transformation
        /// because JSON is a data format and JSONXML is a document format. JSONXML uses
        /// elements, attributes, and content text, while JSON uses unordered
        /// collections of name/value pairs and arrays of values. JSON does not
        /// does not like to distinguish between elements and attributes.
        /// Sequences of similar elements are represented as JSONArrays. Content
        /// text may be placed in a "content" member. Comments, prologs, DTDs, and
        /// &lt;[ [ ]]> are ignored.
        /// </summary>
        /// <param name="str">The source string</param>
        /// <returns>A JSONObject containing the structured data from the JSONXML string</returns>
        public static JSONObject ToJSONObject(string str)
        {
            var o = new JSONObject();
            var x = new JSONXMLTokener(str);
            while (x.More())
            {
                x.SkipPast("<");
                Parse(x, o, null);
            }
            return o;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Convert a JSONObject into a well-formed, element-normal JSONXML string.
        /// </summary>
        /// <param name="o">A JSONObject</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public static string ToString(object o)
        {
            return ToString(o, null);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Convert a JSONObject into a well-formed, element-normal JSONXML string.
        /// </summary>
        /// <param name="o">The JSONObject.</param>
        /// <param name="tagName">The optional name of the enclosing tag</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public static string ToString(object o, string tagName)
        {
            var b = new StringBuilder();
            int i;
            JSONArray ja;
            JSONObject jo;
            string k;
            int len;
            string s;
            object v;
            if (o is JSONObject)
            {

                // Emit <tagName>

                if (tagName != null)
                {
                    b.Append('<');
                    b.Append(tagName);
                    b.Append('>');
                }

                // Loop thru the keys.

                jo = (JSONObject)o;
                var keyset = jo.Keys();
                foreach (var c in keyset)
                {
                    k = c.ToString();
                    v = jo.Get(k);
                    if (v is string)
                    {
                    }

                    // Emit content in body

                    if (k.Equals("content"))
                    {
                        if (v is JSONArray)
                        {
                            ja = (JSONArray)v;
                            len = ja.Length();
                            for (i = 0; i < len; i += 1)
                            {
                                if (i > 0)
                                {
                                    b.Append('\n');
                                }
                                b.Append(Escape(ja.Get(i).ToString()));
                            }
                        }
                        else
                        {
                            b.Append(Escape(v.ToString()));
                        }

                        // Emit an array of similar keys

                    }
                    else if (v is JSONArray)
                    {
                        ja = (JSONArray)v;
                        len = ja.Length();
                        for (i = 0; i < len; i += 1)
                        {
                            b.Append(ToString(ja.Get(i), k));
                        }
                    }
                    else if (v.Equals(""))
                    {
                        b.Append('<');
                        b.Append(k);
                        b.Append("/>");

                        // Emit a new tag <k>

                    }
                    else
                    {
                        b.Append(ToString(v, k));
                    }
                }


                if (tagName != null)
                {

                    // Emit the </tagname> close tag

                    b.Append("</");
                    b.Append(tagName);
                    b.Append('>');
                }
                return b.ToString();

                // JSONXML does not have good support for arrays. If an array appears in a place
                // where JSONXML is lacking, synthesize an <array> element.

            }
            if (o is JSONArray)
            {
                ja = (JSONArray)o;
                len = ja.Length();
                for (i = 0; i < len; ++i)
                {
                    b.Append(ToString(
                        ja.Opt(i), tagName ?? "array"));
                }
                return b.ToString();
            }
            s = (o == null) ? "null" : Escape(o.ToString());
            return (tagName == null) ? "\"" + s + "\"" :
                (s.Length == 0) ? "<" + tagName + "/>" :
                "<" + tagName + ">" + s + "</" + tagName + ">";
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Scan the content following the named tag, attaching it to the context
        /// </summary>
        /// <param name="x">The JSONXMLTokener containing the source string.</param>
        /// <param name="context">The JSONObject that will include the new material</param>
        /// <param name="name">The tag name</param>
        /// <returns>if the close tag is processed</returns>
        private static bool Parse(JSONXMLTokener x, JSONObject context, string name)
        {
            string s;

            // Test for and skip past these forms:
            //      <!-- ... -->
            //      <!   ...   >
            //      <![  ... ]]>
            //      <?   ...  ?>
            // Report errors for these forms:
            //      <>
            //      <=
            //      <<

            var t = x.NextToken();

            // <!

            if (t == (object)Bang)
            {
                var c = x.Next();
                if (c == '-')
                {
                    if (x.Next() == '-')
                    {
                        x.SkipPast("-->");
                        return false;
                    }
                    x.Back();
                }
                else if (c == '[')
                {
                    t = x.NextToken();
                    if (t.Equals("CDATA"))
                    {
                        if (x.Next() == '[')
                        {
                            s = x.NextCDATA();
                            if (s.Length > 0)
                            {
                                context.Accumulate("content", s);
                            }
                            return false;
                        }
                    }
                    throw x.SyntaxError("Expected 'CDATA['");
                }
                var i = 1;
                do
                {
                    t = x.NextMeta();
                    if (t == null)
                    {
                        throw x.SyntaxError("Missing '>' after '<!'.");
                    }
                    if (t == (object)Lt)
                    {
                        i += 1;
                    }
                    else if (t == (object)Gt)
                    {
                        i -= 1;
                    }
                } while (i > 0);
                return false;
            }
            if (t == (object)Quest)
            {

                // <?

                x.SkipPast("?>");
                return false;
            }
            if (t == (object)Slash)
            {

                // Close tag </

                if (name == null || !x.NextToken().Equals(name))
                {
                    throw x.SyntaxError("Mismatched close tag");
                }
                if (x.NextToken() != (object)Gt)
                {
                    throw x.SyntaxError("Misshaped close tag");
                }
                return true;

            }
            if (t is char)
            {
                throw x.SyntaxError("Misshaped tag");

                // Open tag <

            }
            string n = (string)t;
            t = null;
            JSONObject o = new JSONObject();
            for (; ; )
            {
                if (t == null)
                {
                    t = x.NextToken();
                }

                // attribute = value

                if (t is string)
                {
                    s = (string)t;
                    t = x.NextToken();
                    if (t == (object)Eq)
                    {
                        t = x.NextToken();
                        if (!(t is string))
                        {
                            throw x.SyntaxError("Missing value");
                        }
                        o.Accumulate(s, t);
                        t = null;
                    }
                    else
                    {
                        o.Accumulate(s, "");
                    }

                    // Empty tag <.../>

                }
                else if (t == (object)Slash)
                {
                    if (x.NextToken() != (object)Gt)
                    {
                        throw x.SyntaxError("Misshaped tag");
                    }
                    context.Accumulate(n, o);
                    return false;

                    // Content, between <...> and </...>

                }
                else if (t == (object)Gt)
                {
                    for (; ; )
                    {
                        t = x.NextContent();
                        if (t == null)
                        {
                            if (name != null)
                            {
                                throw x.SyntaxError("Unclosed tag " + name);
                            }
                            return false;
                        }
                        if (t is string)
                        {
                            s = (string)t;
                            if (s.Length > 0)
                            {
                                o.Accumulate("content", s);
                            }

                            // Nested element

                        }
                        else if (t == (object)Lt)
                        {
                            if (Parse(x, o, n))
                            {
                                if (o.Length() == 0)
                                {
                                    context.Accumulate(n, "");
                                }
                                else if (o.Length() == 1 &&
                                         o.Opt("content") != null)
                                {
                                    context.Accumulate(n, o.Opt("content"));
                                }
                                else
                                {
                                    context.Accumulate(n, o);
                                }
                                return false;
                            }
                        }
                    }
                }
                else
                {
                    throw x.SyntaxError("Misshaped tag");
                }
            }
        }


    }
}
