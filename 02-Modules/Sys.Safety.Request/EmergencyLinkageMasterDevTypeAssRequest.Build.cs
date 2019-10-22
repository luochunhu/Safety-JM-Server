using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.EmergencyLinkageMasterDevTypeAss
{
    public partial class EmergencyLinkageMasterDevTypeAssAddRequest : Basic.Framework.Web.BasicRequest
    {
        public EmergencyLinkageMasterDevTypeAssInfo EmergencyLinkageMasterDevTypeAssInfo { get; set; }      
    }

	public partial class EmergencyLinkageMasterDevTypeAssUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public EmergencyLinkageMasterDevTypeAssInfo EmergencyLinkageMasterDevTypeAssInfo { get; set; }      
    }

	public partial class EmergencyLinkageMasterDevTypeAssDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class EmergencyLinkageMasterDevTypeAssGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class EmergencyLinkageMasterDevTypeAssGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}
