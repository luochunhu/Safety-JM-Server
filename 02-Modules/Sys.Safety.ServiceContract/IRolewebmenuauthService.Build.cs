using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Rolewebmenuauth;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IRolewebmenuauthService
    {  
	            BasicResponse<RolewebmenuauthInfo> AddRolewebmenuauth(RolewebmenuauthAddRequest rolewebmenuauthrequest);		
		        BasicResponse<RolewebmenuauthInfo> UpdateRolewebmenuauth(RolewebmenuauthUpdateRequest rolewebmenuauthrequest);	 
		        BasicResponse DeleteRolewebmenuauth(RolewebmenuauthDeleteRequest rolewebmenuauthrequest);
		        BasicResponse<List<RolewebmenuauthInfo>> GetRolewebmenuauthList(RolewebmenuauthGetListRequest rolewebmenuauthrequest);
		         BasicResponse<RolewebmenuauthInfo> GetRolewebmenuauthById(RolewebmenuauthGetRequest rolewebmenuauthrequest);	
    }
}

