using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.EmergencyLinkageMasterAreaAss
{
    public partial class EmergencyLinkageMasterAreaAssAddRequest : Basic.Framework.Web.BasicRequest
    {
        public EmergencyLinkageMasterAreaAssInfo EmergencyLinkageMasterAreaAssInfo { get; set; }      
    }

	public partial class EmergencyLinkageMasterAreaAssUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public EmergencyLinkageMasterAreaAssInfo EmergencyLinkageMasterAreaAssInfo { get; set; }      
    }

	public partial class EmergencyLinkageMasterAreaAssDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class EmergencyLinkageMasterAreaAssGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class EmergencyLinkageMasterAreaAssGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}
