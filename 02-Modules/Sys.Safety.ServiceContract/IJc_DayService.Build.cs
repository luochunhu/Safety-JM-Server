using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Jc_Day;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IJc_DayService
    {  
	            BasicResponse<Jc_DayInfo> AddJc_Day(Jc_DayAddRequest jc_Dayrequest);		
		        BasicResponse<Jc_DayInfo> UpdateJc_Day(Jc_DayUpdateRequest jc_Dayrequest);	 
		        BasicResponse DeleteJc_Day(Jc_DayDeleteRequest jc_Dayrequest);
		        BasicResponse<List<Jc_DayInfo>> GetJc_DayList(Jc_DayGetListRequest jc_Dayrequest);
		         BasicResponse<Jc_DayInfo> GetJc_DayById(Jc_DayGetRequest jc_Dayrequest);	
    }
}

