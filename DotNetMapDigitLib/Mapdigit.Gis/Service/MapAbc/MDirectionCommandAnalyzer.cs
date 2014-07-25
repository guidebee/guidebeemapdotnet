//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 28SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System;
using System.Collections;
using Mapdigit.Util;

//--------------------------------- PACKAGE -----------------------------------

namespace Mapdigit.Gis.Service.MapAbc
{
    /// <summary>
    /// Driving diretion command analyser.
    /// </summary>
    internal static class MDirectionCommandAnalyzer
    {
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Analyse the input direction in text (English).
        /// </summary>
        /// <param name="direction">the input direction description.</param>
        /// <param name="roadName"></param>
        /// <returns>command element list (5 elements).</returns>
        public static MapDirectionCommandElement[] Analyse(string direction, string roadName)
        {
            lock (SyncObject)
            {
                _resultDirectionCommandElements
                    = new MapDirectionCommandElement[7];
                if (HasCommand(direction))
                {

                    _resultDirectionCommandElements[MapDirectionCommandElement.DirectionCommand]
                        = new MapDirectionCommandElement
                            (MapDirectionCommandElement.ElementCommand,
                             _commandText);
                    _resultDirectionCommandElements[MapDirectionCommandElement.DirectionCommand]
                        .DirectionCommandType =
                        new MapDirectionCommandType(_directionCommandType);

                }
                else
                {
                    Log.P("Wrong direction:" + direction, Log.Debug);
                    _resultDirectionCommandElements[MapDirectionCommandElement.DirectionCommand]
                        = new MapDirectionCommandElement
                            (MapDirectionCommandElement.ElementCommand,
                             "merge");
                    _resultDirectionCommandElements[MapDirectionCommandElement.DirectionCommand]
                        .DirectionCommandType =
                        new MapDirectionCommandType(MapDirectionCommandType.CommandMerge);
                }
                return _resultDirectionCommandElements;
            }

        }




        private static readonly string[] CommandNoTurn = {
                                                             "减速行驶",
                                                             "直行"
                                                         };

        private static readonly string[] CommandBearLeft = {
                                                               "向左前方行驶",
                                                           };


        private static readonly string[] CommandTurnLeft = {
                                                               "左转",
                                                           };

        private static readonly string[] CommandSharpLeft = {
                                                                "tshl",
                                                            };

        private static readonly string[] CommandSharpRight = {
                                                                 "tshr",
                                                             };

        private static readonly string[] CommandTurnRight = {
                                                                "右转",
                                                            };


        private static readonly string[] CommandBearRight = {
                                                                "向右前方行驶",
                                                            };

        private static readonly string[] CommandMerge = {
                                                            "merge",
                                                            "enter"
                                                        };



        private static readonly string[] CommandKeepLeft = {
                                                               "靠左",
                                   
                                                           };


        private static readonly string[] CommandKeepRight = {
                                                                "靠右",
                                                            };


        private static readonly string[] CommandEnterHighway = {
                                                                   "take the ramp"
                                                               };

        private static readonly string[] CommandEnterHighwayLeft = {
                                                                       "take the ramp on the left",
                                                                       "slight left onto the ramp",
                                                                   };

        private static readonly string[] CommandEnterHighwayRight = {
                                                                        "take the ramp on the right",
                                                                        "slight right onto the ramp"
                                                                    };

        private static readonly string[] CommandLeaveHighway = {
                                                                   "take the exit",
                                                                   "take exit",
                                                                   "exit",
                                                               };

        private static readonly string[] CommandLeaveHighwayLeft = {
                                                                       "take the exit on the left"
                                                                   };

        private static readonly string[] CommandLeaveHighwayRight = {
                                                                        "take the exit on the right"
                                                                    };

        private static readonly string[] CommandUTurn = {
                                                            "左转调头",
                                                            "右转调头",

                                                        };

        //the following are multi commands.
        private static readonly string[] CommandRoundaboutExit = {
                                                                     "take the",
                                                                     "exit"
                                                                 };

        private static readonly string[] CommandTakeLeft = {
                                                               "take the",
                                                               "left"
                                                           };

        private static readonly string[] CommandTakeRight = {
                                                                "take the",
                                                                "right"
                                                            };

        private static readonly string[] CommandHeadDirection = {
                                                                    "head",
                                                                    "north",
                                                                    "northeast",
                                                                    "east",
                                                                    "southeast",
                                                                    "south",
                                                                    "southwest",
                                                                    "west",
                                                                    "northwest"
                                                                };

        private static readonly string[] CommandDestinationOnTheLeft = {
                                                                           "destination will be on the left",
                                                                       };

        private static readonly string[] CommandDestinationOnTheRight = {
                                                                            "destination will be on the right"
                                                                        };


        private static readonly string[][] CommandList = {

                                                             CommandEnterHighway,
                                                             CommandEnterHighwayLeft,
                                                             CommandEnterHighwayRight,
                                                             CommandLeaveHighway,
                                                             CommandLeaveHighwayLeft,
                                                             CommandLeaveHighwayRight,
                                                             CommandBearLeft,
                                                             CommandTurnLeft,
                                                             CommandSharpLeft,
                                                             CommandSharpRight,
                                                             CommandTurnRight,
                                                             CommandBearRight,
                                                             CommandKeepLeft,
                                                             CommandKeepRight,
                                                             CommandMerge,
                                                             CommandNoTurn,
                                                             CommandUTurn,
                                                             //CommandDestinationOnTheLeft,
                                                             //CommandDestinationOnTheRight
                                                             //CommandRoundaboutExit,
                                                             //CommandHeadDirection,
                                                             //CommandTakeRight,
                                                             //CommandTakeLeft
                                                         };

        private static readonly string[] CommandAllList;

        private static readonly string[] CommandDestinaionAllList;

        //initialized the all command list.
        static MDirectionCommandAnalyzer()
        {

            int total = CommandDestinationOnTheLeft.Length
                        + CommandDestinationOnTheRight.Length;
            CommandDestinaionAllList = new string[total];
            total = 0;
            for (int i = 0; i < CommandDestinationOnTheLeft.Length; i++)
            {
                CommandDestinaionAllList[total++] = CommandDestinationOnTheLeft[i];
            }
            for (int i = 0; i < CommandDestinationOnTheRight.Length; i++)
            {
                CommandDestinaionAllList[total++] = CommandDestinationOnTheRight[i];
            }


            total = 0;
            for (int i = 0; i < CommandList.Length; i++)
            {
                string[] commands = CommandList[i];
                if (commands != CommandRoundaboutExit &&
                    commands != CommandHeadDirection)
                {
                    total += commands.Length;
                }
            }

            //add head command.
            total += CommandHeadDirection.Length - 1;

            //add roadabout command.
            total += 9;

            //add take the left command.
            total += 9;

            //add take the right command.
            total += 9;


            CommandAllList = new string[total];
            total = 0;
            for (int i = 0; i < CommandList.Length; i++)
            {
                string[] commands = CommandList[i];
                if (commands != CommandRoundaboutExit &&
                    commands != CommandHeadDirection)
                {
                    for (int j = 0; j < commands.Length; j++)
                    {
                        CommandAllList[total++] = commands[j];
                    }
                }
            }


            //add head command
            for (int k = 1; k < CommandHeadDirection.Length; k++)
            {
                string command = CommandHeadDirection[0] + " " + CommandHeadDirection[k];
                CommandAllList[total++] = command;
            }

            //add exit round command
            CommandAllList[total++] = CommandRoundaboutExit[0]
                                      + " 1st " + CommandRoundaboutExit[1];
            CommandAllList[total++] = CommandRoundaboutExit[0]
                                      + " 2nd " + CommandRoundaboutExit[1];
            CommandAllList[total++] = CommandRoundaboutExit[0]
                                      + " 3rd " + CommandRoundaboutExit[1];

            for (int k = 4; k < 10; k++)
            {
                string command = CommandRoundaboutExit[0]
                                 + " " + k + "th " + CommandRoundaboutExit[1];
                CommandAllList[total++] = command;
            }

            //add take x left command
            CommandAllList[total++] = CommandTakeLeft[0]
                                      + " 1st " + CommandTakeLeft[1];
            CommandAllList[total++] = CommandTakeLeft[0]
                                      + " 2nd " + CommandTakeLeft[1];
            CommandAllList[total++] = CommandTakeLeft[0]
                                      + " 3rd " + CommandTakeLeft[1];

            for (int k = 4; k < 10; k++)
            {
                string command = CommandTakeLeft[0]
                                 + " " + k + "th " + CommandTakeLeft[1];
                CommandAllList[total++] = command;
            }

            //add take x right command
            CommandAllList[total++] = CommandTakeRight[0]
                                      + " 1st " + CommandTakeRight[1];
            CommandAllList[total++] = CommandTakeRight[0]
                                      + " 2nd " + CommandTakeRight[1];
            CommandAllList[total++] = CommandTakeRight[0]
                                      + " 3rd " + CommandTakeRight[1];

            for (int k = 4; k < 10; k++)
            {
                string command = CommandTakeRight[0]
                                 + " " + k + "th " + CommandTakeRight[1];
                CommandAllList[total++] = command;
            }

            for (int i = 0; i < CommandAllList.Length - 1; i++)
            {
                for (int j = i + 1; j < CommandAllList.Length; j++)
                {
                    if (CommandAllList[i].Length < CommandAllList[j].Length)
                    {
                        string temp = CommandAllList[i];
                        CommandAllList[i] = CommandAllList[j];
                        CommandAllList[j] = temp;
                    }
                }
            }

            for (int i = 0; i < CommandNoTurn.Length; i++)
            {
                DirectionCommandTypes.Add(CommandNoTurn[i],
                                          new MapDirectionCommandType(MapDirectionCommandType
                                                                          .CommandNoTurn));
            }
            for (int i = 0; i < CommandTurnLeft.Length; i++)
            {
                DirectionCommandTypes.Add(CommandTurnLeft[i],
                                          new MapDirectionCommandType(MapDirectionCommandType
                                                                          .CommandTurnLeft));
            }
            for (int i = 0; i < CommandBearLeft.Length; i++)
            {
                DirectionCommandTypes.Add(CommandBearLeft[i],
                                          new MapDirectionCommandType(MapDirectionCommandType
                                                                          .CommandBearLeft));
            }
            for (int i = 0; i < CommandSharpLeft.Length; i++)
            {
                DirectionCommandTypes.Add(CommandSharpLeft[i],
                                          new MapDirectionCommandType(MapDirectionCommandType
                                                                          .CommandSharpLeft));
            }
            for (int i = 0; i < CommandSharpRight.Length; i++)
            {
                DirectionCommandTypes.Add(CommandSharpRight[i],
                                          new MapDirectionCommandType(MapDirectionCommandType
                                                                          .CommandSharpRight));
            }
            for (int i = 0; i < CommandTurnRight.Length; i++)
            {
                DirectionCommandTypes.Add(CommandTurnRight[i],
                                          new MapDirectionCommandType(MapDirectionCommandType
                                                                          .CommandTurnRight));
            }
            for (int i = 0; i < CommandBearRight.Length; i++)
            {
                DirectionCommandTypes.Add(CommandBearRight[i],
                                          new MapDirectionCommandType(MapDirectionCommandType
                                                                          .CommandBearRight));
            }
            for (int i = 0; i < CommandMerge.Length; i++)
            {
                DirectionCommandTypes.Add(CommandMerge[i],
                                          new MapDirectionCommandType(MapDirectionCommandType
                                                                          .CommandMerge));
            }
            for (int i = 0; i < CommandEnterHighway.Length; i++)
            {
                DirectionCommandTypes.Add(CommandEnterHighway[i],
                                          new MapDirectionCommandType(MapDirectionCommandType
                                                                          .CommandEnterHighway));
            }
            for (int i = 0; i < CommandEnterHighwayLeft.Length; i++)
            {
                DirectionCommandTypes.Add(CommandEnterHighwayLeft[i],
                                          new MapDirectionCommandType(MapDirectionCommandType
                                                                          .CommandEnterHighwayLeft));
            }
            for (int i = 0; i < CommandEnterHighwayRight.Length; i++)
            {
                DirectionCommandTypes.Add(CommandEnterHighwayRight[i],
                                          new MapDirectionCommandType(MapDirectionCommandType
                                                                          .CommandEnterHighwayRight));
            }
            for (int i = 0; i < CommandLeaveHighway.Length; i++)
            {
                DirectionCommandTypes.Add(CommandLeaveHighway[i],
                                          new MapDirectionCommandType(MapDirectionCommandType
                                                                          .CommandLeaveHighway));
            }
            for (int i = 0; i < CommandLeaveHighwayLeft.Length; i++)
            {
                DirectionCommandTypes.Add(CommandLeaveHighwayLeft[i],
                                          new MapDirectionCommandType(MapDirectionCommandType
                                                                          .CommandLeaveHighwayLeft));
            }
            for (int i = 0; i < CommandLeaveHighwayRight.Length; i++)
            {
                DirectionCommandTypes.Add(CommandLeaveHighwayRight[i],
                                          new MapDirectionCommandType(MapDirectionCommandType
                                                                          .CommandLeaveHighwayRight));
            }
            for (int i = 0; i < CommandKeepLeft.Length; i++)
            {
                DirectionCommandTypes.Add(CommandKeepLeft[i],
                                          new MapDirectionCommandType(MapDirectionCommandType
                                                                          .CommandKeepLeft));
            }
            for (int i = 0; i < CommandKeepRight.Length; i++)
            {
                DirectionCommandTypes.Add(CommandKeepRight[i],
                                          new MapDirectionCommandType(MapDirectionCommandType
                                                                          .CommandKeepRight));
            }
            for (int i = 0; i < CommandUTurn.Length; i++)
            {
                DirectionCommandTypes.Add(CommandUTurn[i],
                                          new MapDirectionCommandType(MapDirectionCommandType
                                                                          .CommandUTurn));
            }
            for (int i = 0; i < CommandDestinationOnTheLeft.Length; i++)
            {
                DirectionCommandTypes.Add(CommandDestinationOnTheLeft[i],
                                          new MapDirectionCommandType(MapDirectionCommandType
                                                                          .CommandDestinationOnTheLeft));
            }
            for (int i = 0; i < CommandDestinationOnTheRight.Length; i++)
            {
                DirectionCommandTypes.Add(CommandDestinationOnTheRight[i],
                                          new MapDirectionCommandType(MapDirectionCommandType
                                                                          .CommandDestinationOnTheRight));
            }
            //roundabout
            DirectionCommandTypes.Add(CommandRoundaboutExit[0]
                                      + " 1st " + CommandRoundaboutExit[1],
                                      new MapDirectionCommandType(MapDirectionCommandType
                                                                      .CommandRoundabout1Exit));
            DirectionCommandTypes.Add(CommandRoundaboutExit[0]
                                      + " 2nd " + CommandRoundaboutExit[1],
                                      new MapDirectionCommandType(MapDirectionCommandType
                                                                      .CommandRoundabout2Exit));
            DirectionCommandTypes.Add(CommandRoundaboutExit[0]
                                      + " 3rd " + CommandRoundaboutExit[1],
                                      new MapDirectionCommandType(MapDirectionCommandType
                                                                      .CommandRoundabout3Exit));
            for (int i = 4; i < 10; i++)
            {
                DirectionCommandTypes.Add(CommandRoundaboutExit[0]
                                          + " " + i + "th " + CommandRoundaboutExit[1],
                                          new MapDirectionCommandType(MapDirectionCommandType
                                                                          .CommandRoundabout3Exit + i - 3));
            }

            //take the xxx left
            DirectionCommandTypes.Add(CommandTakeLeft[0]
                                      + " 1st " + CommandTakeLeft[1],
                                      new MapDirectionCommandType(MapDirectionCommandType
                                                                      .CommandTake1Left));
            DirectionCommandTypes.Add(CommandTakeLeft[0]
                                      + " 2nd " + CommandTakeLeft[1],
                                      new MapDirectionCommandType(MapDirectionCommandType
                                                                      .CommandTake2Left));
            DirectionCommandTypes.Add(CommandTakeLeft[0]
                                      + " 3rd " + CommandTakeLeft[1],
                                      new MapDirectionCommandType(MapDirectionCommandType
                                                                      .CommandTake3Left));
            for (int i = 4; i < 10; i++)
            {
                DirectionCommandTypes.Add(CommandTakeLeft[0]
                                          + " " + i + "th " + CommandTakeLeft[1],
                                          new MapDirectionCommandType(MapDirectionCommandType
                                                                          .CommandTake3Left + i - 3));
            }

            //take the xxx right
            DirectionCommandTypes.Add(CommandTakeRight[0]
                                      + " 1st " + CommandTakeRight[1],
                                      new MapDirectionCommandType(MapDirectionCommandType
                                                                      .CommandTake1Right));
            DirectionCommandTypes.Add(CommandTakeRight[0]
                                      + " 2nd " + CommandTakeRight[1],
                                      new MapDirectionCommandType(MapDirectionCommandType
                                                                      .CommandTake2Right));
            DirectionCommandTypes.Add(CommandTakeRight[0]
                                      + " 3rd " + CommandTakeRight[1],
                                      new MapDirectionCommandType(MapDirectionCommandType
                                                                      .CommandTake3Right));
            for (int i = 4; i < 10; i++)
            {
                DirectionCommandTypes.Add(CommandTakeRight[0]
                                          + " " + i + "th " + CommandTakeRight[1],
                                          new MapDirectionCommandType(MapDirectionCommandType
                                                                          .CommandTake3Right + i - 3));
            }

            //head direction.
            for (int i = 1; i < 9; i++)
            {
                DirectionCommandTypes.Add(CommandHeadDirection[0]
                                          + " " + CommandHeadDirection[i],
                                          new MapDirectionCommandType(MapDirectionCommandType
                                                                          .CommandHeadDirectionN + i - 1));
            }


        }

        private static readonly object SyncObject = new Object();
        private static string _commandText;
        private static int _commandIndex;
        private static MapDirectionCommandType _directionCommandType;
        private static MapDirectionCommandElement[] _resultDirectionCommandElements;

        private static readonly Hashtable DirectionCommandTypes = new Hashtable();


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// check to see if the description contains any direction command.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="index"> the start index of the word to check.</param>
        /// <param name="word">The word.</param>
        /// <returns>
        /// 	true ,it's a word.
        /// </returns>
        private static bool IsWord(string description, int index, string word)
        {
            if (index == 0 || ((index + word.Length + 1) >= description.Length)) return true;
            char ch1 = description[index - 1];
            char ch2 = description[index + word.Length];
            if (ch1 == ' ' && ch2 == ' ') return true;
            return false;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// check to see if the description contains any direction command.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>
        /// 	true ,if contains direction command.
        /// </returns>
        private static bool HasCommand(string description)
        {

            //check if there's command
            for (int j = 0; j < CommandAllList.Length; j++)
            {
                _commandIndex = description.ToLower().IndexOf(CommandAllList[j]);
                if (_commandIndex > -1)
                {
                    _commandText = CommandAllList[j];
                    if (IsWord(description, _commandIndex, _commandText))
                    {
                        _directionCommandType = (MapDirectionCommandType)
                                                DirectionCommandTypes[CommandAllList[j]];
                        return true;
                    }

                }
            }

            return false;
        }

    }
}