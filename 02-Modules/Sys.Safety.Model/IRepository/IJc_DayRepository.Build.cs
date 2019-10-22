using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IJc_DayRepository : IRepository<Jc_DayModel>
    {
                Jc_DayModel AddJc_Day(Jc_DayModel jc_DayModel);
		        void UpdateJc_Day(Jc_DayModel jc_DayModel);
	            void DeleteJc_Day(string id);
		        IList<Jc_DayModel> GetJc_DayList(int pageIndex, int pageSize, out int rowCount);
				Jc_DayModel GetJc_DayById(string id);
    }
}
