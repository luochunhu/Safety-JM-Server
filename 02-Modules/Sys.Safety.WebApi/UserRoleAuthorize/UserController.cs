using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Basic.Framework.Service;
using System.Web.Http;
using Sys.Safety.Request;
using System.Data;
using Sys.Safety.Request.Config;
using Sys.Safety.ServiceContract.UserRoleAuthorize;
using Sys.Safety.Request.Login;
using Sys.Safety.Request.User;

namespace Sys.Safety.WebApi
{
    /// <summary>
    /// 用户管理 WebApi接口
    /// </summary>
    public class UserController : Basic.Framework.Web.WebApi.BasicApiController, IUserService
    {
        static UserController()
        {

        }
        IUserService _userService = ServiceFactory.Create<IUserService>();
        
        [HttpPost]
        [Route("v1/User/Add")]
        public BasicResponse<UserInfo> AddUser(UserAddRequest userrequest)
        {
            return _userService.AddUser(userrequest);
        }
        [HttpPost]
        [Route("v1/User/Update")]
        public BasicResponse<UserInfo> UpdateUser(UserUpdateRequest userrequest)
        {
            return _userService.UpdateUser(userrequest);
        }
        [HttpPost]
        [Route("v1/User/Delete")]
        public BasicResponse DeleteUser(UserDeleteRequest userrequest)
        {
            return _userService.DeleteUser(userrequest);
        }
        [HttpPost]
        [Route("v1/User/GetPageList")]
        public BasicResponse<List<UserInfo>> GetUserList(UserGetListRequest userrequest)
        {
            return _userService.GetUserList(userrequest);
        }
        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/User/GetAllList")]
        public BasicResponse<List<UserInfo>> GetUserList()
        {
            return _userService.GetUserList();
        }        
        [HttpPost]
        [Route("v1/User/Get")]
        public BasicResponse<UserInfo> GetUserById(UserGetRequest userrequest)
        {
            return _userService.GetUserById(userrequest);
        }
        /// <summary>
        /// 根据用户编码获取用户信息
        /// </summary>
        /// <param name="userrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/User/GetUserByCode")]
        public BasicResponse<UserInfo> GetUserByCode(UserGetByCodeRequest userrequest)
        {
            return _userService.GetUserByCode(userrequest);
        }
        /// <summary>
        /// 获取已启用的用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/User/GetEnableUser")]
        public BasicResponse<UserInfo> GetEnableUser()
        {
            return _userService.GetEnableUser();
        }
        /// <summary>
        /// 添加一个全新信息到用户表并返回成功后的用户对象(支持添加和更新操作)
        /// </summary>
        /// <param name="userrequest"></param>
        /// <returns></returns>      
        [HttpPost]
        [Route("v1/User/AddUserEx")]
        public BasicResponse<UserInfo> AddUserEx(UserAddRequest userrequest)
        {
            return _userService.AddUserEx(userrequest);
        }
        /// <summary>
        /// 获取用户对应的菜单信息     
        /// </summary>    
        /// <returns>菜单列表</returns> 
        [HttpPost]
        [Route("v1/User/GetUserMenus")]
        public BasicResponse<List<MenuInfo>> GetUserMenus(UserMenusGetRequest userrequest)
        {
            return _userService.GetUserMenus(userrequest);
        }
        /// <summary>
        /// 获取用户对应的权限信息
        /// </summary>
        /// <param name="userrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/User/GetUserRights")]
        public BasicResponse<List<RightInfo>> GetUserRights(UserRightsGetRequest userrequest)
        {
            return _userService.GetUserRights(userrequest);
        }
        /// <summary>
        /// 启用用户
        /// </summary>
        /// <param name="userrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/User/EnableUser")]
        public BasicResponse EnableUser(UsersRequest userrequest)
        {
            return _userService.EnableUser(userrequest);
        }
        /// <summary>
        /// 禁用用户
        /// </summary>
        /// <param name="userrequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/User/DisableUser")]
        public BasicResponse DisableUser(UsersRequest userrequest)
        {
            return _userService.DisableUser(userrequest);
        }        
    }
}
