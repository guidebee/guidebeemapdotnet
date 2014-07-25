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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Mapdigit.Gis.Drawing;

//--------------------------------- PACKAGE ------------------------------------
namespace PocketStreets.Drawing
{
    ////////////////////////////////////////////////////////////////////////////
    //--------------------------------- REVISIONS ------------------------------
    // Date       Name                 Tracking #         Description
    // ---------  -------------------  -------------      ----------------------
    // 20SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// IImage implemation.
    /// </summary>
    class NETImage : IImage
    {
        internal Bitmap _image;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 20SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates the image.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static IImage CreateImage(Stream stream)
        {
            NETImage netImage = new NETImage {_image = new Bitmap(stream)};

            return netImage;
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
        /// <param name="rgb">The RGB.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public static IImage CreateImage(int[] rgb, int width, int height)
        {
            NETImage netImage = new NETImage {_image = new Bitmap(width, height)};
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData lockedBitmapData = netImage._image.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);

            System.Runtime.InteropServices.Marshal.Copy(rgb, 0, lockedBitmapData.Scan0, rgb.Length);
            netImage._image.UnlockBits(lockedBitmapData);
            return netImage;
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
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public static IImage CreateImage(int width,
                                         int height)
        {
            NETImage netImage = new NETImage {_image = new Bitmap(width, height)};

            return netImage;
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
        /// <param name="bytes">The bytes.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="len">The len.</param>
        /// <returns></returns>
        public static IImage CreateImage(byte[] bytes,
                                         int offset,
                                         int len)
        {
            MemoryStream memoryStream = new MemoryStream(bytes, offset, len);
            NETImage netImage = new NETImage {_image = new Bitmap(memoryStream)};
            return netImage;


        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 20SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the graphcis object associated with this image.
        /// </summary>
        /// <returns>
        /// a graphics object which allow to draw on the image.
        /// </returns>
        public IGraphics GetGraphics()
        {
            NETGraphics netGraphics = new NETGraphics(Graphics.FromImage(_image));

            return netGraphics;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 20SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// get the RGB array of this image
        /// </summary>
        /// <returns>the rgb array of the image.</returns>
        public int[] GetRGB()
        {
            int[] rgb = new int[_image.Width * _image.Height];
            Rectangle rect = new Rectangle(0, 0, _image.Width, _image.Height);
            BitmapData lockedBitmapData = _image.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);

            System.Runtime.InteropServices.Marshal.Copy(lockedBitmapData.Scan0, rgb, 0, rgb.Length);
            _image.UnlockBits(lockedBitmapData);
            return rgb;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 20SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// get the height of the image.
        /// </summary>
        /// <returns>the height of the image.</returns>
        public int GetHeight()
        {
            return _image.Height;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 20SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// get the width of the image
        /// </summary>
        /// <returns>the width of the image</returns>
        public int GetWidth()
        {
            return _image.Width;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 20SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// get the native image assocated with this Image object.
        /// </summary>
        /// <returns>the native image object</returns>
        public object GetNativeImage()
        {
            return _image;
        }

    }
}