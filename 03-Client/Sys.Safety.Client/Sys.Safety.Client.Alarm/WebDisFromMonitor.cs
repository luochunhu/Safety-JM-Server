using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys.Safety.Client.Alarm
{
    public partial class WebDisFromMonitor : DevExpress.XtraEditors.XtraForm
    {
        private string recordId = "";
        public WebDisFromMonitor(string _recordId)
        {
            recordId = _recordId;
            InitializeComponent();
        }

        private void WebDisFromMonitor_Load(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Maximized;

            webBrowser1.Navigate(System.Configuration.ConfigurationManager.AppSettings["RealLinkageWebUrl"].ToString() + "?RecordId=" + recordId);
        }
    }
}
