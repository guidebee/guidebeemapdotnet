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
using System;
using System.IO;
using Mapdigit.Ajax;
using Mapdigit.Gis.Geometry;
using Mapdigit.Network;
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
    /// MapTileDownloader download map image tiles from server (msn,yahoo,etc).
    /// </summary>
    public class MapTileDownloader : MapTileAbstractReader
    {
        ////private readonly WebClient _webClient = new WebClient();

        //////////////////////////////////////////////////////////////////////////////
        ////--------------------------------- REVISIONS ------------------------------
        //// Date       Name                 Tracking #         Description
        //// ---------  -------------------  -------------      ----------------------
        //// 30SEP2010  James Shen                 	          Code review
        //////////////////////////////////////////////////////////////////////////////
        ///// <summary>
        ///// Get image at given location. when the reader is done, imageArray shall
        ///// store the image byte array. imageArraySize is the actually data size.
        ///// isImagevalid indicate the data is valid or not. normally this shall be
        ///// an async call.
        ///// </summary>
        ///// <param name="mtype">the map type of the map tile.</param>
        ///// <param name="x">the x index of the map tile.</param>
        ///// <param name="y">the y index of the map tile.</param>
        ///// <param name="zoomLevel">the zoom level of the map tile.</param>
        //       public override void GetImage(int mtype, int x, int y, int zoomLevel)
        //       {
        //           IsImagevalid = true;
        //           ImageArraySize = 0;
        //           _webClient.Headers[HttpRequestHeader.CacheControl] = "No-Transform";
        //           _webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
        //           _webClient.Proxy = WebRequest.DefaultWebProxy;
        //           _webClient.Proxy.Credentials = CredentialCache.DefaultCredentials;
        //           _webClient.DownloadProgressChanged+=WebClient_DownloadProgressChanged;
        //           _webClient.DownloadDataCompleted+=WebClient_DownloadDataCompleted;
        //           try
        //           {
        //               string location = GetTileUrl(mtype, x, y, Numzoomlevels - zoomLevel);
        //               _webClient.DownloadDataAsync(new Uri(location));
        //               lock(_webClient)
        //               {
        //                   Monitor.Wait(_webClient, 30000);
        //               }
        //}
        //           catch (Exception)
        //           {
        //               IsImagevalid = false;
        //               ImageArray = null;
        //               GC.Collect();
        //           }

        //       }

        //       private void WebClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        //       {
        //           ImageArray=e.Result;
        //           ImageArraySize = ImageArray.Length;
        //           lock (_webClient)
        //           {
        //               Monitor.PulseAll(_webClient);
        //           }
        //       }

        //       private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        //       {
        //           if (ReadListener != null)
        //               ReadListener.ReadProgress((int)e.BytesReceived, (int)e.TotalBytesToReceive);
        //       }


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get image at given location. when the reader is done, imageArray shall
        /// store the image byte array. imageArraySize is the actually data size.
        /// isImagevalid indicate the data is valid or not. normally this shall be
        /// an async call.
        /// </summary>
        /// <param name="mtype">the map type of the map tile.</param>
        /// <param name="x">the x index of the map tile.</param>
        /// <param name="y">the y index of the map tile.</param>
        /// <param name="zoomLevel">the zoom level of the map tile.</param>
        public override void GetImage(int mtype, int x, int y, int zoomLevel)
        {
            mapType = mtype;
            mapXIndex = x;
            mapYIndex = y;
            mapZoomLevel = zoomLevel;
            int imgResponseCode = -1;
            IsImagevalid = true;
            ImageArraySize = 0;
            byte[] readBuffer = new byte[1024];
            try
            {
                string location = GetTileUrl(mtype, x, y, Numzoomlevels - zoomLevel);
                imgConn = Connector.Open(location);
                //imgConn.SetRequestProperty("CacheControl", "No-Transform");
                //imgConn.SetRequestProperty("UserAgent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                imgResponseCode = imgConn.GetResponseCode();
            }
            catch (Exception)
            {
                IsImagevalid = false;
            }

            if (imgResponseCode != HttpConnection.HttpOk)
            {
                Log.P("HTTP error downloading map image: "
                        + imgResponseCode, Log.Warning);

                IsImagevalid = false;
            }

            if (IsImagevalid)
            {
                try
                {
                    int totalToReceive
                        = imgConn.GetHeaderFieldInt(Arg.ContentLength, 0);
                       TotaldownloadedBytes += totalToReceive;
                        MapProgressInputStream input =
                            new MapProgressInputStream(imgConn.OpenInputStream(),
                                                       totalToReceive, readListener, 1024);
                        int totalToRead = totalToReceive;

                        ImageArraySize = totalToRead;
                        ImageArray = new byte[totalToRead];
                        int readLength = 0;
                        while (readLength < totalToRead)
                        {
                            int thisChunkLength = input.Read(readBuffer);
                            if (thisChunkLength > 0)
                            {
                                Array.Copy(readBuffer, 0, ImageArray,
                                           readLength, thisChunkLength);
                                readLength += thisChunkLength;
                            }
                            else
                            {
                                if (readLength == 0)
                                {
                                    IsImagevalid = false;
                                    ImageArray = null;
                                }
                                break;
                            }
                        }
                        input.Close();
                    
                    
                }
                catch (Exception)
                {
                    IsImagevalid = false;
                    ImageArray = null;
                    GC.Collect();
                }
            }
            try
            {
                imgConn.Close();
            }
            catch (Exception)
            {

            }
            imgConn = null;

        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// a way app can cancel the reading process.
        /// </summary>
        public override void CancelRead()
        {
            //if (_webClient != null)
            //{
            //    IsImagevalid = false;
            //    try
            //    {
            //        _webClient.CancelAsync();
            //    }
            //    catch (Exception)
            //    {

            //    }
            //}
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the URL of map tile image.
        /// </summary>
        /// <param name="mtype"> the map tile (msn,yahoo etc)</param>
        /// <param name="x">the x index of the map tile.</param>
        /// <param name="y">the y index of the map tile.</param>
        /// <param name="zoomLevel">current zoom level</param>
        /// <returns>the url of given map tile</returns>
        public string GetTileUrl(int mtype, int x, int y, int zoomLevel)
        {
            string url = MapType.GetTileUrl(mtype, x, y, zoomLevel);
            return url;
        }

        /// <summary>
        /// total zoom levels
        /// </summary>
        protected static int Numzoomlevels = 17;

        /// <summary>
        /// map type
        /// </summary>
        protected int mapType;

        /// <summary>
        ///  X index of the map tile
        /// </summary>
        protected int mapXIndex;

        /// <summary>
        /// Y index of the map tile
        /// </summary>
        protected int mapYIndex;

        /// <summary>
        /// zoom Level of the map tile.
        /// </summary>
        protected int mapZoomLevel;

        /// <summary>
        /// max wait time for download an image in seconds.
        /// </summary>
        protected int maxWaitingTime = 90;

        /// <summary>
        /// Http connection for donwloading images.
        /// </summary>
        protected HttpConnection imgConn;

        /// <summary>
        /// the map tile width.
        /// </summary>
        protected static int MapTileWidth = 256;

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Cast2s the integer.
        /// </summary>
        /// <param name="f">The f.</param>
        /// <returns></returns>
        protected static int Cast2Integer(double f)
        {
            if (f < 0)
            {
                return (int)MathEx.Ceil(f);
            }
            return (int)MathEx.Floor(f);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the index of map tiles based on given piexl coordinates
        /// </summary>
        /// <param name="x">x coordinates</param>
        /// <param name="y">y coordinates .</param>
        /// <returns>the the index of map tiles</returns>
        protected static GeoPoint GetMapIndex(double x, double y)
        {
            double longtiles = x / MapTileWidth;
            int tilex = Cast2Integer(longtiles);
            int tiley = Cast2Integer(y / MapTileWidth);
            return new GeoPoint(tilex, tiley);
        }

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        internal class MapProgressInputStream
        {

            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Initial Creation
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Initializes a new instance of the <see cref="MapProgressInputStream"/> class.
            /// </summary>
            /// <param name="stream">The stream.</param>
            /// <param name="total">The total.</param>
            /// <param name="listener">The listener.</param>
            /// <param name="notifyInterval">The notify interval.</param>
            public MapProgressInputStream(Stream stream, int total, IReaderListener listener,
                int notifyInterval)
            {
                _stream = stream;
                _total = total;
                _listener = listener;
                _notifyInterval = notifyInterval;
                _nread = 0;
            }

            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Initial Creation
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Reads the specified buffer.
            /// </summary>
            /// <param name="buffer">The buffer.</param>
            /// <returns></returns>
            public int Read(byte[] buffer)
            {
                return _stream.Read(buffer, 0, buffer.Length);
            }

            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Initial Creation
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Reads this instance.
            /// </summary>
            /// <returns></returns>
            public int Read()
            {
                if ((++_nread % _notifyInterval) == 0)
                {
                    try
                    {
                        if (_listener != null)
                            _listener.ReadProgress(_nread, _total);
                    }
                    catch (Exception)
                    {

                    }
                }

                return _stream.ReadByte();
            }

            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Initial Creation
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Closes this instance.
            /// </summary>
            public void Close()
            {
                _stream.Close();

            }

            //--------------------------------- REVISIONS --------------------------
            // Date       Name                 Tracking #         Description
            // ---------  -------------------  -------------      ------------------
            // 28SEP2010  James Shen                 	          Initial Creation
            ////////////////////////////////////////////////////////////////////////
            /// <summary>
            /// Avaiables this instance.
            /// </summary>
            /// <returns></returns>
            public int Avaiable()
            {
                try
                {
                    return (int)_stream.Length;
                }
                catch (Exception)
                {
                    return 0;
                }
            }

            private readonly Stream _stream;
            private readonly int _total;
            private readonly IReaderListener _listener;
            private readonly int _notifyInterval;
            private int _nread;

        }
    }

}
