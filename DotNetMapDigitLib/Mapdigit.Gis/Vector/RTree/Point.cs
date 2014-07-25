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
    /// A point in the n-dimensional space. All dimensions are stored in an array of
    /// doubles.
    /// </summary>
    internal class Point
    {

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> class.
        /// </summary>
        /// <param name="d">The d.</param>
        public Point(double[] d)
        {
            if (d == null) throw new
                    ArgumentException("Coordinates cannot be null.");

            if (d.Length < 2) throw new
                    ArgumentException
                    ("Point dimension should be greater than 1.");

            _data = new double[d.Length];
            Array.Copy(d, 0, _data, 0, d.Length);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> class.
        /// </summary>
        /// <param name="d">The d.</param>
        public Point(int[] d)
        {
            if (d == null) throw new
                    ArgumentException("Coordinates cannot be null.");

            if (d.Length < 2) throw new
                    ArgumentException
                    ("Point dimension should be greater than 1.");

            _data = new double[d.Length];

            for (int i = 0; i < d.Length; i++)
            {
                _data[i] = d[i];
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the float coordinate.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <returns></returns>
        public double GetFloatCoordinate(int i)
        {
            return _data[i];
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the int coordinate.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <returns></returns>
        public int GetIntCoordinate(int i)
        {
            return (int)_data[i];
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
            return _data.Length;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Equalses the specified p.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        public bool Equals(Point p)
        {
            if (p.GetDimension() != GetDimension())
            {
                throw new ArgumentException("Points must be of equal dimensions to be compared.");
            }

            bool ret = true;
            for (int i = 0; i < GetDimension(); i++)
            {
                if (GetFloatCoordinate(i) != p.GetFloatCoordinate(i))
                {
                    ret = false;
                    break;
                }
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
            double[] f = new double[_data.Length];
            Array.Copy(_data, 0, f, 0, _data.Length);

            return new Point(f);
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
            string s = "(" + _data[0];

            for (int i = 1; i < _data.Length; i++)
            {
                s += ", " + _data[i];
            }

            return s + ")";
        }


        /**
         * dimensions data.
         */
        private readonly double[] _data;


    }

}
