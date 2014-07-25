//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 25SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System;

//--------------------------------- PACKAGE ------------------------------------

namespace Mapdigit.Drawing.Geometry.Parser
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 25SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// This class represents a parser with support for numbers. remember that
    /// the parameter-less form of the parseNumber methods is meant
    /// for use by subclasses (e.g., TransformListParser).
    /// </summary>
    internal class NumberParser : AbstractParser
    {
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses a float value from the current position in the string
        /// </summary>
        /// <returns>floating point value corresponding to the parsed string</returns>
        public float ParseNumber()
        {
            return ParseNumber(false);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses the content of the input String and converts it to a float.
        /// </summary>
        /// <param name="numberString">the value to parse</param>
        /// <returns>the corresponding single precision floating point value.</returns>
        public float ParseNumber(string numberString)
        {
            SetString(numberString);
            return ParseNumber(true);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses the next float value in the string.
        /// </summary>
        /// <param name="eos">eos If eos is set to true, then there should be no more
        /// characters at the end of the string.</param>
        /// <returns>floating point value corresponding to the parsed string.
        /// An IllegalArgumentException is thrown if
        /// the next number in the string
        /// does not have a valid number syntax or if eos is true
        /// and there are more characters in the string after the
        /// number.</returns>
        public float ParseNumber(bool eos)
        {
            int mant = 0;
            int mantDig = 0;
            bool mantPos = true;
            bool mantRead = false;

            int exp = 0;
            int expDig = 0;
            int expAdj = 0;
            bool expPos = true;

            // Only read the next character if the
            // current one is -1
            if (_current == -1)
            {
                _current = Read();
            }
            // Parse the initial +/- sign if any
            switch ((char) _current)
            {
                case '-':
                    mantPos = false;
                    _current = Read();
                    break;
                case '+':
                    _current = Read();
                    break;
                default:
                    // nothing
                    break;
            }
            // Now, parse the mantisse

            switch ((char) _current)
            {
                default:
                    throw new ArgumentException("" + (char) _current);

                case '.':
                    break;

                case '0':
                    mantRead = true;

                    for (;;)
                    {
                        l1:
                        _current = Read();
                        switch (_current)
                        {
                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                                goto l1;
                            case '.':
                            case 'e':
                            case 'E':
                                goto m1;
                            case -1:
                                goto m1;
                            default:
                                if (eos)
                                {
                                    throw new ArgumentException(">" + (char) _current + "<");
                                }
                                return 0;
                            case '0': // <!>
                                break;
                        }
                    }

                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    mantRead = true;

                    for (;;)
                    {
                        l0:
                        if (mantDig < 9)
                        {
                            mantDig++;
                            mant = mant*10 + (_current - '0');
                        }
                        else
                        {
                            expAdj++;
                        }
                        _current = Read();
                        switch (_current)
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
                                goto l0;
                            default:
                                goto m1;
                        }
                    }
            }

            m1:

            // If we hit a point, parse the fractional part
            if (_current == '.')
            {
                _current = Read();

                switch (_current)
                {
                    default:
                        if (!mantRead)
                        {
                            throw new ArgumentException();
                        }
                        break;

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
                        if (mantDig == 0 && _current == '0')
                        {
                            for (;;)
                            {
                                l2:
                                _current = Read();
                                expAdj--;
                                switch (_current)
                                {
                                    case '1':
                                    case '2':
                                    case '3':
                                    case '4':
                                    case '5':
                                    case '6':
                                    case '7':
                                    case '8':
                                    case '9':
                                        goto l2;
                                    default:
                                        goto m2;
                                    case '0':
                                        break;
                                }
                            }
                        }
                        for (;;)
                        {
                            l:
                            if (mantDig < 9)
                            {
                                mantDig++;
                                mant = mant*10 + (_current - '0');
                                expAdj--;
                            }
                            _current = Read();
                            switch (_current)
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
                                    goto l;
                                default:
                                    goto m2;
                            }
                        }
                }
            }
            m2:

            // Parse the exponent
            switch (_current)
            {
                case 'e':
                case 'E':
                    _current = Read();
                    switch (_current)
                    {
                        default:
                            throw new ArgumentException();
                        case '-':
                            expPos = false;
                            break;
                        case '+':
                            _current = Read();
                            switch (_current)
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
                                    break;
                                default:
                                    throw new ArgumentException();
                            }
                            break;
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
                            break;
                    }


                    switch (_current)
                    {
                        case '0':

                            for (;;)
                            {
                                l3:
                                _current = Read();
                                switch (_current)
                                {
                                    case '1':
                                    case '2':
                                    case '3':
                                    case '4':
                                    case '5':
                                    case '6':
                                    case '7':
                                    case '8':
                                    case '9':
                                        goto l3;
                                    default:
                                        goto en;
                                    case '0':
                                        break;
                                }
                            }

                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':

                            for (;;)
                            {
                                l4:
                                if (expDig < 3)
                                {
                                    expDig++;
                                    exp = exp*10 + (_current - '0');
                                }
                                _current = Read();
                                switch (_current)
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
                                        goto l4;
                                    default:
                                        goto en;
                                }
                            }
                    }
                    break;
                default:
                    break;
            }
            en:


            if (eos && _current != -1)
            {
                throw new ArgumentException();
            }

            if (!expPos)
            {
                exp = -exp;
            }
            exp += expAdj;
            if (!mantPos)
            {
                mant = -mant;
            }

            return BuildFloat(mant, exp);
        }

        ///<summary>
        ///</summary>
        ///<param name="mant"></param>
        ///<param name="exp"></param>
        ///<returns></returns>
        public static float BuildFloat(int mant, int exp)
        {
            if (exp < -125 || mant == 0)
            {
                return 0f;
            }

            if (exp > 128)
            {
                if (mant > 0)
                {
                    return float.PositiveInfinity;
                }
                return float.NegativeInfinity;
            }

            if (exp == 0)
            {
                return mant;
            }

            if (mant >= 1 << 26)
            {
                mant++; // round up trailing bits if they will be dropped.
            }

            if (exp > 0)
            {
                return mant*Pow10[exp];
            }
            return mant/Pow10[-exp];
        }

        /**
        * Array of powers of ten.
        */
        private static readonly float[] Pow10 = new float[128];

        static NumberParser()
        {
            float cur = 0.1f;
            for (int i = 0; i < Pow10.Length; i++)
            {
                Pow10[i] = cur*10;
                cur = Pow10[i];
            }
        }
    }
}