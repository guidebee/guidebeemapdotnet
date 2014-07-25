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
using Mapdigit.Gis.Geometry;
using System.Collections;
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
    /// Guidebee Map file reader.
    /// </summary>
    internal sealed class MapFile
    {

        /**
         * file header section object.
         */
        public Header Header;

        /**
         * tabular data section.
         */
        public TabularData TabularData;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapFile"/> class.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public MapFile(BinaryReader reader)
        {
            _reader = reader;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Open the map file.
        /// </summary>
        public void Open()
        {
            if (Header != null)
            {
                return;
            }

            Header = new Header(_reader, 0, 0);
            _recordIndex = new RecordIndex(_reader, Header.IndexOffset,
                    Header.IndexLength);
            _stringIndex = new StringIndex(_reader, Header.StringIndexOffset,
                    Header.StringIndexLength);
            _stringData = new StringData(_reader, Header.StringDataOffset,
                    Header.StringDataLength);
            _geoData = new GeoData(_reader, Header.GeoDataOffset,
                    Header.GeoDataLength);
            TabularData = new TabularData(_reader, Header.TabularDataOffset,
                    Header.TabularDataLength, Header.Fields,
                    _stringData, _stringIndex);
            _rtreeIndex = new RTreeIndex(_reader, Header.RtreeIndexOffset,
                    Header.RtreeIndexLength);
            _tree = new RTree.RTree(_rtreeIndex.File);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Closes this map.
        /// </summary>
        public void Close()
        {
            if (_reader != null)
            {
                _reader.Close();
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the record count.
        /// </summary>
        /// <returns></returns>
        public int GetRecordCount()
        {
            if (Header != null)
            {
                return Header.RecordCount;
            }
            return 0;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// get MapObject at given mapInfoID.
        /// </summary>
        /// <param name="mapInfoId">The map info id.</param>
        /// <returns>MapObject at given mapInfoID</returns>
        public MapObject GetMapObject(int mapInfoId)
        {
            _recordIndex.GetRecord(mapInfoId);
            MapObject mapObject = _geoData.GetMapObject(_recordIndex);
            mapObject.MapInfoId = mapInfoId;
            return mapObject;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// get tabular record at given mapInfo ID.
        /// </summary>
        /// <param name="mapInfoId">The map info ID.</param>
        /// <returns>tabular record at given mapInfoID.</returns>
        public DataRowValue GetDataRowValue(int mapInfoId)
        {
            return TabularData.GetRecord(mapInfoId);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// get all records based on search condition.
        /// </summary>
        /// <param name="findConditions">The find conditions.</param>
        /// <returns>a hashtable of all matched record.the key is the mapInfo ID</returns>
        public Hashtable Search(FindConditions findConditions)
        {
            Hashtable retTable = new Hashtable();
            ArrayList allCondition = findConditions.GetConditions();
            for (int i = 0; i < allCondition.Count; i++)
            {
                FindCondition findCondition = (FindCondition)allCondition[i];
                int stringId = BinarySearch(findCondition.MatchString);
                int fieldIndex = findCondition.FieldIndex;
                if (stringId != -1)
                {
                    bool bDone = false;
                    _stringIndex.GetRecord(stringId);
                    string strValue = _stringData.GetMapInfoIdAndField(_stringIndex.RecordOffset);
                    while (strValue.StartsWith(findCondition.MatchString) && (!bDone))
                    {
                        int fieldCount = _stringData.FieldCount;
                        for (int j = 0; j < fieldCount; j++)
                        {
                            if (_stringData.FieldId[j] == fieldIndex)
                            {
                                int mapInfoId = _stringData.MapInfoId[j];
                                if (!retTable.ContainsKey(mapInfoId))
                                {
                                    retTable.Add(mapInfoId,
                                            strValue);
                                }
                                if (retTable.Count > FindConditions.MaxMatchRecord)
                                {
                                    return retTable;
                                }
                            }
                        }
                        bDone = _stringIndex.MovePrevious();
                        if (!bDone)
                        {
                            strValue = _stringData.GetMapInfoIdAndField(_stringIndex.RecordOffset);
                        }
                    }
                    bDone = false;
                    _stringIndex.GetRecord(stringId);
                    strValue = _stringData.GetMapInfoIdAndField(_stringIndex.RecordOffset);
                    while (strValue.StartsWith(findCondition.MatchString) && (!bDone))
                    {
                        int fieldCount = _stringData.FieldCount;
                        for (int j = 0; j < fieldCount; j++)
                        {
                            if (_stringData.FieldId[j] == fieldIndex)
                            {
                                int mapInfoId = _stringData.MapInfoId[j];
                                if (!retTable.ContainsKey(mapInfoId))
                                {
                                    retTable.Add(mapInfoId,
                                            strValue);
                                }
                                if (retTable.Count > FindConditions.MaxMatchRecord)
                                {
                                    return retTable;
                                }
                            }
                        }
                        bDone = _stringIndex.MoveNext();
                        if (!bDone)
                        {
                            strValue = _stringData.GetMapInfoIdAndField(_stringIndex.RecordOffset);
                        }
                    }
                }

            }
            return retTable;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// get table field definition.
        /// </summary>
        /// <returns>table field definition</returns>
        public DataField[] GetFields()
        {
            return Header.Fields;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// get all records based on given rectangle.
        /// </summary>
        /// <param name="rectGeo">the boundary</param>
        /// <returns>a hashtable of all matched record.the key is the mapInfo ID</returns>
        public Hashtable SearchMapObjectsInRect(GeoLatLngBounds rectGeo)
        {
            Point pt1, pt2;
            pt1 = new Point(new[]{
                    (int) (rectGeo.X * DoublePrecision+0.5),
                    (int) (rectGeo.Y * DoublePrecision+0.5)});
            pt2 = new Point(new[]{
                    (int) ((rectGeo.X + rectGeo.Width) * DoublePrecision+0.5),
                    (int) ((rectGeo.Y + rectGeo.Height) * DoublePrecision+0.5)});
            HyperCube h1 = new HyperCube(pt1, pt2);
            Hashtable retArrayList = new Hashtable();
            Point p11, p12;
            for (IEnumeration e1 = _tree.Intersection(h1); e1.HasMoreElements(); )
            {

                AbstractNode node = (AbstractNode)(e1.NextElement());
                if (node.IsLeaf())
                {
                    int index = 0;
                    HyperCube[] data = node.GetHyperCubes();
                    HyperCube cube;
                    for (int cubeIndex = 0; cubeIndex < data.Length; cubeIndex++)
                    {
                        cube = data[cubeIndex];
                        if (cube.Intersection(h1))
                        {
                            p11 = cube.GetP1();
                            p12 = cube.GetP2();
                            int mapinfoId = ((Leaf)node).GetDataPointer(index);
                            int mapInfoId = mapinfoId;
                            GeoLatLngBounds mbr = new GeoLatLngBounds();
                            mbr.X = p11.GetFloatCoordinate(0) / DoublePrecision;
                            mbr.Y = p11.GetFloatCoordinate(1) / DoublePrecision;
                            mbr.Width = ((p12.GetFloatCoordinate(0) - p11.GetFloatCoordinate(0))) / DoublePrecision;
                            mbr.Height = ((p12.GetFloatCoordinate(1) - p11.GetFloatCoordinate(1))) / DoublePrecision;
                            if (!retArrayList.Contains(mapInfoId))
                            {
                                retArrayList.Add(mapInfoId, mbr);
                            }

                        }
                        index++;

                    }
                }
            }
            return retArrayList;
        }
        /**
         * reader to read the data from the map file.
         */
        private readonly BinaryReader _reader;

        /**
         *  record index section.
         */
        private RecordIndex _recordIndex;
        /**
         * rtree index section.
         */
        private RTreeIndex _rtreeIndex;
        /**
         * string index section.
         */
        private StringIndex _stringIndex;
        /**
         * string data section.
         */
        private StringData _stringData;
        /**
         * geo data section.
         */
        private GeoData _geoData;

        /**
         * when store latitude/longitude , it store as integer.
         * to Convert to an interget ,it muliple by DOUBLE_PRECISION.
         */
        private const double DoublePrecision = 10000000.0;
        /**
         * Rtree index file.
         */
        private RTree.RTree _tree;


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Binaries the search.
        /// </summary>
        /// <param name="queryValue">The query value.</param>
        /// <returns>the string recrod ID.</returns>
        private int BinarySearch(string queryValue)
        {
            int left = 0;
            int right = (int)(_stringIndex._size / StringIndex.RECORDSIZE) - 1;
            while (left <= right)
            {
                int middle = (int)Math.Floor((left + right) / 2.0);
                {
                    _stringIndex.GetRecord(middle);
                    string middleValue = _stringData.GetRecord(_stringIndex.RecordOffset);
                    _stringIndex.GetRecord(left);
                    //string leftValue = _stringData.GetRecord(_stringIndex.RecordOffset);
                    _stringIndex.GetRecord(right);
                    //string rightValue = _stringData.GetRecord(_stringIndex.RecordOffset);
                    //if (leftValue.Length > queryValue.Length)
                    //{
                    //    leftValue = leftValue.Substring(0, queryValue.Length);
                    //}
                    if (middleValue.Length > queryValue.Length)
                    {
                        middleValue = middleValue.Substring(0, queryValue.Length);
                    }
                    //if (rightValue.Length > queryValue.Length)
                    //{
                    //    rightValue = rightValue.Substring(0, queryValue.Length);
                    //}

                    if (queryValue.CompareTo(middleValue) == 0)
                    {
                        return middle;
                    }
                    if (queryValue.CompareTo(middleValue) > 0)
                    {
                        left = middle + 1;
                    }
                    else
                    {
                        right = middle - 1;
                    }
                }

            }
            return -1;
        }

    }

}
