using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Userrole;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IUserroleService
    {
        BasicResponse<UserroleInfo> AddUserrole(UserroleAddRequest userrolerequest);
        BasicResponse<UserroleInfo> UpdateUserrole(UserroleUpdateRequest userrolerequest);
        BasicResponse DeleteUserrole(UserroleDeleteRequest userrolerequest);
        /// <summary>
        /// 根据角色ID删除用户与角色的关联关系
        /// </summary>
        /// <param name="userrolerequest"></param>
        /// <returns></returns>
        BasicResponse DeleteUserroleByRoleId(UserroleDeleteByRoleIdRequest userrolerequest);
        /// <summary>
        /// 根据用户ID删除用户与角色的关联关系
        /// </summary>
        /// <param name="userrolerequest"></param>
        /// <returns></returns>
        BasicResponse DeleteUserroleByUserId(UserroleDeleteByUserIdRequest userrolerequest);
        BasicResponse<List<UserroleInfo>> GetUserroleList(UserroleGetListRequest userrolerequest);
        BasicResponse<List<UserroleInfo>> GetUserroleList();
        BasicResponse<UserroleInfo> GetUserroleById(UserroleGetRequest userrolerequest);
        /// <summary>
        /// 批量设置用户角色
        /// 注意此接口是先删除原有，再做新增操作
        /// </summary>        
        BasicResponse AddUserRoles(UserrolesAddRequest userrolerequest);
        /// <summary>
        /// 根据用户ID获取用户角色
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>用户角色列表</returns>        
        BasicResponse<List<UserroleInfo>> GetUserRoleByUserId(UserroleGetByUserIdRequest userrolerequest);
        /// <summary>
        /// 判断该用户是否分配角色
        /// </summary>
        /// <param name="userID">用户编号</param>
        /// <returns></returns>
        BasicResponse CheckUserIDExist(UserroleGetCheckUserIDExistRequest userrolerequest);
        /// <summary>
        /// 为用户分配角色
        /// </summary>       
        BasicResponse ForUserAssignmentRole(UserroleForUserAssignmentRoleRequest userrolerequest);
        /// <summary>
        /// 根据用户编号获取所有的该用户所拥有的角色对象
        /// </summary>
        /// <param name="userrolerequest">用户编号</param>
        /// <returns></returns>
        BasicResponse<List<RoleInfo>> GetRoleListByUserId(UserroleGetByUserIdRequest userrolerequest);
    }
}

