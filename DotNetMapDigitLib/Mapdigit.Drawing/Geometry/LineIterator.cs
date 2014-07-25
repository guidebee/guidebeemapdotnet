//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 25SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System;

//--------------------------------- PACKAGE ------------------------------------

namespace Mapdigit.Drawing.Geometry
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 25SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// A utility class to iterate over the path segments of a line segment
    /// through the IPathIterator interface.
    /// </summary>
    internal class LineIterator : PathIterator
    {
        private readonly Line _line;
        private readonly AffineTransform _affine;
        private int _index;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="LineIterator"/> class.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <param name="at">At.</param>
        internal LineIterator(Line l, AffineTransform at)
        {
            _line = l;
            _affine = at;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Returns the winding rule for determining the interior of the path.
        /// </summary>
        /// <returns>the winding rule</returns>
        public override int WindingRule
        {
            get { return WindNonZero; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tests if the iteration is complete.
        /// </summary>
        /// <returns>
        /// 	true if all the segments have
        /// been read; false otherwise.
        /// </returns>
        public override bool IsDone()
        {
            return (_index > 1);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Moves the iterator to the next segment of the path forwards
        /// along the primary direction of traversal as long as there are
        /// more points in that direction.
        /// </summary>
        public override void Next()
        {
            _index++;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the coordinates and type of the current path segment in
        /// the iteration.
        /// The return value is the path-segment type:
        /// SegMoveto, SegLineto, SegQuadto, SegCubicto, or SegClose.
        /// A long array of length 6 must be passed in and can be used to
        /// store the coordinates of the point(s).
        /// Each point is stored as a pair of long x,y coordinates.
        /// SegMoveto and SegLineto types returns one point,
        /// SegQuadto returns two points,
        /// SegCubicto returns 3 points
        /// and SegClose does not return any points.
        /// </summary>
        /// <param name="coords">an array that holds the data returned from
        /// this method</param>
        /// <returns>
        /// the path-segment type of the current path segment
        /// </returns>
        public override int CurrentSegment(int[] coords)
        {
            if (IsDone())
            {
                throw new IndexOutOfRangeException("line iterator out of bounds");
            }
            int type;
            if (_index == 0)
            {
                coords[0] = _line.X1;
                coords[1] = _line.Y1;
                type = SegMoveto;
            }
            else
            {
                coords[0] = _line.X2;
                coords[1] = _line.Y2;
                type = SegLineto;
            }
            if (_affine != null)
            {
                _affine.Transform(coords, 0, coords, 0, 1);
            }
            return type;
        }
    }
}