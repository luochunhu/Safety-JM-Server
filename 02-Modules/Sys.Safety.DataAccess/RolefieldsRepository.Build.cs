using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class RolefieldsRepository:RepositoryBase<RolefieldsModel>,IRolefieldsRepository
    {

                public RolefieldsModel AddRolefields(RolefieldsModel rolefieldsModel)
		{
		   return base.Insert(rolefieldsModel);
		}
		        public void UpdateRolefields(RolefieldsModel rolefieldsModel)
		{
		   base.Update(rolefieldsModel);
		}
	            public void DeleteRolefields(string id)
		{
		   base.Delete(id);
		}
		        public IList<RolefieldsModel> GetRolefieldsList(int pageIndex, int pageSize, out int rowCount)
		{
            var rolefieldsModelLists = base.Datas;
		   rowCount = rolefieldsModelLists.Count();
           return rolefieldsModelLists.OrderBy(p => p.ID).Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public RolefieldsModel GetRolefieldsById(string id)
		{
		    RolefieldsModel rolefieldsModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return rolefieldsModel;
		}
    }
}
