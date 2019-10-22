using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.SyncLocal;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IR_SyncLocalService
    {  
	            BasicResponse<R_SyncLocalInfo> AddSyncLocal(R_SyncLocalAddRequest syncLocalRequest);
                BasicResponse<R_SyncLocalInfo> UpdateSyncLocal(R_SyncLocalUpdateRequest syncLocalRequest);
                BasicResponse DeleteSyncLocal(R_SyncLocalDeleteRequest syncLocalRequest);
                BasicResponse<List<R_SyncLocalInfo>> GetSyncLocalList(R_SyncLocalGetListRequest syncLocalRequest);
                BasicResponse<R_SyncLocalInfo> GetSyncLocalById(R_SyncLocalGetRequest syncLocalRequest);	
    }
}

