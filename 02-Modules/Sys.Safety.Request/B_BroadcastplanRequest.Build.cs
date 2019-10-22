using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.B_Broadcastplan
{
    public partial class B_BroadcastplanAddRequest : Basic.Framework.Web.BasicRequest
    {
        public B_BroadcastplanInfo B_BroadcastplanInfo { get; set; }      
    }

	public partial class B_BroadcastplanUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public B_BroadcastplanInfo B_BroadcastplanInfo { get; set; }      
    }

	public partial class B_BroadcastplanDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class B_BroadcastplanGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class B_BroadcastplanGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}
