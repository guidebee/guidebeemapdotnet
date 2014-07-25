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
using Mapdigit.Gis.Geometry;

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
    /// MapLayer defines a map layer.Computer maps are organized into layers. Think 
    /// of the layers as transparencies that are stacked on top of one another. Each
    /// layer contains different aspects of the entire map. Each layer contains 
    /// different map objects, such as regions, points, lines and text. 
    /// </summary>
    public sealed class MapFeatureLayer
    {

        /// <summary>
        ///  Predominant feature type is Point.
        /// </summary>
        public const int FeatureTypePoint = MapObject.TypePoint;

        /// <summary>
        ///  Predominant feature type is polyline.
        /// </summary>
        public const int FeatureTypePline = MapObject.TypePline;

        /// <summary>
        ///  Predominant feature type is region.
        /// </summary>
        public const int FeatureTypeRegion = MapObject.TypeReginRegion;

        /// <summary>
        /// This property controls whether a layer is automatically labeled. In 
        /// order for a label to be displayed automatically, its centroid must be 
        /// within the viewable map area. This is a Boolean value, and its default
        /// is true.
        /// </summary>
        public bool AutoLabel = true;

        /// <summary>
        /// font color.
        /// </summary>
        public int FontColor;

        /// <summary>
        /// Name of the font.
        /// </summary>
        public string FontName;

        /// <summary>
        /// name of the layer
        /// </summary>
        public string LayerName;

        /// <summary>
        /// A Rectangle object representing the geographic extents 
        /// (i.e., the minimum bounding rectangle) of all objects in the layer.
        /// </summary>
        public GeoLatLngBounds Bounds;

        /// <summary>
        /// The tabular data table object associated with this map layer.
        /// </summary>
        public DataTable DataTable;

        /// <summary>
        /// This string property identifies the column (field) name in the layer's
        /// tabular table that will be the name property of a feature object. 
        /// It currently defaults to the first column in the layer's table or the 
        /// column with name as "Name" if there is any.
        /// </summary>
        public DataField KeyField;

        /// <summary>
        /// Predominant feature type. can be POINT,PLINE or REGION.
        /// </summary>
        public int PredominantFeatureType;

        /// <summary>
        /// Is this map layer visible.
        /// </summary>
        public bool Visible = true;

        /// <summary>
        /// Description for this mapLayer.
        /// </summary>
        public string Description = "";

        /// <summary>
        /// This controls whether the layer is zoom layered. Zoom layering controls 
        /// the range of zoom levels (Distance across map) for which the layer is 
        /// displayed. If Zoom Layering is on, then the values stored in the zoomMax 
        /// and zoomMin properties are used. This is a Boolean value, and the default
        /// is false.
        /// </summary>
        public bool ZoomLevel;

        /// <summary>
        /// If ZoomLayering is on (zoomLevel=true), then this specifies 
        /// the maximum zoom value for which a layer will be drawn on the map.
        /// This takes a double value specifying Distance in Map units (Map.mapUnit).
        /// </summary>
        public double ZoomMax;

        /// <summary>
        /// If ZoomLayering is on (zoomLevel=true), then this specifies 
        /// the minimum zoom value for which a layer will be drawn on the map. 
        /// This takes a double value specifying Distance in Map units (Map.MapUnit).
        /// </summary>
        public double ZoomMin;


        /**
         * the index of the key field.
         */
        private int _keyFieldIndex;

        /**
         * the map file related to this map layer.
         */
        private readonly MapFile.MapFile _mapFile;

        private volatile bool _opened;
        /**
         * MapObject internal cache.
         */
        private readonly Hashtable _mapObjectCache = new Hashtable(CacheSize);

        /**
         * the internal cache size
         */
        private const int CacheSize = 256;


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapFeatureLayer"/> class.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public MapFeatureLayer(BinaryReader reader)
        {
            _mapFile = new MapFile.MapFile(reader);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Opens this map layer.
        /// </summary>
        public void Open()
        {
            if (!_opened)
            {
                _opened = true;
                _mapFile.Open();
                DataTable = new DataTable(_mapFile.TabularData, _mapFile.Header.Fields,
                        _mapFile.Header.RecordCount);
                int foundName = -1;
                for (int i = 0; i < _mapFile.Header.Fields.Length; i++)
                {
                    KeyField = _mapFile.Header.Fields[i];
                    if (KeyField.GetName().ToLower().StartsWith("name"))
                    {
                        foundName = i;
                        _keyFieldIndex = i;
                        break;
                    }
                }
                if (foundName == -1)
                {
                    for (int i = 0; i < _mapFile.Header.Fields.Length; i++)
                    {
                        KeyField = _mapFile.Header.Fields[i];
                        if (KeyField.GetFieldType() == DataField.TypeChar)
                        {
                            foundName = i;
                            _keyFieldIndex = i;
                            break;
                        }
                    }
                }
                if (foundName == -1)
                {
                    KeyField = _mapFile.Header.Fields[0];
                    _keyFieldIndex = 0;
                }
                if (_mapFile.Header.DominantType.ToUpper().Equals("POINT"))
                {
                    PredominantFeatureType = FeatureTypePoint;
                }
                else if (_mapFile.Header.DominantType.ToUpper().Equals("PLINE"))
                {
                    PredominantFeatureType = FeatureTypePline;
                }
                else
                {
                    PredominantFeatureType = FeatureTypeRegion;
                }
                Bounds = _mapFile.Header.MapBounds;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Closes this map layer.
        /// </summary>
        public void Close()
        {
            if (_opened)
            {
                _mapFile.Close();
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// get MapFeature at given mapInfoID.
        /// </summary>
        /// <param name="mapInfoId">The map info id.</param>
        /// <returns>MapFeature at given mapInfoID</returns>
        public MapFeature GetMapFeatureById(int mapInfoId)
        {
            MapFeature mapFeature;
            int mapObjectIdKey = mapInfoId;

            if (_mapObjectCache.ContainsKey(mapObjectIdKey))
            {
                mapFeature = (MapFeature)_mapObjectCache[mapObjectIdKey];
            }
            else
            {
                mapFeature = new MapFeature();
                mapFeature.MapInfoId = mapInfoId;
                mapFeature.MapObject = GetMapObjectById(mapInfoId);
                mapFeature.DataRowValue = GetDataRowValueById(mapInfoId);
                mapFeature.MapObject.Name =
                        mapFeature.DataRowValue.GetString(_keyFieldIndex);
                StoreToCache(mapObjectIdKey, mapFeature);
            }

            return mapFeature;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the map object by id.
        /// </summary>
        /// <param name="mapInfoId">The map info id.</param>
        /// <returns> MapObject at given mapInfoID</returns>
        public MapObject GetMapObjectById(int mapInfoId)
        {
            return _mapFile.GetMapObject(mapInfoId);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// get tabular record at given mapInfo ID.
        /// </summary>
        /// <param name="mapInfoId">The map info id.</param>
        /// <returns>tabular record at given mapInfoID</returns>
        public DataRowValue GetDataRowValueById(int mapInfoId)
        {
            return _mapFile.GetDataRowValue(mapInfoId);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// get all records based on search condition.
        /// </summary>
        /// <param name="findConditions">The find conditions.</param>
        /// <returns>a hashtable of all matched record.the key is the mapInfo ID. the
        ///  value is the matched string</returns>
        public Hashtable Search(FindConditions findConditions)
        {
            return _mapFile.Search(findConditions);
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
        /// <param name="rectGeo">the boundary</param>
        /// <returns>a hashtable of all matched record.the key is the mapInfo ID. the
        /// value is the matched MapObject's MBR</returns>
        public Hashtable Search(GeoLatLngBounds rectGeo)
        {
            return _mapFile.SearchMapObjectsInRect(rectGeo);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the record count.
        /// </summary>
        /// <returns> the total record number</returns>
        public int GetRecordCount()
        {
            return _mapFile.GetRecordCount();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determines whether this instance [can be shown] the specified screen width distance.
        /// </summary>
        /// <param name="screenWidthDistance">The screen width distance.</param>
        /// <returns>
        /// 	<c>true</c> if this instance [can be shown] the specified screen 
        /// width distance; otherwise, <c>false</c>.
        /// </returns>
        internal bool CanBeShown(double screenWidthDistance)
        {
            bool isShown = Visible;
            if (ZoomLevel)
            {
                if (!(screenWidthDistance >= ZoomMin &&
                        screenWidthDistance <= ZoomMax))
                {
                    isShown = false;
                }
            }
            return isShown;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Store the mapFeature to cache.
        /// </summary>
        /// <param name="mapObjectIdKey">The map object id key.</param>
        /// <param name="mapFeature">The map feature.</param>
        private void StoreToCache(int mapObjectIdKey, MapFeature mapFeature)
        {
            if (_mapObjectCache.Count >= CacheSize)
            {
                MapFeature[] mapFeatures = new MapFeature[CacheSize];
                ICollection enuValues = _mapObjectCache.Values;
                int index = 0;
                foreach (var o in enuValues)
                {
                    mapFeatures[index++] = (MapFeature)o;
                }

                SortMapFeature(mapFeatures);
                for (int i = 0; i < CacheSize / 2; i++)
                {
                    int deleteMapObjectId = mapFeatures[i].MapInfoId;
                    _mapObjectCache.Remove(deleteMapObjectId);
                }

            }
            _mapObjectCache.Add(mapObjectIdKey, mapFeature);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sorts the map feature.
        /// </summary>
        /// <param name="mapFeatures">The map features.</param>
        private static void SortMapFeature(MapFeature[] mapFeatures)
        {
            int n = mapFeatures.Length;
            int i, j;
            MapFeature ai;

            for (i = 1; i < n; i++)
            {
                j = i - 1;
                ai = mapFeatures[i];
                while (mapFeatures[j].MapObject.CacheAccessTime >
                        ai.MapObject.CacheAccessTime)
                {
                    mapFeatures[j + 1] = mapFeatures[j];
                    j--;
                    if (j < 0) break;
                }
                mapFeatures[j + 1] = ai;
            }
        }
    }

}
