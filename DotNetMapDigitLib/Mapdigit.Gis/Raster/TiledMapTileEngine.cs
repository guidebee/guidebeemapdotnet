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
using System.Collections;
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
    // 28SEP2010  James Shen                 	          Initial Creation
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Base tiled Map Tile Engine.
    /// </summary>
    internal abstract class TiledMapTileEngine : MapTileEngine
    {
        protected TiledMapTileEngine(int width, int height,
            MapTileDownloadManager mapTileDownloadManager, RasterMap rasterMap)
            : base(width, height, mapTileDownloadManager, rasterMap)
        {
           
            //create a dowloading image.
            IImage imgDownloading = MapTileDownloadManager.TileDownloading;
            imageDownloading = MapLayer.GetAbstractGraphicsFactory()
                    .CreateImage(MapLayer.MapTileWidth,
                    MapLayer.MapTileWidth);
            IGraphics graphics = this.imageDownloading.GetGraphics();
            if (imgDownloading != null)
            {
                int xIndex, yIndex;
                int tileWidth = imgDownloading.GetHeight();
                int count = MapLayer.MapTileWidth / tileWidth;
                for (xIndex = 0; xIndex < count; xIndex++)
                {
                    for (yIndex = 0;
                            yIndex <= count;
                            yIndex++)
                    {
                        graphics.DrawImage(imgDownloading,
                                xIndex * tileWidth, yIndex * tileWidth);
                    }
                }
            }
            int howManyPhysicalTiles = (width / MapLayer.MapTileWidth)
                    * (height / MapLayer.MapTileWidth);
            if (howManyPhysicalTiles < 4)
            {
                howManyPhysicalTiles = 4;
            }
            mapTileImages = new MapTile[howManyPhysicalTiles];
            for (int i = 0; i < howManyPhysicalTiles; i++)
            {
                mapTileImages[i] = new MapTile(this);
            }
            mapTileReadyListener = new MapTileReadyListenerImpl(this);

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
        public override void ClearMapCache()
        {
            lock (syncObject)
            {
                _mapTileDownloadManager.ClearMapCache();
            }
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
        public override void ClearMapDirection()
        {
            lock (syncObject)
            {
                _mapTileDownloadManager.SetMapDirection(null);
                _rasterMap.SetCenter(_rasterMap.GetScreenCenter(), _rasterMap.GetZoom());
                GC.Collect();
            }
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
        public override void RestoreMapCache()
        {
            lock (syncObject)
            {
                _mapTileDownloadManager.RestoreMapCache();
            }
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
        public override void SaveMapCache()
        {
            lock (syncObject)
            {
                _mapTileDownloadManager.SaveMapCache();
            }
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
        public override void SetMapDirections(MapDirection[] newDirections)
        {
            lock (syncObject)
            {
                _mapTileDownloadManager.SetMapDirections(newDirections);
                if (newDirections != null)
                {
                }
                else
                {
                    ClearMapDirection();
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
        /// Sets the map direction.
        /// </summary>
        /// <param name="newDirection">The new direction.</param>
        public override void SetMapDirection(MapDirection newDirection)
        {
            lock (syncObject)
            {
                _mapTileDownloadManager.SetMapDirection(newDirection);
                if (newDirection != null)
                {
                }
                else
                {
                    ClearMapDirection();
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
        /// Sets the download manager.
        /// </summary>
        /// <param name="downloadManager">The map tile download manager.</param>
        public override void SetDownloadManager(MapTileDownloadManager downloadManager)
        {
            if (downloadManager != null)
            {
                _mapTileDownloadManager = downloadManager;
                _mapTileDownloadManager._mapTileReadyListener = mapTileReadyListener;
                _mapTileDownloadManager._mapPanThread = null;
                _mapTileDownloadManager._rasterMap = _rasterMap;
            }
        }

        /**
         * the downloding image.
         */
        protected IImage imageDownloading;
        /**
         * tile download image index.
         */
        protected int tileDownloading = -1;

        /**
         * map tile array.
         */
        protected MapTile[] mapTileImages;

        /**
         * update one map tile handler
         */
        protected IMapTileReadyListener mapTileReadyListener;

        class MapTileReadyListenerImpl:IMapTileReadyListener
        {
            private readonly TiledMapTileEngine _tiledMapTileEngine;

            public MapTileReadyListenerImpl( TiledMapTileEngine tiledMapTileEngine)
            {
                _tiledMapTileEngine = tiledMapTileEngine;
            }
            public void Done(ImageTileIndex imageTileIndex, IImage image)
            {
                try {
                    int zoomLevel = _tiledMapTileEngine._rasterMap.GetZoom();
                    //ingnore the image not same with current map zoom Level.
                    if (zoomLevel == imageTileIndex.MapZoomLevel) {
                        _tiledMapTileEngine.DrawMapTileInMapCanvas(imageTileIndex, image);
                    }
                } catch (Exception ) {
                   
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
        /// draw map cavns with given tile image.
        /// </summary>
        /// <param name="imageTileIndex">Index of the image tile.</param>
        /// <param name="image">The image.</param>
        protected void DrawMapTileInMapCanvas(ImageTileIndex imageTileIndex,
                IImage image)
        {
            ImageTileIndex imageTileIndex1 = new ImageTileIndex();
            imageTileIndex1.MapType = _rasterMap.GetMapType();
            imageTileIndex1.XIndex = imageTileIndex.XIndex;
            imageTileIndex1.YIndex = imageTileIndex.YIndex;
            imageTileIndex1.MapZoomLevel = imageTileIndex.MapZoomLevel;
            int tileIndex = GetAvaiableMapTileIndex(imageTileIndex1);
            if (tileIndex > tileDownloading)
            {
                mapTileImages[tileIndex].DrawMapTileImage(imageTileIndex, image);
                if (mapDrawingListener != null)
                {
                    mapDrawingListener.Done();
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
        /// Clears all map tiles.
        /// </summary>
        protected void ClearAllMapTiles()
        {
            lock (syncObject)
            {
                //first check the array if already has the image.
                for (int i = 0; i < mapTileImages.Length; i++)
                {
                    mapTileImages[i].ClearMapCanvas();
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
        /// Gets the index of the avaiable map tile.
        /// </summary>
        /// <param name="imageTileIndex">Index of the image tile.</param>
        /// <returns></returns>
        protected int GetAvaiableMapTileIndex(ImageTileIndex imageTileIndex)
        {
            lock (syncObject)
            {
                //first check the array if already has the image.
                for (int i = 0; i < mapTileImages.Length; i++)
                {
                    if (!mapTileImages[i]._avaiable
                            && mapTileImages[i]._mapTileIndex.MapType
                                    == imageTileIndex.MapType
                            && mapTileImages[i]._mapTileIndex.XIndex
                                    == imageTileIndex.XIndex
                            && mapTileImages[i]._mapTileIndex.YIndex
                                    == imageTileIndex.YIndex
                            && mapTileImages[i]._mapTileIndex.MapZoomLevel
                                    == imageTileIndex.MapZoomLevel)
                    {
                        return i;
                    }
                }
                //second check if there's a free image.
                for (int i = 0; i < mapTileImages.Length; i++)
                {
                    if (mapTileImages[i]._avaiable
                            || mapTileImages[i]._mapTileIndex.MapZoomLevel
                            != imageTileIndex.MapZoomLevel)
                    {
                        mapTileImages[i].SetMapTileIndex(imageTileIndex);
                        return i;
                    }
                }

                //calculate the max distance.
                int[] distances = new int[mapTileImages.Length];
                for (int i = 0; i < mapTileImages.Length; i++)
                {
                    distances[i] = (mapTileImages[i]._mapTileIndex.XIndex
                            - imageTileIndex.XIndex) *
                            (mapTileImages[i]._mapTileIndex.XIndex
                            - imageTileIndex.XIndex)
                            + (mapTileImages[i]._mapTileIndex.YIndex
                            - imageTileIndex.YIndex) *
                            (mapTileImages[i]._mapTileIndex.YIndex
                            - imageTileIndex.YIndex);
                }
                int maxValue = distances[0];
                for (int i = 1; i < distances.Length; i++)
                {
                    if (maxValue < distances[i])
                    {
                        maxValue = distances[i];
                    }
                }

                //now get the index which has the longest distance
                for (int i = 0; i < distances.Length; i++)
                {
                    if (maxValue == distances[i])
                    {
                        mapTileImages[i].SetMapTileIndex(imageTileIndex);
                        return i;
                    }
                }

                return tileDownloading;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the cached image.
        /// </summary>
        /// <param name="mtype">The mtype.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="zoomLevel">The zoom level.</param>
        /// <returns></returns>
        protected IImage GetCachedImage(int mtype, int x, int y, int zoomLevel)
        {
            return _mapTileDownloadManager.GetCachedImage(mtype, x, y,
                    zoomLevel);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <param name="mtype">The mtype.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="zoomLevel">The zoom level.</param>
        /// <returns></returns>
        protected IImage GetImage(int mtype, int x, int y, int zoomLevel)
        {
            return _mapTileDownloadManager.GetImage(mtype, x, y,
                    zoomLevel);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the tile downloading.
        /// </summary>
        /// <param name="graphics">The graphics.</param>
        /// <param name="offsetX">The offset X.</param>
        /// <param name="offsetY">The offset Y.</param>
        protected void DrawTileDownloading(IGraphics graphics, int offsetX, int offsetY)
        {
            IImage imgDownloading = imageDownloading;
            if (imgDownloading != null)
            {
                graphics.SetColor(-1);
                graphics.SetClip(0, 0, _rasterMap.GetScreenWidth(),
                        _rasterMap.GetScreenHeight());
                graphics.FillRect(0, 0, _rasterMap.GetScreenWidth(),
                        _rasterMap.GetScreenHeight());
                int xIndex, yIndex;
                int tileWidth = MapLayer.MapTileWidth;
                int offX = offsetX - offsetX / tileWidth * tileWidth;
                int offY = offsetY - offsetY / tileWidth * tileWidth;
                for (xIndex = -1; xIndex <= _rasterMap.GetScreenWidth() / tileWidth + 1; xIndex++)
                {
                    for (yIndex = -1;
                            yIndex <= _rasterMap.GetScreenHeight() / tileWidth + 1;
                            yIndex++)
                    {
                        graphics.DrawImage(imgDownloading,
                                offX + xIndex * tileWidth, offY + yIndex * tileWidth);
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
        /// Map tile class.
        /// </summary>
        internal class MapTile
        {
            private readonly TiledMapTileEngine _tiledMapTileEngine;
            /**
             * the map canvas
             */
            internal IImage _mapImage;
            /**
             * Graphics Object for map canvas.
             */
            internal IGraphics _mapGraphics;
            /**
             * mapt tile index.
             */
            internal ImageTileIndex _mapTileIndex;
            /**
             * used or not.
             */
            internal bool _avaiable = true;


            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // --------   -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Initializes a new instance of the <see cref="MapTile"/> class.
            /// </summary>
            /// <param name="tiledMapTileEngine">The tiled map tile engine.</param>
           public MapTile(TiledMapTileEngine tiledMapTileEngine)
            {
                _tiledMapTileEngine = tiledMapTileEngine;
                _mapImage = MapLayer.GetAbstractGraphicsFactory()
                        .CreateImage(MapLayer.MapTileWidth,
                        MapLayer.MapTileWidth);
                _mapGraphics = _mapImage.GetGraphics();
                _mapTileIndex = new ImageTileIndex();
            }

           ////////////////////////////////////////////////////////////////////////
           //--------------------------------- REVISIONS --------------------------
           // Date       Name                 Tracking #         Description
           // --------   -------------------  -------------      ------------------
           // 28SEP2010  James Shen                 	          Code review
           ////////////////////////////////////////////////////////////////////////
           /// <summary>
           /// check to see this map cavas need to update.
           /// </summary>
           /// <returns>true ,need update</returns>
            public bool NeedToUpdate()
            {
                bool retValue = false;
                int mapType = _tiledMapTileEngine._rasterMap.GetMapType();
                int mapZoomLevel = _mapTileIndex.MapZoomLevel;
                int xIndex = _mapTileIndex.XIndex;
                int yIndex = _mapTileIndex.YIndex;
                int[] mapSequences1 =
                        (int[])MapType.MapSequences[mapType];
                int[] mapSequences;
                if (_tiledMapTileEngine._mapTileDownloadManager.GetMapDirection() != null)
                {
                    mapSequences = new int[mapSequences1.Length + 1];
                    Array.Copy(mapSequences1, 0, mapSequences,
                            0, mapSequences1.Length);
                    mapSequences[mapSequences1.Length] = MapType.RoutingDirection;
                }
                else
                {
                    mapSequences = mapSequences1;
                    //check if there's routing image ,if it's ,then
                    //the whole map canvas need to be clear
                    string key = MapType.RoutingDirection
                            + "|" + (xIndex) + "|"
                            + (yIndex) + "|" + mapZoomLevel;
                    if (_whatsInMapCanvas.ContainsKey(key))
                    {
                        ClearMapCanvas();
                        return true;
                    }
                }
                for (int mapSequenceIndex = 0;
                        mapSequenceIndex < mapSequences.Length;
                        mapSequenceIndex++)
                {
                    string key = mapSequences[mapSequenceIndex]
                            + "|" + (xIndex) + "|"
                            + (yIndex) + "|" + mapZoomLevel;
                    if (!_whatsInMapCanvas.ContainsKey(key))
                    {
                        retValue = true;
                        break;
                    }
                }
                return retValue;
            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // --------   -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// set the map tile used for given map tile index.
            /// </summary>
            /// <param name="imageTileIndex">Index of the image tile.</param>
            public void SetMapTileIndex(ImageTileIndex imageTileIndex)
            {
                _avaiable = false;
                _mapTileIndex.MapType = imageTileIndex.MapType;
                _mapTileIndex.XIndex = imageTileIndex.XIndex;
                _mapTileIndex.YIndex = imageTileIndex.YIndex;
                _mapTileIndex.MapZoomLevel = imageTileIndex.MapZoomLevel;
                _mapGraphics.DrawImage(_tiledMapTileEngine.imageDownloading, 0, 0);
                _whatsInMapCanvas.Clear();
                _pendingDrawImageQueue.Clear();

            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // --------   -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Clears the map canvas.
            /// </summary>
            public void ClearMapCanvas()
            {
                _avaiable = true;
                _mapGraphics.DrawImage(_tiledMapTileEngine.imageDownloading, 0, 0);
                _whatsInMapCanvas.Clear();
                _pendingDrawImageQueue.Clear();
            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // --------   -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Draws the testing.
            /// </summary>
            public void DrawTesting()
            {
                string strIndex = (_mapTileIndex.XIndex) + "," + (_mapTileIndex.YIndex);
                _mapGraphics.SetColor(-0x120000);
                _mapGraphics.DrawString(strIndex, 10, 10);
                _mapGraphics.DrawRect(0, 0, 256, 256);
            }

            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // --------   -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Update map canvas, check to see if need to download missing images.
            /// </summary>
            public void UpdateMapCanvas()
            {
                int mapType = _tiledMapTileEngine._rasterMap.GetMapType();
                int mapZoomLevel = _mapTileIndex.MapZoomLevel;
                int xIndex = _mapTileIndex.XIndex;
                int yIndex = _mapTileIndex.YIndex;
                int[] mapSequences1 =
                        (int[])MapType.MapSequences[mapType];
                int[] mapSequences;
                if (_tiledMapTileEngine._mapTileDownloadManager.GetMapDirection() != null)
                {
                    mapSequences = new int[mapSequences1.Length + 1];
                    Array.Copy(mapSequences1, 0, mapSequences,
                            0, mapSequences1.Length);
                    mapSequences[mapSequences1.Length] = MapType.RoutingDirection;
                }
                else
                {
                    mapSequences = mapSequences1;
                }
                for (int mapSequenceIndex = 0;
                        mapSequenceIndex < mapSequences.Length;
                        mapSequenceIndex++)
                {
                    IImage image = _tiledMapTileEngine.GetCachedImage(
                    mapSequences[mapSequenceIndex],
                    xIndex,
                    yIndex,
                    mapZoomLevel);

                    string key = mapSequences[mapSequenceIndex]
                            + "|" + (xIndex) + "|"
                            + (yIndex) + "|" + mapZoomLevel;
                    bool hasImage = false;

                    //draw the image.
                    if (image != null
                            && image
                            != MapTileDownloadManager.TileDownloading)
                    {
                        if (mapSequenceIndex == 0)
                        {
                            _mapGraphics.DrawImage(image, 0, 0);
                        }else
                        {
                            _mapGraphics.DrawImage(image, 0, 0,0xFFFFFF);
                        }
                        hasImage = true;

                    }
                    else
                    {
                        image = (IImage)_pendingDrawImageQueue[key];
                        if (image != null)
                        {
                            if (mapSequenceIndex == 0)
                            {
                                _mapGraphics.DrawImage(image, 0, 0);
                            }
                            else
                            {
                                _mapGraphics.DrawImage(image, 0, 0, 0xFFFFFF);
                            }
                            hasImage = true;

                        }
                    }
                    if (hasImage && image != MapTileDownloadManager.TileDownloading)
                    {
                        GeoPoint mapCavasIndex = new GeoPoint(xIndex, yIndex);
                        _whatsInMapCanvas[key]= mapCavasIndex;
                    }
                    else
                    {
                        image = _tiledMapTileEngine.GetImage(mapSequences[mapSequenceIndex],
                                xIndex, yIndex, mapZoomLevel);
                        if (image != null
                                && image
                                != MapTileDownloadManager.TileDownloading)
                        {
                            if (mapSequenceIndex == 0)
                            {
                                _mapGraphics.DrawImage(image, 0, 0);
                            }
                            else
                            {
                                _mapGraphics.DrawImage(image, 0, 0, 0xFFFFFF);
                            }
                            if (image != MapTileDownloadManager.TileNotAvaiable)
                            {
                                GeoPoint mapCavasIndex = new GeoPoint(xIndex, yIndex);
                                _whatsInMapCanvas[key]= mapCavasIndex;
                            }

                        }
                    }
                }
                //drawTesting();
            }


            ////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // --------   -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// drawn give map tile image on the map tile canvas.
            /// </summary>
            /// <param name="imageTileIndex">Index of the image tile.</param>
            /// <param name="image">The image.</param>
            public void DrawMapTileImage(ImageTileIndex imageTileIndex,
                    IImage image)
            {
                //check to see this image index is same as mine.
                if (!(imageTileIndex.XIndex == _mapTileIndex.XIndex
                        && imageTileIndex.YIndex == _mapTileIndex.YIndex
                        && imageTileIndex.MapZoomLevel == _mapTileIndex.MapZoomLevel))
                {
                    return;
                }

                //check to see if the image is null or download image.
                if (image == null || image
                        == MapTileDownloadManager.TileDownloading)
                {
                    return;
                }

                string key = imageTileIndex.MapType
                                + "|" + (imageTileIndex.XIndex) + "|"
                                + (imageTileIndex.YIndex)
                                + "|" + imageTileIndex.MapZoomLevel;
                if (_whatsInMapCanvas.ContainsKey(key))
                {
                    return;
                }

                int mapType = _tiledMapTileEngine._rasterMap.GetMapType();
                int mapZoomLevel = _mapTileIndex.MapZoomLevel;
                int xIndex = _mapTileIndex.XIndex;
                int yIndex = _mapTileIndex.YIndex;
                int[] mapSequences1 =
                        (int[])MapType.MapSequences[mapType];
                int[] mapSequences;
                if (_tiledMapTileEngine._mapTileDownloadManager.GetMapDirection() != null)
                {
                    mapSequences = new int[mapSequences1.Length + 1];
                    Array.Copy(mapSequences1, 0, mapSequences,
                            0, mapSequences1.Length);
                    mapSequences[mapSequences1.Length] = MapType.RoutingDirection;
                }
                else
                {
                    mapSequences = mapSequences1;
                }
                //check to see need to draw this map tile
                int i;
                for (i = 0; i < mapSequences.Length; i++)
                {
                    if (mapSequences[i] == imageTileIndex.MapType)
                    {
                        break;
                    }
                }
                if (i == mapSequences.Length)
                {
                    return;
                }
                //yes ,this image is for me.start drawing.
                //one map tile can be consisited with several layers. for example
                //a hybrid tile consists two layers. one for satellite ,one for notes.
                //and the direction layer is on top of other layers.
                try
                {
                    //first draw map tiles below current map tile.
                    //if there's map tile below current map tile havn't
                    //be drawn, adding current map tile to pending queue.
                    bool needToAddToQueue = false;
                    for (int mapSequenceIndex = 0;
                            mapSequenceIndex < mapSequences.Length;
                            mapSequenceIndex++)
                    {
                        if (imageTileIndex.MapType
                                == mapSequences[mapSequenceIndex])
                        {
                            break;
                        }
                        IImage backImage = _tiledMapTileEngine.GetCachedImage(
                                mapSequences[mapSequenceIndex],
                                xIndex,
                                yIndex,
                                mapZoomLevel);

                        key = mapSequences[mapSequenceIndex]
                                + "|" + (xIndex) + "|" + (yIndex)
                                + "|" + mapZoomLevel;
                        if (!_whatsInMapCanvas.ContainsKey(key))
                        {
                            needToAddToQueue = true;
                        }
                        if (backImage != null
                                && backImage
                                != MapTileDownloadManager.TileDownloading)
                        {
                            if (mapSequenceIndex == 0)
                            {
                                _mapGraphics.DrawImage(backImage, 0, 0);
                            }
                            else
                            {
                                _mapGraphics.DrawImage(backImage, 0, 0, 0xFFFFFF);
                            }

                        }
                        else
                        {
                            needToAddToQueue = true;
                        }
                    }

                    if (needToAddToQueue)
                    {
                        key = imageTileIndex.MapType + "|"
                                + (xIndex) + "|" + (yIndex) + "|" + mapZoomLevel;
                        _pendingDrawImageQueue[key] =image;
                    }

                    //draw current map tile.
                    if (mapSequences[0] == imageTileIndex.MapType)
                    {
                        _mapGraphics.DrawImage(image, 0, 0);
                    }
                    else
                    {
                        _mapGraphics.DrawImage(image, 0, 0, 0xFFFFFF);
                    }

                    //find next image sequence.
                    int nextImageSequence = 0;
                    for (int mapSequenceIndex = 0;
                            mapSequenceIndex < mapSequences.Length;
                            mapSequenceIndex++)
                    {
                        if (imageTileIndex.MapType == mapSequences[mapSequenceIndex])
                        {
                            nextImageSequence = mapSequenceIndex + 1;
                            break;
                        }
                    }
                    //draw next image sequences.
                    for (int mapSequenceIndex = nextImageSequence;
                            mapSequenceIndex < mapSequences.Length;
                            mapSequenceIndex++)
                    {
                        key = mapSequences[mapSequenceIndex] + "|"
                                + (xIndex) + "|" + (yIndex) + "|"
                                + mapZoomLevel;

                        IImage pendImage = (IImage)_pendingDrawImageQueue[key];
                        if (pendImage != null)
                        {
                            if (mapSequenceIndex == 0)
                            {
                                _mapGraphics.DrawImage(pendImage, 0, 0);
                            }
                            else
                            {
                                _mapGraphics.DrawImage(pendImage, 0, 0, 0xFFFFFF);
                            }

                        }
                    }
                    //now add the drawn image index to the in canvas table.
                    string inMapCanvsKey = imageTileIndex.MapType
                            + "|" + (xIndex) + "|" + (yIndex)
                            + "|" + imageTileIndex.MapZoomLevel;
                    if (image != MapTileDownloadManager.TileNotAvaiable)
                    {
                        GeoPoint mapCavasIndex = new GeoPoint(xIndex, yIndex);
                        _whatsInMapCanvas[inMapCanvsKey]=mapCavasIndex;
                    }

                    //check to see to remove from pendingDrawImageQueue
                    bool needRemovePendingImage = true;
                    for (int mapSequenceIndex = 0;
                            mapSequenceIndex < mapSequences.Length;
                            mapSequenceIndex++)
                    {

                        key = mapSequences[mapSequenceIndex] + "|"
                               + (xIndex) + "|" + (yIndex) + "|"
                               + mapZoomLevel;
                        if (!_whatsInMapCanvas.ContainsKey(key))
                        {
                            needRemovePendingImage = false;
                            break;
                        }
                    }

                    if (needRemovePendingImage)
                    {
                        _pendingDrawImageQueue.Clear();

                    }

                }
                catch (Exception)
                {

                }
                //drawTesting();

            }

            /**
             * pending drawing image queue, used to order the routing and map images.
             */
            private readonly Hashtable _pendingDrawImageQueue = new Hashtable();

            /**
             * record what have been drawn on the map tile canvas.
             */
            private readonly Hashtable _whatsInMapCanvas = new Hashtable();

        }

    }
}
