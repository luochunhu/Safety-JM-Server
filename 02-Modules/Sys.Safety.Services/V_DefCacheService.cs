using Basic.Framework.Web;
using Sys.Safety.Cache.Video;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Services
{
    public class V_DefCacheService : IV_DefCacheService
    {
        public Basic.Framework.Web.BasicResponse Insert(Sys.Safety.Request.Cache.V_DefCacheInsertRequest vDefCacheRequest)
        {
            V_DefCache.Instance.AddItem(vDefCacheRequest.V_DefInfo);
            return new Basic.Framework.Web.BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse BatchInsert(Sys.Safety.Request.Cache.V_DefCacheBatchInsertRequest vDefCacheRequest)
        {
            V_DefCache.Instance.AddItems(vDefCacheRequest.V_DefInfos);
            return new Basic.Framework.Web.BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse Delete(Sys.Safety.Request.Cache.V_DefCacheDeleteRequest vDefCacheRequest)
        {
            V_DefCache.Instance.DeleteItem(vDefCacheRequest.V_DefInfo);
            return new Basic.Framework.Web.BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse BatchDelete(Sys.Safety.Request.Cache.V_DefCacheBatchDeleteRequest vDefCacheRequest)
        {
            V_DefCache.Instance.DeleteItems(vDefCacheRequest.V_DefInfos);
            return new Basic.Framework.Web.BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse<DataContract.V_DefInfo> GetById(Sys.Safety.Request.Cache.V_DefCacheGetByIdRequest vDefCacheRequest)
        {
            var response = new BasicResponse<DataContract.V_DefInfo>();
            response.Data = V_DefCache.Instance.Query(o => o.Id == vDefCacheRequest.Id).FirstOrDefault();
            return response;
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.V_DefInfo>> GetAll(Sys.Safety.Request.Cache.V_DefCacheGetAllRequest vDefCacheRequest)
        {
            var response = new BasicResponse<List<DataContract.V_DefInfo>>();
            response.Data = V_DefCache.Instance.Query();
            return response;
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.V_DefInfo>> Get(Sys.Safety.Request.Cache.V_DefCacheGetByConditionRequest vDefCacheRequest)
        {
            var response = new BasicResponse<List<DataContract.V_DefInfo>>();
            response.Data = V_DefCache.Instance.Query(vDefCacheRequest.predicate);
            return response;
        }

        public BasicResponse Load()
        {
            V_DefCache.Instance.Load();
            return new BasicResponse();
        }

        public BasicResponse Update(Sys.Safety.Request.Cache.V_DefCacheInsertRequest vDefCacheRequest)
        {
            V_DefCache.Instance.UpdateItem(vDefCacheRequest.V_DefInfo);
            return new BasicResponse();
        }

        public BasicResponse BatchUpdate(Sys.Safety.Request.Cache.V_DefCacheBatchInsertRequest vDefCacheRequest)
        {
            V_DefCache.Instance.UpdateItems(vDefCacheRequest.V_DefInfos);
            return new BasicResponse();
        }
    }
}
