using System;
using System.Windows.Forms;
using Mapdigit.Gis.Raster;

namespace PocketStreets
{
    public partial class FrmMapType : Form
    {
        private readonly RasterMap _rasterMap;
        public FrmMapType(RasterMap rasterMap)
        {
            InitializeComponent();
            _rasterMap = rasterMap;


        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            int mapType = MapType.MicrosoftMap;
            Hide();
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    mapType = MapType.GoogleMap;
                    break;
                case 1:
                    mapType = MapType.GoogleChina;
                    break;
                case 2:
                    mapType = MapType.MicrosoftMap;
                    break;
                case 3:
                    mapType = MapType.MicrosoftChina;
                    break;
                case 4:
                    mapType = MapType.MicrosoftSatellite;
                    break;
                case 5:
                    mapType = MapType.MicrosoftHybrid;
                    break;
            }
            _rasterMap.SetMapType(mapType);
            
        }
    }
}