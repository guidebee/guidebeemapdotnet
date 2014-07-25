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
    /// provide dash support.
    /// </summary>
    internal class GraphicsPathDasherFP : GraphicsPathSketchFP
    {
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsPathDasherFP"/> class.
        /// </summary>
        /// <param name="from"> the path which need to be dashed</param>
        /// <param name="dashArray">The dash array.</param>
        /// <param name="offset">from where the dash starts</param>
        public GraphicsPathDasherFP(GraphicsPathFP from, int[] dashArray, int offset)
        {
            _fromPath = new GraphicsPathFP(from);
            var arrayLength = dashArray.Length - offset;
            if (arrayLength > 1)
            {
                _pnts = new PointFP[Blocksize];
                _cmds = new int[Blocksize];
                _dashArray = new int[dashArray.Length - offset];
                Array.Copy(dashArray, offset,
                           _dashArray, 0, dashArray.Length);
                VisitPath(this);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// return the dashed path, if the dash array is null, return the path unchanged.
        /// </summary>
        /// <returns>the dash path</returns>
        public GraphicsPathFP GetDashedGraphicsPath()
        {
            if (_dashArray == null)
            {
                return _fromPath;
            }

            var dashedPath = new GraphicsPathFP();
            var lineFP = new LineFP();
            var j = 0;
            for (var i = 0; i < _cmdsSize; i++)
            {
                switch (_cmds[i])
                {
                    case CmdMoveto:
                        dashedPath.AddMoveTo(_pnts[j++]);
                        break;
                    case CmdLineto:
                        {
                            int pointIndex = j;
                            lineFP.Reset(_pnts[pointIndex - 1], _pnts[pointIndex]);
                            DashLine(dashedPath, lineFP);
                            j++;
                        }
                        break;
                    case CmdClose:
                        dashedPath.AddClose();
                        break;
                }
            }

            return dashedPath;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Moves to given point.
        /// </summary>
        /// <param name="point">The point.</param>
        public override void MoveTo(PointFP point)
        {
            base.MoveTo(point);
            ExtendIfNeeded(1, 1);
            AddMoveTo(point);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// draw a line from current point to given point.
        /// </summary>
        /// <param name="point">The point.</param>
        public override void LineTo(PointFP point)
        {
            base.LineTo(point);
            ExtendIfNeeded(1, 1);
            AddLineTo(point);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Closes the path.
        /// </summary>
        public override void Close()
        {
            base.Close();
            ExtendIfNeeded(1, 0);
            AddClose();
        }


        private const int CmdNop = 0;
        private const int CmdMoveto = 1;
        private const int CmdLineto = 2;
        private const int CmdQcurveto = 3;
        private const int CmdCcurveto = 4;
        private const int CmdClose = 6;
        private const int Blocksize = 16;
        private int[] _cmds;
        private PointFP[] _pnts;
        private int _cmdsSize;
        private int _pntsSize;
        private readonly GraphicsPathFP _fromPath;
        private readonly int[] _dashArray;
        private int _dashIndex;
        private int _nextDistance = -1;
        private bool _isEmpty;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Adds the move to.
        /// </summary>
        /// <param name="point">The point.</param>
        private void AddMoveTo(PointFP point)
        {
            _cmds[_cmdsSize++] = CmdMoveto;
            _pnts[_pntsSize++] = new PointFP(point);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Adds the line to.
        /// </summary>
        /// <param name="point">The point.</param>
        private void AddLineTo(PointFP point)
        {
            _cmds[_cmdsSize++] = CmdLineto;
            _pnts[_pntsSize++] = new PointFP(point);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Adds the close.
        /// </summary>
        private void AddClose()
        {
            _cmds[_cmdsSize++] = CmdClose;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Extends if needed.
        /// </summary>
        /// <param name="cmdsAddNum">The CMDS add num.</param>
        /// <param name="pntsAddNum">The PNTS add num.</param>
        private void ExtendIfNeeded(int cmdsAddNum, int pntsAddNum)
        {
            if (_cmds == null)
            {
                _cmds = new int[Blocksize];
            }
            if (_pnts == null)
            {
                _pnts = new PointFP[Blocksize];
            }

            if (_cmdsSize + cmdsAddNum > _cmds.Length)
            {
                var newdata = new int[_cmds.Length + (cmdsAddNum > Blocksize
                                                          ? cmdsAddNum
                                                          : Blocksize)];
                if (_cmdsSize > 0)
                {
                    Array.Copy(_cmds, 0, newdata, 0, _cmdsSize);
                }
                _cmds = newdata;
            }
            if (_pntsSize + pntsAddNum > _pnts.Length)
            {
                var newdata = new PointFP[_pnts.Length +
                                          (pntsAddNum > Blocksize ? pntsAddNum : Blocksize)];
                if (_pntsSize > 0)
                {
                    Array.Copy(_pnts, 0, newdata, 0, _pntsSize);
                }
                _pnts = newdata;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Dashes the line.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="line">The line.</param>
        private void DashLine(GraphicsPathFP path, LineFP line)
        {
            if (_nextDistance < 0)
            {
                _nextDistance = _dashArray[_dashIndex];
                _dashIndex = (_dashIndex + 1)%_dashArray.Length;
            }
            var distance = _nextDistance;

            var pt = line.GetPointAtDistance(distance);
            while (pt != null)
            {
                if (_isEmpty)
                {
                    path.AddMoveTo(pt);
                }
                else
                {
                    path.AddLineTo(pt);
                }

                _isEmpty = !_isEmpty;
                _nextDistance += _dashArray[_dashIndex];
                distance = _nextDistance;
                pt = line.GetPointAtDistance(distance);
                _dashIndex = (_dashIndex + 1)%_dashArray.Length;
            }
            if (_isEmpty)
            {
                path.AddMoveTo(line.Pt2);
            }
            else
            {
                path.AddLineTo(line.Pt2);
            }
            _nextDistance = _nextDistance - line.GetLength();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Visits the path.
        /// </summary>
        /// <param name="iterator">The iterator.</param>
        private void VisitPath(IGraphicsPathIteratorFP iterator)
        {
            if (iterator != null)
            {
                iterator.Begin();
                int j = 0;
                for (int i = 0; i < _fromPath._cmdsSize; i++)
                {
                    switch (_fromPath._cmds[i])
                    {
                        case CmdNop:
                            break;

                        case CmdMoveto:
                            iterator.MoveTo(_fromPath._pnts[j++]);
                            break;

                        case CmdLineto:
                            iterator.LineTo(_fromPath._pnts[j++]);
                            break;

                        case CmdQcurveto:
                            iterator.QuadTo(_fromPath._pnts[j++],
                                            _fromPath._pnts[j++]);
                            break;

                        case CmdCcurveto:
                            iterator.CurveTo(_fromPath._pnts[j++],
                                             _fromPath._pnts[j++], _fromPath._pnts[j++]);
                            break;

                        case CmdClose:
                            iterator.Close();
                            break;

                        default:
                            return;
                    }
                }
                iterator.End();
            }
        }
    }
}