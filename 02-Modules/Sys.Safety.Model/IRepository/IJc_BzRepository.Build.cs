using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IJc_BzRepository : IRepository<Jc_BzModel>
    {
                Jc_BzModel AddJc_Bz(Jc_BzModel jc_BzModel);
		        void UpdateJc_Bz(Jc_BzModel jc_BzModel);
	            void DeleteJc_Bz(string id);
		        IList<Jc_BzModel> GetJc_BzList(int pageIndex, int pageSize, out int rowCount);
				Jc_BzModel GetJc_BzById(string id);
    }
}
