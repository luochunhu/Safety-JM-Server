using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IJc_YearRepository : IRepository<Jc_YearModel>
    {
                Jc_YearModel AddJc_Year(Jc_YearModel jc_YearModel);
		        void UpdateJc_Year(Jc_YearModel jc_YearModel);
	            void DeleteJc_Year(string id);
		        IList<Jc_YearModel> GetJc_YearList(int pageIndex, int pageSize, out int rowCount);
				Jc_YearModel GetJc_YearById(string id);
    }
}
