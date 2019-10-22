using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class FklibRepository:RepositoryBase<FklibModel>,IFklibRepository
    {

                public FklibModel AddFklib(FklibModel fklibModel)
		{
		   return base.Insert(fklibModel);
		}
		        public void UpdateFklib(FklibModel fklibModel)
		{
		   base.Update(fklibModel);
		}
	            public void DeleteFklib(string id)
		{
		   base.Delete(id);
		}
		        public IList<FklibModel> GetFklibList(int pageIndex, int pageSize, out int rowCount)
		{
            var fklibModelLists = base.Datas.ToList();
		   rowCount = fklibModelLists.Count();
           return fklibModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public FklibModel GetFklibById(string id)
		{
		    FklibModel fklibModel = base.Datas.FirstOrDefault(c => c.FKLibID == id);
            return fklibModel;
		}
    }
}
