using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class R_KqbcRepository:RepositoryBase<R_KqbcModel>,IR_KqbcRepository
    {

                public R_KqbcModel AddKqbc(R_KqbcModel kqbcModel)
		{
		   return base.Insert(kqbcModel);
		}
		        public void UpdateKqbc(R_KqbcModel kqbcModel)
		{
		   base.Update(kqbcModel);
		}
	            public void DeleteKqbc(string id)
		{
		   base.Delete(id);
		}
		        public IList<R_KqbcModel> GetKqbcList(int pageIndex, int pageSize, out int rowCount)
		{
	       var  kqbcModelLists = base.Datas.ToList();
		   rowCount = base.Datas.Count();
           return kqbcModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
                public IList<R_KqbcModel> GetAllKqbcList()
                {
                    var kqbcModelLists = base.Datas.ToList();                    
                    return kqbcModelLists;
                }
				public R_KqbcModel GetKqbcById(string id)
		{
		    R_KqbcModel kqbcModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return kqbcModel;
		}
    }
}
