//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 15OCT2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Util
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 15OCT2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// The class MathEx contains methods for performing basic numeric operations.
    /// It's an extention of java.lang.Math, provides more math functions.
    /// </summary>
    public static class MathEx
    {
        //[------------------------------ CONSTANTS -------------------------------]

        /// <summary>
        /// The double value limit used to termiate a calcuation.
        /// </summary>
        private const double Precision = 1e-12;

        /// <summary>
        /// The double value that is closer than any other to pi, the ratio of the
        /// circumference of a circle to its diameter.
        /// </summary>
        public const double Pi = Math.PI;

        /// <summary>
        /// The double value that is closer than any other to e, the base of the
        /// natural logarithms.
        /// </summary>
        public const double E = Math.E;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the absolute value of a double value.
        /// </summary>
        /// <remarks>
        /// If the argument is not negative, the argument is returned.
        /// If the argument is negative, the negation of the argument is returned.
        /// Special cases:
        /// <ul><li>If the argument is positive zero or negative zero, the result
        /// is positive zero.</li>
        /// <li>If the argument is infinite, the result is positive infinity.</li>
        /// <li>If the argument is NaN, the result is NaN.</li> </ul>
       /// </remarks>
        /// <param name="a">a double value</param>
        /// <returns>the absolute value of the argument.</returns>
        public static double Abs(double a)
        {
            return Math.Abs(a);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the absolute value of a float value.
        /// </summary>
        /// <remarks>
        /// If the argument is not negative, the argument is returned.
        /// If the argument is negative, the negation of the argument is returned.
        /// Special cases:
        /// <ul><li>If the argument is positive zero or negative zero, the
        /// result is positive zero.</li>
        /// <li>If the argument is infinite, the result is positive infinity.</li>
        /// <li>If the argument is NaN, the result is NaN.</li>  </ul>
        ///</remarks>
        /// <param name="a">a float value</param>
        /// <returns>the absolute value of the argument</returns>
        public static float Abs(float a)
        {
            return Math.Abs(a);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the absolute value of an int value.
        /// </summary>
        /// <remarks>
        /// If the argument is not negative, the argument is returned.
        /// If the argument is negative, the negation of the argument is returned.
        ///  remember that if the argument is equal to the value of
        /// Integer.MIN_VALUE, the most negative representable
        /// int value, the result is that same value, which is
        /// negative.</remarks>
        /// <param name="a">a int value</param>
        /// <returns>the absolute value of the argument.</returns>
        public static int Abs(int a)
        {
            return Math.Abs(a);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the absolute value of a long value.
        /// </summary>
        /// <remarks>
        /// If the argument is not negative, the argument is returned.
        /// If the argument is negative, the negation of the argument is returned.
        ///  remember that if the argument is equal to the value of
        /// Long.MIN_VALUE, the most negative representable
        /// long value, the result is that same value, which is
        /// negative.
        /// </remarks>
        /// <param name="a">a long value</param>
        /// <returns>the absolute value of the argument</returns>
        public static long Abs(long a)
        {
            return Math.Abs(a);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the smallest (closest to negative infinity)
        /// </summary>
        /// <remarks>
        /// double value that is not less than the argument and is
        /// equal to a mathematical integer. Special cases:
        /// <ul><li>If the argument value is already equal to a mathematical
        /// integer, then the result is the same as the argument.</li>
        /// <li>If the argument is NaN or an infinity or positive zero or negative
        /// zero, then the result is the same as the argument.</li>
        /// <li>If the argument value is less than zero but greater than -1.0,
        /// then the result is negative zero.</li> </ul>
        /// Remember that the value of Math.ceil(x) is exactly the
        /// value of -Math.floor(-x).</remarks>
        /// <param name="a">a double value.</param>
        /// <returns>the smallest (closest to negative infinity) double value that is
        /// not less than the argument and is equal to a mathematical integer.</returns>
        public static double Ceil(double a)
        {
            return Math.Ceiling(a);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the trigonometric cosine of an angle. 
        /// Special case:
        /// </summary>
        /// <remarks>
        /// If the argument is NaN or an infinity, then the
        /// result is NaN.
        /// </remarks>
        /// <param name="a">an angle, in radians.</param>
        /// <returns> the cosine of the argument.</returns>
        public static double Cos(double a)
        {
            return Math.Cos(a);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the largest (closest to positive infinity)
        /// </summary>
        /// <remarks>
        /// double value that is not greater than the argument and
        /// is equal to a mathematical integer. Special cases:
        /// <ul><li>If the argument value is already equal to a mathematical
        /// integer, then the result is the same as the argument.</li>
        /// <li>If the argument is NaN or an infinity or positive zero or
        /// negative zero, then the result is the same as the argument.</li> </ul>
        /// </remarks>
        /// <param name="a">a double value.</param>
        /// <returns>the largest (closest to positive infinity) double value that is
        /// not greater than the argument and is equal to a mathematical integer.</returns>
        public static double Floor(double a)
        {
            return Math.Floor(a);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the greater of two double values.  
        /// </summary>
        /// <remarks>
        /// That is, the
        /// result is the argument closer to positive infinity. If the
        /// arguments have the same value, the result is that same value. If
        /// either value is NaN, then the result is NaN.
        /// Unlike the the numerical comparison operators, this method considers
        /// negative zero to be strictly smaller than positive zero. If one
        /// argument is positive zero and the other negative zero, the result
        /// is positive zero.
        /// </remarks>
        /// <param name="a">a double value.</param>
        /// <param name="b">a double value.</param>
        /// <returns>the larger of a and b.</returns>
        public static double Max(double a, double b)
        {
            return Math.Max(a, b);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the greater of two float values. 
        /// </summary>
        /// <remarks>
        ///  That is, the result is the argument closer to positive infinity. If the
        /// arguments have the same value, the result is that same value. If
        /// either value is NaN, then the result is NaN.
        /// Unlike the the numerical comparison operators, this method considers
        /// negative zero to be strictly smaller than positive zero. If one
        /// argument is positive zero and the other negative zero, the result
        /// is positive zero.
        /// </remarks>
        /// <param name="a">a float value</param>
        /// <param name="b">a float value.</param>
        /// <returns>the larger of a and b.</returns>
        public static float Max(float a, float b)
        {
            return Math.Max(a, b);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the greater of two int values.
        /// </summary>
        /// <remarks>
        ///  That is, the result is the argument closer to the value of
        /// Integer.MAX_VALUE. If the arguments have the same value,
        /// the result is that same value.
        /// </remarks>
        /// <param name="a">a int value</param>
        /// <param name="b">a int value.</param>
        /// <returns>the larger of a and b.</returns>
        public static int Max(int a, int b)
        {
            return Math.Max(a, b);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the greater of two long values.
        /// </summary>
        /// <remarks>
        ///  That is, the result is the argument closer to the value of
        /// Long.MAX_VALUE. If the arguments have the same value,
        /// the result is that same value.
        /// </remarks>
        /// <param name="a"> a long value</param>
        /// <param name="b"> a long value</param>
        /// <returns>the larger of a and b</returns>
        public static long Max(long a, long b)
        {
            return Math.Max(a, b);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the smaller of two double values.  
        /// </summary>
        /// <remarks>
        /// That is, the result is the value closer to negative infinity. If the arguments have
        /// the same value, the result is that same value. If either value
        /// is NaN, then the result is NaN.  Unlike the
        /// the numerical comparison operators, this method considers negative zero
        /// to be strictly smaller than positive zero. If one argument is
        /// positive zero and the other is negative zero, the result is negative
        /// zero.
        /// </remarks>
        /// <param name="a"> a double value</param>
        /// <param name="b"> a double value</param>
        /// <returns>the smaller of a and b.</returns>
        public static double Min(double a, double b)
        {
            return Math.Min(a, b);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the smaller of two double values. 
        /// </summary>
        /// <remarks>
        ///  That is, the result is the value closer to negative infinity. If the arguments have
        /// the same value, the result is that same value. If either value
        /// is NaN, then the result is NaN.  Unlike the
        /// the numerical comparison operators, this method considers negative zero
        /// to be strictly smaller than positive zero. If one argument is
        /// positive zero and the other is negative zero, the result is negative
        /// zero.
        /// </remarks>
        /// <param name="a">a float value</param>
        /// <param name="b">a float value.</param>
        /// <returns>the smaller of a and b.</returns>
        public static float Min(float a, float b)
        {
            return Math.Min(a, b);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the smaller of two int values.
        /// </summary>
        /// <remarks>
        ///  That is, the
        /// result the argument closer to the value of Integer.MIN_VALUE.
        /// If the arguments have the same value, the result is that same value.
        ///</remarks>
        /// <param name="a">a int value</param>
        /// <param name="b">a int value.</param>
        /// <returns>the smaller of a and b.</returns>
        public static int Min(int a, int b)
        {
            return Math.Min(a, b);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the smaller of two long values. 
        /// </summary>
        /// <remarks>
        /// That is, the
        /// result is the argument closer to the value of
        /// Long.MIN_VALUE. If the arguments have the same value,
        /// the result is that same value.
        /// </remarks>
        /// <param name="a">a long value</param>
        /// <param name="b">a long value</param>
        /// <returns>the smaller of a and b</returns>
        public static long Min(long a, long b)
        {
            return Math.Min(a, b);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the trigonometric sine of an angle.  
        /// </summary>
        /// <remarks>
        /// Special cases:
        /// <ul><li>If the argument is NaN or an infinity, then the
        /// result is NaN.</li>
        /// <li>If the argument is positive zero, then the result is
        /// positive zero; if the argument is negative zero, then the
        /// result is negative zero.</li>  </ul>
        /// </remarks>
        /// <param name="a">an angle, in radians.</param>
        /// <returns>the sine of the argument</returns>
        public static double Sin(double a)
        {
            return Math.Sin(a);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the correctly rounded positive square root of a
        /// double value.
        /// </summary>
        /// <remarks>
        /// Special cases:
        /// <ul><li>If the argument is NaN or less than zero, then the result
        /// is NaN.</li>
        /// <li>If the argument is positive infinity, then the result is positive
        /// infinity.</li>
        /// <li>If the argument is positive zero or negative zero, then the
        /// result is the same as the argument.</li> </ul>
        /// </remarks>
        /// <param name="a">a double value</param>
        /// <returns>the positive square root of a. If the argument is NaN or less
        /// than zero, the result is NaN.</returns>
        public static double Sqrt(double a)
        {
            return Math.Sqrt(a);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///Returns the trigonometric tangent of an angle. 
        /// </summary>
        /// <remarks>
        ///  Special cases:
        /// <ul><li>If the argument is NaN or an infinity, then the result
        /// is NaN.</li>
        /// <li>If the argument is positive zero, then the result is
        /// positive zero; if the argument is negative zero, then the
        /// result is negative zero</li>  </ul>
        /// </remarks>
        /// <param name="a">an angle, in radians.</param>
        /// <returns>the tangent of the argument.</returns>
        public static double Tan(double a)
        {
            return Math.Tan(a);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///Converts an angle measured in radians to the equivalent angle
        ///  measured in degrees.
        /// </summary>
        /// <param name="angrad">an angle, in radians.</param>
        /// <returns>the measurement of the angle angrad in degrees.</returns>
        public static double ToDegrees(double angrad)
        {
            return angrad * 180.0 / Math.PI;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///Converts an angle measured in degrees to the equivalent angle
        /// measured in radians.
        /// </summary>
        /// <param name="angdeg">an angle, in degrees.</param>
        /// <returns>the measurement of the angle angrad in radians.</returns>
        public static double ToRadians(double angdeg)
        {
            return angdeg * Math.PI / 180.0;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///Returns the arc tangent of an angle, in the range of -<i>pi</i>/2
        /// through <i>pi</i>/2.  
        /// </summary>
        /// <remarks>
        /// Special cases:  <ul><li>If the argument is NaN,
        /// then the result is NaN.</li>
        /// <li>If the argument is zero, then the result is a zero with the
        /// same sign as the argument. </li> </ul> 
        /// A result must be within 1 ulp of the correctly rounded result.  Results
        /// must be semi-monotonic.
        /// </remarks>
        /// <param name="a">the value whose arc tangent is to be returned.</param>
        /// <returns>the arc tangent of the argument.</returns>
        public static double Atan(double a)
        {
            return Math.Atan(a);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///Converts rectangular coordinates (x,y)
        /// to polar (r,<i>theta</i>).
        /// </summary>
        /// <remarks>
        /// This method computes the phase <i>theta</i> by computing an arc tangent
        /// of y/x in the range of -<i>pi</i> to <i>pi</i>. Special
        /// cases:
        /// <ul><li>If either argument is NaN, then the result is NaN.</li>
        /// <li>If the first argument is positive zero and the second argument
        /// is positive, or the first argument is positive and finite and the
        /// second argument is positive infinity, then the result is positive
        /// zero.</li>
        /// <li>If the first argument is negative zero and the second argument
        /// is positive, or the first argument is negative and finite and the
        /// second argument is positive infinity, then the result is negative zero.</li>
        /// <li>If the first argument is positive zero and the second argument
        /// is negative, or the first argument is positive and finite and the
        /// second argument is negative infinity, then the result is the
        /// double value closest to <i>pi</i>.</li>
        /// <li>If the first argument is negative zero and the second argument
        /// is negative, or the first argument is negative and finite and the
        /// second argument is negative infinity, then the result is the
        /// double value closest to -<i>pi</i>.</li>
        /// <li>If the first argument is positive and the second argument is
        /// positive zero or negative zero, or the first argument is positive
        /// infinity and the second argument is finite, then the result is the
        /// double value closest to <i>pi</i>/2.</li>
        /// <li>If the first argument is negative and the second argument is
        /// positive zero or negative zero, or the first argument is negative
        /// infinity and the second argument is finite, then the result is the
        /// double value closest to -<i>pi</i>/2.</li>
        /// <li>If both arguments are positive infinity, then the result is the
        /// double value closest to <i>pi</i>/4.</li>
        /// <li>If the first argument is positive infinity and the second argument
        /// is negative infinity, then the result is the double
        /// value closest to 3*<i>pi</i>/4.</li>
        /// <li>If the first argument is negative infinity and the second argument
        /// is positive infinity, then the result is the double value
        /// closest to -<i>pi</i>/4.</li>
        /// <li>If both arguments are negative infinity, then the result is the
        /// double value closest to -3*<i>pi</i>/4. </li> </ul>
        /// A result must be within 2 ulps of the correctly rounded result.  Results
        /// must be semi-monotonic.
        /// </remarks>
        /// <param name="y">the ordinate coordinate.</param>
        /// <param name="x">the abscissa  coordinate.</param>
        /// <returns> the theta component of the point (r, theta) in polar coordinates
        /// that corresponds to the point (x, y) in Cartesian coordinates.</returns>
        public static double Atan2(double y, double x)
        {
            return Math.Atan2(y, x);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the closest int to the argument. 
        /// </summary>
        /// <remarks>
        /// The result is rounded to an integer by adding 1/2, taking the
        /// floor of the result, and casting the result to type int.
        /// In other words, the result is equal to the value of the expression:
        /// 
        /// <pre>(int)Math.floor(a + 0.5f)</pre>
        /// 
        /// Special cases:
        /// <ul>
        ///  <li>If the argument is NaN, the result is 0.</li>
        ///  <li>If the argument is negative infinity or any value less than or
        ///      equal to the value of Integer.MIN_VALUE, the result is
        ///      equal to the value of Integer.MIN_VALUE.</li>
        ///  <li>If the argument is positive infinity or any value greater than or
        ///      equal to the value of Integer.MAX_VALUE, the result is
        ///      equal to the value of Integer.MAX_VALUE.</li>
        /// </ul>
        /// </remarks>
        /// <param name="a">a floating-point value to be rounded to an integer.</param>
        /// <returns> value of the argument rounded to the nearest int value.</returns>
        public static int Round(float a)
        {
            return (int)Math.Floor(a + 0.5f);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the closest long to the argument. 
        /// </summary>
        /// <remarks>
        /// The result is rounded to an integer by adding 1/2, taking the floor of the
        /// result, and casting the result to type long. In other
        /// words, the result is equal to the value of the expression:
        /// 
        /// <pre>(long)Math.floor(a + 0.5d)</pre>
        /// 
        /// Special cases:
        /// <ul>
        ///  <li>If the argument is NaN, the result is 0.</li>
        ///  <li>If the argument is negative infinity or any value less than or
        ///      equal to the value of Long.MIN_VALUE, the result is
        ///      equal to the value of Long.MIN_VALUE.</li>
        ///  <li>If the argument is positive infinity or any value greater than or
        ///      equal to the value of Long.MAX_VALUE, the result is
        ///      equal to the value of Long.MAX_VALUE.</li>
        /// </ul>
        /// </remarks>
        /// <param name="a">a floating-point value to be rounded to a long.</param>
        /// <returns>the value of the argument rounded to the nearest long value</returns>
        public static long Round(double a)
        {
            return (long)Math.Floor(a + 0.5);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Exps the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public static double Exp(double x)
        {
            return Math.Exp(x);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///Returns Euler's number <i>e</i> raised to the power of a double value. 
        /// </summary>
        /// <remarks>
        ///  Special cases:
        /// <ul><li>If the argument is NaN, the result is NaN.</li>
        /// <li>If the argument is positive infinity, then the result is
        /// positive infinity.</li>
        /// <li>If the argument is negative infinity, then the result is
        /// positive zero.</li>  </ul> 
        /// A result must be within 1 ulp of the correctly rounded result.  Results
        /// must be semi-monotonic.
        /// </remarks>
        /// <param name="a">the exponent to raise e to.</param>
        /// <returns>the value ea, where e is the base of the natural logarithms.</returns>
        public static double Exp1(double a)
        {
            if (a == 0.0)
            {
                return 1.0;
            }
            bool isless = (a < 0.0);
            if (isless)
            {
                a = -a;
            }
            long intPart = (int)a;
            double fractionPart = a - intPart;
            double ret = 1;
            for (long i = 0; i < intPart; i++)
            {
                ret *= E;
            }
            double n = 1;
            double an = fractionPart;
            double sn = 1;
            double subRes = 1;
            if (fractionPart > 0)
            {
                subRes += fractionPart;
                while (an > Precision)
                {
                    an *= fractionPart;
                    n++;
                    sn = sn * n;
                    subRes += an / sn;

                }
            }
            ret *= subRes;
            if (isless)
            {
                return 1 / ret;
            }
            return ret;
        }

        ///<summary>
        ///</summary>
        const  double LoGdiv2 = -0.6931471805599453094;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Logs the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public static double Log(double x)
        {
            if (!(x > 0.0))
            {
                return Double.NaN;
            }
            //
            double f = 0.0;
            //
            int appendix = 0;
            while (x > 0.0 && x <= 1.0)
            {
                x *= 2.0;
                appendix++;
            }
            //
            x /= 2.0;
            appendix--;
            //
            double y1 = x - 1.0;
            double y2 = x + 1.0;
            double y = y1 / y2;
            //
            double k = y;
            y2 = k * y;
            //
            for (long i = 1; i < 50; i += 2)
            {
                f += k / i;
                k *= y2;
            }
            //
            f *= 2.0;
            for (int i = 0; i < appendix; i++)
            {
                f += LoGdiv2;
            }
            //
            return f;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Log2s the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        static public double Log2(double x)
        {
            if (!(x > 0.0))
            {
                return Double.NaN;
            }
            //
            if (x == 1.0)
            {
                return 0.0;
            }
            // Argument of Log must be (0; 1]
            if (x > 1.0)
            {
                x = 1 / x;
                return -Log(x);
            }
            //
            return Log(x);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///Returns the natural logarithm (base <i>e</i>) of a double value.  
        /// </summary>
        /// <remarks>
        /// Special cases:
        /// <ul><li>If the argument is NaN or less than zero, then the result
        /// is NaN.</li>
        /// <li>If the argument is positive infinity, then the result is
        /// positive infinity.</li>
        /// <li>If the argument is positive zero or negative zero, then the
        /// result is negative infinity.</li>  </ul>
        /// A result must be within 1 ulp of the correctly rounded result.  Results
        /// must be semi-monotonic.
        /// </remarks>
        /// <param name="a">a number greater than 0.0</param>
        /// <returns>the value ln a, the natural logarithm of a.</returns>
        public static double Log1(double a)
        {
            if (a <= 0.0)
            {
                return Double.NaN;
            }
            bool invert = false;
            if (a > 1.0)
            {
                invert = true;
                a = 1 / a;
            }
            if (a == 1.0)
            {
                return 0.0;
            }
            double x = a - 1.0;
            double a0 = x;
            double n = 1;
            double a1 = (-1) * x * n / (n + 1) * a0;
            double s = a0;
            while (Math.Abs(a1) > Precision)
            {
                s += a1;
                n += 1.0;
                a0 = a1;
                a1 = (-1) * x * n / (n + 1) * a0;
            }
            if (invert)
            {
                return -s;
            }
            return s;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///Returns the value of the first argument raised to the power of the
        /// second argument. 
        /// </summary>
        /// <param name="a">the base</param>
        /// <param name="b"> the exponent.</param>
        /// <returns>the value a<sup>b</sup>.</returns>
        public static double Pow(double a, double b)
        {
            return Math.Pow(a, b);

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///Returns the arc sine of an angle, in the range of -<i>pi</i>/2 through
        /// <i>pi</i>/2.
        /// </summary>
        /// <remarks>
        ///  Special cases:
        /// <ul><li>If the argument is NaN or its absolute value is greater
        /// than 1, then the result is NaN. </li>
        /// <li>If the argument is zero, then the result is a zero with the
        /// same sign as the argument.</li>  </ul> 
        /// A result must be within 1 ulp of the correctly rounded result.  Results
        /// must be semi-monotonic.
        /// </remarks>
        /// <param name="a">the value whose arc sine is to be returned.</param>
        /// <returns>the arc sine of the argument.</returns>
        public static double Asin(double a)
        {
            return Math.Asin(a);

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///Returns the arc cosine of an angle, in the range of 0.0 through
        /// <i>pi</i>.  
        /// </summary>
        /// <remarks>
        /// Special case:
        /// If the argument is NaN or its absolute value is greater
        /// than 1, then the result is NaN.
        /// A result must be within 1 ulp of the correctly rounded result.  Results
        /// must be semi-monotonic.
        /// </remarks>
        /// <param name="a">the value whose arc cosine is to be returned.</param>
        /// <returns>the arc cosine of the argument.</returns>
        public static double Acos(double a)
        {
            double f = Asin(a);
            if (f == Double.NaN)
            {
                return f;
            }
            return Math.PI / 2 - f;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Rints the specified a.
        /// </summary>
        /// <param name="a">A.</param>
        /// <returns></returns>
        public static double Rint(double a)
        {
            return a;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///Computes the remainder operation on two arguments as prescribed
        /// by the IEEE 754 standard.
        /// </summary>
        /// <remarks>
        /// The remainder value is mathematically equal to
        /// f1-f2<i>n</i>,
        /// where <i>n</i> is the mathematical integer closest to the exact
        /// mathematical value of the quotient f1/f2, and if two
        /// mathematical integers are equally close to f1/f2,
        /// then <i>n</i> is the integer that is even. If the remainder is
        /// zero, its sign is the same as the sign of the first argument.
        /// Special cases:
        /// <ul><li>If either argument is NaN, or the first argument is infinite,
        /// or the second argument is positive zero or negative zero, then the
        /// result is NaN.</li>
        /// <li>If the first argument is finite and the second argument is
        /// infinite, then the result is the same as the first argument.</li> </ul>
        /// </remarks>
        /// <param name="f1"> f1 the dividend.</param>
        /// <param name="f2"> f2 the divisor</param>
        /// <returns>the remainder when f1 is divided by f2</returns>
        public static double IEEERemainder(double f1, double f2)
        {
            return Math.IEEERemainder(f1, f2);

        }
    }

}
