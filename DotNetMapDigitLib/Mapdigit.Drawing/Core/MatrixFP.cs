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
    /// affine matrix in fixed point format.
    /// </summary>
    internal class MatrixFP
    {
        /**
         * Scale X factor.
         */
        public int ScaleX; // ScaleX

        /**
         * Scale Y factor.
         */
        public int ScaleY; // ScaleY

        /**
         * Rotate/Shear X factor.
         */
        public int RotateX; // RotateSkewX

        /**
         * Rotate/Shear Y factor.
         */
        public int RotateY; // RotateSkewY

        /**
         * Translate X.
         */
        public int TranslateX; // TranslateX

        /**
         * Translate Y.
         */
        public int TranslateY; // TranslateY

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixFP"/> class.
        /// </summary>
        public MatrixFP()
        {
            ScaleX = ScaleY = SingleFP.One;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixFP"/> class.
        /// </summary>
        /// <param name="ffSx">The ff sx.</param>
        /// <param name="ffSy">The ff sy.</param>
        /// <param name="ffRx">The ff rx.</param>
        /// <param name="ffRy">The ff ry.</param>
        /// <param name="ffTx">The ff tx.</param>
        /// <param name="ffTy">The ff ty.</param>
        public MatrixFP(int ffSx, int ffSy, int ffRx, int ffRy,
                        int ffTx, int ffTy)
        {
            Reset(ffSx, ffSy, ffRx, ffRy, ffTx, ffTy);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixFP"/> class.
        /// </summary>
        /// <param name="m">The matrix to be copied.</param>
        public MatrixFP(MatrixFP m)
        {
            Reset(m.ScaleX, m.ScaleY, m.RotateX, m.RotateY, m.TranslateX, m.TranslateY);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// reset to identity matrix.
        /// </summary>
        public void Reset()
        {
            Reset(SingleFP.One, SingleFP.One, 0, 0, 0, 0);
        }

        /// <summary>
        /// Identity matrix
        /// </summary>
        public static MatrixFP Identity = new MatrixFP(SingleFP.One, SingleFP.One, 0, 0, 0, 0);


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determines whether this instance is identity.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance is identity; otherwise, <c>false</c>.
        /// </returns>
        public bool IsIdentity()
        {
            return ScaleX == SingleFP.One && ScaleY == SingleFP.One && RotateX == 0
                   && RotateY == 0 && TranslateX == 0 && TranslateY == 0;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determines whether this instance is invertible.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance is invertible; otherwise, <c>false</c>.
        /// </returns>
        public bool IsInvertible()
        {
            return Determinant() != 0;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Resets the matrix with given value.
        /// </summary>
        /// <param name="ffSx">The ff sx.</param>
        /// <param name="ffSy">The ff sy.</param>
        /// <param name="ffRx">The ff rx.</param>
        /// <param name="ffRy">The ff ry.</param>
        /// <param name="ffTx">The ff tx.</param>
        /// <param name="ffTy">The ff ty.</param>
        public void Reset(int ffSx, int ffSy, int ffRx, int ffRy,
                          int ffTx, int ffTy)
        {
            ScaleX = ffSx;
            ScaleY = ffSy;
            RotateX = ffRx;
            RotateY = ffRy;
            TranslateX = ffTx;
            TranslateY = ffTy;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Rotates the specified ang.
        /// </summary>
        /// <param name="ffAng">The ff ang.</param>
        /// <returns></returns>
        public MatrixFP Rotate(int ffAng)
        {
            var ffSin = MathFP.Sin(ffAng);
            var ffCos = MathFP.Cos(ffAng);
            return Multiply(new MatrixFP(ffCos, ffCos, ffSin, -ffSin, 0, 0));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Shear or rotate operation.
        /// </summary>
        /// <param name="ffRx">The ff rx.</param>
        /// <param name="ffRy">The ff ry.</param>
        /// <returns></returns>
        public MatrixFP RotateSkew(int ffRx, int ffRy)
        {
            return Multiply(new MatrixFP(SingleFP.One, SingleFP.One, ffRx, ffRy, 0, 0));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// translate operation.
        /// </summary>
        /// <param name="ffDx">The ff dx.</param>
        /// <param name="ffDy">The ff dy.</param>
        /// <returns></returns>
        public MatrixFP Translate(int ffDx, int ffDy)
        {
            TranslateX += ffDx;
            TranslateY += ffDy;
            return this;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Scale operation.
        /// </summary>
        /// <param name="ffSx">The ff sx.</param>
        /// <param name="ffSy">The ff sy.</param>
        /// <returns></returns>
        public MatrixFP Scale(int ffSx, int ffSy)
        {
            Reset(MathFP.Mul(ffSx, ScaleX), MathFP.Mul(ffSy, ScaleY),
                  MathFP.Mul(ffSy, RotateX), MathFP.Mul(ffSx, RotateY),
                  MathFP.Mul(ffSx, TranslateX), MathFP.Mul(ffSy, TranslateY));
            return this;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// multipy with another matrix.
        /// </summary>
        /// <param name="m">The m.</param>
        /// <returns></returns>
        public MatrixFP Multiply(MatrixFP m)
        {
            Reset(MathFP.Mul(m.ScaleX, ScaleX) + MathFP.Mul(m.RotateY, RotateX),
                  MathFP.Mul(m.RotateX, RotateY) + MathFP.Mul(m.ScaleY, ScaleY),
                  MathFP.Mul(m.RotateX, ScaleX) + MathFP.Mul(m.ScaleY, RotateX),
                  MathFP.Mul(m.ScaleX, RotateY) + MathFP.Mul(m.RotateY, ScaleY),
                  MathFP.Mul(m.ScaleX, TranslateX) + MathFP.Mul(m.RotateY, TranslateY) + m.TranslateX,
                  MathFP.Mul(m.RotateX, TranslateX) + MathFP.Mul(m.ScaleY, TranslateY) + m.TranslateY);
            return this;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public new bool Equals(object obj)
        {
            if (obj is MatrixFP)
            {
                var m = (MatrixFP) obj;
                return m.RotateX == RotateX && m.RotateY == RotateY
                       && m.ScaleX == ScaleX && m.ScaleY == ScaleY
                       && m.TranslateX == TranslateX && m.TranslateY == TranslateY;
            }
            return false;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms 
        /// and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return RotateX << 24 + RotateY << 20 + ScaleX << 16
                                                             + ScaleY << 8 + TranslateX << 4 + TranslateY;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// calculat the determinat of the matrix.
        /// </summary>
        /// <returns></returns>
        private int Determinant()
        {
            var ffDet = MathFP.Mul(ScaleX, ScaleY) - MathFP.Mul(RotateX, RotateY);
            return ffDet;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Inverts this instance.
        /// </summary>
        /// <returns></returns>
        public MatrixFP Invert()
        {
            int ffDet = Determinant();
            if (ffDet == 0)
            {
                Reset();
            }
            else
            {
                var ffSxNew = MathFP.Div(ScaleY, ffDet);
                var ffSyNew = MathFP.Div(ScaleX, ffDet);
                var ffRxNew = -MathFP.Div(RotateX, ffDet);
                var ffRyNew = -MathFP.Div(RotateY, ffDet);
                var ffTxNew = MathFP.Div(MathFP.Mul(TranslateY, RotateY)
                                         - MathFP.Mul(TranslateX, ScaleY), ffDet);
                var ffTyNew = -MathFP.Div(MathFP.Mul(TranslateY, ScaleX)
                                          - MathFP.Mul(TranslateX, RotateX), ffDet);
                Reset(ffSxNew, ffSyNew, ffRxNew, ffRyNew,
                      ffTxNew, ffTyNew);
            }
            return this;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return " Matrix(sx,sy,rx,ry,tx,ty)=(" + new SingleFP(ScaleX)
                   + "," + new SingleFP(ScaleY) + "," + new SingleFP(RotateX)
                   + "," + new SingleFP(RotateY) + "," + new SingleFP(TranslateX)
                   + "," + new SingleFP(TranslateY) + ")";
        }
    }
}