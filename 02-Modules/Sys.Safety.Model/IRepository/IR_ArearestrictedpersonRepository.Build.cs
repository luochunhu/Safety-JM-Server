using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IR_ArearestrictedpersonRepository : IRepository<R_ArearestrictedpersonModel>
    {
                R_ArearestrictedpersonModel AddArearestrictedperson(R_ArearestrictedpersonModel arearestrictedpersonModel);
		        void UpdateArearestrictedperson(R_ArearestrictedpersonModel arearestrictedpersonModel);
	            void DeleteArearestrictedperson(string id);
		        IList<R_ArearestrictedpersonModel> GetArearestrictedpersonList(int pageIndex, int pageSize, out int rowCount);
				R_ArearestrictedpersonModel GetArearestrictedpersonById(string id);
    }
}
