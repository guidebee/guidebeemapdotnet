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
using System;
using System.Threading;
using Mapdigit.Drawing.Geometry;
using Mapdigit.Gis.Drawing;
using Mapdigit.Gis.Geometry;
using Mapdigit.Util;

namespace Mapdigit.Gis.Raster
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 28SEP2010  James Shen                 	          Initial Creation
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Array tiled Map Tile Engine.
    /// </summary>
    sealed internal class ArrayTiledMapTileEngine : TiledMapTileEngine
    {
   
        //[-------------------------- MAIN CLASS ----------------------------------]
        ////////////////////////////////////////////////////////////////////////////
        //----------------------------- REVISIONS ----------------------------------
        // Date       Name                 Tracking #         Description
        // --------   -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayTiledMapTileEngine"/> class.
        /// </summary>
        /// <param name="width">width of the map</param>
        /// <param name="height">height of the map</param>
        /// <param name="mapTileDownloadManager">map tile downloader manager</param>
        /// <param name="rasterMap">raster map instance</param>
        public ArrayTiledMapTileEngine(int width, int height,
            MapTileDownloadManager mapTileDownloadManager, RasterMap rasterMap)
            : base(width, height, mapTileDownloadManager, rasterMap)
        {
            _mapTileEngine = this;
            if (mapTileDownloadManager != null)
            {
                _mapTileDownloadManager = mapTileDownloadManager;
                _mapTileDownloadManager._mapPanThread = null;
                _mapTileDownloadManager._mapTileReadyListener = mapTileReadyListener;
                _mapTileDownloadManager._rasterMap = rasterMap;
            }
            _viewRect.X = _viewRect.Y = 0;
            _updateCanvasTask = delegate()
            {
                _mapTileEngine.ForceUpdateMapTiles(false);
            };
            _countDownTimer = new DelayUpdateCanvasTimer(5, _updateCanvasTask);
            _viewRect.SetSize(screenRectangle.Width, screenRectangle.Height);
            _mapRect.SetSize(MapLayer.MapTileWidth, MapLayer.MapTileWidth);
            
            
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
            base.SetScreenSize(width, height);
            _viewRect.SetSize(screenRectangle.Width, screenRectangle.Height);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// pan direction
        /// </summary>
        /// <param name="dx">The dx.</param>
        /// <param name="dy">The dy.</param>
        public override void PanDirection(int dx, int dy)
        {
            _viewRect.X -= dx;
            _viewRect.Y -= dy;
            _countDownTimer.Reset();
            base.PanDirection(dx, dy);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// paint map to given graphics canvas.
        /// </summary>
        /// <param name="graphics">The graphics.</param>
        public override void Paint(IGraphics graphics)
        {
            Paint(graphics, 0, 0);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// paint map to given canvas at given location.
        /// </summary>
        /// <param name="graphics">The graphics.</param>
        /// <param name="offsetX">The offset X.</param>
        /// <param name="offsetY">The offset Y.</param>
        public override void Paint(IGraphics graphics, int offsetX, int offsetY)
        {
            PaintInternal(graphics, offsetX, offsetY);
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
        public override void DrawMapCanvas()
        {
            Log.P("drawMapCanvas");
            //clearAllMapTiles();
            int mapZoomLevel = _rasterMap.GetZoom();
            GeoPoint center = MapLayer.FromLatLngToPixel(_rasterMap.GetCenter(),
                    mapZoomLevel);
            _viewRect.X = (int)center.X - screenRectangle.Width / 2;
            _viewRect.Y = (int)center.Y - screenRectangle.Height / 2;
            screenOffsetX = (_rasterMap.GetMapWidth()
                    - _rasterMap.GetScreenWidth()) / 2;
            screenOffsetY = (_rasterMap.GetMapHeight()
                    - _rasterMap.GetScreenHeight()) / 2;
            if (mapDrawingListener != null)
            {
                mapDrawingListener.Done();
            }
            ForceUpdateMapTiles(false);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the updated map canvas.
        /// </summary>
        public override void DrawUpdatedMapCanvas()
        {
            Log.P("drawUpdatedMapCanvas");
            _countDownTimer.Reset();
            if (mapDrawingListener != null)
            {
                mapDrawingListener.Done();
            }
        }

        /**
         * safe guard check period
         */
        private const int SafeGuardPeriod = 10000;
        /**
         * view rectangle.
         */
        private readonly Rectangle _viewRect = new Rectangle();
        /**
         * temp variable avoid create the rectangle again and again.
         */
        private readonly Rectangle _mapRect = new Rectangle();

        private readonly DelayUpdateCanvasTimer _countDownTimer;

        private volatile bool _stopThread;

        private readonly AutoResetEvent _safeGuardWakeupObject = new AutoResetEvent(false);

        private static int NormalizedValue(int value, int max)
        {
            int ret = value % max;
            if (ret < 0) ret += max;
            return ret;
        }

        private void ForceUpdateMapTiles(bool forceUpdate)
        {
            Log.P("ForceUpdateMapTiles");
            //lock(viewRect)
            {
                Rectangle drawRect = new Rectangle(_viewRect);
                GeoPoint topLeft = new GeoPoint();
                GeoPoint bottomRight = new GeoPoint();
                int mapType = _rasterMap.GetMapType();
                int mapZoomLevel = _rasterMap.GetZoom();
                topLeft.X = (double)drawRect.X / MapLayer.MapTileWidth;
                topLeft.Y = (double)drawRect.Y / MapLayer.MapTileWidth;
                bottomRight.X = (double)(drawRect.X + drawRect.Width)
                        / MapLayer.MapTileWidth;
                bottomRight.Y = (double)(drawRect.Y + drawRect.Height)
                        / MapLayer.MapTileWidth;
                int x1 = (int)topLeft.X;
                int y1 = (int)topLeft.Y;
                int x2 = (int)bottomRight.X;
                int y2 = (int)bottomRight.Y;
                int maxTile = (int)(MathEx.Pow(2, mapZoomLevel) + 0.5);
                if (x1 * MapLayer.MapTileWidth > drawRect.X) x1 -= 1;
                if (y1 * MapLayer.MapTileWidth > drawRect.Y) y1 -= 1;
                if (x2 * MapLayer.MapTileWidth < drawRect.X + drawRect.Width) x2 += 1;
                if (y2 * MapLayer.MapTileWidth < drawRect.Y + drawRect.Height) y2 += 1;
                bool needUpdate = false;
                for (int xIndex = x1; xIndex < x2; xIndex++)
                {
                    for (int yIndex = y1; yIndex < y2; yIndex++)
                    {
                        int x = NormalizedValue(xIndex, maxTile);
                        int y = NormalizedValue(yIndex, maxTile);
                        _mapRect.X = x * MapLayer.MapTileWidth;
                        _mapRect.Y = y * MapLayer.MapTileWidth;
                        ImageTileIndex imageTileIndex = new ImageTileIndex();
                        imageTileIndex.MapType = mapType;
                        imageTileIndex.XIndex = x;
                        imageTileIndex.YIndex = y;
                        imageTileIndex.MapZoomLevel = mapZoomLevel;
                        if (_mapRect.Intersects(drawRect) && x >= 0 && y >= 0)
                        {
                            int tileIndex = GetAvaiableMapTileIndex(imageTileIndex);
                            if (tileIndex > tileDownloading)
                            {
                                if (mapTileImages[tileIndex].NeedToUpdate())
                                {
                                    mapTileImages[tileIndex].UpdateMapCanvas();
                                    needUpdate = true;
                                }
                            }
                        }
                    }
                }
                if (needUpdate || forceUpdate)
                {
                    if (mapDrawingListener != null)
                    {
                        mapDrawingListener.Done();
                    }
                }
            }
        }

        private readonly ThreadStart _updateCanvasTask;

        private static ArrayTiledMapTileEngine _mapTileEngine;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the map image.
        /// </summary>
        /// <param name="graphics">The graphics.</param>
        /// <param name="offsetX">The offset X.</param>
        /// <param name="offsetY">The offset Y.</param>
        private void DrawMapImage(IGraphics graphics, int offsetX,
                 int offsetY)
        {

            GeoPoint topLeft = new GeoPoint();
            GeoPoint bottomRight = new GeoPoint();
            int mapType = _rasterMap.GetMapType();
            int mapZoomLevel = _rasterMap.GetZoom();
            topLeft.X = (double)_viewRect.X / MapLayer.MapTileWidth;
            topLeft.Y = (double)_viewRect.Y / MapLayer.MapTileWidth;
            bottomRight.X = (double)(_viewRect.X + _viewRect.Width)
                    / MapLayer.MapTileWidth;
            bottomRight.Y = (double)(_viewRect.Y + _viewRect.Height)
                    / MapLayer.MapTileWidth;
            int x1 = (int)topLeft.X;
            int y1 = (int)topLeft.Y;
            int x2 = (int)bottomRight.X;
            int y2 = (int)bottomRight.Y;
            int maxTile = (int)(MathEx.Pow(2, mapZoomLevel) + 0.5);
            if (x1 * MapLayer.MapTileWidth > _viewRect.X) x1 -= 1;
            if (y1 * MapLayer.MapTileWidth > _viewRect.Y) y1 -= 1;
            if (x2 * MapLayer.MapTileWidth < _viewRect.X + _viewRect.Width) x2 += 1;
            if (y2 * MapLayer.MapTileWidth < _viewRect.Y + _viewRect.Height) y2 += 1;
            if (x1 < 0 || y1 < 0 || x2 > maxTile || y2 > maxTile)
            {
                ClearBackground(graphics, 0X7FFFFFFF);
            }
            for (int xIndex = x1; xIndex < x2; xIndex++)
            {
                for (int yIndex = y1; yIndex < y2; yIndex++)
                {
                    int x = NormalizedValue(xIndex, maxTile);
                    int y = NormalizedValue(yIndex, maxTile);
                    _mapRect.X = x * MapLayer.MapTileWidth + offsetX;
                    _mapRect.Y = y * MapLayer.MapTileWidth + offsetY;
                    ImageTileIndex imageTileIndex = new ImageTileIndex();
                    imageTileIndex.MapType = mapType;
                    imageTileIndex.XIndex = x;
                    imageTileIndex.YIndex = y;
                    imageTileIndex.MapZoomLevel = mapZoomLevel;
                    if (_mapRect.Intersects(_viewRect))
                    {
                        int tileIndex = GetAvaiableMapTileIndex(imageTileIndex);
                        if (tileIndex > tileDownloading)
                        {
                            graphics.DrawImage(mapTileImages[tileIndex]._mapImage,
                                    _mapRect.X - _viewRect.X,
                                    _mapRect.Y - _viewRect.Y);
                        }
                        else
                        {
                            graphics.DrawImage(imageDownloading,
                                     _mapRect.X - _viewRect.X,
                                     _mapRect.Y - _viewRect.Y);
                        }
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
        /// Clears the background.
        /// </summary>
        /// <param name="graphics">The graphics.</param>
        /// <param name="color">The color.</param>
        private void ClearBackground(IGraphics graphics, int color)
        {
            graphics.SetColor(color);
            graphics.SetClip(0, 0, _rasterMap.GetScreenWidth(),
                    _rasterMap.GetScreenHeight());
            graphics.FillRect(0, 0, _rasterMap.GetScreenWidth(),
                    _rasterMap.GetScreenHeight());
        }
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 03SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /**
         * paint internal.
         * @param graphics graphics object.
         * @param offsetX  offset x
         * @param offsetY  offset y
         */
        private void PaintInternal(IGraphics graphics, int offsetX, int offsetY)
        {
            //ClearBackground(graphics,0xFFFFFFFF);
            DrawMapImage(graphics, offsetX, offsetY);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Starts this instance.
        /// </summary>
        internal override void Start()
        {
            SafeguardThread safeguardThread = new SafeguardThread();
            Thread thread = new Thread(safeguardThread.Run);
            thread.Name = "SafeguardThread";
            thread.Start();
            _countDownTimer.Start();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Stops this instance.
        /// </summary>
        internal override void Stop()
        {
            _countDownTimer.Stop();
            _safeGuardWakeupObject.Set();
        }

        class SafeguardThread
        {

            public void Run()
            {
                Log.P(Thread.CurrentThread.Name + " thread started");
                while (!_mapTileEngine._stopThread)
                {
                    _mapTileEngine._safeGuardWakeupObject.WaitOne(SafeGuardPeriod, false);
                    _mapTileEngine.ForceUpdateMapTiles(true);
                    
                }
                Log.P(Thread.CurrentThread.Name + " thread stopped");
            }
        }

        class DelayUpdateCanvasTimer
        {

            private readonly int _period;
            private readonly ThreadStart _task;
            private Thread _thread;
            private readonly AutoResetEvent _wakeupObject = new AutoResetEvent(false);
            private volatile int _counter;


            public DelayUpdateCanvasTimer(int period, ThreadStart runner)
            {
                _period = period;
                _task = runner;
            }

            public void Reset()
            {
                _wakeupObject.Set();
                _counter = 0;
            }

            public void Stop()
            {
                _mapTileEngine._stopThread = true;
                Reset();
                if (_thread != null)
                {
                    try
                    {
                        _thread.Join();
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            public void Start()
            {
                _mapTileEngine._stopThread = false;
                _thread = new Thread(Run);
                _thread.Name = "DelayUpdateCanvasTimer";
                _thread.Start();

            }

            private void Run()
            {
                Log.P(Thread.CurrentThread.Name + " thread started");
                while (!_mapTileEngine._stopThread)
                {
                    _wakeupObject.WaitOne();
                    if (_task != null && !_mapTileEngine._stopThread)
                    {
                        while (_counter < _period)
                        {
                            _counter++;
                            try
                            {
                                Thread.Sleep(100);
                            }
                            catch (Exception)
                            {

                            }

                        }
                        try
                        {
                            _task();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                Log.P(Thread.CurrentThread.Name + " thread stopped");
            }
        }
    }
}
