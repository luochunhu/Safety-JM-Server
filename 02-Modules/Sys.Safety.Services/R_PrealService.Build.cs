using System.Collections.Generic;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.Request.R_Preal;
using Sys.Safety.ServiceContract.KJ237Cache;
using Sys.Safety.Request.PersonCache;
using Basic.Framework.Logging;
using System;
using System.Linq;

namespace Sys.Safety.Services
{
    public partial class R_PrealService : IR_PrealService
    {
        private IR_PrealRepository _Repository;
        private IRPRealCacheService _RPRealCacheService;

        public R_PrealService(IR_PrealRepository _Repository, IRPRealCacheService _RPRealCacheService)
        {
            this._Repository = _Repository;
            this._RPRealCacheService = _RPRealCacheService;
        }
        public BasicResponse<R_PrealInfo> AddPreal(R_PrealAddRequest prealRequest)
        {
            var _preal = ObjectConverter.Copy<R_PrealInfo, R_PrealModel>(prealRequest.PrealInfo);
            var resultpreal = _Repository.AddPreal(_preal);
            var prealresponse = new BasicResponse<R_PrealInfo>();
            prealresponse.Data = ObjectConverter.Copy<R_PrealModel, R_PrealInfo>(resultpreal);
            return prealresponse;
        }
        public BasicResponse<R_PrealInfo> UpdatePreal(R_PrealUpdateRequest prealRequest)
        {
            var _preal = ObjectConverter.Copy<R_PrealInfo, R_PrealModel>(prealRequest.PrealInfo);
            _Repository.UpdatePreal(_preal);
            var prealresponse = new BasicResponse<R_PrealInfo>();
            prealresponse.Data = ObjectConverter.Copy<R_PrealModel, R_PrealInfo>(_preal);
            return prealresponse;
        }
        public BasicResponse DeletePreal(R_PrealDeleteRequest prealRequest)
        {
            _Repository.DeletePreal(prealRequest.Id);
            var prealresponse = new BasicResponse();
            return prealresponse;
        }
        public BasicResponse<List<R_PrealInfo>> GetPrealList(R_PrealGetListRequest prealRequest)
        {
            var prealresponse = new BasicResponse<List<R_PrealInfo>>();
            prealRequest.PagerInfo.PageIndex = prealRequest.PagerInfo.PageIndex - 1;
            if (prealRequest.PagerInfo.PageIndex < 0)
            {
                prealRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var prealModelLists = _Repository.GetPrealList(prealRequest.PagerInfo.PageIndex, prealRequest.PagerInfo.PageSize, out rowcount);
            var prealInfoLists = new List<R_PrealInfo>();
            foreach (var item in prealModelLists)
            {
                var PrealInfo = ObjectConverter.Copy<R_PrealModel, R_PrealInfo>(item);
                prealInfoLists.Add(PrealInfo);
            }
            prealresponse.Data = prealInfoLists;
            return prealresponse;
        }
        public BasicResponse<R_PrealInfo> GetPrealById(R_PrealGetRequest prealRequest)
        {
            var result = _Repository.GetPrealById(prealRequest.Id);
            var prealInfo = ObjectConverter.Copy<R_PrealModel, R_PrealInfo>(result);
            var prealresponse = new BasicResponse<R_PrealInfo>();
            prealresponse.Data = prealInfo;
            return prealresponse;
        }
        /// <summary>
        /// 获取所有人员实时缓存
        /// </summary>
        /// <param name="prealRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<R_PrealInfo>> GetAllPrealCacheList(RPrealCacheGetAllRequest RealCacheRequest)
        {
            return _RPRealCacheService.GetAllRRealCache(RealCacheRequest);
        }
        /// <summary>
        /// 获取所有报警人员实时缓存
        /// </summary>
        /// <param name="prealRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<R_PrealInfo>> GetAllAlarmPrealCacheList()
        {
            RPrealCacheGetByConditonRequest RealCacheRequest = new RPrealCacheGetByConditonRequest();
            RealCacheRequest.Predicate = a => a.Bjtype > 0;
            return _RPRealCacheService.GetRealCache(RealCacheRequest);
        }

        public BasicResponse OldPlsPersonRealSync(OldPlsPersonRealSyncRequest request)
        {
            try
            {
                var req = new RPrealCacheGetAllRequest();
                var res = _RPRealCacheService.GetAllRRealCache(req);
                var allPersonRealCache = res.Data;      //所有人员实时缓存
                var info = request.Info;

                foreach (var item in info)
                {
                    R_PrealInfo personRealCache = new R_PrealInfo();
                    var ifExist = false;
                    if (allPersonRealCache != null)
                    {
                        personRealCache = allPersonRealCache.FirstOrDefault(a => a.Yid == item.Yid);
                        if (personRealCache != null)
                        {
                            ifExist = true;
                        }
                    }

                    if (!ifExist)
                    {
                        var req2 = new RPrealCacheAddRequest
                        {
                            PrealInfo = item
                        };
                        _RPRealCacheService.AddRRealCache(req2);
                    }
                    else
                    {
                        if (item.Bh != personRealCache.Bh || item.JobNumber != personRealCache.JobNumber ||
                            item.PersonName != personRealCache.PersonName ||
                            item.Department != personRealCache.Department ||
                            item.Duty != personRealCache.Duty ||
                            item.TypeOfWork != personRealCache.TypeOfWork ||
                            item.CurrentPosition != personRealCache.CurrentPosition ||
                            item.Rtime != personRealCache.Rtime ||
                            item.UpPosition != personRealCache.UpPosition ||
                            item.OnPosition != personRealCache.OnPosition ||
                            item.Ontime != personRealCache.Ontime ||
                            item.Rjsc != personRealCache.Rjsc || item.Bjtype != personRealCache.Bjtype || item.Flag != personRealCache.Flag)
                        {
                            var req2 = new RPrealCacheUpdateRequest
                            {
                                PrealInfo = item
                            };
                            _RPRealCacheService.UpdateRRealCahce(req2);
                        }
                    }
                }

                if (allPersonRealCache != null)
                {
                    for (var i = allPersonRealCache.Count - 1; i >= 0; i--)
                    {
                        var ifExist = info.Any(a => a.Yid == allPersonRealCache[i].Yid);
                        if (!ifExist)
                        {
                            var req3 = new RPrealCacheDeleteRequest
                            {
                                PrealInfo = allPersonRealCache[i]
                            };
                            _RPRealCacheService.DeleteRRealCache(req3);
                        }
                    }
                }

                return new BasicResponse();
            }
            catch (Exception exc)
            {
                LogHelper.Error(exc.ToString());
                return new BasicResponse()
                {
                    Code = 101,
                    Message = "同步失败"
                };
            }
        }
    }
}


