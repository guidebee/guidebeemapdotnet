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

namespace Mapdigit.Drawing.Geometry
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 25SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Fixed-Point Math Library. default all number shall be limited in range
    /// between -8388608.999999 to 8388608.999999 (precision bit 20).
    /// </summary>
    internal abstract class MathFP
    {
        /**
         * Default precision lenght.(1/ 2^21).
         */
        public const int DefaultPrecision = 20;
        /**
         * Constant for One, HALF etc for the fixed-point math.
         */
        public static long One, Half, Two, E, Pi, PiHalf, PiTwo;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the precision.
        /// </summary>
        /// <returns>the precision</returns>
        public static int GetPrecision()
        {
            return (int) _precision;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the precision for all fixed-point operations.
        /// The maximum precision is 31 bits.
        /// </summary>
        /// <param name="precision">the desired precision in number of bits</param>
        public static void SetPrecision(int precision)
        {
            if (precision > MaxPrecision || precision < 0)
            {
                return;
            }
            int i;
            _precision = precision;
            One = 1 << precision;
            Half = One >> 1;
            Two = One << 1;
            Pi = (precision <= PiPrecision)
                     ?
                         PiValue >> (PiPrecision - precision)
                     : PiValue << (precision - PiPrecision);
            PiHalf = Pi >> 1;
            PiTwo = Pi << 1;
            E = (precision <= EPrecision)
                    ?
                        EValue >> (EPrecision - precision)
                    : EValue >> (precision - EPrecision);
            for (i = 0; i < SkValue.Length; i++)
            {
                Sk[i] = (precision <= SkPrecision)
                            ?
                                SkValue[i] >> (SkPrecision - precision)
                            : SkValue[i] << (precision - SkPrecision);
            }
            for (i = 0; i < AsValue.Length; i++)
            {
                As[i] = (precision <= AsPrecision)
                            ?
                                AsValue[i] >> (AsPrecision - precision)
                            : AsValue[i] << (precision - AsPrecision);
            }
            _ln2 = (precision <= Ln2Precision)
                       ?
                           Ln2Value >> (Ln2Precision - precision)
                       : Ln2Value << (precision - Ln2Precision);
            _ln2Inv = (precision <= Ln2Precision)
                          ?
                              Ln2InvValue >> (Ln2Precision - precision)
                          :
                              Ln2InvValue << (precision - Ln2Precision);
            for (i = 0; i < LgValue.Length; i++)
            {
                Lg[i] = (precision <= LgPrecision)
                            ?
                                LgValue[i] >> (LgPrecision - precision)
                            : LgValue[i] << (precision - LgPrecision);
            }
            for (i = 0; i < ExpPValue.Length; i++)
            {
                ExpP[i] = (precision <= ExpPPrecision)
                              ? ExpPValue[i] >> (ExpPPrecision - precision)
                              : ExpPValue[i] << (precision - ExpPPrecision);
            }
            _fracMask = One - 1;
            _piOverOneEighty = Div(Pi, ToFP(180));
            _oneEightyOverPi = Div(ToFP(180), Pi);

            _maxDigitsMul = 1;
            _maxDigitsCount = 0;
            for (long j = One; j != 0;)
            {
                j /= 10;
                _maxDigitsMul *= 10;
                _maxDigitsCount++;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Converts a fixed-point value to the current set precision.
        /// </summary>
        /// <param name="fp">the fixed-point value to convert.</param>
        /// <param name="precision">the precision of the fixed-point value passed in.</param>
        /// <returns>a fixed-point value of the current precision</returns>
        public static long Convert(long fp, int precision)
        {
            long num, xabs = Math.Abs(fp);
            if (precision > MaxPrecision || precision < 0)
            {
                return fp;
            }
            if (precision > _precision)
            {
                num = xabs >> (int) (precision - _precision);
            }
            else
            {
                num = xabs << (int) (_precision - precision);
            }
            if (fp < 0)
            {
                num = -num;
            }
            return num;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Converts an long to a fixed-point long.
        /// </summary>
        /// <param name="i">long to convert.</param>
        /// <returns>the converted fixed-point value.</returns>
        public static long ToFP(int i)
        {
            return (i < 0) ? -(-i << (int) _precision) : i << (int) _precision;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Converts a string to a fixed-point value. 
        /// The string should trimmed of any whitespace before-hand. 
        /// A few examples of valid strings:
        ///
        /// <pre>
        /// .01
        /// 0.01
        /// 10
        /// 130.0
        /// -30000.12345
        /// </pre>
        /// </summary>
        /// <param name="s">the string to convert.</param>
        /// <returns>the fixed-point value</returns>
        public static long ToFP(string s)
        {
            long fp, i, integer, frac = 0;
            string fracString = null;
            bool neg = false;
            if (s[0] == '-')
            {
                neg = true;
                s = s.Substring(1);
            }
            int index = s.IndexOf('.');

            if (index < 0)
            {
                integer = int.Parse(s);
            }
            else if (index == 0)
            {
                integer = 0;
                fracString = s.Substring(1);
            }
            else if (index == s.Length - 1)
            {
                integer = int.Parse(s.Substring(0, index));
            }
            else
            {
                integer = int.Parse(s.Substring(0, index));
                fracString = s.Substring(index + 1);
            }

            if (fracString != null)
            {
                if (fracString.Length > _maxDigitsCount)
                {
                    fracString = fracString.Substring(0, (int) _maxDigitsCount);
                }
                if (fracString.Length > 0)
                {
                    frac = int.Parse(fracString);
                    for (i = _maxDigitsCount - fracString.Length; i > 0; --i)
                    {
                        frac *= 10;
                    }
                }
            }
            fp = (integer << (int) _precision) + (frac << (int) (_precision + Half))/_maxDigitsMul;
            if (neg)
            {
                fp = -fp;
            }
            return fp;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Converts a fixed-point value to an long.
        /// </summary>
        /// <param name="fp">fixed-point value to convert</param>
        /// <returns>the converted long value.</returns>
        public static int ToInt(long fp)
        {
            return (int) ((fp < 0) ? -(-fp >> (int) _precision) : fp >> (int) _precision);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Converts a fixed-point value to a string.
        /// </summary>
        /// <param name="fp">the fixed-point value to convert.</param>
        /// <returns>
        ///  a string representing the fixed-point value with a minimum of
        ///         decimals in the string.
        /// </returns>
        public static string ToString(long fp)
        {
            bool neg = false;
            if (fp < 0)
            {
                neg = true;
                fp = -fp;
            }
            long integer = fp >> (int) _precision;
            long fp1 = (fp & _fracMask)*_maxDigitsMul;
            long fp2 = fp1 >> (int) _precision;
            string fracString = fp2.ToString();

            long len = _maxDigitsCount - fracString.Length;
            for (long i = len; i > 0; --i)
            {
                fracString = "0" + fracString;
            }
            if ((neg && integer != 0))
            {
                integer = -integer;
            }
            return integer + "." + fracString;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the smallest (closest to negative infinity) fixed-point value
        /// that is greater than or equal to the argument and is equal to a
        /// mathematical integer.
        /// </summary>
        /// <param name="fp">a fixed-point value..</param>
        /// <returns>
        /// the smallest (closest to negative infinity) fixed-point value
        ///         that is greater than or equal to the argument and is equal to a
        ///         mathematical integer.
        /// </returns>
        public static long Ceil(long fp)
        {
            bool neg = false;
            if (fp < 0)
            {
                fp = -fp;
                neg = true;
            }
            if ((fp & _fracMask) == 0)
            {
                return (neg) ? -fp : fp;
            }
            if (neg)
            {
                return -(fp & ~_fracMask);
            }
            return (fp & ~_fracMask) + One;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the largest (closest to positive infinity) fixed-point value
        /// value that is less than or equal to the argument and is equal to a
        /// mathematical integer.
        /// </summary>
        /// <param name="fp">a fixed-point value.</param>
        /// <returns>
        ///  the largest (closest to positive infinity) fixed-point value that
        ///         less than or equal to the argument and is equal to a mathematical
        ///         integer.
        /// </returns>
        public static long Floor(long fp)
        {
            bool neg = false;
            if (fp < 0)
            {
                fp = -fp;
                neg = true;
            }
            if ((fp & _fracMask) == 0)
            {
                return (neg) ? -fp : fp;
            }
            if (neg)
            {
                return -(fp & ~_fracMask) - One;
            }
            return (fp & ~_fracMask);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Removes the fractional part of a fixed-point value.
        /// </summary>
        /// <param name="fp">the fixed-point value to truncate.</param>
        /// <returns>a truncated fixed-point value.</returns>
        public static long Trunc(long fp)
        {
            return (fp < 0) ? -(-fp & ~_fracMask) : fp & ~_fracMask;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the fractional part of a fixed-point value.
        /// </summary>
        /// <param name="fp">a fixed-point value to get fractional part of.</param>
        /// <returns>positive fractional fixed-point value if input is positive,
        ///          negative fractional otherwise.</returns>
        public static long Frac(long fp)
        {
            return (fp < 0) ? -(-fp & _fracMask) : fp & _fracMask;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Converts a fixed-point integer to an int with only the decimal value.
        /// For example, if fp represents 12.34 the
        /// method returns 34
        /// </summary>
        /// <param name="fp">the fixed-point integer to be converted</param>
        /// <returns>a int in a normal integer representation</returns>
        public static int FracAsInt(long fp)
        {
            if (fp < 0)
            {
                fp = -fp;
            }
            return (int) ((_maxDigitsMul*(fp & _fracMask)) >> (int) _precision);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Returns the closest integer to the argument.
        /// </summary>
        /// <param name="fp">the fixed-point value to round.</param>
        /// <returns>the value of the argument rounded to the nearest integer value.</returns>
        public static long Round(long fp)
        {
            bool neg = false;
            if (fp < 0)
            {
                fp = -fp;
                neg = true;
            }
            fp += Half;
            fp &= ~_fracMask;
            return (neg) ? -fp : fp;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the smaller of two values.
        /// </summary>
        /// <param name="fp1">the fixed-point value.</param>
        /// <param name="fp2">the fixed-point value.</param>
        /// <returns>the smaller of fp1 and fp2.</returns>
        public static long Min(long fp1, long fp2)
        {
            return fp2 >= fp1 ? fp1 : fp2;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the greater of two values.
        /// </summary>
        /// <param name="fp1">the fixed-point value.</param>
        /// <param name="fp2">the fixed-point value.</param>
        /// <returns>the greater of fp1 and fp2.</returns>
        public static long Max(long fp1, long fp2)
        {
            return fp1 >= fp2 ? fp1 : fp2;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the absolute value of a fix float value.
        /// </summary>
        /// <param name="fp">the fixed-point value..</param>
        /// <returns>the absolute value of the argument.</returns>
        public static long Abs(long fp)
        {
            if (fp < 0)
            {
                return -fp;
            }
            return fp;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// add two fixed-point values.
        /// </summary>
        /// <param name="fp1">first fixed-point value.</param>
        /// <param name="fp2">second fixed-point value.</param>
        /// <returns>the result of the addition.</returns>
        public static long Add(long fp1, long fp2)
        {
            return fp1 + fp2;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// substract two fixed-point values.
        /// </summary>
        /// <param name="fp1">first fixed-point value.</param>
        /// <param name="fp2">second fixed-point value.</param>
        /// <returns>the result of the substraction.</returns>
        public static long Sub(long fp1, long fp2)
        {
            return fp1 - fp2;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Computes the remainder operation on two arguments .
        /// </summary>
        /// <param name="fp1">first fixed-point value..</param>
        /// <param name="fp2">second fixed-point value</param>
        /// <returns>the remainder when fp1 is divided by fp2</returns>
        public static long IEEERemainder(long fp1, long fp2)
        {
            return fp1 - Mul(Floor(Div(fp1, fp2)), fp2);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Multiplies two fixed-point values.
        /// </summary>
        /// <param name="fp1">first fixed-point value..</param>
        /// <param name="fp2">second fixed-point value</param>
        /// <returns>the result of the multiplication.</returns>
        public static long Mul(long fp1, long fp2)
        {
            long fp = fp1*fp2;
            return (fp >> (int) _precision);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Divides two fixed-point values.
        /// </summary>
        /// <param name="fp1">first fixed-point value..</param>
        /// <param name="fp2">second fixed-point value</param>
        /// <returns>the result of the division.</returns>
        public static long Div(long fp1, long fp2)
        {
            if (fp1 == 0)
            {
                return 0;
            }
            if (fp2 == 0)
            {
                return (fp1 < 0) ? -Infinity : Infinity;
            }
            long xneg = 0, yneg = 0;
            if (fp1 < 0)
            {
                xneg = 1;
                fp1 = -fp1;
            }
            if (fp2 < 0)
            {
                yneg = 1;
                fp2 = -fp2;
            }
            long msb = 0, lsb = 0;
            while ((fp1 & (1 << (int) (MaxPrecision - msb))) == 0)
            {
                msb++;
            }
            while ((fp2 & (1 << (int) lsb)) == 0)
            {
                lsb++;
            }
            long shifty = _precision - (msb + lsb);
            long res = ((fp1 << (int) msb)/(fp2 >> (int) lsb));
            if (shifty > 0)
            {
                res <<= (int) shifty;
            }
            else
            {
                res >>= (int) -shifty;
            }
            if ((xneg ^ yneg) == 1)
            {
                res = -res;
            }
            return res;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the correctly rounded positive square root of a fixed-point
        /// value.
        /// </summary>
        /// <param name="fp"> a fixed-point value.</param>
        /// <returns>the positive square root of fp. If the argument
        ///       is NaN or less than zero, the result is NaN.</returns>
        public static long Sqrt(long fp)
        {
            long s = (fp + One) >> 1;
            for (int i = 0; i < 8; i++)
            {
                s = (s + Div(fp, s)) >> 1;
            }
            return s;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///Returns the trigonometric sine of an angle.
        /// </summary>
        /// <param name="fp"> the angle in radians</param>
        /// <returns>the sine of the argument.</returns>
        public static long Sin(long fp)
        {
            long sign = 1;
            fp %= Pi*2;
            if (fp < 0)
            {
                fp = Pi*2 + fp;
            }
            if ((fp > PiHalf) && (fp <= Pi))
            {
                fp = Pi - fp;
            }
            else if ((fp > Pi) && (fp <= (Pi + PiHalf)))
            {
                fp = fp - Pi;
                sign = -1;
            }
            else if (fp > (Pi + PiHalf))
            {
                fp = (Pi << 1) - fp;
                sign = -1;
            }

            long sqr = Mul(fp, fp);
            long result = Sk[0];
            result = Mul(result, sqr);
            result -= Sk[1];
            result = Mul(result, sqr);
            result += One;
            result = Mul(result, fp);
            return sign*result;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the trigonometric cosine of an angle.
        /// </summary>
        /// <param name="fp">the angle in radians</param>
        /// <returns>the cosine of the argument.</returns>
        public static long Cos(long fp)
        {
            return Sin(PiHalf - fp);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the trigonometric tangent of an angle.
        /// </summary>
        /// <param name="fp">the angle in radians</param>
        /// <returns>the tangent of the argument.</returns>
        public static long Tan(long fp)
        {
            return Div(Sin(fp), Cos(fp));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the arc sine of a value; the returned angle is in the range
        /// -<i>pi</i>/2 through <i>pi</i>/2.
        /// </summary>
        /// <param name="fp">the fixed-point value whose arc sine is to be returned.</param>
        /// <returns> the arc sine of the argument</returns>
        public static long Asin(long fp)
        {
            bool neg = false;
            if (fp < 0)
            {
                neg = true;
                fp = -fp;
            }

            long fRoot = Sqrt(One - fp);
            long result = As[0];

            result = Mul(result, fp);
            result += As[1];
            result = Mul(result, fp);
            result -= As[2];
            result = Mul(result, fp);
            result += As[3];
            result = PiHalf - (Mul(fRoot, result));
            if (neg)
            {
                result = -result;
            }

            return result;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the arc cosine of a value; the returned angle is in the range 0.0
        /// through <i>pi</i>.
        /// </summary>
        /// <param name="fp">the fixed-point value whose arc cosine is to be returned.</param>
        /// <returns>the arc cosine of the argument</returns>
        public static long Acos(long fp)
        {
            return PiHalf - Asin(fp);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the arc tangent of a value; the returned angle is in the range
        /// -<i>pi</i>/2 through <i>pi</i>/2.
        /// </summary>
        /// <param name="fp">The fiexed-point value whose arc tangent is to be returned.</param>
        /// <returns>the arc tangent of the argument.</returns>
        public static long Atan(long fp)
        {
            return Asin(Div(fp, Sqrt(One + Mul(fp, fp))));
        } // This is a finely tuned error around 0. The inaccuracies stabilize at

        //around this Value.
        private const int Atan2ZeroError = 65;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the angle <i>theta</i> from the conversion of rectangular
        /// coordinates (fpX,fpY) to polar
        /// coordinates (r,<i>theta</i>).
        /// </summary>
        /// <param name="fpX">the ordinate coordinate</param>
        /// <param name="fpY">the abscissa coordinate</param>
        /// <returns>
        /// the <i>theta</i> component of the point
        /// (<i>r</i>,<i>theta</i>)
        /// in polar coordinates that corresponds to the point
        /// (<i>fpX</i>,<i>fpY</i>) in Cartesian coordinates.
        /// </returns>
        public static long Atan2(long fpX, long fpY)
        {
            if (fpX == 0)
            {
                if (fpY >= 0)
                {
                    return 0;
                }
                if (fpY < 0)
                {
                    return Pi;
                }
            }
            else if (fpY >= -Atan2ZeroError && fpY <= Atan2ZeroError)
            {
                return (fpX > 0) ? PiHalf : -PiHalf;
            }
            long z = Atan(Math.Abs(Div(fpX, fpY)));
            if (fpY > 0)
            {
                return (fpX > 0) ? z : -z;
            }
            return (fpX > 0) ? Pi - z : z - Pi;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns Euler's number <i>e</i> raised to the power of a fixed-point
        /// value.
        /// </summary>
        /// <param name="fp">the exponent to raise <i>e</i> to.</param>
        /// <returns>the value <i>e</i><sup>fp</sup>, where <i>e</i>
        ///         is the base of the natural logarithms.</returns>
        public static long Exp(long fp)
        {
            if (fp == 0)
            {
                return One;
            }
            long xabs = Math.Abs(fp);
            long k = Mul(xabs, _ln2Inv);
            k += Half;
            k &= ~_fracMask;
            if (fp < 0)
            {
                k = -k;
            }
            fp -= Mul(k, _ln2);
            long z = Mul(fp, fp);
            long l = Two + Mul(z, ExpP[0] + Mul(z, ExpP[1] + Mul(z, ExpP[2]
                                                                    + Mul(z, ExpP[3] + Mul(z, ExpP[4])))));
            long xp = One + Div(Mul(Two, fp), l - fp);
            if (k < 0)
            {
                k = One >> (int) (-k >> (int) _precision);
            }
            else
            {
                k = One << (int) (k >> (int) _precision);
            }
            return Mul(k, xp);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the natural logarithm (base e) of a fixed-point value.
        /// </summary>
        /// <param name="x">a fixed-point value.</param>
        /// <returns>the value lna, the natural logarithm of
        ///         fp.</returns>
        public static long Log(long x)
        {
            if (x < 0)
            {
                return 0;
            }
            if (x == 0)
            {
                return -Infinity;
            }
            long log2 = 0, xi = x;
            while (xi >= Two)
            {
                xi >>= 1;
                log2++;
            }
            long f = xi - One;
            long s = Div(f, Two + f);
            long z = Mul(s, s);
            long w = Mul(z, z);
            long l = Mul(w, Lg[1] + Mul(w, Lg[3] + Mul(w, Lg[5])))
                     + Mul(z, Lg[0] + Mul(w, Lg[2] + Mul(w, Lg[4] + Mul(w, Lg[6]))));
            return Mul(_ln2, (log2 << (int) _precision)) + f - Mul(s, f - l);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the logarithm (base base) of a fixed-point value.
        /// </summary>
        /// <param name="fp">a fixed-point value</param>
        /// <param name="baseNumber">The base number.</param>
        /// <returns> the value loga, the logarithm of
        ///         fp</returns>
        public static long Log(long fp, long baseNumber)
        {
            return Div(Log(fp), Log(baseNumber));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the value of the first argument raised to the power of the second
        /// argument
        /// </summary>
        /// <param name="fp1">the base.</param>
        /// <param name="fp2">the exponent.</param>
        /// <returns>the value a<sup>b</sup>.</returns>
        public static long Pow(long fp1, long fp2)
        {
            if (fp2 == 0)
            {
                return One;
            }
            if (fp1 < 0)
            {
                return 0;
            }
            return Exp(Mul(Log(fp1), fp2));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Converts an angle measured in degrees to an approximately equivalent
        /// angle measured in radians.
        /// </summary>
        /// <param name="fp">a fixed-point angle in degrees</param>
        /// <returns>the measurement of the angle angrad in radians.</returns>
        public static long ToRadians(long fp)
        {
            return Mul(fp, _piOverOneEighty);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Converts an angle measured in radians to an approximately equivalent
        ///  angle measured in degrees.
        /// </summary>
        /// <param name="fp">a fixed-point angle in radians.</param>
        /// <returns>the measurement of the angle angrad in degrees.
        /// </returns>
        public static long ToDegrees(long fp)
        {
            return Mul(fp, _oneEightyOverPi);
        }

        private const int MaxPrecision = 30;

        /**
         * largest possible number
         */
        public const int Infinity = 0x7fffffff;
        // 2.7182818284590452353602874713527 * 2^29
        private const int EPrecision = 29;
        private const int EValue = 1459366444;
        // 3.1415926535897932384626433832795 * 2^29
        private const int PiPrecision = 29;
        private const int PiValue = 1686629713;
        /**
         * number of fractional bits in all operations, do not modify directly
         */
        private static long _precision;
        private static long _fracMask;
        private static long _oneEightyOverPi;
        private static long _piOverOneEighty;
        private static long _maxDigitsCount;
        private static long _maxDigitsMul;
        private const int SkPrecision = 31;

        private static readonly int[] SkValue = new[]
                                                    {
                                                        16342350, //7.61e-03 * 2^31
                                                        356589659, //1.6605e-01 * 2^31
                                                    };

        private static readonly int[] Sk = new int[SkValue.Length];
        private const int AsPrecision = 30;

        private static readonly int[] AsValue = new[]
                                                    {
                                                        -20110432, //-0.0187293 * 2^30
                                                        79737141, //0.0742610 * 2^30
                                                        227756102, //0.2121144 * 2^30
                                                        1686557206 //1.5707288 * 2^30
                                                    };

        private static readonly int[] As = new int[AsValue.Length];
        //0.69314718055994530941723212145818 * 2^30
        private const int Ln2Precision = 30;
        private const int Ln2Value = 744261117;
        //1.4426950408889634073599246810019 * 2^30
        private const int Ln2InvValue = 1549082004;
        private static int _ln2, _ln2Inv;
        private const int LgPrecision = 31;

        private static readonly int[] LgValue = new[]
                                                    {
                                                        1431655765, //6.666666666666735130e-01 * 2^31
                                                        858993459, //3.999999999940941908e-01 * 2^31
                                                        613566760, //2.857142874366239149e-01 * 2^31
                                                        477218077, //2.222219843214978396e-01 * 2^31
                                                        390489238, //1.818357216161805012e-01 * 2^31
                                                        328862160, //1.531383769920937332e-01 * 2^31
                                                        317788895 //1.479819860511658591e-01 * 2^31
                                                    };

        private static readonly int[] Lg = new int[LgValue.Length];
        private const int ExpPPrecision = 31;

        private static readonly int[] ExpPValue = new[]
                                                      {
                                                          357913941, //1.66666666666666019037e-01 * 2^31
                                                          -5965232, //-2.77777777770155933842e-03 * 2^31
                                                          142029, //6.61375632143793436117e-05 * 2^31
                                                          -3550, //-1.65339022054652515390e-06 * 2^31
                                                          88, //4.13813679705723846039e-08 * 2^31
                                                      };

        private static readonly int[] ExpP = new int[ExpPValue.Length];
        // Init the default precision


        static MathFP()
        {
            SetPrecision(DefaultPrecision);
        }
    }
}