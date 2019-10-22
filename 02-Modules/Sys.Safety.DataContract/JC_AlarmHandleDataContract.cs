using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract
{
    public partial class JC_AlarmHandleInfo
    {
        /// <summary>
        /// 分析模型名称
        /// </summary>
        public string AnalysisModelName { get; set; }
        /// <summary>
        /// 结束时间显示格式（19990-01-01 00:00:00 显示为-） 
        /// </summary>
        public string EtimeDisplay { get; set; }
    }
}
