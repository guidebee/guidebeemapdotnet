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
using Mapdigit.Gis.Geometry;
using Mapdigit.Gis.Raster;

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
    /// VectorMap is the basic building blocks for Guidebee local map. Each map is
    /// consists of multiple map Layers.
    /// </summary>
    public class VectorMap : RasterMap
    {

        private GeoSet _geoSet;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="VectorMap"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="mapTileDownloadManager">The map tile download manager.</param>
        /// <param name="geoSet">The geo set.</param>
        public VectorMap(int width, int height,
                MapTileDownloadManager mapTileDownloadManager, GeoSet geoSet)
            : base(width, height, mapTileDownloadManager)
        {

            _geoSet = geoSet;

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the geo set.
        /// </summary>
        /// <param name="geoSet">The geo set.</param>
        public void SetGeoSet(GeoSet geoSet)
        {
            _geoSet = geoSet;

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the type of the map.
        /// </summary>
        /// <returns>the map type, always MapType.MAPINFOVECTORMAP</returns>
        public override int GetMapType()
        {
            return MapType.MapinfoVectorMap;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the map objects in the screen area whose center is given point
        /// </summary>
        /// <returns> the map ojectes in the screen area</returns>
        public Hashtable[] GetScreenObjects()
        {
            return GetScreenObjects(mapCenterPt);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the map objects in the screen area whose center is given point
        /// </summary>
        /// <param name="pt"> center of the screen.</param>
        /// <returns>the map objects in the screen area</returns>
        public Hashtable[] GetScreenObjects(GeoLatLng pt)
        {
            mapCenterPt.X = pt.X;
            mapCenterPt.Y = pt.Y;
            GeoLatLngBounds rectGeo = GetScreenBounds(mapCenterPt);
            return _geoSet.Search(rectGeo);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Adds the map feature layer.
        /// </summary>
        /// <param name="mapLayer">The map layer.</param>
        public void AddMapFeatureLayer(MapFeatureLayer mapLayer)
        {
            if (_geoSet != null)
            {
                _geoSet.AddMapFeatureLayer(mapLayer);
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
            if (_geoSet != null)
            {
                _geoSet.GetMapFeatureLayer(index);
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
        /// Gets the map feature layer count.
        /// </summary>
        /// <returns></returns>
        public int GetMapFeatureLayerCount()
        {
            if (_geoSet != null)
            {
                return _geoSet.GetMapFeatureLayerCount();
            }
            return 0;
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
        /// <param name="index">The index.</param>
        public void InsertMapFeatureLayer(MapFeatureLayer mapLayer, int index)
        {
            if (_geoSet != null)
            {
                _geoSet.InsertMapFeatureLayer(mapLayer, index);
            }
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Moves the map feature layer.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        public void MoveMapFeatureLayer(int from, int to)
        {
            if (_geoSet != null)
            {
                _geoSet.MoveMapFeatureLayer(from, to);
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
            if (_geoSet != null)
            {
                _geoSet.RemoveMapFeatureLayer(mapLayer);
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
            if (_geoSet != null)
            {
                _geoSet.RemoveAllMapFeatureLayers();
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
            if (_geoSet != null)
            {
                return _geoSet.GetMapFeatureLayers();
            }
            return null;
        }

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// get all records based on given string. the seach will search based on
        ///  map layer's key field.
        /// </summary>
        /// <param name="matchString">The match string.</param>
        /// <returns>a hashtable array contains of all matched record.
        ///  the key is the mapInfo ID. the value is the matched string</returns>
        public Hashtable[] Search(string matchString)
        {
            if (_geoSet != null)
            {
                return _geoSet.Search(matchString);
            }
            return null;
        }

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Searches the specified rect geo.
        /// </summary>
        /// <param name="rectGeo">The rect geo.</param>
        /// <returns></returns>
        public Hashtable[] Search(GeoLatLngBounds rectGeo)
        {
            if (_geoSet != null)
            {
                return _geoSet.Search(rectGeo);
            }
            return null;
        }

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Searches the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="findConditions">The find conditions.</param>
        /// <returns></returns>
        public Hashtable Search(int index, FindConditions findConditions)
        {

            if (_geoSet != null)
            {
                return _geoSet.Search(index, findConditions);
            }
            return null;
        }

        /////////////////////////////////////////////////////////////////////////////
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

            if (_geoSet != null)
            {
                return _geoSet.Search(index, rectGeo);
            }
            return null;
        }


        /////////////////////////////////////////////////////////////////////////////
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
            if (_geoSet != null)
            {
                return _geoSet.GetBounds();
            }
            return new GeoLatLngBounds();
        }

    }

}
