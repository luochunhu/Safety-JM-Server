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
    public partial class CuBaseType : XtraUserControl
    {
        public int _devID; //设备类型对象
        public CuBaseType()
        {
            InitializeComponent();
        }

        public CuBaseType(int devID)
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            _devID = devID;
            InitializeComponent();
        }

        /// <summary>
        /// 有效性验证
        /// </summary>
        public virtual bool SensorInfoverify()
        {
            return true;
        }
        /// <summary>
        /// 设备性质变化产生的影响
        /// </summary>
        public virtual void DevPropertyChanngeEvent(int DevPropertyID)
        {

        }

        /// <summary>
        /// 加载设备的默认信息函数
        /// </summary>
        public virtual void LoadPretermitInf()
        {

        }
        /// <summary>
        /// 加载设备信息的函数
        /// </summary>
        public virtual void LoadInf(long DevID)
        {

        }
        /// <summary>
        /// 保存设备信息的函数
        /// </summary>
        public virtual bool ConfirmInf(Jc_DevInfo DevType)
        {
            return true;
        }

        /// <summary>
        /// 加载信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CuCommonSensorType_Load(object sender, EventArgs e)
        {
            LoadPretermitInf(); //加载默认信息
            LoadInf(_devID);  //加载
        }
    }
}
