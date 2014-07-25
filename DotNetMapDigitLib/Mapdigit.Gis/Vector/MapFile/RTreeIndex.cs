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
using Mapdigit.Gis.Vector.RTree;
using BinaryReader = System.IO.BinaryReader;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Gis.Vector.MapFile
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 02OCT2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// rtree index section of the map file.
    /// </summary>
    internal class RTreeIndex : Section
    {

        /**
         * the rtree index file.
         */
        public PersistentPageFile File;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="RTreeIndex"/> class.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// constructor.
        public RTreeIndex(BinaryReader reader, long offset, long size)
            : base(reader, offset, size)
        {

            File = new PersistentPageFile(reader, offset, size);
        }

    }

}
