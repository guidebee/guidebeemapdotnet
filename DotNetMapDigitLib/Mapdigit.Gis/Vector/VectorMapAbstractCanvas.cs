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
using Mapdigit.Gis.Drawing;
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
    /// Base class for vector map canvas.
    /// </summary>
    internal abstract class VectorMapAbstractCanvas
    {

        /// <summary>
        /// Graphics2D mutex.
        /// </summary>
        public readonly static object GraphicsMutex = new object();

        /**
         * default font color.
         */
        protected int fontColor;


        /// <summary>
        /// font for this map.
        /// </summary>
        protected IFont font;


        /**
         * current map zoom level
         */
        protected volatile int mapZoomLevel = 1;

        /**
         * the center of this map.
         */
        protected volatile GeoLatLng mapCenterPt = new GeoLatLng();


        /**
          * the size of the map size.
          */
        protected volatile GeoBounds mapSize = new GeoBounds();

        /**
         * SutherlandHodgman clip pline and region.
         */
        protected SutherlandHodgman sutherlandHodgman;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Computes the pixel coordinates of the given geographical point in the map.
        /// </summary>
        /// <param name="latlng">the geographical coordinates</param>
        /// <returns>the pixel coordinates in the map</returns>
        protected GeoPoint FromLatLngToMapPixel(GeoLatLng latlng)
        {
            GeoPoint center = MapLayer.FromLatLngToPixel(mapCenterPt, mapZoomLevel);
            GeoPoint topLeft = new GeoPoint(center.X - mapSize.Width / 2.0,
                    center.Y - mapSize.Height / 2.0);
            GeoPoint pointPos = MapLayer.FromLatLngToPixel(latlng, mapZoomLevel);
            pointPos.X -= topLeft.X;
            pointPos.Y -= topLeft.Y;
            return new GeoPoint((int)(pointPos.X + 0.5), (int)(pointPos.Y + 0.5));

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Computes the pixel coordinates of the given geographical point vector
        /// in the map.
        /// </summary>
        /// <param name="vpts">the geographical coordinates vector.</param>
        /// <returns>the pixel coordinates in the map</returns>
        protected GeoPoint[] FromLatLngToMapPixel(ArrayList vpts)
        {

            GeoPoint[] retPoints = new GeoPoint[vpts.Count];
            for (int i = 0; i < vpts.Count; i++)
            {
                retPoints[i] = FromLatLngToMapPixel(
                        (GeoLatLng)vpts[i]);
            }
            return retPoints;

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
        /// <param name="color">Color of the font.</param>
        public void SetFontColor(int color)
        {
            fontColor = color;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the Font.
        /// </summary>
        /// <param name="newFont">The font.</param>
        public void SetFont(IFont newFont)
        {
            font = newFont;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the RGB.
        /// </summary>
        /// <returns></returns>
        public abstract int[] GetRGB();

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the map object.
        /// </summary>
        /// <param name="mapObject">The map object.</param>
        /// <param name="drawBoundary">The draw boundary.</param>
        /// <param name="zoomLevel">The zoom level.</param>
        public abstract void DrawMapObject(MapObject mapObject, GeoLatLngBounds drawBoundary,
                                  int zoomLevel);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the map text.
        /// </summary>
        public abstract void DrawMapText();

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Clears the canvas.
        /// </summary>
        /// <param name="color">The color.</param>
        public abstract void ClearCanvas(int color);
    }
}
