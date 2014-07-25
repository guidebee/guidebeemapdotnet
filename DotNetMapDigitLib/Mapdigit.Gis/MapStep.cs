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
using System.Text;
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
    /// Objects of this class store information about a single step within a route
    /// in a directions result.
    /// </summary>
    public sealed class MapStep : MapObject
    {

        /// <summary>
        /// First point of the step.
        /// </summary>
        public GeoLatLng FirstLatLng;

        /// <summary>
        /// the index of the first point in the polyline of given direciton.
        /// </summary>
        public int FirstLocationIndex;

        /// <summary>
        /// last point of the step.
        /// </summary>
        public GeoLatLng LastLatLng;

        /// <summary>
        /// the index of the last point in the polyline of given direciton.
        /// </summary>
        public int LastLocationIndex;

        /// <summary>
        /// Description about this step.
        /// </summary>
        public string Description;

        /// <summary>
        /// Description about this step in English.
        /// </summary>
        public string DescriptionEnglish;

        /// <summary>
        /// total duration of the step in seconds.
        /// </summary>
        public double Duration;

        /// <summary>
        /// total Distance of the step in meters.
        /// </summary>
        public double Distance;

        /// <summary>
        /// bearing [0-360)
        /// </summary>
        public int Bearing;

        /// <summary>
        /// Direction type.
        /// </summary>
        public MapDirectionCommandType CalculatedDirectionType;

        /// <summary>
        /// Direction command elements used to navigaion.
        /// </summary>
        public MapDirectionCommandElement[] DirectionCommandElements;


        /// <summary>
        /// current road name.
        /// </summary>
        public string CurrentRoadName;

        /// <summary>
        /// tag related to this map steps ,like image or else.
        /// </summary>
        public object Tag;


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapStep"/> class.
        /// </summary>
        internal MapStep()
        {
            CalculatedDirectionType = new MapDirectionCommandType(MapDirectionCommandType.CommandInvalid);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 01JAN2011  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        ///<summary>
        /// return direction command type if has any.
        ///</summary>
        ///<returns>the direction command type</returns>
        public int GetDirectionCommandType()
        {
            try
            {
                return DirectionCommandElements[MapDirectionCommandElement.DirectionCommand]
                        .DirectionCommandType.Type;
            }
            catch (Exception ) { }
            return MapDirectionCommandType.CommandInvalid;
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
            StringBuilder sb = new StringBuilder();
            sb.Append("Current road name:" + CurrentRoadName + "\r\n");
            sb.Append("Description:" + Description + "\r\n");
            sb.Append("Distance:" + Distance + "\r\n");
            sb.Append("Duration:" + Duration + "\r\n");
            sb.Append("Bearing:" + Bearing);
            sb.Append("Calculated Direction:" + CalculatedDirectionType + "\r\n");

            if (DirectionCommandElements[2] != null)
            {
                sb.Append("Analysed Direction:"
                        + DirectionCommandElements[2]
                              .DirectionCommandType + "\r\n");
            }
            if (DirectionCommandElements != null)
            {
                for (int i = 0; i < DirectionCommandElements.Length; i++)
                {
                    if (DirectionCommandElements[i] != null)
                    {
                        sb.Append(DirectionCommandElements[i].Description + "-" + "\r\n");
                    }
                    else
                    {
                        sb.Append(" " + "-" + "\r\n");
                    }
                }
                sb.Append("\r\n");
            }

            return sb.ToString();
        }



    }

}
