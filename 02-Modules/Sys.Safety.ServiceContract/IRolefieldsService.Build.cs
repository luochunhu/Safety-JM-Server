using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Rolefields;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IRolefieldsService
    {  
	            BasicResponse<RolefieldsInfo> AddRolefields(RolefieldsAddRequest rolefieldsrequest);		
		        BasicResponse<RolefieldsInfo> UpdateRolefields(RolefieldsUpdateRequest rolefieldsrequest);	 
		        BasicResponse DeleteRolefields(RolefieldsDeleteRequest rolefieldsrequest);
		        BasicResponse<List<RolefieldsInfo>> GetRolefieldsList(RolefieldsGetListRequest rolefieldsrequest);
		         BasicResponse<RolefieldsInfo> GetRolefieldsById(RolefieldsGetRequest rolefieldsrequest);	
    }
}

