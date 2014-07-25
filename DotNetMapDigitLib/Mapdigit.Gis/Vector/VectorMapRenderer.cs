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
using System;
using System.Collections;
using Mapdigit.Gis.Drawing;
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
    ///  Vector map render, each time, the renderer draw one map tile.
    /// </summary>
    internal class VectorMapRenderer : MapTileAbstractReader
    {

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="VectorMapRenderer"/> class.
        /// </summary>
        /// <param name="geoSet">The geo set.</param>
        public VectorMapRenderer(GeoSet geoSet)
        {
            _geoSet = geoSet;
            _vectorMapCanvas = new VectorMapCanvas();

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the color of the font.
        /// </summary>
        /// <param name="fontColor">Color of the font.</param>
        public void SetFontColor(int fontColor)
        {
            _vectorMapCanvas.SetFontColor(fontColor);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the font.
        /// </summary>
        /// <param name="font">The font.</param>
        public void SetFont(IFont font)
        {
            _vectorMapCanvas.SetFont(font);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the vector map canvas.
        /// </summary>
        /// <param name="vectorMapCanvas">The vector map canvas.</param>
        public void SetVectorMapCanvas(VectorMapAbstractCanvas vectorMapCanvas)
        {
            if (vectorMapCanvas != null)
            {
                _vectorMapCanvas = vectorMapCanvas;
            }

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
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
            lock (VectorMapAbstractCanvas.GraphicsMutex)
            {

                int shiftWidth = 32;
                GeoPoint pt1 = new GeoPoint(x * MapLayer.MapTileWidth - shiftWidth,
                        y * MapLayer.MapTileWidth - shiftWidth);
                GeoPoint pt2 = new GeoPoint((x + 1) * MapLayer.MapTileWidth + shiftWidth,
                        (y + 1) * MapLayer.MapTileWidth + shiftWidth);
                GeoLatLng latLng1 = MapLayer.FromPixelToLatLng(pt1, zoomLevel);
                GeoLatLng latLng2 = MapLayer.FromPixelToLatLng(pt2, zoomLevel);
                double minY = Math.Min(latLng1.Latitude, latLng2.Latitude);
                double maxY = Math.Max(latLng1.Latitude, latLng2.Latitude);
                double minX = Math.Min(latLng1.Longitude, latLng2.Longitude);
                double maxX = Math.Max(latLng1.Longitude, latLng2.Longitude);
                double width =0.00;
                double height = 0.00;

                //width = width < 0.06 ? 0.06 : width;
                //height = height < 0.06 ? 0.06 : height;

                GeoLatLngBounds geoBounds = new GeoLatLngBounds(minX - width / 2.0, minY - height/2.0,
                        maxX - minX + width , maxY - minY + height);
 
                try
                {
                    Hashtable[] mapFeatures = _geoSet.Search(geoBounds);
                    int totalSize = 0;
                    for (int i = 0; i < mapFeatures.Length; i++)
                    {
                        Hashtable mapFeaturesInLayer = mapFeatures[i];
                        totalSize += mapFeaturesInLayer.Count;
                    }
                    totalSize += 1;
                    int mapObjectIndex = 0;
                    _vectorMapCanvas.ClearCanvas(0xffffff);

                    for (int i = 0; i < mapFeatures.Length; i++)
                    {
                        int zOrder = mapFeatures.Length - 1 - i;
                        Hashtable mapFeaturesInLayer = mapFeatures[zOrder];
                        ICollection enuKeys = mapFeaturesInLayer.Keys;
                        MapFeatureLayer mapLayer = _geoSet.GetMapFeatureLayer(zOrder);

                        foreach (var o in enuKeys)
                        {
                            int mapInfoId = (int)o;
                            MapFeature mapFeature = mapLayer
                                    .GetMapFeatureById(mapInfoId);
                            mapObjectIndex++;
                            _vectorMapCanvas.SetFontColor(mapLayer.FontColor);
                            _vectorMapCanvas.DrawMapObject(mapFeature.MapObject,
                                    geoBounds, zoomLevel);
                            if (readListener != null)
                            {
                                readListener.ReadProgress(mapObjectIndex,
                                        totalSize);
                            }
                        }

                        
                    }
                    _vectorMapCanvas.DrawMapText();
                    ImageArray = PngEncoder.GetPngrgb(MapLayer.MapTileWidth,
                                                         MapLayer.MapTileWidth,
                                                         _vectorMapCanvas.GetRGB());
                    ImageArraySize = ImageArray.Length;
                    IsImagevalid = true;

                    if (ImageArraySize == 1933)
                    {
                        ImageArray = null;
                        IsImagevalid = false;
                        ImageArraySize = 0;

                    }
                   
                    
                    if (readListener != null)
                    {
                        readListener.ReadProgress(totalSize, totalSize);
                    }
                }
                catch (Exception)
                {

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
        /// a way app can cancel the reading process.
        /// </summary>
        public override void CancelRead()
        {
        }

        /**
         * Geoset as the database for renderer.
         */
        private readonly GeoSet _geoSet;
        /**
         * Vector map canvas.
         */
        private VectorMapAbstractCanvas _vectorMapCanvas;
    }

}
