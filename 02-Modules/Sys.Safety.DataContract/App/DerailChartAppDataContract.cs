using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract.App
{
    /// <summary>
    /// 开关量曲线数据
    /// </summary>
    public class DerailChartAppDataContract
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
        /// 当前状态值
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 当前状态描述
        /// </summary>
        public string TypeText { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string Stime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string Etime { get; set; }
        /// <summary>
        /// 馈电状态
        /// </summary>
        public string KD { get; set; }
        /// <summary>
        /// 措施
        /// </summary>
        public string CS { get; set; }
    }
}
