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

//--------------------------------- PACKAGE ------------------------------------
using System;

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
    /// Driving diretion command element.A direction for a MapStep consists at most
    /// 5 direction command elements and has following syntax
    /// [Command]-[Adj]-[Road Name]-[Adj]-[Road Name].
    /// loosely speaking, 3 elements is enough for a whole command.
    /// [Command]-[Road Name]-[Road Name]
    /// </summary>
    public class MapDirectionCommandElement
    {

        /// <summary>
        /// no elements.
        /// </summary>
        public const int ElementNone = 0;

        /// <summary>
        /// Road name elements.
        /// </summary>
        public const int ElementRoadName = 1;

        /// <summary>
        /// Roundaboout is treated as special road name in the command syntax.
        /// </summary>
        public const int ElementRoadNameRoundabout = 4;

        /// <summary>
        /// Conjunction element. Conjunction elements is just used for elaboration.
        /// Conjunctions is  onto, on ,towards etc.
        /// </summary>
        public const int ElementConjuction = 2;

        /// <summary>
        /// Command element.
        /// </summary>
        public const int ElementCommand = 3;

        /// <summary>
        /// extenstion. entering
        /// </summary>
        public const int ElementExtentionEntering = 4;

        /// <summary>
        /// extenstion. go through how many roundabout.
        /// </summary>
        public const int ElementExtentionGoThroughRoundabout = 5;

        /// <summary>
        ///  From road conjuction index in array
        /// </summary>
        public const int FromRoadConjunction = 0;

        /// <summary>
        /// From road name index in array
        /// </summary>
        public const int FromRoadName = 1;

        /// <summary>
        /// Direction command type index in array
        /// </summary>
        public const int DirectionCommand = 2;

        /// <summary>
        /// To road conjuection index in array.
        /// </summary>
        public const int ToRoadConjunction = 3;

        /// <summary>
        /// To road name index in array.
        /// </summary>
        public const int ToRoadName = 4;

        /// <summary>
        /// Extention (entering) index in array.
        /// </summary>
        public const int ExtensionEntering = 5;

        /// <summary>
        /// Extension(go through .. roundabout) index in array.
        /// </summary>
        public const int ExtensionGoThroughRoundabout = 6;

        /// <summary>
        /// Description.
        /// </summary>
        public string Description;

        /// <summary>
        /// Element type.
        /// </summary>
        public int ElementType = ElementNone;

        /// <summary>
        /// Direction command type.
        /// </summary>
        public MapDirectionCommandType DirectionCommandType;

        /// <summary>
        /// road property ,toll road or partial toll road.
        /// </summary>
        public string RoadProperty;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapDirectionCommandElement"/> class.
        /// </summary>
        /// <param name="element"> the element object copied from.</param>
        public MapDirectionCommandElement(MapDirectionCommandElement element)
        {
            ElementType = element.ElementType;
            Description = element.Description;

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapDirectionCommandElement"/> class.
        /// </summary>
        /// <param name="elementType">Type of the element.</param>
        /// <param name="description">The description.</param>
        public MapDirectionCommandElement(int elementType, string description)
        {
            ElementType = elementType;
            Description = description;

        }

    }
}
