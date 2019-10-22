using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IRolewebmenuauthRepository : IRepository<RolewebmenuauthModel>
    {
                RolewebmenuauthModel AddRolewebmenuauth(RolewebmenuauthModel rolewebmenuauthModel);
		        void UpdateRolewebmenuauth(RolewebmenuauthModel rolewebmenuauthModel);
	            void DeleteRolewebmenuauth(string id);
		        IList<RolewebmenuauthModel> GetRolewebmenuauthList(int pageIndex, int pageSize, out int rowCount);
				RolewebmenuauthModel GetRolewebmenuauthById(string id);
    }
}
