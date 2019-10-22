using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IUserroleRepository : IRepository<UserroleModel>
    {
        UserroleModel AddUserrole(UserroleModel userroleModel);
        void UpdateUserrole(UserroleModel userroleModel);
        void DeleteUserrole(string id);
        /// <summary>
        /// 根据角色ID删除当前角色与用户的关联关系
        /// </summary>
        /// <param name="RoleId"></param>
        void DeleteUserroleByRoleId(string RoleId);
        /// <summary>
        /// 根据用户ID删除用户对应的角色信息
        /// </summary>
        /// <param name="RoleId"></param>
        void DeleteUserroleByUserId(string UserId);
        IList<UserroleModel> GetUserroleList(int pageIndex, int pageSize, out int rowCount);
        List<UserroleModel> GetUserroleList();
        UserroleModel GetUserroleById(string id);
        /// <summary>
        /// 根据用户ID查找用户对应的角色信息
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        List<UserroleModel> GetUserroleByUserId(string UserId);
    }
}
