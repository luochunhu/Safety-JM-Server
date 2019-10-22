using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IJc_RRepository : IRepository<Jc_RModel>
    {
                Jc_RModel AddJc_R(Jc_RModel jc_RModel);
		        void UpdateJc_R(Jc_RModel jc_RModel);
	            void DeleteJc_R(string id);
		        IList<Jc_RModel> GetJc_RList(int pageIndex, int pageSize, out int rowCount);
				Jc_RModel GetJc_RById(string id);
    }
}
