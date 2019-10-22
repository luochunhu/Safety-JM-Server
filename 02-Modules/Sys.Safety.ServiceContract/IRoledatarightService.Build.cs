using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Roledataright;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IRoledatarightService
    {  
	            BasicResponse<RoledatarightInfo> AddRoledataright(RoledatarightAddRequest roledatarightrequest);		
		        BasicResponse<RoledatarightInfo> UpdateRoledataright(RoledatarightUpdateRequest roledatarightrequest);	 
		        BasicResponse DeleteRoledataright(RoledatarightDeleteRequest roledatarightrequest);
		        BasicResponse<List<RoledatarightInfo>> GetRoledatarightList(RoledatarightGetListRequest roledatarightrequest);
		         BasicResponse<RoledatarightInfo> GetRoledatarightById(RoledatarightGetRequest roledatarightrequest);	
    }
}

