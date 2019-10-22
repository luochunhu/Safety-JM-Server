using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.B_Broadcastplanpointlist
{
    public partial class B_BroadcastplanpointlistAddRequest : Basic.Framework.Web.BasicRequest
    {
        public B_BroadcastplanpointlistInfo B_BroadcastplanpointlistInfo { get; set; }      
    }

	public partial class B_BroadcastplanpointlistUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public B_BroadcastplanpointlistInfo B_BroadcastplanpointlistInfo { get; set; }      
    }

	public partial class B_BroadcastplanpointlistDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class B_BroadcastplanpointlistGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class B_BroadcastplanpointlistGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}
