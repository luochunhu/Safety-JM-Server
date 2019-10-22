using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Client.DataAnalysis.BusinessModel
{
    public class RegionOutageBusinessModel
    {
        /// <summary>
        /// 分析模型ID
        /// </summary>
        public string AnalysisModelId { get; set; } 
        /// <summary>
        /// 区域断电配置表
        /// </summary>
        public List<JC_RegionOutageConfigInfo> RegionOutageConfigInfoList { get; set; }

    }
}
