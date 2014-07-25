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
namespace Mapdigit.Gis
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 27SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Map pen set the pen properties to draw a map object.
    /// </summary>
    public sealed class MapPen
    {

        /// <summary>
        /// the Width of the pen.
        /// </summary>
        public int Width;

        /// <summary>
        /// the pattern of the pen.
        /// </summary>
        public int Pattern;

        /// <summary>
        /// the color of the pen.
        /// </summary>
        public int Color;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapPen"/> class.
        /// </summary>
        public MapPen()
        {
            Pattern = -1;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapPen"/> class.
        /// </summary>
        /// <param name="pen">The pen.</param>
        public MapPen(MapPen pen)
        {
            Width = pen.Width;
            Pattern = pen.Pattern;
            Color = pen.Color;

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapPen"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="color">The color.</param>
        /// <param name="pattern">The pattern.</param>
        public MapPen(int width, int color, int pattern)
        {
            Width = width;
            Pattern = pattern;
            Color = color;

        }

    }

}
