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
using System.Collections;

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
    /// To create a new RTree use the first two constructors. You must specify the
    /// dimension, the fill factor as a float between 0 and 0.5 (0 to 50% capacity)
    /// and the variant of the RTree which is one of:
    /// <ul>
    ///  <li>RtreeQuadratic</li>
    /// </ul>
    /// The first constructor creates by default a new memory resident page file.
    /// The second constructor takes
    /// the page file as an argument. If the given page file is not empty,
    /// then all data are deleted.
    /// 
    /// The third constructor initializes the RTree from an already filled page file.
    /// Thus, you may store the
    /// RTree into a persistent page file and recreate it again at any time.
    /// </summary>
    internal class RTree
    {

        /**
        * version
        */
        public const string Version = "3.0.0";
        /**
         * date
         */
        public const string Date = "Oct 2nd 2010";

        /**
         * Page file where data is stored.
         */
        internal PageFile _file;

        /**
         * static identifier used for the parent of the root node.
         */
        public const int Nil = -1;

        /**
         * Available RTree variants
         */
        public const int RtreeLinear = 0;
        public const int RtreeQuadratic = 1;
        public const int RtreeExponential = 2;
        public const int Rstar = 3;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates an rtree from an already initialized page file, probably stored
        /// into persistent storage.
        /// </summary>
        /// <param name="file">The file.</param>
        public RTree(PageFile file)
        {
            if (file.Tree != null)
            {
                throw new ArgumentException
                        ("PageFile already in use by another rtree instance.");
            }

            if (file.TreeType == -1)
            {
                throw new ArgumentException
                        ("PageFile is empty. Use some other RTree constructor.");
            }

            file.Tree = this;
            _file = file;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Retruns the maximun capacity of each Node.
        /// </summary>
        /// <returns></returns>
        public int GetNodeCapacity()
        {
            return _file.NodeCapacity;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the percentage between 0 and 0.5, used to calculate minimum 
        /// number of entries present in each node.
        /// </summary>
        /// <returns></returns>
        public double GetFillFactor()
        {
            return _file.FillFactor;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the data dimension.
        /// </summary>
        /// <returns></returns>
        public int GetDimension()
        {
            return _file.Dimension;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the size of the page.
        /// </summary>
        /// <returns></returns>
        public int GetPageSize()
        {
            return _file.PageSize;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the level of the root Node, which signifies the level of the 
        /// whole tree. Loads one page into main memory.
        /// </summary>
        /// <returns></returns>
        public int GetTreeLevel()
        {
            return _file.ReadNode(0).GetLevel();
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the RTree variant used
        /// </summary>
        /// <returns></returns>
        public int GetTreeType()
        {
            return _file.TreeType;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a Vector containing all tree nodes from bottom to top, left to 
        /// right.
        /// CAUTION: If the tree is not memory resident, all nodes will be loaded 
        /// into main memory.
        /// </summary>
        /// <param name="root">The node from which the traverse should begin</param>
        /// <returns>A Vector containing all Nodes in the correct order</returns>
        public ArrayList TraverseByLevel(AbstractNode root)
        {
            if (root == null)
            {
                throw new ArgumentException("Node cannot be null.");
            }

            ArrayList ret = new ArrayList();
            ArrayList v = TraversePostOrder(root);

            for (int i = 0; i <= GetTreeLevel(); i++)
            {
                ArrayList a = new ArrayList();
                for (int j = 0; j < v.Count; j++)
                {
                    INode n = (INode)v[j];
                    if (n.GetLevel() == i)
                    {
                        a.Add(n);
                    }
                }
                for (int j = 0; j < a.Count; j++)
                {
                    ret.Add(a[j]);
                }
            }

            return ret;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns an Enumeration containing all tree nodes from bottom to top, 
        /// left to right.
        /// </summary>
        class ByLevelEnum : IEnumeration
        {
            // there is at least one node, the root node.
            private bool _hasNext = true;

            private readonly ArrayList _nodes;

            private int _index;

            public ByLevelEnum(RTree tree)
            {
                AbstractNode root = tree._file.ReadNode(0);
                _nodes = tree.TraverseByLevel(root);
            }

            ////////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS ------------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ----------------------
            // 02OCT2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Determines whether [has more elements].
            /// </summary>
            /// <returns>
            /// 	<c>true</c> if [has more elements]; otherwise, <c>false</c>.
            /// </returns>
            public bool HasMoreElements()
            {
                return _hasNext;
            }

            ////////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS ------------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ----------------------
            // 02OCT2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Nexts the element.
            /// </summary>
            /// <returns></returns>
            public object NextElement()
            {
                if (!_hasNext)
                {
                    throw new ArgumentOutOfRangeException("traver" + "seByLevel");
                }

                object n = _nodes[_index];
                _index++;
                if (_index == _nodes.Count)
                {
                    _hasNext = false;
                }
                return n;
            }
        };

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Post order traverse of tree nodes.
        /// CAUTION: If the tree is not memory resident, all nodes will be loaded 
        /// into main memory.
        /// </summary>
        /// <param name="root"> The node where the traversing should begin</param>
        /// <returns>A Vector containing all tree nodes in the correct order</returns>
        public ArrayList TraversePostOrder(AbstractNode root)
        {
            if (root == null)
            {
                throw new ArgumentException("Node cannot be null.");
            }

            ArrayList v = new ArrayList { root };

            if (root.IsLeaf())
            {
            }
            else
            {
                for (int i = 0; i < root.UsedSpace; i++)
                {
                    ArrayList a = TraversePostOrder(((Index)root).GetChild(i));
                    for (int j = 0; j < a.Count; j++)
                    {
                        v.Add(a[j]);
                    }
                }
            }
            return v;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Post order traverse of all tree nodes, begging with root.
        /// CAUTION: If the tree is not memory resident, all nodes will be loaded 
        /// into main memory.
        /// </summary>
        class PostOrderEnum : IEnumeration
        {
            private bool _hasNext = true;

            private readonly ArrayList _nodes;

            private int _index;

            public PostOrderEnum(RTree tree)
            {
                AbstractNode root = tree._file.ReadNode(0);
                _nodes = tree.TraversePostOrder(root);
            }

            ////////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS ------------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ----------------------
            // 02OCT2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Determines whether [has more elements].
            /// </summary>
            /// <returns>
            /// 	<c>true</c> if [has more elements]; otherwise, <c>false</c>.
            /// </returns>
            public bool HasMoreElements()
            {
                return _hasNext;
            }

            ////////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS ------------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ----------------------
            // 02OCT2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Nexts the element.
            /// </summary>
            /// <returns></returns>
            public object NextElement()
            {
                if (!_hasNext)
                {
                    throw new ArgumentOutOfRangeException("trav" + "erse PostOrder");
                }

                object n = _nodes[_index];
                _index++;
                if (_index == _nodes.Count)
                {
                    _hasNext = false;
                }
                return n;
            }
        };

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Pre order traverse of tree nodes.
        /// CAUTION: If the tree is not memory resident, all nodes will be loaded 
        /// into main memory.
        /// </summary>
        /// <param name="root">The node where the traversing should begin</param>
        /// <returns>A Vector containing all tree nodes in the correct order</returns>
        public ArrayList TraversePreOrder(AbstractNode root)
        {
            if (root == null)
            {
                throw new ArgumentException("Node cannot be null.");
            }

            ArrayList v = new ArrayList();

            if (root.IsLeaf())
            {
                v.Add(root);
            }
            else
            {
                for (int i = 0; i < root.UsedSpace; i++)
                {
                    ArrayList a = TraversePreOrder(((Index)root).GetChild(i));
                    for (int j = 0; j < a.Count; j++)
                    {
                        v.Add(a[j]);
                    }
                }
                v.Add(root);
            }
            return v;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Pre order traverse of all tree nodes, begging with root.
        /// CAUTION: If the tree is not memory resident, all nodes will be loaded 
        /// into main memory. 
        /// </summary>
        class PreOrderEnum : IEnumeration
        {
            private bool _hasNext = true;
            private readonly ArrayList _nodes;
            private int _index;
            public PreOrderEnum(RTree tree)
            {
                AbstractNode root = tree._file.ReadNode(0);
                _nodes = tree.TraversePreOrder(root);
            }

            ////////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS ------------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ----------------------
            // 02OCT2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Determines whether [has more elements].
            /// </summary>
            /// <returns>
            /// 	<c>true</c> if [has more elements]; otherwise, <c>false</c>.
            /// </returns>
            public bool HasMoreElements()
            {
                return _hasNext;
            }

            ////////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS ------------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ----------------------
            // 02OCT2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Nexts the element.
            /// </summary>
            /// <returns></returns>
            public object NextElement()
            {
                if (!_hasNext)
                {
                    throw new ArgumentOutOfRangeException("traverseP" + "reOrder");
                }

                object n = _nodes[_index];
                _index++;
                if (_index == _nodes.Count)
                {
                    _hasNext = false;
                }
                return n;
            }
        };

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a Vector with all nodes that intersect with the given HyperCube.
        /// The nodes are returned in post order traversing
        /// </summary>
        /// <param name="h">The given HyperCube that is tested for overlapping.</param>
        /// <param name="root">The node where the search should begin.</param>
        /// <returns>A Vector containing the appropriate nodes in the correct order</returns>
        public ArrayList Intersection(HyperCube h, AbstractNode root)
        {
            if (h == null || root == null)
            {
                throw new ArgumentException("Arguments cannot be null.");
            }

            if (h.GetDimension() != _file.Dimension)
            {
                throw new ArgumentException
                        ("HyperCube dimension different than RTree dimension.");
            }

            ArrayList v = new ArrayList();

            if (root.GetNodeMbb().Intersection(h))
            {
                v.Add(root);

                if (!root.IsLeaf())
                {
                    for (int i = 0; i < root.UsedSpace; i++)
                    {
                        if (root.Data[i].Intersection(h))
                        {
                            ArrayList a = Intersection(h, ((Index)root).GetChild(i));
                            for (int j = 0; j < a.Count; j++)
                            {
                                v.Add(a[j]);
                            }
                        }
                    }
                }
            }
            return v;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns an Enumeration with all nodes present in the tree that intersect 
        /// with the given HyperCube. The nodes are returned in post order traversing
        /// </summary>
        /// <param name="h">The given HyperCube that is tested for overlapping</param>
        /// <returns>An Enumeration containing the appropriate nodes in the correct 
        /// order.</returns>
        public IEnumeration Enclosure(HyperCube h)
        {

            return new ContainEnum(h, this);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Traverses the by level.
        /// </summary>
        /// <returns></returns>
        public IEnumeration TraverseByLevel()
        {
            return new ByLevelEnum(this);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Post order traverse of all tree nodes, begging with root.
        /// CAUTION: If the tree is not memory resident, all nodes will be loaded into main memory.
        /// </summary>
        public IEnumeration TraversePostOrder()
        {
            return new PostOrderEnum(this);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Traverses the pre order.
        /// </summary>
        /// <returns></returns>
        public IEnumeration TraversePreOrder()
        {
            return new PreOrderEnum(this);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns an Enumeration with all nodes present in the tree that Intersect with the given
        /// HyperCube. The nodes are returned in post order traversing
        /// </summary>
        /// <param name="h">The h.</param>
        /// <returns></returns>
        public IEnumeration Intersection(HyperCube h)
        {
            return new IntersectionEnum(h, this);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        class IntersectionEnum : IEnumeration
        {
            private bool _hasNext = true;

            private readonly ArrayList _nodes;

            private int _index;

            public IntersectionEnum(HyperCube hh, RTree tree)
            {
                _nodes = tree.Intersection(hh, tree._file.ReadNode(0));
                if (_nodes.Count == 0)
                {
                    _hasNext = false;
                }
            }

            public bool HasMoreElements()
            {
                return _hasNext;
            }

            public object NextElement()
            {
                if (!_hasNext)
                {
                    throw new ArgumentOutOfRangeException("intersec" + "tion");
                }

                object c = _nodes[_index];
                _index++;
                if (_index == _nodes.Count)
                {
                    _hasNext = false;
                }
                return c;
            }
        };

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Enclosures the specified h.
        /// </summary>
        /// <param name="h">The h.</param>
        /// <param name="root">The root.</param>
        /// <returns></returns>
        public ArrayList Enclosure(HyperCube h, AbstractNode root)
        {
            if (h == null || root == null) throw new
                    ArgumentException("Arguments cannot be null.");

            if (h.GetDimension() != _file.Dimension) throw new
                    ArgumentException("HyperCube dimension " +
                    "different than RTree dimension.");

            ArrayList v = new ArrayList();

            if (root.GetNodeMbb().Enclosure(h))
            {
                v.Add(root);

                if (!root.IsLeaf())
                {
                    for (int i = 0; i < root.UsedSpace; i++)
                    {
                        if (root.Data[i].Enclosure(h))
                        {
                            ArrayList a = Enclosure(h, ((Index)root).GetChild(i));
                            for (int j = 0; j < a.Count; j++)
                            {
                                v.Add(a[j]);
                            }
                        }
                    }
                }
            }
            return v;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        class ContainEnum : IEnumeration
        {
            private bool _hasNext = true;

            private readonly ArrayList _cubes;

            private int _index;

            public ContainEnum(HyperCube hh, RTree tree)
            {
                _cubes = tree.Enclosure(hh, tree._file.ReadNode(0));
                if (_cubes.Count == 0)
                {
                    _hasNext = false;
                }
            }

            public bool HasMoreElements()
            {
                return _hasNext;
            }

            public object NextElement()
            {
                if (!_hasNext)
                {
                    throw new
                        ArgumentOutOfRangeException("encl" + "osure");
                }

                object c = _cubes[_index];
                _index++;
                if (_index == _cubes.Count)
                {
                    _hasNext = false;
                }
                return c;
            }
        };

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Enclosures the specified p.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="root">The root.</param>
        /// <returns></returns>
        public ArrayList Enclosure(Point p, AbstractNode root)
        {
            return Enclosure(new HyperCube(p, p), root);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Enclosures the specified p.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        public IEnumeration Enclosure(Point p)
        {
            return Enclosure(new HyperCube(p, p));
        }

    }

}
