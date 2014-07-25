//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 24SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System;
using System.Threading;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Util
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 24SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Pluggable logging framework that allows a developer to log into storage
    /// using the file connector API. It is highly recommended to use this
    /// class coupled with Netbeans preprocessing tags to reduce its overhead
    /// completely in runtime.
    /// </summary>
    public class Log
    {

        ///<summary>
        /// Constant indicating the logging level Debug is the default and the
        ///lowest level followed by info, warning and error
        ///</summary>
        public const int Debug = 1;

        /// <summary>
        /// Constant indicating the logging level Debug is the default and the
        /// lowest level followed by info, warning and error
        /// </summary>
        public const int Info = 2;

        ///<summary>
        /// Constant indicating the logging level Debug is the default and the
        ///lowest level followed by info, warning and error
        ///</summary>
        public const int Warning = 3;

        ///<summary>
        /// Constant indicating the logging level Debug is the default and the
        ///lowest level followed by info, warning and error
        ///</summary>
        public const int Error = 4;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Installs a log subclass that can replace the logging destination/behavior
        /// </summary>
        /// <param name="newInstance">The new instance.</param>
        public static void Install(Log newInstance)
        {
            _instance = newInstance;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Default println method invokes the print instance method, uses DEBUG level
        /// </summary>
        /// <param name="text"> text to print</param>
        public static void P(string text)
        {
            P(text, Debug);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///Default println method invokes the print instance method, uses given level
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="level">The level,one of DEBUG, INFO, WARNING, ERROR</param>
        public static void P(string text, int level)
        {
            _instance.Print(text, level);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the logging level for printing log details, the lower the value
        /// the more verbose would the printouts be
        /// </summary>
        /// <param name="level">The level,one of DEBUG, INFO, WARNING, ERROR</param>
        public static void SetLevel(int level)
        {
            _instance.level = level;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the logging level for printing log details, the lower the value
        /// the more verbose would the printouts be
        /// </summary>
        /// <returns>one of DEBUG, INFO, WARNING, ERROR</returns>
        public static int GetLevel()
        {
            return _instance.level;
        }


        /// <summary>
        /// the level of logging.
        /// </summary>
        /// 
        protected int level = Debug;
        private static Log _instance = new Log();
        private readonly DateTime _zeroTime = DateTime.Now;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Prints the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="debugLevel">The level.</param>
        protected void Print(string text, int debugLevel)
        {
            if (level > debugLevel)
            {
                return;
            }
            text = GetThreadAndTimeStamp() + " - " + text;
            Console.WriteLine(text);


        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a simple string containing a timestamp and thread name..
        /// </summary>
        /// <returns></returns>
        protected string GetThreadAndTimeStamp()
        {
            TimeSpan time = DateTime.Now - _zeroTime;
            return "[" + Thread.CurrentThread.Name + "] " + time.Hours + ":"
                    + time.Minutes + ":" + time.Seconds + "," + time.Milliseconds;
        }

    }

}
