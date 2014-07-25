//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 14OCT2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System;

//--------------------------------- PACKAGE ------------------------------------

namespace Mapdigit.Drawing
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 14OCT2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// The Color class is used to encapsulate colors in the default
    /// sRGB color space.
    /// </summary>
    /// <remarks>
    /// Every color has an implicit alpha value of 255 or an explicit one provided
    /// in the constructor.  The alpha value  defines the transparency of a color
    /// and can be represented by a int value in the range 0-255.
    /// An alpha value of  255 means that the color is completely
    /// opaque and an alpha value of 0 means that the color is
    /// completely transparent.
    /// </remarks>
    public class Color
    {
        //[------------------------------ CONSTANTS -------------------------------]
        ///<summary>
        /// Represents image data that is guaranteed to be completely opaque,
        /// meaning that all pixels have an alpha Value of 255.
        ///</summary>
        public const int Opaque = 1;

        /// <summary>
        /// Represents image data that is guaranteed to be either completely
        /// opaque, with an alpha Value of 255, or completely transparent,
        /// with an alpha Value of 0. 
        /// </summary>
        public const int Bitmask = 2;


        /// <summary>
        /// Represents image data that contains or might contain arbitrary
        /// alpha values between and including 0 and 255.
        /// </summary>
        public const int Translucent = 3;


        /// <summary>
        /// The color white.
        /// </summary>
        public static readonly Color White = new Color(255, 255, 255);


        /// <summary>
        /// The color light gray.
        /// </summary>
        public static readonly Color LightGray = new Color(192, 192, 192);

        /// <summary>
        /// The color gray.
        /// </summary>
        public static readonly Color Gray = new Color(128, 128, 128);

        /// <summary>
        /// The color dark gray.
        /// </summary>
        public static readonly Color DarkGray = new Color(64, 64, 64);


        /// <summary>
        /// The color black.
        /// </summary>
        public static readonly Color Black = new Color(0, 0, 0);


        /// <summary>
        /// The color red.
        /// </summary>
        public static readonly Color Red = new Color(255, 0, 0);


        /// <summary>
        /// The color pink.
        /// </summary>
        public static readonly Color Pink = new Color(255, 175, 175);


        /// <summary>
        /// The color orange.
        /// </summary>
        public static readonly Color Orange = new Color(255, 200, 0);


        /// <summary>
        /// The color yellow.
        /// </summary>
        public static readonly Color Yellow = new Color(255, 255, 0);


        /// <summary>
        /// The color green.
        /// </summary>
        public static readonly Color Green = new Color(0, 255, 0);


        /// <summary>
        /// The color magenta.
        /// </summary>
        public static readonly Color Magenta = new Color(255, 0, 255);


        /// <summary>
        /// The color cyan.
        /// </summary>
        public static readonly Color Cyan = new Color(0, 255, 255);


        /// <summary>
        /// The color blue.
        /// </summary>
        public static readonly Color Blue = new Color(0, 0, 255);


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates an opaque sRGB color with the specified red, green,
        /// and blue values in the range (0 - 255).
        /// The actual color used in rendering depends
        /// on finding the best match given the color space
        /// available for a given output device.
        /// Alpha is defaulted to 255.
        /// </summary>
        /// <param name="r">the red component.</param>
        /// <param name="g">the green component</param>
        /// <param name="b">the blue component</param>
        public Color(int r, int g, int b)
            : this(r, g, b, 255)
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates an sRGB color with the specified red, green, blue, and alpha
        /// values in the range (0 - 255).
        /// </summary>
        /// <param name="r">the red component</param>
        /// <param name="g">the green component</param>
        /// <param name="b">the blue component</param>
        /// <param name="a">the alpha component</param>
        public Color(int r, int g, int b, int a)
        {
            _value = ((a & 0xFF) << 24) |
                     ((r & 0xFF) << 16) |
                     ((g & 0xFF) << 8) |
                     ((b & 0xFF));
            TestColorValueRange(r, g, b, a);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates an opaque sRGB color with the specified combined RGB value
        /// consisting of the red component in bits 16-23, the green component
        /// in bits 8-15, and the blue component in bits 0-7.  The actual color
        /// used in rendering depends on finding the best match given the
        /// color space available for a particular output device.  Alpha is
        /// defaulted to 255.
        /// </summary>
        /// <param name="rgb">the combined RGB components</param>
        /// <param name="hasAlpha">true,if the alpha bits are valid</param>
        public Color(uint rgb, bool hasAlpha)
        {
            if (rgb > 0x800000)
            {
                _value = -(int)(0x100000000L - rgb);
            }
            else
            {
                _value = (int)rgb;
            }
            if (!hasAlpha)
            {
                _value = (int)(0xff000000 | (uint)_value);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates an opaque sRGB color with the specified combined RGB value
        /// consisting of the red component in bits 16-23, the green component
        /// in bits 8-15, and the blue component in bits 0-7.  The actual color
        /// used in rendering depends on finding the best match given the
        /// color space available for a particular output device.  Alpha is
        /// defaulted to 255.
        /// </summary>
        /// <param name="rgb">the combined RGB components</param>
        public Color(int rgb)
        {
            _value = (int)(0xff000000 | (uint)rgb);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates an sRGB color with the specified combined RGBA value consisting
        /// of the alpha component in bits 24-31, the red component in bits 16-23,
        /// the green component in bits 8-15, and the blue component in bits 0-7.
        /// If the hasalpha argument is false, alpha
        /// is defaulted to 255.
        /// </summary>
        /// <param name="rgba">the combined RGBA components</param>
        /// <param name="hasalpha">hasalpha true if the alpha bits are valid;
        /// false otherwise</param>
        public Color(int rgba, bool hasalpha)
        {
            if (hasalpha)
            {
                _value = rgba;
            }
            else
            {
                _value = (int)(0xff000000 | (uint)rgba);
            }
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the red component in the range 0-255 in the default sRGB
        /// </summary>
        public int R
        {
            get { return (Argb >> 16) & 0xFF; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the green component in the range 0-255 in the default sRGB
        /// </summary>
        public int G
        {
            get { return (Argb >> 8) & 0xFF; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the blue component in the range 0-255 in the default sRGB
        /// </summary>
        public int B
        {
            get { return (Argb) & 0xFF; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the alpha component in the range 0-255 in the default sRGB
        /// </summary>
        public int A
        {
            get { return (Argb >> 24) & 0xff; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///Returns the RGB value representing the color in the default sRGB
        /// (Bits 24-31 are alpha, 16-23 are red, 8-15 are green, 0-7 are blue).
        /// </summary>
        public int Argb
        {
            get { return _value; }
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates a new Color that is a brighter version of this Color.
        /// This method applies an arbitrary scale factor to each of the three RGB
        /// components of this Color to create a brighter version
        /// of this Color. Although brighter and darker are inverse operations,
        /// the results of a series of invocations of these two methods might be 
        /// inconsistent because of rounding errors.
        /// </summary>
        /// <returns>a new Color object that is a brighter version of this Color.</returns>
        public Color Brighter()
        {
            int r = R;
            int g = G;
            int b = B;

            /* From 2D group:
             * 1. black.brighter() should return grey
             * 2. applying brighter to blue will always return blue, brighter
             * 3. non pure color (non zero rgb) will eventually return white
             */
            const int i = (int)(1.0 / (1.0 - Factor));
            if (r == 0 && g == 0 && b == 0)
            {
                return new Color(i, i, i);
            }
            if (r > 0 && r < i) r = i;
            if (g > 0 && g < i) g = i;
            if (b > 0 && b < i) b = i;

            return new Color(Math.Min((int)(r / Factor), 255),
                             Math.Min((int)(g / Factor), 255),
                             Math.Min((int)(b / Factor), 255));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates a new Color that is a darker version of this Color.
        /// This method applies an arbitrary scale factor to each of the three RGB
        /// components of this Color to create a darker version of this Color.  
        /// Although brighter and darker are inverse operations, the results of a series
        /// of invocations of these two methods might be inconsistent because
        /// of rounding errors.
        /// </summary>
        /// <returns> a new Color object that is a darker version of this Color.</returns>
        public Color Darker()
        {
            return new Color(Math.Max((int)(R * Factor), 0),
                             Math.Max((int)(G * Factor), 0),
                             Math.Max((int)(B * Factor), 0));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms 
        /// and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return _value;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to 
        /// this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(Object obj)
        {
            return obj is Color && ((Color)obj)._value == _value;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "[r=" + R + ",g=" + G + ",b=" + B + "]";
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the transparency mode for this Color. 
        /// </summary>
        public int Transparency
        {
            get
            {
                int alpha = A;
                if (alpha == 0xff)
                {
                    return Opaque;
                }
                if (alpha == 0)
                {
                    return Bitmask;
                }
                return Translucent;
            }
        }


        /**
         * The color Value.
         */
        internal int _value;

        /// <summary>
        /// darker and brigher factor.
        /// </summary>
        private const double Factor = 0.7;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Initial Creation
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Checks the color integer components supplied for validity.
        /// </summary>
        /// <param name="r">The r.</param>
        /// <param name="g">The g.</param>
        /// <param name="b">The b.</param>
        /// <param name="a">A.</param>
        private static void TestColorValueRange(int r, int g, int b, int a)
        {
            bool rangeError = false;
            string badComponentString = "";

            if (a < 0 || a > 255)
            {
                rangeError = true;
                badComponentString = badComponentString + " Alpha";
            }
            if (r < 0 || r > 255)
            {
                rangeError = true;
                badComponentString = badComponentString + " Red";
            }
            if (g < 0 || g > 255)
            {
                rangeError = true;
                badComponentString = badComponentString + " Green";
            }
            if (b < 0 || b > 255)
            {
                rangeError = true;
                badComponentString = badComponentString + " Blue";
            }
            if (rangeError)
            {
                throw new ArgumentException("Color parameter outside of expected range:"
                                            + badComponentString);
            }
        }
    }
}