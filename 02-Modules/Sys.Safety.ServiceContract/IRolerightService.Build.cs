using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Roleright;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IRolerightService
    {
        BasicResponse<RolerightInfo> AddRoleright(RolerightAddRequest rolerightrequest);
        BasicResponse<RolerightInfo> UpdateRoleright(RolerightUpdateRequest rolerightrequest);
        BasicResponse DeleteRoleright(RolerightDeleteRequest rolerightrequest);
        /// <summary>
        /// 根据角色ID删除角色权限
        /// </summary>
        /// <param name="rolerightrequest"></param>
        /// <returns></returns>
        BasicResponse DeleteRolerightByRoleId(RolerightDeleteByRoleIdRequest rolerightrequest);
        /// <summary>
        /// 根据角色ID删除角色权限
        /// </summary>
        /// <param name="rolerightrequest"></param>
        /// <returns></returns>
        BasicResponse DeleteRolerightByRightId(RolerightDeleteByRightIdRequest rolerightrequest);
        BasicResponse<List<RolerightInfo>> GetRolerightList(RolerightGetListRequest rolerightrequest);
        BasicResponse<List<RolerightInfo>> GetRolerightList();
        BasicResponse<RolerightInfo> GetRolerightById(RolerightGetRequest rolerightrequest);
        /// <summary>
        /// 新增角色权限点
        /// 批量增加权限点，增加前会删除原来角色下的所有权限点
        /// </summary>
        /// <param name="roleId">角色编码</param>
        /// <param name="rightList">角色的权限点集合</param>       
        BasicResponse AddRoleRights(RolerightsAddRequest rolerightrequest);
        /// <summary>
        /// 根据角色ID获取角色对应的权限信息
        /// </summary>
        /// <param name="rolerightrequest"></param>
        /// <returns></returns>
        BasicResponse<List<RightInfo>> GetRightsByRoleId(RolerightGetByRoleIdRequest rolerightrequest);
        /// <summary>
        /// 根据角色ID获取角色对应的权限信息
        /// </summary>
        /// <param name="rolerightrequest"></param>
        /// <returns></returns>
        BasicResponse<List<RolerightInfo>> GetRolerightByRoleId(RolerightGetByRoleIdRequest rolerightrequest);
        /// <summary>
        /// 判断该角色是否分配权限
        /// </summary>
        /// <param name="roleID">角色编号</param>
        /// <returns></returns>
        BasicResponse CheckRoleIDExist(RolerightCheckExistByRoleIdRequest rolerightrequest);
        /// <summary>
        /// 为角色分配权限
        /// </summary>        
        BasicResponse ForRoleAssignmentRight(RolerightForRoleAssignmentRightRequest rolerightrequest);
    }
}

