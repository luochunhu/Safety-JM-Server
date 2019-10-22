using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class Jc_MRepository:RepositoryBase<Jc_MModel>,IJc_MRepository
    {

                public Jc_MModel AddJc_M(Jc_MModel jc_MModel)
		{
		   return base.Insert(jc_MModel);
		}
		        public void UpdateJc_M(Jc_MModel jc_MModel)
		{
		   base.Update(jc_MModel);
		}
	            public void DeleteJc_M(string id)
		{
		   base.Delete(id);
		}
		        public IList<Jc_MModel> GetJc_MList(int pageIndex, int pageSize, out int rowCount)
		{
            var jc_MModelLists = base.Datas.ToList();
		   rowCount = jc_MModelLists.Count();
           return jc_MModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public Jc_MModel GetJc_MById(string id)
		{
		    Jc_MModel jc_MModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return jc_MModel;
		}
    }
}
