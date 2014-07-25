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
using System;
using System.Threading;
using Mapdigit.Ajax;
using Mapdigit.Drawing;
using Mapdigit.Drawing.Geometry;
using Mapdigit.Gis.Drawing;
using Mapdigit.Gis.Geometry;
using Mapdigit.Gis.Service;

using Mapdigit.Util;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Gis.Raster
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 28SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// RasterMap a map class uses to display raster map.
    /// </summary>
    public class RasterMap : DigitalMap
    {
        
        private static RasterMap _rasterMap;

        private static Random _rand = new Random();

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the total downloaded bytes.
        /// </summary>
        /// <returns>the total byte has be downloaded</returns>
        public static long GetTotalDownloadedBytes()
        {
            return Request.TotaldownloadedBytes
                    + MapTileAbstractReader.TotaldownloadedBytes;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="RasterMap"/> class.
        /// </summary>
        /// <param name="width">the width of the map image</param>
        /// <param name="height">the height of the map image</param>
        /// <param name="mapTileDownloadManager">The map tile download manager.</param>
        public RasterMap(int width, int height,
                MapTileDownloadManager mapTileDownloadManager)
            : this(width, height, MapType.MicrosoftMap, mapTileDownloadManager)
        {

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="RasterMap"/> class.
        /// </summary>
        /// <param name="width">the width of the map image</param>
        /// <param name="height">the height of the map image</param>
        /// <param name="mapType">Type of the map.</param>
        /// <param name="mapTileDownloadManager">The map tile download manager.</param>
        public RasterMap(int width, int height, int mapType,
                MapTileDownloadManager mapTileDownloadManager)
            : base(width, height)
        {

            _screenRectangle = new Rectangle(0, 0, screenSize.Width,
                    screenSize.Height);
            _mapTileEngine = new ArrayTiledMapTileEngine(width, height,
                      mapTileDownloadManager, this);
            typeOfMap = mapType;
            _rasterMap = this;
        }

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the map drawing listener.
        /// </summary>
        /// <param name="mapDrawingListener">The map drawing listener.</param>
        public void SetMapDrawingListener(IMapDrawingListener mapDrawingListener)
        {
            _mapTileEngine.SetMapDrawingListener(mapDrawingListener);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the size of the screen.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public override void SetScreenSize(int width, int height)
        {
            lock (syncObject)
            {
                base.SetScreenSize(width, height);
                screenSize.Height = height;
                screenSize.Width = width;
                _mapTileEngine.SetScreenSize(width, height);
                _screenRectangle = new Rectangle(0, 0, screenSize.Width,
                        screenSize.Height);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29DEC2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// set current  map service type.
        /// </summary>
        /// <param name="mapServiceType">map service type.defined in DigitalMapService.</param>
        public void SetCurrentMapService(int mapServiceType)
        {
            digitalMapService = DigitalMapService.GetCurrentMapService(mapServiceType);
        }
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the center.
        /// </summary>
        /// <param name="center">The center.</param>
        /// <param name="zoomLevel">The zoom level.</param>
        /// <param name="mapType">Type of the map.</param>
        public void SetCenter(GeoLatLng center, int zoomLevel, int mapType)
        {
            lock (syncObject)
            {
                typeOfMap = mapType;
                base.SetCenter(center, zoomLevel);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// is the point in current screen (is shown or not).
        /// </summary>
        /// <param name="pt">point to be tested</param>
        /// <returns>true is in screen range</returns>
        public override bool IsPointVisible(GeoLatLng pt)
        {
            lock (syncObject)
            {
                GeoPoint screenPt = FromLatLngToMapPixel(pt);
                _screenRectangle.X=_mapTileEngine.GetScreenOffsetX();
                _screenRectangle.Y=_mapTileEngine.GetScreenOffsetY();
                return _screenRectangle.Contains((int)screenPt.X, (int)screenPt.Y);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the screen center.
        /// </summary>
        /// <returns> the center of the screen in latitude,longititude pair</returns>
        public GeoLatLng GetScreenCenter()
        {
            lock (syncObject)
            {
                return _mapTileEngine.GetScreenCenter();
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Increments zoom level by one.
        /// </summary>
        public override void ZoomIn()
        {

            GetMapTileDownloadManager().ClearTaskList();
            lock (syncObject)
            {
                _mapTileEngine.ZoomIn();
                GeoLatLng center = GetScreenCenter();
                mapCenterPt.X = center.X;
                mapCenterPt.Y = center.Y;
                lock (mapLayers)
                {
                    for (int i = 0; i < mapLayers.Count; i++)
                    {
                        MapLayer mapLayer = (MapLayer)mapLayers[i];
                        GeoLatLng layerCenter = mapLayer.GetCenter();
                        layerCenter.X = center.X;
                        layerCenter.Y = center.Y;
                    }
                }
                base.ZoomIn();
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Decrements zoom level by one.
        /// </summary>
        public override void ZoomOut()
        {
            GetMapTileDownloadManager().ClearTaskList();
            lock (syncObject)
            {
                _mapTileEngine.ZoomOut();
                GeoLatLng center = GetScreenCenter();
                mapCenterPt.X = center.X;
                mapCenterPt.Y = center.Y;
                lock (mapLayers)
                {
                    for (int i = 0; i < mapLayers.Count; i++)
                    {
                        MapLayer mapLayer = (MapLayer)mapLayers[i];
                        GeoLatLng layerCenter = mapLayer.GetCenter();
                        layerCenter.X = center.X;
                        layerCenter.Y = center.Y;
                    }
                }
                base.ZoomOut();
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Starts a pan with given distance in pixels.
        /// directions. +1 is right and down, -1 is left and up, respectively.
        /// </summary>
        /// <param name="dx">x offset</param>
        /// <param name="dy">y offset.</param>
        public override void PanDirection(int dx, int dy)
        {
            lock (syncObject)
            {
                _mapTileEngine.PanDirection(dx, dy);
                GeoLatLng center = GetScreenCenter();
                lock (mapLayers)
                {
                    for (int i = 0; i < mapLayers.Count; i++)
                    {
                        MapLayer mapLayer = (MapLayer)mapLayers[i];
                        GeoLatLng layerCenter = mapLayer.GetCenter();
                        layerCenter.X = center.X;
                        layerCenter.Y = center.Y;
                        mapLayer.PanDirection(dx, dy);
                    }
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
        /// Sets the type of the map.
        /// </summary>
        /// <param name="mapType">Type of the map.</param>
        public void SetMapType(int mapType)
        {
            GetMapTileDownloadManager().ClearTaskList();
            lock (syncObject)
            {
                typeOfMap = mapType;
                DrawMapCanvas();
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the route pen.
        /// </summary>
        /// <param name="routePen">The route pen.</param>
        public void SetRoutePen(Pen routePen)
        {
            _mapTileEngine.SetRoutePen(routePen);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the type of the map.
        /// </summary>
        /// <returns></returns>
        public virtual int GetMapType()
        {
            lock (syncObject)
            {
                return typeOfMap;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Stores the current map position and zoom level for later
        /// </summary>
        public void SavePosition()
        {
            lock (syncObject)
            {
                _storedZoomLevel = mapZoomLevel;
                _storedPosition = new GeoLatLng(mapCenterPt);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Restores the map view that was saved by savePosition().
        /// </summary>
        public void ReturnToSavedPosition()
        {
            lock (syncObject)
            {
                if (_storedPosition != null)
                {
                    mapZoomLevel = _storedZoomLevel;
                    PanTo(_storedPosition);
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
        /// Clears the map cache.
        /// </summary>
        public void ClearMapCache()
        {
            _mapTileEngine.ClearMapCache();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Restores the map cache.
        /// </summary>
        public void RestoreMapCache()
        {
            _mapTileEngine.RestoreMapCache();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Saves the map cache.
        /// </summary>
        public void SaveMapCache()
        {
            _mapTileEngine.SaveMapCache();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draw the map layer to an graphics
        /// </summary>
        /// <param name="graphics">the graphics object where the map is drawn..</param>
        /// <param name="offsetX">The offset X.</param>
        /// <param name="offsetY">The offset Y.</param>
        public override void Paint(IGraphics graphics, int offsetX, int offsetY)
        {
            _mapTileEngine.Paint(graphics, offsetX, offsetY);
            DrawLogo(graphics);
            base.Paint(graphics, offsetX, offsetY);

        }


        private void DrawLogo(IGraphics graphics)
        {
            
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the map canvas.
        /// </summary>
        protected override void DrawMapCanvas()
        {
            lock (syncObject)
            {
                _mapTileEngine.DrawMapCanvas();
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// convert laititude,longitude pair to the coordinates on screen.
        /// </summary>
        /// <param name="latlng"> the latitude,longitude location.</param>
        /// <returns>x,y coordniate on screen.</returns>
        public GeoPoint FromLatLngToScreenPixel(GeoLatLng latlng)
        {
            lock (syncObject)
            {
                return _mapTileEngine.FromLatLngToScreenPixel(latlng);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// convert the coordinates on screen to laititude,longitude pair .
        /// </summary>
        /// <param name="pt">x,y coordniate on screen</param>
        /// <returns>the latitude,longitude position.</returns>
        public GeoLatLng FromScreenPixelToLatLng(GeoPoint pt)
        {
            lock (syncObject)
            {
                return _mapTileEngine.FromScreenPixelToLatLng(pt);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the map direction.
        /// </summary>
        /// <param name="newDirection">The new direction.</param>
        public void SetMapDirection(MapDirection newDirection)
        {
            _mapTileEngine.SetMapDirection(newDirection);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the map directions.
        /// </summary>
        /// <param name="newDirections">The new directions.</param>
        public void SetMapDirections(MapDirection[] newDirections)
        {
            _mapTileEngine.SetMapDirections(newDirections);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Clears the map direction.
        /// </summary>
        public void ClearMapDirection()
        {
            _mapTileEngine.ClearMapDirection();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the map tile download manager.
        /// </summary>
        /// <returns></returns>
        public MapTileDownloadManager GetMapTileDownloadManager()
        {
            return _mapTileEngine.GetMapTileDownloadManager();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the download manager.
        /// </summary>
        /// <param name="mapTileDownloadManager">The map tile download manager.</param>
        public void SetDownloadManager(MapTileDownloadManager mapTileDownloadManager)
        {
            _mapTileEngine.SetDownloadManager(mapTileDownloadManager);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="RasterMap"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="mapType">Type of the map.</param>
        internal RasterMap(int width, int height, int mapType)
            : this(width, height, mapType, null)
        {

        }

        //turn on the pan thread or not.
        internal bool _usePanThread = true;

        internal void SetCenterCommand()
        {
            if (_usePanThread)
            {
                _pandirectionThread.SetCenterCommand();
            }
            else
            {
                SetCenter(GetScreenCenter(), mapZoomLevel, typeOfMap);
            }
        }

        internal void UpdatedMapCommand()
        {
            if (_usePanThread)
            {
                _pandirectionThread.UpdatedMapCommand();
            }
            else
            {
                _mapTileEngine.DrawUpdatedMapCanvas();
            }
        }

        internal class PandirectionThread
        {

            private volatile int _command = -1;
            private readonly AutoResetEvent _panSyncObject = new AutoResetEvent(false);
            private SetCenterThread _setCenterThread = new SetCenterThread();
            private UpdateMapThread _updateMapThread = new UpdateMapThread();

            public void Start()
            {

                Thread thread = new Thread(_setCenterThread.Run);
                thread.Name = "setCenterThread";
                thread.Start();
                thread = new Thread(_updateMapThread.Run);
                thread.Name = "updateMapThread";
                thread.Start();
                thread = new Thread(Run);
                thread.Name = "PandirectionThread";
                thread.Start();
            }

            public void Run()
            {
                Log.P(Thread.CurrentThread.Name + " thread started");
                while (!_rasterMap._panStopThread)
                {
                    switch (_command)
                    {
                        case 1:
                            _setCenterThread.SetCenterCommand();
                            break;
                        case 2:
                            _updateMapThread.UpdateMapCommand();
                            break;
                        default:

                            break;
                    }
                    _panSyncObject.WaitOne();


                }
                Log.P(Thread.CurrentThread.Name + " thread stopped");
            }

            public void StopThread()
            {
                _rasterMap._panStopThread = true;
                _panSyncObject.Set();
                _setCenterThread._panSyncObject.Set();
                _updateMapThread._panSyncObject.Set();
                _rasterMap._pandirectionThread = null;
                _setCenterThread = null;
                _updateMapThread = null;
            }

            public void SetCenterCommand()
            {
                lock (_panSyncObject)
                {
                    _command = 1;
                

                }
                _panSyncObject.Set();
            }

            public void UpdatedMapCommand()
            {
                lock (_panSyncObject)
                {
                    _command = 2;
      

                }
                _panSyncObject.Set();
            }
        }

        internal PandirectionThread GetNewPandirectionThread()
        {
            _pandirectionThread = new PandirectionThread();
            //_pandirectionThread.setPriority(Thread.MIN_PRIORITY);
            return _pandirectionThread;
        }

        /**
         * screen rectangle
         */
        private Rectangle _screenRectangle;

        /**
         * map tile engine.
         */
        internal readonly MapTileEngine _mapTileEngine;

        /**
         * stored position
         */
        private GeoLatLng _storedPosition;
        /**
         * stored zoom Level
         */
        private int _storedZoomLevel;
        private volatile bool _panStopThread;
        private PandirectionThread _pandirectionThread;

        private static readonly byte[] ImageGuidebeeLogoArray = new byte[]
                                                                  {
                            0x89,0x50,0x4e,0x47,0x0d,0x0a,0x1a,0x0a,
                            0x00,0x00,0x00,0x0d,0x49,0x48,0x44,0x52,
                            0x00,0x00,0x00,0xe2,0x00,0x00,0x00,0x37,
                            0x08,0x06,0x00,0x00,0x00,0x11,0xe3,0x96,
                            0x4b,0x00,0x00,0x00,0x04,0x67,0x41,0x4d,
                            0x41,0x00,0x00,0xb1,0x8e,0x7c,0xfb,0x51,
                            0x93,0x00,0x00,0x00,0x20,0x63,0x48,0x52,
                            0x4d,0x00,0x00,0x7a,0x25,0x00,0x00,0x80,
                            0x83,0x00,0x00,0xf9,0xff,0x00,0x00,0x80,
                            0xe8,0x00,0x00,0x75,0x30,0x00,0x00,0xea,
                            0x60,0x00,0x00,0x3a,0x97,0x00,0x00,0x17,
                            0x6f,0x97,0xa9,0x99,0xd4,0x00,0x00,0x09,
                            0x4e,0x49,0x44,0x41,0x54,0x78,0x9c,0x62,
                            0xfc,0xff,0xff,0x3f,0xc3,0x28,0x18,0x05,
                            0xa3,0x60,0x60,0x01,0x40,0x00,0x31,0x0d,
                            0xb4,0x03,0x46,0xc1,0x28,0x18,0x05,0x0c,
                            0x0c,0x00,0x01,0x34,0x9a,0x11,0x47,0xc1,
                            0x28,0x18,0x04,0x00,0x20,0x80,0x46,0x33,
                            0xe2,0x28,0x18,0x05,0x83,0x00,0x00,0x04,
                            0xd0,0x68,0x46,0x1c,0x05,0xa3,0x60,0x10,
                            0x00,0x80,0x00,0x1a,0xcd,0x88,0xa3,0x60,
                            0x14,0x0c,0x02,0x00,0x10,0x40,0xa3,0x19,
                            0x71,0x14,0x8c,0x82,0x41,0x00,0x00,0x02,
                            0x68,0x34,0x23,0x8e,0x82,0x51,0x30,0x08,
                            0x00,0x40,0x00,0x8d,0x66,0xc4,0xa1,0x0a,
                            0x2e,0x5e,0x14,0x60,0x98,0x3d,0xdb,0x8d,
                            0xa1,0xb6,0x36,0x65,0xa0,0x9d,0x82,0x01,
                            0x6e,0xdd,0x62,0x63,0x58,0xb1,0xc2,0x82,
                            0x21,0x33,0xb3,0x62,0xa0,0x9d,0x32,0x54,
                            0x00,0x40,0x00,0x31,0x52,0x6d,0x42,0x1f,
                            0x14,0xf8,0x4f,0x9e,0xc8,0x30,0x9c,0x3f,
                            0x6f,0x04,0xe6,0x1f,0x3c,0x68,0xcf,0xb0,
                            0x79,0xb3,0x0f,0x86,0xba,0x8c,0x8c,0x99,
                            0x0c,0x2a,0x2a,0x77,0xc0,0x6c,0x43,0xc3,
                            0x73,0x0c,0x5c,0x5c,0xdf,0x18,0x2c,0x2c,
                            0x5e,0x50,0xc7,0x11,0x74,0x04,0x20,0xff,
                            0xf6,0xf7,0x17,0x31,0xcc,0x98,0x91,0x0e,
                            0xf6,0x53,0x61,0x61,0x1f,0x83,0x9a,0xda,
                            0x2f,0x9a,0xd9,0xb7,0x6f,0x9f,0x12,0xc3,
                            0xab,0x57,0x62,0xc0,0x70,0x75,0x00,0xdb,
                            0x09,0x01,0x0a,0x40,0xfc,0x80,0xe1,0xff,
                            0x7f,0x45,0x9a,0xd9,0x4b,0x0c,0x38,0x71,
                            0x42,0x82,0xe1,0xf5,0x6b,0x31,0x60,0x98,
                            0xa8,0x31,0x94,0x94,0x74,0x43,0x45,0x15,
                            0x18,0x06,0x83,0xdb,0x86,0x08,0x00,0x08,
                            0x20,0xca,0x32,0x22,0x28,0x02,0xb6,0x6e,
                            0xf5,0x61,0x68,0x69,0xa9,0x86,0x8a,0x28,
                            0x40,0xe9,0x07,0x44,0xe8,0xc6,0x54,0xdb,
                            0xd3,0x53,0xca,0x60,0x6d,0x7d,0x64,0xd0,
                            0x67,0x4c,0x50,0x26,0x54,0x57,0xbf,0xc9,
                            0x80,0xf0,0x03,0x08,0x3c,0x60,0xb8,0x79,
                            0x53,0x9d,0xaa,0x99,0x91,0x91,0xf1,0x3e,
                            0x9a,0x88,0x02,0x16,0x55,0x03,0x93,0xd8,
                            0x07,0xb3,0xdb,0x86,0x20,0x00,0x08,0x20,
                            0x06,0x50,0x46,0x24,0x19,0xef,0xdd,0xab,
                            0xf4,0xdf,0xd7,0x77,0x32,0x30,0x0b,0xdf,
                            0x07,0xe2,0xff,0x50,0x7c,0x1f,0x8c,0x67,
                            0xcd,0x72,0xfb,0x7f,0xfc,0xb8,0x04,0x5e,
                            0xfd,0x20,0x79,0x90,0x3a,0x98,0x1e,0x84,
                            0x19,0x10,0x73,0x40,0x66,0x93,0xe3,0x2e,
                            0x7a,0xe1,0x8c,0x8c,0x0a,0x34,0x37,0x43,
                            0x30,0x48,0x9c,0x9a,0xf6,0x20,0xc2,0x07,
                            0x5b,0x38,0x21,0xc2,0x6b,0x20,0xc2,0x60,
                            0x30,0xbb,0x6d,0x08,0x62,0x80,0x00,0x22,
                            0x5d,0x13,0x24,0x11,0xde,0xff,0x8f,0x9e,
                            0x01,0x97,0x2f,0xb7,0x20,0xcb,0x11,0x9b,
                            0x36,0xe9,0x61,0x44,0x66,0x4f,0x4f,0xc8,
                            0x40,0x07,0x0c,0x5e,0x3c,0x10,0x09,0x0f,
                            0x54,0xf8,0x61,0xb7,0x97,0x76,0x76,0x12,
                            0x8b,0x41,0x71,0x3f,0x58,0xdd,0x36,0x44,
                            0x30,0x40,0x00,0x11,0xaf,0x18,0x54,0x8b,
                            0x61,0x96,0x7e,0xd4,0xab,0xbd,0x20,0x35,
                            0x2c,0xc4,0x5c,0x50,0xa2,0x1b,0x04,0x81,
                            0x83,0x13,0xd3,0xab,0x46,0x44,0xc7,0x83,
                            0x39,0xb1,0x0f,0x66,0xb7,0x0d,0x01,0x0c,
                            0x10,0x40,0xc4,0x29,0x44,0xd4,0x5a,0xff,
                            0x69,0x92,0x09,0x41,0xf8,0xe6,0x4d,0x36,
                            0x78,0x46,0xbf,0x70,0x41,0x60,0xa0,0x03,
                            0x86,0x48,0xb7,0xa2,0x86,0x07,0x48,0x9c,
                            0x96,0xf6,0x0e,0xe6,0xc4,0x3e,0x98,0xdd,
                            0x36,0x04,0x30,0x40,0x00,0x11,0x56,0x04,
                            0xca,0x14,0xd8,0x02,0x99,0x16,0xfd,0x38,
                            0x58,0x46,0x1c,0x04,0x01,0x43,0x10,0x83,
                            0x32,0x1d,0xac,0x99,0x0e,0xa2,0x69,0x9d,
                            0x09,0x11,0xe1,0xf3,0x7f,0x50,0x26,0xf6,
                            0xc1,0xec,0xb6,0x21,0x80,0x01,0x02,0x88,
                            0xf0,0xa8,0x29,0x64,0x74,0x4c,0x01,0x4d,
                            0xf4,0x01,0xc3,0xf1,0xe3,0x96,0x54,0x1f,
                            0xdd,0x84,0x8d,0xc4,0x8d,0x8e,0xb4,0x61,
                            0x07,0xb8,0xe2,0x62,0x30,0x84,0xd7,0x60,
                            0x76,0xdb,0x10,0x00,0x00,0x01,0x84,0x7f,
                            0x42,0xbf,0xb7,0x37,0x04,0xab,0x38,0x68,
                            0x9a,0x61,0xb0,0x4f,0x31,0x8c,0x82,0x51,
                            0x30,0x84,0x00,0x40,0x00,0xb1,0xe0,0x94,
                            0x01,0xcd,0x95,0x41,0x26,0x67,0x15,0xd0,
                            0x64,0x1e,0x30,0xb8,0xb8,0xec,0xa1,0x89,
                            0x6b,0x46,0x4b,0xcf,0x51,0x30,0x42,0x01,
                            0x40,0x00,0xe1,0xae,0x11,0x37,0x6f,0xf6,
                            0x63,0xc0,0x36,0x49,0xeb,0xeb,0xbb,0x85,
                            0x41,0x5f,0xff,0x03,0xed,0x9c,0x34,0x0a,
                            0x46,0xc1,0xc8,0x03,0x00,0x01,0x84,0x3b,
                            0x23,0x22,0x96,0x2a,0xa1,0x82,0xa8,0xa8,
                            0xa5,0xb4,0x72,0x0c,0x45,0x00,0xb4,0xf6,
                            0x92,0x9c,0xf5,0x8d,0xb0,0x75,0x91,0x7e,
                            0x7e,0x93,0xc9,0xb2,0x97,0x9a,0xeb,0x2a,
                            0x61,0xe6,0x80,0xfa,0x5b,0x30,0x0c,0x5a,
                            0x4b,0x0a,0x5a,0x53,0x0a,0x5a,0xc5,0x44,
                            0x6d,0x00,0xb2,0x0f,0x64,0x3e,0xb2,0x7d,
                            0xa0,0xee,0x08,0x2d,0xec,0x82,0x01,0x50,
                            0x3c,0x81,0xec,0x40,0xf7,0xe3,0xe6,0xcd,
                            0x7a,0x54,0x31,0x9f,0xd6,0x7e,0xa2,0x91,
                            0xf9,0x00,0x01,0x84,0x7d,0x14,0x07,0xd7,
                            0x48,0xe9,0x60,0x9a,0x5a,0x00,0xcd,0x35,
                            0x82,0x26,0x92,0x6b,0x6a,0x52,0xd0,0xe6,
                            0x37,0xf1,0x8f,0xd4,0x81,0xf4,0x81,0xa6,
                            0x63,0x40,0x8b,0x06,0x48,0xd1,0x07,0xc3,
                            0xa0,0xf9,0x54,0x4a,0xf4,0x63,0xc3,0x88,
                            0x09,0xf1,0xc3,0x48,0x66,0xde,0xc7,0x11,
                            0x07,0x98,0x71,0x42,0xaa,0x7d,0x08,0xb7,
                            0xa3,0xdb,0x87,0x30,0x13,0x34,0x2a,0x4e,
                            0xca,0x7c,0x2e,0x31,0xa3,0xa6,0x08,0x7b,
                            0xb1,0xfb,0x03,0x84,0xc9,0x9d,0x43,0xa6,
                            0x85,0x9f,0xe8,0x68,0x3e,0x40,0x00,0xe1,
                            0x4b,0x18,0xd4,0x89,0x74,0x6a,0x62,0x72,
                            0x97,0x55,0x51,0xba,0x1c,0x8b,0x56,0xcb,
                            0xb9,0x40,0x53,0x1e,0x90,0x85,0x0c,0x87,
                            0xc1,0x11,0x8d,0x3e,0x05,0x02,0xca,0xf4,
                            0x98,0x2b,0x99,0xc8,0xb7,0x17,0xb1,0x3a,
                            0xe7,0x3e,0x46,0xa2,0x47,0xb8,0xe5,0x3e,
                            0x8a,0xd9,0xc4,0xae,0x98,0x22,0x94,0x11,
                            0x11,0x05,0x26,0x2e,0x7f,0x90,0x6e,0x27,
                            0xad,0xfd,0x44,0x0f,0xf3,0xa1,0x18,0x20,
                            0x80,0xb0,0x4b,0x40,0x72,0x3f,0x75,0x12,
                            0x1b,0x35,0xf1,0x70,0xca,0x88,0x88,0x5a,
                            0x10,0xb2,0x3e,0x17,0x9f,0x5a,0xc4,0xba,
                            0x5c,0xf2,0xed,0x45,0xcd,0x08,0xf7,0x71,
                            0xae,0x07,0xc6,0xb6,0x50,0x81,0x98,0x52,
                            0x1e,0x5f,0x46,0x84,0x15,0x36,0xc4,0xd7,
                            0xf6,0xb8,0xdd,0x47,0x4f,0x3f,0xd1,0xda,
                            0x7c,0x24,0x0c,0x10,0x40,0xd8,0x25,0x90,
                            0x97,0x9b,0x0d,0xa6,0x8c,0x88,0x8c,0x11,
                            0x4b,0xee,0x48,0x73,0x23,0xa5,0x6b,0x36,
                            0xa9,0xb1,0xe6,0x13,0x39,0x82,0x89,0x5d,
                            0x18,0x81,0xbb,0x59,0x47,0xd8,0x5e,0xf4,
                            0xda,0x08,0xdf,0x52,0x3c,0x48,0xa6,0x47,
                            0xb5,0x83,0x18,0x37,0xe2,0x72,0x9b,0xaf,
                            0x6f,0x28,0x18,0xa3,0x27,0x4c,0x50,0xfc,
                            0x61,0xd6,0x26,0xc4,0xdb,0x49,0x6b,0x3f,
                            0xd1,0x23,0xcc,0x90,0x30,0x40,0x00,0x91,
                            0x12,0xa8,0xa4,0x25,0x36,0x7a,0x60,0x72,
                            0x13,0x26,0xa5,0x19,0x89,0x12,0xfd,0xe8,
                            0x89,0x8f,0x94,0x66,0x0c,0x39,0xf6,0x62,
                            0x5b,0x90,0x8d,0xcf,0x4e,0xec,0xe3,0x03,
                            0x84,0xc7,0x06,0x70,0x67,0x44,0xfc,0x09,
                            0x12,0x5f,0xa1,0x8f,0xcb,0x4e,0x5a,0xfb,
                            0x89,0x5e,0x61,0x86,0x84,0x01,0x02,0x88,
                            0x94,0x40,0x25,0x3e,0xb1,0xd1,0x0b,0x0f,
                            0xb5,0x8c,0x88,0x19,0xc1,0xa4,0x0d,0x7e,
                            0x91,0x63,0x2f,0xb6,0x04,0x42,0xa8,0xd9,
                            0x87,0xcd,0x1e,0x42,0xcd,0x67,0x72,0xc3,
                            0x04,0xf7,0xc0,0x20,0x6e,0x3b,0x69,0xed,
                            0x27,0x7a,0x85,0x19,0x12,0x06,0x08,0xa0,
                            0xd1,0xa3,0x32,0xe8,0x09,0x22,0x23,0x97,
                            0x33,0xa0,0xcf,0xcd,0xd2,0x72,0x4e,0x16,
                            0xd7,0x90,0x3a,0x39,0xab,0xa2,0x1e,0x3d,
                            0x92,0xa3,0xd4,0x39,0x58,0x01,0xc8,0xff,
                            0xa0,0x13,0x0e,0xb0,0x81,0x73,0xe7,0x8c,
                            0x30,0xc4,0x68,0xed,0xa7,0x01,0x0a,0x33,
                            0x80,0x00,0x1a,0xcd,0x88,0xf4,0x02,0xa0,
                            0xf9,0x33,0x7a,0x83,0xcb,0x97,0x41,0x73,
                            0x73,0x0a,0x54,0x31,0x0b,0x71,0x0a,0x03,
                            0xf5,0x41,0x68,0xe8,0x2a,0x06,0x6c,0xa7,
                            0x3a,0x20,0x8e,0x04,0x41,0x00,0x5a,0xfb,
                            0x69,0x80,0xc2,0x0c,0x20,0x80,0xb0,0x67,
                            0x44,0xd0,0x5a,0xd2,0x51,0x40,0x5d,0x70,
                            0xea,0x94,0x19,0x03,0xb5,0x22,0x98,0x58,
                            0x40,0xab,0x5a,0x8c,0xda,0xc0,0xc9,0xe9,
                            0x1e,0xd1,0x6a,0x69,0xed,0xa7,0x01,0x0a,
                            0x33,0x80,0x00,0xc2,0x9e,0x11,0xa5,0xa5,
                            0x9f,0xe0,0xd4,0x01,0x3a,0xc4,0x68,0x14,
                            0x90,0x0e,0x06,0x22,0x82,0x69,0x59,0x8b,
                            0x0d,0x14,0xa0,0xb5,0x9f,0x06,0x28,0xcc,
                            0x00,0x02,0x08,0xfb,0xa2,0x6f,0x05,0x85,
                            0x07,0x0c,0x90,0xa6,0x82,0x02,0xba,0x0c,
                            0xc3,0xdd,0xbb,0x2a,0x24,0x95,0x60,0x30,
                            0x80,0x79,0xd8,0x10,0x71,0x60,0xb8,0x2c,
                            0x04,0x1f,0x4c,0x99,0x82,0xf4,0xb8,0x78,
                            0x40,0x0b,0x67,0x10,0x04,0x35,0x35,0xad,
                            0x44,0xab,0xa5,0xb5,0x9f,0x68,0x6c,0x3e,
                            0x40,0x00,0x61,0xaf,0x11,0xf1,0x75,0x4c,
                            0x37,0x6f,0xf6,0x25,0xc9,0x39,0xf8,0x81,
                            0x02,0x0e,0x0c,0x01,0xb3,0x66,0x61,0xf6,
                            0x11,0x46,0x01,0xbd,0xc0,0x03,0x60,0x17,
                            0x25,0x94,0x61,0xd3,0x26,0x7d,0xf0,0xe9,
                            0x74,0x03,0x51,0x20,0x6a,0x6b,0x5f,0xa1,
                            0xb2,0x89,0xb4,0xf6,0x13,0xd9,0xe6,0x03,
                            0x04,0x10,0xee,0x6d,0x50,0xa0,0x7e,0x62,
                            0x49,0xc9,0x6a,0x0c,0x71,0xd0,0x59,0xa5,
                            0x17,0x2f,0xd6,0x92,0x3c,0xda,0x87,0xee,
                            0x28,0xd0,0xe2,0x59,0x6c,0xa3,0x88,0xb8,
                            0xd4,0x8f,0x02,0xea,0x81,0xa1,0x11,0xb6,
                            0x0f,0x18,0x34,0x35,0x6f,0x10,0xad,0x9a,
                            0xd6,0x7e,0xa2,0xb1,0xf9,0x00,0x01,0x84,
                            0x7b,0xd4,0x14,0xb2,0xe7,0xf0,0x01,0x16,
                            0x19,0x05,0x86,0x3d,0x7b,0x5c,0x28,0xb6,
                            0x39,0x22,0xe2,0x04,0xc5,0x66,0x0c,0x07,
                            0x40,0xcb,0xd1,0x54,0x5c,0xd3,0x02,0x43,
                            0x05,0x60,0x2b,0xec,0x69,0xed,0xa7,0x01,
                            0x0a,0x33,0x80,0x00,0xc2,0x9d,0x11,0x41,
                            0x81,0x00,0x19,0x3d,0x7d,0x80,0x21,0x07,
                            0xda,0x22,0x05,0xda,0xfe,0x33,0x0a,0x88,
                            0x07,0xd8,0xfb,0x3b,0x0a,0x0c,0xd7,0xaf,
                            0x6b,0xd0,0xcc,0x4e,0xd8,0x89,0xea,0xe8,
                            0x60,0x20,0xa6,0x52,0x48,0x03,0x0f,0x18,
                            0x96,0x2f,0x8f,0xc4,0x2a,0x43,0x6b,0x3f,
                            0x0d,0x50,0x98,0x01,0x04,0x10,0xfe,0x79,
                            0xc4,0xe2,0xe2,0x35,0x38,0x64,0x14,0xc0,
                            0xc7,0xcd,0x8f,0x02,0xe2,0x01,0xae,0xfe,
                            0xce,0xb2,0x65,0xd1,0x34,0xb3,0x13,0x74,
                            0xa5,0x01,0x66,0x41,0x4a,0xdb,0xcc,0x4f,
                            0x2d,0x80,0xab,0xc5,0x44,0x6b,0x3f,0x0d,
                            0x50,0x98,0x01,0x04,0x10,0xe1,0x09,0x7d,
                            0xd0,0x21,0x51,0xb8,0x26,0x5b,0x41,0x1b,
                            0x56,0x07,0x23,0xc0,0x57,0x5b,0x0f,0xd4,
                            0xf4,0x0b,0xa4,0xbf,0xf3,0x00,0x43,0x1c,
                            0xd4,0xe7,0x26,0xc6,0x4d,0xe4,0x6c,0x9c,
                            0xc5,0x35,0xba,0x4d,0xcb,0xcc,0x4f,0x0e,
                            0x40,0xf5,0xdb,0x03,0x86,0xbd,0x7b,0x9d,
                            0x71,0xaa,0xa5,0xb5,0x9f,0x06,0x28,0xcc,
                            0x00,0x02,0x88,0xb8,0x35,0x8e,0xf8,0x4e,
                            0x72,0x26,0xf7,0x84,0x6f,0xdc,0xeb,0x13,
                            0x89,0x5b,0xa3,0x08,0xc2,0xb8,0x16,0x0c,
                            0xe3,0x5b,0xe3,0x47,0x8d,0x9d,0x25,0xe4,
                            0xae,0xab,0x24,0x67,0x81,0x33,0x08,0xe3,
                            0xde,0x69,0x42,0xd8,0x5e,0xec,0xbb,0x36,
                            0xc8,0xdf,0x80,0x4b,0xed,0x30,0x01,0x61,
                            0xc4,0x81,0xcd,0xc4,0xa5,0x27,0x5a,0xfb,
                            0x89,0x5e,0x61,0x86,0x84,0x01,0x02,0x88,
                            0x78,0xc5,0xb4,0xc8,0x8c,0x94,0x66,0x44,
                            0x6c,0xdb,0x4f,0x70,0xb9,0x09,0xfb,0x7d,
                            0x1d,0xa4,0xdb,0x89,0xdb,0xdd,0x84,0xf5,
                            0xe3,0xde,0x42,0x85,0xdd,0xcd,0xa0,0x8d,
                            0xa7,0xf8,0x77,0xb5,0x13,0x67,0x2f,0x36,
                            0xfd,0xd4,0x3e,0x97,0x96,0xb2,0x45,0xdf,
                            0x87,0x49,0x4e,0xe8,0xb4,0xf6,0x13,0x3d,
                            0xc2,0x0c,0x09,0x03,0x04,0x10,0x69,0x1a,
                            0x40,0x81,0x86,0x3d,0x31,0xdf,0x27,0xf9,
                            0xb8,0x79,0x42,0xc7,0x26,0x10,0xeb,0x1e,
                            0x7c,0x66,0xa0,0x63,0x90,0xdb,0xf1,0x1d,
                            0x03,0x42,0x4c,0x81,0x42,0x69,0x81,0x84,
                            0x7f,0xa7,0x3a,0xa6,0x9b,0x61,0xee,0xc6,
                            0x7e,0xba,0x38,0x44,0x0f,0xa1,0x55,0xfe,
                            0x94,0x9e,0xd4,0x4e,0xcc,0x26,0x5d,0xf2,
                            0xb7,0x41,0x85,0x82,0xc3,0x84,0x94,0xb4,
                            0x43,0x0f,0x3f,0xd1,0x23,0xcc,0x90,0x30,
                            0x40,0x00,0x91,0xe6,0x79,0x18,0x46,0xde,
                            0x5d,0x8e,0x9e,0x88,0x40,0x19,0x12,0x97,
                            0x23,0x40,0x99,0x00,0xf3,0xac,0x97,0xff,
                            0x58,0xcd,0x20,0xd6,0x2d,0xc4,0x1d,0xc1,
                            0x70,0x1f,0x25,0xb2,0x89,0xcd,0x08,0xa8,
                            0xea,0xef,0xe3,0x71,0x37,0x7e,0xfd,0xe4,
                            0xb9,0xf9,0x3f,0x46,0x26,0x23,0xd5,0xdd,
                            0xd8,0xe3,0x0c,0x53,0x1f,0xb6,0x02,0x04,
                            0x94,0xf1,0x61,0x7a,0x88,0xb9,0x14,0x08,
                            0x9f,0xdb,0xb0,0x9d,0xe5,0x02,0x29,0xd4,
                            0x43,0x81,0x76,0xf0,0x90,0x95,0x06,0xe9,
                            0xe1,0x27,0x5a,0x9b,0x8f,0x84,0x01,0x02,
                            0x88,0xbc,0x00,0x40,0x76,0x28,0xa2,0x86,
                            0xbc,0x8f,0xe1,0x58,0xec,0x18,0xd3,0x53,
                            0x20,0x33,0x88,0xb9,0xce,0x0d,0x17,0x26,
                            0x54,0xbb,0xa2,0x97,0xb8,0xa8,0x6e,0xc6,
                            0x8d,0x51,0x13,0x19,0xe9,0x98,0xb8,0x48,
                            0xc6,0xe6,0x6e,0x44,0xed,0x4d,0x89,0xbb,
                            0xb1,0x25,0x14,0xec,0x67,0xe0,0xe0,0x36,
                            0x8b,0xd8,0x1a,0x00,0x59,0x3d,0xbe,0xb8,
                            0x86,0xe0,0xc3,0xe0,0x4c,0x48,0x8d,0x83,
                            0xc8,0x68,0xe9,0x27,0x7a,0x98,0x0f,0xc5,
                            0x00,0x01,0xc4,0x08,0xce,0x8d,0xd4,0x00,
                            0xa0,0x91,0x3f,0xd0,0x3a,0xd4,0x4f,0x9f,
                            0xf8,0x70,0xde,0x16,0x0c,0x9a,0x4b,0x13,
                            0x10,0xf8,0xc0,0xc0,0xc7,0xf7,0x89,0x41,
                            0x59,0xf9,0x0e,0x83,0x8c,0xcc,0x13,0xaa,
                            0x5d,0xec,0x09,0x9a,0xe7,0x01,0xed,0x70,
                            0x00,0x2d,0xc1,0x83,0xd9,0x0d,0x9a,0x07,
                            0x05,0x0d,0x47,0x93,0xb3,0x36,0x96,0x5e,
                            0x00,0xb4,0xc2,0xe8,0xea,0x55,0x1d,0xf8,
                            0x5a,0x54,0xd0,0x84,0xb2,0xbd,0xfd,0x01,
                            0x9a,0x2e,0x78,0x00,0x85,0x15,0x68,0x38,
                            0x1e,0xd9,0x5e,0x18,0x80,0xc5,0x11,0x28,
                            0xdc,0x48,0x89,0x1f,0xd0,0x91,0x82,0xa0,
                            0x78,0x4d,0x4d,0xdd,0x05,0xe6,0x83,0xf6,
                            0xf5,0x81,0xb6,0x14,0x81,0x16,0xbb,0x23,
                            0xdb,0x01,0x32,0xdf,0xcc,0xec,0x14,0x83,
                            0xaf,0xef,0x25,0xea,0x79,0x88,0x81,0x36,
                            0x7e,0xa2,0xa3,0xf9,0x00,0x01,0x44,0xbd,
                            0x8c,0x38,0x0a,0x46,0xc1,0x28,0x20,0x1b,
                            0x00,0x04,0xd0,0xe8,0xc6,0xe0,0x51,0x30,
                            0x0a,0x06,0x01,0x00,0x08,0xa0,0xd1,0x8c,
                            0x38,0x0a,0x46,0xc1,0x20,0x00,0x00,0x01,
                            0x34,0x9a,0x11,0x47,0xc1,0x28,0x18,0x04,
                            0x00,0x20,0x80,0x46,0x33,0xe2,0x28,0x18,
                            0x05,0x83,0x00,0x00,0x04,0xd0,0x68,0x46,
                            0x1c,0x05,0xa3,0x60,0x10,0x00,0x80,0x00,
                            0x1a,0xcd,0x88,0xa3,0x60,0x14,0x0c,0x02,
                            0x00,0x10,0x40,0xa3,0x19,0x71,0x14,0x8c,
                            0x82,0x41,0x00,0x00,0x02,0x68,0x34,0x23,
                            0x8e,0x82,0x51,0x30,0x08,0x00,0x40,0x00,
                            0x8d,0x66,0xc4,0x51,0x30,0x0a,0x06,0x01,
                            0x00,0x08,0xa0,0xd1,0x8c,0x38,0x0a,0x46,
                            0xc1,0x20,0x00,0x00,0x01,0x06,0x00,0xa6,
                            0xcd,0x81,0x26,0x7d,0xe2,0x03,0x5d,0x00,
                            0x00,0x00,0x00,0x49,0x45,0x4e,0x44,0xae,
                            0x42,0x60,0x82

        };

        /// <summary>
        /// the map tile is downloading image.
        /// </summary>
        private static IImage _imageLogo;
        /**
         * Intialized the images.
         */
        static RasterMap()
        {
            try
            {
                _imageLogo = GetAbstractGraphicsFactory()
                        .CreateImage(ImageGuidebeeLogoArray, 0, ImageGuidebeeLogoArray.Length);

            }
            catch (Exception)
            {

            }

        }

        private class SetCenterThread
        {

            internal readonly AutoResetEvent _panSyncObject = new AutoResetEvent(false);


            public void Run()
            {
                Log.P(Thread.CurrentThread.Name + " thread started");
                while (!_rasterMap._panStopThread)
                {
                    _panSyncObject.WaitOne();
                    _rasterMap.SetCenter(_rasterMap.GetScreenCenter(), _rasterMap.mapZoomLevel, _rasterMap.typeOfMap);
                    

                }
                Log.P(Thread.CurrentThread.Name + " thread stopped");
            }

            public void SetCenterCommand()
            {
                _panSyncObject.Set();
            }
        }

        private class UpdateMapThread
        {

            internal readonly AutoResetEvent _panSyncObject = new AutoResetEvent(false);



            public void Run()
            {
                Log.P(Thread.CurrentThread.Name + " thread started");
                while (!_rasterMap._panStopThread)
                {
                    _panSyncObject.WaitOne();
                    _rasterMap._mapTileEngine.DrawUpdatedMapCanvas();

                }
                Log.P(Thread.CurrentThread.Name + " thread stopped");
            }

            public void UpdateMapCommand()
            {
                _panSyncObject.Set();
            }
        }
    }
}

