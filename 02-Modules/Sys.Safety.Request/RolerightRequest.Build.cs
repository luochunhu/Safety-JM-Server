using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Roleright
{
    public partial class RolerightAddRequest : Basic.Framework.Web.BasicRequest
    {
        public RolerightInfo RolerightInfo { get; set; }      
    }
    public partial class RolerightsAddRequest : Basic.Framework.Web.BasicRequest
    {
        public string roleId { get; set; }
        public List<RolerightInfo> RolerightInfo { get; set; }
    }

    public partial class RolerightUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public RolerightInfo RolerightInfo { get; set; }      
    }

	public partial class RolerightDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }
    public partial class RolerightDeleteByRoleIdRequest : Basic.Framework.Web.BasicRequest
    {
        public string RoleId { get; set; }
    }
    public partial class RolerightDeleteByRightIdRequest : Basic.Framework.Web.BasicRequest
    {
        public string RightId { get; set; }
    }

    public partial class RolerightGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }
    public partial class RolerightGetByRoleIdRequest : Basic.Framework.Web.BasicRequest
    {
        public string RoleId { get; set; }
    }
    public partial class RolerightCheckExistByRoleIdRequest : Basic.Framework.Web.BasicRequest
    {
        public string RoleId { get; set; }
    }
    public partial class RolerightForRoleAssignmentRightRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public string RoleId { get; set; }
        /// <summary>
        /// 权限列表
        /// </summary>
        public List<string> lstRightID { get; set; }
    }

    public partial class RolerightGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}
