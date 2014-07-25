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
    /// An class that describes a gradient, composed of gradient stops.
    /// Classes that inherit from GradientBrush describe different ways of
    /// interpreting gradient stops.
    /// </summary>
    internal class RadialGradientBrushFP : BrushFP
    {
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="RadialGradientBrushFP"/> class.
        /// </summary>
        /// <param name="ffX">the top left coordinate..</param>
        /// <param name="ffY">the top left coordinate.</param>
        /// <param name="ffRadius">radius of the gradident</param>
        /// <param name="ffAngle">angle.</param>
        public RadialGradientBrushFP(int ffX, int ffY, int ffRadius,
                                     int ffAngle)
        {
            _matrix = new MatrixFP();
            _centerPt.Reset(ffX,
                            ffY);
            _matrix.Translate(-_centerPt.X, -_centerPt.Y);
            _matrix.Rotate(-ffAngle);
            _ffRadius = ffRadius;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set gradient color at given ratio.
        /// </summary>
        /// <param name="ratio">The ratio.</param>
        /// <param name="color">The color.</param>
        public void SetGradientColor(int ratio, int color)
        {
            ratio = ratio >> SingleFP.DecimalBits - RatioBits;
            int i;
            ratio = ratio < 0 ? 0 : (ratio > RatioMax ? RatioMax : ratio);
            if (_ratioCount == _ratios.Length)
            {
                var rs = new int[_ratioCount + 16];
                Array.Copy(_ratios, 0, rs, 0, _ratioCount);
                _ratios = rs;
            }
            _gradientColors[ratio] = color;
            for (i = _ratioCount; i > 0; i--)
            {
                if (ratio >= _ratios[i - 1])
                {
                    break;
                }
            }
            if (!(i > 0 && ratio == _ratios[i]))
            {
                if (i < _ratioCount)
                {
                    Array.Copy(_ratios, i, _ratios, i + 1, _ratioCount - i);
                }
                _ratios[i] = ratio;
                _ratioCount++;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Updates the gradient table.
        /// </summary>
        public void UpdateGradientTable()
        {
            if (_ratioCount == 0)
            {
                return;
            }
            int i;
            for (i = 0; i < _ratios[0]; i++)
            {
                _gradientColors[i] = _gradientColors[_ratios[0]];
            }
            for (i = 1; i < _ratioCount; i++)
            {
                int r1 = _ratios[i - 1];
                int r2 = _ratios[i];
                for (int j = r1 + 1; j < r2; j++)
                {
                    _gradientColors[j] = Interpolate(_gradientColors[r1],
                                                     _gradientColors[r2], 256*(j - r1)/(r2 - r1));
                }
            }
            for (i = _ratios[_ratioCount - 1]; i <= RatioMax; i++)
            {
                _gradientColors[i] = _gradientColors[_ratios[_ratioCount - 1]];
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Interpolates the specified a.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <param name="pos">The pos.</param>
        /// <returns></returns>
        private static int Interpolate(int a, int b, int pos)
        {
            int p2 = pos & 0xFF;
            int p1 = 0xFF - p2;
            int ca = ((a >> 24) & 0xFF)*p1 + ((b >> 24) & 0xFF)*p2;
            int cr = ((a >> 16) & 0xFF)*p1 + ((b >> 16) & 0xFF)*p2;
            int cg = ((a >> 8) & 0xFF)*p1 + ((b >> 8) & 0xFF)*p2;
            int cb = (a & 0xFF)*p1 + (b & 0xFF)*p2;
            return ((ca >> 8) << 24) | ((cr >> 8) << 16) | ((cg >> 8) << 8) | ((cb >> 8));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Check if it's a mono color brush.
        /// </summary>
        /// <returns>true if it's mono color brush.</returns>
        public override bool IsMonoColor()
        {
            return false;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the color value at given position.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="singlePoint">single point or not</param>
        /// <returns>the color value.</returns>
        public override int GetColorAt(int x, int y, bool singlePoint)
        {
            var p = new PointFP(x << SingleFP.DecimalBits,
                                y << SingleFP.DecimalBits);
            _nextPt.X = p.X + SingleFP.One;
            _nextPt.Y = p.Y;
            var newCenterPt = new PointFP(_centerPt);
            if (_finalMatrix != null)
            {
                p.Transform(_finalMatrix);
                //newCenterPt.transform(finalMatrix);
            }
            _ffCurrpos = MathFP.Div(PointFP.Distance(p.X - newCenterPt.X,
                                                     p.Y - newCenterPt.Y), _ffRadius);
            var pos = _ffCurrpos >> SingleFP.DecimalBits - RatioBits;

            switch (FillMode)
            {
                case Reflect:
                    pos = pos%(RatioMax*2);
                    pos = pos < 0 ? pos + RatioMax*2 : pos;
                    pos = (pos < RatioMax) ? pos : RatioMax*2 - pos;
                    break;
                case Repeat:
                    pos = pos%RatioMax;
                    pos = pos < 0 ? pos + RatioMax : pos;
                    break;
                case NoCycle:
                    pos = pos < 0 ? 0 : (pos > RatioMax ? RatioMax : pos);
                    break;
            }

            return _gradientColors[pos];
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the next color for this brush.
        /// </summary>
        /// <returns>the next color</returns>
        public override int GetNextColor()
        {
            var p = new PointFP(_nextPt);
            _nextPt.X = p.X + SingleFP.One;
            _nextPt.Y = p.Y;
            var newCenterPt = new PointFP(_centerPt);
            if (_finalMatrix != null)
            {
                p.Transform(_finalMatrix);
                //newCenterPt.transform(finalMatrix);
            }
            _ffCurrpos = MathFP.Div(PointFP.Distance(p.X - newCenterPt.X,
                                                     p.Y - newCenterPt.Y), _ffRadius);
            var pos = _ffCurrpos >> SingleFP.DecimalBits - RatioBits;

            switch (FillMode)
            {
                case Reflect:
                    pos = pos%(RatioMax*2);
                    pos = pos < 0 ? pos + RatioMax*2 : pos;
                    pos = (pos < RatioMax) ? pos : RatioMax*2 - pos;
                    break;
                case Repeat:
                    pos = pos%RatioMax;
                    pos = pos < 0 ? pos + RatioMax : pos;
                    break;
                case NoCycle:
                    pos = pos < 0 ? 0 : (pos > RatioMax ? RatioMax : pos);
                    break;
            }

            return _gradientColors[pos];
        }

        private const int RatioBits = 10;
        private const int RatioMax = (1 << RatioBits) - 1;

        private readonly int[] _gradientColors = new int[1 << RatioBits];
        private int[] _ratios = new int[64];
        private int _ratioCount;
        private readonly int _ffRadius;
        private int _ffCurrpos;
        protected PointFP _centerPt = new PointFP();
        private readonly PointFP _nextPt = new PointFP(0, 0);
    }
}