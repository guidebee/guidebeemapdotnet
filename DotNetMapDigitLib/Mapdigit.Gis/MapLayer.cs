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
using System;
using Mapdigit.Gis.Geometry;
using Mapdigit.Drawing.Geometry;
using Mapdigit.Gis.Drawing;
using Mapdigit.Util;

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
    /// MapLayer defines a map layer.Computer maps are organized into layers. Think
    /// of the layers as transparencies that are stacked on top of one another. Each
    /// layer contains different aspects of the entire map. Each layer contains
    /// different map objects, such as regions, points, lines and text.
    /// </summary>
    public abstract class MapLayer
    {

        /// <summary>
        /// the Width of each map tile
        /// </summary>
        public const int MapTileWidth = 256;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set the graphics factory for the map layer.
        /// </summary>
        /// <param name="abstractGraphicsFactory">The abstract graphics factory.</param>
        public static void SetAbstractGraphicsFactory(AbstractGraphicsFactory
                abstractGraphicsFactory)
        {
            AbstractGraphicsFactory = abstractGraphicsFactory;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the graphics factory used by this map layer.
        /// </summary>
        /// <returns>the graphics factory used by this map layer.</returns>
        public static AbstractGraphicsFactory GetAbstractGraphicsFactory()
        {
            return AbstractGraphicsFactory;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Convert String to Latitude,Longitude pair, the input string has this format
        /// [longitude,Latitude,altitude] for example  [115.857562,-31.948275,0]
        /// </summary>
        /// <param name="location">location string</param>
        /// <returns>the geographical coordinates</returns>
        public static GeoLatLng FromStringToLatLng(string location)
        {
            location = location.Trim();
            location = location.Substring(1, location.Length - 1);
            int commaIndex = location.IndexOf(",");
            string longitude = location.Substring(0, commaIndex);
            int commaIndex1 = location.IndexOf(",", commaIndex + 1);
            string latitude = location.Substring(commaIndex + 1, commaIndex1 - commaIndex-1);
            double lat = double.Parse(latitude);
            double lng = double.Parse(longitude);
            return new GeoLatLng(lat, lng);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Computes the pixel coordinates of the given geographical point .
        /// </summary>
        /// <param name="latLng">latitude,longitude pair of give point</param>
        /// <param name="zoomLevel">current zoom level.</param>
        /// <returns>the pixel coordinates.</returns>
        public static GeoPoint FromLatLngToPixel(GeoLatLng latLng, int zoomLevel)
        {
            int pixelX, pixelY;
            TileSystem.LatLongToPixelXy(latLng.Latitude, latLng.Longitude, zoomLevel, 
                out pixelX, out pixelY);
            return new GeoPoint(pixelX, pixelY);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Computes the geographical coordinates from pixel coordinates.
        /// </summary>
        /// <param name="pt">pixel coordinates.</param>
        /// <param name="zoomLevel">current zoom level.</param>
        /// <returns>the geographical coordinates (latitude,longitude) pair</returns>
        public static GeoLatLng FromPixelToLatLng(GeoPoint pt, int zoomLevel)
        {
            double lat;
            double lng;
            TileSystem.PixelXyToLatLong((int)pt.X,(int)pt.Y,zoomLevel,out lat,out lng);
            return new GeoLatLng(lat, lng);

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Computes the pixel coordinates of the given geographical point in the map.
        /// </summary>
        /// <param name="latlng">the geographical coordinates.</param>
        /// <returns>the pixel coordinates in the map.</returns>
        public GeoPoint FromLatLngToMapPixel(GeoLatLng latlng)
        {
            GeoPoint center = FromLatLngToPixel(mapCenterPt, mapZoomLevel);
            GeoPoint topLeft = new GeoPoint(center.X - mapSize.Width / 2.0,
                    center.Y - mapSize.Height / 2.0);
            GeoPoint pointPos = FromLatLngToPixel(latlng, mapZoomLevel);
            pointPos.X -= topLeft.X;
            pointPos.Y -= topLeft.Y;
            return new GeoPoint((int)(pointPos.X + 0.5), (int)(pointPos.Y + 0.5));

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Computes the geographical coordinates from pixel coordinates in the map.
        /// </summary>
        /// <param name="pt">pixel coordinates in the map..</param>
        /// <returns>the geographical coordinates.</returns>
        public GeoLatLng FromMapPixelToLatLng(GeoPoint pt)
        {
            GeoPoint center = FromLatLngToPixel(mapCenterPt, mapZoomLevel);
            GeoPoint topLeft = new GeoPoint(center.X - mapSize.Width / 2.0,
                    center.Y - mapSize.Height / 2.0);
            GeoPoint pointPos = new GeoPoint(pt.X, pt.Y);
            pointPos.X += topLeft.X;
            pointPos.Y += topLeft.Y;
            GeoLatLng latLng = FromPixelToLatLng(pointPos, mapZoomLevel);
            return latLng;

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        ///<summary>
        /// Set the map zoom Level, default is 17
        ///</summary>
        ///<param name="zoom"></param>
        public void SetMaxZoomLevel(int zoom)
        {
            _maxZoomlevel = zoom;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        ///<summary>
        /// set the min zoom level, default is 0
        ///</summary>
        ///<param name="zoom"></param>
        public void SetMinZoomLevel(int zoom)
        {
            _minZoomlevel = zoom;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// return screen boundary in geo coordinates.
        /// </summary>
        /// <param name="pt">the center of the screen..</param>
        /// <returns>screen boundary in geo coordinates</returns>
        public GeoLatLngBounds GetScreenBounds(GeoLatLng pt)
        {
            lock (syncObject)
            {
                GeoPoint center = FromLatLngToPixel(pt, mapZoomLevel);
                int shiftWidth = screenSize.Width;
                GeoPoint topLeft = new GeoPoint(center.X - screenSize.Width / 2.0 - shiftWidth,
                                                center.Y - screenSize.Height / 2.0 - screenSize.Height);
                GeoPoint bottomRight = new GeoPoint(center.X + screenSize.Width / 2.0 + shiftWidth,
                                                    center.Y + screenSize.Height / 2.0 + screenSize.Height);
                GeoLatLng topLeftLatLng = FromPixelToLatLng(topLeft, mapZoomLevel);
                GeoLatLng bottomRightLatLng = FromPixelToLatLng(bottomRight, mapZoomLevel);
                double minY = Math.Min(bottomRightLatLng.Latitude, topLeftLatLng.Latitude);
                double maxY = Math.Max(bottomRightLatLng.Latitude, topLeftLatLng.Latitude);
                double minX = Math.Min(bottomRightLatLng.Longitude, topLeftLatLng.Longitude);
                double maxX = Math.Max(bottomRightLatLng.Longitude, topLeftLatLng.Longitude);
                return new GeoLatLngBounds(minX, minY, maxX - minX, maxY - minY);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// return screen boundary in geo coordinates.
        /// </summary>
        /// <returns>screen boundary in geo coordinates.</returns>
        public GeoLatLngBounds GetScreenBounds()
        {
            return GetScreenBounds(mapCenterPt);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// return screen boundary in geo coordinates.
        /// </summary>
        /// <param name="pt">the center of the screen..</param>
        /// <returns>screen boundary in geo coordinates.</returns>
        public GeoLatLngBounds GetMapBounds(GeoLatLng pt)
        {
            lock (syncObject)
            {
                GeoPoint center = FromLatLngToPixel(pt, mapZoomLevel);
                int shiftWidth = mapSize.Width / 8;
                GeoPoint topLeft = new GeoPoint(center.X - mapSize.Width / 2.0 - shiftWidth,
                                                center.Y - mapSize.Height / 2.0 - mapSize.Height);
                GeoPoint bottomRight = new GeoPoint(center.X + mapSize.Width / 2.0 + shiftWidth,
                                                    center.Y + mapSize.Height / 2.0 + mapSize.Height);
                GeoLatLng topLeftLatLng = FromPixelToLatLng(topLeft, mapZoomLevel);
                GeoLatLng bottomRightLatLng = FromPixelToLatLng(bottomRight, mapZoomLevel);

                double minY = Math.Min(bottomRightLatLng.Latitude, topLeftLatLng.Latitude);
                double maxY = Math.Max(bottomRightLatLng.Latitude, topLeftLatLng.Latitude);
                double minX = Math.Min(bottomRightLatLng.Longitude, topLeftLatLng.Longitude);
                double maxX = Math.Max(bottomRightLatLng.Longitude, topLeftLatLng.Longitude);
                return new GeoLatLngBounds(minX, minY, maxX - minX, maxY - minY);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// return screen boundary in geo coordinates.
        /// </summary>
        /// <returns>screen boundary in geo coordinates.</returns>
        public GeoLatLngBounds GetMapBounds()
        {
            return GetMapBounds(mapCenterPt);
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
        public virtual void PanDirection(int dx, int dy)
        {
            lock (syncObject)
            {
                GeoPoint center = FromLatLngToPixel(mapCenterPt, mapZoomLevel);
                center.X += dx;
                center.Y += dy;
                GeoLatLng newCenter = FromPixelToLatLng(center, mapZoomLevel);
                PanTo(newCenter);
            }
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
        /// <param name="center"> a new center point of the map.</param>
        public virtual void PanTo(GeoLatLng center)
        {
            lock (syncObject)
            {
                mapCenterPt.X = center.X;
                mapCenterPt.Y = center.Y;
                DrawMapCanvas();
            }
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
        public virtual void SetCenter(GeoLatLng center, int zoomLevel)
        {
            lock (syncObject)
            {
                mapZoomLevel = zoomLevel;
                mapCenterPt.X = center.X;
                mapCenterPt.Y = center.Y;
                DrawMapCanvas();
            }

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// return the center point of the map.
        /// </summary>
        /// <returns>current map center point.</returns>
        public GeoLatLng GetCenter()
        {
            lock (syncObject)
            {
                return mapCenterPt;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the sync root. this sync object is used to sync the map operation.
        /// </summary>
        /// <returns>sync root</returns>
        public object GetSyncRoot()
        {
            return syncObject;
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
        public virtual void ZoomIn()
        {
            lock (syncObject)
            {
                mapZoomLevel++;
                if (mapZoomLevel >= _maxZoomlevel)
                {
                    mapZoomLevel = _maxZoomlevel;
                }
                DrawMapCanvas();
            }
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
        public virtual void ZoomOut()
        {
            lock (syncObject)
            {
                mapZoomLevel--;
                if (mapZoomLevel < 0)
                {
                    mapZoomLevel = 0;
                }
                DrawMapCanvas();
            }
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
        public virtual void SetZoom(int level)
        {
            lock (syncObject)
            {
                mapZoomLevel = level;
                DrawMapCanvas();
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// return the zoom level of the map.
        /// </summary>
        /// <returns> current map zoom level.</returns>
        public int GetZoom()
        {
            lock (syncObject)
            {
                return mapZoomLevel;
            }
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
        public virtual void Resize(GeoLatLngBounds bounds)
        {
            lock (syncObject)
            {
                GeoLatLng sw = bounds.SouthWest;
                GeoLatLng ne = bounds.NorthEast;
                GeoLatLng center = new GeoLatLng {X = (sw.X + ne.X)/2.0, Y = (sw.Y + ne.Y)/2.0};
                GeoPoint pt1, pt2;
                for (int i = _maxZoomlevel; i >= _minZoomlevel; i--)
                {
                    pt1 = FromLatLngToPixel(sw, i);
                    pt2 = FromLatLngToPixel(ne, i);
                    double dblWidth = Math.Abs(pt1.X - pt2.X);
                    double dblHeight = Math.Abs(pt1.Y - pt2.Y);
                    if (dblWidth < mapSize.Width && dblHeight < mapSize.Height)
                    {
                        mapZoomLevel = i;
                        SetCenter(center, i);
                        break;
                    }
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
        /// is the point in current screen (is shown or not).
        /// </summary>
        /// <param name="pt">point to be tested</param>
        /// <returns>
        /// 	true is in screen range
        /// </returns>
        public virtual bool IsPointVisible(GeoLatLng pt)
        {
            GeoPoint screenPt = FromLatLngToMapPixel(pt);
            return mapSize.Contains((int)screenPt.X, (int)screenPt.Y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the width of the map.
        /// </summary>
        /// <returns>the map screen width</returns>
        public int GetMapWidth()
        {
            return mapSize.Width;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the height of the map.
        /// </summary>
        /// <returns>the map screen height.</returns>
        public int GetMapHeight()
        {
            return mapSize.Height;
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
        public virtual void SetMapSize(int width, int height)
        {
            lock (syncObject)
            {
                mapSize.Width = width;
                mapSize.Height = height;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27DEC2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the size of the screen.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public void SetViewSize(int width, int height)
        {
            SetScreenSize(width, height);
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
        public virtual void SetScreenSize(int width, int height)
        {
            lock (syncObject)
            {
                screenSize.Width = width;
                screenSize.Height = height;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the width of the screen.
        /// </summary>
        /// <returns>the map screen width</returns>
        public int GetScreenWidth()
        {
            return screenSize.Width;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the height of the screen.
        /// </summary>
        /// <returns>the map screen height</returns>
        public int GetScreenHeight()
        {
            return screenSize.Height;
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
        public abstract void Paint(IGraphics graphics);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draw the map layer to an graphics
        /// </summary>
        /// <param name="graphics"> the graphics object where the map is drawn..</param>
        /// <param name="offsetX">The offset X.</param>
        /// <param name="offsetY">The offset Y.</param>
        public abstract void Paint(IGraphics graphics, int offsetX, int offsetY);

        /// <summary>
        /// Max map zoom Level
        /// </summary>
        protected int _maxZoomlevel = 17;

        /// <summary>
        /// Min map zoom Level
        /// </summary>
        protected int _minZoomlevel;

        /// <summary>
        /// the center of this map.
        /// </summary>
        protected volatile GeoLatLng mapCenterPt = new GeoLatLng();

        /// <summary>
        /// current map zoom level
        /// </summary>
        protected volatile int mapZoomLevel = 1;

        /// <summary>
        /// the size of the map size.
        /// </summary>
        protected volatile Rectangle mapSize = new Rectangle();

        /// <summary>
        /// the size of the screen size.
        /// </summary>
        protected volatile Rectangle screenSize = new Rectangle();

        /// <summary>
        /// sync object.
        /// </summary>
        protected object syncObject = new object();

        /// <summary>
        /// Abstract graphics factory.
        /// </summary>
        protected static AbstractGraphicsFactory AbstractGraphicsFactory;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the map canvas.
        /// </summary>
        protected abstract void DrawMapCanvas();

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapLayer"/> class.
        /// </summary>
        /// <param name="width">the width of the map layer</param>
        /// <param name="height">the height of the map layer</param>
        protected MapLayer(int width, int height)
        {
            screenSize.X = 0; screenSize.Y = 0;
            screenSize.Width = 768;
            screenSize.Height = 768;
            mapSize.X = 0; mapSize.Y = 0;
            mapSize.Width = width; mapSize.Height = height;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the index of map tiles based on given piexl coordinates
        /// </summary>
        /// <param name="x">x coordinates.</param>
        /// <param name="y">y coordinates.</param>
        /// <returns></returns>
        protected static GeoPoint GetMapIndex(double x, double y)
        {
            double longtiles = x / MapTileWidth;
            int tilex = Cast2Integer(longtiles);
            int tiley = Cast2Integer(y / MapTileWidth);
            return new GeoPoint(tilex, tiley);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the index of map tiles based on given geographical coordinates
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="zoomLevel">The zoom level.</param>
        /// <returns>the the index of map tiles</returns>
        protected static GeoPoint ConvertCoordindates2Tiles(double latitude,
                double longitude, int zoomLevel)
        {

            GeoPoint pt = FromLatLngToPixel(new GeoLatLng(latitude, longitude), zoomLevel);
            double pixelx = pt.X;
            double longtiles = pixelx / MapTileWidth;
            int tilex = Cast2Integer(longtiles);
            double pixely = pt.Y;
            int tiley = Cast2Integer(pixely / MapTileWidth);
            return new GeoPoint(tilex, tiley);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// cast double to integer
        /// </summary>
        /// <param name="f">double value.</param>
        /// <returns>the closed interger for the double value.</returns>
        protected static int Cast2Integer(double f)
        {
            if (f < 0)
            {
                return (int)MathEx.Ceil(f);
            }
            return (int)MathEx.Floor(f);
        }
    }

}
