using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract
{
    public partial class JC_RegionOutageConfigInfo 
    {
        /// <summary>
        /// 分析模型名称
        /// </summary>
        public string AnalysisModelName
        {
            get;
            set;
        }
        /// <summary>
        /// 解除控制分析模型名称
        /// </summary>
        public string RemoveModelName
        {
            get;
            set;
        }

        /// <summary>
        /// 测点号
        /// </summary>
        public string Point { get; set; }
    }
}
