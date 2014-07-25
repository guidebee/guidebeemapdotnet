//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 27SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System.Collections;
using Mapdigit.Gis.Geometry;
using Mapdigit.Gis.Drawing;

//--------------------------------- PACKAGE ------------------------------------

namespace Mapdigit.Gis
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 27SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// MapLayerContainer is a collection of map layers.
    /// </summary>
    public abstract class MapLayerContainer : MapLayer
    {

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Add a map layer to the tail of the container
        /// </summary>
        /// <param name="mapLayer">The map layer.</param>
        public void AddMapLayer(MapLayer mapLayer)
        {
            lock (mapLayers)
            {
                if (!mapLayers.Contains(mapLayer))
                {
                    mapLayers.Add(mapLayer);
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Add a map layer after given index
        /// </summary>
        /// <param name="index">the index after which a new map layer is added.</param>
        /// <param name="mapLayer">a map layer inserted into the container.</param>
        public void AddMapLayerAt(int index, MapLayer mapLayer)
        {
            lock (mapLayers)
            {
                mapLayers.Insert(index, mapLayer);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Removes all map layers.
        /// </summary>
        public void RemoveAllMapLayers()
        {
            lock (mapLayers)
            {
                mapLayers.Clear();
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// remove a map layer at given index
        /// </summary>
        /// <param name="index">the index of the map layer.</param>
        public void RemoveMapLayerAt(int index)
        {
            lock (mapLayers)
            {
                mapLayers.RemoveAt(index);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// remove a givne map layer from the container.
        /// </summary>
        /// <param name="mapLayer">The map layer.</param>
        public void RemoveMapLayer(MapLayer mapLayer)
        {
            lock (mapLayers)
            {
                mapLayers.Remove(mapLayer);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get a map layer at given index.
        /// </summary>
        /// <param name="index">the index of the map layer.</param>
        /// <returns>the map layer at given index.</returns>
        public MapLayer GetMapLayerAt(int index)
        {
            lock (mapLayers)
            {
                return (MapLayer)mapLayers[index];
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get all map layers as an array.
        /// </summary>
        /// <returns> all map layers included in this container</returns>
        public MapLayer[] GetMapLayers()
        {
            lock (mapLayers)
            {
                MapLayer[] retArray = new MapLayer[mapLayers.Count];
                mapLayers.CopyTo(retArray);
                return retArray;
            }
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the count of the map layers in this container.
        /// </summary>
        /// <returns>the count of the map layers</returns>
        public int GetMapLayerCount()
        {
            lock (mapLayers)
            {
                return mapLayers.Count;
            }
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draw the map layer to an graphics.
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
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draw the map layer to an graphics
        /// </summary>
        /// <param name="graphics">the graphics object where the map is drawn..</param>
        /// <param name="offsetX">The offset X.</param>
        /// <param name="offsetY">The offset Y.</param>
        public override void Paint(IGraphics graphics, int offsetX, int offsetY)
        {
            lock (mapLayers)
            {
                for (int i = 0; i < mapLayers.Count; i++)
                {
                    MapLayer mapLayer = (MapLayer)mapLayers[i];
                    mapLayer.Paint(graphics, offsetX, offsetY);
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Starts a pan with given distance in pixels.
        /// directions. +1 is right and down, -1 is left and up, respectively.
        /// </summary>
        /// <param name="dx">x offset</param>
        /// <param name="dy">y offset.</param>
        public override void PanDirection(int dx, int dy)
        {
            lock (mapLayers)
            {
                for (int i = 0; i < mapLayers.Count; i++)
                {
                    MapLayer mapLayer = (MapLayer)mapLayers[i];
                    mapLayer.PanDirection(dx, dy);
                }
            }
            base.PanDirection(dx, dy);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Changes the center point of the map to the given point.
        /// </summary>
        /// <param name="center">a new center point of the map.</param>
        public override void PanTo(GeoLatLng center)
        {
            lock (mapLayers)
            {
                for (int i = 0; i < mapLayers.Count; i++)
                {
                    MapLayer mapLayer = (MapLayer)mapLayers[i];
                    mapLayer.PanTo(center);
                }
            }
            base.PanTo(center);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the map view to the given center.
        /// </summary>
        /// <param name="center">the center latitude,longitude of the map..</param>
        /// <param name="zoomLevel">the zoom Level of the map [0,17].</param>
        public override void SetCenter(GeoLatLng center, int zoomLevel)
        {
            lock (mapLayers)
            {
                for (int i = 0; i < mapLayers.Count; i++)
                {
                    MapLayer mapLayer = (MapLayer)mapLayers[i];
                    mapLayer.SetCenter(center, zoomLevel);
                }
            }
            base.SetCenter(center, zoomLevel);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Increments zoom level by one.
        /// </summary>
        public override void ZoomIn()
        {
            lock (mapLayers)
            {
                for (int i = 0; i < mapLayers.Count; i++)
                {
                    MapLayer mapLayer = (MapLayer)mapLayers[i];
                    mapLayer.ZoomIn();
                }
            }
            base.ZoomIn();
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Decrements zoom level by one.
        /// </summary>
        public override void ZoomOut()
        {
            lock (mapLayers)
            {
                for (int i = 0; i < mapLayers.Count; i++)
                {
                    MapLayer mapLayer = (MapLayer)mapLayers[i];
                    mapLayer.ZoomOut();
                }
            }
            base.ZoomOut();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the zoom level to the given new value.
        /// </summary>
        /// <param name="level">new map zoom level</param>
        public override void SetZoom(int level)
        {
            lock (mapLayers)
            {
                for (int i = 0; i < mapLayers.Count; i++)
                {
                    MapLayer mapLayer = (MapLayer)mapLayers[i];
                    mapLayer.SetZoom(level);
                }
            }
            base.SetZoom(level);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Resize the map to a level that include given bounds
        /// </summary>
        /// <param name="bounds">new bound</param>
        public override void Resize(GeoLatLngBounds bounds)
        {
            lock (mapLayers)
            {
                for (int i = 0; i < mapLayers.Count; i++)
                {
                    MapLayer mapLayer = (MapLayer)mapLayers[i];
                    mapLayer.Resize(bounds);
                }
            }
            base.Resize(bounds);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set the size for the map.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public override void SetMapSize(int width, int height)
        {
            lock (mapLayers)
            {
                for (int i = 0; i < mapLayers.Count; i++)
                {
                    MapLayer mapLayer = (MapLayer)mapLayers[i];
                    mapLayer.SetMapSize(width, height);
                }
            }
            base.SetMapSize(width, height);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the size of the screen.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public override void SetScreenSize(int width, int height)
        {
            lock (mapLayers)
            {
                for (int i = 0; i < mapLayers.Count; i++)
                {
                    MapLayer mapLayer = (MapLayer)mapLayers[i];
                    mapLayer.SetScreenSize(width, height);
                }
            }
            base.SetScreenSize(width, height);
        }

        /// <summary>
        /// ArrayList store all map layers.
        /// </summary>
        protected ArrayList mapLayers = new ArrayList();

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapLayerContainer"/> class.
        /// </summary>
        /// <param name="width">the width of the map layer</param>
        /// <param name="height">the height of the map layer</param>
        protected MapLayerContainer(int width, int height)
            : base(width, height)
        {

        }

    }

}
