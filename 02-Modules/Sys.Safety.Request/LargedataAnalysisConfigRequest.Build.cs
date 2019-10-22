using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.JC_Largedataanalysisconfig
{
    public partial class LargedataAnalysisConfigAddRequest : Basic.Framework.Web.BasicRequest
    {
        public JC_LargedataAnalysisConfigInfo JC_LargedataAnalysisConfigInfo { get; set; }
    }

	public partial class LargedataAnalysisConfigUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public JC_LargedataAnalysisConfigInfo JC_LargedataAnalysisConfigInfo { get; set; }      
    }

	public partial class LargedataAnalysisConfigDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class LargedataAnalysisConfigGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string TempleteId { get; set; }
        public string Id { get; set; }
    }

	public partial class LargedataAnalysisConfigGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
    public partial class LargedataAnalysisConfigGetListByNameRequest : Basic.Framework.Web.BasicRequest
    {
        public string Name { get; set; }
    }

    public partial class LargedataAnalysisConfigGetListWithRegionOutageRequest : Basic.Framework.Web.BasicRequest
    {
        public string AnalysisModelName { get; set; }
    }

    public partial class LargeDataAnalysisConfigListForCurveRequest : Basic.Framework.Web.BasicRequest
    {
        public string QueryDate { get; set; }
    }
}
