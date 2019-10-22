using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Runtime.InteropServices;
using Sys.Safety.DataContract;
using DevExpress.XtraGrid.Columns;

namespace Sys.Safety.Client.Alarm
{
    public partial class frmPopupAlert : DevExpress.XtraEditors.XtraForm
    {
        [DllImport("user32")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);

        //下面是可用的常量,按照不合的动画结果声明本身须要的
        private const int AW_HOR_POSITIVE = 0x0001;//自左向右显示窗口,该标记可以在迁移转变动画和滑动动画中应用。应用AW_CENTER标记时忽视该标记
        private const int AW_HOR_NEGATIVE = 0x0002;//自右向左显示窗口,该标记可以在迁移转变动画和滑动动画中应用。应用AW_CENTER标记时忽视该标记
        private const int AW_VER_POSITIVE = 0x0004;//自顶向下显示窗口,该标记可以在迁移转变动画和滑动动画中应用。应用AW_CENTER标记时忽视该标记
        private const int AW_VER_NEGATIVE = 0x0008;//自下向上显示窗口,该标记可以在迁移转变动画和滑动动画中应用。应用AW_CENTER标记时忽视该标记该标记
        private const int AW_CENTER = 0x0010;//若应用了AW_HIDE标记,则使窗口向内重叠;不然向外扩大
        private const int AW_HIDE = 0x10000;//隐蔽窗口
        private const int AW_ACTIVE = 0x20000;//激活窗口,在应用了AW_HIDE标记后不要应用这个标记
        private const int AW_SLIDE = 0x40000;//应用滑动类型动画结果,默认为迁移转变动画类型,当应用AW_CENTER标记时,这个标记就被忽视
        private const int AW_BLEND = 0x80000;//应用淡入淡出结果

        public frmPopupAlert()
        {
            InitializeComponent();
        }

        private void XtraForm1_Load(object sender, EventArgs e)
        {
            this.mainGrid.DataSource = _alertList;

            int x = Screen.PrimaryScreen.WorkingArea.Right - this.Width;
            int y = Screen.PrimaryScreen.WorkingArea.Bottom - this.Height;
            this.Location = new Point(x, y);//设置窗体在屏幕右下角显示
            AnimateWindow(this.Handle, 1000, AW_SLIDE | AW_ACTIVE | AW_VER_NEGATIVE);
        }

        private void XtraForm1_FormClosing(object sender, FormClosingEventArgs e)
        {
            AnimateWindow(this.Handle, 1000, AW_BLEND | AW_HIDE);
            e.Cancel = true;
            this.Visible = false;
        }

        private void mainGridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
        }

        List<ShowDataInfo> _alertList = new List<ShowDataInfo>();

        public void ShowData(ShowDataInfo item)
        {
            _alertList.Insert(0, item);

            if (_alertList.Count > 1000)
            {
                _alertList.RemoveAt(1000);
            }

            this.mainGridView.RefreshData();
            this.Show();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearViewList();
        }

        public void ClearViewList()
        {
            this._alertList.Clear();
            this.mainGridView.RefreshData();
        }

        public void CancelAlarm()
        {
            //this.Close();//关闭报警时不关闭弹出窗口  20170913            
        }

        public void OpenAlarm()
        {
            this.Visible = true;
            this.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmAlarmProcess frmAlarm = new frmAlarmProcess();
            frmAlarm.Show();
        }

        private void mainGridView_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            object obj;
            try
            {
                if (e.RowHandle > -1)
                {
                    obj = mainGridView.GetRowCellValue(e.RowHandle, mainGridView.Columns["AlarmColor"]);
                    if (obj == null || obj.ToString() == "")
                    {
                        e.Appearance.ForeColor = Color.Empty;
                    }
                    else
                    {
                        int color = 0;
                        int.TryParse(obj.ToString(), out color);
                        e.Appearance.ForeColor = Color.FromArgb(color);
                    }
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 双击详细查看
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mainGrid_DoubleClick(object sender, EventArgs e)
        {
            int selectedHandle;
            selectedHandle = this.mainGridView.GetSelectedRows()[0];
            if (selectedHandle >= 0)
            {
                string TypeDisplay = this.mainGridView.GetRowCellValue(selectedHandle, "TypeDisplay").ToString();
                if (TypeDisplay.Contains("传感器未标校"))
                {
                    SensorCalibration sensorCalibration = new SensorCalibration();
                    sensorCalibration.Show();
                }
                else if (TypeDisplay.Contains("传感器超期服役"))
                {
                    OvertermService overtermService = new OvertermService();
                    overtermService.Show();
                }
                else if (TypeDisplay.Contains("逻辑分析报警"))
                {
                    object Ssz = this.mainGridView.GetRowCellValue(selectedHandle, "Ssz");
                    if (null != Ssz)
                    {
                        frmViewAnalysisAlarm.Instance.DisplayText = Ssz.ToString();
                        frmViewAnalysisAlarm.Instance.Show();
                    }
                }
                else if (TypeDisplay.Contains("传感器定义不匹配"))
                {
                    SensorDefineError sensorDefineError = new SensorDefineError();
                    sensorDefineError.Show();
                }
            }
        }
    }
}