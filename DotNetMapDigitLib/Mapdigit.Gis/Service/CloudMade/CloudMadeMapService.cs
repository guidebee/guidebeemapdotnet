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

//--------------------------------- PACKAGE -----------------------------------
namespace Mapdigit.Gis.Service.CloudMade
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 27SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// This class defines cloudmade map services.
    /// </summary>
    internal class CloudMadeMapService : DigitalMapService
    {

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="CloudMadeMapService"/> class.
        /// </summary>
        public CloudMadeMapService()
        {
            localSearch = new CLocalSearch();
            geocoder = new CClientGeocoder();
            reverseGeocoder = new CReverseClientGeocoder();
            directionQuery = new CDirections();

        }

        internal static bool _usingJson = true;//use JSON or KML

        internal static string GetCloudMadeKey()
        {
            return MapKeyRepository.GetKey(MapKey.MapkeyTypeCloudMade);

        }


    }

}
