using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.Request.User;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent.UserRoleAuthorize
{
    public class UserControllerProxy : BaseProxy, IUserService
    {

        public BasicResponse<UserInfo> AddUser(UserAddRequest userrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/User/Add?token=" + Token, JSONHelper.ToJSONString(userrequest));
            return JSONHelper.ParseJSONString<BasicResponse<UserInfo>>(responseStr);
        }
        public BasicResponse<UserInfo> UpdateUser(UserUpdateRequest userrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/User/Update?token=" + Token, JSONHelper.ToJSONString(userrequest));
            return JSONHelper.ParseJSONString<BasicResponse<UserInfo>>(responseStr);
        }
        public BasicResponse DeleteUser(UserDeleteRequest userrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/User/Delete?token=" + Token, JSONHelper.ToJSONString(userrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        public BasicResponse<List<UserInfo>> GetUserList(UserGetListRequest userrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/User/GetPageList?token=" + Token, JSONHelper.ToJSONString(userrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<UserInfo>>>(responseStr);
        }
        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<UserInfo>> GetUserList()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/User/GetAllList?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<UserInfo>>>(responseStr);
        }
        public BasicResponse<UserInfo> GetUserById(UserGetRequest userrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/User/Get?token=" + Token, JSONHelper.ToJSONString(userrequest));
            return JSONHelper.ParseJSONString<BasicResponse<UserInfo>>(responseStr);
        }
        /// <summary>
        /// 根据用户编码获取用户信息
        /// </summary>
        /// <param name="userrequest"></param>
        /// <returns></returns>        
        public BasicResponse<UserInfo> GetUserByCode(UserGetByCodeRequest userrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/User/GetUserByCode?token=" + Token, JSONHelper.ToJSONString(userrequest));
            return JSONHelper.ParseJSONString<BasicResponse<UserInfo>>(responseStr);
        }
        /// <summary>
        /// 获取已启用的用户
        /// </summary>
        /// <returns></returns>        
        public BasicResponse<UserInfo> GetEnableUser()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/User/GetEnableUser?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<UserInfo>>(responseStr);
        }
        /// <summary>
        /// 添加一个全新信息到用户表并返回成功后的用户对象(支持添加和更新操作)
        /// </summary>
        /// <param name="userrequest"></param>
        /// <returns></returns>     
        public BasicResponse<UserInfo> AddUserEx(UserAddRequest userrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/User/AddUserEx?token=" + Token, JSONHelper.ToJSONString(userrequest));
            return JSONHelper.ParseJSONString<BasicResponse<UserInfo>>(responseStr);
        }
        /// <summary>
        /// 获取用户对应的菜单信息     
        /// </summary>    
        /// <returns>菜单列表</returns>
        public BasicResponse<List<MenuInfo>> GetUserMenus(UserMenusGetRequest userrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/User/GetUserMenus?token=" + Token, JSONHelper.ToJSONString(userrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<MenuInfo>>>(responseStr);
        }
        /// <summary>
        /// 获取用户对应的权限信息
        /// </summary>
        /// <param name="userrequest"></param>
        /// <returns></returns>        
        public BasicResponse<List<RightInfo>> GetUserRights(UserRightsGetRequest userrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/User/GetUserRights?token=" + Token, JSONHelper.ToJSONString(userrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<RightInfo>>>(responseStr);
        }
        /// <summary>
        /// 启用用户
        /// </summary>
        /// <param name="userrequest"></param>
        /// <returns></returns>        
        public BasicResponse EnableUser(UsersRequest userrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/User/EnableUser?token=" + Token, JSONHelper.ToJSONString(userrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        /// <summary>
        /// 禁用用户
        /// </summary>
        /// <param name="userrequest"></param>
        /// <returns></returns>        
        public BasicResponse DisableUser(UsersRequest userrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/User/DisableUser?token=" + Token, JSONHelper.ToJSONString(userrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }        
    }
}
