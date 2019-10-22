using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.MsgLog;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class MsgLogService:IMsgLogService
    {
		private IMsgLogRepository _Repository;

		public MsgLogService(IMsgLogRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<MsgLogInfo> AddMsgLog(MsgLogAddRequest msgLogRequest)
        {
            var _msgLog = ObjectConverter.Copy<MsgLogInfo, MsgLogModel>(msgLogRequest.MsgLogInfo);
            var resultmsgLog = _Repository.AddMsgLog(_msgLog);
            var msgLogresponse = new BasicResponse<MsgLogInfo>();
            msgLogresponse.Data = ObjectConverter.Copy<MsgLogModel, MsgLogInfo>(resultmsgLog);
            return msgLogresponse;
        }
				public BasicResponse<MsgLogInfo> UpdateMsgLog(MsgLogUpdateRequest msgLogRequest)
        {
            var _msgLog = ObjectConverter.Copy<MsgLogInfo, MsgLogModel>(msgLogRequest.MsgLogInfo);
            _Repository.UpdateMsgLog(_msgLog);
            var msgLogresponse = new BasicResponse<MsgLogInfo>();
            msgLogresponse.Data = ObjectConverter.Copy<MsgLogModel, MsgLogInfo>(_msgLog);  
            return msgLogresponse;
        }
				public BasicResponse DeleteMsgLog(MsgLogDeleteRequest msgLogRequest)
        {
            _Repository.DeleteMsgLog(msgLogRequest.Id);
            var msgLogresponse = new BasicResponse();            
            return msgLogresponse;
        }
				public BasicResponse<List<MsgLogInfo>> GetMsgLogList(MsgLogGetListRequest msgLogRequest)
        {
            var msgLogresponse = new BasicResponse<List<MsgLogInfo>>();
            msgLogRequest.PagerInfo.PageIndex = msgLogRequest.PagerInfo.PageIndex - 1;
            if (msgLogRequest.PagerInfo.PageIndex < 0)
            {
                msgLogRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var msgLogModelLists = _Repository.GetMsgLogList(msgLogRequest.PagerInfo.PageIndex, msgLogRequest.PagerInfo.PageSize, out rowcount);
            var msgLogInfoLists = new List<MsgLogInfo>();
            foreach (var item in msgLogModelLists)
            {
                var MsgLogInfo = ObjectConverter.Copy<MsgLogModel, MsgLogInfo>(item);
                msgLogInfoLists.Add(MsgLogInfo);
            }
            msgLogresponse.Data = msgLogInfoLists;
            return msgLogresponse;
        }
				public BasicResponse<MsgLogInfo> GetMsgLogById(MsgLogGetRequest msgLogRequest)
        {
            var result = _Repository.GetMsgLogById(msgLogRequest.Id);
            var msgLogInfo = ObjectConverter.Copy<MsgLogModel, MsgLogInfo>(result);
            var msgLogresponse = new BasicResponse<MsgLogInfo>();
            msgLogresponse.Data = msgLogInfo;
            return msgLogresponse;
        }
	}
}


