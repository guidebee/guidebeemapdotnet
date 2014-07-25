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
    /// Map symbol used to display a point.
    /// </summary>
    public sealed class MapSymbol
    {

        /// <summary>
        /// the shape of the symbol.
        /// </summary>
        public int Shape;

        /// <summary>
        /// the color of the symbol.
        /// </summary>
        public int Color;

        /// <summary>
        /// the size of the symbol.
        /// </summary>
        public int Size;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapSymbol"/> class.
        /// </summary>
        public MapSymbol()
        {
            Shape = -1;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapSymbol"/> class.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        public MapSymbol(MapSymbol symbol)
        {
            Shape = symbol.Shape;
            Color = symbol.Color;
            Size = symbol.Size;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapSymbol"/> class.
        /// </summary>
        /// <param name="shape">the shape of the symbol</param>
        /// <param name="color">The color of the symbol.</param>
        /// <param name="size">The size of the symbol.</param>
        public MapSymbol(int shape, int color, int size)
        {
            Shape = shape;
            Color = color;
            Size = size;
        }

    }

}
