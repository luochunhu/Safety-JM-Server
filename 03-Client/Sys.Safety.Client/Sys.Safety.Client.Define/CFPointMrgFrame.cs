using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using DevExpress.XtraEditors;
using Sys.Safety.Client.Define.Model;
using DevExpress.XtraBars.Ribbon;
using Basic.Framework.Logging;
using DevExpress.LookAndFeel;

namespace Sys.Safety.Client.Define
{
    public partial class CFPointMrgFrame : XtraForm
    {
        public CFPointMrgFrame()
        {
            try
            {
                ////设置所有窗体支持皮肤设置

                DevExpress.Skins.SkinManager.EnableFormSkins();
                DefaultLookAndFeel defaultLookAndFeel = new DevExpress.LookAndFeel.DefaultLookAndFeel();
                if (!string.IsNullOrEmpty(Program.WindowStypeNow))
                {
                    defaultLookAndFeel.LookAndFeel.SetSkinStyle(Program.WindowStypeNow);
                }
                else
                {
                    defaultLookAndFeel.LookAndFeel.SetSkinStyle("Visual Studio 2013 Blue");
                }

                SetStyle(ControlStyles.UserPaint, true);
                SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
                SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
                InitializeComponent();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

        }
        public CFPointMrgFrame(int fzh)
        {
            try
            {
                ////设置所有窗体支持皮肤设置
                PubFzh = fzh;
                DevExpress.Skins.SkinManager.EnableFormSkins();
                DefaultLookAndFeel defaultLookAndFeel = new DevExpress.LookAndFeel.DefaultLookAndFeel();
                if (!string.IsNullOrEmpty(Program.WindowStypeNow))
                {
                    defaultLookAndFeel.LookAndFeel.SetSkinStyle(Program.WindowStypeNow);
                }
                else
                {
                    defaultLookAndFeel.LookAndFeel.SetSkinStyle("Visual Studio 2013 Blue");
                }

                SetStyle(ControlStyles.UserPaint, true);
                SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
                SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
                InitializeComponent();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

        }
        public CFPointMrgFrame(int fzh, int chanelNumber,int dzh, int devPropertyId, int deviceId)
        {
            try
            {
                ////设置所有窗体支持皮肤设置
                PubFzh = fzh;
                PubChanelNumber = chanelNumber;
                PubAddressNumber = dzh;
                PubDevPropertyId = devPropertyId;
                PubDeviceId = deviceId;
                DevExpress.Skins.SkinManager.EnableFormSkins();
                DefaultLookAndFeel defaultLookAndFeel = new DevExpress.LookAndFeel.DefaultLookAndFeel();
                if (!string.IsNullOrEmpty(Program.WindowStypeNow))
                {
                    defaultLookAndFeel.LookAndFeel.SetSkinStyle(Program.WindowStypeNow);
                }
                else
                {
                    defaultLookAndFeel.LookAndFeel.SetSkinStyle("Visual Studio 2013 Blue");
                }

                SetStyle(ControlStyles.UserPaint, true);
                SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
                SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
                InitializeComponent();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

        }
        public int PubFzh = 0, PubChanelNumber = 0, PubAddressNumber = 0, PubDevPropertyId = 0, PubDeviceId = 0;
        /// <summary>
        /// Grid 对象
        /// </summary>
        public CuGrid cuGrid;
        /// <summary>
        /// 导航栏
        /// </summary>
        public CuNavigation cuNavigation;
        /// <summary>
        /// 用于更新UI的委托定义
        /// </summary>
        private delegate void UpdateControl();
        /// <summary>
        /// 后台委托封装
        /// </summary>
        /// <returns></returns>
        private delegate bool dl_DoAsync();
        /// <summary>
        /// 用于更新UI委托声明
        /// </summary>
        private UpdateControl dl_updataUI;
        /// <summary>
        /// 处理完成回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void CallBackMethod(IAsyncResult ar)
        {
            try
            {
                dl_DoAsync dl_do = (dl_DoAsync)ar.AsyncState;
                dl_do.EndInvoke(ar);
                this.BeginInvoke(dl_updataUI); //更新UI
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 处理后台事件的过程
        /// </summary>
        /// <returns></returns>
        private bool DoAsync()
        {
            #region 此处写后台代码
            try
            {
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            #endregion
            return true;
        }
        /// <summary>
        /// 更新UI
        /// </summary>
        private void updateUI()
        {
            #region 此处写更新UI的代码
            #endregion
        }
        /// <summary>
        /// 定义框架用提示栏更新事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BlockConfig_DefineFramstatustringChanged(object sender, EventArgs e)
        {
            ClbTip.Text = "";
        }
        public void Reload()
        {
            object sender1 = null;
            var e1 = new EventArgs();
            DefineFrame_Load(sender1, e1);
        }
        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DefineFrame_Load(object sender, EventArgs e)
        {
            try
            {
                //设置窗体高度和宽度
                Width = Convert.ToInt32(Screen.GetWorkingArea(this).Width * 0.9);
                Height = Convert.ToInt32(Screen.GetWorkingArea(this).Height * 0.9);
                Left = Convert.ToInt32(Screen.GetWorkingArea(this).Width * 0.1 / 2);
                Top = Convert.ToInt32(Screen.GetWorkingArea(this).Height * 0.1 / 2);

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                if (cuGrid == null)
                {
                    cuGrid = new CuGrid(this);
                    CpTip.Controls.Add(cuGrid);
                    cuGrid.Dock = DockStyle.Fill;
                }
                else
                {
                    cuGrid.EditStationNow = null;//重置分站选择  20171016
                }
                stopwatch.Stop();
                LogHelper.Debug("右边列表加载，耗时：" + stopwatch.ElapsedMilliseconds);
                stopwatch.Restart();
                if (cuNavigation == null)
                {
                    cuNavigation = new CuNavigation(this);
                    CpleftDocument.Controls.Add(cuNavigation);
                    cuNavigation.Dock = DockStyle.Fill;
                }
                if (PubFzh > 0)
                {
                    //根据传入的参数加载需要修改的分站  20180606
                    string tempPoint = PubFzh.ToString("000") + "0000";
                    cuNavigation.ShowFormForDev("8", "", tempPoint, "基础通道," + PubChanelNumber.ToString() + "," + PubAddressNumber.ToString() + "," + PubDevPropertyId + "," + PubDeviceId);
                    cuNavigation.RefreshGridInDev("7", tempPoint);
                }
                stopwatch.Stop();
                LogHelper.Debug("左边导航树加载，耗时：" + stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 适应屏幕变化，留出任务栏
        /// </summary>
        private void ScreenAdapter()
        {
            this.Top = 0;
            this.Left = 0;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width;
        }
        /// <summary>
        /// 从新加载信息
        /// </summary>
        public void overLoadInf()
        {
            try
            {
                DoAsync();

                ReleaseResource();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        private void DefineFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                CONFIGServiceModel.SaveRouting();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 清除定义缓存
        /// </summary>
        private void ReleaseResource()
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary> 保存巡检
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbSave_Click(object sender, EventArgs e)
        {
            try
            {
                CONFIGServiceModel.SaveRouting();
                XtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            ClbTip.Text = "保存成功";
        }
        /// <summary>退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbExist_Click(object sender, EventArgs e)
        {
            //移除控件  20170330
            try
            {

                //CpleftDocument.Controls.Remove(cuNavigation);
                //CpTip.Controls.Remove(cuGrid);

                //cuGrid = null;
                //cuNavigation = null;

                this.Close();


            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void barButtonSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                CONFIGServiceModel.SaveRouting();
                XtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            //ClbTip.Text = "保存成功";
        }

        private void barButtonExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //移除控件  20170330
            try
            {

                //CpleftDocument.Controls.Remove(cuNavigation);
                //CpTip.Controls.Remove(cuGrid);

                //cuGrid = null;
                //cuNavigation = null;

                this.Close();


            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
    }
}
