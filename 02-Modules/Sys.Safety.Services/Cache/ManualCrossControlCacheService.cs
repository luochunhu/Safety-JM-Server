using Sys.Safety.ServiceContract.Cache;
using System.Collections.Generic;
using System.Linq;
using Basic.Framework.Web;
using Sys.Safety.Request.Cache;
using Sys.Safety.Cache.Safety;
using Sys.Safety.DataContract;

namespace Sys.Safety.Services.Cache
{
    /// <summary>
    /// 作者:
    /// 时间:2017-05-24
    /// 描述:手动交叉控制缓存业务
    /// 修改记录
    /// 2017-05-24
    /// </summary>
    public class ManualCrossControlCacheService : IManualCrossControlCacheService
    {
        public BasicResponse AddManualCrossControlCache(ManualCrossControlCacheAddRequest manualCrossControlCacheRequest)
        {
            ManualCrossControlCache.ManualCrossControlCahceInstance.AddItem(manualCrossControlCacheRequest.ManualCrossControlInfo);
            return new BasicResponse();
        }

        public BasicResponse BatchAddManualCrossControlCache(ManualCrossControlCacheBatchAddRequest manualCrossControlCacheRequest)
        {
            ManualCrossControlCache.ManualCrossControlCahceInstance.AddItems(manualCrossControlCacheRequest.ManualCrossControlInfos);
            return new BasicResponse();
        }

        public BasicResponse BatchDeleteManualCrossControlCache(ManualCrossControlCacheBatchDeleteRequest manualCrossControlCacheRequest)
        {
            ManualCrossControlCache.ManualCrossControlCahceInstance.DeleteItems(manualCrossControlCacheRequest.ManualCrossControlInfos);
            return new BasicResponse();
        }

        public BasicResponse BatchUpdateManualCrossControlCache(ManualCrossControlCacheBatchUpdateRequest manualCrossControlCacheRequest)
        {
            ManualCrossControlCache.ManualCrossControlCahceInstance.UpdateItems(manualCrossControlCacheRequest.ManualCrossControlInfos);
            return new BasicResponse();
        }

        public BasicResponse DeleteManualCrossControlCache(ManualCrossControlCacheDeleteRequest manualCrossControlCacheRequest)
        {
            ManualCrossControlCache.ManualCrossControlCahceInstance.DeleteItem(manualCrossControlCacheRequest.ManualCrossControlInfo);
            return new BasicResponse();
        }

        public BasicResponse<List<Jc_JcsdkzInfo>> GetAllManualCrossControlCache(ManualCrossControlCacheGetAllRequest manualCrossControlCacheRequest)
        {
            var manualCrossControlCache = ManualCrossControlCache.ManualCrossControlCahceInstance.Query();
            var manualCrossControlCacheResponse = new BasicResponse<List<Jc_JcsdkzInfo>>();
            manualCrossControlCacheResponse.Data = manualCrossControlCache;
            return manualCrossControlCacheResponse;
        }

        public BasicResponse<Jc_JcsdkzInfo> GetByKeyManualCrossControlCache(ManualCrossControlCacheGetByKeyRequest manualCrossControlCacheRequest)
        {
            var manualCrossControlCache = ManualCrossControlCache.ManualCrossControlCahceInstance.Query(manualcontrol => manualcontrol.ID == manualCrossControlCacheRequest.ManualCrosControlId).FirstOrDefault();
            var manualCrossControlCacheResponse = new BasicResponse<Jc_JcsdkzInfo>();
            manualCrossControlCacheResponse.Data = manualCrossControlCache;
            return manualCrossControlCacheResponse;
        }

        public BasicResponse<List<Jc_JcsdkzInfo>> GetManualCrossControlCache(ManualCrossControlCacheGetByConditionRequest manualCrossControlCacheRequest)
        {
            var manualCrossControlCache = ManualCrossControlCache.ManualCrossControlCahceInstance.Query(manualCrossControlCacheRequest.Predicate);
            var manualCrossControlCacheResponse = new BasicResponse<List<Jc_JcsdkzInfo>>();
            manualCrossControlCacheResponse.Data = manualCrossControlCache;
            return manualCrossControlCacheResponse;
        }

        public BasicResponse<bool> IsExistsManualCrossControlCache(ManualCrossControlCacheIsExistsRequest manualCrossControlCacheRequest)
        {
            var manualCrossControlCache = ManualCrossControlCache.ManualCrossControlCahceInstance.Query(manualcontrol => manualcontrol.ID == manualCrossControlCacheRequest.ManualCrosControlId).FirstOrDefault();
            var manualCrossControlCacheResponse = new BasicResponse<bool>();
            manualCrossControlCacheResponse.Data = manualCrossControlCache!=null;
            return manualCrossControlCacheResponse;
        }

        public BasicResponse LoadManualCrossControlCache(ManualCrossControlCacheLoadRequest manualCrossControlCacheRequest)
        {
            ManualCrossControlCache.ManualCrossControlCahceInstance.Load();
            return new BasicResponse();
        }

        public BasicResponse UpdateManualCrossControlCache(ManualCrossControlCacheUpdateRequest manualCrossControlCacheRequest)
        {
            ManualCrossControlCache.ManualCrossControlCahceInstance.UpdateItem(manualCrossControlCacheRequest.ManualCrossControlInfo);
            return new BasicResponse();
        }
    }
}
