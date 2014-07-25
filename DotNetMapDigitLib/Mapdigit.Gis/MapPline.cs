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
    /// Class MapPline stands for a map pline object.
    /// </summary>
    public sealed class MapPline : MapObject
    {

        /// <summary>
        /// the pen style of the pline.
        /// </summary>
        public MapPen PenStyle;

        /// <summary>
        /// the geo information for the pline object.
        /// </summary>
        public GeoPolyline Pline;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapPline"/> class.
        /// </summary>
        /// <param name="pline">The pline.</param>
        public MapPline(MapPline pline)
            : base(pline)
        {
            MapObjectType = TypePline;
            PenStyle = new MapPen(pline.PenStyle);
            Pline = new GeoPolyline(pline.Pline);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapPline"/> class.
        /// </summary>
        public MapPline()
        {

            MapObjectType = TypePline;
            PenStyle = new MapPen();
            Pline = null;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the pen type of the map pline.
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
        /// <param name="mapPen">the pen type.</param>
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
        /// Get the GeoPolyline of the map Pline.
        /// </summary>
        /// <returns>the GeoPolyline object</returns>
        public GeoPolyline GetPline()
        {
            return Pline;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set GeoPolyline of the map Pline.
        /// </summary>
        /// <param name="pline">the GeoPolyline object.</param>
        public void SetPline(GeoPolyline pline)
        {
            Pline = pline;
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
            string retStr = "PLINE";
            retStr += "  " + Pline.GetVertexCount() + Crlf;
            for (int i = 0; i < Pline.GetVertexCount(); i++)
            {
                GeoLatLng latLng = Pline.GetVertex(i);
                retStr += latLng.X + " " + latLng.Y + Crlf;
            }
            retStr += "\t" + "PEN(" + PenStyle.Width + "," + PenStyle.Pattern + ","
                    + PenStyle.Color + ")" + Crlf;
            return retStr;
        }
    }

}
