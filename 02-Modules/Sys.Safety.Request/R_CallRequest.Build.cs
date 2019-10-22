using Basic.Framework.Rpc;
using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.R_Call
{
    public partial class R_CallAddRequest : Basic.Framework.Web.BasicRequest
    {
        public R_CallInfo CallInfo { get; set; }      
    }

	public partial class R_CallUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public R_CallInfo CallInfo { get; set; }      
    }

	public partial class R_CallDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class R_CallGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class R_CallGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }

    public partial class R_CallUpdateProperitesRequest : Basic.Framework.Web.BasicRequest
    {
        public Dictionary<string, Dictionary<string, object>> updateItems { get; set; }
    }
}
