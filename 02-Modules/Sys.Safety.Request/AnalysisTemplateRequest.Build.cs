using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.AnalysisTemplate
{
    public partial class AnalysisTemplateAddRequest : Basic.Framework.Web.BasicRequest
    {   
        /// <summary>
        /// 逻辑分析模板
        /// </summary>
        public JC_AnalysisTemplateInfo JC_AnalysisTemplateInfo { get; set; }
    
        /// <summary>
        /// 表达式
        /// </summary>
        public List<JC_AnalyticalExpressionInfo> AnalysisExpressionInfoList { get; set; }
        /// <summary>
        /// 表达式配置
        /// </summary>
        public List<JC_ExpressionConfigInfo> ExpressionConfigInfoList { get; set; }
 
        /// <summary>
        /// 分析模板配置表
        /// </summary>
        public List<JC_AnalysisTemplateConfigInfo> JC_AnalysisTemplateConfigInfoList { get; set; }
    
    }

	public partial class AnalysisTemplateUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public JC_AnalysisTemplateInfo JC_AnalysisTemplateInfo { get; set; }      
    }

	public partial class AnalysisTemplateDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }

        public List<string> Ids { get; set; }
    }

	public partial class AnalysisTemplateGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string TempleteId { get; set; }
        public string Id { get; set; }
    }

	public partial class AnalysisTemplateGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
    public partial class AnalysisTemplateGetListByNameRequest : Basic.Framework.Web.BasicRequest
    {
        public string Name { get; set; }
    }
}
