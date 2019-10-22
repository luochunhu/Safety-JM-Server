using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.SyncLocal;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class R_SyncLocalService : IR_SyncLocalService
    {
        private IR_SyncLocalRepository _Repository;

        public R_SyncLocalService(IR_SyncLocalRepository _Repository)
        {
            this._Repository = _Repository;
        }
        public BasicResponse<R_SyncLocalInfo> AddSyncLocal(R_SyncLocalAddRequest syncLocalRequest)
        {
            var _syncLocal = ObjectConverter.Copy<R_SyncLocalInfo, R_SyncLocalModel>(syncLocalRequest.SyncLocalInfo);
            var resultsyncLocal = _Repository.AddSyncLocal(_syncLocal);
            var syncLocalresponse = new BasicResponse<R_SyncLocalInfo>();
            syncLocalresponse.Data = ObjectConverter.Copy<R_SyncLocalModel, R_SyncLocalInfo>(resultsyncLocal);
            return syncLocalresponse;
        }
        public BasicResponse<R_SyncLocalInfo> UpdateSyncLocal(R_SyncLocalUpdateRequest syncLocalRequest)
        {
            var _syncLocal = ObjectConverter.Copy<R_SyncLocalInfo, R_SyncLocalModel>(syncLocalRequest.SyncLocalInfo);
            _Repository.UpdateSyncLocal(_syncLocal);
            var syncLocalresponse = new BasicResponse<R_SyncLocalInfo>();
            syncLocalresponse.Data = ObjectConverter.Copy<R_SyncLocalModel, R_SyncLocalInfo>(_syncLocal);
            return syncLocalresponse;
        }
        public BasicResponse DeleteSyncLocal(R_SyncLocalDeleteRequest syncLocalRequest)
        {
            _Repository.DeleteSyncLocal(syncLocalRequest.Id);
            var syncLocalresponse = new BasicResponse();
            return syncLocalresponse;
        }
        public BasicResponse<List<R_SyncLocalInfo>> GetSyncLocalList(R_SyncLocalGetListRequest syncLocalRequest)
        {
            var syncLocalresponse = new BasicResponse<List<R_SyncLocalInfo>>();
            syncLocalRequest.PagerInfo.PageIndex = syncLocalRequest.PagerInfo.PageIndex - 1;
            if (syncLocalRequest.PagerInfo.PageIndex < 0)
            {
                syncLocalRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var syncLocalModelLists = _Repository.GetSyncLocalList(syncLocalRequest.PagerInfo.PageIndex, syncLocalRequest.PagerInfo.PageSize, out rowcount);
            var syncLocalInfoLists = new List<R_SyncLocalInfo>();
            foreach (var item in syncLocalModelLists)
            {
                var SyncLocalInfo = ObjectConverter.Copy<R_SyncLocalModel, R_SyncLocalInfo>(item);
                syncLocalInfoLists.Add(SyncLocalInfo);
            }
            syncLocalresponse.Data = syncLocalInfoLists;
            return syncLocalresponse;
        }
        public BasicResponse<R_SyncLocalInfo> GetSyncLocalById(R_SyncLocalGetRequest syncLocalRequest)
        {
            var result = _Repository.GetSyncLocalById(syncLocalRequest.Id);
            var syncLocalInfo = ObjectConverter.Copy<R_SyncLocalModel, R_SyncLocalInfo>(result);
            var syncLocalresponse = new BasicResponse<R_SyncLocalInfo>();
            syncLocalresponse.Data = syncLocalInfo;
            return syncLocalresponse;
        }
    }
}


