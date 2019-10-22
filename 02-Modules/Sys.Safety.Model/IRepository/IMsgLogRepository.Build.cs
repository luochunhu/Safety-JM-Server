using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IMsgLogRepository : IRepository<MsgLogModel>
    {
                MsgLogModel AddMsgLog(MsgLogModel msgLogModel);
		        void UpdateMsgLog(MsgLogModel msgLogModel);
	            void DeleteMsgLog(string id);
		        IList<MsgLogModel> GetMsgLogList(int pageIndex, int pageSize, out int rowCount);
				MsgLogModel GetMsgLogById(string id);
    }
}
