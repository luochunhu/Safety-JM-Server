using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.EmergencyLinkagePassiveAreaAss
{
    public partial class EmergencyLinkagePassiveAreaAssAddRequest : Basic.Framework.Web.BasicRequest
    {
        public EmergencyLinkagePassiveAreaAssInfo EmergencyLinkagePassiveAreaAssInfo { get; set; }      
    }

	public partial class EmergencyLinkagePassiveAreaAssUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public EmergencyLinkagePassiveAreaAssInfo EmergencyLinkagePassiveAreaAssInfo { get; set; }      
    }

	public partial class EmergencyLinkagePassiveAreaAssDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class EmergencyLinkagePassiveAreaAssGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class EmergencyLinkagePassiveAreaAssGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}
