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
    /// Class MapMultiRegion stands for map regions' collection.
    /// </summary>
    public sealed class MapMultiRegion : MapObject
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
        public GeoPolygon[] Regions;

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
        /// Initializes a new instance of the <see cref="MapMultiRegion"/> class.
        /// </summary>
        /// <param name="multiRegion">The multi region.</param>
        public MapMultiRegion(MapMultiRegion multiRegion)
            : base(multiRegion)
        {
            MapObjectType = TypeMultiRegion;
            PenStyle = new MapPen(multiRegion.PenStyle);
            BrushStyle = new MapBrush(multiRegion.BrushStyle);
            Regions = new GeoPolygon[multiRegion.Regions.Length];
            for (int i = 0; i < Regions.Length; i++)
            {
                Regions[i] = new GeoPolygon(multiRegion.Regions[i]);
            }
            CenterPt = new GeoLatLng(multiRegion.CenterPt);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapMultiRegion"/> class.
        /// </summary>
        public MapMultiRegion()
        {
            MapObjectType = TypeMultiRegion;
            PenStyle = new MapPen();
            BrushStyle = new MapBrush();
            CenterPt = new GeoLatLng();
            Regions = null;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the pen type of the map region
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
        /// <param name="mapPen"> the pen type</param>
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
        /// <param name="mapBrush">the brush type.</param>
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
        public GeoPolygon[] GetRegions()
        {
            return Regions;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set GeoPolygon array of the map Region.
        /// </summary>
        /// <param name="regions">the GeoPolygon object array.</param>
        public void SetRegions(GeoPolygon[] regions)
        {
            Regions = regions;
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
            string retStr = "REGION ";
            retStr += Regions.Length + Crlf;
            for (int j = 0; j < Regions.Length; j++)
            {
                retStr += "  " + Regions[j].GetVertexCount() + Crlf;
                for (int i = 0; i < Regions[j].GetVertexCount(); i++)
                {
                    GeoLatLng latLng = Regions[j].GetVertex(i);
                    retStr += latLng.X + " " + latLng.Y + Crlf;
                }
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
