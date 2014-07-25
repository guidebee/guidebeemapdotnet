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
    /// JSONWriter provides a quick and convenient way of producing JSON text.
    /// The texts produced strictly conform to JSON syntax rules. No whitespace is
    /// added, so the results are ready for transmission or storage. Each instance of
    /// JSONWriter can produce one JSON text.
    /// 
    /// A JSONWriter instance provides a value method for appending
    /// values to the text, and a key
    /// method for adding keys before values in objects. There are array
    /// and endArray methods that make and bound array values, and
    /// object and endObject methods which make and bound
    /// object values. All of these methods return the JSONWriter instance,
    /// permitting a cascade style. For example, <pre>
    ///  new JSONWriter(myWriter)
    ///      .object()
    ///         .key("JSON")
    ///         .value("Hello, World!")
    ///      .endObject();</pre> which writes <pre>
    ///  {"JSON":"Hello, World!"}</pre>
    /// 
    /// The first method called must be array or object.
    /// There are no methods for adding commas or colons. JSONWriter adds them for
    /// you. Objects and arrays can be nested up to 20 levels deep.
    /// 
    /// This can sometimes be easier than using a JSONObject to build a string.
    /// </summary>
    public class JSONWriter
    {

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///Make a fresh JSONWriter. It can be used to build one JSON text.
        /// </summary>
        /// <param name="w">The writer.</param>
        public JSONWriter(StringWriter w)
        {
            _comma = false;
            _mode = 'i';
            _stack = new char[Maxdepth];
            _top = 0;
            _writer = w;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Append a value.
        /// </summary>
        /// <param name="s">A string value.</param>
        /// <returns>this</returns>
        private JSONWriter Append(string s)
        {
            if (s == null)
            {
                throw new JSONException("Null pointer");
            }
            if (_mode == 'o' || _mode == 'a')
            {
                try
                {
                    if (_comma && _mode == 'a')
                    {
                        _writer.Write(',');
                    }
                    _writer.Write(s);
                }
                catch (IOException e)
                {
                    throw new JSONException(e.Message);
                }
                if (_mode == 'o')
                {
                    _mode = 'k';
                }
                _comma = true;
                return this;
            }
            throw new JSONException("Value out of sequence.");
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Begin appending a new array. All values until the balancing
        /// endArray will be appended to this array. The
        /// endArray method must be called to mark the array's end.
        /// </summary>
        /// <returns>this</returns>
        public JSONWriter Array()
        {
            if (_mode == 'i' || _mode == 'o' || _mode == 'a')
            {
                Push('a');
                Append("[");
                _comma = false;
                return this;
            }
            throw new JSONException("Misplaced array.");
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// End something.
        /// </summary>
        /// <param name="m">The Mode.</param>
        /// <param name="c">Closing character.</param>
        /// <returns>this</returns>
        private JSONWriter End(char m, char c)
        {
            if (_mode != m)
            {
                throw new JSONException(m == 'o' ? "Misplaced endObject." :
                    "Misplaced endArray.");
            }
            Pop(m);
            try
            {
                _writer.Write(c);
            }
            catch (IOException e)
            {
                throw new JSONException(e.Message);
            }
            _comma = true;
            return this;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///End an array. This method most be called to balance calls to
        /// array.
        /// </summary>
        /// <returns>this</returns>
        public JSONWriter EndArray()
        {
            return End('a', ']');
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// End an object. This method most be called to balance calls to object
        /// </summary>
        /// <returns>this</returns>
        public JSONWriter EndObject()
        {
            return End('k', '}');
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Append a key. The key will be associated with the next value. In an
        /// object, every value must be preceded by a key.
        /// </summary>
        /// <param name="s">A key string</param>
        /// <returns>this</returns>
        public JSONWriter Key(string s)
        {
            if (s == null)
            {
                throw new JSONException("Null key.");
            }
            if (_mode == 'k')
            {
                try
                {
                    if (_comma)
                    {
                        _writer.Write(',');
                    }
                    _writer.Write(JSONObject.Quote(s));
                    _writer.Write(':');
                    _comma = false;
                    _mode = 'o';
                    return this;
                }
                catch (IOException e)
                {
                    throw new JSONException(e.Message);
                }
            }
            throw new JSONException("Misplaced key.");
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Begin appending a new object. All keys and values until the balancing
        /// endObject will be appended to this object. The
        /// endObject method must be called to mark the object's end.
        /// </summary>
        /// <returns>this</returns>
        public JSONWriter Object()
        {
            if (_mode == 'i')
            {
                _mode = 'o';
            }
            if (_mode == 'o' || _mode == 'a')
            {
                Append("{");
                Push('k');
                _comma = false;
                return this;
            }
            throw new JSONException("Misplaced object.");

        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Pop an array or object scope.
        /// </summary>
        /// <param name="c">The scope to close.</param>
        private void Pop(char c)
        {
            if (_top <= 0 || _stack[_top - 1] != c)
            {
                throw new JSONException("Nesting error.");
            }
            _top -= 1;
            _mode = _top == 0 ? 'd' : _stack[_top - 1];
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Push an array or object scope.
        /// </summary>
        /// <param name="c">The scope to open</param>
        private void Push(char c)
        {
            if (_top >= Maxdepth)
            {
                throw new JSONException("Nesting too deep.");
            }
            _stack[_top] = c;
            _mode = c;
            _top += 1;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Append either the value true or the value
        /// false.
        /// </summary>
        /// <param name="b">A boolean</param>
        /// <returns>this</returns>
        public JSONWriter Value(bool b)
        {
            return Append(b ? "true" : "false");
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Append a double value.
        /// </summary>
        /// <param name="d">The double.</param>
        /// <returns>this</returns>
        public JSONWriter Value(double d)
        {
            return Value(d.ToString());
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Append a long value.
        /// </summary>
        /// <param name="l">The long.</param>
        /// <returns>this</returns>
        public JSONWriter Value(long l)
        {
            return Append(l.ToString());
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Append an object value.
        /// </summary>
        /// <param name="o">The object to append. It can be null, or a Boolean, Number,
        /// String, JSONObject, or JSONArray, or an object with a toJSONString()
        /// method.</param>
        /// <returns>this</returns>
        public JSONWriter Value(object o)
        {
            return Append(JSONObject.ValueToString(o));
        }


        private const int Maxdepth = 20;

        /**
         * The comma flag determines if a comma should be output before the next
         * value.
         */
        private bool _comma;

        /**
         * The current mode. Values:
         * 'a' (array),
         * 'd' (done),
         * 'i' (initial),
         * 'k' (key),
         * 'o' (object).
         */
        protected char _mode;

        /**
         * The object/array stack.
         */
        private readonly char[] _stack;

        /**
         * The stack top index. A value of 0 indicates that the stack is empty.
         */
        private int _top;

        /**
         * The writer that will receive the output.
         */
        protected StringWriter _writer;

    }
}
