using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class UserroleRepository : RepositoryBase<UserroleModel>, IUserroleRepository
    {

        public UserroleModel AddUserrole(UserroleModel userroleModel)
        {
            return base.Insert(userroleModel);
        }
        public void UpdateUserrole(UserroleModel userroleModel)
        {
            base.Update(userroleModel);
        }
        public void DeleteUserrole(string id)
        {
            base.Delete(id);
        }
        /// <summary>
        /// 根据角色ID删除当前角色与用户的关联关系
        /// </summary>
        /// <param name="RoleId"></param>
        public void DeleteUserroleByRoleId(string RoleId)
        {
            List<UserroleModel> RolerightList = base.Datas.Where(a => a.RoleID == RoleId).ToList();
            foreach (UserroleModel roleright in RolerightList)
            {
                base.Delete(roleright.UserRoleID);
            }
        }
        /// <summary>
        /// 根据用户ID删除用户对应的角色信息
        /// </summary>
        /// <param name="RoleId"></param>
        public void DeleteUserroleByUserId(string UserId)
        {
            List<UserroleModel> RolerightList = base.Datas.Where(a => a.UserID == UserId).ToList();
            foreach (UserroleModel roleright in RolerightList)
            {
                base.Delete(roleright.UserRoleID);
            }
        }
        public IList<UserroleModel> GetUserroleList(int pageIndex, int pageSize, out int rowCount)
        {
            var userroleModelLists = base.Datas.ToList();
            rowCount = userroleModelLists.Count();
            return userroleModelLists.OrderBy(p => p.UserRoleID).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public List<UserroleModel> GetUserroleList()
        {
            var userroleModelLists = base.Datas.ToList();           
            return userroleModelLists;
        }
        public UserroleModel GetUserroleById(string id)
        {
            UserroleModel userroleModel = base.Datas.FirstOrDefault(c => c.UserRoleID == id);
            return userroleModel;
        }
        /// <summary>
        /// 根据用户ID查找用户对应的角色信息
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<UserroleModel> GetUserroleByUserId(string UserId)
        {
            List<UserroleModel> userroleModel = base.Datas.Where(c => c.UserID == UserId).ToList();
            return userroleModel;
        }
    }
}
