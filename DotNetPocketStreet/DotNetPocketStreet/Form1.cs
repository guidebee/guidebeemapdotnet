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
using System.Drawing;
using System.Windows.Forms;
using Mapdigit.Gis;
using Mapdigit.Gis.Drawing;
using Mapdigit.Gis.Geometry;
using Mapdigit.Gis.Raster;
using Mapdigit.Gis.Service;
using Mapdigit.Licence;
using PocketStreets.Drawing;

namespace PocketStreets
{
    public partial class Form1 : Form, IMapDrawingListener, IReaderListener,
         IRoutingListener, IGeocodingListener
    {
        /// <summary>
        /// map tile downloader manager
        /// </summary>
        private readonly MapTileDownloadManager _mapTileDownloadManager;
        /// <summary>
        /// raster map.
        /// </summary>
        private readonly RasterMap _rasterMap;
        /// <summary>
        /// map image.
        /// </summary>
        private readonly IImage _mapImage;
        /// <summary>
        /// map graphics object.
        /// </summary>
        private IGraphics _mapGraphics;

        /// <summary>
        /// map type.
        /// </summary>
        private const int MapType = Mapdigit.Gis.Raster.MapType.MicrosoftMap;

        private int _oldX;
        private int _oldY;
        private bool _isPanMode;

        private delegate void UpdateInfo(string message);

        private readonly FrmFindAddress _frmFindAddress;
        private readonly FrmMapType _frmMapType;
        private readonly FrmGetDirection _frmGetDirection;
        
        public Form1()
        {
            InitializeComponent();
            //set the licence info.
            LicenceManager licenceManager = LicenceManager.GetInstance();
            long[] keys = { -0x150dc6a05f379456L, -0x703237078e243c4fL, -0x104afc92926c5418L, 
                              -0x4af1782b11010f5dL, -0x27d1f7ce354a3419L, 0x17ded67edd3a5281L, };
            licenceManager.AddLicence("GuidebeeMap_DotNet", keys);

            //optional, get the tile url from server.
            Mapdigit.Gis.Raster.MapType.UpdateMapTileUrl();

            MapLayer.SetAbstractGraphicsFactory(NETGraphicsFactory.GetInstance());
            MapConfiguration.SetParameter(MapConfiguration.WorkerThreadNumber, 32);
            _mapImage = MapLayer.GetAbstractGraphicsFactory().CreateImage(Width, Height);
            _mapTileDownloadManager = new MapTileDownloadManager(this);
            _rasterMap = new RasterMap(1024, 1024, MapType, _mapTileDownloadManager);
            _rasterMap.SetScreenSize(Width, Height);
            _mapTileDownloadManager.Start();
            _rasterMap.SetMapDrawingListener(this);
            _rasterMap.SetRoutingListener(this);
            _rasterMap.SetGeocodingListener(this);
            GeoLatLng center = new GeoLatLng(-31.948275, 115.857562);
            _rasterMap.SetCenter(center, 15, MapType);
            _frmFindAddress = new FrmFindAddress(_rasterMap);
            _frmMapType = new FrmMapType(_rasterMap);
            _frmGetDirection = new FrmGetDirection(_rasterMap);
  
        }



        private void PanMap(int x, int y)
        {
            int dx = x - _oldX;
            int dy = y - _oldY;
            _rasterMap.PanDirection(dx, dy);

        }
        private void UpdateStatus(string messsage)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateInfo(UpdateStatus), messsage);

            }
            else
            {
                _mapGraphics = _mapImage.GetGraphics();
                _rasterMap.Paint(_mapGraphics);
                picMap.Image = (Bitmap)_mapImage.GetNativeImage();
                Console.WriteLine(messsage);
            }
        }

        public void Done()
        {
            UpdateStatus("");
        }

       

        public void Done(string query, MapDirection result)
        {
            if (result != null)
            {
                _rasterMap.SetMapDirection(result);
                _rasterMap.Resize(result.Bound);
            }
        }

        public void ReadProgress(int bytes, int total)
        {
            if (total != 0)
            {
                UpdateStatus("Reading ...");
            }
        }

        public void Done(string query, MapPoint[] result)
        {
            if (result != null)
            {
                _rasterMap.PanTo(result[0].Point);

            }
        }
        void IGeocodingListener.ReadProgress(int bytes, int total)
        {
            if (total != 0)
            {
                UpdateStatus("Reading ...");
            }
        }

        void IReaderListener.ReadProgress(int bytes, int total)
        {
            if (total != 0)
            {
                UpdateStatus("Reading ...");
            }
        }


        private void picMap_MouseDown(object sender, MouseEventArgs e)
        {
            _oldX = e.X;
            _oldY = e.Y;
            _isPanMode = true;
        }

        private void picMap_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isPanMode)
            {
                PanMap(e.X, e.Y);
                _oldX = e.X;
                _oldY = e.Y;
            }
        }

        private void picMap_MouseUp(object sender, MouseEventArgs e)
        {
            _oldX = e.X;
            _oldY = e.Y;

            _isPanMode = false;
        }

        private void menuItem2_Click(object sender, System.EventArgs e)
        {
            _frmFindAddress.Show();
        }

        private void menuItem6_Click(object sender, System.EventArgs e)
        {
            _mapTileDownloadManager.Stop();
            Application.Exit();
        }

        private void menuItem4_Click(object sender, System.EventArgs e)
        {
            _frmMapType.Show();
        }

        private void menuItem5_Click(object sender, System.EventArgs e)
        {
            _rasterMap.ZoomIn();
        }

        private void menuItem7_Click(object sender, System.EventArgs e)
        {
            _rasterMap.ZoomOut();
        }

        private void menuItem3_Click(object sender, System.EventArgs e)
        {
            _frmGetDirection.Show();
        }
    }
}