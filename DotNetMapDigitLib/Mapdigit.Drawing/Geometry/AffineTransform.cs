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
    /// The AffineTransform class represents a 2D affine transform
    /// that performs a linear mapping from 2D coordinates to other 2D
    /// coordinates that preserves the "straightness" and
    /// "parallelness" of lines. 
    /// </summary>
    /// <remarks>
    ///  Affine transformations can be constructed
    /// using sequences of translations, scales, flips, rotations, and shears.
    /// 
    /// Such a coordinate transformation can be represented by a 3 row by
    /// 3 column matrix with an implied last row of [ 0 0 1 ].  This matrix
    /// transforms source coordinates (x,y) into
    /// destination coordinates  (x',y') by considering
    /// them to be a column vector and multiplying the coordinate vector
    /// by the matrix according to the following process:
    /// <pre>
    ///	[ x']   [  m00  m01  m02  ] [ x ]   [ m00x + m01y + m02 ]
    ///	[ y'] = [  m10  m11  m12  ] [ y ] = [ m10x + m11y + m12 ]
    ///	[ 1 ]   [   0    0    1   ] [ 1 ]   [         1         ]
    /// </pre>
    /// 
    /// <a name="quadrantapproximation"><h4>Handling 90-Degree Rotations</h4></a>
    /// 
    /// In some variations of the rotate methods in the
    /// AffineTransform class, a double-precision argument
    /// specifies the angle of rotation in radians.
    /// These methods have special handling for rotations of approximately
    /// 90 degrees (including multiples such as 180, 270, and 360 degrees),
    /// so that the common case of quadrant rotation is handled more
    /// efficiently.
    /// This special handling can cause angles very close to multiples of
    /// 90 degrees to be treated as if they were exact multiples of
    /// 90 degrees.
    /// For small multiples of 90 degrees the range of angles treated
    /// as a quadrant rotation is approximately 0.00000121 degrees wide.
    /// This section explains why such special care is needed and how
    /// it is implemented.
    /// 
    /// Since 90 degrees is represented as Pi/2 in radians,
    /// and since Pi is a transcendental (and therefore irrational) number,
    /// it is not possible to exactly represent a multiple of 90 degrees as
    /// an exact double precision Value measured in radians.
    /// As a result it is theoretically impossible to describe quadrant
    /// rotations (90, 180, 270 or 360 degrees) using these values.
    /// Double precision floating point values can get very close to
    /// non-zero multiples of Pi/2 but never close enough
    /// for the sine or cosine to be exactly 0.0, 1.0 or -1.0.
    /// The implementations of Math.Sin() and
    /// Math.Cos() correspondingly never return 0.0
    /// for any case other than Math.Sin(0.0).
    /// These same implementations do, however, return exactly 1.0 and
    /// -1.0 for some range of numbers around each multiple of 90
    /// degrees since the correct answer is so close to 1.0 or -1.0 that
    /// the double precision significand cannot represent the difference
    /// as accurately as it can for numbers that are near 0.0.
    /// 
    /// The net result of these issues is that if the
    /// Math.Sin() and Math.Cos() methods
    /// are used to directly generate the values for the matrix modifications
    /// during these radian-based rotation operations then the resulting
    /// transform is never strictly classifiable as a quadrant rotation
    /// even for a simple case like rotate(Math.Pi/2.0),
    /// due to minor variations in the matrix caused by the non-0.0 values
    /// obtained for the sine and cosine.
    /// If these transforms are not classified as quadrant rotations then
    /// subsequent c which attempts to optimize further operations based
    /// upon the type of the transform will be relegated to its most general
    /// implementation.
    /// 
    /// Because quadrant rotations are fairly common,
    /// this class should handle these cases reasonably quickly, both in
    /// applying the rotations to the transform and in applying the resulting
    /// transform to the coordinates.
    /// To facilitate this optimal handling, the methods which take an angle
    /// of rotation measured in radians attempt to detect angles that are
    /// intended to be quadrant rotations and treat them as such.
    /// These methods therefore treat an angle <em>theta</em> as a quadrant
    /// rotation if either Math.Sin(<em>theta</em>) or
    /// Math.Cos(<em>theta</em>) returns exactly 1.0 or -1.0.
    /// As a rule of thumb, this property holds true for a range of
    /// approximately 0.0000000211 radians (or 0.00000121 degrees) around
    /// small multiples of Math.Pi/2.0.
    /// </remarks>
    public class AffineTransform
    {
 
        ///<summary>
        /// This constant indicates that the transform defined by this object
        /// is an identity transform.
        /// </summary>
        /// <remarks>
        /// An identity transform is one in which the output coordinates are
        /// always the same as the input coordinates.
        /// If this transform is anything other than the identity transform,
        /// the type will either be the constant GENERAL_TRANSFORM or a
        /// combination of the appropriate flag bits for the various coordinate
        /// conversions that this transform performs.
        ///</remarks>
        public const int TypeIdentity = 0;

        ///<summary>
        /// This flag bit indicates that the transform defined by this object
        /// performs a translation in addition to the conversions indicated
        /// by other flag bits.
        /// </summary>
        /// <remarks>
        /// A translation moves the coordinates by a constant amount in x
        /// and y without changing the length or angle of vectors.
        ///</remarks>
        public const int TypeTranslation = 1;

        /// <summary>
        /// This flag bit indicates that the transform defined by this object
        /// performs a uniform scale in addition to the conversions indicated
        /// by other flag bits.
        /// </summary>
        /// <remarks>
        /// A uniform scale multiplies the length of vectors by the same amount
        /// in both the x and y directions without changing the angle between
        /// vectors.
        /// This flag bit is mutually exclusive with the TypeGeneralScale flag.
        /// </remarks>
        public const int TypeUniformScale = 2;

        /// <summary>
        /// This flag bit indicates that the transform defined by this object
        /// performs a general scale in addition to the conversions indicated
        /// by other flag bits.
        /// </summary>
        /// <remarks>
        /// A general scale multiplies the length of vectors by different
        /// amounts in the x and y directions without changing the angle
        /// between perpendicular vectors.
        /// This flag bit is mutually exclusive with the TypeUniformScale flag.
        /// </remarks>
        public const int TypeGeneralScale = 4;

        ///<summary>
        /// This constant is a bit mask for any of the scale flag bits.
        ///</summary>
        public const int TypeMaskScale = (TypeUniformScale |
                                          TypeGeneralScale);


        /// <summary>
        /// This flag bit indicates that the transform defined by this object
        /// performs a mirror image flip about some axis which changes the
        /// normally right handed coordinate system into a left handed
        /// system in addition to the conversions indicated by other flag bits.
        /// </summary>
        /// <remarks>
        /// A right handed coordinate system is one where the positive X
        /// axis rotates counterclockwise to overlay the positive Y axis
        /// similar to the direction that the fingers on your right hand
        /// curl when you stare end on at your thumb.
        /// A left handed coordinate system is one where the positive X
        /// axis rotates clockwise to overlay the positive Y axis similar
        /// to the direction that the fingers on your left hand curl.
        /// There is no mathematical way to determine the angle of the
        /// original flipping or mirroring transformation since all angles
        /// of flip are identical given an appropriate adjusting rotation.
        /// </remarks>
        public const int TypeFlip = 64;

        /// <summary>
        /// This flag bit indicates that the transform defined by this object
        /// performs a quadrant rotation by some multiple of 90 degrees in
        /// addition to the conversions indicated by other flag bits.
        /// </summary>
        /// <remarks>
        /// A rotation changes the angles of vectors by the same amount
        /// regardless of the original direction of the vector and without
        /// changing the length of the vector.
        /// This flag bit is mutually exclusive with the TypeGeneralRotation flag.
        /// </remarks>
        public const int TypeQuadrantRotation = 8;

        /// <summary>
        /// This flag bit indicates that the transform defined by this object
        /// performs a rotation by an arbitrary angle in addition to the
        /// conversions indicated by other flag bits.
        /// </summary>
        /// <remarks>
        /// A rotation changes the angles of vectors by the same amount
        /// regardless of the original direction of the vector and without
        /// changing the length of the vector.
        /// This flag bit is mutually exclusive with the
        /// TypeQuadrantRotation flag.
        /// </remarks>
        public const int TypeGeneralRotation = 16;

        ///<summary>
        ///  This constant is a bit mask for any of the rotation flag bits.
        ///</summary>
        public const int TypeMaskRotation = (TypeQuadrantRotation |
                                             TypeGeneralRotation);

        /// <summary>
        /// This constant indicates that the transform defined by this object
        /// performs an arbitrary conversion of the input coordinates.
        /// </summary>
        /// <remarks>
        /// If this transform can be classified by any of the above constants,
        /// the type will either be the constant TypeIdentity or a
        /// combination of the appropriate flag bits for the various coordinate
        /// conversions that this transform performs.
        /// </remarks>
        public const int TypeGeneralTransform = 32;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses Transforms from a list of transform definitions,
        /// which are applied in the order provided.
        /// </summary>
        /// <param name="input">input transform string. i.e
        /// translate(-10,-20) scale(2) rotate(45) translate(5,10)</param>
        /// <returns>affine transform.</returns>
        public static AffineTransform FromString(string input)
        {
            lock (TransformListParser)
            {
                return TransformListParser.ParseTransformList(input);
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="AffineTransform"/> class.
        /// </summary>
        public AffineTransform()
        {
            _m00 = _m11 = 1.0;
            _m01 = _m10 = _m02 = _m12 = 0.0;
            _state = ApplyIdentity;
            _type = TypeIdentity;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="AffineTransform"/> class.
        /// </summary>
        /// <param name="tx">the AffineTransform object to copy</param>
        public AffineTransform(AffineTransform tx)
        {
            _m00 = tx._m00;
            _m10 = tx._m10;
            _m01 = tx._m01;
            _m11 = tx._m11;
            _m02 = tx._m02;
            _m12 = tx._m12;
            _state = tx._state;
            _type = tx._type;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a new AffineTransform from 6 double
        /// precision values representing the 6 specifiable entries of the 3x3
        /// transformation matrix.
        /// </summary>
        /// <param name="m00">the X coordinate scaling element of the 3x3 matrix</param>
        /// <param name="m10">the Y coordinate shearing element of the 3x3 matrix</param>
        /// <param name="m01">the X coordinate shearing element of the 3x3 matrix.</param>
        /// <param name="m11">the Y coordinate scaling element of the 3x3 matrix.</param>
        /// <param name="m02">X coordinate translation element of the 3x3 matrix.</param>
        /// <param name="m12">Y coordinate translation element of the 3x3 matrix.</param>
        public AffineTransform(double m00, double m10,
                               double m01, double m11,
                               double m02, double m12)
        {
            _m00 = m00;
            _m10 = m10;
            _m01 = m01;
            _m11 = m11;
            _m02 = m02;
            _m12 = m12;
            UpdateState();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a new AffineTransform from an array of
        /// double precision values representing either the 4 non-translation
        /// entries or the 6 specifiable entries of the 3x3 transformation
        /// matrix. The values are retrieved from the array as
        /// {m00 m10 m01 m11 [m02 m12]}.
        /// </summary>
        /// <param name="flatmatrix">the double array containing the values to be set
        /// in the new AffineTransform object. The length of the
        /// array is assumed to be at least 4. If the length of the array is
        /// less than 6, only the first 4 values are taken. If the length of
        /// the array is greater than 6, the first 6 values are taken.</param>
        public AffineTransform(double[] flatmatrix)
        {
            _m00 = flatmatrix[0];
            _m10 = flatmatrix[1];
            _m01 = flatmatrix[2];
            _m11 = flatmatrix[3];
            if (flatmatrix.Length > 5)
            {
                _m02 = flatmatrix[4];
                _m12 = flatmatrix[5];
            }
            UpdateState();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a transform representing a translation transformation.
        /// </summary>
        /// <remarks>
        /// The matrix representing the returned transform is:
        /// <pre>
        ///		[   1    0    tx  ]
        ///		[   0    1    ty  ]
        ///		[   0    0    1   ]
        /// </pre>
        /// </remarks>
        /// <param name="tx">the distance by which coordinates are translated in the
        /// X axis direction</param>
        /// <param name="ty">ty the distance by which coordinates are translated in the
        /// Y axis direction</param>
        /// <returns>an AffineTransform object that represents a
        /// translation transformation, created with the specified vector.</returns>
        public static AffineTransform GetTranslateInstance(double tx, double ty)
        {
            var trans = new AffineTransform();
            trans.SetToTranslation(tx, ty);
            return trans;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a transform representing a rotation transformation.
        /// </summary>
        /// <remarks>
        /// The matrix representing the returned transform is:
        /// <pre>
        ///		[   cos(theta)    -sin(theta)    0   ]
        ///		[   sin(theta)     cos(theta)    0   ]
        ///		[       0              0         1   ]
        /// </pre>
        /// Rotating by a positive angle theta rotates points on the positive
        /// X axis toward the positive Y axis.
        /// also the discussion of
        /// <a href="#quadrantapproximation">Handling 90-Degree Rotations</a>
        /// above.
        /// </remarks>
        /// <param name="theta">the angle of rotation measured in radians.</param>
        /// <returns>an AffineTransform object that is a rotation
        /// transformation, created with the specified angle of rotation</returns>
        public static AffineTransform GetRotateInstance(double theta)
        {
            var tx = new AffineTransform();
            tx.SetToRotation(theta);
            return tx;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a transform that rotates coordinates around an anchor point.
        /// </summary>
        /// <remarks>
        /// This operation is equivalent to translating the coordinates so
        /// that the anchor point is at the origin (S1), then rotating them
        /// about the new origin (S2), and finally translating so that the
        /// intermediate origin is restored to the coordinates of the original
        /// anchor point (S3).
        /// 
        /// This operation is equivalent to the following sequence of calls:
        /// <pre>
        ///     AffineTransform Tx = new AffineTransform();
        ///     Tx.translate(anchorx, anchory);    // S3: final translation
        ///     Tx.rotate(theta);		      // S2: rotate around anchor
        ///     Tx.translate(-anchorx, -anchory);  // S1: translate anchor to origin
        /// </pre>
        /// The matrix representing the returned transform is:
        /// <pre>
        ///		[   cos(theta)    -sin(theta)    x-x///cos+y///sin  ]
        ///		[   sin(theta)     cos(theta)    y-x///sin-y///cos  ]
        ///		[       0              0               1        ]
        /// </pre>
        /// Rotating by a positive angle theta rotates points on the positive
        /// X axis toward the positive Y axis.
        /// also the discussion of
        /// <a href="#quadrantapproximation">Handling 90-Degree Rotations</a>
        /// above.
        ///</remarks>
        /// <param name="theta">the angle of rotation measured in radians</param>
        /// <param name="anchorx">the X coordinate of the rotation anchor point</param>
        /// <param name="anchory">the Y coordinate of the rotation anchor point</param>
        /// <returns>an AffineTransform object that rotates
        /// coordinates around the specified point by the specified angle of
        /// rotation.</returns>
        public static AffineTransform GetRotateInstance(double theta,
                                                        double anchorx,
                                                        double anchory)
        {
            var tx = new AffineTransform();
            tx.SetToRotation(theta, anchorx, anchory);
            return tx;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a transform that rotates coordinates according to
        /// a rotation vector.
        /// </summary>
        /// <remarks>
        /// All coordinates rotate about the origin by the same amount.
        /// The amount of rotation is such that coordinates along the former
        /// positive X axis will subsequently align with the vector pointing
        /// from the origin to the specified vector coordinates.
        /// If both vecx and vecy are 0.0,
        /// an identity transform is returned.
        /// This operation is equivalent to calling:
        /// <pre>
        ///     AffineTransform.getRotateInstance(Math.atan2(vecy, vecx));
        /// </pre>
        /// </remarks>
        /// <param name="vecx">the X coordinate of the rotation vector</param>
        /// <param name="vecy">the Y coordinate of the rotation vector</param>
        /// <returns>an AffineTransform object that rotates
        /// coordinates according to the specified rotation vector.</returns>
        public static AffineTransform GetRotateInstance(double vecx, double vecy)
        {
            var tx = new AffineTransform();
            tx.SetToRotation(vecx, vecy);
            return tx;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a transform that rotates coordinates around an anchor
        /// point accordinate to a rotation vector.
        /// </summary>
        /// <remarks>
        /// All coordinates rotate about the specified anchor coordinates
        /// by the same amount.
        /// The amount of rotation is such that coordinates along the former
        /// positive X axis will subsequently align with the vector pointing
        /// from the origin to the specified vector coordinates.
        /// If both vecx and vecy are 0.0,
        /// an identity transform is returned.
        /// This operation is equivalent to calling:
        /// <pre>
        ///     AffineTransform.getRotateInstance(Math.atan2(vecy, vecx),
        ///                                       anchorx, anchory);
        /// </pre>
        ///</remarks>
        /// <param name="vecx">the X coordinate of the rotation vector.</param>
        /// <param name="vecy">the Y coordinate of the rotation vector.</param>
        /// <param name="anchorx">the X coordinate of the rotation anchor point.</param>
        /// <param name="anchory">the Y coordinate of the rotation anchor point.</param>
        /// <returns>an AffineTransform object that rotates
        /// coordinates around the specified point according to the
        /// specified rotation vector</returns>
        public static AffineTransform GetRotateInstance(double vecx,
                                                        double vecy,
                                                        double anchorx,
                                                        double anchory)
        {
            var tx = new AffineTransform();
            tx.SetToRotation(vecx, vecy, anchorx, anchory);
            return tx;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a transform that rotates coordinates by the specified
        /// number of quadrants.
        /// </summary>
        /// <remarks>
        /// This operation is equivalent to calling:
        /// <pre>
        ///     AffineTransform.getRotateInstance(numquadrants * Math.Pi / 2.0);
        /// </pre>
        /// Rotating by a positive number of quadrants rotates points on
        /// the positive X axis toward the positive Y axis.
        /// </remarks>
        /// <param name="numquadrants">the number of 90 degree arcs to rotate by</param>
        /// <returns>an AffineTransform object that rotates
        /// coordinates by the specified number of quadrants.</returns>
        public static AffineTransform GetQuadrantRotateInstance(int numquadrants)
        {
            var tx = new AffineTransform();
            tx.SetToQuadrantRotation(numquadrants);
            return tx;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a transform that rotates coordinates by the specified
        /// number of quadrants around the specified anchor point.
        /// </summary>
        /// <remarks>
        /// This operation is equivalent to calling:
        /// <pre>
        ///     AffineTransform.getRotateInstance(numquadrants * Math.Pi / 2.0,
        ///                                       anchorx, anchory);
        /// </pre>
        /// Rotating by a positive number of quadrants rotates points on
        /// the positive X axis toward the positive Y axis.
        /// </remarks>
        /// <param name="numquadrants">the number of 90 degree arcs to rotate by.</param>
        /// <param name="anchorx">the X coordinate of the rotation anchor point.</param>
        /// <param name="anchory">the Y coordinate of the rotation anchor point.</param>
        /// <returns>an AffineTransform object that rotates
        /// coordinates by the specified number of quadrants around the
        /// specified anchor point.</returns>
        public static AffineTransform GetQuadrantRotateInstance(int numquadrants,
                                                                double anchorx,
                                                                double anchory)
        {
            var tx = new AffineTransform();
            tx.SetToQuadrantRotation(numquadrants, anchorx, anchory);
            return tx;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a transform representing a scaling transformation.
        /// </summary>
        /// <remarks>
        /// The matrix representing the returned transform is:
        /// <pre>
        ///		[   sx   0    0   ]
        ///		[   0    sy   0   ]
        ///		[   0    0    1   ]
        /// </pre>
        /// </remarks>
        /// <param name="sx">the factor by which coordinates are scaled along the
        /// X axis direction</param>
        /// <param name="sy">the factor by which coordinates are scaled along the
        /// Y axis direction.</param>
        /// <returns>an AffineTransform object that scale
        /// coordinates by the specified multipliers.</returns>
        public static AffineTransform GetScaleInstance(double sx, double sy)
        {
            var tx = new AffineTransform();
            tx.SetToScale(sx, sy);
            return tx;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a transform representing a shearing transformation.
        /// </summary>
        /// <remarks>
        /// The matrix representing the returned transform is:
        /// <pre>
        ///		[   1   shx   0   ]
        ///		[  shy   1    0   ]
        ///		[   0    0    1   ]
        /// </pre>
        /// </remarks>
        /// <param name="shx">the multiplier by which coordinates are shifted in the
        /// direction of the positive X axis as a factor of their Y coordinate.</param>
        /// <param name="shy">the multiplier by which coordinates are shifted in the
        /// direction of the positive Y axis as a factor of their X coordinate.</param>
        /// <returns>an AffineTransform object that shears
        /// coordinates by the specified multipliers.</returns>
        public static AffineTransform GetShearInstance(double shx, double shy)
        {
            var tx = new AffineTransform();
            tx.SetToShear(shx, shy);
            return tx;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Retrieves the flag bits describing the conversion properties of
        /// this transform.
        /// </summary>
        /// <remarks>
        /// The return value is either one of the constants TypeIdentity
        /// or TypeGeneralTransform, or a combination of the appriopriate flag bits.
        /// A valid combination of flag bits is an exclusive OR operation
        /// that can combine the TypeTranslation flag bit in addition to either of the
        /// TypeUniformScale or TypeGeneralScale flag bits as well as either of the
        /// TypeQuadrantRotation or TypeGeneralRotation flag bits.
        /// </remarks>
        public int TransformType
        {
            get
            {
                if (_type == TypeUnknown)
                {
                    CalculateType();
                }
                return _type;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the determinant of the matrix representation of the transform.
        /// </summary>
        /// <remarks>
        /// The determinant is useful both to determine if the transform can
        /// be inverted and to get a single value representing the
        /// combined X and Y scaling of the transform.
        /// 
        /// If the determinant is non-zero, then this transform is
        /// invertible and the various methods that depend on the inverse
        /// transform do not need to throw a
        ///  NoninvertibleTransformException.
        /// If the determinant is zero then this transform can not be
        /// inverted since the transform maps all input coordinates onto
        /// a line or a point.
        /// If the determinant is near enough to zero then inverse transform
        /// operations might not carry enough precision to produce meaningful
        /// results.
        /// 
        /// If this transform represents a uniform scale, as indicated by
        /// the getType method then the determinant also
        /// represents the square of the uniform scale factor by which all of
        /// the points are expanded from or contracted towards the origin.
        /// If this transform represents a non-uniform scale or more general
        /// transform then the determinant is not likely to represent a
        /// value useful for any purpose other than determining if inverse
        /// transforms are possible.
        /// 
        /// Mathematically, the determinant is calculated using the formula:
        /// <pre>
        ///		|  m00  m01  m02  |
        ///		|  m10  m11  m12  |  =  m00 /// m11 - m01 /// m10
        ///		|   0    0    1   |
        /// </pre>
        /// </remarks>
        public double Determinant
        {
            get
            {
                switch (_state)
                {
                    default:
                        StateError();
                        return _m00*_m11 - _m01*_m10;
                    case (ApplyShear | ApplyScale | ApplyTranslate):
                        return _m00*_m11 - _m01*_m10;
                    case (ApplyShear | ApplyScale):
                        return _m00*_m11 - _m01*_m10;
                    case (ApplyShear | ApplyTranslate):
                    case (ApplyShear):
                        return -(_m01*_m10);
                    case (ApplyScale | ApplyTranslate):
                    case (ApplyScale):
                        return _m00*_m11;
                    case (ApplyTranslate):
                    case (ApplyIdentity):
                        return 1.0;
                }
            }
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Retrieves the 6 specifiable values in the 3x3 affine transformation
        /// matrix and places them into an array of double precisions values.
        /// </summary>
        /// <remarks>
        /// The values are stored in the array as
        /// {m00 m10 m01 m11 m02 m12 }.
        /// An array of 4 doubles can also be specified, in which case only the
        /// first four elements representing the non-transform
        /// parts of the array are retrieved and the values are stored into
        /// the array as { m00 m10 m01 m11 }
        /// </remarks>
        /// <param name="flatmatrix">the double array used to store the returned
        /// values.</param>
        public void GetMatrix(double[] flatmatrix)
        {
            flatmatrix[0] = _m00;
            flatmatrix[1] = _m10;
            flatmatrix[2] = _m01;
            flatmatrix[3] = _m11;
            if (flatmatrix.Length > 5)
            {
                flatmatrix[4] = _m02;
                flatmatrix[5] = _m12;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///Returns the X coordinate scaling element (m00) of the 3x3
        /// affine transformation matrix.
        /// </summary>
        public double ScaleX
        {
            get { return _m00; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the Y coordinate scaling element (m11) of the 3x3
        /// affine transformation matrix.
        /// </summary>
        public double ScaleY
        {
            get { return _m11; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Returns the X coordinate shearing element (m01) of the 3x3
        /// affine transformation matrix.
        /// </summary>
        public double ShearX
        {
            get { return _m01; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the Y coordinate shearing element (m10) of the 3x3
        /// affine transformation matrix.
        /// </summary>
        public double ShearY
        {
            get { return _m10; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the X coordinate of the translation element (m02) of the
        /// 3x3 affine transformation matrix.
        /// </summary>
        public double TranslateX
        {
            get { return _m02; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the Y coordinate of the translation element (m12) of the
        /// 3x3 affine transformation matrix.
        /// </summary>
        public double TranslateY
        {
            get { return _m12; }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Concatenates this transform with a translation transformation.
        /// </summary>
        /// <remarks>
        /// This is equivalent to calling concatenate(T), where T is an
        /// AffineTransform represented by the following matrix:
        /// <pre>
        ///		[   1    0    tx  ]
        ///		[   0    1    ty  ]
        ///		[   0    0    1   ]
        /// </pre>
        /// </remarks>
        /// <param name="tx">the distance by which coordinates are translated in the
        /// X axis direction.</param>
        /// <param name="ty">the distance by which coordinates are translated in the
        /// Y axis direction.</param>
        public void Translate(double tx, double ty)
        {
            switch (_state)
            {
                default:
                    StateError();
                    return;
                    /* NOTREACHED */
                case (ApplyShear | ApplyScale | ApplyTranslate):
                    _m02 = tx*_m00 + ty*_m01 + _m02;
                    _m12 = tx*_m10 + ty*_m11 + _m12;
                    if (_m02 == 0.0 && _m12 == 0.0)
                    {
                        _state = ApplyShear | ApplyScale;
                        if (_type != TypeUnknown)
                        {
                            _type -= TypeTranslation;
                        }
                    }
                    return;
                case (ApplyShear | ApplyScale):
                    _m02 = tx*_m00 + ty*_m01;
                    _m12 = tx*_m10 + ty*_m11;
                    if (_m02 != 0.0 || _m12 != 0.0)
                    {
                        _state = ApplyShear | ApplyScale | ApplyTranslate;
                        _type |= TypeTranslation;
                    }
                    return;
                case (ApplyShear | ApplyTranslate):
                    _m02 = ty*_m01 + _m02;
                    _m12 = tx*_m10 + _m12;
                    if (_m02 == 0.0 && _m12 == 0.0)
                    {
                        _state = ApplyShear;
                        if (_type != TypeUnknown)
                        {
                            _type -= TypeTranslation;
                        }
                    }
                    return;
                case (ApplyShear):
                    _m02 = ty*_m01;
                    _m12 = tx*_m10;
                    if (_m02 != 0.0 || _m12 != 0.0)
                    {
                        _state = ApplyShear | ApplyTranslate;
                        _type |= TypeTranslation;
                    }
                    return;
                case (ApplyScale | ApplyTranslate):
                    _m02 = tx*_m00 + _m02;
                    _m12 = ty*_m11 + _m12;
                    if (_m02 == 0.0 && _m12 == 0.0)
                    {
                        _state = ApplyScale;
                        if (_type != TypeUnknown)
                        {
                            _type -= TypeTranslation;
                        }
                    }
                    return;
                case (ApplyScale):
                    _m02 = tx*_m00;
                    _m12 = ty*_m11;
                    if (_m02 != 0.0 || _m12 != 0.0)
                    {
                        _state = ApplyScale | ApplyTranslate;
                        _type |= TypeTranslation;
                    }
                    return;
                case (ApplyTranslate):
                    _m02 = tx + _m02;
                    _m12 = ty + _m12;
                    if (_m02 == 0.0 && _m12 == 0.0)
                    {
                        _state = ApplyIdentity;
                        _type = TypeIdentity;
                    }
                    return;
                case (ApplyIdentity):
                    _m02 = tx;
                    _m12 = ty;
                    if (tx != 0.0 || ty != 0.0)
                    {
                        _state = ApplyTranslate;
                        _type = TypeTranslation;
                    }
                    return;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Concatenates this transform with a rotation transformation.
        /// </summary>
        /// <remarks>
        /// This is equivalent to calling concatenate(R), where R is an
        /// AffineTransform represented by the following matrix:
        /// <pre>
        ///		[   cos(theta)    -sin(theta)    0   ]
        ///		[   sin(theta)     cos(theta)    0   ]
        ///		[       0              0         1   ]
        /// </pre>
        /// Rotating by a positive angle theta rotates points on the positive
        /// X axis toward the positive Y axis.
        /// also the discussion of
        /// <a href="#quadrantapproximation">Handling 90-Degree Rotations</a>
        /// above.
        ///</remarks>
        /// <param name="theta">the angle of rotation measured in radians.</param>
        public void Rotate(double theta)
        {
            var sin = Math.Sin(theta);
            if (sin == 1.0)
            {
                Rotate90();
            }
            else if (sin == -1.0)
            {
                Rotate270();
            }
            else
            {
                var cos = Math.Cos(theta);
                var m1 = _m01;
                if (cos == -1.0)
                {
                    Rotate180();
                }
                else if (cos != 1.0)
                {
                    var m0 = _m00;
                    _m00 = cos*m0 + sin*m1;
                    _m01 = -sin*m0 + cos*m1;
                    m0 = _m10;
                    m1 = _m11;
                    _m10 = cos*m0 + sin*m1;
                    _m11 = -sin*m0 + cos*m1;
                    UpdateState();
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Concatenates this transform with a transform that rotates
        /// coordinates around an anchor point.
        /// </summary>
        /// <remarks>
        /// This operation is equivalent to translating the coordinates so
        /// that the anchor point is at the origin (S1), then rotating them
        /// about the new origin (S2), and finally translating so that the
        /// intermediate origin is restored to the coordinates of the original
        /// anchor point (S3).
        /// 
        /// This operation is equivalent to the following sequence of calls:
        /// <pre>
        ///     translate(anchorx, anchory);      // S3: final translation
        ///     rotate(theta);                    // S2: rotate around anchor
        ///     translate(-anchorx, -anchory);    // S1: translate anchor to origin
        /// </pre>
        /// Rotating by a positive angle theta rotates points on the positive
        /// X axis toward the positive Y axis.
        /// Remember also the discussion of
        /// <a href="#quadrantapproximation">Handling 90-Degree Rotations</a>
        /// above.
        /// </remarks>
        /// <param name="theta">the angle of rotation measured in radians.</param>
        /// <param name="anchorx">the X coordinate of the rotation anchor point.</param>
        /// <param name="anchory">the Y coordinate of the rotation anchor point.</param>
        public void Rotate(double theta, double anchorx, double anchory)
        {
            // REMIND: Simple for now - optimize later
            Translate(anchorx, anchory);
            Rotate(theta);
            Translate(-anchorx, -anchory);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Concatenates this transform with a transform that rotates
        /// coordinates according to a rotation vector.
        /// </summary>
        /// <remarks>
        /// All coordinates rotate about the origin by the same amount.
        /// The amount of rotation is such that coordinates along the former
        /// positive X axis will subsequently align with the vector pointing
        /// from the origin to the specified vector coordinates.
        /// If both vecx and vecy are 0.0,
        /// no additional rotation is added to this transform.
        /// This operation is equivalent to calling:
        /// <pre>
        ///          rotate(Math.atan2(vecy, vecx));
        /// </pre>
        /// </remarks>
        /// <param name="vecx">the X coordinate of the rotation vector.</param>
        /// <param name="vecy">the Y coordinate of the rotation vector.</param>
        public void Rotate(double vecx, double vecy)
        {
            if (vecy == 0.0)
            {
                if (vecx < 0.0)
                {
                    Rotate180();
                }
                // If vecx > 0.0 - no rotation
                // If vecx == 0.0 - undefined rotation - treat as no rotation
            }
            else if (vecx == 0.0)
            {
                if (vecy > 0.0)
                {
                    Rotate90();
                }
                else
                {
                    // vecy must be < 0.0
                    Rotate270();
                }
            }
            else
            {
                var len = Math.Sqrt(vecx*vecx + vecy*vecy);
                var sin = vecy/len;
                var cos = vecx/len;
                var m0 = _m00;
                var m1 = _m01;
                _m00 = cos*m0 + sin*m1;
                _m01 = -sin*m0 + cos*m1;
                m0 = _m10;
                m1 = _m11;
                _m10 = cos*m0 + sin*m1;
                _m11 = -sin*m0 + cos*m1;
                UpdateState();
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Concatenates this transform with a transform that rotates
        /// coordinates around an anchor point according to a rotation
        /// vector.
        /// </summary>
        /// <remarks>
        /// All coordinates rotate about the specified anchor coordinates
        /// by the same amount.
        /// The amount of rotation is such that coordinates along the former
        /// positive X axis will subsequently align with the vector pointing
        /// from the origin to the specified vector coordinates.
        /// If both vecx and vecy are 0.0,
        /// the transform is not modified in any way.
        /// This method is equivalent to calling:
        /// <pre>
        ///     rotate(Math.atan2(vecy, vecx), anchorx, anchory);
        /// </pre>
        /// </remarks>
        /// <param name="vecx">the X coordinate of the rotation vector.</param>
        /// <param name="vecy">the Y coordinate of the rotation vector.</param>
        /// <param name="anchorx">The  X coordinate of the rotation anchor point.</param>
        /// <param name="anchory">The Y coordinate of the rotation anchor point.</param>
        public void Rotate(double vecx, double vecy,
                           double anchorx, double anchory)
        {
            // REMIND: Simple for now - optimize later
            Translate(anchorx, anchory);
            Rotate(vecx, vecy);
            Translate(-anchorx, -anchory);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Concatenates this transform with a transform that rotates
        /// coordinates by the specified number of quadrants.
        /// </summary>
        /// <remarks>
        /// This is equivalent to calling:
        /// <pre>
        ///     rotate(numquadrants * Math.Pi / 2.0);
        /// </pre>
        /// Rotating by a positive number of quadrants rotates points on
        /// the positive X a
        /// </remarks>
        /// <param name="numquadrants">the number of 90 degree arcs to rotate by.</param>
        public void QuadrantRotate(int numquadrants)
        {
            switch (numquadrants & 3)
            {
                case 0:
                    break;
                case 1:
                    Rotate90();
                    break;
                case 2:
                    Rotate180();
                    break;
                case 3:
                    Rotate270();
                    break;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Concatenates this transform with a transform that rotates
        /// coordinates by the specified number of quadrants around
        /// the specified anchor point.
        /// </summary>
        /// <remarks>
        /// This method is equivalent to calling:
        /// <pre>
        ///     rotate(numquadrants * Math.Pi / 2.0, anchorx, anchory);
        /// </pre>
        /// Rotating by a positive number of quadrants rotates points on
        /// the positive X axis toward the positive Y axis.
        ///</remarks>
        /// <param name="numquadrants">the number of 90 degree arcs to rotate by.</param>
        /// <param name="anchorx">the X coordinate of the rotation anchor point.</param>
        /// <param name="anchory">the Y coordinate of the rotation anchor point.</param>
        public void QuadrantRotate(int numquadrants,
                                   double anchorx, double anchory)
        {
            switch (numquadrants & 3)
            {
                case 0:
                    return;
                case 1:
                    _m02 += anchorx*(_m00 - _m01) + anchory*(_m01 + _m00);
                    _m12 += anchorx*(_m10 - _m11) + anchory*(_m11 + _m10);
                    Rotate90();
                    break;
                case 2:
                    _m02 += anchorx*(_m00 + _m00) + anchory*(_m01 + _m01);
                    _m12 += anchorx*(_m10 + _m10) + anchory*(_m11 + _m11);
                    Rotate180();
                    break;
                case 3:
                    _m02 += anchorx*(_m00 + _m01) + anchory*(_m01 - _m00);
                    _m12 += anchorx*(_m10 + _m11) + anchory*(_m11 - _m10);
                    Rotate270();
                    break;
            }
            if (_m02 == 0.0 && _m12 == 0.0)
            {
                _state &= ~ApplyTranslate;
            }
            else
            {
                _state |= ApplyTranslate;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Concatenates this transform with a scaling transformation.
        /// </summary>
        /// <remarks>
        /// This is equivalent to calling concatenate(S), where S is an
        /// AffineTransform represented by the following matrix:
        /// <pre>
        ///		[   sx   0    0   ]
        ///		[   0    sy   0   ]
        ///		[   0    0    1   ]
        /// </pre>
        /// </remarks>
        /// <param name="sx">the factor by which coordinates are scaled along the
        /// X axis direction</param>
        /// <param name="sy">the factor by which coordinates are scaled along the
        /// Y axis direction.</param>
        public void Scale(double sx, double sy)
        {
            var currentState = _state;
            switch (currentState)
            {
                default:
                    StateError();
                    /* NOTREACHED */
                    return;
                case (ApplyShear | ApplyScale | ApplyTranslate):
                case (ApplyShear | ApplyScale):
                    _m00 *= sx;
                    _m11 *= sy;
                    if (_m01 == 0 && _m10 == 0)
                    {
                        currentState &= ApplyTranslate;
                        if (_m00 == 1.0 && _m11 == 1.0)
                        {
                            _type = (currentState == ApplyIdentity
                                         ? TypeIdentity
                                         : TypeTranslation);
                        }
                        else
                        {
                            currentState |= ApplyScale;
                            _type = TypeUnknown;
                        }
                        _state = currentState;
                    }
                    return;
                    /* NOBREAK */
                case (ApplyShear | ApplyTranslate):
                case (ApplyShear):
                    _m01 *= sy;
                    _m10 *= sx;
                    if (_m01 == 0 && _m10 == 0)
                    {
                        currentState &= ApplyTranslate;
                        if (_m00 == 1.0 && _m11 == 1.0)
                        {
                            _type = (currentState == ApplyIdentity
                                         ? TypeIdentity
                                         : TypeTranslation);
                        }
                        else
                        {
                            currentState |= ApplyScale;
                            _type = TypeUnknown;
                        }
                        _state = currentState;
                    }
                    return;
                case (ApplyScale | ApplyTranslate):
                case (ApplyScale):
                    _m00 *= sx;
                    _m11 *= sy;
                    if (_m00 == 1.0 && _m11 == 1.0)
                    {
                        _state = (currentState &= ApplyTranslate);
                        _type = (currentState == ApplyIdentity
                                     ? TypeIdentity
                                     : TypeTranslation);
                    }
                    else
                    {
                        _type = TypeUnknown;
                    }
                    return;
                case (ApplyTranslate):
                case (ApplyIdentity):
                    _m00 = sx;
                    _m11 = sy;
                    if (sx != 1.0 || sy != 1.0)
                    {
                        _state = currentState | ApplyScale;
                        _type = TypeUnknown;
                    }
                    return;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Concatenates this transform with a shearing transformation.
        /// </summary>
        /// <remarks>
        /// This is equivalent to calling concatenate(SH), where SH is an
        /// AffineTransform represented by the following matrix:
        /// <pre>
        ///		[   1   shx   0   ]
        ///		[  shy   1    0   ]
        ///		[   0    0    1   ]
        /// </pre>
        /// </remarks>
        /// <param name="shx">the multiplier by which coordinates are shifted in the
        /// direction of the positive X axis as a factor of their Y coordinate.</param>
        /// <param name="shy">the multiplier by which coordinates are shifted in the
        /// direction of the positive Y axis as a factor of their X coordinate.</param>
        public void Shear(double shx, double shy)
        {
            var currentState = _state;
            switch (currentState)
            {
                default:
                    StateError();
                    return;
                    /* NOTREACHED */
                case (ApplyShear | ApplyScale | ApplyTranslate):
                case (ApplyShear | ApplyScale):
                    double m0 = _m00;
                    double m1 = _m01;
                    _m00 = m0 + m1*shy;
                    _m01 = m0*shx + m1;

                    m0 = _m10;
                    m1 = _m11;
                    _m10 = m0 + m1*shy;
                    _m11 = m0*shx + m1;
                    UpdateState();
                    return;
                case (ApplyShear | ApplyTranslate):
                case (ApplyShear):
                    _m00 = _m01*shy;
                    _m11 = _m10*shx;
                    if (_m00 != 0.0 || _m11 != 0.0)
                    {
                        _state = currentState | ApplyScale;
                    }
                    _type = TypeUnknown;
                    return;
                case (ApplyScale | ApplyTranslate):
                case (ApplyScale):
                    _m01 = _m00*shx;
                    _m10 = _m11*shy;
                    if (_m01 != 0.0 || _m10 != 0.0)
                    {
                        _state = currentState | ApplyShear;
                    }
                    _type = TypeUnknown;
                    return;
                case (ApplyTranslate):
                case (ApplyIdentity):
                    _m01 = shx;
                    _m10 = shy;
                    if (_m01 != 0.0 || _m10 != 0.0)
                    {
                        _state = currentState | ApplyScale | ApplyShear;
                        _type = TypeUnknown;
                    }
                    return;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Resets this transform to the Identity transform.
        /// </summary>
        public void SetToIdentity()
        {
            _m00 = _m11 = 1.0;
            _m10 = _m01 = _m02 = _m12 = 0.0;
            _state = ApplyIdentity;
            _type = TypeIdentity;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets this transform to a translation transformation.
        /// </summary>
        /// <remarks>
        /// The matrix representing this transform becomes:
        /// <pre>
        ///		[   1    0    tx  ]
        ///		[   0    1    ty  ]
        ///		[   0    0    1   ]
        /// </pre>
        /// </remarks>
        /// <param name="tx">the distance by which coordinates are translated in the
        /// X axis direction.</param>
        /// <param name="ty">the distance by which coordinates are translated in the
        /// Y axis direction.</param>
        public void SetToTranslation(double tx, double ty)
        {
            _m00 = 1.0;
            _m10 = 0.0;
            _m01 = 0.0;
            _m11 = 1.0;
            _m02 = tx;
            _m12 = ty;
            if (tx != 0.0 || ty != 0.0)
            {
                _state = ApplyTranslate;
                _type = TypeTranslation;
            }
            else
            {
                _state = ApplyIdentity;
                _type = TypeIdentity;
            }
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets this transform to a rotation transformation.
        /// </summary>
        /// <remarks>
        /// The matrix representing this transform becomes:
        /// <pre>
        ///		[   cos(theta)    -sin(theta)    0   ]
        ///		[   sin(theta)     cos(theta)    0   ]
        ///		[       0              0         1   ]
        /// </pre>
        /// Rotating by a positive angle theta rotates points on the positive
        /// X axis toward the positive Y axis.
        /// also the discussion of
        /// <a href="#quadrantapproximation">Handling 90-Degree Rotations</a>
        /// above.
        /// </remarks>
        /// <param name="theta">the angle of rotation measured in radians</param>
        public void SetToRotation(double theta)
        {
            double sin = Math.Sin(theta);
            double cos;
            if (sin == 1.0 || sin == -1.0)
            {
                cos = 0.0;
                _state = ApplyShear;
                _type = TypeQuadrantRotation;
            }
            else
            {
                cos = Math.Cos(theta);
                if (cos == -1.0)
                {
                    sin = 0.0;
                    _state = ApplyScale;
                    _type = TypeQuadrantRotation;
                }
                else if (cos == 1.0)
                {
                    sin = 0.0;
                    _state = ApplyIdentity;
                    _type = TypeIdentity;
                }
                else
                {
                    _state = ApplyShear | ApplyScale;
                    _type = TypeGeneralRotation;
                }
            }
            _m00 = cos;
            _m10 = sin;
            _m01 = -sin;
            _m11 = cos;
            _m02 = 0.0;
            _m12 = 0.0;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets this transform to a translated rotation transformation.
        /// </summary>
        /// <remarks>
        /// This operation is equivalent to translating the coordinates so
        /// that the anchor point is at the origin (S1), then rotating them
        /// about the new origin (S2), and finally translating so that the
        /// intermediate origin is restored to the coordinates of the original
        /// anchor point (S3).
        /// 
        /// This operation is equivalent to the following sequence of calls:
        /// <pre>
        ///     setToTranslation(anchorx, anchory); // S3: final translation
        ///     rotate(theta);                      // S2: rotate around anchor
        ///     translate(-anchorx, -anchory);      // S1: translate anchor to origin
        /// </pre>
        /// The matrix representing this transform becomes:
        /// <pre>
        ///		[   cos(theta)    -sin(theta)    x-x///cos+y///sin  ]
        ///		[   sin(theta)     cos(theta)    y-x///sin-y///cos  ]
        ///		[       0              0               1        ]
        /// </pre>
        /// Rotating by a positive angle theta rotates points on the positive
        /// X axis toward the positive Y axis.
        /// also the discussion of
        /// <a href="#quadrantapproximation">Handling 90-Degree Rotations</a>
        /// above.
        /// </remarks>
        /// <param name="theta">the angle of rotation measured in radians.</param>
        /// <param name="anchorx">the X coordinate of the rotation anchor point.</param>
        /// <param name="anchory">the Y coordinate of the rotation anchor point.</param>
        public void SetToRotation(double theta, double anchorx, double anchory)
        {
            SetToRotation(theta);
            var sin = _m10;
            var oneMinusCos = 1.0 - _m00;
            _m02 = anchorx*oneMinusCos + anchory*sin;
            _m12 = anchory*oneMinusCos - anchorx*sin;
            if (_m02 != 0.0 || _m12 != 0.0)
            {
                _state |= ApplyTranslate;
                _type |= TypeTranslation;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets this transform to a rotation transformation that rotates
        /// coordinates according to a rotation vector.
        /// </summary>
        /// <remarks>
        /// All coordinates rotate about the origin by the same amount.
        /// The amount of rotation is such that coordinates along the former
        /// positive X axis will subsequently align with the vector pointing
        /// from the origin to the specified vector coordinates.
        /// If both vecx and vecy are 0.0,
        /// the transform is set to an identity transform.
        /// This operation is equivalent to calling:
        /// <pre>
        ///     setToRotation(Math.atan2(vecy, vecx));
        /// </pre>
        /// </remarks>
        /// <param name="vecx">the X coordinate of the rotation vector.</param>
        /// <param name="vecy">the Y coordinate of the rotation vector.</param>
        public void SetToRotation(double vecx, double vecy)
        {
            double sin, cos;
            if (vecy == 0)
            {
                sin = 0.0;
                if (vecx < 0.0)
                {
                    cos = -1.0;
                    _state = ApplyScale;
                    _type = TypeQuadrantRotation;
                }
                else
                {
                    cos = 1.0;
                    _state = ApplyIdentity;
                    _type = TypeIdentity;
                }
            }
            else if (vecx == 0)
            {
                cos = 0.0;
                sin = (vecy > 0.0) ? 1.0 : -1.0;
                _state = ApplyShear;
                _type = TypeQuadrantRotation;
            }
            else
            {
                double len = Math.Sqrt(vecx*vecx + vecy*vecy);
                cos = vecx/len;
                sin = vecy/len;
                _state = ApplyShear | ApplyScale;
                _type = TypeGeneralRotation;
            }
            _m00 = cos;
            _m10 = sin;
            _m01 = -sin;
            _m11 = cos;
            _m02 = 0.0;
            _m12 = 0.0;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets this transform to a rotation transformation that rotates
        /// coordinates around an anchor point according to a rotation
        /// vector.
        /// </summary>
        /// <remarks>
        /// All coordinates rotate about the specified anchor coordinates
        /// by the same amount.
        /// The amount of rotation is such that coordinates along the former
        /// positive X axis will subsequently align with the vector pointing
        /// from the origin to the specified vector coordinates.
        /// If both vecx and vecy are 0.0,
        /// the transform is set to an identity transform.
        /// This operation is equivalent to calling:
        /// <pre>
        ///     setToTranslation(Math.atan2(vecy, vecx), anchorx, anchory);
        /// </pre>
        /// </remarks>
        /// <param name="vecx">the X coordinate of the rotation vector</param>
        /// <param name="vecy">the Y coordinate of the rotation vector.</param>
        /// <param name="anchorx">the X coordinate of the rotation anchor point.</param>
        /// <param name="anchory">Tthe Y coordinate of the rotation anchor point.</param>
        public void SetToRotation(double vecx, double vecy,
                                  double anchorx, double anchory)
        {
            SetToRotation(vecx, vecy);
            var sin = _m10;
            var oneMinusCos = 1.0 - _m00;
            _m02 = anchorx*oneMinusCos + anchory*sin;
            _m12 = anchory*oneMinusCos - anchorx*sin;
            if (_m02 != 0.0 || _m12 != 0.0)
            {
                _state |= ApplyTranslate;
                _type |= TypeTranslation;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        //  --------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets this transform to a rotation transformation that rotates
        /// coordinates by the specified number of quadrants.
        /// </summary>
        /// <remarks>
        /// This operation is equivalent to calling:
        /// <pre>
        ///     setToRotation(numquadrants * Math.Pi / 2.0);
        /// </pre>
        /// Rotating by a positive number of quadrants rotates points on
        /// the positive X axis toward the positive Y axis.
        /// </remarks>
        /// <param name="numquadrants">the number of 90 degree arcs to rotate by.</param>
        public void SetToQuadrantRotation(int numquadrants)
        {
            switch (numquadrants & 3)
            {
                case 0:
                    _m00 = 1.0;
                    _m10 = 0.0;
                    _m01 = 0.0;
                    _m11 = 1.0;
                    _m02 = 0.0;
                    _m12 = 0.0;
                    _state = ApplyIdentity;
                    _type = TypeIdentity;
                    break;
                case 1:
                    _m00 = 0.0;
                    _m10 = 1.0;
                    _m01 = -1.0;
                    _m11 = 0.0;
                    _m02 = 0.0;
                    _m12 = 0.0;
                    _state = ApplyShear;
                    _type = TypeQuadrantRotation;
                    break;
                case 2:
                    _m00 = -1.0;
                    _m10 = 0.0;
                    _m01 = 0.0;
                    _m11 = -1.0;
                    _m02 = 0.0;
                    _m12 = 0.0;
                    _state = ApplyScale;
                    _type = TypeQuadrantRotation;
                    break;
                case 3:
                    _m00 = 0.0;
                    _m10 = -1.0;
                    _m01 = 1.0;
                    _m11 = 0.0;
                    _m02 = 0.0;
                    _m12 = 0.0;
                    _state = ApplyShear;
                    _type = TypeQuadrantRotation;
                    break;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets this transform to a translated rotation transformation
        /// that rotates coordinates by the specified number of quadrants
        /// around the specified anchor point.
        /// </summary>
        /// <remarks>
        /// This operation is equivalent to calling:
        /// <pre>
        ///     setToRotation(numquadrants * Math.Pi / 2.0, anchorx, anchory);
        /// </pre>
        /// Rotating by a positive number of quadrants rotates points on
        /// the positive X axis toward the positive Y axis.
        /// </remarks>
        /// <param name="numquadrants">the number of 90 degree arcs to rotate by.</param>
        /// <param name="anchorx">the X coordinate of the rotation anchor point.</param>
        /// <param name="anchory">the Y coordinate of the rotation anchor point.</param>
        public void SetToQuadrantRotation(int numquadrants,
                                          double anchorx, double anchory)
        {
            switch (numquadrants & 3)
            {
                case 0:
                    _m00 = 1.0;
                    _m10 = 0.0;
                    _m01 = 0.0;
                    _m11 = 1.0;
                    _m02 = 0.0;
                    _m12 = 0.0;
                    _state = ApplyIdentity;
                    _type = TypeIdentity;
                    break;
                case 1:
                    _m00 = 0.0;
                    _m10 = 1.0;
                    _m01 = -1.0;
                    _m11 = 0.0;
                    _m02 = anchorx + anchory;
                    _m12 = anchory - anchorx;
                    if (_m02 == 0.0 && _m12 == 0.0)
                    {
                        _state = ApplyShear;
                        _type = TypeQuadrantRotation;
                    }
                    else
                    {
                        _state = ApplyShear | ApplyTranslate;
                        _type = TypeQuadrantRotation | TypeTranslation;
                    }
                    break;
                case 2:
                    _m00 = -1.0;
                    _m10 = 0.0;
                    _m01 = 0.0;
                    _m11 = -1.0;
                    _m02 = anchorx + anchorx;
                    _m12 = anchory + anchory;
                    if (_m02 == 0.0 && _m12 == 0.0)
                    {
                        _state = ApplyScale;
                        _type = TypeQuadrantRotation;
                    }
                    else
                    {
                        _state = ApplyScale | ApplyTranslate;
                        _type = TypeQuadrantRotation | TypeTranslation;
                    }
                    break;
                case 3:
                    _m00 = 0.0;
                    _m10 = -1.0;
                    _m01 = 1.0;
                    _m11 = 0.0;
                    _m02 = anchorx - anchory;
                    _m12 = anchory + anchorx;
                    if (_m02 == 0.0 && _m12 == 0.0)
                    {
                        _state = ApplyShear;
                        _type = TypeQuadrantRotation;
                    }
                    else
                    {
                        _state = ApplyShear | ApplyTranslate;
                        _type = TypeQuadrantRotation | TypeTranslation;
                    }
                    break;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets this transform to a scaling transformation.
        /// </summary>
        /// <remarks>
        /// The matrix representing this transform becomes:
        /// <pre>
        ///		[   sx   0    0   ]
        ///		[   0    sy   0   ]
        ///		[   0    0    1   ]
        /// </pre>
        /// </remarks>
        /// <param name="sx">the factor by which coordinates are scaled along the
        /// X axis direction.</param>
        /// <param name="sy">the factor by which coordinates are scaled along the
        /// Y axis direction.</param>
        public void SetToScale(double sx, double sy)
        {
            _m00 = sx;
            _m10 = 0.0;
            _m01 = 0.0;
            _m11 = sy;
            _m02 = 0.0;
            _m12 = 0.0;
            if (sx != 1.0 || sy != 1.0)
            {
                _state = ApplyScale;
                _type = TypeUnknown;
            }
            else
            {
                _state = ApplyIdentity;
                _type = TypeIdentity;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets this transform to a shearing transformation.
        /// </summary>
        /// <remarks>
        /// The matrix representing this transform becomes:
        /// <pre>
        ///		[   1   shx   0   ]
        ///		[  shy   1    0   ]
        ///		[   0    0    1   ]
        /// </pre>
        /// </remarks>
        /// <param name="shx">the multiplier by which coordinates are shifted in the
        /// direction of the positive X axis as a factor of their Y coordinate.</param>
        /// <param name="shy">the multiplier by which coordinates are shifted in the
        /// direction of the positive Y axis as a factor of their X coordinate.</param>
        public void SetToShear(double shx, double shy)
        {
            _m00 = 1.0;
            _m01 = shx;
            _m10 = shy;
            _m11 = 1.0;
            _m02 = 0.0;
            _m12 = 0.0;
            if (shx != 0.0 || shy != 0.0)
            {
                _state = (ApplyShear | ApplyScale);
                _type = TypeUnknown;
            }
            else
            {
                _state = ApplyIdentity;
                _type = TypeIdentity;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets this transform to a copy of the transform in the specified
        /// AffineTransform object.
        /// </summary>
        /// <param name="tx">the AffineTransform object from which to
        /// copy the transform.</param>
        public void SetTransform(AffineTransform tx)
        {
            _m00 = tx._m00;
            _m10 = tx._m10;
            _m01 = tx._m01;
            _m11 = tx._m11;
            _m02 = tx._m02;
            _m12 = tx._m12;
            _state = tx._state;
            _type = tx._type;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets this transform to the matrix specified by the 6
        /// double precision values.
        /// </summary>
        /// <param name="m00">the X coordinate scaling element of the 3x3 matrix.</param>
        /// <param name="m10">the Y coordinate shearing element of the 3x3 matrix.</param>
        /// <param name="m01">the X coordinate shearing element of the 3x3 matrix.</param>
        /// <param name="m11">the Y coordinate scaling element of the 3x3 matrix.</param>
        /// <param name="m02">the X coordinate translation element of the 3x3 matrix.</param>
        /// <param name="m12">the Y coordinate translation element of the 3x3 matrix.</param>
        public void SetTransform(double m00, double m10,
                                 double m01, double m11,
                                 double m02, double m12)
        {
            _m00 = m00;
            _m10 = m10;
            _m01 = m01;
            _m11 = m11;
            _m02 = m02;
            _m12 = m12;
            UpdateState();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Concatenates an AffineTransform Tx to
        /// this AffineTransform Cx in the most commonly useful
        /// way to provide a new user space
        /// that is mapped to the former user space by Tx.
        /// </summary>
        /// <remarks>
        /// Cx is updated to perform the combined transformation.
        /// Transforming a point p by the updated transform Cx' is
        /// equivalent to first transforming p by Tx and then
        /// transforming the result by the original transform Cx like this:
        /// Cx'(p) = Cx(Tx(p))
        /// In matrix notation, if this transform Cx is
        /// represented by the matrix [this] and Tx is represented
        /// by the matrix [Tx] then this method does the following:
        /// <pre>
        ///		[this] = [this] x [Tx]
        /// </pre>
        /// </remarks>
        /// <param name="tx">the AffineTransform object to be
        /// concatenated with this AffineTransform object..</param>
        public void Concatenate(AffineTransform tx)
        {
            double m0, m1;
            double t01, t10;
            int mystate = _state;
            int txstate = tx._state;
            switch ((txstate << HiShift) | mystate)
            {
                    /* ---------- Tx == IDENTITY cases ---------- */
                case (HiIdentity | ApplyIdentity):
                case (HiIdentity | ApplyTranslate):
                case (HiIdentity | ApplyScale):
                case (HiIdentity | ApplyScale | ApplyTranslate):
                case (HiIdentity | ApplyShear):
                case (HiIdentity | ApplyShear | ApplyTranslate):
                case (HiIdentity | ApplyShear | ApplyScale):
                case (HiIdentity | ApplyShear | ApplyScale | ApplyTranslate):
                    return;

                    /* ---------- this == IDENTITY cases ---------- */
                case (HiShear | HiScale | HiTranslate | ApplyIdentity):
                    _m01 = tx._m01;
                    _m10 = tx._m10;
                    _m00 = tx._m00;
                    _m11 = tx._m11;
                    _m02 = tx._m02;
                    _m12 = tx._m12;
                    _state = txstate;
                    _type = tx._type;
                    return;

                case (HiScale | HiTranslate | ApplyIdentity):
                    _m00 = tx._m00;
                    _m11 = tx._m11;
                    _m02 = tx._m02;
                    _m12 = tx._m12;
                    _state = txstate;
                    _type = tx._type;
                    return;
                case (HiTranslate | ApplyIdentity):
                    _m02 = tx._m02;
                    _m12 = tx._m12;
                    _state = txstate;
                    _type = tx._type;
                    return;
                case (HiShear | HiScale | ApplyIdentity):
                    _m01 = tx._m01;
                    _m10 = tx._m10;
                    _m00 = tx._m00;
                    _m11 = tx._m11;
                    _state = txstate;
                    _type = tx._type;
                    return;
                case (HiScale | ApplyIdentity):
                    _m00 = tx._m00;
                    _m11 = tx._m11;
                    _state = txstate;
                    _type = tx._type;
                    return;
                case (HiShear | HiTranslate | ApplyIdentity):
                    _m02 = tx._m02;
                    _m12 = tx._m12;
                    _m01 = tx._m01;
                    _m10 = tx._m10;
                    _m00 = _m11 = 0.0;
                    _state = txstate;
                    _type = tx._type;
                    return;
                case (HiShear | ApplyIdentity):
                    _m01 = tx._m01;
                    _m10 = tx._m10;
                    _m00 = _m11 = 0.0;
                    _state = txstate;
                    _type = tx._type;
                    return;

                    /* ---------- Tx == TRANSLATE cases ---------- */
                case (HiTranslate | ApplyShear | ApplyScale | ApplyTranslate):
                case (HiTranslate | ApplyShear | ApplyScale):
                case (HiTranslate | ApplyShear | ApplyTranslate):
                case (HiTranslate | ApplyShear):
                case (HiTranslate | ApplyScale | ApplyTranslate):
                case (HiTranslate | ApplyScale):
                case (HiTranslate | ApplyTranslate):
                    Translate(tx._m02, tx._m12);
                    return;

                    /* ---------- Tx == SCALE cases ---------- */
                case (HiScale | ApplyShear | ApplyScale | ApplyTranslate):
                case (HiScale | ApplyShear | ApplyScale):
                case (HiScale | ApplyShear | ApplyTranslate):
                case (HiScale | ApplyShear):
                case (HiScale | ApplyScale | ApplyTranslate):
                case (HiScale | ApplyScale):
                case (HiScale | ApplyTranslate):
                    Scale(tx._m00, tx._m11);
                    return;

                    /* ---------- Tx == SHEAR cases ---------- */
                case (HiShear | ApplyShear | ApplyScale | ApplyTranslate):
                case (HiShear | ApplyShear | ApplyScale):
                    t01 = tx._m01;
                    t10 = tx._m10;
                    m0 = _m00;
                    _m00 = _m01*t10;
                    _m01 = m0*t01;
                    m0 = _m10;
                    _m10 = _m11*t10;
                    _m11 = m0*t01;
                    _type = TypeUnknown;
                    return;
                case (HiShear | ApplyShear | ApplyTranslate):
                case (HiShear | ApplyShear):
                    _m00 = _m01*tx._m10;
                    _m01 = 0.0;
                    _m11 = _m10*tx._m01;
                    _m10 = 0.0;
                    _state = mystate ^ (ApplyShear | ApplyScale);
                    _type = TypeUnknown;
                    return;
                case (HiShear | ApplyScale | ApplyTranslate):
                case (HiShear | ApplyScale):
                    _m01 = _m00*tx._m01;
                    _m00 = 0.0;
                    _m10 = _m11*tx._m10;
                    _m11 = 0.0;
                    _state = mystate ^ (ApplyShear | ApplyScale);
                    _type = TypeUnknown;
                    return;
                case (HiShear | ApplyTranslate):
                    _m00 = 0.0;
                    _m01 = tx._m01;
                    _m10 = tx._m10;
                    _m11 = 0.0;
                    _state = ApplyTranslate | ApplyShear;
                    _type = TypeUnknown;
                    return;
            }
            // If Tx has more than one attribute, it is not worth optimizing
            // all of those cases...
            double t00 = tx._m00;
            t01 = tx._m01;
            double t02 = tx._m02;
            t10 = tx._m10;
            double t11 = tx._m11;
            double t12 = tx._m12;
            switch (mystate)
            {
                default:
                    StateError();
                    return;
                case (ApplyShear | ApplyScale):
                    _state = mystate | txstate;
                    m0 = _m00;
                    m1 = _m01;
                    _m00 = t00*m0 + t10*m1;
                    _m01 = t01*m0 + t11*m1;
                    _m02 += t02*m0 + t12*m1;

                    m0 = _m10;
                    m1 = _m11;
                    _m10 = t00*m0 + t10*m1;
                    _m11 = t01*m0 + t11*m1;
                    _m12 += t02*m0 + t12*m1;
                    _type = TypeUnknown;
                    return;
                case (ApplyShear | ApplyScale | ApplyTranslate):
                    m0 = _m00;
                    m1 = _m01;
                    _m00 = t00*m0 + t10*m1;
                    _m01 = t01*m0 + t11*m1;
                    _m02 += t02*m0 + t12*m1;

                    m0 = _m10;
                    m1 = _m11;
                    _m10 = t00*m0 + t10*m1;
                    _m11 = t01*m0 + t11*m1;
                    _m12 += t02*m0 + t12*m1;
                    _type = TypeUnknown;
                    return;

                case (ApplyShear | ApplyTranslate):
                case (ApplyShear):
                    m0 = _m01;
                    _m00 = t10*m0;
                    _m01 = t11*m0;
                    _m02 += t12*m0;

                    m0 = _m10;
                    _m10 = t00*m0;
                    _m11 = t01*m0;
                    _m12 += t02*m0;
                    break;

                case (ApplyScale | ApplyTranslate):
                case (ApplyScale):
                    m0 = _m00;
                    _m00 = t00*m0;
                    _m01 = t01*m0;
                    _m02 += t02*m0;

                    m0 = _m11;
                    _m10 = t10*m0;
                    _m11 = t11*m0;
                    _m12 += t12*m0;
                    break;

                case (ApplyTranslate):
                    _m00 = t00;
                    _m01 = t01;
                    _m02 += t02;

                    _m10 = t10;
                    _m11 = t11;
                    _m12 += t12;
                    _state = txstate | ApplyTranslate;
                    _type = TypeUnknown;
                    return;
            }
            UpdateState();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Concatenates an AffineTransform Tx to
        /// this AffineTransform Cx
        /// in a less commonly used way such that Tx modifies the
        /// coordinate transformation relative to the absolute pixel
        /// space rather than relative to the existing user space.
        /// </summary>
        /// <remarks>
        /// Cx is updated to perform the combined transformation.
        /// Transforming a point p by the updated transform Cx' is
        /// equivalent to first transforming p by the original transform
        /// Cx and then transforming the result by
        /// Tx like this:
        /// Cx'(p) = Tx(Cx(p))
        /// In matrix notation, if this transform Cx
        /// is represented by the matrix [this] and Tx is
        /// represented by the matrix [Tx] then this method does the
        /// following:
        /// <pre>
        ///		[this] = [Tx] x [this]
        /// </pre>
        /// </remarks>
        /// <param name="tx">the AffineTransform object to be
        /// concatenated with this AffineTransform object.</param>
        public void PreConcatenate(AffineTransform tx)
        {
            double m0;
            double t00, t01, t10, t11;
            int mystate = _state;
            int txstate = tx._state;
            switch ((txstate << HiShift) | mystate)
            {
                case (HiIdentity | ApplyIdentity):
                case (HiIdentity | ApplyTranslate):
                case (HiIdentity | ApplyScale):
                case (HiIdentity | ApplyScale | ApplyTranslate):
                case (HiIdentity | ApplyShear):
                case (HiIdentity | ApplyShear | ApplyTranslate):
                case (HiIdentity | ApplyShear | ApplyScale):
                case (HiIdentity | ApplyShear | ApplyScale | ApplyTranslate):
                    // Tx is IDENTITY...
                    return;

                case (HiTranslate | ApplyIdentity):
                case (HiTranslate | ApplyScale):
                case (HiTranslate | ApplyShear):
                case (HiTranslate | ApplyShear | ApplyScale):
                    // Tx is TRANSLATE, this has no TRANSLATE
                    _m02 = tx._m02;
                    _m12 = tx._m12;
                    _state = mystate | ApplyTranslate;
                    _type |= TypeTranslation;
                    return;

                case (HiTranslate | ApplyTranslate):
                case (HiTranslate | ApplyScale | ApplyTranslate):
                case (HiTranslate | ApplyShear | ApplyTranslate):
                case (HiTranslate | ApplyShear | ApplyScale | ApplyTranslate):
                    // Tx is TRANSLATE, this has one too
                    _m02 = _m02 + tx._m02;
                    _m12 = _m12 + tx._m12;
                    return;

                case (HiScale | ApplyTranslate):
                case (HiScale | ApplyIdentity):
                    // Only these two existing states need a new state
                    _state = mystate | ApplyScale;
                    t00 = tx._m00;
                    t11 = tx._m11;
                    if ((mystate & ApplyShear) != 0)
                    {
                        _m01 = _m01*t00;
                        _m10 = _m10*t11;
                        if ((mystate & ApplyScale) != 0)
                        {
                            _m00 = _m00*t00;
                            _m11 = _m11*t11;
                        }
                    }
                    else
                    {
                        _m00 = _m00*t00;
                        _m11 = _m11*t11;
                    }
                    if ((mystate & ApplyTranslate) != 0)
                    {
                        _m02 = _m02*t00;
                        _m12 = _m12*t11;
                    }
                    _type = TypeUnknown;
                    return;
                case (HiScale | ApplyShear | ApplyScale | ApplyTranslate):
                case (HiScale | ApplyShear | ApplyScale):
                case (HiScale | ApplyShear | ApplyTranslate):
                case (HiScale | ApplyShear):
                case (HiScale | ApplyScale | ApplyTranslate):
                case (HiScale | ApplyScale):
                    // Tx is SCALE, this is anything
                    t00 = tx._m00;
                    t11 = tx._m11;
                    if ((mystate & ApplyShear) != 0)
                    {
                        _m01 = _m01*t00;
                        _m10 = _m10*t11;
                        if ((mystate & ApplyScale) != 0)
                        {
                            _m00 = _m00*t00;
                            _m11 = _m11*t11;
                        }
                    }
                    else
                    {
                        _m00 = _m00*t00;
                        _m11 = _m11*t11;
                    }
                    if ((mystate & ApplyTranslate) != 0)
                    {
                        _m02 = _m02*t00;
                        _m12 = _m12*t11;
                    }
                    _type = TypeUnknown;
                    return;
                case (HiShear | ApplyShear | ApplyTranslate):
                case (HiShear | ApplyShear):
                    mystate = mystate | ApplyScale;
                    _state = mystate ^ ApplyShear;
                    // Tx is SHEAR, this is anything
                    t01 = tx._m01;
                    t10 = tx._m10;

                    m0 = _m00;
                    _m00 = _m10*t01;
                    _m10 = m0*t10;

                    m0 = _m01;
                    _m01 = _m11*t01;
                    _m11 = m0*t10;

                    m0 = _m02;
                    _m02 = _m12*t01;
                    _m12 = m0*t10;
                    _type = TypeUnknown;
                    return;
                case (HiShear | ApplyTranslate):
                case (HiShear | ApplyIdentity):
                case (HiShear | ApplyScale | ApplyTranslate):
                case (HiShear | ApplyScale):
                    _state = mystate ^ ApplyShear;
                    // Tx is SHEAR, this is anything
                    t01 = tx._m01;
                    t10 = tx._m10;

                    m0 = _m00;
                    _m00 = _m10*t01;
                    _m10 = m0*t10;

                    m0 = _m01;
                    _m01 = _m11*t01;
                    _m11 = m0*t10;

                    m0 = _m02;
                    _m02 = _m12*t01;
                    _m12 = m0*t10;
                    _type = TypeUnknown;
                    return;
                case (HiShear | ApplyShear | ApplyScale | ApplyTranslate):
                case (HiShear | ApplyShear | ApplyScale):
                    // Tx is SHEAR, this is anything
                    t01 = tx._m01;
                    t10 = tx._m10;

                    m0 = _m00;
                    _m00 = _m10*t01;
                    _m10 = m0*t10;

                    m0 = _m01;
                    _m01 = _m11*t01;
                    _m11 = m0*t10;

                    m0 = _m02;
                    _m02 = _m12*t01;
                    _m12 = m0*t10;
                    _type = TypeUnknown;
                    return;
            }
            // If Tx has more than one attribute, it is not worth optimizing
            // all of those cases...
            t00 = tx._m00;
            t01 = tx._m01;
            double t02 = tx._m02;
            t10 = tx._m10;
            t11 = tx._m11;
            double t12 = tx._m12;
            switch (mystate)
            {
                default:
                    StateError();
                    break;
                case (ApplyShear | ApplyScale | ApplyTranslate):
                    m0 = _m02;
                    double m1 = _m12;
                    t02 += m0*t00 + m1*t01;
                    t12 += m0*t10 + m1*t11;

                    _m02 = t02;
                    _m12 = t12;

                    m0 = _m00;
                    m1 = _m10;
                    _m00 = m0*t00 + m1*t01;
                    _m10 = m0*t10 + m1*t11;

                    m0 = _m01;
                    m1 = _m11;
                    _m01 = m0*t00 + m1*t01;
                    _m11 = m0*t10 + m1*t11;
                    break;
                case (ApplyShear | ApplyScale):
                    _m02 = t02;
                    _m12 = t12;

                    m0 = _m00;
                    m1 = _m10;
                    _m00 = m0*t00 + m1*t01;
                    _m10 = m0*t10 + m1*t11;

                    m0 = _m01;
                    m1 = _m11;
                    _m01 = m0*t00 + m1*t01;
                    _m11 = m0*t10 + m1*t11;
                    break;

                case (ApplyShear | ApplyTranslate):
                    m0 = _m02;
                    m1 = _m12;
                    t02 += m0*t00 + m1*t01;
                    t12 += m0*t10 + m1*t11;

                    _m02 = t02;
                    _m12 = t12;

                    m0 = _m10;
                    _m00 = m0*t01;
                    _m10 = m0*t11;

                    m0 = _m01;
                    _m01 = m0*t00;
                    _m11 = m0*t10;
                    break;
                case (ApplyShear):
                    _m02 = t02;
                    _m12 = t12;

                    m0 = _m10;
                    _m00 = m0*t01;
                    _m10 = m0*t11;

                    m0 = _m01;
                    _m01 = m0*t00;
                    _m11 = m0*t10;
                    break;

                case (ApplyScale | ApplyTranslate):
                    m0 = _m02;
                    m1 = _m12;
                    t02 += m0*t00 + m1*t01;
                    t12 += m0*t10 + m1*t11;

                    _m02 = t02;
                    _m12 = t12;

                    m0 = _m00;
                    _m00 = m0*t00;
                    _m10 = m0*t10;

                    m0 = _m11;
                    _m01 = m0*t01;
                    _m11 = m0*t11;
                    break;
                case (ApplyScale):
                    _m02 = t02;
                    _m12 = t12;

                    m0 = _m00;
                    _m00 = m0*t00;
                    _m10 = m0*t10;

                    m0 = _m11;
                    _m01 = m0*t01;
                    _m11 = m0*t11;
                    break;

                case (ApplyTranslate):
                    m0 = _m02;
                    m1 = _m12;
                    t02 += m0*t00 + m1*t01;
                    t12 += m0*t10 + m1*t11;

                    _m02 = t02;
                    _m12 = t12;

                    _m00 = t00;
                    _m10 = t10;

                    _m01 = t01;
                    _m11 = t11;

                    _state = mystate | txstate;
                    _type = TypeUnknown;
                    return;
                case (ApplyIdentity):
                    _m02 = t02;
                    _m12 = t12;

                    _m00 = t00;
                    _m10 = t10;

                    _m01 = t01;
                    _m11 = t11;

                    _state = mystate | txstate;
                    _type = TypeUnknown;
                    return;
            }
            UpdateState();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns an AffineTransform object representing the
        /// inverse transformation.
        /// </summary>
        /// <remarks>
        /// The inverse transform Tx' of this transform Tx
        /// maps coordinates transformed by Tx back
        /// to their original coordinates.
        /// In other words, Tx'(Tx(p)) = p = Tx(Tx'(p)).
        /// 
        /// If this transform maps all coordinates onto a point or a line
        /// then it will not have an inverse, since coordinates that do
        /// not lie on the destination point or line will not have an inverse
        /// mapping.
        /// The getDeterminant method can be used to determine if this
        /// transform has no inverse, in which case an exception will be
        /// thrown if the createInverse method is called.
        /// </remarks>
        /// <returns>a new AffineTransform object representing the
        /// inverse transformation.</returns>
        public AffineTransform CreateInverse()
        {
            double det;
            switch (_state)
            {
                default:
                    StateError();
                    return null;
                case (ApplyShear | ApplyScale | ApplyTranslate):
                    det = _m00*_m11 - _m01*_m10;
                    if (Math.Abs(det) <= double.MinValue)
                    {
                        throw new NoninvertibleTransformException("Determinant is " +
                                                                  det);
                    }
                    return new AffineTransform(_m11/det, -_m10/det,
                                               -_m01/det, _m00/det,
                                               (_m01*_m12 - _m11*_m02)/det,
                                               (_m10*_m02 - _m00*_m12)/det,
                                               (ApplyShear |
                                                ApplyScale |
                                                ApplyTranslate));
                case (ApplyShear | ApplyScale):
                    det = _m00*_m11 - _m01*_m10;
                    if (Math.Abs(det) <= double.MinValue)
                    {
                        throw new NoninvertibleTransformException("Determinant is " +
                                                                  det);
                    }
                    return new AffineTransform(_m11/det, -_m10/det,
                                               -_m01/det, _m00/det,
                                               0.0, 0.0,
                                               (ApplyShear | ApplyScale));
                case (ApplyShear | ApplyTranslate):
                    if (_m01 == 0.0 || _m10 == 0.0)
                    {
                        throw new NoninvertibleTransformException("Determinant is 0");
                    }
                    return new AffineTransform(0.0, 1.0/_m01,
                                               1.0/_m10, 0.0,
                                               -_m12/_m10, -_m02/_m01,
                                               (ApplyShear | ApplyTranslate));
                case (ApplyShear):
                    if (_m01 == 0.0 || _m10 == 0.0)
                    {
                        throw new NoninvertibleTransformException("Determinant is 0");
                    }
                    return new AffineTransform(0.0, 1.0/_m01,
                                               1.0/_m10, 0.0,
                                               0.0, 0.0,
                                               (ApplyShear));
                case (ApplyScale | ApplyTranslate):
                    if (_m00 == 0.0 || _m11 == 0.0)
                    {
                        throw new NoninvertibleTransformException("Determinant is 0");
                    }
                    return new AffineTransform(1.0/_m00, 0.0,
                                               0.0, 1.0/_m11,
                                               -_m02/_m00, -_m12/_m11,
                                               (ApplyScale | ApplyTranslate));
                case (ApplyScale):
                    if (_m00 == 0.0 || _m11 == 0.0)
                    {
                        throw new NoninvertibleTransformException("Determinant is 0");
                    }
                    return new AffineTransform(1.0/_m00, 0.0,
                                               0.0, 1.0/_m11,
                                               0.0, 0.0,
                                               (ApplyScale));
                case (ApplyTranslate):
                    return new AffineTransform(1.0, 0.0,
                                               0.0, 1.0,
                                               -_m02, -_m12,
                                               (ApplyTranslate));
                case (ApplyIdentity):
                    return new AffineTransform();
            }

            /* NOTREACHED */
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets this transform to the inverse of itself.
        /// The inverse transform Tx' of this transform Tx
        /// maps coordinates transformed by Tx back
        /// to their original coordinates.
        /// </summary>
        /// <remarks>
        /// In other words, Tx'(Tx(p)) = p = Tx(Tx'(p)).
        /// 
        /// If this transform maps all coordinates onto a point or a line
        /// then it will not have an inverse, since coordinates that do
        /// not lie on the destination point or line will not have an inverse
        /// mapping.
        /// The getDeterminant method can be used to determine if this
        /// transform has no inverse, in which case an exception will be
        /// thrown if the invert method is called.
        /// </remarks>
        public void Invert()
        {
            double m00, m01, m02;
            double m10, m11, m12;
            double det;
            switch (_state)
            {
                default:
                    StateError();
                    break;
                case (ApplyShear | ApplyScale | ApplyTranslate):
                    m00 = _m00;
                    m01 = _m01;
                    m02 = _m02;
                    m10 = _m10;
                    m11 = _m11;
                    m12 = _m12;
                    det = m00*m11 - m01*m10;
                    if (Math.Abs(det) <= double.MinValue)
                    {
                        throw new NoninvertibleTransformException("Determinant is " +
                                                                  det);
                    }
                    _m00 = m11/det;
                    _m10 = -m10/det;
                    _m01 = -m01/det;
                    _m11 = m00/det;
                    _m02 = (m01*m12 - m11*m02)/det;
                    _m12 = (m10*m02 - m00*m12)/det;
                    break;
                case (ApplyShear | ApplyScale):
                    m00 = _m00;
                    m01 = _m01;
                    m10 = _m10;
                    m11 = _m11;
                    det = m00*m11 - m01*m10;
                    if (Math.Abs(det) <= double.MinValue)
                    {
                        throw new NoninvertibleTransformException("Determinant is " +
                                                                  det);
                    }
                    _m00 = m11/det;
                    _m10 = -m10/det;
                    _m01 = -m01/det;
                    _m11 = m00/det;
                    // m02 = 0.0;
                    // m12 = 0.0;
                    break;
                case (ApplyShear | ApplyTranslate):
                    m01 = _m01;
                    m02 = _m02;
                    m10 = _m10;
                    m12 = _m12;
                    if (m01 == 0.0 || m10 == 0.0)
                    {
                        throw new NoninvertibleTransformException("Determinant is 0");
                    }
                    // m00 = 0.0;
                    _m10 = 1.0/m01;
                    _m01 = 1.0/m10;
                    // m11 = 0.0;
                    _m02 = -m12/m10;
                    _m12 = -m02/m01;
                    break;
                case (ApplyShear):
                    m01 = _m01;
                    m10 = _m10;
                    if (m01 == 0.0 || m10 == 0.0)
                    {
                        throw new NoninvertibleTransformException("Determinant is 0");
                    }
                    // m00 = 0.0;
                    _m10 = 1.0/m01;
                    _m01 = 1.0/m10;
                    // m11 = 0.0;
                    // m02 = 0.0;
                    // m12 = 0.0;
                    break;
                case (ApplyScale | ApplyTranslate):
                    m00 = _m00;
                    m02 = _m02;
                    m11 = _m11;
                    m12 = _m12;
                    if (m00 == 0.0 || m11 == 0.0)
                    {
                        throw new NoninvertibleTransformException("Determinant is 0");
                    }
                    _m00 = 1.0/m00;
                    // m10 = 0.0;
                    // m01 = 0.0;
                    _m11 = 1.0/m11;
                    _m02 = -m02/m00;
                    _m12 = -m12/m11;
                    break;
                case (ApplyScale):
                    m00 = _m00;
                    m11 = _m11;
                    if (m00 == 0.0 || m11 == 0.0)
                    {
                        throw new NoninvertibleTransformException("Determinant is 0");
                    }
                    _m00 = 1.0/m00;
                    // m10 = 0.0;
                    // m01 = 0.0;
                    _m11 = 1.0/m11;
                    // m02 = 0.0;
                    // m12 = 0.0;
                    break;
                case (ApplyTranslate):
                    // m00 = 1.0;
                    // m10 = 0.0;
                    // m01 = 0.0;
                    // m11 = 1.0;
                    _m02 = -_m02;
                    _m12 = -_m12;
                    break;
                case (ApplyIdentity):
                    // m00 = 1.0;
                    // m10 = 0.0;
                    // m01 = 0.0;
                    // m11 = 1.0;
                    // m02 = 0.0;
                    // m12 = 0.0;
                    break;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Transforms the specified ptSrc and stores the result in ptDst.
        /// </summary>
        /// <remarks>
        /// If ptDst is null, a new Point
        /// object is allocated and then the result of the transformation is
        /// stored in this object.
        /// In either case, ptDst, which contains the
        /// transformed point, is returned for convenience.
        /// If ptSrc and ptDst are the same
        /// object, the input point is correctly overwritten with
        /// the transformed point.
        /// </remarks>
        /// <param name="ptSrc">the specified Point to be transformed.</param>
        /// <param name="ptDst">the specified Point that stores the
        /// result of transforming ptSrc.</param>
        /// <returns>the ptDst after transforming</returns>
        public Point Transform(Point ptSrc, Point ptDst)
        {
            if (ptDst == null)
            {
                ptDst = new Point();
            }
            // Copy source coords into local variables in case src == dst
            double x = ptSrc.X;
            double y = ptSrc.Y;
            switch (_state)
            {
                default:
                    StateError();
                    return ptDst;
                case (ApplyShear | ApplyScale | ApplyTranslate):
                    ptDst.SetLocation(Round(x*_m00 + y*_m01 + _m02),
                                      Round(x*_m10 + y*_m11 + _m12));
                    return ptDst;
                case (ApplyShear | ApplyScale):
                    ptDst.SetLocation(Round(x*_m00 + y*_m01),
                                      Round(x*_m10 + y*_m11));
                    return ptDst;
                case (ApplyShear | ApplyTranslate):
                    ptDst.SetLocation(Round(y*_m01 + _m02),
                                      Round(x*_m10 + _m12));
                    return ptDst;
                case (ApplyShear):
                    ptDst.SetLocation(Round(y*_m01),
                                      Round(x*_m10));
                    return ptDst;
                case (ApplyScale | ApplyTranslate):
                    ptDst.SetLocation(Round(x*_m00 + _m02),
                                      Round(y*_m11 + _m12));
                    return ptDst;
                case (ApplyScale):
                    ptDst.SetLocation(Round(x*_m00),
                                      Round(y*_m11));
                    return ptDst;
                case (ApplyTranslate):
                    ptDst.SetLocation(Round(x + _m02),
                                      Round(y + _m12));
                    return ptDst;
                case (ApplyIdentity):
                    ptDst.SetLocation(Round(x), Round(y));
                    return ptDst;
            }

            /* NOTREACHED */
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Transforms an array of point objects by this transform.
        /// </summary>
        /// <remarks>
        /// If any element of the ptDst array is null, a new Point object is
        /// allocated and stored into that element before storing the results of the
        /// transformation.
        /// that this method does not take any precautions to
        /// avoid problems caused by storing results into Point
        /// objects that will be used as the source for calculations
        /// further down the source array.
        /// This method does guarantee that if a specified Point
        /// object is both the source and destination for the same single point
        /// transform operation then the results will not be stored until
        /// the calculations are complete to avoid storing the results on
        /// top of the operands.
        /// If, however, the destination Point object for one
        /// operation is the same object as the source Point
        /// object for another operation further down the source array then
        /// the original coordinates in that point are overwritten before
        /// they can be converted.
        /// </remarks>
        /// <param name="ptSrc">the array containing the source point objects.</param>
        /// <param name="srcOff">the offset to the first point object to be
        /// transformed in the source array.</param>
        /// <param name="ptDst">the array into which the transform point objects are
        /// returned.</param>
        /// <param name="dstOff">the offset to the location of the first
        /// transformed point object that is stored in the destination array</param>
        /// <param name="numPts">the number of point objects to be transformed.</param>
        public void Transform(Point[] ptSrc, int srcOff,
                              Point[] ptDst, int dstOff,
                              int numPts)
        {
            int currentState = _state;
            while (--numPts >= 0)
            {
                // Copy source coords into local variables in case src == dst
                Point src = ptSrc[srcOff++];
                double x = src.X;
                double y = src.Y;
                Point dst = ptDst[dstOff++];
                if (dst == null)
                {
                    dst = new Point();
                    ptDst[dstOff - 1] = dst;
                }
                switch (currentState)
                {
                    default:
                        StateError();
                        break;
                    case (ApplyShear | ApplyScale | ApplyTranslate):
                        dst.SetLocation(Round(x*_m00 + y*_m01 + _m02),
                                        Round(x*_m10 + y*_m11 + _m12));
                        break;
                    case (ApplyShear | ApplyScale):
                        dst.SetLocation(Round(x*_m00 + y*_m01),
                                        Round(x*_m10 + y*_m11));
                        break;
                    case (ApplyShear | ApplyTranslate):
                        dst.SetLocation(Round(y*_m01 + _m02),
                                        Round(x*_m10 + _m12));
                        break;
                    case (ApplyShear):
                        dst.SetLocation(Round(y*_m01), Round(x*_m10));
                        break;
                    case (ApplyScale | ApplyTranslate):
                        dst.SetLocation(Round(x*_m00 + _m02),
                                        Round(y*_m11 + _m12));
                        break;
                    case (ApplyScale):
                        dst.SetLocation(Round(x*_m00),
                                        Round(y*_m11));
                        break;
                    case (ApplyTranslate):
                        dst.SetLocation(Round(x + _m02),
                                        Round(y + _m12));
                        break;
                    case (ApplyIdentity):
                        dst.SetLocation(Round(x), Round(y));
                        break;
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Transforms an array of floating point coordinates by this transform.
        /// </summary>
        /// <remarks>
        /// The two coordinate array sections can be exactly the same or
        /// can be overlapping sections of the same array without affecting the
        /// validity of the results.
        /// This method ensures that no source coordinates are overwritten by a
        /// previous operation before they can be transformed.
        /// The coordinates are stored in the arrays starting at the specified
        /// offset in the order [x0, y0, x1, y1, ..., xn, yn].
        /// </remarks>
        /// <param name="srcPts">the array containing the source point coordinates.
        /// Each point is stored as a pair of x, y coordinates..</param>
        /// <param name="srcOff">the offset to the first point to be transformed
        /// in the source array.</param>
        /// <param name="dstPts">the array into which the transformed point coordinates
        /// are returned.  Each point is stored as a pair of x, y
        /// coordinates.</param>
        /// <param name="dstOff">the offset to the location of the first
        /// transformed point that is stored in the destination array.</param>
        /// <param name="numPts">the number of points to be transformed.</param>
        public void Transform(int[] srcPts, int srcOff,
                              int[] dstPts, int dstOff,
                              int numPts)
        {
            double m00, m01, m02, m10, m11, m12; // For caching
            if (dstPts == srcPts &&
                dstOff > srcOff && dstOff < srcOff + numPts*2)
            {
                // If the arrays overlap partially with the destination higher
                // than the source and we transform the coordinates normally
                // we would overwrite some of the later source coordinates
                // with results of previous transformations.
                // To get around this we use arraycopy to copy the points
                // to their final destination with correct overwrite
                // handling and then transform them in place in the new
                // safer location.
                Array.Copy(srcPts, srcOff, dstPts, dstOff, numPts*2);
                // srcPts = dstPts;		// They are known to be equal.
                srcOff = dstOff;
            }
            switch (_state)
            {
                default:
                    StateError();
                    break;
                case (ApplyShear | ApplyScale | ApplyTranslate):
                    m00 = _m00;
                    m01 = _m01;
                    m02 = _m02;
                    m10 = _m10;
                    m11 = _m11;
                    m12 = _m12;
                    while (--numPts >= 0)
                    {
                        double x = srcPts[srcOff++];
                        double y = srcPts[srcOff++];
                        dstPts[dstOff++] = Round((m00*x + m01*y + m02));
                        dstPts[dstOff++] = Round((m10*x + m11*y + m12));
                    }
                    return;
                case (ApplyShear | ApplyScale):
                    m00 = _m00;
                    m01 = _m01;
                    m10 = _m10;
                    m11 = _m11;
                    while (--numPts >= 0)
                    {
                        double x = srcPts[srcOff++];
                        double y = srcPts[srcOff++];
                        dstPts[dstOff++] = Round((m00*x + m01*y));
                        dstPts[dstOff++] = Round((m10*x + m11*y));
                    }
                    return;
                case (ApplyShear | ApplyTranslate):
                    m01 = _m01;
                    m02 = _m02;
                    m10 = _m10;
                    m12 = _m12;
                    while (--numPts >= 0)
                    {
                        double x = srcPts[srcOff++];
                        dstPts[dstOff++] = Round((m01*srcPts[srcOff++] + m02));
                        dstPts[dstOff++] = Round((m10*x + m12));
                    }
                    return;
                case (ApplyShear):
                    m01 = _m01;
                    m10 = _m10;
                    while (--numPts >= 0)
                    {
                        double x = srcPts[srcOff++];
                        dstPts[dstOff++] = Round((m01*srcPts[srcOff++]));
                        dstPts[dstOff++] = Round((m10*x));
                    }
                    return;
                case (ApplyScale | ApplyTranslate):
                    m00 = _m00;
                    m02 = _m02;
                    m11 = _m11;
                    m12 = _m12;
                    while (--numPts >= 0)
                    {
                        dstPts[dstOff++] = Round((m00*srcPts[srcOff++] + m02));
                        dstPts[dstOff++] = Round((m11*srcPts[srcOff++] + m12));
                    }
                    return;
                case (ApplyScale):
                    m00 = _m00;
                    m11 = _m11;
                    while (--numPts >= 0)
                    {
                        dstPts[dstOff++] = Round((m00*srcPts[srcOff++]));
                        dstPts[dstOff++] = Round((m11*srcPts[srcOff++]));
                    }
                    return;
                case (ApplyTranslate):
                    m02 = _m02;
                    m12 = _m12;
                    while (--numPts >= 0)
                    {
                        dstPts[dstOff++] = Round((srcPts[srcOff++] + m02));
                        dstPts[dstOff++] = Round((srcPts[srcOff++] + m12));
                    }
                    return;
                case (ApplyIdentity):
                    if (srcPts != dstPts || srcOff != dstOff)
                    {
                        Array.Copy(srcPts, srcOff, dstPts, dstOff,
                                   numPts*2);
                    }
                    return;
            }
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Inverse transforms the specified ptSrc and stores the
        /// result in ptDst.
        /// </summary>
        /// <remarks>
        /// If ptDst is null, a new Point object is allocated and then the result of the
        /// transform is stored in this object.
        /// In either case, ptDst, which contains the transformed point, is returned 
        /// for convenience.
        /// If ptSrc and ptDst are the same object, the input point is correctly 
        /// overwritten with the transformed point.
        /// </remarks>
        /// <param name="ptSrc">the point to be inverse transformed</param>
        /// <param name="ptDst">the resulting transformed point.</param>
        /// <returns>which contains the result of the
        /// inverse transform.</returns>
        public Point InverseTransform(Point ptSrc, Point ptDst)
        {
            if (ptDst == null)
            {
                ptDst = new Point();
            }
            // Copy source coords into local variables in case src == dst
            double x = ptSrc.X;
            double y = ptSrc.Y;
            switch (_state)
            {
                default:
                    StateError();
                    return ptDst;
                case (ApplyShear | ApplyScale | ApplyTranslate):
                    {
                        x -= _m02;
                        y -= _m12;
                        double det = _m00*_m11 - _m01*_m10;
                        if (Math.Abs(det) <= double.MinValue)
                        {
                            throw new NoninvertibleTransformException("Determinant is " +
                                                                      det);
                        }
                        ptDst.SetLocation(Round((x*_m11 - y*_m01)/det),
                                          Round((y*_m00 - x*_m10)/det));
                        return ptDst;
                    }
                case (ApplyShear | ApplyScale):
                    {
                        double det = _m00*_m11 - _m01*_m10;
                        if (Math.Abs(det) <= double.MinValue)
                        {
                            throw new NoninvertibleTransformException("Determinant is " +
                                                                      det);
                        }
                        ptDst.SetLocation(Round((x*_m11 - y*_m01)/det),
                                          Round((y*_m00 - x*_m10)/det));
                        return ptDst;
                    }
                case (ApplyShear | ApplyTranslate):
                    x -= _m02;
                    y -= _m12;
                    if (_m01 == 0.0 || _m10 == 0.0)
                    {
                        throw new NoninvertibleTransformException("Determinant is 0");
                    }
                    ptDst.SetLocation(Round(y/_m10),
                                      Round(x/_m01));
                    return ptDst;
                case (ApplyShear):
                    if (_m01 == 0.0 || _m10 == 0.0)
                    {
                        throw new NoninvertibleTransformException("Determinant is 0");
                    }
                    ptDst.SetLocation(Round(y/_m10),
                                      Round(x/_m01));
                    return ptDst;
                case (ApplyScale | ApplyTranslate):
                    x -= _m02;
                    y -= _m12;
                    if (_m00 == 0.0 || _m11 == 0.0)
                    {
                        throw new NoninvertibleTransformException("Determinant is 0");
                    }
                    ptDst.SetLocation(Round(x/_m00),
                                      Round(y/_m11));
                    return ptDst;
                case (ApplyScale):
                    if (_m00 == 0.0 || _m11 == 0.0)
                    {
                        throw new NoninvertibleTransformException("Determinant is 0");
                    }
                    ptDst.SetLocation(Round(x/_m00),
                                      Round(y/_m11));
                    return ptDst;
                case (ApplyTranslate):
                    ptDst.SetLocation(Round(x - _m02), Round(y - _m12));
                    return ptDst;
                case (ApplyIdentity):
                    ptDst.SetLocation(Round(x), Round(y));
                    return ptDst;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Inverse transforms an array of double precision coordinates by
        /// this transform.
        /// </summary>
        /// <remarks>
        /// The two coordinate array sections can be exactly the same or
        /// can be overlapping sections of the same array without affecting the
        /// validity of the results.
        /// This method ensures that no source coordinates are
        /// overwritten by a previous operation before they can be transformed.
        /// The coordinates are stored in the arrays starting at the specified
        /// offset in the order [x0, y0, x1, y1, ..., xn, yn].
        /// </remarks>
        /// <param name="srcPts">the array containing the source point coordinates.
        /// Each point is stored as a pair of x, y coordinates.</param>
        /// <param name="srcOff">the offset to the first point to be transformed
        /// in the source array.</param>
        /// <param name="dstPts">the array into which the transformed point
        /// coordinates are returned.  Each point is stored as a pair of
        /// x, y coordinates..</param>
        /// <param name="dstOff">the offset to the location of the first
        /// transformed point that is stored in the destination array.</param>
        /// <param name="numPts">the number of point objects to be transformed.</param>
        public void InverseTransform(int[] srcPts, int srcOff,
                                     int[] dstPts, int dstOff,
                                     int numPts)
        {
            double m00, m01, m02, m10, m11, m12; // For caching
            double det;
            if (dstPts == srcPts &&
                dstOff > srcOff && dstOff < srcOff + numPts*2)
            {
                // If the arrays overlap partially with the destination higher
                // than the source and we transform the coordinates normally
                // we would overwrite some of the later source coordinates
                // with results of previous transformations.
                // To get around this we use arraycopy to copy the points
                // to their final destination with correct overwrite
                // handling and then transform them in place in the new
                // safer location.
                Array.Copy(srcPts, srcOff, dstPts, dstOff, numPts*2);
                // srcPts = dstPts;		// They are known to be equal.
                srcOff = dstOff;
            }
            switch (_state)
            {
                default:
                    StateError();
                    break;
                case (ApplyShear | ApplyScale | ApplyTranslate):
                    m00 = _m00;
                    m01 = _m01;
                    m02 = _m02;
                    m10 = _m10;
                    m11 = _m11;
                    m12 = _m12;
                    det = m00*m11 - m01*m10;
                    if (Math.Abs(det) <= double.MinValue)
                    {
                        throw new NoninvertibleTransformException("Determinant is " +
                                                                  det);
                    }
                    while (--numPts >= 0)
                    {
                        double x = srcPts[srcOff++] - m02;
                        double y = srcPts[srcOff++] - m12;
                        dstPts[dstOff++] = Round((x*m11 - y*m01)/det);
                        dstPts[dstOff++] = Round((y*m00 - x*m10)/det);
                    }
                    return;
                case (ApplyShear | ApplyScale):
                    m00 = _m00;
                    m01 = _m01;
                    m10 = _m10;
                    m11 = _m11;
                    det = m00*m11 - m01*m10;
                    if (Math.Abs(det) <= double.MinValue)
                    {
                        throw new NoninvertibleTransformException("Determinant is " +
                                                                  det);
                    }
                    while (--numPts >= 0)
                    {
                        double x = srcPts[srcOff++];
                        double y = srcPts[srcOff++];
                        dstPts[dstOff++] = Round((x*m11 - y*m01)/det);
                        dstPts[dstOff++] = Round((y*m00 - x*m10)/det);
                    }
                    return;
                case (ApplyShear | ApplyTranslate):
                    m01 = _m01;
                    m02 = _m02;
                    m10 = _m10;
                    m12 = _m12;
                    if (m01 == 0.0 || m10 == 0.0)
                    {
                        throw new NoninvertibleTransformException("Determinant is 0");
                    }
                    while (--numPts >= 0)
                    {
                        double x = srcPts[srcOff++] - m02;
                        dstPts[dstOff++] = Round((srcPts[srcOff++] - m12)/m10);
                        dstPts[dstOff++] = Round(x/m01);
                    }
                    return;
                case (ApplyShear):
                    m01 = _m01;
                    m10 = _m10;
                    if (m01 == 0.0 || m10 == 0.0)
                    {
                        throw new NoninvertibleTransformException("Determinant is 0");
                    }
                    while (--numPts >= 0)
                    {
                        double x = srcPts[srcOff++];
                        dstPts[dstOff++] = Round(srcPts[srcOff++]/m10);
                        dstPts[dstOff++] = Round(x/m01);
                    }
                    return;
                case (ApplyScale | ApplyTranslate):
                    m00 = _m00;
                    m02 = _m02;
                    m11 = _m11;
                    m12 = _m12;
                    if (m00 == 0.0 || m11 == 0.0)
                    {
                        throw new NoninvertibleTransformException("Determinant is 0");
                    }
                    while (--numPts >= 0)
                    {
                        dstPts[dstOff++] = Round((srcPts[srcOff++] - m02)/m00);
                        dstPts[dstOff++] = Round((srcPts[srcOff++] - m12)/m11);
                    }
                    return;
                case (ApplyScale):
                    m00 = _m00;
                    m11 = _m11;
                    if (m00 == 0.0 || m11 == 0.0)
                    {
                        throw new NoninvertibleTransformException("Determinant is 0");
                    }
                    while (--numPts >= 0)
                    {
                        dstPts[dstOff++] = Round(srcPts[srcOff++]/m00);
                        dstPts[dstOff++] = Round(srcPts[srcOff++]/m11);
                    }
                    return;
                case (ApplyTranslate):
                    m02 = _m02;
                    m12 = _m12;
                    while (--numPts >= 0)
                    {
                        dstPts[dstOff++] = Round(srcPts[srcOff++] - m02);
                        dstPts[dstOff++] = Round(srcPts[srcOff++] - m12);
                    }
                    return;
                case (ApplyIdentity):
                    if (srcPts != dstPts || srcOff != dstOff)
                    {
                        Array.Copy(srcPts, srcOff, dstPts, dstOff,
                                   numPts*2);
                    }
                    return;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Transforms the relative distance vector specified by
        /// ptSrc and stores the result in ptDst.
        /// </summary>
        /// <remarks>
        /// A relative distance vector is transformed without applying the
        /// translation components of the affine transformation matrix
        /// using the following equations:
        /// <pre>
        ///	[  x' ]   [  m00  m01 (m02) ] [  x  ]   [ m00x + m01y ]
        ///	[  y' ] = [  m10  m11 (m12) ] [  y  ] = [ m10x + m11y ]
        ///	[ (1) ]   [  (0)  (0) ( 1 ) ] [ (1) ]   [     (1)     ]
        /// </pre>
        /// If ptDst is null, a new
        /// Point object is allocated and then the result of the
        /// transform is stored in this object.
        /// In either case, ptDst, which contains the
        /// transformed point, is returned for convenience.
        /// If ptSrc and ptDst are the same object,
        /// the input point is correctly overwritten with the transformed
        /// point.
        /// </remarks>
        /// <param name="ptSrc">the distance vector to be delta transformed.</param>
        /// <param name="ptDst">the resulting transformed distance vector.</param>
        /// <returns> ptDst, which contains the result of the
        /// transformation</returns>
        public Point DeltaTransform(Point ptSrc, Point ptDst)
        {
            if (ptDst == null)
            {
                ptDst = new Point();
            }
            // Copy source coords into local variables in case src == dst
            double x = ptSrc.X;
            double y = ptSrc.Y;
            switch (_state)
            {
                default:
                    StateError();
                    return ptDst;
                case (ApplyShear | ApplyScale | ApplyTranslate):
                case (ApplyShear | ApplyScale):
                    ptDst.SetLocation(Round(x*_m00 + y*_m01),
                                      Round(x*_m10 + y*_m11));
                    return ptDst;
                case (ApplyShear | ApplyTranslate):
                case (ApplyShear):
                    ptDst.SetLocation(Round(y*_m01),
                                      Round(x*_m10));
                    return ptDst;
                case (ApplyScale | ApplyTranslate):
                case (ApplyScale):
                    ptDst.SetLocation(Round(x*_m00), Round(y*_m11));
                    return ptDst;
                case (ApplyTranslate):
                case (ApplyIdentity):
                    ptDst.SetLocation(Round(x), Round(y));
                    return ptDst;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Transforms an array of relative distance vectors by this transform.
        /// </summary>
        /// <remarks>
        /// A relative distance vector is transformed without applying the
        /// translation components of the affine transformation matrix
        /// using the following equations:
        /// <pre>
        ///	[  x' ]   [  m00  m01 (m02) ] [  x  ]   [ m00x + m01y ]
        ///	[  y' ] = [  m10  m11 (m12) ] [  y  ] = [ m10x + m11y ]
        ///	[ (1) ]   [  (0)  (0) ( 1 ) ] [ (1) ]   [     (1)     ]
        /// </pre>
        /// The two coordinate array sections can be exactly the same or
        /// can be overlapping sections of the same array without affecting the
        /// validity of the results.
        /// This method ensures that no source coordinates are
        /// overwritten by a previous operation before they can be transformed.
        /// The coordinates are stored in the arrays starting at the indicated
        /// offset in the order [x0, y0, x1, y1, ..., xn, yn].
        /// </remarks>
        /// <param name="srcPts">the array containing the source distance vectors.
        /// Each vector is stored as a pair of relative x,y coordinates.</param>
        /// <param name="srcOff">the offset to the first vector to be transformed
        /// in the source array.</param>
        /// <param name="dstPts">the array into which the transformed distance vectors
        /// are returned.  Each vector is stored as a pair of relative.</param>
        /// <param name="dstOff">the offset to the location of the first
        /// transformed vector that is stored in the destination array</param>
        /// <param name="numPts">the number of vector coordinate pairs to be
        /// transformed.</param>
        public void DeltaTransform(int[] srcPts, int srcOff,
                                   int[] dstPts, int dstOff,
                                   int numPts)
        {
            double m00, m01, m10, m11; // For caching
            if (dstPts == srcPts &&
                dstOff > srcOff && dstOff < srcOff + numPts*2)
            {
                // If the arrays overlap partially with the destination higher
                // than the source and we transform the coordinates normally
                // we would overwrite some of the later source coordinates
                // with results of previous transformations.
                // To get around this we use arraycopy to copy the points
                // to their final destination with correct overwrite
                // handling and then transform them in place in the new
                // safer location.
                Array.Copy(srcPts, srcOff, dstPts, dstOff, numPts*2);
                // srcPts = dstPts;		// They are known to be equal.
                srcOff = dstOff;
            }
            switch (_state)
            {
                default:
                    StateError();
                    break;
                case (ApplyShear | ApplyScale | ApplyTranslate):
                case (ApplyShear | ApplyScale):
                    m00 = _m00;
                    m01 = _m01;
                    m10 = _m10;
                    m11 = _m11;
                    while (--numPts >= 0)
                    {
                        double x = srcPts[srcOff++];
                        double y = srcPts[srcOff++];
                        dstPts[dstOff++] = Round(x*m00 + y*m01);
                        dstPts[dstOff++] = Round(x*m10 + y*m11);
                    }
                    return;
                case (ApplyShear | ApplyTranslate):
                case (ApplyShear):
                    m01 = _m01;
                    m10 = _m10;
                    while (--numPts >= 0)
                    {
                        double x = srcPts[srcOff++];
                        dstPts[dstOff++] = Round(srcPts[srcOff++]*m01);
                        dstPts[dstOff++] = Round(x*m10);
                    }
                    return;
                case (ApplyScale | ApplyTranslate):
                case (ApplyScale):
                    m00 = _m00;
                    m11 = _m11;
                    while (--numPts >= 0)
                    {
                        dstPts[dstOff++] = Round(srcPts[srcOff++]*m00);
                        dstPts[dstOff++] = Round(srcPts[srcOff++]*m11);
                    }
                    return;
                case (ApplyTranslate):
                case (ApplyIdentity):
                    if (srcPts != dstPts || srcOff != dstOff)
                    {
                        Array.Copy(srcPts, srcOff, dstPts, dstOff,
                                   numPts*2);
                    }
                    return;
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a new  IShape object defined by the geometry of the
        /// specified IShape after it has been transformed by
        /// this transform.
        /// </summary>
        /// <param name="pSrc">the specified IShape object to be
        /// transformed by this transform..</param>
        /// <returns>a new IShape object that defines the geometry
        /// of the transformed IShape, or null if pSrc is null.</returns>
        public IShape CreateTransformedShape(IShape pSrc)
        {
            if (pSrc == null)
            {
                return null;
            }
            return new Path(pSrc, this);
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
            return ("AffineTransform[["
                    + Matround(_m00) + ", "
                    + Matround(_m01) + ", "
                    + Matround(_m02) + "], ["
                    + Matround(_m10) + ", "
                    + Matround(_m11) + ", "
                    + Matround(_m12) + "]]");
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns true if this AffineTransform is an identity transform.
        /// </summary>
        /// <returns>
        /// 	true if this instance is identity; otherwise, false.
        /// </returns>
        public bool IsIdentity()
        {
            return (_state == ApplyIdentity || (TransformType == TypeIdentity));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a hash c for this instance.
        /// </summary>
        /// <returns>
        /// A hash c for this instance, suitable for use in hashing algorithms 
        /// and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            long bits = Util.Utils.DoubleToInt64Bits(_m00);
            bits = bits*31 + Util.Utils.DoubleToInt64Bits(_m01);
            bits = bits*31 + Util.Utils.DoubleToInt64Bits(_m02);
            bits = bits*31 + Util.Utils.DoubleToInt64Bits(_m10);
            bits = bits*31 + Util.Utils.DoubleToInt64Bits(_m11);
            bits = bits*31 + Util.Utils.DoubleToInt64Bits(_m12);
            return (((int) bits) ^ ((int) (bits >> 32)));
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
        /// 	true if the specified <see cref="System.Object"/> is equal 
        /// to this instance; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            if (!(obj is AffineTransform))
            {
                return false;
            }

            var a = (AffineTransform) obj;

            return ((_m00 == a._m00) && (_m01 == a._m01) && (_m02 == a._m02) &&
                    (_m10 == a._m10) && (_m11 == a._m11) && (_m12 == a._m12));
        }

        /*
         * This constant is only useful for the cached type field.
         * It indicates that the type has been decached and must be recalculated.
         */
        private const int TypeUnknown = -1;

        /**
         * This constant is used for the internal state variable to indicate
         * that the translation components of the matrix (m02 and m12) need
         * to be added to complete the transformation equation of this transform.
         */
        private const int ApplyTranslate = 1;

        /**
         * This constant is used for the internal state variable to indicate
         * that the scaling components of the matrix (m00 and m11) need
         * to be factored in to complete the transformation equation of
         * this transform.  If the ApplyShear bit is also set then it
         * indicates that the scaling components are not both 0.0.  If the
         * ApplyShear bit is not also set then it indicates that the
         * scaling components are not both 1.0.  If neither the ApplyShear
         * nor the ApplyScale bits are set then the scaling components
         * are both 1.0, which means that the x and y components contribute
         * to the transformed coordinate, but they are not multiplied by
         * any scaling factor.
         */
        private const int ApplyScale = 2;

        /**
         * This constant is used for the internal state variable to indicate
         * that the shearing components of the matrix (m01 and m10) need
         * to be factored in to complete the transformation equation of this
         * transform.  The presence of this bit in the state variable changes
         * the interpretation of the ApplyScale bit as indicated in its
         * documentation.
         */
        private const int ApplyShear = 4;

        /**
         * This constant is used for the internal state variable to indicate
         * that no calculations need to be performed and that the source
         * coordinates only need to be copied to their destinations to
         * complete the transformation equation of this transform.
         */
        private const int ApplyIdentity = 0;

        /*
         * For methods which combine together the state of two separate
         * transforms and dispatch based upon the combination, these constants
         * specify how far to shift one of the states so that the two states
         * are mutually non-interfering and provide constants for testing the
         * bits of the shifted (HI) state.  The methods in this class use
         * the convention that the state of "this" transform is unshifted and
         * the state of the "other" or "argument" transform is shifted (HI).
         */
        private const int HiShift = 3;
        private const int HiIdentity = ApplyIdentity << HiShift;
        private const int HiTranslate = ApplyTranslate << HiShift;
        private const int HiScale = ApplyScale << HiShift;
        private const int HiShear = ApplyShear << HiShift;

        /**
         * The X coordinate scaling element of the 3x3
         * affine transformation matrix.
         *
         * @serial
         */
        private double _m00;

        /**
         * The Y coordinate shearing element of the 3x3
         * affine transformation matrix.
         */
        private double _m10;

        /**
         * The X coordinate shearing element of the 3x3
         * affine transformation matrix.
         */
        private double _m01;

        /**
         * The Y coordinate scaling element of the 3x3
         * affine transformation matrix.
         */
        private double _m11;

        /**
         * The X coordinate of the translation element of the
         * 3x3 affine transformation matrix.
         */
        private double _m02;

        /**
         * The Y coordinate of the translation element of the
         * 3x3 affine transformation matrix.
         */
        private double _m12;

        /**
         * This field keeps track of which components of the matrix need to
         * be applied when performing a transformation.
         */
        private int _state;

        /**
         * This field caches the current transformation type of the matrix.
         */
        private int _type;

        /**
         * tranform parser
         */
        private static readonly TransformListParser TransformListParser = new TransformListParser();

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Round values to sane precision for printing
        /// remember that Math.sin(Math.Pi) has an error of about 10^-16
        /// </summary>
        /// <param name="matval">The matval.</param>
        /// <returns></returns>
        private static double Matround(double matval)
        {
            return Rint(matval*1E15)/1E15;
        }


        ///////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Rounds the specified a.
        /// </summary>
        /// <param name="a">A.</param>
        /// <returns></returns>
        private static int Round(double a)
        {
            return (int) Math.Floor(a + 0.5);
        }

        ///////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Rints the specified a.
        /// </summary>
        /// <param name="a">A.</param>
        /// <returns></returns>
        private static double Rint(double a)
        {
            return a;
        }

        ///////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="AffineTransform"/> class.
        /// </summary>
        /// <param name="m00">The M00.</param>
        /// <param name="m10">The M10.</param>
        /// <param name="m01">The M01.</param>
        /// <param name="m11">The M11.</param>
        /// <param name="m02">The M02.</param>
        /// <param name="m12">The M12.</param>
        /// <param name="state">The state.</param>
        private AffineTransform(double m00, double m10,
                                double m01, double m11,
                                double m02, double m12,
                                int state)
        {
            _m00 = m00;
            _m10 = m10;
            _m01 = m01;
            _m11 = m11;
            _m02 = m02;
            _m12 = m12;
            _state = state;
            _type = TypeUnknown;
        }

        ///////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        // Utility methods to optimize rotate methods.
        // These tables translate the flags during predictable quadrant
        // rotations where the shear and scale values are swapped and negated.
        private static readonly int[] Rot90Conversion = new[]
                                                        {
                                                            /* IDENTITY => */       
                                                            ApplyShear,
                                                             /* TRANSLATE (TR) => */
                                                             ApplyShear | ApplyTranslate,
                                                             /* SCALE (SC) => */
                                                             ApplyShear,
                                                             /* SC | TR => */
                                                             ApplyShear | ApplyTranslate,
                                                             /* SHEAR (SH) => */
                                                             ApplyScale,
                                                             /* SH | TR => */
                                                             ApplyScale | ApplyTranslate,
                                                             /* SH | SC => */
                                                             ApplyShear | ApplyScale,
                                                             /* SH | SC | TR => */
                                                             ApplyShear | ApplyScale |
                                                             ApplyTranslate,
                                                            };

        ///////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This is the utility function to calculate the flag bits when
        /// they have not been cached.
        /// </summary>
        private void CalculateType()
        {
            int ret = TypeIdentity;
            bool sgn0, sgn1;
            double m0, m1, m2, m3;
            UpdateState();
            switch (_state)
            {
                default:
                    StateError();
                    break;
                case (ApplyShear | ApplyScale | ApplyTranslate):
                    ret = TypeTranslation;
                    if ((m0 = _m00)*(m2 = _m01) + (m3 = _m10)*(m1 = _m11) != 0)
                    {
                        // Transformed unit vectors are not perpendicular...
                        _type = TypeGeneralTransform;
                        return;
                    }
                    sgn0 = (m0 >= 0.0);
                    sgn1 = (m1 >= 0.0);
                    if (sgn0 == sgn1)
                    {
                        // sgn(M0) == sgn(M1) therefore sgn(M2) == -sgn(M3)
                        // This is the "unflipped" (right-handed) state
                        if (m0 != m1 || m2 != -m3)
                        {
                            ret |= (TypeGeneralRotation | TypeGeneralScale);
                        }
                        else if (m0*m1 - m2*m3 != 1.0)
                        {
                            ret |= (TypeGeneralRotation | TypeUniformScale);
                        }
                        else
                        {
                            ret |= TypeGeneralRotation;
                        }
                    }
                    else
                    {
                        // sgn(M0) == -sgn(M1) therefore sgn(M2) == sgn(M3)
                        // This is the "flipped" (left-handed) state
                        if (m0 != -m1 || m2 != m3)
                        {
                            ret |= (TypeGeneralRotation |
                                    TypeFlip |
                                    TypeGeneralScale);
                        }
                        else if (m0*m1 - m2*m3 != 1.0)
                        {
                            ret |= (TypeGeneralRotation |
                                    TypeFlip |
                                    TypeUniformScale);
                        }
                        else
                        {
                            ret |= (TypeGeneralRotation | TypeFlip);
                        }
                    }
                    break;
                case (ApplyShear | ApplyScale):
                    if ((m0 = _m00)*(m2 = _m01) + (m3 = _m10)*(m1 = _m11) != 0)
                    {
                        // Transformed unit vectors are not perpendicular...
                        _type = TypeGeneralTransform;
                        return;
                    }
                    sgn0 = (m0 >= 0.0);
                    sgn1 = (m1 >= 0.0);
                    if (sgn0 == sgn1)
                    {
                        // sgn(M0) == sgn(M1) therefore sgn(M2) == -sgn(M3)
                        // This is the "unflipped" (right-handed) state
                        if (m0 != m1 || m2 != -m3)
                        {
                            ret |= (TypeGeneralRotation | TypeGeneralScale);
                        }
                        else if (m0*m1 - m2*m3 != 1.0)
                        {
                            ret |= (TypeGeneralRotation | TypeUniformScale);
                        }
                        else
                        {
                            ret |= TypeGeneralRotation;
                        }
                    }
                    else
                    {
                        // sgn(M0) == -sgn(M1) therefore sgn(M2) == sgn(M3)
                        // This is the "flipped" (left-handed) state
                        if (m0 != -m1 || m2 != m3)
                        {
                            ret |= (TypeGeneralRotation |
                                    TypeFlip |
                                    TypeGeneralScale);
                        }
                        else if (m0*m1 - m2*m3 != 1.0)
                        {
                            ret |= (TypeGeneralRotation |
                                    TypeFlip |
                                    TypeUniformScale);
                        }
                        else
                        {
                            ret |= (TypeGeneralRotation | TypeFlip);
                        }
                    }
                    break;
                case (ApplyShear | ApplyTranslate):
                    ret = TypeTranslation;
                    sgn0 = ((m0 = _m01) >= 0.0);
                    sgn1 = ((m1 = _m10) >= 0.0);
                    if (sgn0 != sgn1)
                    {
                        // Different signs - simple 90 degree rotation
                        if (m0 != -m1)
                        {
                            ret |= (TypeQuadrantRotation | TypeGeneralScale);
                        }
                        else if (m0 != 1.0 && m0 != -1.0)
                        {
                            ret |= (TypeQuadrantRotation | TypeUniformScale);
                        }
                        else
                        {
                            ret |= TypeQuadrantRotation;
                        }
                    }
                    else
                    {
                        // Same signs - 90 degree rotation plus an axis flip too
                        if (m0 == m1)
                        {
                            ret |= (TypeQuadrantRotation |
                                    TypeFlip |
                                    TypeUniformScale);
                        }
                        else
                        {
                            ret |= (TypeQuadrantRotation |
                                    TypeFlip |
                                    TypeGeneralScale);
                        }
                    }
                    break;
                case (ApplyShear):
                    sgn0 = ((m0 = _m01) >= 0.0);
                    sgn1 = ((m1 = _m10) >= 0.0);
                    if (sgn0 != sgn1)
                    {
                        // Different signs - simple 90 degree rotation
                        if (m0 != -m1)
                        {
                            ret |= (TypeQuadrantRotation | TypeGeneralScale);
                        }
                        else if (m0 != 1.0 && m0 != -1.0)
                        {
                            ret |= (TypeQuadrantRotation | TypeUniformScale);
                        }
                        else
                        {
                            ret |= TypeQuadrantRotation;
                        }
                    }
                    else
                    {
                        // Same signs - 90 degree rotation plus an axis flip too
                        if (m0 == m1)
                        {
                            ret |= (TypeQuadrantRotation |
                                    TypeFlip |
                                    TypeUniformScale);
                        }
                        else
                        {
                            ret |= (TypeQuadrantRotation |
                                    TypeFlip |
                                    TypeGeneralScale);
                        }
                    }
                    break;
                case (ApplyScale | ApplyTranslate):
                    ret = TypeTranslation;
                    sgn0 = ((m0 = _m00) >= 0.0);
                    sgn1 = ((m1 = _m11) >= 0.0);
                    if (sgn0 == sgn1)
                    {
                        if (sgn0)
                        {
                            // Both scaling factors non-negative - simple scale
                            // Remember: ApplyScale implies M0, M1 are not both 1
                            if (m0 == m1)
                            {
                                ret |= TypeUniformScale;
                            }
                            else
                            {
                                ret |= TypeGeneralScale;
                            }
                        }
                        else
                        {
                            // Both scaling factors negative - 180 degree rotation
                            if (m0 != m1)
                            {
                                ret |= (TypeQuadrantRotation | TypeGeneralScale);
                            }
                            else if (m0 != -1.0)
                            {
                                ret |= (TypeQuadrantRotation | TypeUniformScale);
                            }
                            else
                            {
                                ret |= TypeQuadrantRotation;
                            }
                        }
                    }
                    else
                    {
                        // Scaling factor signs different - flip about some axis
                        if (m0 == -m1)
                        {
                            if (m0 == 1.0 || m0 == -1.0)
                            {
                                ret |= TypeFlip;
                            }
                            else
                            {
                                ret |= (TypeFlip | TypeUniformScale);
                            }
                        }
                        else
                        {
                            ret |= (TypeFlip | TypeGeneralScale);
                        }
                    }
                    break;
                case (ApplyScale):
                    sgn0 = ((m0 = _m00) >= 0.0);
                    sgn1 = ((m1 = _m11) >= 0.0);
                    if (sgn0 == sgn1)
                    {
                        if (sgn0)
                        {
                            // Both scaling factors non-negative - simple scale
                            // Remember: ApplyScale implies M0, M1 are not both 1
                            if (m0 == m1)
                            {
                                ret |= TypeUniformScale;
                            }
                            else
                            {
                                ret |= TypeGeneralScale;
                            }
                        }
                        else
                        {
                            // Both scaling factors negative - 180 degree rotation
                            if (m0 != m1)
                            {
                                ret |= (TypeQuadrantRotation | TypeGeneralScale);
                            }
                            else if (m0 != -1.0)
                            {
                                ret |= (TypeQuadrantRotation | TypeUniformScale);
                            }
                            else
                            {
                                ret |= TypeQuadrantRotation;
                            }
                        }
                    }
                    else
                    {
                        // Scaling factor signs different - flip about some axis
                        if (m0 == -m1)
                        {
                            if (m0 == 1.0 || m0 == -1.0)
                            {
                                ret |= TypeFlip;
                            }
                            else
                            {
                                ret |= (TypeFlip | TypeUniformScale);
                            }
                        }
                        else
                        {
                            ret |= (TypeFlip | TypeGeneralScale);
                        }
                    }
                    break;
                case (ApplyTranslate):
                    ret = TypeTranslation;
                    break;
                case (ApplyIdentity):
                    break;
            }
            _type = ret;
        }

        ///////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ///////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Manually recalculates the state of the transform when the matrix
        /// changes too much to predict the effects on the state.
        /// The following table specifies what the various settings of the
        /// state field say about the values of the corresponding matrix
        /// element fields.
        /// Remember that the rules governing the SCALE fields are slightly
        /// different depending on whether the SHEAR flag is also set.
        /// <pre>
        ///                     SCALE            SHEAR          TRANSLATE
        ///                    m00/m11          m01/m10          m02/m12
        ///
        /// IDENTITY             1.0              0.0              0.0
        /// TRANSLATE (TR)       1.0              0.0          not both 0.0
        /// SCALE (SC)       not both 1.0         0.0              0.0
        /// TR | SC          not both 1.0         0.0          not both 0.0
        /// SHEAR (SH)           0.0          not both 0.0         0.0
        /// TR | SH              0.0          not both 0.0     not both 0.0
        /// SC | SH          not both 0.0     not both 0.0         0.0
        /// TR | SC | SH     not both 0.0     not both 0.0     not both 0.0
        /// </pre>
        /// </summary>
        private void UpdateState()
        {
            if (_m01 == 0.0 && _m10 == 0.0)
            {
                if (_m00 == 1.0 && _m11 == 1.0)
                {
                    if (_m02 == 0.0 && _m12 == 0.0)
                    {
                        _state = ApplyIdentity;
                        _type = TypeIdentity;
                    }
                    else
                    {
                        _state = ApplyTranslate;
                        _type = TypeTranslation;
                    }
                }
                else
                {
                    if (_m02 == 0.0 && _m12 == 0.0)
                    {
                        _state = ApplyScale;
                        _type = TypeUnknown;
                    }
                    else
                    {
                        _state = (ApplyScale | ApplyTranslate);
                        _type = TypeUnknown;
                    }
                }
            }
            else
            {
                if (_m00 == 0.0 && _m11 == 0.0)
                {
                    if (_m02 == 0.0 && _m12 == 0.0)
                    {
                        _state = ApplyShear;
                        _type = TypeUnknown;
                    }
                    else
                    {
                        _state = (ApplyShear | ApplyTranslate);
                        _type = TypeUnknown;
                    }
                }
                else
                {
                    if (_m02 == 0.0 && _m12 == 0.0)
                    {
                        _state = (ApplyShear | ApplyScale);
                        _type = TypeUnknown;
                    }
                    else
                    {
                        _state = (ApplyShear | ApplyScale | ApplyTranslate);
                        _type = TypeUnknown;
                    }
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ///////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Convenience method used internally to throw exceptions when
        /// a case was forgotten in a switch statement.
        /// </summary>
        private static void StateError()
        {
            throw new SystemException("missing case in transform state switch");
        }

        ///////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ///////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Rotate 90s this instance.
        /// </summary>
        private void Rotate90()
        {
            double m0 = _m00;
            _m00 = _m01;
            _m01 = -m0;
            m0 = _m10;
            _m10 = _m11;
            _m11 = -m0;
            int currentState = Rot90Conversion[_state];
            if ((currentState & (ApplyShear | ApplyScale)) == ApplyScale &&
                _m00 == 1.0 && _m11 == 1.0)
            {
                currentState -= ApplyScale;
            }
            _state = currentState;
            _type = TypeUnknown;
        }

        ///////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ///////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Rotate 180s this instance.
        /// </summary>
        private void Rotate180()
        {
            _m00 = -_m00;
            _m11 = -_m11;
            int currentState = _state;
            if ((currentState & (ApplyShear)) != 0)
            {
                // If there was a shear, then this rotation has no
                // effect on the state.
                _m01 = -_m01;
                _m10 = -_m10;
            }
            else
            {
                // No shear means the SCALE state may toggle when
                // m00 and m11 are negated.
                if (_m00 == 1.0 && _m11 == 1.0)
                {
                    _state = currentState & ~ApplyScale;
                }
                else
                {
                    _state = currentState | ApplyScale;
                }
            }
            _type = TypeUnknown;
        }

        ///////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 14OCT2010  James Shen                 	          Code review
        ///////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Rotate 270s this instance.
        /// </summary>
        private void Rotate270()
        {
            double m0 = _m00;
            _m00 = -_m01;
            _m01 = m0;
            m0 = _m10;
            _m10 = -_m11;
            _m11 = m0;
            int currentState = Rot90Conversion[_state];
            if ((currentState & (ApplyShear | ApplyScale)) == ApplyScale &&
                _m00 == 1.0 && _m11 == 1.0)
            {
                currentState -= ApplyScale;
            }
            _state = currentState;
            _type = TypeUnknown;
        }
    }
}