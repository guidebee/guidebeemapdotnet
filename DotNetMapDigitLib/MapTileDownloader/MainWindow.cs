//------------------------------------------------------------------------------
//                         COPYRIGHT 2010 GUIDEBEE
//                           ALL RIGHTS RESERVED.
//                     GUIDEBEE CONFIDENTIAL PROPRIETARY 
///////////////////////////////////// REVISIONS ////////////////////////////////
// Date       Name                 Tracking #         Description
// ---------  -------------------  ----------         --------------------------
// 20SEP2010  James Shen                 	          Code review
////////////////////////////////////////////////////////////////////////////////
//--------------------------------- IMPORTS ------------------------------------
using System;
using System.Configuration;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Mapdigit.Ajax;
using Mapdigit.Drawing;
using Mapdigit.Gis;
using Mapdigit.Gis.Drawing;
using Mapdigit.Gis.Geometry;
using Mapdigit.Gis.Raster;
using Mapdigit.Gis.Service;

using Mapdigit.Util;

//--------------------------------- PACKAGE ------------------------------------
namespace Mapdigit
{
    //[-------------------------- MAIN CLASS ----------------------------------]
    ////////////////////////////////////////////////////////////////////////////
    //----------------------------- REVISIONS ----------------------------------
    // Date       Name                 Tracking #         Description
    // --------   -------------------  -------------      ----------------------
    // 20SEP2010  James Shen                 	          Code review
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Main Form.
    /// </summary>
    public partial class MainWindow : Form, IRequestListener, IMapDrawingListener,
        IReaderListener, IGeocodingListener, IRoutingListener
    {
        /// <summary>
        /// map download manager.
        /// </summary>
        private readonly MapTileDownloadManager _mapTileDownloadManager;
        /// <summary>
        /// raster map interface
        /// </summary>
        private readonly RasterMap _rasterMap;
        /// <summary>
        /// map image.
        /// </summary>
        private readonly IImage _mapImage;
        /// <summary>
        /// graphics for map image.
        /// </summary>
        private readonly IGraphics _mapGraphics;

        /// <summary>
        /// previous x
        /// </summary>
        private int _oldX;

        /// <summary>
        /// previous y
        /// </summary>
        private int _oldY;
        private delegate void UpdateInfo(string message);

        /// <summary>
        /// map type.
        /// </summary>
        private int _mapType = MapType.GenericMaptype7;

        /// <summary>
        /// Updates the status.
        /// </summary>
        /// <param name="messsage">The messsage.</param>
        private void UpdateStatus(string messsage)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateInfo(UpdateStatus), messsage);

            }
            else
            {
                lblStatus.Text = messsage;
                picMapCanvas.Invalidate();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            

            //optional, get the tile url from server.
            MapType.UpdateMapTileUrl();
            MapType.SetCustomMapTileUrl(new CustomMapType()); 

            MapLayer.SetAbstractGraphicsFactory(NETGraphicsFactory.GetInstance());
            MapConfiguration.SetParameter(MapConfiguration.WorkerThreadNumber, 1);
            _mapImage = MapLayer.GetAbstractGraphicsFactory().CreateImage(768, 768);
            _mapGraphics = _mapImage.GetGraphics();

            _mapTileDownloadManager = new MapTileDownloadManager(this);
            _rasterMap = new RasterMap(2048, 2048, _mapType, _mapTileDownloadManager);
            //_rasterMap.SetCurrentMapService(DigitalMapService.CloudmadeMapService);
            //DigitalMapService.GetSearchOptions().LanguageId = "zh-cn";
            _rasterMap.SetScreenSize(768, 768);
            _mapTileDownloadManager.Start();
            _rasterMap.SetMapDrawingListener(this);
            _rasterMap.SetGeocodingListener(this);
            _rasterMap.SetRoutingListener(this);

            // Get the configuration file.
            Configuration config =
              ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            // Get the AppSetins section.
            AppSettingsSection appSettingSection =
                (AppSettingsSection)config.GetSection("appSettings");
            foreach (var obj in appSettingSection.Settings.AllKeys)
            {
                object type = MapType.MapTypeNames[obj];
                if (type != null)
                {
                    cboMapType.Items.Add(obj);
                }

            }
            cboMapType.Text = "GOOGLEMAP";

        }



        /// <summary>
        /// Read progress notification.
        /// </summary>
        /// <param name="context">message context, can be any object.</param>
        /// <param name="bytes">the number of bytes has been read.</param>
        /// <param name="total">total bytes to be read.Total will be zero if not available</param>
        public void ReadProgress(object context, int bytes, int total)
        {
            if (total != 0)
            {
                UpdateStatus("Reading ..." + (int)((bytes / (double)total) * 100.0) + "%");
            }
        }

        /// <summary>
        /// Write progress notification.
        /// </summary>
        /// <param name="context">message context, can be any object.</param>
        /// <param name="bytes">the number of bytes has been written.</param>
        /// <param name="total">total bytes to be written.Total will be zero if not available .</param>
        public void WriteProgress(object context, int bytes, int total)
        {

        }

        /// <summary>
        /// Handle the http response.
        /// </summary>
        /// <param name="context">message context.</param>
        /// <param name="result">the result object..</param>
        public void Done(object context, Response result)
        {

        }

        /// <summary>
        /// Dones the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="rawResult">The raw result.</param>
        public void Done(object context, string rawResult)
        {

        }


        /// <summary>
        /// Dones the specified query.
        /// </summary>
        /// <param name="query">message query context(string)</param>
        /// <param name="result">the result object.</param>
        public void Done(string query, MapPoint[] result)
        {
            if (result != null)
            {
                _rasterMap.SetCenter(result[0].Point, 15, _rasterMap.GetMapType());
            }
            else
            {
                UpdateStatus("Address not found!");
            }
        }

        /// <summary>
        /// Dones the specified query.
        /// </summary>
        /// <param name="query">message query context(string).</param>
        /// <param name="result">The result.</param>
        public void Done(string query, MapDirection result)
        {
            if (result != null)
            {
                _rasterMap.SetMapDirection(result);
                _rasterMap.Resize(result.Bound);
            }
        }

        /// <summary>
        /// Read progress notification.
        /// </summary>
        /// <param name="bytes">the number of bytes has been read.</param>
        /// <param name="total">total bytes to be read.Total will be zero if not available
        /// (content-length header not set)</param>
        public void ReadProgress(int bytes, int total)
        {
            if (total != 0)
            {
                UpdateStatus("Reading ...");
            }
        }


        /// <summary>
        /// the drawing is done.
        /// </summary>
        public void Done()
        {

            UpdateStatus("");


        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            var center = new GeoLatLng(-31.948275, 115.857562);
            _rasterMap.SetCenter(center, 15, _rasterMap.GetMapType());
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            _mapTileDownloadManager.Stop();

        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            _rasterMap.PanDirection(0, 64);
        }

        private void btnDown_Click(object sender, EventArgs e)
        {

            _rasterMap.PanDirection(0, -64);
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            _rasterMap.PanDirection(64, 0);
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            _rasterMap.PanDirection(-64, 0);
        }


        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            _rasterMap.ZoomIn();

        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            _rasterMap.ZoomOut();

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            _mapTileDownloadManager.Stop();
            Application.Exit();
        }

        private readonly UTF8Encoding _encoding = new UTF8Encoding();

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAddress.Text))
            {
                string name = txtAddress.Text;
                
                _rasterMap.GetLocations(name);

            }
        }




        private void picMapCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _rasterMap.PanDirection(e.X - _oldX, e.Y - _oldY);

            }

            _oldX = e.X;
            _oldY = e.Y;

        }

        private void picMapCanvas_MouseDown(object sender, MouseEventArgs e)
        {

            _oldX = e.X;
            _oldY = e.Y;

        }






        private void cboMapType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strType = cboMapType.Text;
            object mtype = MapType.MapTypeNames[strType];
            if (mtype != null)
            {
                _mapType = (int)mtype;
                _rasterMap.SetMapType(_mapType);
            }
        }




        private void picMapCanvas_Paint(object sender, PaintEventArgs e)
        {
            _rasterMap.Paint(_mapGraphics);
            e.Graphics.DrawImage((Bitmap)_mapImage.GetNativeImage(), 0, 0);
        }

        private void btnGetDirection_Click(object sender, EventArgs e)
        {
            if (txtAddress1.Text.Length != 0 && txtAddress2.Text.Length != 0)
            {
                string name1 = txtAddress1.Text;
                string name2 = txtAddress2.Text;

                _rasterMap.GetDirections("from: " + name1 + " to: " + name2);
            }
           
        }
    }

    class CustomMapType : ICustomMapType
    {
        public string GetTileUrl(int mtype, int x, int y, int zoomLevel)
        {
            string returnURL = string.Empty;
            switch(mtype)
            {
                case MapType.GenericMaptype7:
                    returnURL = string.Format("http://www.nearmap.com/maps/hl=en&x={0}&y={1}&z={2}&nml=MapT", x, y,
                                              zoomLevel);
                    break;
                case MapType.GenericMaptype6:
                    returnURL = string.Format("http://www.nearmap.com/maps/hl=en&x={0}&y={1}&z={2}&nml=vert", x, y,
                                              zoomLevel);
                    break;
            }
            return returnURL;
        }
    }
}
