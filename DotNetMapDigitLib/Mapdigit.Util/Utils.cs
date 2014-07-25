//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 27SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System;
using System.IO;
using System.Text.RegularExpressions;

//--------------------------------- PACKAGE ------------------------------------

namespace Mapdigit.Util
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 27SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Utility class.
    /// </summary>
    public class Utils
    {

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a short converted to a byte array.
        /// </summary>
        /// <param name="l">a short number</param>
        /// <param name="bigendian"> true if big-endian output</param>
        /// <returns>endian byte array</returns>
        public static byte[] ShortToBytes(short l, bool bigendian)
        {
            byte[] by = new byte[2];
            if (bigendian)
            {
                by[0] = (byte)(l >> 8);
                by[1] = (byte)(l & 0xff);
            }
            else
            {
                by[0] = (byte)(l & 0xff);
                by[1] = (byte)(l >> 8);
            }
            return by;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns an int converted to a byte array.
        /// </summary>
        /// <param name="l">an integer number</param>
        /// <param name="bigendian">true if big-endian output</param>
        /// <returns>endian byte array</returns>
        public static byte[] IntToBytes(int l, bool bigendian)
        {
            byte[] b = new byte[4];
            if (bigendian)
            {
                b[0] = (byte)(l >> 24);
                b[1] = (byte)(l >> 16);
                b[2] = (byte)(l >> 8);
                b[3] = (byte)(l & 0xff);
            }
            else
            {
                b[0] = (byte)(l & 0xff);
                b[1] = (byte)(l >> 8);
                b[2] = (byte)(l >> 16);
                b[3] = (byte)(l >> 24);
            }
            return b;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns an long converted to a byte array.
        /// </summary>
        /// <param name="l">a long integer number.</param>
        /// <param name="bigendian">true if big-endian output</param>
        /// <returns>endian byte array</returns>
        public static byte[] LongToBytes(long l, bool bigendian)
        {
            byte[] b = new byte[8];
            if (bigendian)
            {
                b[0] = (byte)(l >> 56);
                b[1] = (byte)(l >> 48);
                b[2] = (byte)(l >> 40);
                b[3] = (byte)(l >> 32);
                b[4] = (byte)(l >> 24);
                b[5] = (byte)(l >> 16);
                b[6] = (byte)(l >> 8);
                b[7] = (byte)(l & 0xff);
            }
            else
            {
                b[0] = (byte)(l & 0xff);
                b[1] = (byte)(l >> 8);
                b[2] = (byte)(l >> 16);
                b[3] = (byte)(l >> 24);
                b[4] = (byte)(l >> 32);
                b[5] = (byte)(l >> 40);
                b[6] = (byte)(l >> 48);
                b[7] = (byte)(l >> 54);
            }
            return b;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a short converted from a byte array.
        /// </summary>
        /// <param name="by">a byte array</param>
        /// <param name="bigendian">true if big-endian output.</param>
        /// <returns>short built from bytes</returns>
        public static short BytesToShort(byte[] by, bool bigendian)
        {
            short s;
            if (bigendian)
            {
                s = (short)((by[0] << 8) | (0xff & by[1]));
            }
            else
            {
                s = (short)((by[1] << 8) | (0xff & by[0]));
            }
            return s;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns an int converted to from a byte array.
        /// We have to get an int because in java we can only so represent
        /// an unsigned short.
        /// </summary>
        /// <param name="by">by a byte array</param>
        /// <param name="bigendian">true if big-endian output</param>
        /// <returns></returns>
        public static int BytesToUShort(byte[] by, bool bigendian)
        {
            int i;
            if (by.Length == 1)
            {
                i = (0xff & by[0]);
            }
            else if (bigendian)
            {
                i = (((by[0] & 0xff) << 8) | (0xff & by[1]));
            }
            else
            {
                i = (((by[1] & 0xff) << 8) | (0xff & by[0]));
            }
            return i;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Doubles to int64 bits.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static long DoubleToInt64Bits(double value)
        {
            MemoryStream memoryStream = new MemoryStream(8);
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
            binaryWriter.Write(value);
            BinaryReader binaryReader = new BinaryReader(memoryStream);
            long ret= binaryReader.ReadInt64();
            binaryWriter.Close();
            binaryReader.Close();
            memoryStream.Close();
            return ret;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns an int converted to from a byte array.
        /// We have to get an int because in java we can only so represent
        /// an unsigned short.
        /// </summary>
        /// <param name="b">a byte array</param>
        /// <param name="bigendian">true if big-endian output</param>
        /// <returns></returns>
        public static int BytesToInt(byte[] b, bool bigendian)
        {
            int i;
            if (b.Length == 1)
            {
                i = (0xff & b[0]);
            }
            else if (b.Length == 2)
            {
                if (bigendian)
                {
                    i = (((b[0] & 0xff) << 8) | (0xff & b[1]));
                }
                else
                {
                    i = (((b[1] & 0xff) << 8) | (0xff & b[1]));
                }
            }
            else if (b.Length == 3)
            {
                if (bigendian)
                {
                    i = (((((b[0] & 0xff) << 8) | (0xff & b[1])) << 8) | (0xff & b[2]));
                }
                else
                {
                    i = (((((b[2] & 0xff) << 8) | (0xff & b[1])) << 8) | (0xff & b[0]));
                }
            }
            else
            {
                if (bigendian)
                {
                    i = (((((((b[0] & 0xff) << 8) | (0xff & b[1])) << 8) | (0xff & b[2])) << 8) | (0xff & b[3]));
                }
                else
                {
                    i = (((((((b[3] & 0xff) << 8) | (0xff & b[2])) << 8) | (0xff & b[1])) << 8) | (0xff & b[0]));
                }
            }
            return i;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a long converted to from a byte array.
        /// </summary>
        /// <param name="b">a byte array</param>
        /// <param name="bigendian">true if big-endian output</param>
        /// <returns></returns>
        public static long BytesToLong(byte[] b, bool bigendian)
        {
            int i = 0;
            if (b.Length == 1)
            {
                i = (0xff & b[0]);
            }
            else if (b.Length == 2)
            {
                if (bigendian)
                {
                    i = (((b[0] & 0xff) << 8) | (0xff & b[1]));
                }
                else
                {
                    i = (((b[1] & 0xff) << 8) | (0xff & b[1]));
                }
            }
            else if (b.Length == 3)
            {
                if (bigendian)
                {
                    i = (((((b[0] & 0xff) << 8) | (0xff & b[1])) << 8) | (0xff & b[2]));
                }
                else
                {
                    i = (((((b[2] & 0xff) << 8) | (0xff & b[1])) << 8) | (0xff & b[0]));
                }
            }
            else if (b.Length == 4)
            {
                if (bigendian)
                {
                    i = (((((((b[0] & 0xff) << 8) | (0xff & b[1])) << 8) | (0xff & b[2])) << 8) | (0xff & b[3]));
                }
                else
                {
                    i = (((((((b[3] & 0xff) << 8) | (0xff & b[2])) << 8) | (0xff & b[1])) << 8) | (0xff & b[0]));
                }
            }
            else if (b.Length == 5)
            {
                if (bigendian)
                {
                    i = (((((((((b[0] & 0xff) << 8) | (b[1] & 0xff)) << 8) | (0xff & b[2])) << 8) | (0xff & b[3])) << 8) | (0xff & b[4]));
                }
                else
                {
                    i = (((((((((b[4] & 0xff) << 8) | (b[3] & 0xff)) << 8) | (0xff & b[2])) << 8) | (0xff & b[1])) << 8) | (0xff & b[0]));
                }
            }
            else if (b.Length == 6)
            {
                if (bigendian)
                {
                    i = (((((((((((b[0] & 0xff) << 8) | (b[1] & 0xff)) << 8) | (b[2] & 0xff)) << 8) | (0xff & b[3])) << 8) | (0xff & b[4])) << 8) | (0xff & b[5]));
                }
                else
                {
                    i = (((((((((((b[5] & 0xff) << 8) | (b[4] & 0xff)) << 8) | (b[3] & 0xff)) << 8) | (0xff & b[2])) << 8) | (0xff & b[1])) << 8) | (0xff & b[0]));
                }
            }
            else if (b.Length == 7)
            {
                if (bigendian)
                {
                    i = (((((((((((((b[0] & 0xff) << 8) | (b[1] & 0xff)) << 8) | (b[2] & 0xff)) << 8) | (b[3] & 0xff)) << 8) | (0xff & b[4])) << 8) | (0xff & b[5])) << 8) | (0xff & b[6]));
                }
                else
                {
                    i = ((((((((((((b[6] & 0xff) << 8) | ((b[5] & 0xff)) << 8) | (b[4] & 0xff)) << 8) | (b[3] & 0xff)) << 8) | (0xff & b[2])) << 8) | (0xff & b[1])) << 8) | (0xff & b[0]));
                }
            }
            else if (b.Length == 8)
            {
                if (bigendian)
                {
                    i = (((((((((((((((b[0] & 0xff) << 8) | (b[1] & 0xff)) << 8) | (b[2] & 0xff)) << 8) | (b[3] & 0xff)) << 8) | (b[4] & 0xff)) << 8) | (0xff & b[5])) << 8) | (0xff & b[6])) << 8) | (0xff & b[7]));
                }
                else
                {
                    i = ((((((((((((((b[7] & 0xff) << 8) | (b[6] & 0xff)) << 8) | ((b[5] & 0xff)) << 8) | (b[4] & 0xff)) << 8) | (b[3] & 0xff)) << 8) | (0xff & b[2])) << 8) | (0xff & b[1])) << 8) | (0xff & b[0]));
                }
            }
            return i;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a int converted from a hexadecimal number
        /// </summary>
        /// <param name="s">s string representing a hexadecimal number</param>
        /// <returns></returns>
        public int HexToInt(string s)
        {
            int[] n = new int[s.Length];
            char c;
            int sum = 0;
            int koef = 1;
            for (int i = n.Length - 1; i >= 0; i--)
            {
                c = s[i];
                switch ((int)c)
                {
                    case 48:
                        n[i] = 0;
                        break;
                    case 49:
                        n[i] = 1;
                        break;
                    case 50:
                        n[i] = 2;
                        break;
                    case 51:
                        n[i] = 3;
                        break;
                    case 52:
                        n[i] = 4;
                        break;
                    case 53:
                        n[i] = 5;
                        break;
                    case 54:
                        n[i] = 6;
                        break;
                    case 55:
                        n[i] = 7;
                        break;
                    case 56:
                        n[i] = 8;
                        break;
                    case 57:
                        n[i] = 9;
                        break;
                    case 97:
                        n[i] = 10;
                        break;
                    case 98:
                        n[i] = 11;
                        break;
                    case 99:
                        n[i] = 12;
                        break;
                    case 100:
                        n[i] = 13;
                        break;
                    case 101:
                        n[i] = 14;
                        break;
                    case 102:
                        n[i] = 15;
                        break;
                }

                sum = sum + n[i] * koef;
                koef = koef * 16;
            }
            return sum;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// It's just a hack to facilitate the short to byte array conversion
        /// </summary>
        /// <param name="s">a value in the unsigned short range 0 to 65535</param>
        /// <returns>unsigned short</returns>
        public static short UnsignShort(int s)
        {
            short b = (short)s;
            s = b;
            s = s + 32768;
            b = (short)(0 - s);
            return b;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Convert a byte[] array to readable string format. This makes the "hex" readable!
        /// </summary>
        /// <param name="input">buffer to convert to string format</param>
        /// <returns>String buffer in String format</returns>
        public static string ByteArrayToHexString(byte[] input)
        {
            return BitConverter.ToString(input);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// split string by separator and return String[] of elements
        /// </summary>
        /// <param name="separator">The separator.</param>
        /// <param name="str">str entire string to split by separator</param>
        /// <returns></returns>
        public static string[] Explode(char separator, string str)
        {
            return str.Split(new[] { separator });
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// replace all entries of pattern[N] with value replace[N]
        /// length of pattern[] must equal to length of replace[]
        /// </summary>
        /// <param name="pattern">array of patterns to be replaced</param>
        /// <param name="replace">array of values to be inserted instead of pattern[i].</param>
        /// <param name="source">The source.</param>
        /// <returns>result String with replaced values</returns>
        public static string Replace(string[] pattern, string[] replace, string source)
        {
            if (pattern.Length != replace.Length)
            {
                return source;
            }
            string result = source;
            for (int i = 0; i < pattern.Length; i++)
            {
                result = ReplaceString(pattern[i], replace[i], result);
            }
            return result;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// replace all entries of pattern with value replace
        /// </summary>
        /// <param name="pattern">pattern to be replaced.</param>
        /// <param name="replace">The replace.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static string ReplaceString(string pattern, string replace, string source)
        {
            return source.Replace(pattern, replace);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        ///<summary>
        /// check if the string is a double
        ///</summary>
        ///<param name="input"></param>
        ///<returns></returns>
        public static bool IsDouble(string input)
        {
            Regex regex = new Regex(@"^\d*[0-9](|.\d*[0-9]|,\d*[0-9])?$");
            return regex.IsMatch(input);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        ///<summary>
        /// check if the string is a int32
        ///</summary>
        ///<param name="input"></param>
        ///<returns></returns>
        public static bool IsInt32(string input)
        {
            Regex regex =
                new Regex(
                    @"(0|[1-9]{1}[0-9]{0,8}|[1]{1}[0-9]{1,9}|[-]{1}[2]{1}([0]{1}[0-9]{8}|[1]{1}([0-3]{1}[0-9]{7}|[4]{1}([0-6]{1}[0-9]{6}|[7]{1}([0-3]{1}[0-9]{5}|[4]{1}([0-7]{1}[0-9]{4}|[8]{1}([0-2]{1}[0-9]{3}|[3]{1}([0-5]{1}[0-9]{2}|[6]{1}([0-3]{1}[0-9]{1}|[4]{1}[0-8]{1}))))))))|(\+)?[2]{1}([0]{1}[0-9]{8}|[1]{1}([0-3]{1}[0-9]{7}|[4]{1}([0-6]{1}[0-9]{6}|[7]{1}([0-3]{1}[0-9]{5}|[4]{1}([0-7]{1}[0-9]{4}|[8]{1}([0-2]{1}[0-9]{3}|[3]{1}([0-5]{1}[0-9]{2}|[6]{1}([0-3]{1}[0-9]{1}|[4]{1}[0-7]{1})))))))))");
            return regex.IsMatch(input) & IsNumber(input);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        ///<summary>
        /// check if the string is a number
        ///</summary>
        ///<param name="input"></param>
        ///<returns></returns>
        public static bool IsNumber(string input)
        {
            Regex regex=new Regex(@"^\d*$" );
            return regex.IsMatch(input);
        }

    }

}
