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
using Mapdigit.Drawing.Geometry;
using Mapdigit.Drawing.Core;
using Mapdigit.Util;
using MathFP = Mapdigit.Drawing.Core.MathFP;

//--------------------------------- PACKAGE ------------------------------------

namespace Mapdigit.Drawing
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 14OCT2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// The LinearGradientBrush class provides a way to fill a IShape with a 
    /// linear color gradient pattern. 
    /// </summary>
    /// <remarks>
    /// The user may specify two or more gradient colors, and this brush will 
    /// provide an interpolation between each color.  The user also specifies start
    /// and end points which define where in user space the color gradient should 
    /// begin and end.
    /// The user must provide an array of integers specifying how to distribute the
    /// colors along the gradient.  These values should range from 0 to 255 and 
    /// act like keyframes along the gradient (they mark where the gradient should 
    /// be exactly a particular color).
    /// In the event that the user does not set the first keyframe value equal
    /// to 0 and/or the last keyframe value equal to 255, keyframes will be created
    /// at these positions and the first and last colors will be replicated there.
    /// So, if a user specifies the following arrays to construct a gradient:
    /// <pre>
    ///     {Color.BLUE, Color.RED}, {100, 140}
    /// </pre>
    /// this will be converted to a gradient with the following keyframes:
    /// <pre>
    ///     {Color.BLUE, Color.BLUE, Color.RED, Color.RED}, {0, 100, 140, 255}
    /// </pre>
    /// The user may also select what action the {@code LinearGradientBrush}
    /// should take when filling color outside the start and end points.
    /// If no cycle method is specified, NoCycle will be chosen by
    /// default, which means the endpoint colors will be used to fill the
    /// remaining area.
    /// The following code demonstrates typical usage of
    /// LinearGradientBrush:
    /// <code>
    ///     Point start = new Point(0, 0);
    ///     Point end = new Point(50, 50);
    ///     int[] dist = {0, 100f, 255};
    ///     Color[] colors = {Color.RED, Color.WHITE, Color.BLUE};
    ///     LinearGradientBrush p =
    ///     new LinearGradientBrush(start, end, dist, colors);
    /// </code>
    /// This code will create a LinearGradientBrush which interpolates
    /// between red and white for the first 20% of the gradient and between white
    /// and blue for the remaining 80%.
    /// </remarks>
    public sealed class LinearGradientBrush : Brush
    {
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a LinearGradientBrush with a default 
        /// NoCycle repeating method and SRGB color space.
        /// </summary>
        /// <param name="startX">the X coordinate of the gradient axis start point 
        ///               in user space.</param>
        /// <param name="startY">the Y coordinate of the gradient axis start point 
        ///                in user space.</param>
        /// <param name="endX">the X coordinate of the gradient axis end point 
        ///                in user space.</param>
        /// <param name="endY">the Y coordinate of the gradient axis end point 
        ///                in user space.</param>
        /// <param name="fractions">numbers ranging from 0 to 255 specifying the 
        ///                   distribution of colors along the gradient.</param>
        /// <param name="colors">array of colors corresponding to each fractional value.</param>
        /// <param name="fillType">Type of the fill.</param>
        public LinearGradientBrush(int startX, int startY,
                                   int endX, int endY,
                                   int[] fractions, Color[] colors, int fillType)
            : this(new Point(startX, startY),
                   new Point(endX, endY),
                   fractions,
                   colors, fillType)
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a LinearGradientBrush with a default
        /// NoCycle repeating method and SRGBcolor space.
        /// </summary>
        /// <param name="start">the gradient axis start Point in user space</param>
        /// <param name="end">the gradient axis end Point in user space</param>
        /// <param name="fractions">numbers ranging from 0 to 255 specifying the 
        ///                   distribution of colors along the gradient.</param>
        /// <param name="colors">array of colors corresponding to each fractional value.</param>
        public LinearGradientBrush(Point start, Point end,
                                   int[] fractions, Color[] colors)
            : this(start, end,
                   fractions, colors,
                   NoCycle)
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a LinearGradientBrush with a default
        /// NoCycle repeating method and SRGB color space.
        /// </summary>
        /// <param name="start">the gradient axis start Point in user space</param>
        /// <param name="end">the gradient axis end Point in user space</param>
        /// <param name="fractions">numbers ranging from 0 to 255 specifying the 
        ///                   distribution of colors along the gradient.</param>
        /// <param name="colors">array of colors corresponding to each fractional value.</param>
        /// <param name="fillType">Type of the fill.</param>
        public LinearGradientBrush(Point start, Point end,
                                   int[] fractions, Color[] colors,
                                   int fillType)
            : this(start, end,
                   fractions, colors,
                   new AffineTransform(), fillType)
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a LinearGradientBrush with two colors and angle.
        /// </summary>
        /// <param name="rect">rectangle area of the linear gradien brush.</param>
        /// <param name="color1">start color</param>
        /// <param name="color2">end color.</param>
        /// <param name="angle">the anagle from start color to the end color.</param>
        public LinearGradientBrush(Rectangle rect, Color color1, Color color2,
                                   float angle)
            : this(rect, color1, color2, angle, NoCycle)
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a LinearGradientBrush with two colors and angle.
        /// </summary>
        /// <param name="rect">rectangle area of the linear gradien brush.</param>
        /// <param name="color1">start color</param>
        /// <param name="color2">end color.</param>
        /// <param name="angle">the anagle from start color to the end color.</param>
        /// <param name="fillType">Type of the fill.</param>
        public LinearGradientBrush(Rectangle rect, Color color1, Color color2,
                                   float angle, int fillType)
        {
            _start = new Point(rect.X, rect.Y);
            _end = new Point(rect.X + rect.Width, rect.Y + rect.Height);
            _gradientTransform = new AffineTransform();
            _fractions = new[] {0, 100};
            _colors = new[] {color1, color2};
            bool opaque = true;
            for (int i = 0; i < _colors.Length; i++)
            {
                opaque = opaque && (_colors[i].A == 0xff);
            }
            _transparency = opaque ? Color.Opaque : Color.Translucent;
            RectangleFP r = Utils.ToRectangleFP(rect);
            _wrappedBrushFp = new LinearGradientBrushFP(r.GetLeft(), r.GetTop(),
                                                        r.GetRight(), r.GetBottom(),
                                                        MathFP.ToRadians(SingleFP.FromFloat(angle)));
            for (int i = 0; i < _colors.Length; i++)
            {
                ((LinearGradientBrushFP) _wrappedBrushFp)
                    .SetGradientColor(SingleFP.FromFloat(_fractions[i]
                                                         /255.0f),
                                      _colors[i]._value);
            }
            ((LinearGradientBrushFP) _wrappedBrushFp).UpdateGradientTable();
            _wrappedBrushFp.SetMatrix(Utils.ToMatrixFP(_gradientTransform));
            _wrappedBrushFp.FillMode = fillType;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="LinearGradientBrush"/> class.
        /// </summary>
        /// <param name="start">the gradient axis start Point in user space</param>
        /// <param name="end">the gradient axis end Point in user space</param>
        /// <param name="fractions">numbers ranging from 0 to 255 specifying the 
        ///                   distribution of colors along the gradient.</param>
        /// <param name="colors">array of colors corresponding to each fractional value.</param>
        /// <param name="gradientTransform">transform to apply to the gradient</param>
        /// <param name="fillType">Type of the fill.</param>
        public LinearGradientBrush(Point start, Point end,
                                   int[] fractions, Color[] colors,
                                   AffineTransform gradientTransform, int fillType)
        {
            if (fractions == null)
            {
                throw new NullReferenceException("Fractions array cannot be null");
            }

            if (colors == null)
            {
                throw new NullReferenceException("Colors array cannot be null");
            }

            if (gradientTransform == null)
            {
                throw new NullReferenceException("Gradient transform cannot be " +
                                                 "null");
            }

            if (fractions.Length != colors.Length)
            {
                throw new ArgumentException("Colors and fractions must " +
                                            "have equal size");
            }

            if (colors.Length < 2)
            {
                throw new ArgumentException("User must specify at least " +
                                            "2 colors");
            }

            // check that values are in the proper range and progress
            // in increasing order from 0 to 1
            int previousFraction = -255;
            for (int i = 0; i < fractions.Length; i++)
            {
                int currentFraction = fractions[i];
                if (currentFraction < 0 || currentFraction > 255)
                {
                    throw new ArgumentException("Fraction values must " +
                                                "be in the range 0 to 255: " +
                                                currentFraction);
                }

                if (currentFraction <= previousFraction)
                {
                    throw new ArgumentException("Keyframe fractions " +
                                                "must be increasing: " +
                                                currentFraction);
                }

                previousFraction = currentFraction;
            }

            // We have to deal with the cases where the first gradient stop is not
            // equal to 0 and/or the last gradient stop is not equal to 1.
            // In both cases, create a new point and replicate the previous
            // extreme point's color.
            bool fixFirst = false;
            bool fixLast = false;
            int len = fractions.Length;
            int off = 0;

            if (fractions[0] != 0)
            {
                // first stop is not equal to zero, fix this condition
                fixFirst = true;
                len++;
                off++;
            }
            if (fractions[fractions.Length - 1] != 255)
            {
                // last stop is not equal to one, fix this condition
                fixLast = true;
                len++;
            }

            _fractions = new int[len];
            Array.Copy(fractions, 0, _fractions, off, fractions.Length);
            _colors = new Color[len];
            Array.Copy(colors, 0, _colors, off, colors.Length);

            if (fixFirst)
            {
                _fractions[0] = 0;
                _colors[0] = colors[0];
            }
            if (fixLast)
            {
                _fractions[len - 1] = 255;
                _colors[len - 1] = colors[colors.Length - 1];
            }

            // copy the gradient transform
            _gradientTransform = new AffineTransform(gradientTransform);

            // determine transparency
            bool opaque = true;
            for (int i = 0; i < colors.Length; i++)
            {
                opaque = opaque && (colors[i].A == 0xff);
            }
            _transparency = opaque ? Color.Opaque : Color.Translucent;

            // check input parameters
            if (start == null || end == null)
            {
                throw new NullReferenceException("Start and end points must be" +
                                                 "non-null");
            }

            if (start.Equals(end))
            {
                throw new ArgumentException("Start point cannot equal" +
                                            "endpoint");
            }

            // copy the points...
            _start = new Point(start.X, start.Y);
            _end = new Point(end.X, end.Y);

            Rectangle rectangle = new Rectangle(start, end);
            float dx = start.X - end.X;
            float dy = start.Y - end.Y;
            double angle = MathEx.Atan2(dy, dx);
            int intAngle = SingleFP.FromDouble(angle);
            RectangleFP r = Utils.ToRectangleFP(rectangle);
            _wrappedBrushFp = new LinearGradientBrushFP(r.GetLeft(), r.GetTop(),
                                                        r.GetRight(), r.GetBottom(),
                                                        intAngle);
            for (int i = 0; i < colors.Length; i++)
            {
                ((LinearGradientBrushFP) _wrappedBrushFp).SetGradientColor
                    (SingleFP.FromFloat(fractions[i]/100.0f),
                     colors[i]._value);
            }
            ((LinearGradientBrushFP) _wrappedBrushFp).UpdateGradientTable();
            _wrappedBrushFp.SetMatrix(Utils.ToMatrixFP(gradientTransform));
            _wrappedBrushFp.FillMode = fillType;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a copy of the start point of the gradient axis.
        /// </summary>
        public Point StartPoint
        {
            get { return new Point(_start.X, _start.Y); }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a copy of the end point of the gradient axis.
        /// </summary>
        public Point EndPoint
        {
            get { return new Point(_end.X, _end.Y); }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a copy of the array of floats used by this gradient
        /// to calculate color distribution.
        /// The returned array always has 0 as its first value and 1 as its
        /// last value, with increasing values in between.
        /// </summary>
        public int[] Fractions
        {
            get { return _fractions; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a copy of the array of colors used by this gradient.
        /// The first color maps to the first value in the fractions array,
        /// and the last color maps to the last value in the fractions array.
        /// </summary>
        public Color[] Colors
        {
            get { return _colors; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the transparency mode for this Color.
        ///  </summary>
        public override int Transparency
        {
            get { return _transparency; }
        }

        /** Gradient start and end points. */
        private readonly Point _start;
        private readonly Point _end;
        private readonly AffineTransform _gradientTransform;
        /** Gradient keyframe values in the range 0 to 1. */
        private readonly int[] _fractions;
        /** Gradient colors. */
        private readonly Color[] _colors;
        /** The transparency of this paint object. */
        private readonly int _transparency;
    }
}