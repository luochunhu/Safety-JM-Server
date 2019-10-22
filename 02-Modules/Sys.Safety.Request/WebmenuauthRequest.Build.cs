using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Webmenuauth
{
    public partial class WebmenuauthAddRequest : Basic.Framework.Web.BasicRequest
    {
        public WebmenuauthInfo WebmenuauthInfo { get; set; }      
    }

	public partial class WebmenuauthUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public WebmenuauthInfo WebmenuauthInfo { get; set; }      
    }

	public partial class WebmenuauthDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class WebmenuauthGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class WebmenuauthGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}
