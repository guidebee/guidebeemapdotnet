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
using System;
using Mapdigit.Gis.Location;

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
    /// Navigation listener.
    /// </summary>
    public interface INavigationListener : ILocationListener
    {
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Called by the VirtualGPSDevice to which this listener is registered.
        /// </summary>
        /// <remarks>
        /// This method will be called periodically according to the interval defined
        /// when registering the listener to provide updates of the current location.
        /// </remarks>
        /// <param name="device">The device.</param>
        /// <param name="rawLocation">the raw location to which the event relates, i.e. the
        /// new position direction from Location Provider.</param>
        /// <param name="adjustLocation">the adjust location to which the event relates,it may be
        /// adjusted to put on the map direction.</param>
        void LocationUpdated(LocationProvider device, Location.Location rawLocation,
            Location.Location adjustLocation);


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Navigation is done.
        /// </summary>
        void NavigationDone();


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Navigation status change happend.
        /// </summary>
        /// <param name="oldStatus">The old status.</param>
        /// <param name="newStatus">The new status.</param>
        void StatusChange(int oldStatus, int newStatus);


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///rerouting is done.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="result">The result.</param>
        void ReroutingDone(string query, MapDirection result);


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// rerouing progress progress notification.
        /// </summary>
        /// <param name="bytes">the number of bytes has been read</param>
        /// <param name="total">total bytes to be read.Total will be zero if not available
        /// (content-length header not set)</param>
        void ReroutingProgress(int bytes, int total);
    }
}
