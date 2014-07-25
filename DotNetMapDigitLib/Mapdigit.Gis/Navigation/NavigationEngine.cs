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
using System.Collections;
using System.Threading;
using Mapdigit.Gis.Geometry;
using Mapdigit.Gis.Location;
using Mapdigit.Gis.Service;
using Mapdigit.Gis.Service.Google;
using Mapdigit.Util;

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
    /// Navigation Engine. 
    /// </summary>
    /// <remarks>
    /// it include 3 threads , 1) location monitor thread used to
    /// monitor current location againest current navigation route if there's one
    /// and ajust raw location to the nearest position on the route. 2) voice command
    /// generate thread create upcoming voice command based on current step and
    /// location and put them to the voice command queue. 3) voice command processor
    /// pick up the voice command from the queue and notify the voice command listener.
    /// </remarks>
    public class NavigationEngine : LocationProvider
    {

        /// <summary>
        /// idle status.
        /// </summary>
        public const int StatusIdle = 0;

        /// <summary>
        /// routing status.
        /// </summary>
        public const int StatusRoutingStart = 1;

        /// <summary>
        /// routing status.
        /// </summary>
        public const int StatusRoutingEnd = 2;

        /// <summary>
        /// navigating status on road mode
        /// </summary>
        public const int StatusNavigatingOnRoadMode = 3;

        /// <summary>
        /// navigating status on road mode preparation (starting point)
        /// </summary>
        public const int StatusNavigatingOnRoadModePrepration = 4;

        //    private const int STATUS_NAVIGATING_ON_ROAD_MODE_NEAR_DESTINATION = 5;

        ///<summary>
        /// navigating status off road mode.
        ///</summary>
        public const int StatusNavigatingOffRoadMode = 6;

        /// <summary>
        /// deviation status.
        /// </summary>
        public const int StatusDeviation = 7;

        /// <summary>
        /// navigation engine paused.
        /// </summary>
        public const int StatusPaused = 10;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationEngine"/> class.
        /// </summary>
        /// <param name="rawLocationProvider">The raw location provider.the raw location provider
        /// refer the GPS devices(either internal or bluetooth etc).</param>
        public NavigationEngine(LocationProvider rawLocationProvider)
            : this(rawLocationProvider, new GoogleMapService())
        {

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationEngine"/> class.
        /// </summary>
        /// <param name="rawLocationProvider">The raw location provider.</param>
        /// <param name="digitalMapService">The digital map service.</param>
        public NavigationEngine(LocationProvider rawLocationProvider,
                DigitalMapService digitalMapService)
        {
            if (rawLocationProvider == null || digitalMapService == null)
            {
                throw new ArgumentException("Argument cannot be null");
            }

            _locationMonitor = new LocationMonitor(this);

            _voiceCommandGenerator
                   = new VoiceCommandGenerator(this);

         
            _voiceCommandProcessor
                   = new VoiceCommandProcessor(this);
            _rawLocationProvider = rawLocationProvider;
            _digitalMapService = digitalMapService;
            _currentLocationProvider = rawLocationProvider;
            _pauseThread = true;
            _stopThread = true;
            //location monitor intercepts location update from location provider
            _currentLocationProvider.SetLocationListener(_locationMonitor, 1, -1, -1);

        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Cancels the navigation.
        /// </summary>
        public void CancelNavigation()
        {
            lock (syncObject)
            {
                if (_engineStatus == StatusNavigatingOnRoadMode ||
                     _engineStatus == StatusNavigatingOnRoadModePrepration)
                {
                    _wayPoints.Clear();
                    _mapDirection = null;
                    _engineStatus = StatusIdle;
                    Pause();
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Starts the navigation.
        /// </summary>
        /// <param name="mapDirection">the map direction used to navigation,it can be null,
        /// if it's null,navigation engine will try to use digital map service to
        /// find the diection from current location to all it's first way point.
        /// the last wayPoint is considered as the destination.</param>
        /// <param name="wayPointArray">The way point array.</param>
        public void StartNavigation(MapDirection mapDirection,
                WayPoint[] wayPointArray)
        {
            lock (syncObject)
            {
                if (wayPointArray == null)
                {
                    throw new ArgumentException("wayPointArray cannot be null");
                }
                //cancel previous navigation.
                if (_mapDirection != null)
                {
                    CancelNavigation();
                    Resume();
                }
                _wayPoints.Clear();
                for (int i = 0; i < wayPointArray.Length; i++)
                {
                    _wayPoints.Add(wayPointArray[i]);
                }
                _mapDirection = mapDirection;
                _currentWalkOnRoute.MapDirection = mapDirection;
                _currentWalkOnRoute.PointIndex = -1;
                _currentWalkOnRoute.RouteIndex = -1;
                _currentWalkOnRoute.StepIndex = -1;
                try
                {
                    Location.Location location = _currentLocationProvider.GetLocation(10);
                    _currentMonitorLocation.Copy(location);
                    _currentLatLng.X = _currentMonitorLocation.Longitude;
                    _currentLatLng.Y = _currentMonitorLocation.Latitude;
                    _previousLatLng.X = _currentLatLng.X;
                    _previousLatLng.Y = _currentLatLng.Y;
                    _locationMonitor.Initialize();

                }
                catch (Exception)
                {
                }
                _locationMonitor.FlushCommandQueue();
                _voiceCommandGenerator._nearestIndex = -1;
                if (mapDirection == null)
                {
                    //if the map direction is null and allow rerouting
                    //then go to routing start status
                    //otherwise in off road navigation mode.
                    if (_allowRerouting)
                    {
                        _firstRouting = true;
                        _engineStatus = StatusRoutingStart;
                    }
                    else
                    {
                        _engineStatus = StatusNavigatingOffRoadMode;
                        int lastIndex = wayPointArray.Length;
                        _destinationLatLng.X = wayPointArray[lastIndex - 1].GetLatLng().X;
                        _destinationLatLng.Y = wayPointArray[lastIndex - 1].GetLatLng().Y;
                    }
                }
                else
                {
                    _firstRouting = false;
                    _engineStatus = StatusNavigatingOnRoadModePrepration;
                    _destinationLatLng.X = mapDirection.Polyline
                            .GetVertex(mapDirection.Polyline.GetVertexCount() - 1).X;
                    _destinationLatLng.Y = mapDirection.Polyline
                            .GetVertex(mapDirection.Polyline.GetVertexCount() - 1).Y;
                }
                if (_navigationListener != null)
                {
                    _navigationListener.StatusChange(StatusPaused, _engineStatus);
                }

            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the navigation listener.
        /// </summary>
        /// <param name="listener">The listener.</param>
        public void SetNavigationListener(INavigationListener listener)
        {
            SetNavigationListener(listener, 1, -1, -1);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the voice command listener.
        /// </summary>
        /// <param name="listener">The listener.</param>
        public void SetVoiceCommandListener(IVoiceCommandListener listener)
        {
            _voiceCommandListener = listener;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Adds a LocationListener for updates at the defined interval. The listener
        /// will be called with updated location at the defined interval. The
        /// listener also gets updates when the availablilty state of the
        /// Device changes.
        /// Passing in -1 as the interval selects the default interval which is
        /// dependent on the used location method.
        /// Only one listener can be registered with each LocationProvider instance.
        /// Setting the listener replaces any possibly previously set listener.
        /// Setting the listener to null cancels the registration of any previously
        /// set listener.
        /// </summary>
        /// <param name="listener"> The listener to be registered. If set to null the
        /// registration of any previously set listener is cancelled.</param>
        /// <param name="interval">The interval in seconds. -1 is used for the default
        ///  interval of this provider.</param>
        /// <param name="timeout">Timeout value in seconds, must be greater than 0.
        /// If the value is -1, the default timeout for this provider is used.</param>
        /// <param name="maxAge">Maximum age of the returned location in seconds,must
        /// be greater than 0 or equal to -1 to indicate that the default maximum
        /// age for this provider is used.</param>
        public void SetNavigationListener(INavigationListener listener,
                                    int interval,
                                    int timeout,
                                    int maxAge)
        {

            _navigationListener = listener;
            SetLocationListener(_navigationListener, interval, timeout, maxAge);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Starts the simulation.
        /// </summary>
        /// <param name="mapDirectionSimu">The map direction simu.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="isMile">is mile or not</param>
        public void StartSimulation(MapDirection mapDirectionSimu, int speed,
                bool isMile)
        {
            lock (syncObject)
            {
                //cancel previous navigation.
                if (_mapDirection != null)
                {
                    CancelNavigation();
                    Resume();
                }
                _currentLocationProvider.SetLocationListener(null, 1, 1, 1);
                _currentLocationProvider = _simulatedDirectionLocationProvider;
                _simulatedDirectionLocationProvider.StopSimulation();
                _currentLocationProvider
                        .SetLocationListener(_locationMonitor, 1, -1, -1);
                _simulatedDirectionLocationProvider
                        .StartSimulation(mapDirectionSimu, speed, isMile);

            }
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Pauses the simulation.
        /// </summary>
        public void PauseSimulation()
        {
            lock (syncObject)
            {
                if (_currentLocationProvider == _simulatedDirectionLocationProvider)
                {
                    _simulatedDirectionLocationProvider.PauseSimulation();
                }
            }

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Resumes the simulation.
        /// </summary>
        public void ResumeSimulation()
        {
            lock (syncObject)
            {
                if (_currentLocationProvider == _simulatedDirectionLocationProvider)
                {
                    _simulatedDirectionLocationProvider.ResumeSimulation();
                }
            }

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Stops the simulation.
        /// </summary>
        public void StopSimulation()
        {
            lock (syncObject)
            {
                if (_currentLocationProvider == _simulatedDirectionLocationProvider)
                {
                    _currentLocationProvider.SetLocationListener(null, 1, 1, 1);
                    _simulatedDirectionLocationProvider.StopSimulation();
                    _currentLocationProvider = _rawLocationProvider;
                    _currentLocationProvider.SetLocationListener(_locationMonitor,
                            1, -1, -1);
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the auto rerouting.
        /// </summary>
        /// <param name="allow">true, allow rerouting</param>
        public void SetAutoRerouting(bool allow)
        {
            _allowRerouting = allow;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set the distance unit. either use kilometer/meter or mile/yard
        /// </summary>
        /// <param name="useKilometer">true, uses kilometer otherwise uses miles</param>
        public void SetDistanceUnit(bool useKilometer)
        {
            _useKilometer = useKilometer;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// set if the navigation engines give more or less voice command .
        /// default is false, then for each turn ,maximum 4 voice commands.
        /// </summary>
        /// <param name="moreVoiceCommand">true, navigation will give more voice command</param>
        public void SetMoreVoiceCommand(bool moreVoiceCommand)
        {
            _moreVoiceCommand = moreVoiceCommand;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            lock (syncObject)
            {
                if (_stopThread)
                {
                    _stopThread = false;
                    _pauseThread = false;
                    _locationMonitorThread = new Thread(_locationMonitor.Run) { Name = "locationMonitor" };
                    _locationMonitorThread.Start();
                    _voiceCommandGeneratorThread = new Thread(_voiceCommandGenerator.Run);
                    _locationMonitorThread.Name = "voiceCommandGenerator";
                    _voiceCommandGeneratorThread.Start();
                    _voiceCommandProcessorThread = new Thread(_voiceCommandProcessor.Run) { Name = "voiceCommandProcessor" };
                    _voiceCommandProcessorThread.Start();
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the current state of this LocationProvider. The return value
        /// shall be one of the availability status code constants defined in
        /// VirtualGPSDevice class.
        /// </summary>
        /// <returns>the availability state of this device.</returns>
        public override int GetState()
        {
            return _currentLocationProvider.GetState();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the current state of this LocationProvider. The return value
        /// shall be one of the availability status code constants defined in
        /// VirtualGPSDevice class.
        /// </summary>
        /// <returns>the availability state of this device</returns>
        public int GetStatus()
        {
            return _engineStatus;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the status string.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public static string GetStatusString(int status)
        {
            string statusString;
            switch (status)
            {
                case StatusIdle:
                    statusString = "STATUS_IDLE";
                    break;
                case StatusRoutingStart:
                    statusString = "STATUS_ROUTING_START";
                    break;
                case StatusRoutingEnd:
                    statusString = "STATUS_ROUTING_END";
                    break;
                case StatusNavigatingOnRoadMode:
                    statusString = "STATUS_NAVIGATING_ON_ROAD_MODE";
                    break;
                case StatusNavigatingOnRoadModePrepration:
                    statusString = "STATUS_NAVIGATING_ON_ROAD_MODE_PREPRATION";
                    break;
                case StatusNavigatingOffRoadMode:
                    statusString = "STATUS_NAVIGATING_OFF_ROAD_MODE";
                    break;
                case StatusDeviation:
                    statusString = "STATUS_DEVIATION";
                    break;
                case StatusPaused:
                    statusString = "STATUS_PAUSED";
                    break;
                default:
                    statusString = "STATUS_IDLE";
                    break;
            }
            return statusString;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Pauses this instance.
        /// </summary>
        public void Pause()
        {
            lock (syncObject)
            {
                if (!_pauseThread)
                {
                    _pauseThread = true;
                    _pauseStatus = _engineStatus;
                    _engineStatus = StatusPaused;
                    if (_navigationListener != null)
                    {
                        _navigationListener.StatusChange(_pauseStatus, _engineStatus);
                    }
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Resumes this instance.
        /// </summary>
        public void Resume()
        {
            lock (syncObject)
            {
                if (_pauseThread)
                {
                    _pauseThread = false;
                    _pauseObject.Set();
                    _engineStatus = _pauseStatus;
                    if (_navigationListener != null)
                    {
                        _navigationListener.StatusChange(StatusPaused, _engineStatus);
                    }
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            lock (syncObject)
            {
                if (_stopThread == false)
                {
                    _stopThread = true;
                    try
                    {
                        //stop location monitor
                        _pauseObject.Set();
                        _currentMonitorLocationMutext.Set();
                        //the routingobject is used to make the get
                        //direction call a lock one.
                        _locationMonitor._routingObject.Set();
                        _voiceCommandQueueMutex.Set();
                        _voiceCommandGenerator._generatorObject.Set();
                        _voiceCommandProcessor._currentLocationMutex.Set();

                        _locationMonitorThread = null;
                        _voiceCommandGeneratorThread = null;
                        _voiceCommandProcessorThread = null;
                    }
                    catch (Exception)
                    {

                    }
                }
                _simulatedDirectionLocationProvider.StopSimulation();
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the offline mode current target, if not found or in
        /// other status,return null;
        /// </summary>
        /// <returns></returns>
        public GeoLatLng GetOffRoadNavigationTarget()
        {
            lock (syncObject)
            {
                if (_engineStatus != StatusNavigatingOffRoadMode)
                {
                    return null;
                }
                if (_mapDirection != null)
                {
                    _locationMonitor.ResetCurrentPointIndexBasedOnCurrentLocation();
                    return _mapDirection.Polyline.GetVertex(_locationMonitor._currentPointIndex);

                }
                return new GeoLatLng(_destinationLatLng);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the current walk on route.
        /// </summary>
        /// <returns>current walk on route info</returns>
        public WalkOnRoute GetCurrentWalkOnRoute()
        {
            lock (syncObject)
            {
                if (_mapDirection == null)
                {
                    return null;
                }
                GeoPoint routeStepIndex = _mapDirection
                        .GetMapRouteStepIndexByPointIndex(_locationMonitor
                        ._currentPointIndex);
                int routeIndex = (int)routeStepIndex.X;
                int stepIndex = (int)routeStepIndex.Y;
                WalkOnRoute walkOnRoute = new WalkOnRoute
                                              {
                                                  MapDirection = _mapDirection,
                                                  RouteIndex = routeIndex,
                                                  StepIndex = stepIndex,
                                                  PointIndex = _locationMonitor._currentPointIndex
                                              };
                return walkOnRoute;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// reset the location provider
        /// </summary>
        public override void Reset()
        {
            if (_rawLocationProvider != null) _rawLocationProvider.Reset();
            _simulatedDirectionLocationProvider.Reset();
        }


        /**
         * the distance limit of command is 50 meters.
         */
        private const int CommandDistanceLimit = 100;
        /**
         * if current speed exceets 60kph, it's considered as highway.
         */
        private const int HighwaySpeedLimit = 60;


        /**
         * all distances used for voice command.
         */
        private static readonly int[] VoiceDistances = new[]{
            VoiceCommandType.Distance050,                      //0
            VoiceCommandType.Distance100,                      //1
            VoiceCommandType.Distance150,                      //2
            VoiceCommandType.Distance200,                      //3
            VoiceCommandType.Distance250,                      //4
            VoiceCommandType.Distance300,                      //5
            VoiceCommandType.Distance400,                      //6
            VoiceCommandType.Distance500,                      //7
            VoiceCommandType.Distance600,                      //8
            VoiceCommandType.Distance700,                      //9
            VoiceCommandType.Distance800,                      //10
            VoiceCommandType.Distance900,                      //11
            VoiceCommandType.Distance1000,                      //12
            VoiceCommandType.Distance1100,                      //13
            VoiceCommandType.Distance1200,                      //14
            VoiceCommandType.Distance1300,                      //15
            VoiceCommandType.Distance1400,                      //16
            VoiceCommandType.Distance1500,                      //17
            VoiceCommandType.Distance1600,                      //18
            VoiceCommandType.Distance1700,                      //19
            VoiceCommandType.Distance1800,                      //20
            VoiceCommandType.Distance1900,                      //21
            VoiceCommandType.Distance002K,                      //22
            VoiceCommandType.Distance003K,                      //23
            VoiceCommandType.Distance004K,                      //24
            VoiceCommandType.Distance005K,                      //25
            VoiceCommandType.Distance006K,                      //26
            VoiceCommandType.Distance005K,                      //27
            VoiceCommandType.Distance008K,                      //28
            VoiceCommandType.Distance009K,                      //29
            VoiceCommandType.Distance010K,                      //30
            VoiceCommandType.Distance015K,                      //31
            VoiceCommandType.Distance020K,                      //32
            VoiceCommandType.Distance025K,                      //33
            VoiceCommandType.Distance030K,                      //34
            VoiceCommandType.Distance035K,                      //35
            VoiceCommandType.Distance040K,                      //36
            VoiceCommandType.Distance045K,                      //37
            VoiceCommandType.Distance050K,                      //38
            VoiceCommandType.Distance055K,                      //39
            VoiceCommandType.Distance060K,                      //40
            VoiceCommandType.Distance065K,                      //41
            VoiceCommandType.Distance070K,                      //42
            VoiceCommandType.Distance075K,                      //43
            VoiceCommandType.Distance080K,                      //44
            VoiceCommandType.Distance085K,                      //45
            VoiceCommandType.Distance090K,                      //46
            VoiceCommandType.Distance095K,                      //47
            VoiceCommandType.Distance100K,                      //48
    };


        /**
         * all distances used for voice command which is Optional.
         */
        private static readonly int[] OptionalVoiceDistances = new[]{
            VoiceCommandType.Distance1100,                      //13
            VoiceCommandType.Distance1200,                      //14
            VoiceCommandType.Distance1300,                      //15
            VoiceCommandType.Distance1400,                      //16
            VoiceCommandType.Distance1500,                      //17
            VoiceCommandType.Distance1600,                      //18
            VoiceCommandType.Distance1700,                      //19
            VoiceCommandType.Distance1800,                      //20
            VoiceCommandType.Distance1900,                      //21
            VoiceCommandType.Distance015K,                      //31
            VoiceCommandType.Distance020K,                      //32
            VoiceCommandType.Distance025K,                      //33
            VoiceCommandType.Distance030K,                      //34
            VoiceCommandType.Distance035K,                      //35
            VoiceCommandType.Distance040K,                      //36
            VoiceCommandType.Distance045K,                      //37
            VoiceCommandType.Distance050K,                      //38
            VoiceCommandType.Distance055K,                      //39
            VoiceCommandType.Distance060K,                      //40
            VoiceCommandType.Distance065K,                      //41
            VoiceCommandType.Distance070K,                      //42
            VoiceCommandType.Distance075K,                      //43
            VoiceCommandType.Distance080K,                      //44
            VoiceCommandType.Distance085K,                      //45
            VoiceCommandType.Distance090K,                      //46
            VoiceCommandType.Distance095K,                      //47
            VoiceCommandType.Distance100K,                      //48
    };




        /**
         * all way points.
         */
        private readonly ArrayList _wayPoints = new ArrayList();

        /**
         * if it's the first routing, will use all way points, otherwize only
         * use the last way point as destination.
         */
        private volatile bool _firstRouting = true;

        /**
         * raw location provider.
         */
        private readonly LocationProvider _rawLocationProvider;
        /**
         * current location provider.either be the raw or the simulated.
         */
        private LocationProvider _currentLocationProvider;
        /**
         * Digital map service,used to routing.
         */
        private readonly DigitalMapService _digitalMapService;
        /**
         * current navigation engine status;
         */
        private volatile int _engineStatus = StatusIdle;

        /**
         * current walk on route.
         */
        private readonly WalkOnRoute _currentWalkOnRoute = new WalkOnRoute();

        /**
         * record engine status when paused /resumed.
         */
        private volatile int _pauseStatus = StatusIdle;

        /**
         * current map direction.
         */
        private MapDirection _mapDirection;

        /**
         * simulated location provider.
         */
        private readonly SimulatedDirectionLocationProvider
                _simulatedDirectionLocationProvider
                = new SimulatedDirectionLocationProvider();

        /**
         * navigation listener
         */
        private INavigationListener _navigationListener;

        /**
         * start time deviation Limit ,default is 100 meter.
         */
        private const int StartPointDeviationLimit = 100;

        /**
         * deviation limit in meter ,default is 20 meters.
         */
        private const int DeviationLimit = 35;

        /**
         * deviation limit counter max time, default is 3 times.
         */
        private const int DeviationMaxTimes = 2;

        /**
         * location monitor.
         */
        private readonly LocationMonitor _locationMonitor;

        /**
         * voice command generator.
         */
        private readonly VoiceCommandGenerator _voiceCommandGenerator;


        /**
         * voice command processor.
         */
        private readonly VoiceCommandProcessor _voiceCommandProcessor;



        /**
         * offline navigation voice command interval.
         */
        private const int OfflineNavigationVoiceCommandInterval = 15;
        /**
         * allow rerouting or not,default is true.
         */
        private volatile bool _allowRerouting = true;

        /**
         * default distance unit is km or mile
         */
        private bool _useKilometer = true;

        /**
         * generate more voice command or not.
         */
        private bool _moreVoiceCommand;

        /**
         * voice command queue
         */
        private readonly ArrayList _voiceCommandQueue = new ArrayList();
        private readonly AutoResetEvent _voiceCommandQueueMutex = new AutoResetEvent(false);

        /**
         * minium count of the command in the queue.
         */
        private const int VoiceCommandQueueSize = 8;

        /**
         * voice command listener.
         */
        private IVoiceCommandListener _voiceCommandListener;

        /**
         * stop the thead or not.
         */
        private volatile bool _stopThread = true;

        /**
         * use to control the pause/resume the thread.
         */
        private volatile bool _pauseThread = true;

        /**
         * object use to signal pause/resume.
         */
        private readonly AutoResetEvent _pauseObject = new AutoResetEvent(false);

        /**
         * location monitor thread.
         */
        private Thread _locationMonitorThread;

        /**
         * voice command generator thread
         */
        private Thread _voiceCommandGeneratorThread;

        /**
         * voice command processor thread
         */
        private Thread _voiceCommandProcessorThread;

        /**
         * current monitor lat/longitude.
         */
        private readonly GeoLatLng _currentLatLng = new GeoLatLng();

        /**
         * current location.
         */
        private readonly Location.Location _currentMonitorLocation = new Location.Location();
        private readonly AutoResetEvent _currentMonitorLocationMutext = new AutoResetEvent(false);
        /**
         * previous lat/longitude,temp used to avoid going backwards when adjust
         * the location on route
         */
        private readonly GeoLatLng _previousLatLng = new GeoLatLng();

        /**
         * destination
         */
        private readonly GeoLatLng _destinationLatLng = new GeoLatLng();

        private static readonly DateTime StartTime = new DateTime(1971, 1, 1);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// check to see if need pause current thread.
        /// </summary>
        private void CheckIfNeedPauseThread()
        {
            //check if this thread need to be paused.
            if (_pauseThread)
            {
                Log.P(Thread.CurrentThread.Name + " thread paused");
                _pauseObject.WaitOne();

                Log.P(Thread.CurrentThread.Name + " thread resumed");
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// check to see given distance command is optional or not.
        /// </summary>
        /// <param name="type">distance command type</param>
        /// <returns>
        /// 	true is optional
        /// </returns>
        private static bool IsOptionalDistanceCommand(int type)
        {
            for (int i = 0; i < OptionalVoiceDistances.Length; i++)
            {
                if (OptionalVoiceDistances[i] == type)
                {
                    return true;
                }
            }
            return false;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Location monitor class, used to monitor current location againest
        ///  current routing direction.
        /// </summary>
        private class LocationMonitor : IRoutingListener,
                ILocationListener
        {
            private readonly NavigationEngine _navigationEngine;

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 29SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Initializes a new instance of the <see cref="LocationMonitor"/> class.
            /// </summary>
            /// <param name="navigationEngine">The navigation engine.</param>
            public LocationMonitor(NavigationEngine navigationEngine)
            {
                _navigationEngine = navigationEngine;
            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 29SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// state mechine, used to monitor current location.
            /// </summary>
            public void Run()
            {
                Log.P(Thread.CurrentThread.Name + " thread started");
                while (!_navigationEngine._stopThread)
                {
                    try
                    {

                        _navigationEngine.CheckIfNeedPauseThread();
                        //state machine ,transition.

                        switch (_navigationEngine._engineStatus)
                        {
                            case StatusIdle:
                                //Log.P("STATUS_IDLE");
                                break;
                            case StatusPaused:
                                //Log.P("STATUS_PAUSED");
                                break;
                            case StatusRoutingStart:
                                //Log.P("STATUS_ROUTING_START");
                                {

                                    _navigationEngine._currentMonitorLocation
                                            .Copy(_navigationEngine._currentLocationProvider
                                            .GetLocation(10));
                                    if (_navigationEngine._currentMonitorLocation != null &&
                                            !Double.IsNaN(_navigationEngine._currentMonitorLocation.Longitude)
                                            && !Double.IsNaN(_navigationEngine._currentMonitorLocation.Latitude))
                                    {
                                        GeoLatLng estimatedStartPoint
                                                = GetEstimatedLocation
                                                (EstimatedRoutingTime);
                                        if (!Double.IsNaN(estimatedStartPoint.X) &&
                                                !Double.IsNaN(estimatedStartPoint.Y))
                                        {
                                            string queryString = "from:" + "@"
                                                    + estimatedStartPoint.Latitude
                                                    + "," + estimatedStartPoint.Longitude;
                                            if (_navigationEngine._firstRouting)
                                            {
                                                _navigationEngine._firstRouting = false;
                                                for (int i = 0; i < _navigationEngine._wayPoints.Count; i++)
                                                {
                                                    WayPoint wayPoint = (WayPoint)
                                                        _navigationEngine._wayPoints[i];
                                                    queryString += " to:" + "@"
                                                        + wayPoint.GetLatLng().Latitude
                                                        + "," + wayPoint.GetLatLng().Longitude;
                                                }
                                            }
                                            else
                                            {
                                                WayPoint wayPoint = (WayPoint)
                                                        _navigationEngine._wayPoints[_navigationEngine._wayPoints.Count - 1];
                                                queryString += " to:" + "@"
                                                        + wayPoint.GetLatLng().Latitude
                                                        + "," + wayPoint.GetLatLng().Longitude;
                                            }
                                            Log.P("Rerouting started:" + queryString);
                                            _navigationEngine._digitalMapService.SetRoutingListener(this);
                                            _navigationEngine._digitalMapService.GetDirections(queryString);
                                            _routingObject.WaitOne();
                                            if (_navigationEngine._navigationListener != null)
                                            {
                                                _navigationEngine._navigationListener
                                                        .ReroutingDone(queryString,
                                                        _navigationEngine._mapDirection);
                                            }
                                            if (_navigationEngine._mapDirection != null)
                                            {
                                                Log.P("Start:" + _navigationEngine._mapDirection.GeoCodes[0].Name);
                                                for (int i = 1; i < _navigationEngine._mapDirection.GeoCodes.Length - 1; i++)
                                                {
                                                    Log.P("Waypoint:" + _navigationEngine._mapDirection.GeoCodes[i].Name);
                                                }
                                                Log.P("End:" + _navigationEngine._mapDirection
                                                        .GeoCodes[_navigationEngine._mapDirection.GeoCodes.Length - 1].Name);
                                                _counter = 0;
                                                _navigationEngine._engineStatus = StatusRoutingEnd;
                                                if (_navigationEngine._navigationListener != null)
                                                {
                                                    _navigationEngine._navigationListener.StatusChange(StatusRoutingStart,
                                                            _navigationEngine._engineStatus);
                                                }
                                                continue;
                                            }

                                        }
                                        else
                                        {
                                            Log.P("Cannot routing with given way points");
                                        }
                                    }
                                    else
                                    {
                                        Log.P("Cannot get a fix,Sleep 5 seconds and then try");
                                        Thread.Sleep(5000);
                                    }
                                }
                                break;
                            case StatusRoutingEnd:
                                //Log.P("STATUS_ROUTING_END");
                                {

                                    if (_counter > DeviationMaxTimes)
                                    {
                                        //still can not make current location
                                        //on the route, need to reroute.
                                        _counter = 0;
                                        _navigationEngine._engineStatus = StatusRoutingStart;
                                        if (_navigationEngine._navigationListener != null)
                                        {
                                            _navigationEngine._navigationListener.StatusChange
                                                    (StatusRoutingEnd,
                                                    _navigationEngine._engineStatus);
                                        }
                                        continue;
                                    }
                                    long currentTimeStamp = (long)(DateTime.Now - StartTime).TotalMilliseconds;
                                    ResetCurrentPointIndexBasedOnCurrentLocation();
                                    bool onTrack = IsCurrentPointIndexOnTrack();
                                    if (onTrack)
                                    {
                                        _counter = 0;
                                        Initialize();
                                        FlushCommandQueue();
                                        _navigationEngine._engineStatus = StatusNavigatingOnRoadMode;
                                        if (_navigationEngine._navigationListener != null)
                                        {
                                            _navigationEngine._navigationListener.StatusChange
                                                    (StatusRoutingEnd,
                                                    _navigationEngine._engineStatus);
                                        }
                                        continue;
                                    }
                                    if (!onTrack &&
                                            (currentTimeStamp - _lastTimeStamp >
                                            EstimatedRoutingTime * 1000))
                                    {
                                        _counter++;
                                        Log.P("regain route failed:" + _counter);
                                        _lastTimeStamp = currentTimeStamp;
                                    }
                                }
                                break;

                            case StatusNavigatingOnRoadModePrepration:
                                //Log.P("STATUS_NAVIGATING_ON_ROAD_MODE_PREPRATION");
                                {
                                    GeoLatLng firstLatlng = _navigationEngine._mapDirection.Polyline.GetVertex(0);
                                    _navigationEngine._currentLatLng.Y = _navigationEngine._currentMonitorLocation.Latitude;
                                    _navigationEngine._currentLatLng.X = _navigationEngine._currentMonitorLocation.Longitude;
                                    double distance = GeoLatLng.Distance(_navigationEngine._currentLatLng,
                                            firstLatlng) * 1000;
                                    if (distance < DeviationLimit)
                                    {

                                        _counter = 0;
                                        Initialize();
                                        _navigationEngine._engineStatus = StatusNavigatingOnRoadMode;
                                        if (_navigationEngine._navigationListener != null)
                                        {
                                            _navigationEngine._navigationListener
                                                    .StatusChange(StatusNavigatingOnRoadModePrepration,
                                                    _navigationEngine._engineStatus);
                                        }
                                        continue;
                                    }
                                    if (distance > StartPointDeviationLimit)
                                    {
                                        _counter = 0;
                                        _navigationEngine._engineStatus = StatusDeviation;
                                        if (_navigationEngine._navigationListener != null)
                                        {
                                            _navigationEngine._navigationListener
                                                    .StatusChange(StatusNavigatingOnRoadMode,
                                                    _navigationEngine._engineStatus);
                                        }
                                        continue;

                                    }
                                }
                                break;

                            case StatusNavigatingOnRoadMode:
                                //Log.P("STATUS_NAVIGATING_ON_ROAD_MODE");
                                {
                                    //save the raw location before align
                                    //raw location to the map direction's polyline
                                    _rawLocation.Copy(_navigationEngine._currentMonitorLocation);
                                    if (_counter > DeviationMaxTimes - 1)
                                    {
                                        //get the nearest location from the route,
                                        //if still within the limit, change current point index
                                        //and keep navgtion, otherwise ,change to deviation
                                        //status
                                        ResetCurrentPointIndexBasedOnCurrentLocation();
                                        if (IsCurrentPointIndexOnTrack())
                                        {
                                            FlushCommandQueue();
                                            Log.P("Found nearest position, continue navgating");
                                            _counter = 0;
                                        }
                                        else
                                        {
                                            _counter = 0;
                                            _navigationEngine._engineStatus = StatusDeviation;
                                            if (_navigationEngine._navigationListener != null)
                                            {
                                                _navigationEngine._navigationListener.StatusChange
                                                        (StatusNavigatingOnRoadMode,
                                                        _navigationEngine._engineStatus);
                                            }
                                            continue;
                                        }

                                    }
                                    //now to try to find the nearest point on the
                                    //map direction's polyline.
                                    int polylineLength = _navigationEngine._mapDirection.Polyline.GetVertexCount();
                                    _navigationEngine._currentLatLng.Y = _navigationEngine._currentMonitorLocation.Latitude;
                                    _navigationEngine._currentLatLng.X = _navigationEngine._currentMonitorLocation.Longitude;
                                    int howManyPoints = Math.Min(36,
                                            polylineLength - _currentPointIndex);
                                    _tempVector.Clear();
                                    int i = 0;
                                    GeoLatLng oldTempLatLng = new GeoLatLng();
                                    while (_tempVector.Count < howManyPoints
                                            && (_currentPointIndex + i < polylineLength))
                                    {
                                        GeoLatLng tempLatLng = _navigationEngine._mapDirection
                                                    .Polyline
                                                    .GetVertex(_currentPointIndex + i++);
                                        if (tempLatLng.X != oldTempLatLng.X &&
                                               tempLatLng.Y != oldTempLatLng.Y)
                                        {
                                            _tempVector.Add(tempLatLng);
                                            oldTempLatLng.X = tempLatLng.X;
                                            oldTempLatLng.Y = tempLatLng.Y;
                                        }
                                    }
                                    howManyPoints = _tempVector.Count;
                                    if (howManyPoints > 0)
                                    {
                                        double[] xpts = new double[howManyPoints];
                                        double[] ypts = new double[howManyPoints];
                                        for (i = 0; i < howManyPoints; i++)
                                        {
                                            GeoLatLng tempLatLng = (GeoLatLng)_tempVector[i];
                                            xpts[i] = tempLatLng.X;
                                            ypts[i] = tempLatLng.Y;
                                        }
                                        GeoPoint result = GeoPolyline
                                                .IndexOfClosestdistanceToPoly
                                                (_navigationEngine._currentLatLng.X,
                                                _navigationEngine._currentLatLng.Y, xpts, ypts, false);
                                        _currentPointIndex = Math.Max(_oldPointIndex,
                                                (int)result.Y
                                                + _currentPointIndex);
                                        _oldPointIndex = _currentPointIndex;
                                    }
                                    if (_currentPointIndex >= 0 &&
                                              _currentPointIndex < polylineLength - 1)
                                    {
                                        bool onTrack = IsCurrentPointIndexOnTrack();
                                        long currentTimeStamp =
                                            (long)(DateTime.Now - StartTime).TotalMilliseconds;
                                        if (!onTrack
                                                && (currentTimeStamp - _lastTimeStamp >
                                                EstimatedRoutingTime * 1000))
                                        {//need rerouting
                                            _counter++;
                                            Log.P("deviation detected:" + _counter);
                                            _lastTimeStamp = currentTimeStamp;

                                        }
                                        if (onTrack)
                                        {
                                            //reset counter and ajust current location to the route.
                                            if (_counter > 0)
                                            {
                                                Log.P("deviation amended");
                                            }
                                            _counter = 0;
                                            _lastTimeStamp = currentTimeStamp;

                                        }
                                        //adjust location ot avoid going backwards
                                        if (_navigationEngine._previousLatLng.X == 0 ||
                                                _navigationEngine._previousLatLng.Y == 0)
                                        {

                                            _navigationEngine._previousLatLng.Y =
                                                    _navigationEngine._currentMonitorLocation.Latitude;
                                            _navigationEngine._previousLatLng.X =
                                                    _navigationEngine._currentMonitorLocation.Longitude;

                                        }
                                        if (_currentPointIndex == 0 && onTrack)
                                        {
                                            GeoLatLng pt1 = _navigationEngine._mapDirection.Polyline
                                                   .GetVertex(_currentPointIndex);
                                            _navigationEngine._currentMonitorLocation.Latitude = pt1.Latitude;
                                            _navigationEngine._currentMonitorLocation.Longitude = pt1.Longitude;
                                        }

                                    }

                                    AlignCurrentLocationOnTheRoad();
                                    if (_navigationEngine.LocationListener != null)
                                    {
                                        _navigationEngine.LocationListener
                                                .LocationUpdated(_navigationEngine._currentLocationProvider,
                                                _navigationEngine._currentMonitorLocation);
                                    }
                                    if (_navigationEngine._navigationListener != null)
                                    {
                                        _navigationEngine._navigationListener
                                                .LocationUpdated(_navigationEngine._currentLocationProvider,
                                                _rawLocation, _navigationEngine._currentMonitorLocation);
                                    }
                                    NotifyVoiceCommandProcessor();
                                }
                                break;

                            case StatusNavigatingOffRoadMode:
                                {
                                    _navigationEngine._currentLatLng.Y = _navigationEngine._currentMonitorLocation.Latitude;
                                    _navigationEngine._currentLatLng.X = _navigationEngine._currentMonitorLocation.Longitude;
                                    double distance = GeoLatLng.Distance(_navigationEngine._currentLatLng,
                                            _navigationEngine._destinationLatLng) * 1000;
                                    if (distance < DeviationLimit)
                                    {
                                        //navigation is done, return to idle status.
                                        _navigationEngine._navigationListener.NavigationDone();
                                        _counter = 0;
                                        _navigationEngine._engineStatus = StatusIdle;
                                        if (_navigationEngine._navigationListener != null)
                                        {
                                            _navigationEngine._navigationListener.StatusChange
                                                    (StatusNavigatingOffRoadMode,
                                                    _navigationEngine._engineStatus);
                                        }
                                        continue;
                                    }
                                    if (_navigationEngine._mapDirection != null)
                                    {
                                        //if the map direction is not null
                                        //check if it's on track,if so
                                        //go the on road navigation mode
                                        ResetCurrentPointIndexBasedOnCurrentLocation();
                                        if (IsCurrentPointIndexOnTrack())
                                        {
                                            _counter = 0;
                                            _navigationEngine._engineStatus = StatusNavigatingOnRoadMode;
                                            if (_navigationEngine._navigationListener != null)
                                            {
                                                _navigationEngine._navigationListener.StatusChange
                                                        (StatusNavigatingOffRoadMode,
                                                        _navigationEngine._engineStatus);
                                            }
                                            continue;
                                        }
                                    }
                                    NotifyVoiceCommandProcessor();
                                }

                                break;
                            case StatusDeviation:
                                // Log.P("STATUS_DEVIATION");
                                {

                                    if (_counter > DeviationMaxTimes)
                                    {
                                        _counter = 0;
                                        if (_navigationEngine._allowRerouting)
                                        {
                                            FlushCommandQueue();
                                            _navigationEngine._engineStatus = StatusRoutingStart;
                                        }
                                        else
                                        {
                                            //dont allow rerouting,
                                            //go to off road navigation mode.
                                            _navigationEngine._engineStatus = StatusNavigatingOffRoadMode;
                                            if (_navigationEngine._navigationListener != null)
                                            {
                                                _navigationEngine._navigationListener.StatusChange
                                                        (StatusDeviation, _navigationEngine._engineStatus);
                                            }
                                            _counter = 0;
                                        }
                                        continue;
                                    }
                                    ResetCurrentPointIndexBasedOnCurrentLocation();
                                    bool onTrack = IsCurrentPointIndexOnTrack();
                                    if (onTrack)
                                    {
                                        _counter = 0;
                                        Log.P("regained route successfully,continue navigating");
                                        _navigationEngine._engineStatus = StatusNavigatingOnRoadMode;
                                        if (_navigationEngine._navigationListener != null)
                                        {
                                            _navigationEngine._navigationListener.StatusChange
                                                    (StatusDeviation, _navigationEngine._engineStatus);
                                        }
                                        continue;
                                    }
                                    long currentTimeStamp = (long)(DateTime.Now - StartTime).TotalMilliseconds;
                                    if (!onTrack &&
                                            (currentTimeStamp - _lastTimeStamp >
                                            EstimatedRoutingTime * 1000))
                                    {
                                        _counter++;
                                        Log.P("confirmed deviation:" + _counter);
                                        _lastTimeStamp = currentTimeStamp;
                                    }
                                    NotifyVoiceCommandProcessor();
                                }
                                break;
                        }
                        _locationReady.WaitOne();

                    }
                    catch (Exception e)
                    {
                        //catch all exeptions.
                        Log.P(e.Message);
                    }
                }
                Log.P(Thread.CurrentThread.Name + " thread stopped!");
            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 29SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Notifies the voice command processor.
            /// </summary>
            private void NotifyVoiceCommandProcessor()
            {
                //notifiy voice command processor.
                lock (_navigationEngine._voiceCommandProcessor._currentLocation)
                {
                    _navigationEngine._voiceCommandProcessor._currentLocation.X = _navigationEngine._currentMonitorLocation.Longitude;
                    _navigationEngine._voiceCommandProcessor._currentLocation.Y = _navigationEngine._currentMonitorLocation.Latitude;
                    _navigationEngine._voiceCommandProcessor._currentBearing = _navigationEngine._currentMonitorLocation.Course;

                }
                _navigationEngine._voiceCommandProcessor._currentLocationMutex.Set();
            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 29SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Dones the specified query.
            /// </summary>
            /// <param name="query">message query context(string).</param>
            /// <param name="result">The result.</param>
            public void Done(string query, MapDirection result)
            {
                _navigationEngine._mapDirection = result;
                if (_navigationEngine._mapDirection != null)
                {
                    //reset the destination location.
                    _navigationEngine._destinationLatLng.X =
                            _navigationEngine._mapDirection.Polyline
                            .GetVertex(_navigationEngine._mapDirection.Polyline.GetVertexCount() - 1).X;
                    _navigationEngine._destinationLatLng.Y =
                            _navigationEngine._mapDirection.Polyline
                            .GetVertex(_navigationEngine._mapDirection.Polyline.GetVertexCount() - 1).Y;
                }
                Log.P("routing is done!");
                _routingObject.Set();
            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 29SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Read progress notification.
            /// </summary>
            /// <param name="bytes">the number of bytes has been read.</param>
            /// <param name="total">total bytes to be read.Total will be zero if not available
            /// (content-length header not set)</param>
            public void ReadProgress(int bytes, int total)
            {
                if (_navigationEngine._navigationListener != null)
                {
                    _navigationEngine._navigationListener.ReroutingProgress(bytes, total);
                }
            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 29SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Called by the VirtualGPSDevice to which this listener is registered.
            /// This method will be called periodically according to the interval defined
            /// when registering the listener to provide updates of the current location.
            /// </summary>
            /// <param name="device">The device.</param>
            /// <param name="location">the location to which the event relates, i.e. the
            /// new position.</param>
            public void LocationUpdated(LocationProvider device, Location.Location location)
            {
                lock (_locationReady)
                {
                    _navigationEngine.CurrentLocation.Copy(location);
                    _navigationEngine._currentMonitorLocation.Copy(location);
                    if (_navigationEngine.LocationListener != null)
                    {
                        //if it's not in navagion status, just use the raw location
                        //because no alignment is needed.
                        if (_navigationEngine._engineStatus != StatusNavigatingOnRoadMode)
                        {
                            _navigationEngine.LocationListener.LocationUpdated(device,
                                    _navigationEngine._currentMonitorLocation);
                            if (_navigationEngine._navigationListener != null)
                            {
                                _navigationEngine._navigationListener.LocationUpdated(device,
                                        _navigationEngine._currentMonitorLocation, _navigationEngine._currentMonitorLocation);
                            }
                        }
                    }
                    //notify the location monitor thread.
                    
                }
                _locationReady.Set();
            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 29SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Called by the VirtualGPSDevice to which this listener is registered if
            /// the state of the VirtualGPSDevice has changed.
            /// These device state changes are delivered to the application as soon as
            /// possible after the state of a provider changes. The timing of these
            /// events is not related to the period of the location updates.
            /// If the application is subscribed to receive periodic location updates,
            /// it will continue to receive these regardless of the state of the
            /// VirtualGPSDevice. If the application wishes to stop receiving location
            /// updates for an unavailable provider, it should de-register itself from
            /// the provider.
            /// </summary>
            /// <param name="device">The device.</param>
            /// <param name="newState">The new state of the VirtualGPSDevice</param>
            public void ProviderStateChanged(LocationProvider device, int newState)
            {
                if (_navigationEngine.LocationListener != null)
                {
                    _navigationEngine.LocationListener.ProviderStateChanged(device, newState);
                }
            }



            /**
             * current location.
             */
            private readonly Location.Location _rawLocation = new Location.Location();

            /**
             * estimated routing calculating time ,default is 5 seconds.
             */
            private const int EstimatedRoutingTime = 5;

            /**
             * use to sync routing result;
             */
            internal readonly AutoResetEvent _routingObject = new AutoResetEvent(false);

            private readonly ArrayList _tempVector = new ArrayList();

            /**
             * counter to count deviation
             */
            private int _counter;

            /**
             * current point index of the map directions' polyline.
             */
            internal int _currentPointIndex;

            /**
             * last time stamp used to detects deviation.
             */
            private long _lastTimeStamp;

            /**
             * old point index, to avoid going backwards
             */
            private int _oldPointIndex;

            /**
             * used to drive location thread.
             */
            private readonly AutoResetEvent _locationReady = new AutoResetEvent(false);

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 29SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Initializes this instance.
            /// </summary>
            internal void Initialize()
            {
                _counter = 0;
                _currentPointIndex = 0;
                _lastTimeStamp = 0;
                _oldPointIndex = 0;
                _rawLocation.Latitude = 0;
                _rawLocation.Longitude = 0;
                _navigationEngine._currentWalkOnRoute.PointIndex = -1;
                _navigationEngine._currentWalkOnRoute.RouteIndex = -1;
                _navigationEngine._currentWalkOnRoute.StepIndex = -1;
            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 29SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Flushes the command queue.
            /// </summary>
            internal void FlushCommandQueue()
            {
                _navigationEngine._voiceCommandProcessor._clearCommandQueue = true;
                NotifyVoiceCommandProcessor();
            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 29SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// calculate estimated location with current speed and time
            /// </summary>
            /// <param name="time">The time.</param>
            /// <returns></returns>
            private GeoLatLng GetEstimatedLocation(int time)
            {
                if (_navigationEngine._currentMonitorLocation != null)
                {
                    //km/h
                    double currentSpeed = _navigationEngine._currentMonitorLocation.Speed;
                    //in meter
                    double distance = currentSpeed * time / 3600.0 * 1000.0;
                    const double delta = 0.0001;
                    _navigationEngine._currentLatLng.Y = _navigationEngine._currentMonitorLocation.Latitude;
                    _navigationEngine._currentLatLng.X = _navigationEngine._currentMonitorLocation.Longitude;
                    GeoLatLng estimatedLatLng = new GeoLatLng();
                    double angle = _navigationEngine._currentMonitorLocation.Course;
                    if (Double.IsNaN(distance) || Double.IsNaN(angle) || distance == 0)
                    {
                        //in degree
                        estimatedLatLng.X = _navigationEngine._currentLatLng.X;
                        estimatedLatLng.Y = _navigationEngine._currentLatLng.Y;
                    }
                    else
                    {

                        estimatedLatLng.X = _navigationEngine._currentLatLng.X + delta * Math.Sin(angle);
                        estimatedLatLng.Y = _navigationEngine._currentLatLng.Y + delta * Math.Cos(angle);
                        double estimatedDistance = GeoLatLng.Distance(_navigationEngine._currentLatLng,
                                estimatedLatLng) * 1000;

                        estimatedLatLng.X = _navigationEngine._currentLatLng.X + delta * Math.Sin(angle)
                                * distance / estimatedDistance;
                        estimatedLatLng.Y = _navigationEngine._currentLatLng.Y + delta * Math.Cos(angle)
                                * distance / estimatedDistance;
                    }

                    return estimatedLatLng;
                }
                return null;
            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 29SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Resets the current point index based on current location.
            /// </summary>
            internal void ResetCurrentPointIndexBasedOnCurrentLocation()
            {
                _navigationEngine._currentLatLng.Y = _navigationEngine._currentMonitorLocation.Latitude;
                _navigationEngine._currentLatLng.X = _navigationEngine._currentMonitorLocation.Longitude;
                GeoPoint result = _navigationEngine._mapDirection.Polyline
                        .IndexOfClosestdistanceToPoly(_navigationEngine._currentLatLng);
                _currentPointIndex = (int)result.Y;
                _oldPointIndex = _currentPointIndex;
            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 29SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Determines whether [is current point index on track].
            /// </summary>
            /// <returns>
            /// 	<c>true</c> if [is current point index on track]; otherwise, <c>false</c>.
            /// </returns>
            private bool IsCurrentPointIndexOnTrack()
            {
                int polyLineCount = _navigationEngine._mapDirection.Polyline.GetVertexCount();
                GeoLatLng nearestPoint = _navigationEngine._mapDirection
                        .Polyline.GetVertex(_currentPointIndex);
                double distance2;
                if (_currentPointIndex >= 0
                        && _currentPointIndex < polyLineCount - 1)
                {
                    int nextIndex = _currentPointIndex + 1;
                    GeoLatLng nearestPoint1 = _navigationEngine._mapDirection.Polyline.GetVertex(nextIndex);
                    while (nearestPoint1.X == nearestPoint.X &&
                            nearestPoint1.Y == nearestPoint.Y &&
                            nextIndex < polyLineCount - 1)
                    {
                        nearestPoint1 = _navigationEngine._mapDirection.Polyline.GetVertex(++nextIndex);
                    }
                    if (nearestPoint1.X == nearestPoint.X &&
                            nearestPoint1.Y == nearestPoint.Y)
                    {
                        distance2 = GeoLatLng.Distance(_navigationEngine._currentLatLng, nearestPoint) * 1000;
                    }
                    else
                    {
                        GeoPoint closestPointOnStep = GeoPolyline
                                .GetClosetPoint(nearestPoint, nearestPoint1,
                                _navigationEngine._currentLatLng, false);
                        GeoLatLng closestLatLng = new GeoLatLng(closestPointOnStep.Y,
                                closestPointOnStep.X);
                        distance2 = GeoLatLng.Distance(_navigationEngine._currentLatLng, closestLatLng) * 1000;
                    }
                }
                else
                {
                    distance2 = GeoLatLng.Distance(_navigationEngine._currentLatLng, nearestPoint) * 1000;
                }
                if (distance2 < DeviationLimit)
                {
                    return true;
                }
                return false;
            }


            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 29SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// align current location to the route,(route matching)
            /// and check to see if going backwards, it back word detected, keep the
            /// current location.
            /// </summary>
            private void AlignCurrentLocationOnTheRoad()
            {
                int polyLineCount = _navigationEngine._mapDirection.Polyline.GetVertexCount();
                _navigationEngine._currentLatLng.Y =
                        _rawLocation.Latitude;
                _navigationEngine._currentLatLng.X =
                        _rawLocation.Longitude;
                GeoLatLng pt1 = _navigationEngine._mapDirection.Polyline.GetVertex(_currentPointIndex);
                int nextPointIndex = _currentPointIndex + 1;
                GeoLatLng pt2 = _navigationEngine._mapDirection.Polyline.GetVertex(nextPointIndex);
                while (pt2.X == pt1.X && pt2.Y == pt1.Y && nextPointIndex < polyLineCount - 1)
                {
                    pt2 = _navigationEngine._mapDirection.Polyline.GetVertex(++nextPointIndex);
                }

                _currentPointIndex = nextPointIndex - 1;
                GeoPoint rt = GeoPolyline.GetClosetPoint(pt1, pt2,
                        _navigationEngine._currentLatLng, true);
                //if equals the end of the line, try next line segment.
                int tryTimes = 0;
                while (rt.X == pt2.X && rt.Y == pt2.Y && tryTimes < 3 && nextPointIndex < polyLineCount - 1)
                {
                    _currentPointIndex = nextPointIndex;
                    nextPointIndex = _currentPointIndex + 1;
                    pt1 = _navigationEngine._mapDirection.Polyline.GetVertex(_currentPointIndex);
                    pt2 = _navigationEngine._mapDirection.Polyline.GetVertex(nextPointIndex);
                    while (pt2.X == pt1.X && pt2.Y == pt1.Y && nextPointIndex < polyLineCount - 1)
                    {
                        pt2 = _navigationEngine._mapDirection.Polyline.GetVertex(++nextPointIndex);
                    }
                    rt = GeoPolyline.GetClosetPoint(pt1, pt2,
                        _navigationEngine._currentLatLng, true);
                    _currentPointIndex = nextPointIndex - 1;
                    tryTimes++;
                }
                GeoLatLng clostPoint = new GeoLatLng(rt.Y, rt.X);
                _navigationEngine._currentMonitorLocation.Latitude = clostPoint.Latitude;
                _navigationEngine._currentMonitorLocation.Longitude = clostPoint.Longitude;
                _navigationEngine._currentMonitorLocation.Course = GeoLatLng.AzimuthTo(pt1, pt2);
                //            if((pt1.X==clostPoint.X && pt1.Y == clostPoint.Y) ||
                //                    (pt2.X==clostPoint.X && pt2.Y == clostPoint.Y)){
                //                //do nothing
                //            }else{
                //                double angle1=GeoLatLng.azimuthTo(clostPoint, previousLatLng);
                //                double angle2=GeoLatLng.azimuthTo(pt2, clostPoint);
                //                if(Math.abs(angle2-angle1)>90){
                //                    //Log.P("Backwards detected:"+(angle2-angle1));
                //                    _navigationEngine.currentMonitorLocation.Latitude = previousLatLng.Lat();
                //                    _navigationEngine.currentMonitorLocation.Longitude = previousLatLng.Lng();
                //                }else{
                //                    previousLatLng.X=_navigationEngine.currentMonitorLocation.Longitude;
                //                    previousLatLng.Y=_navigationEngine.currentMonitorLocation.Latitude;
                //                }
                //            }
                _navigationEngine._currentLatLng.Y =
                        _navigationEngine._currentMonitorLocation.Latitude;
                _navigationEngine._currentLatLng.X =
                        _navigationEngine._currentMonitorLocation.Longitude;
            }

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // --------   -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Voice command generator. it's driven by voice command processor.
        /// </summary>
        private class VoiceCommandGenerator
        {
            private readonly NavigationEngine _navigationEngine;

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 29SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Initializes a new instance of the <see cref="VoiceCommandGenerator"/> class.
            /// </summary>
            /// <param name="navigationEngine">The navigation engine.</param>
            public VoiceCommandGenerator(NavigationEngine navigationEngine)
            {
                _navigationEngine = navigationEngine;
            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 29SEP2010  James Shen                 	           Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Runs this instance.
            /// </summary>
            public void Run()
            {
                Log.P(Thread.CurrentThread.Name + " thread started.");
                while (!_navigationEngine._stopThread)
                {
                    try
                    {

                        //check if this thread need to be paused.
                        _navigationEngine.CheckIfNeedPauseThread();
                        //voice command processor wakes generator up
                        _generatorObject.WaitOne();
                        switch (_navigationEngine._engineStatus)
                        {
                            case StatusDeviation:
                                break;
                            case StatusNavigatingOffRoadMode:

                                break;
                            case StatusNavigatingOnRoadMode:
                                {
                                    //initialize current walk on route
                                    //normaly it's not actually 'current walkonroute'
                                    //it's current walkonroute for command
                                    //generation, normaly it's ahead of current
                                    //locations
                                    if (_navigationEngine._currentWalkOnRoute.RouteIndex == -1)
                                    {
                                        WalkOnRoute walkOnRoute = _navigationEngine.GetCurrentWalkOnRoute();
                                        _navigationEngine._currentWalkOnRoute.RouteIndex = walkOnRoute.RouteIndex;
                                        _navigationEngine._currentWalkOnRoute.StepIndex = walkOnRoute.StepIndex;
                                        _navigationEngine._currentWalkOnRoute.PointIndex = walkOnRoute.PointIndex;
                                    }
                                    lock (_navigationEngine._voiceCommandQueue)
                                    {
                                        GenerateOnRoadVoiceCommand();
                                    }
                                }

                                break;
                        }

                    }
                    catch (Exception e)
                    {
                        //catch all exeptions.
                        Log.P(e.Message);
                    }
                }
                Log.P(Thread.CurrentThread.Name + " thread stopped!");
            }

            /**
             * the distance limit on normal road is 150 meters.
             */
            private const int NormalDistanceLimit = 150;


            /**
             * mile to km;
             */
            private const double MileToKm = 1.609344;

            /**
             * notify generator.
             */
            internal readonly AutoResetEvent _generatorObject = new AutoResetEvent(false);

            /**
             * internal usage.
             */
            internal int _nearestIndex = -1;


            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 29SEP2010  James Shen                 	           Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Get the location at given distance towards the end of the step.
            /// </summary>
            /// <param name="walkOnRoute">The walk on route.</param>
            /// <param name="mapStep">The map step.</param>
            /// <param name="distance">The distance.</param>
            /// <returns></returns>
            private GeoLatLng GetLocationAtDistance(WalkOnRoute walkOnRoute,
                    MapStep mapStep, double distance)
            {
                GeoLatLng latLng = new GeoLatLng();

                //should calculate backwards from the end of the step
                int startIndex = mapStep.LastLocationIndex;
                int endIndex = walkOnRoute.PointIndex;
                if (!_navigationEngine._useKilometer)
                {
                    distance *= MileToKm;
                }

                double length1 = 0;
                double length2 = 0;
                GeoLatLng pt1 = new GeoLatLng();
                GeoLatLng pt2 = new GeoLatLng();
                GeoLatLng[] latlngs = _navigationEngine._mapDirection.Polyline.GetPoints();
                _nearestIndex = -1;
                pt1.X = latlngs[startIndex].X;
                pt1.Y = latlngs[startIndex].Y;
                for (int i = startIndex - 1; i >= endIndex; i--)
                {
                    pt2.X = latlngs[i].X;
                    pt2.Y = latlngs[i].Y;
                    length1 = length2;
                    length2 += pt1.DistanceFrom(pt2) * 1000;
                    if (length2 >= distance && length1 <= distance)
                    {
                        _nearestIndex = i;
                        break;
                    }
                    pt1.X = pt2.X;
                    pt1.Y = pt2.Y;
                }
                if (_nearestIndex >= 0)
                {
                    double deltaDistance = length2 - length1;
                    double remainDistance = distance - length1;
                    if (deltaDistance != 0)
                    {
                        latLng.X = pt1.X + (pt2.X - pt1.X) * remainDistance / deltaDistance;
                        latLng.Y = pt1.Y + (pt2.Y - pt1.Y) * remainDistance / deltaDistance;
                    }
                    else
                    {
                        latLng.X = pt2.X;
                        latLng.Y = pt2.Y;

                    }
                }
                if (length2 < 50)
                {
                    //if total remain lenght is less than 50 meters,
                    //the minimum voice command distance, just use the end point
                    latLng.X = latlngs[endIndex].X;
                    latLng.Y = latlngs[endIndex].Y;
                    _nearestIndex = endIndex;
                }
                return latLng;
            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 29SEP2010  James Shen                 	           Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// get the distnace command to the end of the step.
            /// in revser order, for example , 250 --> 200 --> 150 -->100
            /// </summary>
            /// <param name="walkOnRoute">The walk on route.</param>
            /// <param name="mapStep">The map step.</param>
            /// <param name="isHighway">if set to <c>true</c> [is highway].</param>
            /// <returns></returns>
            private ArrayList GetDistanceToEndOfStep(WalkOnRoute walkOnRoute,
                    MapStep mapStep, bool isHighway)
            {
                ArrayList retVector;
                ArrayList distanceToStep = new ArrayList();
                long addMile = _navigationEngine._useKilometer ? VoiceCommandType.DistanceUnitKilometer :
                    VoiceCommandType.DistanceUnitMile;
                //should calculate backwards from the end of the step
                int startIndex = mapStep.LastLocationIndex;
                int endIndex = walkOnRoute.PointIndex;
                //get the remaining lenght of the step in meters.
                double distanceToEndOfStep;
                if (endIndex == mapStep.FirstLocationIndex)
                {
                    distanceToEndOfStep = mapStep.Distance;
                }
                else
                {
                    distanceToEndOfStep = _navigationEngine._mapDirection.Polyline.GetLength(endIndex,
                            startIndex);
                }
                //inglore the short remaining step.
                if (distanceToEndOfStep < 1) return distanceToStep;
                int maxDistanceIndex = GetMaximumVoiceCommandIndex(distanceToEndOfStep,
                        !_navigationEngine._useKilometer);
                for (int i = maxDistanceIndex - 1; i >= 0; i--)
                {
                    int voiceCommandType = VoiceDistances[i];
                    if (IsOptionalDistanceCommand(VoiceDistances[i]) && !_navigationEngine._moreVoiceCommand)
                    {
                        continue;
                    }
                    double distance = voiceCommandType;
                    GeoLatLng latLng = GetLocationAtDistance(walkOnRoute, mapStep, distance);
                    if (_nearestIndex >= 0)
                    {
                        VoiceCommandArg newCommandArg =
                            new VoiceCommandArg((int)((uint)voiceCommandType | (uint)addMile),
                                                mapStep.CurrentRoadName);
                        newCommandArg._commandLocation = new GeoLatLng(latLng);
                        newCommandArg._pointIndex = _nearestIndex;
                        newCommandArg._routeIndex = walkOnRoute.RouteIndex;
                        newCommandArg._stepIndex = walkOnRoute.StepIndex;
                        newCommandArg.SubVoiceCommandType = voiceCommandType;
                        distanceToStep.Add(newCommandArg);
                    }
                }
                if (_navigationEngine._moreVoiceCommand ||
                        distanceToEndOfStep < CommandDistanceLimit
                        || distanceToStep.Count <= 4)
                {
                    retVector = distanceToStep;
                }
                else
                {
                    //choose maximum 4 entries in the vector.
                    ArrayList selectVector = new ArrayList();
                    int lenOfTotal = distanceToStep.Count;
                    int middle = lenOfTotal / 4;
                    //get the first one (longest distance)
                    selectVector.Add(distanceToStep[0]);

                    //get the second first one
                    selectVector.Add(distanceToStep[middle]);

                    //which is the second one depends on the highway on not
                    if (!isHighway)
                    {
                        selectVector.Add(distanceToStep[lenOfTotal
                                - middle - 1]);
                    }
                    else
                    {
                        if (lenOfTotal > 11 && middle < lenOfTotal - 11)
                        {
                            selectVector.Add(distanceToStep
                                        [lenOfTotal - 11]);
                        }
                        else
                        {
                            selectVector.Add(distanceToStep[
                                    (middle + lenOfTotal
                                    - 1) / 2]);
                        }
                    }
                    //get the last one (nearest command 50 meters).
                    selectVector.Add(distanceToStep[lenOfTotal - 1]);
                    retVector = selectVector;
                }
                return retVector;
            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 29SEP2010  James Shen                 	           Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Gets the maximum index of the voice command.
            /// </summary>
            /// <param name="distance">The distance.</param>
            /// <param name="isMile">if set to <c>true</c> [is mile].</param>
            /// <returns></returns>
            private int GetMaximumVoiceCommandIndex(double distance, bool isMile)
            {
                int ret = 0;
                double miles = isMile ? MileToKm : 1.0;
                double yards = isMile ? 1.76 : 1.0;
                int maxIndex = VoiceDistances.Length;
                const int delimiterIndex = 12;
                for (int i = maxIndex - 1; i >= delimiterIndex; i--)
                {
                    if (IsOptionalDistanceCommand(VoiceDistances[i]) && !_navigationEngine._moreVoiceCommand)
                    {
                        continue;
                    }
                    if (distance >= VoiceDistances[i] * miles)
                    {
                        ret = i;
                        break;
                    }
                }
                if (ret < delimiterIndex)
                {

                    for (int i = delimiterIndex - 1; i >= 0; i--)
                    {
                        if (IsOptionalDistanceCommand(VoiceDistances[i]) && !_navigationEngine._moreVoiceCommand)
                        {
                            continue;
                        }
                        if (distance >= VoiceDistances[i] * miles * yards)
                        {
                            ret = i;
                            break;
                        }
                    }
                }
                return ret + 1;

            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 29SEP2010  James Shen                 	           Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Adds to voice command queue.
            /// </summary>
            /// <param name="voiceCommandArgs">The voice command args.</param>
            private void AddToVoiceCommandQueue(VoiceCommandArg[] voiceCommandArgs)
            {
                _navigationEngine._voiceCommandQueue.Add(voiceCommandArgs);
            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 29SEP2010  James Shen                 	           Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Generates the on road voice command.
            /// </summary>
            private void GenerateOnRoadVoiceCommand()
            {
                bool isHighway = _navigationEngine._currentMonitorLocation.Speed
                        > HighwaySpeedLimit;
                MapStep currentMapStep;
                if (_navigationEngine._mapDirection != null &&
                        _navigationEngine._currentWalkOnRoute.RouteIndex < _navigationEngine._mapDirection.Routes.Length)
                {
                    int maxStepLength = _navigationEngine._mapDirection
                            .Routes[_navigationEngine._currentWalkOnRoute.RouteIndex].Steps.Length;
                    if (_navigationEngine._currentWalkOnRoute.StepIndex < maxStepLength)
                    {
                        currentMapStep = _navigationEngine._mapDirection
                                .Routes[_navigationEngine._currentWalkOnRoute.RouteIndex]
                                .Steps[_navigationEngine._currentWalkOnRoute.StepIndex];
                        //next voice command type and route name
                        int voiceCommandType;
                        int voiceSubCommandType;
                        string commandRouteName;
                        //next next voice command type and route name if needed
                        bool needIncludeNextStep = false;
                        string nextCommandRouteName = "";
                        int nextVoiceCommandType = VoiceCommandType.None;
                        int nextVoiceSubCommandType = VoiceCommandType.None;
                        //if current step is not the last step
                        //then still need to create next voice command.
                        if (_navigationEngine._currentWalkOnRoute.StepIndex < maxStepLength - 1)
                        {
                            MapStep nextCommmandMapStep = _navigationEngine._mapDirection
                                    .Routes[_navigationEngine._currentWalkOnRoute.RouteIndex]
                                    .Steps[_navigationEngine._currentWalkOnRoute.StepIndex + 1];
                            double nextDistanceToEndOfStep = nextCommmandMapStep.Distance;
                            //if next step is too short, we will include
                            //next next turn if there's one.
                            if (nextDistanceToEndOfStep < NormalDistanceLimit)
                            {
                                needIncludeNextStep = true;
                            }
                            commandRouteName = nextCommmandMapStep.CurrentRoadName;
                            if (nextCommmandMapStep.DirectionCommandElements
                                    [MapDirectionCommandElement.DirectionCommand] != null)
                            {
                                voiceCommandType = nextCommmandMapStep
                                        .DirectionCommandElements
                                        [MapDirectionCommandElement.DirectionCommand]
                                        .DirectionCommandType.Type;
                            }
                            else
                            {
                                voiceCommandType = nextCommmandMapStep.CalculatedDirectionType.Type;
                            }
                            //calculated command type.
                            voiceSubCommandType = nextCommmandMapStep.CalculatedDirectionType.Type;
                        }
                        else
                        {
                            //it's the last step ,the create the destination command.
                            commandRouteName = currentMapStep.CurrentRoadName;
                            voiceCommandType = VoiceCommandType.Destination;
                            voiceSubCommandType = VoiceCommandType.Destination;
                        }
                        if (needIncludeNextStep)
                        {
                            //if need to include step ,the check if there's one
                            if (_navigationEngine._currentWalkOnRoute.StepIndex <
                                    _navigationEngine._mapDirection.Routes[_navigationEngine._currentWalkOnRoute.RouteIndex].Steps.Length - 2)
                            {
                                MapStep nextNextCommandMapStep = _navigationEngine._mapDirection
                                        .Routes[_navigationEngine._currentWalkOnRoute.RouteIndex]
                                        .Steps[_navigationEngine._currentWalkOnRoute.StepIndex + 2];
                                nextCommandRouteName = nextNextCommandMapStep.CurrentRoadName;
                                if (nextNextCommandMapStep.DirectionCommandElements
                                        [MapDirectionCommandElement.DirectionCommand] != null)
                                {
                                    nextVoiceCommandType = nextNextCommandMapStep
                                            .DirectionCommandElements
                                            [MapDirectionCommandElement.DirectionCommand]
                                            .DirectionCommandType.Type;
                                }
                                else
                                {
                                    nextVoiceCommandType = nextNextCommandMapStep.CalculatedDirectionType.Type;
                                }
                                nextVoiceSubCommandType = nextNextCommandMapStep.CalculatedDirectionType.Type;
                            }
                            else
                            {
                                //otherwise ,next command is reach the destination.
                                nextVoiceCommandType = VoiceCommandType.Destination;
                                nextVoiceSubCommandType = nextVoiceCommandType;
                                nextCommandRouteName = commandRouteName;
                            }
                        }

                        ArrayList distanceToNextStep = GetDistanceToEndOfStep(_navigationEngine._currentWalkOnRoute,
                                currentMapStep, isHighway);
                        int commandLength = distanceToNextStep.Count;
                        //the command has distance to xxx and action.
                        for (int i = 0; i < commandLength - 2; i++)
                        {
                            VoiceCommandArg voiceCommandArg = (VoiceCommandArg)
                                                              distanceToNextStep[i];
                            VoiceCommandArg[] voiceCommandArgs = new VoiceCommandArg[2];
                            voiceCommandArgs[0] = voiceCommandArg;
                            VoiceCommandArg voiceCommandArg1 =
                                    new VoiceCommandArg(voiceCommandType,
                                    commandRouteName)
                                        {
                                            _pointIndex = voiceCommandArg._pointIndex,
                                            _routeIndex = voiceCommandArg._routeIndex,
                                            _stepIndex = voiceCommandArg._stepIndex,
                                            _commandLocation = voiceCommandArg._commandLocation,
                                            SubVoiceCommandType = voiceSubCommandType
                                        };
                            voiceCommandArgs[1] = voiceCommandArg1;
                            AddToVoiceCommandQueue(voiceCommandArgs);
                        }

                        //get the last 2 commands
                        if (commandLength > 0)
                        {
                            VoiceCommandArg[] voiceCommandArgs;
                            VoiceCommandArg voiceCommandArg;
                            VoiceCommandArg voiceCommandArg1;
                            if (commandLength > 1)
                            {
                                voiceCommandArg = (VoiceCommandArg)
                                        distanceToNextStep[commandLength - 2];
                                if (needIncludeNextStep)
                                {
                                    voiceCommandArgs = new VoiceCommandArg[3];
                                    VoiceCommandArg voiceCommandArg2 =
                                            new VoiceCommandArg(nextVoiceCommandType,
                                            nextCommandRouteName)
                                                {
                                                    _pointIndex = voiceCommandArg._pointIndex,
                                                    _routeIndex = voiceCommandArg._routeIndex,
                                                    _stepIndex = voiceCommandArg._stepIndex,
                                                    _commandLocation = voiceCommandArg._commandLocation,
                                                    _optional = voiceCommandArg._optional,
                                                    SubVoiceCommandType = nextVoiceSubCommandType
                                                };
                                    voiceCommandArgs[2] = voiceCommandArg2;
                                }
                                else
                                {
                                    voiceCommandArgs = new VoiceCommandArg[2];
                                }
                                voiceCommandArgs[0] = voiceCommandArg;
                                voiceCommandArg1 = new VoiceCommandArg(voiceCommandType,
                                        commandRouteName)
                                                       {
                                                           _pointIndex = voiceCommandArg._pointIndex,
                                                           _routeIndex = voiceCommandArg._routeIndex,
                                                           _stepIndex = voiceCommandArg._stepIndex,
                                                           _commandLocation = voiceCommandArg._commandLocation,
                                                           _optional = voiceCommandArg._optional,
                                                           SubVoiceCommandType = voiceSubCommandType
                                                       };
                                voiceCommandArgs[1] = voiceCommandArg1;
                                AddToVoiceCommandQueue(voiceCommandArgs);
                            }


                            //only the command near the turning point is not Optional
                            voiceCommandArg = (VoiceCommandArg)
                                    distanceToNextStep[commandLength - 1];
                            voiceCommandArg._optional = false;
                            if (needIncludeNextStep)
                            {
                                voiceCommandArgs = new VoiceCommandArg[2];
                                VoiceCommandArg voiceCommandArg2 =
                                        new VoiceCommandArg(nextVoiceCommandType,
                                        nextCommandRouteName)
                                            {
                                                _pointIndex = voiceCommandArg._pointIndex,
                                                _routeIndex = voiceCommandArg._routeIndex,
                                                _stepIndex = voiceCommandArg._stepIndex,
                                                _commandLocation = voiceCommandArg._commandLocation,
                                                _optional = voiceCommandArg._optional,
                                                SubVoiceCommandType = nextVoiceSubCommandType
                                            };
                                voiceCommandArgs[1] = voiceCommandArg2;
                            }
                            else
                            {
                                voiceCommandArgs = new VoiceCommandArg[1];
                            }
                            voiceCommandArg1 = new VoiceCommandArg(voiceCommandType,
                                    commandRouteName)
                                                   {
                                                       _pointIndex = voiceCommandArg._pointIndex,
                                                       _routeIndex = voiceCommandArg._routeIndex,
                                                       _stepIndex = voiceCommandArg._stepIndex,
                                                       _commandLocation = voiceCommandArg._commandLocation,
                                                       _optional = voiceCommandArg._optional,
                                                       SubVoiceCommandType = voiceSubCommandType
                                                   };
                            voiceCommandArgs[0] = voiceCommandArg1;
                            AddToVoiceCommandQueue(voiceCommandArgs);
                        }
                        //move one step forward
                        if (_navigationEngine._currentWalkOnRoute.StepIndex
                                < _navigationEngine._mapDirection.Routes[_navigationEngine._currentWalkOnRoute.RouteIndex]
                                .Steps.Length - 1)
                        {
                            _navigationEngine._currentWalkOnRoute.StepIndex += 1;
                        }
                        else if (_navigationEngine._currentWalkOnRoute.RouteIndex
                                < _navigationEngine._mapDirection.Routes.Length - 1)
                        {
                            _navigationEngine._currentWalkOnRoute.StepIndex = 0;
                            _navigationEngine._currentWalkOnRoute.RouteIndex += 1;
                        }
                        //we have finish generate voice command for this step,
                        //move the  point index to next step.
                        _navigationEngine._currentWalkOnRoute.PointIndex = currentMapStep.LastLocationIndex;
                    }

                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // --------   -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Voice command processor. it's driven by the location monitor.
        /// </summary>
        internal class VoiceCommandProcessor
        {
            private readonly NavigationEngine _navigationEngine;

            public VoiceCommandProcessor(NavigationEngine navigationEngine)
            {
                _navigationEngine = navigationEngine;
            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 29SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// thread used to create the voice command and put them to the queue.
            /// </summary>
            public void Run()
            {
                Log.P(Thread.CurrentThread.Name + " thread started.");
                while (!_navigationEngine._stopThread)
                {
                    try
                    {

                        //check if this thread need to be paused.
                        _navigationEngine.CheckIfNeedPauseThread();
                        _currentLocationMutex.WaitOne();
                        if (_navigationEngine._engineStatus == StatusNavigatingOnRoadMode ||
                                _navigationEngine._engineStatus == StatusNavigatingOnRoadModePrepration)
                        {
                            if (NeedToGenerateCommand())
                            {
                                _navigationEngine._voiceCommandGenerator._generatorObject.Set();
                            }
                        }
                        if (_navigationEngine._engineStatus == StatusNavigatingOnRoadMode)
                        {

                            GetOneVoiceCommandFromQueue();
                        }
                        else if (_navigationEngine._engineStatus == StatusDeviation)
                        {
                            GenerateDeviationVoiceCommand();
                        }
                        else
                        {
                            GenerateOffRoadVoiceCommand();
                        }
                    }
                    catch (Exception e)
                    {
                        //catch all exeptions.
                        Log.P(e.Message);
                    }
                }
                Log.P(Thread.CurrentThread.Name + " thread stopped!");
            }

            /**
             * current GPS location.
             */
            internal GeoLatLng _currentLocation = new GeoLatLng();

            internal AutoResetEvent _currentLocationMutex = new AutoResetEvent(false);

            /**
             * current bearing.
             */
            internal double _currentBearing = -1;

            /**
             * last voice command time;
             */
            private long _lastVoiceCommandTime = -1;

            /**
             * last location of off road voice command.
             */
            private double _lastVoiceCommandDistance = Double.PositiveInfinity;



            /**
             * deviation command type.
             */
            private readonly VoiceCommandArg[] _reachedTargetCommandArgs
                                ={new VoiceCommandArg(VoiceCommandType.ReachedTarget,
                                    VoiceCommandType.ReachedTarget,
                            "",true)};

            /**
             * deviation command type.
             */
            private readonly VoiceCommandArg[] _deviationVoiceCommandArgs
                                ={new VoiceCommandArg(VoiceCommandType.TurnAround,
                                    VoiceCommandType.TurnAround,
                            "",true)};

            /**
             * clear the command Queue.
             */
            internal volatile bool _clearCommandQueue;

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 29SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Needs to generate command.
            /// </summary>
            /// <returns></returns>
            private bool NeedToGenerateCommand()
            {
                lock (_navigationEngine._voiceCommandQueue)
                {
                    if (_clearCommandQueue)
                    {
                        _navigationEngine._voiceCommandQueue.Clear();
                        _clearCommandQueue = false;
                    }
                    WalkOnRoute wor = _navigationEngine.GetCurrentWalkOnRoute();
                    if ((_navigationEngine._voiceCommandQueue.Count < VoiceCommandQueueSize) &&
                            wor.PointIndex < _navigationEngine._mapDirection.Polyline.GetVertexCount() - 1)
                    {
                        return true;
                    }
                    return false;
                }
            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 29SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Generates the deviation voice command.
            /// </summary>
            private void GenerateDeviationVoiceCommand()
            {
                long currentTime = (long)(DateTime.Now - StartTime).TotalMilliseconds;
                if (currentTime > _lastVoiceCommandTime +
                        OfflineNavigationVoiceCommandInterval * 1000)
                {
                    _lastVoiceCommandTime = currentTime;
                    _deviationVoiceCommandArgs[0]._commandLocation = new GeoLatLng(_navigationEngine._currentLatLng);
                    _navigationEngine._voiceCommandListener.VoiceCommandAction(_deviationVoiceCommandArgs, true);

                }
            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 29SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Generates the off road voice command.
            /// </summary>
            private void GenerateOffRoadVoiceCommand()
            {
                long currentTime = (long)(DateTime.Now - StartTime).TotalMilliseconds;
                if (currentTime > _lastVoiceCommandTime +
                        OfflineNavigationVoiceCommandInterval * 1000)
                {
                    _lastVoiceCommandTime = currentTime;
                    double angle = GeoLatLng.AzimuthTo(_navigationEngine._currentLatLng, _navigationEngine._destinationLatLng);
                    double bearing = angle - _currentBearing;

                    if (bearing < 0) bearing += 360;
                    double distance = GeoLatLng.Distance(_navigationEngine._destinationLatLng, _navigationEngine._currentLatLng) * 1000;
                    bool isClosing = true;
                    if (_lastVoiceCommandDistance < distance)
                    {
                        isClosing = false;
                    }
                    _lastVoiceCommandDistance = distance;
                    if (_navigationEngine._voiceCommandListener != null)
                    {
                        if (distance < 5)
                        {
                            _reachedTargetCommandArgs[0]._commandLocation = new GeoLatLng(_navigationEngine._destinationLatLng);
                            _navigationEngine._voiceCommandListener.VoiceCommandAction(_reachedTargetCommandArgs, true);
                            //navigation is done, return to idle status.
                            _navigationEngine._engineStatus = StatusIdle;
                            if (_navigationEngine._navigationListener != null)
                            {
                                _navigationEngine._navigationListener.NavigationDone();
                                _navigationEngine._navigationListener.StatusChange(StatusNavigatingOffRoadMode,
                                        _navigationEngine._engineStatus);
                            }

                        }
                        else
                        {
                            SendOneOffRoadVoiceCommand(bearing, isClosing);
                        }
                    }

                }
            }


            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 29SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Sends the one off road voice command.
            /// </summary>
            /// <param name="bearing">The bearing.</param>
            /// <param name="isClosing">if set to <c>true</c> [is closing].</param>
            private void SendOneOffRoadVoiceCommand(double bearing, bool isClosing)
            {
                int arg = ((int)(bearing + 0.5));
                GeoLatLng argLatLng = new GeoLatLng(_navigationEngine._currentLatLng);
                VoiceCommandArg[] voiceCommandArgs = new VoiceCommandArg[2];
                if (isClosing)
                {
                    voiceCommandArgs[1] = new VoiceCommandArg(VoiceCommandType.ClosingTarget, arg, true)
                                            {
                                                SubVoiceCommandType = VoiceCommandType.ClosingTarget
                                            };
                }
                else
                {
                    voiceCommandArgs[1] = new VoiceCommandArg(VoiceCommandType.AwayfromTarget, arg, true)
                                            {
                                                SubVoiceCommandType = VoiceCommandType.AwayfromTarget
                                            };
                }

                voiceCommandArgs[1]._commandLocation = argLatLng;

                bearing -= 15;
                if (bearing < 0) bearing += 360;
                int oclock = ((int)bearing / 30) % 12;
                int commandType = VoiceCommandType.TargetAt01Oclock + oclock;
                voiceCommandArgs[0] = new VoiceCommandArg(commandType, arg, true)
                                        {
                                            SubVoiceCommandType = commandType,
                                            _commandLocation = argLatLng
                                        };
                _navigationEngine._voiceCommandListener.VoiceCommandAction(voiceCommandArgs, true);
            }



            /////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 29SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Gets the one voice command from queue.
            /// </summary>
            private void GetOneVoiceCommandFromQueue()
            {
                GeoLatLng latLng;
                lock (_currentLocation)
                {
                    latLng = new GeoLatLng(_currentLocation);

                }
                lock (_navigationEngine._voiceCommandQueue)
                {
                    {
                        double distance = GeoLatLng.Distance(_navigationEngine._currentLatLng,
                                _navigationEngine._destinationLatLng) * 1000;
                        if (distance < 5)
                        {
                            //navigation is done, return to idle status.
                            _navigationEngine._engineStatus = StatusIdle;
                            if (_navigationEngine._navigationListener != null)
                            {
                                _navigationEngine._navigationListener.NavigationDone();
                                _navigationEngine._navigationListener.StatusChange(StatusNavigatingOnRoadMode,
                                        _navigationEngine._engineStatus);
                            }
                            _navigationEngine._voiceCommandQueue.Clear();
                        }
                    }
                    if (_navigationEngine._voiceCommandQueue.Count == 0)
                    {
                        return;
                    }
                    int closestIndex = -1;
                    int expiredIndex = -1;
                    WalkOnRoute wor = _navigationEngine.GetCurrentWalkOnRoute();
                    int currentPointIndex = wor.PointIndex;
                    double distanceLimit = (double)CommandDistanceLimit / 4;
                    if (_navigationEngine._currentMonitorLocation.Speed
                        > HighwaySpeedLimit)
                    {
                        distanceLimit = CommandDistanceLimit;
                    }
                    for (int i = 0; i < _navigationEngine._voiceCommandQueue.Count; i++)
                    {
                        VoiceCommandArg[] elems = (VoiceCommandArg[])
                                                  _navigationEngine._voiceCommandQueue[i];
                        GeoLatLng latLng1 = elems[0]._commandLocation;
                        int routeIndex = elems[0]._routeIndex;
                        int stepIndex = elems[0]._stepIndex;
                        double distance = latLng.DistanceFrom(latLng1) * 1000;
                        if (distance < distanceLimit
                                && routeIndex <= wor.RouteIndex &&
                                stepIndex <= wor.StepIndex)
                        {
                            closestIndex = i;
                        }
                        if (elems[0]._pointIndex < currentPointIndex)
                        {
                            expiredIndex = i;
                        }
                    }

                    int needToDelete = Math.Max(closestIndex, expiredIndex);
                    if (needToDelete >= 0)
                    {
                        object[] objectToDelete = new object[needToDelete + 1];
                        for (int i = 0; i < objectToDelete.Length; i++)
                        {
                            objectToDelete[i] = _navigationEngine._voiceCommandQueue[i];

                        }
                        VoiceCommandArg[] lastVoiceCommandQueue =
                               (VoiceCommandArg[])objectToDelete[needToDelete];
                        if (_navigationEngine._voiceCommandListener != null)
                        {
                            _navigationEngine._voiceCommandListener.VoiceCommandAction(lastVoiceCommandQueue,
                                    lastVoiceCommandQueue[0]._optional);

                        }
                        for (int i = 0; i < objectToDelete.Length; i++)
                        {
                            _navigationEngine._voiceCommandQueue.Remove(objectToDelete[i]);
                        }
                    }

                }

            }

        }
    }
}

