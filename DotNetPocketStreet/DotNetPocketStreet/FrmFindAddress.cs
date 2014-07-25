using System;
using System.Text;
using System.Windows.Forms;
using Mapdigit.Gis.Raster;
using Mapdigit.Util;

namespace PocketStreets
{
    public partial class FrmFindAddress : Form
    {
        private readonly RasterMap _rasterMap;
        private readonly UTF8Encoding _encoding = new UTF8Encoding();

        public FrmFindAddress(RasterMap rasterMap)
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
            if (!string.IsNullOrEmpty(txtAddress.Text))
            {
                Hide();
                string name = txtAddress.Text;
                try
                {
                    name = Html2Text.Encodeutf8(_encoding.GetBytes(name));
                }
                catch (Exception)
                {

                }
                _rasterMap.GetLocations(name);
               

            }
        }

        
    }
}