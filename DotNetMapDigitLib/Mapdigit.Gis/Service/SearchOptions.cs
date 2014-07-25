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
    /// This class is used to store driving directions options
    /// </summary>
    public class SearchOptions
    {
        /// <summary>
        /// route for car driving
        /// </summary>
        public const int RouteTypeDriving = 0;

        /// <summary>
        /// route for walking
        /// </summary>
        public const int RouteTypeWalking = 1;

        /// <summary>
        /// route for bus /train
        /// </summary>
        public const int RouteTypeCommuting = 2;

        /// <summary>
        /// KM
        /// </summary>
        public const int RouteUnitKm = 0;

        /// <summary>
        /// Mile.
        /// </summary>
        public const int RouteUnitMile = 1;

        /// <summary>
        /// type of routing
        /// </summary>
        public int RoutingType = RouteTypeDriving;

        /// <summary>
        /// Route unit
        /// </summary>
        public int RouteUnit = RouteUnitKm;

        /// <summary>
        /// language id ,default is en-US.
        /// </summary>
        public string LanguageId = "en-US";

        /// <summary>
        /// Avoid high way or not
        /// </summary>
        public bool AvoidHighway;

        /// <summary>
        /// Avoid toll way or not.
        /// </summary>
        public bool AvoidTollway;

        /// <summary>
        /// default number of search result.
        /// </summary>
        public int NumberOfSearchResult = 10;

    }
}
