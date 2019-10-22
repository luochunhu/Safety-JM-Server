using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Basic.Framework.Logging;
using Sys.Safety.DataContract;

namespace Sys.Safety.Client.Display
{
    public partial class CuCharge : UserControl
    {
        public CuCharge()
        {
            InitializeComponent();
        }

        public CuCharge(string StationPoint)
        {
            _StationPoint = StationPoint;
            InitializeComponent();
        }

        /// <summary>
        /// 分站测点
        /// </summary>
        private string _StationPoint = "";

        /// <summary>
        /// 分站对象
        /// </summary>
        private Jc_DefInfo tempStation = null;

        /// <summary>
        /// 加载信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CuCharge_Load(object sender, EventArgs e)
        {
            try
            {
                LoadBaseInf();
                LoadControlInf();
                EvaluatePowerPackInf();
                timerPowerPack.Enabled = true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 加载默认基础信息
        /// </summary>
        private void LoadBaseInf()
        {
        }

        /// <summary>
        /// 加载充放电信息
        /// </summary>
        private void LoadControlInf()
        {
            if (string.IsNullOrEmpty(_StationPoint))
            {
                return;
            }

            //TODO:与其它业务模块相关联
            //IList<JCJCSDKZDTO> tempControls = Model.ChargeMrg.QueryJCSDKZbyInf(10, _StationPoint);
            //if (null == tempControls)
            //{
            //    return;
            //}
            //if (tempControls.Count == 0)
            //{
            //    return;
            //}

            barSubItemExecute.Caption = "取消放电(&E)";

        }

        /// <summary>
        /// 获取电源箱状态并赋值
        /// </summary>
        private void EvaluatePowerPackInf()
        {
            if (string.IsNullOrEmpty(_StationPoint))
            {
                return;
            }

            //TODO:与其它业务模块相关联
            //tempStation = Model.ChargeMrg.QueryPointByCodeCache(_StationPoint);
            //if (tempStation == null)
            //{
            //    return;
            //}

            //if (barSubItemExecute.Caption == "取消放电(&E)")
            //{
            //    if (ClbPowerInf.Text != "执行放电")
            //    {
            //        ClbPowerInf.Text = "执行放电";
            //        ClbPowerInf.ForeColor = Color.DeepPink;
            //        CPPowerInf.ContentImage = Properties.Resources.d002;
            //    }
            //}
            //else if (tempStation.State == 4)
            //{
            //    if (ClbPowerInf.Text != "直流正常")
            //    {
            //        ClbPowerInf.Text = "直流正常";
            //        ClbPowerInf.ForeColor = Color.DeepPink;
            //        CPPowerInf.ContentImage = Properties.Resources.d002;
            //    }
            //}
            //else if (tempStation.State == 0)
            //{
            //    if (ClbPowerInf.Text != "通讯中断")
            //    {
            //        ClbPowerInf.Text = "通讯中断";
            //        ClbPowerInf.ForeColor = Color.Black;
            //        CPPowerInf.ContentImage = Properties.Resources.d003;
            //    }
            //}
            //else
            //{
            //    if (ClbPowerInf.Text != "交流正常")
            //    {
            //        ClbPowerInf.Text = "交流正常";
            //        ClbPowerInf.ForeColor = Color.Green;
            //        CPPowerInf.ContentImage = Properties.Resources.d001;
            //    }
            //}

            //ClabelControlVOL.Text = tempStation.PowerPackVOL.ToString() + "%"; //电量
            //ClabelControlMA.Text = tempStation.PowerPackMA.ToString() + "A";   //负载电流
            //if (tempStation.BatteryVOL != null)
            //{
            //    for (int i = 0; i < tempStation.BatteryVOL.Length; i++)
            //    {
            //        if (i > 3)
            //        {
            //            break;
            //        }
            //        windowsUIButtonPanel1.Buttons[i].Properties.Caption = tempStation.BatteryVOL[i].ToString() + "V"; //电池电压
            //    }
            //}
            ////设备过热
            //if ((tempStation.PowerPackState & 0x1) == 0x1)
            //{
            //    ClabelControlHot.Visible = true;
            //}
            //else
            //{
            //    ClabelControlHot.Visible = false;
            //}
            ////电池欠压
            //if ((tempStation.PowerPackState & 0x2) == 0x2)
            //{
            //    ClabelControlLessVOL.Visible = true;
            //}
            //else
            //{
            //    ClabelControlLessVOL.Visible = false;
            //}
            ////电池过充
            //if ((tempStation.PowerPackState & 0x4) == 0x4)
            //{
            //    ClabelControlOverVOL.Visible = true;
            //}
            //else
            //{
            //    ClabelControlOverVOL.Visible = false;
            //}

            ////充放电状态
            //if ((tempStation.PowerPackState & 0x8) == 0x8)
            //{
            //    ClabelControlStatue.Text = "充电中";
            //}
            //else if ((tempStation.PowerPackState & 0x10) == 0x10)
            //{
            //    ClabelControlStatue.Text = "放电中";
            //}
            //else if ((tempStation.PowerPackState & 0x20) == 0x20)
            //{
            //    ClabelControlStatue.Text = "均衡中";
            //}
            //else
            //{
            //    ClabelControlStatue.Text = "未知";
            //}
        }

        /// <summary>
        /// 获取电源箱状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(_StationPoint))
                {
                    return;
                }
                //DOTO:与其它业务模块相关联
                //JCDEFDTO Station = Model.ChargeMrg.QueryPointByCodeCache(_StationPoint);
                //if (Station == null)
                //{
                //    return;
                //}

                //Model.ChargeMrg.sendD(0, Station.Fzh.ToString());//txy 20170330
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 放电
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barSubItemExecute_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            barSubItemExecute.Enabled = false;
            try
            {
                if (string.IsNullOrEmpty(_StationPoint))
                {
                    return;
                }
                //与其它业务模块相关联
                //IList<JCJCSDKZDTO> tempControls = Model.ChargeMrg.QueryJCSDKZbyInf(10, _StationPoint);
                //JCJCSDKZDTO tempControlADD;
                //if (null == tempControls)   //原来没有放电命令
                //{
                //    if (barSubItemExecute.Caption == "执行放电(&E)") //现在增加
                //    {
                //        tempControlADD = new JCJCSDKZDTO();
                //        tempControlADD.ID = Basic.Framework.Utils.Date.DateTimeUtil.GetDateTimeNowToInt64();//ID
                //        tempControlADD.Type = 10; //放电命令
                //        tempControlADD.ZkPoint = "0000000";//主控测点
                //        tempControlADD.Bkpoint = _StationPoint;//被控测点
                //        tempControlADD.DTOState = Framework.Core.Service.DTO.DTOStateEnum.AddNew;
                //        try
                //        {
                //            Model.ChargeMrg.AddJC_JCSDKZCache(tempControlADD);
                //        }
                //        catch (Exception ex)
                //        {
                //            XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //            return;
                //        }
                //        Basic.CBF.Common.Util.OperateLogHelper.InsertOperateLog(4, "执行放电：主控【" + tempControlADD.ZkPoint + "】-【" + tempControlADD.Bkpoint + "】-【" + DateTime.Now.ToString() + "】", "");
                //        barSubItemExecute.Caption = "取消放电(&E)";
                //    }

                //}
                //else
                //{
                //    if (tempControls.Count > 0) //原来有放电命令
                //    {
                //        if (barSubItemExecute.Caption == "取消放电(&E)") //现在取消
                //        {
                //            for (int i = 0; i < tempControls.Count; i++)
                //            {
                //                tempControls[i].DTOState = Framework.Core.Service.DTO.DTOStateEnum.Delete;
                //                // Model.ChargeMrg.DelJC_JCSDKZCache(tempControls[i].ID);//删除，改为调用批量接口  20170323
                //                Basic.CBF.Common.Util.OperateLogHelper.InsertOperateLog(4, "取消放电：主控【" + tempControls[i].ZkPoint + "】-【" + tempControls[i].Bkpoint + "】-【" + DateTime.Now.ToString() + "】", "");
                //            }
                //            //统一调用批量删除的接口来执行  20170323
                //            Model.ChargeMrg.DelJC_JCSDKZCache(tempControls.ToList());
                //            barSubItemExecute.Caption = "执行放电(&E)";
                //        }
                //    }
                //    else
                //    {
                //        //原来没有放电命令
                //        if (barSubItemExecute.Caption == "执行放电(&E)") //现在增加
                //        {
                //            tempControlADD = new JCJCSDKZDTO();
                //            tempControlADD.ID = Basic.Framework.Utils.Date.DateTimeUtil.GetDateTimeNowToInt64();//ID
                //            tempControlADD.Type = 10; //放电命令
                //            tempControlADD.ZkPoint = "0000000";//主控测点
                //            tempControlADD.Bkpoint = _StationPoint;//被控测点
                //            tempControlADD.DTOState = Framework.Core.Service.DTO.DTOStateEnum.AddNew;
                //            try
                //            {
                //                Model.ChargeMrg.AddJC_JCSDKZCache(tempControlADD);
                //            }
                //            catch (Exception ex)
                //            {
                //                XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //                return;
                //            }
                //            Basic.CBF.Common.Util.OperateLogHelper.InsertOperateLog(4, "执行放电：主控【" + tempControlADD.ZkPoint + "】-【" + tempControlADD.Bkpoint + "】-【" + DateTime.Now.ToString() + "】", "");
                //            barSubItemExecute.Caption = "取消放电(&E)";
                //        }
                //    }
                //}

                //Model.ChargeMrg.SaveDataJCSDKZ();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            barSubItemExecute.Enabled = true;
        }

        /// <summary>
        /// 间隔3秒刷新一次电源箱状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerPowerPack_Tick(object sender, EventArgs e)
        {
            try
            {
                EvaluatePowerPackInf();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

    }
}
