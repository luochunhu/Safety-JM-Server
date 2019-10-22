using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Jc_Month;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IJc_MonthService
    {  
	            BasicResponse<Jc_MonthInfo> AddJc_Month(Jc_MonthAddRequest jc_Monthrequest);		
		        BasicResponse<Jc_MonthInfo> UpdateJc_Month(Jc_MonthUpdateRequest jc_Monthrequest);	 
		        BasicResponse DeleteJc_Month(Jc_MonthDeleteRequest jc_Monthrequest);
		        BasicResponse<List<Jc_MonthInfo>> GetJc_MonthList(Jc_MonthGetListRequest jc_Monthrequest);
		         BasicResponse<Jc_MonthInfo> GetJc_MonthById(Jc_MonthGetRequest jc_Monthrequest);	
    }
}

