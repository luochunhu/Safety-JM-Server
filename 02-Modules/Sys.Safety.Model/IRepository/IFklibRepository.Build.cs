using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IFklibRepository : IRepository<FklibModel>
    {
                FklibModel AddFklib(FklibModel fklibModel);
		        void UpdateFklib(FklibModel fklibModel);
	            void DeleteFklib(string id);
		        IList<FklibModel> GetFklibList(int pageIndex, int pageSize, out int rowCount);
				FklibModel GetFklibById(string id);
    }
}
