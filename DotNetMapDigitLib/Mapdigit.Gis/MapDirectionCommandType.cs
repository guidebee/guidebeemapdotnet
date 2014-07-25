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
    /// Driving diretion types.
    /// </summary>
    public class MapDirectionCommandType
    {

        /// <summary>
        /// Invalid direction.
        /// </summary>
        public const int CommandInvalid = -1;

        /// <summary>
        /// go straight ahead 
        /// </summary>
        public const int CommandNoTurn = 5001;

        /// <summary>
        /// turn slightly left
        /// </summary>
        public const int CommandBearLeft = 5002;

        /// <summary>
        /// turn left
        /// </summary>
        public const int CommandTurnLeft = 5003;

        /// <summary>
        /// turn sharply left
        /// </summary>
        public const int CommandSharpLeft = 5004;

        /// <summary>
        /// U turn.
        /// </summary>
        public const int CommandUTurn = 5005;

        /// <summary>
        /// turn sharply right
        /// </summary>
        public const int CommandSharpRight = 5006;

        /// <summary>
        /// turn right
        /// </summary>
        public const int CommandTurnRight = 5007;
        /// <summary>
        /// turn slightly right
        /// </summary>
        public const int CommandBearRight = 5008;
        /// <summary>
        /// road merge command
        /// </summary>
        public const int CommandMerge = 5009;

        /// <summary>
        /// keep left at the fork to continue
        /// </summary>
        public const int CommandKeepLeft = 5010;

        /// <summary>
        /// keep right at the fork to continue
        /// </summary>
        public const int CommandKeepRight = 5011;

        /// <summary>
        /// reach the destination.
        /// </summary>
        public const int CommandReachDestination = 5018;


        /// <summary>
        /// round about 1st exit
        /// </summary>
        public const int CommandRoundabout1Exit = 5021;
        /// <summary>
        /// round about 2nd exit
        /// </summary>
        public const int CommandRoundabout2Exit = 5022;
        /// <summary>
        /// round about 3rd exit
        /// </summary>
        public const int CommandRoundabout3Exit = 5023;
        /// <summary>
        /// round about 4th exit
        /// </summary>
        public const int CommandRoundabout4Exit = 5024;
        /// <summary>
        /// round about 5th exit
        /// </summary>
        public const int CommandRoundabout5Exit = 5025;
        /// <summary>
        /// round about 6th exit
        /// </summary>
        public const int CommandRoundabout6Exit = 5026;
        /// <summary>
        /// round about 7th exit
        /// </summary>
        public const int CommandRoundabout7Exit = 5027;
        /// <summary>
        /// round about 8th exit
        /// </summary>
        public const int CommandRoundabout8Exit = 5028;
        /// <summary>
        /// round about 9th exit
        /// </summary>
        public const int CommandRoundabout9Exit = 5029;
        /// <summary>
        /// round about 10th exit
        /// </summary>
        public const int CommandRoundabout10Exit = 5030;

        /// <summary>
        /// enter highway.
        /// </summary>
        public const int CommandEnterHighway = 5040;
        /// <summary>
        /// enter highway on the left.
        /// </summary>
        public const int CommandEnterHighwayLeft = 5041;
        /// <summary>
        /// enter highway on the right.
        /// </summary>
        public const int CommandEnterHighwayRight = 5042;

        /// <summary>
        /// exit the highway
        /// </summary>
        public const int CommandLeaveHighway = 5043;
        /// <summary>
        /// exit the highway on the left
        /// </summary>
        public const int CommandLeaveHighwayLeft = 5044;
        /// <summary>
        /// exit the highway on the right
        /// </summary>
        public const int CommandLeaveHighwayRight = 5045;

        /// <summary>
        /// start command direction, head north direction.
        /// </summary>
        public const int CommandHeadDirectionN = 5050;
        /// <summary>
        /// start command direction, head north east direction.
        /// </summary>
        public const int CommandHeadDirectionNe = 5051;
        /// <summary>
        /// start command direction, head east direction.
        /// </summary>
        public const int CommandHeadDirectionE = 5052;
        /// <summary>
        /// start command direction, head south east direction.
        /// </summary>
        public const int CommandHeadDirectionSe = 5053;
        /// <summary>
        /// start command direction, head south direction.
        /// </summary>
        public const int CommandHeadDirectionS = 5054;
        /// <summary>
        /// start command direction, head south west direction.
        /// </summary>
        public const int CommandHeadDirectionSw = 5055;
        /// <summary>
        /// start command direction, head west direction.
        /// </summary>
        public const int CommandHeadDirectionW = 5056;
        /// <summary>
        /// start command direction, head north west direction.
        /// </summary>
        public const int CommandHeadDirectionNw = 5057;

        /// <summary>
        /// take the 1st left.
        /// </summary>
        public const int CommandTake1Left = 5061;

        /// <summary>
        /// take the 2nd left.
        /// </summary>
        public const int CommandTake2Left = 5062;

        /// <summary>
        /// take the 3rd left.
        /// </summary>
        public const int CommandTake3Left = 5063;

        /// <summary>
        /// take the 4th left.
        /// </summary>
        public const int CommandTake4Left = 5064;

        /// <summary>
        /// take the 5th left.
        /// </summary>
        public const int CommandTake5Left = 5065;

        /// <summary>
        /// take the 6th left.
        /// </summary>
        public const int CommandTake6Left = 5066;

        /// <summary>
        /// take the 7th left.
        /// </summary>
        public const int CommandTake7Left = 5067;

        /// <summary>
        /// take the 8th left.
        /// </summary>
        public const int CommandTake8Left = 5068;

        /// <summary>
        /// take the 9th left.
        /// </summary>
        public const int CommandTake9Left = 5069;

        /// <summary>
        /// take the 1st right.
        /// </summary>
        public const int CommandTake1Right = 5071;

        /// <summary>
        /// take the 2nd right.
        /// </summary>
        public const int CommandTake2Right = 5072;

        /// <summary>
        /// take the 3rd right.
        /// </summary>
        public const int CommandTake3Right = 5073;

        /// <summary>
        /// take the 4th right.
        /// </summary>
        public const int CommandTake4Right = 5074;

        /// <summary>
        /// take the 5th right.
        /// </summary>
        public const int CommandTake5Right = 5075;

        /// <summary>
        /// take the 6th right.
        /// </summary>
        public const int CommandTake6Right = 5076;

        /// <summary>
        /// take the 7th right.
        /// </summary>
        public const int CommandTake7Right = 5077;

        /// <summary>
        /// take the 8th right.
        /// </summary>
        public const int CommandTake8Right = 5078;

        /// <summary>
        /// take the 9th right.
        /// </summary>
        public const int CommandTake9Right = 5079;

        /// <summary>
        /// destination on the left.
        /// </summary>
        public const int CommandDestinationOnTheLeft = 5080;

        /// <summary>
        /// destination on the right.
        /// </summary>
        public const int CommandDestinationOnTheRight = 5081;

        /// <summary>
        /// direction type.
        /// </summary>
        public int Type = CommandInvalid;


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapDirectionCommandType"/> class.
        /// </summary>
        /// <param name="direction">T the direction object copied from</param>
        public MapDirectionCommandType(MapDirectionCommandType direction)
        {
            Type = direction.Type;

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapDirectionCommandType"/> class.
        /// </summary>
        /// <param name="type">the direction type.</param>
        public MapDirectionCommandType(int type)
        {
            Type = type;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Check if it's round about.
        /// </summary>
        /// <returns>
        /// 	true,it's roundabout.
        /// </returns>
        public bool IsRoundAbout()
        {
            return (Type >= CommandRoundabout1Exit &&
                    Type <= CommandRoundabout10Exit);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Check if a junction command
        /// </summary>
        /// <returns>
        /// 	true,it's a junction.
        /// </returns>
        public bool IsJunction()
        {
            return (Type >= CommandNoTurn &&
                    Type <= CommandMerge);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Check if a enter highway command
        /// </summary>
        /// <returns>
        /// 	true,it's a enter highway command.
        /// </returns>
        public bool IsEnterHighway()
        {
            return (Type >= CommandEnterHighway &&
                    Type <= CommandEnterHighwayRight);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///Check if a leave highway command
        /// </summary>
        /// <returns>
        /// 	true,it's a leave highway command.
        /// </returns>
        public bool IsLeaveHighway()
        {
            return (Type >= CommandLeaveHighway &&
                    Type <= CommandLeaveHighwayRight);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Check if a start command
        /// </summary>
        /// <returns>
        /// 	true,it's a a start command.
        /// </returns>
        public bool IsStart()
        {
            return (Type >= CommandHeadDirectionN &&
                    Type <= CommandHeadDirectionNw);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Check if a end command
        /// </summary>
        /// <returns>
        /// 	true,it's a a end command.
        /// </returns>
        public bool IsEnd()
        {
            return (Type == CommandReachDestination);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the string format.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string GetStringFormat(int type)
        {
            string retStr = "";
            switch (type)
            {
                case CommandInvalid:
                    retStr = "CommandInvalid";
                    break;
                case CommandNoTurn:
                    retStr = "CommandNoTurn";
                    break;
                case CommandBearLeft:
                    retStr = "CommandBearLeft";
                    break;
                case CommandTurnLeft:
                    retStr = "CommandTurnLeft";
                    break;
                case CommandSharpLeft:
                    retStr = "CommandSharpLeft";
                    break;
                case CommandUTurn:
                    retStr = "CommandUTurn";
                    break;
                case CommandSharpRight:
                    retStr = "CommandSharpRight";
                    break;
                case CommandTurnRight:
                    retStr = "CommandTurnRight";
                    break;
                case CommandBearRight:
                    retStr = "CommandBearRight";
                    break;
                case CommandMerge:
                    retStr = "CommandMerge";
                    break;
                case CommandKeepLeft:
                    retStr = "CommandKeepLeft";
                    break;
                case CommandKeepRight:
                    retStr = "CommandKeepRight";
                    break;
                case CommandReachDestination:
                    retStr = "CommandReachDestination";
                    break;
                case CommandRoundabout1Exit:
                    retStr = "CommandRoundabout1Exit";
                    break;
                case CommandRoundabout2Exit:
                    retStr = "CommandRoundabout2Exit";
                    break;
                case CommandRoundabout3Exit:
                    retStr = "CommandRoundabout3Exit";
                    break;
                case CommandRoundabout4Exit:
                    retStr = "CommandRoundabout4Exit";
                    break;
                case CommandRoundabout5Exit:
                    retStr = "CommandRoundabout5Exit";
                    break;
                case CommandRoundabout6Exit:
                    retStr = "CommandRoundabout6Exit";
                    break;
                case CommandRoundabout7Exit:
                    retStr = "CommandRoundabout7Exit";
                    break;
                case CommandRoundabout8Exit:
                    retStr = "CommandRoundabout8Exit";
                    break;
                case CommandEnterHighway:
                    retStr = "CommandEnterHighway";
                    break;
                case CommandEnterHighwayLeft:
                    retStr = "CommandEnterHighwayLeft";
                    break;
                case CommandEnterHighwayRight:
                    retStr = "CommandEnterHighwayRight";
                    break;
                case CommandLeaveHighway:
                    retStr = "CommandLeaveHighway";
                    break;
                case CommandLeaveHighwayLeft:
                    retStr = "CommandLeaveHighwayLeft";
                    break;
                case CommandLeaveHighwayRight:
                    retStr = "CommandLeaveHighwayRight";
                    break;
                case CommandHeadDirectionN:
                    retStr = "CommandHeadDirectionN";
                    break;
                case CommandHeadDirectionNe:
                    retStr = "CommandHeadDirectionNe";
                    break;
                case CommandHeadDirectionE:
                    retStr = "CommandHeadDirectionE";
                    break;
                case CommandHeadDirectionSe:
                    retStr = "CommandHeadDirectionSe";
                    break;
                case CommandHeadDirectionS:
                    retStr = "CommandHeadDirectionS";
                    break;
                case CommandHeadDirectionSw:
                    retStr = "CommandHeadDirectionSw";
                    break;
                case CommandHeadDirectionW:
                    retStr = "CommandHeadDirectionW";
                    break;
                case CommandHeadDirectionNw:
                    retStr = "CommandHeadDirectionNw";
                    break;
                case CommandTake1Left:
                    retStr = "CommandTake1Left";
                    break;
                case CommandTake2Left:
                    retStr = "CommandTake2Left";
                    break;
                case CommandTake3Left:
                    retStr = "CommandTake3Left";
                    break;
                case CommandTake4Left:
                    retStr = "CommandTake4Left";
                    break;
                case CommandTake5Left:
                    retStr = "CommandTake5Left";
                    break;
                case CommandTake6Left:
                    retStr = "CommandTake6Left";
                    break;
                case CommandTake7Left:
                    retStr = "CommandTake7Left";
                    break;
                case CommandTake8Left:
                    retStr = "CommandTake8Left";
                    break;
                case CommandTake9Left:
                    retStr = "CommandTake9Left";
                    break;
                case CommandTake1Right:
                    retStr = "CommandTake1Right";
                    break;
                case CommandTake2Right:
                    retStr = "CommandTake2Right";
                    break;
                case CommandTake3Right:
                    retStr = "CommandTake3Right";
                    break;
                case CommandTake4Right:
                    retStr = "CommandTake4Right";
                    break;
                case CommandTake5Right:
                    retStr = "CommandTake5Right";
                    break;
                case CommandTake6Right:
                    retStr = "CommandTake6Right";
                    break;
                case CommandTake7Right:
                    retStr = "CommandTake7Right";
                    break;
                case CommandTake8Right:
                    retStr = "CommandTake8Right";
                    break;
                case CommandTake9Right:
                    retStr = "CommandTake9Right";
                    break;
                case CommandDestinationOnTheLeft:
                    retStr = "CommandDestinationOnTheLeft";
                    break;
                case CommandDestinationOnTheRight:
                    retStr = "CommandDestinationOnTheRight";
                    break;
            }
            return retStr;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// to string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return GetStringFormat(Type);
        }
    }
}
