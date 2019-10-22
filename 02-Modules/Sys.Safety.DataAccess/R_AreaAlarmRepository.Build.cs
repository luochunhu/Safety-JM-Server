using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class R_AreaAlarmRepository:RepositoryBase<R_AreaAlarmModel>,IR_AreaAlarmRepository
    {

                public R_AreaAlarmModel AddAreaAlarm(R_AreaAlarmModel areaAlarmModel)
		{
		   return base.Insert(areaAlarmModel);
		}
		        public void UpdateAreaAlarm(R_AreaAlarmModel areaAlarmModel)
		{
		   base.Update(areaAlarmModel);
		}
	            public void DeleteAreaAlarm(string id)
		{
		   base.Delete(id);
		}
		        public IList<R_AreaAlarmModel> GetAreaAlarmList(int pageIndex, int pageSize, out int rowCount)
		{
	       var  areaAlarmModelLists = base.Datas.ToList();
		   rowCount = base.Datas.Count();
           return areaAlarmModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public R_AreaAlarmModel GetAreaAlarmById(string id)
		{
		    R_AreaAlarmModel areaAlarmModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return areaAlarmModel;
		}
    }
}
