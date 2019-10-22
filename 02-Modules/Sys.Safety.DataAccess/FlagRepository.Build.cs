using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class FlagRepository:RepositoryBase<FlagModel>,IFlagRepository
    {

                public FlagModel AddFlag(FlagModel flagModel)
		{
		   return base.Insert(flagModel);
		}
		        public void UpdateFlag(FlagModel flagModel)
		{
		   base.Update(flagModel);
		}
	            public void DeleteFlag(string id)
		{
		   base.Delete(id);
		}
		        public IList<FlagModel> GetFlagList(int pageIndex, int pageSize, out int rowCount)
		{
            var flagModelLists = base.Datas;
		   rowCount = flagModelLists.Count();
           return flagModelLists.OrderBy(p => p.ID).Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public FlagModel GetFlagById(string id)
		{
		    FlagModel flagModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return flagModel;
		}
    }
}
