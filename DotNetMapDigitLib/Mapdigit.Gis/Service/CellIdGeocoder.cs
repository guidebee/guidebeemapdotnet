//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 27SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System;
using Mapdigit.Ajax;
using Mapdigit.Network;
using Mapdigit.Util;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit.Gis.Service
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 27SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// This class is used to communicate directly with Google servers to obtain
    /// geocodes for user specified addresses. In addition, a geocoder maintains
    /// its own cache of addresses, which allows repeated queries to be answered
    /// without a round trip to the server.
    /// </summary>
    public class CellIdGeocoder : ICellIdGeocoder
    {
        
        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="CellIdGeocoder"/> class.
        /// </summary>
        public CellIdGeocoder()
        {
            _addressQuery = new CellIdAddressQuery(this);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the locations.
        /// </summary>
        /// <param name="stringMmc">The string MMC.</param>
        /// <param name="stringMnc">The string MNC.</param>
        /// <param name="stringLac">The string lac.</param>
        /// <param name="stringCid">The string cid.</param>
        /// <param name="listener">The listener.</param>
        public void GetLocations(string stringMmc, string stringMnc, string stringLac, string stringCid,
                IGeocodingListener listener)
        {
            try
            {
                _listener = listener;
                long mcc = long.Parse(stringMmc);
                long mnc = long.Parse(stringMnc);
                long lac = long.Parse(stringLac);
                long cid = long.Parse(stringCid);
                byte[] pd = PostData(mcc, mnc, lac, cid,
                        false);
                Arg[] httpArgs = new Arg[1];
                httpArgs[0] = new Arg("ContentType", "application/binary");
                Part part = new Part(pd, null);
                PostData postData = new PostData(new[] { part }, "");
                Request.Post(SearchBase, null, httpArgs, _addressQuery, postData, this);
                _searchCellInfo = stringMmc + "-" + stringMnc + "-" + stringLac + "-" + stringCid;
            }
            catch (Exception)
            {
                if (listener != null)
                {
                    listener.Done(_searchCellInfo, null);
                }
            }

        }

        private const string SearchBase = "http://www.google.com/glm/mmap";
        IGeocodingListener _listener;
        readonly CellIdAddressQuery _addressQuery;
        string _searchCellInfo;


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Posts the data.
        /// </summary>
        /// <param name="mcc">The MCC.</param>
        /// <param name="mnc">The MNC.</param>
        /// <param name="lac">The lac.</param>
        /// <param name="cid">The cid.</param>
        /// <param name="shortCid">if set to <c>true</c> [short cid].</param>
        /// <returns></returns>
        private static byte[] PostData(long mcc, long mnc, long lac, long cid,
                bool shortCid)
        {
            /* The shortCID parameter follows heuristic experiences:
             * Sometimes UMTS CIDs are build up from the original GSM CID (lower 4 hex digits)
             * and the RNC-ID left shifted into the upper 4 digits.
             */
            byte[] pd = new byte[]{
            0x00, 0x0e,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00,
            0x00, 0x00,
            0x00, 0x00,
            0x1b,
            0x00, 0x00, 0x00, 0x00, // Offset 0x11
            0x00, 0x00, 0x00, 0x00, // Offset 0x15
            0x00, 0x00, 0x00, 0x00, // Offset 0x19
            0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, // Offset 0x1f
            0x00, 0x00, 0x00, 0x00, // Offset 0x23
            0x00, 0x00, 0x00, 0x00, // Offset 0x27
            0x00, 0x00, 0x00, 0x00, // Offset 0x2b
            0xff, 0xff, 0xff, 0xff,
            0x00, 0x00, 0x00, 0x00
        };

            if (shortCid)
            {
                cid &= 0xFFFF;      /* Attempt to resolve the cell using the
                                    GSM CID part */
            }

            if (cid > 65536) /* GSM: 4 hex digits, UTMS: 6 hex     digits */
            {
                pd[0x1c] = 5;
            }
            else
            {
                pd[0x1c] = 3;
            }

            pd[0x11] = (byte)((mnc >> 24) & 0xFF);
            pd[0x12] = (byte)((mnc >> 16) & 0xFF);
            pd[0x13] = (byte)((mnc >> 8) & 0xFF);
            pd[0x14] = (byte)((mnc >> 0) & 0xFF);

            pd[0x15] = (byte)((mcc >> 24) & 0xFF);
            pd[0x16] = (byte)((mcc >> 16) & 0xFF);
            pd[0x17] = (byte)((mcc >> 8) & 0xFF);
            pd[0x18] = (byte)((mcc >> 0) & 0xFF);

            pd[0x27] = (byte)((mnc >> 24) & 0xFF);
            pd[0x28] = (byte)((mnc >> 16) & 0xFF);
            pd[0x29] = (byte)((mnc >> 8) & 0xFF);
            pd[0x2a] = (byte)((mnc >> 0) & 0xFF);

            pd[0x2b] = (byte)((mcc >> 24) & 0xFF);
            pd[0x2c] = (byte)((mcc >> 16) & 0xFF);
            pd[0x2d] = (byte)((mcc >> 8) & 0xFF);
            pd[0x2e] = (byte)((mcc >> 0) & 0xFF);

            pd[0x1f] = (byte)((cid >> 24) & 0xFF);
            pd[0x20] = (byte)((cid >> 16) & 0xFF);
            pd[0x21] = (byte)((cid >> 8) & 0xFF);
            pd[0x22] = (byte)((cid >> 0) & 0xFF);

            pd[0x23] = (byte)((lac >> 24) & 0xFF);
            pd[0x24] = (byte)((lac >> 16) & 0xFF);
            pd[0x25] = (byte)((lac >> 8) & 0xFF);
            pd[0x26] = (byte)((lac >> 0) & 0xFF);

            return pd;
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Searches the response.
        /// </summary>
        /// <param name="geoCoder">The geo coder.</param>
        /// <param name="response">The response.</param>
        private void SearchResponse(CellIdGeocoder geoCoder, Response response)
        {
            if (response.GetCode() != HttpConnection.HttpOk)
            {
                Log.P("Error connecting to search service", Log.Error);
                if (geoCoder._listener != null)
                {
                    geoCoder._listener.Done(_searchCellInfo, null);
                }
                return;
            } try
            {
                byte[] ps = response.GetRawContentArray();
                int retCode = (ps[3] << 24) | (ps[4] << 16) |
                               (ps[5] << 8) | (ps[6]);
                if (retCode == 0)
                {
                    int[] values = new int[4];
                    for (int i = 0; i < 4; i++)
                    {
                        if (ps[7 + i] < 0)
                        {
                            values[i] = ps[7 + i] + 256;
                        }
                        else
                        {
                            values[i] = ps[7 + i];
                        }
                    }
                    long latLng = (values[0] << 24) | (values[1] << 16)
                                 | (values[2] << 8) | (values[3]);

                    double lat = latLng / 1000000.0;

                    for (int i = 0; i < 4; i++)
                    {
                        if (ps[11 + i] < 0)
                        {
                            values[i] = ps[11 + i] + 256;
                        }
                        else
                        {
                            values[i] = ps[11 + i];
                        }
                    }
                    latLng = (values[0] << 24) | (values[1] << 16)
                                 | (values[2] << 8) | (values[3]);

                    double lon = latLng / 1000000.0;
                    MapPoint mapPoint = new MapPoint();
                    mapPoint.Point.X = lon;
                    mapPoint.Point.Y = lat;
                    if (_listener != null)
                    {
                        _listener.Done(_searchCellInfo, new[] { mapPoint });
                    }

                }
                else
                {
                    if (_listener != null)
                    {
                        _listener.Done(_searchCellInfo, null);
                    }
                }

            }
            catch (Exception)
            {
                if (_listener != null)
                {
                    _listener.Done(_searchCellInfo, null);
                }
            }
        }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 27SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        class CellIdAddressQuery : IRequestListener
        {
            private readonly CellIdGeocoder _cellIdGeocoder;

            ////////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS ------------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ----------------------
            // 27SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Initializes a new instance of the <see cref="CellIdAddressQuery"/> class.
            /// </summary>
            /// <param name="cellIdGeocoder">The cell id geocoder.</param>
            public CellIdAddressQuery(CellIdGeocoder cellIdGeocoder)
            {
                _cellIdGeocoder = cellIdGeocoder;
            }

            ////////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS ------------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ----------------------
            // 27SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Read progress notification.
            /// </summary>
            /// <param name="context">message context, can be any object.</param>
            /// <param name="bytes">the number of bytes has been read.</param>
            /// <param name="total">total bytes to be read.Total will be zero if not available</param>
            public void ReadProgress(object context, int bytes, int total)
            {
                CellIdGeocoder geoCoder = (CellIdGeocoder)context;
                geoCoder._listener.ReadProgress(bytes, total);
            }

            ////////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS ------------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ----------------------
            // 27SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Write progress notification.
            /// </summary>
            /// <param name="context">message context, can be any object.</param>
            /// <param name="bytes">the number of bytes has been written.</param>
            /// <param name="total">total bytes to be written.Total will be zero if not available .</param>
            public void WriteProgress(object context, int bytes, int total)
            {
            }

            ////////////////////////////////////////////////////////////////////////////
            //--------------------------------- REVISIONS ------------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ----------------------
            // 27SEP2010  James Shen                 	          Code review
            ////////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Dones the specified context.
            /// </summary>
            /// <param name="context">The context.</param>
            /// <param name="response">The response.</param>
            public void Done(object context, Response response)
            {
                _cellIdGeocoder.SearchResponse((CellIdGeocoder)context, response);
            }
        }

    }
}
