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
using System.IO;
using Mapdigit.Util;
using Mapdigit.Gis.Geometry;

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
    /// geo data section of the map file.
    /// </summary>
    internal class GeoData : Section
    {
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoData"/> class.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// constructor.
        public GeoData(BinaryReader reader, long offset, long size)
            : base(reader, offset, size)
        {

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  get a map object at given index.
        /// </summary>
        /// <param name="recordIndex">Index of the record.</param>
        /// <returns></returns>
        public MapObject GetMapObject(RecordIndex recordIndex)
        {
            DataReader.Seek(reader, recordIndex.RecordOffset);
            MapObject mapObject = null;
            switch (recordIndex.MapObjectType)
            {
                case MapObject.TypeNone:
                    mapObject = new MapNoneObject();
                    break;
                case MapObject.TypePoint:
                    mapObject = GetMapPoint(recordIndex);
                    break;
                case MapObject.TypeMultiPoint:
                    mapObject = GetMapMultiPoint(recordIndex);
                    break;
                case MapObject.TypePline:
                    mapObject = GetMapPline(recordIndex);
                    break;
                case MapObject.TypeMultiPline:
                    mapObject = GetMapMultiPline(recordIndex);
                    break;
                case MapObject.TypeReginRegion:
                    mapObject = GetMapRegion(recordIndex);
                    break;
                case MapObject.TypeMultiRegion:
                    mapObject = GetMapMultiRegion(recordIndex);
                    break;
                case MapObject.TypeCollection:
                    mapObject = GetMapCollection(recordIndex);
                    break;
                case MapObject.TypeText:
                    mapObject = GetMapText(recordIndex);
                    break;

            }
            return mapObject;
        }


        /**
         * when store latitude/longitude , it store as integer.
         * to Convert to an interget ,it muliple by DOUBLE_PRECISION.
         */
        private const double DoublePrecision = 10000000.0;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// get a map point object at given index.
        /// </summary>
        /// <param name="recordIndex">Index of the record.</param>
        /// <returns></returns>
        private static MapPoint GetMapPoint(RecordIndex recordIndex)
        {
            MapPoint mapPoint = new MapPoint
            {
                SymbolType =
                {
                    Shape = recordIndex.Param1,
                    Color = recordIndex.Param2,
                    Size = recordIndex.Param3
                },
                Point =
                {
                    X = recordIndex.MinX / DoublePrecision,
                    Y = recordIndex.MinY / DoublePrecision
                }
            };
            mapPoint.Bounds.X = mapPoint.Point.X;
            mapPoint.Bounds.Y = mapPoint.Point.Y;
            mapPoint.Bounds.Width = 0;
            mapPoint.Bounds.Height = 0;
            return mapPoint;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// get a map multipoint object at given index.
        /// </summary>
        /// <param name="recordIndex">Index of the record.</param>
        /// <returns></returns>
        private MapMultiPoint GetMapMultiPoint(RecordIndex recordIndex)
        {
            MapMultiPoint mapMultiPoint = new MapMultiPoint
            {
                SymbolType =
                {
                    Shape = recordIndex.Param1,
                    Color = recordIndex.Param2,
                    Size = recordIndex.Param3
                },
                Bounds =
                {
                    X = recordIndex.MinX / DoublePrecision,
                    Y = recordIndex.MinY / DoublePrecision,
                    Width = (recordIndex.MaxX - recordIndex.MinX) / DoublePrecision,
                    Height =
                        (recordIndex.MaxY - recordIndex.MinY) / DoublePrecision
                }
            };
            int numberOfPoints = DataReader.ReadInt(reader);
            mapMultiPoint.Points = new GeoLatLng[numberOfPoints];
            for (int i = 0; i < numberOfPoints; i++)
            {
                int x = DataReader.ReadInt(reader);
                int y = DataReader.ReadInt(reader);
                mapMultiPoint.Points[i] = new GeoLatLng(y / DoublePrecision,
                        x / DoublePrecision);
            }
            return mapMultiPoint;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// get a map pline object at given index.
        /// </summary>
        /// <param name="recordIndex">Index of the record.</param>
        /// <returns></returns>
        private MapPline GetMapPline(RecordIndex recordIndex)
        {
            MapPline mapPline = new MapPline
            {
                PenStyle =
                {
                    Pattern = recordIndex.Param1,
                    Width = recordIndex.Param2,
                    Color = recordIndex.Param3
                },
                Bounds =
                {
                    X = recordIndex.MinX / DoublePrecision,
                    Y = recordIndex.MinY / DoublePrecision,
                    Width = (recordIndex.MaxX - recordIndex.MinX) / DoublePrecision,
                    Height = (recordIndex.MaxY - recordIndex.MinY) / DoublePrecision
                }
            };
            int numberOfPoints = DataReader.ReadInt(reader);
            GeoLatLng[] latLngs = new GeoLatLng[numberOfPoints];
            for (int i = 0; i < numberOfPoints; i++)
            {
                int x = DataReader.ReadInt(reader);
                int y = DataReader.ReadInt(reader);
                latLngs[i] = new GeoLatLng(y / DoublePrecision,
                        x / DoublePrecision);
            }
            mapPline.Pline = new GeoPolyline(latLngs, mapPline.PenStyle.Color,
                    mapPline.PenStyle.Width, mapPline.PenStyle.Pattern);
            return mapPline;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// get a map multipline object at given index.
        /// </summary>
        /// <param name="recordIndex">Index of the record.</param>
        /// <returns></returns>
        private MapMultiPline GetMapMultiPline(RecordIndex recordIndex)
        {
            MapMultiPline mapMultiPline = new MapMultiPline
            {
                PenStyle =
                {
                    Pattern = recordIndex.Param1,
                    Width = recordIndex.Param2,
                    Color = recordIndex.Param3
                },
                Bounds =
                {
                    X = recordIndex.MinX / DoublePrecision,
                    Y = recordIndex.MinY / DoublePrecision,
                    Width = (recordIndex.MaxX - recordIndex.MinX) / DoublePrecision,
                    Height =
                        (recordIndex.MaxY - recordIndex.MinY) / DoublePrecision
                }
            };
            int numberOfPart = DataReader.ReadInt(reader);
            mapMultiPline.Plines = new GeoPolyline[numberOfPart];
            for (int j = 0; j < numberOfPart; j++)
            {

                int numberOfPoints = DataReader.ReadInt(reader);
                GeoLatLng[] latLngs = new GeoLatLng[numberOfPoints];
                for (int i = 0; i < numberOfPoints; i++)
                {
                    int x = DataReader.ReadInt(reader);
                    int y = DataReader.ReadInt(reader);
                    latLngs[i] = new GeoLatLng(y / DoublePrecision,
                            x / DoublePrecision);
                }
                mapMultiPline.Plines[j] = new GeoPolyline(latLngs,
                        mapMultiPline.PenStyle.Color,
                        mapMultiPline.PenStyle.Width,
                        mapMultiPline.PenStyle.Pattern);
            }
            return mapMultiPline;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// get a map region object at given index.
        /// </summary>
        /// <param name="recordIndex">Index of the record.</param>
        /// <returns></returns>
        private MapRegion GetMapRegion(RecordIndex recordIndex)
        {
            MapRegion mapRegion = new MapRegion
            {
                BrushStyle =
                {
                    Pattern = recordIndex.Param1,
                    ForeColor = recordIndex.Param2,
                    BackColor = recordIndex.Param3
                }
            };
            mapRegion.PenStyle.Pattern = DataReader.ReadInt(reader);
            mapRegion.PenStyle.Width = DataReader.ReadInt(reader);
            mapRegion.PenStyle.Color = DataReader.ReadInt(reader);
            mapRegion.Bounds.X = recordIndex.MinX / DoublePrecision;
            mapRegion.Bounds.Y = recordIndex.MinY / DoublePrecision;
            mapRegion.Bounds.Width =
                    (recordIndex.MaxX - recordIndex.MinX) / DoublePrecision;
            mapRegion.Bounds.Height =
                    (recordIndex.MaxY - recordIndex.MinY) / DoublePrecision;
            int centerX = DataReader.ReadInt(reader);
            int centerY = DataReader.ReadInt(reader);
            mapRegion.CenterPt.X = centerX / DoublePrecision;
            mapRegion.CenterPt.Y = centerY / DoublePrecision;
            int numberOfPoints = DataReader.ReadInt(reader);
            GeoLatLng[] latLngs = new GeoLatLng[numberOfPoints];
            for (int i = 0; i < numberOfPoints; i++)
            {
                int x = DataReader.ReadInt(reader);
                int y = DataReader.ReadInt(reader);
                latLngs[i] = new GeoLatLng(y / DoublePrecision,
                        x / DoublePrecision);
            }
            mapRegion.Region = new GeoPolygon(latLngs, mapRegion.PenStyle.Color,
                    mapRegion.PenStyle.Width, mapRegion.PenStyle.Pattern,
                    mapRegion.BrushStyle.ForeColor, mapRegion.BrushStyle.Pattern);
            return mapRegion;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// get a map multiregion object at given index.
        /// </summary>
        /// <param name="recordIndex">Index of the record.</param>
        /// <returns></returns>
        private MapMultiRegion GetMapMultiRegion(RecordIndex recordIndex)
        {
            MapMultiRegion mapMultiRegion = new MapMultiRegion();
            mapMultiRegion.BrushStyle.Pattern = recordIndex.Param1;
            mapMultiRegion.BrushStyle.ForeColor = recordIndex.Param2;
            mapMultiRegion.BrushStyle.BackColor = recordIndex.Param3;
            mapMultiRegion.PenStyle.Pattern = DataReader.ReadInt(reader);
            mapMultiRegion.PenStyle.Width = DataReader.ReadInt(reader);
            mapMultiRegion.PenStyle.Color = DataReader.ReadInt(reader);
            mapMultiRegion.Bounds.X = recordIndex.MinX / DoublePrecision;
            mapMultiRegion.Bounds.Y = recordIndex.MinY / DoublePrecision;
            mapMultiRegion.Bounds.Width =
                    (recordIndex.MaxX - recordIndex.MinX) / DoublePrecision;
            mapMultiRegion.Bounds.Height =
                    (recordIndex.MaxY - recordIndex.MinY) / DoublePrecision;
            int centerX = DataReader.ReadInt(reader);
            int centerY = DataReader.ReadInt(reader);
            mapMultiRegion.CenterPt.X = centerX / DoublePrecision;
            mapMultiRegion.CenterPt.Y = centerY / DoublePrecision;
            int numberOfPart = DataReader.ReadInt(reader);
            mapMultiRegion.Regions = new GeoPolygon[numberOfPart];
            for (int j = 0; j < numberOfPart; j++)
            {
                int numberOfPoints = DataReader.ReadInt(reader);
                GeoLatLng[] latLngs = new GeoLatLng[numberOfPoints];
                for (int i = 0; i < numberOfPoints; i++)
                {
                    int x = DataReader.ReadInt(reader);
                    int y = DataReader.ReadInt(reader);
                    latLngs[i] = new GeoLatLng(y / DoublePrecision,
                            x / DoublePrecision);
                }
                mapMultiRegion.Regions[j] = new GeoPolygon(latLngs,
                        mapMultiRegion.PenStyle.Color,
                        mapMultiRegion.PenStyle.Width,
                        mapMultiRegion.PenStyle.Pattern,
                        mapMultiRegion.BrushStyle.ForeColor,
                        mapMultiRegion.BrushStyle.Pattern);
            }
            return mapMultiRegion;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// get a map collection object at given index.
        /// </summary>
        /// <param name="recordIndex">Index of the record.</param>
        /// <returns></returns>
        private MapCollection GetMapCollection(RecordIndex recordIndex)
        {
            MapCollection mapCollection = new MapCollection();
            int regionPart = recordIndex.Param1;
            int plinePart = recordIndex.Param2;
            int multiPointPart = recordIndex.Param3;
            mapCollection.Bounds.X = recordIndex.MinX / DoublePrecision;
            mapCollection.Bounds.Y = recordIndex.MinY / DoublePrecision;
            mapCollection.Bounds.Width =
                    (recordIndex.MaxX - recordIndex.MinX) / DoublePrecision;
            mapCollection.Bounds.Height =
                    (recordIndex.MaxY - recordIndex.MinY) / DoublePrecision;
            if (regionPart > 0)
            {
                mapCollection.MultiRegion = new MapMultiRegion();
                mapCollection.MultiRegion.BrushStyle.Pattern = DataReader.ReadInt(reader);
                mapCollection.MultiRegion.BrushStyle.ForeColor = DataReader.ReadInt(reader);
                mapCollection.MultiRegion.BrushStyle.BackColor = DataReader.ReadInt(reader);
                mapCollection.MultiRegion.PenStyle.Pattern = DataReader.ReadInt(reader);
                mapCollection.MultiRegion.PenStyle.Width = DataReader.ReadInt(reader);
                mapCollection.MultiRegion.PenStyle.Color = DataReader.ReadInt(reader);
                int centerX = DataReader.ReadInt(reader);
                int centerY = DataReader.ReadInt(reader);
                mapCollection.MultiRegion.CenterPt.X = centerX / DoublePrecision;
                mapCollection.MultiRegion.CenterPt.Y = centerY / DoublePrecision;
                mapCollection.MultiRegion.Regions = new GeoPolygon[regionPart];
                for (int j = 0; j < regionPart; j++)
                {
                    int numberOfPoints = DataReader.ReadInt(reader);
                    GeoLatLng[] latLngs = new GeoLatLng[numberOfPoints];
                    for (int i = 0; i < numberOfPoints; i++)
                    {
                        int x = DataReader.ReadInt(reader);
                        int y = DataReader.ReadInt(reader);
                        latLngs[i] = new GeoLatLng(y / DoublePrecision,
                                x / DoublePrecision);
                    }
                    mapCollection.MultiRegion.Regions[j] = new GeoPolygon(latLngs,
                            0,
                            0,
                            0,
                            0,
                            0);
                }
            }
            if (plinePart > 0)
            {
                mapCollection.MultiPline = new MapMultiPline();
                mapCollection.MultiPline.PenStyle.Pattern = DataReader.ReadInt(reader);
                mapCollection.MultiPline.PenStyle.Width = DataReader.ReadInt(reader);
                mapCollection.MultiPline.PenStyle.Color = DataReader.ReadInt(reader);
                mapCollection.MultiPline.Plines = new GeoPolyline[plinePart];
                for (int j = 0; j < plinePart; j++)
                {

                    int numberOfPoints = DataReader.ReadInt(reader);
                    GeoLatLng[] latLngs = new GeoLatLng[numberOfPoints];
                    for (int i = 0; i < numberOfPoints; i++)
                    {
                        int x = DataReader.ReadInt(reader);
                        int y = DataReader.ReadInt(reader);
                        latLngs[i] = new GeoLatLng(y / DoublePrecision,
                                x / DoublePrecision);
                    }
                    mapCollection.MultiPline.Plines[j] = new GeoPolyline(latLngs,
                            0,
                            0,
                            0);
                }
            }
            if (multiPointPart > 0)
            {
                mapCollection.MultiPoint = new MapMultiPoint();
                mapCollection.MultiPoint.SymbolType.Shape = DataReader.ReadInt(reader);
                mapCollection.MultiPoint.SymbolType.Color = DataReader.ReadInt(reader);
                mapCollection.MultiPoint.SymbolType.Size = DataReader.ReadInt(reader);
                multiPointPart = DataReader.ReadInt(reader);
                mapCollection.MultiPoint.Points = new GeoLatLng[multiPointPart];
                for (int i = 0; i < multiPointPart; i++)
                {
                    int x = DataReader.ReadInt(reader);
                    int y = DataReader.ReadInt(reader);
                    mapCollection.MultiPoint.Points[i] =
                            new GeoLatLng(y / DoublePrecision,
                            x / DoublePrecision);
                }
            }
            return mapCollection;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 02OCT2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// get a map text object at given index.
        /// </summary>
        /// <param name="recordIndex">Index of the record.</param>
        /// <returns></returns>
        private MapText GetMapText(RecordIndex recordIndex)
        {
            MapText mapText = new MapText();
            mapText.Angle = recordIndex.Param1;
            mapText.ForeColor = recordIndex.Param2;
            mapText.BackColor = recordIndex.Param3;
            mapText.Bounds.X = recordIndex.MinX / DoublePrecision;
            mapText.Bounds.Y = recordIndex.MinY / DoublePrecision;
            mapText.Bounds.Width =
                    (recordIndex.MaxX - recordIndex.MinX) / DoublePrecision;
            mapText.Bounds.Height =
                    (recordIndex.MaxY - recordIndex.MinY) / DoublePrecision;
            mapText.Justification = DataReader.ReadInt(reader);
            mapText.Spacing = DataReader.ReadInt(reader);
            mapText.LineType = DataReader.ReadInt(reader);
            mapText.TextString = DataReader.ReadString(reader);
            return mapText;
        }
    }

}
