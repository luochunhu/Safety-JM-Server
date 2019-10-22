using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IRoleRepository : IRepository<RoleModel>
    {
        RoleModel AddRole(RoleModel roleModel);
        void UpdateRole(RoleModel roleModel);
        void DeleteRole(string id);
        IList<RoleModel> GetRoleList(int pageIndex, int pageSize, out int rowCount);
        List<RoleModel> GetRoleList();

        RoleModel GetRoleById(string id);
    }
}
