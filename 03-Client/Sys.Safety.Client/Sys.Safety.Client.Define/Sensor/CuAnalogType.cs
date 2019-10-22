using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Collections;
using Sys.Safety.DataContract;
using Sys.Safety.Client.Define.Model;

namespace Sys.Safety.Client.Define.Sensor
{
    public partial class CuAnalogType : CuBaseType
    {
        public CuAnalogType()
        {
            InitializeComponent();
        }

        public CuAnalogType(int devID)
            : base(devID)
        {
            InitializeComponent();
        }

        /// <summary>
        /// 有效性验证
        /// </summary>
        public override bool SensorInfoverify()
        {
            bool ret = false;
            if (string.IsNullOrEmpty(CtxbCheckCycle.Text))
            {
                XtraMessageBox.Show("请填写标校周期", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            int tempInt = 0;
            int.TryParse(CtxbCheckCycle.Text, out tempInt);
            if (tempInt > 90)
            {
                XtraMessageBox.Show("传感器标效周期最长可设置90天！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            if (string.IsNullOrEmpty(CtxbMidRange.Text))
            {
                XtraMessageBox.Show("请填写分段量程", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CtxbHiRange.Text))
            {
                XtraMessageBox.Show("请填写高量程", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (CtxbHiRange.Text == "0")//xuzp20151229
            {
                XtraMessageBox.Show("高量程一般不能为零", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CtxbLowHZ.Text))
            {
                XtraMessageBox.Show("请填写低频率", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CtxbMidHZ.Text))
            {
                XtraMessageBox.Show("请填写分段频率", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            if (float.Parse(CtxbHiPreAlarm.Text) != 0 || float.Parse(CtxbHiAlarm.Text) != 0
               || float.Parse(CtxbHiPowerBack.Text) != 0 || float.Parse(CtxbHiPowerOff.Text) != 0)
            {
                if (float.Parse(CtxbHiPreAlarm.Text) >= float.Parse(CtxbHiAlarm.Text))
                {
                    XtraMessageBox.Show("上限预警值需小于上限报警值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return ret;
                }
                if (float.Parse(CtxbHiAlarm.Text) > float.Parse(CtxbHiPowerOff.Text))
                {
                    XtraMessageBox.Show("上限报警值需小于等于上限断电值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return ret;
                }
                if (float.Parse(CtxbHiPowerBack.Text) >= float.Parse(CtxbHiPowerOff.Text))
                {
                    XtraMessageBox.Show("上限复电值需小于上限断电值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return ret;
                }
            }
            if (float.Parse(CtxbLowPreAlarm.Text) != 0 || float.Parse(CtxbLowAlarm.Text) != 0
               || float.Parse(CtxbLowPowerBack.Text) != 0 || float.Parse(CtxbLowPowerOff.Text) != 0)
            {
                if (float.Parse(CtxbLowPreAlarm.Text) <= float.Parse(CtxbLowAlarm.Text))
                {
                    XtraMessageBox.Show("下限预警值需大于下限报警值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return ret;
                }
                if (float.Parse(CtxbLowAlarm.Text) < float.Parse(CtxbLowPowerOff.Text))
                {
                    XtraMessageBox.Show("下限报警值需大于等下限断电值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return ret;
                }
                if (float.Parse(CtxbLowPowerBack.Text) <= float.Parse(CtxbLowPowerOff.Text))
                {
                    XtraMessageBox.Show("下限复电值需大于下限断电值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return ret;
                }
            }
            ret = true;
            return ret;
        }
        /// <summary>
        /// 设备性质变化产生的影响
        /// </summary>
        public override void DevPropertyChanngeEvent(int DevPropertyID)
        {

        }
        /// <summary>
        /// 加载设备的默认信息函数
        /// </summary>
        public override void LoadPretermitInf()
        {
            CtxbCheckCycle.Text = "0";
            IList<Jc_DevInfo> temp = Model.DEVServiceModel.QueryDevByDevpropertIDCache(1);
            for (int i = 0; i < temp.Count; i++)
            {
                if (null == temp[i].Xs1)
                {
                    continue;
                }
                if (!this.CcmbUint.Properties.Items.Contains(temp[i].Xs1))
                {
                    this.CcmbUint.Properties.Items.Add(temp[i].Xs1);
                }
            }
            if (this.CcmbUint.Properties.Items.Count > 0)
            {
                this.CcmbUint.SelectedIndex = 0;
            }
            CtxbLowRange.Text = "0";
            CtxbMidRange.Text = "0";
            CtxbHiRange.Text = "0";
            CtxbLowHZ.Text = "200";
            CtxbMidHZ.Text = "0";
            CtxbHiHZ.Text = "1000";

            CtxbHiPreAlarm.Text = "0";
            CtxbHiAlarm.Text = "0";
            CtxbHiPowerOff.Text = "0";
            CtxbHiPowerBack.Text = "0";
            CtxbLowPreAlarm.Text = "0";
            CtxbLowAlarm.Text = "0";
            CtxbLowPowerOff.Text = "0";
            CtxbLowPowerBack.Text = "0";
            CorrectionCoefficient.Value = 0;
            //判断是否显示修正系数  20180113
            SettingInfo settingInfo = CONFIGServiceModel.GetConfigFKey("IsCorrectionCoefficient");
            string isCorrectionCoefficient = "0";
            if (settingInfo != null)
            {
                isCorrectionCoefficient = settingInfo.StrValue;
            }
            if (isCorrectionCoefficient == "1")
            {
                layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItem5.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItem7.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItem9.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItem17.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {
                layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem5.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem7.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem9.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem17.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }
        /// <summary>
        /// 加载设备信息的函数
        /// </summary>
        public override void LoadInf(long DevID)
        {
            if (DevID <= 0)
            {
                return;
            }
            Jc_DevInfo temp = Model.DEVServiceModel.QueryDevByDevIDCache(DevID.ToString());
            if (null == temp)
            {
                return;
            }

            CcmbUint.Text = temp.Xs1;
            if (string.IsNullOrEmpty(temp.Bz11))//xuzp20160104
            {
                CtxbLowRange.Text = "0";
            }
            else
            {
                CtxbLowRange.Text = temp.Bz11;
            }
            CtxbMidRange.Text = temp.LC2.ToString();
            CtxbHiRange.Text = temp.LC.ToString();
            CtxbLowHZ.Text = temp.Pl1.ToString();
            CtxbMidHZ.Text = temp.Pl3.ToString();
            CtxbHiHZ.Text = temp.Pl2.ToString();
            CtxbCheckCycle.Text = temp.Pl4.ToString();

            CtxbHiPreAlarm.Text = temp.Z1.ToString();
            CtxbHiAlarm.Text = temp.Z2.ToString(); ;
            CtxbHiPowerOff.Text = temp.Z3.ToString();
            CtxbHiPowerBack.Text = temp.Z4.ToString();
            CtxbLowPreAlarm.Text = temp.Z5.ToString();
            CtxbLowAlarm.Text = temp.Z6.ToString();
            CtxbLowPowerOff.Text = temp.Z7.ToString();
            CtxbLowPowerBack.Text = temp.Z8.ToString();
            CorrectionCoefficient.Value = (decimal)temp.Xzxs;//增加修正系数参数  20180113

        }
        /// <summary>
        /// 保存设备信息的函数
        /// </summary>
        public override bool ConfirmInf(Jc_DevInfo DevType)
        {
            if (!SensorInfoverify())
            {
                return false;
            }
            DevType.Xs1 = CcmbUint.Text;
            DevType.Bz11 = CtxbLowRange.Text;//xuzp20160104
            DevType.LC2 = Convert.ToInt16(CtxbMidRange.Text);
            DevType.LC = Convert.ToInt16(CtxbHiRange.Text);
            DevType.Pl1 = Convert.ToInt16(CtxbLowHZ.Text);
            DevType.Pl2 = Convert.ToInt16(CtxbHiHZ.Text);
            DevType.Pl3 = Convert.ToInt16(CtxbMidHZ.Text);
            DevType.Pl4 = Convert.ToInt16(CtxbCheckCycle.Text);

            DevType.Z1 = float.Parse(CtxbHiPreAlarm.Text);
            DevType.Z2 = float.Parse(CtxbHiAlarm.Text);
            DevType.Z3 = float.Parse(CtxbHiPowerOff.Text);
            DevType.Z4 = float.Parse(CtxbHiPowerBack.Text);
            DevType.Z5 = float.Parse(CtxbLowPreAlarm.Text);
            DevType.Z6 = float.Parse(CtxbLowAlarm.Text);
            DevType.Z7 = float.Parse(CtxbLowPowerOff.Text);
            DevType.Z8 = float.Parse(CtxbLowPowerBack.Text);
            DevType.Xzxs = (float)CorrectionCoefficient.Value;//增加修正系数参数  20180113

            return true;
        }
    }
}
