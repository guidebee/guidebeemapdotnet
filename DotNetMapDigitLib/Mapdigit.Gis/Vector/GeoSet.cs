//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 02OCT2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System.Collections;
using System.IO;
using Mapdigit.Gis.Drawing;
using Mapdigit.Gis.Geometry;
using Mapdigit.Util;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Gis.Vector
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 02OCT2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// This class is used to store driving directions results
    /// </summary>
    public sealed class GeoSet
    {

        /**
         * map unit is in miles.
         */
        private const int MapUnitMile = 0;
        /**
         * map unit is in kilometer.
         */
        private const int MapUnitKm = 1;
        /**
         * map unit.
         */
        private volatile int _mapUnit = MapUnitKm;

        /// <summary>
        /// default font Ex
        /// </summary>
        public volatile IFont FontEx;

        /// <summary>
        /// the backcolor for this map.
        /// </summary>
        public volatile int BackColor = 0xffffff;


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoSet"/> class.
        /// </summary>
        public GeoSet()
        {

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoSet"/> class.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public GeoSet(BinaryReader reader)
        {
            ReadGeoSet(reader);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Add a map layer to map's layer collection.
        /// </summary>
        /// <param name="mapLayer">The map layer.</param>
        public void AddMapFeatureLayer(MapFeatureLayer mapLayer)
        {
            lock (_mapFeatureLayers)
            {
                if (!_mapFeatureLayers.Contains(mapLayer))
                {
                    _mapFeatureLayers.Add(mapLayer);
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the map feature layer.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public MapFeatureLayer GetMapFeatureLayer(int index)
        {
            lock (_mapFeatureLayers)
            {
                MapFeatureLayer mapLayer = null;
                if (index >= 0 && index < _mapFeatureLayers.Count)
                {
                    mapLayer = (MapFeatureLayer)_mapFeatureLayers[index];
                }
                return mapLayer;
            }
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the map feature layer count.
        /// </summary>
        /// <returns></returns>
        public int GetMapFeatureLayerCount()
        {
            lock (_mapFeatureLayers)
            {
                return _mapFeatureLayers.Count;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Inserts the specified map layer to map's layer collection at the
        /// specified index. Each map layer in map's layer collection  with an index
        /// greater or equal to the specified index is shifted upward to have an
        /// index one greater than the value it had previously.
        /// </summary>
        /// <param name="mapLayer">The map layer.</param>
        /// <param name="index"> where to insert the new map layer.</param>
        public void InsertMapFeatureLayer(MapFeatureLayer mapLayer, int index)
        {
            lock (_mapFeatureLayers)
            {
                if (!_mapFeatureLayers.Contains(mapLayer))
                {
                    _mapFeatureLayers.Insert(index, mapLayer);
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///Moves a layer in the Layer collection to change the order in which
        /// layers are drawn.
        /// </summary>
        /// <param name="from">Index number of the layer to move. The topmost layer is 0.</param>
        /// <param name="to">new location for the layer. For example, if you want it to be
        ///  the second layer, use 1</param>
        public void MoveMapFeatureLayer(int from, int to)
        {
            lock (_mapFeatureLayers)
            {
                if (from < 0 || from >= _mapFeatureLayers.Count ||
                        to < 0 || to >= _mapFeatureLayers.Count)
                {
                    return;
                }
                MapFeatureLayer mapLayerFrom = (MapFeatureLayer)_mapFeatureLayers[from];
                MapFeatureLayer mapLayerTo = (MapFeatureLayer)_mapFeatureLayers[to];
                _mapFeatureLayers[from] = mapLayerTo;
                _mapFeatureLayers[to] = mapLayerFrom;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Removes the map feature layer.
        /// </summary>
        /// <param name="mapLayer">The map layer.</param>
        public void RemoveMapFeatureLayer(MapFeatureLayer mapLayer)
        {
            lock (_mapFeatureLayers)
            {
                if (_mapFeatureLayers.Contains(mapLayer))
                {
                    _mapFeatureLayers.Remove(mapLayer);
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Removes all map feature layers.
        /// </summary>
        public void RemoveAllMapFeatureLayers()
        {
            lock (_mapFeatureLayers)
            {
                _mapFeatureLayers.Clear();
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the map feature layers.
        /// </summary>
        /// <returns></returns>
        public MapFeatureLayer[] GetMapFeatureLayers()
        {
            lock (_mapFeatureLayers)
            {
                if (_mapFeatureLayers.Count > 0)
                {
                    MapFeatureLayer[] copiedFeatureLayers = new MapFeatureLayer[_mapFeatureLayers.Count];
                    _mapFeatureLayers.CopyTo(copiedFeatureLayers);
                    return copiedFeatureLayers;
                }

            }
            return null;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///get all records based on given string. the seach will search based on
        /// map layer's key field.
        /// </summary>
        /// <param name="matchString">The match string.</param>
        /// <returns>a hashtable array contains of all matched record.
        /// the key is the mapInfo ID. the value is the matched string.</returns>
        public Hashtable[] Search(string matchString)
        {
            lock (_mapFeatureLayers)
            {
                Hashtable[] retTable = new Hashtable[_mapFeatureLayers.Count];
                for (int i = 0; i < _mapFeatureLayers.Count; i++)
                {
                    MapFeatureLayer mapLayer = (MapFeatureLayer)_mapFeatureLayers[i];
                    FindConditions findConditions = new FindConditions();
                    findConditions.AddCondition(mapLayer.DataTable.GetFieldIndex(mapLayer.KeyField.GetName()), matchString);
                    retTable[i] = mapLayer.Search(findConditions);
                }
                return retTable;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// get all records based on given rectangle.
        /// </summary>
        /// <param name="rectGeo">The rect geo.</param>
        /// <returns>a hashtable array contains of all matched record.
        ///  the key is the mapInfo ID. the value is the MBR of map object.</returns>
        public Hashtable[] Search(GeoLatLngBounds rectGeo)
        {
            lock (_mapFeatureLayers)
            {
                Hashtable[] retTable = new Hashtable[_mapFeatureLayers.Count];
                GeoLatLng pt1 = new GeoLatLng(rectGeo.Y, rectGeo.X);
                GeoLatLng pt2 = new GeoLatLng(rectGeo.Y + rectGeo.Height,
                        rectGeo.X + rectGeo.Width);
                double distance = GeoLatLng.Distance(pt1, pt2);

                if (_mapUnit == MapUnitMile)
                {
                    distance /= 1.632;
                }

                for (int i = 0; i < _mapFeatureLayers.Count; i++)
                {
                    MapFeatureLayer mapLayer = (MapFeatureLayer)_mapFeatureLayers[i];
                    if (mapLayer.CanBeShown(distance))
                    {
                        retTable[i] = mapLayer.Search(rectGeo);
                    }
                    else
                    {
                        retTable[i] = new Hashtable();
                    }
                }
                return retTable;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Opens this geoset.
        /// </summary>
        public void Open()
        {
            lock (_mapFeatureLayers)
            {
                int layerCount = _mapFeatureLayers.Count;


                if (layerCount > 0)
                {
                    ((MapFeatureLayer)_mapFeatureLayers[0]).Open();
                    _bounds = ((MapFeatureLayer)_mapFeatureLayers[0]).Bounds;
                }
                else
                {
                    _bounds = new GeoLatLngBounds();
                }

                for (int i = 1; i < layerCount; i++)
                {
                    MapFeatureLayer mapLayer = (MapFeatureLayer)_mapFeatureLayers[i];
                    mapLayer.Open();
                    GeoBounds.Union(mapLayer.Bounds, _bounds, _bounds);
                }
            }

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Closes this geoset.
        /// </summary>
        public void Close()
        {
            lock (_mapFeatureLayers)
            {
                int layerCount = _mapFeatureLayers.Count;
                for (int i = 0; i < layerCount; i++)
                {
                    MapFeatureLayer mapLayer = (MapFeatureLayer)_mapFeatureLayers[i];
                    mapLayer.Close();
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// get all records based on search condition  in give map layer.
        /// </summary>
        /// <param name="index">the index of given map layer</param>
        /// <param name="findConditions">The find conditions.</param>
        /// <returns>a hashtable of all matched record.the key is the mapInfo ID</returns>
        public Hashtable Search(int index, FindConditions findConditions)
        {
            lock (_mapFeatureLayers)
            {
                MapFeatureLayer mapLayer = GetMapFeatureLayer(index);
                if (mapLayer != null)
                {
                    return mapLayer.Search(findConditions);
                }

                return null;
            }

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Searches the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="rectGeo">The rect geo.</param>
        /// <returns></returns>
        public Hashtable Search(int index, GeoLatLngBounds rectGeo)
        {
            lock (_mapFeatureLayers)
            {
                MapFeatureLayer mapLayer = GetMapFeatureLayer(index);
                if (mapLayer != null)
                {
                    return mapLayer.Search(rectGeo);
                }
                return null;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Reads the geo set.
        /// </summary>
        /// <param name="reader">The reader.</param>
        private void ReadGeoSet(BinaryReader reader)
        {
            if (reader == null)
            {
                throw new IOException("can not read from null reader!");
            }

            _mapFeatureLayerInfos.Clear();
            

            DataReader.Seek(reader, 0);
            //string fileVersion = DataReader.ReadString(reader);
            DataReader.Seek(reader, 16);
            string fileFormat = DataReader.ReadString(reader);
            DataReader.Seek(reader, 32);
            string pstType = DataReader.ReadString(reader);
            DataReader.Seek(reader, 48);
            if (!(fileFormat.ToUpper().Equals("JAVA") &&
                    pstType.ToUpper().Equals("PST")))
            {
                throw new IOException("Invalid file format!");
            }
            DataReader.ReadString(reader);
            DataReader.Seek(reader, 128);
            _mapUnit = DataReader.ReadInt(reader);
            DataReader.ReadDouble(reader);
            int mapLayerCount = DataReader.ReadInt(reader);

            for (int i = 0; i < mapLayerCount; i++)
            {
                DataReader.Seek(reader, i * 512 + 144);
                string layerName = DataReader.ReadString(reader);
                string description = DataReader.ReadString(reader);
                byte layerVisible = reader.ReadByte();
                double zoomMax = DataReader.ReadDouble(reader);
                double zoomMin = DataReader.ReadDouble(reader);
                MapFeatureLayerInfo mapLayerInfo = new MapFeatureLayerInfo();
                mapLayerInfo.Description = description;
                mapLayerInfo.LayerName = layerName;

                if (layerVisible == 1)
                {
                    mapLayerInfo.Visible = true;
                }
                else
                {
                    mapLayerInfo.Visible = false;
                }
                mapLayerInfo.ZoomMax = zoomMax;
                mapLayerInfo.ZoomMin = zoomMin;
                if (zoomMax == zoomMin && zoomMin < 0.001)
                {
                    mapLayerInfo.ZoomLevel = false;
                }
                else
                {
                    mapLayerInfo.ZoomLevel = true;
                }
                _mapFeatureLayerInfos.Add(mapLayerInfo);
            }
            reader.Close();
            if (_mapFeatureLayerInfos.Count > 0)
            {
                _layerNames = new string[_mapFeatureLayerInfos.Count];
                for (int i = 0; i < _mapFeatureLayerInfos.Count; i++)
                {
                    _layerNames[i] = ((MapFeatureLayerInfo)_mapFeatureLayerInfos[i]).LayerName;
                }
            }

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the map map feature layer info.
        /// </summary>
        /// <param name="layerName">Name of the layer.</param>
        /// <returns></returns>
        public MapFeatureLayerInfo GetMapMapFeatureLayerInfo(string layerName)
        {
            foreach(MapFeatureLayerInfo info in _mapFeatureLayerInfos)
            {
                if(info.LayerName.CompareTo(layerName)==0)
                {
                    return info;
                }
            }
            return null;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the map map feature layer.
        /// </summary>
        /// <param name="layerName">Name of the layer.</param>
        /// <returns></returns>
        public MapFeatureLayer GetMapMapFeatureLayer(string layerName)
        {
            foreach (MapFeatureLayer layer in _mapFeatureLayers)
            {
                if (layer.LayerName.CompareTo(layerName) == 0)
                {
                    return layer;
                }
            }
            return null;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the layer names.
        /// </summary>
        /// <returns></returns>
        public string []GetLayerNames()
        {
            string[] retString = new string[_layerNames.Length];
            System.Array.Copy(_layerNames, retString, retString.Length);
            return retString;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the bounds.
        /// </summary>
        /// <returns></returns>
        public GeoLatLngBounds GetBounds()
        {
            return new GeoLatLngBounds(_bounds);
        }
        /**
         * map Layers object.
         */
        private readonly ArrayList _mapFeatureLayers = new ArrayList();
        private readonly ArrayList _mapFeatureLayerInfos = new ArrayList();

        private string[] _layerNames;
        /**
         * the boundary of this map.
         */
        private GeoLatLngBounds _bounds;

        /// <summary>
        /// Map Layer Info.
        /// </summary>
        public class MapFeatureLayerInfo
        {

            /// <summary>
            /// support zoom Level
            /// </summary>
            public bool ZoomLevel;
            /// <summary>
            /// Visiable or not
            /// </summary>
            public bool Visible;
            /// <summary>
            /// max zoom 
            /// </summary>
            public double ZoomMax;
            /// <summary>
            /// min zoom 
            /// </summary>
            public double ZoomMin;
            /// <summary>
            /// name of the layer
            /// </summary>
            public string LayerName;
            /// <summary>
            /// Description
            /// </summary>
            public string Description;
            /// <summary>
            /// name of the font.
            /// </summary>
            public string FontName;
            /// <summary>
            /// color of the font.
            /// </summary>
            public int FontColor;
        }
    }

}
