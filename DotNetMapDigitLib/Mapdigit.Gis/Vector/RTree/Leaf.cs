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
    /// A Leaf node. Containts pointers to the real data.
    /// </summary>
    internal class Leaf : AbstractNode
    {

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the pointer of the <i>i-th</i> data entry.
        /// </summary>
        /// <param name="i">The index of the child in the data array.</param>
        /// <returns>The pointer of the <i>i-th</i> child.</returns>
        public int GetDataPointer(int i)
        {
            if (i < 0 || i >= UsedSpace)
            {
                throw new IndexOutOfRangeException("" + i);
            }

            return Branches[i];
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="Leaf"/> class.
        /// </summary>
        /// <param name="tree">The tree.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="pageNumber">The page number.</param>
        internal Leaf(RTree tree, int parent, int pageNumber)
            : base(tree, parent, pageNumber, 0)
        {
            // Leaf nodes belong by default to level 0.

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="Leaf"/> class.
        /// </summary>
        /// <param name="tree">The tree.</param>
        /// <param name="parent">The parent.</param>
        protected Leaf(RTree tree, int parent)
            : base(tree, parent, -1, 0)
        {
            // Leaf nodes belong by default to level 0.

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// chooseLeaf finds the most appropriate leaf where the given HyperCube
        /// should be stored.
        /// </summary>
        /// <param name="h">The new HyperCube</param>
        /// <returns>
        /// The leaf where the new HyperCube should be inserted
        /// </returns>
        internal override Leaf ChooseLeaf(HyperCube h)
        {
            return this;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// findLeaf returns the leaf that contains the given hypercube, null if the
        /// hypercube is not contained in any of the leaves of this node.
        /// </summary>
        /// <param name="h">The HyperCube to search for.</param>
        /// <returns>
        /// The leaf where the HyperCube is contained, null if such a leaf is not found.
        /// </returns>
        internal override Leaf FindLeaf(HyperCube h)
        {
            for (int i = 0; i < UsedSpace; i++)
            {
                if (Data[i].Enclosure(h))
                {
                    return this;
                }
            }

            return null;
        }

    }


}
