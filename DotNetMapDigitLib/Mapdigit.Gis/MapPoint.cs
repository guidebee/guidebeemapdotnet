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
    /// Class MapPoint stands for a point map oject.
    /// </summary>
    public sealed class MapPoint : MapObject
    {

        /// <summary>
        /// The symbol type of the map point.
        /// </summary>
        public MapSymbol SymbolType;

        /// <summary>
        /// The location of the map point.
        /// </summary>
        public GeoLatLng Point;


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapPoint"/> class.
        /// </summary>
        /// <param name="mapPoint">The map point.</param>
        public MapPoint(MapPoint mapPoint)
            : base(mapPoint)
        {

            MapObjectType = TypePoint;
            SymbolType = new MapSymbol(mapPoint.SymbolType);
            Point = new GeoLatLng(mapPoint.Point);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapPoint"/> class.
        /// </summary>
        public MapPoint()
        {

            MapObjectType = TypePoint;
            SymbolType = new MapSymbol();
            Point = new GeoLatLng();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the symbol type of the map point.
        /// </summary>
        /// <returns>the symbol type</returns>
        public MapSymbol GetSymbolType()
        {
            return SymbolType;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set the symbol type of the map point
        /// </summary>
        /// <param name="symbol">the symbol type</param>
        public void SetSymbolType(MapSymbol symbol)
        {
            SymbolType = symbol;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the location of the map point.
        /// </summary>
        /// <returns>the location</returns>
        public GeoLatLng GetPoint()
        {
            return Point;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set the location of the map point.
        /// </summary>
        /// <param name="p">the location</param>
        public void SetPoint(GeoLatLng p)
        {
            Point = new GeoLatLng(p);
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
            string retStr = "POINT    ";
            retStr += Point.X + " " + Point.Y + Crlf;
            retStr += "\t" + "SYMBOL(" + SymbolType.Shape + "," + SymbolType.Color + ","
                    + SymbolType.Size + ")" + Crlf;
            return retStr;
        }
    }

}
