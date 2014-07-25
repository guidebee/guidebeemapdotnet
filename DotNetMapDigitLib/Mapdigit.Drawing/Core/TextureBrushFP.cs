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
    /// Defines a brush of a single color. Brushes are used to fill graphics shapes,
    /// such as rectangles, ellipses, pies, polygons, and paths.
    /// </summary>
    internal sealed class TextureBrushFP : BrushFP
    {
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
        /// Initializes a new instance of the <see cref="TextureBrushFP"/> class.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public TextureBrushFP(int[] image, int width, int height)
        {
            _textureBuffer = new int[image.Length];
            Array.Copy(image, 0, _textureBuffer, 0, _textureBuffer.Length);
            _width = width;
            _height = height;
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
            if (_finalMatrix != null)
            {
                p.Transform(_finalMatrix);
            }
            var xPos = (p.X >> SingleFP.DecimalBits)%_width;
            var yPos = (p.Y >> SingleFP.DecimalBits)%_height;

            if (xPos < 0) xPos += _width;
            if (yPos < 0) yPos += _height;


            return _textureBuffer[(xPos + yPos*_width)];
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

            _nextPt.X += SingleFP.One;
            _nextPt.Y = p.Y;

            if (_finalMatrix != null)
            {
                p.Transform(_finalMatrix);
            }
            var xPos = (p.X >> SingleFP.DecimalBits)%_width;
            var yPos = (p.Y >> SingleFP.DecimalBits)%_height;

            if (xPos < 0) xPos += _width;
            if (yPos < 0) yPos += _height;

            return _textureBuffer[xPos + yPos*_width];
        }


        /**
         * the width of the texture
         */
        private readonly int _width = 1;

        /**
         * the height of the texture brush
         */
        private readonly int _height = 1;

        /**
         * the texture buffer
         */
        private readonly int[] _textureBuffer;

        /**
         * next point position.
         */
        private readonly PointFP _nextPt = new PointFP(0, 0);
    }
}