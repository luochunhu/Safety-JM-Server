using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class RolewebmenuauthRepository:RepositoryBase<RolewebmenuauthModel>,IRolewebmenuauthRepository
    {

                public RolewebmenuauthModel AddRolewebmenuauth(RolewebmenuauthModel rolewebmenuauthModel)
		{
		   return base.Insert(rolewebmenuauthModel);
		}
		        public void UpdateRolewebmenuauth(RolewebmenuauthModel rolewebmenuauthModel)
		{
		   base.Update(rolewebmenuauthModel);
		}
	            public void DeleteRolewebmenuauth(string id)
		{
		   base.Delete(id);
		}
		        public IList<RolewebmenuauthModel> GetRolewebmenuauthList(int pageIndex, int pageSize, out int rowCount)
		{
            var rolewebmenuauthModelLists = base.Datas;
		   rowCount = rolewebmenuauthModelLists.Count();
           return rolewebmenuauthModelLists.OrderBy(p => p.ID).Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public RolewebmenuauthModel GetRolewebmenuauthById(string id)
		{
		    RolewebmenuauthModel rolewebmenuauthModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return rolewebmenuauthModel;
		}
    }
}
