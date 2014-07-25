//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 27SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using Mapdigit.Util;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Gis.Geometry
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 27SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// A point representing a location in  (x,y) coordinate space.
    /// </summary>
    public class GeoPoint
    {

        /// <summary>
        /// The X coordinate of this GeoPoint.
        /// </summary>
        public double X;

        /// <summary>
        /// The Y coordinate of this GeoPoint.
        /// </summary>
        public double Y;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoPoint"/> class.
        /// </summary>
        public GeoPoint()
        {

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoPoint"/> class.
        /// </summary>
        /// <param name="p">a point</param>
        public GeoPoint(GeoPoint p)
            : this(p.X, p.Y)
        {

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoPoint"/> class.
        /// </summary>
        /// <param name="x">the X coordinate of the newly constructed Point</param>
        /// <param name="y">the Y coordinate of the newly constructed Point</param>
        public GeoPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a string representation of this point and its location 
        /// in the  (x,y) coordinate space. This method is 
        /// intended to be used only for debugging purposes, and the content 
        /// and format of the returned string may vary between implementations. 
        /// The returned string may be empty but may not be null.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return X + "," + Y;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the location of this GeoPoint to the same
        /// coordinates as the specified GeoPoint object.
        /// </summary>
        /// <param name="p">the specified GeoPoint to which to set
        /// this GeoPoint</param>
        public void SetLocation(GeoPoint p)
        {
            SetLocation(p.X, p.Y);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the square of the distance between two points.
        /// </summary>
        /// <param name="x1">the X coordinate of the first specified point.</param>
        /// <param name="y1">the Y coordinate of the first specified point.</param>
        /// <param name="x2">the X coordinate of the second specified point.</param>
        /// <param name="y2">the Y coordinate of the second specified point.</param>
        /// <returns>the square of the distance between the two
        /// sets of specified coordinates.</returns>
        public static double DistanceSq(double x1, double y1,
                double x2, double y2)
        {
            x1 -= x2;
            y1 -= y2;
            return (x1 * x1 + y1 * y1);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the distance between two points
        /// </summary>
        /// <param name="x1">the X coordinate of the first specified point.</param>
        /// <param name="y1">the Y coordinate of the first specified point.</param>
        /// <param name="x2">the X coordinate of the second specified point.</param>
        /// <param name="y2">the Y coordinate of the second specified point.</param>
        /// <returns>the distance between the two sets of specified
        /// coordinates</returns>
        public static double Distance(double x1, double y1,
                double x2, double y2)
        {
            x1 -= x2;
            y1 -= y2;
            return MathEx.Sqrt(x1 * x1 + y1 * y1);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the square of the distance from this
        ///  GeoPoint to a specified point.
        /// </summary>
        /// <param name="px">the X coordinate of the specified point to be measured
        ///            against this GeoPoint</param>
        /// <param name="py">the Y coordinate of the specified point to be measured
        ///            against this GeoPoint</param>
        /// <returns>the square of the distance between this
        /// GeoPoint and the specified point.</returns>
        public double DistanceSq(double px, double py)
        {
            px -= X;
            py -= Y;
            return (px * px + py * py);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the square of the distance from this
        /// GeoPoint to a specified GeoPoint.
        /// </summary>
        /// <param name="pt">the specified point to be measured
        ///            against this GeoPoint</param>
        /// <returns>the square of the distance between this
        /// GeoPoint to a specified GeoPoint.</returns>
        public double DistanceSq(GeoPoint pt)
        {
            double px = pt.X - X;
            double py = pt.Y - Y;
            return (px * px + py * py);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Returns the distance from this GeoPoint to
        ///  a specified point.
        /// </summary>
        /// <param name="px">the X coordinate of the specified point to be measured
        ///            against this GeoPoint</param>
        /// <param name="py">the Y coordinate of the specified point to be measured
        ///            against this GeoPoint</param>
        /// <returns>the distance between this GeoPoint
        /// and a specified point</returns>
        public double Distance(double px, double py)
        {
            px -= X;
            py -= Y;
            return MathEx.Sqrt(px * px + py * py);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the distance from this GeoPoint to a
        /// specified GeoPoint.
        /// </summary>
        /// <param name="pt">the specified point to be measured
        ///           against this GeoPoint</param>
        /// <returns>the distance between this GeoPoint and
        /// the specified GeoPoint.</returns>
        public double Distance(GeoPoint pt)
        {
            double px = pt.X - X;
            double py = pt.Y - Y;
            return MathEx.Sqrt(px * px + py * py);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
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
            long bits = Utils.DoubleToInt64Bits(X);
            bits ^= Utils.DoubleToInt64Bits(Y) * 31;
            return (((int)bits) ^ ((int)(bits >> 32)));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determines whether or not two points are equal. Two instances of
        /// GeoPoint are equal if the values of their
        /// x and y member fields, representing
        /// their position in the coordinate space, are the same.
        /// </summary>
        /// <param name="obj"> an object to be compared with this GeoPoint</param>
        /// <returns>
        /// 	 true if the object to be compared is
        ///         an instance of GeoPoint and has
        ///         the same values; false otherwise.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is GeoPoint)
            {
                GeoPoint p2D = (GeoPoint)obj;
                return (X == p2D.X) && (Y == p2D.Y);
            }
            return base.Equals(obj);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the location.
        /// </summary>
        /// <param name="x">new x coordinate</param>
        /// <param name="y">new Y coordinate</param>
        public void SetLocation(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

}
