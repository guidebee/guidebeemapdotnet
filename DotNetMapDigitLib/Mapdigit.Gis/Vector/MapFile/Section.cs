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
    /// Abstract class for one section in the map file
    /// </summary>
    internal abstract class Section
    {

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="Section"/> class.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="size">The size.</param>
        protected Section(BinaryReader reader,
                long offset, long size)
        {
            this.offset = offset;
            this.reader = reader;
            _size = size;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// return the size of this section.
        /// </summary>
        /// <returns>the size of this section.</returns>
        public long GetSize()
        {
            return _size;
        }


        /**
         * the start offset(in map file) of this section.
         */
        protected long offset;

        /**
         * the data input reader for the map file.
         */
        protected BinaryReader reader;

        /**
         * the size(length) of this section.
         */
        internal long _size;

    }

}
