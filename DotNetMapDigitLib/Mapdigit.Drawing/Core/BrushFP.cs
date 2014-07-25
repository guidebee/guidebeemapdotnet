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
    /// Defines objects used to fill the interiors of graphical shapes such as
    /// rectangles, ellipses, pies, polygons, and paths.
    /// </summary>
    internal abstract class BrushFP
    {
        /**
         * Use the terminal colors to fill the remaining area.
         */
        public const int NoCycle = 0;

        /**
         * Cycle the gradient colors start-to-end, end-to-start
         * to fill the remaining area.
         */
        public const int Reflect = 1;


        /**
         * Cycle the gradient colors start-to-end, start-to-end
         * to fill the remaining area.
         */
        public const int Repeat = 2;

        /**
         * Fill mode of the brush.
         */
        public int FillMode = Repeat;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the matrix associated with this brush
        /// </summary>
        /// <returns>the matrix</returns>
        public MatrixFP GetMatrix()
        {
            return _matrix;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set the matrix for this brush.
        /// </summary>
        /// <param name="value">value a new matrix.</param>
        public void SetMatrix(MatrixFP value)
        {
            _matrix = new MatrixFP(value);
            _matrix.Invert();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Transform with a new matrix.
        /// </summary>
        /// <param name="m1">a new matrix</param>
        public void Transform(MatrixFP m1)
        {
            var m = new MatrixFP(m1);
            m.Invert();
            if (_matrix == null)
            {
                _matrix = m;
            }
            else
            {
                _matrix.Multiply(m);
            }
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
        /// <returns>
        /// 	true if it's mono color brush.
        /// </returns>
        public abstract bool IsMonoColor();

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
        public abstract int GetColorAt(int x, int y, bool singlePoint);

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
        public abstract int GetNextColor();

        /**
         * Brush matrix.
         */
        protected MatrixFP _matrix;

        /**
         * Graphics matrix.
         */
        protected MatrixFP _graphicsMatrix;

        /**
         * The combined matrix.
         */
        protected MatrixFP _finalMatrix;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// get the matrix associated with the graphics object.
        /// </summary>
        /// <returns>the matrix of the graphics object</returns>
        internal MatrixFP GetGraphicsMatrix()
        {
            return _graphicsMatrix;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set the graphics matrix.
        /// </summary>
        /// <param name="value">the graphics matrix.</param>
        internal void SetGraphicsMatrix(MatrixFP value)
        {
            _graphicsMatrix = new MatrixFP(value);
            _graphicsMatrix.Invert();
            _finalMatrix = new MatrixFP(_graphicsMatrix);
            if (_matrix != null)
            {
                _finalMatrix.Multiply(_matrix);
            }
        }
    }
}