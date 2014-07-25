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

//--------------------------------- PACKAGE ------------------------------------

namespace Mapdigit.Drawing.Core
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 24SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Defines an object used to draw lines and curves.
    /// </summary>
    internal sealed class PenFP
    {
        /**
         * Specifies a butt line cap.
         */
        public const int LinecapButt = 1;

        /**
         * Specifies a round line cap.
         */
        public const int LinecapRound = 2;

        /**
         * Specifies a square line cap.
         */
        public const int LinecapSquare = 3;

        /**
         * Specifies a mitered join. This produces a sharp corner or a clipped
         * corner, depending on whether the length of the miter exceeds the miter
         * limit.
         */
        public const int LinejoinMiter = 1;

        /**
         * Specifies a circular join. This produces a smooth, circular arc
         * between the lines.
         */
        public const int LinejoinRound = 2;

        /**
         * Specifies a beveled join. This produces a diagonal corner.
         */
        public const int LinejoinBevel = 3; //public int Color;


        /**
         * the stroke width of the pen.
         */
        public int Width;

        /**
         * the line join for this pen.
         */
        public int LineJoin;

        /**
         * the brush
         */
        public BrushFP Brush;

        /**
         * cap style used at the beginning of lines drawn with this Pen.
         */
        public int StartCap;

        /**
         * cap style used at the edning of lines drawn with this Pen.
         */
        public int EndCap;

        /**
         * the dash Array ,and if dash array is not null,
         *  then startCap = PenFP.LinecapButt;
         *  endCap = PenFP.LinecapButt;
         *  and  lineJoin = PenFP.LinejoinBevel;
         */
        public int[] DashArray;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="PenFP"/> class.
        /// </summary>
        /// <param name="color">the color of this pen.</param>
        public PenFP(int color)
            : this(color, SingleFP.One)
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="PenFP"/> class.
        /// </summary>
        /// <param name="color">the color of this pen</param>
        /// <param name="ffWidth">the width of this pen.</param>
        public PenFP(int color, int ffWidth)
            : this(color, ffWidth, LinecapButt, LinecapButt, LinejoinMiter)
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="PenFP"/> class.
        /// </summary>
        /// <param name="color">the color of the pen</param>
        /// <param name="ffWidth">the width of the pen</param>
        /// <param name="linecap">the cap style of this pen</param>
        /// <param name="linejoin">the join style of this pen</param>
        public PenFP(int color, int ffWidth, int linecap, int linejoin)
            : this(color, ffWidth, linecap, linecap, linejoin)
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="PenFP"/> class.
        /// </summary>
        /// <param name="brush">the brush.</param>
        /// <param name="ffWidth">the width of the pen</param>
        /// <param name="linecap">the cap style of this pen</param>
        /// <param name="linejoin">the join style of this pen</param>
        public PenFP(BrushFP brush, int ffWidth, int linecap, int linejoin)
            : this(brush, ffWidth, linecap, linecap, linejoin)
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="PenFP"/> class.
        /// </summary>
        /// <param name="color">the color of the pen</param>
        /// <param name="ffWidth">the width of the pen</param>
        /// <param name="startlinecap">the start cap style of this pen</param>
        /// <param name="endlinecap">the end cap style of this pen</param>
        /// <param name="linejoin">the join style of this pen</param>
        public PenFP(int color, int ffWidth, int startlinecap, int endlinecap,
                     int linejoin)
            : this(new SolidBrushFP(color), ffWidth, startlinecap, endlinecap,
                   linejoin)
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="PenFP"/> class.
        /// </summary>
        /// <param name="brush">the brush of the pen</param>
        /// <param name="ffWidth">the width of the pen</param>
        /// <param name="startlinecap">the start cap style of this pen</param>
        /// <param name="endlinecap">the end cap style of this pen</param>
        /// <param name="linejoin">the join style of this pen</param>
        public PenFP(BrushFP brush, int ffWidth, int startlinecap,
                     int endlinecap, int linejoin)
        {
            Brush = brush;
            Width = ffWidth;
            StartCap = startlinecap;
            EndCap = endlinecap;
            LineJoin = linejoin;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set the dash array for this pen.
        /// </summary>
        /// <param name="dashArrays">The dash arrays.</param>
        /// <param name="offset">The offset.</param>
        public void SetDashArray(int[] dashArrays, int offset)
        {
            int len = dashArrays.Length - offset;
            DashArray = null;
            if (len > 1)
            {
                DashArray = new int[len];
                Array.Copy(dashArrays, offset, DashArray, 0, len);
            }
        }
    }
}