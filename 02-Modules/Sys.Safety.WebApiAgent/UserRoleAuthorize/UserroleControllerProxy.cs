using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Userrole;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent.UserRoleAuthorize
{
    public class UserroleControllerProxy : BaseProxy, IUserroleService
    {

        public BasicResponse<UserroleInfo> AddUserrole(UserroleAddRequest userrolerequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Userrole/Add?token=" + Token, JSONHelper.ToJSONString(userrolerequest));
            return JSONHelper.ParseJSONString<BasicResponse<UserroleInfo>>(responseStr);
        }
        public BasicResponse<UserroleInfo> UpdateUserrole(UserroleUpdateRequest userrolerequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Userrole/Update?token=" + Token, JSONHelper.ToJSONString(userrolerequest));
            return JSONHelper.ParseJSONString<BasicResponse<UserroleInfo>>(responseStr);
        }
        public BasicResponse DeleteUserrole(UserroleDeleteRequest userrolerequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Userrole/Delete?token=" + Token, JSONHelper.ToJSONString(userrolerequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        /// <summary>
        /// 根据角色ID删除用户与角色的关联关系
        /// </summary>
        /// <param name="userrolerequest"></param>
        /// <returns></returns>        
        public BasicResponse DeleteUserroleByRoleId(UserroleDeleteByRoleIdRequest userrolerequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Userrole/DeleteUserroleByRoleId?token=" + Token, JSONHelper.ToJSONString(userrolerequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        /// <summary>
        /// 根据用户ID删除用户与角色的关联关系
        /// </summary>
        /// <param name="userrolerequest"></param>
        /// <returns></returns>        
        public BasicResponse DeleteUserroleByUserId(UserroleDeleteByUserIdRequest userrolerequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Userrole/DeleteUserroleByUserId?token=" + Token, JSONHelper.ToJSONString(userrolerequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        public BasicResponse<List<UserroleInfo>> GetUserroleList(UserroleGetListRequest userrolerequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Userrole/GetPageList?token=" + Token, JSONHelper.ToJSONString(userrolerequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<UserroleInfo>>>(responseStr);
        }
        public BasicResponse<List<UserroleInfo>> GetUserroleList()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Userrole/GetAllList?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<UserroleInfo>>>(responseStr);
        }
        public BasicResponse<UserroleInfo> GetUserroleById(UserroleGetRequest userrolerequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Userrole/Get?token=" + Token, JSONHelper.ToJSONString(userrolerequest));
            return JSONHelper.ParseJSONString<BasicResponse<UserroleInfo>>(responseStr);
        }
        /// <summary>
        /// 批量设置用户角色
        /// 注意此接口是先删除原有，再做新增操作
        /// </summary>    
        public BasicResponse AddUserRoles(UserrolesAddRequest userrolerequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Userrole/AddUserRoles?token=" + Token, JSONHelper.ToJSONString(userrolerequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        /// <summary>
        /// 根据用户ID获取用户角色
        /// </summary>
        /// <param name="userrolerequest">用户ID</param>
        /// <returns>用户角色列表</returns>     
        public BasicResponse<List<UserroleInfo>> GetUserRoleByUserId(UserroleGetByUserIdRequest userrolerequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Userrole/GetUserRoleByUserId?token=" + Token, JSONHelper.ToJSONString(userrolerequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<UserroleInfo>>>(responseStr);
        }
        /// <summary>
        /// 判断该用户是否分配角色
        /// </summary>
        /// <param name="userrolerequest">用户编号</param>
        /// <returns></returns>
        public BasicResponse CheckUserIDExist(UserroleGetCheckUserIDExistRequest userrolerequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Userrole/CheckUserIDExist?token=" + Token, JSONHelper.ToJSONString(userrolerequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        /// <summary>
        /// 为用户分配角色
        /// </summary>
        public BasicResponse ForUserAssignmentRole(UserroleForUserAssignmentRoleRequest userrolerequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Userrole/ForUserAssignmentRole?token=" + Token, JSONHelper.ToJSONString(userrolerequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        /// <summary>
        /// 根据用户编号获取所有的该用户所拥有的角色对象
        /// </summary>
        /// <param name="userrolerequest">用户编号</param>
        /// <returns></returns>
        public BasicResponse<List<RoleInfo>> GetRoleListByUserId(UserroleGetByUserIdRequest userrolerequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Userrole/GetRoleListByUserId?token=" + Token, JSONHelper.ToJSONString(userrolerequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<RoleInfo>>>(responseStr);
        }
    }
}
