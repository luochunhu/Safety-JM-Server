using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.AreaAlarm;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IR_AreaAlarmService
    {  
	            BasicResponse<R_AreaAlarmInfo> AddAreaAlarm(R_AreaAlarmAddRequest areaAlarmRequest);		
		        BasicResponse<R_AreaAlarmInfo> UpdateAreaAlarm(R_AreaAlarmUpdateRequest areaAlarmRequest);	 
		        BasicResponse DeleteAreaAlarm(R_AreaAlarmDeleteRequest areaAlarmRequest);
		        BasicResponse<List<R_AreaAlarmInfo>> GetAreaAlarmList(R_AreaAlarmGetListRequest areaAlarmRequest);
		         BasicResponse<R_AreaAlarmInfo> GetAreaAlarmById(R_AreaAlarmGetRequest areaAlarmRequest);	
    }
}

