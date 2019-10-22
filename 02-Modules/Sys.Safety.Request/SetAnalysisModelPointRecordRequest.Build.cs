using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.JC_Setanalysismodelpointrecord
{
    public partial class SetAnalysisModelPointRecordAddRequest : Basic.Framework.Web.BasicRequest
    {
        public JC_SetAnalysisModelPointRecordInfo JC_SetAnalysisModelPointRecordInfo { get; set; }      
    }

	public partial class SetAnalysisModelPointRecordUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public JC_SetAnalysisModelPointRecordInfo JC_SetAnalysisModelPointRecordInfo { get; set; }      
    }

	public partial class SetanalysismodelpointrecordDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class SetAnalysisModelPointRecordGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class SetAnalysisModelPointRecordGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }

    /// <summary>
    /// 通过模板Id, 获取设置表达式测点的模板
    /// </summary>
    public partial class SetAnalysisModelPointRecordGetTempleteRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 模板Id
        /// </summary>
        public string TemplateId { get; set; }
    }

    public partial class SetAnalysisModelPointRecordByModelIdGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string AnalysisModelId { get; set; }
    }
}
