using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IRolefieldsRepository : IRepository<RolefieldsModel>
    {
                RolefieldsModel AddRolefields(RolefieldsModel rolefieldsModel);
		        void UpdateRolefields(RolefieldsModel rolefieldsModel);
	            void DeleteRolefields(string id);
		        IList<RolefieldsModel> GetRolefieldsList(int pageIndex, int pageSize, out int rowCount);
				RolefieldsModel GetRolefieldsById(string id);
    }
}
