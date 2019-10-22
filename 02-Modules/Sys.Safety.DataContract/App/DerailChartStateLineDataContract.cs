using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract.App
{
    /// <summary>
    /// 开关量柱状图数据
    /// </summary>
    public class DerailChartStateLineDataContract
    {
        /// <summary>
        /// 值的ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 测点ID
        /// </summary>
        public string PointID { get; set; }
        /// <summary>
        /// 当前开机率
        /// </summary>
        public string Machine { get; set; }
        /// <summary>
        /// 当前开机时间
        /// </summary>
        public string BootTime { get; set; }
        /// <summary>
        /// 当前开停次数
        /// </summary>
        public string StopOpenTime { get; set; }
        /// <summary>
        /// 当前小时
        /// </summary>
        public string Timer { get; set; }       
    }
}
