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
    /// a 2D rectangle class.
    /// </summary>
    internal class RectangleFP
    {
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the bottom.
        /// </summary>
        /// <returns></returns>
        public int GetBottom()
        {
            return _ffYmax;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the top.
        /// </summary>
        /// <returns></returns>
        public int GetTop()
        {
            return _ffYmin;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the left.
        /// </summary>
        /// <returns></returns>
        public int GetLeft()
        {
            return _ffXmin;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the right.
        /// </summary>
        /// <returns></returns>
        public int GetRight()
        {
            return _ffXmax;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <returns></returns>
        public int GetWidth()
        {
            return _ffXmax - _ffXmin;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the width.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetWidth(int value)
        {
            if (value < 0)
            {
                return;
            }
            _ffXmax = _ffXmin + value;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <returns></returns>
        public int GetHeight()
        {
            return _ffYmax - _ffYmin;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the height.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetHeight(int value)
        {
            if (value < 0)
            {
                return;
            }
            _ffYmax = _ffYmin + value;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the X.
        /// </summary>
        /// <returns></returns>
        public int GetX()
        {
            return _ffXmin;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the X.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetX(int value)
        {
            _ffXmin = value;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the Y.
        /// </summary>
        /// <returns></returns>
        public int GetY()
        {
            return _ffYmin;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the Y.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetY(int value)
        {
            _ffYmin = value;
        }

        /**
         * The empty rectangle.
         */
        public static readonly RectangleFP Empty = new RectangleFP();
        private int _ffXmin;
        private int _ffXmax;
        private int _ffYmin;
        private int _ffYmax;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleFP"/> class.
        /// </summary>
        public RectangleFP()
        {
            _ffXmin = _ffXmax = _ffYmin = _ffYmax = SingleFP.NotANumber;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleFP"/> class.
        /// </summary>
        /// <param name="r">The rect to be copied from.</param>
        public RectangleFP(RectangleFP r)
        {
            Reset(r);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleFP"/> class.
        /// </summary>
        /// <param name="ffXmin">The ff xmin.</param>
        /// <param name="ffYmin">The ff ymin.</param>
        /// <param name="ffXmax">The ff xmax.</param>
        /// <param name="ffYmax">The ff ymax.</param>
        public RectangleFP(int ffXmin, int ffYmin, int ffXmax, int ffYmax)
        {
            Reset(ffXmin, ffYmin, ffXmax, ffYmax);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Resets the specified rectangle.
        /// </summary>
        /// <param name="ffXmin">The ff xmin.</param>
        /// <param name="ffYmin">The ff ymin.</param>
        /// <param name="ffXmax">The ff xmax.</param>
        /// <param name="ffYmax">The ff ymax.</param>
        /// <returns></returns>
        public RectangleFP Reset(int ffXmin, int ffYmin, int ffXmax, int ffYmax)
        {
            _ffXmin = MathFP.Min(ffXmin, ffXmax);
            _ffXmax = MathFP.Max(ffXmin, ffXmax);
            _ffYmin = MathFP.Min(ffYmin, ffYmax);
            _ffYmax = MathFP.Max(ffYmin, ffYmax);
            return this;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Resets the rectangle.
        /// </summary>
        /// <param name="r">The r.</param>
        /// <returns></returns>
        public RectangleFP Reset(RectangleFP r)
        {
            return Reset(r._ffXmin, r._ffYmin, r._ffXmax, r._ffYmax);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determines whether this instance is empty.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance is empty; otherwise, <c>false</c>.
        /// </returns>
        public bool IsEmpty()
        {
            return _ffXmin == SingleFP.NotANumber || _ffXmax == SingleFP.NotANumber ||
                   _ffYmin == SingleFP.NotANumber || _ffYmax == SingleFP.NotANumber;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Translate the rectangle.
        /// </summary>
        /// <param name="ffDx">The ff dx.</param>
        /// <param name="ffDy">The ff dy.</param>
        public void Offset(int ffDx, int ffDy)
        {
            if (!IsEmpty())
            {
                _ffXmin += ffDx;
                _ffXmax += ffDx;
                _ffYmin += ffDy;
                _ffYmax += ffDy;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 24SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Calculate the union of the two rectangle.
        /// </summary>
        /// <param name="r">The r.</param>
        /// <returns></returns>
        public RectangleFP Union(RectangleFP r)
        {
            if (!r.IsEmpty())
            {
                if (IsEmpty())
                {
                    Reset(r);
                }
                else
                {
                    Reset(MathFP.Min(_ffXmin, r._ffXmin),
                          MathFP.Max(_ffXmax, r._ffXmax),
                          MathFP.Min(_ffYmin, r._ffYmin),
                          MathFP.Max(_ffYmax, r._ffYmax));
                }
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
        /// return the union of the rectangle and the given point.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        public RectangleFP Union(PointFP p)
        {
            if (!IsEmpty())
            {
                Reset(MathFP.Min(_ffXmin, p.X), MathFP.Max(_ffXmax, p.X),
                      MathFP.Min(_ffYmin, p.Y), MathFP.Max(_ffYmax, p.Y));
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
        /// Check to see this rectange intersect with given rectange.
        /// </summary>
        /// <param name="r">The r.</param>
        /// <returns></returns>
        public bool IntersectsWith(RectangleFP r)
        {
            return _ffXmin <= r._ffXmax && r._ffXmin <= _ffXmax &&
                   _ffYmin <= r._ffYmax && r._ffYmin <= _ffYmax;
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
        /// <param name="p">The p.</param>
        /// <returns>
        /// 	<c>true</c> if [contains] [the specified p]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(PointFP p)
        {
            return _ffXmin <= p.X && p.X <= _ffXmax
                   && _ffYmin <= p.Y && p.Y <= _ffYmax;
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
        /// <param name="o">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this 
        /// instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object o)
        {
            if (o is RectangleFP)
            {
                var r = (RectangleFP) o;
                return r._ffXmax == _ffXmax && r._ffXmin == _ffXmin
                       && r._ffYmax == _ffYmax && r._ffYmin == _ffYmin;
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
        ///  and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            var bits = (int) (_ffXmin & 0xFFFF0000 + _ffYmin & 0x0000FFFF);

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
            return "Rectangle" + " (" + new SingleFP(_ffXmin) + "," +
                   new SingleFP(_ffYmin) + ")-(" + new SingleFP(_ffXmax) + "," +
                   new SingleFP(_ffYmax) + ")";
        }
    }
}