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
    /// Classes derived from this abstract base class define objects used to fill the
    /// interiors of graphical shapes such as rectangles, ellipses, pies, polygons,
    /// and paths.
    /// </summary>
    public abstract class Brush
    {
        ///<summary>
        /// Use the terminal colors to fill the remaining area.
        ///</summary>
        public const int NoCycle = BrushFP.NoCycle;

        ///<summary>
        /// Cycle the gradient colors start-to-end, end-to-start
        /// to fill the remaining area.
        ///</summary>
        public const int Reflect = BrushFP.Reflect;

        ///<summary>
        /// Cycle the gradient colors start-to-end, start-to-end
        /// to fill the remaining area.
        ///</summary>
        public const int Repeat = BrushFP.Repeat;


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get/Set the matrix associated with this brush.
        /// </summary>
        public AffineTransform Matrix
        {
            get { return Utils.ToMatrix(_wrappedBrushFp.GetMatrix()); }
            set { _wrappedBrushFp.SetMatrix(Utils.ToMatrixFP(value)); }
        }

       
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Transform with a new matrix.
        /// </summary>
        /// <param name="m1">a new matrix</param>
        public void Transform(AffineTransform m1)
        {
            _wrappedBrushFp.Transform(Utils.ToMatrixFP(m1));
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
        public abstract int Transparency { get; }

        /**
         * internal wrapped brush in the drawing core package.
         */
        internal BrushFP _wrappedBrushFp;
    }
}