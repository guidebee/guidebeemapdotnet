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
    /// The ColorFP class is used to encapsulate colors in the default
    /// sRGB color space  Every color has an implicit alpha value of 1.0 or
    /// an explicit one provided in the constructor.  The alpha value
    /// defines the transparency of a color and can be represented by
    /// a int value in the range 0-255.
    /// An alpha value of  255 means that the color is completely
    /// opaque and an alpha value of 0 means that the color is
    /// completely transparent.
    /// </summary>
    internal class ColorFP
    {
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Creates a Color structure from the four 8-bit ARGB components
        /// (alpha, red, green, and blue) values.
        /// </summary>
        /// <param name="color">A value specifying the 32-bit ARGB value</param>
        /// <returns>The Color object that this method creates</returns>
        public static ColorFP FromArgb(int color)
        {
            return new ColorFP(color);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates a Color structure from the four 8-bit ARGB components
        /// (alpha, red, green, and blue) values.
        /// </summary>
        /// <param name="red">The red.</param>
        /// <param name="green">The green.</param>
        /// <param name="blue">The blue.</param>
        /// <returns>The Color object that this method creates.</returns>
        public static ColorFP FromArgb(int red, int green, int blue)
        {
            var value =
                ((red & 0xFF) << 16) |
                ((green & 0xFF) << 8) |
                ((blue & 0xFF) << 0);
            return new ColorFP(value);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates an opaque sRGB color with the specified combined RGB value
        /// consisting of the red component in bits 16-23, the green component
        /// in bits 8-15, and the blue component in bits 0-7.  The actual color
        /// used in rendering depends on finding the best match given the
        /// color space available for a particular output device.  Alpha is
        /// defaulted to 255.
        /// </summary>
        /// <param name="rgb">the combined RGB components.</param>
        public ColorFP(int rgb)
        {
            Value = rgb;
            Red = (Value >> 16) & 0xFF;
            Green = (Value >> 8) & 0xFF;
            Blue = Value & 0xFF;
            Alpha = (Value >> 24) & 0xff;
        }

        /**
         * The color Value.
         */
        public int Value;

        /**
         * the red component.
         */
        public int Red;

        /**
         * the green compoent
         */
        public int Green;

        /**
         * the blue component.
         */
        public int Blue;

        /**
         * the alpha compoent.
         */
        public int Alpha;
    }
}