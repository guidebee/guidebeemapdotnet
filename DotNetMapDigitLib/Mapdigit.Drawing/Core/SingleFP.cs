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
    /// Single is a fix point single class.
    /// </summary>
    internal class SingleFP
    {
        /**
         * Positive Infinity.
         */
        private const int PositiveInfinity = int.MaxValue;

        /**
         * Negative infinity.
         */
        private const int NegativeInfinity = int.MinValue;

        /**
         * Max Value.
         */
        public const int MaxValue = PositiveInfinity - 1;

        /**
         * Min Value.
         */
        public const int MinValue = NegativeInfinity + 2;


        /**
         * Not a number.
         */
        public const int NotANumber = NegativeInfinity + 1;

        /**
         * Fix point length.
         */
        public const int DecimalBits = 16;

        /**
         * the number 1 in this fix point float.
         */
        public const int One = 1 << DecimalBits;

        /**
         * int format for this single.
         */
        private readonly int _value;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="SingleFP"/> class.
        /// </summary>
        /// <param name="v">the integer format for this fixed point number.</param>
        public SingleFP(int v)
        {
            _value = v;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="SingleFP"/> class.
        /// </summary>
        /// <param name="f">The single to be copied from.</param>
        public SingleFP(SingleFP f)
        {
            _value = f._value;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Test if it's NaN.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns>
        /// 	true, if it's NaN.
        /// </returns>
        public static bool IsNaN(int x)
        {
            return x == NotANumber;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determines whether the specified x is infinity.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns>
        /// 	<c>true</c> if the specified x is infinity; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInfinity(int x)
        {
            return x == NegativeInfinity || x == PositiveInfinity;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Test if it's negative infinity.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns>
        /// 	 true it's  negitive infinity.
        /// </returns>
        public static bool IsNegativeInfinity(int x)
        {
            return x == NegativeInfinity;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Test if it's positive infinity.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns>
        /// 	true it's  positive infinity.
        /// </returns>
        public static bool IsPositiveInfinity(int x)
        {
            return x == PositiveInfinity;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// convert a float to this fixed point float.
        /// </summary>
        /// <param name="f">a float number</param>
        /// <returns></returns>
        public static int FromFloat(float f)
        {
            return (int) (f*One + 0.5);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// convert a double to this fixed point float.
        /// </summary>
        /// <param name="f">The f.</param>
        /// <returns></returns>
        public static int FromDouble(double f)
        {
            return (int) (f*One + 0.5);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Convert this fixed point float to a float.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public static float ToFloat(int x)
        {
            return (float) x/One;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Convert this fixed point float to a double.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public static double ToDouble(int x)
        {
            return (double) x/One;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Convert an integer to the fixed point float.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public static int FromInt(int x)
        {
            return x << DecimalBits;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Convert the fixed point float back to an integer.
        /// </summary>
        /// <param name="ffX">The ff X.</param>
        /// <returns></returns>
        public static int ToInt(int ffX)
        {
            return ffX >> DecimalBits;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parse an string can convert it to fixed point float.
        /// </summary>
        /// <param name="strValue">a string reprents a float</param>
        /// <returns></returns>
        public static SingleFP ParseSingle(string strValue)
        {
            var s = strValue;
            var eNeg = false;
            int v, e = 0;
            var posE = s.IndexOf('E');
            if (posE == -1)
            {
                posE = s.IndexOf('e');
            }
            if (posE != -1)
            {
                e = int.Parse(s.Substring(posE + 1));
                if (e < 0)
                {
                    eNeg = true;
                    e = -e;
                }
                s = s.Substring(0, (posE) - (0));
            }
            var posDot = s.IndexOf('.');
            if (posDot == -1)
            {
                v = int.Parse(s);
                v = v << DecimalBits;
            }
            else
            {
                v = int.Parse(s.Substring(0, (posDot) - (0))) << DecimalBits;
                s = s.Substring(posDot + 1);
                s = s + "0000";
                s = s.Substring(0, (4) - (0));
                var f = int.Parse(s);
                f = (f << DecimalBits)/10000;
                if (v < 0)
                {
                    v -= f;
                }
                else
                {
                    v += f;
                }
            }
            for (int i = 0; i < e; i++)
            {
                if (eNeg)
                {
                    v /= 10;
                }
                else
                {
                    v *= 10;
                }
            }
            return new SingleFP(v);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var s = "";
            var v = _value;
            if (v < 0)
            {
                s = "-";
                v = -v;
            }
            s = s + (v >> DecimalBits);
            v = 0xFFFF & v;
            if (v != 0)
            {
                s = s + ".";
            }
            //while (v != 0)
            for (int i = 0; i < 4; i++)
            {
                v = v*10;
                s = s + (v >> DecimalBits);
                v = 0xFFFF & v;
            }
            return s;
        }
    }
}