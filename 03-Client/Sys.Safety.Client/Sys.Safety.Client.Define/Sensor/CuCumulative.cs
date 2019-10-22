using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Sys.Safety.DataContract;


namespace Sys.Safety.Client.Define.Sensor
{
    public partial class CuCumulative : CuBase
    {       
        public CuCumulative()
        {
            InitializeComponent();
        }
        public CuCumulative(string arrPoint, int devID, uint SourceNum)
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
            IsEnableRevised.Checked = false;
            RevisedValue.Text = "";
        }
        /// <summary>
        /// 加载测点信息的函数
        /// </summary>
        public override void LoadInf(string arrPoint, int _devID)
        {
            if (string.IsNullOrEmpty(arrPoint))
            {
                return;
            }
            Jc_DefInfo temp = Model.DEFServiceModel.QueryPointByCodeCache(arrPoint);
            if (null == temp)
            {
                return;
            }
            Jc_DevInfo tempDev = Model.DEVServiceModel.QueryDevByDevIDCache(temp.Devid);
            if (null == tempDev)
            {
                return;
            }
            LoadPointInfo(temp); 
        }
        #endregion

        #region =============================确认信息===============================


        /// <summary>
        /// 保存测点信息的函数
        /// </summary>
        public override bool ConfirmInf(Jc_DefInfo point)
        {  
            if (!SensorInfoverify())
            {
                return false;
            }

            point.Bz6 = IsEnableRevised.Checked ? "1" : "0";
            point.Bz7 = RevisedValue.ToString();
            return true;
        }

        /// <summary>
        /// 有效性验证
        /// </summary>
        public override bool SensorInfoverify()
        {
            bool ret = false;
            if (IsEnableRevised.Checked)
            {
                if (string.IsNullOrEmpty(RevisedValue.Text))
                {
                    XtraMessageBox.Show("请输入修正值！！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return ret;
                }
            }
            
            
            ret = true;
            return ret;
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
                if (Point != null)
                {
                    LoadPointInfo(Point);
                }
                else
                {
                    IsEnableRevised.Checked = false;
                    RevisedValue.Text = "";
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private void LoadPointInfo(Jc_DefInfo temp)
        {
            if (temp.Bz6 == "1")
            {
                IsEnableRevised.Checked = true;
            }
            else
            {
                IsEnableRevised.Checked = false;
            }
            RevisedValue.Text = temp.Bz7;
        }
        #endregion
    }
}
