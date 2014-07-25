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
namespace Mapdigit.Gis.Service.MapAbc
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 27SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// This class defines mapabc map services.
    /// </summary>
    internal class MapAbcMapService : DigitalMapService
    {

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapAbcMapService"/> class.
        /// </summary>
        public MapAbcMapService()
        {
            localSearch = new MLocalSearch();
            geocoder = new MClientGeocoder();
            reverseGeocoder = new MReverseClientGeocoder();
            directionQuery = new MDirections();

        }

        internal static bool _usingJson = true;//use JSON or KML

        internal static string GetMapAbcKey()
        {
            return MapKeyRepository.GetKey(MapKey.MapkeyTypeMapabc);

        }

        internal static string _mapabcServiceVer = "2.0";

    }

}
