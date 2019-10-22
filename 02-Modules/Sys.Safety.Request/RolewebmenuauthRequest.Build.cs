using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Rolewebmenuauth
{
    public partial class RolewebmenuauthAddRequest : Basic.Framework.Web.BasicRequest
    {
        public RolewebmenuauthInfo RolewebmenuauthInfo { get; set; }      
    }

	public partial class RolewebmenuauthUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public RolewebmenuauthInfo RolewebmenuauthInfo { get; set; }      
    }

	public partial class RolewebmenuauthDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class RolewebmenuauthGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class RolewebmenuauthGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}
