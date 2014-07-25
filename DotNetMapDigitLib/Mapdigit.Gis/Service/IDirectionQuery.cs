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
namespace Mapdigit.Gis.Service
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 27SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// This class is used to query driving directions.
    /// </summary>
    public interface IDirectionQuery
    {

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This method issues a new directions query. The query parameter is
        /// a string containing any valid directions query,
        /// e.g. "from: Seattle to: San Francisco" or
        /// "from: Toronto to: Ottawa to: New York".
        /// </summary>
        /// <param name="waypoints">the directions query string.</param>
        /// <param name="listener">the routing listener.</param>
        void GetDirection(GeoLatLng[] waypoints, IRoutingListener listener);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This method issues a new directions query. The query parameter is
        /// a string containing any valid directions query,
        /// e.g. "from: Seattle to: San Francisco" or
        /// "from: Toronto to: Ottawa to: New York".
        /// or the longitude,latitude list depends on which map server.
        /// </summary>
        /// <param name="query">the directions query string.</param>
        /// <param name="listener">the routing listener.</param>
        void GetDirection(string query, IRoutingListener listener);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This method issues a new directions query. The query parameter is
        /// a string containing any valid directions query,
        /// e.g. "from: Seattle to: San Francisco" or
        /// "from: Toronto to: Ottawa to: New York".
        /// or the longitude,latitude list depends on which map server.
        /// </summary>
        /// <param name="mapType">Type of the map.</param>
        /// <param name="query">the directions query string</param>
        /// <param name="listener">the routing listener.</param>
        void GetDirection(int mapType, string query, IRoutingListener listener);

    }

}
