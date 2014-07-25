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
using Mapdigit.Drawing.Core;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Drawing
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 14OCT2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// The Pen class defines a basic set of rendering attributes for the outlines
    /// of graphics primitives, which are rendered with a Graphics2D object that 
    /// has its Stroke attribute set to this Pen.
    /// </summary>
    /// <remarks>
    /// The rendering attributes defined by Pen describe the shape of the mark 
    /// made by a pen drawn along the outline of a IShape and the decorations 
    /// applied at the ends and joins of path segments of the IShape.
    /// These rendering attributes include:
    /// width--The pen width, measured perpendicularly to the pen trajectory.
    /// End caps--The decoration applied to the ends of unclosed subpaths and
    /// dash segments.  Subpaths that start and end on the same point are
    /// still considered unclosed if they do not have a CLOSE segment.
    /// The limit to trim a line join that has a JoinMiter decoration.
    /// A line join is trimmed when the ratio of miter length to stroke
    /// width is greater than the miterlimit value.  The miter length is
    /// the diagonal length of the miter, which is the distance between
    /// the inside corner and the outside corner of the intersection.
    /// The smaller the angle formed by two line segments, the longer
    /// the miter length and the sharper the angle of intersection.  The
    /// default miterlimit value of 10 causes all angles less than
    /// 11 degrees to be trimmed.  Trimming miters converts
    /// the decoration of the line join to bevel.
    /// dash attributes--The definition of how to make a dash pattern by alternating
    /// between opaque and transparent sections.
    /// For more information on the user space coordinate system and the
    /// rendering process, see the Graphics2D class comments.
    /// </remarks>
    public class Pen
    {
        //[------------------------------ CONSTANTS -------------------------------]
        
        ///<summary>
        /// Joins path segments by extending their outside edges until
        /// they meet.
        ///</summary>
        public const int JoinMiter = PenFP.LinejoinMiter;

        ///<summary>
        /// Joins path segments by rounding off the corner at a radius
        /// of half the line width.
        ///</summary>
        public const int JoinRound = PenFP.LinejoinRound;

        ///<summary>
        /// Joins path segments by connecting the outer corners of their
        /// wide outlines with a straight segment.
        ///</summary>
        public const int JoinBevel = PenFP.LinejoinBevel;

        ///<summary>
        /// Ends unclosed subpaths and dash segments with no added decoration.
        ///</summary>
        public const int CapButt = PenFP.LinecapButt;

        /// <summary>
        /// Ends unclosed subpaths and dash segments with a Round
        /// decoration that has a radius equal to half of the width
        /// of the pen.
        /// </summary>
        public const int CapRound = PenFP.LinecapRound;

        /// <summary>
        /// Ends unclosed subpaths and dash segments with a square
        /// projection that extends beyond the end of the segment
        /// to a distance equal to half of the line width.
        /// </summary>
        public const int CapSquare = PenFP.LinecapSquare;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// IConstructs a new Pen with the specified attributes.
        /// </summary>
        /// <param name="brush">brush used to construct the pen object.</param>
        /// <param name="width">the width of this Pen.  The
        ///         width must be greater than or equal to 0.  If width is
        ///         set to 0, the stroke is rendered as the thinnest
        ///         possible line for the target device and the antialias
        ///         hint setting.</param>
        /// <param name="cap">the decoration of the ends of a Pen.</param>
        /// <param name="join">the decoration applied where path segments meet.</param>
        /// <param name="dash">the array representing the dashing pattern.</param>
        /// <param name="dashPhase">the offset to start the dashing pattern.</param>
        public Pen(Brush brush, int width, int cap, int join, int[] dash, int dashPhase)
            : this(Color.Black, width, cap, join, dash, dashPhase)
        {
            _brush = brush;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="Pen"/> class.
        /// </summary>
        /// <param name="color">the color of the pen.</param>
        /// <param name="width">the width of this Pen.  The
        ///         width must be greater than or equal to 0.  If width is
        ///         set to 0, the stroke is rendered as the thinnest
        ///         possible line for the target device and the antialias
        ///         hint setting.</param>
        /// <param name="cap">the decoration of the ends of a Pen.</param>
        /// <param name="join">the decoration applied where path segments meet.</param>
        /// <param name="dash">the array representing the dashing pattern.</param>
        /// <param name="dashPhase">the offset to start the dashing pattern.</param>
        public Pen(Color color, int width, int cap, int join,
                   int[] dash, int dashPhase)
        {
            if (width < 0)
            {
                throw new ArgumentException("negative width");
            }
            if (cap != CapButt && cap != CapRound && cap != CapSquare)
            {
                throw new ArgumentException("illegal end cap Value");
            }
            if (join != JoinRound && join != JoinBevel && join != JoinMiter)
            {
                throw new ArgumentException("illegal line join Value");
            }
            if (dash != null)
            {
                if (dashPhase < 0)
                {
                    throw new ArgumentException("negative dash phase");
                }
                bool allzero = true;
                for (int i = 0; i < dash.Length; i++)
                {
                    int d = dash[i];
                    if (d > 0)
                    {
                        allzero = false;
                    }
                    else if (d < 0)
                    {
                        throw new ArgumentException("negative dash length");
                    }
                }
                if (allzero)
                {
                    throw new ArgumentException("dash lengths all zero");
                }
            }
            _width = width;
            _cap = cap;
            _join = join;
            _color = color;
            if (dash != null)
            {
                _dash = dash;
            }

            _dashPhase = dashPhase;
            _wrappedPenFP = new PenFP(color._value, width << SingleFP.DecimalBits,
                                      cap, cap, join);
            if (dash != null)
            {
                int[] newDash = new int[dash.Length];
                for (int i = 0; i < newDash.Length; i++)
                {
                    newDash[i] = dash[i] << SingleFP.DecimalBits;
                }
                _wrappedPenFP.SetDashArray(newDash, dashPhase);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="Pen"/> class.
        /// </summary>
        /// <param name="brush">brush used to construct the pen object.</param>
        /// <param name="width">the width of this Pen.  The
        ///         width must be greater than or equal to 0.  If width is
        ///         set to 0, the stroke is rendered as the thinnest
        ///         possible line for the target device and the antialias
        ///         hint setting.</param>
        /// <param name="cap">the decoration of the ends of a Pen.</param>
        /// <param name="join">the decoration applied where path segments meet.</param>
        public Pen(Brush brush, int width, int cap, int join)
            : this(brush, width, cap, join, null, 0)
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="Pen"/> class.
        /// </summary>
        /// <param name="color">the color of the Pen</param>
        /// <param name="width">the width of the Pen</param>
        /// <param name="cap">the decoration of the ends of a Pen.</param>
        /// <param name="join">the decoration applied where path segments meet.</param>
        public Pen(Color color, int width, int cap, int join)
            : this(color, width, cap, join, null, 0)
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a solid Pen with the specified
        /// line width and with default values for the cap and join styles.
        /// </summary>
        /// <param name="brush">the brush used to create the pen.</param>
        /// <param name="width">the width of the Pen</param>
        public Pen(Brush brush, int width)
            : this(brush, width, CapSquare, JoinMiter, null, 0)
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="Pen"/> class.
        /// </summary>
        /// <param name="color">color the color of the Pen</param>
        /// <param name="width">the width of the Pen</param>
        public Pen(Color color, int width)
            : this(color, width, CapSquare, JoinMiter, null, 0)
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Constructs a new Pen with given brush.
        /// </summary>
        /// <param name="brush"> brush used to create the pen.</param>
        public Pen(Brush brush)
            : this(brush, 1, CapSquare, JoinMiter, null, 0)
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a new Pen with defaults for all
        /// attributes.
        /// The default attributes are a solid line of width 1.0, CapSquare,
        /// JoinMiter, a miter limit of 10.
        /// </summary>
        /// <param name="color">the color of the Pen</param>
        public Pen(Color color)
            : this(color, 1, CapSquare, JoinMiter, null, 0)
        {
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the line width. 
        /// </summary>
        /// <remarks>
        ///  Line width is represented in user space,
        /// which is the default-coordinate space used by Java 2D.  See the
        /// Graphics2D class comments for more information on
        /// the user space coordinate system.
        /// </remarks>
        public int Width
        {
            get { return _width; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the pen color.
        /// </summary>
        public Color Color
        {
            get { return _color; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the end cap style.
        /// </summary>
        public int EndCap
        {
            get { return _cap; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the line join style.
        /// </summary>
        public int LineJoin
        {
            get { return _join; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the array representing the lengths of the dash segments.
        /// </summary>
        /// <remarks>
        /// Alternate entries in the array represent the user space lengths
        /// of the opaque and transparent segments of the dashes.
        /// As the pen moves along the outline of the IShape
        /// to be stroked, the user space
        /// distance that the pen travels is accumulated.  The distance
        /// value is used to index into the dash array.
        /// The pen is opaque when its current cumulative distance maps
        /// to an even element of the dash array and transparent otherwise.
        /// </remarks>
        public int[] DashArray
        {
            get
            {
                if (_dash == null)
                {
                    return null;
                }
                return _dash;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the current dash phase.
        /// </summary>
        /// <remarks>
        /// The dash phase is a distance specified in user coordinates that
        /// represents an offset into the dashing pattern. In other words, the dash
        /// phase defines the point in the dashing pattern that will correspond to
        /// the beginning of the stroke.
        /// </remarks>
        public int DashPhase
        {
            get { return _dashPhase; }
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
        /// and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            int hash = (int) Util.Utils.DoubleToInt64Bits(_width);
            hash = hash*31 + _join;
            hash = hash*31 + _cap;
            if (_dash != null)
            {
                hash = hash*31 + (int) Util.Utils.DoubleToInt64Bits(_dashPhase);
                for (int i = 0; i < _dash.Length; i++)
                {
                    hash = hash*31 + (int) Util.Utils.DoubleToInt64Bits(_dash[i]);
                }
            }
            return hash;
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
            if (!(obj is Pen))
            {
                return false;
            }

            Pen bs = (Pen) obj;
            if (_width != bs._width)
            {
                return false;
            }

            if (_join != bs._join)
            {
                return false;
            }

            if (_cap != bs._cap)
            {
                return false;
            }


            if (_color != bs._color)
            {
                return false;
            }

            if (_dash != null)
            {
                if (_dashPhase != bs._dashPhase)
                {
                    return false;
                }

                if (!Equals(_dash, bs._dash))
                {
                    return false;
                }
            }
            else if (bs._dash != null)
            {
                return false;
            }

            return true;
        }

        internal readonly int _width;
        internal readonly int _join;
        internal readonly int _cap;
        internal readonly int[] _dash;
        internal readonly int _dashPhase;
        internal readonly Color _color;
        internal Brush _brush;
        internal readonly PenFP _wrappedPenFP;
    }
}