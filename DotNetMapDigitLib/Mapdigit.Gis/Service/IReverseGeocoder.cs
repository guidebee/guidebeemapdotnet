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
    /// query reverse geo coding.
    /// </summary>
    public interface IReverseGeocoder
    {
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sends a request to map servers to reverse geocode the specified address
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="listener">The listener.</param>
        void GetLocations(GeoLatLng address, IReverseGeocodingListener listener);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sends a request to map servers to reverse geocode the specified address
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="listener">The listener.</param>
        void GetLocations(string address, IReverseGeocodingListener listener);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sends a request to map servers to reverse geocode the specified address
        /// </summary>
        /// <param name="mapType">Type of the map.</param>
        /// <param name="address">The address.</param>
        /// <param name="listener">The listener.</param>
        void GetLocations(int mapType, string address, IReverseGeocodingListener listener);

    }

}
