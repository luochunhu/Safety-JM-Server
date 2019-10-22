using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Userright;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IUserrightService
    {  
	            BasicResponse<UserrightInfo> AddUserright(UserrightAddRequest userrightrequest);		
		        BasicResponse<UserrightInfo> UpdateUserright(UserrightUpdateRequest userrightrequest);	 
		        BasicResponse DeleteUserright(UserrightDeleteRequest userrightrequest);
		        BasicResponse<List<UserrightInfo>> GetUserrightList(UserrightGetListRequest userrightrequest);
		         BasicResponse<UserrightInfo> GetUserrightById(UserrightGetRequest userrightrequest);	
    }
}

