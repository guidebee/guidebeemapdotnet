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
    /// The RadialGradientBrush class provides a way to fill a shape with
    /// a circular radial color gradient pattern. 
    /// </summary>
    /// <remarks>
    /// The user may specify 2 or more gradient colors, and this paint will
    /// provide an interpolation between each color.
    /// The user must specify the circle controlling the gradient pattern,
    /// which is described by a center point and a radius.  The user can also
    /// specify a separate focus point within that circle, which controls the
    /// location of the first color of the gradient.  By default the focus is
    /// set to be the center of the circle.
    /// This paint will map the first color of the gradient to the focus point,
    /// and the last color to the perimeter of the circle, interpolating
    /// smoothly for any in-between colors specified by the user.  Any line drawn
    /// from the focus point to the circumference will thus span all the gradient
    /// colors.
    /// Specifying a focus point outside of the circle's radius will result in the
    /// focus being set to the intersection point of the focus-center line and the
    /// perimeter of the circle.
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
    /// The user may also select what action the RadialGradientBrush
    /// should take when filling color outside the bounds of the circle's radius.
    /// If no cycle method is specified, NoCycle will be chosen by
    /// default, which means the the last keyframe color will be used to fill the
    /// remaining area.
    /// The following code demonstrates typical usage of
    /// RadialGradientBrush, where the center and focus points are
    /// the same:
    /// <pre>
    ///     Point center = new Point(50, 50);
    ///     int radius = 25;
    ///     int[] dist = {0, 52, 255};
    ///     Color[] colors = {Color.RED, Color.WHITE, Color.BLUE};
    ///     RadialGradientBrush p =
    ///         new RadialGradientBrush(center, radius, dist, colors);
    /// </pre> 
    /// </remarks>
    public sealed class RadialGradientBrush : Brush
    {
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a RadialGradientBrush with a default
        /// NoCycle repeating method and SRGB color space,
        /// using the center as the focus point.
        /// </summary>
        /// <param name="cx">the X coordinate in user space of the center point of the
        ///           circle defining the gradient.  The last color of the
        ///           gradient is mapped to the perimeter of this circle.</param>
        /// <param name="cy">the Y coordinate in user space of the center point of the
        ///           circle defining the gradient.  The last color of the
        ///           gradient is mapped to the perimeter of this circle.</param>
        /// <param name="radius">the radius of the circle defining the extents of the
        ///                color gradient</param>
        /// <param name="fractions">numbers ranging from 0.0 to 1.0 specifying the
        ///                   distribution of colors along the gradient</param>
        /// <param name="colors">array of colors to use in the gradient.  The first color
        ///               is used at the focus point, the last color around the
        ///               perimeter of the circle.</param>
        public RadialGradientBrush(int cx, int cy, int radius,
                                   int[] fractions, Color[] colors)
            : this(new Point(cx, cy),
                   radius,
                   fractions,
                   colors,
                   NoCycle, new AffineTransform())
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a RadialGradientBrush with a default
        /// NoCycle repeating method and SRGB color space,
        /// using the center as the focus point.
        /// </summary>
        /// <param name="center">the center point, in user space, of the circle defining
        ///                the gradient</param>
        /// <param name="radius">the radius of the circle defining the extents of the
        ///                color gradient.</param>
        /// <param name="fractions">numbers ranging from 0.0 to 1.0 specifying the
        ///                   distribution of colors along the gradient</param>
        /// <param name="colors">array of colors to use in the gradient.  The first color
        ///               is used at the focus point, the last color around the
        ///               perimeter of the circle.</param>
        public RadialGradientBrush(Point center, int radius,
                                   int[] fractions, Color[] colors)
            : this(center,
                   radius,
                   fractions,
                   colors,
                   NoCycle, new AffineTransform())
        {
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="RadialGradientBrush"/> class.
        /// </summary>
        /// <param name="center">the center point in user space of the circle defining the
        ///               gradient.  The last color of the gradient is mapped to
        ///               the perimeter of this circle.</param>
        /// <param name="radius">the radius of the circle defining the extents of the
        ///                color gradient.</param>
        /// <param name="fractions">numbers ranging from 0.0 to 1.0 specifying the
        ///                   distribution of colors along the gradient</param>
        /// <param name="colors">array of colors to use in the gradient.  The first color
        ///               is used at the focus point, the last color around the
        ///               perimeter of the circle.</param>
        /// <param name="fillType">either NoCycle,Reflect,
        ///                     or Repeat</param>
        /// <param name="gradientTransform">ransform to apply to the gradient.</param>
        public RadialGradientBrush(Point center,
                                   int radius,
                                   int[] fractions, Color[] colors,
                                   int fillType,
                                   AffineTransform gradientTransform)
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

            // determine transparency
            bool opaque = true;
            for (int i = 0; i < colors.Length; i++)
            {
                opaque = opaque && (colors[i].A == 0xff);
            }
            _transparency = opaque ? Color.Opaque : Color.Translucent;


            // check input arguments
            if (center == null)
            {
                throw new NullReferenceException("Center point must be non-null");
            }


            if (radius <= 0)
            {
                throw new ArgumentException("Radius must be greater " +
                                            "than zero");
            }

            // copy parameters
            _center = new Point(center.X, center.Y);
            _radius = radius;
            _fillType = fillType;


            _wrappedBrushFp = new RadialGradientBrushFP(SingleFP.FromInt(center.X),
                                                        SingleFP.FromInt(center.Y), SingleFP.FromInt(radius), 0);
            for (int i = 0; i < colors.Length; i++)
            {
                ((RadialGradientBrushFP) _wrappedBrushFp).SetGradientColor(SingleFP.FromFloat(fractions[i]/100.0f),
                                                                           colors[i]._value);
            }
            ((RadialGradientBrushFP) _wrappedBrushFp).UpdateGradientTable();
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
        /// Returns a copy of the center point of the radial gradient.
        /// </summary>
        public Point CenterPoint
        {
            get { return new Point(_center.X, _center.Y); }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the radius of the circle defining the radial gradient
        /// </summary>
        public int Radius
        {
            get { return _radius; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// get fill type of the radial gradient brush
        /// </summary>
        public int FillType
        {
            get { return _fillType; }
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
        /// </summary>
        /// <remarks>
        /// The returned array always has 0 as its first value and 1 as its
        /// last value, with increasing values in between.
        /// </remarks>
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
        /// </summary>
        /// <remarks>
        /// The first color maps to the first value in the fractions array,
        /// and the last color maps to the last value in the fractions array.
        /// </remarks>
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
        /// </summary>
        /// <returns>
        /// this Color object's transparency mode.
        /// </returns>
        public override int Transparency
        {
            get { return _transparency; }
        }

        /** Center of the circle defining the 100% gradient stop X coordinate. */
        private readonly Point _center;
        /** Radius of the outermost circle defining the 100% gradient stop. */
        private readonly int _radius;
        private readonly int _fillType;
        /** Gradient keyframe values in the range 0 to 1. */
        private readonly int[] _fractions;
        /** Gradient colors. */
        private readonly Color[] _colors;
        /** The transparency of this paint object. */
        private readonly int _transparency;
    }
}