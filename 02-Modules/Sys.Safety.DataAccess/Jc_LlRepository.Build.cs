using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class Jc_LlRepository:RepositoryBase<Jc_LlModel>,IJc_LlRepository
    {

                public Jc_LlModel AddJc_Ll(Jc_LlModel jc_LlModel)
		{
		   return base.Insert(jc_LlModel);
		}
		        public void UpdateJc_Ll(Jc_LlModel jc_LlModel)
		{
		   base.Update(jc_LlModel);
		}
	            public void DeleteJc_Ll(string id)
		{
		   base.Delete(id);
		}
		        public IList<Jc_LlModel> GetJc_LlList(int pageIndex, int pageSize, out int rowCount)
		{
            var jc_LlModelLists = base.Datas.ToList();
		   rowCount = jc_LlModelLists.Count();
           return jc_LlModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public Jc_LlModel GetJc_LlById(string id)
		{
		    Jc_LlModel jc_LlModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return jc_LlModel;
		}
    }
}
