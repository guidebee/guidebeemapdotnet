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
using Mapdigit.Gis.Geometry;

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
    /// Objects of this class store information about a single route in a 
    /// directions result. it should not directly create objects of this class. 
    /// </summary>
    public sealed class MapRoute : MapObject
    {

        /// <summary>
        /// Steps included in this route.
        /// </summary>
        public MapStep[] Steps;

        /// <summary>
        /// Start point of this route.
        /// </summary>
        public MapPoint StartGeocode;

        /// <summary>
        /// end point of this route.
        /// </summary>
        public MapPoint EndGeocode;

        /// <summary>
        /// last position of the route.
        /// </summary>
        public GeoLatLng LastLatLng;

        /// <summary>
        /// Summary of this route.
        /// </summary>
        public string Summary;

        /// <summary>
        /// total Distance of the route in meters.
        /// </summary>
        public double Distance;

        /// <summary>
        /// total duration of the route in seconds.
        /// </summary>
        public double Duration;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapRoute"/> class.
        /// </summary>
        internal MapRoute()
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Create a new MapStep object.
        /// </summary>
        /// <returns>a new MapStep object</returns>
        public static MapStep NewStep()
        {
            return new MapStep();
        }

    }

}
