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
    /// Class MapMultiPline stands for map plines’ collection.
    /// </summary>
    public sealed class MapMultiPline : MapObject
    {

        /// <summary>
        /// the pen style of the pline.
        /// </summary>
        public MapPen PenStyle;

        /// <summary>
        /// the geo information for the pline object.
        /// </summary>
        public GeoPolyline[] Plines;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapMultiPline"/> class.
        /// </summary>
        /// <param name="multiPline">The multi pline coped from.</param>
        public MapMultiPline(MapMultiPline multiPline)
            : base(multiPline)
        {

            MapObjectType = TypeMultiPline;
            PenStyle = new MapPen(multiPline.PenStyle);
            Plines = new GeoPolyline[multiPline.Plines.Length];
            for (int i = 0; i < Plines.Length; i++)
            {
                Plines[i] = new GeoPolyline(multiPline.Plines[i]);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapMultiPline"/> class.
        /// </summary>
        public MapMultiPline()
        {

            MapObjectType = TypeMultiPline;
            PenStyle = new MapPen();
            Plines = null;
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
        /// Get the GeoPolyline of the map MultiPline.
        /// </summary>
        /// <returns>the GeoPolyline object.</returns>
        public GeoPolyline[] GetPlines()
        {
            return Plines;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set GeoPolyline array of the map MultiPline.
        /// </summary>
        /// <param name="plines">the GeoPolyline object array.</param>
        public void SetPlines(GeoPolyline[] plines)
        {
            Plines = plines;
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
            string retStr = "PLINE  MULTIPLE  ";
            retStr += Plines.Length + Crlf;
            for (int j = 0; j < Plines.Length; j++)
            {
                retStr += "  " + Plines[j].GetVertexCount() + Crlf;
                for (int i = 0; i < Plines[j].GetVertexCount(); i++)
                {
                    GeoLatLng latLng = Plines[j].GetVertex(i);
                    retStr += latLng.X + " " + latLng.Y + Crlf;
                }
            }
            retStr += "\t" + "PEN(" + PenStyle.Width + "," + PenStyle.Pattern + ","
                    + PenStyle.Color + ")" + Crlf;
            return retStr;
        }
    }

}
