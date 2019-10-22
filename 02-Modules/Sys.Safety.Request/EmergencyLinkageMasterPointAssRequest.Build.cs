using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.EmergencyLinkageMasterPointAss
{
    public partial class EmergencyLinkageMasterPointAssAddRequest : Basic.Framework.Web.BasicRequest
    {
        public EmergencyLinkageMasterPointAssInfo EmergencyLinkageMasterPointAssInfo { get; set; }      
    }

	public partial class EmergencyLinkageMasterPointAssUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public EmergencyLinkageMasterPointAssInfo EmergencyLinkageMasterPointAssInfo { get; set; }      
    }

	public partial class EmergencyLinkageMasterPointAssDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class EmergencyLinkageMasterPointAssGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class EmergencyLinkageMasterPointAssGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}
