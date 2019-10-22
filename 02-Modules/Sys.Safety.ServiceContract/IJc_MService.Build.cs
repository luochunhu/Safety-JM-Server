using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Jc_M;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IJc_MService
    {  
	            BasicResponse<Jc_MInfo> AddJc_M(Jc_MAddRequest jc_Mrequest);		
		        BasicResponse<Jc_MInfo> UpdateJc_M(Jc_MUpdateRequest jc_Mrequest);	 
		        BasicResponse DeleteJc_M(Jc_MDeleteRequest jc_Mrequest);
		        BasicResponse<List<Jc_MInfo>> GetJc_MList(Jc_MGetListRequest jc_Mrequest);
		         BasicResponse<Jc_MInfo> GetJc_MById(Jc_MGetRequest jc_Mrequest);	
    }
}

