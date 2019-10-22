using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IR_AreaAlarmRepository : IRepository<R_AreaAlarmModel>
    {
                R_AreaAlarmModel AddAreaAlarm(R_AreaAlarmModel areaAlarmModel);
		        void UpdateAreaAlarm(R_AreaAlarmModel areaAlarmModel);
	            void DeleteAreaAlarm(string id);
		        IList<R_AreaAlarmModel> GetAreaAlarmList(int pageIndex, int pageSize, out int rowCount);
				R_AreaAlarmModel GetAreaAlarmById(string id);
    }
}
