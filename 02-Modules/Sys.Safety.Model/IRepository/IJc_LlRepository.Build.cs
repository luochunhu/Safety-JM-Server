using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IJc_LlRepository : IRepository<Jc_LlModel>
    {
                Jc_LlModel AddJc_Ll(Jc_LlModel jc_LlModel);
		        void UpdateJc_Ll(Jc_LlModel jc_LlModel);
	            void DeleteJc_Ll(string id);
		        IList<Jc_LlModel> GetJc_LlList(int pageIndex, int pageSize, out int rowCount);
				Jc_LlModel GetJc_LlById(string id);
    }
}
