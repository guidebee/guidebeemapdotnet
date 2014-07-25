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
using System.Collections;
using System.IO;
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
    /// A JSONObject is an unordered collection of name/value pairs. Its
    /// external form is a string wrapped in curly braces with colons between the
    /// names and values, and commas between the values and names. The internal form
    /// is an object having get and opt methods for
    /// accessing the values by name, and put methods for adding or
    /// replacing values by name. The values can be any of these types:
    /// Boolean, JSONArray, JSONObject,
    /// Number, String, or the JSONObject.NULL
    /// object. A JSONObject constructor can be used to convert an external form
    /// JSON text into an internal form whose values can be retrieved with the
    /// get and opt methods, or to convert values into a
    /// JSON text using the put and toString methods.
    /// A get method returns a value if one can be found, and throws an
    /// exception if one cannot be found. An opt method returns a
    /// default value instead of throwing an exception, and so is useful for
    /// obtaining optional values.
    /// 
    /// The generic get() and opt() methods return an
    /// object, which you can cast or query for type. There are also typed
    /// get and opt methods that do type checking and type
    /// coersion for you.
    /// 
    /// The put methods adds values to an object. For example, <pre>
    /// myString = new JSONObject().put("JSON", "Hello, World!").toString();</pre>
    /// produces the string {"JSON": "Hello, World"}.
    /// 
    /// The texts produced by the toString methods strictly conform to
    /// the JSON sysntax rules.
    /// The constructors are more forgiving in the texts they will accept:
    /// <ul>
    /// <li>An extra ,<small>(comma)</small> may appear just
    /// before the closing brace.</li>
    /// <li>Strings may be quoted with '<small>(single
    /// quote)</small>.</li>
    /// <li>Strings do not need to be quoted at all if they do not begin with a quote
    /// or single quote, and if they do not contain leading or trailing spaces,
    /// and if they do not contain any of these characters:
    /// { } [ ] / \ : , = ; # and if they do not look like numbers
    /// and if they are not the reserved words true,
    /// false, or null.</li>
    /// <li>Keys can be followed by = or => as well as
    /// by :.</li>
    /// <li>Values can be followed by ; <small>(semicolon)</small> as
    /// well as by , <small>(comma)</small>.</li>
    /// <li>Numbers may have the 0- <small>(octal)</small> or
    /// 0x- <small>(hex)</small> prefix.</li>
    /// <li>Comments written in the slashshlash, slashstar, and hash conventions
    /// will be ignored.</li>
    /// </ul>
    /// </summary>
    public class JSONObject : JSONPath
    {

        /**
         * Boolean TRUE.
         */
        public const bool True = true;

        /**
         * Boolean FALSE.
         */
        public const bool False = false;


        /**
         * It is sometimes more convenient and less ambiguous to have a
         * NULL object than to use Java's null value.
         * JSONObject.NULL.equals(null) returns true.
         * JSONObject.NULL.ToString() returns "null".
         */
        public static readonly object NULL = new Null();

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="JSONObject"/> class.
        /// </summary>
        public JSONObject()
        {
            _myHashMap = new Hashtable();
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Construct a JSONObject from a subset of another JSONObject.
        /// An array of strings is used to identify the keys that should be copied.
        /// Missing keys are ignored.
        /// </summary>
        /// <param name="jo">A JSONObject.</param>
        /// <param name="sa">An array of strings.</param>
        public JSONObject(JSONObject jo, string[] sa): this()
        {

            for (var i = 0; i < sa.Length; i += 1)
            {
                PutOpt(sa[i], jo.Opt(sa[i]));
            }
        }
        
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Construct a JSONObject from a Map.
        /// </summary>
        /// <param name="map">A map object that can be used to initialize the contents of
        /// the JSONObject.</param>
        public JSONObject(IDictionary map)
        {
            if (map == null)
            {
                _myHashMap = new Hashtable();
            }
            else
            {
                _myHashMap = new Hashtable(map.Count);
                var keys = map.Keys;
                foreach (var o in keys)
                {
                    _myHashMap[o]= map[o];
                }

            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Construct a JSONObject from a string.
        /// This is the most commonly used JSONObject constructor.
        /// </summary>
        /// <param name="str">A string beginning</param>
        public JSONObject(string str): this(new JSONTokener(str))
        {

        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Accumulate values under a key. It is similar to the put method except
        /// that if there is already an object stored under the key then a
        /// JSONArray is stored under the key to hold all of the accumulated values.
        /// If there is already a JSONArray, then the new value is appended to it.
        /// In contrast, the put method replaces the previous value.
        /// </summary>
        /// <param name="key">The key string.</param>
        /// <param name="value">An object to be accumulated under the key.</param>
        /// <returns>this</returns>
        public JSONObject Accumulate(string key, object value)
        {
            TestValidity(value);
            var o = Opt(key);
            if (o == null)
            {
                Put(key, value);
            }
            else if (o is JSONArray)
            {
                ((JSONArray)o).Put(value);
            }
            else
            {
                Put(key, new JSONArray().Put(o).Put(value));
            }
            return this;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Append values to the array under a key. If the key does not exist in the
        /// JSONObject, then the key is put in the JSONObject with its value being a
        /// JSONArray containing the value parameter. If the key was already
        /// associated with a JSONArray, then the value parameter is appended to it.
        /// </summary>
        /// <param name="key">The key string.</param>
        /// <param name="value">An object to be accumulated under the key.</param>
        /// <returns>this</returns>
        public JSONObject Append(string key, object value)
        {
            TestValidity(value);
            var o = Opt(key);
            if (o == null)
            {
                Put(key, new JSONArray().Put(value));
            }
            else if (o is JSONArray)
            {
                throw new JSONException("JSONObject[" + key +
                    "] is not a JSONArray.");
            }
            else
            {
                Put(key, new JSONArray().Put(o).Put(value));
            }
            return this;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Produce a string from a double. The string "null" will be returned if
        /// the number is not finite.
        /// </summary>
        /// <param name="d">The double.</param>
        /// <returns>a String</returns>
        static public string DoubleToString(double d)
        {
            if (double.IsInfinity(d) || double.IsNaN(d))
            {
                return "null";
            }

            // Shave off trailing zeros and decimal point, if possible.

            var s = d.ToString();
            if (s.IndexOf('.') > 0 && s.IndexOf('e') < 0 && s.IndexOf('E') < 0)
            {
                while (s.EndsWith("0"))
                {
                    s = s.Substring(0, s.Length - 1);
                }
                if (s.EndsWith("."))
                {
                    s = s.Substring(0, s.Length - 1);
                }
            }
            return s;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the value object associated with a key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The object associated with the key.</returns>
        public object Get(string key)
        {
            var o = Opt(key);
            if (o == null)
            {
                throw new JSONException("JSONObject[" + Quote(key) +"] not found.");
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
        /// Get the boolean value associated with a key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The truth.</returns>
        public bool GetBoolean(string key)
        {
            var o = Get(key);
            if (o.Equals(false) ||
               o.Equals(False) ||
               (o is string &&
                    ((string)o).ToLower().Equals("false")))
            {
                return false;
            }
            if (o.Equals(true) ||
                o.Equals(True) ||
               (o is string &&
                    ((string)o).ToLower().Equals("true")))
            {
                return true;
            }
            throw new JSONException("JSONObject[" + Quote(key) +"] is not a Boolean.");
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the double value associated with a key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The numeric value.</returns>
        public double GetDouble(string key)
        {
            object o = Get(key);
            if (o is byte)
            {
                return (byte)o;
            }
            if (o is short)
            {
                return (short)o;
            }
            if (o is int)
            {
                return (int)o;
            }
            if (o is long)
            {
                return (long)o;
            }
            if (o is float)
            {
                return (float)o;
            }
            if (o is double)
            {
                return ((double)o);
            }
            if (o is string)
            {
                try
                {
                    return double.Parse((string)o);
                }
                catch (Exception)
                {
                    throw new JSONException("JSONObject[" + Quote(key) +
                        "] is not a number.");
                }
            }
            throw new JSONException("JSONObject[" + Quote(key) +
                "] is not a number.");
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the int value associated with a key. If the number value is too
        /// large for an int, it will be clipped.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The integer value</returns>
        public int GetInt(string key)
        {
            var o = Get(key);
            if (o is byte)
            {
                return ((byte)o);
            }
            if (o is short)
            {
                return ((short)o);
            }
            if (o is int)
            {
                return ((int)o);
            }
            if (o is long)
            {
                return (int)((long)o);
            }
            if (o is float)
            {
                return (int)((float)o);
            }
            if (o is double)
            {
                return (int)((double)o);
            }
            if (o is string)
            {
                return (int)GetDouble(key);
            }
            throw new JSONException("JSONObject[" + Quote(key) +
                "] is not a number.");
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the JSONArray value associated with a key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A JSONArray which is the value.</returns>
        public JSONArray GetJSONArray(string key)
        {
            var o = Get(key);
            if (o is JSONArray)
            {
                return (JSONArray)o;
            }
            throw new JSONException("JSONObject[" + Quote(key) +
                    "] is not a JSONArray.");
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the JSONObject value associated with a key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A JSONObject which is the value</returns>
        public JSONObject GetJSONObject(string key)
        {
            var o = Get(key);
            if (o is JSONObject)
            {
                return (JSONObject)o;
            }
            throw new JSONException("JSONObject[" + Quote(key) +
                    "] is not a JSONObject.");
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the long value associated with a key. If the number value is too
        /// long for a long, it will be clipped.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The long value</returns>
        public long GetLong(string key)
        {
            var o = Get(key);
            if (o is byte)
            {
                return ((byte)o);
            }
            if (o is short)
            {
                return ((short)o);
            }
            if (o is int)
            {
                return ((int)o);
            }
            if (o is long)
            {
                return ((long)o);
            }
            if (o is float)
            {
                return (long)((float)o);
            }
            if (o is double)
            {
                return (long)((double)o);
            }
            if (o is string)
            {
                return (long)GetDouble(key);
            }
            throw new JSONException("JSONObject[" + Quote(key) +
                "] is not a number.");
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the string associated with a key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A string which is the value.</returns>
        public string GetString(string key)
        {
            return Get(key).ToString();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determine if the JSONObject contains a specific key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// 	true if the key exists in the JSONObject.
        /// </returns>
        public bool Has(string key)
        {
            return _myHashMap.ContainsKey(key);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determine if the value associated with the key is null or if there is
        /// no value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// 	 true if there is no value associated with the key or if
        /// the value is the JSONObject.NULL object.
        /// </returns>
        public bool IsNull(string key)
        {
            return NULL.Equals(Opt(key));
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get an enumeration of the keys of the JSONObject.
        /// </summary>
        /// <returns>An iterator of the keys</returns>
        public ICollection Keys()
        {
            return _myHashMap.Keys;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the number of keys stored in the JSONObject.
        /// </summary>
        /// <returns>The number of keys in the JSONObject.</returns>
        public int Length()
        {
            return _myHashMap.Count;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Produce a JSONArray containing the names of the elements of this
        /// JSONObject.
        /// </summary>
        /// <returns>
        /// A JSONArray containing the key strings, or null if the JSONObject is empty.
        /// </returns>
        public JSONArray Names()
        {
            var ja = new JSONArray();
            var keyset = Keys();
            foreach (var o in keyset)
            {
                ja.Put(o);
            }
            return ja.Length() == 0 ? null : ja;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Shave off trailing zeros and decimal point, if possible.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <returns>trimmed number</returns>
        public static string TrimNumber(string s)
        {
            if (s.IndexOf('.') > 0 && s.IndexOf('e') < 0 && s.IndexOf('E') < 0)
            {
                while (s.EndsWith("0"))
                {
                    s = s.Substring(0, s.Length - 1);
                }
                if (s.EndsWith("."))
                {
                    s = s.Substring(0, s.Length - 1);
                }
            }
            return s;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Produce a string from a Number.
        /// </summary>
        /// <param name="n">A Number.</param>
        /// <returns>A String</returns>
        static public string NumberToString(object n)
        {
            if (n == null)
            {
                throw new JSONException("Null pointer");
            }
            TestValidity(n);
            return TrimNumber(n.ToString());
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get an optional value associated with a key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>An object which is the value, or null if there is no value.</returns>
        public object Opt(string key)
        {
            return key == null ? null : _myHashMap[key];
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get an optional boolean associated with a key.
        /// It returns false if there is no such key, or if the value is not
        /// Boolean.TRUE or the String "true".
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The truth</returns>
        public bool OptBoolean(string key)
        {
            return OptBoolean(key, false);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get an optional boolean associated with a key.
        /// It returns the defaultValue if there is no such key, or if it is not
        /// a Boolean or the String "true" or "false" (case insensitive).
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The truth</returns>
        public bool OptBoolean(string key, bool defaultValue)
        {
            try
            {
                return GetBoolean(key);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Put a key/value pair in the JSONObject, where the value will be a
        /// JSONArray which is produced from a Collection.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">A Collection value.</param>
        /// <returns>this</returns>
        public JSONObject Put(string key, ArrayList value)
        {
            Put(key, new JSONArray(value));
            return this;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get an optional double associated with a key,
        /// or NaN if there is no such key or if its value is not a number.
        /// If the value is a string, an attempt will be made to evaluate it as
        /// a number.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>An object which is the value</returns>
        public double OptDouble(string key)
        {
            return OptDouble(key, Double.NaN);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Get an optional double associated with a key, or the
        /// defaultValue if there is no such key or if its value is not a number.
        /// If the value is a string, an attempt will be made to evaluate it as
        /// a number.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>An object which is the value</returns>
        public double OptDouble(string key, double defaultValue)
        {
            try
            {
                var o = Opt(key);
                return double.Parse((string)o);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get an optional int value associated with a key,
        /// or zero if there is no such key or if the value is not a number.
        /// If the value is a string, an attempt will be made to evaluate it as
        /// a number.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>An object which is the value</returns>
        public int OptInt(string key)
        {
            return OptInt(key, 0);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get an optional int value associated with a key,
        /// or the default if there is no such key or if the value is not a number.
        /// If the value is a string, an attempt will be made to evaluate it as
        /// a number.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>An object which is the value</returns>
        public int OptInt(string key, int defaultValue)
        {
            try
            {
                return GetInt(key);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get an optional JSONArray associated with a key.
        /// It returns null if there is no such key, or if its value is not a JSONArray.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A JSONArray which is the value</returns>
        public JSONArray OptJSONArray(string key)
        {
            var o = Opt(key);
            return o is JSONArray ? (JSONArray)o : null;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get an optional JSONObject associated with a key.
        /// It returns null if there is no such key, or if its value is not a
        /// JSONObject.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A JSONObject which is the value</returns>
        public JSONObject OptJSONObject(string key)
        {
            var o = Opt(key);
            return o is JSONObject ? (JSONObject)o : null;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get an optional long value associated with a key,
        /// or zero if there is no such key or if the value is not a number.
        /// If the value is a string, an attempt will be made to evaluate it as
        /// a number.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public long OptLong(string key)
        {
            return OptLong(key, 0);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get an optional long value associated with a key,
        /// or the default if there is no such key or if the value is not a number.
        /// If the value is a string, an attempt will be made to evaluate it as
        /// a number.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>An object which is the value</returns>
        public long OptLong(string key, long defaultValue)
        {
            try
            {
                return GetLong(key);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get an optional string associated with a key.
        /// It returns an empty string if there is no such key. If the value is not
        /// a string and is not null, then it is coverted to a string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A string which is the value</returns>
        public string OptString(string key)
        {
            return OptString(key, "");
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get an optional string associated with a key.
        /// It returns the defaultValue if there is no such key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>A string which is the value</returns>
        public string OptString(string key, string defaultValue)
        {
            var o = Opt(key);
            return o != null ? o.ToString() : defaultValue;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Put a key/boolean pair in the JSONObject.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">A boolean which is the value.</param>
        /// <returns>this</returns>
        public JSONObject Put(string key, bool value)
        {
            Put(key, value ? true : false);
            return this;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Put a key/double pair in the JSONObject.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">A double which is the value.</param>
        /// <returns>this</returns>
        public JSONObject Put(string key, double value)
        {
            Put(key, value);
            return this;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Put a key/int pair in the JSONObject.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">An int which is the value.</param>
        /// <returns>this</returns>
        public JSONObject Put(string key, int value)
        {
            Put(key, value);
            return this;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Put a key/long pair in the JSONObject.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">A long which is the value</param>
        /// <returns>this</returns>
        public JSONObject Put(string key, long value)
        {
            Put(key, value);
            return this;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Put a key/value pair in the JSONObject, where the value will be a
        /// JSONObject which is produced from a Map.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The Map value.</param>
        /// <returns>this</returns>
        public JSONObject Put(string key, Hashtable value)
        {
            Put(key, new JSONObject(value));
            return this;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Put a key/value pair in the JSONObject. If the value is null,
        /// then the key will be removed from the JSONObject if it is present.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value"> An object which is the value. It should be of one of these
        /// types: Boolean, Double, Integer, JSONArray, JSONObject, Long, String,
        /// or the JSONObject.NULL object.</param>
        /// <returns>this</returns>
        public JSONObject Put(string key, object value)
        {
            if (key == null)
            {
                throw new JSONException("Null key.");
            }
            if (value != null)
            {
                TestValidity(value);
                _myHashMap[key]= value;
            }
            else
            {
                Remove(key);
            }
            return this;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Put a key/value pair in the JSONObject, but only if the
        /// key and the value are both non-null.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">An object which is the value. It should be of one of these
        /// types: Boolean, Double, Integer, JSONArray, JSONObject, Long, String,
        /// or the JSONObject.NULL object.</param>
        /// <returns>this</returns>
        public JSONObject PutOpt(string key, object value)
        {
            if (key != null && value != null)
            {
                Put(key, value);
            }
            return this;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Produce a string in double quotes with backslash sequences in all the
        /// right places. A backslash will be inserted within </, allowing JSON
        /// text to be delivered in HTML. In JSON text, a string cannot contain a
        /// control character or an unescaped quote or backslash.
        /// </summary>
        /// <param name="str">The String.</param>
        /// <returns>A String correctly formatted for insertion in a JSON text</returns>
        public static string Quote(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "\"\"";
            }

            var c = '\0';
            int i;
            var len = str.Length;
            var sb = new StringBuilder(len + 4);
            string t;

            sb.Append('"');
            for (i = 0; i < len; i += 1)
            {
                var b = c;
                c = str[i];
                switch (c)
                {
                    case '\\':
                    case '"':
                        sb.Append('\\');
                        sb.Append(c);
                        break;
                    case '/':
                        if (b == '<')
                        {
                            sb.Append('\\');
                        }
                        sb.Append(c);
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    default:
                        if (c < ' ')
                        {
                            t = "000" + BitConverter.ToString(BitConverter.GetBytes(c));
                            sb.Append("\\u" + t.Substring(t.Length - 4));
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }
            sb.Append('"');
            return sb.ToString();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Remove a name and its value, if present.
        /// </summary>
        /// <param name="key"> The name to be removed.</param>
        /// <returns>that was associated with the name,
        /// or null if there was no value</returns>
        public object Remove(string key)
        {
            var obj = _myHashMap[key];
            _myHashMap.Remove(key);
            return obj;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Throw an exception if the object is an NaN or infinite number.
        /// </summary>
        /// <param name="o">The object to test.</param>
        internal static void TestValidity(object o)
        {
            if (o != null)
            {
                if (o is double)
                {

                    if (double.IsInfinity((double)o) || double.IsInfinity((double)o))
                    {
                        throw new JSONException(
                            "JSON does not allow non-finite numbers");
                    }
                }
                else if (o is float)
                {
                    if (float.IsNaN((float)o) || float.IsInfinity((float)o))
                    {
                        throw new JSONException(
                            "JSON does not allow non-finite numbers.");
                    }
                }

            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Produce a JSONArray containing the values of the members of this
        /// JSONObject.
        /// </summary>
        /// <param name="names">A JSONArray containing a list of key strings. This
        /// determines the sequence of the values in the result.</param>
        /// <returns>A JSONArray of values</returns>
        public JSONArray ToJSONArray(JSONArray names)
        {
            if (names == null || names.Length() == 0)
            {
                return null;
            }
            var ja = new JSONArray();
            for (var i = 0; i < names.Length(); i += 1)
            {
                ja.Put(Opt(names.GetString(i)));
            }
            return ja;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///Make a JSON text of this JSONObject. For compactness, no whitespace
        /// is added. If this would not result in a syntactically correct JSON text,
        /// then null will be returned instead.
        /// 
        /// Warning: This method assumes that the data structure is acyclical.*
        /// </summary>
        /// <returns>
        /// a printable, displayable, portable, transmittable
        ///  representation of the object, beginning
        /// with {<small>(left brace)</small> and ending
        /// with }<small>(right brace)</small>.
        /// </returns>
        public override string ToString()
        {
            try
            {
                var keyset = Keys();
                var sb = new StringBuilder("{");
                foreach (var o in keyset)
                {
                    if (sb.Length > 1)
                    {
                        sb.Append(',');
                    }
                    sb.Append(Quote(o.ToString()));
                    sb.Append(':');
                    sb.Append(ValueToString(_myHashMap[o]));
                }


                sb.Append('}');
                return sb.ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Make a prettyprinted JSON text of this JSONObject.
        /// 
        /// Warning: This method assumes that the data structure is acyclical.
        /// </summary>
        /// <param name="indentFactor">The number of spaces to add to each level of
        /// indentation.</param>
        /// <returns>
        /// a printable, displayable, portable, transmittable
        /// representation of the object, beginning
        /// with {<small>(left brace)</small> and ending
        /// with }<small>(right brace)</small>.
        /// </returns>
        public string ToString(int indentFactor)
        {
            return ToString(indentFactor, 0);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Write the contents of the JSONObject as JSON text to a writer.
        /// For compactness, no whitespace is added.
        /// 
        /// Warning: This method assumes that the data structure is acyclical.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <returns>The writer</returns>
        public StringWriter Write(StringWriter writer)
        {
            try
            {
                var b = false;
                var keyset = Keys();
                writer.Write('{');
                foreach (var k in keyset)
                {
                    if (b)
                    {
                        writer.Write(',');
                    }

                    writer.Write(Quote(k.ToString()));
                    writer.Write(':');
                    var v = _myHashMap[k];
                    if (v is JSONObject)
                    {
                        ((JSONObject)v).Write(writer);
                    }
                    else if (v is JSONArray)
                    {
                        ((JSONArray)v).Write(writer);
                    }
                    else
                    {
                        writer.Write(ValueToString(v));
                    }
                    b = true;
                }


                writer.Write('}');
                return writer;
            }
            catch (IOException e)
            {
                throw new JSONException(e.Message);
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
            var tokens = new JSONPathTokenizer(path).Tokenize();
            var obj = Apply(this, tokens, 0);
            return obj == null ? false : obj.OptBoolean((string)tokens[tokens.Count-1]);
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
            var tokens = new JSONPathTokenizer(path).Tokenize();
            var obj = Apply(this, tokens, 0);
            return obj == null ? 0 : obj.OptInt((string)tokens[tokens.Count-1]);
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
            var tokens = new JSONPathTokenizer(path).Tokenize();
            var obj = Apply(this, tokens, 0);
            return obj == null ? 0 : obj.OptLong((string)tokens[tokens.Count-1]);
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
            var tokens = new JSONPathTokenizer(path).Tokenize();
            var obj = Apply(this, tokens, 0);
            return obj == null ? 0 : obj.OptDouble((string)tokens[tokens.Count-1]);
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
            var tokens = new JSONPathTokenizer(path).Tokenize();
            var obj = Apply(this, tokens, 0);
            return obj == null ? null : obj.OptString((string)tokens[tokens.Count-1]);
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
            var array = GetAsArray(path);
            return array == null ? 0 : array.Length();
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
            var jarr = GetAsArray(path);
            var arr = new string[jarr == null ? 0 : jarr.Length()];
            for (var i = 0; i < arr.Length; i++)
            {
                if (jarr != null) arr[i] = (string)jarr.Opt(i);
            }
            return arr;
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
            var jarr = GetAsArray(path);
            var arr = new int[jarr == null ? 0 : jarr.Length()];
            for (var i = 0; i < arr.Length; i++)
            {
                if (jarr != null) arr[i] = ((int)jarr.Opt(i));
            }
            return arr;
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
            var jarr = GetAsArray(path);
            var arr = new double[jarr == null ? 0 : jarr.Length()];
            for (var i = 0; i < arr.Length; i++)
            {
                if (jarr != null) arr[i] = ((double)jarr.Opt(i));
            }
            return arr;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Convert a well-formed (but not necessarily valid) XML string into a 
        /// JSONObject. Some information may be lost in this transformation because 
        /// JSON is a data format and XML is a document format. XML uses elements,
        /// attributes, and content text, while JSON uses unordered collections of 
        /// name/value pairs and arrays of values. JSON does not does not like to 
        /// distinguish between elements and attributes. Sequences of similar 
        /// elements are represented as JSONArrays. Content text may be 
        /// placed in a "content" member. Comments, prologs, DTDs, 
        /// and <[ [ ]]> are ignored. 
        /// </summary>
        /// <param name="xmlString">The XML string.</param>
        /// <returns>a JSONObject converted from the XML</returns>
        public static JSONObject FromXmlString(string xmlString)
        {
            return JSONXML.ToJSONObject(xmlString);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Convert this JSONObject into a well-formed, element-normal XML string.
        /// </summary>
        /// <returns>a XML string</returns>
        public string ToXmlString()
        {
            return JSONXML.ToString(this);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Convert this JSONObject into a well-formed, element-normal XML string.
        /// </summary>
        /// <param name="tagName">The optional name of the enclosing tag.</param>
        /// <returns> a XML string</returns>
        public string ToXmlString(string tagName)
        {
            return JSONXML.ToString(this, tagName);
        }

        /**
         * JSONObject.NULL is equivalent to the value that JavaScript calls null,
         * whilst Java's null is equivalent to the value that JavaScript calls
         * undefined.
         */
        private class Null
        {
            /**
             * Get the "null" string value.
             * @return The string "null".
             */
            public override string ToString()
            {
                return "null";
            }
        }


        /**
         * The hash map where the JSONObject's properties are kept.
         */
        private readonly Hashtable _myHashMap;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Construct a JSONObject from a JSONTokener.
        /// </summary>
        /// <param name="x">object containing the source string</param>
        internal JSONObject(JSONTokener x): this()
        {
            if (x.NextClean() != '{')
            {
                throw x.SyntaxError("A JSONObject text must begin with '{'");
            }
            for (; ; )
            {
                var c = x.NextClean();
                string key;
                switch (c)
                {
                    case '\0':
                        throw x.SyntaxError("A JSONObject text must end with '}'");
                    case '}':
                        return;
                    default:
                        x.Back();
                        key = x.NextValue().ToString();
                        break;
                }

                /*
                 * The key is followed by ':'. We will also tolerate '=' or '=>'.
                 */

                c = x.NextClean();
                if (c == '=')
                {
                    if (x.Next() != '>')
                    {
                        x.Back();
                    }
                }
                else if (c != ':')
                {
                    throw x.SyntaxError("Expected a ':' after a key");
                }
                Put(key, x.NextValue());

                /*
                 * Pairs are separated by ','. We will also tolerate ';'.
                 */

                switch (x.NextClean())
                {
                    case ';':
                    case ',':
                        if (x.NextClean() == '}')
                        {
                            return;
                        }
                        x.Back();
                        break;
                    case '}':
                        return;
                    default:
                        throw x.SyntaxError("Expected a ',' or '}'");
                }
            }
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Make a prettyprinted JSON text of this JSONObject.
        /// Warning: This method assumes that the data structure is acyclical.
        /// </summary>
        /// <param name="indentFactor">The number of spaces to add to each level of
        /// indentation.</param>
        /// <param name="indent">The indentation of the top level</param>
        /// <returns>
        /// a printable, displayable, transmittable
        /// representation of the object, beginning
        /// with {<small>(left brace)</small> and ending
        /// with }<small>(right brace)</small>.
        /// </returns>
        string ToString(int indentFactor, int indent)
        {
            var n = Length();
            if (n == 0)
            {
                return "{}";
            }
            var keyset = Keys();
            var sb = new StringBuilder("{");
            var newindent = indent + indentFactor;

            if (n == 1)
            {
                foreach (var o in keyset)
                {
                    sb.Append(Quote(o.ToString()));
                    sb.Append(": ");
                    sb.Append(ValueToString(_myHashMap[o], indentFactor,
                            indent));
                }

            }
            else
            {
                int i;
                foreach (var o in keyset)
                {

                    if (sb.Length > 1)
                    {
                        sb.Append(",\n");
                    }
                    else
                    {
                        sb.Append('\n');
                    }
                    for (i = 0; i < newindent; i += 1)
                    {
                        sb.Append(' ');
                    }
                    sb.Append(Quote(o.ToString()));
                    sb.Append(": ");
                    sb.Append(ValueToString(_myHashMap[o], indentFactor,
                            newindent));
                }


                if (sb.Length > 1)
                {
                    sb.Append('\n');
                    for (i = 0; i < indent; i += 1)
                    {
                        sb.Append(' ');
                    }
                }
            }
            sb.Append('}');
            return sb.ToString();
        }



        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// * Make a JSON text of an Object value. If the object has an
        /// value.toJSONString() method, then that method will be used to produce
        /// the JSON text. The method is required to produce a strictly
        /// conforming text. If the object does not contain a toJSONString
        /// method (which is the most common case), then a text will be
        /// produced by the rules.
        /// </summary>
        /// <param name="value">value The value to be serialized.</param>
        /// <returns>
        /// a printable, displayable, transmittable
        /// representation of the object, beginning
        /// with {<small>(left brace)</small> and ending
        /// with }<small>(right brace)</small>.
        /// </returns>
        internal static string ValueToString(object value)
        {
            if (value == null || value.Equals(null))
            {
                return "null";
            }
            if (value is IJSONString)
            {
                object o;
                try
                {
                    o = ((IJSONString)value).ToJSONString();
                }
                catch (Exception e)
                {
                    throw new JSONException(e.Message);
                }
                if (o is string)
                {
                    return (string)o;
                }
                throw new JSONException("Bad value from toJSONString: " + o);
            }

            if (value is float || value is double ||
                value is byte || value is short ||
                value is int || value is long)
            {
                return NumberToString(value);
            }
            if (value is bool || value is JSONObject ||
                    value is JSONArray)
            {
                return value.ToString();
            }
            return Quote(value.ToString());
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Make a prettyprinted JSON text of an object value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="indentFactor">The number of spaces to add to each level of
        /// indentation.</param>
        /// <param name="indent">The indentation of the top level</param>
        /// <returns>
        /// a printable, displayable, transmittable
        /// representation of the object, beginning
        /// with {<small>(left brace)</small> and ending
        /// with }<small>(right brace)</small>.
        /// </returns>
        internal static string ValueToString(object value, int indentFactor, int indent)
        {
            if (value == null || value.Equals(null))
            {
                return "null";
            }
            try
            {
                if (value is IJSONString)
                {
                    object o = ((IJSONString)value).ToJSONString();
                    if (o is string)
                    {
                        return (string)o;
                    }
                }
            }
            catch (Exception)
            {
                /* forget about it */
            }

            if (value is float || value is double ||
                value is byte || value is short ||
                value is int || value is long)
            {
                return NumberToString(value);
            }
            if (value is bool)
            {
                return value.ToString();
            }
            if (value is JSONObject)
            {
                return ((JSONObject)value).ToString(indentFactor, indent);
            }
            if (value is JSONArray)
            {
                return ((JSONArray)value).ToString(indentFactor, indent);
            }
            return Quote(value.ToString());
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Applies the specified start.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="tokens">The tokens.</param>
        /// <param name="firstToken">The first token.</param>
        /// <returns></returns>
        internal static JSONObject Apply(JSONObject start, ArrayList tokens,
                 int firstToken)
        {

            if (start == null)
            {
                return null;
            }

            var nTokens = tokens.Count;
            if (firstToken >= nTokens)
            {
                return start;
            }

            for (var i = firstToken; i < nTokens; i++)
            {
                var tok1 = (string)tokens[i];
                if (tok1.Length == 1 && JSONPathTokenizer.IsDelimiter(tok1[0]))
                {
                    throw new JSONException("Syntax error: path cannot start with a delimiter: " + tok1);
                }

                if (i + 1 >= nTokens)
                {
                    return start;
                }
                var tok2 = (string)tokens[i + 1];
                var t2 = tok2[0];
                switch (t2)
                {
                    case Separator:
                        return Apply(start.OptJSONObject(tok1), tokens, i + 2);

                    case ArrayStart:
                        if (i + 2 >= nTokens)
                        {
                            throw new JSONException("Syntax error: array must be followed by a dimension: " + tok1);
                        }
                        var tok3 = (string)tokens[i + 2];
                        int dim;
                        try
                        {
                            dim = int.Parse(tok3);
                        }
                        catch (FormatException)
                        {
                            throw new JSONException("Syntax error: illegal array dimension: " + tok3);
                        }
                        if (i + 3 >= nTokens)
                        {
                            throw new JSONException("Syntax error: array dimension must be closed: " + tok3);
                        }
                        var tok4 = (string)tokens[i + 3];
                        if (tok4.Length != 1 && tok4[0] != ArrayEnd)
                        {
                            throw new JSONException("Syntax error: illegal close of array dimension: " + tok4);
                        }
                        if (i + 4 >= nTokens)
                        {
                            throw new JSONException("Syntax error: array close must be followed by a separator: " + tok1);
                        }
                        var tok5 = (string)tokens[i + 4];
                        if (tok5.Length != 1 && tok5[0] != Separator)
                        {
                            throw new JSONException("Syntax error: illegal separator after array: " + tok4);
                        }
                        i += 4;
                        var array = start.OptJSONArray(tok1);
                        return array == null ? null : Apply((JSONObject)array.Opt(dim), tokens, i + 1);
                }
            }

            return start;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets as array.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public override JSONArray GetAsArray(string path)
        {
            var tokens = new JSONPathTokenizer(path).Tokenize();

            var obj = Apply(this, tokens, 0);
            return obj == null ? null : obj.OptJSONArray((string)tokens[tokens.Count - 1]);
        }


    }

}
