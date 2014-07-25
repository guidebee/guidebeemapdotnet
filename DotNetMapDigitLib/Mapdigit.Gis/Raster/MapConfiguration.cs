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
    /// map configuration.
    /// </summary>
    public class MapConfiguration
    {

        /// <summary>
        /// is cache on or not.
        /// </summary>
        public const int IsCacheOn = 1;

        /// <summary>
        /// the no of worker thread.
        /// </summary>
        public const int WorkerThreadNumber = 8;

        /// <summary>
        /// the map cache size.
        /// </summary>
        public const int MapCacheSizeInBytes = 256 * 1024;

        /// <summary>
        ///  low memory mode or not.
        /// </summary>
        public const int LowMemoryMode = 6;

        /// <summary>
        /// route start icon.
        /// </summary>
        public const int RouteStartIcon = 7;

        /// <summary>
        ///  route middle icon.
        /// </summary>
        public const int RouteMiddleIcon = 8;

        /// <summary>
        /// route end icon.
        /// </summary>
        public const int RouteEndIcon = 9;

        /// <summary>
        /// route draw pen.
        /// </summary>
        public const int RouteDrawPen = 10;

        /// <summary>
        /// is mark supported or not.
        /// </summary>
        public const int IsMarkSupported = 11;

        /// <summary>
        /// ignore the map type in stored map or not.
        /// </summary>
        public const int IgnoreMapTypeForStoredMap = 12;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the parameter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void SetParameter(int field, bool value)
        {
            switch (field)
            {
                case IsCacheOn:
                    _isCacheOn = value;
                    break;
                case LowMemoryMode:
                    _lowMemoryMode = value;
                    break;
                case IsMarkSupported:
                    _isMarkSupported = value;
                    break;
                case IgnoreMapTypeForStoredMap:
                    _ignoreMapTypeInStoredMap = value;
                    break;
            }
        }

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the parameter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        public static void SetParameter(int field, int value)
        {
            switch (field)
            {
                case WorkerThreadNumber:
                    if (value < 0 || value > 64)
                    {
                        throw new ArgumentException("Thread no should between 1 and 64");
                    }
                    _workerThreadNumber = value;
                    break;
                case MapCacheSizeInBytes:
                    if (value < 0 && _isCacheOn)
                    {
                        throw new ArgumentException("Cache size shall be great than 0");
                    }
                    _mapCacheSizeInBytes = value * 1024;
                    break;
            }
        }


        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the parameter.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        public static void SetParameter(int field, object value)
        {
            switch (field)
            {
                case RouteStartIcon:
                    if (value is Brush)
                    {
                        _startIconBrush = (Brush)value;
                    }
                    break;
                case RouteMiddleIcon:
                    if (value is Brush)
                    {
                        _middleIconBrush = (Brush)value;
                    }
                    break;
                case RouteEndIcon:
                    if (value is Brush)
                    {
                        _endIconBrush = (Brush)value;
                    }
                    break;
                case RouteDrawPen:
                    if (value is Pen)
                    {
                        _routePen = (Pen)value;
                    }
                    break;


            }
        }

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the parameters.
        /// </summary>
        /// <param name="pen">The pen.</param>
        /// <param name="start">The start.</param>
        /// <param name="middle">The middle.</param>
        /// <param name="end">The end.</param>
        public static void SetParameters(Pen pen, Brush start,
            Brush middle, Brush end)
        {
            if (pen != null)
            {
                _routePen = pen;
            }
            if (start != null)
            {
                _startIconBrush = start;
            }
            if (middle != null)
            {
                _middleIconBrush = middle;
            }
            if (end != null)
            {
                _endIconBrush = end;
            }

        }

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Reset map configuration parameters. the resetting parameters should before
        /// the initialization of DigitalMap ,and MapTileDownloaderManager.
        /// </summary>
        /// <param name="cacheOn">if cache is on, Digital Map will appy an internal cache
        /// which can save some loaded map tile to speed up map rendering, but it'll
        /// consume some memory whose max size is speicfied by cachesize.</param>
        /// <param name="workerThreadNo">how many worker thread,default is 4, these threads
        /// are in charge of downloading/reading/render map tiles from server,stored
        /// map tile file or vector map file.the thread no should between 1 and 64.</param>
        /// <param name="cacheSize">the max size of internal map tile caches.</param>
        /// <param name="isLowMemory">low memory mode or not</param>
        /// <param name="directionRenderBlocks">when render direction, it uses an internal
        ///  vector picture engine to draw the polyline, which requires memory ,the
        /// memory size is determined by the block size, the default render picture
        /// size is 256X256 ,which takes 256X256X4 bytes(256K), for memory constraints
        /// device, speicify a small block size requires smaller memory useage. but
        /// it effects the render performace,the valid value for directionRenderBlocks
        /// is 1,2,4, whose corrosponing block size is 256X256X4 bytes(256K)(default)
        /// 128X128X4 bytes(64K) and 64X64X4 bytes(16K)</param>
        public static void SetParameters(bool cacheOn,
            int workerThreadNo,
            long cacheSize,
            bool isLowMemory,
            int directionRenderBlocks)
        {
            _isCacheOn = cacheOn;
            _lowMemoryMode = isLowMemory;
            if (workerThreadNo < 0 || workerThreadNo > 64)
            {
                throw new ArgumentException("Thread no should between 1 and 64");
            }
            _workerThreadNumber = workerThreadNo;
            if (cacheSize < 0 && cacheOn)
            {
                throw new ArgumentException("Cache size shall be great than 0");
            }
            _mapCacheSizeInBytes = cacheSize;
            if (!(directionRenderBlocks == 1 || directionRenderBlocks == 2 ||
                    directionRenderBlocks == 4))
            {
                throw new ArgumentException("block size should be 1, or 2, or 4");
            }
            _mapDirectionRenderBlocks = directionRenderBlocks;
        }

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapConfiguration"/> class.
        /// </summary>
        private MapConfiguration()
        {

        }

        /**
         * is mark supported not.
         */
        internal static bool _isMarkSupported;

        /**
         * is cache on not.
         */
        internal static bool _isCacheOn = true;

        /**
         * the no of worker thread.
         */
        internal static int _workerThreadNumber = 4;

        /**
         * the map cache size.
         */
        internal static long _mapCacheSizeInBytes = 256 * 1024;

        /**
         * the direction render size = 256/MAP_DIRECTION_RENDER_BLOCKS;
         * this will always be 1 for new version. if low memory ,it should
         * disable the drawRouting, and use native graphics to draw routes.
         */
        internal static int _mapDirectionRenderBlocks = 1;

        /**
         * is low memory mode,
         */
        internal static bool _lowMemoryMode;

        /**
         * default route drawing pen.
         */
        internal static Pen _routePen = new Pen(new Color(0x7F00FF00, false), 4);

        /**
         * start route icon.
         */
        internal static Brush _startIconBrush;

        /**
         * start route icon.
         */
        internal static Brush _middleIconBrush;


        /**
         * start route icon.
         */
        internal static Brush _endIconBrush;

        /**
         * ignore map type in stored map or not.
         */
        internal static bool _ignoreMapTypeInStoredMap = true;

    }

}
