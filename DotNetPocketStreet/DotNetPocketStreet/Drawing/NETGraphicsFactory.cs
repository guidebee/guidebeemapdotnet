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
using System.IO;
using Mapdigit.Gis.Drawing;

//--------------------------------- PACKAGE ------------------------------------

namespace PocketStreets.Drawing
{
    /// <summary>
    /// AbstractGraphicsFactory implementation.
    /// </summary>
    public class NETGraphicsFactory : AbstractGraphicsFactory
    {

        private static readonly NETGraphicsFactory Instance = new NETGraphicsFactory();

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 20SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="NETGraphicsFactory"/> class.
        /// </summary>
        private NETGraphicsFactory()
        {

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 20SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns></returns>
        public static NETGraphicsFactory GetInstance()
        {
            return Instance;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 20SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Create an image instace from an input stream.
        /// </summary>
        /// <param name="stream">the input stream.</param>
        /// <returns>image instance</returns>
        public IImage CreateImage(Stream stream)
        {
            try
            {
                return NETImage.CreateImage(stream);
            }
            catch (IOException)
            {

            }
            return null;
        }



        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 20SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates the image.
        /// </summary>
        /// <param name="width">the width of the image.</param>
        /// <param name="height">the height of the image.</param>
        /// <returns>an image instance</returns>
        public override IImage CreateImage(int width, int height)
        {
            return NETImage.CreateImage(width, height);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 20SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Create an image instance from rgb array
        /// </summary>
        /// <param name="rgb">the rgb array.</param>
        /// <param name="width">the width of the image.</param>
        /// <param name="height">the height of the image.</param>
        /// <returns>an image instance</returns>
        public override IImage CreateImage(int[] rgb, int width, int height)
        {
            return NETImage.CreateImage(rgb, width, height);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 20SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Create an image instance.
        /// </summary>
        /// <param name="bytes">the byte array</param>
        /// <param name="offset">the start position for the image.</param>
        /// <param name="len">he lenght of the image..</param>
        /// <returns>an image instance</returns>
        public override IImage CreateImage(byte[] bytes, int offset, int len)
        {
            return NETImage.CreateImage(bytes, offset, len);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 20SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Create an font instance from native font.
        /// </summary>
        /// <param name="nativeFont">the native font instance.</param>
        /// <returns>and IFont object</returns>
        public IFont CreateFont(Object nativeFont)
        {
            if (nativeFont is Font)
            {
                NETFont netFont = new NETFont { _font = (Font)nativeFont };
                return netFont;
            }
            throw new ArgumentException("Font type is not valid");
        }
    }
}