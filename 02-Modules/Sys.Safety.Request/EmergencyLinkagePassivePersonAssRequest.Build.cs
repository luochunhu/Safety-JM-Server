using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.EmergencyLinkagePassivePersonAss
{
    public partial class EmergencyLinkagePassivePersonAssAddRequest : Basic.Framework.Web.BasicRequest
    {
        public EmergencyLinkagePassivePersonAssInfo EmergencyLinkagePassivePersonAssInfo { get; set; }      
    }

	public partial class EmergencyLinkagePassivePersonAssUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public EmergencyLinkagePassivePersonAssInfo EmergencyLinkagePassivePersonAssInfo { get; set; }      
    }

	public partial class EmergencyLinkagePassivePersonAssDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class EmergencyLinkagePassivePersonAssGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class EmergencyLinkagePassivePersonAssGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}
