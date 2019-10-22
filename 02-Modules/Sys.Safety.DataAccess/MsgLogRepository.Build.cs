using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class MsgLogRepository:RepositoryBase<MsgLogModel>,IMsgLogRepository
    {

                public MsgLogModel AddMsgLog(MsgLogModel msgLogModel)
		{
		   return base.Insert(msgLogModel);
		}
		        public void UpdateMsgLog(MsgLogModel msgLogModel)
		{
		   base.Update(msgLogModel);
		}
	            public void DeleteMsgLog(string id)
		{
		   base.Delete(id);
		}
		        public IList<MsgLogModel> GetMsgLogList(int pageIndex, int pageSize, out int rowCount)
		{
	       var  msgLogModelLists = base.Datas.ToList();
		   rowCount = base.Datas.Count();
           return msgLogModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public MsgLogModel GetMsgLogById(string id)
		{
		    MsgLogModel msgLogModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return msgLogModel;
		}
    }
}
