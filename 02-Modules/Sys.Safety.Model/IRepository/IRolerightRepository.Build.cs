using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IRolerightRepository : IRepository<RolerightModel>
    {
        RolerightModel AddRoleright(RolerightModel rolerightModel);
        void UpdateRoleright(RolerightModel rolerightModel);
        void DeleteRoleright(string id);
        IList<RolerightModel> GetRolerightList(int pageIndex, int pageSize, out int rowCount);
        List<RolerightModel> GetRolerightList();
        RolerightModel GetRolerightById(string id);
        /// <summary>
        /// 根据角色ID删除角色对应的权限绑定
        /// </summary>
        /// <param name="RoleId"></param>
        void DeleteRolerightByRoleId(string RoleId);
        /// <summary>
        /// 根据权限ID删除角色对应权限
        /// </summary>
        /// <param name="RightId"></param>
        void DeleteRolerightByRightId(string RightId);
        /// <summary>
        /// 根据角色ID获取角色对应的权限信息
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        List<RightModel> GetRightsByRoleId(string RoleId);
        /// <summary>
        /// 根据角色ID获取角色对应的权限关联信息
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        List<RolerightModel> GetRolerightByRoleId(string RoleId);
    }
}
