using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.User
{
    public partial class UserAddRequest : Basic.Framework.Web.BasicRequest
    {
        public UserInfo UserInfo { get; set; }      
    }
    public partial class UsersRequest : Basic.Framework.Web.BasicRequest
    {
        public List<UserInfo> UserInfo { get; set; }
    }
    public partial class UserUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public UserInfo UserInfo { get; set; }      
    }

	public partial class UserDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class UserGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }
    public partial class UserGetByCodeRequest : Basic.Framework.Web.BasicRequest
    {
        public string Code { get; set; }
    }
    public partial class UserMenusGetRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 用户编码
        /// </summary>
        public string userCode { get; set; }
        /// <summary>
        /// 菜单类型（0：标准菜单，1：AQ菜单）
        /// </summary>
        public string MenuType { get; set; }
    }
    public partial class UserRightsGetRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 用户编码
        /// </summary>
        public string userCode { get; set; }
    }
    
    public partial class UserGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}
