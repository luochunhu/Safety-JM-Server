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
    public partial class CuBase : XtraUserControl
    {
        public string _arrPoint;
        public int _devID;
        public uint _SourceNum;
        public CuBase()
        {
            InitializeComponent();
        }
        public CuBase(string arrPoint, int devID, uint SourceNum)
        {
            _SourceNum = SourceNum;
            _arrPoint = arrPoint;
            _devID = devID;
            InitializeComponent();
        }
        /// <summary>
        /// 加载测点的默认信息函数
        /// </summary>
        public virtual void LoadPretermitInf()
        {

        }
        /// <summary>
        /// 加载测点信息的函数
        /// </summary>
        public virtual void LoadInf(string arrPoint, int _devID)
        {

        }
        /// <summary>
        /// 保存测点信息的函数
        /// </summary>
        public virtual bool ConfirmInf(Jc_DefInfo point)
        {
            return true;
        }
        /// <summary>
        /// 有效性验证
        /// </summary>
        public virtual bool SensorInfoverify()
        {
            return true;
        }
        /// <summary>
        /// 设备类型变化产生的影响
        /// </summary>
        public virtual void DevTypeChanngeEvent(long DevID,Jc_DefInfo Point)  
        {

        }
        /// <summary>
        /// 加载信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CuCommonSensor_Load(object sender, EventArgs e)
        {
            LoadPretermitInf();//加载默认信息
            LoadInf(_arrPoint, _devID);//加载显示的信息
        }
    }
}
