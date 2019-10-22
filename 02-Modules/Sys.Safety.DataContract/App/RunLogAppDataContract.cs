using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract.App
{
    public class RunLogAppDataContract
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 测点ID
        /// </summary>
        public string PointID { get; set; }

        /// <summary>
        /// 设备类型名称
        /// </summary>
        public string DevModelName { get; set; }

        /// <summary>
        /// 安装位置
        /// </summary>
        public string place { get; set; }


        /// <summary>
        /// 时间(用于密采和运行记录)
        /// </summary>
        public string time { get; set; }


        /// <summary>
        /// 值
        /// </summary>
        public string value { get; set; }


        /// <summary>
        /// (用于模拟量、开关量报警和断电)
        /// </summary>
        public string StartTime { get; set; }


        /// <summary>
        /// 结束时间(用于模拟量、开关量报警和断电)
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// 数据状态
        /// </summary>
        public string DataState { get; set; }
        /// <summary>
        /// 设备状态
        /// </summary>
        public string DeviceState { get; set; }
        /// <summary>
        /// 异常持续时间
        /// </summary>
        public string Duration { get; set; }
    }
}
