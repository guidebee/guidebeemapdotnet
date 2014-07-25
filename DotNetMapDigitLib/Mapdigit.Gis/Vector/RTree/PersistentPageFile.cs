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
using System;
using System.IO;
using Mapdigit.Util;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Gis.Vector.RTree
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 02OCT2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// A page file that stores all pages into Persistent storage. It uses a
    /// RandomAccessFile to store node data.
    /// The format of the page file is as follows. First, a header is writen
    /// that stores important information
    /// about the RTree. The header format is as shown below:
    /// int dimension
    /// double fillFactor
    /// int nodeCapacity
    /// int pageSize
    /// int treeType
    /// 
    /// All the pages are stored after the header, with the following format:
    /// int parent
    /// int level
    /// int usedSpace
    /// // HyperCubes
    /// for (i in usedSpace)
    /// for (j in dimension) {
    /// double p(i)1 [j]
    /// double p(i)2 [j]
    /// }
    /// int branch
    /// }
    /// 
    /// Deleted pages are stored into a Stack. If a new entry is inserted it
    /// is placed in the last deleted page.
    /// That way the page file does not grow for ever. 
    /// </summary>
    internal class PersistentPageFile : PageFile
    {

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="PersistentPageFile"/> class.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="size">The size.</param>
        public PersistentPageFile(BinaryReader reader, long offset,
                long size)
        {
            _reader = reader;
            _offset = offset;
            _size = size;
            if (size >= HeaderSize)
            {
                DataReader.Seek(reader, offset);
                Dimension = DataReader.ReadInt(reader);
                FillFactor = DataReader.ReadDouble(reader);
                NodeCapacity = DataReader.ReadInt(reader);
                PageSize = DataReader.ReadInt(reader);
                TreeType = DataReader.ReadInt(reader);

            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Closes this file.
        /// </summary>
        public void Close()
        {
            _reader.Close();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// get the size of the file.
        /// </summary>
        /// <returns></returns>
        public long Size()
        {
            return _size;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the node stored in the requested page.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        internal override AbstractNode ReadNode(int page)
        {
            if (page < 0)
            {
                throw new ArgumentException("Page number cannot be negative.");
            }

            try
            {
                DataReader.Seek(_reader, _offset + HeaderSize + page * PageSize);

                byte[] b = new byte[PageSize];
                int l = _reader.Read(b, 0, b.Length);
                if (-1 == l)
                {
                    throw new PageFaultException("EOF found while trying to read page "
                            + page + ".");
                }

                BinaryReader ds = new BinaryReader(new MemoryStream(b));

                int parent = DataReader.ReadInt(ds);
                if (parent == EmptyPage)
                {
                    throw new PageFaultException("Page " + page + " is empty.");
                }

                int level = DataReader.ReadInt(ds);
                int usedSpace = DataReader.ReadInt(ds);

                AbstractNode n;
                if (level != 0)
                {
                    n = new Index(Tree, parent, page, level);
                }
                else
                {
                    n = new Leaf(Tree, parent, page);
                }

                n.Parent = parent;
                n.Level = level;
                n.UsedSpace = usedSpace;

                double[] p1 = new double[Dimension];
                double[] p2 = new double[Dimension];

                for (int i = 0; i < usedSpace; i++)
                {
                    for (int j = 0; j < Dimension; j++)
                    {
                        p1[j] = DataReader.ReadDouble(ds);
                        p2[j] = DataReader.ReadDouble(ds);
                    }

                    n.Data[i] = new HyperCube(new Point(p1), new Point(p2));
                    n.Branches[i] = DataReader.ReadInt(ds);
                }

                return n;
            }
            catch (IOException)
            {

                return null;
            }

        }

        private const int EmptyPage = -2;
        /**
         * Stores node data into Persistent storage.
         */
        private readonly BinaryReader _reader;
        /**
         * Header size calculated using the following formula:
         * headerSize = dimension + fillFactor + nodeCapacity + pageSize + treeType
         */
        private const int HeaderSize = 24;
        private readonly long _offset;
        private readonly long _size;
    }
}
