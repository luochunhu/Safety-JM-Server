using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Rolewebmenu
{
    public partial class RolewebmenuAddRequest : Basic.Framework.Web.BasicRequest
    {
        public RolewebmenuInfo RolewebmenuInfo { get; set; }      
    }

	public partial class RolewebmenuUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public RolewebmenuInfo RolewebmenuInfo { get; set; }      
    }

	public partial class RolewebmenuDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class RolewebmenuGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class RolewebmenuGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }

    public partial class RoleWebMenuUpdateByRoleRequest : Basic.Framework.Web.BasicRequest 
    {
        public string RoleId { get; set; }

        public List<RolewebmenuInfo> RolewebMenuInfos { get; set; }
    }

    public partial class RolewebmenuGetByRoleRequest : Basic.Framework.Web.BasicRequest 
    {
        public string RoleId { get; set; }
    }
}
