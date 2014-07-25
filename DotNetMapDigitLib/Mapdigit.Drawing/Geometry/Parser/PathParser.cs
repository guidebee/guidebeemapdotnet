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
    /// The PathParser class converts attributes conforming to the
    /// SVG <a href="http://www.w3.org/TR/SVG11/paths.html#PathDataBNF">path
    /// syntax</a> with the
    /// <a href="http://www.w3.org/TR/SVGMobile/#sec-shapes">limitation</a> of SVG
    /// Tiny which says that SVG Tiny does not support arc to commands.
    /// </summary>
    internal class PathParser : NumberParser
    {
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the Current working path. This can be used,
        /// for example, when the ParsePath method throws an error
        /// to retrieve the state of the path at the time the error occured
        /// </summary>
        /// <returns>the Path built from the parsed
        /// string</returns>
        public Path GetPath()
        {
            return _p;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses the input String and returns the corresponding
        /// Path.
        /// </summary>
        /// <param name="s">the String to parse.</param>
        /// <returns>the Path built from the parsed</returns>
        public Path ParsePoints(string s)
        {
            SetString(s);
            _p = new Path();
            _current = Read();

            SkipSpaces();
            if (_current == -1)
            {
                // No coordinate pair
                return _p;
            }

            // Initial MoveTo
            float x = ParseNumber();
            SkipCommaSpaces();
            float y = ParseNumber();
            _p.MoveTo((int) x, (int) y);
            _lastMoveToX = x;
            _lastMoveToY = y;

            while (_current != -1)
            {
                SkipSpaces();
                if (_current != -1)
                {
                    SkipCommaSpaces();
                    x = ParseNumber();
                    SkipCommaSpaces();
                    y = ParseNumber();
                    _p.LineTo((int) x, (int) y);
                }
            }

            return _p;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses the input String and returns the corresponding
        /// Path if no error is found. If an error occurs,
        /// this method throws an IllegalArgumentException.
        /// </summary>
        /// <param name="s">s the String to parse.</param>
        /// <returns>the Path built from the parsed
        /// String</returns>
        public Path ParsePath(string s)
        {
            SetString(s);
            _p = new Path();


            _currentX = 0;
            _currentY = 0;
            _smoothQCenterX = 0;
            _smoothQCenterY = 0;
            _smoothCCenterX = 0;
            _smoothCCenterY = 0;

            _current = Read();
            SkipSpaces();

            // Multiple coordinate pairs after a moveto
            // are like a moveto followed by lineto
            switch (_current)
            {
                case 'm':
                    Parsem();
                    Parsel();
                    break;
                case 'M':
                    ParseM();
                    ParseL();
                    break;
                case -1:
                    //an empty path is valid.
                    break;
                default:
                    throw new ArgumentException();
            }


            for (;;)
            {
                switch (_current)
                {
                    case 0xD:
                    case 0xA:
                    case 0x20:
                    case 0x9:
                        _current = Read();
                        break;
                    case 'z':
                    case 'Z':
                        _current = Read();
                        _p.ClosePath();
                        _currentX = _lastMoveToX;
                        _currentY = _lastMoveToY;
                        break;
                    case 'm':
                        Parsem();
                        break;
                    case 'l':
                        Parsel();
                        break;
                    case 'M':
                        ParseM();
                        break;
                    case 'L':
                        ParseL();
                        break;
                    case 'h':
                        Parseh();
                        break;
                    case 'H':
                        ParseH();
                        break;
                    case 'v':
                        Parsev();
                        break;
                    case 'V':
                        ParseV();
                        break;
                    case 'c':
                        Parsec();
                        break;
                    case 'C':
                        ParseC();
                        break;
                    case 'q':
                        Parseq();
                        break;
                    case 'Q':
                        ParseQ();
                        break;
                    case 's':
                        Parses();
                        break;
                    case 'S':
                        ParseS();
                        break;
                    case 't':
                        Parset();
                        break;
                    case 'T':
                        ParseT();
                        break;
                    case -1:
                        goto loop;
                    default:
                        throw new ArgumentException();
                }
            }
            loop:
            SkipSpaces();
            if (_current != -1)
            {
                throw new ArgumentException();
            }

            return _p;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses a 'm' command.
        /// </summary>
        protected void Parsem()
        {
            _current = Read();
            SkipSpaces();

            float x = ParseNumber();
            SkipCommaSpaces();
            float y = ParseNumber();

            _currentX += x;
            _smoothQCenterX = _currentX;
            _smoothCCenterX = _currentX;
            _currentY += y;
            _smoothQCenterY = _currentY;
            _smoothCCenterY = _currentY;
            _p.MoveTo((int) _smoothCCenterX, (int) _smoothCCenterY);
            _lastMoveToX = _smoothCCenterX;
            _lastMoveToY = _smoothCCenterY;

            SkipCommaSpaces();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses a 'l' command.
        /// </summary>
        protected void Parsel()
        {
            if (_current == 'l')
            {
                _current = Read();
            }
            SkipSpaces();
            for (;;)
            {
                switch (_current)
                {
                    case '+':
                    case '-':
                    case '.':
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        float x = ParseNumber();
                        SkipCommaSpaces();
                        float y = ParseNumber();

                        _currentX += x;
                        _smoothQCenterX = _currentX;
                        _smoothCCenterX = _currentX;

                        _currentY += y;
                        _smoothQCenterY = _currentY;
                        _smoothCCenterY = _currentY;
                        _p.LineTo((int) _smoothCCenterX, (int) _smoothCCenterY);
                        break;
                    default:
                        return;
                }
                SkipCommaSpaces();
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses a 'M' command.
        /// </summary>
        protected void ParseM()
        {
            _current = Read();
            SkipSpaces();

            float x = ParseNumber();
            SkipCommaSpaces();
            float y = ParseNumber();

            _currentX = x;
            _smoothQCenterX = x;
            _smoothCCenterX = x;

            _currentY = y;
            _smoothQCenterY = y;
            _smoothCCenterY = y;
            _p.MoveTo((int) x, (int) y);
            _lastMoveToX = x;
            _lastMoveToY = y;

            SkipCommaSpaces();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses a 'L' command.
        /// </summary>
        protected void ParseL()
        {
            if (_current == 'L')
            {
                _current = Read();
            }
            SkipSpaces();
            for (;;)
            {
                switch (_current)
                {
                    case '+':
                    case '-':
                    case '.':
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        float x = ParseNumber();
                        SkipCommaSpaces();
                        float y = ParseNumber();

                        _currentX = x;
                        _smoothQCenterX = x;
                        _smoothCCenterX = x;

                        _currentY = y;
                        _smoothQCenterY = y;
                        _smoothCCenterY = y;

                        _p.LineTo((int) _smoothCCenterX, (int) _smoothCCenterY);
                        break;
                    default:
                        return;
                }
                SkipCommaSpaces();
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses a 'h' command.
        /// </summary>
        protected void Parseh()
        {
            _current = Read();
            SkipSpaces();

            for (;;)
            {
                switch (_current)
                {
                    case '+':
                    case '-':
                    case '.':
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        float x = ParseNumber();
                        _currentX += x;
                        _smoothQCenterX = _currentX;
                        _smoothCCenterX = _currentX;

                        _smoothQCenterY = _currentY;
                        _smoothCCenterY = _currentY;
                        _p.LineTo((int) _smoothCCenterX, (int) _smoothCCenterY);
                        break;
                    default:
                        return;
                }
                SkipCommaSpaces();
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses a 'H' command.
        /// </summary>
        protected void ParseH()
        {
            _current = Read();
            SkipSpaces();

            for (;;)
            {
                switch (_current)
                {
                    case '+':
                    case '-':
                    case '.':
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        float x = ParseNumber();
                        _currentX = x;
                        _smoothQCenterX = x;
                        _smoothCCenterX = x;

                        _smoothQCenterY = _currentY;
                        _smoothCCenterY = _currentY;
                        _p.LineTo((int) _smoothCCenterX, (int) _smoothCCenterY);
                        break;
                    default:
                        return;
                }
                SkipCommaSpaces();
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses a 'v' command.
        /// </summary>
        protected void Parsev()
        {
            _current = Read();
            SkipSpaces();

            for (;;)
            {
                switch (_current)
                {
                    case '+':
                    case '-':
                    case '.':
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        float y = ParseNumber();
                        _smoothQCenterX = _currentX;
                        _smoothCCenterX = _currentX;

                        _currentY += y;
                        _smoothQCenterY = _currentY;
                        _smoothCCenterY = _currentY;
                        _p.LineTo((int) _smoothCCenterX, (int) _smoothCCenterY);
                        break;
                    default:
                        return;
                }
                SkipCommaSpaces();
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses a 'V' command.
        /// </summary>
        protected void ParseV()
        {
            _current = Read();
            SkipSpaces();

            for (;;)
            {
                switch (_current)
                {
                    case '+':
                    case '-':
                    case '.':
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        float y = ParseNumber();
                        _smoothQCenterX = _currentX;
                        _smoothCCenterX = _currentX;

                        _currentY = y;
                        _smoothQCenterY = y;
                        _smoothCCenterY = y;
                        _p.LineTo((int) _smoothCCenterX, (int) _smoothCCenterY);
                        break;
                    default:
                        return;
                }
                SkipCommaSpaces();
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses a 'c' command.
        /// </summary>
        protected void Parsec()
        {
            _current = Read();
            SkipSpaces();

            for (;;)
            {
                switch (_current)
                {
                    default:
                        return;
                    case '+':
                    case '-':
                    case '.':
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        break;
                }

                float x1 = ParseNumber();
                SkipCommaSpaces();
                float y1 = ParseNumber();
                SkipCommaSpaces();
                float x2 = ParseNumber();
                SkipCommaSpaces();
                float y2 = ParseNumber();
                SkipCommaSpaces();
                float x = ParseNumber();
                SkipCommaSpaces();
                float y = ParseNumber();

                _smoothCCenterX = _currentX + x2;
                _smoothCCenterY = _currentY + y2;
                _smoothQCenterX = _currentX + x;
                _smoothQCenterY = _currentY + y;
                _p.CurveTo((int) (_currentX + x1), (int) (_currentY + y1),
                           (int) _smoothCCenterX, (int) _smoothCCenterY,
                           (int) _smoothQCenterX, (int) _smoothQCenterY);
                _currentX = _smoothQCenterX;
                _currentY = _smoothQCenterY;
                SkipCommaSpaces();
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses a 'C' command.
        /// </summary>
        protected void ParseC()
        {
            _current = Read();
            SkipSpaces();

            for (;;)
            {
                switch (_current)
                {
                    default:
                        return;
                    case '+':
                    case '-':
                    case '.':
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        break;
                }

                float x1 = ParseNumber();
                SkipCommaSpaces();
                float y1 = ParseNumber();
                SkipCommaSpaces();
                float x2 = ParseNumber();
                SkipCommaSpaces();
                float y2 = ParseNumber();
                SkipCommaSpaces();
                float x = ParseNumber();
                SkipCommaSpaces();
                float y = ParseNumber();

                _smoothCCenterX = x2;
                _smoothCCenterY = y2;
                _currentX = x;
                _currentY = y;
                _p.CurveTo((int) x1, (int) y1, (int) _smoothCCenterX, (int) _smoothCCenterY,
                           (int) _currentX, (int) _currentY);
                _smoothQCenterX = _currentX;
                _smoothQCenterY = _currentY;
                SkipCommaSpaces();
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses a 'q' command.
        /// </summary>
        protected void Parseq()
        {
            _current = Read();
            SkipSpaces();

            for (;;)
            {
                switch (_current)
                {
                    default:
                        return;
                    case '+':
                    case '-':
                    case '.':
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        break;
                }

                float x1 = ParseNumber();
                SkipCommaSpaces();
                float y1 = ParseNumber();
                SkipCommaSpaces();
                float x = ParseNumber();
                SkipCommaSpaces();
                float y = ParseNumber();

                _smoothQCenterX = _currentX + x1;
                _smoothQCenterY = _currentY + y1;
                _currentX += x;
                _currentY += y;
                _p.QuadTo((int) _smoothQCenterX, (int) _smoothQCenterY, (int) _currentX, (int) _currentY);
                _smoothCCenterX = _currentX;
                _smoothCCenterY = _currentY;

                SkipCommaSpaces();
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses a 'Q' command.
        /// </summary>
        protected void ParseQ()
        {
            _current = Read();
            SkipSpaces();

            for (;;)
            {
                switch (_current)
                {
                    default:
                        return;
                    case '+':
                    case '-':
                    case '.':
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        break;
                }

                float x1 = ParseNumber();
                SkipCommaSpaces();
                float y1 = ParseNumber();
                SkipCommaSpaces();
                float x = ParseNumber();
                SkipCommaSpaces();
                float y = ParseNumber();

                _smoothQCenterX = x1;
                _smoothQCenterY = y1;
                _currentX = x;
                _currentY = y;
                _p.QuadTo((int) _smoothQCenterX, (int) _smoothQCenterY, (int) _currentX, (int) _currentY);
                _smoothCCenterX = _currentX;
                _smoothCCenterY = _currentY;
                SkipCommaSpaces();
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses a 's' command.
        /// </summary>
        protected void Parses()
        {
            _current = Read();
            SkipSpaces();

            for (;;)
            {
                switch (_current)
                {
                    default:
                        return;
                    case '+':
                    case '-':
                    case '.':
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        break;
                }

                float x2 = ParseNumber();
                SkipCommaSpaces();
                float y2 = ParseNumber();
                SkipCommaSpaces();
                float x = ParseNumber();
                SkipCommaSpaces();
                float y = ParseNumber();

                float smoothX = _currentX*2 - _smoothCCenterX;
                float smoothY = _currentY*2 - _smoothCCenterY;
                _smoothCCenterX = _currentX + x2;
                _smoothCCenterY = _currentY + y2;
                _currentX += x;
                _currentY += y;

                _p.CurveTo((int) smoothX, (int) smoothY,
                           (int) _smoothCCenterX, (int) _smoothCCenterY,
                           (int) _currentX, (int) _currentY);

                _smoothQCenterX = _currentX;
                _smoothQCenterY = _currentY;
                SkipCommaSpaces();
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses a 'S' command.
        /// </summary>
        protected void ParseS()
        {
            _current = Read();
            SkipSpaces();

            for (;;)
            {
                switch (_current)
                {
                    default:
                        return;
                    case '+':
                    case '-':
                    case '.':
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        break;
                }

                float x2 = ParseNumber();
                SkipCommaSpaces();
                float y2 = ParseNumber();
                SkipCommaSpaces();
                float x = ParseNumber();
                SkipCommaSpaces();
                float y = ParseNumber();

                float smoothX = _currentX*2 - _smoothCCenterX;
                float smoothY = _currentY*2 - _smoothCCenterY;
                _currentX = x;
                _currentY = y;
                _p.CurveTo((int) smoothX, (int) smoothY,
                           (int) x2, (int) y2,
                           (int) _currentX, (int) _currentY);
                _smoothCCenterX = x2;
                _smoothCCenterY = y2;
                _smoothQCenterX = _currentX;
                _smoothQCenterY = _currentY;

                SkipCommaSpaces();
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses a 't' command.
        /// </summary>
        protected void Parset()
        {
            _current = Read();
            SkipSpaces();

            for (;;)
            {
                switch (_current)
                {
                    default:
                        return;
                    case '+':
                    case '-':
                    case '.':
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        break;
                }

                float x = ParseNumber();
                SkipCommaSpaces();
                float y = ParseNumber();

                _smoothQCenterX = _currentX*2 - _smoothQCenterX;
                _smoothQCenterY = _currentY*2 - _smoothQCenterY;
                _currentX += x;
                _currentY += y;
                _p.QuadTo((int) _smoothQCenterX, (int) _smoothQCenterY, (int) _currentX, (int) _currentY);
                _smoothCCenterX = _currentX;
                _smoothCCenterY = _currentY;
                SkipCommaSpaces();
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 25SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses a 'T' command.
        /// </summary>
        protected void ParseT()
        {
            _current = Read();
            SkipSpaces();

            for (;;)
            {
                switch (_current)
                {
                    default:
                        return;
                    case '+':
                    case '-':
                    case '.':
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        break;
                }

                float x = ParseNumber();
                SkipCommaSpaces();
                float y = ParseNumber();

                _smoothQCenterX = _currentX*2 - _smoothQCenterX;
                _smoothQCenterY = _currentY*2 - _smoothQCenterY;
                _currentX = x;
                _currentY = y;
                _p.QuadTo((int) _smoothQCenterX, (int) _smoothQCenterY,
                          (int) _currentX, (int) _currentY);
                _smoothCCenterX = _currentX;
                _smoothCCenterY = _currentY;
                SkipCommaSpaces();
            }
        }

        /**
         * Current x and y positions in the path, set by
         * commands such as MoveTo or LineTo.
         */
        private float _currentX, _currentY;
        /**
         * Last MoveTo command.
         */
        private float _lastMoveToX, _lastMoveToY;
        /**
         * The smoothQCenter point is used for smootg quad curves
         */
        private float _smoothQCenterX, _smoothQCenterY;
        /**
         * The smoothQCenter point is used for smooth cubic curves
         */
        private float _smoothCCenterX, _smoothCCenterY;
        /**
         * The GeneralPath under construction
         */
        private Path _p;
    }
}