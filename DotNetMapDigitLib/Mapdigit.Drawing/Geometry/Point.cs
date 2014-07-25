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
using System;
using Mapdigit.Drawing.Geometry.Parser;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Drawing.Geometry
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 14OCT2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///  A point representing a location in (x,y) coordinate space,
    ///  specified in integer.
    /// </summary>
    public class Point
    {

        /// <summary>
        /// The X coordinate of this Point.
        /// If no X coordinate is set it will default to 0.
        /// </summary>
        public int X;


        /// <summary>
        /// The Y coordinate of this Point. 
        /// If no Y coordinate is set it will default to 0.
        /// </summary>
        public int Y;


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> class.
        /// </summary>
        public Point()
            : this(0, 0)
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs and initializes a point with the same location as
        ///  the specified Point object.
        /// </summary>
        /// <param name="p">The point.</param>
        public Point(Point p)
        {
            X = p.X;
            Y = p.Y;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Constructs and initializes a point at the specified 
        /// (x,y) location in the coordinate space. 
        /// </summary>
        /// <param name="x">the X coordinate of the newly constructed Point</param>
        /// <param name="y">the Y coordinate of the newly constructed Point</param>
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// parse point from a string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>a array of point</returns>
        public static Point[] FromString(string input)
        {
            lock (NumberListParser)
            {
                float[] coords = NumberListParser.ParseNumberList(input);
                int length = coords.Length/2;
                Point[] points = new Point[length];
                if (length >= 2)
                {
                    for (int i = 0; i < length; i++)
                    {
                        points[i] = new Point();
                        points[i].X = (int) coords[i*2];
                        points[i].Y = (int) coords[i*2 + 1];
                    }
                    return points;
                }
                return null;
            }
        }

      
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// the location of this point.
        /// </summary>
        public Point Location
        {
            get { return new Point(X, Y); }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Changes the point to have the specified location.
        /// 
        /// This method is included for completeness, to parallel the
        /// setLocation method of Component.
        /// Its behavior is identical with move(int,int).
        /// </summary>
        /// <param name="x">the X coordinate of the new location</param>
        /// <param name="y">the Y coordinate of the new location</param>
        public void SetLocation(int x, int y)
        {
            X = x;
            Y = y;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Moves this point to the specified location in the 
        /// (x,y) coordinate plane. This method
        /// is identical with setLocation(int,int).
        /// </summary>
        /// <param name="x">the X coordinate of the new location</param>
        /// <param name="y">the Y coordinate of the new location</param>
        public void Move(int x, int y)
        {
            X = x;
            Y = y;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Translates this point, at location (x,y), 
        /// by dx along the x axis and dy 
        /// along the y axis so that it now represents the point 
        /// (x+dx,y+dy)}.
        /// </summary>
        /// <param name="dx">the distance to move this point 
        ///                             along the X axis</param>
        /// <param name="dy">the distance to move this point 
        ///                            along the Y axis</param>
        public void Translate(int dx, int dy)
        {
            X += dx;
            Y += dy;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
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
            if (obj is Point)
            {
                Point pt = (Point) obj;
                return (X == pt.X) && (Y == pt.Y);
            }
            return false;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "POINT [" + X +
                   "," + Y + "]";
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the square of the distance between two points.
        /// </summary>
        /// <param name="x1"> the X coordinate of the first specified point.</param>
        /// <param name="y1"> the Y coordinate of the first specified point</param>
        /// <param name="x2">the X coordinate of the second specified point.</param>
        /// <param name="y2">the Y coordinate of the second specified point.</param>
        /// <returns>the square of the distance between the two
        ///  sets of specified coordinates.</returns>
        public static int DistanceSq(int x1, int y1,
                                     int x2, int y2)
        {
            x1 -= x2;
            y1 -= y2;
            return (x1*x1 + y1*y1);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the distance between two points.
        /// </summary>
        /// <param name="x1"> the X coordinate of the first specified point.</param>
        /// <param name="y1"> the Y coordinate of the first specified point</param>
        /// <param name="x2">the X coordinate of the second specified point.</param>
        /// <param name="y2">the Y coordinate of the second specified point.</param>
        /// <returns>the distance between the two sets of specified
        /// coordinates.</returns>
        public static int Distance(int x1, int y1,
                                   int x2, int y2)
        {
            x1 -= x2;
            y1 -= y2;
            long disSq = x1*x1 + y1*y1;
            long dis = MathFP.Sqrt(disSq << MathFP.DefaultPrecision);
            return MathFP.ToInt(dis);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Returns the square of the distance from this
        ///  Point to a specified point.
        /// </summary>
        /// <param name="px">the X coordinate of the specified point to be measured
        ///            against this Point</param>
        /// <param name="py">the Y coordinate of the specified point to be measured
        ///            against this Point</param>
        /// <returns> the square of the distance between this
        ///  Point and the specified point.</returns>
        public int DistanceSq(int px, int py)
        {
            px -= X;
            py -=Y;
            return (px*px + py*py);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Returns the square of the distance from this
        ///  Point to a specified Point.
        /// </summary>
        /// <param name="pt"> the specified point to be measured
        ///            against this Point</param>
        /// <returns>the square of the distance between this
        /// Point to a specified Point.</returns>
        public int DistanceSq(Point pt)
        {
            int px = pt.X - X;
            int py = pt.Y - Y;
            return (px*px + py*py);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the distance from this Point to
        ///  a specified point.
        /// </summary>
        /// <param name="px">the X coordinate of the specified point to be measured
        ///            against this Point</param>
        /// <param name="py">the Y coordinate of the specified point to be measured
        ///            against this Point</param>
        /// <returns>the distance between this Point
        ///  and a specified point.</returns>
        public int Distance(int px, int py)
        {
            px -= X;
            py -= Y;
            long disSq = px*px + py*py;
            long dis = MathFP.Sqrt(disSq << MathFP.DefaultPrecision);
            return MathFP.ToInt(dis);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the distance from this Point to a
        /// specified Point.
        /// </summary>
        /// <param name="pt">the specified point to be measured
        ///           against this Point</param>
        /// <returns> the distance between this Point and
        ///  the specified Point.</returns>
        public int Distance(Point pt)
        {
            int px = pt.X - X;
            int py = pt.Y - Y;
            long disSq = px*px + py*py;
            long dis = MathFP.Sqrt(disSq << MathFP.DefaultPrecision);
            return MathFP.ToInt(dis);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
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
            int bits = (int) ((X << 16) & 0xFFFF0000);
            bits ^= Y;
            return bits;
        }

        private static readonly NumberListParser NumberListParser = new NumberListParser();
    }
}