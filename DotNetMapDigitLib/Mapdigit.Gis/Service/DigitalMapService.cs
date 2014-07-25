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

//--------------------------------- PACKAGE ------------------------------------
using Mapdigit.Gis.Geometry;
using Mapdigit.Gis.Service.CloudMade;
using Mapdigit.Gis.Service.Google;
using Mapdigit.Gis.Service.MapAbc;

namespace Mapdigit.Gis.Service
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 27SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// This class is a base class for map service.
    /// </summary>
    public abstract class DigitalMapService
    {
        /// <summary>
        /// google map service
        /// </summary>
        public const int GoogleMapService = 0;

        /// <summary>
        /// map abc map service
        /// </summary>
        public const int MapabcMapService = 1;

        /// <summary>
        /// Bing map service
        /// </summary>
        public const int BingMapService = 2;

        /// <summary>
        /// cloud made map service
        /// </summary>
        public const int CloudmadeMapService = 3;


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29DEC2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get current map service.
        /// </summary>
        /// <param name="mapServiceType">map service type</param>
        /// <returnsmap service instance></returns>
        public static DigitalMapService GetCurrentMapService(int mapServiceType)
        {
            switch (mapServiceType)
            {
                case MapabcMapService:
                    return MapAbcMapServiceInstance;
                case GoogleMapService:
                    return GoogleMapServiceInstance;
                case CloudmadeMapService:
                    return CloudMadeMapServiceInstance;
            }
            return GoogleMapServiceInstance;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set the listener for geocoding query.
        /// </summary>
        /// <param name="listener"> callback when query is done and in progress</param>
        public void SetGeocodingListener(IGeocodingListener listener)
        {
            geocodingListener = listener;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set the listener for reverse geocoding query.
        /// </summary>
        /// <param name="listener">callback when query is done and in progress</param>
        public void SetReverseGeocodingListener(IReverseGeocodingListener listener)
        {
            reverseGeocodingListener = listener;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 03JAN2009  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sends a request to servers to reverse geocode the specified address
        /// </summary>
        /// <param name="latlngAddress">address to query</param>
        public void GetReverseLocations(GeoLatLng latlngAddress)
        {
            if (reverseGeocodingListener != null)
            {
                reverseGeocoder.GetLocations(latlngAddress,
                        reverseGeocodingListener);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 03JAN2009  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sends a request to servers to get the direction
        /// </summary>
        /// <param name="waypoints"address to query></param>
        public void GetDirections(GeoLatLng[] waypoints)
        {
            if (routingListener != null)
            {
                directionQuery.GetDirection(waypoints, routingListener);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the ip address geocoding listener.
        /// </summary>
        /// <param name="listener">callback when query is done and in progress</param>
        public void SetIpAddressGeocodingListener(IIpAddressGeocodingListener listener)
        {
            ipAddressGeocodingListener = listener;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set the listener for direction query..
        /// </summary>
        /// <param name="listener">The routing listener.</param>
        public void SetRoutingListener(IRoutingListener listener)
        {
            routingListener = listener;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sends a request to servers to reverse geocode the specified address。
        /// </summary>
        /// <param name="latlngAddress">The latlng address.</param>
        public void GetReverseLocations(string latlngAddress)
        {
            if (reverseGeocodingListener != null)
            {
                reverseGeocoder.GetLocations(latlngAddress,
                        reverseGeocodingListener);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sends a request to servers to reverse geocode the specified address。
        /// </summary>
        /// <param name="mapType">Type of the map.</param>
        /// <param name="latlngAddress">latitude,longitude string address.delimited by comma</param>
        public void GetReverseLocations(int mapType, string latlngAddress)
        {
            if (reverseGeocodingListener != null)
            {
                reverseGeocoder.GetLocations(mapType, latlngAddress,
                        reverseGeocodingListener);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sends a request to servers to geocode the specified address。
        /// </summary>
        /// <param name="address">The address.</param>
        public void GetLocations(string address)
        {
            if (geocodingListener != null)
            {
                geocoder.GetLocations(address, geocodingListener);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// local search
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="start">The start index.</param>
        /// <param name="center">The center.</param>
        /// <param name="bound">The bound.</param>
        public void GetLocations(string address, int start, GeoLatLng center, GeoBounds bound)
        {
            if (localSearch != null)
            {
                localSearch.GetLocations(address, start, center, bound, geocodingListener);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// local search
        /// </summary>
        /// <param name="mapType">Type of the map.</param>
        /// <param name="address">The address.</param>
        /// <param name="start">The start index.</param>
        /// <param name="center">The center.</param>
        /// <param name="bound">The bound.</param>
        public void GetLocations(int mapType, string address, int start,
            GeoLatLng center, GeoBounds bound)
        {
            if (localSearch != null)
            {
                localSearch.GetLocations(mapType, address, start, center, bound, geocodingListener);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the cell locations.
        /// </summary>
        /// <param name="mmc">The MMC.</param>
        /// <param name="mnc">The MNC.</param>
        /// <param name="lac">The lac.</param>
        /// <param name="cid">The cid.</param>
        public void GetLocations(string mmc, string mnc, string lac, string cid)
        {
            if (cellIdGeocoder != null)
            {
                cellIdGeocoder.GetLocations(mmc, mnc, lac, cid, geocodingListener);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sends a request to servers to geocode the specified address
        /// </summary>
        /// <param name="mapType">Type of the map.</param>
        /// <param name="address">The address.</param>
        public void GetLocations(int mapType, string address)
        {
            if (geocodingListener != null)
            {
                geocoder.GetLocations(mapType, address, geocodingListener);
            }
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the ip locations.
        /// </summary>
        /// <param name="address">The address.</param>
        public void GetIpLocations(string address)
        {
            if (ipAddressGeocodingListener != null)
            {
                ipAddressGeocoder.GetLocations(address, ipAddressGeocodingListener);

            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the search options.
        /// </summary>
        /// <param name="options">The options.</param>
        public static void SetSearchOptions(SearchOptions options)
        {
            _searchOptions = options;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the search options.
        /// </summary>
        /// <returns>the search options</returns>
        public static SearchOptions GetSearchOptions()
        {
            return _searchOptions;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sends a request to servers to get the direction
        /// </summary>
        /// <param name="query">The query.</param>
        public void GetDirections(string query)
        {
            if (routingListener != null)
            {
                directionQuery.GetDirection(query, routingListener);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the directions.
        /// </summary>
        /// <param name="mapType">Type of the map.</param>
        /// <param name="query">The query.</param>
        public void GetDirections(int mapType, string query)
        {
            if (routingListener != null)
            {
                directionQuery.GetDirection(mapType, query, routingListener);
            }
        }

        /// <summary>
        /// IP Address geocoding listener.
        /// </summary>
        protected IIpAddressGeocodingListener ipAddressGeocodingListener;

        /// <summary>
        /// geo coding listener
        /// </summary>
        protected IGeocodingListener geocodingListener;

        /// <summary>
        /// local search
        /// </summary>
        protected ILocalSearch localSearch;

        /// <summary>
        /// reverse geocoding listener
        /// </summary>
        protected IReverseGeocodingListener reverseGeocodingListener;

        /// <summary>
        /// routing listener
        /// </summary>
        protected IRoutingListener routingListener;

        /// <summary>
        /// geo coder
        /// </summary>
        protected IGeocoder geocoder;
        /// <summary>
        /// reverse geo coder
        /// </summary>
        protected IReverseGeocoder reverseGeocoder;
        /// <summary>
        /// direction query
        /// </summary>
        protected IDirectionQuery directionQuery;
        /// <summary>
        /// ip address geo coder
        /// </summary>
        protected IIpAddressGeocoder ipAddressGeocoder = new IpAddressGeocoder();
        /// <summary>
        /// cell id geo coder
        /// </summary>
        protected ICellIdGeocoder cellIdGeocoder = new CellIdGeocoder();

        private static SearchOptions _searchOptions = new SearchOptions();

        private static readonly GoogleMapService GoogleMapServiceInstance = new GoogleMapService();
        private static readonly MapAbcMapService MapAbcMapServiceInstance = new MapAbcMapService();
        private static readonly CloudMadeMapService CloudMadeMapServiceInstance = new CloudMadeMapService();
    }
}


