using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Jc_Kd;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IJc_KdService
    {  
	            BasicResponse<Jc_KdInfo> AddJc_Kd(Jc_KdAddRequest jc_Kdrequest);		
		        BasicResponse<Jc_KdInfo> UpdateJc_Kd(Jc_KdUpdateRequest jc_Kdrequest);	 
		        BasicResponse DeleteJc_Kd(Jc_KdDeleteRequest jc_Kdrequest);
		        BasicResponse<List<Jc_KdInfo>> GetJc_KdList(Jc_KdGetListRequest jc_Kdrequest);
		         BasicResponse<Jc_KdInfo> GetJc_KdById(Jc_KdGetRequest jc_Kdrequest);	
    }
}

