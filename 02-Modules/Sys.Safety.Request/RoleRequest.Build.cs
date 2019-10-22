using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Role
{
    public partial class RoleAddRequest : Basic.Framework.Web.BasicRequest
    {
        public RoleInfo RoleInfo { get; set; }      
    }
    public partial class RolesRequest : Basic.Framework.Web.BasicRequest
    {
        public List<RoleInfo> RoleInfo { get; set; }
    }

    public partial class RoleUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public RoleInfo RoleInfo { get; set; }      
    }

	public partial class RoleDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }
    public partial class RolesDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public List<string> IdList { get; set; }
    }

    public partial class RoleGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class RoleGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}
