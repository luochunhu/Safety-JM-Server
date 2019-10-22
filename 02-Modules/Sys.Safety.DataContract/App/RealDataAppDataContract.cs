using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract.App
{
    /// <summary>
    /// 实时数据信息
    /// </summary>
    public class RealDataAppDataContract
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
        /// 实时值（带单位）
        /// </summary>
        public string Value { get; set; }        
        /// <summary>
        /// 分级报警等级
        /// </summary>
        public string GradingAlarmLevel { get; set; }
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
        /// 异常开始时间
        /// </summary>
        public string AlarmTime { get; set; }
        /// <summary>
        /// 异常持续时间（分钟数）
        /// </summary>
        public int Duration { get; set; }
        /// <summary>
        /// 测点号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 联动设备是否正常(-1:无联动设备，0：联动设备正常，1：联动设备异常)
        /// </summary>
        public string LinkageState { get; set; }
        /// <summary>
        /// 定义更新时间
        /// </summary>
        public string DefDateTime { get; set; }
        /// <summary>
        /// 联动设备详细信息
        /// </summary>
        public List<LinkPointDetailAppDataContract> LinkPointDetail { get; set; }
        /// <summary>
        /// 子设备实时数据列表
        /// </summary>
        public List<RealDataAppDataContract> SonPointDetail { get; set; }
    }
}
