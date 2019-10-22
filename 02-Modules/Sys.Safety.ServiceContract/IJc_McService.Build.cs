using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Jc_Mc;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IJc_McService
    {  
	            BasicResponse<Jc_McInfo> AddJc_Mc(Jc_McAddRequest jc_Mcrequest);		
		        BasicResponse<Jc_McInfo> UpdateJc_Mc(Jc_McUpdateRequest jc_Mcrequest);	 
		        BasicResponse DeleteJc_Mc(Jc_McDeleteRequest jc_Mcrequest);
		        BasicResponse<List<Jc_McInfo>> GetJc_McList(Jc_McGetListRequest jc_Mcrequest);
		         BasicResponse<Jc_McInfo> GetJc_McById(Jc_McGetRequest jc_Mcrequest);	
    }
}

