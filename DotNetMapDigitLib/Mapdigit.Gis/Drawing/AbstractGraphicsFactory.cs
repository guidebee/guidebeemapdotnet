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
using System.IO;

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
    /// Abstract Graphics Factory class used to create Graphics related classes.
    /// </summary>
    public abstract class AbstractGraphicsFactory
    {
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Create an image instance from rgb array
        /// </summary>
        /// <param name="rgb">the rgb array.</param>
        /// <param name="width">the width of the image.</param>
        /// <param name="height">the height of the image.</param>
        /// <returns>an image instance</returns>
        public abstract IImage CreateImage(int[] rgb, int width, int height);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Create an image instance.
        /// </summary>
        /// <param name="bytes">the byte array</param>
        /// <param name="offset">the start position for the image.</param>
        /// <param name="len">he lenght of the image..</param>
        /// <returns>an image instance</returns>
        public abstract IImage CreateImage(byte[] bytes, int offset, int len);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates the image.
        /// </summary>
        /// <param name="width">the width of the image.</param>
        /// <param name="height"> the height of the image.</param>
        /// <returns>an image instance</returns>
        public abstract IImage CreateImage(int width, int height);
        
    }
}