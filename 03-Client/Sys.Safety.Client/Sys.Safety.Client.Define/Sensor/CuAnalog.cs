using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Reflection;
using Sys.Safety.DataContract;
using Basic.Framework.Logging;
using Sys.Safety.Client.Define.Model;

namespace Sys.Safety.Client.Define.Sensor
{
    public partial class CuAnalog : CuBase
    {
        public CuAnalog()
        {
            InitializeComponent();
        }

        public CuAnalog(string arrPoint, int devID, uint SourceNum)
            : base(arrPoint, devID, SourceNum)
        {
            InitializeComponent();
        }

        #region =============================加载信息===============================
        /// <summary>
        /// 加载测点的默认信息函数
        /// </summary>
        public override void LoadPretermitInf()
        {
            try
            {
                //标校周期
                CtxbCheckCycle.Text = "7";
                ////单位
                IList<Jc_DevInfo> tempDevs = Model.DEVServiceModel.QueryDevByDevpropertIDCache(1); //获得所有模拟量测点
                if (null != tempDevs)
                {
                    CcmbUint.Properties.Items.Clear();
                    for (int i = 0; i < tempDevs.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(tempDevs[i].Xs1))
                        {
                            CcmbUint.Properties.Items.Add(tempDevs[i].Xs1);
                        }
                    }
                    if (CcmbUint.Properties.Items.Count > 0)
                    {
                        CcmbUint.SelectedIndex = 0;
                    }
                }



                //量程
                CtxbHiRange.Text = "0";
                CtxbMidRange.Text = "0";
                CtxbLowRange.Text = "0";
                CtxbHiHZ.Text = "0";
                CtxbLowHZ.Text = "0";
                CtxbMidHZ.Text = "0";

                //报警控制
                CtxbHiPreAlarm.Text = "0";
                CtxbHiAlarm.Text = "0";
                CtxbHiPowerBack.Text = "0";
                CtxbHiPowerOff.Text = "0";
                CtxbLowPreAlarm.Text = "0";
                CtxbLowAlarm.Text = "0";
                CtxbLowPowerOff.Text = "0";
                CtxbLowPowerBack.Text = "0";

                //赋默认值
                CckHiPreAlarm.Checked = false;
                CckHiAlarm.Checked = true;
                CckHiPower.Checked = true;
                CckLowPreAlarm.Checked = false;
                CckLowAlarm.Checked = false;
                CckLowPower.Checked = false;
                //CckOutRangeAlarm.Checked = false;
                //CckLowRangeAlarm.Checked = false;
                CckHitchRangeAlarm.Checked = false;

                //上、下限报警默认不启用
                checkEdit5.Checked = false;
                checkEdit6.Checked = false;

                //分级报警值               
                checkEdit7.Checked = false;
                levelAlarm4.Text = "";
                levelAlarm3.Text = "";
                levelAlarm2.Text = "";
                levelAlarm1.Text = "";
                levelAlarmTime4.Text = "0";
                levelAlarmTime3.Text = "";
                levelAlarmTime2.Text = "";
                levelAlarmTime1.Text = "";

                //电压报警值
                voltageAlarmCheck.Checked = false;



                //加载默认控制信息
                IList<Jc_DefInfo> tempLocalControl = Model.DEFServiceModel.QueryPointByInfs((int)_SourceNum, 3);
                if (tempLocalControl != null)
                {
                    for (int i = 0; i < tempLocalControl.Count; i++)
                    {
                        cckLocalControlHiAlarm.Properties.Items.Add(tempLocalControl[i].Point);
                        cckLocalControlHiPower.Properties.Items.Add(tempLocalControl[i].Point);
                        cckLocalControlLowAlarm.Properties.Items.Add(tempLocalControl[i].Point);
                        cckLocalControlLowPower.Properties.Items.Add(tempLocalControl[i].Point);
                        //cckLocalControlLowRange.Properties.Items.Add(tempLocalControl[i].Point);
                        //cckLocalControlOutRange.Properties.Items.Add(tempLocalControl[i].Point);
                        cckLocalControlHitch.Properties.Items.Add(tempLocalControl[i].Point);

                        if (!Model.RelateUpdate.ControlPointLegal(tempLocalControl[i])) //xuzp20151126
                        {
                            cckLocalControlHiAlarm.Properties.Items[tempLocalControl[i].Point].Enabled = false;
                            cckLocalControlHiPower.Properties.Items[tempLocalControl[i].Point].Enabled = false;
                            cckLocalControlLowAlarm.Properties.Items[tempLocalControl[i].Point].Enabled = false;
                            cckLocalControlLowPower.Properties.Items[tempLocalControl[i].Point].Enabled = false;
                            //cckLocalControlLowRange.Properties.Items[tempLocalControl[i].Point].Enabled = false;
                            //cckLocalControlOutRange.Properties.Items[tempLocalControl[i].Point].Enabled = false;
                            cckLocalControlHitch.Properties.Items[tempLocalControl[i].Point].Enabled = false;
                        }
                    }
                }

                #region ==================交叉控制==================

                List<string> StationWindBreakControlPoint = new List<string>();

                StationWindBreakControlPoint = Model.RelateUpdate.GetStationWindBreakControlPoint();//获取所有甲烷风电闭锁控制口  20170923                   

                IList<Jc_DefInfo> tempRControl = Model.DEFServiceModel.QueryPointByDevpropertIDCache(3); //获得所有的控制量
                if (null != tempRControl) //xuzp20151126
                {
                    if (tempRControl.Count > 0)
                    {
                        try
                        {
                            tempRControl = tempRControl.OrderBy(item => item.Fzh).ThenBy(item => item.Kh).ToList();
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error(ex);
                        }
                        List<CrossControlItem> tempControlList = new List<CrossControlItem>();
                        for (int i = 0; i < tempRControl.Count; i++)
                        {
                            if (tempRControl[i].Fzh != base._SourceNum)
                            {
                                //if (!Model.RelateUpdate.ControlPointLegal(tempRControl[i])) //xuzp20151126
                                //{
                                //    continue;
                                //}
                                //不每次从服务端查询，先获取分站对应的风电闭锁控制口，再判断当前是否包含在风电闭锁控制中  20170721
                                if (StationWindBreakControlPoint.Contains(tempRControl[i].Point))
                                {
                                    continue;
                                }
                                CrossControlItem tempItem = new CrossControlItem();
                                tempItem.ArrPoint = tempRControl[i].Point;
                                tempItem.PointName = tempRControl[i].Point + ":" + tempRControl[i].Wz;
                                tempControlList.Add(tempItem);
                            }
                        }
                        repositoryItemLookUpEdit1.DataSource = tempControlList;
                    }
                }
                repositoryItemComboBox2.Items.Add("断电控制");
                repositoryItemComboBox2.Items.Add("断线控制");
                //交叉故障控制未实现。暂时屏蔽  20170318
                //repositoryItemComboBox2.Items.Add("故障控制");
                #endregion
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 加载测点信息的函数
        /// </summary>
        public override void LoadInf(string arrPoint, int devID)
        {
            Jc_DefInfo tempStation = Model.DEFServiceModel.QueryPointByFzhCache((int)_SourceNum).Find(a => a.Kh == 0);
            Jc_DevInfo tempStationDev = Model.DEVServiceModel.QueryDevByDevIDCache(tempStation.Devid);
            //SettingInfo settingInfo = CONFIGServiceModel.GetConfigFKey("IsDisconnectControlReuse");
            string isDisconnectControlReuse = "0";
            //if (settingInfo != null)
            //{
            //    isDisconnectControlReuse = settingInfo.StrValue;
            //}

            //if (tempStationDev != null && tempStationDev.LC2 == 13)
            //{ //智能分站
            //    if (isDisconnectControlReuse == "1")
            //    {
            //        layoutControlItem35.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            //        layoutControlItem36.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            //        layoutControlItem37.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            //        layoutControlItem38.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            //        layoutControlItem33.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //        //layoutControlItem31.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //        //layoutControlItem28.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //        //初始化复用选项
            //        checkEdit1.Checked = false;
            //        checkEdit2.Checked = false;
            //        checkEdit3.Checked = false;
            //        checkEdit4.Checked = false;
            //    }
            //    else
            //    {
            //layoutControlItem35.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //layoutControlItem36.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //layoutControlItem37.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //layoutControlItem38.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //layoutControlItem33.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            //layoutControlItem31.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //layoutControlItem28.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //    }
            //}
            //else
            //{ //非智能分站
            //    layoutControlItem35.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //    layoutControlItem36.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //    layoutControlItem37.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //    layoutControlItem38.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //    layoutControlItem33.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

            //    //layoutControlItem31.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //    //layoutControlItem28.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //}

            if (string.IsNullOrEmpty(arrPoint))
            {
                return;
            }
            Jc_DefInfo temp = Model.DEFServiceModel.QueryPointByCodeCache(arrPoint);
            if (temp == null)
            {
                return;
            }
            Jc_DevInfo tempDev = Model.DEVServiceModel.QueryDevByDevIDCache(temp.Devid);
            if (tempDev == null)
            {
                return;
            }

            LoadPointInfo(temp, tempDev);

            //#region ==================扩展属性==================
            //if (!string.IsNullOrEmpty(temp.Bz6))
            //{
            //    CtxbCheckCycle.Text = temp.Bz6.ToString();//标校周期
            //}
            //CcmbUint.SelectedItem = tempDev.Xs1.ToString();//单位
            //CtxbLowRange.Text = "0";//低量程?
            //CtxbMidRange.Text = tempDev.LC2.ToString();//中间量程
            //CtxbHiRange.Text = tempDev.LC.ToString();//高量程
            //CtxbLowHZ.Text = tempDev.Pl1.ToString();//低频率
            //CtxbMidHZ.Text = tempDev.Pl3.ToString();//中间频率
            //CtxbHiHZ.Text = tempDev.Pl2.ToString();//高频率
            //CtxbDesc.Text = temp.Remark;//描述
            //#endregion

            //#region ==================报警控制==================

            ////if ((temp.K8 & 0x1) == 0x1)//是否上限预警
            ////{
            ////    CckHiPreAlarm.Checked = true;
            ////}
            ////else
            ////{
            ////    CckHiPreAlarm.Checked = false;
            ////}
            ////if (((temp.K8 >> 1) & 0x1) == 0x1)//是否上限报警
            ////{
            ////    CckHiAlarm.Checked = true;
            ////}
            ////else
            ////{
            ////    CckHiAlarm.Checked = false;
            ////}
            ////if (((temp.K8 >> 2) & 0x1) == 0x1)//是否上限断电
            ////{
            ////    CckHiPower.Checked = true;
            ////}
            ////else
            ////{
            ////    CckHiPower.Checked = false;
            ////}
            ////if (((temp.K8 >> 3) & 0x1) == 0x1)//是否下限预警
            ////{
            ////    CckLowPreAlarm.Checked = true;
            ////}
            ////else
            ////{
            ////    CckLowPreAlarm.Checked = false;
            ////}
            ////if (((temp.K8 >> 4) & 0x1) == 0x1)//是否下限报警
            ////{
            ////    CckLowAlarm.Checked = true;
            ////}
            ////else
            ////{
            ////    CckLowAlarm.Checked = false;
            ////}
            ////if (((temp.K8 >> 5) & 0x1) == 0x1)//是否下限断电
            ////{
            ////    CckLowPower.Checked = true;
            ////}
            ////else
            ////{
            ////    CckLowPower.Checked = false;
            ////}
            ////if (((temp.K8 >> 6) & 0x1) == 0x1)//是否上溢
            ////{
            ////    CckOutRangeAlarm.Checked = true;
            ////}
            ////else
            ////{
            ////    CckOutRangeAlarm.Checked = false;
            ////}
            ////if (((temp.K8 >> 7) & 0x1) == 0x1)//是否负漂
            ////{
            ////    CckLowRangeAlarm.Checked = true;
            ////}
            ////else
            ////{
            ////    CckLowRangeAlarm.Checked = false;
            ////}
            ////if (((temp.K8 >> 8) & 0x1) == 0x1)//是否断线
            ////{
            ////    CckHitchRangeAlarm.Checked = true;
            ////}
            ////else
            ////{
            ////    CckHitchRangeAlarm.Checked = false;
            ////}
            ////默认都可以设置  20170627           
            //CckHiPreAlarm.Checked = true;
            //CckHiAlarm.Checked = true;
            //CckHiPower.Checked = true;
            //CckLowPreAlarm.Checked = true;
            //CckLowAlarm.Checked = true;
            //CckLowPower.Checked = true;
            //CckOutRangeAlarm.Checked = true;
            //CckLowRangeAlarm.Checked = true;
            //CckHitchRangeAlarm.Checked = true;

            //CtxbHiPreAlarm.Text = temp.Z1.ToString();//上限预警值
            //CtxbHiAlarm.Text = temp.Z2.ToString();//上限报警值
            //CtxbHiPowerOff.Text = temp.Z3.ToString();//上限断电值
            //CtxbHiPowerBack.Text = temp.Z4.ToString();//上限复电值
            //CtxbLowPreAlarm.Text = temp.Z5.ToString();//下限预警值
            //CtxbLowAlarm.Text = temp.Z6.ToString();//下限报警值
            //CtxbLowPowerOff.Text = temp.Z7.ToString();//下限断电值
            //CtxbLowPowerBack.Text = temp.Z8.ToString();//下限复电值

            //cckLocalControlHiAlarm.Text = SetLocalControlText(temp.K1);//上限报警控制口
            //cckLocalControlHiPower.Text = SetLocalControlText(temp.K2);//上限断电控制口
            //cckLocalControlLowAlarm.Text = SetLocalControlText(temp.K3);//下限报警控制口
            //cckLocalControlLowPower.Text = SetLocalControlText(temp.K4);//下限断电控制口
            //cckLocalControlOutRange.Text = SetLocalControlText(temp.K5);//上溢控制口
            //cckLocalControlLowRange.Text = SetLocalControlText(temp.K6);//负漂控制口
            //cckLocalControlHitch.Text = SetLocalControlText(temp.K7);//断线控制口          

            //GetBoltControl();//加载断电复用
            //#endregion

            //#region ==================交叉控制==================
            //getCrossInf(arrPoint);
            //CrossControlDT = ToDataTable(CrossControlList);
            //CdgControl.DataSource = CrossControlDT;
            //CdgControl.RefreshDataSource();
            //#endregion


        }
        /// <summary>
        /// 交叉控制数据源
        /// </summary>
        private List<CrossControlItem> CrossControlList = new List<CrossControlItem>();
        /// <summary>
        /// 数据源DT
        /// </summary>
        private DataTable CrossControlDT = new DataTable();
        /// <summary>
        /// 获得交叉控制信息
        /// </summary>
        /// <param name="arrPoint"></param>
        /// <returns></returns>
        private void getCrossInf(string arrPoint)
        {
            try
            {
                Jc_DefInfo temp = Model.DEFServiceModel.QueryPointByCodeCache(arrPoint);
                if (temp == null)
                {
                    return;
                }
                CrossControlList.Clear();
                string[] ControlPoints;
                Jc_DefInfo tempPoint;
                CrossControlItem tempCrossItem;
                if (!string.IsNullOrEmpty(temp.Jckz1))
                {
                    ControlPoints = temp.Jckz1.Split('|');
                    if (ControlPoints.Length > 0)
                    {
                        for (int i = 0; i < ControlPoints.Length; i++)
                        {
                            tempCrossItem = new CrossControlItem();
                            tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(ControlPoints[i]);
                            if (null != tempPoint)
                            {
                                tempCrossItem.ControlType = "断电控制";
                                tempCrossItem.ArrPoint = tempPoint.Point;
                                tempCrossItem.PointName = tempPoint.Point + ":" + tempPoint.Wz;

                                if (tempPoint.K1 > 0 & tempPoint.K2 > 0)
                                {
                                    tempCrossItem.FeedBackPointName = tempPoint.K1.ToString().PadLeft(3, '0') + "D" + tempPoint.K2.ToString().PadLeft(2, '0') + tempPoint.K4.ToString() + "." + Model.DEFServiceModel.QueryPointByCodeCache(tempPoint.K1.ToString().PadLeft(3, '0') + "D" + tempPoint.K2.ToString().PadLeft(2, '0') + tempPoint.K4.ToString()).Wz;
                                }
                                CrossControlList.Add(tempCrossItem);
                            }
                        }
                    }
                }


                if (!string.IsNullOrEmpty(temp.Jckz2))
                {
                    ControlPoints = temp.Jckz2.Split('|');
                    if (ControlPoints.Length > 0)
                    {
                        for (int i = 0; i < ControlPoints.Length; i++)
                        {
                            tempCrossItem = new CrossControlItem();
                            tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(ControlPoints[i]);
                            if (null != tempPoint)
                            {
                                tempCrossItem.ControlType = "断线控制";
                                tempCrossItem.ArrPoint = tempPoint.Point;
                                tempCrossItem.PointName = tempPoint.Point + ":" + tempPoint.Wz;

                                if (tempPoint.K1 > 0 & tempPoint.K2 > 0)
                                {
                                    tempCrossItem.FeedBackPointName = tempPoint.K1.ToString().PadLeft(3, '0') + "D" + tempPoint.K2.ToString().PadLeft(2, '0') + tempPoint.K4.ToString() + "." + Model.DEFServiceModel.QueryPointByCodeCache(tempPoint.K1.ToString().PadLeft(3, '0') + "D" + tempPoint.K2.ToString().PadLeft(2, '0') + tempPoint.K4.ToString()).Wz;
                                }
                                CrossControlList.Add(tempCrossItem);
                            }
                        }
                    }
                }


                if (!string.IsNullOrEmpty(temp.Jckz3))
                {
                    ControlPoints = temp.Jckz3.Split('|');
                    if (ControlPoints.Length > 0)
                    {
                        for (int i = 0; i < ControlPoints.Length; i++)
                        {
                            tempCrossItem = new CrossControlItem();
                            tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(ControlPoints[i]);
                            if (null != tempPoint)
                            {
                                tempCrossItem.ControlType = "故障控制";
                                tempCrossItem.ArrPoint = tempPoint.Point;
                                tempCrossItem.PointName = tempPoint.Point + ":" + tempPoint.Wz;

                                if (tempPoint.K1 > 0 & tempPoint.K2 > 0)
                                {
                                    tempCrossItem.FeedBackPointName = tempPoint.K1.ToString().PadLeft(3, '0') + "D" + tempPoint.K2.ToString().PadLeft(2, '0') + tempPoint.K4.ToString() + "." + Model.DEFServiceModel.QueryPointByCodeCache(tempPoint.K1.ToString().PadLeft(3, '0') + "D" + tempPoint.K2.ToString().PadLeft(2, '0') + tempPoint.K4.ToString()).Wz;
                                }
                                CrossControlList.Add(tempCrossItem);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("获得交叉控制信息【getCrossInf】", ex);
            }
        }
        #endregion

        #region =============================确认信息===============================
        /// <summary>
        /// 保存测点信息的函数
        /// </summary>
        public override bool ConfirmInf(Jc_DefInfo point)
        {
            SetBoltControl();//动态设置断线控制口复用

            if (!SensorInfoverify())
            {
                return false;
            }

            if (point == null)
            {
                return false;
            }
            if (point.DevClass.Contains("甲烷"))
            {
                if (CckHiPower.Checked && string.IsNullOrEmpty(cckLocalControlHiPower.Text))
                {
                    XtraMessageBox.Show("设置了上限断电值，但是上限控制口未设置", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (CckLowPower.Checked && string.IsNullOrEmpty(cckLocalControlLowPower.Text))
                {
                    XtraMessageBox.Show("设置了下限断电值，但是下限控制口未设置", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            Jc_DevInfo temp = new Jc_DevInfo();
            //temp.LC = Convert.ToInt16(CtxbLowRange.Text);//低量程?
            temp.LC2 = Convert.ToInt16(CtxbMidRange.Text);//中间量程
            temp.LC = Convert.ToInt16(CtxbHiRange.Text);//高量程
            temp.Pl1 = Convert.ToInt16(CtxbLowHZ.Text);//低频率
            temp.Pl3 = Convert.ToInt16(CtxbMidHZ.Text);//中间频率
            temp.Pl2 = Convert.ToInt16(CtxbHiHZ.Text);//高频率
            temp.Xs1 = CcmbUint.Text;//单位

            point.Z1 = float.Parse(CtxbHiPreAlarm.Text);//上预警
            point.Z2 = float.Parse(CtxbHiAlarm.Text);//上报警
            point.Z3 = float.Parse(CtxbHiPowerOff.Text);//上断电
            point.Z4 = float.Parse(CtxbHiPowerBack.Text);//上恢复
            point.Z5 = float.Parse(CtxbLowPreAlarm.Text);//下预警
            point.Z6 = float.Parse(CtxbLowAlarm.Text);//下报警
            point.Z7 = float.Parse(CtxbLowPowerOff.Text);//下断电
            point.Z8 = float.Parse(CtxbLowPowerBack.Text);//下恢复
            point.Bz6 = CtxbCheckCycle.Text;//标校周期
            point.K1 = ConfirmCheckBoxValue(cckLocalControlHiAlarm.Text);//上报控制
            point.K2 = ConfirmCheckBoxValue(cckLocalControlHiPower.Text);//上断控制
            point.K3 = ConfirmCheckBoxValue(cckLocalControlLowAlarm.Text);//下报控制
            point.K4 = ConfirmCheckBoxValue(cckLocalControlLowPower.Text);//下断控制
            //point.K5 = ConfirmCheckBoxValue(cckLocalControlOutRange.Text);//上溢控制
            //point.K6 = ConfirmCheckBoxValue(cckLocalControlLowRange.Text);//负漂控制

            point.K7 = ConfirmCheckBoxValue(cckLocalControlHitch.Text);//异常控制
            point.K8 = 0;//报警控制标记
            if (CckHiPreAlarm.Checked) //报警控制标记
            {
                point.K8 |= 0x1; //上预警
            }
            if (CckHiAlarm.Checked)
            {
                point.K8 |= (0x1 << 1);//上报警
            }
            if (CckHiPower.Checked)
            {
                point.K8 |= (0x1 << 2);//上断电
            }
            if (CckLowPreAlarm.Checked)
            {
                point.K8 |= (0x1 << 3);//下预警
            }
            if (CckLowAlarm.Checked)
            {
                point.K8 |= (0x1 << 4);//下报警
            }
            if (CckLowPower.Checked)
            {
                point.K8 |= (0x1 << 5);//下断电
            }
            //if (CckOutRangeAlarm.Checked)
            //{
            //    point.K8 |= (0x1 << 6);//上溢
            //}
            //if (CckLowRangeAlarm.Checked)
            //{
            //    point.K8 |= (0x1 << 7);//浮漂
            //}
            if (CckHitchRangeAlarm.Checked)
            {
                point.K8 |= (0x1 << 8);//异常
            }
            #region ====================交叉控制====================
            point.Jckz1 = "";
            point.Jckz2 = "";
            point.Jckz3 = "";
            List<CrossControlItem> listTemp = new List<CrossControlItem>();
            listTemp = ToList(CrossControlDT); //将数据源DT转换成List
            if (listTemp != null)
            {
                if (listTemp.Count > 0)
                {
                    Jc_DefInfo PointTemp;
                    for (int i = 0; i < listTemp.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(listTemp[i].ArrPoint))
                        {
                            PointTemp = Model.DEFServiceModel.QueryPointByCodeCache(listTemp[i].ArrPoint);
                            if (!string.IsNullOrEmpty(PointTemp.Point))
                            {
                                if (listTemp[i].ControlType == "断电控制")
                                {
                                    point.Jckz1 += PointTemp.Fzh.ToString().PadLeft(3, '0') + "C" + PointTemp.Kh.ToString().PadLeft(2, '0')
                                        + PointTemp.Dzh.ToString() + "|";
                                }
                                else if (listTemp[i].ControlType == "断线控制")
                                {
                                    point.Jckz2 += PointTemp.Fzh.ToString().PadLeft(3, '0') + "C" + PointTemp.Kh.ToString().PadLeft(2, '0')
                                        + PointTemp.Dzh.ToString() + "|";
                                }
                                else if (listTemp[i].ControlType == "故障控制")
                                {
                                    point.Jckz3 += PointTemp.Fzh.ToString().PadLeft(3, '0') + "C" + PointTemp.Kh.ToString().PadLeft(2, '0')
                                        + PointTemp.Dzh.ToString() + "|";
                                }
                                else
                                {
                                    XtraMessageBox.Show("交叉控制中存在控制类型未选择的数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return false;
                                }
                            }
                        }
                    }
                    //处理完成后去掉最后一根“|”
                    if (point.Jckz1 != null)
                    {
                        if (point.Jckz1.Length > 0)
                        {
                            point.Jckz1 = point.Jckz1.Substring(0, point.Jckz1.Length - 1);
                        }
                    }
                    if (point.Jckz2 != null)
                    {
                        if (point.Jckz2.Length > 0)
                        {
                            point.Jckz2 = point.Jckz2.Substring(0, point.Jckz2.Length - 1);
                        }
                    }
                    if (point.Jckz3 != null)
                    {
                        if (point.Jckz3.Length > 0)
                        {
                            point.Jckz3 = point.Jckz3.Substring(0, point.Jckz3.Length - 1);
                        }
                    }
                }
            }
            #endregion

            point.Unit = CcmbUint.Text;

            point.Remark = CtxbDesc.Text;

            //分级报警
            if (checkEdit7.Checked)
            {
                if (string.IsNullOrEmpty(levelAlarm1.Text) || string.IsNullOrEmpty(levelAlarm2.Text) || string.IsNullOrEmpty(levelAlarm3.Text) || string.IsNullOrEmpty(levelAlarm4.Text))
                {
                    XtraMessageBox.Show("请输入分级报警值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (string.IsNullOrEmpty(levelAlarmTime1.Text) || string.IsNullOrEmpty(levelAlarmTime2.Text) || string.IsNullOrEmpty(levelAlarmTime3.Text))
                {
                    XtraMessageBox.Show("请输入分级报警时间", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                float vallevelAlarm1 = 0.0f;
                float vallevelAlarm2 = 0.0f;
                float vallevelAlarm3 = 0.0f;
                float vallevelAlarm4 = 0.0f;
                float.TryParse(levelAlarm1.Text, out vallevelAlarm1);
                float.TryParse(levelAlarm2.Text, out vallevelAlarm2);
                float.TryParse(levelAlarm3.Text, out vallevelAlarm3);
                float.TryParse(levelAlarm4.Text, out vallevelAlarm4);
                if (vallevelAlarm4 >= vallevelAlarm3)
                {
                    XtraMessageBox.Show("4级报警值需要小于3级报警值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (vallevelAlarm3 >= vallevelAlarm2)
                {
                    XtraMessageBox.Show("3级报警值需要小于2级报警值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (vallevelAlarm2 >= vallevelAlarm1)
                {
                    XtraMessageBox.Show("2级报警值需要小于1级报警值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                point.Bz8 = levelAlarm1.Text + "," + levelAlarm2.Text + "," + levelAlarm3.Text + "," + levelAlarm4.Text;
                point.Bz9 = levelAlarmTime1.Text + "," + levelAlarmTime2.Text + "," + levelAlarmTime3.Text + ",255";
            }
            else
            {
                //如果没启用分级报警，则默认下发报警值 
                string level1 = "65535";
                string level2 = "65535";
                string level3 = "65535";
                string level4 = "65535";

                //针对传感器特殊处理 风速只支持上限,因此，处理方式和其它传感器一样。只有氧气只支持下限需要特殊处理
                if (point.DevName.Contains("氧气"))//Bz4 == 8)//point.Bz4 == 4 ||
                {//氧气只支持下限 20190312 todo:修改成型号判断。
                    if (point.Z6 > 0)
                    {
                        level1 = point.Z6.ToString();
                        level2 = point.Z6.ToString();
                        level3 = point.Z6.ToString();
                        level4 = point.Z6.ToString();
                    }
                }
                else //if (point.DevName.Contains("风速"))
                {
                    if (point.Z2 > 0)
                    {
                        level1 = point.Z2.ToString();
                        level2 = point.Z2.ToString();
                        level3 = point.Z2.ToString();
                        level4 = point.Z2.ToString();
                    }
                }
                point.Bz8 = "" + level1 + "," + level2 + "," + level3 + "," + level4 + "";
                point.Bz9 = "255,255,255,255";
            }
            point.Bz10 = checkEdit7.Checked ? "1" : "0";
            //电压报警值
            if (voltageAlarmCheck.Checked)
            {
                int tempvoltageAlarmVal = 0;
                int.TryParse(voltageAlarmVal.Text, out tempvoltageAlarmVal);
                point.Bz5 = tempvoltageAlarmVal;
            }
            else
            {
                point.Bz5 = 0;
            }
            return true;
        }
        /// <summary>
        /// 有效性验证
        /// </summary>
        public override bool SensorInfoverify()
        {
            bool ret = false;
            if (string.IsNullOrEmpty(this.CtxbCheckCycle.Text))
            {
                XtraMessageBox.Show("请填写标校周期", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CcmbUint.Text))
            {
                XtraMessageBox.Show("请选择单位", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CtxbLowRange.Text))
            {
                XtraMessageBox.Show("请填写低量程", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CtxbLowHZ.Text))
            {
                XtraMessageBox.Show("请填写低频率", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CtxbMidRange.Text))
            {
                XtraMessageBox.Show("请填写分段量程", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CtxbMidHZ.Text))
            {
                XtraMessageBox.Show("请填写分段频率", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CtxbHiRange.Text))
            {
                XtraMessageBox.Show("请填写高量程", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CtxbHiHZ.Text))
            {
                XtraMessageBox.Show("请填写高频率", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CtxbHiPreAlarm.Text))
            {
                XtraMessageBox.Show("请填写上限预警值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CtxbHiAlarm.Text))
            {
                XtraMessageBox.Show("请填写上限报警值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CtxbHiPowerOff.Text))
            {
                XtraMessageBox.Show("请填写上限断电值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CtxbHiPowerBack.Text))
            {
                XtraMessageBox.Show("请填写上限复电值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CtxbLowPreAlarm.Text))
            {
                XtraMessageBox.Show("请填写下限预警值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CtxbLowAlarm.Text))
            {
                XtraMessageBox.Show("请填写下限报警值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CtxbLowPowerOff.Text))
            {
                XtraMessageBox.Show("请填写下限断电值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CtxbLowPowerBack.Text))
            {
                XtraMessageBox.Show("请填写下限复电值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (!Basic.Framework.Common.ValidationHelper.IsDecimal(CtxbHiPreAlarm.Text) ||
               !Basic.Framework.Common.ValidationHelper.IsDecimal(CtxbHiAlarm.Text) ||
               !Basic.Framework.Common.ValidationHelper.IsDecimal(CtxbHiPowerBack.Text) ||
               !Basic.Framework.Common.ValidationHelper.IsDecimal(CtxbHiPowerOff.Text) ||
               !Basic.Framework.Common.ValidationHelper.IsDecimal(CtxbLowPreAlarm.Text) ||
               !Basic.Framework.Common.ValidationHelper.IsDecimal(CtxbLowAlarm.Text) ||
               !Basic.Framework.Common.ValidationHelper.IsDecimal(CtxbLowPowerBack.Text) ||
               !Basic.Framework.Common.ValidationHelper.IsDecimal(CtxbLowPowerOff.Text))
            {
                XtraMessageBox.Show("上下限阈值输入不合法，只能输入小数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (float.Parse(CtxbHiPreAlarm.Text) != 0 || float.Parse(CtxbHiAlarm.Text) != 0
               || float.Parse(CtxbHiPowerBack.Text) != 0 || float.Parse(CtxbHiPowerOff.Text) != 0)
            {
                if (float.Parse(CtxbHiRange.Text) < float.Parse(CtxbHiPreAlarm.Text)
                    || float.Parse(CtxbHiRange.Text) < float.Parse(CtxbHiAlarm.Text)
                    || float.Parse(CtxbHiRange.Text) < float.Parse(CtxbHiPowerBack.Text)
                    || float.Parse(CtxbHiRange.Text) < float.Parse(CtxbHiPowerOff.Text))
                {
                    XtraMessageBox.Show("设置的上限预警、报警、断电、复电值不能超过量程！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return ret;
                }
                if (CckHiPreAlarm.Checked && CckHiAlarm.Checked)
                {
                    if (float.Parse(CtxbHiPreAlarm.Text) > float.Parse(CtxbHiAlarm.Text))
                    {
                        XtraMessageBox.Show("上限预警值需小于等于上限报警值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return ret;
                    }
                }
                if (CckHiAlarm.Checked && CckHiPower.Checked)
                {
                    if (float.Parse(CtxbHiAlarm.Text) > float.Parse(CtxbHiPowerOff.Text))
                    {
                        XtraMessageBox.Show("上限报警值需小于等于上限断电值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return ret;
                    }
                }
                if (CckHiPower.Checked)
                {
                    if (float.Parse(CtxbHiPowerBack.Text) == 0)
                    {
                        XtraMessageBox.Show("上限复电值不能设置成0", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return ret;
                    }
                    if (float.Parse(CtxbHiPowerBack.Text) > float.Parse(CtxbHiPowerOff.Text))
                    {
                        XtraMessageBox.Show("上限复电值需小于等于上限断电值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return ret;
                    }                   
                }

                if (CckHiAlarm.Checked && CckHiPower.Checked)
                {
                    if (float.Parse(CtxbHiPowerBack.Text) > float.Parse(CtxbHiAlarm.Text))
                    {
                        XtraMessageBox.Show("上限复电值需小于等于上限报警值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return ret;
                    }
                }
            }
            if (float.Parse(CtxbLowPreAlarm.Text) != 0 || float.Parse(CtxbLowAlarm.Text) != 0
               || float.Parse(CtxbLowPowerBack.Text) != 0 || float.Parse(CtxbLowPowerOff.Text) != 0)
            {
                if (CckLowPreAlarm.Checked && CckLowAlarm.Checked)
                {
                    if (float.Parse(CtxbLowPreAlarm.Text) < float.Parse(CtxbLowAlarm.Text))
                    {
                        XtraMessageBox.Show("下限预警值需大于等于下限报警值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return ret;
                    }
                }
                if (CckLowPower.Checked && CckLowAlarm.Checked)
                {
                    if (float.Parse(CtxbLowAlarm.Text) < float.Parse(CtxbLowPowerOff.Text))
                    {
                        XtraMessageBox.Show("下限报警值需大于等于下限断电值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return ret;
                    }
                }
                if (CckLowPower.Checked)
                {
                    if (float.Parse(CtxbLowPowerBack.Text) < float.Parse(CtxbLowPowerOff.Text))
                    {
                        XtraMessageBox.Show("下限复电值需大于等于下限断电值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return ret;
                    }
                    if (float.Parse(CtxbLowPowerBack.Text) == 0)
                    {
                        XtraMessageBox.Show("下限复电值不能设置成0", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return ret;
                    }
                }
                if (CckLowPower.Checked && CckLowAlarm.Checked)
                {
                    if (float.Parse(CtxbLowPowerBack.Text) < float.Parse(CtxbLowAlarm.Text))
                    {
                        XtraMessageBox.Show("下限复电值需大于等于下限报警值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return ret;
                    }
                }
            }
            
            ret = true;
            return ret;
        }
        /// <summary>
        /// 根据选择的断线复用控制口，动态设置断电控制口选择项
        /// </summary>
        public void SetBoltControl()
        {


            string BoltControlString = "";
            if (checkEdit1.Checked)
            {
                string[] tempPoint = cckLocalControlHiAlarm.Text.Split(',');
                foreach (string Point in tempPoint)
                {
                    if (Point.Length > 0 && !BoltControlString.Contains(Point))
                    {
                        BoltControlString += Point + ",";
                    }
                }
            }
            if (checkEdit2.Checked)
            {
                string[] tempPoint = cckLocalControlHiPower.Text.Split(',');
                foreach (string Point in tempPoint)
                {
                    if (Point.Length > 0 && !BoltControlString.Contains(Point))
                    {
                        BoltControlString += Point + ",";
                    }
                }
            }
            if (checkEdit3.Checked)
            {
                string[] tempPoint = cckLocalControlLowAlarm.Text.Split(',');
                foreach (string Point in tempPoint)
                {
                    if (Point.Length > 0 && !BoltControlString.Contains(Point))
                    {
                        BoltControlString += Point + ",";
                    }
                }
            }
            if (checkEdit4.Checked)
            {
                string[] tempPoint = cckLocalControlLowPower.Text.Split(',');
                foreach (string Point in tempPoint)
                {
                    if (Point.Length > 0 && !BoltControlString.Contains(Point))
                    {
                        BoltControlString += Point + ",";
                    }
                }
            }
            if (BoltControlString.Length > 0)
            {
                BoltControlString = BoltControlString.TrimEnd(',');
            }

            if (checkEdit1.Checked)
            { //智能分站，才进行复用保存
                cckLocalControlHitch.Text = BoltControlString;
            }

        }
        /// <summary>
        /// 根据加载断电控制口复用
        /// </summary>
        public void GetBoltControl()
        {



            //加载断线复的选择框
            string LocalBoltControl = cckLocalControlHitch.Text;
            //上限报警复用判断
            bool cckLocalControlHiAlarmReuse = true;
            if (cckLocalControlHiAlarm.Text.Length > 0)
            {
                foreach (string Point in cckLocalControlHiAlarm.Text.Split(','))
                {
                    if (Point.Length > 0)
                    {
                        if (!LocalBoltControl.Contains(Point))
                        {
                            cckLocalControlHiAlarmReuse = false;
                            break;
                        }
                    }
                }
            }
            else
            {
                cckLocalControlHiAlarmReuse = false;
            }
            checkEdit1.Checked = cckLocalControlHiAlarmReuse;
            //上限断电复用判断
            bool cckLocalControlHiPowerReuse = true;
            if (cckLocalControlHiPower.Text.Length > 0)
            {
                foreach (string Point in cckLocalControlHiPower.Text.Split(','))
                {
                    if (Point.Length > 0)
                    {
                        if (!LocalBoltControl.Contains(Point))
                        {
                            cckLocalControlHiPowerReuse = false;
                            break;
                        }
                    }
                }
            }
            else
            {
                cckLocalControlHiPowerReuse = false;
            }
            checkEdit2.Checked = cckLocalControlHiPowerReuse;
            //下限报警复用判断
            bool cckLocalControlLowAlarmReuse = true;
            if (cckLocalControlLowAlarm.Text.Length > 0)
            {
                foreach (string Point in cckLocalControlLowAlarm.Text.Split(','))
                {
                    if (Point.Length > 0)
                    {
                        if (!LocalBoltControl.Contains(Point))
                        {
                            cckLocalControlLowAlarmReuse = false;
                            break;
                        }
                    }
                }
            }
            else
            {
                cckLocalControlLowAlarmReuse = false;
            }
            checkEdit3.Checked = cckLocalControlLowAlarmReuse;
            //下限报警复用判断
            bool cckLocalControlLowPowerReuse = true;
            if (cckLocalControlLowAlarm.Text.Length > 0)
            {
                foreach (string Point in cckLocalControlLowPower.Text.Split(','))
                {
                    if (Point.Length > 0)
                    {
                        if (!LocalBoltControl.Contains(Point))
                        {
                            cckLocalControlLowPowerReuse = false;
                            break;
                        }
                    }
                }
            }
            else
            {
                cckLocalControlLowPowerReuse = false;
            }
            checkEdit4.Checked = cckLocalControlLowPowerReuse;
        }
        #endregion

        #region  =============================触发事件===============================
        /// <summary>
        /// 设备类型变化产生的影响
        /// </summary>
        public override void DevTypeChanngeEvent(long DevID, Jc_DefInfo Point)
        {
            try
            {
                Jc_DefInfo tempStation = Model.DEFServiceModel.QueryPointByFzhCache((int)_SourceNum).Find(a => a.Kh == 0);
                Jc_DevInfo tempStationDev = Model.DEVServiceModel.QueryDevByDevIDCache(tempStation.Devid);
                SettingInfo settingInfo = CONFIGServiceModel.GetConfigFKey("IsDisconnectControlReuse");
                string isDisconnectControlReuse = "0";
                //if (settingInfo != null)
                //{
                //    isDisconnectControlReuse = settingInfo.StrValue;
                //}

                //string[] paraArr = paras.Split(',');

                if (tempStationDev != null && tempStationDev.LC2 == 13)
                { //智能分站
                    if (isDisconnectControlReuse == "1")
                    {
                        //layoutControlItem35.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        //layoutControlItem36.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        //layoutControlItem37.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        //layoutControlItem38.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        //layoutControlItem33.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        //初始化复用选项
                        checkEdit1.Checked = false;
                        checkEdit2.Checked = false;
                        checkEdit3.Checked = false;
                        checkEdit4.Checked = false;
                    }
                    else
                    {
                        //layoutControlItem35.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        //layoutControlItem36.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        //layoutControlItem37.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        //layoutControlItem38.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        //layoutControlItem33.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    }
                }
                else
                { //非智能分站
                    //layoutControlItem35.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    //layoutControlItem36.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    //layoutControlItem37.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    //layoutControlItem38.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    //layoutControlItem33.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                }

                if (Point != null)
                {
                    //if ((Point.K8 & 0x1) == 0x1)//是否上限预警
                    //{
                    //    CckHiPreAlarm.Checked = true;
                    //}
                    //else
                    //{
                    //    CckHiPreAlarm.Checked = false;
                    //}
                    //if (((Point.K8 >> 1) & 0x1) == 0x1)//是否上限报警
                    //{
                    //    CckHiAlarm.Checked = true;
                    //}
                    //else
                    //{
                    //    CckHiAlarm.Checked = false;
                    //}
                    //if (((Point.K8 >> 2) & 0x1) == 0x1)//是否上限断电
                    //{
                    //    CckHiPower.Checked = true;
                    //}
                    //else
                    //{
                    //    CckHiPower.Checked = false;
                    //}
                    //if (((Point.K8 >> 3) & 0x1) == 0x1)//是否下限预警
                    //{
                    //    CckLowPreAlarm.Checked = true;
                    //}
                    //else
                    //{
                    //    CckLowPreAlarm.Checked = false;
                    //}
                    //if (((Point.K8 >> 4) & 0x1) == 0x1)//是否下限报警
                    //{
                    //    CckLowAlarm.Checked = true;
                    //}
                    //else
                    //{
                    //    CckLowAlarm.Checked = false;
                    //}
                    //if (((Point.K8 >> 5) & 0x1) == 0x1)//是否下限断电
                    //{
                    //    CckLowPower.Checked = true;
                    //}
                    //else
                    //{
                    //    CckLowPower.Checked = false;
                    //}
                    //if (((Point.K8 >> 6) & 0x1) == 0x1)//是否上溢
                    //{
                    //    CckOutRangeAlarm.Checked = true;
                    //}
                    //else
                    //{
                    //    CckOutRangeAlarm.Checked = false;
                    //}
                    //if (((Point.K8 >> 7) & 0x1) == 0x1)//是否负漂
                    //{
                    //    CckLowRangeAlarm.Checked = true;
                    //}
                    //else
                    //{
                    //    CckLowRangeAlarm.Checked = false;
                    //}
                    //if (((Point.K8 >> 8) & 0x1) == 0x1)//是否断线
                    //{
                    //    CckHitchRangeAlarm.Checked = true;
                    //}
                    //else
                    //{
                    //    CckHitchRangeAlarm.Checked = false;
                    //}

                    //CtxbDesc.Text = Point.Remark;

                    //cckLocalControlHiAlarm.Text = SetLocalControlText(Point.K1);//上限报警控制口
                    //cckLocalControlHiPower.Text = SetLocalControlText(Point.K2);//上限断电控制口
                    //cckLocalControlLowAlarm.Text = SetLocalControlText(Point.K3);//下限报警控制口
                    //cckLocalControlLowPower.Text = SetLocalControlText(Point.K4);//下限断电控制口
                    //cckLocalControlOutRange.Text = SetLocalControlText(Point.K5);//上溢控制口
                    //cckLocalControlLowRange.Text = SetLocalControlText(Point.K6);//负漂控制口
                    //cckLocalControlHitch.Text = SetLocalControlText(Point.K7);//断线控制口

                    //GetBoltControl();//加载断电复用

                    //#region ==================交叉控制==================
                    //getCrossInf(Point.Point);
                    //CrossControlDT = ToDataTable(CrossControlList);
                    //CdgControl.DataSource = CrossControlDT;
                    //CdgControl.RefreshDataSource();
                    //#endregion

                    //CtxbHiPreAlarm.Text = Point.Z1.ToString();//上限预警值
                    //CtxbHiAlarm.Text = Point.Z2.ToString();//上限报警值
                    //CtxbHiPowerOff.Text = Point.Z3.ToString();//上限断电值
                    //CtxbHiPowerBack.Text = Point.Z4.ToString();//上限复电值
                    //CtxbLowPreAlarm.Text = Point.Z5.ToString();//下限预警值
                    //CtxbLowAlarm.Text = Point.Z6.ToString();//下限报警值
                    //CtxbLowPowerOff.Text = Point.Z7.ToString();//下限断电值
                    //CtxbLowPowerBack.Text = Point.Z8.ToString();//下限复电值     

                    Jc_DevInfo tempDev = Model.DEVServiceModel.QueryDevByDevIDCache(Point.Devid);
                    LoadPointInfo(Point, tempDev);
                }
                else
                {//取jc_dev的定义信息

                    ////默认全部可以选择  20170627
                    //CckHiPreAlarm.Checked = true; //是否上限预警
                    //CckHiAlarm.Checked = true; //是否上限报警
                    //CckHiPower.Checked = true;  //是否上限断电
                    //CckLowPreAlarm.Checked = true;//是否下限预警
                    //CckLowAlarm.Checked = true; //是否下限报警
                    //CckLowPower.Checked = true;//是否下限断电
                    ////CckOutRangeAlarm.Checked = true; //是否上溢
                    ////CckLowRangeAlarm.Checked = true; //是否负漂
                    //CckHitchRangeAlarm.Checked = true;//是否断线

                    CtxbDesc.Text = "";

                    cckLocalControlHiAlarm.Text = "";//上限报警控制口
                    cckLocalControlHiPower.Text = "";//上限断电控制口
                    cckLocalControlLowAlarm.Text = "";//下限报警控制口
                    cckLocalControlLowPower.Text = "";//下限断电控制口
                    //cckLocalControlOutRange.Text = "";//上溢控制口
                    //cckLocalControlLowRange.Text = "";//负漂控制口
                    cckLocalControlHitch.Text = "";//断线控制口

                    #region ==================交叉控制==================
                    CrossControlList.Clear();
                    CrossControlDT = ToDataTable(CrossControlList);
                    CdgControl.DataSource = CrossControlDT;
                    CdgControl.RefreshDataSource();
                    #endregion


                    Jc_DevInfo temp = Model.DEVServiceModel.QueryDevByDevIDCache(DevID.ToString());
                    if (null == temp)
                    {
                        return;
                    }
                    #region ==================扩展属性==================
                    CtxbCheckCycle.Text = "7";//标校周期 ? 
                    CcmbUint.SelectedItem = temp.Xs1;//单位
                    CtxbLowRange.Text = "0";//低量程
                    CtxbMidRange.Text = temp.LC2.ToString();//中间量程
                    CtxbHiRange.Text = temp.LC.ToString();//高量程
                    CtxbLowHZ.Text = temp.Pl1.ToString();//低频率
                    CtxbMidHZ.Text = temp.Pl3.ToString();//中间频率
                    CtxbHiHZ.Text = temp.Pl2.ToString();//高频率
                    #endregion


                    #region ==================报警属性==================
                    if (checkEdit5.Checked)
                    {
                        CtxbHiPreAlarm.Text = temp.Z1.ToString();//上限预警值
                        CtxbHiAlarm.Text = temp.Z2.ToString();//上限报警值
                        CtxbHiPowerOff.Text = temp.Z3.ToString();//上限断电值
                        CtxbHiPowerBack.Text = temp.Z4.ToString();//上限复电值
                    }
                    if (checkEdit6.Checked)
                    {
                        CtxbLowPreAlarm.Text = temp.Z5.ToString();//下限预警值
                        CtxbLowAlarm.Text = temp.Z6.ToString();//下限报警值
                        CtxbLowPowerOff.Text = temp.Z7.ToString();//下限断电值
                        CtxbLowPowerBack.Text = temp.Z8.ToString();//下限复电值
                    }
                    #endregion

                }

                //int tempZ1 = 0, tempZ2 = 0, tempZ3 = 0, tempZ4 = 0, tempZ5 = 0, tempZ6 = 0, tempZ7 = 0, tempZ8 = 0;
                //if (paraArr.Length >= 8)
                //{
                //    int.TryParse(paraArr[0], out tempZ1);
                //    int.TryParse(paraArr[1], out tempZ2);
                //    int.TryParse(paraArr[2], out tempZ3);
                //    int.TryParse(paraArr[3], out tempZ4);
                //    int.TryParse(paraArr[4], out tempZ5);
                //    int.TryParse(paraArr[5], out tempZ6);
                //    int.TryParse(paraArr[6], out tempZ7);
                //    int.TryParse(paraArr[7], out tempZ8);
                //}

                ////默认都可以设置  20170627
                //if (tempZ1 > 0 || tempZ2 > 0 || tempZ3 > 0 || tempZ4 > 0)
                //{
                //    checkEdit5.Checked = true;
                //    CckHiPreAlarm.Checked = true;
                //    CckHiAlarm.Checked = true;
                //    CckHiPower.Checked = true;
                //    CckHiPreAlarm.Enabled = true;
                //    CckHiAlarm.Enabled = true;
                //    CckHiPower.Enabled = true;
                //}
                //else
                //{
                //    checkEdit5.Checked = false;
                //    CckHiPreAlarm.Checked = false;
                //    CckHiAlarm.Checked = false;
                //    CckHiPower.Checked = false;
                //    CckHiPreAlarm.Enabled = false;
                //    CckHiAlarm.Enabled = false;
                //    CckHiPower.Enabled = false;
                //}
                //if (tempZ5 > 0 || tempZ6 > 0 || tempZ7 > 0 || tempZ8 > 0)
                //{
                //    checkEdit6.Checked = true;
                //    CckLowPreAlarm.Checked = true;
                //    CckLowAlarm.Checked = true;
                //    CckLowPower.Checked = true;
                //    CckLowPreAlarm.Enabled = true;
                //    CckLowAlarm.Enabled = true;
                //    CckLowPower.Enabled = true;
                //}
                //else
                //{
                //    checkEdit6.Checked = false;
                //    CckLowPreAlarm.Checked = false;
                //    CckLowAlarm.Checked = false;
                //    CckLowPower.Checked = false;
                //    CckLowPreAlarm.Enabled = false;
                //    CckLowAlarm.Enabled = false;
                //    CckLowPower.Enabled = false;
                //}

                //CtxbHiPreAlarm.Text = tempZ1.ToString();//上限预警值
                //CtxbHiAlarm.Text = tempZ2.ToString();//上限报警值
                //CtxbHiPowerOff.Text = tempZ3.ToString();//上限断电值
                //CtxbHiPowerBack.Text = tempZ4.ToString();//上限复电值
                //CtxbLowPreAlarm.Text = tempZ5.ToString();//下限预警值
                //CtxbLowAlarm.Text = tempZ6.ToString();//下限报警值
                //CtxbLowPowerOff.Text = tempZ7.ToString();//下限断电值
                //CtxbLowPowerBack.Text = tempZ8.ToString();//下限复电值

            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private void LoadPointInfo(Jc_DefInfo temp, Jc_DevInfo tempDev)
        {
            #region ==================扩展属性==================
            if (!string.IsNullOrEmpty(temp.Bz6))
            {
                CtxbCheckCycle.Text = temp.Bz6.ToString();//标校周期
            }
            CcmbUint.SelectedItem = tempDev.Xs1.ToString();//单位
            CtxbLowRange.Text = "0";//低量程?
            CtxbMidRange.Text = tempDev.LC2.ToString();//中间量程
            CtxbHiRange.Text = tempDev.LC.ToString();//高量程
            CtxbLowHZ.Text = tempDev.Pl1.ToString();//低频率
            CtxbMidHZ.Text = tempDev.Pl3.ToString();//中间频率
            CtxbHiHZ.Text = tempDev.Pl2.ToString();//高频率
            CtxbDesc.Text = temp.Remark;//描述
            #endregion

            if (temp.Z1 > 0 || temp.Z2 > 0 || temp.Z3 > 0)
            {
                checkEdit5.Checked = true;
                //CckHiPreAlarm.Checked = true;
                //CckHiAlarm.Checked = true;
                //CckHiPower.Checked = true;
                //CckHiPreAlarm.Enabled = true;
                //CckHiAlarm.Enabled = true;
                //CckHiPower.Enabled = true;
            }
            else
            {
                checkEdit5.Checked = false;
                //CckHiPreAlarm.Checked = false;
                //CckHiAlarm.Checked = false;
                //CckHiPower.Checked = false;
                //CckHiPreAlarm.Enabled = false;
                //CckHiAlarm.Enabled = false;
                //CckHiPower.Enabled = false;
            }
            if (temp.Z5 > 0 || temp.Z6 > 0 || temp.Z7 > 0)
            {
                checkEdit6.Checked = true;
                //CckLowPreAlarm.Checked = true;
                //CckLowAlarm.Checked = true;
                //CckLowPower.Checked = true;
                //CckLowPreAlarm.Enabled = true;
                //CckLowAlarm.Enabled = true;
                //CckLowPower.Enabled = true;
            }
            else
            {
                checkEdit6.Checked = false;
                //CckLowPreAlarm.Checked = false;
                //CckLowAlarm.Checked = false;
                //CckLowPower.Checked = false;
                //CckLowPreAlarm.Enabled = false;
                //CckLowAlarm.Enabled = false;
                //CckLowPower.Enabled = false;
            }
            #region ==================报警控制==================

            if ((temp.K8 & 0x1) == 0x1)//是否上限预警
            {
                CckHiPreAlarm.Checked = true;
            }
            else
            {
                CckHiPreAlarm.Checked = false;
            }
            if (((temp.K8 >> 1) & 0x1) == 0x1)//是否上限报警
            {
                CckHiAlarm.Checked = true;
            }
            else
            {
                CckHiAlarm.Checked = false;
            }
            if (((temp.K8 >> 2) & 0x1) == 0x1)//是否上限断电
            {
                CckHiPower.Checked = true;
            }
            else
            {
                CckHiPower.Checked = false;
            }
            if (((temp.K8 >> 3) & 0x1) == 0x1)//是否下限预警
            {
                CckLowPreAlarm.Checked = true;
            }
            else
            {
                CckLowPreAlarm.Checked = false;
            }
            if (((temp.K8 >> 4) & 0x1) == 0x1)//是否下限报警
            {
                CckLowAlarm.Checked = true;
            }
            else
            {
                CckLowAlarm.Checked = false;
            }
            if (((temp.K8 >> 5) & 0x1) == 0x1)//是否下限断电
            {
                CckLowPower.Checked = true;
            }
            else
            {
                CckLowPower.Checked = false;
            }

            if (((temp.K8 >> 8) & 0x1) == 0x1)//是否断线
            {
                CckHitchRangeAlarm.Checked = true;
            }
            else
            {
                CckHitchRangeAlarm.Checked = false;
            }
            //默认都可以设置  20170627           
            //CckHiPreAlarm.Checked = true;
            //CckHiAlarm.Checked = true;
            //CckHiPower.Checked = true;
            //CckLowPreAlarm.Checked = true;
            //CckLowAlarm.Checked = true;
            //CckLowPower.Checked = true;
            //CckOutRangeAlarm.Checked = true;
            //CckLowRangeAlarm.Checked = true;
            //CckHitchRangeAlarm.Checked = true;


            CtxbHiPreAlarm.Text = temp.Z1.ToString();//上限预警值
            CtxbHiAlarm.Text = temp.Z2.ToString();//上限报警值
            CtxbHiPowerOff.Text = temp.Z3.ToString();//上限断电值
            CtxbHiPowerBack.Text = temp.Z4.ToString();//上限复电值
            CtxbLowPreAlarm.Text = temp.Z5.ToString();//下限预警值
            CtxbLowAlarm.Text = temp.Z6.ToString();//下限报警值
            CtxbLowPowerOff.Text = temp.Z7.ToString();//下限断电值
            CtxbLowPowerBack.Text = temp.Z8.ToString();//下限复电值

            cckLocalControlHiAlarm.Text = SetLocalControlText(temp.K1);//上限报警控制口
            cckLocalControlHiPower.Text = SetLocalControlText(temp.K2);//上限断电控制口
            cckLocalControlLowAlarm.Text = SetLocalControlText(temp.K3);//下限报警控制口
            cckLocalControlLowPower.Text = SetLocalControlText(temp.K4);//下限断电控制口
            //cckLocalControlOutRange.Text = SetLocalControlText(temp.K5);//上溢控制口
            //cckLocalControlLowRange.Text = SetLocalControlText(temp.K6);//负漂控制口
            cckLocalControlHitch.Text = SetLocalControlText(temp.K7);//断线控制口          

            GetBoltControl();//加载断电复用
            #endregion

            #region ==================交叉控制==================
            getCrossInf(temp.Point);
            CrossControlDT = ToDataTable(CrossControlList);
            CdgControl.DataSource = CrossControlDT;
            CdgControl.RefreshDataSource();
            #endregion


            if (!string.IsNullOrEmpty(temp.Bz10) && temp.Bz10 == "1")
            {
                checkEdit7.Checked = true;
            }
            else
            {
                checkEdit7.Checked = false;
            }
            //分级报警
            string[] levelAlarmArr = temp.Bz8.Split(',');
            if (levelAlarmArr.Length == 4)
            {
                if (levelAlarmArr[0] == "65535")
                {
                    //checkEdit7.Checked = false;
                    levelAlarm4.Text = "";
                    levelAlarm3.Text = "";
                    levelAlarm2.Text = "";
                    levelAlarm1.Text = "";
                }
                else
                {
                    //checkEdit7.Checked = true;
                    levelAlarm1.Text = levelAlarmArr[0];
                    levelAlarm2.Text = levelAlarmArr[1];
                    levelAlarm3.Text = levelAlarmArr[2];
                    levelAlarm4.Text = levelAlarmArr[3];
                }
            }
            else
            {
                //checkEdit7.Checked = false;
                levelAlarm4.Text = "";
                levelAlarm3.Text = "";
                levelAlarm2.Text = "";
                levelAlarm1.Text = "";
            }
            string[] levelAlarmTimeArr = temp.Bz9.Split(',');
            if (levelAlarmTimeArr.Length == 4)
            {
                if (levelAlarmTimeArr[0] == "255")
                {
                    //checkEdit7.Checked = false;
                    levelAlarmTime1.Text = "";
                    levelAlarmTime2.Text = "";
                    levelAlarmTime3.Text = "";
                    levelAlarmTime4.Text = "0";
                }
                else
                {
                    //checkEdit7.Checked = true;
                    levelAlarmTime1.Text = levelAlarmTimeArr[0];
                    levelAlarmTime2.Text = levelAlarmTimeArr[1];
                    levelAlarmTime3.Text = levelAlarmTimeArr[2];
                    levelAlarmTime4.Text = "0";
                }
            }
            else
            {
                //checkEdit7.Checked = false;
                levelAlarmTime4.Text = "0";
                levelAlarmTime3.Text = "";
                levelAlarmTime2.Text = "";
                levelAlarmTime1.Text = "";
            }

            //电压报警值
            if (temp.Bz5 > 0)
            {
                voltageAlarmCheck.Checked = true;
                voltageAlarmVal.Text = temp.Bz5.ToString();
            }
            else
            {
                voltageAlarmCheck.Checked = false;
            }
        }

        private void CckHiPower_CheckedChanged(object sender, EventArgs e)
        {
            if (true == CckHiPower.Checked)
            {

                CtxbHiPowerOff.Enabled = true;
                CtxbHiPowerBack.Enabled = true;
                cckLocalControlHiPower.Enabled = true;
                checkEdit2.Enabled = true;
            }
            else
            {
                CtxbHiPowerOff.Enabled = false;
                CtxbHiPowerBack.Enabled = false;
                cckLocalControlHiPower.Enabled = false;
                checkEdit2.Enabled = false;
                CtxbHiPowerOff.Text = "0";
                CtxbHiPowerBack.Text = "0";
            }
        }

        private void CckHiPreAlarm_CheckedChanged(object sender, EventArgs e)
        {
            if (true == CckHiPreAlarm.Checked)
            {
                CtxbHiPreAlarm.Enabled = true;
            }
            else
            {
                CtxbHiPreAlarm.Enabled = false;
                CtxbHiPreAlarm.Text = "0";
            }
        }

        private void CckHiAlarm_CheckedChanged(object sender, EventArgs e)
        {
            if (true == CckHiAlarm.Checked)
            {
                CtxbHiAlarm.Enabled = true;
                cckLocalControlHiAlarm.Enabled = true;
                checkEdit1.Enabled = true;
            }
            else
            {
                CtxbHiAlarm.Enabled = false;
                cckLocalControlHiAlarm.Enabled = false;
                checkEdit1.Enabled = false;
                CtxbHiAlarm.Text = "0";
            }
        }

        private void CckLowPreAlarm_CheckedChanged(object sender, EventArgs e)
        {
            if (true == CckLowPreAlarm.Checked)
            {
                CtxbLowPreAlarm.Enabled = true;
            }
            else
            {
                CtxbLowPreAlarm.Enabled = false;
                CtxbLowPreAlarm.Text = "0";
            }
        }

        private void CckLowAlarm_CheckedChanged(object sender, EventArgs e)
        {
            if (true == CckLowAlarm.Checked)
            {
                CtxbLowAlarm.Enabled = true;
                cckLocalControlLowAlarm.Enabled = true;
                checkEdit3.Enabled = true;
            }
            else
            {
                CtxbLowAlarm.Enabled = false;
                cckLocalControlLowAlarm.Enabled = false;
                checkEdit3.Enabled = false;
                CtxbLowAlarm.Text = "0";
            }
        }

        private void CckLowPower_CheckedChanged(object sender, EventArgs e)
        {
            if (true == CckLowPower.Checked)
            {
                CtxbLowPowerOff.Enabled = true;
                CtxbLowPowerBack.Enabled = true;
                cckLocalControlLowPower.Enabled = true;
                checkEdit4.Enabled = true;
            }
            else
            {
                CtxbLowPowerOff.Enabled = false;
                CtxbLowPowerBack.Enabled = false;
                cckLocalControlLowPower.Enabled = false;
                checkEdit4.Enabled = false;
                CtxbLowPowerOff.Text = "0";
                CtxbLowPowerBack.Text = "0";
            }
        }

        private void CckOutRangeAlarm_CheckedChanged(object sender, EventArgs e)
        {
            //if (true == CckOutRangeAlarm.Checked)
            //{
            //    cckLocalControlOutRange.Enabled = true;
            //}
            //else
            //{
            //    cckLocalControlOutRange.Enabled = false;
            //}
        }

        private void CckLowRangeAlarm_CheckedChanged(object sender, EventArgs e)
        {
            //if (true == CckLowRangeAlarm.Checked)
            //{
            //    cckLocalControlLowRange.Enabled = true;
            //}
            //else
            //{
            //    cckLocalControlLowRange.Enabled = false;
            //}
        }

        private void CckHitchRangeAlarm_CheckedChanged(object sender, EventArgs e)
        {
            if (true == CckHitchRangeAlarm.Checked)
            {
                cckLocalControlHitch.Enabled = true;
            }
            else
            {
                cckLocalControlHitch.Enabled = false;
            }
        }

        private void CdvCrossControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && this.CdvCrossControl.FocusedRowHandle >= 0)
            {
                if (XtraMessageBox.Show("删除不可恢复,是否确定删除？", "删除确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    CdvCrossControl.DeleteRow(CdvCrossControl.FocusedRowHandle);
                }
            }
        }
        /// <summary>
        /// 自动加载馈电测点号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CdvCrossControl_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (null == e.Value)
            {
                return;
            }
            if (e.Column.FieldName != "ArrPoint")
            {
                return;
            }
            Jc_DefInfo tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(e.Value.ToString());
            if (tempPoint == null)
            {
                return;
            }
            if (tempPoint.K1 > 0 && tempPoint.K2 > 0)
            {
                Jc_DefInfo tempFeedBackPoint = Model.DEFServiceModel.QueryPointByCodeCache(tempPoint.K1.ToString().PadLeft(3, '0') + "D" + tempPoint.K2.ToString().PadLeft(2, '0') + tempPoint.K4.ToString());
                if (tempFeedBackPoint == null)
                {
                    return;
                }
                if (!string.IsNullOrEmpty(tempFeedBackPoint.Point))
                {
                    CdvCrossControl.SetRowCellValue(e.RowHandle, "FeedBackPointName", tempFeedBackPoint.Point + "." + tempFeedBackPoint.Wz); //自动生成反馈测点
                }
            }



        }


        private void repositoryItemButtonEdit1_Click(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show("删除不可恢复,是否确定删除？", "删除确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                CdvCrossControl.DeleteRow(CdvCrossControl.FocusedRowHandle);
            }
        }
        private void CdvCrossControl_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = e.RowHandle.ToString();
            }
        }

        /// <summary>
        /// 验证行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CdvCrossControl_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            for (int i = 0; i < CdvCrossControl.RowCount; i++)
            {
                if (e.RowHandle != i || e.RowHandle == -2147483647)
                {
                    if (CdvCrossControl.GetRowCellValue(i, "ArrPoint").ToString() == CdvCrossControl.GetRowCellValue(e.RowHandle, "ArrPoint").ToString() && CdvCrossControl.GetRowCellValue(i, "ControlType").ToString() == CdvCrossControl.GetRowCellValue(e.RowHandle, "ControlType").ToString() && CdvCrossControl.GetRowCellValue(i, "FeedBackPointName").ToString() == CdvCrossControl.GetRowCellValue(e.RowHandle, "FeedBackPointName").ToString())
                    {
                        XtraMessageBox.Show("存在重复定义！", "消息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Valid = false;
                        return;
                    }
                }
            }
        }
        #endregion

        #region =============================业务函数===============================
        /// <summary>
        /// 将值对象列表转换为DataTable
        /// 如果vos为空,则返回空
        /// </summary>
        /// <param name="voList"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(IList<T> vos)
        {
            Type voType = typeof(T);
            //构造数据表
            DataTable dt = new DataTable(voType.Name);
            PropertyInfo[] properties = voType.GetProperties();
            IDictionary<string, PropertyInfo> voProperties = new Dictionary<string, PropertyInfo>();
            //构造数据列
            foreach (PropertyInfo property in properties)
            {
                DataColumn col = new DataColumn(property.Name);
                col.DataType = property.PropertyType;
                col.Caption = property.Name;
                dt.Columns.Add(col);
                voProperties.Add(property.Name, property);
            }
            if (vos == null || vos.Count == 0)
            {
                return dt;
            }
            //读取记录数据
            foreach (object obj in vos)
            {
                DataRow dr = dt.NewRow();
                foreach (PropertyInfo pro in voProperties.Values)
                {
                    dr[pro.Name] = pro.GetValue(obj, null);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        /// <summary>
        /// 将DT转换为List
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<CrossControlItem> ToList(DataTable dt)
        {
            List<CrossControlItem> ret = new List<CrossControlItem>();
            if (null != dt)
            {
                CrossControlItem temp;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    temp = new CrossControlItem();
                    temp.ArrPoint = dt.Rows[i]["ArrPoint"].ToString();
                    temp.ControlType = dt.Rows[i]["ControlType"].ToString();
                    temp.FeedBackPointName = dt.Rows[i]["FeedBackPointName"].ToString();
                    temp.DelInfBtnStr = "删除";
                    ret.Add(temp);
                }
            }
            return ret;
        }
        /// <summary>
        /// 设置本地控制信息
        /// </summary>
        /// <param name="K"></param>
        /// <returns></returns>
        private string SetLocalControlText(int K)
        {
            string temp = "";
            if (K > 0)
            {
                for (int i = 0; i < 24; i++)
                {
                    if (((K >> i) & 0x1) == 0x1)
                    {
                        if (i < 8)
                        {

                            temp += _SourceNum.ToString().PadLeft(3, '0') + "C" + (i + 1).ToString().PadLeft(2, '0') + "0" + ",";

                        }
                        else
                        {
                            temp += _SourceNum.ToString().PadLeft(3, '0') + "C" + (i - 7).ToString().PadLeft(2, '0') + "1" + ",";
                        }
                    }
                }
                if (temp.Contains(","))
                {
                    temp = temp.Substring(0, temp.Length - 1);
                }
            }
            return temp;
        }
        /// <summary>
        /// 获取控制信息
        /// </summary>
        /// <param name="LocalControlText"></param>
        /// <returns></returns>
        private int ConfirmCheckBoxValue(string LocalControlText)
        {
            int temp = 0;
            if (string.IsNullOrEmpty(LocalControlText))
            {
                return temp;
            }
            string[] tempArray = LocalControlText.Split(',');
            if (tempArray.Length <= 0)
            {
                return temp;
            }
            int Channel = 0;
            for (int i = 0; i < tempArray.Length; i++)
            {
                Channel = 0;
                if (tempArray[i].Trim().Length >= 6)
                {
                    if (tempArray[i].Trim().Length == 6)
                    {
                        Channel = Convert.ToInt32(tempArray[i].Trim().Substring(4, 2));
                    }
                    else
                    {
                        if (tempArray[i].Trim()[6] == '0')
                        {
                            Channel = Convert.ToInt32(tempArray[i].Trim().Substring(4, 2));
                        }
                        else
                        {
                            Channel = Convert.ToInt32(tempArray[i].Trim().Substring(4, 2)) + 8;
                        }
                    }

                }
                if (Channel > 0)
                {
                    temp += 1 << (Channel - 1);
                }
            }
            return temp;
        }


        #endregion

        private void checkEdit5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit5.Checked)
            {
                CckHiPreAlarm.Enabled = true;
                CtxbHiPreAlarm.Enabled = true;
                CckHiAlarm.Enabled = true;
                CtxbHiAlarm.Enabled = true;
                CckHiPower.Enabled = true;
                CtxbHiPowerOff.Enabled = true;
                CtxbHiPowerBack.Enabled = true;
                cckLocalControlHiPower.Enabled = true;
                cckLocalControlHiAlarm.Enabled = true;
                CckHiPreAlarm.Checked = true;
                CckHiAlarm.Checked = true;
                CckHiPower.Checked = true;
            }
            else
            {
                //if (XtraMessageBox.Show("取消上限后，会将所有上限预、报、断电值恢复到0，确定取消吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //{
                CckHiPreAlarm.Enabled = false;
                CtxbHiPreAlarm.Enabled = false;
                CckHiAlarm.Enabled = false;
                CtxbHiAlarm.Enabled = false;
                CckHiPower.Enabled = false;
                CtxbHiPowerOff.Enabled = false;
                CtxbHiPowerBack.Enabled = false;
                cckLocalControlHiPower.Enabled = false;
                cckLocalControlHiAlarm.Enabled = false;
                CckHiPreAlarm.Checked = false;
                CckHiAlarm.Checked = false;
                CckHiPower.Checked = false;
                //}
                //else
                //{
                //    checkEdit5.Checked = true;
                //}
            }
        }

        private void checkEdit6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit6.Checked)
            {
                CckLowPreAlarm.Enabled = true;
                CtxbLowPreAlarm.Enabled = true;
                CckLowAlarm.Enabled = true;
                CtxbLowAlarm.Enabled = true;
                CckLowPower.Enabled = true;
                CtxbLowPowerOff.Enabled = true;
                CtxbLowPowerBack.Enabled = true;
                cckLocalControlLowPower.Enabled = true;
                cckLocalControlLowAlarm.Enabled = true;
                CckLowPreAlarm.Checked = true;
                CckLowAlarm.Checked = true;
                CckLowPower.Checked = true;
            }
            else
            {
                //if (XtraMessageBox.Show("取消下限后，会将所有下限预、报、断电值恢复到0，确定取消吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //{
                CckLowPreAlarm.Enabled = false;
                CtxbLowPreAlarm.Enabled = false;
                CckLowAlarm.Enabled = false;
                CtxbLowAlarm.Enabled = false;
                CckLowPower.Enabled = false;
                CtxbLowPowerOff.Enabled = false;
                CtxbLowPowerBack.Enabled = false;
                cckLocalControlLowPower.Enabled = false;
                cckLocalControlLowAlarm.Enabled = false;
                CckLowPreAlarm.Checked = false;
                CckLowAlarm.Checked = false;
                CckLowPower.Checked = false;
                //}
                //else
                //{
                //    checkEdit6.Checked = true;
                //}
            }
        }

        private void checkEdit7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit7.Checked)
            {
                levelAlarm4.Enabled = true;
                levelAlarm3.Enabled = true;
                levelAlarm2.Enabled = true;
                levelAlarm1.Enabled = true;
                //levelAlarmTime4.Enabled = true;
                levelAlarmTime3.Enabled = true;
                levelAlarmTime2.Enabled = true;
                levelAlarmTime1.Enabled = true;
                float hiAlarm = 0;
                float.TryParse(CtxbHiAlarm.Text, out hiAlarm);
                if (hiAlarm > 0)
                {
                    levelAlarm4.Text = Math.Round(hiAlarm / 2.5, 2).ToString();
                    levelAlarm3.Text = Math.Round(hiAlarm / 2.5 * 1.5, 2).ToString();
                    levelAlarm2.Text = Math.Round(hiAlarm / 2.5 * 2, 2).ToString();
                    levelAlarm1.Text = Math.Round(hiAlarm, 2).ToString();

                    levelAlarmTime4.Text = "0";
                    levelAlarmTime3.Text = "1";
                    levelAlarmTime2.Text = "30";
                    levelAlarmTime1.Text = "60";
                }
                else
                {
                    levelAlarmTime4.Text = "0";
                    levelAlarmTime3.Text = "1";
                    levelAlarmTime2.Text = "30";
                    levelAlarmTime1.Text = "60";
                }
            }
            else
            {
                levelAlarm4.Enabled = false;
                levelAlarm3.Enabled = false;
                levelAlarm2.Enabled = false;
                levelAlarm1.Enabled = false;
                //levelAlarmTime4.Enabled = false;
                levelAlarmTime3.Enabled = false;
                levelAlarmTime2.Enabled = false;
                levelAlarmTime1.Enabled = false;

                levelAlarm4.Text = "";
                levelAlarm3.Text = "";
                levelAlarm2.Text = "";
                levelAlarm1.Text = "";

                levelAlarmTime4.Text = "0";
                levelAlarmTime3.Text = "";
                levelAlarmTime2.Text = "";
                levelAlarmTime1.Text = "";
            }
        }

        private void CtxbHiAlarm_EditValueChanged(object sender, EventArgs e)
        {
            //if (CtxbLowAlarm.Enabled)
            //{
            //    levelAlarm3.Text = CtxbHiAlarm.Text;
            //    levelAlarm1.Text = CtxbLowAlarm.Text;
            //}
            //else
            //{
            //    levelAlarm1.Text = CtxbHiAlarm.Text;
            //}
            if (checkEdit7.Checked)
            {
                float hiAlarm = 0;
                float.TryParse(CtxbHiAlarm.Text, out hiAlarm);
                if (hiAlarm > 0)
                {
                    levelAlarm4.Text = Math.Round(hiAlarm / 2.5, 2).ToString();
                    levelAlarm3.Text = Math.Round(hiAlarm / 2.5 * 1.5, 2).ToString();
                    levelAlarm2.Text = Math.Round(hiAlarm / 2.5 * 2, 2).ToString();
                    levelAlarm1.Text = Math.Round(hiAlarm, 2).ToString();
                }
            }
        }

        private void CtxbLowAlarm_EditValueChanged(object sender, EventArgs e)
        {
            //if (CtxbLowAlarm.Enabled)
            //{ 
            //    levelAlarm1.Text = CtxbLowAlarm.Text;
            //}
        }

        private void voltageAlarmCheck_CheckedChanged(object sender, EventArgs e)
        {
            voltageAlarmVal.Enabled = voltageAlarmCheck.Checked;
            if (!voltageAlarmCheck.Checked)
            {
                voltageAlarmVal.Text = "0";
            }
        }
    }
    /// <summary>
    /// 交叉控制Dg显示对象类
    /// </summary>
    public class CrossControlItem
    {
        public string ArrPoint { get; set; }
        /// <summary>
        /// 测点名称
        /// </summary>
        public string PointName { get; set; }
        /// <summary>
        /// 控制类型
        /// </summary>
        public string ControlType { get; set; }
        /// <summary>
        /// 馈电测点名称
        /// </summary>
        public string FeedBackPointName { get; set; }
        /// <summary>
        /// 删除按钮
        /// </summary>
        public string DelInfBtnStr { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public CrossControlItem()
        {
            ArrPoint = string.Empty;
            PointName = string.Empty;
            ControlType = string.Empty;
            FeedBackPointName = string.Empty;
            DelInfBtnStr = "删除";
        }
    }
}
