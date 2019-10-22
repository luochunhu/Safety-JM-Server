using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using Basic.Framework.Common;
using Basic.Framework.Logging;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.CommunicateExtend;
using Sys.Safety.ClientFramework.CBFCommon;
using Sys.Safety.Client.Control.Model;
using Sys.Safety.Client.Control.Properties;

namespace Sys.Safety.Client.Control
{
    public partial class CuCharge : UserControl
    {
        //private readonly string _fzh = "";

        ///// <summary>
        /////     电源箱地址号
        ///// </summary>
        //private readonly string _StationPoint = "";

        private BatteryItem _batteryItem = null;

        private BatteryPowerConsumption _batteryPowerConsumption = null;

        public CuCharge()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="StationPoint">电源箱地址号</param>
        /// <param name="fzh">分站号</param>
        //public CuCharge(string StationPoint, string fzh)
        //{
        //    _StationPoint = StationPoint;
        //    _fzh = fzh;
        //    InitializeComponent();
        //}
        public CuCharge(BatteryItem bi, BatteryPowerConsumption batteryPowerConsumption)
        {
            //_StationPoint = StationPoint;
            //_fzh = fzh;
            _batteryItem = bi;
            _batteryPowerConsumption = batteryPowerConsumption;
            InitializeComponent();
        }

        /// <summary>
        ///     分站对象
        /// </summary>
        /// <summary>
        ///     加载信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CuCharge_Load(object sender, EventArgs e)
        {
            try
            {
                //LoadControlInf();
                EvaluatePowerPackInf();
                //timerPowerPack.Enabled = true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        /// <summary>
        ///     获取电源箱状态并赋值
        /// </summary>
        private void EvaluatePowerPackInf()
        {
            //if (string.IsNullOrEmpty(_StationPoint) || string.IsNullOrEmpty(_fzh))
            //    return;
            ////tempStation = Model.ChargeMrg.QueryPointByCodeCache(_StationPoint);
            //var dyxInfo = ChargeMrg.GetSubstationBatteryInfo(_StationPoint, _fzh);
            //if (dyxInfo == null)
            //    return;
            var dyxInfo = _batteryItem;

            //switch (dyxInfo.PowerPackState)
            //{
            //    case 0:
            //        if (ClbPowerInf.Text != "通讯中断")
            //        {
            //            ClbPowerInf.Text = "通讯中断";
            //            ClbPowerInf.ForeColor = Color.Black;
            //            CPPowerInf.ContentImage = Resources.d003;
            //        }
            //        break;
            //    case 4:
            //        if (ClbPowerInf.Text != "直流正常")
            //        {
            //            ClbPowerInf.Text = "直流正常";
            //            ClbPowerInf.ForeColor = Color.DeepPink;
            //            CPPowerInf.ContentImage = Resources.d002;
            //        }
            //        break;
            //    default:
            //        if (ClbPowerInf.Text != "交流正常")
            //        {
            //            ClbPowerInf.Text = "交流正常";
            //            ClbPowerInf.ForeColor = Color.Green;
            //            CPPowerInf.ContentImage = Resources.d001;
            //        }
            //        break;
            //}

            if (dyxInfo.Channel == "0")
            {
                ClbPowerInf.Text = "通讯中断";
                ClbPowerInf.ForeColor = Color.Red;
                CPPowerInf.ContentImage = Resources.d003;

                ClabelControlVOL.Text = "未知";
                ClabelControlMA.Text = "未知";
                for (var i = 0; i < 8; i++)
                {
                    if (i > dyxInfo.BatteryVOL.Length - 1 || ClbPowerInf.Text == "通讯中断")
                    {
                        windowsUIButtonPanel1.Buttons[i].Properties.Visible = false;
                    }
                }

                return;
            }

            if (dyxInfo.BatteryACDC == 2)
            {
                if (ClbPowerInf.Text != "直流正常")
                {
                    ClbPowerInf.Text = "直流正常";
                    ClbPowerInf.ForeColor = Color.DeepPink;
                    CPPowerInf.ContentImage = Resources.d002;
                }
            }
            else if (dyxInfo.BatteryACDC == 1)
            {
                if (ClbPowerInf.Text != "交流正常")
                {
                    ClbPowerInf.Text = "交流正常";
                    ClbPowerInf.ForeColor = Color.Green;
                    CPPowerInf.ContentImage = Resources.d001;
                }
            }
            else
            {
                if (ClbPowerInf.Text != "通讯中断")
                {
                    ClbPowerInf.Text = "通讯中断";
                    ClbPowerInf.ForeColor = Color.Red;
                    CPPowerInf.ContentImage = Resources.d003;
                }
            }

            float PowerBoxVol = (float)(((dyxInfo.TotalVoltage - 19.2) * 100) / 6);
            if (PowerBoxVol > 100) PowerBoxVol = 100.0f;
            if (PowerBoxVol < 0) PowerBoxVol = 0.0f;
            ClabelControlVOL.Text = PowerBoxVol.ToString("f1") + "%";
            //计算当前电源箱电池的剩余使用时间  20180125
            if (dyxInfo.BatteryACDC == 2)//直流时进行计算
            {
                if (_batteryPowerConsumption.PowerConsumption > 0)
                {
                    float totalMinite = (PowerBoxVol / (float)(_batteryPowerConsumption.PowerConsumption / 5.0));
                    ClabelControlVOL.Text += ",时间约：" + ((int)(totalMinite / 60)) + "小时" + ((int)(totalMinite % 60)) + "分钟";
                }
            }
            ClabelControlMA.Text = "温度T1:" + dyxInfo.DeviceTemperature1.ToString("f1") + "℃" + "," + "温度T2:" + dyxInfo.DeviceTemperature2.ToString("f1") + "℃";
            if (dyxInfo.BatteryVOL != null)
                for (var i = 0; i < 8; i++)
                    if (i > dyxInfo.BatteryVOL.Length - 1 || ClbPowerInf.Text == "通讯中断")
                    {
                        windowsUIButtonPanel1.Buttons[i].Properties.Visible = false;
                    }
                    else
                    {
                        windowsUIButtonPanel1.Buttons[i].Properties.Caption = dyxInfo.BatteryVOL[i] + "V"; //电池电压
                        windowsUIButtonPanel1.Buttons[i].Properties.Visible = true;
                    }
            //设备过热

            //ClabelControlHot.Visible = dyxInfo.BatteryTooHot;


            //电池欠压

            //ClabelControlLessVOL.Visible = dyxInfo.BatteryUndervoltage;


            //电池过充
            //ClabelControlLessVOL.Visible = dyxInfo.BatteryOverCharge;

            //充放电状态
            ClabelControlStatue.Text = "";

            //if (dyxInfo.BatteryState == 1)
            //    ClabelControlStatue.Text += " 维护性放电 ";

            //if (dyxInfo.BatteryPackStateCd == 1)
            //    ClabelControlStatue.Text += " 充电中";
            //if (dyxInfo.BatteryPackStateFd == 1)
            //    ClabelControlStatue.Text += " 放电中";
            //if (dyxInfo.BatteryPackStateJh == 1)
            //    ClabelControlStatue.Text += " 均衡中";

            if (ClabelControlStatue.Text.Trim().Length < 1)
            {
                ClabelControlStatue.Text = "正常";
            }

        }

        /// <summary>
        ///     获取电源箱状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            //try
            //{
            //    if (string.IsNullOrEmpty(_StationPoint))
            //        return;
            //    var Station = ChargeMrg.QueryPointByCodeCache(_StationPoint);
            //    if (Station == null)
            //        return;

            //    ChargeMrg.sendD(0, Station.Fzh.ToString()); //txy 20170330
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.Error(ex);
            //}
        }

        /// <summary>
        ///     放电
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barSubItemExecute_ItemClick(object sender, ItemClickEventArgs e)
        {
            //barSubItemExecute.Enabled = false;
            //try
            //{
            //    if (string.IsNullOrEmpty(_StationPoint))
            //        return;
            //    var tempControls = ChargeMrg.QueryJCSDKZbyInf(10, _StationPoint);
            //    Jc_JcsdkzInfo tempControlADD;
            //    if (null == tempControls) //原来没有放电命令
            //    {
            //        if (barSubItemExecute.Caption == "执行放电(&E)") //现在增加
            //        {
            //            tempControlADD = new Jc_JcsdkzInfo
            //            {
            //                ID = IdHelper.CreateLongId().ToString(),
            //                Type = 10,
            //                ZkPoint = "0000000",
            //                Bkpoint = _StationPoint,
            //                InfoState = InfoState.AddNew,
            //                Upflag = ""
            //            };
            //            try
            //            {
            //                ChargeMrg.AddJC_JCSDKZCache(tempControlADD);
            //            }
            //            catch (Exception ex)
            //            {
            //                XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //                return;
            //            }
            //            //Basic.CBF.Common.Util.OperateLogHelper.InsertOperateLog(4, "执行放电：主控【" + tempControlADD.ZkPoint + "】-【" + tempControlADD.Bkpoint + "】-【" + DateTime.Now.ToString() + "】", "");
            //            OperateLogHelper.InsertOperateLog(4,
            //                "执行放电：主控【" + tempControlADD.ZkPoint + "】-【" + tempControlADD.Bkpoint + "】-【" +
            //                DateTime.Now + "】", "");
            //            barSubItemExecute.Caption = "取消放电(&E)";
            //        }
            //    }
            //    else
            //    {
            //        if (tempControls.Count > 0) //原来有放电命令
            //        {
            //            if (barSubItemExecute.Caption == "取消放电(&E)") //现在取消
            //            {
            //                for (var i = 0; i < tempControls.Count; i++)
            //                {
            //                    tempControls[i].InfoState = InfoState.Delete;
            //                    // Model.ChargeMrg.DelJC_JCSDKZCache(tempControls[i].ID);//删除，改为调用批量接口  20170323
            //                    //Basic.CBF.Common.Util.OperateLogHelper.InsertOperateLog(4, "取消放电：主控【" + tempControls[i].ZkPoint + "】-【" + tempControls[i].Bkpoint + "】-【" + DateTime.Now.ToString() + "】", "");
            //                    OperateLogHelper.InsertOperateLog(4,
            //                        "取消放电：主控【" + tempControls[i].ZkPoint + "】-【" + tempControls[i].Bkpoint + "】-【" +
            //                        DateTime.Now + "】", "");
            //                }
            //                //统一调用批量删除的接口来执行  20170323
            //                ChargeMrg.DelJC_JCSDKZCache(tempControls.ToList());
            //                barSubItemExecute.Caption = "执行放电(&E)";
            //            }
            //        }
            //        else
            //        {
            //            //原来没有放电命令
            //            if (barSubItemExecute.Caption == "执行放电(&E)") //现在增加
            //            {
            //                tempControlADD = new Jc_JcsdkzInfo();
            //                tempControlADD.ID = IdHelper.CreateLongId().ToString(); //ID
            //                tempControlADD.Type = 10; //放电命令
            //                tempControlADD.ZkPoint = "0000000"; //主控测点
            //                tempControlADD.Bkpoint = _StationPoint; //被控测点
            //                tempControlADD.InfoState = InfoState.AddNew;
            //                try
            //                {
            //                    ChargeMrg.AddJC_JCSDKZCache(tempControlADD);
            //                }
            //                catch (Exception ex)
            //                {
            //                    XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //                    return;
            //                }
            //                OperateLogHelper.InsertOperateLog(4,
            //                    "执行放电：主控【" + tempControlADD.ZkPoint + "】-【" + tempControlADD.Bkpoint + "】-【" +
            //                    DateTime.Now + "】", "");
            //                barSubItemExecute.Caption = "取消放电(&E)";
            //            }
            //        }
            //    }

            //    //Model.ChargeMrg.SaveDataJCSDKZ();
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.Error(ex);
            //}
            //barSubItemExecute.Enabled = true;
        }

        /// <summary>
        ///     间隔3秒刷新一次电源箱状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerPowerPack_Tick(object sender, EventArgs e)
        {
            //try
            //{
            //    EvaluatePowerPackInf();
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.Error(ex);
            //}
        }
    }
}