using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.EmergencyLinkHistory;
using Basic.Framework.Common;
using Basic.Framework.Data;
using Basic.Framework.Web;
using Sys.Safety.DataAccess;
using Sys.Safety.Enums;
using Sys.Safety.Request.Listex;

namespace Sys.Safety.Services
{
    public partial class EmergencyLinkHistoryService : IEmergencyLinkHistoryService
    {
        private IEmergencyLinkageHistoryMasterPointAssRepository _emergencyLinkageHistoryMasterPointAssRepository;

        private IEmergencyLinkHistoryRepository _Repository;

        private ISysEmergencyLinkageRepository _sysEmergencyLinkageRepository;

        public EmergencyLinkHistoryService(IEmergencyLinkHistoryRepository _Repository, IEmergencyLinkageHistoryMasterPointAssRepository emergencyLinkageHistoryMasterPointAssRepository, ISysEmergencyLinkageRepository sysEmergencyLinkageRepository)
        {
            this._Repository = _Repository;
            _emergencyLinkageHistoryMasterPointAssRepository = emergencyLinkageHistoryMasterPointAssRepository;
            _sysEmergencyLinkageRepository = sysEmergencyLinkageRepository;
        }
        public BasicResponse<EmergencyLinkHistoryInfo> AddEmergencyLinkHistory(EmergencyLinkHistoryAddRequest emergencyLinkHistoryRequest)
        {
            var _emergencyLinkHistory = ObjectConverter.Copy<EmergencyLinkHistoryInfo, EmergencyLinkHistoryModel>(emergencyLinkHistoryRequest.EmergencyLinkHistoryInfo);
            var resultemergencyLinkHistory = _Repository.AddEmergencyLinkHistory(_emergencyLinkHistory);
            var emergencyLinkHistoryresponse = new BasicResponse<EmergencyLinkHistoryInfo>();
            emergencyLinkHistoryresponse.Data = ObjectConverter.Copy<EmergencyLinkHistoryModel, EmergencyLinkHistoryInfo>(resultemergencyLinkHistory);
            return emergencyLinkHistoryresponse;
        }
        public BasicResponse<EmergencyLinkHistoryInfo> UpdateEmergencyLinkHistory(EmergencyLinkHistoryUpdateRequest emergencyLinkHistoryRequest)
        {
            var _emergencyLinkHistory = ObjectConverter.Copy<EmergencyLinkHistoryInfo, EmergencyLinkHistoryModel>(emergencyLinkHistoryRequest.EmergencyLinkHistoryInfo);
            _Repository.UpdateEmergencyLinkHistory(_emergencyLinkHistory);
            var emergencyLinkHistoryresponse = new BasicResponse<EmergencyLinkHistoryInfo>();
            emergencyLinkHistoryresponse.Data = ObjectConverter.Copy<EmergencyLinkHistoryModel, EmergencyLinkHistoryInfo>(_emergencyLinkHistory);
            return emergencyLinkHistoryresponse;
        }
        public BasicResponse DeleteEmergencyLinkHistory(EmergencyLinkHistoryDeleteRequest emergencyLinkHistoryRequest)
        {
            _Repository.DeleteEmergencyLinkHistory(emergencyLinkHistoryRequest.Id);
            var emergencyLinkHistoryresponse = new BasicResponse();
            return emergencyLinkHistoryresponse;
        }
        public BasicResponse<List<EmergencyLinkHistoryInfo>> GetEmergencyLinkHistoryList(EmergencyLinkHistoryGetListRequest emergencyLinkHistoryRequest)
        {
            var emergencyLinkHistoryresponse = new BasicResponse<List<EmergencyLinkHistoryInfo>>();
            emergencyLinkHistoryRequest.PagerInfo.PageIndex = emergencyLinkHistoryRequest.PagerInfo.PageIndex - 1;
            if (emergencyLinkHistoryRequest.PagerInfo.PageIndex < 0)
            {
                emergencyLinkHistoryRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var emergencyLinkHistoryModelLists = _Repository.GetEmergencyLinkHistoryList(emergencyLinkHistoryRequest.PagerInfo.PageIndex, emergencyLinkHistoryRequest.PagerInfo.PageSize, out rowcount);
            var emergencyLinkHistoryInfoLists = new List<EmergencyLinkHistoryInfo>();
            foreach (var item in emergencyLinkHistoryModelLists)
            {
                var EmergencyLinkHistoryInfo = ObjectConverter.Copy<EmergencyLinkHistoryModel, EmergencyLinkHistoryInfo>(item);
                emergencyLinkHistoryInfoLists.Add(EmergencyLinkHistoryInfo);
            }
            emergencyLinkHistoryresponse.Data = emergencyLinkHistoryInfoLists;
            return emergencyLinkHistoryresponse;
        }
        public BasicResponse<EmergencyLinkHistoryInfo> GetEmergencyLinkHistoryById(EmergencyLinkHistoryGetRequest emergencyLinkHistoryRequest)
        {
            var result = _Repository.GetEmergencyLinkHistoryById(emergencyLinkHistoryRequest.Id);
            var emergencyLinkHistoryInfo = ObjectConverter.Copy<EmergencyLinkHistoryModel, EmergencyLinkHistoryInfo>(result);
            var emergencyLinkHistoryresponse = new BasicResponse<EmergencyLinkHistoryInfo>();
            emergencyLinkHistoryresponse.Data = emergencyLinkHistoryInfo;
            return emergencyLinkHistoryresponse;
        }
        
        public BasicResponse<EmergencyLinkHistoryInfo> GetEmergencyLinkHistoryByEmergency(EmergencyLinkHistoryGetByEmergencyRequest emergencyLinkHistoryRequest)
        {
            var result = _Repository.Datas.FirstOrDefault(o => o.SysEmergencyLinkageId == emergencyLinkHistoryRequest.EmergencyId && o.EndTime == new DateTime(1900, 1, 1, 0, 0, 0));

            var emergencyLinkHistoryInfo = ObjectConverter.Copy<EmergencyLinkHistoryModel, EmergencyLinkHistoryInfo>(result);
            var emergencyLinkHistoryresponse = new BasicResponse<EmergencyLinkHistoryInfo>();
            emergencyLinkHistoryresponse.Data = emergencyLinkHistoryInfo;
            return emergencyLinkHistoryresponse;
        }

        public BasicResponse BatchAddEmergencyLinkHistory(BatchAddEmergencyLinkHistoryRequest request)
        {
            var lisModel =
                ObjectConverter.CopyList<EmergencyLinkHistoryInfo, EmergencyLinkHistoryModel>(
                    request.LisEmergencyLinkHistoryInfo);
            TransactionsManager.BeginTransaction(() =>
            {
                foreach (var itmem in lisModel)
                {
                    _Repository.AddEmergencyLinkHistory(itmem);
                }
            });
            return new BasicResponse();
        }

        public BasicResponse EndAll(EndAllRequest request)
        {
            var notEndRec = _Repository.GetNotEndEmergencyLinkHistory();
            TransactionsManager.BeginTransaction(() =>
            {
                foreach (var item in notEndRec)
                {
                    item.EndTime = request.EndTime;
                    _Repository.UpdateEmergencyLinkHistory(item);
                }
            });
            return new BasicResponse();
        }

        public BasicResponse EndByLinkageId(EndByLinkageIdRequest request)
        {
            var models = _Repository.GetNotEndEmergencyLinkHistoryByLinkageId(request.Id);
            
            TransactionsManager.BeginTransaction(() =>
            {
                foreach (var item in models)
                {
                    if (item.EndTime != new DateTime(1900, 1, 1))
                    {
                        continue;
                    }

                    item.EndTime = request.EndTime;
                    _Repository.UpdateEmergencyLinkHistory(item);
                }
            });
            return new BasicResponse();
        }

        public BasicResponse<IList<EmergencyLinkageHistoryMasterPointAssInfo>>
            GetNotEndLastLinkageHistoryMasterPointByLinkageId(LongIdRequest request)
        {
            var ret = new BasicResponse<IList<EmergencyLinkageHistoryMasterPointAssInfo>>()
            {
                Data = new List<EmergencyLinkageHistoryMasterPointAssInfo>()
            };

            var lastLinkHistory = _Repository.GetLastLinkHistoryByLinkageId(request.Id.ToString());
            if (lastLinkHistory == null)
            {
                return ret;
            }

            if (lastLinkHistory.EndTime != new DateTime(1900, 1, 1))
            {
                return ret;
            }

            var model = _emergencyLinkageHistoryMasterPointAssRepository.GetLinkageHistoryMasterPointAssListByLinkageHistoryId(
                lastLinkHistory.Id);
            var info = ObjectConverter
                .CopyList<EmergencyLinkageHistoryMasterPointAssModel, EmergencyLinkageHistoryMasterPointAssInfo>(model);
            ret.Data = info;
            return ret;
        }

        public BasicResponse<IList<SysEmergencyLinkageInfo>> GetDeleteButNotEndLinkageIds()
        {
            var time1900 = new DateTime(1900, 1, 1);
            var res = _Repository.Datas.Where(a => a.EndTime == time1900).Select(a => a.SysEmergencyLinkageId).ToList();
            //var res2 = _sysEmergencyLinkageRepository.Datas.Where(a => a.Activity != 1 && a.Type == 1 && res.Contains(a.Id));
            var res2 = _sysEmergencyLinkageRepository.Datas.Where(a => a.Activity != 1 && res.Contains(a.Id));
            var res3 = ObjectConverter.CopyList<SysEmergencyLinkageModel, SysEmergencyLinkageInfo>(res2.ToList());
            var ret = new BasicResponse<IList<SysEmergencyLinkageInfo>>()
            {
                Data = res3
            };
            return ret;
        }

        public BasicResponse AddEmergencyLinkHistoryAndAss(AddEmergencyLinkHistoryAndAssRequest request)
        {
            var primaryId = IdHelper.CreateLongId().ToString();
            request.EmergencyLinkHistoryInfo.Id = primaryId;
            foreach (var item in request.LinkageHistoryMasterPointAssInfoList)
            {
                item.Id = IdHelper.CreateLongId().ToString();
                item.EmergencyLinkHistoryId = primaryId;
            }
            var model =
                ObjectConverter.Copy<EmergencyLinkHistoryInfo, EmergencyLinkHistoryModel>(
                    request.EmergencyLinkHistoryInfo);
            var models =
                ObjectConverter
                    .CopyList<EmergencyLinkageHistoryMasterPointAssInfo, EmergencyLinkageHistoryMasterPointAssModel>(
                        request.LinkageHistoryMasterPointAssInfoList);

            TransactionsManager.BeginTransaction(() =>
            {
                _Repository.AddEmergencyLinkHistory(model);
                foreach (var item in models)
                {
                    _emergencyLinkageHistoryMasterPointAssRepository.AddEmergencyLinkageHistoryMasterPointAss(item);
                }
            });
            return new BasicResponse();
        }
    }
}


