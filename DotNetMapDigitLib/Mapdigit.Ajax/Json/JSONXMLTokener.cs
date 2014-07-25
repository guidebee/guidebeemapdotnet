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
using System.Collections;
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
    /// The JSONXMLTokener extends the JSONTokener to provide additional methods
    /// for the parsing of JSONXML texts.
    /// </summary>
    internal class JSONXMLTokener : JSONTokener
    {

        /** 
         * The table of entity values. It initially contains Character values for
         * amp, apos, gt, lt, quot.
         */
        public static Hashtable Entity;

        static JSONXMLTokener()
        {
            Entity = new Hashtable(8)
                         {
                             {"amp", JSONXML.Amp},
                             {"apos", JSONXML.Apos},
                             {"gt", JSONXML.Gt},
                             {"lt", JSONXML.Lt},
                             {"quot", JSONXML.Quot}
                         };
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Construct an JSONXMLTokener from a string.
        /// </summary>
        /// <param name="s">A source string.</param>
        public JSONXMLTokener(string s)
            : base(s)
        {

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the text in the CDATA block.
        /// </summary>
        /// <returns>The string up to the ]]&gt;.</returns>
        public string NextCDATA()
        {
            var sb = new StringBuilder();
            for (; ; )
            {
                var c = Next();
                if (c == 0)
                {
                    throw SyntaxError("Unclosed CDATA.");
                }
                sb.Append(c);
                var i = sb.Length - 3;
                if (i >= 0 && sb[i] == ']' &&
                              sb[i + 1] == ']' && sb[i + 2] == '>')
                {
                    sb.Length = i;
                    return sb.ToString();
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
        /// Get the next JSONXML outer token, trimming whitespace. There are two kinds
        /// of tokens: the '<' character which begins a markup tag, and the content
        /// text between markup tags.
        /// </summary>
        /// <returns>A string, or a '<' Character, or null if there is no more
        /// source text.</returns>
        public object NextContent()
        {
            char c;
            do
            {
                c = Next();
            } while (IsWhitespace(c));
            if (c == 0)
            {
                return null;
            }
            if (c == '<')
            {
                return JSONXML.Lt;
            }
            StringBuilder sb = new StringBuilder();
            for (; ; )
            {
                if (c == '<' || c == 0)
                {
                    Back();
                    return sb.ToString().Trim();
                }
                if (c == '&')
                {
                    sb.Append(NextEntity(c));
                }
                else
                {
                    sb.Append(c);
                }
                c = Next();
            }
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Return the next entity. These entities are translated to Characters:
        ///      &amp;  &apos;  &gt;  &lt;  &quot;.
        /// </summary>
        /// <param name="a">An ampersand character</param>
        /// <returns>A Character or an entity String if the entity is not recognized.</returns>
        public object NextEntity(char a)
        {
            var sb = new StringBuilder();
            for (; ; )
            {
                var c = Next();
                if (IsLetterOrDigit(c) || c == '#')
                {
                    sb.Append(char.ToLower(c));
                }
                else if (c == ';')
                {
                    break;
                }
                else
                {
                    throw SyntaxError("Missing ';' in JSONXML entity: &" + sb);
                }
            }
            var s = sb.ToString();
            var e = Entity[s];
            return e ?? a + s + ";";
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the next JSONXML meta token. This is used for skipping over <!...>
        /// and <?...?> structures.
        /// </summary>
        /// <returns>Syntax characters (< > / = ! ?) are returned as
        /// Character, and strings and names are returned as Boolean. We don't care
        /// what the values actually are.</returns>
        public object NextMeta()
        {
            char c;
            do
            {
                c = Next();
            } while (IsWhitespace(c));
            switch (c)
            {
                case '\0':
                    throw SyntaxError("Misshaped meta tag.");
                case '<':
                    return JSONXML.Lt;
                case '>':
                    return JSONXML.Gt;
                case '/':
                    return JSONXML.Slash;
                case '=':
                    return JSONXML.Eq;
                case '!':
                    return JSONXML.Bang;
                case '?':
                    return JSONXML.Quest;
                case '"':
                case '\'':
                    var q = c;
                    for (; ; )
                    {
                        c = Next();
                        if (c == 0)
                        {
                            throw SyntaxError("Unterminated string.");
                        }
                        if (c == q)
                        {
                            return true;
                        }
                    }
                default:
                    for (; ; )
                    {
                        c = Next();
                        if (IsWhitespace(c))
                        {
                            return true;
                        }
                        switch (c)
                        {
                            case '\0':
                            case '<':
                            case '>':
                            case '/':
                            case '=':
                            case '!':
                            case '?':
                            case '"':
                            case '\'':
                                Back();

                                return true;
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
        ///  Get the next JSONXML Token. These tokens are found inside of angle
        /// brackets. It may be one of these characters: / > = ! ? or it
        /// may be a string wrapped in single quotes or double quotes, or it may be a
        /// name.
        /// </summary>
        /// <returns>a String or a Character</returns>
        public object NextToken()
        {
            char c;
            StringBuilder sb;
            do
            {
                c = Next();
            } while (IsWhitespace(c));
            switch (c)
            {
                case '\0':
                    throw SyntaxError("Misshaped element.");
                case '<':
                    throw SyntaxError("Misplaced '<'.");
                case '>':
                    return JSONXML.Gt;
                case '/':
                    return JSONXML.Slash;
                case '=':
                    return JSONXML.Eq;
                case '!':
                    return JSONXML.Bang;
                case '?':
                    return JSONXML.Quest;

                // Quoted string

                case '"':
                case '\'':
                    var q = c;
                    sb = new StringBuilder();
                    for (; ; )
                    {
                        c = Next();
                        if (c == 0)
                        {
                            throw SyntaxError("Unterminated string.");
                        }
                        if (c == q)
                        {
                            return sb.ToString();
                        }
                        if (c == '&')
                        {
                            sb.Append(NextEntity(c));
                        }
                        else
                        {
                            sb.Append(c);
                        }
                    }
                default:

                    // Name

                    sb = new StringBuilder();
                    for (; ; )
                    {
                        sb.Append(c);
                        c = Next();
                        if (IsWhitespace(c))
                        {
                            return sb.ToString();
                        }
                        switch (c)
                        {
                            case '\0':
                            case '>':
                            case '/':
                            case '=':
                            case '!':
                            case '?':
                            case '[':
                            case ']':
                                Back();
                                return sb.ToString();
                            case '<':
                            case '"':
                            case '\'':
                                throw SyntaxError("Bad character in a name.");
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
        /// Determines whether the specified c is whitespace.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns>
        /// 	<c>true</c> if the specified c is whitespace; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsWhitespace(char c)
        {
            switch (c)
            {
                case ' ':
                case '\r':
                case '\n':
                case '\t':
                    return true;
            }
            return false;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determines whether [is letter or digit] [the specified c].
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns>
        /// 	<c>true</c> if [is letter or digit] [the specified c]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsLetterOrDigit(char c)
        {
            switch (c)
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':

                case 'a':
                case 'b':
                case 'c':
                case 'd':
                case 'e':
                case 'f':
                case 'g':
                case 'h':
                case 'i':
                case 'j':
                case 'k':
                case 'l':
                case 'm':
                case 'n':
                case 'o':
                case 'p':
                case 'q':
                case 'r':
                case 's':
                case 't':
                case 'u':
                case 'v':
                case 'w':
                case 'x':
                case 'y':
                case 'z':

                case 'A':
                case 'B':
                case 'C':
                case 'D':
                case 'E':
                case 'F':
                case 'G':
                case 'H':
                case 'I':
                case 'J':
                case 'K':
                case 'L':
                case 'M':
                case 'N':
                case 'O':
                case 'P':
                case 'Q':
                case 'R':
                case 'S':
                case 'T':
                case 'U':
                case 'V':
                case 'W':
                case 'X':
                case 'Y':
                case 'Z':
                    return true;
            }
            return false;
        }

    }
}
