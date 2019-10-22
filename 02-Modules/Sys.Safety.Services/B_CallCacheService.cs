using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.Cache.Audio;
using Sys.Safety.Request.B_Callpointlist;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Services
{
    public class B_CallCacheService : IB_CallCacheService
    {
        IB_CallpointlistService callpoinservice = ServiceFactory.Create<IB_CallpointlistService>();

        public Basic.Framework.Web.BasicResponse LoadBCallCache(Sys.Safety.Request.Cache.BCallCacheLoadRequest BCallCacheRequest)
        {
            B_CallCache.Instance.Load();

            //加载缓存之后，加载CallPointList
            var bcallinfos = B_CallCache.Instance.Query();
            if (bcallinfos.Any())
            {
                bcallinfos.ForEach(call =>
                {
                    var callpointlist = callpoinservice.GetB_CallByBCallId(new B_CallpointlistGetRequest { Id = call.Id });
                    if (callpointlist.IsSuccess && callpointlist.Data != null)
                        call.CallPointList = callpointlist.Data;
                });

                B_CallCache.Instance.UpdateItems(bcallinfos);
            }
            return new BasicResponse();

        }

        public Basic.Framework.Web.BasicResponse AddBCallCache(Sys.Safety.Request.Cache.BCallCacheAddRequest BCallCacheRequest)
        {
            B_CallCache.Instance.AddItem(BCallCacheRequest.BCallInfo);
            return new Basic.Framework.Web.BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse BatchAddBCallCache(Sys.Safety.Request.Cache.BCallCacheBatchAddRequest BCallCacheRequest)
        {
            B_CallCache.Instance.AddItems(BCallCacheRequest.BCallInfos);
            return new Basic.Framework.Web.BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse UpdateBCallCache(Sys.Safety.Request.Cache.BCallCacheUpdateRequest BCallCacheRequest)
        {
            B_CallCache.Instance.UpdateItem(BCallCacheRequest.BCallInfo);
            return new Basic.Framework.Web.BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse BatchUpdateBCallCache(Sys.Safety.Request.Cache.BCallCacheBatchUpdateRequest BCallCacheRequest)
        {
            B_CallCache.Instance.UpdateItems(BCallCacheRequest.BCallInfos);
            return new Basic.Framework.Web.BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse DeleteBCallCache(Sys.Safety.Request.Cache.BCallCacheDeleteRequest BCallCacheRequest)
        {
            B_CallCache.Instance.DeleteItem(BCallCacheRequest.BCallInfo);
            return new Basic.Framework.Web.BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse BatchDeleteBCallCache(Sys.Safety.Request.Cache.BCallCacheBatchDeleteRequest BCallCacheRequest)
        {
            B_CallCache.Instance.DeleteItems(BCallCacheRequest.BCallInfos);
            return new Basic.Framework.Web.BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.B_CallInfo>> GetAllBCallCache(Sys.Safety.Request.Cache.BCallCacheGetAllRequest BCallCacheRequest)
        {
            var BasicResponse = new BasicResponse<List<DataContract.B_CallInfo>>();
            BasicResponse.Data = B_CallCache.Instance.Query();
            return BasicResponse;
        }

        public Basic.Framework.Web.BasicResponse<DataContract.B_CallInfo> GetByKeyBCallCache(Sys.Safety.Request.Cache.BCallCacheGetByKeyRequest BCallCacheRequest)
        {
            var BasicResponse = new BasicResponse<DataContract.B_CallInfo>();
            BasicResponse.Data = B_CallCache.Instance.Query(o => o.Id == BCallCacheRequest.Id).FirstOrDefault();
            return BasicResponse;
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.B_CallInfo>> GetBCallCache(Sys.Safety.Request.Cache.BCallCacheGetByConditionRequest BCallCacheRequest)
        {
            var BasicResponse = new BasicResponse<List<DataContract.B_CallInfo>>();
            BasicResponse.Data = B_CallCache.Instance.Query(BCallCacheRequest.Predicate);
            return BasicResponse;
        }

        public Basic.Framework.Web.BasicResponse<bool> IsExistsBCallCache(Sys.Safety.Request.Cache.BCallCacheIsExistsRequest BCallCacheRequest)
        {
            var BasicResponse = new BasicResponse<bool>();
            var bcall = B_CallCache.Instance.Query(o => o.Id == BCallCacheRequest.Id).FirstOrDefault();
            BasicResponse.Data = bcall != null;
            return BasicResponse;
        }


        public BasicResponse BatchUpdatePointInfo(Sys.Safety.Request.Cache.BatchUpdatePointInfoRequest Request)
        {
            B_CallCache.Instance.BatchUpdatePointInfo(Request.updateItems);
            return new Basic.Framework.Web.BasicResponse();
        }
    }
}
