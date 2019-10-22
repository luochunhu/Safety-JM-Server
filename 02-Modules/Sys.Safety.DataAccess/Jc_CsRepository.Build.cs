using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class Jc_CsRepository:RepositoryBase<Jc_CsModel>,IJc_CsRepository
    {

                public Jc_CsModel AddJc_Cs(Jc_CsModel jc_CsModel)
		{
		   return base.Insert(jc_CsModel);
		}
		        public void UpdateJc_Cs(Jc_CsModel jc_CsModel)
		{
		   base.Update(jc_CsModel);
		}
	            public void DeleteJc_Cs(string id)
		{
		   base.Delete(id);
		}
		        public IList<Jc_CsModel> GetJc_CsList(int pageIndex, int pageSize, out int rowCount)
		{
            var jc_CsModelLists = base.Datas.ToList();
		   rowCount = jc_CsModelLists.Count();
           return jc_CsModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public Jc_CsModel GetJc_CsById(string id)
		{
		    Jc_CsModel jc_CsModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return jc_CsModel;
		}
    }
}
