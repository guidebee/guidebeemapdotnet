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
    /// Internal node of the RTree. Used to access Leaf nodes, where real data lies
    /// </summary>
    internal class Index : AbstractNode
    {

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Retrieves the <B>i-th</B> child node. Loads one page into main memory
        /// </summary>
        /// <param name="i">The index of the child in the data array</param>
        /// <returns>The i-th child</returns>
        public AbstractNode GetChild(int i)
        {
            if (i < 0 || i >= UsedSpace)
            {
                throw new IndexOutOfRangeException("" + i);
            }

            return Tree._file.ReadNode(Branches[i]);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="Index"/> class.
        /// </summary>
        /// <param name="tree">The tree.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="level">The level.</param>
        internal Index(RTree tree, int parent, int pageNumber, int level)
            : base(tree, parent, pageNumber, level)
        {

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
            int i;

            switch (Tree.GetTreeType())
            {
                case RTree.RtreeLinear:
                case RTree.RtreeQuadratic:
                case RTree.RtreeExponential:
                    i = FindLeastEnlargement(h);
                    break;
                case RTree.Rstar:
                    if (Level == 1)
                    {
                        // if this node points to leaves...
                        i = FindLeastOverlap(h);
                    }
                    else
                    {
                        i = FindLeastEnlargement(h);
                    }
                    break;
                default:
                    throw new Exception("Invalid tree type.");
            }

            return GetChild(i).ChooseLeaf(h);
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Add the new HyperCube to all mbbs present in this node. Calculate the 
        /// area difference and choose the entry with the least enlargement. Based 
        /// on that metric we choose the path that leads to the leaf that will 
        /// hold the new HyperCube.
        /// [A. Guttman 'R-trees a dynamic index structure for spatial searching']
        /// </summary>
        /// <param name="h">The h.</param>
        /// <returns>The index of the branch of the path that leads to the Leaf where 
        /// the new HyperCube should be inserted.</returns>
        private int FindLeastEnlargement(HyperCube h)
        {
            double area = Double.PositiveInfinity;
            int sel = -1;

            for (int i = 0; i < UsedSpace; i++)
            {
                double enl = Data[i].GetUnionMbb(h).GetArea() - Data[i].GetArea();
                if (enl < area)
                {
                    area = enl;
                    sel = i;
                }
                else if (enl == area)
                {
                    sel = (Data[sel].GetArea() <= Data[i].GetArea()) ? sel : i;
                }
            }
            return sel;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// R*-tree criterion for choosing the best branch to follow.
        /// [Beckmann, Kriegel, Schneider, Seeger 'The R*-tree: An efficient and 
        /// Robust Access Method for Points and Rectangles]
        /// </summary>
        /// <param name="h">The h.</param>
        /// <returns>The index of the branch of the path that leads to the Leaf where 
        /// the new HyperCube should be inserted.</returns>
        private int FindLeastOverlap(HyperCube h)
        {
            float overlap = float.PositiveInfinity;
            int sel = -1;

            for (int i = 0; i < UsedSpace; i++)
            {
                AbstractNode n = GetChild(i);
                float o = 0;
                for (int j = 0; j < n.Data.Length; j++)
                {
                    o += (float)h.IntersectingArea(n.Data[j]);
                }
                if (o < overlap)
                {
                    overlap = o;
                    sel = i;
                }
                else if (o == overlap)
                {
                    double area1 = Data[i].GetUnionMbb(h).GetArea()
                        - Data[i].GetArea();
                    double area2 = Data[sel].GetUnionMbb(h).GetArea()
                        - Data[sel].GetArea();

                    if (area1 == area2)
                    {
                        sel = (Data[sel].GetArea() <= Data[i].GetArea()) ? sel : i;
                    }
                    else
                    {
                        sel = (area1 < area2) ? i : sel;
                    }
                }
            }
            return sel;
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
                    Leaf l = GetChild(i).FindLeaf(h);
                    if (l != null)
                    {
                        return l;
                    }
                }
            }

            return null;
        }
    }
}
