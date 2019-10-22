using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract
{
    /// <summary>
    /// 井下自动挂接设备对象
    /// </summary>
    public partial class AutomaticArticulatedDeviceInfo : Basic.Framework.Web.BasicInfo
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string ID
        {
            get;
            set;
        }
        /// <summary>
        /// 分支号：=1表示智能口1；=2表示智能口2；=3表示智能口3；=4表示智能口4；=5表示扩展的智能开停；=6表示挂接的模拟量采集板
        /// </summary>
        public byte BranchNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 分站号
        /// </summary>
        public short StationNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 口号
        /// </summary>
        public short ChanelNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 地址号
        /// </summary>
        public short AddressNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 设备唯一编码
        /// </summary>
        public string DeviceOnlyCode

        {
            get;
            set;
        }
        /// <summary>
        /// 设备型号
        /// </summary>
        public int DeviceModel
        {
            get;
            set;
        }
        /// <summary>
        /// 值
        /// </summary>
        public string Value
        {
            get;
            set;
        }
        /// <summary>
        /// 接收时间 2017.10.17 by
        /// </summary>
        public DateTime ReciveTime
        {
            get;
            set;
        }
    }
}
