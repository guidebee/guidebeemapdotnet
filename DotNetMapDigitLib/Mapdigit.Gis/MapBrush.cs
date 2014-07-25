//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 15OCT2010  James Shen                 	          Code review
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
    // 15OCT2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Map brush used to paint a map region object.
    /// </summary>
    public class MapBrush
    {

        /// <summary>
        /// the pattern of the brush.
        /// </summary>
        public int Pattern;

        /// <summary>
        /// the fore color of the brush.
        /// </summary>
        public int ForeColor;

        /// <summary>
        /// the backcolor of the brush.
        /// </summary>
        public int BackColor;


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapBrush"/> class.
        /// </summary>
        public MapBrush()
        {
            Pattern = -1;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapBrush"/> class.
        /// </summary>
        /// <param name="brush">the map pen copied from</param>
        public MapBrush(MapBrush brush)
        {
            Pattern = brush.Pattern;
            ForeColor = brush.ForeColor;
            BackColor = brush.BackColor;

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapBrush"/> class.
        /// </summary>
        /// <param name="pattern">the pattern of the brush</param>
        /// <param name="forecolor">the forecolor of the brush</param>
        /// <param name="backcolor">the backcolor of the brush</param>
        public MapBrush(int pattern, int forecolor, int backcolor)
        {
            Pattern = pattern;
            ForeColor = forecolor;
            BackColor = backcolor;
        }

    }

}
