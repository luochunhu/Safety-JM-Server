using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using Basic.Framework.Common;
using Basic.Framework.Data;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.PersonCache;
using Sys.Safety.Request.Area;
using Sys.Safety.Request.Def;
using Sys.Safety.Request.Enumcode;
using Sys.Safety.Request.Listex;
using Sys.Safety.Request.SysEmergencyLinkage;
using Sys.Safety.ServiceContract;
using Sys.Safety.ServiceContract.Cache;
using Sys.Safety.ServiceContract.KJ237Cache;
using Sys.Safety.Request;

namespace Sys.Safety.Services
{
    public class SysEmergencyLinkageService : ISysEmergencyLinkageService
    {
        private readonly ISysEmergencyLinkageRepository _Repository;

        private readonly IEmergencyLinkageMasterAreaAssRepository _emergencyLinkageMasterAreaAssRepository;

        private readonly IEmergencyLinkageMasterDevTypeAssRepository _emergencyLinkageMasterDevTypeAssRepository;

        private readonly IEmergencyLinkageMasterPointAssRepository _emergencyLinkageMasterPointAssRepository;

        private readonly IEmergencyLinkageMasterTriDataStateAssRepository _emergencyLinkageMasterTriDataStateAssRepository;

        private readonly IEmergencyLinkagePassiveAreaAssRepository _emergencyLinkagePassiveAreaAssRepository;

        private readonly IEmergencyLinkagePassivePersonAssRepository _emergencyLinkagePassivePersonAssRepository;

        private readonly IEmergencyLinkagePassivePointAssRepository _emergencyLinkagePassivePointAssRepository;

        private readonly IPointDefineCacheService _pointDefineCacheService;

        private readonly IAreaCacheService _areaCacheService;

        private readonly IDeviceDefineCacheService _deviceDefineCacheService;

        private readonly IEnumcodeService _enumcodeService;

        private readonly IRPersoninfCacheService _rPersoninfCacheService;

        private readonly IPersonPointDefineService _personPointDefineService;

        private readonly IV_DefCacheService _vDefCacheService;

        private readonly IB_DefCacheService _bDefCacheService;

        private readonly ISysEmergencyLinkageCacheService _sysEmergencyLinkageCacheService;

        private readonly ILargeDataAnalysisConfigCacheService _largeDataAnalysisConfigCacheService;

        public SysEmergencyLinkageService(ISysEmergencyLinkageRepository _Repository,
            IEmergencyLinkageMasterAreaAssRepository emergencyLinkageMasterAreaAssRepository,
            IEmergencyLinkageMasterDevTypeAssRepository emergencyLinkageMasterDevTypeAssRepository,
            IEmergencyLinkageMasterPointAssRepository emergencyLinkageMasterPointAssRepository,
            IEmergencyLinkageMasterTriDataStateAssRepository emergencyLinkageMasterTriDataStateAssRepository,
            IEmergencyLinkagePassiveAreaAssRepository emergencyLinkagePassiveAreaAssRepository,
            IEmergencyLinkagePassivePersonAssRepository emergencyLinkagePassivePersonAssRepository,
            IEmergencyLinkagePassivePointAssRepository emergencyLinkagePassivePointAssRepository,
            IPointDefineCacheService pointDefineCacheService, IAreaCacheService areaCacheService,
            IDeviceDefineCacheService deviceDefineCacheService, IEnumcodeService enumcodeService,
            IRPersoninfCacheService rPersoninfCacheService, IPersonPointDefineService personPointDefineService,
            IB_DefCacheService bDefCacheService,
            ISysEmergencyLinkageCacheService sysEmergencyLinkageCacheService,
            ILargeDataAnalysisConfigCacheService largeDataAnalysisConfigCacheService,
            IV_DefCacheService vDefCacheService)
        {
            this._Repository = _Repository;
            _emergencyLinkageMasterAreaAssRepository = emergencyLinkageMasterAreaAssRepository;
            _emergencyLinkageMasterDevTypeAssRepository = emergencyLinkageMasterDevTypeAssRepository;
            _emergencyLinkageMasterPointAssRepository = emergencyLinkageMasterPointAssRepository;
            _emergencyLinkageMasterTriDataStateAssRepository = emergencyLinkageMasterTriDataStateAssRepository;
            _emergencyLinkagePassiveAreaAssRepository = emergencyLinkagePassiveAreaAssRepository;
            _emergencyLinkagePassivePersonAssRepository = emergencyLinkagePassivePersonAssRepository;
            _emergencyLinkagePassivePointAssRepository = emergencyLinkagePassivePointAssRepository;
            _pointDefineCacheService = pointDefineCacheService;
            _areaCacheService = areaCacheService;
            _deviceDefineCacheService = deviceDefineCacheService;
            _enumcodeService = enumcodeService;
            _rPersoninfCacheService = rPersoninfCacheService;
            _personPointDefineService = personPointDefineService;
            _bDefCacheService = bDefCacheService;
            _sysEmergencyLinkageCacheService = sysEmergencyLinkageCacheService;
            _largeDataAnalysisConfigCacheService = largeDataAnalysisConfigCacheService;
            _vDefCacheService = vDefCacheService;
        }

        //public BasicResponse<SysEmergencyLinkageInfo> AddSysEmergencyLinkage(
        //    SysEmergencyLinkageAddRequest sysEmergencyLinkageRequest)
        //{
        //    var _sysEmergencyLinkage =
        //        ObjectConverter.Copy<SysEmergencyLinkageInfo, SysEmergencyLinkageModel>(sysEmergencyLinkageRequest
        //            .SysEmergencyLinkageInfo);
        //    var resultsysEmergencyLinkage = _Repository.AddSysEmergencyLinkage(_sysEmergencyLinkage);
        //    var sysEmergencyLinkageresponse = new BasicResponse<SysEmergencyLinkageInfo>();
        //    sysEmergencyLinkageresponse.Data =
        //        ObjectConverter.Copy<SysEmergencyLinkageModel, SysEmergencyLinkageInfo>(resultsysEmergencyLinkage);
        //    return sysEmergencyLinkageresponse;
        //}

        //public BasicResponse<SysEmergencyLinkageInfo> UpdateSysEmergencyLinkage(
        //    SysEmergencyLinkageUpdateRequest sysEmergencyLinkageRequest)
        //{
        //    var _sysEmergencyLinkage =
        //        ObjectConverter.Copy<SysEmergencyLinkageInfo, SysEmergencyLinkageModel>(sysEmergencyLinkageRequest
        //            .SysEmergencyLinkageInfo);
        //    _Repository.UpdateSysEmergencyLinkage(_sysEmergencyLinkage);
        //    var sysEmergencyLinkageresponse = new BasicResponse<SysEmergencyLinkageInfo>();
        //    sysEmergencyLinkageresponse.Data =
        //        ObjectConverter.Copy<SysEmergencyLinkageModel, SysEmergencyLinkageInfo>(_sysEmergencyLinkage);
        //    return sysEmergencyLinkageresponse;
        //}

        //public BasicResponse DeleteSysEmergencyLinkage(SysEmergencyLinkageDeleteRequest sysEmergencyLinkageRequest)
        //{
        //    _Repository.DeleteSysEmergencyLinkage(sysEmergencyLinkageRequest.Id);
        //    var sysEmergencyLinkageresponse = new BasicResponse();
        //    return sysEmergencyLinkageresponse;
        //}

        public BasicResponse<List<SysEmergencyLinkageInfo>> GetSysEmergencyLinkageList(
            SysEmergencyLinkageGetListRequest sysEmergencyLinkageRequest)
        {
            var sysEmergencyLinkageresponse = new BasicResponse<List<SysEmergencyLinkageInfo>>();
            sysEmergencyLinkageRequest.PagerInfo.PageIndex = sysEmergencyLinkageRequest.PagerInfo.PageIndex - 1;
            if (sysEmergencyLinkageRequest.PagerInfo.PageIndex < 0)
                sysEmergencyLinkageRequest.PagerInfo.PageIndex = 0;
            var rowcount = 0;
            var sysEmergencyLinkageModelLists = _Repository.GetSysEmergencyLinkageList(
                sysEmergencyLinkageRequest.PagerInfo.PageIndex, sysEmergencyLinkageRequest.PagerInfo.PageSize,
                out rowcount);
            var sysEmergencyLinkageInfoLists = new List<SysEmergencyLinkageInfo>();
            foreach (var item in sysEmergencyLinkageModelLists)
            {
                var SysEmergencyLinkageInfo =
                    ObjectConverter.Copy<SysEmergencyLinkageModel, SysEmergencyLinkageInfo>(item);
                sysEmergencyLinkageInfoLists.Add(SysEmergencyLinkageInfo);
            }
            sysEmergencyLinkageresponse.Data = sysEmergencyLinkageInfoLists;
            return sysEmergencyLinkageresponse;
        }

        public BasicResponse<SysEmergencyLinkageInfo> GetSysEmergencyLinkageById(
            SysEmergencyLinkageGetRequest sysEmergencyLinkageRequest)
        {
            var result = _Repository.GetSysEmergencyLinkageById(sysEmergencyLinkageRequest.Id);
            var sysEmergencyLinkageInfo =
                ObjectConverter.Copy<SysEmergencyLinkageModel, SysEmergencyLinkageInfo>(result);
            var sysEmergencyLinkageresponse = new BasicResponse<SysEmergencyLinkageInfo>();
            sysEmergencyLinkageresponse.Data = sysEmergencyLinkageInfo;
            return sysEmergencyLinkageresponse;
        }

        public BasicResponse AddEmergencylinkageconfigMasterInfoPassiveInfo(
            AddEmergencylinkageconfigMasterInfoPassiveInfoRequest request)
        {
            var exist = _Repository.AnySysEmergencyLinkageByLambda(a => a.Name == request.SysEmergencyLinkageInfo.Name && a.Activity == 1);
            if (exist)
            {
                return new BasicResponse()
                {
                    Code = 1,
                    Message = "应急联动名称已存在！"
                };
            }

            //同一个大数据分析模型只能配置一个联动
            var masterModelId = request.SysEmergencyLinkageInfo.MasterModelId;
            if (masterModelId != "0")
            {
                var req4 = new EmergencyLinkageConfigCacheGetByConditonRequest
                {
                    Predicate = a => a.MasterModelId == masterModelId
                };
                var res4 = _sysEmergencyLinkageCacheService.GetSysEmergencyLinkageCache(req4);
                if (res4.Data.Count != 0)
                {
                    return new BasicResponse()
                    {
                        Code = 2,
                        Message = "同一个大数据分析模型只能配置一个应急联动！"
                    };
                }
            }

            request.SysEmergencyLinkageInfo.Id = IdHelper.CreateLongId().ToString();

            //主控测点关联id
            if (request.EmergencyLinkageMasterPointAssInfo.Count != 0)
            {
                request.SysEmergencyLinkageInfo.MasterPointAssId = IdHelper.CreateLongId().ToString();
            }
            else
            {
                request.SysEmergencyLinkageInfo.MasterPointAssId = "0";
            }

            //主控设备类型关联id
            if (request.EmergencyLinkageMasterDevTypeAssInfo.Count != 0)
            {
                request.SysEmergencyLinkageInfo.MasterDevTypeAssId = IdHelper.CreateLongId().ToString();
            }
            else
            {
                request.SysEmergencyLinkageInfo.MasterDevTypeAssId = "0";
            }

            //主控区域关联id
            if (request.EmergencyLinkageMasterAreaAssInfo.Count != 0)
            {
                request.SysEmergencyLinkageInfo.MasterAreaAssId = IdHelper.CreateLongId().ToString();
            }
            else
            {
                request.SysEmergencyLinkageInfo.MasterAreaAssId = "0";
            }

            request.SysEmergencyLinkageInfo.MasterTriDataStateAssId = IdHelper.CreateLongId().ToString();       //主控触发数据状态

            //被控测点关联id
            if (request.EmergencyLinkagePassivePointAssInfo.Count != 0)
            {
                request.SysEmergencyLinkageInfo.PassivePointAssId = IdHelper.CreateLongId().ToString();
            }
            else
            {
                request.SysEmergencyLinkageInfo.PassivePointAssId = "0";
            }

            //被控区域关联id
            if (request.EmergencyLinkagePassiveAreaAssInfo.Count != 0)
            {
                request.SysEmergencyLinkageInfo.PassiveAreaAssId = IdHelper.CreateLongId().ToString();
            }
            else
            {
                request.SysEmergencyLinkageInfo.PassiveAreaAssId = "0";
            }

            //被控人员关联id
            if (request.EmergencyLinkagePassivePersonAssInfo.Count != 0)
            {
                request.SysEmergencyLinkageInfo.PassivePersonAssId = IdHelper.CreateLongId().ToString();
            }
            else
            {
                request.SysEmergencyLinkageInfo.PassivePersonAssId = "0";
            }

            foreach (var item in request.EmergencyLinkageMasterAreaAssInfo)
            {
                item.Id = IdHelper.CreateLongId().ToString();
                item.MasterAreaAssId = request.SysEmergencyLinkageInfo.MasterAreaAssId;
            }

            foreach (var item in request.EmergencyLinkageMasterDevTypeAssInfo)
            {
                item.Id = IdHelper.CreateLongId().ToString();
                item.MasterDevTypeAssId = request.SysEmergencyLinkageInfo.MasterDevTypeAssId;
            }

            foreach (var item in request.EmergencyLinkageMasterPointAssInfo)
            {
                item.Id = IdHelper.CreateLongId().ToString();
                item.MasterPointAssId = request.SysEmergencyLinkageInfo.MasterPointAssId;
            }

            foreach (var item in request.EmergencyLinkageMasterTriDataStateAssInfo)
            {
                item.Id = IdHelper.CreateLongId().ToString();
                item.MasterTriDataStateAssId = request.SysEmergencyLinkageInfo.MasterTriDataStateAssId;
            }

            foreach (var item in request.EmergencyLinkagePassiveAreaAssInfo)
            {
                item.Id = IdHelper.CreateLongId().ToString();
                item.PassiveAreaAssId = request.SysEmergencyLinkageInfo.PassiveAreaAssId;
            }

            foreach (var item in request.EmergencyLinkagePassivePersonAssInfo)
            {
                item.Id = IdHelper.CreateLongId().ToString();
                item.PassivePersonAssId = request.SysEmergencyLinkageInfo.PassivePersonAssId;
            }

            var req = new V_DefCacheGetAllRequest();
            var res = _vDefCacheService.GetAll(req);
            if (res.Code != 100)
            {
                throw new Exception(res.Message);
            }
            var allVideoPoint = res.Data;     //所有视频系统测点

            var res2 = _personPointDefineService.GetAllPointDefineCache();
            if (res2.Code != 100)
            {
                throw new Exception(res2.Message);
            }
            var allPersonPoint = res2.Data;     //所有人员定位系统测点

            var req3 = new B_DefCacheGetAllRequest();
            var res3 = _bDefCacheService.GetAll(req3);
            if (res3.Code != 100)
            {
                throw new Exception(res3.Message);
            }
            var allBroadcastPoint = res3.Data;      //所有广播系统测点


            foreach (var item in request.EmergencyLinkagePassivePointAssInfo)
            {
                item.Id = IdHelper.CreateLongId().ToString();
                item.PassivePointAssId = request.SysEmergencyLinkageInfo.PassivePointAssId;

                var existMonitoringPoint = allVideoPoint.Any(a => a.Id == item.PointId);
                if (existMonitoringPoint)
                {
                    item.Sysid = 74;
                }

                var existPersonPoint = allPersonPoint.Any(a => a.PointID == item.PointId);
                if (existPersonPoint)
                {
                    item.Sysid = 11;
                }

                var existBroadcastPoint = allBroadcastPoint.Any(a => a.PointID == item.PointId);
                if (existBroadcastPoint)
                {
                    item.Sysid = 12;
                }
            }

            request.SysEmergencyLinkageInfo.EditTime = DateTime.Now;
            request.SysEmergencyLinkageInfo.Activity = 1;
            request.SysEmergencyLinkageInfo.DeletePerson = "";
            request.SysEmergencyLinkageInfo.DeleteTime = Convert.ToDateTime("1900-01-01");
            request.SysEmergencyLinkageInfo.Bz1 = "";
            request.SysEmergencyLinkageInfo.Bz2 = "";
            request.SysEmergencyLinkageInfo.Bz3 = "";

            TransactionsManager.BeginTransaction(() =>
            {
                var model = ObjectConverter.Copy<SysEmergencyLinkageInfo, SysEmergencyLinkageModel>(request.SysEmergencyLinkageInfo);
                _Repository.AddSysEmergencyLinkage(model);

                foreach (var item in request.EmergencyLinkageMasterAreaAssInfo)
                {
                    var model2 = ObjectConverter
                        .Copy<EmergencyLinkageMasterAreaAssInfo, EmergencyLinkageMasterAreaAssModel>(item);
                    _emergencyLinkageMasterAreaAssRepository.AddEmergencyLinkageMasterAreaAss(model2);
                }
                foreach (var item in request.EmergencyLinkageMasterDevTypeAssInfo)
                {
                    var model3 = ObjectConverter
                        .Copy<EmergencyLinkageMasterDevTypeAssInfo, EmergencyLinkageMasterDevTypeAssModel>(item);
                    _emergencyLinkageMasterDevTypeAssRepository.AddEmergencyLinkageMasterDevTypeAss(model3);
                }
                foreach (var item in request.EmergencyLinkageMasterPointAssInfo)
                {
                    var model4 = ObjectConverter
                        .Copy<EmergencyLinkageMasterPointAssInfo, EmergencyLinkageMasterPointAssModel>(item);
                    _emergencyLinkageMasterPointAssRepository.AddEmergencyLinkageMasterPointAss(model4);
                }
                foreach (var item in request.EmergencyLinkageMasterTriDataStateAssInfo)
                {
                    var model5 = ObjectConverter
                        .Copy<EmergencyLinkageMasterTriDataStateAssInfo, EmergencyLinkageMasterTriDataStateAssModel>(
                            item);
                    _emergencyLinkageMasterTriDataStateAssRepository.AddEmergencyLinkageMasterTriDataStateAss(model5);
                }
                foreach (var item in request.EmergencyLinkagePassiveAreaAssInfo)
                {
                    var model6 = ObjectConverter
                        .Copy<EmergencyLinkagePassiveAreaAssInfo, EmergencyLinkagePassiveAreaAssModel>(item);
                    _emergencyLinkagePassiveAreaAssRepository.AddEmergencyLinkagePassiveAreaAss(model6);
                }
                foreach (var item in request.EmergencyLinkagePassivePersonAssInfo)
                {
                    var model7 = ObjectConverter
                        .Copy<EmergencyLinkagePassivePersonAssInfo, EmergencyLinkagePassivePersonAssModel>(item);
                    _emergencyLinkagePassivePersonAssRepository.AddEmergencyLinkagePassivePersonAss(model7);
                }
                foreach (var item in request.EmergencyLinkagePassivePointAssInfo)
                {
                    var model8 = ObjectConverter
                        .Copy<EmergencyLinkagePassivePointAssInfo, EmergencyLinkagePassivePointAssModel>(item);
                    _emergencyLinkagePassivePointAssRepository.AddEmergencyLinkagePassivePointAss(model8);
                }
            });

            //更新缓存
            request.SysEmergencyLinkageInfo.MasterAreas = request.EmergencyLinkageMasterAreaAssInfo;
            request.SysEmergencyLinkageInfo.MasterDevTypes = request.EmergencyLinkageMasterDevTypeAssInfo;
            request.SysEmergencyLinkageInfo.MasterPoint = request.EmergencyLinkageMasterPointAssInfo;
            request.SysEmergencyLinkageInfo.MasterTriDataStates = request.EmergencyLinkageMasterTriDataStateAssInfo;
            request.SysEmergencyLinkageInfo.PassiveAreas = request.EmergencyLinkagePassiveAreaAssInfo;
            request.SysEmergencyLinkageInfo.PassivePersons = request.EmergencyLinkagePassivePersonAssInfo;
            request.SysEmergencyLinkageInfo.PassivePoints = request.EmergencyLinkagePassivePointAssInfo;
            request.SysEmergencyLinkageInfo.EmergencyLinkageState = 0;

            var req2 = new EmergencyLinkageConfigCacheAddRequest
            {
                SysEmergencyLinkageInfo = request.SysEmergencyLinkageInfo
            };
            _sysEmergencyLinkageCacheService.AddSysEmergencyLinkageCache(req2);

            return new BasicResponse();
        }

        public BasicResponse<List<SysEmergencyLinkageInfo>> GetAllSysEmergencyLinkageList()
        {
            //var res = _Repository.GetAllSysEmergencyLinkageList();
            //var convert = ObjectConverter.CopyList<SysEmergencyLinkageModel, SysEmergencyLinkageInfo>(res).ToList();
            //var ret = new BasicResponse<List<SysEmergencyLinkageInfo>>()
            //{
            //    Data = convert
            //};
            //return ret;
            var req = new EmergencyLinkageConfigCacheGetAllRequest();
            var res = _sysEmergencyLinkageCacheService.GetAllSysEmergencyLinkageCache(req);
            var ret = new BasicResponse<List<SysEmergencyLinkageInfo>>()
            {
                Data = res.Data
            };
            return ret;
        }

        public BasicResponse<List<SysEmergencyLinkageInfo>> GetAllSysEmergencyLinkageListDb()
        {
            var res = _Repository.GetAllSysEmergencyLinkageList();
            var convert = ObjectConverter.CopyList<SysEmergencyLinkageModel, SysEmergencyLinkageInfo>(res).ToList();
            var ret = new BasicResponse<List<SysEmergencyLinkageInfo>>()
            {
                Data = convert
            };
            return ret;
        }

        public BasicResponse<List<GetSysEmergencyLinkageListAndStatisticsResponse>> GetSysEmergencyLinkageListAndStatistics(StringRequest request)
        {
            var lis = new List<GetSysEmergencyLinkageListAndStatisticsResponse>();

            var req2 = new LargeDataAnalysisConfigCacheGetAllRequest();
            var res2 = _largeDataAnalysisConfigCacheService.GetAllLargeDataAnalysisConfigCache(req2);
            var allLargeDataConfig = res2.Data;

            var req = new EmergencyLinkageConfigCacheGetAllRequest();
            var res = _sysEmergencyLinkageCacheService.GetAllSysEmergencyLinkageCache(req);
            var name = request.Str;

            List<SysEmergencyLinkageInfo> queryData;
            if (string.IsNullOrEmpty(name))
            {
                queryData = res.Data.OrderByDescending(a => a.EditTime).ToList();
            }
            else
            {
                queryData = res.Data.Where(a => a.Name == request.Str).OrderByDescending(a => a.EditTime).ToList();
            }
            foreach (var item in queryData)
            {
                var largeDataConfig = allLargeDataConfig.FirstOrDefault(a => a.Id == item.MasterModelId);
                var largeDataConfigName = largeDataConfig == null ? "" : largeDataConfig.Name;

                var newItem = new GetSysEmergencyLinkageListAndStatisticsResponse()
                {
                    DeletePerson = item.DeletePerson,
                    DeleteTime = item.DeleteTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    EditPerson = item.EditPerson,
                    EditTime = item.EditTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    Id = item.Id,
                    MasterAreaAssId = item.MasterAreaAssId,
                    MasterAreaNum = item.MasterAreas.Count.ToString(),
                    MasterDevTypeAssId = item.MasterDevTypeAssId,
                    MasterDevTypeNum = item.MasterDevTypes.Count.ToString(),
                    MasterModelName = largeDataConfigName,
                    MasterPointAssId = item.MasterPointAssId,
                    MasterPointNum = item.MasterPoint.Count.ToString(),
                    MasterTriDataStateAssId = item.MasterTriDataStateAssId,
                    MasterTriDataStateNum = item.MasterTriDataStates.Count.ToString(),
                    PassiveAreaAssId = item.PassiveAreaAssId,
                    PassiveAreaNum = item.PassiveAreas.Count.ToString(),
                    PassivePersonAssId = item.PassivePersonAssId,
                    PassivePersonNum = item.PassivePersons.Count.ToString(),
                    PassivePointAssId = item.PassivePointAssId,
                    PassivePointNum = item.PassivePoints.Count.ToString(),
                    Name = item.Name,
                    Type = item.Type,
                    TypeName = item.Type == 1 ? "普通联动" : "大数据分析联动",
                    Duration = item.Duration
                };
                lis.Add(newItem);
            }

            //var name = request.Str;
            //var dt = _Repository.QueryTable(
            //    "global_SysEmergencyLinkageService_GetSysEmergencyLinkageListAndStatistics", name);

            //foreach (DataRow item in dt.Rows)
            //{
            //    var newItem = new GetSysEmergencyLinkageListAndStatisticsResponse()
            //    {
            //        DeletePerson = item["DeletePerson"].ToString(),
            //        DeleteTime = item["DeleteTimeConvert"].ToString(),
            //        EditPerson = item["EditPerson"].ToString(),
            //        EditTime = item["EditTime"].ToString(),
            //        Id = item["Id"].ToString(),
            //        MasterAreaAssId = item["MasterAreaAssId"].ToString(),
            //        MasterAreaNum = item["MasterAreaNum"].ToString(),
            //        MasterDevTypeAssId = item["MasterDevTypeAssId"].ToString(),
            //        MasterDevTypeNum = item["MasterDevTypeNum"].ToString(),
            //        MasterModelName = item["MasterModelName"] == null ? "" : item["MasterModelName"].ToString(),
            //        MasterPointAssId = item["MasterPointAssId"].ToString(),
            //        MasterPointNum = item["MasterPointNum"].ToString(),
            //        MasterTriDataStateAssId = item["MasterTriDataStateAssId"].ToString(),
            //        MasterTriDataStateNum = item["MasterTriDataStateNum"].ToString(),
            //        PassiveAreaAssId = item["PassiveAreaAssId"].ToString(),
            //        PassiveAreaNum = item["PassiveAreaNum"].ToString(),
            //        PassivePersonAssId = item["PassivePersonAssId"].ToString(),
            //        PassivePersonNum = item["PassivePersonNum"].ToString(),
            //        PassivePointAssId = item["PassivePointAssId"].ToString(),
            //        PassivePointNum = item["PassivePointNum"].ToString(),
            //        Name = item["Name"].ToString(),
            //        Type = (int)item["Type"],
            //        TypeName = (int)item["Type"] == 1 ? "普通联动" : "大数据分析联动",
            //        Duration = Convert.ToInt32(item["Duration"])
            //    };
            //    lis.Add(newItem);
            //}

            var ret = new BasicResponse<List<GetSysEmergencyLinkageListAndStatisticsResponse>>()
            {
                Data = lis
            };
            return ret;
        }

        public BasicResponse<List<Jc_DefInfo>> GetMasterPointInfoByAssId(LongIdRequest request)
        {
            var req3 = new EmergencyLinkageConfigCacheGetAllRequest();
            var res3 = _sysEmergencyLinkageCacheService.GetAllSysEmergencyLinkageCache(req3);
            var linkageInfo = res3.Data.FirstOrDefault(a => a.MasterPointAssId == request.Id.ToString());
            var masterPointAss = linkageInfo == null ? new List<EmergencyLinkageMasterPointAssInfo>() : linkageInfo.MasterPoint;

            var req = new PointDefineCacheGetAllRequest();
            var res = _pointDefineCacheService.GetAllPointDefineCache(req);
            if (res.Code != 100)
            {
                throw new Exception(res.Message);
            }
            var allPoint = res.Data;
            //var allMasterPointAss = _emergencyLinkageMasterPointAssRepository.Datas.ToList();
            var res2 = from a in masterPointAss
                       join b in allPoint on a.PointId equals b.PointID into temp
                       from ab in temp.DefaultIfEmpty()
                       //where a.MasterPointAssId == request.Id.ToString()
                       select ab;

            var retLis = res2.ToList();
            var ret = new BasicResponse<List<Jc_DefInfo>>()
            {
                Data = retLis
            };
            return ret;
        }

        public BasicResponse<List<AreaInfo>> GetMasterAreaInfoByAssId(LongIdRequest request)
        {
            var req3 = new EmergencyLinkageConfigCacheGetAllRequest();
            var res3 = _sysEmergencyLinkageCacheService.GetAllSysEmergencyLinkageCache(req3);
            var linkageInfo = res3.Data.FirstOrDefault(a => a.MasterAreaAssId == request.Id.ToString());
            var masterAreaAss = linkageInfo == null ? new List<EmergencyLinkageMasterAreaAssInfo>() : linkageInfo.MasterAreas;


            var req = new AreaCacheGetAllRequest();
            var res = _areaCacheService.GetAllAreaCache(req);
            if (res.Code != 100)
            {
                throw new Exception(res.Message);
            }
            var allArea = res.Data;
            //var allMasterAreaAss = _emergencyLinkageMasterAreaAssRepository.Datas.ToList();

            var res2 = from a in masterAreaAss
                join b in allArea on a.AreaId equals b.Areaid into temp
                from ab in temp.DefaultIfEmpty()
                //where a.MasterAreaAssId == request.Id.ToString()
                select ab;

            var retLis = res2.ToList();
            var ret = new BasicResponse<List<AreaInfo>>()
            {
                Data = retLis
            };
            return ret;
        }

        public BasicResponse<List<Jc_DevInfo>> GetMasterEquTypeInfoByAssId(LongIdRequest request)
        {
            var req3 = new EmergencyLinkageConfigCacheGetAllRequest();
            var res3 = _sysEmergencyLinkageCacheService.GetAllSysEmergencyLinkageCache(req3);
            var linkageInfo = res3.Data.FirstOrDefault(a => a.MasterDevTypeAssId == request.Id.ToString());
            var masterDevTypeAss = linkageInfo == null
                ? new List<EmergencyLinkageMasterDevTypeAssInfo>()
                : linkageInfo.MasterDevTypes;

            var req = new DeviceDefineCacheGetAllRequest();
            var res = _deviceDefineCacheService.GetAllPointDefineCache(req);
            if (res.Code != 100)
            {
                throw new Exception(res.Message);
            }
            var allEquType = res.Data;
            //var allMasterDevTypeAss = _emergencyLinkageMasterDevTypeAssRepository.Datas.ToList();

            var res2 = from a in masterDevTypeAss
                join b in allEquType on a.DevId equals b.Devid into temp
                from ab in temp.DefaultIfEmpty()
                //where a.MasterDevTypeAssId == request.Id.ToString()
                select ab;

            var retLis = res2.ToList();
            var ret = new BasicResponse<List<Jc_DevInfo>>()
            {
                Data = retLis
            };
            return ret;
        }

        public BasicResponse<List<EnumcodeInfo>> GetMasterTriDataStateByAssId(LongIdRequest request)
        {
            var req3 = new EmergencyLinkageConfigCacheGetAllRequest();
            var res3 = _sysEmergencyLinkageCacheService.GetAllSysEmergencyLinkageCache(req3);
            var linkageInfo = res3.Data.FirstOrDefault(a => a.MasterTriDataStateAssId == request.Id.ToString());
            var masterTriDataStateAss = linkageInfo == null ? new List<EmergencyLinkageMasterTriDataStateAssInfo>() : linkageInfo.MasterTriDataStates;

            var req = new EnumcodeGetByEnumTypeIDRequest()
            {
                EnumTypeId = 4.ToString()
            };
            var res = _enumcodeService.GetEnumcodeByEnumTypeID(req);
            if (res.Code != 100)
            {
                throw new Exception(res.Message);
            }

            var allDataState = res.Data;
            //var allMasterTriDataStateAss = _emergencyLinkageMasterTriDataStateAssRepository.Datas.ToList();

            var res2 = from a in masterTriDataStateAss
                join b in allDataState on a.DataStateId equals b.LngEnumValue.ToString() into temp
                from ab in temp.DefaultIfEmpty()
                //where a.MasterTriDataStateAssId == request.Id.ToString()
                select ab;

            var retLis = res2.ToList();
            var ret = new BasicResponse<List<EnumcodeInfo>>()
            {
                Data = retLis
            };
            return ret;
        }

        public BasicResponse<List<R_PersoninfInfo>> GetPassivePersonByAssId(LongIdRequest request)
        {
            var req3 = new EmergencyLinkageConfigCacheGetAllRequest();
            var res3 = _sysEmergencyLinkageCacheService.GetAllSysEmergencyLinkageCache(req3);
            var linkageInfo = res3.Data.FirstOrDefault(a => a.PassivePersonAssId == request.Id.ToString());
            var passivePersonAss = linkageInfo == null ? new List<EmergencyLinkagePassivePersonAssInfo>() : linkageInfo.PassivePersons;

            var req = new RPersoninfCacheGetAllRequest();
            var res = _rPersoninfCacheService.GetAllRPersoninfCache(req);
            if (res.Code != 100)
            {
                throw new Exception(res.Message);
            }

            var allPerson = res.Data;
            //var allPassivePersonAss = _emergencyLinkagePassivePersonAssRepository.Datas.ToList();

            var res2 = from a in passivePersonAss
                join b in allPerson on a.PersonId equals b.Id into temp
                from ab in temp.DefaultIfEmpty()
                //where a.PassivePersonAssId == request.Id.ToString()
                select ab;

            var retLis = res2.ToList();
            var ret = new BasicResponse<List<R_PersoninfInfo>>()
            {
                Data = retLis
            };
            return ret;
        }

        public BasicResponse<List<IdTextCheck>> GetPassivePointInfoByAssId(LongIdRequest request)
        {
            var req3 = new EmergencyLinkageConfigCacheGetAllRequest();
            var res3 = _sysEmergencyLinkageCacheService.GetAllSysEmergencyLinkageCache(req3);
            var linkageInfo = res3.Data.FirstOrDefault(a => a.PassivePointAssId == request.Id.ToString());
            var passivePointAss = linkageInfo == null ? new List<EmergencyLinkagePassivePointAssInfo>() : linkageInfo.PassivePoints;

            var res = GetAllPassivePointInfo();
            if (res.Code != 100)
            {
                throw new Exception(res.Message);
            }
            var allPassivePoint = res.Data;
            //var allPassivePointAss = _emergencyLinkagePassivePointAssRepository.Datas.ToList();

            var res2 = from a in passivePointAss
                join b in allPassivePoint on a.PointId equals b.Id into temp
                from ab in temp.DefaultIfEmpty()
                //where a.PassivePointAssId == request.Id.ToString()
                select ab;

            var retLis = res2.ToList();
            var ret = new BasicResponse<List<IdTextCheck>>()
            {
                Data = retLis
            };
            return ret;
        }

        public BasicResponse<List<AreaInfo>> GetPassiveAreaInfoByAssId(LongIdRequest request)
        {
            var req3 = new EmergencyLinkageConfigCacheGetAllRequest();
            var res3 = _sysEmergencyLinkageCacheService.GetAllSysEmergencyLinkageCache(req3);
            var linkageInfo = res3.Data.FirstOrDefault(a => a.PassiveAreaAssId == request.Id.ToString());
            var passiveAreaAss = linkageInfo == null ? new List<EmergencyLinkagePassiveAreaAssInfo>() : linkageInfo.PassiveAreas;

            var req = new AreaCacheGetAllRequest();
            var res = _areaCacheService.GetAllAreaCache(req);
            if (res.Code != 100)
            {
                throw new Exception(res.Message);
            }
            var allArea = res.Data;
            //var allPassiveAreaAss = _emergencyLinkagePassiveAreaAssRepository.Datas.ToList();

            var res2 = from a in passiveAreaAss
                join b in allArea on a.AreaId equals b.Areaid into temp
                from ab in temp.DefaultIfEmpty()
                //where a.PassiveAreaAssId == request.Id.ToString()
                select ab;

            var retLis = res2.ToList();
            var ret = new BasicResponse<List<AreaInfo>>()
            {
                Data = retLis
            };
            return ret;
        }

        public BasicResponse<List<IdTextCheck>> GetAllPassivePointInfo()
        {
            var retLis = new List<IdTextCheck>();

            var res = _personPointDefineService.GetAllPointDefineCache();
            if (res.Code != 100)
            {
                throw new Exception(res.Message);
            }
            var allPersonPoint = res.Data;
            var personPoint = allPersonPoint.Where(a => a.DevPropertyID != 0).ToList();
            foreach (var item in personPoint)
            {
                var newItem = new IdTextCheck
                {
                    Check = false,
                    Id = item.PointID,
                    Text = item.Wz + "（" + item.Point + "）",
                    AreaId = item.Areaid,
                    SysId = "11",
                    Point = item.Point
                };
                retLis.Add(newItem);
            }

            var req2 = new V_DefCacheGetAllRequest();
            var res2 = _vDefCacheService.GetAll(req2);
            if (res2.Code != 100)
            {
                throw new Exception(res2.Message);
            }
            var videoPoint = res2.Data;
            foreach (var item in videoPoint)
            {
                var newItem = new IdTextCheck
                {
                    Check = false,
                    Id = item.Id,
                    Text = item.Devname + "（" + item.IPAddress + "）",
                    AreaId = item.AreaId,
                    SysId = "74",
                    Point = item.IPAddress
                };
                retLis.Add(newItem);
            }

            //获取广播测点
            var req3 = new B_DefCacheGetAllRequest();
            var res3 = _bDefCacheService.GetAll(req3);
            if (res3.Code != 100)
            {
                throw new Exception(res3.Message);
            }
            var broadcastPoint = res3.Data;
            foreach (var item in broadcastPoint)
            {
                var newItem = new IdTextCheck
                {
                    Check = false,
                    Id = item.PointID,
                    Text = item.Wz + "（" + item.Point + "）",
                    AreaId = item.Areaid,
                    SysId = "12",
                    Point = item.Point
                };
                retLis.Add(newItem);
            }

            retLis = retLis.OrderBy(a => a.Text).ToList();
            var ret = new BasicResponse<List<IdTextCheck>>()
            {
                Data = retLis
            };
            return ret;
        }

        public BasicResponse SoftDeleteSysEmergencyLinkageById(LongIdRequest request)
        {
            var req = request.Id.ToString();
            var res = _Repository.GetSysEmergencyLinkageById(req);
            res.Activity = 0;
            _Repository.UpdateSysEmergencyLinkage(res);

            //更新缓存
            var res2 = new EmergencyLinkageConfigCacheDeleteRequest
            {
                SysEmergencyLinkageInfo = new SysEmergencyLinkageInfo()
                {
                    Id = request.Id.ToString()
                }
            };
            _sysEmergencyLinkageCacheService.DeleteSysEmergencyLinkageCache(res2);

            return new BasicResponse();
        }

        public BasicResponse UpdateRealTimeState(UpdateRealTimeStateRequest request)
        {
            //根据linkageId获取应急联动信息
            var req = new EmergencyLinkageConfigCacheGetByKeyRequest()
            {
                Id = request.LinkageId
            };
            var res = _sysEmergencyLinkageCacheService.GetSysEmergencyLinkageCacheByKey(req);
            if (res.Code != 100)
            {
                throw new Exception(res.Message);
            }
            var linkageInfo = res.Data;
            if (linkageInfo != null && linkageInfo.EmergencyLinkageState != Convert.ToInt32(request.State))
            {
                linkageInfo.EmergencyLinkageState = Convert.ToInt32(request.State);

                var req2 = new EmergencyLinkageConfigCacheUpdateRequest()
                {
                    SysEmergencyLinkageInfo = linkageInfo
                };
                var res2 = _sysEmergencyLinkageCacheService.UpdateSysEmergencyLinkageCache(req2);
                if (res2.Code != 100)
                {
                    throw new Exception(res2.Message);
                }
            }

            return new BasicResponse();
        }

        public BasicResponse<List<Jc_DefInfo>> GetAllMasterPointsById(SysEmergencyLinkageGetRequest request)
        {
            List<Jc_DefInfo> masterpoints=new List<Jc_DefInfo>();

            EmergencyLinkageConfigCacheGetByKeyRequest getrequest=new EmergencyLinkageConfigCacheGetByKeyRequest();
            getrequest.Id=request.Id;
            SysEmergencyLinkageInfo item = _sysEmergencyLinkageCacheService.GetSysEmergencyLinkageCacheByKey(getrequest).Data;

            PointDefineCacheGetByConditonRequest pointgetrequest = new PointDefineCacheGetByConditonRequest();
            if (item != null && !string.IsNullOrEmpty(item.MasterAreaAssId) && item.MasterAreaAssId!="0")
            {
                item.MasterAreas.ForEach(area =>
                {
                    pointgetrequest.Predicate = p => p.Areaid == area.AreaId;
                    var areapoints = _pointDefineCacheService.GetPointDefineCache(pointgetrequest).Data;
                    masterpoints.AddRange(areapoints);
                });
            }
            else if (item != null && !string.IsNullOrEmpty(item.MasterDevTypeAssId) && item.MasterDevTypeAssId != "0") 
            {
                item.MasterDevTypes.ForEach(devtype =>
                {
                    pointgetrequest.Predicate = p => p.Devid == devtype.DevId;
                    var areapoints = _pointDefineCacheService.GetPointDefineCache(pointgetrequest).Data;
                    masterpoints.AddRange(areapoints);
                });
            }
            else if (item != null && !string.IsNullOrEmpty(item.MasterPointAssId) && item.MasterPointAssId != "0") 
            {
                item.MasterPoint.ForEach(point =>
                {
                    pointgetrequest.Predicate = p => p.PointID == point.PointId;
                    var pointdefine = _pointDefineCacheService.GetPointDefineCache(pointgetrequest).Data.FirstOrDefault();
                    masterpoints.Add(pointdefine);
                });
            }

            var response = new BasicResponse<List<Jc_DefInfo>>();
            response.Data = masterpoints;

            return response;
        }
        
        public BasicResponse<List<SysEmergencyLinkageInfo>> GetAllSysEmergencyLinkageInfo()
        {
            var res = _Repository.GetAllSysEmergencyLinkageList();
            var convert = ObjectConverter.CopyList<SysEmergencyLinkageModel, SysEmergencyLinkageInfo>(res).ToList();
            var ret = new BasicResponse<List<SysEmergencyLinkageInfo>>()
            {
                Data = convert
            };
            return ret;
        }

        /// <summary>
        /// 更新应急联动配置
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BasicResponse UpdateSysEmergencyLinkage(SysEmergencyLinkageUpdateRequest request)
        {
            _sysEmergencyLinkageCacheService.UpdateSysEmergencyLinkageCache(new Sys.Safety.Request.EmergencyLinkageConfigCacheUpdateRequest { SysEmergencyLinkageInfo = request.SysEmergencyLinkageInfo });
            return new BasicResponse();
        }
    }
}