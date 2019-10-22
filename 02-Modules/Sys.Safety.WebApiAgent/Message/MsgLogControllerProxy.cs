using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent.Message
{
    public class MsgLogControllerProxy:IMsgLogService
    {
        public Basic.Framework.Web.BasicResponse<DataContract.MsgLogInfo> AddMsgLog(Sys.Safety.Request.MsgLog.MsgLogAddRequest msgLogRequest)
        {
            throw new NotImplementedException();
        }

        public Basic.Framework.Web.BasicResponse<DataContract.MsgLogInfo> UpdateMsgLog(Sys.Safety.Request.MsgLog.MsgLogUpdateRequest msgLogRequest)
        {
            throw new NotImplementedException();
        }

        public Basic.Framework.Web.BasicResponse DeleteMsgLog(Sys.Safety.Request.MsgLog.MsgLogDeleteRequest msgLogRequest)
        {
            throw new NotImplementedException();
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.MsgLogInfo>> GetMsgLogList(Sys.Safety.Request.MsgLog.MsgLogGetListRequest msgLogRequest)
        {
            throw new NotImplementedException();
        }

        public Basic.Framework.Web.BasicResponse<DataContract.MsgLogInfo> GetMsgLogById(Sys.Safety.Request.MsgLog.MsgLogGetRequest msgLogRequest)
        {
            throw new NotImplementedException();
        }
    }
}
