using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IR_SyncLocalRepository : IRepository<R_SyncLocalModel>
    {
                R_SyncLocalModel AddSyncLocal(R_SyncLocalModel syncLocalModel);
		        void UpdateSyncLocal(R_SyncLocalModel syncLocalModel);
	            void DeleteSyncLocal(string id);
		        IList<R_SyncLocalModel> GetSyncLocalList(int pageIndex, int pageSize, out int rowCount);
				R_SyncLocalModel GetSyncLocalById(string id);
    }
}
