using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Basic.Framework.Common;
using Basic.Framework.Data;
using Basic.Framework.Web;
using Sys.Safety.Request.R_Call;
using Sys.Safety.Request.Cache;
using Sys.Safety.ServiceContract.Cache;

namespace Sys.Safety.Services
{
    /// <summary>
    /// 实时通话/广播管理
    /// </summary>
    public partial class B_CallService : IB_CallService
    {
        private IB_CallRepository _Repository;
        private IB_CallCacheService _BCallCacheService;
        private IB_CallpointlistRepository _bcallPointListRepository;
        private IB_DefCacheService _bDefCacheService;

        public B_CallService(IB_CallRepository _Repository, IB_CallCacheService _BCallCacheService, IB_CallpointlistRepository _bcallPointListRepository, IB_DefCacheService bDefCacheService)
        {
            this._Repository = _Repository;
            this._BCallCacheService = _BCallCacheService;
            this._bcallPointListRepository = _bcallPointListRepository;
            _bDefCacheService = bDefCacheService;
        }
        public BasicResponse<B_CallInfo> AddCall(B_CallAddRequest callRequest)
        {
            //var _call = ObjectConverter.Copy<B_CallInfo, B_CallModel>(callRequest.CallInfo);
            //var resultcall = _Repository.AddCall(_call);
            //var callresponse = new BasicResponse<B_CallInfo>();
            //callresponse.Data = ObjectConverter.Copy<B_CallModel, B_CallInfo>(resultcall);
            //return callresponse;

            var callresponse = new BasicResponse<B_CallInfo>();
            //判断缓存中是否存在
            B_CallInfo oldB_CallInfo = null;
            BCallCacheGetByKeyRequest RCallCacheRequest = new BCallCacheGetByKeyRequest();
            RCallCacheRequest.Id = callRequest.CallInfo.Id;
            oldB_CallInfo = _BCallCacheService.GetByKeyBCallCache(RCallCacheRequest).Data;
            if (oldB_CallInfo != null)
            {
                //缓存中存在此测点
                callresponse.Code = 1;
                callresponse.Message = "当前添加的呼叫控制已存在！";
                return callresponse;
            }

            var _call = ObjectConverter.Copy<B_CallInfo, B_CallModel>(callRequest.CallInfo);
            var resultcall = _Repository.AddCall(_call);

            //
            if (callRequest.CallInfo.CallPointList != null && callRequest.CallInfo.CallPointList.Count > 0)
            {
                var callpointlist = ObjectConverter.CopyList<B_CallpointlistInfo, B_CallpointlistModel>(callRequest.CallInfo.CallPointList);
                _bcallPointListRepository.Insert(callpointlist);
            }

            //更新缓存
            BCallCacheAddRequest BCallCacheAddRequest = new BCallCacheAddRequest();
            BCallCacheAddRequest.BCallInfo = callRequest.CallInfo;
            _BCallCacheService.AddBCallCache(BCallCacheAddRequest);

            callresponse.Data = ObjectConverter.Copy<B_CallModel, B_CallInfo>(resultcall);
            return callresponse;
        }
        public BasicResponse<B_CallInfo> UpdateCall(B_CallUpdateRequest callRequest)
        {
            //var _call = ObjectConverter.Copy<B_CallInfo, B_CallModel>(callRequest.CallInfo);
            //_Repository.UpdateCall(_call);
            //var callresponse = new BasicResponse<B_CallInfo>();
            //callresponse.Data = ObjectConverter.Copy<B_CallModel, B_CallInfo>(_call);
            //return callresponse;

            var callresponse = new BasicResponse<B_CallInfo>();

            //判断缓存中是否存在
            B_CallInfo oldB_CallInfo = null;
            BCallCacheGetByKeyRequest BCallCacheRequest = new BCallCacheGetByKeyRequest();
            BCallCacheRequest.Id = callRequest.CallInfo.Id;
            oldB_CallInfo = _BCallCacheService.GetByKeyBCallCache(BCallCacheRequest).Data;
            if (oldB_CallInfo == null)
            {
                //缓存中存在此测点
                callresponse.Code = 1;
                callresponse.Message = "当前更新的呼叫控制记录不存在！";
                return callresponse;
            }

            var _call = ObjectConverter.Copy<B_CallInfo, B_CallModel>(callRequest.CallInfo);
            _Repository.UpdateCall(_call);

            //更新缓存
            BCallCacheUpdateRequest BCallCacheUpdateRequest = new BCallCacheUpdateRequest();
            BCallCacheUpdateRequest.BCallInfo = callRequest.CallInfo;
            _BCallCacheService.UpdateBCallCache(BCallCacheUpdateRequest);

            callresponse.Data = ObjectConverter.Copy<B_CallModel, B_CallInfo>(_call);
            return callresponse;

        }
        public BasicResponse DeleteCall(B_CallDeleteRequest callRequest)
        {
            //_Repository.DeleteCall(callRequest.Id);
            //var callresponse = new BasicResponse();
            //return callresponse;

            var callresponse = new BasicResponse();
            //判断缓存中是否存在
            B_CallInfo oldB_CallInfo = null;
            BCallCacheGetByKeyRequest BCallCacheRequest = new BCallCacheGetByKeyRequest();
            BCallCacheRequest.Id = callRequest.Id;
            oldB_CallInfo = _BCallCacheService.GetByKeyBCallCache(BCallCacheRequest).Data;
            if (oldB_CallInfo == null)
            {
                //缓存中存在此测点
                callresponse.Code = 1;
                callresponse.Message = "当前删除的呼叫控制记录不存在！";
                return callresponse;
            }

            _Repository.DeleteCall(callRequest.Id);

            //更新缓存
            BCallCacheDeleteRequest BCallCacheDeleteRequest = new BCallCacheDeleteRequest();
            BCallCacheDeleteRequest.BCallInfo = oldB_CallInfo;
            _BCallCacheService.DeleteBCallCache(BCallCacheDeleteRequest);

            return callresponse;

        }
        public BasicResponse<List<B_CallInfo>> GetCallList(B_CallGetListRequest callRequest)
        {
            var callresponse = new BasicResponse<List<B_CallInfo>>();
            callRequest.PagerInfo.PageIndex = callRequest.PagerInfo.PageIndex - 1;
            if (callRequest.PagerInfo.PageIndex < 0)
            {
                callRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var callModelLists = _Repository.GetCallList(callRequest.PagerInfo.PageIndex, callRequest.PagerInfo.PageSize, out rowcount);
            var callInfoLists = new List<B_CallInfo>();
            foreach (var item in callModelLists)
            {
                var CallInfo = ObjectConverter.Copy<B_CallModel, B_CallInfo>(item);
                callInfoLists.Add(CallInfo);
            }
            callresponse.Data = callInfoLists;
            return callresponse;
        }
        public BasicResponse<B_CallInfo> GetCallById(B_CallGetRequest callRequest)
        {
            var result = _Repository.GetCallById(callRequest.Id);
            var callInfo = ObjectConverter.Copy<B_CallModel, B_CallInfo>(result);
            var callresponse = new BasicResponse<B_CallInfo>();
            callresponse.Data = callInfo;
            return callresponse;
        }

        public BasicResponse<List<B_CallInfo>> GetAll(BasicRequest callRequest)
        {
            var callresponse = new BasicResponse<List<B_CallInfo>>();
            var callModelLists = _Repository.GetAllCall();
            var callinfolists = ObjectConverter.CopyList<B_CallModel, B_CallInfo>(callModelLists).ToList();
            callresponse.Data = callinfolists;
            return callresponse;
        }

        public BasicResponse<List<B_CallInfo>> GetAllCache()
        {
            var req = new BCallCacheGetAllRequest();
            var res = _BCallCacheService.GetAllBCallCache(req);
            var ret = new BasicResponse<List<B_CallInfo>>
            {
                Data = res.Data
            };
            return ret;
        }

        public BasicResponse<List<B_CallInfo>> GetFusionCache()
        {
            var req = new BCallCacheGetAllRequest();
            var listBcall = _BCallCacheService.GetAllBCallCache(req).Data;

            var req2 = new B_DefCacheGetAllRequest();
            var listBDef = _bDefCacheService.GetAll(req2).Data;

            foreach (var item in listBcall)
            {
                foreach (var item2 in item.CallPointList)
                {
                    var bDef = listBDef.FirstOrDefault(a => a.PointID == item2.CalledPointId);
                    if (bDef != null)
                    {
                        item2.CalledPointId = bDef.Point;
                    }
                    else
                    {
                        item2.CalledPointId = "";
                    }
                }
            }

            var ret = new BasicResponse<List<B_CallInfo>>
            {
                Data = listBcall
            };
            return ret;
        }

        public BasicResponse<List<B_CallInfo>> GetBCallInfoByMasterID(BCallInfoGetByMasterIDRequest callRequest)
        {
            var callresponse = new BasicResponse<List<B_CallInfo>>();

            BCallCacheGetByConditionRequest r_defExistsRequest = new BCallCacheGetByConditionRequest();
            r_defExistsRequest.Predicate = o => o.MasterId == callRequest.MasterId && o.CallType == callRequest.CallType;
            var rcallinfo = _BCallCacheService.GetBCallCache(r_defExistsRequest).Data;
            callresponse.Data = rcallinfo;
            return callresponse;
        }

        public BasicResponse EndBcallByBcallInfoList(EndBcallByBcallInfoListRequest request)
        {
            //var models = ObjectConverter.CopyList<B_CallInfo, B_CallModel>(request.Info);

            TransactionsManager.BeginTransaction(() =>
            {
                foreach (var item in request.Info)
                {
                    if (item.CallType == 1)
                    {
                        item.CallType = 2;
                        //_Repository.UpdateCall(item);
                        var req = new B_CallUpdateRequest()
                        {
                            CallInfo = item
                        };
                        UpdateCall(req);
                    }
                }
            });
            return new BasicResponse();
        }

        public BasicResponse EndBcallDbByBcallInfoList(EndBcallDbByBcallInfoListRequest request)
        {
            TransactionsManager.BeginTransaction(() =>
            {
                foreach (var item in request.Info)
                {
                    if (item.CallType == 1)
                    {
                        item.CallType = 2;
                        var req = new B_CallUpdateRequest()
                        {
                            CallInfo = item
                        };
                        var _call = ObjectConverter.Copy<B_CallInfo, B_CallModel>(item);
                        _Repository.UpdateCall(_call);
                    }
                }
            });
            return new BasicResponse();

        }

        public BasicResponse DeleteFinishedBcall()
        {
            var time = DateTime.Now.AddDays(-2);
            _Repository.Delete(a => a.CallType == 2 && a.CallTime < time);
            return new BasicResponse();
        }
    }
}


