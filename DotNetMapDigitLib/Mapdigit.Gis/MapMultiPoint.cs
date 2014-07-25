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
    ///  Class MapMultiPoint stands for a map points collection.
    /// </summary>
    public sealed class MapMultiPoint : MapObject
    {

        /// <summary>
        ///  The symbol type of the map point.
        /// </summary>
        public MapSymbol SymbolType;

        /// <summary>
        /// The location of the map point.
        /// </summary>
        public GeoLatLng[] Points;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapMultiPoint"/> class.
        /// </summary>
        /// <param name="mapPoints">The map points.</param>
        public MapMultiPoint(MapMultiPoint mapPoints)
            : base(mapPoints)
        {
            MapObjectType = TypeMultiPoint;
            SymbolType = new MapSymbol(mapPoints.SymbolType);
            Points = new GeoLatLng[mapPoints.Points.Length];
            for (int i = 0; i < Points.Length; i++)
            {
                Points[i] = new GeoLatLng(mapPoints.Points[i]);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapMultiPoint"/> class.
        /// </summary>
        public MapMultiPoint()
        {

            MapObjectType = TypeMultiPoint;
            SymbolType = new MapSymbol();
            Points = null;
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
        /// Set the symbol type of the map point.
        /// </summary>
        /// <param name="symbol">symbol the symbol type</param>
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
        /// Get the location of the map points.
        /// </summary>
        /// <returns>the location array</returns>
        public GeoLatLng[] GetPoints()
        {
            return Points;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set the location of the map points.
        /// </summary>
        /// <param name="pts">the location</param>
        public void SetPoint(GeoLatLng[] pts)
        {
            Points = pts;
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
            string retStr = "MULTIPOINT    ";
            retStr += Points.Length + Crlf;
            for (int i = 0; i < Points.Length; i++)
            {
                retStr += Points[i].X + " " + Points[i].Y + Crlf;
            }
            retStr += "\t" + "SYMBOL(" + SymbolType.Shape + "," + SymbolType.Color + ","
                    + SymbolType.Size + ")" + Crlf;
            return retStr;
        }
    }

}
