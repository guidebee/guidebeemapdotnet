//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 28SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Gis.Location
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 28SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// The LocationListener represents a listener that receives events associated 
    /// with a particular VirtualGPSDevice.
    /// </summary>
    /// <remarks>
    ///  Applications implement this interface 
    /// and register it with a VirtualGPSDevice to obtain regular position updates.
    /// When the listener is registered with a VirtualGPSDevice with some update 
    /// period, the implementation shall attempt to provide updates at the defined 
    /// interval. If it isn't possible to determine the location, e.g. because of
    /// the VirtualGPSDevice being TemporarilyUnavailable or OutOfService or 
    /// because the update period is too frequent for the location method to 
    /// provide updates, the implementation can send an update to the listener that 
    /// contains an 'invalid' Location instance.
    /// The implementation shall use best effort to post the location updates at the
    /// specified interval, but this timing is not guaranteed to be very exact 
    /// (i.e. this is not an exact timer facility for an application).
    /// The application is responsible for any possible synchronization needed in 
    /// the listener methods.
    /// The listener methods must return quickly and should not perform any extensive
    /// processing. The method calls are intended as triggers to the application.
    /// Application should do any necessary extensive processing in a separate thread
    /// and only use these methods to initiate the processing.
    /// </remarks>
    public interface ILocationListener
    {
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Called by the VirtualGPSDevice to which this listener is registered. 
        /// </summary>
        /// <remarks>
        /// This method will be called periodically according to the interval defined
        /// when registering the listener to provide updates of the current location. 
        /// </remarks>
        /// <param name="device">The device.</param>
        /// <param name="location">the location to which the event relates, i.e. the 
        /// new position.</param>
        void LocationUpdated(LocationProvider device, Location location);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Called by the VirtualGPSDevice to which this listener is registered if 
        /// the state of the VirtualGPSDevice has changed.
        /// </summary>
        /// <remarks>
        /// These device state changes are delivered to the application as soon as 
        /// possible after the state of a provider changes. The timing of these 
        /// events is not related to the period of the location updates.
        /// If the application is subscribed to receive periodic location updates, 
        /// it will continue to receive these regardless of the state of the 
        /// VirtualGPSDevice. If the application wishes to stop receiving location
        /// updates for an unavailable provider, it should de-register itself from 
        /// the provider.
        /// </remarks>
        /// <param name="device">The device.</param>
        /// <param name="newState">The new state of the VirtualGPSDevice</param>
        void ProviderStateChanged(LocationProvider device, int newState);
    }
}
