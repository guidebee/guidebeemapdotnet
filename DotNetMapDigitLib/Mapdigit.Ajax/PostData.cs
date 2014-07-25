//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 23SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Ajax
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 23SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// PostData defines HTTP multi-part Post message contents.
    /// </summary>
    public class PostData
    {

        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 23SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="PostData"/> class.
        /// </summary>
        /// <param name="parts">POST multipart message array.</param>
        /// <param name="boundary">Boundray string splits the message body.</param>
        public PostData(Part[] parts, string boundary)
        {
            if (parts == null)
            {
                throw new ArgumentException("parts must be supplied");
            }

            if (parts.Length > 1 && boundary == null)
            {
                throw new ArgumentException
                        ("boundary must be specified for multipart");
            }

            _parts = parts;
            _boundary = boundary;
        }

        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 23SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Check if the message is multi-parted.
        /// </summary>
        /// <returns>
        /// 	true if the message is a multipart message.
        /// </returns>
        public bool IsMultiPart()
        {
            return _parts.Length > 1;
        }

        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 23SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// return all message bodys.
        /// </summary>
        /// <returns>all message bodys</returns>
        public Part[] GetParts()
        {
            return _parts;
        }

        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 23SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the boundary.
        /// </summary>
        /// <returns>the boundary string.</returns>
        public string GetBoundary()
        {
            return _boundary;
        }

        private readonly Part[] _parts;
        private readonly string _boundary;

    }

}
