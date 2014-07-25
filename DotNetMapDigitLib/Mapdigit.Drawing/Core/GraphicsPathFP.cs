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
using Mapdigit.Drawing.Geometry;

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
    /// Represents a series of connected lines and curves.
    /// </summary>
    internal sealed class GraphicsPathFP
    {
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsPathFP"/> class.
        /// </summary>
        public GraphicsPathFP()
        {
            _cmds = null;
            _pnts = null;
            _cmdsSize = _pntsSize = 0;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsPathFP"/> class.
        /// </summary>
        /// <param name="from">from the one to be copied</param>
        public GraphicsPathFP(GraphicsPathFP from)
        {
            _cmdsSize = from._cmdsSize;
            _pntsSize = from._pntsSize;
            if (_cmdsSize > 0)
            {
                _cmds = new int[_cmdsSize];
                _pnts = new PointFP[_pntsSize];
                Array.Copy(from._cmds, 0, _cmds, 0, _cmdsSize);
                for (int i = 0; i < _pntsSize; i++)
                {
                    _pnts[i] = new PointFP(from._pnts[i]);
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Create the line path from given coordinates.
        /// </summary>
        /// <param name="ffX1">The ff x1.</param>
        /// <param name="ffY1">The ff y1.</param>
        /// <param name="ffX2">The ff x2.</param>
        /// <param name="ffY2">The ff y2.</param>
        /// <returns></returns>
        public static GraphicsPathFP CreateLine(int ffX1, int ffY1,
                                                int ffX2, int ffY2)
        {
            var path = new GraphicsPathFP();
            path.AddMoveTo(new PointFP(ffX1, ffY1));
            path.AddLineTo(new PointFP(ffX2, ffY2));
            return path;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Create the oval path from given rectangle.
        /// </summary>
        /// <param name="ffXmin">The ff xmin.</param>
        /// <param name="ffYmin">The ff ymin.</param>
        /// <param name="ffXmax">The ff xmax.</param>
        /// <param name="ffYmax">The ff ymax.</param>
        /// <returns></returns>
        public static GraphicsPathFP CreateOval(int ffXmin, int ffYmin,
                                                int ffXmax, int ffYmax)
        {
            var path = CreateArc(ffXmin, ffYmin,
                                 ffXmax, ffYmax, 0, MathFP.Pi*2, false);
            path.AddClose();
            return path;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates the round rect.
        /// </summary>
        /// <param name="ffXmin">The ff xmin.</param>
        /// <param name="ffYmin">The ff ymin.</param>
        /// <param name="ffXmax">The ff xmax.</param>
        /// <param name="ffYmax">The ff ymax.</param>
        /// <param name="ffRx">The ff rx.</param>
        /// <param name="ffRy">The ff ry.</param>
        /// <returns></returns>
        public static GraphicsPathFP CreateRoundRect(int ffXmin, int ffYmin,
                                                     int ffXmax, int ffYmax, int ffRx, int ffRy)
        {
            const int ffPi = MathFP.Pi;
            var path = new GraphicsPathFP();
            path.AddMoveTo(new PointFP(ffXmin + ffRx, ffYmin));
            path.AddLineTo(new PointFP(ffXmax - ffRx, ffYmin));
            var ffRmax = MathFP.Min(ffXmax - ffXmin, ffYmax - ffYmin)/2;
            if (ffRx > ffRmax)
            {
                ffRx = ffRmax;
            }
            if (ffRy > ffRmax)
            {
                ffRy = ffRmax;
            }
            if (ffRx != 0 && ffRy != 0)
            {
                path.AddPath(CreateArc(ffXmax - ffRx*2,
                                       ffYmin, ffXmax, ffYmin + ffRy*2,
                                       (-ffPi)/2, 0, false, false));
            }
            path.AddLineTo(new PointFP(ffXmax, ffYmin + ffRy));
            path.AddLineTo(new PointFP(ffXmax, ffYmax - ffRy));
            if (ffRx != 0 && ffRy != 0)
            {
                path.AddPath(CreateArc(ffXmax - ffRx*2,
                                       ffYmax - ffRy*2, ffXmax, ffYmax, 0,
                                       ffPi/2, false, false));
            }
            path.AddLineTo(new PointFP(ffXmax - ffRx, ffYmax));
            path.AddLineTo(new PointFP(ffXmin + ffRx, ffYmax));
            if (ffRx != 0 && ffRy != 0)
            {
                path.AddPath(CreateArc(ffXmin, ffYmax - ffRy*2,
                                       ffXmin + ffRx*2, ffYmax,
                                       ffPi/2, ffPi, false, false));
            }
            path.AddLineTo(new PointFP(ffXmin, ffYmax - ffRy));
            path.AddLineTo(new PointFP(ffXmin, ffYmin + ffRy));
            if (ffRx != 0 && ffRy != 0)
            {
                path.AddPath(CreateArc(ffXmin, ffYmin,
                                       ffXmin + ffRx*2, ffYmin + ffRy*2, -ffPi,
                                       (-ffPi)/2, false, false));
            }
            path.AddClose();
            return path;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates the smooth curves.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="numberOfSegments">The number of segments.</param>
        /// <param name="ffFactor">The ff factor.</param>
        /// <param name="closed">if set to <c>true</c> [closed].</param>
        /// <returns></returns>
        public static GraphicsPathFP CreateSmoothCurves(PointFP[] points,
                                                        int offset, int numberOfSegments, int ffFactor, bool closed)
        {
            var len = points.Length;
            var path = new GraphicsPathFP();

            if (numberOfSegments < 1 ||
                numberOfSegments > points.Length - 1 ||
                offset < 0 ||
                offset + numberOfSegments > len - 1)
            {
                return path;
            }

            var pc1S = new PointFP[points.Length];
            var pc2S = new PointFP[points.Length];
            if (!closed)
            {
                pc1S[0] = points[0];
                pc2S[len - 1] = points[len - 1];
            }
            else
            {
                pc1S[0] = CalcControlPoint(points[len - 1],
                                           points[0], points[1], ffFactor);
                pc2S[0] = CalcControlPoint(points[1], points[0],
                                           points[len - 1], ffFactor);
                pc1S[len - 1] = CalcControlPoint(points[len - 2], points[len - 1],
                                                 points[0], ffFactor);
                pc2S[len - 1] = CalcControlPoint(points[0], points[len - 1],
                                                 points[len - 2], ffFactor);
            }
            for (var i = 1; i < len - 1; i++)
            {
                pc1S[i] = CalcControlPoint(points[i - 1], points[i],
                                           points[i + 1], ffFactor);
                pc2S[i] = CalcControlPoint(points[i + 1], points[i],
                                           points[i - 1], ffFactor);
            }

            path.AddMoveTo(points[offset]);
            for (var i = 0; i < numberOfSegments; i++)
            {
                path.AddCurveTo(pc1S[offset + i], pc2S[offset + i + 1],
                                points[offset + i + 1]);
            }
            if (closed)
            {
                path.AddCurveTo(pc1S[len - 1], pc2S[0], points[0]);
                path.AddClose();
            }
            return path;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates the polyline.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <returns></returns>
        public static GraphicsPathFP CreatePolyline(PointFP[] points)
        {
            var path = new GraphicsPathFP();
            if (points.Length > 0)
            {
                path.AddMoveTo(points[0]);
                for (var i = 1; i < points.Length; i++)
                {
                    path.AddLineTo(points[i]);
                }
            }
            return path;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates the polygon.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <returns></returns>
        public static GraphicsPathFP CreatePolygon(PointFP[] points)
        {
            var path = CreatePolyline(points);
            if (points.Length > 0)
            {
                path.AddClose();
            }
            return path;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates the rect.
        /// </summary>
        /// <param name="ffXmin">The ff xmin.</param>
        /// <param name="ffYmin">The ff ymin.</param>
        /// <param name="ffXmax">The ff xmax.</param>
        /// <param name="ffYmax">The ff ymax.</param>
        /// <returns></returns>
        public static GraphicsPathFP CreateRect(int ffXmin, int ffYmin,
                                                int ffXmax, int ffYmax)
        {
            return CreatePolygon(
                new[]
                    {
                        new PointFP(ffXmin, ffYmin),
                        new PointFP(ffXmax, ffYmin),
                        new PointFP(ffXmax, ffYmax),
                        new PointFP(ffXmin, ffYmax)
                    });
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates the arc.
        /// </summary>
        /// <param name="ffXmin">The ff xmin.</param>
        /// <param name="ffYmin">The ff ymin.</param>
        /// <param name="ffXmax">The ff xmax.</param>
        /// <param name="ffYmax">The ff ymax.</param>
        /// <param name="ffArg1">The ff arg1.</param>
        /// <param name="ffArg2">The ff arg2.</param>
        /// <param name="closed">if set to <c>true</c> [closed].</param>
        /// <returns></returns>
        public static GraphicsPathFP CreateArc(int ffXmin, int ffYmin,
                                               int ffXmax, int ffYmax, int ffArg1, int ffArg2,
                                               bool closed)
        {
            return CreateArc(ffXmin, ffYmin, ffXmax, ffYmax, ffArg1,
                             ffArg2, closed, true);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates the arc.
        /// </summary>
        /// <param name="ffXmin">The ff xmin.</param>
        /// <param name="ffYmin">The ff ymin.</param>
        /// <param name="ffXmax">The ff xmax.</param>
        /// <param name="ffYmax">The ff ymax.</param>
        /// <param name="ffStartangle">The ff startangle.</param>
        /// <param name="ffSweepangle">The ff sweepangle.</param>
        /// <param name="closed">if set to <c>true</c> [closed].</param>
        /// <param name="standalone">if set to <c>true</c> [standalone].</param>
        /// <returns></returns>
        public static GraphicsPathFP CreateArc(int ffXmin, int ffYmin,
                                               int ffXmax, int ffYmax, int ffStartangle,
                                               int ffSweepangle, bool closed, bool standalone)
        {
            if (ffSweepangle < 0)
            {
                ffStartangle += ffSweepangle;
                ffSweepangle = -ffSweepangle;
            }
            var segments = MathFP.Round(MathFP.Div(4*MathFP.Abs(ffSweepangle),
                                                   MathFP.Pi)) >> SingleFP.DecimalBits;
            if (segments == 0)
            {
                segments = 1;
            }
            var path = new GraphicsPathFP();
            var ffDarg = ffSweepangle/segments;
            var ffArg = ffStartangle;
            var ffLastcos = MathFP.Cos(ffStartangle);
            var ffLastsin = MathFP.Sin(ffStartangle);
            var ffXc = (ffXmin + ffXmax)/2;
            var ffYc = (ffYmin + ffYmax)/2;
            var ffRx = (ffXmax - ffXmin)/2;
            var ffRy = (ffYmax - ffYmin)/2;
            var ffRxbeta = MathFP.Mul(17381, ffRx);
            var ffRybeta = MathFP.Mul(17381, ffRy);

            if (closed)
            {
                path.AddMoveTo(new PointFP(ffXc, ffYc));
            }

            for (var i = 1; i <= segments; i++)
            {
                ffArg = i == segments
                            ? ffStartangle + ffSweepangle
                            : ffArg + ffDarg;
                var ffCurrcos = MathFP.Cos(ffArg);
                var ffCurrsin = MathFP.Sin(ffArg);
                var ffX1 = ffXc + MathFP.Mul(ffRx, ffLastcos);
                var ffY1 = ffYc + MathFP.Mul(ffRy, ffLastsin);
                var ffX2 = ffXc + MathFP.Mul(ffRx, ffCurrcos);
                var ffY2 = ffYc + MathFP.Mul(ffRy, ffCurrsin);
                if (i == 1)
                {
                    if (closed)
                    {
                        path.AddLineTo(new PointFP(ffX1, ffY1));
                    }
                    else if (standalone)
                    {
                        path.AddMoveTo(new PointFP(ffX1, ffY1));
                    }
                }

                path.AddCurveTo(
                    new PointFP(ffX1 - MathFP.Mul(ffRxbeta, ffLastsin),
                                ffY1 + MathFP.Mul(ffRybeta, ffLastcos)),
                    new PointFP(ffX2 + MathFP.Mul(ffRxbeta, ffCurrsin),
                                ffY2 - MathFP.Mul(ffRybeta, ffCurrcos)),
                    new PointFP(ffX2, ffY2));
                ffLastcos = ffCurrcos;
                ffLastsin = ffCurrsin;
            }
            if (closed)
            {
                path.AddClose();
            }
            return path;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Adds the path.
        /// </summary>
        /// <param name="path">The path.</param>
        public void AddPath(GraphicsPathFP path)
        {
            if (path._cmdsSize > 0)
            {
                ExtendIfNeeded(path._cmdsSize, path._pntsSize);
                Array.Copy(path._cmds, 0, _cmds, _cmdsSize, path._cmdsSize);
                for (int i = 0; i < path._pntsSize; i++)
                {
                    _pnts[i + _pntsSize] = new PointFP(path._pnts[i]);
                }
                _cmdsSize += path._cmdsSize;
                _pntsSize += path._pntsSize;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// add move to this path
        /// </summary>
        /// <param name="point">The point.</param>
        public void AddMoveTo(PointFP point)
        {
            ExtendIfNeeded(1, 1);
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
        public void AddLineTo(PointFP point)
        {
            ExtendIfNeeded(1, 1);
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
        /// Adds the quad to.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="point">The point.</param>
        public void AddQuadTo(PointFP control, PointFP point)
        {
            if (control.Equals(point))
            {
                AddLineTo(point);
                return;
            }
            ExtendIfNeeded(1, 2);
            _cmds[_cmdsSize++] = CmdQcurveto;
            _pnts[_pntsSize++] = new PointFP(control);
            _pnts[_pntsSize++] = new PointFP(point);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Adds the curve to.
        /// </summary>
        /// <param name="control1">The control1.</param>
        /// <param name="control2">The control2.</param>
        /// <param name="point">The point.</param>
        public void AddCurveTo(PointFP control1, PointFP control2, PointFP point)
        {
            if (_pnts[_pntsSize - 1].Equals(control1))
            {
                AddQuadTo(control2, point);
                return;
            }
            if (point.Equals(control2))
            {
                AddQuadTo(control1, point);
                return;
            }
            ExtendIfNeeded(1, 3);
            _cmds[_cmdsSize++] = CmdCcurveto;
            _pnts[_pntsSize++] = new PointFP(control1);
            _pnts[_pntsSize++] = new PointFP(control2);
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
        public void AddClose()
        {
            ExtendIfNeeded(1, 0);
            _cmds[_cmdsSize++] = CmdClose;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Calculate outline with given pen.
        /// </summary>
        /// <param name="lineStyle">The line style.</param>
        /// <returns></returns>
        public GraphicsPathFP CalcOutline(PenFP lineStyle)
        {
            var outline = new GraphicsPathFP();
            var outlineGenerator =
                new GraphicsPathOutlineFP(outline, lineStyle);
            Visit(outlineGenerator);
            return outline;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// To path object in drawing package.
        /// </summary>
        /// <returns></returns>
        public Path ToPath()
        {
            Path path = new Path();
            int j = 0;
            for (int i = 0; i < _cmdsSize; i++)
            {
                switch (_cmds[i])
                {
                    case CmdNop:
                        break;
                    case CmdMoveto:
                        {
                            PointFP pt = _pnts[j++];
                            path.MoveTo(pt.X >> SingleFP.DecimalBits,
                                        pt.Y >> SingleFP.DecimalBits);
                        }
                        break;
                    case CmdLineto:
                        {
                            PointFP pt = _pnts[j++];
                            path.LineTo(pt.X >> SingleFP.DecimalBits,
                                        pt.Y >> SingleFP.DecimalBits);
                        }

                        break;
                    case CmdQcurveto:
                        {
                            PointFP pt1 = _pnts[j++];
                            PointFP pt2 = _pnts[j++];
                            path.QuadTo(pt1.X >> SingleFP.DecimalBits,
                                        pt1.Y >> SingleFP.DecimalBits,
                                        pt2.X >> SingleFP.DecimalBits,
                                        pt2.Y >> SingleFP.DecimalBits);
                        }
                        break;
                    case CmdCcurveto:
                        {
                            PointFP pt1 = _pnts[j++];
                            PointFP pt2 = _pnts[j++];
                            PointFP pt3 = _pnts[j++];
                            path.CurveTo(pt1.X >> SingleFP.DecimalBits,
                                         pt1.Y >> SingleFP.DecimalBits,
                                         pt2.X >> SingleFP.DecimalBits,
                                         pt2.Y >> SingleFP.DecimalBits,
                                         pt3.X >> SingleFP.DecimalBits,
                                         pt3.Y >> SingleFP.DecimalBits);
                        }
                        break;
                    case CmdClose:
                        path.ClosePath();
                        break;
                    default:
                        break;
                }
            }
            return path;
        }


        internal int[] _cmds;
        internal PointFP[] _pnts;
        internal int _cmdsSize;
        internal int _pntsSize;

        internal static readonly PointFP[] _roundcap = new PointFP[7];
        internal static readonly PointFP[] _squarecap = new PointFP[2];
        internal static readonly int _one;


        private const int CmdNop = 0;
        private const int CmdMoveto = 1;
        private const int CmdLineto = 2;
        private const int CmdQcurveto = 3;
        private const int CmdCcurveto = 4;
        private const int CmdClose = 6;
        private const int Blocksize = 16;


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes the <see cref="GraphicsPathFP"/> class.
        /// </summary>
        static GraphicsPathFP()
        {
            _one = SingleFP.One;
            _roundcap[0] = new PointFP(25080, 60547);
            _roundcap[1] = new PointFP(46341, 46341);
            _roundcap[2] = new PointFP(60547, 25080);
            _roundcap[3] = new PointFP(_one, 0);
            _roundcap[4] = new PointFP(60547, -25080);
            _roundcap[5] = new PointFP(46341, -46341);
            _roundcap[6] = new PointFP(25080, -60547);
            _squarecap[0] = new PointFP(_one, _one);
            _squarecap[1] = new PointFP(_one, -_one);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Visits the specified iterator.
        /// </summary>
        /// <param name="iterator">The iterator.</param>
        internal void Visit(IGraphicsPathIteratorFP iterator)
        {
            if (iterator != null)
            {
                iterator.Begin();
                int j = 0;
                for (int i = 0; i < _cmdsSize; i++)
                {
                    switch (_cmds[i])
                    {
                        case CmdNop:
                            break;

                        case CmdMoveto:
                            iterator.MoveTo(_pnts[j++]);
                            break;

                        case CmdLineto:
                            iterator.LineTo(_pnts[j++]);
                            break;

                        case CmdQcurveto:
                            iterator.QuadTo(_pnts[j++], _pnts[j++]);
                            break;

                        case CmdCcurveto:
                            iterator.CurveTo(_pnts[j++], _pnts[j++], _pnts[j++]);
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

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Calcs the control point.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        /// <param name="ffFactor">The ff factor.</param>
        /// <returns></returns>
        private static PointFP CalcControlPoint(PointFP p1, PointFP p2,
                                                PointFP p3, int ffFactor)
        {
            var ps = new PointFP(p2.X + MathFP.Mul(p2.X - p1.X, ffFactor),
                                 p2.Y + MathFP.Mul(p2.Y - p1.Y, ffFactor));
            return new LineFP((new LineFP(p2, ps)).GetCenter(),
                              (new LineFP(p2, p3)).GetCenter()).GetCenter();
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
        internal void ExtendIfNeeded(int cmdsAddNum, int pntsAddNum)
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
    }
}