using Basic.Framework.Web;
using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Client.DataAnalysis.BusinessModel
{
    /// <summary>
    /// 逻辑分析模板的业务模型
    /// </summary>
    public class AnalysisTemplateBusinessModel
    {
        public AnalysisTemplateBusinessModel() {
            pagerInfo = new PagerInfo();
        }
        /// <summary>
        /// 逻辑分析模板
        /// </summary>
        public JC_AnalysisTemplateInfo AnalysisTemplateInfo { get; set; }
        /// <summary>
        /// 逻辑分析模板列表
        /// </summary>
        public List<JC_AnalysisTemplateInfo> AnalysisTemplateInfoList { get; set; }
        /// <summary>
        /// 表达式
        /// </summary>
        public List<JC_AnalyticalExpressionInfo> AnalysisExpressionInfoList { get; set; }
        /// <summary>
        /// 表达式配置
        /// </summary>
        public List<JC_ExpressionConfigInfo> ExpressionConfigInfoList { get; set; }
        /// <summary>
        /// 参数表
        /// </summary>
        public List<JC_ParameterInfo> ParameterInfoList { get; set; }
        /// <summary>
        /// 因子表
        /// </summary>
        public List<JC_FactorInfo> FactorInfoList { get; set; }
        /// <summary>
        /// 分析模板配置表
        /// </summary>
        public List<JC_AnalysisTemplateConfigInfo> JC_AnalysisTemplateConfigInfoList { get; set; }
        /// <summary>
        /// 分页
        /// </summary>
        public PagerInfo pagerInfo { get; set; }
    }
}
