//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 29SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System;
using Mapdigit.Gis.Geometry;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Gis.Navigation
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 29SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// voice command argument.
    /// </summary>
    public class VoiceCommandArg
    {
        /// <summary>
        /// voice command type.
        /// </summary>
        public int VoiceCommandType;

        /// <summary>
        /// sub voice command type ,give more detail about the voice command.
        /// </summary>
        public int SubVoiceCommandType;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="VoiceCommandArg"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="subType">Type of the sub.</param>
        /// <param name="arg">The arg.</param>
        /// <param name="optional"></param>
        public VoiceCommandArg(int type, int subType, object arg, bool optional)
        {
            SubVoiceCommandType = subType;
            VoiceCommandType = type;
            _commandArgument = arg;
            _optional = optional;
        }

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="VoiceCommandArg"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="arg">The arg.</param>
        /// <param name="optional">is the command optinal or not.</param>
        public VoiceCommandArg(int type, object arg, bool optional)
        {
         
            VoiceCommandType = type;
            _commandArgument = arg;
            _optional = optional;
        }

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="VoiceCommandArg"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="arg">The arg.</param>
        public VoiceCommandArg(int type, object arg)
        {
            VoiceCommandType = type;
            _commandArgument = arg;
            _optional = true;
        }

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the type of the command.
        /// </summary>
        /// <returns></returns>
        public int GetCommandType()
        {
            return VoiceCommandType;
        }

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the calculated type of the command.
        /// </summary>
        /// <returns></returns>
        public int GetCalculatedCommandType()
        {
            return SubVoiceCommandType;
        }

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the command arg.
        /// </summary>
        /// <returns></returns>
        public object GetCommandArg()
        {
            return _commandArgument;
        }

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determines whether this instance is optional.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance is optional; otherwise, <c>false</c>.
        /// </returns>
        public bool IsOptional()
        {
            return _optional;
        }


        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <returns>command location</returns>
        public GeoLatLng GetLocation()
        {
            return new GeoLatLng(_commandLocation);
        }

        /**
         * optioal or not
         */
        internal bool _optional = true;

        /**
         * voice command argument, normally it's road name (string).
         */
        internal object _commandArgument;

        /**
         * the index of the route.
         */
        internal int _routeIndex;

        /**
         * the index of the step.
         */
        internal int _stepIndex;

        /**
         * the index of the  point
         */
        internal int _pointIndex;

        /**
         * location where the command should be fired.
         */
        internal GeoLatLng _commandLocation;
    }
}
