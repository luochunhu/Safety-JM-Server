using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class R_SyncLocalRepository : RepositoryBase<R_SyncLocalModel>, IR_SyncLocalRepository
    {

                public R_SyncLocalModel AddSyncLocal(R_SyncLocalModel syncLocalModel)
		{
		   return base.Insert(syncLocalModel);
		}
		        public void UpdateSyncLocal(R_SyncLocalModel syncLocalModel)
		{
		   base.Update(syncLocalModel);
		}
	            public void DeleteSyncLocal(string id)
		{
		   base.Delete(id);
		}
		        public IList<R_SyncLocalModel> GetSyncLocalList(int pageIndex, int pageSize, out int rowCount)
		{
	       var  syncLocalModelLists = base.Datas.ToList();
		   rowCount = base.Datas.Count();
           return syncLocalModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public R_SyncLocalModel GetSyncLocalById(string id)
		{
		    R_SyncLocalModel syncLocalModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return syncLocalModel;
		}
    }
}
