using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract.CommunicateExtend
{
    /// <summary>
    /// 设备基础信息
    /// </summary>
    public class DeviceInfoMation
    {
        /// <summary>
        /// 表示设备的通道号（=0表示采集设备本身，=1表示此采集设备下的1号设备….）;人员定位：kh
        /// </summary>
        public string Channel { get; set; }
        /// <summary>
        /// 表示设备的地址号（单参数传感器此值为0，多参数传感器此值为其地址号） 人员定位：bh
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 设备型号编码
        /// </summary>
        public int DeviceTypeCode { get; set; }
        /// <summary>
        /// 上报值
        /// </summary>
        public float UpAarmValue { get; set; }
        /// <summary>
        /// 下报值
        /// </summary>
        public float DownAarmValue { get; set; }
        /// <summary>
        /// 上断值
        /// </summary>
        public float UpDdValue { get; set; }
        /// <summary>
        /// 下断值
        /// </summary>
        public float DownDdValue { get; set; }
        /// <summary>
        /// 上恢复值
        /// </summary>
        public float UpHfValue { get; set; }
        /// <summary>
        /// 下恢复值
        /// </summary>
        public float DownHfValue { get; set; }
        /// <summary>
        /// 量程开始值
        /// </summary>
        public float LC1 { get; set; }
        /// <summary>
        /// 量程结始值
        /// </summary>
        public float LC2 { get; set; }
        /// <summary>
        /// 分级报警1的值
        /// </summary>
        public float SeniorGradeAlarmValue1 { get; set; }
        /// <summary>
        /// 分级报警2的值
        /// </summary>
        public float SeniorGradeAlarmValue2 { get; set; }
        /// <summary>
        /// 分级报警3的值
        /// </summary>
        public float SeniorGradeAlarmValue3 { get; set; }
        /// <summary>
        /// 分级报警4的值
        /// </summary>
        public float SeniorGradeAlarmValue4 { get; set; }
        /// <summary>
        /// 分级报警1时长值
        /// </summary>
        public float SeniorGradeTimeValue1 { get; set; }
        /// <summary>
        /// 分级报警2时长值
        /// </summary>
        public float SeniorGradeTimeValue2 { get; set; }
        /// <summary>
        /// 分级报警3时长值
        /// </summary>
        public float SeniorGradeTimeValue3 { get; set; }
        /// <summary>
        /// 分级报警4时长值
        /// </summary>
        public float SeniorGradeTimeValue4 { get; set; }
    }
}
