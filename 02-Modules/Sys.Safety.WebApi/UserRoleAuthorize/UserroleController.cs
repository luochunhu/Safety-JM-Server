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
using Sys.Safety.Request.Userrole;

namespace Sys.Safety.WebApi
{
    /// <summary>
    /// 用户与角色关联 WebApi接口
    /// </summary>
    public class UserroleController : Basic.Framework.Web.WebApi.BasicApiController, IUserroleService
    {
        static UserroleController()
        {

        }
        IUserroleService _userroleService = ServiceFactory.Create<IUserroleService>();

        [HttpPost]
        [Route("v1/Userrole/Add")]
        public BasicResponse<UserroleInfo> AddUserrole(UserroleAddRequest userrolerequest)
        {
            return _userroleService.AddUserrole(userrolerequest);
        }
        [HttpPost]
        [Route("v1/Userrole/Update")]
        public BasicResponse<UserroleInfo> UpdateUserrole(UserroleUpdateRequest userrolerequest)
        {
            return _userroleService.UpdateUserrole(userrolerequest);
        }
        [HttpPost]
        [Route("v1/Userrole/Delete")]
        public BasicResponse DeleteUserrole(UserroleDeleteRequest userrolerequest)
        {
            return _userroleService.DeleteUserrole(userrolerequest);
        }
        /// <summary>
        /// 根据角色ID删除用户与角色的关联关系
        /// </summary>
        /// <param name="userrolerequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Userrole/DeleteUserroleByRoleId")]
        public BasicResponse DeleteUserroleByRoleId(UserroleDeleteByRoleIdRequest userrolerequest)
        {
            return _userroleService.DeleteUserroleByRoleId(userrolerequest);
        }
        /// <summary>
        /// 根据用户ID删除用户与角色的关联关系
        /// </summary>
        /// <param name="userrolerequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Userrole/DeleteUserroleByUserId")]
        public BasicResponse DeleteUserroleByUserId(UserroleDeleteByUserIdRequest userrolerequest)
        {
            return _userroleService.DeleteUserroleByUserId(userrolerequest);
        }
        [HttpPost]
        [Route("v1/Userrole/GetPageList")]
        public BasicResponse<List<UserroleInfo>> GetUserroleList(UserroleGetListRequest userrolerequest)
        {
            return _userroleService.GetUserroleList(userrolerequest);
        }
        [HttpPost]
        [Route("v1/Userrole/GetAllList")]
        public BasicResponse<List<UserroleInfo>> GetUserroleList()
        {
            return _userroleService.GetUserroleList();
        }
        [HttpPost]
        [Route("v1/Userrole/Get")]
        public BasicResponse<UserroleInfo> GetUserroleById(UserroleGetRequest userrolerequest)
        {
            return _userroleService.GetUserroleById(userrolerequest);
        }
        /// <summary>
        /// 批量设置用户角色
        /// 注意此接口是先删除原有，再做新增操作
        /// </summary>     
        [HttpPost]
        [Route("v1/Userrole/AddUserRoles")]
        public BasicResponse AddUserRoles(UserrolesAddRequest userrolerequest)
        {
            return _userroleService.AddUserRoles(userrolerequest);
        }
        /// <summary>
        /// 根据用户ID获取用户角色
        /// </summary>
        /// <param name="userrolerequest">用户ID</param>
        /// <returns>用户角色列表</returns>     
        [HttpPost]
        [Route("v1/Userrole/GetUserRoleByUserId")]
        public BasicResponse<List<UserroleInfo>> GetUserRoleByUserId(UserroleGetByUserIdRequest userrolerequest)
        {
            return _userroleService.GetUserRoleByUserId(userrolerequest);
        }
        /// <summary>
        /// 判断该用户是否分配角色
        /// </summary>
        /// <param name="userrolerequest">用户编号</param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Userrole/CheckUserIDExist")]
        public BasicResponse CheckUserIDExist(UserroleGetCheckUserIDExistRequest userrolerequest)
        {
            return _userroleService.CheckUserIDExist(userrolerequest);
        }
        /// <summary>
        /// 为用户分配角色
        /// </summary> 
        [HttpPost]
        [Route("v1/Userrole/ForUserAssignmentRole")]
        public BasicResponse ForUserAssignmentRole(UserroleForUserAssignmentRoleRequest userrolerequest)
        {
            return _userroleService.ForUserAssignmentRole(userrolerequest);
        }
        /// <summary>
        /// 根据用户编号获取所有的该用户所拥有的角色对象
        /// </summary>
        /// <param name="userrolerequest">用户编号</param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Userrole/GetRoleListByUserId")]
        public BasicResponse<List<RoleInfo>> GetRoleListByUserId(UserroleGetByUserIdRequest userrolerequest)
        {
            return _userroleService.GetRoleListByUserId(userrolerequest);
        }
    }
}
