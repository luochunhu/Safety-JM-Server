using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.AnalysisTemplateConfig
{
    public partial class AnalysisTemplateConfigAddRequest : Basic.Framework.Web.BasicRequest
    {
        public JC_AnalysisTemplateConfigInfo JC_AnalysisTemplateConfigInfo { get; set; }      
    }
    public partial class AnalysisTemplateConfigListAddRequest : Basic.Framework.Web.BasicRequest
    {
        public List<JC_AnalysisTemplateConfigInfo> JC_AnalysisTemplateConfigInfoList { get; set; }
    }

    public partial class AnalysisTemplateConfigUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public JC_AnalysisTemplateConfigInfo JC_AnalysisTemplateConfigInfo { get; set; }      
    }

	public partial class AnalysisTemplateConfigDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class AnalysisTemplateConfigGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }
    public partial class AnalysisTemplateConfigGetByTempleteIdRequest : Basic.Framework.Web.BasicRequest
    {
        public string TempleteId { get; set; }
    }


	public partial class AnalysisTemplateConfigGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}
