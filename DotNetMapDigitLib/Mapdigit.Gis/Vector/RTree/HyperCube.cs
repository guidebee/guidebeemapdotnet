//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 02OCT2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Gis.Vector.RTree
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 02OCT2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// A HyperCube in the n-dimensional space. It is represented by two points of
    /// n dimensions each.Implements basic calculation functions, like <B>getArea()</B>
    /// and <B>getUnionMbb()</B>.
    /// </summary>
    internal class HyperCube
    {

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="HyperCube"/> class.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        public HyperCube(Point p1, Point p2)
        {
            bool bSwitch = false;
            if (p1 == null || p2 == null)
            {
                throw new
                        ArgumentException("Points cannot be null.");
            }

            if (p1.GetDimension() != p2.GetDimension())
            {
                throw new
                 ArgumentException("Points must be of the same dimension.");
            }

            for (int i = 0; i < p1.GetDimension(); i++)
            {
                if (p1.GetFloatCoordinate(i) > p2.GetFloatCoordinate(i))
                {
                    bSwitch = true;
                    break;
                }
            }

            if (!bSwitch)
            {
                _p1 = (Point)p1.Clone();
                _p2 = (Point)p2.Clone();
            }
            else
            {
                _p1 = (Point)p2.Clone();
                _p2 = (Point)p1.Clone();
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the dimension.
        /// </summary>
        /// <returns></returns>
        public int GetDimension()
        {
            return _p1.GetDimension();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the p1.
        /// </summary>
        /// <returns></returns>
        public Point GetP1()
        {
            return (Point)_p1.Clone();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the p2.
        /// </summary>
        /// <returns></returns>
        public Point GetP2()
        {
            return (Point)_p2.Clone();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Equalses the specified h.
        /// </summary>
        /// <param name="h">The h.</param>
        /// <returns></returns>
        public bool Equals(HyperCube h)
        {
            if (_p1.Equals(h.GetP1()) && _p2.Equals(h.GetP2()))
            {
                return true;
            }
            return false;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tests to see whether <B>h</B> has any common points with this HyperCube.
        /// If <B>h</B> is inside this object (or vice versa), it returns true.
        /// </summary>
        /// <param name="h">The h.</param>
        /// <returns> if <B>h</B> and this HyperCube intersect, false otherwise</returns>
        public bool Intersection(HyperCube h)
        {
            if (h == null) throw new
                    ArgumentException("HyperCube cannot be null.");

            if (h.GetDimension() != GetDimension()) throw new
                      ArgumentException
                    ("HyperCube dimension is different from current dimension.");

            bool intersect = true;
            for (int i = 0; i < GetDimension(); i++)
            {
                if (_p1.GetFloatCoordinate(i) > h._p2.GetFloatCoordinate(i) ||
                        _p2.GetFloatCoordinate(i) < h._p1.GetFloatCoordinate(i))
                {
                    intersect = false;
                    break;
                }
            }
            return intersect;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tests to see whether <B>h</B> is inside this HyperCube.
        /// If <B>h</B> is exactly the same shape as this object, it is considered 
        /// to be inside.
        /// </summary>
        /// <param name="h">The h.</param>
        /// <returns>True if <B>h</B> is inside, false otherwise.</returns>
        public bool Enclosure(HyperCube h)
        {
            if (h == null) throw new
                    ArgumentException("HyperCube cannot be null.");

            if (h.GetDimension() != GetDimension()) throw new
                    ArgumentException
                    ("HyperCube dimension is different from current dimension.");

            bool inside = true;
            for (int i = 0; i < GetDimension(); i++)
            {
                if (_p1.GetFloatCoordinate(i) > h._p1.GetFloatCoordinate(i) ||
                        _p2.GetFloatCoordinate(i) < h._p2.GetFloatCoordinate(i))
                {
                    inside = false;
                    break;
                }
            }

            return inside;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Tests to see whether <B>p</B> is inside this HyperCube.
        /// If <B>p</B> lies on the boundary
        /// of the HyperCube, it is considered to be inside the object.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>True if <B>p</B> is inside, false otherwise</returns>
        public bool Enclosure(Point p)
        {
            if (p == null) throw new
                    ArgumentException("Point cannot be null.");

            if (p.GetDimension() != GetDimension()) throw new
                    ArgumentException
                    ("Point dimension is different from HyperCube dimension.");

            return Enclosure(new HyperCube(p, p));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the area of the intersecting region between this HyperCube and 
        /// the argument.
        ///
        /// Below, all possible situations are depicted.
        ///
        ///     -------   -------      ---------   ---------      ------         ------
        ///    |2      | |2      |    |2        | |1        |    |2     |       |1     |
        ///  --|--     | |     --|--  | ------  | | ------  |  --|------|--   --|------|--
        /// |1 |  |    | |    |1 |  | ||1     | | ||2     | | |1 |      |  | |2 |      |  |
        ///  --|--     | |     --|--  | ------  | | ------  |  --|------|--   --|------|--
        ///     -------   -------      ---------   ---------      ------         ------
        /// </summary>
        /// <param name="h">Given HyperCube</param>
        /// <returns> Area of intersecting region</returns>
        public double IntersectingArea(HyperCube h)
        {
            if (!Intersection(h))
            {
                return 0;
            }
            double ret = 1;
            for (int i = 0; i < GetDimension(); i++)
            {
                double l1 = _p1.GetFloatCoordinate(i);
                double h1 = _p2.GetFloatCoordinate(i);
                double l2 = h._p1.GetFloatCoordinate(i);
                double h2 = h._p2.GetFloatCoordinate(i);

                if (l1 <= l2 && h1 <= h2)
                {
                    // cube1 left of cube2.
                    ret *= (h1 - l1) - (l2 - l1);
                }
                else if (l2 <= l1 && h2 <= h1)
                {
                    // cube1 right of cube2.
                    ret *= (h2 - l2) - (l1 - l2);
                }
                else if (l2 <= l1 && h1 <= h2)
                {
                    // cube1 inside cube2.
                    ret *= h1 - l1;
                }
                else if (l1 <= l2 && h2 <= h1)
                {
                    // cube1 Contains cube2.
                    ret *= h2 - l2;
                }
                else if (l1 <= l2 && h2 <= h1)
                {
                    // cube1 crosses cube2.
                    ret *= h2 - l2;
                }
                else if (l2 <= l1 && h1 <= h2)
                {
                    // cube1 crossed by cube2.
                    ret *= h1 - l1;
                }
            }
            if (ret <= 0) throw new
                ArithmeticException("Intersecting area cannot be negative!");
            return ret;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Static impementation. Takes an array of HyperCubes and calculates the 
        /// mbb of their union.
        /// </summary>
        /// <param name="a">The array of HyperCubes.</param>
        /// <returns>The mbb of their union</returns>
        public static HyperCube GetUnionMbb(HyperCube[] a)
        {
            if (a == null || a.Length == 0) throw new
                    ArgumentException("HyperCube array is empty.");

            HyperCube h = (HyperCube)a[0].Clone();

            for (int i = 1; i < a.Length; i++)
            {
                h = h.GetUnionMbb(a[i]);
            }

            return h;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Return a new HyperCube representing the mbb of the union of this 
        /// HyperCube and <B>h</B>
        /// </summary>
        /// <param name="h">The HyperCube that we want to union with this HyperCube.</param>
        /// <returns> A HyperCube representing the mbb of their union</returns>
        public HyperCube GetUnionMbb(HyperCube h)
        {
            if (h == null) throw new
                    ArgumentException("HyperCube cannot be null.");

            if (h.GetDimension() != GetDimension()) throw new
                    ArgumentException
                    ("HyperCubes must be of the same dimension.");

            double[] min = new double[GetDimension()];
            double[] max = new double[GetDimension()];

            for (int i = 0; i < GetDimension(); i++)
            {
                min[i] = Math.Min(_p1.GetFloatCoordinate(i),
                        h._p1.GetFloatCoordinate(i));
                max[i] = Math.Max(_p2.GetFloatCoordinate(i),
                        h._p2.GetFloatCoordinate(i));
            }

            return new HyperCube(new Point(min), new Point(max));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the area of this HyperCube.
        /// </summary>
        /// <returns></returns>
        public double GetArea()
        {
            double area = 1;

            for (int i = 0; i < GetDimension(); i++)
            {
                area *= _p2.GetFloatCoordinate(i) - _p1.GetFloatCoordinate(i);
            }

            return area;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the min dist.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        public double GetMinDist(Point p)
        {
            if (p == null) throw new
                    ArgumentException("Point cannot be null.");

            if (p.GetDimension() != GetDimension()) throw new
                    ArgumentException
                    ("Point dimension is different from HyperCube dimension.");

            double ret = 0;
            for (int i = 0; i < GetDimension(); i++)
            {
                double q = p.GetFloatCoordinate(i);
                double s = _p1.GetFloatCoordinate(i);
                double t = _p2.GetFloatCoordinate(i);
                double r;

                if (q < s) r = s;
                else if (q > t) r = t;
                else r = q;

                ret += Math.Abs(q - r) * Math.Abs(q - r);
            }

            return ret;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new HyperCube((Point)_p1.Clone(), (Point)_p2.Clone());
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "P1" + _p1 + ":P2" + _p2;
        }

        /**
         * point 1 .
         */
        private readonly Point _p1;

        /**
         * point 2.
         */
        private readonly Point _p2;
    }
}
