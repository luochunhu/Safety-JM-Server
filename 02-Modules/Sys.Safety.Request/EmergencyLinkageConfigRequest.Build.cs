using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;

namespace Sys.Safety.Request.JC_Emergencylinkageconfig
{
    public partial class EmergencyLinkageConfigAddRequest : Basic.Framework.Web.BasicRequest
    {
        public JC_EmergencyLinkageConfigInfo JC_EmergencyLinkageConfigInfo { get; set; }      
    }

	public partial class EmergencyLinkageConfigUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public JC_EmergencyLinkageConfigInfo JC_EmergencyLinkageConfigInfo { get; set; }      
    }

	public partial class EmergencylinkageconfigDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class EmergencyLinkageConfigGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class EmergencyLinkageConfigGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }

    public partial class EmergencyLinkageConfigGetByAnalysisModelIdRequest : Basic.Framework.Web.BasicRequest
    {
        public string AnalysisModelId { get; set; }
    }

    public partial class GetAllEmergencyLinkageConfigRequest : Basic.Framework.Web.BasicRequest
    {

    }
}
