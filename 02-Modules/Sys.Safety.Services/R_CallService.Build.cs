using System.Collections.Generic;
using System.Linq;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.Request.R_Call;
using Sys.Safety.Cache.Person;
using Sys.Safety.ServiceContract.KJ237Cache;
using Sys.Safety.Request.PersonCache;
using System;
using Basic.Framework.Data;
using Sys.Safety.Request.Phj;
using Sys.Safety.Request.PointDefine;

namespace Sys.Safety.Services
{
    public partial class R_CallService : IR_CallService
    {
        private IR_CallRepository _Repository;
        private IRCallCacheService _RCallCacheService;
        private IR_PhjService _R_PhjService;
        private IR_PersoninfService _R_PersoninfService;
        private IPersonPointDefineService _PersonPointDefineService;

        public R_CallService(IR_CallRepository _Repository, IRCallCacheService _RCallCacheService, IR_PhjService _R_PhjService,
            IR_PersoninfService _R_PersoninfService, IPersonPointDefineService _PersonPointDefineService)
        {
            this._Repository = _Repository;
            this._RCallCacheService = _RCallCacheService;
            this._R_PhjService = _R_PhjService;
            this._R_PersoninfService = _R_PersoninfService;
            this._PersonPointDefineService = _PersonPointDefineService;
        }
        public BasicResponse<R_CallInfo> AddCall(R_CallAddRequest callRequest)
        {
            var callresponse = new BasicResponse<R_CallInfo>();
            //判断缓存中是否存在
            R_CallInfo oldR_CallInfo = null;
            RCallCacheGetByKeyRequest RCallCacheRequest = new RCallCacheGetByKeyRequest();
            RCallCacheRequest.Id = callRequest.CallInfo.Id;
            oldR_CallInfo = _RCallCacheService.GetByKeyRCallCache(RCallCacheRequest).Data;
            if (oldR_CallInfo != null)
            {
                //缓存中存在此测点
                callresponse.Code = 1;
                callresponse.Message = "当前添加的呼叫控制已存在！";
                return callresponse;
            }
            var _call = ObjectConverter.Copy<R_CallInfo, R_CallModel>(callRequest.CallInfo);
            var resultcall = _Repository.AddCall(_call);
            //写呼叫历史记录
            AddR_PHJInfo(callRequest.CallInfo);

            //更新缓存
            RCallCacheAddRequest RCallCacheAddRequest = new RCallCacheAddRequest();
            RCallCacheAddRequest.RCallInfo = callRequest.CallInfo;
            _RCallCacheService.AddRCallCache(RCallCacheAddRequest);

            callresponse.Data = ObjectConverter.Copy<R_CallModel, R_CallInfo>(resultcall);
            return callresponse;
        }
        /// <summary>
        /// 将R_Call转换成呼叫历史记录，并写入到R_PHJ表中
        /// </summary>
        /// <param name="tempCallInfo"></param>
        public void AddR_PHJInfo(R_CallInfo tempCallInfo)
        {
            //写呼叫历史记录
            R_PhjInfo r_phj = new R_PhjInfo();
            r_phj.Id = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
            r_phj.Hjlx = 1;
            r_phj.Bh = "0";
            r_phj.Yid = "0";
            r_phj.PointId = "0";
            r_phj.CallTime = DateTime.Now;
            r_phj.Tsycs = 0;
            r_phj.State = tempCallInfo.Type;
            r_phj.Type = tempCallInfo.CallType;
            //修改，将呼叫卡号及设备写入明细表中
            //if (tempCallInfo.Type == 0)
            //{
            //    r_phj.Card = tempCallInfo.BhContent;
            //}
            //else
            //{
            //    r_phj.Card = tempCallInfo.PointList;
            //}            
            r_phj.Username = tempCallInfo.CreateUserName;
            r_phj.IP = tempCallInfo.CreateClientIP;
            r_phj.Timer = DateTime.Now;
            r_phj.Flag = 0;
            r_phj.SysFlag = 0;
            r_phj.upflag = "0";
            r_phj.By2 = tempCallInfo.Type.ToString();
            PhjAddRequest phjRequest = new PhjAddRequest();
            phjRequest.PhjInfo = r_phj;
            _R_PhjService.AddPhjToDB(phjRequest);
            //添加呼叫明细表  20180312
            if (tempCallInfo.BhContent != null && tempCallInfo.BhContent.Length > 0)
            {
                string[] bhList;
                if (tempCallInfo.BhContent.Contains("-"))
                {
                    int sBh = int.Parse(tempCallInfo.BhContent.Split('-')[0]);
                    int eBh = int.Parse(tempCallInfo.BhContent.Split('-')[1]);
                    bhList = new string[12];
                    int bhIndex = sBh;
                    for (int i = 0; i < 12; i++)
                    {
                        bhList[i] = bhIndex.ToString();
                        bhIndex++;
                    }
                }
                bhList = tempCallInfo.BhContent.Split(',');
                foreach (string bh in bhList)
                {
                    List<R_PersoninfInfo> tempPerson = _R_PersoninfService.GetPersoninfCacheByBh(new Sys.Safety.Request.R_Personinf.R_PersoninfGetByBhRequest() { Bh = bh }).Data;
                    if (tempPerson.Count > 0)
                    {
                        //写入呼叫人员历史记录明细表中  20180312
                        _Repository.ExecuteNonQuery("global_R_PhjService_AddPhjPersonDetailToDB",
                            r_phj.Timer.ToString("yyyyMM"),
                            Basic.Framework.Common.IdHelper.CreateLongId().ToString(),
                            r_phj.Id,
                            tempPerson[0].Yid);
                    }
                }
            }
            if (tempCallInfo.PointList != null && tempCallInfo.PointList.Length > 0)
            {
                string[] pointList = tempCallInfo.PointList.Split(',');
                foreach (string point in pointList)
                {
                    PointDefineGetByPointRequest PointDefineRequest = new PointDefineGetByPointRequest();
                    PointDefineRequest.Point = point;
                    Jc_DefInfo tempPoint = _PersonPointDefineService.GetPointDefineCacheByPoint(PointDefineRequest).Data;
                    if (tempPoint != null)
                    {
                        //写入呼叫人员历史记录明细表中  20180312
                        _Repository.ExecuteNonQuery("global_R_PhjService_AddPhjPointDetailToDB",
                            r_phj.Timer.ToString("yyyyMM"),
                            Basic.Framework.Common.IdHelper.CreateLongId().ToString(),
                            r_phj.Id,
                            tempPoint.PointID);
                    }
                }
            }
        }
        public BasicResponse<R_CallInfo> UpdateCall(R_CallUpdateRequest callRequest)
        {
            var callresponse = new BasicResponse<R_CallInfo>();

            //判断缓存中是否存在
            R_CallInfo oldR_CallInfo = null;
            RCallCacheGetByKeyRequest RCallCacheRequest = new RCallCacheGetByKeyRequest();
            RCallCacheRequest.Id = callRequest.CallInfo.Id;
            oldR_CallInfo = _RCallCacheService.GetByKeyRCallCache(RCallCacheRequest).Data;
            if (oldR_CallInfo == null)
            {
                //缓存中存在此测点
                callresponse.Code = 1;
                callresponse.Message = "当前更新的呼叫控制记录不存在！";
                return callresponse;
            }

            var _call = ObjectConverter.Copy<R_CallInfo, R_CallModel>(callRequest.CallInfo);
            _Repository.UpdateCall(_call);
            //写呼叫历史记录
            AddR_PHJInfo(callRequest.CallInfo);

            //更新缓存
            RCallCacheUpdateRequest RCallCacheUpdateRequest = new RCallCacheUpdateRequest();
            RCallCacheUpdateRequest.RCallInfo = callRequest.CallInfo;
            _RCallCacheService.UpdateRCallCache(RCallCacheUpdateRequest);

            callresponse.Data = ObjectConverter.Copy<R_CallModel, R_CallInfo>(_call);
            return callresponse;
        }
        public BasicResponse DeleteCall(R_CallDeleteRequest callRequest)
        {
            var callresponse = new BasicResponse();
            //判断缓存中是否存在
            R_CallInfo oldR_CallInfo = null;
            RCallCacheGetByKeyRequest RCallCacheRequest = new RCallCacheGetByKeyRequest();
            RCallCacheRequest.Id = callRequest.Id;
            oldR_CallInfo = _RCallCacheService.GetByKeyRCallCache(RCallCacheRequest).Data;
            if (oldR_CallInfo == null)
            {
                //缓存中存在此测点
                callresponse.Code = 1;
                callresponse.Message = "当前删除的呼叫控制记录不存在！";
                return callresponse;
            }

            _Repository.DeleteCall(callRequest.Id);

            //更新缓存
            RCallCacheDeleteRequest RCallCacheDeleteRequest = new RCallCacheDeleteRequest();
            RCallCacheDeleteRequest.RCallInfo = oldR_CallInfo;
            _RCallCacheService.DeleteRCallCache(RCallCacheDeleteRequest);

            return callresponse;
        }
        public BasicResponse<List<R_CallInfo>> GetCallList(R_CallGetListRequest callRequest)
        {
            var callresponse = new BasicResponse<List<R_CallInfo>>();
            callRequest.PagerInfo.PageIndex = callRequest.PagerInfo.PageIndex - 1;
            if (callRequest.PagerInfo.PageIndex < 0)
            {
                callRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var callModelLists = _Repository.GetCallList(callRequest.PagerInfo.PageIndex, callRequest.PagerInfo.PageSize, out rowcount);
            var callInfoLists = new List<R_CallInfo>();
            foreach (var item in callModelLists)
            {
                var CallInfo = ObjectConverter.Copy<R_CallModel, R_CallInfo>(item);
                callInfoLists.Add(CallInfo);
            }
            callresponse.Data = callInfoLists;
            return callresponse;
        }
        public BasicResponse<R_CallInfo> GetCallById(R_CallGetRequest callRequest)
        {
            var result = _Repository.GetCallById(callRequest.Id);
            var callInfo = ObjectConverter.Copy<R_CallModel, R_CallInfo>(result);
            var callresponse = new BasicResponse<R_CallInfo>();
            callresponse.Data = callInfo;
            return callresponse;
        }


        public BasicResponse<List<R_CallInfo>> GetAllCall()
        {
            var callresponse = new BasicResponse<List<R_CallInfo>>();

            var callModelLists = _Repository.Datas.ToList();
            var callInfoLists = ObjectConverter.CopyList<R_CallModel, R_CallInfo>(callModelLists);
            callresponse.Data = callInfoLists.ToList();
            return callresponse;
        }
        /// <summary>
        /// 更新指定属性（只更新缓存）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BasicResponse BachUpdateAlarmInfoProperties(R_CallUpdateProperitesRequest upRequest)
        {
            RCallCache.RCallCahceInstance.BachUpdateAlarmInfoProperties(upRequest.updateItems);
            return new BasicResponse();
        }
        /// <summary>
        /// 获取所有人员呼叫缓存
        /// </summary>
        /// <param name="RCallCacheRequest"></param>
        /// <returns></returns>
        public Basic.Framework.Web.BasicResponse<List<DataContract.R_CallInfo>> GetAllRCallCache(Sys.Safety.Request.PersonCache.RCallCacheGetAllRequest RCallCacheRequest)
        {
            return _RCallCacheService.GetAllRCallCache(RCallCacheRequest);
        }
        /// <summary>
        /// 根据ID获取人员呼叫缓存
        /// </summary>
        /// <param name="RCallCacheRequest"></param>
        /// <returns></returns>
        public Basic.Framework.Web.BasicResponse<DataContract.R_CallInfo> GetByKeyRCallCache(Sys.Safety.Request.PersonCache.RCallCacheGetByKeyRequest RCallCacheRequest)
        {
            return _RCallCacheService.GetByKeyRCallCache(RCallCacheRequest);
        }

        public BasicResponse<List<R_CallInfo>> GetRCallInfoByMasterID(RCallInfoGetByMasterIDRequest RCallCacheRequest)
        {
            var callresponse = new BasicResponse<List<R_CallInfo>>();

            RCallCacheGetByConditionRequest r_defExistsRequest = new RCallCacheGetByConditionRequest();
            if (RCallCacheRequest.IsQueryByType)
            {
                r_defExistsRequest.Predicate = o => o.MasterId == RCallCacheRequest.MasterId && o.Type == RCallCacheRequest.Type && o.CallType == RCallCacheRequest.CallType;
            }
            else
            {
                r_defExistsRequest.Predicate = o => o.MasterId == RCallCacheRequest.MasterId && o.CallType == RCallCacheRequest.CallType;
            }
            var rcallinfo = _RCallCacheService.GetRCallCache(r_defExistsRequest).Data;
            callresponse.Data = rcallinfo;
            return callresponse;
        }

        public BasicResponse EndRcallByRcallInfoList(EndRcallByRcallInfoListEequest request)
        {
            //var dicInPar = new Dictionary<string, object>();
            //dicInPar.Add("CallType", 2);
            //var dicPar = new Dictionary<string, Dictionary<string, object>>();
            //foreach (var item in request.RcallInfo)
            //{
            //    if (item.CallType == 1)
            //    {
            //        dicPar.Add(item.Id, dicInPar);
            //    }
            //}
            //var req4 = new R_CallUpdateProperitesRequest()
            //{
            //    updateItems = dicPar
            //};
            //this.BachUpdateAlarmInfoProperties(req4);

            //var models = ObjectConverter.CopyList<R_CallInfo, R_CallModel>(request.RcallInfo);
            TransactionsManager.BeginTransaction(() =>
            {
                foreach (var item in request.RcallInfo)
                {
                    if (item.CallType == 1)
                    {
                        item.CallType = 2;
                        //_Repository.UpdateCall(item);
                        var req = new R_CallUpdateRequest()
                        {
                            CallInfo = item
                        };
                        UpdateCall(req);
                    }
                }
            });

            return new BasicResponse();
        }

        public BasicResponse EndRcallDbByRcallInfoList(EndRcallDbByRcallInfoListEequest request)
        {
            TransactionsManager.BeginTransaction(() =>
            {
                foreach (var item in request.RcallInfo)
                {
                    if (item.CallType == 1)
                    {
                        item.CallType = 2;
                        var req = new R_CallUpdateRequest()
                        {
                            CallInfo = item
                        };
                        var _call = ObjectConverter.Copy<R_CallInfo, R_CallModel>(item);
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


