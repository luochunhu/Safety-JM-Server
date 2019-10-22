using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys.Safety.Client.Graphic
{
    public partial class WebGisDisplay : XtraForm
    {
        string url = "";
        public WebGisDisplay()
        {
            InitializeComponent();
        }
        public WebGisDisplay(Dictionary<string, string> param)
        {
            if (param != null && param.Count > 0)
            {
                url = param["Url"].ToString();
            }
            else
            {
                return;
            }
            InitializeComponent();
        }
        private void WebGisDisplay_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate(url);
        }
    }
}
