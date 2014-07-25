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
using System.Threading;

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
    /// This is the starting point for applications using location information in 
    /// this API and represents a source of the location information. 
    /// </summary>
    /// <remarks>
    /// A VirtualGPSDevice represents a vitural location-providing module, generating
    /// Locations.
    /// Applications obtain VirtualGPSDevice instances (classes implementing the 
    /// actual functionality by extending this abstract class) by calling the one 
    /// of the factory methods. 
    /// </remarks>
    public abstract class LocationProvider
    {

        /// <summary>
        /// Availability status code: the location device is available. 
        /// </summary>
        public const int Available = 0;

        /// <summary>
        /// Availability status code: the location device is out of service. Being
        /// out of service means that the method is unavailable and the 
        /// implementation is not able to expect that this situation would change
        /// in the near future. An example is when using a location method 
        /// implemented in an external device and the external device is detached. 
        /// </summary>
        public const int OutOfService = 1;

        /// <summary>
        /// Availability status code: the location device is temporarily unavailable.
        /// Temporary unavailability means that the method is unavailable due to 
        /// reasons that can be expected to possibly change in the future and the
        /// provider to become available. An example is not being able to receive 
        /// the signal because the signal used by the location method is currently 
        /// being obstructed, e.g. when deep inside a building for satellite based 
        /// methods. However, a very short transient obstruction of the signal 
        /// should not cause the provider to toggle quickly between 
        /// TemporarilyUnavailable and Available. 
        /// </summary>
        public const int TemporarilyUnavailable = 2;

        /// <summary>
        /// the location listener.
        /// </summary>
        public ILocationListener LocationListener;

        /// <summary>
        /// current device status.
        /// </summary>
        public int CurrentState = OutOfService;

        /// <summary>
        ///  current location.
        /// </summary>
        public Location CurrentLocation = new Location();

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the last known location that the implementation has. This is the
        /// best estimate that the implementation has for the previously known 
        /// location.
        /// </summary>
        /// <remarks>
        /// Applications can use this method to obtain the last known location and 
        /// check the timestamp and other fields to determine if this is recent 
        /// enough and good enough for the application to use without needing to make
        /// a new request for the current location.
        /// </remarks>
        public Location LastKnownLocation
        {
            get { return CurrentLocation; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Retrieves a Location associated with this class. If no result could be 
        /// retrieved, a LocationException is thrown. If the location can not be 
        /// determined within the timeout period specified in the parameter,
        /// the method shall throw a LocationException.
        /// </summary>
        /// <remarks>
        /// If the device is temporarily unavailable, the implementation shall wait 
        /// and try to obtain the location until the timeout expires. If the provider
        /// is out of service, then the LocationException is thrown immediately.
        /// </remarks>
        /// <param name="timeout">a timeout value in seconds, -1 is used to indicate that 
        /// the implementation shall use its default timeout value for this device.
        /// return a Location object </param>
        /// <returns></returns>
        public Location GetLocation(int timeout)
        {
            lock (syncObject)
            {
                getOneFix = true;
                if (timeout != -1)
                {
                    locationTimeout = timeout;
                }
                
            }
            syncObject.WaitOne(1000, false);
            getOneFix = false;
            return CurrentLocation;

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the current state of this LocationProvider. The return value 
        /// shall be one of the availability status code constants defined in 
        /// VirtualGPSDevice class. 
        /// </summary>
        /// <returns>the availability state of this device.</returns>
        public virtual int GetState()
        {
            return CurrentState;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Adds a LocationListener for updates at the defined interval. The listener
        /// will be called with updated location at the defined interval. The 
        /// listener also gets updates when the availablilty state of the 
        /// Device changes.
        /// </summary>
        /// <remarks>
        /// Passing in -1 as the interval selects the default interval which is 
        /// dependent on the used location method. 
        /// Only one listener can be registered with each LocationProvider instance. 
        /// Setting the listener replaces any possibly previously set listener. 
        /// Setting the listener to null cancels the registration of any previously 
        /// set listener.
        /// </remarks>
        /// <param name="listener">The listener to be registered. If set to null the 
        /// registration of any previously set listener is cancelled.</param>
        /// <param name="interval">The interval in seconds. -1 is used for the default 
        /// interval of this provider. </param>
        /// <param name="timeout">Timeout value in seconds, must be greater than 0. 
        /// If the value is -1, the default timeout for this provider is used.</param>
        /// <param name="maxAge"> Maximum age of the returned location in seconds,must 
        /// be greater than 0 or equal to -1 to indicate that the default maximum
        /// age for this provider is used.</param>
        public void SetLocationListener(ILocationListener listener,
                                    int interval,
                                    int timeout,
                                    int maxAge)
        {
            lock (syncObject)
            {
                LocationListener = listener;
                if (interval != -1)
                {
                    locationInterval = interval;
                }
                if (timeout != -1)
                {
                    locationTimeout = timeout;
                }
                if (maxAge != -1)
                {
                    locationMaxAge = maxAge;
                }
            }
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// return last known orientation.
        /// </summary>
        /// <returns>last known orientation.</returns>
        public Orientation GetOrientation()
        {
            return currentOrientation;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// reset the location provider
        /// </summary>
        public abstract void Reset();

        /// <summary>
        /// interval.
        /// </summary>
        protected int locationInterval = 1;

        /// <summary>
        /// time out for location.
        /// </summary>
        protected int locationTimeout = 30;

        /// <summary>
        /// max age for location.
        /// </summary>
        protected int locationMaxAge = 300;

        /// <summary>
        /// current Orientation.
        /// </summary>
        protected Orientation currentOrientation;

        /// <summary>
        /// Get one fix or not.
        /// </summary>
        protected bool getOneFix;

        /// <summary>
        /// sync object.
        /// </summary>
        protected AutoResetEvent syncObject = new AutoResetEvent(false);

        /// <summary>
        /// previous fix time;
        /// </summary>
        protected long previousFixtime;

    }
}
