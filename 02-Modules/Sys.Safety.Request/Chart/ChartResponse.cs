using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Chart
{
    [Serializable]
    public class GetStateBarTableResponse
    {
        /// <summary>
        /// 统计信息
        /// </summary>       

        public string TjTxt { get; set; }
        /// <summary>
        /// 状态变化柱状图数据
        /// </summary>       

        public DataTable dtBarStateChg { get; set; }
        /// <summary>
        /// 状态统计列表数据
        /// </summary>       

        public DataTable dtTotal { get; set; }
        /// <summary>
        /// 小时开机率统计柱状图数据
        /// </summary>       

        public DataTable dtBarHourTj { get; set; }
    }
}
