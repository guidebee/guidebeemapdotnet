//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 28SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System.IO;
using Mapdigit.Gis.Geometry;
using Mapdigit.Util;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Gis.Raster
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 28SEP2010  James Shen                 	          Initial Creation
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Define a tiled map zone.
    /// </summary>
    public sealed class MapTiledZone
    {

        /// <summary>
        /// the boundary of this map zone.
        /// </summary>
        public GeoLatLngBounds Bounds;

        /// <summary>
        /// the map type of this map zone.
        /// </summary>
        public int MapType;


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="MapTiledZone"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public MapTiledZone(Stream stream)
        {
            _reader = new BinaryReader(stream);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Opens this instance.
        /// </summary>
        public void Open()
        {
            if (_opened)
            {
                return;
            }

            _opened = true;
            DataReader.Seek(_reader, 0);
            DataReader.ReadString(_reader);
            DataReader.Seek(_reader, 16);
            DataReader.ReadString(_reader);

            DataReader.Seek(_reader, 32);
            string mapTile = DataReader.ReadString(_reader);
            if (!mapTile.ToUpper().Equals("TILE"))
            {
                throw new IOException("Invalid map format!");
            }
            DataReader.Seek(_reader, 48);
            MapType = DataReader.ReadInt(_reader);
            int numOfLevel = DataReader.ReadInt(_reader);
            double minY = DataReader.ReadDouble(_reader);
            double minX = DataReader.ReadDouble(_reader);
            double maxY = DataReader.ReadDouble(_reader);
            double maxX = DataReader.ReadDouble(_reader);
            Bounds = new GeoLatLngBounds(minX, minY, maxX - minX, maxY - minY);
            DataReader.Seek(_reader, Headsize);
            _levelInfos = new LevelInfo[numOfLevel];
            for (int i = 0; i < numOfLevel; i++)
            {
                _levelInfos[i] = new LevelInfo
                                    {
                                        LevelNo = DataReader.ReadInt(_reader),
                                        MinX = DataReader.ReadInt(_reader),
                                        MinY = DataReader.ReadInt(_reader),
                                        MaxX = DataReader.ReadInt(_reader),
                                        MaxY = DataReader.ReadInt(_reader),
                                        Offset = DataReader.ReadInt(_reader),
                                        Length = DataReader.ReadInt(_reader)
                                    };
            }

        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the image .
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public byte[] GetImage(int level, int x, int y)
        {
            LevelInfo levelInfo = GetLevelInfo(level);
            byte[] buffer = null;
            if (levelInfo != null)
            {
                int rows = levelInfo.MaxY + 1 - levelInfo.MinY;
                if (x <= levelInfo.MaxX && x >= levelInfo.MinX &&
                        y <= levelInfo.MaxY && y >= levelInfo.MinY)
                {
                    int imageIndex = (x - levelInfo.MinX) * rows + y - levelInfo.MinY;
                    DataReader.Seek(_reader, levelInfo.Offset + imageIndex * 8);
                    int offset = DataReader.ReadInt(_reader);
                    int length = DataReader.ReadInt(_reader);
                    buffer = new byte[length];
                    DataReader.Seek(_reader, offset);

                    int howManyKs = length / 1024;
                    int remainBytes = length - howManyKs * 1024;
                    for (int i = 0; i < howManyKs; i++)
                    {
                        _reader.Read(buffer, i * 1024, 1024);
                        if (_readListener != null)
                        {
                            _readListener.ReadProgress(i * 1024, length);
                        }
                    }
                    if (remainBytes > 0)
                    {
                        _reader.Read(buffer, howManyKs * 1024, remainBytes);
                        if (_readListener != null)
                        {
                            _readListener.ReadProgress(length, length);
                        }

                    }
                }
            }
            return buffer;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            if (_reader != null)
            {
                _reader.Close();
            }
        }



        /**
         * reader to read the data from the map file.
         */
        private readonly BinaryReader _reader;


        /**
         * check if it's opened;
         */
        private bool _opened;

        /**
         * level info.
         */
        private LevelInfo[] _levelInfos;

        /**
         * the header size.
         */
        private const int Headsize = 256;


        /**
         * downloader listener
         */
        internal IReaderListener _readListener;




        private LevelInfo GetLevelInfo(int level)
        {
            for (int i = 0; i < _levelInfos.Length; i++)
            {
                if (_levelInfos[i].LevelNo == level)
                {
                    return _levelInfos[i];
                }
            }
            return null;
        }

        internal class LevelInfo
        {

            public int LevelNo;
            public int MinX;
            public int MinY;
            public int MaxX;
            public int MaxY;
            public int Offset;
            public int Length;

        }

    }


}
