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
using Mapdigit.Drawing;
using Mapdigit.Drawing.Geometry;
using Mapdigit.Gis.Drawing;
using Mapdigit.Gis.Geometry;

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
    /// Base map tile Engine.
    /// </summary>
    internal abstract class MapTileEngine
    {
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// the screen offset X
        /// </summary>
        /// <returns>screen offset x</returns>
        public virtual int GetScreenOffsetX()
        {
            return screenOffsetX;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the screen offset Y.
        /// </summary>
        /// <returns>screen offset y</returns>
        public virtual int GetScreenOffsetY()
        {
            return screenOffsetY;
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
        public virtual void ZoomIn()
        {
            if (mapDrawingListener != null)
            {
                mapDrawingListener.Done();
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
        public virtual void ZoomOut()
        {
            if (mapDrawingListener != null)
            {
                mapDrawingListener.Done();
            }
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
        public virtual void PanDirection(int dx, int dy)
        {
            screenOffsetX -= dx;
            screenOffsetY -= dy;
            if (mapDrawingListener != null)
            {
                mapDrawingListener.Done();
            }
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
        public virtual void SetScreenSize(int width, int height)
        {
            screenOffsetX = (mapSize.Width - width) / 2;
            screenOffsetY = (mapSize.Height - height) / 2;
            screenRectangle.Width = width;
            screenRectangle.Height = height;
            screenRectangle.X = screenRectangle.Y = 0;
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
            lock (syncObject)
            {
                _mapTileDownloadManager.SetRoutePen(routePen);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the map drawing listener.
        /// </summary>
        /// <param name="listener">The map drawing listener.</param>
        public void SetMapDrawingListener(IMapDrawingListener listener)
        {
            mapDrawingListener = listener;
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
            return _mapTileDownloadManager;
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
        /// <param name="latlng">The latlng.</param>
        /// <returns>x,y coordniate on screen</returns>
        public GeoPoint FromLatLngToScreenPixel(GeoLatLng latlng)
        {
            lock (syncObject)
            {
                GeoPoint pt = _rasterMap.FromLatLngToMapPixel(latlng);
                pt.X -= GetScreenOffsetX();
                pt.Y -= GetScreenOffsetY();
                return pt;
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
        /// <param name="pt">(x,y) coordinates on screen</param>
        /// <returns>the latitude,longitude position</returns>
        public GeoLatLng FromScreenPixelToLatLng(GeoPoint pt)
        {
            lock (syncObject)
            {
                GeoPoint pt1 = new GeoPoint(pt);
                pt1.X += GetScreenOffsetX();
                pt1.Y += GetScreenOffsetY();
                return _rasterMap.FromMapPixelToLatLng(pt1);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// return the latitude,longitude of the screen center.
        /// </summary>
        /// <returns>the center of the screen in latitude,longititude pair.</returns>
        public virtual GeoLatLng GetScreenCenter()
        {
            lock (syncObject)
            {
                GeoPoint pt = new GeoPoint(GetScreenOffsetX()
                        + screenRectangle.Width / 2,
                        GetScreenOffsetY()
                        + screenRectangle.Height / 2);
                return _rasterMap.FromMapPixelToLatLng(pt);
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
        public abstract void ClearMapCache();

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Restores the map cache.
        /// </summary>
        public abstract void RestoreMapCache();

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Saves the map cache.
        /// </summary>
        public abstract void SaveMapCache();

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
        public abstract void Paint(IGraphics graphics);


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
        public abstract void Paint(IGraphics graphics, int offsetX, int offsetY);

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
        public abstract void SetMapDirection(MapDirection newDirection);

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
        public abstract void SetMapDirections(MapDirection[] newDirections);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Clears the map direction.
        /// </summary>
        public abstract void ClearMapDirection();

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
        public abstract void SetDownloadManager(MapTileDownloadManager mapTileDownloadManager);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the map canvas.
        /// </summary>
        public abstract void DrawMapCanvas();

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the updated map canvas.
        /// </summary>
        public abstract void DrawUpdatedMapCanvas();

        /**
         * Map drawing  listener.
         */
        protected IMapDrawingListener mapDrawingListener;
        /**
         * Map tiles downloader
         */
        internal MapTileDownloadManager _mapTileDownloadManager;
        protected Rectangle mapSize = new Rectangle();
        protected Rectangle screenRectangle;
        protected int screenOffsetX;
        protected int screenOffsetY;
        protected object syncObject;
        protected Rectangle mapRectangle;
        internal RasterMap _rasterMap;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapTileEngine"/> class.
        /// </summary>
        /// <param name="width">The width of the map.</param>
        /// <param name="height">The height of the map.</param>
        /// <param name="mapTileDownloadManager">The map tile download manager.</param>
        /// <param name="rasterMap">The raster map.</param>
        protected MapTileEngine(int width, int height,
                MapTileDownloadManager mapTileDownloadManager, RasterMap rasterMap)
        {
            _rasterMap = rasterMap;
            syncObject = rasterMap.GetSyncRoot();
            screenRectangle = new Rectangle(0, 0, rasterMap.GetScreenWidth(),
                    rasterMap.GetScreenHeight());
            screenOffsetX = (width - screenRectangle.Width) / 2;
            screenOffsetY = (height - screenRectangle.Height) / 2;
            mapSize.X = 0;
            mapSize.Y = 0;
            mapSize.Width = width;
            mapSize.Height = height;
            mapRectangle = new Rectangle(0, 0, width, height);
            this._mapTileDownloadManager = mapTileDownloadManager;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Needs to get new map image.
        /// </summary>
        /// <returns></returns>
        protected bool NeedToGetNewMapImage()
        {
            screenRectangle.X=screenOffsetX;
            screenRectangle.Y=screenOffsetY;
            return !mapRectangle.Contains(screenRectangle);
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
        internal virtual void Start()
        {

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
        internal virtual void Stop()
        {

        }

    }
}
