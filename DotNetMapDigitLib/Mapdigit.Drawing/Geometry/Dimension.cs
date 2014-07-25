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
    /// The Dimension class encapsulates the width and
    /// height of a component (in integer) in a single object.
    /// </summary>
    /// <remarks>
    /// The class is
    /// associated with certain properties of components. Several methods
    /// defined by the Component class and the
    /// LayoutManager interface return a
    /// Dimension object.
    /// 
    /// Normally the values of width
    /// and height are non-negative integers.
    /// The constructors that allow you to create a dimension do
    /// not prevent you from setting a negative value for these properties.
    /// If the value of width or height is
    /// negative, the behavior of some methods defined by other objects is
    /// undefined.
    /// </remarks>
    public class Dimension
    {

        /// <summary>
        /// The width dimension; negative values can be used.
        /// </summary>
        public int Width;

        /// <summary>
        /// The height dimension; negative values can be used.
        /// </summary>
        public int Height;


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          ICode review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="Dimension"/> class.
        /// </summary>
        public Dimension() : this(0, 0)
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          ICode review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates an instance of Dimension whose width
        /// and height are the same as for the specified dimension.
        /// </summary>
        /// <param name="d">d   the specified dimension for the
        ///                width and
        ///                height values.</param>
        public Dimension(Dimension d)
        {
            Width = d.Width;
            Height = d.Height;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          ICode review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a Dimension and initializes
        /// it to the specified width and specified height.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public Dimension(int width, int height)
        {
            Width = width;
            Height = height;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          ICode review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the size of this Dimension object to
        /// the specified width and height in int precision.
        /// rem that if width or height
        /// are larger than Integer.MAX_VALUE, they will
        /// be reset to Integer.MAX_VALUE.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public void SetSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          ICode review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// the size of this Dimension object.
        /// This method is included for completeness, to parallel the
        /// getSize method defined by Component.
        /// </summary>
        public Dimension Size
        {
            get { return new Dimension(Width, Height); }
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          ICode review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	true if the specified <see cref="System.Object"/> is equal
        ///  to this instance; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            if (obj is Dimension)
            {
                Dimension d = (Dimension) obj;
                return (Width == d.Width) && (Height == d.Height);
            }
            return false;
        }

     
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          ICode review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a hash c for this instance.
        /// </summary>
        /// <returns>
        /// A hash c for this instance, suitable for use in hashing algorithms
        ///  and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            int sum = Width + Height;
            return (sum*(sum + 1)/2 + Width);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          ICode review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "SIZE [width=" + Width + ",height=" + Height + "]";
        }
    }
}