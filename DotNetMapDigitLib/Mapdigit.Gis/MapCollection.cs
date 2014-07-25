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
    /// Class MapCollection stands for a collection of map objects.
    /// </summary>
    public sealed class MapCollection : MapObject
    {

        /// <summary>
        /// The multiPoint part of the collection.
        /// </summary>
        public MapMultiPoint MultiPoint;

        /// <summary>
        /// The multiPline part of the collection.
        /// </summary>
        public MapMultiPline MultiPline;

        /// <summary>
        /// The multiRegion part of the collection.
        /// </summary>
        public MapMultiRegion MultiRegion;


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapCollection"/> class.
        /// </summary>
        /// <param name="mapCollection">The map collection coped from.</param>
        public MapCollection(MapCollection mapCollection)
            : base(mapCollection)
        {

            MapObjectType = TypeCollection;
            MultiPline = null;
            MultiPoint = null;
            MultiRegion = null;
            if (mapCollection.MultiPline != null)
            {
                MultiPline = new MapMultiPline(mapCollection.MultiPline);
            }
            if (mapCollection.MultiPoint != null)
            {
                MultiPoint = new MapMultiPoint(mapCollection.MultiPoint);
            }
            if (mapCollection.MultiRegion != null)
            {
                MultiRegion = new MapMultiRegion(mapCollection.MultiRegion);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapCollection"/> class.
        /// </summary>
        public MapCollection()
        {

            MapObjectType = TypeCollection;
            MultiPline = null;
            MultiPoint = null;
            MultiRegion = null;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the multipoint part the collection.
        /// </summary>
        /// <returns>the multipoint part the collection</returns>
        public MapMultiPoint GetMultiPoint()
        {
            return MultiPoint;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set the multipoint part the collection.
        /// </summary>
        /// <param name="multiPoint">the multipoint part the collection</param>
        public void SetMultiPoint(MapMultiPoint multiPoint)
        {
            MultiPoint = multiPoint;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the MultiPline part the collection.
        /// </summary>
        /// <returns>the MultiPline part the collection</returns>
        public MapMultiPline GetMultiPline()
        {
            return MultiPline;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set the multiPline part the collection.
        /// </summary>
        /// <param name="multiPline">the multiPline part the collection.</param>
        public void SetMultiPline(MapMultiPline multiPline)
        {
            MultiPline = multiPline;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the multiRegion part the collection.
        /// </summary>
        /// <returns>the multiRegion part the collection</returns>
        public MapMultiRegion GetMultiRegion()
        {
            return MultiRegion;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set the multiRegion part the collection.
        /// </summary>
        /// <param name="multiRegion">the multiRegion part the collection</param>
        public void SetMultiRegion(MapMultiRegion multiRegion)
        {
            MultiRegion = multiRegion;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            string retStr = "COLLECTION ";
            int collectionPart = 0;
            if (MultiPoint != null)
            {
                collectionPart++;
            }
            if (MultiPline != null)
            {
                collectionPart++;
            }
            if (MultiRegion != null)
            {
                collectionPart++;
            }

            retStr += collectionPart + Crlf;
            if (MultiRegion != null)
            {
                retStr += MultiRegion.ToString();
            }

            if (MultiPline != null)
            {
                retStr += MultiPline.ToString();
            }
            if (MultiPoint != null)
            {
                retStr += MultiPoint.ToString();
            }
            return retStr;
        }

    }

}
