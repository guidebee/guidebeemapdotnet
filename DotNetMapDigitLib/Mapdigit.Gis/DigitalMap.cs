//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 15OCT2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using Mapdigit.Gis.Geometry;
using Mapdigit.Gis.Service;
using Mapdigit.Gis.Service.Google;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Gis
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 15OCT2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// DigitalMap is the base class for VectorMap and RasterMap. It's an abstract
    /// class provides all common functions for digtial maps.
    /// </summary>
    public abstract class DigitalMap : MapLayerContainer
    {

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set digital map service instance for this digital map.
        /// </summary>
        /// <param name="mapService">The digital map service.</param>
        public void SetDigitalMapService(DigitalMapService mapService)
        {
            digitalMapService = mapService;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// return the digital map service associcated with this map
        /// </summary>
        /// <returns>the digital map service instance</returns>
        public DigitalMapService GetDigitalMapService()
        {
            return digitalMapService;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sends a request to servers to get the direction
        /// </summary>
        /// <param name="query">address to query</param>
        public void GetDirections(string query)
        {
            digitalMapService.GetDirections(typeOfMap, query);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sends a request to servers to get the direction
        /// </summary>
        /// <param name="type">map type</param>
        /// <param name="query">address to query</param>
        public void GetDirections(int type,string query)
        {
            digitalMapService.GetDirections(type, query);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sends a request to servers to get the direction
        /// </summary>
        /// <param name="query">address to query</param>
        public void GetDirections(GeoLatLng[] query)
        {
            digitalMapService.GetDirections(query);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set the listener for geocoding query.
        /// </summary>
        /// <param name="geocodingListener"> callback when query is done and in progress.</param>
        public void SetGeocodingListener(IGeocodingListener geocodingListener)
        {
            digitalMapService.SetGeocodingListener(geocodingListener);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set the listener for reverse geocoding query.
        /// </summary>
        /// <param name="reverseGeocodingListener">callback when query is done and in progress</param>
        public void SetReverseGeocodingListener(IReverseGeocodingListener
                reverseGeocodingListener)
        {
            digitalMapService.SetReverseGeocodingListener(reverseGeocodingListener);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the ip address geocoding listener.
        /// </summary>
        /// <param name="geocodingListener">callback when query is done and in progress.</param>
        public void SetIpAddressGeocodingListener(IIpAddressGeocodingListener
                geocodingListener)
        {
            digitalMapService.SetIpAddressGeocodingListener(geocodingListener);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the routing listener.
        /// </summary>
        /// <param name="routingListener">The routing listener.</param>
        public void SetRoutingListener(IRoutingListener routingListener)
        {
            digitalMapService.SetRoutingListener(routingListener);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the ip locations.
        /// </summary>
        /// <param name="ipaddress">The ipaddress.</param>
        public void GetIpLocations(string ipaddress)
        {
            digitalMapService.GetIpLocations(ipaddress);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sends a request to servers to geocode the specified address
        /// </summary>
        /// <param name="address">The address.</param>
        public void GetLocations(string address)
        {
            digitalMapService.GetLocations(typeOfMap, address);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sends a request to servers to geocode the specified address
        /// </summary>
        /// <param name="type">map type.</param>
        /// <param name="address">The address.</param>
        public void GetLocations(int type,string address)
        {
            digitalMapService.GetLocations(type, address);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// local search, sends a request to server for local search.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="start">The start index.</param>
        /// <param name="center">The center.</param>
        /// <param name="bound">The bound.</param>
        public void GetLocations(string address, int start, GeoLatLng center, GeoBounds bound)
        {
            digitalMapService.GetLocations(typeOfMap, address, start, center, bound);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// local search, sends a request to server for local search.
        /// </summary>
        /// <param name="type">map type.</param>
        /// <param name="address">The address.</param>
        /// <param name="start">The start index.</param>
        /// <param name="center">The center.</param>
        /// <param name="bound">The bound.</param>
        public void GetLocations(int type,string address, int start, GeoLatLng center, GeoBounds bound)
        {
            digitalMapService.GetLocations(type, address, start, center, bound);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// search based on cell id.
        /// </summary>
        /// <param name="mmc">The MMC.</param>
        /// <param name="mnc">The MNC.</param>
        /// <param name="lac">The lac.</param>
        /// <param name="cid">The cid.</param>
        public void GetLocations(string mmc, string mnc, string lac, string cid)
        {
            digitalMapService.GetLocations(mmc, mnc, lac, cid);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sends a request to servers to geocode the specified address
        /// </summary>
        /// <param name="latlngAddress">The latlng address.</param>
        public void GetReverseLocations(string latlngAddress)
        {
            digitalMapService.GetReverseLocations(typeOfMap, latlngAddress);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sends a request to servers to geocode the specified address
        /// </summary>
        /// <param name="latlngAddress">The latlng address.</param>
        public void GetReverseLocations(GeoLatLng latlngAddress)
        {
            digitalMapService.GetReverseLocations(latlngAddress);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sends a request to servers to geocode the specified address
        /// </summary>
        /// <param name="type">map type.</param>
        /// <param name="latlngAddress">The latlng address.</param>
        public void GetReverseLocations(int type,string latlngAddress)
        {
            digitalMapService.GetReverseLocations(type, latlngAddress);
        }
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Updates the map tile URL.
        /// </summary>
        public void UpdateMapServiceUrl()
        {
            if (digitalMapService is GoogleMapService)
            {
                GoogleMapService.UpdateMapServiceUrl();
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the version no.
        /// </summary>
        /// <returns>the version no</returns>
        public string GetVersionNo()
        {
            return "3.0.0";
        }

        /// <summary>
        /// the type of map.
        /// </summary>
        protected int typeOfMap;

        /// <summary>
        /// Digital map service instance.
        /// </summary>
        protected DigitalMapService digitalMapService;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 15OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalMap"/> class.
        /// </summary>
        /// <param name="width">the width of the map layer</param>
        /// <param name="height">the height of the map layer</param>
        protected DigitalMap(int width, int height)
            : base(width, height)
        {
            digitalMapService = DigitalMapService.GetCurrentMapService(DigitalMapService.GoogleMapService);
        }
    }

}
