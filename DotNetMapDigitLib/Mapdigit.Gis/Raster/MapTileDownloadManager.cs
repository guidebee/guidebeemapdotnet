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
using System.IO;
using System.Collections;
using System.Threading;
using Mapdigit.Drawing;
using Mapdigit.Gis.Drawing;
using Mapdigit.Rms;
using Mapdigit.Util;

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
    /// map tile download manager.
    /// </summary>
    public sealed class MapTileDownloadManager
    {


        /// <summary>
        ///  The map tile not avaiable image.
        /// </summary>
        public static IImage TileNotAvaiable;

        /// <summary>
        /// the map tile is downloading image.
        /// </summary>
        public static IImage TileDownloading;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapTileDownloadManager"/> class.
        /// </summary>
        public MapTileDownloadManager()
            : this(null, null)
        {

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        ///<summary>
        /// set download listener
        ///</summary>
        ///<param name="mapDownloadingListener"></param>
        public void SetReaderListener(IReaderListener mapDownloadingListener)
        {
            _mapDownloadingListener = mapDownloadingListener;
            lock (_threadListMutex)
            {
                for (int i = 0; i < _maxImageDownloadWorkder; i++)
                {
                    if (_imageDownloadWorkers[i] != null)
                    {
                        _imageDownloadWorkers[i].mapTileReader.SetReadListener(mapDownloadingListener);
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
        /// Initializes a new instance of the <see cref="MapTileDownloadManager"/> class.
        /// </summary>
        /// <param name="mapDownloadingListener">The map downloading listener.</param>
        public MapTileDownloadManager(IReaderListener mapDownloadingListener)
            : this(mapDownloadingListener, null)
        {

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapTileDownloadManager"/> class.
        /// </summary>
        /// <param name="mapDownloadingListener">The map downloading listener.</param>
        /// <param name="mapTileReader">The map tile reader.</param>
        public MapTileDownloadManager(IReaderListener mapDownloadingListener,
                MapTileAbstractReader mapTileReader)
        {
            _isCacheOn = MapConfiguration._isCacheOn;
            _notLowMemoryMode = !MapConfiguration._lowMemoryMode;
            _maxImageDownloadWorkder = MapConfiguration._workerThreadNumber;
            _maxBytesInCache = MapConfiguration._mapCacheSizeInBytes;
            _imageDownloadWorkers =
                new MapTileDownloadWorker[_maxImageDownloadWorkder + 1];
            _mapDownloadingListener = mapDownloadingListener;
            lock (_threadListMutex)
            {
                _threadLists.Clear();
                for (int i = 0; i < _maxImageDownloadWorkder; i++)
                {
                    if (mapTileReader != null)
                    {
                        _imageDownloadWorkers[i] =
                                new MapTileDownloadWorker(this, mapTileReader,
                                "MapTileDownloadWorker" + i);
                    }
                    else
                    {
                        _imageDownloadWorkers[i] = new MapTileDownloadWorker(this,
                                "MapTileDownloadWorker" + i);
                    }
                    _threadLists.Add("MapTileDownloadWorker" + i,
                            _imageDownloadWorkers[i]._mapTileDownloadWorkerThread);
                }
                if (_notLowMemoryMode)
                {
                    _imageDownloadWorkers[_maxImageDownloadWorkder] = new MapDirectionRendererWorker(this);
                    _threadLists.Add("MapDirectionRendererWorker",
                            _imageDownloadWorkers[_maxImageDownloadWorkder]._mapTileDownloadWorkerThread);
                }

                _mapTileDownloadManagerThread = new Thread(Run) { Name = "MapTileDownloadManager" };
                //stream reader for stored map tiles.
                _mapTileStreamReader = new MapTileStreamReader();
                _mapTileStreamReader.SetMapDownloadingListener(mapDownloadingListener);
            }
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// start the manager thread and the worker threads.
        /// </summary>
        public void Start()
        {
            _stopDownloadManager = false;
            int threadCount = _maxImageDownloadWorkder;
            if (_notLowMemoryMode)
            {
                threadCount = _maxImageDownloadWorkder + 1;
            }
            for (int i = 0; i < threadCount; i++)
            {
                _imageDownloadWorkers[i].Start();
            }
            //_mapTileDownloadManagerThread.Priority = ThreadPriority.Lowest;
            _mapTileDownloadManagerThread.Start();
            if (_rasterMap._usePanThread)
            {
                if (_mapPanThread == null)
                {
                    _mapPanThread = _rasterMap.GetNewPandirectionThread();
                }
                _mapPanThread.Start();
            }
            _rasterMap._mapTileEngine.Start();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// restart the worker thread in case worker thread is died.
        /// </summary>
        public void RestartWorker()
        {
            GC.Collect();
            lock (_threadListMutex)
            {
                _threadLists.Clear();
                for (int i = 0; i < _maxImageDownloadWorkder; i++)
                {
                    if (_imageDownloadWorkers[i] != null)
                    {
                        _imageDownloadWorkers[i].Stop();
                        _imageDownloadWorkers[i] = null;
                    }
                    _imageDownloadWorkers[i] = new MapTileDownloadWorker(this,
                            "MapTileDownloadWorker" + i);
                    _imageDownloadWorkers[i].Start();
                    _threadLists.Add("MapTileDownloadWorker" + i,
                            _imageDownloadWorkers[i]._mapTileDownloadWorkerThread);
                }
                if (_notLowMemoryMode)
                {
                    _imageDownloadWorkers[_maxImageDownloadWorkder] = new MapDirectionRendererWorker(this);
                    _threadLists.Add("MapDirectionRendererWorker",
                            _imageDownloadWorkers[_maxImageDownloadWorkder]._mapTileDownloadWorkerThread);
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
        /// Gets the interal map tile stream reader.
        /// </summary>
        /// <returns>the internal stream reader</returns>
        public MapTileStreamReader GetInteralMapTileStreamReader()
        {
            return _mapTileStreamReader;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Close the manager thread and all the worker threads.
        /// </summary>
        public void Close()
        {
            try
            {
                Stop();
                _mapTileStreamReader.Close();
            }
            catch (Exception e)
            {
                Log.P("close" + e.Message);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Stop the manager thread and all the worker threads.
        /// </summary>
        public void Stop()
        {
            _stopDownloadManager = true;
            _assignedMapDirectionRenderListMutex.Set();
            _syncObjectManager.Set();
            int threadCount = _maxImageDownloadWorkder;
            if (_notLowMemoryMode)
            {
                threadCount = _maxImageDownloadWorkder + 1;
            }
            for (int i = 0; i < threadCount; i++)
            {
                if (_imageDownloadWorkers[i] != null)
                {
                    _imageDownloadWorkers[i].Stop();
                }
            }
            if (_rasterMap._usePanThread)
            {
                if (_mapPanThread != null)
                {
                    _mapPanThread.StopThread();
                    _mapPanThread = null;
                }
            }
            _rasterMap._mapTileEngine.Stop();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// return all the worker threads.
        /// </summary>
        /// <returns>the hashtable contains all the worker threads.</returns>
        public Hashtable GetThreads()
        {
            lock (_threadListMutex)
            {
                return _threadLists;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// the running method of this manager thread.
        /// </summary>
        public void Run()
        {
            Log.P(Thread.CurrentThread.Name + " thread started");
            while (!_stopDownloadManager)
            {
                int size = _imageTileDownloadList.Count;
                if (size > 0)
                {
                    int threadCount = _maxImageDownloadWorkder;
                    if (_notLowMemoryMode)
                    {
                        threadCount = _maxImageDownloadWorkder + 1;
                    }
                    for (int i = 0; i < threadCount; i++)
                    {
                        if (_imageDownloadWorkers[i].IsPaused())
                        {
                            _imageDownloadWorkers[i].Resume();
                        }
                    }
                }
                _syncObjectManager.WaitOne();

            }
            Log.P(Thread.CurrentThread.Name + " thread stopped");
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// clear map cache;
        /// </summary>
        internal void ClearMapCache()
        {
            if (_isCacheOn)
            {
                lock (_imageCache)
                {
                    _imageCache.Clear();
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
        /// Removes image from image cache.
        /// </summary>
        /// <param name="mtype">The mtype.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="zoomLevel">The zoom level.</param>
        public void RemoveFromImageCache(int mtype, int x, int y, int zoomLevel)
        {
            string mapIndex = mtype + "|" + x + "|" +
                              y + "|" + zoomLevel;
            lock (_imageCache)
            {
                if (_imageCache.ContainsKey(mapIndex))
                {
                    _imageCache.Remove(mapIndex);
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
        /// Gets image from image cache.
        /// </summary>
        /// <param name="mtype">The mtype.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="zoomLevel">The zoom level.</param>
        /// <returns></returns>
        public byte[] GetFromImageCache(int mtype, int x, int y, int zoomLevel)
        {
            string mapIndex = mtype + "|" + x + "|" +
                              y + "|" + zoomLevel;
            lock (_imageCache)
            {
                if (_imageCache.ContainsKey(mapIndex))
                {
                    return (byte[])_imageCache[mapIndex];
                }
                GetImage(mtype, x, y, zoomLevel);
            }
            return null;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Save map image cache to persistent memory.
        /// </summary>
        internal void SaveMapCache()
        {
            if (_isCacheOn)
            {
                try
                {
                    RecordStore.DeleteRecordStore(MapDataRecordstoreName);
                }
                catch (RecordStoreException)
                {

                }
                try
                {

                    lock (_imageCache)
                    {
                        _mapDataRecordStore = RecordStore.OpenRecordStore(MapDataRecordstoreName, true);
                        IEnumerator enumerator = _imageCache.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            DictionaryEntry dictionaryEntry = (DictionaryEntry)enumerator.Current;
                            string key = (string)dictionaryEntry.Key;
                            byte[] imageArray = (byte[])_imageCache[key];
                            if (imageArray != null)
                            {
                                byte[] recordDate = Image2ByteArray(key, imageArray);
                                _mapDataRecordStore.AddRecord(recordDate, 0, recordDate.Length);
                            }
                        }
                    }
                    _mapDataRecordStore.CloseRecordStore();
                    _mapDataRecordStore = null;

                }
                catch (RecordStoreException )
                {

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
        /// Restore map image cache from persistent memory.
        /// </summary>
        internal void RestoreMapCache()
        {
            if (_isCacheOn)
            {
                try
                {
                    lock (_imageCache)
                    {
                        _imageCache.Clear();
                        if (_mapDataRecordStore == null)
                        {
                            _mapDataRecordStore = RecordStore.OpenRecordStore(MapDataRecordstoreName, false);
                            int numOfRecords = _mapDataRecordStore.GetNumRecords();
                            numOfRecords = Math.Min(numOfRecords, MaxMapTilesNumbers);
                            for (int i = 0; i < numOfRecords; i++)
                            {
                                byte[] recordDate = _mapDataRecordStore.GetRecord(i + 1);
                                AddOneImageToCacheFromRecordStore(recordDate);

                            }
                            _mapDataRecordStore.CloseRecordStore();
                            _mapDataRecordStore = null;
                        }
                    }

                }
                catch (RecordStoreException)
                {

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
        /// get the route direction.
        /// </summary>
        /// <returns>get the first map direction.</returns>
        internal MapDirection GetMapDirection()
        {
            return _mapDirectionRenderer.GetMapDirection();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// get the route direction.
        /// </summary>
        /// <returns> the map direction array</returns>
        internal MapDirection[] GetMapDirections()
        {
            return _mapDirectionRenderer.GetMapDirections();
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
        /// <param name="newDirection"> first map direction.</param>
        internal void SetMapDirection(MapDirection newDirection)
        {
            _mapDirectionRenderer.SetMapDirection(newDirection);
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
        internal void SetMapDirections(MapDirection[] newDirections)
        {
            _mapDirectionRenderer.SetMapDirections(newDirections);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// set route pen
        /// </summary>
        /// <param name="routePen">The route pen.</param>
        internal void SetRoutePen(Pen routePen)
        {
            _mapDirectionRenderer.SetRoutePen(routePen);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Clears the task list.
        /// </summary>
        internal void ClearTaskList()
        {
            lock (_assignedImageTileDownloadListMutex)
            {
                _assignedImageTileDownloadList.Clear();
            }
            lock (_assignedMapDirectionRenderList)
            {
                _assignedMapDirectionRenderList.Clear();
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get a map tile image in cache.
        /// </summary>
        /// <param name="mtype"> the map type of the map tile.</param>
        /// <param name="x">x index of the map tile.</param>
        /// <param name="y">The  y index of the map tile.</param>
        /// <param name="zoomLevel"> the zoom level of the map tile.</param>
        /// <returns>the image at give location</returns>
        internal IImage GetCachedImage(int mtype, int x, int y, int zoomLevel)
        {
            int maxTile = (int)(MathEx.Pow(2, zoomLevel) + 0.5);
            x = x % maxTile; y = y % maxTile;
            string key = mtype + "|" +
                    x + "|" +
                    y + "|" +
                    zoomLevel;
            IImage image;
            _lastestZoomLevel = zoomLevel;
            byte[] imageArray;
            lock (_imageCache)
            {
                imageArray = (byte[])_imageCache[key];
            }
            if (imageArray == null)
            {
                image = TileDownloading;
            }
            else
            {
                image = MapLayer.GetAbstractGraphicsFactory().
                        CreateImage(imageArray, 0, imageArray.Length);
            }

            return image;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get a map tile image.
        /// </summary>
        /// <param name="mtype"> the map type of the map tile.</param>
        /// <param name="x">x index of the map tile.</param>
        /// <param name="y">The  y index of the map tile.</param>
        /// <param name="zoomLevel"> the zoom level of the map tile.</param>
        /// <returns>the image at give location</returns>
        internal IImage GetImage(int mtype, int x, int y, int zoomLevel)
        {
            if (mtype == MapType.RoutingDirection && !_notLowMemoryMode) return null;
            int maxTile = (int)(MathEx.Pow(2, zoomLevel) + 0.5);
            x = x % maxTile; y = y % maxTile;
            string key = mtype + "|" +
                    x + "|" +
                    y + "|" +
                    zoomLevel;
            IImage image;
            _lastestZoomLevel = zoomLevel;
            byte[] imageArray;
            lock (_imageCache)
            {
                imageArray = (byte[])_imageCache[key];
            }
            if (imageArray == null)
            {

                //Add check for empty url
                String url = MapType.GetTileUrl(mtype, x, y, MapType.Numzoomlevels- zoomLevel);
                if (url.StartsWith(MapType.EmptyTileUrl))
                {
                    image = TileNotAvaiable;
                }
                else
                {
                    ImageTileIndex needToDownloadImageTileIndex
                        = new ImageTileIndex();
                    needToDownloadImageTileIndex.MapType = mtype;
                    needToDownloadImageTileIndex.XIndex = x;
                    needToDownloadImageTileIndex.YIndex = y;
                    needToDownloadImageTileIndex.MapZoomLevel = zoomLevel;
                    AddToImageDownloadList(needToDownloadImageTileIndex);
                    image = TileDownloading;
                }
            }
            else
            {
                image = MapLayer.GetAbstractGraphicsFactory().CreateImage(imageArray, 0, imageArray.Length);
            }
            return image;
        }


        //The follow are configuration variables
        /**
         *  the maximum number of map tile to be downloaded in the queue
         */
        private const int MaxDownloadMapTile = 256;
        /**
         * cache size.
         */
        private const int MaxMapTilesNumbers = 256;

        /**
         * Max sizes in the image cache.
         */
        private readonly long _maxBytesInCache;

        /**
         * maximum image download worker thread size
         */
        private readonly int _maxImageDownloadWorkder;

        /**
         * is cache on or not.
         */
        private readonly bool _isCacheOn;

        /**
     * is it low momory mode or not.
     */
        private readonly bool _notLowMemoryMode;





        /**
         * worker thread array ,the extra one is mapdirectionrendererworker
         */
        private readonly MapTileDownloadWorker[] _imageDownloadWorkers;
        /**
         * This image cache stores map tiles downloaded
         */
        private readonly Hashtable _imageCache = new Hashtable(MaxMapTilesNumbers + 1);
        /**
         * the download map tile list.
         */
        private readonly Hashtable _imageTileDownloadList = new Hashtable(MaxDownloadMapTile);


        private readonly object _threadListMutex = new object();
        private  IReaderListener _mapDownloadingListener;
        internal IMapTileReadyListener _mapTileReadyListener;

        internal RasterMap.PandirectionThread _mapPanThread;
        internal RasterMap _rasterMap;

        private readonly Hashtable _threadLists = new Hashtable();

        private readonly Hashtable _assignedImageTileDownloadList = new Hashtable();
        private readonly object _assignedImageTileDownloadListMutex = new object();

        private readonly Hashtable _assignedMapDirectionRenderList = new Hashtable();
        private readonly AutoResetEvent _assignedMapDirectionRenderListMutex = new AutoResetEvent(false);


        private volatile bool _stopDownloadManager;
        private readonly Thread _mapTileDownloadManagerThread;
        private volatile int _lastestZoomLevel = -1;

        /**
         * route direction renderer.
         */
        private readonly MapDirectionRenderer _mapDirectionRenderer = new MapDirectionRenderer();

        /**
     * stream reader used to read map tiles from SD card
     */
        private readonly MapTileStreamReader _mapTileStreamReader;

        /**
         * record store
         */
        private static RecordStore _mapDataRecordStore;
        /**
         * record store name
         */
        private const string MapDataRecordstoreName = "Guidebee_MapData";
        /**
         * thread sync object.
         */
        private readonly AutoResetEvent _syncObjectManager = new AutoResetEvent(false);

        private static readonly byte[] ImageDownloadingArray = new byte[]{
         0x89,  0x50,  0x4e,  0x47,  0x0d,  0x0a,  0x1a,  0x0a,
         0x00,  0x00,  0x00,  0x0d,  0x49,  0x48,  0x44,  0x52,
         0x00,  0x00,  0x00,  0x40,  0x00,  0x00,  0x00,  0x40,
         0x04,  0x03,  0x00,  0x00,  0x00,  0x58,  0x47,  0x6c,
         0xed,  0x00,  0x00,  0x00,  0x30,  0x50,  0x4c,  0x54,
         0x45,  0xab,  0xa6,  0x9c,  0xaf,  0xaa,  0xa2,  0xb6,
         0xb2,  0xaa,  0xc1,  0xbc,  0xb6,  0xc7,  0xc3,  0xbb,
         0xcd,  0xcb,  0xc5,  0xd5,  0xd1,  0xcc,  0xdc,  0xda,
         0xd5,  0xe1,  0xdf,  0xdc,  0xe7,  0xe4,  0xdf,  0xeb,
         0xea,  0xe7,  0xf0,  0xed,  0xe8,  0xf0,  0xef,  0xee,
         0xf5,  0xf5,  0xf3,  0xfa,  0xfa,  0xf9,  0xff,  0xff,
         0xff,  0xfd,  0xa4,  0xa8,  0x3f,  0x00,  0x00,  0x00,
         0xc9,  0x49,  0x44,  0x41,  0x54,  0x48,  0xc7,  0x63,
         0xf8,  0xff,  0x7f,  0xf7,  0x6e,  0x7c,  0x98,  0x81,
         0x0e,  0x0a,  0xf0,  0x4b,  0xff,  0xff,  0x4f,  0x0f,
         0x05,  0xa3,  0xe1,  0x30,  0x1a,  0x0e,  0x38,  0xc3,
         0xe1,  0x5f,  0x12,  0x83,  0x50,  0x2c,  0x3e,  0x5f,
         0x1c,  0x62,  0x60,  0x30,  0x33,  0xca,  0xc6,  0xad,
         0xe0,  0x8f,  0x22,  0x83,  0xe5,  0xff,  0x7f,  0xee,
         0xb8,  0x15,  0x3c,  0x12,  0x64,  0x01,  0xd2,  0x3b,
         0x71,  0x87,  0x83,  0x03,  0x83,  0x3e,  0x3e,  0x5f,
         0x7c,  0x5f,  0x2b,  0xc0,  0xd0,  0x0f,  0xa2,  0xcf,
         0xe3,  0x50,  0xf0,  0x55,  0x90,  0x81,  0x61,  0xd6,
         0xff,  0xdd,  0xbb,  0x02,  0xed,  0x71,  0x28,  0xf8,
         0xa7,  0xc8,  0xc0,  0x20,  0xd8,  0xbf,  0x7b,  0x92,
         0xa0,  0x18,  0xae,  0x70,  0x28,  0x10,  0x14,  0x14,
         0xdf,  0xfd,  0x0b,  0xa8,  0x6c,  0x3d,  0x0e,  0x5f,
         0x6c,  0x15,  0x14,  0x9e,  0xbd,  0xbb,  0x11,  0x68,
         0x91,  0x1e,  0x2e,  0x6f,  0x1a,  0xc6,  0xed,  0xde,
         0xfd,  0x6b,  0x22,  0x03,  0x83,  0x28,  0x2e,  0x05,
         0xc7,  0x41,  0x74,  0x23,  0x83,  0xe4,  0x72,  0xbc,
         0xe9,  0x41,  0x41,  0x30,  0x1f,  0x67,  0x6c,  0x82,
         0xc2,  0x70,  0x0b,  0x83,  0xd0,  0x7b,  0x9c,  0x0a,
         0xb6,  0x7b,  0xfd,  0xff,  0x6b,  0xc0,  0xa0,  0x83,
         0x27,  0x3d,  0x24,  0x9a,  0x19,  0x32,  0x30,  0x9d,
         0xc7,  0x93,  0x1e,  0xfe,  0x3a,  0x32,  0x08,  0xd7,
         0x8d,  0xe6,  0x8b,  0xd1,  0xf2,  0x61,  0xc4,  0x87,
         0x03,  0x00,  0x95,  0x74,  0xb0,  0xed,  0x65,  0x48,
         0x6d,  0x06,  0x00,  0x00,  0x00,  0x00,  0x49,  0x45,
         0x4e,  0x44,  0xae,  0x42,  0x60,  0x82};

        private static readonly byte[] ImageNoavaiableArray = new byte[]{
            0x89,0x50,0x4E,0x47,0x0D,0x0A,0x1A,
            0x0A,0x00,0x00,0x00,0x0D,0x49,0x48,0x44,
            0x52,0x00,0x00,0x01,0x00,0x00,0x00,0x01,
            0x00,0x08,0x03,0x00,0x00,0x00,0x6B,0xAC,
            0x58,0x54,0x00,0x00,0x00,0x04,0x67,0x41,
            0x4D,0x41,0x00,0x00,0xB1,0x8E,0x7C,0xFB,
            0x51,0x93,0x00,0x00,0x00,0x20,0x63,0x48,
            0x52,0x4D,0x00,0x00,0x7A,0x25,0x00,0x00,
            0x80,0x83,0x00,0x00,0xF9,0xFF,0x00,0x00,
            0x80,0xE6,0x00,0x00,0x75,0x2E,0x00,0x00,
            0xEA,0x5F,0x00,0x00,0x3A,0x97,0x00,0x00,
            0x17,0x6F,0x69,0xE4,0xC4,0x2B,0x00,0x00,
            0x03,0x00,0x50,0x4C,0x54,0x45,0x00,0x00,
            0x00,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x48,0xCA,
            0xB7,0x07,0x00,0x00,0x00,0x03,0x74,0x52,
            0x4E,0x53,0xFF,0xFF,0x00,0xD7,0xCA,0x0D,
            0x41,0x00,0x00,0x05,0xAC,0x49,0x44,0x41,
            0x54,0x78,0x9C,0x62,0x60,0x1A,0xE1,0x00,
            0x20,0x80,0x18,0x06,0xDA,0x01,0x03,0x0D,
            0x00,0x02,0x68,0xC4,0x07,0x00,0x40,0x00,
            0x8D,0xF8,0x00,0x00,0x08,0xA0,0x11,0x1F,
            0x00,0x00,0x01,0x34,0xE2,0x03,0x00,0x20,
            0x80,0x46,0x7C,0x00,0x00,0x04,0xD0,0x88,
            0x0F,0x00,0x80,0x00,0x1A,0xF1,0x01,0x00,
            0x10,0x40,0x23,0x3E,0x00,0x00,0x02,0x68,
            0xC4,0x07,0x00,0x40,0x00,0x8D,0xF8,0x00,
            0x00,0x08,0xA0,0x11,0x1F,0x00,0x00,0x01,
            0x34,0xE2,0x03,0x00,0x20,0x80,0x46,0x7C,
            0x00,0x00,0x04,0xD0,0x88,0x0F,0x00,0x80,
            0x00,0x1A,0xF1,0x01,0x00,0x10,0x40,0x23,
            0x3E,0x00,0x00,0x02,0x68,0xC4,0x07,0x00,
            0x40,0x00,0x8D,0xF8,0x00,0x00,0x08,0xA0,
            0x11,0x1F,0x00,0x00,0x01,0x34,0xE2,0x03,
            0x00,0x20,0x80,0x46,0x7C,0x00,0x00,0x04,
            0xD0,0x88,0x0F,0x00,0x80,0x00,0x1A,0xF1,
            0x01,0x00,0x10,0x40,0x23,0x3E,0x00,0x00,
            0x02,0x68,0xC4,0x07,0x00,0x40,0x00,0x8D,
            0xF8,0x00,0x00,0x08,0xA0,0x11,0x1F,0x00,
            0x00,0x01,0x34,0xE2,0x03,0x00,0x20,0x80,
            0x46,0x7C,0x00,0x00,0x04,0xD0,0x88,0x0F,
            0x00,0x80,0x00,0x1A,0xF1,0x01,0x00,0x10,
            0x40,0x23,0x3E,0x00,0x00,0x02,0x68,0xC4,
            0x07,0x00,0x40,0x00,0x8D,0xF8,0x00,0x00,
            0x08,0xA0,0x11,0x1F,0x00,0x00,0x01,0x34,
            0xE2,0x03,0x00,0x20,0x80,0x46,0x7C,0x00,
            0x00,0x04,0xD0,0x88,0x0F,0x00,0x80,0x00,
            0x1A,0xF1,0x01,0x00,0x10,0x40,0x23,0x3E,
            0x00,0x00,0x02,0x68,0xC4,0x07,0x00,0x40,
            0x00,0x8D,0xF8,0x00,0x00,0x08,0xA0,0x11,
            0x1F,0x00,0x00,0x01,0x34,0xE2,0x03,0x00,
            0x20,0x80,0x46,0x7C,0x00,0x00,0x04,0xD0,
            0x88,0x0F,0x00,0x80,0x00,0x1A,0xF1,0x01,
            0x00,0x10,0x40,0x23,0x3E,0x00,0x00,0x02,
            0x68,0xC4,0x07,0x00,0x40,0x00,0x8D,0xF8,
            0x00,0x00,0x08,0xA0,0x11,0x1F,0x00,0x00,
            0x01,0x34,0xE2,0x03,0x00,0x20,0x80,0x46,
            0x7C,0x00,0x00,0x04,0xD0,0x88,0x0F,0x00,
            0x80,0x00,0x1A,0xF1,0x01,0x00,0x10,0x40,
            0x23,0x3E,0x00,0x00,0x02,0x68,0xC4,0x07,
            0x00,0x40,0x00,0x8D,0xF8,0x00,0x00,0x08,
            0xA0,0x11,0x1F,0x00,0x00,0x01,0x34,0xE2,
            0x03,0x00,0x20,0x80,0x46,0x7C,0x00,0x00,
            0x04,0xD0,0x88,0x0F,0x00,0x80,0x00,0x1A,
            0xF1,0x01,0x00,0x10,0x40,0x23,0x3E,0x00,
            0x00,0x02,0x68,0xC4,0x07,0x00,0x40,0x00,
            0x8D,0xF8,0x00,0x00,0x08,0xA0,0x11,0x1F,
            0x00,0x00,0x01,0x34,0xE2,0x03,0x00,0x20,
            0x80,0x46,0x7C,0x00,0x00,0x04,0xD0,0x88,
            0x0F,0x00,0x80,0x00,0x1A,0xF1,0x01,0x00,
            0x10,0x40,0x23,0x3E,0x00,0x00,0x02,0x68,
            0xC4,0x07,0x00,0x40,0x00,0x8D,0xF8,0x00,
            0x00,0x08,0xA0,0x11,0x1F,0x00,0x00,0x01,
            0x34,0xE2,0x03,0x00,0x20,0x80,0x46,0x7C,
            0x00,0x00,0x04,0xD0,0x88,0x0F,0x00,0x80,
            0x00,0x1A,0xF1,0x01,0x00,0x10,0x40,0x23,
            0x3E,0x00,0x00,0x02,0x68,0xC4,0x07,0x00,
            0x40,0x00,0x8D,0xF8,0x00,0x00,0x08,0xA0,
            0x11,0x1F,0x00,0x00,0x01,0x34,0xE2,0x03,
            0x00,0x20,0x80,0x46,0x7C,0x00,0x00,0x04,
            0xD0,0x88,0x0F,0x00,0x80,0x00,0x1A,0xF1,
            0x01,0x00,0x10,0x40,0x23,0x3E,0x00,0x00,
            0x02,0x68,0xC4,0x07,0x00,0x40,0x00,0x8D,
            0xF8,0x00,0x00,0x08,0xA0,0x11,0x1F,0x00,
            0x00,0x01,0x34,0xE2,0x03,0x00,0x20,0x80,
            0x46,0x7C,0x00,0x00,0x04,0xD0,0x88,0x0F,
            0x00,0x80,0x00,0x1A,0xF1,0x01,0x00,0x10,
            0x40,0x23,0x3E,0x00,0x00,0x02,0x68,0xC4,
            0x07,0x00,0x40,0x00,0x8D,0xF8,0x00,0x00,
            0x08,0xA0,0x11,0x1F,0x00,0x00,0x01,0x34,
            0xE2,0x03,0x00,0x20,0x80,0x46,0x7C,0x00,
            0x00,0x04,0xD0,0x88,0x0F,0x00,0x80,0x00,
            0x1A,0xF1,0x01,0x00,0x10,0x40,0x23,0x3E,
            0x00,0x00,0x02,0x68,0xC4,0x07,0x00,0x40,
            0x00,0x8D,0xF8,0x00,0x00,0x08,0xA0,0x11,
            0x1F,0x00,0x00,0x01,0x34,0xE2,0x03,0x00,
            0x20,0x80,0x46,0x7C,0x00,0x00,0x04,0xD0,
            0x88,0x0F,0x00,0x80,0x00,0x1A,0xF1,0x01,
            0x00,0x10,0x40,0x23,0x3E,0x00,0x00,0x02,
            0x68,0xC4,0x07,0x00,0x40,0x00,0x8D,0xF8,
            0x00,0x00,0x08,0xA0,0x11,0x1F,0x00,0x00,
            0x01,0x34,0xE2,0x03,0x00,0x20,0x80,0x46,
            0x7C,0x00,0x00,0x04,0xD0,0x88,0x0F,0x00,
            0x80,0x00,0x1A,0xF1,0x01,0x00,0x10,0x40,
            0x23,0x3E,0x00,0x00,0x02,0x68,0xC4,0x07,
            0x00,0x40,0x00,0x8D,0xF8,0x00,0x00,0x08,
            0xA0,0x11,0x1F,0x00,0x00,0x01,0x34,0xE2,
            0x03,0x00,0x20,0x80,0x46,0x7C,0x00,0x00,
            0x04,0xD0,0x88,0x0F,0x00,0x80,0x00,0x1A,
            0xF1,0x01,0x00,0x10,0x40,0x23,0x3E,0x00,
            0x00,0x02,0x68,0xC4,0x07,0x00,0x40,0x00,
            0x8D,0xF8,0x00,0x00,0x08,0xA0,0x11,0x1F,
            0x00,0x00,0x01,0x34,0xE2,0x03,0x00,0x20,
            0x80,0x46,0x7C,0x00,0x00,0x04,0xD0,0x88,
            0x0F,0x00,0x80,0x00,0x1A,0xF1,0x01,0x00,
            0x10,0x40,0x23,0x3E,0x00,0x00,0x02,0x68,
            0xC4,0x07,0x00,0x40,0x00,0x8D,0xF8,0x00,
            0x00,0x08,0xA0,0x11,0x1F,0x00,0x00,0x01,
            0x34,0xE2,0x03,0x00,0x20,0x80,0x46,0x7C,
            0x00,0x00,0x04,0xD0,0x88,0x0F,0x00,0x80,
            0x00,0x1A,0xF1,0x01,0x00,0x10,0x40,0x23,
            0x3E,0x00,0x00,0x02,0x68,0xC4,0x07,0x00,
            0x40,0x00,0x8D,0xF8,0x00,0x00,0x08,0xA0,
            0x11,0x1F,0x00,0x00,0x01,0x34,0xE2,0x03,
            0x00,0x20,0x80,0x46,0x7C,0x00,0x00,0x04,
            0xD0,0x88,0x0F,0x00,0x80,0x00,0x1A,0xF1,
            0x01,0x00,0x10,0x40,0x23,0x3E,0x00,0x00,
            0x02,0x68,0xC4,0x07,0x00,0x40,0x00,0x8D,
            0xF8,0x00,0x00,0x08,0xA0,0x11,0x1F,0x00,
            0x00,0x01,0x34,0xE2,0x03,0x00,0x20,0x80,
            0x46,0x7C,0x00,0x00,0x04,0xD0,0x88,0x0F,
            0x00,0x80,0x00,0x1A,0xF1,0x01,0x00,0x10,
            0x40,0x23,0x3E,0x00,0x00,0x02,0x68,0xC4,
            0x07,0x00,0x40,0x00,0x8D,0xF8,0x00,0x00,
            0x08,0xA0,0x11,0x1F,0x00,0x00,0x01,0x34,
            0xE2,0x03,0x00,0x20,0x80,0x46,0x7C,0x00,
            0x00,0x04,0xD0,0x88,0x0F,0x00,0x80,0x00,
            0x1A,0xF1,0x01,0x00,0x10,0x40,0x23,0x3E,
            0x00,0x00,0x02,0x68,0xC4,0x07,0x00,0x40,
            0x00,0x8D,0xF8,0x00,0x00,0x08,0xA0,0x11,
            0x1F,0x00,0x00,0x01,0x34,0xE2,0x03,0x00,
            0x20,0x80,0x46,0x7C,0x00,0x00,0x04,0xD0,
            0x88,0x0F,0x00,0x80,0x00,0x1A,0xF1,0x01,
            0x00,0x10,0x40,0x23,0x3E,0x00,0x00,0x02,
            0x68,0xC4,0x07,0x00,0x40,0x00,0x8D,0xF8,
            0x00,0x00,0x08,0xA0,0x11,0x1F,0x00,0x00,
            0x01,0x34,0xE2,0x03,0x00,0x20,0x80,0x46,
            0x7C,0x00,0x00,0x04,0xD0,0x88,0x0F,0x00,
            0x80,0x00,0x1A,0xF1,0x01,0x00,0x10,0x40,
            0x23,0x3E,0x00,0x00,0x02,0x68,0xC4,0x07,
            0x00,0x40,0x00,0x8D,0xF8,0x00,0x00,0x08,
            0xA0,0x11,0x1F,0x00,0x00,0x01,0x34,0xE2,
            0x03,0x00,0x20,0x80,0x46,0x7C,0x00,0x00,
            0x04,0xD0,0x88,0x0F,0x00,0x80,0x00,0x1A,
            0xF1,0x01,0x00,0x10,0x40,0x23,0x3E,0x00,
            0x00,0x02,0x68,0xC4,0x07,0x00,0x40,0x00,
            0x8D,0xF8,0x00,0x00,0x08,0xA0,0x11,0x1F,
            0x00,0x00,0x01,0x34,0xE2,0x03,0x00,0x20,
            0x80,0x46,0x7C,0x00,0x00,0x04,0xD0,0x88,
            0x0F,0x00,0x80,0x00,0x1A,0xF1,0x01,0x00,
            0x10,0x40,0x23,0x3E,0x00,0x00,0x02,0x68,
            0xC4,0x07,0x00,0x40,0x00,0x8D,0xF8,0x00,
            0x00,0x08,0xA0,0x11,0x1F,0x00,0x00,0x01,
            0x34,0xE2,0x03,0x00,0x20,0x80,0x46,0x7C,
            0x00,0x00,0x04,0xD0,0x88,0x0F,0x00,0x80,
            0x00,0x1A,0xF1,0x01,0x00,0x10,0x40,0x23,
            0x3E,0x00,0x00,0x02,0x68,0xC4,0x07,0x00,
            0x40,0x00,0x8D,0xF8,0x00,0x00,0x08,0xA0,
            0x11,0x1F,0x00,0x00,0x01,0x34,0xE2,0x03,
            0x00,0x20,0x80,0x46,0x7C,0x00,0x00,0x04,
            0xD0,0x88,0x0F,0x00,0x80,0x00,0x1A,0xF1,
            0x01,0x00,0x10,0x40,0x23,0x3E,0x00,0x00,
            0x02,0x68,0xC4,0x07,0x00,0x40,0x00,0x8D,
            0xF8,0x00,0x00,0x08,0xA0,0x11,0x1F,0x00,
            0x00,0x01,0x34,0xE2,0x03,0x00,0x20,0x80,
            0x46,0x7C,0x00,0x00,0x04,0xD0,0x88,0x0F,
            0x00,0x80,0x00,0x1A,0xF1,0x01,0x00,0x10,
            0x40,0x23,0x3E,0x00,0x00,0x02,0x68,0xC4,
            0x07,0x00,0x40,0x00,0x8D,0xF8,0x00,0x00,
            0x08,0xA0,0x11,0x1F,0x00,0x00,0x01,0x34,
            0xE2,0x03,0x00,0x20,0x80,0x46,0x7C,0x00,
            0x00,0x04,0xD0,0x88,0x0F,0x00,0x80,0x00,
            0x1A,0xF1,0x01,0x00,0x10,0x40,0x23,0x3E,
            0x00,0x00,0x02,0x68,0xC4,0x07,0x00,0x40,
            0x00,0x8D,0xF8,0x00,0x00,0x08,0xA0,0x11,
            0x1F,0x00,0x00,0x01,0x34,0xE2,0x03,0x00,
            0x20,0x80,0x46,0x7C,0x00,0x00,0x04,0xD0,
            0x88,0x0F,0x00,0x80,0x00,0x1A,0xF1,0x01,
            0x00,0x10,0x40,0x23,0x3E,0x00,0x00,0x02,
            0x68,0xC4,0x07,0x00,0x40,0x00,0x8D,0xF8,
            0x00,0x00,0x08,0xA0,0x11,0x1F,0x00,0x00,
            0x01,0x34,0xE2,0x03,0x00,0x20,0x80,0x46,
            0x7C,0x00,0x00,0x04,0xD0,0x88,0x0F,0x00,
            0x80,0x00,0x1A,0xF1,0x01,0x00,0x10,0x40,
            0x23,0x3E,0x00,0x00,0x02,0x68,0xC4,0x07,
            0x00,0x40,0x00,0x8D,0xF8,0x00,0x00,0x08,
            0xA0,0x11,0x1F,0x00,0x00,0x01,0x34,0xE2,
            0x03,0x00,0x20,0x80,0x46,0x7C,0x00,0x00,
            0x04,0xD0,0x88,0x0F,0x00,0x80,0x00,0x03,
            0x00,0x10,0xF0,0x00,0x1F,0xBE,0x86,0x97,
            0x97,0x00,0x00,0x00,0x00,0x49,0x45,0x4E,
            0x44,0xAE,0x42,0x60,0x82};


        /**
         * Intialized the images.
         */
        static MapTileDownloadManager()
        {
            try
            {
                TileNotAvaiable = MapLayer.GetAbstractGraphicsFactory()
                        .CreateImage(ImageNoavaiableArray, 0, ImageNoavaiableArray.Length);
                TileDownloading = MapLayer.GetAbstractGraphicsFactory()
                        .CreateImage(ImageDownloadingArray, 0, ImageDownloadingArray.Length);
           
            }
            catch (Exception)
            {

            }

        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Adds to assigned image download list.
        /// </summary>
        /// <param name="imageTileIndex">Index of the image tile.</param>
        private void AddToAssignedImageDownloadList(ImageTileIndex imageTileIndex)
        {
            lock (_assignedImageTileDownloadListMutex)
            {
                string key = imageTileIndex.MapType + "|" +
                        imageTileIndex.XIndex + "|" +
                        imageTileIndex.YIndex + "|" +
                        imageTileIndex.MapZoomLevel;
                _assignedImageTileDownloadList[key] = imageTileIndex;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Adds to image download list.
        /// </summary>
        /// <param name="imageTileIndex">Index of the image tile.</param>
        private void AddToImageDownloadList(ImageTileIndex imageTileIndex)
        {
            lock (_assignedImageTileDownloadListMutex)
            {
                string key = imageTileIndex.MapType + "|" +
                        imageTileIndex.XIndex + "|" +
                        imageTileIndex.YIndex + "|" +
                        imageTileIndex.MapZoomLevel;
                object object3 = _imageCache[key];
                object object1 = _imageTileDownloadList[key];
                object object2 = _assignedImageTileDownloadList[key];

                if (object3 == null && object1 == null && object2 == null)
                {

                    ImageTileIndex newImagetileIndex = new ImageTileIndex();
                    newImagetileIndex.MapType = imageTileIndex.MapType;
                    newImagetileIndex.XIndex = imageTileIndex.XIndex;
                    newImagetileIndex.YIndex = imageTileIndex.YIndex;
                    newImagetileIndex.MapZoomLevel = imageTileIndex.MapZoomLevel;

                    _imageTileDownloadList[key]= newImagetileIndex;
                    _syncObjectManager.Set();
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
        /// Called when [download image tile done].
        /// </summary>
        /// <param name="key">The key.</param>
        private void OneDownloadImageTileDone(string key)
        {
            lock (_assignedImageTileDownloadListMutex)
            {
                _assignedImageTileDownloadList.Remove(key);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the index of the A image tile.
        /// </summary>
        /// <returns></returns>
        private ImageTileIndex GetAImageTileIndex()
        {
            lock (_assignedImageTileDownloadListMutex)
            {
                ImageTileIndex imageTileIndex = null;
                if (_imageTileDownloadList.Count > 0)
                {
                    ICollection keys = _imageTileDownloadList.Keys;
                    foreach (object key in keys)
                    {
                        imageTileIndex =
                            (ImageTileIndex)_imageTileDownloadList[key];
                        _imageTileDownloadList.Remove(key);
                        break;
                    }


                }
                return imageTileIndex;

            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Adds to image cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="imageArray">The image array.</param>
        private void AddToImageCache(string key, byte[] imageArray)
        {
            if (_isCacheOn)
            {
                lock (_imageCache)
                {
                    long bytesInCache = 0;
                    foreach (var o in _imageCache.Values)
                    {
                        byte[] array = (byte[])o;
                        bytesInCache += array.Length;
                    }


                    if (bytesInCache > _maxBytesInCache)
                    {
                        //imageCache.removeHalfElements();
                        _imageCache.Clear();
                    }
                    _imageCache[key] = imageArray;
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
        /// Adds the one image to cache from record store.
        /// </summary>
        /// <param name="imageArray">The image array.</param>
        private void AddOneImageToCacheFromRecordStore(byte[] imageArray)
        {
            MemoryStream bais = new MemoryStream(imageArray);
            BinaryReader dis = new BinaryReader(bais);
            byte[] image;
            try
            {
                string key = dis.ReadString();
                int imageSize = dis.ReadInt32();
                image = new byte[imageSize];
                dis.Read(image, 0, image.Length);

                AddToImageCache(key, image);

            }
            catch (Exception)
            {

            }

        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// each record has the format of [Key,imageLength,imagedata]
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        private static byte[] Image2ByteArray(string key, byte[] image)
        {
            byte[] imageArray = null;
            try
            {
                MemoryStream baos = new MemoryStream();
                BinaryWriter dos = new BinaryWriter(baos);
                dos.Write(key);
                dos.Write(image.Length);
                dos.Write(image);
                imageArray = baos.GetBuffer();
                dos.Close();
                baos.Close();
            }
            catch (IOException)
            {

            }
            return imageArray;
        }

        internal class MapDirectionRendererWorker : MapTileDownloadWorker
        {
            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Initial Creation
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Initializes a new instance of the <see cref="MapDirectionRendererWorker"/> class.
            /// </summary>
            /// <param name="manager">The manager.</param>
            internal MapDirectionRendererWorker(MapTileDownloadManager manager)
                : base(manager, manager._mapDirectionRenderer, "MapDirectionRendererWorker")
            {

            }

            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Initial Creation
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// </summary>
            /// <param name="imageTileIndex"></param>
            /// get one map tile.
            /// @param imageTileIndex the index of given map tile.
            internal override void GetImage(ImageTileIndex imageTileIndex)
            {
                try
                {
                    string key = imageTileIndex.MapType + "|" +
                            imageTileIndex.XIndex + "|" +
                            imageTileIndex.YIndex + "|" +
                            imageTileIndex.MapZoomLevel;
                    IImage image = TileNotAvaiable;
                    if (mapTileReader is MapDirectionRenderer)
                    {
                        MapDirectionRenderer mapDirectionRenderer = (MapDirectionRenderer)mapTileReader;
                        //this is a block methods,it returns when the download is done.
                        if (imageTileIndex.MapZoomLevel == mapTileDownloadManager._lastestZoomLevel)
                        {
                            image = mapDirectionRenderer.GetImage(
                                    imageTileIndex.XIndex, imageTileIndex.YIndex,
                                    imageTileIndex.MapZoomLevel);
                        }
                        if (image == null)
                        {
                            image = TileNotAvaiable;
                        }
                        mapTileDownloadManager.OneDownloadImageTileDone(key);
                        mapTileDownloadManager._mapTileReadyListener.Done(imageTileIndex, image);
                    }

                }
                catch (Exception)
                {

                }

            }

            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Initial Creation
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// </summary>
            /// the actually thread run.
            public override void Run()
            {
                while (!stopDownloadWorker)
                {
                    try
                    {
                        bool needToWait = false;
                        lock (mapTileDownloadManager._assignedMapDirectionRenderListMutex)
                        {
                            if (mapTileDownloadManager._assignedMapDirectionRenderList.Count > 0)
                            {
                                ImageTileIndex imageTileIndex = null;


                                ICollection keys = mapTileDownloadManager._assignedMapDirectionRenderList.Keys;
                                foreach (object key in keys)
                                {
                                    imageTileIndex =
                                        (ImageTileIndex) mapTileDownloadManager._assignedMapDirectionRenderList[key];
                                    mapTileDownloadManager._assignedMapDirectionRenderList.Remove(key);

                                    break;
                                }

                                GetImage(imageTileIndex);

                            }
                            else
                            {
                                needToWait = true;
                            }
                        }

                        if(needToWait)
                        {
                             pauseDownloadWorker = true;
                             mapTileDownloadManager._assignedMapDirectionRenderListMutex.WaitOne(maxWaitingTime * 1000, false);
                                  
                        }
  

                    }
                    catch (Exception)
                    {
                        //catch whatever exception to make sure the thread is not dead.

                    }
                }
            }
        }

        //--------------------------------- REVISIONS --------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ------------------
        // 28SEP2010  James Shen                 	          Initial Creation
        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// map download work thread.
        /// </summary>
        internal class MapTileDownloadWorker
        {

            /**
             * the map tile downloader actually do the download work.
             */
            internal MapTileAbstractReader mapTileReader;
            /**
             * Download manager object.
             */
            protected MapTileDownloadManager mapTileDownloadManager;
            protected volatile bool stopDownloadWorker;
            protected volatile bool pauseDownloadWorker;
            internal Thread _mapTileDownloadWorkerThread;
            protected string threadName;
            protected AutoResetEvent syncObjectWorker = new AutoResetEvent(false);
            /**
             * Max wait time for download an image in seconds.
             */
            protected int maxWaitingTime = 120;

            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Initial Creation
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Initializes a new instance of the <see cref="MapTileDownloadWorker"/> class.
            /// </summary>
            /// <param name="manager">The manager.</param>
            /// <param name="threadName">Name of the thread.</param>
            internal MapTileDownloadWorker(MapTileDownloadManager manager, string threadName)
            {
                mapTileDownloadManager = manager;
                this.threadName = threadName;
                _mapTileDownloadWorkerThread = new Thread(Run);
                _mapTileDownloadWorkerThread.Name = threadName;
                mapTileReader = new MapTileDownloader();
                mapTileReader.SetMapDownloadingListener(manager._mapDownloadingListener);
            }

            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Initial Creation
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Initializes a new instance of the <see cref="MapTileDownloadWorker"/> class.
            /// </summary>
            /// <param name="manager">The manager.</param>
            /// <param name="mapTileReader">The map tile reader.</param>
            /// <param name="threadName">Name of the thread.</param>
            internal MapTileDownloadWorker(MapTileDownloadManager manager,
                    MapTileAbstractReader mapTileReader, string threadName)
            {
                mapTileDownloadManager = manager;
                this.threadName = threadName;
                _mapTileDownloadWorkerThread = new Thread(Run);
                _mapTileDownloadWorkerThread.Name = threadName;
                this.mapTileReader = mapTileReader;
                this.mapTileReader.SetMapDownloadingListener(manager._mapDownloadingListener);
            }

            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Initial Creation
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Starts this instance.
            /// </summary>
            public void Start()
            {
                stopDownloadWorker = false;

                _mapTileDownloadWorkerThread.Start();
            }

            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Initial Creation
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Gets the image.
            /// </summary>
            /// <param name="imageTileIndex">Index of the image tile.</param>
            internal virtual void GetImage(ImageTileIndex imageTileIndex)
            {
                try
                {
                    string key = imageTileIndex.MapType + "|" +
                            imageTileIndex.XIndex + "|" +
                            imageTileIndex.YIndex + "|" +
                            imageTileIndex.MapZoomLevel;
                    byte[] imageArray;
                    IImage image = TileNotAvaiable;
                    //this is a block methods,it returns when the download is done.

                    mapTileReader.GetImage(imageTileIndex.MapType,
                            imageTileIndex.XIndex, imageTileIndex.YIndex,
                            imageTileIndex.MapZoomLevel);
                    //if the downloading is successful
                    if (mapTileReader.ImageArraySize > 0
                            && mapTileReader.IsImagevalid)
                    {
                        imageArray = mapTileReader.ImageArray;
                        if (imageArray != null)
                        {
                            try
                            {
                                image = MapLayer.GetAbstractGraphicsFactory()
                                        .CreateImage(imageArray, 0, imageArray.Length);
                            }
                            catch (Exception)
                            {

                                mapTileReader.IsImagevalid = false;
                                image = TileNotAvaiable;

                            }
                            if (mapTileReader.IsImagevalid)
                            {
                                mapTileDownloadManager.AddToImageCache(key, imageArray);
                            }
                        }

                    }
                    mapTileDownloadManager.OneDownloadImageTileDone(key);
                    mapTileDownloadManager._mapTileReadyListener.Done(imageTileIndex, image);
                }
                catch (Exception)
                {

                }

            }

            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Initial Creation
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Stops this instance.
            /// </summary>
            public void Stop()
            {
                stopDownloadWorker = true;
                syncObjectWorker.Set();
            }

            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Initial Creation
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Runs this instance.
            /// </summary>
            public virtual void Run()
            {
                Log.P(Thread.CurrentThread.Name + " thread started");
                while (!stopDownloadWorker)
                {
                    try
                    {
                        //get one map tile index from dowload manager's download list.
                        ImageTileIndex imageTileIndex = mapTileDownloadManager
                                .GetAImageTileIndex();
                        if (imageTileIndex != null)
                        {
                            pauseDownloadWorker = false;
                            mapTileDownloadManager.
                                    AddToAssignedImageDownloadList(imageTileIndex);
                            if (imageTileIndex.MapType == MapType.RoutingDirection)
                            {
                                bool needToNotify = false;
                                //if it's to render the map direction, just assign
                                //it to  mapDirectionRenderer.
                                lock (mapTileDownloadManager._assignedMapDirectionRenderList)
                                {
                                    string key = imageTileIndex.MapType + "|" +
                                            imageTileIndex.XIndex + "|" +
                                            imageTileIndex.YIndex + "|" +
                                            imageTileIndex.MapZoomLevel;
                                    if (!mapTileDownloadManager._assignedMapDirectionRenderList.ContainsKey(key))
                                    {
                                        ImageTileIndex newImagetileIndex = new ImageTileIndex();
                                        newImagetileIndex.MapType = imageTileIndex.MapType;
                                        newImagetileIndex.XIndex = imageTileIndex.XIndex;
                                        newImagetileIndex.YIndex = imageTileIndex.YIndex;
                                        newImagetileIndex.MapZoomLevel = imageTileIndex.MapZoomLevel;
                                        mapTileDownloadManager._assignedMapDirectionRenderList[key]= newImagetileIndex;
                                        needToNotify = true;
                                        
                                    }

                                }
                                if(needToNotify)
                                {
                                    mapTileDownloadManager._assignedMapDirectionRenderListMutex.Set();
                                }
                            }
                            else
                            {
                                GetImage(imageTileIndex);
                            }
                        }
                        else
                        {
                            pauseDownloadWorker = true;
                            syncObjectWorker.WaitOne(maxWaitingTime * 1000, false);
                           
                        }
                    }
                    catch (Exception)
                    {
                        //catch whatever exception to make sure the thread is not dead.

                    }
                }
                Log.P(Thread.CurrentThread.Name + " thread stopped");
            }

            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Initial Creation
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Resumes this instance.
            /// </summary>
            public void Resume()
            {
                pauseDownloadWorker = false;
                syncObjectWorker.Set();

            }

            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Initial Creation
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// check if the thread is paused or not
            /// </summary>
            /// <returns>
            /// 	if ture then the thread is paused.
            /// </returns>
            public bool IsPaused()
            {
                if (_mapTileDownloadWorkerThread != null)
                {
                    return pauseDownloadWorker;
                }
                return false;
            }

            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Initial Creation
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Sets the paused.
            /// </summary>
            /// <param name="value">if set to <c>true</c> [value].</param>
            public void SetPaused(bool value)
            {
                pauseDownloadWorker = value;
            }

            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Initial Creation
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// check if the thread is alive.
            /// </summary>
            /// <returns>
            /// 	<c>true</c> if this instance is alive; otherwise, <c>false</c>.
            /// </returns>
            public bool IsAlive()
            {
                if (_mapTileDownloadWorkerThread != null)
                {
                    return true;
                }
                return false;
            }
        }
    }



}
