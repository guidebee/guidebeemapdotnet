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
    /// Implements basic functions of Node interface. Also implements splitting
    /// functions.
    /// </summary>
    internal abstract class AbstractNode : INode
    {

        /**
         * Level of this node. Leaves always have a level equal to 0.
         */
        public int Level;
        /**
         * Parent of all nodes.
         */
        public RTree Tree;
        /**
         * The pageNumber where the parent of this node is stored.
         */
        public int Parent;
        /**
         * The pageNumber where this node is stored.
         */
        public int PageNumber;
        /**
         * All node data are stored into this array. It must have a size of
         * <B>nodeCapacity + 1</B> to hold
         * all data plus an overflow HyperCube, when the node must be split.
         */
        public HyperCube[] Data;
        /**
         * Holds the pageNumbers containing the children of this node.
         * Always has same size with the data array.
         * If this is a Leaf node, than all branches should point to the real
         * data objects.
         */
        public int[] Branches;
        /**
         * How much space is used up into this node. If equal to nodeCapacity
         * then node is full.
         */
        public int UsedSpace;


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the node level. Always zero for leaf nodes.
        /// </summary>
        /// <returns></returns>
        public int GetLevel()
        {
            return Level;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns true if this node is the root node.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance is root; otherwise, <c>false</c>.
        /// </returns>
        public bool IsRoot()
        {
            return (Parent == RTree.Nil);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns true if this node is an Index. Root node is an Index too,
        /// unless it is a Leaf.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance is index; otherwise, <c>false</c>.
        /// </returns>
        public bool IsIndex()
        {
            return (Level != 0);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns true if this node is a Leaf. Root may be a Leaf too.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance is leaf; otherwise, <c>false</c>.
        /// </returns>
        public bool IsLeaf()
        {
            return (Level == 0);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the mbb of all HyperCubes present in this node.
        /// </summary>
        /// <returns>
        /// A new HyperCube object, representing the mbb of this node
        /// </returns>
        public HyperCube GetNodeMbb()
        {
            if (UsedSpace > 0)
            {
                HyperCube[] h = new HyperCube[UsedSpace];
                Array.Copy(Data, 0, h, 0, UsedSpace);
                return HyperCube.GetUnionMbb(h);
            }
            return new HyperCube(new Point(new double[] { 0, 0 }),
                                 new Point(new double[] { 0, 0 }));
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a unique id for this node. The page number is unique for every  node.
        /// </summary>
        /// <returns>
        /// A string representing a unique id for this node
        /// </returns>
        public string GetUniqueId()
        {
            return PageNumber.ToString();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the parent of this node. If there is a parent, it must be an
        /// Index.If this node is the root, returns null. This function loads one
        /// disk page into main memory.
        /// </summary>
        /// <returns></returns>
        public AbstractNode GetParent()
        {
            if (IsRoot())
            {
                return null;
            }
            return Tree._file.ReadNode(Parent);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Return a copy of the HyperCubes present in this node.
        /// </summary>
        /// <returns>
        /// An array of HyperCubes containing copies of the original data
        /// </returns>
        public HyperCube[] GetHyperCubes()
        {
            HyperCube[] h = new HyperCube[UsedSpace];

            for (int i = 0; i < UsedSpace; i++)
            {
                h[i] = (HyperCube)Data[i].Clone();
            }

            return h;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            string s = "< Page: " + PageNumber + ", Level: "
                    + Level + ", UsedSpace: " + UsedSpace
                    + ", Parent: " + Parent + " >\n";

            for (int i = 0; i < UsedSpace; i++)
            {
                s += "  " + (i + 1) + ") " + Data[i]
                        + " --> " + " page: " + Branches[i] + "\n";
            }

            return s;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractNode"/> class.
        /// </summary>
        /// <param name="tree">The tree.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="level">The level.</param>
        protected AbstractNode(RTree tree, int parent, int pageNumber, int level)
        {
            Parent = parent;
            Tree = tree;
            PageNumber = pageNumber;
            Level = level;
            Data = new HyperCube[tree.GetNodeCapacity() + 1];
            Branches = new int[tree.GetNodeCapacity() + 1];
            UsedSpace = 0;
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
        /// <param name="h"> The new HyperCube</param>
        /// <returns>The leaf where the new HyperCube should be inserted</returns>
        internal abstract Leaf ChooseLeaf(HyperCube h);

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
        /// <returns>The leaf where the HyperCube is contained, null if such a leaf is not found.</returns>
        internal abstract Leaf FindLeaf(HyperCube h);
    }



}
