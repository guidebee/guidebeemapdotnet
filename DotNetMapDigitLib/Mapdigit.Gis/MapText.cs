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
using Mapdigit.Gis.Drawing;
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
    /// Class MapText stands for a text map object.
    /// </summary>
    public sealed class MapText : MapObject
    {

        /// <summary>
        /// default font 
        /// </summary>
        public IFont Font;

        /// <summary>
        /// The angle of the map text.
        /// </summary>
        public int Angle;

        /// <summary>
        /// The location of the map text.
        /// </summary>
        public GeoLatLng Point;

        /// <summary>
        /// The back color of the map text.
        /// </summary>
        public int BackColor;

        /// <summary>
        /// The fore color of the map text.
        /// </summary>
        public int ForeColor;

        /// <summary>
        /// The justification of the map text.
        /// </summary>
        public int Justification;

        /// <summary>
        /// The spacing of the map text.
        /// </summary>
        public int Spacing;

        /// <summary>
        /// The lineType of the map text.
        /// </summary>
        public int LineType;

        /// <summary>
        /// the text string of the text.
        /// </summary>
        public string TextString;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapText"/> class.
        /// </summary>
        /// <param name="mapText">The map text.</param>
        public MapText(MapText mapText)
            : base(mapText)
        {

            MapObjectType = TypeText;
            Point = new GeoLatLng(mapText.Point);
            Angle = mapText.Angle;
            BackColor = mapText.BackColor;
            ForeColor = mapText.ForeColor;
            Justification = mapText.Justification;
            Spacing = mapText.Spacing;
            LineType = mapText.LineType;
            TextString = mapText.TextString;
            Font = mapText.Font;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapText"/> class.
        /// </summary>
        public MapText()
        {

            MapObjectType = TypeText;
            Point = new GeoLatLng();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set the angle of the map text
        /// </summary>
        /// <param name="angle">The angle.</param>
        public void SetAngle(int angle)
        {
            Angle = angle;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the angle of the map text.
        /// </summary>
        /// <returns>the angle of the map text</returns>
        public int GetAngle()
        {
            return Angle;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set the back color of the map text.
        /// </summary>
        /// <param name="backColor"> color the map text.</param>
        public void SetBackColor(int backColor)
        {
            BackColor = backColor;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the back color of the map text.
        /// </summary>
        /// <returns>the back color of the map text</returns>
        public int GetBackColor()
        {
            return BackColor;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set the fore color of the map text.
        /// </summary>
        /// <param name="foreColor">fore color the map text.</param>
        public void SetForeColor(int foreColor)
        {
            ForeColor = foreColor;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the foreColor of the map text.
        /// </summary>
        /// <returns>the foreColor of the map text</returns>
        public int GetForeColor()
        {
            return ForeColor;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set the justification of the map text.
        /// </summary>
        /// <param name="justification">justification the map text</param>
        public void SetJustification(int justification)
        {
            Justification = justification;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the justification of the map text.
        /// </summary>
        /// <returns>the justification of the map text.</returns>
        public int GetJustification()
        {
            return Justification;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set the spacing of the map text.
        /// </summary>
        /// <param name="spacing">The spacing the map text.</param>
        public void SetSpacing(int spacing)
        {
            Spacing = spacing;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the spacing of the map text.
        /// </summary>
        /// <returns>the spacing of the map text.</returns>
        public int GetSpacing()
        {
            return Spacing;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set the lineType of the map text.
        /// </summary>
        /// <param name="lineType">the lineType of the map text.</param>
        public void SetLineType(int lineType)
        {
            LineType = lineType;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the lineType of the map text.
        /// </summary>
        /// <returns>the lineType of the map text.</returns>
        public int GetLineType()
        {
            return LineType;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set the textString of the map text.
        /// </summary>
        /// <param name="textString">The text string.</param>
        public void SetTextString(string textString)
        {
            TextString = textString;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the textString of the map text.
        /// </summary>
        /// <returns>the textString of the map text.</returns>
        public string GetTextString()
        {
            return TextString;
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
        /// <param name="p">The location.</param>
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
            string retStr = "TEXT  ";
            retStr += "\"" + TextString + "\"" + Crlf;

            retStr += Bounds.GetMinX() + " " + Bounds.GetMinY() + " " +
                    Bounds.GetMaxX() + " " + Bounds.GetMaxY() + Crlf;

            return retStr;
        }

    }

}
