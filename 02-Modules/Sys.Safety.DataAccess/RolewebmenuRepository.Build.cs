using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class RolewebmenuRepository:RepositoryBase<RolewebmenuModel>,IRolewebmenuRepository
    {

                public RolewebmenuModel AddRolewebmenu(RolewebmenuModel rolewebmenuModel)
		{
		   return base.Insert(rolewebmenuModel);
		}
		        public void UpdateRolewebmenu(RolewebmenuModel rolewebmenuModel)
		{
		   base.Update(rolewebmenuModel);
		}
	            public void DeleteRolewebmenu(string id)
		{
		   base.Delete(id);
		}
		        public IList<RolewebmenuModel> GetRolewebmenuList(int pageIndex, int pageSize, out int rowCount)
		{
            var rolewebmenuModelLists = base.Datas.ToList();
		   rowCount = rolewebmenuModelLists.Count();
           return rolewebmenuModelLists.OrderBy(p => p.ID).Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public RolewebmenuModel GetRolewebmenuById(string id)
		{
		    RolewebmenuModel rolewebmenuModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return rolewebmenuModel;
		}
    }
}
