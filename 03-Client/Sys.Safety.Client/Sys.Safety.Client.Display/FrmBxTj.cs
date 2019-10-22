using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request.Calibration;
using Basic.Framework.Service;
using Message = System.ServiceModel.Channels.Message;

namespace Sys.Safety.Client.Display
{
    public partial class FrmBxTj : DevExpress.XtraEditors.XtraForm
    {
        private ICalibrationService calibrationService = ServiceFactory.Create<ICalibrationService>();
        public class ComboxData
        {
            public string Text { set; get; }

            public string Value { set; get; }

            public override string ToString()
            {
                return Text;
            }
        }

        public FrmBxTj()
        {
            InitializeComponent();
            gvLogInfo.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gvLogInfo.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            int iYear = DateTime.Now.Year;
            for (int i = 0; i < 10; i++)
            {
                int iY = iYear - 5 + i;

                QueryTime.Properties.Items.Add(new ComboxData
                {
                    Text = iY.ToString(),
                    Value = iY.ToString()
                });
            }

            QueryTime.EditValue = iYear;
            LoadGrid(iYear.ToString());
        }

        private void LoadGrid(string year)
        {
            DateTime dt;
            try
            {
                dt = new DateTime(Convert.ToInt32(year), 1, 1);
            }
            catch
            {
                MessageBox.Show("查询年份有误。");
                return;
            }
            try
            {
                var request = new GetCalibrationStatisticsRequest() { Time = dt };
                var response = calibrationService.GetBxStatistics(request);
                if (response.Data != null)
                {
                    BxTjGri.DataSource = response.Data;
                }
            }
            catch
            {
                MessageBox.Show("数据获取失败,请稍后再试。");
            }
        }

        private void Query_Click(object sender, EventArgs e)
        {
            Query.Enabled = false;
            LoadGrid(QueryTime.EditValue.ToString());
            Query.Enabled = true;
        }

        private void BxTjGri_DoubleClick(object sender, EventArgs e)
        {
            DataRow dr = gvLogInfo.GetDataRow(gvLogInfo.FocusedRowHandle);

            if (dr == null)
            {
                return;
            }

            string time = dr["time"].ToString();
            DateTime dt;
            try
            {
                dt = Convert.ToDateTime(time);
            }
            catch
            {
                MessageBox.Show("数据错误。");
                return;
            }
            FrmBxDetail fbd = new FrmBxDetail(dt);
            fbd.ShowDialog();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            SetPoints f = new SetPoints();
            f.ShowDialog();
        }
    }
}