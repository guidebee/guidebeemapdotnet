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
using System.Drawing.Imaging;
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
    /// IGraphics implemation.
    /// </summary>
    public class NETGraphics : IGraphics
    {
        private readonly Graphics _graphics;
        private readonly ImageAttributes _imageAttributes = new ImageAttributes();
        private Color _color = Color.Black;
        private Font _font = new Font(FontFamily.GenericSerif, 12, FontStyle.Regular);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 20SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="NETGraphics"/> class.
        /// </summary>
        /// <param name="graphics">The graphics.</param>
        public NETGraphics(Graphics graphics)
        {
            _graphics = graphics;


        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 20SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// set the clip region for this graphics
        /// </summary>
        /// <param name="x">the top left x coordinate.</param>
        /// <param name="y">the top left y coordinate.</param>
        /// <param name="width">the width of the clip region.</param>
        /// <param name="height">the height of the clip region.</param>
        public void SetClip(int x, int y, int width, int height)
        {
            _graphics.Clip = (new Region(new Rectangle(x, y, width, height)));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 20SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the image.
        /// </summary>
        /// <param name="img">the image object</param>
        /// <param name="x">the x coordinate where the image is drawn</param>
        /// <param name="y">the y coordinate where the image is drawn</param>
        public void DrawImage(IImage img, int x, int y)
        {
            _graphics.DrawImage(((NETImage)img)._image, x, y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 20SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the image.
        /// </summary>
        /// <param name="img">The img.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="transparent">The transparent.</param>
        public void DrawImage(IImage img, int x, int y, int transparent)
        {
            Color color1 = Color.FromArgb(transparent);
            _imageAttributes.SetColorKey(color1, color1);
            Rectangle dstRect = new Rectangle(x, y, img.GetWidth(), img.GetHeight());
            _graphics.DrawImage(((NETImage)img)._image, dstRect, 0, 0, img.GetWidth(),
                img.GetHeight(),
                GraphicsUnit.Pixel, _imageAttributes);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 20SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the color.
        /// </summary>
        /// <param name="rgb">RGB an RGB color</param>
        public void SetColor(int rgb)
        {
            _color = Color.FromArgb(rgb);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 20SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// fill an rectangle.
        /// </summary>
        /// <param name="x">the top left x coordinate.</param>
        /// <param name="y">the top left y coordinate.</param>
        /// <param name="width">the width of the rectangle.</param>
        /// <param name="height">the height of the rectangle.</param>
        public void FillRect(int x, int y, int width, int height)
        {
            _graphics.FillRectangle(new SolidBrush(_color), x, y, width, height);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 20SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the font.
        /// </summary>
        /// <param name="font">the font object</param>
        public void SetFont(IFont font)
        {

            _font = (Font)font.GetNativeFont();

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 20SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the line.
        /// </summary>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        public void DrawLine(int x1, int y1, int x2, int y2)
        {
            _graphics.DrawLine(new Pen(_color), x1, y1, x2, y2);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 20SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the string.
        /// </summary>
        /// <param name="str">the string to draw</param>
        /// <param name="x">the x coordinate where the string is drawn.</param>
        /// <param name="y">the y coordinate where the string is drawn.</param>
        public void DrawString(String str, int x, int y)
        {
            _graphics.DrawString(str, _font, new SolidBrush(_color), x, y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 20SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// draw an rectangle.
        /// </summary>
        /// <param name="x">the top left x coordinate.</param>
        /// <param name="y">the top left y coordinate.</param>
        /// <param name="width">the width of the rectangle.</param>
        /// <param name="height">the height of the rectangle.</param>
        public void DrawRect(int x, int y, int width, int height)
        {
            _graphics.DrawRectangle(new Pen(_color), x, y, width, height);
        }

    }
}