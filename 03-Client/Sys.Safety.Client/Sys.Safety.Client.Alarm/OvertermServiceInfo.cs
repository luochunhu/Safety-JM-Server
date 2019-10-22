using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Client.Alarm
{
    /// <summary>
    /// 超期服役传感器对象
    /// </summary>
    public class OvertermServiceInfo
    {
        /// <summary>
        /// 测点号
        /// </summary>
        public string Point { get; set; }
        /// <summary>
        /// 安装位置
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// 设备类型
        /// </summary>
        public string DevName { get; set; }
        /// <summary>
        /// 使用年限（年）
        /// </summary>
        public string UseYear { get; set; }
        /// <summary>
        /// 出厂日期
        /// </summary>
        public string ManufactureDate { get; set; }
        /// <summary>
        /// 已使用时间
        /// </summary>
        public string UsedTime { get; set; }
    }
}
