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
    /// a 2D Line class.
    /// </summary>
    internal class LineFP
    {
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// get the lenght of the line.
        /// </summary>
        /// <returns></returns>
        public int GetLength()
        {
            return PointFP.Distance(Pt1, Pt2);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the center of the line.
        /// </summary>
        /// <returns></returns>
        public PointFP GetCenter()
        {
            return new PointFP((Pt1.X + Pt2.X)/2, (Pt1.Y + Pt2.Y)/2);
        }

        /**
         * start point the line.
         */
        public PointFP Pt1 = new PointFP(0, 0);

        /**
         * end point of the line.
         */
        public PointFP Pt2 = new PointFP(0, 0);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="LineFP"/> class.
        /// </summary>
        public LineFP()
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="LineFP"/> class.
        /// </summary>
        /// <param name="l">The line to be copied.</param>
        public LineFP(LineFP l)
        {
            Reset(l.Pt1, l.Pt2);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="LineFP"/> class.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        public LineFP(PointFP p1, PointFP p2)
        {
            Reset(p1, p2);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="LineFP"/> class.
        /// </summary>
        /// <param name="ffX1">The ff x1.</param>
        /// <param name="ffY1">The ff y1.</param>
        /// <param name="ffX2">The ff x2.</param>
        /// <param name="ffY2">The ff y2.</param>
        public LineFP(int ffX1, int ffY1, int ffX2, int ffY2)
        {
            Reset(ffX1, ffY1, ffX2, ffY2);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Resets the specified line.
        /// </summary>
        /// <param name="l">The l.</param>
        public void Reset(LineFP l)
        {
            Reset(l.Pt1, l.Pt2);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Resets the specified line with given point.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        public void Reset(PointFP p1, PointFP p2)
        {
            Pt1.Reset(p1);
            Pt2.Reset(p2);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Resets the specified line with givne coordinates.
        /// </summary>
        /// <param name="ffX1">The ff x1.</param>
        /// <param name="ffY1">The ff y1.</param>
        /// <param name="ffX2">The ff x2.</param>
        /// <param name="ffY2">The ff y2.</param>
        public void Reset(int ffX1, int ffY1, int ffX2, int ffY2)
        {
            Pt1.Reset(ffX1, ffY1);
            Pt2.Reset(ffX2, ffY2);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the head outline.
        /// </summary>
        /// <param name="ffRad">The ff RAD.</param>
        /// <returns></returns>
        public LineFP GetHeadOutline(int ffRad)
        {
            var p = new PointFP(Pt1.X - Pt2.X, Pt1.Y - Pt2.Y);
            var len = GetLength();
            if (len != 0)
            {
                p.Reset(MathFP.Div(-p.Y, len), MathFP.Div(p.X, len));
                p.Reset(MathFP.Mul(p.X, ffRad), MathFP.Mul(p.Y, ffRad));
            }
            return new LineFP(Pt1.X - p.X, Pt1.Y - p.Y, Pt1.X + p.X, Pt1.Y + p.Y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the tail outline.
        /// </summary>
        /// <param name="ffRad">The ff RAD.</param>
        /// <returns></returns>
        public LineFP GetTailOutline(int ffRad)
        {
            var c = GetCenter();
            var p = new PointFP(Pt2.X - c.X, Pt2.Y - c.Y);
            p.Reset(p.Y, -p.X);
            var dis = PointFP.Distance(PointFP.Origin, p);
            if (dis == 0)
            {
                dis = 1;
            }
            p.Reset(MathFP.Div(MathFP.Mul(p.X, ffRad), dis),
                    MathFP.Div(MathFP.Mul(p.Y, ffRad), dis));
            return new LineFP(Pt2.X - p.X, Pt2.Y - p.Y, Pt2.X + p.X, Pt2.Y + p.Y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determines whether the specified ff val1 is equal.
        /// </summary>
        /// <param name="ffVal1">The ff val1.</param>
        /// <param name="ffVal2">The ff val2.</param>
        /// <returns>
        /// 	<c>true</c> if the specified ff val1 is equal; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsEqual(int ffVal1, int ffVal2)
        {
            return IsZero(ffVal1 - ffVal2);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determines whether the specified ff val is zero.
        /// </summary>
        /// <param name="ffVal">The ff val.</param>
        /// <returns>
        /// 	<c>true</c> if the specified ff val is zero; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsZero(int ffVal)
        {
            return MathFP.Abs(ffVal) < (1 << SingleFP.DecimalBits/2);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// check to see if two line intects and return the the intersction point.
        /// </summary>
        /// <param name="l1">The l1.</param>
        /// <param name="l2">The l2.</param>
        /// <param name="intersection">The intersection.</param>
        /// <returns></returns>
        public static bool Intersects(LineFP l1, LineFP l2, PointFP intersection)
        {
            var x = SingleFP.NotANumber;
            var y = SingleFP.NotANumber;

            if (intersection != null)
            {
                intersection.Reset(x, y);
            }

            var ax0 = l1.Pt1.X;
            var ax1 = l1.Pt2.X;
            var ay0 = l1.Pt1.Y;
            var ay1 = l1.Pt2.Y;
            var bx0 = l2.Pt1.X;
            var bx1 = l2.Pt2.X;
            var by0 = l2.Pt1.Y;
            var by1 = l2.Pt2.Y;

            var adx = (ax1 - ax0);
            var ady = (ay1 - ay0);
            var bdx = (bx1 - bx0);
            var bdy = (by1 - by0);

            if (IsZero(adx) && IsZero(bdx))
            {
                return IsEqual(ax0, bx0);
            }
            if (IsZero(ady) && IsZero(bdy))
            {
                return IsEqual(ay0, by0);
            }
            if (IsZero(adx))
            {
                // A  vertical
                x = ax0;
                y = IsZero(bdy) ? by0 : MathFP.Mul(MathFP.Div(bdy, bdx), x - bx0) + by0;
            }
            else if (IsZero(bdx))
            {
                // B vertical
                x = bx0;
                y = IsZero(ady) ? ay0 : MathFP.Mul(MathFP.Div(ady, adx), x - ax0) + ay0;
            }
            else if (IsZero(ady))
            {
                y = ay0;
                x = MathFP.Mul(MathFP.Div(bdx, bdy), y - by0) + bx0;
            }
            else if (IsZero(bdy))
            {
                y = by0;
                x = MathFP.Mul(MathFP.Div(adx, ady), y - ay0) + ax0;
            }
            else
            {
                var xma = MathFP.Div(ady, adx); // slope segment A
                var xba = ay0 - (MathFP.Mul(ax0, xma)); // y intercept of segment A

                var xmb = MathFP.Div(bdy, bdx); // slope segment B
                var xbb = by0 - (MathFP.Mul(bx0, xmb)); // y intercept of segment B

                // parallel lines?
                if (xma == xmb)
                {
                    // Need trig functions
                    return xba == xbb;
                }
                // Calculate points of intersection
                // At the intersection of line segment A and B,
                //XA=XB=XINT and YA=YB=YINT
                x = MathFP.Div((xbb - xba), (xma - xmb));
                y = (MathFP.Mul(xma, x)) + xba;
            }

            // After the point or points of intersection are calculated, each
            // solution must be checked to ensure that the point of intersection lies
            // on line segment A and B.

            var minxa = MathFP.Min(ax0, ax1);
            var maxxa = MathFP.Max(ax0, ax1);

            var minya = MathFP.Min(ay0, ay1);
            var maxya = MathFP.Max(ay0, ay1);

            var minxb = MathFP.Min(bx0, bx1);
            var maxxb = MathFP.Max(bx0, bx1);

            var minyb = MathFP.Min(by0, by1);
            var maxyb = MathFP.Max(by0, by1);

            if (intersection != null)
            {
                intersection.Reset(x, y);
            }
            return ((x >= minxa) && (x <= maxxa) && (y >= minya) && (y <= maxya)
                    && (x >= minxb) && (x <= maxxb) && (y >= minyb) && (y <= maxyb));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the point at distance.
        /// </summary>
        /// <param name="distance">The distance.</param>
        /// <returns></returns>
        internal PointFP GetPointAtDistance(int distance)
        {
            var lineLength = GetLength();
            if (distance > lineLength)
            {
                return null;
            }
            if (distance == lineLength)
            {
                return new PointFP(Pt2);
            }
            var scale = MathFP.Div(distance, lineLength);
            var pointFP = new PointFP();
            pointFP.Reset(Pt1.X + MathFP.Mul(Pt2.X - Pt1.X, scale),
                          Pt1.Y + MathFP.Mul(Pt2.Y - Pt1.Y, scale));
            return pointFP;
        }
    }
}