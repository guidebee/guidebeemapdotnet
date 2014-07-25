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
    /// This class actually renders path in memory.
    /// </summary>
    internal class GraphicsPathRendererFP : GraphicsPathSketchFP
    {
        /**
         * paint mode XOR
         */
        public const int ModeXor = 1;
        /**
         * paint mode ZERO.(copy)
         */
        public const int ModeZero = 2;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsPathRendererFP"/> class.
        /// </summary>
        public GraphicsPathRendererFP()
            : this(1, 1)
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsPathRendererFP"/> class.
        /// </summary>
        /// <param name="width">the width for the drawing canvas</param>
        /// <param name="height">the height for the drawing cavas.</param>
        public GraphicsPathRendererFP(int width, int height)
        {
            _ffXmin = SingleFP.MaxValue;
            _ffXmax = SingleFP.MinValue;
            _ffYmin = SingleFP.MaxValue;
            _ffYmax = SingleFP.MinValue;
            Reset(width, height, width);
            _scanbuf = new int[4096];
            _scanbufTmp = new int[4096];
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the width of graphics object.
        /// </summary>
        /// <returns>the width of graphics object</returns>
        public int GetWidth()
        {
            return _width;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the height of graphics object.
        /// </summary>
        /// <returns>the height of graphics object.</returns>
        public int GetHeight()
        {
            return _height;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="style">The style.</param>
        /// <param name="mode">The mode.</param>
        public void DrawPath(GraphicsPathFP path, BrushFP style, int mode)
        {
            _scanIndex = 0;
            _drawMode = mode;
            path.Visit(this);
            RadixSort(_scanbuf, _scanbufTmp, _scanIndex);
            _fillStyle = style;
            if (_transformMatrix != null)
            {
                _fillStyle.SetGraphicsMatrix(_transformMatrix);
            }
            DrawBuffer();
            _fillStyle = null;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="matrix">The matrix.</param>
        /// <param name="fs">The fs.</param>
        /// <param name="mode">The mode.</param>
        public void DrawPath(GraphicsPathFP path, MatrixFP matrix,
                             BrushFP fs, int mode)
        {
            _transformMatrix = matrix;
            DrawPath(path, fs, mode);
            _transformMatrix = null;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Resets the specified width.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="scanline">The scanline.</param>
        public void Reset(int width, int height, int scanline)
        {
            _buffer = new int[width*height];
            _width = width;
            _height = height;
            SetClip(0, 0, width, height);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Clears the specified color.
        /// </summary>
        /// <param name="color">The color.</param>
        public void Clear(int color)
        {
            _backGroundColor = color;
            for (int i = 0; i < _buffer.Length; i++)
            {
                _buffer[i] = color;
            }
            SetClip(0, 0, _width, _height);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Finalizes the buffer.
        /// </summary>
        /// <param name="color">The color.</param>
        public void FinalizeBuffer(int color)
        {
            _backGroundColor = color;
            var bk = ColorFP.FromArgb(color);
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    if (ClipContains(x, y))
                    {
                        var c = ColorFP.FromArgb(_buffer[x + y*_width]);
                        if (c.Alpha != 0x00)
                        {
                            if (c.Alpha != 0xFF)
                            {
                                _buffer[x + y*_width] = ColorFP.FromArgb(
                                    (c.Red*c.Alpha +
                                     (0xFF - c.Alpha)*bk.Red) >> 8,
                                    (c.Green*c.Alpha
                                     + (0xFF - c.Alpha)*bk.Green) >> 8,
                                    (c.Blue*c.Alpha
                                     + (0xFF - c.Alpha)*bk.Blue) >> 8).Value;
                            }
                        }
                        else
                        {
                            _buffer[x + y*_width] = color;
                        }
                    }
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
        /// Moves to given point.
        /// </summary>
        /// <param name="point">The point.</param>
        public override void MoveTo(PointFP point)
        {
            _transformedPoint = new PointFP(point);
            if (_transformMatrix != null)
            {
                _transformedPoint.Transform(_transformMatrix);
            }
            base.MoveTo(point);
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
            var pntTemp = new PointFP(point);

            _ffXmin = MathFP.Min(_ffXmin, CurrentPoint().X);
            _ffXmax = MathFP.Max(_ffXmax, point.X);
            _ffYmin = MathFP.Min(_ffYmin, CurrentPoint().Y);
            _ffYmax = MathFP.Max(_ffYmax, point.Y);

            if (_transformMatrix != null)
            {
                pntTemp.Transform(_transformMatrix);
            }

            Scanline(_transformedPoint.X, _transformedPoint.Y, pntTemp.X, pntTemp.Y);
            _transformedPoint = pntTemp;
            base.LineTo(point);
        }

        public void SetClip(int x,
                            int y,
                            int width,
                            int height)
        {
            _clipX = x;
            _clipY = y;
            _clipHeight = height;
            _clipWidth = width;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Check to see if this rectangle contains given point.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        private bool ClipContains(int x, int y)
        {
            return _clipX <= x && x <= _clipX + _clipWidth
                   && _clipY <= y && y <= _clipY + _clipHeight;
        }


        internal int[] _buffer;
        internal int _backGroundColor = 0x00FFFFFF;
        private const int RendererFracY = 4;
        private const int RendererFracX = 4;
        private const int RendererRealX = 12;
        private const int RendererRealY = 11;
        private const int Buffersize = 2048;
        private const int RendererRealXMask = (1 << RendererRealX) - 1;
        private const int RendererRealYMask = (1 << RendererRealY) - 1;
        private const int RendererFracXFactor = 1 << RendererFracX;
        private const int RendererFracXMask = (1 << RendererFracX) - 1;
        private MatrixFP _transformMatrix;
        private BrushFP _fillStyle;
        private static int[] _scanbuf;
        private static int[] _scanbufTmp;
        private static readonly int[] Counts = new int[256];
        private static readonly int[] Index = new int[256];
        private PointFP _transformedPoint;
        private int _width;
        private int _height;
        private int _drawMode = ModeXor;
        private int _scanIndex;
        private int _ffXmin;
        private int _ffXmax;
        private int _ffYmin;
        private int _ffYmax;

        internal int _clipX;
        internal int _clipY;
        internal int _clipWidth;
        internal int _clipHeight;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Radixes the sort.
        /// </summary>
        /// <param name="dataSrc">The data SRC.</param>
        /// <param name="dataTmp">The data TMP.</param>
        /// <param name="num">The num.</param>
        private static void RadixSort(int[] dataSrc, int[] dataTmp, int num)
        {
            int shift, i;
            var src = dataSrc;
            var dst = dataTmp;
            for (shift = 0; shift <= 24; shift += 8)
            {
                for (i = 0; i < 256; i++)
                {
                    Counts[i] = 0;
                }

                for (i = 0; i < num; i++)
                {
                    Counts[(src[i] >> shift) & 0xFF]++;
                }
                int indexnow = 0;
                for (i = 0; i < 256; i++)
                {
                    Index[i] = indexnow;
                    indexnow += Counts[i];
                }
                for (i = 0; i < num; i++)
                {
                    dst[Index[(src[i] >> shift) & 0xFF]++] = src[i];
                }
                var tmp = src;
                src = dst;
                dst = tmp;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the buffer.
        /// </summary>
        private void DrawBuffer()
        {
            var curd = 0;
            var cure = 0;
            var cura = 0;
            var cula = 0;
            var cury = 0;
            var curx = 0;
            var count = _scanIndex;
            for (int c = 0; c <= count; c++)
            {
                var curs = c == count ? 0 : _scanbuf[c];

                var newy = ((curs >> (RendererRealX + RendererFracX + 1))
                            & RendererRealYMask);
                var newx = ((curs >> (RendererFracX + 1)) & RendererRealXMask);
                if ((newx != curx) || (newy != cury))
                {
                    var alp = (256*cure)/(RendererFracY) +
                              (256*cula)/(RendererFracY
                                          *(RendererFracXFactor - 1)) +
                              (256*cura)/(RendererFracY
                                          *(RendererFracXFactor - 1));
                    if (alp != 0)
                    {
                        if (_drawMode == ModeXor)
                        {
                            alp = (alp & 0x100) != 0
                                      ? (0xFF - (alp & 0xFF))
                                      : (alp & 0xFF);
                        }
                        else
                        {
                            alp = MathFP.Min(255, MathFP.Abs(alp));
                        }
                        if (alp != 0)
                        {
                            MergePixels(curx, cury, 1, alp);
                        }
                    }
                    cure = curd;

                    if (newy == cury)
                    {
                        if (curd != 0)
                        {
                            alp = (256*curd)/RendererFracY;
                            if (alp != 0)
                            {
                                if (_drawMode == ModeXor)
                                {
                                    alp = (alp & 0x100) != 0
                                              ? (0xFF - (alp & 0xFF))
                                              : (alp & 0xFF);
                                }
                                else
                                {
                                    alp = MathFP.Min(255, MathFP.Abs(alp));
                                }
                                if (alp != 0)
                                {
                                    MergePixels(curx + 1, cury, newx - curx - 1, alp);
                                }
                            }
                        }
                    }
                    else
                    {
                        cury = newy;
                        curd = cure = 0;
                    }

                    curx = newx;
                    cura = cula = 0;
                }

                if ((curs & 1) != 0)
                {
                    curd++;
                    cula += ((~(curs >> 1)) & RendererFracXMask);
                }
                else
                {
                    curd--;
                    cura -= ((~(curs >> 1)) & RendererFracXMask);
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
        /// Scanlines the specified ff sx.
        /// </summary>
        /// <param name="ffSx">The ff sx.</param>
        /// <param name="ffSy">The ff sy.</param>
        /// <param name="ffEx">The ff ex.</param>
        /// <param name="ffEy">The ff ey.</param>
        private void Scanline(int ffSx, int ffSy, int ffEx, int ffEy)
        {
            var sx = ffSx >> (SingleFP.DecimalBits - RendererFracX);
            var ex = ffEx >> (SingleFP.DecimalBits - RendererFracX);
            var sy = (ffSy*RendererFracY) >> SingleFP.DecimalBits;
            var ey = (ffEy*RendererFracY) >> SingleFP.DecimalBits;
            var xmin = MathFP.Min(sx, ex);
            var xmax = MathFP.Max(sx, ex);
            var ymin = MathFP.Min(sy, ey);
            var ymax = MathFP.Max(sy, ey);
            var incx = ffSx < ffEx && ffSy < ffEy || ffSx >= ffEx
                                                     && ffSy >= ffEy
                           ? 1
                           : -1;
            var x = incx == 1 ? xmin : xmax;
            var dire = ffSy < ffEy ? 1 : 0;

            if (((ymin < 0) && (ymax < 0)) || ((ymin >= (_height*RendererFracY))
                                               && (ymax >= (_height*RendererFracY))))
            {
                return;
            }

            var n = MathFP.Abs(xmax - xmin);
            var d = MathFP.Abs(ymax - ymin);
            var i = d;

            ymax = MathFP.Min(ymax, _height*RendererFracY);

            for (var y = ymin; y < ymax; y++)
            {
                if (y >= 0)
                {
                    if (_scanIndex >= _scanbuf.Length)
                    {
                        var bufSize = _scanIndex/Buffersize;
                        if ((_scanIndex + 1)%Buffersize != 0)
                        {
                            bufSize += 1;
                        }
                        _scanbufTmp = new int[bufSize*Buffersize];
                        Array.Copy(_scanbuf, 0, _scanbufTmp, 0, _scanIndex);
                        _scanbuf = new int[bufSize*Buffersize];
                        Array.Copy(_scanbufTmp, 0, _scanbuf, 0, _scanIndex);
                    }
                    _scanbuf[_scanIndex++] = ((y/RendererFracY)
                                              << (RendererRealX + RendererFracX + 1))
                                             | (MathFP.Max(0, MathFP.Min((_width*
                                                                          RendererFracXFactor) - 1, x)) << 1) | dire;
                }
                i += n;
                if (i > d)
                {
                    var idivd = (i - 1)/d;
                    x += incx*idivd;
                    i -= d*idivd;
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
        /// Merges the pixels.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="count">The count.</param>
        /// <param name="opacity">The opacity.</param>
        private void MergePixels(int x, int y, int count, int opacity)
        {
            var isMonoColor = _fillStyle.IsMonoColor();
            var color = 0;
            if (isMonoColor)
            {
                color = _fillStyle.GetNextColor();
                color = ((((color >> 24) & 0xFF)*opacity) >> 8)
                        << 24 | color & 0xFFFFFF;
            }
            var lastBackColor = 0;
            var lastMergedColor = 0;
            for (var i = 0; i < count; i++)
            {
                if (ClipContains(x + i, y))
                {
                    var bkColor = _buffer[x + i + y*_width];
                    if (!isMonoColor)
                    {
                        color = i == 0
                                    ? _fillStyle.GetColorAt(x + i, y, count == 1)
                                    : _fillStyle.GetNextColor();
                        if (opacity != 0xFF)
                        {
                            color = ((((color >> 24) & 0xFF)*opacity) >> 8)
                                    << 24 | color & 0xFFFFFF;
                        }
                    }
                    if (lastBackColor == bkColor && isMonoColor)
                    {
                        _buffer[x + i + y*_width] = lastMergedColor;
                    }
                    else
                    {
                        _buffer[x + i + y*_width] = Merge(bkColor, color);
                        lastBackColor = bkColor;
                        lastMergedColor = _buffer[x + i + y*_width];
                    }
                }
            }
        }

        /////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Merges the specified color1.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <returns></returns>
        private static int Merge(int color1, int color2)
        {
            int a2 = (color2 >> 24) & 0xFF;
            if (a2 == 0xFF || color1 == 0x0)
            {
                return color2;
            }
            if (a2 == 0)
            {
                return color1;
            }
            var a1 = 0xFF - ((color1 >> 24) & 0xFF);
            var a3 = 0xFF - a2;
            var b1 = color1 & 0xFF;
            var g1 = (color1 >> 8) & 0xFF;
            var r1 = (color1 >> 16) & 0xFF;
            var b2 = color2 & 0xFF;
            var g2 = (color2 >> 8) & 0xFF;
            var r2 = (color2 >> 16) & 0xFF;

            var ca = (0xFF*0xFF - a1*a3) >> 8;
            var cr = (r1*a3 + r2*a2) >> 8;
            var cg = (g1*a3 + g2*a2) >> 8;
            var cb = (b1*a3 + b2*a2) >> 8;
            return ca << 24 | cr << 16 | cg << 8 | cb;
        }
    }
}