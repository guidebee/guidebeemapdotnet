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
    /// define all voice command type. some command type is duplicated (as defined
    /// in MapDirectionCommandType).
    /// </summary>
    public abstract class VoiceCommandType
    {

        /// <summary>
        /// away from target.
        /// </summary>
        public const int AwayfromTarget = 8021;

        /// <summary>
        /// Bear Left
        /// </summary>
        public const int BearLeft = MapDirectionCommandType.CommandBearLeft;

        /// <summary>
        /// Bear Right
        /// </summary>
        public const int BearRight = MapDirectionCommandType.CommandBearRight;

        /// <summary>
        /// change the motoway.
        /// </summary>
        public const int ChangeMotoway = MapDirectionCommandType.CommandMerge;

        /// <summary>
        /// closing target.
        /// </summary>
        public const int ClosingTarget = 8020;

        /// <summary>
        /// reached destiation.
        /// </summary>
        public const int Destination = MapDirectionCommandType.CommandReachDestination;

        /// <summary>
        /// destination on the left
        /// </summary>
        public const int DestinationOnTheLeft = MapDirectionCommandType
            .CommandDestinationOnTheLeft;

        /// <summary>
        /// destination on the right
        /// </summary>
        public const int DestinationOnTheRight = MapDirectionCommandType
            .CommandDestinationOnTheRight;

        /// <summary>
        /// Distance , 2 km or 2 mile
        /// </summary>
        public const int Distance002K = 2000;

        /// <summary>
        /// Distance , 3 km or 3 mile
        /// </summary>
        public const int Distance003K = 3000;

        /// <summary>
        /// Distance , 4 km or 4 mile
        /// </summary>
        public const int Distance004K = 4000;

        /// <summary>
        /// Distance , 5 km or 5 mile
        /// </summary>
        public const int Distance005K = 5000;

        /// <summary>
        /// Distance , 6 km or 6 mile
        /// </summary>
        public const int Distance006K = 6000;

        /// <summary>
        /// Distance , 7 km or 7 mile
        /// </summary>
        public const int Distance007K = 7000;

        /// <summary>
        /// Distance , 8 km or 8 mile
        /// </summary>
        public const int Distance008K = 8000;

        /// <summary>
        /// Distance , 9 km or 9 mile
        /// </summary>
        public const int Distance009K = 9000;

        /// <summary>
        ///  Distance , 10 km or 10 mile
        /// </summary>
        public const int Distance010K = 10000;

        /// <summary>
        /// Distance , 15 km or 15 mile
        /// </summary>
        public const int Distance015K = 15000;

        /// <summary>
        /// Distance , 20 km or 20 mile
        /// </summary>
        public const int Distance020K = 20000;

        /// <summary>
        /// Distance , 25 km or 25 mile
        /// </summary>
        public const int Distance025K = 25000;

        /// <summary>
        /// Distance , 30 km or 30 mile
        /// </summary>
        public const int Distance030K = 30000;

        /// <summary>
        /// Distance , 35 km or 35 mile
        /// </summary>
        public const int Distance035K = 35000;

        /// <summary>
        ///  Distance , 40 km or 40 mile
        /// </summary>
        public const int Distance040K = 40000;

        /// <summary>
        /// Distance , 45 km or 45 mile
        /// </summary>
        public const int Distance045K = 45000;
        /// <summary>
        /// 50 meters or 50 yards.
        /// </summary>
        public const int Distance050 = 50;

        /// <summary>
        /// Distance , 50 km or 50 mile
        /// </summary>
        public const int Distance050K = 50000;

        /// <summary>
        /// Distance , 55 km or 55 mile
        /// </summary>
        public const int Distance055K = 55000;

        /// <summary>
        /// Distance , 60 km or 60 mile
        /// </summary>
        public const int Distance060K = 60000;

        /// <summary>
        /// Distance , 65 km or 65 mile
        /// </summary>
        public const int Distance065K = 65000;

        /// <summary>
        /// Distance , 70 km or 70 mile
        /// </summary>
        public const int Distance070K = 70000;

        /// <summary>
        /// Distance , 75 km or 75 mile
        /// </summary>
        public const int Distance075K = 75000;

        /// <summary>
        /// Distance , 80 km or 80 mile
        /// </summary>
        public const int Distance080K = 80000;

        /// <summary>
        ///  Distance , 85 km or 85 mile
        /// </summary>
        public const int Distance085K = 85000;

        /// <summary>
        /// Distance , 90 km or 90 mile
        /// </summary>
        public const int Distance090K = 90000;

        /// <summary>
        ///  Distance , 95 km or 95 mile
        /// </summary>
        public const int Distance095K = 95000;

        /// <summary>
        /// 100 meters or 100 yards.
        /// </summary>
        public const int Distance100 = 100;

        /// <summary>
        /// 1000 meters or 1000 yards.
        /// </summary>
        public const int Distance1000 = 1000;

        /// <summary>
        /// Distance , 100 km or 100 mile
        /// </summary>
        public const int Distance100K = 100000;

        /// <summary>
        /// 1100 meters or 1100 yards.
        /// </summary>
        public const int Distance1100 = 1100;

        /// <summary>
        /// 1200 meters or 1200 yards.
        /// </summary>
        public const int Distance1200 = 1200;

        /// <summary>
        /// 1300 meters or 1300 yards.
        /// </summary>
        public const int Distance1300 = 1300;

        /// <summary>
        /// 1400 meters or 1400 yards.
        /// </summary>
        public const int Distance1400 = 1400;

        /// <summary>
        /// 150 meters or 150 yards.
        /// </summary>
        public const int Distance150 = 150;

        /// <summary>
        /// 1500 meters or 1500 yards.
        /// </summary>
        public const int Distance1500 = 1500;

        /// <summary>
        /// 1600 meters or 1600 yards.
        /// </summary>
        public const int Distance1600 = 1600;

        /// <summary>
        /// 1700 meters or 1700 yards.
        /// </summary>
        public const int Distance1700 = 1700;

        /// <summary>
        /// 1800 meters or 1800 yards.
        /// </summary>
        public const int Distance1800 = 1800;

        /// <summary>
        /// 1900 meters or 1900 yards.
        /// </summary>
        public const int Distance1900 = 1900;

        /// <summary>
        /// 200 meters or 200 yards.
        /// </summary>
        public const int Distance200 = 200;

        /// <summary>
        /// 250 meters or 250 yards.
        /// </summary>
        public const int Distance250 = 250;

        /// <summary>
        /// 300 meters or 300 yards.
        /// </summary>
        public const int Distance300 = 300;

        /// <summary>
        /// 400 meters or 400 yards.
        /// </summary>
        public const int Distance400 = 400;

        /// <summary>
        /// 500 meters or 500 yards.
        /// </summary>
        public const int Distance500 = 500;

        /// <summary>
        /// 600 meters or 600 yards.
        /// </summary>
        public const int Distance600 = 600;

        /// <summary>
        /// 700 meters or 700 yards.
        /// </summary>
        public const int Distance700 = 700;

        /// <summary>
        /// 800 meters or 800 yards
        /// </summary>
        public const int Distance800 = 800;

        /// <summary>
        /// 900 meters or 900 yards.
        /// </summary>
        public const int Distance900 = 900;

        internal const int DistanceUnitKilometer = 0x00000000;
        /**
         * Distance unit is mile/yard ,the highest bit is 1.
         */
        internal const uint DistanceUnitMile = 0x80000000;

        /// <summary>
        ///  enter the motoway.
        /// </summary>
        public const int EnterMotoway = MapDirectionCommandType.CommandEnterHighway;

        /// <summary>
        ///  enter the motoway on the left
        /// </summary>
        public const int EnterMotowayLeft = MapDirectionCommandType.CommandEnterHighwayLeft;

        /// <summary>
        /// enter the motoway on the right
        /// </summary>
        public const int EnterMotowayRight = MapDirectionCommandType.CommandEnterHighwayRight;

        /// <summary>
        /// exit the motoway.
        /// </summary>
        public const int ExitMotoway = MapDirectionCommandType.CommandLeaveHighway;

        /// <summary>
        /// exit the motoway
        /// </summary>
        public const int ExitMotowayLeft = MapDirectionCommandType.CommandLeaveHighwayLeft;

        /// <summary>
        /// exit the motoway.
        /// </summary>
        public const int ExitMotowayRight = MapDirectionCommandType.CommandLeaveHighwayRight;

        /// <summary>
        /// follow the moto way.
        /// </summary>
        public const int FollowMotoway = MapDirectionCommandType.CommandNoTurn;

        /// <summary>
        /// Go straight.
        /// </summary>
        public const int GoStraight = MapDirectionCommandType.CommandNoTurn;

        /// <summary>
        ///  head east.
        /// </summary>
        public const int HeadEast = MapDirectionCommandType.CommandHeadDirectionE;

        /// <summary>
        /// head north.
        /// </summary>
        public const int HeadNorth = MapDirectionCommandType.CommandHeadDirectionN;

        /// <summary>
        /// head north east.
        /// </summary>
        public const int HeadNortheast = MapDirectionCommandType.CommandHeadDirectionNe;

        /// <summary>
        /// head north west.
        /// </summary>
        public const int HeadNorthwest = MapDirectionCommandType.CommandHeadDirectionNw;

        /// <summary>
        /// head south.
        /// </summary>
        public const int HeadSouth = MapDirectionCommandType.CommandHeadDirectionS;

        /// <summary>
        /// head south east.
        /// </summary>
        public const int HeadSoutheast = MapDirectionCommandType.CommandHeadDirectionSe;

        /// <summary>
        ///  head south west.
        /// </summary>
        public const int HeadSouthwest = MapDirectionCommandType.CommandHeadDirectionSw;

        /// <summary>
        /// head west.
        /// </summary>
        public const int HeadWest = MapDirectionCommandType.CommandHeadDirectionW;

        /// <summary>
        /// in the middle lane.
        /// </summary>
        public const int InMiddleMotoway = MapDirectionCommandType.CommandNoTurn;

        /// <summary>
        /// Keep left.
        /// </summary>
        public const int KeepLeft = MapDirectionCommandType.CommandKeepLeft;

        /// <summary>
        /// Keep right.
        /// </summary>
        public const int KeepRight = MapDirectionCommandType.CommandKeepRight;

        /// <summary>
        /// Make U turn.
        /// </summary>
        public const int MakeUTurn = MapDirectionCommandType.CommandUTurn;

        /// <summary>
        /// None command.
        /// </summary>
        public const int None = -1;

        /// <summary>
        /// Reached target.
        /// </summary>
        public const int ReachedTarget = 8022;

        /// <summary>
        /// take the first exit.
        /// </summary>
        public const int RoundaboutTake1Exit = MapDirectionCommandType.CommandRoundabout1Exit;

        /// <summary>
        /// take the second exit.
        /// </summary>
        public const int RoundaboutTake2Exit = MapDirectionCommandType.CommandRoundabout2Exit;

        /// <summary>
        /// take the third exit.
        /// </summary>
        public const int RoundaboutTake3Exit = MapDirectionCommandType.CommandRoundabout3Exit;

        /// <summary>
        ///  take the 4th exit.
        /// </summary>
        public const int RoundaboutTake4Exit = MapDirectionCommandType.CommandRoundabout4Exit;

        /// <summary>
        /// take the 5th exit.
        /// </summary>
        public const int RoundaboutTake5Exit = MapDirectionCommandType.CommandRoundabout5Exit;

        /// <summary>
        ///  take the 6th exit.
        /// </summary>
        public const int RoundaboutTake6Exit = MapDirectionCommandType.CommandRoundabout6Exit;

        /// <summary>
        ///  take the 7th exit.
        /// </summary>
        public const int RoundaboutTake7Exit = MapDirectionCommandType.CommandRoundabout7Exit;

        /// <summary>
        /// take the 8th exit.
        /// </summary>
        public const int RoundaboutTake8Exit = MapDirectionCommandType.CommandRoundabout8Exit;

        /// <summary>
        /// take the 9th exit.
        /// </summary>
        public const int RoundaboutTake9Exit = MapDirectionCommandType.CommandRoundabout9Exit;

        /// <summary>
        ///  take the exit.
        /// </summary>
        public const int RoundaboutTakeExit = MapDirectionCommandType.CommandRoundabout1Exit;

        /// <summary>
        /// turn sharp left
        /// </summary>
        public const int SharpLeft = MapDirectionCommandType.CommandSharpLeft;

        /// <summary>
        /// turn sharp right.
        /// </summary>
        public const int SharpRight = MapDirectionCommandType.CommandSharpRight;

        /// <summary>
        ///  take 1st left
        /// </summary>
        public const int Take1Left = MapDirectionCommandType.CommandTake1Left;

        /// <summary>
        /// take 2nd left
        /// </summary>
        public const int Take2Left = MapDirectionCommandType.CommandTake2Left;

        /// <summary>
        /// take 3rd left
        /// </summary>
        public const int Take3Left = MapDirectionCommandType.CommandTake3Left;

        /// <summary>
        /// take 4th left
        /// </summary>
        public const int Take4Left = MapDirectionCommandType.CommandTake4Left;

        /// <summary>
        /// take 5th left
        /// </summary>
        public const int Take5Left = MapDirectionCommandType.CommandTake5Left;

        /// <summary>
        /// take 6th left
        /// </summary>
        public const int Take6Left = MapDirectionCommandType.CommandTake6Left;

        /// <summary>
        /// take 7th left
        /// </summary>
        public const int Take7Left = MapDirectionCommandType.CommandTake7Left;

        /// <summary>
        /// take 1st right
        /// </summary>
        public const int Take1Right = MapDirectionCommandType.CommandTake1Right;

        /// <summary>
        ///  take 2nd right
        /// </summary>
        public const int Take2Right = MapDirectionCommandType.CommandTake2Right;

        /// <summary>
        /// take 3rd right
        /// </summary>
        public const int Take3Right = MapDirectionCommandType.CommandTake3Right;

        /// <summary>
        /// take 4th right
        /// </summary>
        public const int Take4Right = MapDirectionCommandType.CommandTake4Right;

        /// <summary>
        /// take 5th right
        /// </summary>
        public const int Take5Right = MapDirectionCommandType.CommandTake5Right;

        /// <summary>
        /// take 6th right
        /// </summary>
        public const int Take6Right = MapDirectionCommandType.CommandTake6Right;

        /// <summary>
        /// take 7th right
        /// </summary>
        public const int Take7Right = MapDirectionCommandType.CommandTake7Right;

        /// <summary>
        /// take 8th left
        /// </summary>
        public const int Take8Left = MapDirectionCommandType.CommandTake8Left;

        /// <summary>
        /// take 8th right
        /// </summary>
        public const int Take8Right = MapDirectionCommandType.CommandTake8Right;

        /// <summary>
        ///  take 9th left
        /// </summary>
        public const int Take9Left = MapDirectionCommandType.CommandTake9Left;

        /// <summary>
        /// take 9th right
        /// </summary>
        public const int Take9Right = MapDirectionCommandType.CommandTake9Right;



        //Off road navigation voice command type.
        
        ///<summary>
        /// Target at 1 o'clock direction,
        ///</summary>
        public const int TargetAt01Oclock = 8001;

        /// <summary>
        /// Target at 2 o'clock direction,
        /// </summary>
        public const int TargetAt02Oclock = 8002;

        /// <summary>
        /// Target at 3 o'clock direction,
        /// </summary>
        public const int TargetAt03Oclock = 8003;

        /// <summary>
        /// Target at 4 o'clock direction,
        /// </summary>
        public const int TargetAt04Oclock = 8004;

        /// <summary>
        /// Target at 5 o'clock direction,
        /// </summary>
        public const int TargetAt05Oclock = 8005;

        /// <summary>
        ///  Target at 6 o'clock direction,
        /// </summary>
        public const int TargetAt06Oclock = 8006;

        /// <summary>
        /// Target at 7 o'clock direction,
        /// </summary>
        public const int TargetAt07Oclock = 8007;

        /// <summary>
        /// Target at 8 o'clock direction,
        /// </summary>
        public const int TargetAt08Oclock = 8008;

        /// <summary>
        /// Target at 9 o'clock direction,
        /// </summary>
        public const int TargetAt09Oclock = 8009;

        /// <summary>
        /// Target at 10 o'clock direction
        /// </summary>
        public const int TargetAt10Oclock = 8010;

        /// <summary>
        ///  Target at 11 o'clock direction,
        /// </summary>
        public const int TargetAt11Oclock = 8011;

        /// <summary>
        /// Target at 12 o'clock direction,
        /// </summary>
        public const int TargetAt12Oclock = 8012;

        /// <summary>
        ///  turn around
        /// </summary>
        public const int TurnAround = 9999;

        /// <summary>
        /// turn left.
        /// </summary>
        public const int TurnLeft = MapDirectionCommandType.CommandTurnLeft;

        /// <summary>
        /// turn right.
        /// </summary>
        public const int TurnRight = MapDirectionCommandType.CommandTurnRight;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// check to see if it the distance command uses kilometer/meters.
        /// </summary>
        /// <param name="commandType">the command typ</param>
        /// <returns>
        /// 	uses kilometer/meter otherwise uses mile/yard
        /// </returns>
        public static bool IsKilometer(int commandType)
        {
            if (IsDistanceCommand(commandType))
            {
                return !((commandType & DistanceUnitMile) == DistanceUnitMile);
            }
            return false;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// check to see if it a disntace command.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// 	ture its a distance command.
        /// </returns>
        public static bool IsDistanceCommand(int type)
        {
            int newType = type & 0x7FFFFFF;
            switch (newType)
            {
                case Distance050:
                case Distance100:
                case Distance150:
                case Distance200:
                case Distance250:
                case Distance300:
                case Distance400:
                case Distance500:
                case Distance600:
                case Distance700:
                case Distance800:
                case Distance900:
                case Distance1000:
                case Distance1100:
                case Distance1200:
                case Distance1300:
                case Distance1400:
                case Distance1500:
                case Distance1600:
                case Distance1700:
                case Distance1800:
                case Distance1900:
                case Distance002K:
                case Distance003K:
                case Distance004K:
                case Distance005K:
                case Distance006K:
                case Distance007K:
                case Distance008K:
                case Distance009K:
                case Distance010K:
                case Distance015K:
                case Distance020K:
                case Distance025K:
                case Distance030K:
                case Distance035K:
                case Distance040K:
                case Distance045K:
                case Distance050K:
                case Distance055K:
                case Distance060K:
                case Distance065K:
                case Distance070K:
                case Distance075K:
                case Distance080K:
                case Distance085K:
                case Distance090K:
                case Distance095K:
                case Distance100K:
                    return true;
            }
            return false;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 29SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the string format
        /// </summary>
        /// <param name="type">voice command type</param>
        /// <returns>it's string format</returns>
        public static string GetStringFormat(int type)
        {
            string retStr = "";
            int distanceType = 0x7FFFFFFF & type;
            switch (distanceType)
            {
                case GoStraight:
                    retStr = "GO_STRAIGHT";
                    break;
                case BearLeft:
                    retStr = "BEAR_LEFT";
                    break;
                case TurnLeft:
                    retStr = "TURN_LEFT";
                    break;
                case SharpLeft:
                    retStr = "SharpLeft";
                    break;
                case MakeUTurn:
                    retStr = "MAKE_U_TURN";
                    break;
                case SharpRight:
                    retStr = "SHARP_RIGHT";
                    break;
                case TurnRight:
                    retStr = "TURN_RIGHT";
                    break;
                case BearRight:
                    retStr = "BEAR_RIGHT";
                    break;
                case KeepLeft:
                    retStr = "KEEP_LEFT";
                    break;
                case KeepRight:
                    retStr = "KEEP_RIGHT";
                    break;
                case Destination:
                    retStr = "DESTATION";
                    break;
                case RoundaboutTake1Exit:
                    retStr = "ROUNDABOUT_TAKE_1_EXIT";
                    break;
                case RoundaboutTake2Exit:
                    retStr = "ROUNDABOUT_TAKE_2_EXIT";
                    break;
                case RoundaboutTake3Exit:
                    retStr = "ROUNDABOUT_TAKE_3_EXIT";
                    break;
                case RoundaboutTake4Exit:
                    retStr = "ROUNDABOUT_TAKE_4_EXIT";
                    break;
                case RoundaboutTake5Exit:
                    retStr = "ROUNDABOUT_TAKE_5_EXIT";
                    break;
                case RoundaboutTake6Exit:
                    retStr = "ROUNDABOUT_TAKE_6_EXIT";
                    break;
                case RoundaboutTake7Exit:
                    retStr = "ROUNDABOUT_TAKE_7_EXIT";
                    break;
                case RoundaboutTake8Exit:
                    retStr = "ROUNDABOUT_TAKE_8_EXIT";
                    break;
                case EnterMotoway:
                    retStr = "ENTER_MOTOWAY";
                    break;
                case EnterMotowayLeft:
                    retStr = "ENTER_MOTOWAY_LEFT";
                    break;
                case EnterMotowayRight:
                    retStr = "ENTER_HIGHWAY_RIGHT";
                    break;
                case ExitMotoway:
                    retStr = "EXIT_MOTOWAY";
                    break;
                case ExitMotowayLeft:
                    retStr = "EXIT_MOTOWAY_LEFT";
                    break;
                case ExitMotowayRight:
                    retStr = "EXIT_MOTOWAY_RIGHT";
                    break;
                case HeadNorth:
                    retStr = "HEAD_NORTH";
                    break;
                case HeadNortheast:
                    retStr = "HEAD_NORTHEAST";
                    break;
                case HeadEast:
                    retStr = "HEAD_EAST";
                    break;
                case HeadSoutheast:
                    retStr = "HEAD_SOUTHEAST";
                    break;
                case HeadSouth:
                    retStr = "HEAD_SOUTH";
                    break;
                case HeadSouthwest:
                    retStr = "HEAD_SOUTHWEST";
                    break;
                case HeadWest:
                    retStr = "HEAD_WEST";
                    break;
                case HeadNorthwest:
                    retStr = "HEAD_NORTHWEST";
                    break;
                case Distance050:
                    retStr = "50";
                    break;
                case Distance100:
                    retStr = "100";
                    break;
                case Distance150:
                    retStr = "150";
                    break;
                case Distance200:
                    retStr = "200";
                    break;
                case Distance250:
                    retStr = "250";
                    break;
                case Distance300:
                    retStr = "300";
                    break;
                case Distance400:
                    retStr = "400";
                    break;
                case Distance500:
                    retStr = "500";
                    break;
                case Distance600:
                    retStr = "600";
                    break;
                case Distance700:
                    retStr = "700";
                    break;
                case Distance800:
                    retStr = "800";
                    break;
                case Distance900:
                    retStr = "900";
                    break;
                case Distance1000:
                    retStr = "1000";
                    break;
                case Distance1100:
                    retStr = "1100";
                    break;
                case Distance1200:
                    retStr = "1200";
                    break;
                case Distance1300:
                    retStr = "1300";
                    break;
                case Distance1400:
                    retStr = "1400";
                    break;
                case Distance1500:
                    retStr = "1500";
                    break;
                case Distance1600:
                    retStr = "1600";
                    break;
                case Distance1700:
                    retStr = "1700";
                    break;
                case Distance1800:
                    retStr = "1800";
                    break;
                case Distance1900:
                    retStr = "1900";
                    break;
                case Distance002K:
                    retStr = "2K";
                    break;
                case Distance003K:
                    retStr = "3K";
                    break;
                case Distance004K:
                    retStr = "4K";
                    break;
                case Distance005K:
                    retStr = "5K";
                    break;
                case Distance006K:
                    retStr = "6K";
                    break;
                case Distance007K:
                    retStr = "7K";
                    break;
                case Distance008K:
                    retStr = "8K";
                    break;
                case Distance009K:
                    retStr = "9K";
                    break;
                case Distance010K:
                    retStr = "10K";
                    break;
                case Distance015K:
                    retStr = "15K";
                    break;
                case Distance020K:
                    retStr = "20K";
                    break;
                case Distance025K:
                    retStr = "25K";
                    break;
                case Distance030K:
                    retStr = "30K";
                    break;
                case Distance035K:
                    retStr = "30K";
                    break;
                case Distance040K:
                    retStr = "40K";
                    break;
                case Distance045K:
                    retStr = "45K";
                    break;
                case Distance050K:
                    retStr = "50K";
                    break;
                case Distance055K:
                    retStr = "55K";
                    break;
                case Distance060K:
                    retStr = "60K";
                    break;
                case Distance065K:
                    retStr = "65K";
                    break;
                case Distance070K:
                    retStr = "70K";
                    break;
                case Distance075K:
                    retStr = "75K";
                    break;
                case Distance080K:
                    retStr = "80K";
                    break;
                case Distance085K:
                    retStr = "85K";
                    break;
                case Distance090K:
                    retStr = "90K";
                    break;
                case Distance095K:
                    retStr = "95K";
                    break;
                case Distance100K:
                    retStr = "100K";
                    break;
                case Take1Left:
                    retStr = "TAKE_1_LEFT";
                    break;
                case Take2Left:
                    retStr = "TAKE_2_LEFT";
                    break;
                case Take3Left:
                    retStr = "TAKE_3_LEFT";
                    break;
                case Take4Left:
                    retStr = "TAKE_4_LEFT";
                    break;
                case Take5Left:
                    retStr = "TAKE_5_LEFT";
                    break;
                case Take6Left:
                    retStr = "TAKE_6_LEFT";
                    break;
                case Take7Left:
                    retStr = "TAKE_7_LEFT";
                    break;
                case Take8Left:
                    retStr = "TAKE_8_LEFT";
                    break;
                case Take9Left:
                    retStr = "TAKE_9_LEFT";
                    break;
                case Take1Right:
                    retStr = "TAKE_1_RIGHT";
                    break;
                case Take2Right:
                    retStr = "TAKE_2_RIGHT";
                    break;
                case Take3Right:
                    retStr = "TAKE_3_RIGHT";
                    break;
                case Take4Right:
                    retStr = "TAKE_4_RIGHT";
                    break;
                case Take5Right:
                    retStr = "TAKE_5_RIGHT";
                    break;
                case Take6Right:
                    retStr = "TAKE_6_RIGHT";
                    break;
                case Take7Right:
                    retStr = "TAKE_7_RIGHT";
                    break;
                case Take8Right:
                    retStr = "TAKE_8_RIGHT";
                    break;
                case Take9Right:
                    retStr = "TAKE_9_RIGHT";
                    break;
                case DestinationOnTheLeft:
                    retStr = "DESTINATION_ON_THE_LEFT";
                    break;
                case DestinationOnTheRight:
                    retStr = "DESTINATION_ON_THE_RIGHT";
                    break;
                case TargetAt01Oclock:
                    retStr = "TARGET_AT_01_OCLOCK";
                    break;
                case TargetAt02Oclock:
                    retStr = "TARGET_AT_02_OCLOCK";
                    break;
                case TargetAt03Oclock:
                    retStr = "TARGET_AT_03_OCLOCK";
                    break;
                case TargetAt04Oclock:
                    retStr = "TARGET_AT_04_OCLOCK";
                    break;
                case TargetAt05Oclock:
                    retStr = "TARGET_AT_05_OCLOCK";
                    break;
                case TargetAt06Oclock:
                    retStr = "TARGET_AT_06_OCLOCK";
                    break;
                case TargetAt07Oclock:
                    retStr = "TARGET_AT_07_OCLOCK";
                    break;
                case TargetAt08Oclock:
                    retStr = "TARGET_AT_08_OCLOCK";
                    break;
                case TargetAt09Oclock:
                    retStr = "TARGET_AT_09_OCLOCK";
                    break;
                case TargetAt10Oclock:
                    retStr = "TARGET_AT_10_OCLOCK";
                    break;
                case TargetAt11Oclock:
                    retStr = "TARGET_AT_11_OCLOCK";
                    break;
                case TargetAt12Oclock:
                    retStr = "TARGET_AT_12_OCLOCK";
                    break;
                case ClosingTarget:
                    retStr = "CLOSING_TARGET";
                    break;
                case AwayfromTarget:
                    retStr = "AWAYFROM_TARGET";
                    break;
                case ReachedTarget:
                    retStr = "REACHED_TARGET";
                    break;
                case TurnAround:
                    retStr = "TURN_AROUND";
                    break;
            }
            return retStr;
        }

    }
}