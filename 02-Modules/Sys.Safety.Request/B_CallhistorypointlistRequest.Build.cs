using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.B_Callhistorypointlist
{
    public partial class B_CallhistorypointlistAddRequest : Basic.Framework.Web.BasicRequest
    {
        public B_CallhistorypointlistInfo B_CallhistorypointlistInfo { get; set; }      
    }

	public partial class B_CallhistorypointlistUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public B_CallhistorypointlistInfo B_CallhistorypointlistInfo { get; set; }      
    }

	public partial class B_CallhistorypointlistDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class B_CallhistorypointlistGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class B_CallhistorypointlistGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}
