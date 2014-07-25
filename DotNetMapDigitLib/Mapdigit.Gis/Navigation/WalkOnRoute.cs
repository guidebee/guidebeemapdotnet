//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 29SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------


//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Gis.Navigation
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 29SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Current position on the route (nearest).
    /// </summary>
    public class WalkOnRoute
    {

        /// <summary>
        /// navigation route.
        /// </summary>
        public MapDirection MapDirection;

        /// <summary>
        /// index of the route.
        /// </summary>
        public int RouteIndex;

        /// <summary>
        /// index of the step of the route.
        /// </summary>
        public int StepIndex;

        /// <summary>
        /// point index of the polyline.
        /// </summary>
        public int PointIndex;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="WalkOnRoute"/> class.
        /// </summary>
        internal WalkOnRoute()
        {

        }

    }
}
