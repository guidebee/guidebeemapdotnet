//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 26SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System;
using System.Globalization;
using System.IO;
using System.Text;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Util
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 26SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// This class convert html string to plain text.
    /// </summary>
    public sealed class Html2Text
    {
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Remove double "\\" and change to one "\"
        /// </summary>
        /// <param name="source">the string need to change</param>
        /// <returns>the result string</returns>
        public static string RemoveDoubleBackSlash(string source)
        {
            StringBuilder result2 = new StringBuilder();
            StringGetter input = new StringGetter(source);

            try
            {
                int c = input.Read();

                while (c != -1) // Convert until EOF
                {
                    string text;
                    if (c == '\\')
                    {
                        c = input.Read();
                        if (c == '\\')
                        {
                            text = "\\";
                        }
                        else
                        {
                            text = "\\" + (char)c;
                        }


                    }
                    else
                    {
                        text = "" + (char)c;
                    }

                    StringBuilder s = result2;
                    s.Append(text);

                    c = input.Read();
                }
            }
            catch
            {
                input.Close();

            }
            return result2.ToString().Trim();
        }

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// convert \\uxxxx to it's string format
        /// </summary>
        /// <param name="source">string to convert..</param>
        /// <returns>result string</returns>
        public static string ConvertUTF8(string source)
        {

            StringBuilder result2 = new StringBuilder();
            StringGetter input = new StringGetter(source);

            try
            {
                int c = input.Read();

                while (c != -1) // Convert until EOF
                {
                    string text;
                    if (c == '\\')
                    {
                        c = input.Read();
                        switch ((char)c)
                        {
                            case 'u':
                                text = "";
                                for (int i = 0; i < 4; i++)
                                {
                                    text += ((char)input.Read()).ToString();
                                }
                                text = int.Parse(text, NumberStyles.HexNumber).ToString();
                                break;
                            case 'x':
                                text = "";
                                for (int i = 0; i < 2; i++)
                                {
                                    text += ((char)input.Read());
                                }
                                text = int.Parse(text, NumberStyles.HexNumber).ToString();
                                break;
                            default:
                                text = "\\" + (char)c;
                                break;

                        }

                    }
                    else
                    {
                        text = "" + (char)c;
                    }

                    StringBuilder s = result2;
                    s.Append(text);

                    c = input.Read();
                }
            }
            catch
            {
                input.Close();

            }


            return result2.ToString().Trim();
        }

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// convert to utf8 string
        /// </summary>
        /// <param name="b">byte array.</param>
        /// <returns>result string</returns>
        public static string Encodeutf8(byte[] b)
        {
            string result = "";
            for (int i = 0; i < b.Length; i++)
            {
                byte[] bytes = BitConverter.GetBytes(b[i]);
                result +=
                       "%" + BitConverter.ToString(bytes).Substring(0, 2);
            }
            return result.ToLower();
        }

        ///<summary>
        /// a simple utf8 string decoder.
        ///</summary>
        ///<param name="input"></param>
        ///<returns></returns>
        public static string UTF8Decoder(string input)
        {
            string[] letters = input.Split(new[]{'%'});
            if (letters.Length > 1)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < letters.Length; i++)
                    {
                        if (letters[i].Length > 0)
                        {
                            sb.Append((char)int.Parse(letters[i],NumberStyles.HexNumber));
                        }
                    }
                    return sb.ToString();
                }
                catch (Exception)
                {
                    return input;
                }
            }
            return input;
        }
        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Convert html to plain text
        /// </summary>
        /// <param name="source">HTML string.</param>
        /// <returns>plain text</returns>
        public string Convert(string source)
        {
            StringBuilder result = new StringBuilder();
            StringBuilder result2 = new StringBuilder();
            StringGetter input = new StringGetter(source);

            try
            {
                int c = input.Read();

                while (c != -1) // Convert until EOF
                {
                    string text;
                    if (c == '<') // It's a tag!!
                    {
                        string currentTag = GetTag(input); // Get the rest of the tag
                        text = ConvertTag(currentTag);
                    }
                    else if (c == '&')
                    {
                        string specialchar = GetSpecial(input);
                        if (specialchar.Equals("lt;") || specialchar.Equals("#60"))
                        {
                            text = "<";
                        }
                        else if (specialchar.Equals("gt;") || specialchar.Equals("#62"))
                        {
                            text = ">";
                        }
                        else if (specialchar.Equals("amp;") || specialchar.Equals("#38"))
                        {
                            text = "&";
                        }
                        else if (specialchar.Equals("nbsp;"))
                        {
                            text = " ";
                        }
                        else if (specialchar.Equals("quot;") || specialchar.Equals("#34"))
                        {
                            text = "\"";
                        }
                        else if (specialchar.Equals("copy;") || specialchar.Equals("#169"))
                        {
                            text = "[Copyright]";
                        }
                        else if (specialchar.Equals("reg;") || specialchar.Equals("#174"))
                        {
                            text = "[Registered]";
                        }
                        else if (specialchar.Equals("trade;") || specialchar.Equals("#153"))
                        {
                            text = "[Trademark]";
                        }
                        else
                        {
                            text = "&" + specialchar;
                        }
                    }
                    else if (!_pre && IsWhitespace((char)c))
                    {
                        StringBuilder s = _inBody ? result : result2;
                        if (s.Length > 0 && IsWhitespace(s[s.Length - 1]))
                        {
                            text = "";
                        }
                        else
                        {
                            text = " ";
                        }
                    }
                    else
                    {
                        text = "" + (char)c;
                    }

                    StringBuilder s2 = _inBody ? result : result2;
                    s2.Append(text);

                    c = input.Read();
                }
            }
            catch
            {
                input.Close();

            }

            StringBuilder s1 = _bodyFound ? result : result2;
            return s1.ToString().Trim();
        }


        private bool _bodyFound;
        private bool _inBody;
        private bool _pre;
        private string _href = "";

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the tag.
        /// </summary>
        /// <param name="r">The r.</param>
        /// <returns></returns>
        private static string GetTag(StringGetter r)
        {
            StringBuilder result = new StringBuilder();
            int level = 1;

            result.Append('<');
            while (level > 0)
            {
                int c = r.Read();
                if (c == -1)
                {
                    break; // EOF
                } // EOF
                result.Append((char)c);
                if (c == '<')
                {
                    level++;
                }
                else if (c == '>')
                {
                    level--;
                }
            }

            return result.ToString();
        }

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the special.
        /// </summary>
        /// <param name="r">The r.</param>
        /// <returns></returns>
        private static string GetSpecial(StringGetter r)
        {
            StringBuilder result = new StringBuilder();
            r.Mark(1);//Mark the present position in the stream
            int c = r.Read();

            while (IsLetter((char)c))
            {
                result.Append((char)c);
                r.Mark(1);
                c = r.Read();
            }

            if (c == ';')
            {
                result.Append(';');
            }
            else
            {
                r.Reset();
            }

            return result.ToString();
        }


        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determines whether the specified s1 is tag.
        /// </summary>
        /// <param name="s1">The s1.</param>
        /// <param name="s2">The s2.</param>
        /// <returns>
        /// 	<c>true</c> if the specified s1 is tag; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsTag(string s1, string s2)
        {
            s1 = s1.ToLower();
            string t1 = "<" + s2.ToLower() + ">";
            string t2 = "<" + s2.ToLower() + " ";

            return s1.StartsWith(t1) || s1.StartsWith(t2);
        }

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Converts the tag.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
        private string ConvertTag(string t)
        {
            string result = "";

            if (IsTag(t, "body"))
            {
                _inBody = true; _bodyFound = true;
            }
            else if (IsTag(t, "/body"))
            {
                _inBody = false; result = "";
            }
            else if (IsTag(t, "center"))
            {
                result = "";
            }
            else if (IsTag(t, "/center"))
            {
                result = "";
            }
            else if (IsTag(t, "_pre"))
            {
                result = ""; _pre = true;
            }
            else if (IsTag(t, "/_pre"))
            {
                result = ""; _pre = false;
            }
            else if (IsTag(t, "p"))
            {
                result = "";
            }
            else if (IsTag(t, "br"))
            {
                result = "";
            }
            else if (IsTag(t, "h1") || IsTag(t, "h2") || IsTag(t, "h3")
                || IsTag(t, "h4") || IsTag(t, "h5") || IsTag(t, "h6") || IsTag(t, "h7"))
            {
                result = "";
            }
            else if (IsTag(t, "/h1") || IsTag(t, "/h2") || IsTag(t, "/h3")
                || IsTag(t, "/h4") || IsTag(t, "/h5") || IsTag(t, "/h6") || IsTag(t, "/h7"))
            {
                result = "";
            }
            else if (IsTag(t, "/dl"))
            {
                result = "";
            }
            else if (IsTag(t, "dd"))
            {
                result = "  * ";
            }
            else if (IsTag(t, "dt"))
            {
                result = "      ";
            }
            else if (IsTag(t, "li"))
            {
                result = "  * ";
            }
            else if (IsTag(t, "/ul"))
            {
                result = "";
            }
            else if (IsTag(t, "/ol"))
            {
                result = "";
            }
            else if (IsTag(t, "hr"))
            {
                result = "_________________________________________";
            }
            else if (IsTag(t, "table"))
            {
                result = "";
            }
            else if (IsTag(t, "/table"))
            {
                result = "";
            }
            else if (IsTag(t, "form"))
            {
                result = "";
            }
            else if (IsTag(t, "/form"))
            {
                result = "";
            }
            else if (IsTag(t, "b"))
            {
                result = "";
            }
            else if (IsTag(t, "/b"))
            {
                result = "";
            }
            else if (IsTag(t, "i"))
            {
                result = "\"";
            }
            else if (IsTag(t, "/i"))
            {
                result = "\"";
            }
            else if (IsTag(t, "img"))
            {
                int idx = t.IndexOf("alt=\"");
                if (idx != -1)
                {
                    idx += 5;
                    int idx2 = t.IndexOf("\"", idx);
                    result = t.Substring(idx, idx2 - idx);
                }
            }
            else if (IsTag(t, "a"))
            {
                int idx = t.IndexOf("_href=\"");
                if (idx != -1)
                {
                    idx += 6;
                    int idx2 = t.IndexOf("\"", idx);
                    _href = t.Substring(idx, idx2 - idx);
                }
                else
                {
                    _href = "";
                }
            }
            else if (IsTag(t, "/a"))
            {
                if (_href.Length > 0)
                {
                    result = " [ " + _href + " ]";
                    _href = "";
                }
            }

            return result;
        }

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determines whether the specified ch is whitespace.
        /// </summary>
        /// <param name="ch">The ch.</param>
        /// <returns>
        /// 	<c>true</c> if the specified ch is whitespace; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsWhitespace(char ch)
        {
            if (ch == ' ' || ch == '\t' || ch == '\n')
            {
                return true;
            }
            return false;

        }

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determines whether the specified ch is letter.
        /// </summary>
        /// <param name="ch">The ch.</param>
        /// <returns>
        /// 	<c>true</c> if the specified ch is letter; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsLetter(char ch)
        {
            if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z'))
            {
                return true;
            }
            return false;
        }


    }


    /// <summary>
    /// 
    /// </summary>
    class StringGetter
    {

        private string _str;
        private readonly int _length;
        private int _next;
        private int _mark;
        private readonly object _stringLock = new object();

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="StringGetter"/> class.
        /// </summary>
        /// <param name="s">String providing the character stream.</param>
        public StringGetter(string s)
        {
            _str = s;
            _length = s.Length;
        }

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Check to make sure that the stream has not been closed 
        /// </summary>
        private void EnsureOpen()
        {
            if (_str == null)
            {
                throw new IOException("Stream closed");
            }
        }

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Reads a single character.
        /// </summary>
        /// <returns>The character read, or -1 if the end of the stream has been
        ///              reached</returns>
        public int Read()
        {
            lock (_stringLock)
            {
                EnsureOpen();
                if (_next >= _length)
                {
                    return -1;
                }
                return _str[_next++];
            }
        }

        

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Skips the specified number of characters in the stream. Returns
        /// the number of characters that were skipped.
        ///
        /// The ns parameter may be negative, even though the
        /// skip method of the Reader superclass throws
        /// an exception in this case. Negative values of ns cause the
        /// stream to skip backwards. Negative return values indicate a skip
        /// backwards. It is not possible to skip backwards past the beginning of
        /// the string.
        ///
        /// If the entire string has been read or skipped, then this method has
        /// no effect and always returns 0.
        /// </summary>
        /// <param name="ns">The ns.</param>
        /// <returns></returns>
        public long Skip(long ns)
        {
            lock (_stringLock)
            {
                EnsureOpen();
                if (_next >= _length)
                {
                    return 0;
                }
                // Bound Skip by beginning and end of the source
                long n = Math.Min(_length - _next, ns);
                n = Math.Max(-_next, n);
                _next += (int)n;
                return n;
            }
        }

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tells whether this stream is ready to be read.
        /// </summary>
        /// <returns>True if the next read() is guaranteed not to block for input</returns>
        public bool Ready()
        {
            lock (_stringLock)
            {
                EnsureOpen();
                return true;
            }
        }

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tells whether this stream supports the mark() operation, which it does.
        /// </summary>
        /// <returns></returns>
        public bool MarkSupported()
        {
            return true;
        }

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Marks the present position in the stream.  Subsequent calls to reset()
        /// will reposition the stream to this point.
        /// </summary>
        /// <param name="readAheadLimit">The read ahead limit.</param>
        public void Mark(int readAheadLimit)
        {
            if (readAheadLimit < 0)
            {
                throw new ArgumentException("Read-ahead limit < 0");
            }
            lock (_stringLock)
            {
                EnsureOpen();
                _mark = _next;
            }
        }

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Resets the stream to the most recent mark, or to the beginning of the
        /// string if it has never been marked.
        /// </summary>
        public void Reset()
        {
            lock (_stringLock)
            {
                EnsureOpen();
                _next = _mark;
            }
        }

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            _str = null;
        }
    }


}


