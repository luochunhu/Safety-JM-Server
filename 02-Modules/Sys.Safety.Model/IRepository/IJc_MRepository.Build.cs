using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IJc_MRepository : IRepository<Jc_MModel>
    {
                Jc_MModel AddJc_M(Jc_MModel jc_MModel);
		        void UpdateJc_M(Jc_MModel jc_MModel);
	            void DeleteJc_M(string id);
		        IList<Jc_MModel> GetJc_MList(int pageIndex, int pageSize, out int rowCount);
				Jc_MModel GetJc_MById(string id);
    }
}
