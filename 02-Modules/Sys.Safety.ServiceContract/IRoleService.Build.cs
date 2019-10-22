using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Role;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IRoleService
    {
        BasicResponse<RoleInfo> AddRole(RoleAddRequest rolerequest);
        BasicResponse<RoleInfo> UpdateRole(RoleUpdateRequest rolerequest);
        BasicResponse DeleteRole(RoleDeleteRequest rolerequest);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="rolerequest"></param>
        /// <returns></returns>
        BasicResponse DeleteRoles(RolesDeleteRequest rolerequest);
        BasicResponse<List<RoleInfo>> GetRoleList(RoleGetListRequest rolerequest);
        BasicResponse<List<RoleInfo>> GetRoleList();
        BasicResponse<RoleInfo> GetRoleById(RoleGetRequest rolerequest);
        /// <summary>
        /// 添加一个全新信息到角色表并返回成功后的角色对象(支持新增和更新)
        /// </summary>
        /// <param name="roleDTO"></param>
        /// <returns></returns>
        BasicResponse<RoleInfo> AddRoleEx(RoleAddRequest rolerequest);
        /// <summary>
        /// 启用角色
        /// </summary>
        /// <param name="rolerequest"></param>
        /// <returns></returns>
        BasicResponse EnableRole(RolesRequest rolerequest);
        /// <summary>
        /// 禁用角色
        /// </summary>
        /// <param name="rolerequest"></param>
        /// <returns></returns>
        BasicResponse DisableRole(RolesRequest rolerequest);

    }
}

