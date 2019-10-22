using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Kqbc;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.ServiceContract.KJ237Cache;
using Sys.Safety.Request.PersonCache;

namespace Sys.Safety.Services
{
    public partial class R_KqbcService : IR_KqbcService
    {
        private IR_KqbcRepository _Repository;
        private IRKqbcCacheService _RKqbcCacheService;

        public R_KqbcService(IR_KqbcRepository _Repository, IRKqbcCacheService _RKqbcCacheService)
        {
            this._Repository = _Repository;
            this._RKqbcCacheService = _RKqbcCacheService;
        }
        public BasicResponse<R_KqbcInfo> AddKqbc(R_KqbcAddRequest kqbcRequest)
        {
            var _kqbc = ObjectConverter.Copy<R_KqbcInfo, R_KqbcModel>(kqbcRequest.KqbcInfo);
            var resultkqbc = _Repository.AddKqbc(_kqbc);
            var kqbcresponse = new BasicResponse<R_KqbcInfo>();
            kqbcresponse.Data = ObjectConverter.Copy<R_KqbcModel, R_KqbcInfo>(resultkqbc);
            return kqbcresponse;
        }
        public BasicResponse<R_KqbcInfo> UpdateKqbc(R_KqbcUpdateRequest kqbcRequest)
        {
            var _kqbc = ObjectConverter.Copy<R_KqbcInfo, R_KqbcModel>(kqbcRequest.KqbcInfo);
            _Repository.UpdateKqbc(_kqbc);
            var kqbcresponse = new BasicResponse<R_KqbcInfo>();
            kqbcresponse.Data = ObjectConverter.Copy<R_KqbcModel, R_KqbcInfo>(_kqbc);
            return kqbcresponse;
        }
        public BasicResponse DeleteKqbc(R_KqbcDeleteRequest kqbcRequest)
        {
            _Repository.DeleteKqbc(kqbcRequest.Id);
            var kqbcresponse = new BasicResponse();
            return kqbcresponse;
        }
        public BasicResponse<List<R_KqbcInfo>> GetKqbcList(R_KqbcGetListRequest kqbcRequest)
        {
            var kqbcresponse = new BasicResponse<List<R_KqbcInfo>>();
            kqbcRequest.PagerInfo.PageIndex = kqbcRequest.PagerInfo.PageIndex - 1;
            if (kqbcRequest.PagerInfo.PageIndex < 0)
            {
                kqbcRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var kqbcModelLists = _Repository.GetKqbcList(kqbcRequest.PagerInfo.PageIndex, kqbcRequest.PagerInfo.PageSize, out rowcount);
            var kqbcInfoLists = new List<R_KqbcInfo>();
            foreach (var item in kqbcModelLists)
            {
                var KqbcInfo = ObjectConverter.Copy<R_KqbcModel, R_KqbcInfo>(item);
                kqbcInfoLists.Add(KqbcInfo);
            }
            kqbcresponse.Data = kqbcInfoLists;
            return kqbcresponse;
        }
        public BasicResponse<List<R_KqbcInfo>> GetAllKqbcList()
        {
            var kqbcresponse = new BasicResponse<List<R_KqbcInfo>>();
            
            var kqbcModelLists = _Repository.GetAllKqbcList();
            var kqbcInfoLists = new List<R_KqbcInfo>();
            foreach (var item in kqbcModelLists)
            {
                var KqbcInfo = ObjectConverter.Copy<R_KqbcModel, R_KqbcInfo>(item);
                kqbcInfoLists.Add(KqbcInfo);
            }
            kqbcresponse.Data = kqbcInfoLists;
            return kqbcresponse;
        }
        public BasicResponse<R_KqbcInfo> GetKqbcById(R_KqbcGetRequest kqbcRequest)
        {
            var result = _Repository.GetKqbcById(kqbcRequest.Id);
            var kqbcInfo = ObjectConverter.Copy<R_KqbcModel, R_KqbcInfo>(result);
            var kqbcresponse = new BasicResponse<R_KqbcInfo>();
            kqbcresponse.Data = kqbcInfo;
            return kqbcresponse;
        }
        /// <summary>
        /// 获取所有考勤班次缓存列表
        /// </summary>
        /// <param name="RKqbcCacheRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<R_KqbcInfo>> GetAllKqbcCacheList(RKqbcCacheGetAllRequest RKqbcCacheRequest)
        {
          return  _RKqbcCacheService.GetAllRKqbcCache(RKqbcCacheRequest);
        }
        /// <summary>
        /// 获取默认班次
        /// </summary>
        /// <param name="RKqbcCacheRequest"></param>
        /// <returns></returns>
        public BasicResponse<R_KqbcInfo> GetDefaultKqbcCache(RKqbcCacheGetByConditionRequest RKqbcCacheRequest)
        {
            BasicResponse<R_KqbcInfo> result = new BasicResponse<R_KqbcInfo>();
            result.Data = null;
            RKqbcCacheGetByConditionRequest KqbcCacheRequest = new RKqbcCacheGetByConditionRequest();
            KqbcCacheRequest.Predicate = a => a.Mrbc == "1";
            var resultCache = _RKqbcCacheService.GetRKqbcCache(KqbcCacheRequest);
            if (resultCache.Data.Count > 0) {
                result.Data = resultCache.Data[0];
            }
            return result;
        }
    }
}


