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
using System.Collections;
using System.IO;
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
    /// This class is used to store driving directions results
    /// </summary>
    public sealed class MapTileStreamReader : MapTileAbstractReader
    {

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Open the map.
        /// </summary>
        public void Open()
        {
            lock (_mapTiledZones)
            {
                int layerCount = _mapTiledZones.Count;
                if (layerCount > 0)
                {
                    ((MapTiledZone)_mapTiledZones[0]).Open();
                    _bounds = ((MapTiledZone)_mapTiledZones[0]).Bounds;
                }
                else
                {
                    _bounds = new GeoLatLngBounds();
                }
                for (int i = 1; i < layerCount; i++)
                {
                    MapTiledZone mapTiledZone = (MapTiledZone)_mapTiledZones[i];
                    mapTiledZone.Open();
                    GeoBounds.Union(mapTiledZone.Bounds, _bounds, _bounds);
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
        /// close the map.
        /// </summary>
        public void Close()
        {
            lock (_mapTiledZones)
            {
                int layerCount = _mapTiledZones.Count;
                for (int i = 1; i < layerCount; i++)
                {
                    MapTiledZone mapTiledZone = (MapTiledZone)_mapTiledZones[i];
                    mapTiledZone.Close();
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
        /// Add a map zone to map's zone collection.
        /// </summary>
        /// <param name="mapZone">a map zone to add.</param>
        public void AddZone(MapTiledZone mapZone)
        {
            lock (_mapTiledZones)
            {
                if (!_mapTiledZones.Contains(mapZone))
                {
                    mapZone._readListener = readListener;
                    _mapTiledZones.Add(mapZone);
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
        /// Sets the map downloading listener.
        /// </summary>
        /// <param name="listener">The listener.</param>
        public override void SetMapDownloadingListener(IReaderListener listener)
        {
            base.SetMapDownloadingListener(listener);
            lock (_mapTiledZones)
            {
                for (int i = 0; i < _mapTiledZones.Count; i++)
                {
                    ((MapTiledZone)_mapTiledZones[i])._readListener
                            = listener;
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
        /// return map zone object of given index.
        /// </summary>
        /// <param name="index">index of the map zone.</param>
        /// <returns>map zone object of given index</returns>
        public MapTiledZone GetMapZone(int index)
        {
            lock (_mapTiledZones)
            {
                MapTiledZone mapZone = null;
                if (index >= 0 && index < _mapTiledZones.Count)
                {
                    mapZone = (MapTiledZone)_mapTiledZones[index];
                }
                return mapZone;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// return the count of zones in the map..
        /// </summary>
        /// <returns>the number of map zones in the map zone collection</returns>
        public int GetZoneCount()
        {
            lock (_mapTiledZones)
            {
                return _mapTiledZones.Count;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Inserts the specified map zone to map's zone collection at the
        /// specified index. Each map zone in map's zone collection  with an index
        /// greater or equal to the specified index is shifted upward to have an
        /// index one greater than the value it had previously.
        /// </summary>
        /// <param name="mapZone">the map zone to insert.</param>
        /// <param name="index">where to insert the new map zone.</param>
        public void InsertZone(MapTiledZone mapZone, int index)
        {
            lock (_mapTiledZones)
            {
                if (!_mapTiledZones.Contains(mapZone))
                {
                    _mapTiledZones.Insert(index, mapZone);
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
        ///  Moves a zone in the Zone collection to change the order in which
        /// zones are drawn.
        /// </summary>
        /// <param name="from">Index number of the zone to move. The topmost zone is 0.</param>
        /// <param name="to">New location for the zone. For example, if you want it to be
        ///  the second zone, use 1</param>
        public void MoveZone(int from, int to)
        {
            lock (_mapTiledZones)
            {
                if (from < 0 || from >= _mapTiledZones.Count ||
                        to < 0 || to >= _mapTiledZones.Count)
                {
                    return;
                }
                MapTiledZone mapZoneFrom = (MapTiledZone)_mapTiledZones[from];
                MapTiledZone mapZoneTo = (MapTiledZone)_mapTiledZones[to];
                _mapTiledZones[from] = mapZoneTo;
                _mapTiledZones[to] = mapZoneFrom;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Remove a map zone from map's zone collection.
        /// </summary>
        /// <param name="mapZone"> map zone to be removed.</param>
        public void RemoveZone(MapTiledZone mapZone)
        {
            lock (_mapTiledZones)
            {
                if (_mapTiledZones.Contains(mapZone))
                {
                    _mapTiledZones.Remove(mapZone);
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
        /// Remove all map zones from map's zone collection.
        /// </summary>
        public void RemoveAllZones()
        {
            lock (_mapTiledZones)
            {
                _mapTiledZones.Clear();
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the map zone collection.
        /// </summary>
        /// <returns>the map zone collection.</returns>
        public ArrayList GetMapZones()
        {
            return _mapTiledZones;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get image at given location. when the reader is done, imageArray shall
        /// store the image byte array. imageArraySize is the actually data size.
        /// isImagevalid indicate the data is valid or not. normally this shall be
        /// an async call.
        /// </summary>
        /// <param name="mtype">the map type of the map tile.</param>
        /// <param name="x">the x index of the map tile.</param>
        /// <param name="y">the y index of the map tile.</param>
        /// <param name="zoomLevel">the zoom level of the map tile.</param>
        public override void GetImage(int mtype, int x, int y, int zoomLevel)
        {
            byte[] imgBuffer = null;
            try
            {
                lock (_mapTiledZones)
                {
                    int zoneCount = _mapTiledZones.Count;
                    for (int i = 0; i < zoneCount; i++)
                    {
                        MapTiledZone mapTiledZone
                                = (MapTiledZone)_mapTiledZones[i];
                        imgBuffer = mapTiledZone.GetImage(zoomLevel, x, y);
                        if (imgBuffer != null)
                        {
                            break;
                        }
                    }
                }

            }
            catch (IOException )
            {

            }
            if (imgBuffer == null)
            {
                IsImagevalid = false;
                ImageArray = null;

            }
            else
            {
                ImageArray = imgBuffer;
                IsImagevalid = true;
                ImageArraySize = ImageArray.Length;
            }

        }

        /**
         * map zones object.
         */
        private readonly ArrayList _mapTiledZones = new ArrayList();
        /**
         * the boundary of this map.
         */
        private GeoLatLngBounds _bounds;


    }

}
