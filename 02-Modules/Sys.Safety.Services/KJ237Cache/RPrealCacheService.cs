using Basic.Framework.Web;
using Sys.Safety.Cache;
using Sys.Safety.Cache.Person;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract.KJ237Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Services.KJ237Cache
{
    public class RPrealCacheService : IRPRealCacheService
    {
        public Basic.Framework.Web.BasicResponse LoadRRealCache(Sys.Safety.Request.PersonCache.RPralCacheLoadRequest RealCacheRequest)
        {
            
            //RPRealCache.RPrealCahceInstance.Load();
            //如果采用本地缓存，此处待优化

            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse AddRRealCache(Sys.Safety.Request.PersonCache.RPrealCacheAddRequest RealCacheRequest)
        {
           // RPRealCache.RPrealCahceInstance.AddItem(RealCacheRequest.PrealInfo);
            KJ237CacheHelper.Cache.Insert<R_PrealInfo>(RealCacheRequest.PrealInfo);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse BacthAddRRealCache(Sys.Safety.Request.PersonCache.RPrealCacheBatchAddRequest RealCacheRequest)
        {
            //RPRealCache.RPrealCahceInstance.AddItems(RealCacheRequest.PrealInfos);
            KJ237CacheHelper.Cache.BatchInsert<R_PrealInfo>(RealCacheRequest.PrealInfos);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse UpdateRRealCahce(Sys.Safety.Request.PersonCache.RPrealCacheUpdateRequest RealCacheRequest)
        {
            //RPRealCache.RPrealCahceInstance.UpdateItem(RealCacheRequest.PrealInfo);
            KJ237CacheHelper.Cache.Update<R_PrealInfo>(RealCacheRequest.PrealInfo);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse BatchRUpdateRealCache(Sys.Safety.Request.PersonCache.RPrealCacheBatchUpdateRequest RealCacheRequest)
        {
            //RPRealCache.RPrealCahceInstance.UpdateItems(RealCacheRequest.PrealInfos);
            KJ237CacheHelper.Cache.BatchUpdate<R_PrealInfo>(RealCacheRequest.PrealInfos);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse DeleteRRealCache(Sys.Safety.Request.PersonCache.RPrealCacheDeleteRequest RealCacheRequest)
        {
            //RPRealCache.RPrealCahceInstance.DeleteItem(RealCacheRequest.PrealInfo);
            KJ237CacheHelper.Cache.Delete<R_PrealInfo>(RealCacheRequest.PrealInfo);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse BatchDeleteRRealCache(Sys.Safety.Request.PersonCache.RPrealCacheBatchDeleteRequest RealCacheRequest)
        {
            //RPRealCache.RPrealCahceInstance.DeleteItems(RealCacheRequest.PrealInfos);
            KJ237CacheHelper.Cache.BatchDelete<R_PrealInfo>(RealCacheRequest.PrealInfos);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.R_PrealInfo>> GetAllRRealCache(Sys.Safety.Request.PersonCache.RPrealCacheGetAllRequest RealCacheRequest)
        {
            var rPersoninfResponse = new BasicResponse<List<R_PrealInfo>>();
            try
            {
                //var rPersoninfCache = RPRealCache.RPrealCahceInstance.Query();
                var rPersoninfCache = KJ237CacheHelper.Cache.FindAll<R_PrealInfo>(true);
                rPersoninfResponse.Data = rPersoninfCache;
            }
            catch {
                rPersoninfResponse = new BasicResponse<List<R_PrealInfo>>();
            }
            return rPersoninfResponse;
        }

        public Basic.Framework.Web.BasicResponse<DataContract.R_PrealInfo> RealCacheByPointIdRequeest(Sys.Safety.Request.PersonCache.RPrealCacheByPointIdRequeest RealCacheRequest)
        {
            var rPersoninfResponse = new BasicResponse<R_PrealInfo>();
            //var rPersoninfCache = RPRealCache.RPrealCahceInstance.Query(o => o.Id == RealCacheRequest.Id).FirstOrDefault();
            var rPersoninfCache = KJ237CacheHelper.Cache.FindById<R_PrealInfo>(RealCacheRequest.Id, true);
            rPersoninfResponse.Data = rPersoninfCache;
            return rPersoninfResponse;
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.R_PrealInfo>> GetRealCache(Sys.Safety.Request.PersonCache.RPrealCacheGetByConditonRequest RealCacheRequest)
        {
            var rPersoninfResponse = new BasicResponse<List<R_PrealInfo>>();
            //var rPersoninfCache = RPRealCache.RPrealCahceInstance.Query(RealCacheRequest.Predicate);
            var rPersoninfCache = KJ237CacheHelper.Cache.Find<R_PrealInfo>(RealCacheRequest.Predicate, true);
            rPersoninfResponse.Data = rPersoninfCache;
            return rPersoninfResponse;
        }

        public Basic.Framework.Web.BasicResponse UpdateRRealInfo(Sys.Safety.Request.PersonCache.RPrealCacheUpdatePropertiesRequest RealCacheRequest)
        {
            R_PrealInfo updateitem = KJ237CacheHelper.Cache.FindFirst<R_PrealInfo>(o => o.Id == RealCacheRequest.Id);
            updateitem.CopyProperties(RealCacheRequest.UpdateItems);
            KJ237CacheHelper.Cache.Update<R_PrealInfo>(updateitem);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse BatchUpdateRealInfo(Sys.Safety.Request.PersonCache.RPrealCacheBatchUpdatePropertiesRequest request)
        {
            List<R_PrealInfo> updateitems = new List<R_PrealInfo>();
            foreach (string tempId in request.UpdateItems.Keys)
            {
                R_PrealInfo updateitem = KJ237CacheHelper.Cache.FindFirst<R_PrealInfo>(o => o.Id == tempId);
                if (updateitem != null)
                {
                    updateitem.CopyProperties(request.UpdateItems[tempId]);
                    updateitems.Add(updateitem);
                }
            }

            KJ237CacheHelper.Cache.BatchUpdate<R_PrealInfo>(updateitems);
            return new BasicResponse();
        }
    }
}
