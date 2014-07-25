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
using Mapdigit.Ajax;
using Mapdigit.Network;

//--------------------------------- PACKAGE ------------------------------------
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
    /// This class is used to communicate directly with Google servers to obtain
    /// geocodes for user specified addresses. In addition, a geocoder maintains
    /// its own cache of addresses, which allows repeated queries to be answered
    /// without a round trip to the server.
    /// </summary>
    public sealed class IpAddressGeocoder : IIpAddressGeocoder
    {
        /// <summary>
        /// local address
        /// </summary>
        public static string LocalAddress = "127.0.0.1";

        /// <summary>
        /// IP not found
        /// </summary>
        public static string IpNotFound = "IP_NOT_FOUND";


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="IpAddressGeocoder"/> class.
        /// </summary>
        public IpAddressGeocoder()
        {
            _addressQuery = new AddressQuery();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sends a request to Google servers to geocode the specified address
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <param name="listener">callback when query is done.</param>
        public void GetLocations(string ipAddress, IIpAddressGeocodingListener listener)
        {
            _listener = listener;
            _searchAddress = ipAddress;
            if (_searchAddress.EndsWith(LocalAddress))
            {
                Request.Get(LocalSearchBase, null, null, _addressQuery, this);
            }
            else
            {
                Arg[] args = new Arg[2];
                args[0] = new Arg("l", "EuWpotBFCA2I");
                args[1] = new Arg("i", _searchAddress);
                Request.Get(SearchBase, args, null, _addressQuery, this);
            }

        }

        private const string SearchBase = "http://geoip1.maxmind.com/f";
        private const string LocalSearchBase = "http://www.mapdigit.com/guidebeemap/ipaddress.php";
        internal IIpAddressGeocodingListener _listener;
        internal string _searchAddress;
        internal AddressQuery _addressQuery;
    }

    internal class AddressQuery : IRequestListener
    {
        private static void SearchResponse(IpAddressGeocoder geoCoder, Response response)
        {
            IpAddressLocation ipAddressLocation = null;
            Exception ex = response.GetException();
            if (ex != null || response.GetCode() != HttpConnection.HttpOk)
            {

                if (geoCoder._listener != null)
                {
                    geoCoder._listener.Done(geoCoder._searchAddress, null);
                }
                return;
            }
            try
            {
                Result result = response.GetResult();
                ipAddressLocation = new IpAddressLocation
                                        {
                                            Ipaddress = result.GetAsString("ipaddress"),
                                            Country = result.GetAsString("country"),
                                            Region = result.GetAsString("region"),
                                            City = result.GetAsString("city"),
                                            Postal = result.GetAsString("postal"),
                                            Latitude = result.GetAsString("latitude"),
                                            Longitude = result.GetAsString("longitude"),
                                            MetroCode = result.GetAsString("metro_code"),
                                            AreaCode = result.GetAsString("area_code"),
                                            Isp = result.GetAsString("isp"),
                                            Organization = result.GetAsString("organization"),
                                            Error = result.GetAsString("error")
                                        };
            }
            catch (Exception)
            {


            }
            if (geoCoder._listener != null)
            {
                geoCoder._listener.Done(geoCoder._searchAddress, ipAddressLocation);
            }

        }

        public void ReadProgress(object context, int bytes, int total)
        {
            IpAddressGeocoder geoCoder = (IpAddressGeocoder)context;
            geoCoder._listener.ReadProgress(bytes, total);
        }

        public void WriteProgress(object context, int bytes, int total)
        {
        }

        public void Done(object context, Response response)
        {
            IpAddressGeocoder geoCoder = (IpAddressGeocoder)context;
            SearchResponse(geoCoder, response);
        }

        public void Done(object context, string rawResult)
        {

        }
    }

}
