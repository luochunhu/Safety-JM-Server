using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IRolewebmenuRepository : IRepository<RolewebmenuModel>
    {
                RolewebmenuModel AddRolewebmenu(RolewebmenuModel rolewebmenuModel);
		        void UpdateRolewebmenu(RolewebmenuModel rolewebmenuModel);
	            void DeleteRolewebmenu(string id);
		        IList<RolewebmenuModel> GetRolewebmenuList(int pageIndex, int pageSize, out int rowCount);
				RolewebmenuModel GetRolewebmenuById(string id);
    }
}
