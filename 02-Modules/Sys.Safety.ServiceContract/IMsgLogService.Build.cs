using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.MsgLog;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IMsgLogService
    {  
	            BasicResponse<MsgLogInfo> AddMsgLog(MsgLogAddRequest msgLogRequest);		
		        BasicResponse<MsgLogInfo> UpdateMsgLog(MsgLogUpdateRequest msgLogRequest);	 
		        BasicResponse DeleteMsgLog(MsgLogDeleteRequest msgLogRequest);
		        BasicResponse<List<MsgLogInfo>> GetMsgLogList(MsgLogGetListRequest msgLogRequest);
		         BasicResponse<MsgLogInfo> GetMsgLogById(MsgLogGetRequest msgLogRequest);	
    }
}

