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
using Mapdigit.Gis.Geometry;
using Mapdigit.Util;

//--------------------------------- PACKAGE -----------------------------------
namespace Mapdigit.Gis.Service.Google
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 28SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Driving diretion command analyser.
    /// </summary>
    internal static class GDirectionCommandAnalyzer
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
        /// <returns>command element list (5 elements).</returns>
        public static MapDirectionCommandElement[] Analyse(string direction)
        {
            lock (SyncObject)
            {
                string[] strings = Tokenize(direction, ' ');
                _originalDescription = "";
                for (int i = 0; i < strings.Length; i++)
                {
                    _originalDescription += strings[i] + " ";
                }
                _originalDescription = _originalDescription.Trim();
                //get rid of comma
                direction = _originalDescription;
                strings = Tokenize(direction, ',');
                _originalDescription = "";
                for (int i = 0; i < strings.Length; i++)
                {
                    _originalDescription += strings[i] + " ";
                }
                _originalDescription = _originalDescription.Trim();
                direction = _originalDescription;
                _resultDirectionCommandElements
                            = new MapDirectionCommandElement[7];
                if (HasCommand(direction))
                {

                    string conjText;
                    if (_commandIndex == 0)
                    {
                        conjText = direction.Substring(_commandText.Length).Trim();
                    }
                    else
                    {
                        conjText = (direction.Substring(0, _commandIndex - 1) +
                                direction
                                .Substring(_commandIndex + _commandText.Length)).Trim();
                    }
                    _resultDirectionCommandElements[MapDirectionCommandElement.DirectionCommand]
                            = new MapDirectionCommandElement
                            (MapDirectionCommandElement.ElementCommand,
                            _commandText);
                    _resultDirectionCommandElements[MapDirectionCommandElement.DirectionCommand]
                            .DirectionCommandType =
                            new MapDirectionCommandType(_directionCommandType);
                    ProcessConjuction(conjText);

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

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// analysis last step to see if it contains direction on left/right comannd.
        /// </summary>
        /// <param name="lastStep">The last step.</param>
        /// <returns></returns>
        public static MapStep AnalyseLastStep(MapStep lastStep)
        {
            lock (SyncObject)
            {
                string description = lastStep.Description;
                MapStep mapStep = null;
                if (lastStep.DirectionCommandElements[MapDirectionCommandElement.DirectionCommand]
                        .DirectionCommandType.Type != MapDirectionCommandType.CommandDestinationOnTheLeft
                        && lastStep.DirectionCommandElements[MapDirectionCommandElement.DirectionCommand]
                        .DirectionCommandType.Type != MapDirectionCommandType.CommandDestinationOnTheRight)
                    for (int i = 0; i < CommandDestinaionAllList.Length; i++)
                    {
                        _commandIndex = description.ToLower().IndexOf(CommandDestinaionAllList[i]);
                        if (_commandIndex > -1)
                        {
                            _commandText = CommandDestinaionAllList[i];
                            if (IsWord(description, _commandIndex, _commandText))
                            {
                                _directionCommandType = (MapDirectionCommandType)
                                        DirectionCommandTypes[CommandDestinaionAllList[i]];
                                //found destination on the left
                                string newdescription = description.Substring(0, _commandIndex);
                                mapStep = MapRoute.NewStep();
                                mapStep.Bearing = 0;
                                mapStep.Description = _commandText;
                                mapStep.DirectionCommandElements = new MapDirectionCommandElement[7];
                                mapStep.DirectionCommandElements[MapDirectionCommandElement.DirectionCommand]
                                    = new MapDirectionCommandElement
                                    (MapDirectionCommandElement.ElementCommand,
                                    _commandText);
                                mapStep.DirectionCommandElements[MapDirectionCommandElement.DirectionCommand]
                                    .DirectionCommandType =
                                    new MapDirectionCommandType(_directionCommandType);
                                mapStep.CalculatedDirectionType = new MapDirectionCommandType(_directionCommandType);
                                mapStep.Distance = 0;
                                mapStep.Duration = 0;
                                mapStep.FirstLatLng = new GeoLatLng(lastStep.LastLatLng);
                                mapStep.LastLatLng = new GeoLatLng(lastStep.LastLatLng);
                                mapStep.FirstLocationIndex = lastStep.LastLocationIndex;
                                mapStep.LastLocationIndex = lastStep.LastLocationIndex;
                                lastStep.DirectionCommandElements = Analyse(newdescription);
                                lastStep.Description = newdescription;
                                if (lastStep.DirectionCommandElements
                                        [MapDirectionCommandElement.ToRoadName] != null)
                                {
                                    lastStep.CurrentRoadName = lastStep
                                            .DirectionCommandElements
                                            [MapDirectionCommandElement.ToRoadName].Description;
                                }
                                else if (lastStep.DirectionCommandElements
                                        [MapDirectionCommandElement.FromRoadName] != null)
                                {
                                    lastStep.CurrentRoadName = lastStep
                                            .DirectionCommandElements
                                            [MapDirectionCommandElement.FromRoadName].Description;
                                }
                                mapStep.CurrentRoadName = lastStep.CurrentRoadName;
                                break;
                            }
                        }
                    }
                return mapStep;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tokenizes a string with the given delimiter.
        /// </summary>
        /// <param name="s">s String to be tokenized.</param>
        /// <param name="delimiter"> Character that delimits the string</param>
        /// <returns>Array of tokens(strings)</returns>
        private static string[] Tokenize(string s, char delimiter)
        {
            return s.Split(new[] {delimiter});
        }



        private static readonly string[] CommandNoTurn = {
                                                               "continue",
                                                               "continue straight"
                                                           };

        private static readonly string[] CommandBearLeft = {
                                                                 "turns slightly left",
                                                                 "turn slightly left",
                                                                 "slight left"
                                                             };
                                                           

        private static readonly string[] CommandTurnLeft = {
                                                                 "turns left",
                                                                 "turn left"
                                                             };

        private static readonly string[] CommandSharpLeft = {
                                                                  "turns sharply left",
                                                                  "turn sharply left",
                                                                  "sharp left"
                                                              };

        private static readonly string[] CommandSharpRight = {
                                                                   "turns sharply right",
                                                                   "turn sharply right",
                                                                   "sharp right"
                                                               };

        private static readonly string[] CommandTurnRight = {
                                                                  "turns right",
                                                                  "turn right"
                                                              };


        private static readonly string[] CommandBearRight = {
                                                                  "turns slightly right",
                                                                  "turn slightly right",
                                                                  "slight right"
                                                              };

        private static readonly string[] CommandMerge = {
                                                             "merge",
                                                             "enter"
                                                         };



        private static readonly string[] CommandKeepLeft = {
                                                                 "keep left at the fork to continue",
                                                                 "keep left at the fork"
                                                             };


        private static readonly string[] CommandKeepRight = {
                                                                  "keep right at the fork to continue",
                                                                  "keep right at the fork"
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
                                                              "make a u-turn",
                                                              "take a u-turn",
                                                              "make a uturn",
                                                              "take a uturn"
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

        private static readonly string[] ElementConjuction = {
                                                                  "and becomes", //0
                                                                  "and merge onto", //1
                                                                  "to merge onto", //2
                                                                  "to stay on", //3
                                                                  "to merge onto", //4
                                                                  "follow signs for", //5
                                                                  "onto", //6
                                                                  "on", //7
                                                                  "towards", //8
                                                                  "toward", //9
                                                                  "at", //10
                                                                  "to", //11
                                                                  "and stay on", //12
                                                              };

        private static readonly string[] ElementToConjuction = {
                                                                     ElementConjuction[0],
                                                                     ElementConjuction[1],
                                                                     ElementConjuction[2],
                                                                     ElementConjuction[3],
                                                                     ElementConjuction[4],
                                                                     ElementConjuction[6],
                                                                     ElementConjuction[8],
                                                                     ElementConjuction[9],
                                                                     ElementConjuction[11]
                                                                 };


        private static readonly string[] ElementFinish = {
                                                              "entering",
                                                              "go through"
                                                          };

        private static readonly string[] ElementToll = {
                                                            "partial toll road",
                                                            "toll road"
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
        static GDirectionCommandAnalyzer()
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
        private static string _originalDescription;
        private static string _commandText;
        private static int _commandIndex;
        private static MapDirectionCommandType _directionCommandType;
        private static string _conjuctionText;
        private static int _conjuctionIndex;
        private static MapDirectionCommandElement[] _resultDirectionCommandElements;

        private static readonly Hashtable DirectionCommandTypes = new Hashtable();



        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// check to see if the input start with conjuction.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        private static bool StartWithConjuction(string description)
        {
            for (int i = 0; i < ElementConjuction.Length; i++)
            {
                if (description.ToLower().StartsWith(ElementConjuction[i]))
                {
                    _conjuctionIndex = 0;
                    _conjuctionText = ElementConjuction[i];
                    return true;
                }
            }
            return false;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// check to see if its' to conjuction (for to road).
        /// </summary>
        /// <param name="conj">The conj.</param>
        /// <returns>
        /// 	<c>true</c> if [is to conjuction] [the specified conj]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsToConjuction(string conj)
        {
            for (int i = 0; i < ElementToConjuction.Length; i++)
            {
                if (ElementToConjuction[i].Equals(conj, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// check to see if contains conjuction string.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>
        /// 	<c>true</c> if the specified description has conjuction; otherwise, <c>false</c>.
        /// </returns>
        private static bool HasConjuction(string description)
        {
            for (int i = 0; i < ElementConjuction.Length; i++)
            {
                _conjuctionIndex = description.ToLower().IndexOf(ElementConjuction[i]);
                if (_conjuctionIndex > -1)
                {
                    _conjuctionText = ElementConjuction[i];
                    if (IsWord(description, _conjuctionIndex, _conjuctionText))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// process road conjuction string.
        /// </summary>
        /// <param name="description">The description.</param>
        private static void ProcessConjuction(string description)
        {
            string remainDescription = description;
            if (!StartWithConjuction(description))
            {
                remainDescription = "At " + description;
            }
            //From road
            string extenstion = "";
            if (StartWithConjuction(remainDescription))
            {
                bool isToRoad = IsToConjuction(_conjuctionText);
                if (!isToRoad)
                {
                    _resultDirectionCommandElements[MapDirectionCommandElement.FromRoadConjunction]
                            = new MapDirectionCommandElement
                            (MapDirectionCommandElement.ElementConjuction,
                            _conjuctionText);

                }
                else
                {
                    _resultDirectionCommandElements[MapDirectionCommandElement.ToRoadConjunction]
                            = new MapDirectionCommandElement
                            (MapDirectionCommandElement.ElementConjuction,
                            _conjuctionText);
                }
                remainDescription = remainDescription.Substring(_conjuctionIndex
                        + _conjuctionText.Length);
                if (HasConjuction(remainDescription))
                {//has to road
                    string roadName = remainDescription.Substring(0, _conjuctionIndex);
                    ProcessRoadName(roadName, !isToRoad);
                    isToRoad = !isToRoad;
                    if (isToRoad)
                    {
                        _resultDirectionCommandElements[MapDirectionCommandElement.ToRoadConjunction]
                                = new MapDirectionCommandElement
                                (MapDirectionCommandElement.ElementConjuction,
                                _conjuctionText);
                    }
                    else
                    {
                        _resultDirectionCommandElements[MapDirectionCommandElement.FromRoadConjunction]
                                = new MapDirectionCommandElement
                                (MapDirectionCommandElement.ElementConjuction,
                                _conjuctionText);
                    }
                    remainDescription = remainDescription.Substring(_conjuctionIndex
                            + _conjuctionText.Length);
                    extenstion = ProcessRoadName(remainDescription, !isToRoad);
                }
                else
                {
                    extenstion = ProcessRoadName(remainDescription, !isToRoad);
                }
            }
            ProcessExtenstion(extenstion);

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// process road name
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="from">if set to <c>true</c> [from].</param>
        /// <returns></returns>
        private static string ProcessRoadName(string description, bool from)
        {
            int tollTypeIndex = -1;
            string remainText = null;
            string roadName;
            int tollIndex = -1;
            for (int i = 0; i < ElementToll.Length; i++)
            {
                tollTypeIndex = description.ToLower().IndexOf(ElementToll[i]);
                if (tollTypeIndex > -1)
                {
                    tollIndex = i;
                    break;
                }
            }
            int finishIndex = -1;
            for (int i = 0; i < ElementFinish.Length; i++)
            {
                int extIndex = description.ToLower().IndexOf(ElementFinish[i]);
                if (finishIndex == -1 && extIndex > -1) finishIndex = extIndex;
                if (extIndex > -1 && finishIndex > extIndex)
                {
                    finishIndex = extIndex;
                }
            }
            if (tollTypeIndex > -1)
            {
                roadName = description.Substring(0, tollTypeIndex).Trim();
            }
            else if (finishIndex > -1)
            {

                roadName = description.Substring(0, finishIndex).Trim();
            }
            else
            {
                roadName = description.Trim();
            }
            if (finishIndex > -1)
            {
                remainText = description.Substring(finishIndex).Trim();
            }
            if (from)
            {
                _resultDirectionCommandElements[MapDirectionCommandElement.FromRoadName] =
                        new MapDirectionCommandElement(MapDirectionCommandElement
                        .ElementRoadName, roadName);
                if (tollTypeIndex > -1)
                {
                    _resultDirectionCommandElements[MapDirectionCommandElement.FromRoadName].RoadProperty
                            = ElementToll[tollIndex];
                }
                else
                {
                    _resultDirectionCommandElements[MapDirectionCommandElement.FromRoadName].RoadProperty = "";
                }

            }
            else
            {
                _resultDirectionCommandElements[MapDirectionCommandElement.ToRoadName] =
                        new MapDirectionCommandElement(MapDirectionCommandElement
                        .ElementRoadName, roadName);
                if (tollTypeIndex > -1)
                {
                    _resultDirectionCommandElements[MapDirectionCommandElement.ToRoadName].RoadProperty
                            = ElementToll[tollIndex];
                }
                else
                {
                    _resultDirectionCommandElements[MapDirectionCommandElement.ToRoadName].RoadProperty = "";
                }

            }
            return remainText;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Process extentions (entering or go through xxx roundabout).
        /// </summary>
        /// <param name="extension">The extension.</param>
        private static void ProcessExtenstion(string extension)
        {
            if (string.IsNullOrEmpty(extension)) return;
            extension = extension.Trim();
            int enteringIndex = extension.ToLower().IndexOf(ElementFinish[0]);
            int goThroughIndex = extension.ToLower().IndexOf(ElementFinish[1]);
            //start with entering.
            if (extension.ToLower().StartsWith(ElementFinish[0]))
            {
                if (goThroughIndex > 0)
                {
                    _resultDirectionCommandElements[MapDirectionCommandElement.ExtensionEntering] =
                        new MapDirectionCommandElement(MapDirectionCommandElement
                        .ElementRoadName, extension.Substring(0, goThroughIndex));
                    _resultDirectionCommandElements[MapDirectionCommandElement.ExtensionGoThroughRoundabout] =
                        new MapDirectionCommandElement(MapDirectionCommandElement
                        .ElementRoadName, extension.Substring(goThroughIndex));
                }
                else
                {
                    _resultDirectionCommandElements[MapDirectionCommandElement.ExtensionEntering] =
                        new MapDirectionCommandElement(MapDirectionCommandElement
                        .ElementRoadName, extension);
                }
            }
            else if (extension.ToLower().StartsWith(ElementFinish[1]))
            {
                if (enteringIndex > 0)
                {
                    _resultDirectionCommandElements[MapDirectionCommandElement.ExtensionGoThroughRoundabout] =
                        new MapDirectionCommandElement(MapDirectionCommandElement
                        .ElementRoadName, extension.Substring(0, enteringIndex));
                    _resultDirectionCommandElements[MapDirectionCommandElement.ExtensionEntering] =
                        new MapDirectionCommandElement(MapDirectionCommandElement
                        .ElementRoadName, extension.Substring(enteringIndex));
                }
                else
                {
                    _resultDirectionCommandElements[MapDirectionCommandElement.ExtensionGoThroughRoundabout] =
                        new MapDirectionCommandElement(MapDirectionCommandElement
                        .ElementRoadName, extension);
                }
            }

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
