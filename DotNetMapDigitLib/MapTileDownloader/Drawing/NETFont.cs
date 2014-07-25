//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 20SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System;
using System.Drawing;
using Mapdigit.Gis.Drawing;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Drawing
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 20SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// IFont implementation.
    /// </summary>
    public class NETFont : IFont
    {

        internal Font _font;
        internal static  Graphics _graphics;
        private readonly object _syncObject = new object();

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 20SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes the <see cref="NETFont"/> class.
        /// </summary>
        static NETFont()
        {
            Bitmap map = new Bitmap(20, 20);
            _graphics = Graphics.FromImage(map);

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 20SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the navtive font object.
        /// </summary>
        /// <returns>
        /// the nactive font associated with the IFont object
        /// </returns>
        public object GetNativeFont()
        {
            return _font;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 20SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// calculate the chars width with this font.
        /// </summary>
        /// <param name="ch">the char array.</param>
        /// <param name="offset">the start index of the char array.</param>
        /// <param name="length">the length of the chars.</param>
        /// <returns>the width of the char string</returns>
        public int CharsWidth(char[] ch, int offset, int length)
        {
            lock (_syncObject)
            {
                char[] str = new char[length];
                Array.Copy(ch, offset, str, 0, length);
                return (int) _graphics.MeasureString(new string(str), _font).Width;
            }

        }

    }

}
