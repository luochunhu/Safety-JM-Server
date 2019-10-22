using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class Jc_BzRepository:RepositoryBase<Jc_BzModel>,IJc_BzRepository
    {

                public Jc_BzModel AddJc_Bz(Jc_BzModel jc_BzModel)
		{
		   return base.Insert(jc_BzModel);
		}
		        public void UpdateJc_Bz(Jc_BzModel jc_BzModel)
		{
		   base.Update(jc_BzModel);
		}
	            public void DeleteJc_Bz(string id)
		{
		   base.Delete(id);
		}
		        public IList<Jc_BzModel> GetJc_BzList(int pageIndex, int pageSize, out int rowCount)
		{
            var jc_BzModelLists = base.Datas.ToList();
		   rowCount = jc_BzModelLists.Count();
           return jc_BzModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public Jc_BzModel GetJc_BzById(string id)
		{
		    Jc_BzModel jc_BzModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return jc_BzModel;
		}
    }
}
