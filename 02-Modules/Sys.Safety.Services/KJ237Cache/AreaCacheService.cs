using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.Cache.Person;
using Sys.Safety.Cache.Safety;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Area;
using Sys.Safety.ServiceContract;
using Sys.Safety.ServiceContract.KJ237Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Services.KJ237Cache
{
    public class AreaCacheService : IAreaCacheService
    {
        public Basic.Framework.Web.BasicResponse LoadAreaCache(Sys.Safety.Request.PersonCache.AreaCacheLoadRequest AreaCacheRequest)
        {
            AreaCache.AreaCacheInstance.Load();
            IAreaRuleService areaRuleService = ServiceFactory.Create<IAreaRuleService>();
            IR_ArearestrictedpersonService r_ArearestrictedpersonService = ServiceFactory.Create<IR_ArearestrictedpersonService>();
            //加载区域定义基本信息之后，加载区域定义拓展属性
            var areaDefineList = AreaCache.AreaCacheInstance.Query();
            if (areaDefineList.Any())
            {
                //区域设备类型定义限制信息
                var areaRuleList = areaRuleService.GetAreaRuleList(new AreaRuleGetListRequest()).Data;
                var arearestrictedpersonList = r_ArearestrictedpersonService.GetArearestrictedpersonList(new Sys.Safety.Request.Arearestrictedperson.R_ArearestrictedpersonGetListRequest()).Data;
                areaDefineList.ForEach(nwModule =>
                {
                    var tempareaRuleList = areaRuleList.FindAll(p => p.Areaid == nwModule.Areaid);
                    var tempRestrictedpersonInfoList = arearestrictedpersonList.FindAll(p => p.AreaId == nwModule.Areaid);
                    nwModule.AreaRuleInfoList = tempareaRuleList;
                    nwModule.RestrictedpersonInfoList = tempRestrictedpersonInfoList;
                });
                AreaCache.AreaCacheInstance.UpdateItems(areaDefineList);
            }
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse AddAreaCache(Sys.Safety.Request.PersonCache.AreaCacheAddRequest AreaCacheRequest)
        {
            AreaCache.AreaCacheInstance.AddItem(AreaCacheRequest.AreaInfo);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse BatchAddAreaCache(Sys.Safety.Request.PersonCache.AreaCacheBatchAddRequest AreaCacheRequest)
        {
            AreaCache.AreaCacheInstance.AddItems(AreaCacheRequest.AreaInfos);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse UpdateAreaCache(Sys.Safety.Request.PersonCache.AreaCacheUpdateRequest AreaCacheRequest)
        {
            AreaCache.AreaCacheInstance.UpdateItem(AreaCacheRequest.AreaInfo);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse BatchUpdateAreaCache(Sys.Safety.Request.PersonCache.AreaCacheBatchUpdateRequest AreaCacheRequest)
        {
            AreaCache.AreaCacheInstance.UpdateItems(AreaCacheRequest.AreaInfos);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse DeleteAreaCache(Sys.Safety.Request.PersonCache.AreaCacheDeleteRequest AreaCacheRequest)
        {
            AreaCache.AreaCacheInstance.DeleteItem(AreaCacheRequest.AreaInfo);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse BatchDeleteAreaCache(Sys.Safety.Request.PersonCache.AreaCacheBatchDeleteRequest AreaCacheRequest)
        {
            AreaCache.AreaCacheInstance.DeleteItems(AreaCacheRequest.AreaInfos);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.AreaInfo>> GetAllAreaCache(Sys.Safety.Request.PersonCache.AreaCacheGetAllRequest AreaCacheRequest)
        {
            var _AreaCache = AreaCache.AreaCacheInstance.Query();
            var AreaCacheResponse = new BasicResponse<List<AreaInfo>>();
            AreaCacheResponse.Data = _AreaCache;
            return AreaCacheResponse;
        }

        public Basic.Framework.Web.BasicResponse<DataContract.AreaInfo> GetByKeyAreaCache(Sys.Safety.Request.PersonCache.AreaCacheGetByKeyRequest AreaCacheRequest)
        {
            var _AreaCache = AreaCache.AreaCacheInstance.Query(o => o.Areaid == AreaCacheRequest.Areaid).FirstOrDefault();
            var AreaCacheResponse = new BasicResponse<AreaInfo>();
            AreaCacheResponse.Data = _AreaCache;
            return AreaCacheResponse;
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.AreaInfo>> GetAreaCache(Sys.Safety.Request.PersonCache.AreaCacheGetByConditionRequest AreaCacheRequest)
        {
            var _AreaCache = AreaCache.AreaCacheInstance.Query(AreaCacheRequest.Predicate);
            var AreaCacheResponse = new BasicResponse<List<AreaInfo>>();
            AreaCacheResponse.Data = _AreaCache;
            return AreaCacheResponse;
        }

        public Basic.Framework.Web.BasicResponse<bool> IsExistsAreaCache(Sys.Safety.Request.PersonCache.AreaCacheIsExistsRequest AreaCacheRequest)
        {
            var _AreaCache = AreaCache.AreaCacheInstance.Query(call => call.Areaid == AreaCacheRequest.Areaid).FirstOrDefault();
            var AreaCacheResponse = new BasicResponse<bool>();
            AreaCacheResponse.Data = _AreaCache != null;
            return AreaCacheResponse;
        }
    }
}
