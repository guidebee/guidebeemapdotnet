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

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Drawing.Core
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 24SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Fixed point float math.
    /// </summary>
    internal class MathFP
    {
        /**
         * Pi.
         */
        public const int Pi = 205887;

        /**
         * E
         */
        public const int E = 178145;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// return the minimun of the two values.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static int Min(int x, int y)
        {
            return x < y ? x : y;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// return the maximum of the two values.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static int Max(int x, int y)
        {
            return x > y ? x : y;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// return the abs of the value.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public static int Abs(int x)
        {
            return x < 0 ? -x : x;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// return the product of the two values.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static int Mul(int x, int y)
        {
            var res = x*(long) y >> SingleFP.DecimalBits;
            return (int) res;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// return the division of the two values.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static int Div(int x, int y)
        {
            var res = ((long) x << SingleFP.DecimalBits)/y;
            return (int) res;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// return the square root of the given value.
        /// </summary>
        /// <param name="n">The n.</param>
        /// <returns></returns>
        public static int Sqrt(int n)
        {
            int s;
            if (n < (1000 << SingleFP.DecimalBits))
            {
                s = n/20;
            }
            else if (n < (2500 << SingleFP.DecimalBits))
            {
                s = n/40;
            }
            else if (n < (5000 << SingleFP.DecimalBits))
            {
                s = n/60;
            }
            else if (n < (10000 << SingleFP.DecimalBits))
            {
                s = n/86;
            }
            else if (n < (25000 << SingleFP.DecimalBits))
            {
                s = n/132;
            }
            else
            {
                s = n/168;
            }

            s = (s + Div(n, s)) >> 1;
            s = (s + Div(n, s)) >> 1;
            s = (s + Div(n, s)) >> 1;
            return s;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// calculat the IEEE Reminder.
        /// </summary>
        /// <param name="n">The n.</param>
        /// <param name="m">The m.</param>
        /// <returns></returns>
        public static int IEEERemainder(int n, int m)
        {
            return n - Mul(Floor(Div(n, m)), m);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// calculate the floor of the value.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public static int Floor(int x)
        {
            return x < 0 && (-x & 0xFFFF) != 0
                       ?
                           -((-x + SingleFP.One >> SingleFP.DecimalBits) << SingleFP.DecimalBits)
                       : ((x >> SingleFP.DecimalBits) << SingleFP.DecimalBits);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Calculate the round the value.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public static int Round(int x)
        {
            if (x < 0)
            {
                return -(((-x + SingleFP.One/2) >> SingleFP.DecimalBits)
                         << SingleFP.DecimalBits);
            }
            return ((x + SingleFP.One/2) >> SingleFP.DecimalBits)
                   << SingleFP.DecimalBits;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// to degree.
        /// </summary>
        /// <param name="f">The f.</param>
        /// <returns></returns>
        public static int ToDegrees(int f)
        {
            return Div(f*180, Pi);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// to radians.
        /// </summary>
        /// <param name="f">The f.</param>
        /// <returns></returns>
        public static int ToRadians(int f)
        {
            return Mul(f, Pi)/180;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// calculate of the sine.
        /// </summary>
        /// <param name="f">The f.</param>
        /// <returns></returns>
        public static int Sin(int f)
        {
            if (f < 0 || f >= Pi*2)
            {
                f = IEEERemainder(f, Pi*2);
            }
            var sign = 1;
            if ((f > Pi/2) && (f <= Pi))
            {
                f = Pi - f;
            }
            else if ((f > Pi) && (f <= (Pi + Pi/2)))
            {
                f = f - Pi;
                sign = -1;
            }
            else if (f > (Pi + Pi/2))
            {
                f = (Pi << 1) - f;
                sign = -1;
            }

            var sqr = Mul(f, f);
            var result = 498;
            result = Mul(result, sqr);
            result -= 10882;
            result = Mul(result, sqr);
            result += (1 << SingleFP.DecimalBits);
            result = Mul(result, f);
            return sign*result;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// calculate the cosine.
        /// </summary>
        /// <param name="ffAng">The ff ang.</param>
        /// <returns></returns>
        public static int Cos(int ffAng)
        {
            return Sin(Pi/2 - ffAng);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  return the tan.
        /// </summary>
        /// <param name="f">The f.</param>
        /// <returns></returns>
        public static int Tan(int f)
        {
            return Div(Sin(f), Cos(f));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// return the atan.
        /// </summary>
        /// <param name="ffVal">The ff val.</param>
        /// <returns></returns>
        public static int Atan(int ffVal)
        {
            var ffVal1 = ffVal > SingleFP.One
                             ? Div(SingleFP.One, ffVal)
                             : (ffVal < -SingleFP.One ? Div(SingleFP.One, -ffVal) : ffVal);
            var sqr = Mul(ffVal1, ffVal1);
            var result = 1365;
            result = Mul(result, sqr);
            result -= 5579;
            result = Mul(result, sqr);
            result += 11805;
            result = Mul(result, sqr);
            result -= 21646;
            result = Mul(result, sqr);
            result += 65527;
            result = Mul(result, ffVal1);
            return ffVal > SingleFP.One
                       ? Pi/2 - result
                       : (ffVal < -SingleFP.One ? -(Pi/2 - result) : result);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// return the asin.
        /// </summary>
        /// <param name="f">The f.</param>
        /// <returns></returns>
        public static int Asin(int f)
        {
            return Pi/2 - Acos(f);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// return the acosine.
        /// </summary>
        /// <param name="f">The f.</param>
        /// <returns></returns>
        public static int Acos(int f)
        {
            var fRoot = Sqrt(SingleFP.One - f);
            var result = -1228;
            result = Mul(result, f);
            result += 4866;
            result = Mul(result, f);
            result -= 13901;
            result = Mul(result, f);
            result += 102939;
            result = Mul(fRoot, result);
            return result;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// return the minimum of the two values.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static long Min(long x, long y)
        {
            return x < y ? x : y;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// return the maximum of the two values.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static long Max(long x, long y)
        {
            return x > y ? x : y;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// return the abs of the value.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public static long Abs(long x)
        {
            return x < 0 ? -x : x;
        }
    }
}