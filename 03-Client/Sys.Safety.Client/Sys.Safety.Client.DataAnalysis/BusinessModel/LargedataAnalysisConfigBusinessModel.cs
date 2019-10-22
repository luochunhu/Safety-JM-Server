using Basic.Framework.Web;
using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Client.DataAnalysis.BusinessModel
{
    public class LargedataAnalysisConfigBusinessModel
    {    /// <summary>
        /// 模型列表
        /// </summary>
        public List<JC_LargedataAnalysisConfigInfo> LargedataAnalysisConfigInfoList { get; set; }
        /// <summary>
        /// 模型
        /// </summary>
        public JC_LargedataAnalysisConfigInfo LargedataAnalysisConfigInfo { get; set; }
        /// <summary>
        /// 分页
        /// </summary>
        public PagerInfo pagerInfo { get; set; }
    }
}
