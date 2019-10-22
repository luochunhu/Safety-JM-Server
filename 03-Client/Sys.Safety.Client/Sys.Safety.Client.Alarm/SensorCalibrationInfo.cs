using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Client.Alarm
{
    /// <summary>
    /// 未标校传感器对象
    /// </summary>
    public class SensorCalibrationInfo
    {
        public string id { get; set; }
        /// <summary>
        /// 测点号
        /// </summary>
        public string Point { get; set; }

        public string pointid { get; set; }

        /// <summary>
        /// 安装位置
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// 设备类型
        /// </summary>
        public string DevName { get; set; }
        /// <summary>
        /// 传感器标校时间（天）
        /// </summary>
        public string SetCalibrationTime { get; set; }
        /// <summary>
        /// 最近一次标校时间
        /// </summary>
        public string LastCalibrationTime { get; set; }
        /// <summary>
        /// 未标校天数
        /// </summary>
        public string CalibrationDays { get; set; }
    }
}
