using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Webauthority
{
    public partial class WebauthorityAddRequest : Basic.Framework.Web.BasicRequest
    {
        public WebauthorityInfo WebauthorityInfo { get; set; }      
    }

	public partial class WebauthorityUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public WebauthorityInfo WebauthorityInfo { get; set; }      
    }

	public partial class WebauthorityDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class WebauthorityGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class WebauthorityGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}
