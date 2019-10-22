using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.B_Callpointlist
{
    public partial class B_CallpointlistAddRequest : Basic.Framework.Web.BasicRequest
    {
        public B_CallpointlistInfo B_CallpointlistInfo { get; set; }      
    }

	public partial class B_CallpointlistUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public B_CallpointlistInfo B_CallpointlistInfo { get; set; }      
    }

	public partial class B_CallpointlistDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class B_CallpointlistGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class B_CallpointlistGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}
