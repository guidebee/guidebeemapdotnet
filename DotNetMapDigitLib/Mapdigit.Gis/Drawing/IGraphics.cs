//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 27SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Gis.Drawing
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 27SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Graphics  interface.
    /// </summary>
    public interface IGraphics
    {
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// set the clip region for this graphics
        /// </summary>
        /// <param name="x">the top left x coordinate.</param>
        /// <param name="y">the top left y coordinate.</param>
        /// <param name="width">the width of the clip region.</param>
        /// <param name="height">the height of the clip region.</param>
        void SetClip(int x, int y, int width, int height);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the image.
        /// </summary>
        /// <param name="img">the image object</param>
        /// <param name="x">the x coordinate where the image is drawn</param>
        /// <param name="y">the y coordinate where the image is drawn</param>
        void DrawImage(IImage img, int x, int y);


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the image.
        /// </summary>
        /// <param name="img">the image object</param>
        /// <param name="x">the x coordinate where the image is drawn</param>
        /// <param name="y">the y coordinate where the image is drawn</param>
        /// <param name="transparentColor">transparent color</param>
        void DrawImage(IImage img, int x, int y,int transparentColor);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the line.
        /// </summary>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        void DrawLine(int x1, int y1, int x2, int y2);


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the color.
        /// </summary>
        /// <param name="rgb">RGB an RGB color</param>
        void SetColor(int rgb);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// fill an rectangle.
        /// </summary>
        /// <param name="x">the top left x coordinate.</param>
        /// <param name="y">the top left y coordinate.</param>
        /// <param name="width">the width of the rectangle.</param>
        /// <param name="height">the height of the rectangle.</param>
        void FillRect(int x, int y, int width, int height);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  draw an rectangle.
        /// </summary>
        /// <param name="x">the top left x coordinate.</param>
        /// <param name="y">the top left y coordinate.</param>
        /// <param name="width">the width of the rectangle.</param>
        /// <param name="height">the height of the rectangle.</param>
        void DrawRect(int x, int y, int width, int height);


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the string.
        /// </summary>
        /// <param name="str"> the string to draw</param>
        /// <param name="x">the x coordinate where the string is drawn.</param>
        /// <param name="y">the y coordinate where the string is drawn.</param>
        void DrawString(string str, int x, int y);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the font.
        /// </summary>
        /// <param name="font"> the font object</param>
        void SetFont(IFont font);
    }
}