﻿//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 24SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------

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
    /// Create outline for a given path.
    /// </summary>
    internal class GraphicsPathOutlineFP : GraphicsPathSketchFP
    {
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsPathOutlineFP"/> class.
        /// </summary>
        /// <param name="outline">The outline.</param>
        /// <param name="lineStyle">The line style.</param>
        public GraphicsPathOutlineFP(GraphicsPathFP outline, PenFP lineStyle)
        {
            _outline = outline;
            _ffRad = lineStyle.Width/2;
            _startLineCap = lineStyle.StartCap;
            _endLineCap = lineStyle.EndCap;
            _lineJoin = lineStyle.LineJoin;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Ends the path.
        /// </summary>
        public override void End()
        {
            FinishCurrentSegment();
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
            FinishCurrentSegment();
            _needDrawStartCap = true;
            _closed = false;
            _startCapP1 = _startCapP2 = null;
            base.MoveTo(point);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draw quadic curve with given control points.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="point">The point.</param>
        public override void QuadTo(PointFP control, PointFP point)
        {
            CurveBegin(control);
            base.QuadTo(control, point);
            CurveEnd(control, control, point);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draw curve with given control points.
        /// </summary>
        /// <param name="control1">The control1.</param>
        /// <param name="control2">The control2.</param>
        /// <param name="point">The point.</param>
        public override void CurveTo(PointFP control1, PointFP control2, PointFP point)
        {
            CurveBegin(control1);
            base.CurveTo(control1, control2, point);
            CurveEnd(control1, control2, point);
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
            _closed = true;
            if (_startCapP1 != null && _startCapP2 != null &&
                _lastPoint != null && _currPoint != null)
            {
                AddLineJoin(_startCapP1.Equals(_currPoint)
                                ? _lastPoint
                                : _currPoint, _startCapP1, _startCapP2);
            }
            LineTo(_startPoint);
            _started = false;
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
            if (point.Equals(_currPoint))
            {
                return;
            }

            LineFP head, tail;
            CalcHeadTail(_currPoint, point,
                         head = new LineFP(), tail = new LineFP());

            if (_drawingCurve)
            {
                if (_lastCurveTail != null)
                {
                    _curvePath1.AddLineTo(_lastCurveTail.Pt1);
                    _curvePath2.AddLineTo(_lastCurveTail.Pt2);
                }
                _lastCurveTail = new LineFP(tail);
            }
            else
            {
                if (_needDrawStartCap)
                {
                    _startCapP1 = new PointFP(_currPoint);
                    _startCapP2 = new PointFP(point);
                    _needDrawStartCap = false;
                }
                AddLineJoin(_lastPoint, _currPoint, point);

                _outline.AddMoveTo(head.Pt1);
                _outline.AddLineTo(tail.Pt1);
                _outline.AddLineTo(tail.Pt2);
                _outline.AddLineTo(head.Pt2);
                _outline.AddLineTo(head.Pt1);
                _outline.AddClose();
                _lastPoint = new PointFP(_currPoint);
            }
            base.LineTo(point);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Finishes the current segment.
        /// </summary>
        private void FinishCurrentSegment()
        {
            if (_closed)
            {
                return;
            }
            if (_startCapP1 != null && _startCapP2 != null)
            {
                AddLineCap(_startCapP2, _startCapP1, _startLineCap);
            }
            if (_lastPoint != null)
            {
                AddLineCap(_lastPoint, _currPoint, _endLineCap);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Adds the line cap.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="lineCap">The line cap.</param>
        private void AddLineCap(PointFP p1, PointFP p2, int lineCap)
        {
            if (lineCap == PenFP.LinecapButt || p1.Equals(p2))
            {
                return;
            }
            var dx = p2.X - p1.X;
            var dy = p2.Y - p1.Y;
            var len = PointFP.Distance(dx, dy);
            var cap = lineCap == PenFP.LinecapRound
                          ? GraphicsPathFP._roundcap
                          : GraphicsPathFP._squarecap;

            dx = MathFP.Mul(_ffRad, MathFP.Div(dx, len));
            dy = MathFP.Mul(_ffRad, MathFP.Div(dy, len));

            var m = new MatrixFP(dx, dx, dy, -dy, p2.X, p2.Y);
            _outline.AddMoveTo(new PointFP(0, GraphicsPathFP._one).Transform(m));
            for (var i = 0; i < cap.Length; i++)
            {
                _outline.AddLineTo(new PointFP(cap[i]).Transform(m));
            }
            _outline.AddLineTo(new PointFP(0, -GraphicsPathFP._one).Transform(m));
            _outline.AddClose();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Calcs the head tail.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="head">The head.</param>
        /// <param name="tail">The tail.</param>
        private void CalcHeadTail(PointFP p1, PointFP p2, LineFP head,
                                  LineFP tail)
        {
            var curr = new LineFP(p1, p2);
            head.Reset(curr.GetHeadOutline(_ffRad));
            var dx = p2.X - p1.X;
            var dy = p2.Y - p1.Y;
            tail.Reset(head.Pt1.X + dx, head.Pt1.Y + dy,
                       head.Pt2.X + dx, head.Pt2.Y + dy);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Adds the line join.
        /// </summary>
        /// <param name="lastPoint">The last point.</param>
        /// <param name="currPoint">The curr point.</param>
        /// <param name="nextPoint">The next point.</param>
        private void AddLineJoin(PointFP lastPoint, PointFP currPoint,
                                 PointFP nextPoint)
        {
            if (lastPoint == null || currPoint == null || nextPoint == null
                || nextPoint.Equals(currPoint) || lastPoint.Equals(currPoint))
            {
                return;
            }

            PointFP p1 = null, p2 = null;
            LineFP head, tail, lastHead, lastTail;
            CalcHeadTail(currPoint, nextPoint,
                         head = new LineFP(), tail = new LineFP());
            CalcHeadTail(lastPoint, currPoint,
                         lastHead = new LineFP(), lastTail = new LineFP());
            var needLineJoin = false;
            var pi1 = new PointFP();
            var pi2 = new PointFP();

            var cross1 = LineFP.Intersects(new LineFP(head.Pt1, tail.Pt1),
                                           new LineFP(lastHead.Pt1, lastTail.Pt1), pi1);
            var cross2 = LineFP.Intersects(new LineFP(head.Pt2, tail.Pt2),
                                           new LineFP(lastHead.Pt2, lastTail.Pt2), pi2);
            if (cross1 && !cross2 && pi1.X != SingleFP.NotANumber)
            {
                p1 = lastTail.Pt2;
                p2 = head.Pt2;
                needLineJoin = true;
            }
            else if (!cross1 && cross2 && pi2.X != SingleFP.NotANumber)
            {
                p1 = lastTail.Pt1;
                p2 = head.Pt1;
                needLineJoin = true;
            }
            if (needLineJoin)
            {
                _outline.AddMoveTo(cross1 ? pi1 : pi2);
                _outline.AddLineTo(cross1 ? p2 : p1);
                if (_lineJoin == PenFP.LinejoinMiter)
                {
                    _outline.AddLineTo(cross1 ? pi2 : pi1);
                }
                _outline.AddLineTo(cross1 ? p1 : p2);
                _outline.AddClose();
                if (_lineJoin == PenFP.LinejoinRound)
                {
                    AddLineCap(cross2 ? pi2 : pi1, currPoint, PenFP.LinecapRound);
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
        /// Curves the begin.
        /// </summary>
        /// <param name="control">The control.</param>
        private void CurveBegin(PointFP control)
        {
            AddLineJoin(_lastPoint, _currPoint, control);
            _drawingCurve = true;
            _curvePath1 = new GraphicsPathFP();
            _curvePath2 = new GraphicsPathFP();
            _curveBegin = new PointFP(_currPoint);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Curves the end.
        /// </summary>
        /// <param name="control1">The control1.</param>
        /// <param name="control2">The control2.</param>
        /// <param name="curveEnd">The curve end.</param>
        private void CurveEnd(PointFP control1, PointFP control2, PointFP curveEnd)
        {
            _drawingCurve = false;
            if (_needDrawStartCap)
            {
                _startCapP1 = new PointFP(_curveBegin);
                _startCapP2 = new PointFP(control1);
                _needDrawStartCap = false;
            }
            var head = new LineFP();
            var tail = new LineFP();
            CalcHeadTail(_curveBegin, control1, head, new LineFP());
            _outline.AddMoveTo(head.Pt1);
            _outline.AddPath(_curvePath1);
            CalcHeadTail(control2, curveEnd, new LineFP(), tail);
            _outline.AddLineTo(tail.Pt1);
            _outline.AddLineTo(tail.Pt2);
            _outline.ExtendIfNeeded(_curvePath1._cmdsSize, _curvePath1._pntsSize);
            int j = _curvePath2._pntsSize - 1;
            for (int i = _curvePath2._cmdsSize - 1; i >= 0; i--)
            {
                _outline.AddLineTo(_curvePath2._pnts[j--]);
            }
            _outline.AddLineTo(head.Pt2);
            _outline.AddClose();
            _curvePath1 = null;
            _curvePath2 = null;
            _lastCurveTail = null;
            _lastPoint = new PointFP(control2);
            _drawingCurve = false;
        }

        private readonly int _ffRad;
        private readonly int _startLineCap;
        private readonly int _endLineCap;
        private readonly int _lineJoin;
        private bool _needDrawStartCap;
        private PointFP _lastPoint;
        private LineFP _lastCurveTail;
        private GraphicsPathFP _curvePath1;
        private GraphicsPathFP _curvePath2;
        private PointFP _curveBegin;
        private bool _drawingCurve;
        private readonly GraphicsPathFP _outline;
        private PointFP _startCapP1, _startCapP2;
        private bool _closed = true;
    }
}