using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request
{
    public partial class RequestAddRequest : Basic.Framework.Web.BasicRequest
    {
        public RequestInfo RequestInfo { get; set; }      
    }

	public partial class RequestUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public RequestInfo RequestInfo { get; set; }      
    }

	public partial class RequestDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class RequestGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class RequestGetByCodeRequest : Basic.Framework.Web.BasicRequest
    {
        public string Code { get; set; }
    }
    public partial class RequestGetMenuByCodeRequest : Basic.Framework.Web.BasicRequest
    {
        public string Code { get; set; }
    }

    public partial class RequestGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}
