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
    /// Base class of all map objects.
    /// </summary>
    public abstract class MapObject
    {

        /// <summary>
        /// Unkown Object type.
        /// </summary>
        public const int TypeUnknown = -1;

        /// <summary>
        /// None Object type.
        /// </summary>
        public const int TypeNone = 0;

        /// <summary>
        /// Point Object type.
        /// </summary>
        public const int TypePoint = 1;

        /// <summary>
        /// multi point Object type.
        /// </summary>
        public const int TypeMultiPoint = 2;

        /// <summary>
        /// Pline Object type.
        /// </summary>
        public const int TypePline = 3;

        /// <summary>
        /// multi pline Object type.
        /// </summary>
        public const int TypeMultiPline = 4;

        /// <summary>
        /// region Object type.
        /// </summary>
        public const int TypeReginRegion = 5;


        /// <summary>
        /// multi region Object type.
        /// </summary>
        public const int TypeMultiRegion = 6;


        /// <summary>
        /// Collection Object type.
        /// </summary>
        public const int TypeCollection = 7;

        /// <summary>
        /// text Object type.
        /// </summary>
        public const int TypeText = 8;

        /// <summary>
        /// ROAD Object type.
        /// </summary>
        public const int TypeRoad = 15;

        /// <summary>
        /// Direction Object type.
        /// </summary>
        public const int TypeDirection = 16;

        /// <summary>
        /// Route Object type.
        /// </summary>
        public const int TypeRoute = 17;

        /// <summary>
        /// route step Object type.
        /// </summary>
        public const int TypeRouteStep = 18;

        /// <summary>
        /// The MapInfo ID of the map object.
        /// </summary>
        public int MapInfoId;

        /// <summary>
        /// The name of the map object.
        /// </summary>
        public string Name;

        /// <summary>
        /// any notes related to this object
        /// </summary>
        public string ObjectNote;

        /// <summary>
        /// The out bound of the map object.
        /// </summary>
        public GeoLatLngBounds Bounds;

        /// <summary>
        /// The type of the map object.
        /// </summary>
        protected int mapObjectType;

        /// <summary>
        /// The Time for cache
        /// </summary>
        public DateTime CacheAccessTime;

        /// <summary>
        /// indicate Highlighted or not
        /// </summary>
        public bool Highlighted;


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapObject"/> class.
        /// </summary>
        /// <param name="mapObject">map object copy from.</param>
        protected MapObject(MapObject mapObject)
        {
            MapInfoId = mapObject.MapInfoId;
            Name = mapObject.Name;
            Bounds = new GeoLatLngBounds(mapObject.Bounds);
            mapObjectType = mapObject.mapObjectType;
            CacheAccessTime = mapObject.CacheAccessTime;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapObject"/> class.
        /// </summary>
        protected MapObject()
        {
            mapObjectType = TypeUnknown;
            MapInfoId = -1;
            Name = "TypeUnknown";
            Bounds = new GeoLatLngBounds();
            CacheAccessTime = new DateTime();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// the out bound of the map object.
        /// </summary>
        public GeoLatLngBounds Bound
        {
            set { Bounds = new GeoLatLngBounds(value); }
            get { return Bounds; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the type of the map object.
        /// </summary>
        public int MapObjectType
        {
            get { return mapObjectType; }
            set { mapObjectType = value; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Carriage return.
        /// </summary>
        protected const string Crlf = "\n";


    }

}
