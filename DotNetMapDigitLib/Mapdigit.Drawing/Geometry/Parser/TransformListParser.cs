//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 25SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System;
using Mapdigit.Util;

//--------------------------------- PACKAGE ------------------------------------

namespace Mapdigit.Drawing.Geometry.Parser
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 25SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// The TransformListParser class converts attributes
    /// conforming to the SVG
    /// <a href="http://www.w3.org/TR/SVG11/coords.html#TransformAttribute">
    /// transform</a>
    /// syntax into AffineTransform objects.
    /// </summary>
    internal class TransformListParser : NumberParser
    {
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses the transform list.
        /// </summary>
        /// <param name="txfStr">the string containing the set of transform commands</param>
        /// <returns>An AffineTransform object corresponding to the
        /// input transform list.</returns>
        public AffineTransform ParseTransformList(string txfStr)
        {
            SetString(txfStr);

            _transform = new AffineTransform(1, 0, 0, 1, 0, 0);

            // Parse leading wsp*
            _current = Read();
            SkipSpaces();

            // Now, iterate on 'transforms?'
            for (;;)
            {
                switch (_current)
                {
                    case 'm':
                        ParseMatrix();
                        break;
                    case 'r':
                        ParseRotate();
                        break;
                    case 't':
                        ParseTranslate();
                        break;
                    case 's':
                        _current = Read();
                        switch (_current)
                        {
                            case 'c':
                                ParseScale();
                                break;
                            case 'k':
                                ParseSkew();
                                break;
                            default:
                                throw new ArgumentException();
                        }
                        break;
                    case -1:
                        goto loop2;
                    default:
                        throw new ArgumentException();
                }
                _current = Read();
                SkipCommaSpaces();
            }
            loop2:
            return _transform;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses a matrix transform. 'm' is assumed to be the current character.
        /// </summary>
        protected void ParseMatrix()
        {
            _current = Read();

            // Parse 'atrix wsp? ( wsp?'
            if (_current != 'a')
            {
                throw new ArgumentException();
            }
            _current = Read();
            if (_current != 't')
            {
                throw new ArgumentException();
            }
            _current = Read();
            if (_current != 'r')
            {
                throw new ArgumentException();
            }
            _current = Read();
            if (_current != 'i')
            {
                throw new ArgumentException();
            }
            _current = Read();
            if (_current != 'x')
            {
                throw new ArgumentException();
            }
            _current = Read();
            SkipSpaces();
            if (_current != '(')
            {
                throw new ArgumentException();
            }
            _current = Read();
            SkipSpaces();

            float a = ParseNumber();
            SkipCommaSpaces();
            float b = ParseNumber();
            SkipCommaSpaces();
            float c = ParseNumber();
            SkipCommaSpaces();
            float d = ParseNumber();
            SkipCommaSpaces();
            float e = ParseNumber();
            SkipCommaSpaces();
            float f = ParseNumber();

            SkipSpaces();

            if (_current != ')')
            {
                throw new ArgumentException("Expected ')' and got >"
                                            + (char) _current + "<");
            }

            _transform.Concatenate(new AffineTransform(a, b, c, d, e, f));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses a rotate transform. 'r' is assumed to be the current character.
        /// </summary>
        protected void ParseRotate()
        {
            _current = Read();

            // Parse 'otate wsp? ( wsp?'
            if (_current != 'o')
            {
                throw new ArgumentException();
            }
            _current = Read();
            if (_current != 't')
            {
                throw new ArgumentException();
            }
            _current = Read();
            if (_current != 'a')
            {
                throw new ArgumentException();
            }
            _current = Read();
            if (_current != 't')
            {
                throw new ArgumentException();
            }
            _current = Read();
            if (_current != 'e')
            {
                throw new ArgumentException();
            }
            _current = Read();
            SkipSpaces();

            if (_current != '(')
            {
                throw new ArgumentException();
            }
            _current = Read();
            SkipSpaces();

            float theta = ParseNumber();
            SkipSpaces();

            switch (_current)
            {
                case ')':
                    _transform.Rotate(theta);
                    return;
                case ',':
                    _current = Read();
                    SkipSpaces();
                    break;
                default:
                    // nothing.
                    break;
            }

            float cx = ParseNumber();
            SkipCommaSpaces();
            float cy = ParseNumber();

            SkipSpaces();
            if (_current != ')')
            {
                throw new ArgumentException();
            }

            _transform.Translate(cx, cy);
            _transform.Rotate(theta);
            _transform.Translate(-cx, -cy);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses a translate transform. 't' is assumed to be the current character.
        /// </summary>
        protected void ParseTranslate()
        {
            _current = Read();

            // Parse 'ranslate wsp? ( wsp?'
            if (_current != 'r')
            {
                throw new ArgumentException();
            }
            _current = Read();
            if (_current != 'a')
            {
                throw new ArgumentException();
            }
            _current = Read();
            if (_current != 'n')
            {
                throw new ArgumentException();
            }
            _current = Read();
            if (_current != 's')
            {
                throw new ArgumentException();
            }
            _current = Read();
            if (_current != 'l')
            {
                throw new ArgumentException();
            }
            _current = Read();
            if (_current != 'a')
            {
                throw new ArgumentException();
            }
            _current = Read();
            if (_current != 't')
            {
                throw new ArgumentException();
            }
            _current = Read();
            if (_current != 'e')
            {
                throw new ArgumentException();
            }
            _current = Read();
            SkipSpaces();
            if (_current != '(')
            {
                throw new ArgumentException();
            }
            _current = Read();
            SkipSpaces();

            float tx = ParseNumber();
            SkipSpaces();

            switch (_current)
            {
                case ')':
                    _transform.Translate(tx, 0);
                    return;
                case ',':
                    _current = Read();
                    SkipSpaces();
                    break;
                default:
                    // nothing
                    break;
            }

            float ty = ParseNumber();

            SkipSpaces();
            if (_current != ')')
            {
                throw new ArgumentException();
            }

            _transform.Translate(tx, ty);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses a scale transform. 'c' is assumed to be the current character.
        /// </summary>
        protected void ParseScale()
        {
            _current = Read();

            // Parse 'ale wsp? ( wsp?'
            if (_current != 'a')
            {
                throw new ArgumentException();
            }
            _current = Read();
            if (_current != 'l')
            {
                throw new ArgumentException();
            }
            _current = Read();
            if (_current != 'e')
            {
                throw new ArgumentException();
            }
            _current = Read();
            SkipSpaces();
            if (_current != '(')
            {
                throw new ArgumentException();
            }
            _current = Read();
            SkipSpaces();

            float sx = ParseNumber();
            SkipSpaces();

            switch (_current)
            {
                case ')':
                    _transform.Scale(sx, sx);
                    return;
                case ',':
                    _current = Read();
                    SkipSpaces();
                    break;
                default:
                    // nothing
                    break;
            }

            float sy = ParseNumber();

            SkipSpaces();
            if (_current != ')')
            {
                throw new ArgumentException();
            }

            _transform.Scale(sx, sy);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses a skew transform. 'e' is assumed to be the current character.
        /// </summary>
        protected void ParseSkew()
        {
            _current = Read();

            // Parse 'ew[XY] wsp? ( wsp?'
            if (_current != 'e')
            {
                throw new ArgumentException();
            }
            _current = Read();
            if (_current != 'w')
            {
                throw new ArgumentException();
            }
            _current = Read();

            bool skewX = false;
            switch (_current)
            {
                case 'X':
                    skewX = true;
                    break;
                case 'Y':
                    break;
                default:
                    throw new ArgumentException();
            }
            _current = Read();
            SkipSpaces();
            if (_current != '(')
            {
                throw new ArgumentException();
            }
            _current = Read();
            SkipSpaces();

            float sk = ParseNumber();

            SkipSpaces();
            if (_current != ')')
            {
                throw new ArgumentException();
            }

            float tan = (float) MathEx.Tan(MathEx.ToRadians(sk));

            if (skewX)
            {
                AffineTransform shear = new AffineTransform(1, 0, tan, 1, 0, 0);
                _transform.Concatenate(shear);
                // transform.shear(tan, 0);
            }
            else
            {
                AffineTransform shear = new AffineTransform(1, tan, 0, 1, 0, 0);
                _transform.Concatenate(shear);
                // transform.shear(0, tan);
            }
        }

        /**
         * Captures the transform built by this parser
         */
        private AffineTransform _transform;
    }
}