using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IJc_CsRepository : IRepository<Jc_CsModel>
    {
                Jc_CsModel AddJc_Cs(Jc_CsModel jc_CsModel);
		        void UpdateJc_Cs(Jc_CsModel jc_CsModel);
	            void DeleteJc_Cs(string id);
		        IList<Jc_CsModel> GetJc_CsList(int pageIndex, int pageSize, out int rowCount);
				Jc_CsModel GetJc_CsById(string id);
    }
}
