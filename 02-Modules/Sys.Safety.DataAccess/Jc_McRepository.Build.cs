using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class Jc_McRepository:RepositoryBase<Jc_McModel>,IJc_McRepository
    {

                public Jc_McModel AddJc_Mc(Jc_McModel jc_McModel)
		{
		   return base.Insert(jc_McModel);
		}
		        public void UpdateJc_Mc(Jc_McModel jc_McModel)
		{
		   base.Update(jc_McModel);
		}
	            public void DeleteJc_Mc(string id)
		{
		   base.Delete(id);
		}
		        public IList<Jc_McModel> GetJc_McList(int pageIndex, int pageSize, out int rowCount)
		{
            var jc_McModelLists = base.Datas.ToList();
		   rowCount = jc_McModelLists.Count();
           return jc_McModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public Jc_McModel GetJc_McById(string id)
		{
		    Jc_McModel jc_McModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return jc_McModel;
		}
    }
}
