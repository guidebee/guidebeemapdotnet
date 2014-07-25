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
    /// Abstract class for all classes implementing a storage manager for the RTree.
    /// Every node should be stored in a unique page. The root node is always stored
    /// in page 0. The storage manager should have the control over the page numbers
    /// where each node should be stored.
    /// </summary>
    internal abstract class PageFile
    {

        /**
         * related rtree object.
         */
        public RTree Tree;

        /**
         * Dimension of data inserted into the tree.
         */
        public int Dimension = -1;

        /**
         * fillFactor specifies minimum node entries present in each node.
         * It must be a double between 0 and 0.5.
         */
        public double FillFactor = -1;

        /**
         * Maximum node capacity. Each node will be able to hold at most 
         * nodeCapacity entries.
         */
        public int NodeCapacity = -1;

        /**
         * The page size needed in bytes to store a full node. Calculated using
         * the following formula:
         * [nodeCapacity * (sizeof(HyperCube) + sizeof(Branch))] + 
         *    parent + level + usedSpace =
         * {nodeCapacity * [(2 * dimension * sizeof(double)) + sizeof(int)]} +
         * sizeof(int) + sizeof(int) + sizeof(int)
         */
        public int PageSize = -1;

        /**
         * RTree variant used. Specified when creating a new tree.
         */
        public int TreeType = -1;


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
        internal abstract AbstractNode ReadNode(int page);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes the specified tree.
        /// </summary>
        /// <param name="tree">The tree.</param>
        /// <param name="dimension">The dimension.</param>
        /// <param name="fillFactor">The fill factor.</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="treeType">Type of the tree.</param>
        protected void Initialize(RTree tree, int dimension, float fillFactor,
                int capacity, int treeType)
        {
            Dimension = dimension;
            FillFactor = fillFactor;
            NodeCapacity = capacity;
            TreeType = treeType;
            Tree = tree;
            PageSize = capacity * (8 * dimension + 4) + 12;
        }
    }
}
