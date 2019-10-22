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
    public partial class OvertermService : DevExpress.XtraEditors.XtraForm
    {
        public OvertermService()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //方法1
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Title = "传感器超期服役记录";
            fileDialog.FileName = "传感器超期服役记录";
            fileDialog.Filter = "Excel文件(*.xls)|*.xls";
            DialogResult dialogResult = fileDialog.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                gridControl1.ExportToXls(fileDialog.FileName);
                DevExpress.XtraEditors.XtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SensorCalibration_Load(object sender, EventArgs e)
        {
            gridControl1.DataSource = ClientAlarmServer.sensorOvertermServiceInfoList;          
        }
    }
}
