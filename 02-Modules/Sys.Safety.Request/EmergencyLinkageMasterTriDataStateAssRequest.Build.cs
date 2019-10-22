using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.EmergencyLinkageMasterTriDataStateAss
{
    public partial class EmergencyLinkageMasterTriDataStateAssAddRequest : Basic.Framework.Web.BasicRequest
    {
        public EmergencyLinkageMasterTriDataStateAssInfo EmergencyLinkageMasterTriDataStateAssInfo { get; set; }      
    }

	public partial class EmergencyLinkageMasterTriDataStateAssUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public EmergencyLinkageMasterTriDataStateAssInfo EmergencyLinkageMasterTriDataStateAssInfo { get; set; }      
    }

	public partial class EmergencyLinkageMasterTriDataStateAssDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class EmergencyLinkageMasterTriDataStateAssGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class EmergencyLinkageMasterTriDataStateAssGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}
