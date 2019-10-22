using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IJc_McRepository : IRepository<Jc_McModel>
    {
                Jc_McModel AddJc_Mc(Jc_McModel jc_McModel);
		        void UpdateJc_Mc(Jc_McModel jc_McModel);
	            void DeleteJc_Mc(string id);
		        IList<Jc_McModel> GetJc_McList(int pageIndex, int pageSize, out int rowCount);
				Jc_McModel GetJc_McById(string id);
    }
}
