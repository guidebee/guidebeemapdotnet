//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 26SEP2010  James Shen                 	          Code review
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
    // 26SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// utility functions to convert class from drawing to drawingfp.
    /// </summary>
    internal abstract class Utils
    {
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// from Affline Matrix to it's FP matrix
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns></returns>
        internal static MatrixFP ToMatrixFP(AffineTransform matrix)
        {
            if (matrix == null)
            {
                return null;
            }
            if (matrix.IsIdentity())
            {
                return MatrixFP.Identity;
            }

            MatrixFP matrixFP = new MatrixFP(SingleFP.FromDouble(matrix.ScaleX),
                                             SingleFP.FromDouble(matrix.ScaleY),
                                             SingleFP.FromDouble(-matrix.ShearX),
                                             SingleFP.FromDouble(-matrix.ShearY),
                                             SingleFP.FromDouble(matrix.TranslateX),
                                             SingleFP.FromDouble(matrix.TranslateY));
            return matrixFP;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// from FP matrix to affine matrix
        /// </summary>
        /// <param name="matrixFP">The matrix FP.</param>
        /// <returns></returns>
        internal static AffineTransform ToMatrix(MatrixFP matrixFP)
        {
            if (matrixFP == null)
            {
                return null;
            }
            if (matrixFP.IsIdentity())
            {
                return new AffineTransform();
            }
            AffineTransform matrix = new AffineTransform(SingleFP.ToDouble(matrixFP.ScaleX),
                                                         SingleFP.ToDouble(-matrixFP.RotateX),
                                                         SingleFP.ToDouble(-matrixFP.RotateY),
                                                         SingleFP.ToDouble(matrixFP.ScaleY),
                                                         SingleFP.ToDouble(matrixFP.TranslateX),
                                                         SingleFP.ToDouble(matrixFP.TranslateY));
            return matrix;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///from rectange to rectangeFP
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <returns></returns>
        internal static RectangleFP ToRectangleFP(Rectangle rect)
        {
            return new RectangleFP(
                SingleFP.FromInt(rect.IntMinX),
                SingleFP.FromInt(rect.IntMinY),
                SingleFP.FromInt(rect.IntMaxX),
                SingleFP.FromInt(rect.IntMaxY));
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///from point to pointFP
        /// </summary>
        /// <param name="pnt">The PNT.</param>
        /// <returns></returns>
        public static PointFP ToPointFP(Point pnt)
        {
            return new PointFP(SingleFP.FromInt(pnt.X), SingleFP.FromInt(pnt.Y));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// from PointFP to Point
        /// </summary>
        /// <param name="pnt">The PNT.</param>
        /// <returns></returns>
        public static Point ToPoint(PointFP pnt)
        {
            return new Point(SingleFP.ToInt(pnt.X), SingleFP.ToInt(pnt.Y));
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// from Point array to pintFP array
        /// </summary>
        /// <param name="pnts">The PNTS.</param>
        /// <returns></returns>
        internal static PointFP[] ToPointFPArray(Point[] pnts)
        {
            PointFP[] result = new PointFP[pnts.Length];
            for (int i = 0; i < pnts.Length; i++)
            {
                result[i] = ToPointFP(pnts[i]);
            }
            return result;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 26SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// from PointFP array to point array
        /// </summary>
        /// <param name="pnts">The PNTS.</param>
        /// <returns></returns>
        public static Point[] ToPointArray(PointFP[] pnts)
        {
            Point[] result = new Point[pnts.Length];
            for (int i = 0; i < pnts.Length; i++)
            {
                result[i] = ToPoint(pnts[i]);
            }
            return result;
        }
    }
}