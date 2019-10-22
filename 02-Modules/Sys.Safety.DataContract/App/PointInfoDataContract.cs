using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract.App
{
    /// <summary>
    /// 测点数据信息
    /// </summary>
    public class PointInfoDataContract
    {
        // <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 设备类型名称
        /// </summary>
        public string ModelName { get; set; }
        /// <summary>
        /// 安装位置
        /// </summary>
        public string Place { get; set; }
        /// <summary>
        /// 实时值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 数据状态
        /// </summary>
        public string DataState { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        public string DeviceState { get; set; }
        /// <summary>
        /// 设备状态编号
        /// </summary>
        public string DeviceStateCode { get; set; }
        /// <summary>
        /// 设备性质编号
        /// </summary>
        public string DevicePropertyCode { get; set; }
        /// <summary>
        /// 设备性质
        /// </summary>
        public string DeviceProperty { get; set; }
        /// <summary>
        /// 是否异常(true:异常，false:正常)
        /// </summary>
        public bool Alarm { get; set; }       
        
        /// <summary>
        /// 测点号
        /// </summary>
        public string Code { get; set; } 
    }
}
