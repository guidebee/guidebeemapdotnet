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
    /// a 2D point class.
    /// </summary>
    internal class PointFP
    {
        /**
         * X coordinate.
         */
        public int X;

        /**
         * Y coordinate.
         */
        public int Y;

        /**
         * the (0,0) point.
         */
        public static readonly PointFP Origin = new PointFP(0, 0);

        /**
         * Empty point.
         */
        public static readonly PointFP Empty = new PointFP(SingleFP.NotANumber, SingleFP.NotANumber);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="PointFP"/> class.
        /// </summary>
        public PointFP()
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="PointFP"/> class.
        /// </summary>
        /// <param name="p">The p.</param>
        public PointFP(PointFP p)
        {
            Reset(p);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="PointFP"/> class.
        /// </summary>
        /// <param name="ffX">The ff X.</param>
        /// <param name="ffY">The ff Y.</param>
        public PointFP(int ffX, int ffY)
        {
            Reset(ffX, ffY);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determines whether the specified point is empty.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>
        /// 	<c>true</c> if the specified p is empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEmpty(PointFP p)
        {
            return Empty.Equals(p);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// reset the point to the same location as the given point.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        public PointFP Reset(PointFP p)
        {
            return Reset(p.X, p.Y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// reset the point to give location.
        /// </summary>
        /// <param name="ffX">The ff X.</param>
        /// <param name="ffY">The ff Y.</param>
        /// <returns></returns>
        public PointFP Reset(int ffX, int ffY)
        {
            X = ffX;
            Y = ffY;
            return this;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// transform the point with give matrix.
        /// </summary>
        /// <param name="m">The m.</param>
        /// <returns></returns>
        public PointFP Transform(MatrixFP m)
        {
            Reset(MathFP.Mul(X, m.ScaleX) + MathFP.Mul(Y, m.RotateY) + m.TranslateX,
                  MathFP.Mul(Y, m.ScaleY) + MathFP.Mul(X, m.RotateX) + m.TranslateY);
            return this;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// the distance between 2 points.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <returns></returns>
        public static int Distance(PointFP p1, PointFP p2)
        {
            return Distance(p1.X - p2.X, p1.Y - p2.Y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// calculate the distance.
        /// </summary>
        /// <param name="dx">The dx.</param>
        /// <param name="dy">The dy.</param>
        /// <returns></returns>
        public static int Distance(int dx, int dy)
        {
            dx = MathFP.Abs(dx);
            dy = MathFP.Abs(dy);
            if (dx == 0)
            {
                return dy;
            }
            if (dy == 0)
            {
                return dx;
            }

            var len = (((long) dx*dx) >> SingleFP.DecimalBits)
                      + (((long) dy*dy) >> SingleFP.DecimalBits);
            long s = (dx + dy) - (MathFP.Min(dx, dy) >> 1);
            s = (s + ((len << SingleFP.DecimalBits)/s)) >> 1;
            s = (s + ((len << SingleFP.DecimalBits)/s)) >> 1;
            return (int) s;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Add given point the location to this point.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        public PointFP Add(PointFP p)
        {
            Reset(X + p.X, Y + p.Y);
            return this;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  substract given distance (x,y) to this point.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        public PointFP Sub(PointFP p)
        {
            Reset(X - p.X, Y - p.Y);
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
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal 
        /// to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            if (obj is PointFP)
            {
                var p = (PointFP) obj;
                return X == p.X && Y == p.Y;
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
            var bits = (int) ((X << 16) & 0xFFFF0000);
            bits ^= Y;
            return bits;
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
            return "Point(" + new SingleFP(X) + "," + new SingleFP(Y) + ")";
        }
    }
}