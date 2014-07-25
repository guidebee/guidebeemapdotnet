using System;
using System.Text;
using System.Windows.Forms;
using Mapdigit.Gis.Raster;
using Mapdigit.Util;

namespace PocketStreets
{
    public partial class FrmGetDirection : Form
    {
        private readonly RasterMap _rasterMap;
        public FrmGetDirection(RasterMap rasterMap)
        {
            InitializeComponent();
            _rasterMap = rasterMap;
        }

        private void menuItem2_Click(object sender, System.EventArgs e)
        {
            Hide();
        }

        private readonly UTF8Encoding _encoding = new UTF8Encoding();

        private void menuItem1_Click(object sender, System.EventArgs e)
        {
            if (txtAddress1.Text.Length != 0 && txtAddress2.Text.Length != 0)
            {
                Hide();
                string name1 = txtAddress1.Text;
                string name2 = txtAddress2.Text;
                try
                {
                    name1 = Html2Text.Encodeutf8(_encoding.GetBytes(name1));
                    name2 = Html2Text.Encodeutf8(_encoding.GetBytes(name2));
                }
                catch (Exception)
                {

                }
                _rasterMap.GetDirections("from: " + name1 + " to: " + name2);
            }
        }
    }
}