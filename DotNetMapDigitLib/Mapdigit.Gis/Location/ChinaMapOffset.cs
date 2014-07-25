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
using System.Collections;
using System.IO;
using Mapdigit.Gis.Geometry;
using Mapdigit.Network;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Gis.Location
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 28SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// The map in china has so-call offet, the laitude/longitude received by GPS device
    /// is not actually mapped to the real position, it has "offset", this class
    /// is used for amend such offset.
    /// </summary>
    public class ChinaMapOffset
    {
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get map offset at given location and level.
        /// </summary>
        /// <param name="longitude">The longitude.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="mapLevel">The map level.</param>
        /// <returns>offset in china</returns>
        public GeoPoint GetOffset(double longitude, double latitude, int mapLevel)
        {
            if (mapLevel < 11)
            {
                return new GeoPoint(0, 0);
            }
            GeoPoint queryPoint = GetQueryLocation(latitude, longitude);
            string key = queryPoint.X + "|" + queryPoint.Y;
            GeoPoint cachedPoint = (GeoPoint)_offsetCache[key];
            if (cachedPoint == null)
            {
                GeoPoint pt = GetOffsetFromServer(queryPoint.X / 100.0, queryPoint.Y / 100.0);
                _offsetCache.Add(key, pt);
                cachedPoint = pt;
            }
            return new GeoPoint((int)cachedPoint.X >> (18 - mapLevel),
                    (int)cachedPoint.Y >> (18 - mapLevel));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Convert coordinates from WGS to Mars(China)
        /// </summary>
        /// <param name="earth">WGS lat/lng pair.</param>
        /// <returns>Mars' coordinates  lat/lng with deviation in China.</returns>
        public GeoLatLng FromEarthToMars(GeoLatLng earth)
        {
            GeoPoint ptOffset = GetOffset(earth.X, earth.Y, 18);
            if (ptOffset.X != 0 || ptOffset.Y != 0)
            {
                GeoPoint pt = MapLayer.FromLatLngToPixel(earth, 18);
                pt.X += ptOffset.X;
                pt.Y += ptOffset.Y;
                return MapLayer.FromPixelToLatLng(pt, 18);

            }
            return new GeoLatLng(earth);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Convert coordinates from  Mars(China) to WGS
        /// </summary>
        /// <param name="mar"> lat/lng with deviation in China.</param>
        /// <returns>WGS coordinates</returns>
        public GeoLatLng FromMarsToEarth(GeoLatLng mar)
        {
            GeoPoint ptOffset = GetOffset(mar.X, mar.Y, 18);
            if (ptOffset.X != 0 || ptOffset.Y != 0)
            {
                GeoPoint pt = MapLayer.FromLatLngToPixel(mar, 18);
                pt.X -= ptOffset.X;
                pt.Y -= ptOffset.Y;
                return MapLayer.FromPixelToLatLng(pt, 18);

            }
            return new GeoLatLng(mar);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the query base URL.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        public void SetQueryBaseUrl(string baseUrl)
        {
            _queryUrl = baseUrl;
        }

        /**
         * internal cache.
         */
        private readonly Hashtable _offsetCache = new Hashtable(128);

        /**
         * default sever url.
         */
        private string _queryUrl = "http://www.mapdigit.com/guidebeemap/offset.php?";

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the query location.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns></returns>
        private static GeoPoint GetQueryLocation(double latitude, double longitude)
        {
            int lat = (int)(latitude * 100);
            int lng = (int)(longitude * 100);
            double lat1 = ((int)(latitude * 1000 + 0.499999)) / 10.0;
            double lng1 = ((int)(longitude * 1000 + 0.499999)) / 10.0;
            for (double x = longitude; x < longitude + 1; x += 0.5)
            {
                for (double y = latitude; x < latitude + 1; y += 0.5)
                {
                    if (x <= lng1 && lng1 < (x + 0.5) && lat1 >= y && lat1 < (y + 0.5))
                    {
                        return new GeoPoint((int)(x + 0.5), (int)(y + 0.5));
                    }
                }
            }
            return new GeoPoint(lng, lat);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the offset from server.
        /// </summary>
        /// <param name="longitude">The longitude.</param>
        /// <param name="latitude">The latitude.</param>
        /// <returns></returns>
        private GeoPoint GetOffsetFromServer(double longitude, double latitude)
        {
            string queryUrl = _queryUrl + "lng=" + longitude + "&lat=" + latitude;
            HttpConnection c = null;
            Stream inputStream = null;
            try
            {

                try
                {
                    c = Connector.Open(queryUrl);
                    int rc = c.GetResponseCode();
                    if (rc == HttpConnection.HttpOk)
                    {
                        inputStream = c.OpenInputStream();
                        byte[] data = new byte[32];
                        inputStream.Read(data, 0, data.Length);
                        string offsetString = System.Text.Encoding.UTF8.GetString(data,0,data.Length);
                        int index = offsetString.IndexOf(",");
                        if (index > 0)
                        {
                            string offsetX = offsetString.Substring(0, index).Trim();
                            string offsetY = offsetString.Substring(index + 1).Trim();
                            int x = int.Parse(offsetX);
                            int y = int.Parse(offsetY);
                            return new GeoPoint(x, y);
                        }

                    }
                }
                finally
                {

                    if (inputStream != null)
                    {
                        inputStream.Close();
                    }
                    if (c != null)
                    {
                        c.Close();
                    }
                }


            }
            catch (Exception)
            {
            }
            return new GeoPoint(0, 0);


        }
    }
}
