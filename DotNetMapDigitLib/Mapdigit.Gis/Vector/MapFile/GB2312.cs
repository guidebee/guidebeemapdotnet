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
using System.IO;
using Mapdigit.Util;

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
    /// GB2312 get Pinyin code for a chinese.
    /// </summary>
    internal sealed class Gb2312
    {

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="Gb2312"/> class.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public Gb2312(BinaryReader reader)
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
        /// binary search (Chinese).
        /// </summary>
        /// <param name="queryValue">The query value.</param>
        /// <returns>the string recrod ID</returns>
        public int BinarySearch(string queryValue)
        {
            int left = 0;
            int right = NumberOfChinese - 1;
            while (left <= right)
            {
                int middle = (int)Math.Floor((left + right) / 2.0);
                {
                    DataReader.Seek(_reader, middle * Recordsize);
                    string middleValuePinYin = DataReader.ReadString(_reader);
                    DataReader.Seek(_reader, middle * Recordsize + 8);
                    string middleValue = DataReader.ReadString(_reader);

                    DataReader.Seek(_reader, left * Recordsize);
                    DataReader.ReadString(_reader);
                    DataReader.Seek(_reader, left * Recordsize + 8);
                    //string leftValue = DataReader.ReadString(_reader);

                    DataReader.Seek(_reader, right * Recordsize);
                    DataReader.ReadString(_reader);
                    DataReader.Seek(_reader, right * Recordsize + 8);
                    //string rightValue = DataReader.ReadString(_reader);

                    //if (leftValue.Length > queryValue.Length)
                    //    leftValue = leftValue.Substring(0, queryValue.Length);
                    if (middleValue.Length > queryValue.Length)
                        middleValue = middleValue.Substring(0, queryValue.Length);
                    //if (rightValue.Length > queryValue.Length)
                    //    rightValue = rightValue.Substring(0, queryValue.Length);

                    if (queryValue.CompareTo(middleValue) == 0)
                    {
                        _firstLetter = middleValuePinYin.Substring(0, 1);
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
            _firstLetter = queryValue.Substring(0, 1);
            return -1;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// binary search (Chinese).
        /// </summary>
        /// <param name="queryValue">The query value.</param>
        /// <param name="reader">The reader.</param>
        /// <returns>the string recrod ID</returns>
        public static int BinarySearch(string queryValue, BinaryReader reader)
        {
            int left = 0;
            int right = NumberOfChinese - 1;
            while (left <= right)
            {
                int middle = (int)Math.Floor((left + right) / 2.0);
                {
                    DataReader.Seek(reader, middle * Recordsize);
                    string middleValue = DataReader.ReadString(reader);
                    DataReader.Seek(reader, middle * Recordsize + 8);
                    string middleValuePinYin = DataReader.ReadString(reader);

                    DataReader.Seek(reader, left * Recordsize);
                    //string leftValue = DataReader.ReadString(reader);
                    DataReader.Seek(reader, left * Recordsize + 8);
                    DataReader.ReadString(reader);

                    DataReader.Seek(reader, right * Recordsize);
                    //string rightValue = DataReader.ReadString(reader);
                    DataReader.Seek(reader, right * Recordsize + 8);
                    DataReader.ReadString(reader);

                    //if (leftValue.Length > queryValue.Length)
                    //    leftValue = leftValue.Substring(0, queryValue.Length);
                    if (middleValue.Length > queryValue.Length)
                        middleValue = middleValue.Substring(0, queryValue.Length);
                    //if (rightValue.Length > queryValue.Length)
                    //    rightValue = rightValue.Substring(0, queryValue.Length);

                    if (queryValue.CompareTo(middleValue) == 0)
                    {
                        _staticFirstLetter = middleValuePinYin;
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
            _staticFirstLetter = queryValue;
            return -1;
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get pinyin code for given chinese string. the pinyin code is consists with
        /// first letter of pinyin for each Chinese character.
        /// </summary>
        /// <param name="chinese">The chinese.</param>
        /// <returns>pinyin code</returns>
        public string GetPinyinCode(string chinese)
        {
            string ret = "";
            try
            {
                for (int i = 0; i < chinese.Length; i++)
                {
                    string keyValue = chinese.Substring(i, i + 1);
                    BinarySearch(keyValue);
                    ret += _firstLetter;
                }

            }
            catch (IOException)
            {
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
        /// Get pinyin code for given chinese string. the pinyin code is consists with
        /// first letter of pinyin for each Chinese character.
        /// </summary>
        /// <param name="chinese">The chinese.</param>
        /// <param name="reader">The reader.</param>
        /// <returns>pinyin code</returns>
        public static string GetPinyinCode(string chinese, BinaryReader reader)
        {
            string ret = "";
            try
            {
                for (int i = 0; i < chinese.Length; i++)
                {
                    string keyValue = chinese.Substring(i, i + 1);
                    BinarySearch(keyValue, reader);
                    ret += _staticFirstLetter;
                }

            }
            catch (IOException)
            {
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
        /// * Get pinyin code for given chinese string. the pinyin code is consists with
        /// first letter of pinyin for each Chinese character.
        /// </summary>
        /// <param name="chinese">The chinese.</param>
        /// <param name="reader">The reader.</param>
        /// <returns>pinyin code</returns>
        public static ArrayList[] GetPinyinCodes(string chinese, BinaryReader reader)
        {
            int strLen = chinese.Length;
            ArrayList[] pinyinCodes = new ArrayList[strLen];
            try
            {
                for (int i = 0; i < strLen; i++)
                {
                    pinyinCodes[i] = new ArrayList();
                    string keyValue = chinese.Substring(i, i + 1);
                    int chineseId = BinarySearch(keyValue, reader);
                    if (chineseId != -1)
                    {
                        int checkId = chineseId;
                        string pinyin = GetPinYinAtPosition(checkId, keyValue, reader);
                        while (pinyin != null)
                        {
                            pinyinCodes[i].Add(pinyin);
                            checkId++;
                            pinyin = GetPinYinAtPosition(checkId, keyValue, reader);
                        }
                        checkId = chineseId - 1;
                        pinyin = GetPinYinAtPosition(checkId, keyValue, reader);
                        while (pinyin != null)
                        {
                            pinyinCodes[i].Add(pinyin);
                            checkId--;
                            pinyin = GetPinYinAtPosition(checkId, keyValue, reader);
                        }

                    }
                    else
                    {
                        pinyinCodes[i].Add(keyValue);
                    }
                }

            }
            catch (IOException)
            {
            }

            return pinyinCodes;
        }

        /**
         * the data input reader for the map file.
         */
        private readonly BinaryReader _reader;

        /**
         * how many chinese character.
         */
        private const int NumberOfChinese = 27954;

        /**
         * the size of each record.
         */
        private const int Recordsize = 16;

        /**
         * the first Pinyin letter;
         */
        private string _firstLetter;


        /**
         * the first Pinyin letter;
         */
        private static string _staticFirstLetter;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the pin yin at position.
        /// </summary>
        /// <param name="chineseId">The chinese id.</param>
        /// <param name="queryValue">The query value.</param>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        private static string GetPinYinAtPosition(int chineseId, string queryValue,
                BinaryReader reader)
        {
            DataReader.Seek(reader, chineseId * Recordsize);
            string retValue = DataReader.ReadString(reader);
            DataReader.Seek(reader, chineseId * Recordsize + 8);
            string retValuePinYin = DataReader.ReadString(reader);
            if (retValue.CompareTo(queryValue) == 0)
            {
                return retValuePinYin;
            }
            return null;
        }

    }

}
