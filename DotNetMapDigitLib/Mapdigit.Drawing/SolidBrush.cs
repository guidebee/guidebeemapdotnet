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
using Mapdigit.Drawing.Core;

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
    /// Defines a brush of a single color. Brushes are used to fill graphics shapes,
    /// such as rectangles, ellipses, pies, polygons, and paths. This class cannot
    /// be inherited.
    /// </summary>
    public sealed class SolidBrush : Brush
    {
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates an opaque sRGB brush with the specified red, green,
        /// and blue values in the range (0 - 255).
        /// The actual color used in rendering depends
        /// on finding the best match given the color space
        /// available for a given output device.
        /// Alpha is defaulted to 255.
        /// </summary>
        /// <param name="r">the red component</param>
        /// <param name="g">the green component</param>
        /// <param name="b">the blue component.</param>
        public SolidBrush(int r, int g, int b)
        {
            _brushColor = new Color(r, g, b);
            _wrappedBrushFp = new SolidBrushFP(_brushColor._value);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates an opaque sRGB brush with the specified red, green,
        /// and blue values in the range (0 - 255).
        /// The actual color used in rendering depends
        /// on finding the best match given the color space
        /// available for a given output device.
        /// Alpha is defaulted to 255.
        /// </summary>
        /// <param name="r">the red component</param>
        /// <param name="g">the green component</param>
        /// <param name="b">the blue component.</param>
        /// <param name="a">the alpha value</param>
        public SolidBrush(int r, int g, int b, int a)
        {
            _brushColor = new Color(r, g, b, a);
            _wrappedBrushFp = new SolidBrushFP(_brushColor._value);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates an opaque sRGB brush with the specified combined RGB value
        /// consisting of the red component in bits 16-23, the green component
        /// in bits 8-15, and the blue component in bits 0-7.  The actual color
        /// used in rendering depends on finding the best match given the
        /// color space available for a particular output device.  Alpha is
        /// defaulted to 255.
        /// </summary>
        /// <param name="rgb">the combined RGB components</param>
        public SolidBrush(int rgb)
        {
            _brushColor = new Color(rgb);
            _wrappedBrushFp = new SolidBrushFP(_brushColor._value);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates an sRGB brush with the specified combined RGBA value consisting
        /// of the alpha component in bits 24-31, the red component in bits 16-23,
        /// the green component in bits 8-15, and the blue component in bits 0-7.
        /// If the hasalpha argument is false, alpha
        /// is defaulted to 255.
        /// </summary>
        /// <param name="rgba">the combined RGBA components</param>
        /// <param name="hasalpha"> true if the alpha bits are valid;
        ///         false otherwise</param>
        public SolidBrush(int rgba, bool hasalpha)
        {
            _brushColor = new Color(rgba, hasalpha);
            _wrappedBrushFp = new SolidBrushFP(_brushColor._value);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates an sRGB brush with the specified combined RGBA value consisting
        /// of the alpha component in bits 24-31, the red component in bits 16-23,
        /// the green component in bits 8-15, and the blue component in bits 0-7.
        /// If the hasalpha argument is false, alpha
        /// is defaulted to 255.
        /// </summary>
        /// <param name="color"> the color of the brush.</param>
        public SolidBrush(Color color)
        {
            _brushColor = color;
            _wrappedBrushFp = new SolidBrushFP(_brushColor._value);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// get the color of the solid brush.
        /// </summary>
        public Color Color
        {
            get { return _brushColor; }
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
        /// <returns>
        /// this Color object's transparency mode.
        /// </returns>
        public override int Transparency
        {
            get { return _brushColor.Transparency; }
        }

        //the color of the solid brush
        private readonly Color _brushColor;
    }
}