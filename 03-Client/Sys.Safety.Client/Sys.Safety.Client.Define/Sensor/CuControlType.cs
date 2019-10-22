using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using DevExpress.XtraEditors;
using Sys.Safety.DataContract;

namespace Sys.Safety.Client.Define.Sensor
{
    public partial class CuControlType : CuBaseType 
    {
        public CuControlType()
        {
            InitializeComponent();
        }

        public CuControlType(int devID)
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
            if (string.IsNullOrEmpty(CcmbZeroContent.Text))
            {
                XtraMessageBox.Show("请选择或输入0态显示内容", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CcmbOneContent.Text))
            {
                XtraMessageBox.Show("请选择或输入1态显示内容", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
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
            CckZeroAlarm.Checked = true;
            CckOneAlarm.Checked = true;

            IList<Jc_DevInfo> temp = Model.DEVServiceModel.QueryDevByDevpropertIDCache(2);
            if (null == temp)
            {
                for (int i = 0; i < temp.Count; i++)
                {
                    this.CcmbZeroContent.Properties.Items.Add(temp[i].Xs1);
                    this.CcmbOneContent.Properties.Items.Add(temp[i].Xs2);
                }
                if (temp.Count > 0)
                {
                    this.CcmbZeroContent.SelectedIndex = 0;
                    this.CcmbOneContent.SelectedIndex = 0;
                }
            }
        }
        /// <summary>
        /// 加载设备信息的函数
        /// </summary>
        public override void LoadInf(long DevID)
        {
            if (DevID == 0)
            {
                return;
            }
            Jc_DevInfo temp = Model.DEVServiceModel.QueryDevByDevIDCache(DevID.ToString());
            if (null == temp)
            {
                return;
            }
            CckZeroAlarm.Checked = (temp.Pl1 == 1) ? true : false;
            CckOneAlarm.Checked = (temp.Pl2 == 1) ? true : false;

            CcmbZeroContent.Text = temp.Xs1;
            CcmbOneContent.Text = temp.Xs2;

            CpZeroColour.Color = Color.FromArgb(temp.Color1);
            CpOneColour.Color = Color.FromArgb(temp.Color2);
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

            DevType.Pl1 = (short)((CckZeroAlarm.Checked) ? 1 : 0);
            DevType.Pl2 = (short)((CckOneAlarm.Checked) ? 1 : 0);

            DevType.Xs1 = CcmbZeroContent.Text;
            DevType.Xs2 = CcmbOneContent.Text;

            DevType.Color1 = CpZeroColour.Color.ToArgb();
            DevType.Color2 = CpOneColour.Color.ToArgb();

            return true;
        }
    }
}
