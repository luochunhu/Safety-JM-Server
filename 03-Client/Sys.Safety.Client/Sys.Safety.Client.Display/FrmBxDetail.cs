using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Sys.Safety.Client.Chart;
using Basic.Framework.Service;
using Sys.Safety.Request.Calibration;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Client.Display
{
    public partial class FrmBxDetail : DevExpress.XtraEditors.XtraForm
    {
        private readonly DateTime _dtQuery;

        public FrmBxDetail(DateTime dtQuery)
        {
            InitializeComponent();
            gvLogInfo.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gvLogInfo.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            _dtQuery = dtQuery;
            QueryTime.Text = dtQuery.ToString("yyyy年MM月dd日");
            LoadData();
        }

        private void Query_Click(object sender, EventArgs e)
        {
            Query.Enabled = false;
            LoadData();
            Query.Enabled = true;
        }

        private void LoadData()
        {
            try
            {
                ICalibrationService irms = ServiceFactory.Create<ICalibrationService>();
                var req = new GetCalibrationDetailRequest
                {
                    Time = _dtQuery
                };
                var res = irms.GetBxDetail(req);
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                BxDetailGri.DataSource = res.Data;
            }
            catch
            {
                MessageBox.Show("数据获取失败,请稍后再试。");
            }
        }

        private void BxDetailGri_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                popupMenu1.ShowPopup(Control.MousePosition);
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataRow dr = gvLogInfo.GetDataRow(gvLogInfo.FocusedRowHandle);

            if (dr == null)
            {
                return;
            }

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("PointID", dr["pointid"].ToString());
            Mnl_McLine mfml = new Mnl_McLine(dic);
            mfml.Show();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataRow dr = gvLogInfo.GetDataRow(gvLogInfo.FocusedRowHandle);

            if (dr == null)
            {
                return;
            }

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("PointID", dr["pointid"].ToString());
            Mnl_FiveMiniteLine mfml = new Mnl_FiveMiniteLine(dic);
            mfml.Show();
        }
    }
}