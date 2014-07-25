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
    /// A JSONArray is an ordered sequence of values. Its external text form is a
    /// string wrapped in square brackets with commas separating the values. The
    /// internal form is an object having get and opt
    /// methods for accessing the values by index, and put methods for
    /// adding or replacing values. The values can be any of these types:
    /// Boolean, JSONArray, JSONObject,
    /// Number, String, or the
    /// JSONObject.NULL object.
    /// 
    /// The constructor can convert a JSON text into a Java object. The
    /// toString method converts to JSON text.
    /// 
    /// A get method returns a value if one can be found, and throws an
    /// exception if one cannot be found. An opt method returns a
    /// default value instead of throwing an exception, and so is useful for
    /// obtaining optional values.
    /// 
    /// The generic get() and opt() methods return an
    /// object which you can cast or query for type. There are also typed
    /// get and opt methods that do type checking and type
    /// coersion for you.
    /// 
    /// The texts produced by the toString methods strictly conform to
    /// JSON syntax rules. The constructors are more forgiving in the texts they will
    /// accept:
    /// <ul>
    /// <li>An extra ,<small>(comma)</small> may appear just
    /// before the closing bracket.</li>
    /// <li>The null value will be inserted when there
    /// is ,<small>(comma)</small> elision.</li>
    /// <li>Strings may be quoted with '<small>(singlequote)
    /// </small>.</li>
    /// <li>Strings do not need to be quoted at all if they do not begin with a quote
    /// or single quote, and if they do not contain leading or trailing spaces,
    /// and if they do not contain any of these characters:
    /// { } [ ] / \ : , = ; # and if they do not look like numbers
    /// and if they are not the reserved words true,
    /// false, or null.</li>
    /// <li>Values can be separated by ; <small>(semicolon)</small> as
    /// well as by , <small>(comma)</small>.</li>
    /// <li>Numbers may have the 0- <small>(octal)</small> or
    /// 0x- <small>(hex)</small> prefix.</li>
    /// <li>Comments written in the slashshlash, slashstar, and hash conventions
    /// will be ignored.</li>
    /// </ul>
    /// </summary>
    public class JSONArray : JSONPath
    {
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="JSONArray"/> class.
        /// </summary>
        public JSONArray()
        {
            _myArrayList = new ArrayList();
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Construct a JSONArray from a source sJSON text.
        /// </summary>
        /// <param name="str">A string that begins with
        /// [<small>(left bracket)</small>
        /// and ends with ]<small>(right bracket)</small>.</param>
        public JSONArray(string str)
            : this(new JSONTokener(str))
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Construct a JSONArray from a Collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public JSONArray(IList collection)
        {
            if (collection == null)
            {
                _myArrayList = new ArrayList();
            }
            else
            {
                int size = collection.Count;
                _myArrayList = new ArrayList(size);
                for (int i = 0; i < size; i++)
                {
                    _myArrayList.Add(collection[i]);
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
        /// Get the object value associated with an index.
        /// </summary>
        /// <param name="index">The index must be between 0 and length() - 1</param>
        /// <returns>An object value.</returns>
        public object Get(int index)
        {
            var o = Opt(index);
            if (o == null)
            {
                throw new JSONException("JSONArray[" + index + "] not found.");
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
        /// Get the boolean value associated with an index..
        /// </summary>
        /// <param name="index">The index must be between 0 and length() - 1</param>
        /// <returns>The truth</returns>
        public bool GetBoolean(int index)
        {
            var o = Get(index);
            if (o.Equals(true) ||
                o.Equals(JSONObject.False) ||
                    (o is string &&
                    ((string)o).ToLower().Equals("false")))
            {
                return false;

            }
            if (o.Equals(true) ||
                       o.Equals(JSONObject.True) ||
                      (o is string &&
                      ((string)o).ToLower().Equals("true")))
            {
                return true;
            }
            throw new JSONException("JSONArray[" + index + "] is not a Boolean.");
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the double value associated with an index.
        /// </summary>
        /// <param name="index">The index must be between 0 and length() - 1</param>
        /// <returns>The value</returns>
        public double GetDouble(int index)
        {
            var o = Get(index);
            try
            {
                return double.Parse((string)o);
            }
            catch (Exception)
            {
                throw new JSONException("JSONArray[" + index + "] is not a number.");
            }
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the int value associated with an index.
        /// </summary>
        /// <param name="index">The index must be between 0 and length() - 1</param>
        /// <returns>The value</returns>
        public int GetInt(int index)
        {
            return (int)GetDouble(index);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the JSONArray associated with an index.
        /// </summary>
        /// <param name="index">The index must be between 0 and length() - 1</param>
        /// <returns>A JSONArray value</returns>
        public JSONArray GetJSONArray(int index)
        {
            var o = Get(index);
            if (o is JSONArray)
            {
                return (JSONArray)o;
            }
            throw new JSONException("JSONArray[" + index + "] is not a JSONArray.");
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the JSONObject associated with an index.
        /// </summary>
        /// <param name="index">The index must be between 0 and length() - 1</param>
        /// <returns>A JSONObject value.</returns>
        public JSONObject GetJSONObject(int index)
        {
            var o = Get(index);
            if (o is JSONObject)
            {
                return (JSONObject)o;
            }
            throw new JSONException("JSONArray[" + index + "] is not a JSONObject.");
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the long value associated with an index..
        /// </summary>
        /// <param name="index">The index must be between 0 and length() - 1</param>
        /// <returns>The value</returns>
        public long GetLong(int index)
        {
            return (long)GetDouble(index);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the string associated with an index.
        /// </summary>
        /// <param name="index">The index must be between 0 and length() - 1</param>
        /// <returns>A string value</returns>
        public string GetString(int index)
        {
            return Get(index).ToString();
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determine if the value is null.
        /// </summary>
        /// <param name="index">The index must be between 0 and length() - 1</param>
        /// <returns>
        /// 	<c>true</c> if the specified index is null; otherwise, <c>false</c>.
        /// </returns>
        public bool IsNull(int index)
        {
            return JSONObject.NULL.Equals(Opt(index));
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Make a string from the contents of this JSONArray. The
        /// separator string is inserted between each element.
        /// Warning: This method assumes that the data structure is acyclical.
        /// </summary>
        /// <param name="separator">A string that will be inserted between the elements</param>
        /// <returns>a string</returns>
        public string Join(string separator)
        {
            var len = Length();
            var sb = new StringBuilder();

            for (var i = 0; i < len; i += 1)
            {
                if (i > 0)
                {
                    sb.Append(separator);
                }
                sb.Append(JSONObject.ValueToString(_myArrayList[i]));
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
        /// Get the number of elements in the JSONArray, included nulls.
        /// </summary>
        /// <returns>The length (or size)</returns>
        public int Length()
        {
            return _myArrayList.Count;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the optional object value associated with an index.
        /// </summary>
        /// <param name="index">The index must be between 0 and length() - 1.</param>
        /// <returns>
        /// An object value, or null if there is no object at that index.
        /// </returns>
        public object Opt(int index)
        {
            return (index < 0 || index >= Length()) ?
                null : _myArrayList[index];
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Get the optional boolean value associated with an index.
        /// It returns false if there is no value at that index,
        /// or if the value is not Boolean.TRUE or the String "true".
        /// </summary>
        /// <param name="index">The index must be between 0 and length() - 1</param>
        /// <returns>The truth</returns>
        public bool OptBoolean(int index)
        {
            return OptBoolean(index, false);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the optional boolean value associated with an index.
        /// It returns the defaultValue if there is no value at that index or if
        /// it is not a Boolean or the String "true" or "false" (case insensitive).
        /// </summary>
        /// <param name="index">The index must be between 0 and length() - 1</param>
        /// <param name="defaultValue">A boolean default.</param>
        /// <returns>The truth</returns>
        public bool OptBoolean(int index, bool defaultValue)
        {
            try
            {
                return GetBoolean(index);
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
        /// Get the optional double value associated with an index.
        /// NaN is returned if there is no value for the index,
        /// or if the value is not a number and cannot be converted to a number.
        /// </summary>
        /// <param name="index">The index must be between 0 and length() - 1</param>
        /// <returns>The value</returns>
        public double OptDouble(int index)
        {
            return OptDouble(index, Double.NaN);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the optional double value associated with an index.
        /// The defaultValue is returned if there is no value for the index,
        /// or if the value is not a number and cannot be converted to a number.
        /// </summary>
        /// <param name="index">The index must be between 0 and length() - 1</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The value</returns>
        public double OptDouble(int index, double defaultValue)
        {
            try
            {
                return GetDouble(index);
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
        /// Get the optional int value associated with an index.
        /// Zero is returned if there is no value for the index,
        /// or if the value is not a number and cannot be converted to a number.
        /// </summary>
        /// <param name="index">The index must be between 0 and length() - 1</param>
        /// <returns>The value</returns>
        public int OptInt(int index)
        {
            return OptInt(index, 0);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the optional int value associated with an index.
        /// The defaultValue is returned if there is no value for the index,
        /// or if the value is not a number and cannot be converted to a number.
        /// </summary>
        /// <param name="index">The index must be between 0 and length() - 1</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The value</returns>
        public int OptInt(int index, int defaultValue)
        {
            try
            {
                return GetInt(index);
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
        /// Get the optional JSONArray associated with an index..
        /// </summary>
        /// <param name="index">The index must be between 0 and length() - 1</param>
        /// <returns>
        /// A JSONArray value, or null if the index has no value,
        /// or if the value is not a JSONArray
        /// </returns>
        public JSONArray OptJSONArray(int index)
        {
            var o = Opt(index);
            return o is JSONArray ? (JSONArray)o : null;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the optional JSONObject associated with an index.
        /// Null is returned if the key is not found, or null if the index has
        /// no value, or if the value is not a JSONObject.
        /// </summary>
        /// <param name="index">The index must be between 0 and length() - 1</param>
        /// <returns>A JSONObject value</returns>
        public JSONObject OptJSONObject(int index)
        {
            var o = Opt(index);
            return o is JSONObject ? (JSONObject)o : null;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Get the optional long value associated with an index.
        ///  Zero is returned if there is no value for the index,
        /// or if the value is not a number and cannot be converted to a number.
        /// </summary>
        /// <param name="index">The index must be between 0 and length() - 1</param>
        /// <returns>The value</returns>
        public long OptLong(int index)
        {
            return OptLong(index, 0);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the optional long value associated with an index.
        /// The defaultValue is returned if there is no value for the index,
        /// or if the value is not a number and cannot be converted to a number.
        /// </summary>
        /// <param name="index">The index must be between 0 and length() - 1</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The value</returns>
        public long OptLong(int index, long defaultValue)
        {
            try
            {
                return GetLong(index);
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
        /// Get the optional string value associated with an index. It returns an
        /// empty string if there is no value at that index. If the value
        /// is not a string and is not null, then it is coverted to a string.
        /// </summary>
        /// <param name="index">The index must be between 0 and length() - 1</param>
        /// <returns>A String value</returns>
        public string OptString(int index)
        {
            return OptString(index, "");
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the optional string associated with an index.
        /// The defaultValue is returned if the key is not found.
        /// </summary>
        /// <param name="index">The index must be between 0 and length() - 1</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>A String value</returns>
        public string OptString(int index, string defaultValue)
        {
            var o = Opt(index);
            return o != null ? o.ToString() : defaultValue;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Append a boolean value. This increases the array's length by one.
        /// </summary>
        /// <param name="value">A boolean value</param>
        /// <returns>this</returns>
        public JSONArray Put(bool value)
        {
            Put(value ? true : false);
            return this;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Put a value in the JSONArray, where the value will be a
        /// JSONArray which is produced from a Collection.
        /// </summary>
        /// <param name="value">A Collection value</param>
        /// <returns>this</returns>
        public JSONArray Put(ArrayList value)
        {
            Put(new JSONArray(value));
            return this;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Append a double value. This increases the array's length by one.
        /// </summary>
        /// <param name="value">A double value</param>
        /// <returns>this</returns>
        public JSONArray Put(double value)
        {
            JSONObject.TestValidity(value);
            Put(value);
            return this;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Append an int value. This increases the array's length by one.
        /// </summary>
        /// <param name="value">An int value</param>
        /// <returns>this</returns>
        public JSONArray Put(int value)
        {
            Put(value);
            return this;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Append an long value. This increases the array's length by one.
        /// </summary>
        /// <param name="value">The long value.</param>
        /// <returns>this</returns>
        public JSONArray Put(long value)
        {
            Put(value);
            return this;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Put a value in the JSONArray, where the value will be a
        /// JSONObject which is produced from a Map.
        /// </summary>
        /// <param name="value">The Map value.</param>
        /// <returns>this</returns>
        public JSONArray Put(Hashtable value)
        {
            Put(new JSONObject(value));
            return this;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Append an object value. This increases the array's length by one.
        /// </summary>
        /// <param name="value">An object value.  The value should be a
        /// Boolean, Double, Integer, JSONArray, JSONObject, Long, or String, or the
        /// JSONObject.NULL object.</param>
        /// <returns>this</returns>
        public JSONArray Put(object value)
        {
            _myArrayList.Add(value);
            return this;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Put or replace a boolean value in the JSONArray. If the index is greater
        /// than the length of the JSONArray, then null elements will be added as
        /// necessary to pad it out.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="value">A boolean value</param>
        /// <returns>this</returns>
        public JSONArray Put(int index, bool value)
        {
            Put(index, value ? true : false);
            return this;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Put a value in the JSONArray, where the value will be a
        /// JSONArray which is produced from a Collection.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="value">A Collection value.</param>
        /// <returns>this</returns>
        public JSONArray Put(int index, ArrayList value)
        {
            Put(index, new JSONArray(value));
            return this;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Put or replace a double value. If the index is greater than the length of
        /// the JSONArray, then null elements will be added as necessary to pad it out.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="value">A double value</param>
        /// <returns>this</returns>
        public JSONArray Put(int index, double value)
        {
            Put(index, value);
            return this;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Put or replace an int value. If the index is greater than the length of
        /// the JSONArray, then null elements will be added as necessary to pad it out.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="value">An int value.</param>
        /// <returns>this</returns>
        public JSONArray Put(int index, int value)
        {
            Put(index, value);
            return this;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Put or replace a long value. If the index is greater than the length of
        /// the JSONArray, then null elements will be added as necessary to pad it out.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="value">The long value.</param>
        /// <returns>this</returns>
        public JSONArray Put(int index, long value)
        {
            Put(index, value);
            return this;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Put a value in the JSONArray, where the value will be a
        /// JSONObject which is produced from a Map.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="value">The Map value.</param>
        /// <returns>this</returns>
        public JSONArray Put(int index, Hashtable value)
        {
            Put(index, new JSONObject(value));
            return this;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Put or replace an object value in the JSONArray. If the index is greater
        /// than the length of the JSONArray, then null elements will be added as
        /// necessary to pad it out.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="value">The value to put into the array. The value should be a
        ///  Boolean, Double, Integer, JSONArray, JSONObject, Long, or String, or the
        /// JSONObject.NULL object.</param>
        /// <returns>this</returns>
        public JSONArray Put(int index, object value)
        {
            JSONObject.TestValidity(value);
            if (index < 0)
            {
                throw new JSONException("JSONArray[" + index + "] not found.");
            }
            if (index < Length())
            {
                _myArrayList[index] = value;
            }
            else
            {
                while (index != Length())
                {
                    Put(JSONObject.NULL);
                }
                Put(value);
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
        /// Produce a JSONObject by combining a JSONArray of names with the values
        /// of this JSONArray.
        /// </summary>
        /// <param name="names">A JSONArray containing a list of key strings. These will be
        /// paired with the values.</param>
        /// <returns> A JSONObject, or null if there are no names or if this JSONArray
        /// has no values.</returns>
        public JSONObject ToJSONObject(JSONArray names)
        {
            if (names == null || names.Length() == 0 || Length() == 0)
            {
                return null;
            }
            var jo = new JSONObject();
            for (int i = 0; i < names.Length(); i += 1)
            {
                jo.Put(names.GetString(i), Opt(i));
            }
            return jo;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Make a JSON text of this JSONArray. For compactness, no
        /// unnecessary whitespace is added. If it is not possible to produce a
        /// syntactically correct JSON text then null will be returned instead. This
        /// could occur if the array contains an invalid number.
        /// Warning: This method assumes that the data structure is acyclical.
        /// </summary>
        /// <returns>
        /// a printable, displayable, transmittable representation of the array
        /// </returns>
        public override string ToString()
        {
            try
            {
                return '[' + Join(",") + ']';
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
        ///  Make a prettyprinted JSON text of this JSONArray.
        /// Warning: This method assumes that the data structure is acyclical.
        /// </summary>
        /// <param name="indentFactor">The number of spaces to add to each level of
        /// indentation.</param>
        /// <returns>
        /// a printable, displayable, transmittable
        /// representation of the object, beginning
        /// with [<small>(left bracket)</small> and ending
        /// with ]<small>(right bracket)</small>.
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
        /// Write the contents of the JSONArray as JSON text to a writer.
        /// For compactness, no whitespace is added.
        /// Warning: This method assumes that the data structure is acyclical.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <returns>The writer.</returns>
        public StringWriter Write(StringWriter writer)
        {
            try
            {
                var b = false;
                var len = Length();

                writer.Write('[');

                for (var i = 0; i < len; i += 1)
                {
                    if (b)
                    {
                        writer.Write(',');
                    }
                    var v = _myArrayList[i];
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
                        writer.Write(JSONObject.ValueToString(v));
                    }
                    b = true;
                }
                writer.Write(']');
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
            return obj == null ? false : obj.OptBoolean((string)tokens[tokens.Count - 1]);
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
            return obj == null ? 0 : obj.OptInt((string)tokens[tokens.Count - 1]);
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
            return obj == null ? 0 : obj.OptLong((string)tokens[tokens.Count - 1]);
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
            return obj == null ? 0 : obj.OptDouble((string)tokens[tokens.Count - 1]);
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
            return obj == null ? null : obj.OptString((string)tokens[tokens.Count - 1]);
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
                if (jarr != null) arr[i] = jarr.Opt(i) as string;
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


        /**
         * The ArrayList where the JSONArray's properties are kept.
         */
        private readonly ArrayList _myArrayList;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="JSONArray"/> class.
        /// </summary>
        /// <param name="x">A JSONTokener</param>
        internal JSONArray(JSONTokener x)
            : this()
        {

            if (x.NextClean() != '[')
            {
                throw x.SyntaxError("A JSONArray text must start with '['");
            }
            if (x.NextClean() == ']')
            {
                return;
            }
            x.Back();
            for (; ; )
            {
                if (x.NextClean() == ',')
                {
                    x.Back();
                    _myArrayList.Add(null);
                }
                else
                {
                    x.Back();
                    _myArrayList.Add(x.NextValue());
                }
                switch (x.NextClean())
                {
                    case ';':
                    case ',':
                        if (x.NextClean() == ']')
                        {
                            return;
                        }
                        x.Back();
                        break;
                    case ']':
                        return;
                    default:
                        throw x.SyntaxError("Expected a ',' or ']'");
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
        /// Make a prettyprinted JSON text of this JSONArray.
        /// Warning: This method assumes that the data structure is acyclical.
        /// </summary>
        /// <param name="indentFactor">
        /// The number of spaces to add to each level of indentation.</param>
        /// <param name="indent">The indention of the top level.</param>
        /// <returns>
        /// a printable, displayable, transmittable representation of the array.
        /// </returns>
        internal string ToString(int indentFactor, int indent)
        {
            var len = Length();
            if (len == 0)
            {
                return "[]";
            }
            var sb = new StringBuilder("[");
            if (len == 1)
            {
                sb.Append(JSONObject.ValueToString(_myArrayList[0], indentFactor, indent));
            }
            else
            {
                int newindent = indent + indentFactor;
                sb.Append('\n');
                int i;
                for (i = 0; i < len; i += 1)
                {
                    if (i > 0)
                    {
                        sb.Append(",\n");
                    }
                    for (int j = 0; j < newindent; j += 1)
                    {
                        sb.Append(' ');
                    }
                    sb.Append(JSONObject.ValueToString(_myArrayList[i],
                            indentFactor, newindent));
                }
                sb.Append('\n');
                for (i = 0; i < indent; i += 1)
                {
                    sb.Append(' ');
                }
            }
            sb.Append(']');
            return sb.ToString();
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
        internal static JSONObject Apply(JSONArray start, ArrayList tokens, int firstToken)
        {

            if (start == null)
            {
                return null;
            }

            var nTokens = tokens.Count;
            for (var i = firstToken; i < nTokens; )
            {
                var tok1 = (string)tokens[i];
                var t1 = tok1[0];
                switch (t1)
                {
                    case Separator:
                        throw new JSONException("Syntax error: must start with an "
                                + "array: " + tok1);

                    case ArrayStart:
                        if (i + 1 >= nTokens)
                        {
                            throw new JSONException("Syntax error: array must be " +
                                    "followed by a dimension: " + tok1);
                        }
                        var tok2 = (string)tokens[i + 1];
                        int dim;
                        try
                        {
                            dim = int.Parse(tok2);
                        }
                        catch (FormatException)
                        {
                            throw new JSONException("Syntax error: illegal " +
                                    "array dimension: " + tok2);
                        }
                        if (i + 2 >= nTokens)
                        {
                            throw new JSONException("Syntax error: array " +
                                    "dimension must be closed: " + tok2);
                        }
                        var tok3 = (string)tokens[i + 2];
                        if (tok3.Length != 1 && tok3[0] != ArrayEnd)
                        {
                            throw new JSONException("Syntax error: illegal " +
                                    "close of array dimension: " + tok3);
                        }
                        if (i + 3 >= nTokens)
                        {
                            throw new JSONException("Syntax error: array close " +
                                    "must be followed by a separator or " +
                                    "array open: " + tok3);
                        }
                        var tok4 = (string)tokens[i + 3];
                        if (tok4.Length != 1 && tok4[0]
                            != Separator && tok4[0] != ArrayStart)
                        {
                            throw new JSONException("Syntax error: illegal " +
                                    "separator after array: " + tok4);
                        }
                        i += 4;
                        if (tok4[0] == Separator)
                        {
                            return JSONObject.Apply(start.OptJSONObject(dim), tokens, i);
                        }
                        if (tok4[0] == ArrayStart)
                        {
                            return Apply(start.OptJSONArray(dim), tokens, i);
                        }
                        throw new JSONException("Syntax error: illegal" +
                                " token after array: " + tok4);

                    default:
                        throw new JSONException("Syntax error: unknown" +
                                " delimiter: " + tok1);
                }
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
        ///  get a json array.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public override JSONArray GetAsArray(string path)
        {
            var tokens = new JSONPathTokenizer(path).Tokenize();
            if (tokens.Count == 0)
            {
                return this;
            }
            var obj = Apply(this, tokens, 0);
            return obj == null ? null : obj.OptJSONArray((string)tokens[tokens.Count - 1]);
        }

    }
}
