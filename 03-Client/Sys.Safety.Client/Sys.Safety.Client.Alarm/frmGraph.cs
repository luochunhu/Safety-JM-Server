using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Runtime.InteropServices;
using Basic.Framework.Logging;

namespace Sys.Safety.Client.Alarm
{
    public partial class frmGraph : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 循环下标
        /// </summary>
        int iIndex = 0;

        /// <summary>
        /// 父窗体
        /// </summary>
        frmAlarmBgd _ParentFrm = null;

        public frmGraph(frmAlarmBgd ParentFrm)
        {
            _ParentFrm = ParentFrm;
            InitializeComponent();
        }

        private void frmGraph_Load(object sender, EventArgs e)
        {
            GraphShow();
            this.timerGraph.Enabled = true;
        }

        private void GraphShow()
        {
            try
            {
                if (ClientAlarmConfigCache.showDataGraph == null || ClientAlarmConfigCache.showDataGraph.Count < 1)
                {
                    iIndex = 0;
                    CancelAlarm();
                    return;
                }
                if (iIndex >= ClientAlarmConfigCache.showDataGraph.Count) { iIndex = 0; }

                this.lab_pointCode.Text = "";
                this.lab_address.Text = "";
                this.lab_devname.Text = "";
                this.lab_realtimeVal.Text = "";
                this.lab_dataType.Text = "";
                this.lab_deviceState.Text = "";

                this.lab_pointCode.Text = "编号：" + ClientAlarmConfigCache.showDataGraph[iIndex].Point;
                this.lab_address.Text = "名称：" + ClientAlarmConfigCache.showDataGraph[iIndex].Wz;
                this.lab_devname.Text = "类型：" + ClientAlarmConfigCache.showDataGraph[iIndex].Devname;
                this.lab_realtimeVal.Text = "实时值：" + GetSszProc(ClientAlarmConfigCache.showDataGraph[iIndex].Property, ClientAlarmConfigCache.showDataGraph[iIndex].Ssz, ClientAlarmConfigCache.showDataGraph[iIndex].Unit);
                this.lab_dataType.Text = "数据状态：" + ClientAlarmConfigCache.showDataGraph[iIndex].TypeDisplay;
                this.lab_deviceState.Text = "设备状态：" + ClientAlarmConfigCache.showDataGraph[iIndex].StateDisplay;
                this.lab_Timer.Text = "时间：" + ClientAlarmConfigCache.showDataGraph[iIndex].Timer;

                if (ClientAlarmConfigCache.showDataGraph[iIndex].Property == 6)//其它报警类型，不显示设备状态  20171214
                {
                    this.lab_deviceState.Visible = false;
                    this.lab_devname.Visible = false;
                }
                else
                {
                    this.lab_deviceState.Visible = true;
                    this.lab_devname.Visible = true;
                }

                LabelSet();
                iIndex++;
            }
            catch (Exception ex)
            {
                LogHelper.Error("报警图文显示发生异常 " + ex.Message);
            }
        }

        /// <summary>
        /// 实时值显示处理
        /// </summary>
        /// <param name="Property">测点性质</param>
        /// <param name="RealtimeVal">实时值</param>
        /// <param name="Unit">单位</param>
        /// <returns>显示实时值</returns>
        private string GetSszProc(int Property, string RealtimeVal, string Unit)
        {
            string bc = string.Empty;
            if (Property != 1)//非模拟量
            {
                bc = RealtimeVal;
                return bc;
            }
            if (RealtimeVal == "上溢" || RealtimeVal == "负漂" || RealtimeVal == "断线")
            {
                bc = RealtimeVal;
                return bc;
            }
            bc = RealtimeVal + Unit;
            return bc;
        }

        private void LabelSet()
        {
            try
            {
                string str = lab_address.Text;
                //int iLen = str.Length;//Lab内容长度
                //int iRowNum = 10;//每行显示字数
                //int iBefore = 0;//之前的长度
                int iNowLen = 0;//当前的长度
                for (int i = 0; i < str.Length; i++)
                {
                    iNowLen++;
                    if ((int)str[i] > 128)//中文字符
                    {
                        iNowLen++;
                    }
                    if (iNowLen > 20)
                    {
                        str = str.Insert(i, System.Environment.NewLine);
                        i++;
                        iNowLen = 0;
                    }
                }
                //int iCnt = iLen % iRowNum == 0 ? iLen / iRowNum - 1 : iLen / iRowNum;//需要插入的换行符个数
                //for (int i = 1; i < iCnt + 1; i++)
                //{
                //    str = str.Insert(i * iRowNum, System.Environment.NewLine);
                //}
                lab_address.Text = str;
            }
            catch (Exception ex)
            {
                LogHelper.Error("处理图文报警的位置换行出现异常" + ex.Message);
            }
        }

        public void CancelAlarm()
        {
            if (ClientAlarmConfigCache.showDataGraph != null)
            {
                ClientAlarmConfigCache.showDataGraph.Clear();
            }
            _ParentFrm.bIsOpenGraphDlg = false;
            this.Close();
            this.Dispose();
        }

        private void timerGraph_Tick(object sender, EventArgs e)
        {
            timerGraph.Enabled = false;//20151028 txy
            GraphShow();
            timerGraph.Enabled = true;
        }

        //public const int WM_SYSCOMMAND = 0x0112;

        //public const int SC_MOVE = 0xF010;

        //public const int HTCAPTION = 0x0002;

        public static void DragWinFrom(IntPtr HWND)
        {
            //扑捉事件
            ReleaseCapture();
            //发送消息给window Api 来实现
            //SendMessage(HWND, 0x0112, 0xF010 + 0x0002, 0);
            SendMessage(HWND, 0xA1, 0x2, 0);
        }

        /// <summary>
        /// 重写WndProc方法,实现窗体移动和禁止双击最大化
        /// </summary>
        /// <param name="m">Windows 消息</param>
        //protected override void WndProc(ref Message m)
        //{
        //    switch (m.Msg)
        //    {
        //        case 0x4e:
        //        case 0xd:
        //        case 0xe:
        //        case 0x14:
        //            base.WndProc(ref m);
        //            break;
        //        case 0x84://鼠标点任意位置后可以拖动窗体
        //            this.DefWndProc(ref m);
        //            if (m.Result.ToInt32() == 0x01)
        //            {
        //                m.Result = new IntPtr(0x02);
        //            }
        //            break;
        //        case 0xA3://禁止双击最大化
        //            break;
        //        default:
        //            base.WndProc(ref m);
        //            break;
        //    }
        //}

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        public void pictureEdit1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    colseAlarm();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("图片双击发生异常 " + ex.Message);
            }
        }

        private void pictureEdit1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left && e.Clicks == 1)
                {
                    DragWinFrom(this.Handle);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("拖动弹窗发生异常 " + ex.Message);
            }
        }

        private void frmGraph_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    colseAlarm();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("弹窗双击发生异常 " + ex.Message);
            }
        }

        private void frmGraph_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left && e.Clicks == 1)
                {
                    DragWinFrom(this.Handle);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("拖动弹窗发生异常 " + ex.Message);
            }
        }
        

        private void colseAlarm()
        {
            try
            {
                _ParentFrm.CancleAlarm();
                _ParentFrm.bIsOpenGraphDlg = false;
                this.Close();
                this.Dispose();
            }
            catch (Exception ex)
            {
                LogHelper.Error("图片双击发生异常 " + ex.Message);
                _ParentFrm.bIsOpenGraphDlg = false;
                this.Close();
                this.Dispose();
            }
        }
       

        private void lab_pointCode_DoubleClick(object sender, MouseEventArgs e)
        {
            colseAlarm();
        }

        private void lab_devname_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            colseAlarm();
        }

        private void lab_address_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            colseAlarm();
        }

        private void lab_realtimeVal_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            colseAlarm();
        }

        private void lab_dataType_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            colseAlarm();
        }

        private void lab_deviceState_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            colseAlarm();
        }
    }
}