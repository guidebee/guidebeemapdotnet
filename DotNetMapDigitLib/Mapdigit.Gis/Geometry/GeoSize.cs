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
using System;

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
    /// The GeoSize class encapsulates the width and 
    /// height of a component (in integer precision) in a single object. 
    /// </summary>
    public class GeoSize
    {

        /// <summary>
        /// The Width GeoSize; negative values can be used.
        /// </summary>
        public double Width;

        /// <summary>
        /// The Height GeoSize; negative values can be used.
        /// </summary>
        public double Height;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoSize"/> class.
        /// </summary>
        public GeoSize()
            : this(0, 0)
        {

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoSize"/> class.
        /// </summary>
        /// <param name="size">the specified GeoSize for the  width and 
        ///               height values</param>
        public GeoSize(GeoSize size)
            : this(size.Width, size.Height)
        {

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoSize"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public GeoSize(double width, double height)
        {
            Width = width;
            Height = height;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the size of this GeoSize object to
        /// match the specified size.
        /// This method is included for completeness, to parallel the
        /// getSize method of Component.
        /// </summary>
        /// <param name="d">the new size for the GeoSize object</param>
        public void SetSize(GeoSize d)
        {
            SetSize(d.Width, d.Height);
        }

        

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the size of this GeoSize object to
        /// the specified width and height in double precision.
        /// remember that if width or height
        /// are larger than Integer.MAX_VALUE, they will
        /// be reset to Integer.MAX_VALUE.
        /// </summary>
        /// <param name="width">the new width for the GeoSize object</param>
        /// <param name="height">the new height for the GeoSize object.</param>
        public void SetSize(double width, double height)
        {
            Width = (int)Math.Ceiling(width);
            Height = (int)Math.Ceiling(height);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the size of this GeoSize object.
        /// This method is included for completeness, to parallel the
        /// getSize method defined by Component.
        /// </summary>
        /// <returns>the size of this GeoSize, a new instance of
        ///         GeoSize with the same width and height</returns>
        public GeoSize GetSize()
        {
            return new GeoSize(Width, Height);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the size of this GeoSize object
        /// to the specified width and height.
        /// This method is included for completeness, to parallel the
        /// setSize method defined by Component.
        /// </summary>
        /// <param name="width"> the new width for this GeoSize object</param>
        /// <param name="height"> the new height for this GeoSize object</param>
        public void SetSize(int width, int height)
        {
            Width = width;
            Height = height;
        }



        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to 
        /// this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            if (obj is GeoSize)
            {
                GeoSize d = (GeoSize)obj;
                return (Width == d.Width) && (Height == d.Height);
            }
            return false;
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
            double sum = Width + Height;
            return (int)(sum * (sum + 1) / 2 + Width);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a string representation of the values of this
        /// GeoSize object's height and
        /// width fields. This method is intended to be used only
        /// for debugging purposes, and the content and format of the returned
        /// string may vary between implementations. The returned string may be
        /// empty but may not be null.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "[Width=" + Width + ",Height=" + Height + "]";
        }
    }

}
