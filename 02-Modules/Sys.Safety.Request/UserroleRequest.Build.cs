using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Userrole
{
    public partial class UserroleAddRequest : Basic.Framework.Web.BasicRequest
    {
        public UserroleInfo UserroleInfo { get; set; }      
    }
    public partial class UserrolesAddRequest : Basic.Framework.Web.BasicRequest
    {
        public string userId { get; set; }
        public List<UserroleInfo> userRoleList { get; set; }
    }

    public partial class UserroleUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public UserroleInfo UserroleInfo { get; set; }      
    }

	public partial class UserroleDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }
    public partial class UserroleDeleteByRoleIdRequest : Basic.Framework.Web.BasicRequest
    {
        public string RoleId { get; set; }
    }
    public partial class UserroleDeleteByUserIdRequest : Basic.Framework.Web.BasicRequest
    {
        public string UserId { get; set; }
    }

    public partial class UserroleGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }
    public partial class UserroleGetByUserIdRequest : Basic.Framework.Web.BasicRequest
    {
        public string UserId { get; set; }
    }
    public partial class UserroleGetCheckUserIDExistRequest : Basic.Framework.Web.BasicRequest
    {
        public string UserId { get; set; }
    }
    public partial class UserroleForUserAssignmentRoleRequest : Basic.Framework.Web.BasicRequest
    {
        public string UserId { get; set; }
        public List<string> lstRoleID { get; set; }
    }

    public partial class UserroleGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}
