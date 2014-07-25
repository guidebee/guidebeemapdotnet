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
    /// Base class for all map tile downloader/reader/render.
    /// </summary>
    public abstract class MapTileAbstractReader
    {

        /// <summary>
        /// This image array stores map tiles downloaded
        /// </summary>
        public volatile byte[] ImageArray;

        /// <summary>
        /// the actual image size in the image array
        /// </summary>
        public volatile int ImageArraySize;

        /// <summary>
        /// indicates the data in the image array is valid or not.
        /// </summary>
        public volatile bool IsImagevalid;

        /// <summary>
        /// total bytes downloaded with this downloader
        /// </summary>
        public static long TotaldownloadedBytes;


        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the map downloading listener.
        /// </summary>
        /// <param name="listener">The listener.</param>
        public virtual void SetMapDownloadingListener(IReaderListener listener)
        {
            readListener = listener;
  
        }

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
        /// <param name="mtype"> the map type of the map tile.</param>
        /// <param name="x"> the x index of the map tile.</param>
        /// <param name="y">the y index of the map tile.</param>
        /// <param name="zoomLevel">the zoom level of the map tile.</param>
        public abstract void GetImage(int mtype, int x, int y, int zoomLevel);

        ////////////////////////////////////////////////////////////////////////////
        //--------------------------------- REVISIONS ------------------------------
        // Date       Name                 Tracking #         Description
        // ---------  -------------------  -------------      ----------------------
        // 28SEP2010  James Shen                 	          Code review
        ////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// a way app can cancel the reading process.
        /// </summary>
        public virtual void CancelRead()
        {
        }

        /// <summary>
        /// downloader listener
        /// </summary>
        protected IReaderListener readListener;

        internal void SetReadListener(IReaderListener readListener)
        {
            this.readListener = readListener;
        }


    }

}
