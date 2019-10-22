using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IJc_MonthRepository : IRepository<Jc_MonthModel>
    {
                Jc_MonthModel AddJc_Month(Jc_MonthModel jc_MonthModel);
		        void UpdateJc_Month(Jc_MonthModel jc_MonthModel);
	            void DeleteJc_Month(string id);
		        IList<Jc_MonthModel> GetJc_MonthList(int pageIndex, int pageSize, out int rowCount);
				Jc_MonthModel GetJc_MonthById(string id);
    }
}
