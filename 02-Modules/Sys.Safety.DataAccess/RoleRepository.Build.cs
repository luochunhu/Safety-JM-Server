using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class RoleRepository : RepositoryBase<RoleModel>, IRoleRepository
    {

        public RoleModel AddRole(RoleModel roleModel)
        {
            return base.Insert(roleModel);
        }
        public void UpdateRole(RoleModel roleModel)
        {
            base.Update(roleModel);
        }
        public void DeleteRole(string id)
        {
            base.Delete(id);
        }
        public IList<RoleModel> GetRoleList(int pageIndex, int pageSize, out int rowCount)
        {
            var roleModelLists = base.Datas;
            rowCount = roleModelLists.Count();
            return roleModelLists.OrderBy(p => p.RoleID).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public List<RoleModel> GetRoleList()
        {
            var roleModelLists = base.Datas.ToList();           
            return roleModelLists;
        }
        public RoleModel GetRoleById(string id)
        {
            RoleModel roleModel = base.Datas.FirstOrDefault(c => c.RoleID == id);
            return roleModel;
        }
    }
}
