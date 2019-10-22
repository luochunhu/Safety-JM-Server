using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.EmergencyLinkagePassivePointAss
{
    public partial class EmergencyLinkagePassivePointAssAddRequest : Basic.Framework.Web.BasicRequest
    {
        public EmergencyLinkagePassivePointAssInfo EmergencyLinkagePassivePointAssInfo { get; set; }      
    }

	public partial class EmergencyLinkagePassivePointAssUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public EmergencyLinkagePassivePointAssInfo EmergencyLinkagePassivePointAssInfo { get; set; }      
    }

	public partial class EmergencyLinkagePassivePointAssDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class EmergencyLinkagePassivePointAssGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class EmergencyLinkagePassivePointAssGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}
