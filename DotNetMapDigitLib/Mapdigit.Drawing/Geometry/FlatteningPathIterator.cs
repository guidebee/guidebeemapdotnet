//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 14OCT2010  James Shen                 	          Code review
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
    // 14OCT2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// The FlatteningPathIterator class returns a flattened view of
    /// another IPathIterator object.
    /// </summary>
    /// <remarks>
    /// Other IShape classes can use this class to provide flattening behavior for 
    /// their paths without having to perform the interpolation calculations themselves.
    ///</remarks>
    public class FlatteningPathIterator : PathIterator
    {
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Constructs a new FlatteningPathIterator object that
        /// flattens a path as it iterates over it.  
        /// </summary>
        /// <remarks>
        /// The iterator does not
        /// subdivide any curve read from the source iterator to more than
        /// 10 levels of subdivision which yields a maximum of 1024 line
        /// segments per curve.
        /// </remarks>
        /// <param name="src"> the original unflattened path being iterated over</param>
        /// <param name="flatness">the maximum allowable distance between the
        /// control points and the flattened curve.</param>
        public FlatteningPathIterator(PathIterator src, int flatness)
            : this(src, flatness, 10)
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a new FlatteningPathIterator object
        /// that flattens a path as it iterates over it.
        /// </summary>
        /// <remarks>
        /// The limit parameter allows you to control the
        /// maximum number of recursive subdivisions that the iterator
        /// can make before it assumes that the curve is flat enough
        /// without measuring against the flatness parameter.
        /// The flattened iteration therefore never generates more than
        /// a maximum of (2^limit) line segments per curve.
        /// </remarks>
        /// <param name="src">the original unflattened path being iterated over</param>
        /// <param name="flatness">he maximum allowable distance between the
        ///  control points and the flattened curve</param>
        /// <param name="limit">the maximum number of recursive subdivisions
        /// allowed for any curved segment</param>
        public FlatteningPathIterator(PathIterator src, int flatness,
                                      int limit)
        {
            if (flatness < 0.0)
            {
                throw new ArgumentException("flatness must be >= 0");
            }
            if (limit < 0)
            {
                throw new ArgumentException("limit must be >= 0");
            }
            _src = src;
            _squareflat = flatness*flatness;
            _limit = limit;
            _levels = new int[limit + 1];
            // prime the first path segment
            Next(false);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the flatness of this iterator.
        /// </summary>
        public int Flatness
        {
            get { return (int) (Math.Sqrt(_squareflat) + .5); }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the recursion limit of this iterator.
        /// </summary>
        public int RecursionLimit
        {
            get { return _limit; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the winding rule for determining the interior of the path.
        /// </summary>
        public override int WindingRule
        {
            get { return _src.WindingRule; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
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
            return _done;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Moves the iterator to the next segment of the path forwards
        /// along the primary direction of traversal as long as there are
        /// more points in that direction.
        /// </summary>
        public override void Next()
        {
            Next(true);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the coordinates and type of the current path segment in
        /// the iteration.
        /// </summary>
        /// <remarks>
        /// The return value is the path-segment type:
        /// SegMoveto, SegLineto, SegQuadto, SegCubicto, or SegClose.
        /// A long array of length 6 must be passed in and can be used to
        /// store the coordinates of the point(s).
        /// Each point is stored as a pair of long x,y coordinates.
        /// SegMoveto and SegLineto types returns one point,
        /// SegQuadto returns two points,
        /// SegCubicto returns 3 points
        /// and SegClose does not return any points.
        /// </remarks>
        /// <param name="coords">an array that holds the data returned from
        /// this method</param>
        /// <returns>
        /// the path-segment type of the current path segment
        /// </returns>
        public override int CurrentSegment(int[] coords)
        {
            if (IsDone())
            {
                throw new IndexOutOfRangeException("flattening iterator out of bounds");
            }
            int type = _holdType;
            if (type != SegClose)
            {
                coords[0] = (int) (_hold[_holdIndex + 0] + .5);
                coords[1] = (int) (_hold[_holdIndex + 1] + .5);
                if (type != SegMoveto)
                {
                    type = SegLineto;
                }
            }
            return type;
        }

        private const int GrowSize = 24; // Multiple of cubic & quad curve size
        private readonly PathIterator _src; // The source iterator
        private readonly double _squareflat; // Square of the flatness parameter
        // for testing against squared lengths
        private readonly int _limit; // Maximum number of recursion levels
        private double[] _hold = new double[14]; // The cache of interpolated coords
        // that this must be long enough
        // to store a full cubic segment and
        // a relative cubic segment to avoid
        // aliasing when copying the coords
        // of a curve to the end of the array.
        // This is also serendipitously equal
        // to the size of a full quad segment
        // and 2 relative quad segments.
        private readonly int[] _intHold = new int[14];
        private double _curx, _cury; // The ending x,y of the last segment
        private double _movx, _movy; // The x,y of the last move segment
        private int _holdType; // The type of the curve being held
        // for interpolation
        private int _holdEnd; // The index of the last curve segment
        // being held for interpolation
        private int _holdIndex; // The index of the curve segment
        // that was last interpolated.  This
        // is the curve segment ready to be
        // returned in the next call to
        // currentSegment().
        private readonly int[] _levels; // The recursion level at which
        // each curve being held in storage
        // was generated.
        private int _levelIndex; // The index of the entry in the
        // levels array of the curve segment
        // at the holdIndex
        private bool _done; // True when iteration is done

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Ensures that the hold array can hold up to (want) more values.
        /// It is currently holding (hold.length - holdIndex) values.
        /// </summary>
        /// <param name="want">The want.</param>
        private void EnsureHoldCapacity(int want)
        {
            if (_holdIndex - want < 0)
            {
                int have = _hold.Length - _holdIndex;
                int newsize = _hold.Length + GrowSize;
                double[] newhold = new double[newsize];
                Array.Copy(_hold, _holdIndex,
                           newhold, _holdIndex + GrowSize,
                           have);
                _hold = newhold;
                _holdIndex += GrowSize;
                _holdEnd += GrowSize;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Nexts the specified do next.
        /// </summary>
        /// <param name="doNext">if set to <c>true</c> [do next].</param>
        private void Next(bool doNext)
        {
            int level;

            if (_holdIndex >= _holdEnd)
            {
                if (doNext)
                {
                    _src.Next();
                }
                if (_src.IsDone())
                {
                    _done = true;
                    return;
                }
                _holdType = _src.CurrentSegment(_intHold);
                for (int i = 0; i < _intHold.Length; i++)
                {
                    _hold[i] = _intHold[i];
                }
                _levelIndex = 0;
                _levels[0] = 0;
            }

            switch (_holdType)
            {
                case SegMoveto:
                case SegLineto:
                    _curx = _hold[0];
                    _cury = _hold[1];
                    if (_holdType == SegMoveto)
                    {
                        _movx = _curx;
                        _movy = _cury;
                    }
                    _holdIndex = 0;
                    _holdEnd = 0;
                    break;
                case SegClose:
                    _curx = _movx;
                    _cury = _movy;
                    _holdIndex = 0;
                    _holdEnd = 0;
                    break;
                case SegQuadto:
                    if (_holdIndex >= _holdEnd)
                    {
                        // Move the coordinates to the end of the array.
                        _holdIndex = _hold.Length - 6;
                        _holdEnd = _hold.Length - 2;
                        _hold[_holdIndex + 0] = _curx;
                        _hold[_holdIndex + 1] = _cury;
                        _hold[_holdIndex + 2] = _hold[0];
                        _hold[_holdIndex + 3] = _hold[1];
                        _hold[_holdIndex + 4] = _curx = _hold[2];
                        _hold[_holdIndex + 5] = _cury = _hold[3];
                    }

                    level = _levels[_levelIndex];
                    while (level < _limit)
                    {
                        for (int i = 0; i < _intHold.Length; i++)
                        {
                            _intHold[i] = (int) (_hold[i] + .5);
                        }
                        if (QuadCurve.GetFlatnessSq(_intHold, _holdIndex) < _squareflat)
                        {
                            break;
                        }

                        EnsureHoldCapacity(4);
                        QuadCurve.Subdivide(_hold, _holdIndex,
                                            _hold, _holdIndex - 4,
                                            _hold, _holdIndex);
                        _holdIndex -= 4;

                        // Now that we have subdivided, we have constructed
                        // two curves of one depth lower than the original
                        // curve.  One of those curves is in the place of
                        // the former curve and one of them is in the next
                        // set of held coordinate slots.  We now set both
                        // curves level values to the next higher level.
                        level++;
                        _levels[_levelIndex] = level;
                        _levelIndex++;
                        _levels[_levelIndex] = level;
                    }

                    // This curve segment is flat enough, or it is too deep
                    // in recursion levels to try to flatten any more.  The
                    // two coordinates at holdIndex+4 and holdIndex+5 now
                    // contain the endpoint of the curve which can be the
                    // endpoint of an approximating line segment.
                    _holdIndex += 4;
                    _levelIndex--;
                    break;
                case SegCubicto:
                    if (_holdIndex >= _holdEnd)
                    {
                        // Move the coordinates to the end of the array.
                        _holdIndex = _hold.Length - 8;
                        _holdEnd = _hold.Length - 2;
                        _hold[_holdIndex + 0] = _curx;
                        _hold[_holdIndex + 1] = _cury;
                        _hold[_holdIndex + 2] = _hold[0];
                        _hold[_holdIndex + 3] = _hold[1];
                        _hold[_holdIndex + 4] = _hold[2];
                        _hold[_holdIndex + 5] = _hold[3];
                        _hold[_holdIndex + 6] = _curx = _hold[4];
                        _hold[_holdIndex + 7] = _cury = _hold[5];
                    }

                    level = _levels[_levelIndex];
                    while (level < _limit)
                    {
                        for (int i = 0; i < _intHold.Length; i++)
                        {
                            _intHold[i] = (int) (_hold[i] + .5);
                        }
                        if (CubicCurve.GetFlatnessSq(_intHold, _holdIndex) < _squareflat)
                        {
                            break;
                        }

                        EnsureHoldCapacity(6);
                        CubicCurve.Subdivide(_hold, _holdIndex,
                                             _hold, _holdIndex - 6,
                                             _hold, _holdIndex);
                        _holdIndex -= 6;

                        // Now that we have subdivided, we have constructed
                        // two curves of one depth lower than the original
                        // curve.  One of those curves is in the place of
                        // the former curve and one of them is in the next
                        // set of held coordinate slots.  We now set both
                        // curves level values to the next higher level.
                        level++;
                        _levels[_levelIndex] = level;
                        _levelIndex++;
                        _levels[_levelIndex] = level;
                    }

                    // This curve segment is flat enough, or it is too deep
                    // in recursion levels to try to flatten any more.  The
                    // two coordinates at holdIndex+6 and holdIndex+7 now
                    // contain the endpoint of the curve which can be the
                    // endpoint of an approximating line segment.
                    _holdIndex += 6;
                    _levelIndex--;
                    break;
            }
        }
    }
}