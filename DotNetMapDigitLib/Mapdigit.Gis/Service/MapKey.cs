//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 28DEC2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Gis.Service
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 28DEC2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// This class defines the map service key for different map servers. i.e.
    /// Google map key, bing map key, MapAbc Key ,OpenStreet Map Key.
    /// </summary>
    public class MapKey
    {

        /// <summary>
        /// google map key.
        /// </summary>
        public const int MapkeyTypeGoogle = 1;

        /// <summary>
        /// bing map key.
        /// </summary>
        public const int MapkeyTypeBing = 2;

        /// <summary>
        /// map abc map key.
        /// </summary>
        public const int MapkeyTypeMapabc = 3;

        /// <summary>
        /// open street map key.
        /// </summary>
        public const int MapkeyTypeCloudMade = 4;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28DEC2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="key">key type</param>
        /// <param name="keyValue">key value.</param>
        public MapKey(int key, string keyValue)
        {
            _keyType = key;
            _keyString = keyValue;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28DEC2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        ///<summary>
        /// The key type.
        ///</summary>
        public int KeyType
        {
            get { return _keyType; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28DEC2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// get the key value.
        /// </summary>
        public string KeyValue
        {
            get { return _keyString; }
        }

        /// <summary>
        /// key type.
        /// </summary>
        private readonly int _keyType = MapkeyTypeGoogle;

        /// <summary>
        /// key string.
        /// </summary>
        private readonly string _keyString;

    }

}
