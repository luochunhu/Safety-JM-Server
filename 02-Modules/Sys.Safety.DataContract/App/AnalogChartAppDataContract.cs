using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract.App
{
    /// <summary>
    /// 模拟量曲线数据
    /// </summary>
    public class AnalogChartAppDataContract
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
        /// 最大值
        /// </summary>
        public string ZDZ { get; set; }
        /// <summary>
        /// 最小值
        /// </summary>
        public string ZXZ { get; set; }
        /// <summary>
        /// 平均值
        /// </summary>
        public string PJZ { get; set; }
        /// <summary>
        /// 实时值
        /// </summary>
        public string SSZ { get; set; }
        /// <summary>
        /// 移动值
        /// </summary>
        public string YDZ { get; set; }
        /// <summary>
        /// 当前状态值
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 当前状态描述
        /// </summary>
        public string TypeText { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public string Timer { get; set; }  
    }
}
