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
    /// Class MapRegion stands for a map region object.
    /// </summary>
    public sealed class MapRegion : MapObject
    {

        /// <summary>
        /// the pen style of the region.
        /// </summary>
        public MapPen PenStyle;

        /// <summary>
        /// the brush style of the region.
        /// </summary>
        public MapBrush BrushStyle;

        /// <summary>
        /// the geo information for the region object.
        /// </summary>
        public GeoPolygon Region;

        /// <summary>
        /// the center of the region.
        /// </summary>
        public GeoLatLng CenterPt;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapRegion"/> class.
        /// </summary>
        /// <param name="region">The region.</param>
        public MapRegion(MapRegion region)
            : base(region)
        {

            MapObjectType = TypeReginRegion;
            PenStyle = new MapPen(region.PenStyle);
            BrushStyle = new MapBrush(region.BrushStyle);
            Region = new GeoPolygon(region.Region);
            CenterPt = new GeoLatLng(region.CenterPt);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapRegion"/> class.
        /// </summary>
        public MapRegion()
        {

            MapObjectType = TypeReginRegion;
            PenStyle = new MapPen();
            BrushStyle = new MapBrush();
            CenterPt = new GeoLatLng();
            Region = null;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the pen type of the map region.
        /// </summary>
        /// <returns>the pen type</returns>
        public MapPen GetPenType()
        {
            return PenStyle;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set the pen type of the map point.
        /// </summary>
        /// <param name="mapPen">the pen type</param>
        public void SetPenType(MapPen mapPen)
        {
            PenStyle = mapPen;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the brush type of the map region.
        /// </summary>
        /// <returns>the brush type</returns>
        public MapBrush GetBrushType()
        {
            return BrushStyle;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set the brush type of the map region.
        /// </summary>
        /// <param name="mapBrush">The map brush.</param>
        public void SetPenType(MapBrush mapBrush)
        {
            BrushStyle = mapBrush;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the GeoPolygon of the map Region.
        /// </summary>
        /// <returns>the GeoPolygon object</returns>
        public GeoPolygon GetRegion()
        {
            return Region;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set GeoPolygon of the map Region.
        /// </summary>
        /// <param name="region">the GeoPolygon object</param>
        public void SetRegion(GeoPolygon region)
        {
            Region = region;
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
            string retStr = "REGION 1" + Crlf;
            retStr += "\t" + Region.GetVertexCount() + Crlf;
            for (int i = 0; i < Region.GetVertexCount(); i++)
            {
                GeoLatLng latLng = Region.GetVertex(i);
                retStr += latLng.X + " " + latLng.Y + Crlf;
            }
            retStr += "\t" + "PEN(" + PenStyle.Width + "," + PenStyle.Pattern + ","
                    + PenStyle.Color + ")" + Crlf;
            retStr += "\t" + "BRUSH(" + BrushStyle.Pattern + "," + BrushStyle.ForeColor + ","
                    + BrushStyle.BackColor + ")" + Crlf;
            retStr += "\tCENTER " + CenterPt.X + " " + CenterPt.Y + Crlf;
            return retStr;
        }
    }

}
