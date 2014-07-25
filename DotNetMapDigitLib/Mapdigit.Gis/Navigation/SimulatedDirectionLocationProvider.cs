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
using System.Threading;
using Mapdigit.Gis.Geometry;
using Mapdigit.Gis.Location;
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
    /// Simulation location provider, it uses current route as the simulation source.
    /// </summary>
    public class SimulatedDirectionLocationProvider : LocationProvider
    {

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatedDirectionLocationProvider"/> class.
        /// </summary>
        public SimulatedDirectionLocationProvider()
        {
            CurrentLocation.Status = true;
            CurrentState = Available;
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
        /// <param name="mapDirection">The map direction.</param>
        /// <param name="speed">simulation driving speed  in kph or mile/h</param>
        /// <param name="isMile">is mile or not.</param>
        public void StartSimulation(MapDirection mapDirection, int speed,
                bool isMile)
        {
            lock (syncObject)
            {
                if (_stopThread)
                {
                    if (mapDirection == null)
                    {
                        throw new ArgumentException("Map Direction cannot be null");
                    }
                    //make a copy of the input mapDirection object.
                    _mapDirection = new MapDirection(mapDirection);
                    GeoLatLng firstLocation = mapDirection.Polyline.GetVertex(0);
                    lock (CurrentLocation)
                    {
                        _currentPointIndex = -1;
                        CurrentLocation.Latitude = firstLocation.Y;
                        CurrentLocation.Longitude = firstLocation.X;
                    }
                    _simulationSpeed = speed;
                    _isMile = isMile;
                    _stopThread = false;
                    _pauseThread = false;
                    if (_thisThread != null)
                    {
                        //_thisThread.Interrupt();
                        _thisThread = null;
                    }
                    _thisThread = new Thread(Run);
                    _thisThread.Name = "SimulationDirectionLocationProvider" + _currentThreadIndex++;
                    _thisThread.Start();
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
        /// Pauses the simulation.
        /// </summary>
        public void PauseSimulation()
        {
            _pauseThread = true;
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
            if (_pauseThread)
            {
                _pauseObject.Set();
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
            if (!_stopThread)
            {
                _stopThread = true;
                try
                {
                    _pauseObject.Set();
                    //_thisThread.Interrupt();
                    _thisThread = null;
                    _currentPointIndex = -1;
                    CurrentLocation.Latitude = CurrentLocation.Longitude = 0;
                }
                catch (Exception)
                {

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
        /// set the deviation value, if not zero, the simulated location will have
        /// deviation with a random value with a circle with radius of value
        /// from it's exact location.
        /// </summary>
        /// <param name="value">the deviation value in meters.</param>
        public void SetDeviation(int value)
        {
            _variation = value;
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
            StopSimulation();
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Runs this instance.
        /// </summary>
        public void Run()
        {
            Log.P(Thread.CurrentThread.Name + " thread started!");
            _currentPointIndex = 0;
            while (!_stopThread)
            {
                try
                {
                    if (_pauseThread)
                    {
                        Log.P("Simulation thread paused", Log.Debug);
                        _pauseObject.WaitOne();
                        Log.P("Simulation thread resumed", Log.Debug);
                    }

                    if (_currentPointIndex < _mapDirection.Polyline.GetVertexCount())
                    {
                        GeoLatLng latLng = _mapDirection.Polyline.GetVertex(_currentPointIndex);
                        if (_currentPointIndex == 0)
                        {
                            lock (CurrentLocation)
                            {
                                CurrentLocation.Latitude = latLng.Latitude;
                                CurrentLocation.Longitude = latLng.Longitude;
                                CurrentLocation.Speed = _simulationSpeed;
                                CurrentLocation.TimeStamp = DateTime.Now;
                                CurrentLocation.Course = 0;
                            }
                            if (LocationListener != null)
                            {
                                LocationListener.LocationUpdated(this, CurrentLocation);
                            }

                        }
                        else
                        {
                            GeoLatLng latLng1 = _mapDirection.Polyline
                                    .GetVertex(_currentPointIndex - 1);
                            if (latLng1.X != latLng.X || latLng1.Y != latLng.Y)
                            {
                                CurrentLocation.Course = GeoLatLng.AzimuthTo(latLng1, latLng);
                                double distance = GeoLatLng.Distance(latLng1, latLng);
                                double deltaRadius = 0;
                                if (_variation != 0)
                                {
                                    double distInMeter = distance * 1000;
                                    double deltaX = (_variation / distInMeter)
                                            * Math.Abs(latLng1.X - latLng.X);
                                    double deltaY = (_variation / distInMeter)
                                            * Math.Abs(latLng1.Y - latLng.Y);
                                    deltaRadius = Math.Min(deltaX, deltaY);

                                }
                                double time;
                                if (_isMile)
                                {
                                    time = distance / (_simulationSpeed * 1.6);

                                }
                                else
                                {
                                    time = distance / (_simulationSpeed);
                                }
                                time = time * 3600 / locationInterval;
                                int numOfNewPoint = (int)time;
                                GeoPoint[] newPoints = GeoPolyline.LineSegments(latLng1, latLng,
                                        numOfNewPoint, true);
                                if (newPoints != null)
                                {
                                    for (int i = 0; i < newPoints.Length; i++)
                                    {
                                        lock (CurrentLocation)
                                        {
                                            CurrentLocation.Latitude = newPoints[i].Y;
                                            CurrentLocation.Longitude = newPoints[i].X;
                                            if (_variation != 0)
                                            {
                                                Random rd = new Random(DateTime.Now.Millisecond);
                                                int angle = rd.Next();
                                                int deviation = rd.Next(_variation);
                                                CurrentLocation.Latitude =
                                                        CurrentLocation.Latitude
                                                        + deviation * deltaRadius
                                                        * Math.Sin(angle * Math.PI / 180.0);
                                                CurrentLocation.Longitude =
                                                        CurrentLocation.Longitude
                                                        + deviation * deltaRadius
                                                        * Math.Cos(angle * Math.PI / 180.0);
                                            }
                                            CurrentLocation.Speed = _simulationSpeed;
                                            CurrentLocation.TimeStamp = DateTime.Now;
                                        }
                                        if (LocationListener != null)
                                        {
                                            LocationListener.LocationUpdated(this, CurrentLocation);
                                        }
                                        //wait some time
                                        try
                                        {
                                            Thread.Sleep(locationInterval * 1000);
                                        }
                                        catch (Exception)
                                        {
                                            //ex.printStackTrace();
                                        }


                                    }
                                }

                            }

                        }

                        _currentPointIndex++;
                        if (_currentPointIndex > _mapDirection.Polyline.GetVertexCount() - 1)
                        {
                            //last location will be send 10 times
                            for (int i = 0; i < 10; i++)
                            {
                                latLng = _mapDirection.Polyline
                                        .GetVertex(_mapDirection.Polyline
                                        .GetVertexCount() - 1);
                                lock (CurrentLocation)
                                {
                                    CurrentLocation.Latitude = latLng.Latitude;
                                    CurrentLocation.Longitude = latLng.Longitude;
                                    CurrentLocation.Speed = _simulationSpeed;
                                    CurrentLocation.TimeStamp = DateTime.Now;
                                    CurrentLocation.Course = 0;
                                }
                                if (LocationListener != null)
                                {
                                    LocationListener.LocationUpdated(this, CurrentLocation);
                                }
                                try
                                {
                                    Thread.Sleep(locationInterval * 1000);
                                }
                                catch (Exception)
                                {
                                    //ex.printStackTrace();
                                }
                            }

                            break;
                        }
                    }
                    else
                    {
                        _currentPointIndex = 0;
                        GeoLatLng latLng = _mapDirection.Polyline.GetVertex(_currentPointIndex);
                        lock (CurrentLocation)
                        {
                            CurrentLocation.Latitude = latLng.Latitude;
                            CurrentLocation.Longitude = latLng.Longitude;
                            CurrentLocation.Speed = _simulationSpeed;
                            CurrentLocation.TimeStamp = DateTime.Now;
                            CurrentLocation.Course = 0;
                        }
                        if (LocationListener != null)
                        {
                            LocationListener.LocationUpdated(this, CurrentLocation);
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.P(Thread.CurrentThread.Name
                            + " thread stopped with exception!" + e.Message);
                }

            }
            Log.P(Thread.CurrentThread.Name + " thread stopped!");
        }

        /**
         * Map Direction for simulation.
         */
        private MapDirection _mapDirection;

        /**
         * allowed varation in meters,default is 0.
         */
        private int _variation;

        /**
         * Simulation speed, default is 50Kph.
         */
        private int _simulationSpeed = 50;


        /**
         * use to control the stop the thread.
         */
        private volatile bool _stopThread = true;

        /**
         * current index of point in the polyline.
         */
        private volatile int _currentPointIndex = -1;


        /**
         * Is the speed mile or not.
         */
        private bool _isMile;


        /**
         * use to control the pause/resume the thread.
         */
        private volatile bool _pauseThread;


        /**
         * object use to signal pause/resume.
         */
        private readonly AutoResetEvent _pauseObject = new AutoResetEvent(false);

        /**
         * this thread.
         */
        private Thread _thisThread;


        /**
         * thread index
         */
        private static int _currentThreadIndex;



    }

}
