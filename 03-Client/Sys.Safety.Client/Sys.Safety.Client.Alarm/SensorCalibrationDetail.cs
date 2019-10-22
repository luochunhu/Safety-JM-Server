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

namespace Sys.Safety.Client.Alarm
{
    public partial class SensorCalibrationDetail : XtraForm
    {
        public bool isSave = false;
        public string csStr = "";
        public DateTime stime = DateTime.Now;
        public DateTime etime = DateTime.Now;
        public SensorCalibrationDetail()
        {
            InitializeComponent();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            
            if (txt_cs.Text.Trim() == "")
            {
                MessageBox.Show("请先录入处理措施");
                return;
            }
            if (dtp_stime.Value > dtp_etime.Value)
            {
                MessageBox.Show("录入时间错误");
                return;
            }
            csStr = txt_cs.Text.Trim();
            stime = dtp_stime.Value;
            etime = dtp_etime.Value;
            isSave = true;
            this.Close();
        }
    }
}
