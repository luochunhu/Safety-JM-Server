using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract.App
{
    /// <summary>
    /// 网络模块信息
    /// </summary>
    public class NetworkModuleAppDataContract
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// IP
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// MAC
        /// </summary>
        public string MAC { get; set; }
        /// <summary>
        /// 连接号
        /// </summary>
        public string NO { get; set; }
        /// <summary>
        /// 挂接设备数
        /// </summary>
        public string DeviceNum { get; set; }
        /// <summary>
        /// 异常设备数
        /// </summary>
        public string AlarmNum { get; set; }
        /// <summary>
        /// 实时值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 是否异常
        /// </summary>
        public bool Alarm { get; set; }
        /// <summary>
        /// 测点详情
        /// </summary>
        public List<RealDataAppDataContract> PointList { get; set; }
    }
}
