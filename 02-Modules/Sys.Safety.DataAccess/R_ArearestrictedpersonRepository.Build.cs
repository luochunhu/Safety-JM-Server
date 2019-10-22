using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class R_ArearestrictedpersonRepository:RepositoryBase<R_ArearestrictedpersonModel>,IR_ArearestrictedpersonRepository
    {

                public R_ArearestrictedpersonModel AddArearestrictedperson(R_ArearestrictedpersonModel arearestrictedpersonModel)
		{
		   return base.Insert(arearestrictedpersonModel);
		}
		        public void UpdateArearestrictedperson(R_ArearestrictedpersonModel arearestrictedpersonModel)
		{
		   base.Update(arearestrictedpersonModel);
		}
	            public void DeleteArearestrictedperson(string id)
		{
		   base.Delete(id);
		}
		        public IList<R_ArearestrictedpersonModel> GetArearestrictedpersonList(int pageIndex, int pageSize, out int rowCount)
		{
	       var  arearestrictedpersonModelLists = base.Datas.ToList();
		   rowCount = base.Datas.Count();
           return arearestrictedpersonModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public R_ArearestrictedpersonModel GetArearestrictedpersonById(string id)
		{
		    R_ArearestrictedpersonModel arearestrictedpersonModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return arearestrictedpersonModel;
		}
    }
}
