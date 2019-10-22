using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.PointDefine;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.ServiceContract.Cache;
using Sys.Safety.Request.Cache;
using Basic.Framework.Logging;
using Sys.Safety.Services.PointDefineComom;
using Basic.Framework.Data;
using Sys.Safety.Request.NetworkModule;
using Sys.DataCollection.Common.Protocols;
using Sys.Safety.DataContract.CommunicateExtend;
using Sys.Safety.Processing.Rpc;
using Sys.DataCollection.Common.Rpc;
using Sys.Safety.Processing.DataProcessing;
using Sys.DataCollection.Common.Protocols.Devices;
using Sys.Safety.Services.Cache;
using Sys.Safety.Enums;
using Sys.Safety.Request.Listex;
using Sys.Safety.ServiceContract.KJ237Cache;
using Sys.Safety.Request.PersonCache;
using Sys.Safety.Request.Area;
using Sys.Safety.Request.Config;
using Sys.Safety.Request.Position;
using Sys.Safety.Request.R_Restrictedperson;
using Sys.Safety.Request.UndefinedDef;

namespace Sys.Safety.Services
{
    /// <summary>
    /// 测点定义服务(人员定位系统)
    /// </summary>
    public partial class PersonPointDefineService : IPersonPointDefineService
    {
        private IConfigRepository _configRepository;

        private IConfigCacheService _configCacheService;

        private IPositionService _positionService;

        private IDeviceTypeCacheService _deviceTypeCacheService;

        private IDeviceClassCacheService _deviceClassCacheService;

        private IDevicePropertyCacheService _devicePropertyCacheService;

        private IAreaService _areaService;

        private IR_DefRepository _repository;
        private IR_RestrictedpersonService _restrictedpersonService;
        /// <summary>
        /// 测点定义缓存接口
        /// </summary>
        private IRPointDefineCacheService _rPointDefineCacheService;
        /// <summary>
        /// Jc_Mac表操作类
        /// </summary>
        private INetworkModuleService _networkModuleService;
        /// <summary>
        /// JC_MAC表缓存操作
        /// </summary>
        private INetworkModuleCacheService _networkModuleCacheService;
        /// <summary>
        /// 设备类型定义缓存
        /// </summary>
        private IDeviceDefineCacheService _deviceDefineCacheService;
        /// <summary>
        /// 人员定位未定义设备列表
        /// </summary>
        private IR_UndefinedDefService _R_UndefinedDefService;

        private static DateTime lastSyncTime;

        public PersonPointDefineService(IR_DefRepository _Repository, IR_RestrictedpersonService _restrictedpersonService,
            IRPointDefineCacheService _rPointDefineCacheService,
            INetworkModuleService _networkModuleService,
            INetworkModuleCacheService _networkModuleCacheService,
            IDeviceDefineCacheService deviceDefineCacheService,
            IR_UndefinedDefService _R_UndefinedDefService, IAreaService areaService, IDevicePropertyCacheService devicePropertyCacheService, IDeviceClassCacheService deviceClassCacheService, IDeviceTypeCacheService deviceTypeCacheService, IPositionService positionService, IConfigCacheService configCacheService, IConfigRepository configRepository)
        {
            this._repository = _Repository;
            this._restrictedpersonService = _restrictedpersonService;
            this._rPointDefineCacheService = _rPointDefineCacheService;
            this._networkModuleService = _networkModuleService;
            this._networkModuleCacheService = _networkModuleCacheService;
            this._deviceDefineCacheService = deviceDefineCacheService;
            this._R_UndefinedDefService = _R_UndefinedDefService;
            _areaService = areaService;
            _devicePropertyCacheService = devicePropertyCacheService;
            _deviceClassCacheService = deviceClassCacheService;
            _deviceTypeCacheService = deviceTypeCacheService;
            _positionService = positionService;
            _configCacheService = configCacheService;
            _configRepository = configRepository;
        }

        /// <summary>
        /// 添加测点
        /// </summary>
        /// <param name="jc_Defrequest"></param>
        /// <returns></returns>
        public BasicResponse AddPointDefine(PointDefineAddRequest PointDefineRequest)
        {
            BasicResponse Result = new BasicResponse();
            Jc_DefInfo item = PointDefineRequest.PointDefineInfo;
            AddExtension(item);//2017.6.9 by
            Jc_DefInfo olditem = null;

            Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest pointDefineCacheRequest = new Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest();
            pointDefineCacheRequest.Predicate = a => a.Point == item.Point && a.Activity == "1";
            var result = _rPointDefineCacheService.GetPointDefineCache(pointDefineCacheRequest);
            if (result.Data != null && result.Data.Count > 0)
            {
                if (result.Data[0].Activity == "1")
                {
                    olditem = result.Data[0];
                }
            }

            //增加重复判断
            if (result.Data != null && result.Data.Count > 0)
            { //缓存中存在此测点
                Result.Code = 1;
                Result.Message = "当前添加的测点已存在！";
                return Result;
            }


            //向网关同步数据
            List<Jc_DefInfo> SendItemList = new List<Jc_DefInfo>();
            SendItemList.Add(item);
            var resultSync = SynchronousDataToGateway(SendItemList);
            if (!resultSync)
            {
                Result.Code = 1;
                Result.Message = "向网关同步数据失败！";
                return Result;
            }

            //保存数据库
            var _jc_Def = ObjectConverter.Copy<Jc_DefInfo, R_DefModel>(item);
            var resultjc_Def = _repository.AddDef(_jc_Def);
            //保存识别器限制进入、禁止进入信息  20171123
            SaveRestrictedperson(item.PointID, item.RestrictedpersonInfoList);

            //置初始化标记及休眠处理
            PointInitializes(item, olditem);

            //添加到缓存
            RPointDefineCacheAddRequest AddPointDefineCacheRequest = new RPointDefineCacheAddRequest();
            AddPointDefineCacheRequest.PointDefineInfo = item;
            _rPointDefineCacheService.AddRPointDefineCache(AddPointDefineCacheRequest);

            //清除未定义设备缓存
            R_UndefinedDefInfo undefinedDefInfo = _R_UndefinedDefService.GetAllRUndefinedDefCache(new RUndefinedDefCacheGetAllRequest()).Data.Find(a => a.Point == item.Point);
            if (undefinedDefInfo != null)
            {
                R_UndefinedDefDeleteRequest undefinedDefDeleteRequest = new R_UndefinedDefDeleteRequest();
                undefinedDefDeleteRequest.Id = undefinedDefInfo.Id;
                _R_UndefinedDefService.DeleteUndefinedDef(undefinedDefDeleteRequest);
            }

            return Result;
        }

        private void ClearR_UndefinedDefCache()
        {

        }
        /// <summary>
        /// 保存识别器限制进入、禁止进入信息  20171123
        /// </summary>
        /// <param name="restrictedpersonInfoList"></param>
        private void SaveRestrictedperson(string PointId, List<R_RestrictedpersonInfo> restrictedpersonInfoList)
        {
            if (restrictedpersonInfoList == null)
            {
                return;
            }
            //先删除原来的限制进入、禁止进入信息
            R_RestrictedpersonDeleteByPointIdRequest restrictedpersonRequest = new R_RestrictedpersonDeleteByPointIdRequest();
            restrictedpersonRequest.PointId = PointId;
            _restrictedpersonService.DeleteRestrictedpersonByPointId(restrictedpersonRequest);
            //再进行添加操作
            foreach (R_RestrictedpersonInfo addRestrictedpersonInfo in restrictedpersonInfoList)
            {
                R_RestrictedpersonAddRequest add_restrictedpersonRequest = new R_RestrictedpersonAddRequest();
                add_restrictedpersonRequest.RestrictedpersonInfo = addRestrictedpersonInfo;
                _restrictedpersonService.AddRestrictedperson(add_restrictedpersonRequest);
            }
        }
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="jc_Defrequest"></param>
        /// <returns></returns>
        public BasicResponse AddPointDefines(PointDefinesAddRequest PointDefineRequest)
        {
            BasicResponse Result = new BasicResponse();
            List<Jc_DefInfo> items = PointDefineRequest.PointDefinesInfo;
            List<Jc_DefInfo> Jc_DefCaches = new List<Jc_DefInfo>();
            Jc_DefInfo olditem = null;

            Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest pointDefineCacheRequest = new Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest();
            pointDefineCacheRequest.Predicate = a => a.Activity == "1";
            var result = _rPointDefineCacheService.GetPointDefineCache(pointDefineCacheRequest);
            Jc_DefCaches = result.Data;

            foreach (Jc_DefInfo item in items)
            {
                olditem = Jc_DefCaches.Find(a => a.Point == item.Point && a.Activity == "1");
                //增加重复判断
                if (olditem != null)
                { //缓存中存在此测点
                    Result.Code = 1;
                    Result.Message = "当前添加列表中的数据在数据库中已经存在！";
                    return Result;
                }
            }
            //向网关同步数据
            List<Jc_DefInfo> SendItemList = items;
            var resultSync = SynchronousDataToGateway(SendItemList);
            if (!resultSync)
            {
                Result.Code = 1;
                Result.Message = "向网关同步数据失败！";
                return Result;
            }
            TransactionsManager.BeginTransaction(() =>
                {


                    foreach (Jc_DefInfo item in items)
                    {
                        AddExtension(item);//2017.6.9 by

                        //保存数据库
                        var _jc_Def = ObjectConverter.Copy<Jc_DefInfo, R_DefModel>(item);
                        var resultjc_Def = _repository.AddDef(_jc_Def);
                        //保存识别器限制进入、禁止进入信息  20171123
                        SaveRestrictedperson(item.PointID, item.RestrictedpersonInfoList);

                        //置下发初始化标记
                        PointInitializes(item, olditem);
                    }


                    //添加到缓存
                    RPointDefineCacheBatchAddRequest AddPointDefineCacheRequest = new RPointDefineCacheBatchAddRequest();
                    AddPointDefineCacheRequest.PointDefineInfos = items;
                    _rPointDefineCacheService.BacthAddRPointDefineCache(AddPointDefineCacheRequest);

                    //未定义识别器更新到缓存和数据库后，在未定义设备缓存和数据库中删除 20171204
                    foreach (Jc_DefInfo item in items)
                    {
                        R_UndefinedDefInfo undefinedDefInfo = _R_UndefinedDefService.GetAllRUndefinedDefCache(new RUndefinedDefCacheGetAllRequest()).Data.Find(a => a.Point == item.Point);
                        if (undefinedDefInfo != null)
                        {
                            R_UndefinedDefDeleteRequest undefinedDefDeleteRequest = new R_UndefinedDefDeleteRequest();
                            undefinedDefDeleteRequest.Id = undefinedDefInfo.Id;
                            _R_UndefinedDefService.DeleteUndefinedDef(undefinedDefDeleteRequest);
                        }
                    }

                });

            return Result;
        }

        /// <summary>
        /// 扩展属性实例化 //2017.6.9 by
        /// </summary>
        /// <param name="item"></param>
        private void AddExtension(Jc_DefInfo item)
        {
            //item.NErrCount = 0;
            //item.Fdstate = 0;
            //item.BDisCharge = 0;
            //item.NCtrlSate = 46;//馈电状态未知
            //item.DeviceControlItems = new List<ControlItem>();
            //item.SoleCodingChanels = new List<ControlItem>();
            //item.GasThreeUnlockContro = 0;
            //item.ModificationItems = new List<EditDeviceAddressItem>();
            //item.HistoryControlState = 0;
            //item.HistoryRealDataState = 0;
            //item.RealTypeInfo = "";

            //if (item.ClsAlarmObj == null)
            //{
            //    item.ClsAlarmObj = new AlarmProperty();
            //}
            //if (item.ClsCommObj == null)
            //{
            //    item.ClsCommObj = new CommProperty((uint)item.Fzh);
            //}
            //if (item.ClsCtrlObj == null)
            //{
            //    item.ClsCtrlObj = new List<ControlRemote>();
            //}
            //if (item.ClsFiveMinObj == null)
            //{
            //    item.ClsFiveMinObj = new FiveMinData();
            //}
        }
        private void PointInitializes(Jc_DefInfo item, Jc_DefInfo olditem)
        {
            //置是否需要下发初始化标记
            if (item.InfoState == InfoState.AddNew || item.Activity == "0")
            {
                item.DefIsInit = true;//新增加测点，都发初始化
            }
            if ((item.Bz4 & 0x02) == 0x02)//如果是休眠状态，则暂时置为上一次的状态，待保存巡检的时候再置成休眠(数据处理是直接根据Bz4进行处理的，不然保存了定义就会开始处理了)  20170705
            {
                if (olditem != null)
                {
                    item.Bz4 = olditem.Bz4;
                }
            }
        }
        /// <summary>
        /// 更新测点
        /// </summary>
        /// <param name="jc_Defrequest"></param>
        /// <returns></returns>
        public BasicResponse UpdatePointDefine(PointDefineUpdateRequest PointDefineRequest)
        {
            BasicResponse Result = new BasicResponse();

            Jc_DefInfo item = PointDefineRequest.PointDefineInfo;
            Jc_DefInfo olditem = null;

            Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest pointDefineCacheRequest = new Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest();
            pointDefineCacheRequest.Predicate = a => a.Point == item.Point && a.Activity == "1";
            var result = _rPointDefineCacheService.GetPointDefineCache(pointDefineCacheRequest);
            if (result.Data != null && result.Data.Count > 0)
            {
                if (result.Data[0].Activity == "1")
                {
                    olditem = result.Data[0];
                }
            }

            //增加重复判断            
            if (result.Data == null || result.Data.Count < 1)
            { //缓存中存在此测点
                Result.Code = 1;
                Result.Message = "当前更新的测点不存在！";

                return Result;
            }

            //向网关同步数据
            List<Jc_DefInfo> SendItemList = new List<Jc_DefInfo>();
            SendItemList.Add(item);
            var resultSync = SynchronousDataToGateway(SendItemList);
            if (!resultSync)
            {
                Result.Code = 1;
                Result.Message = "向网关同步数据失败！";
                return Result;
            }

            //保存数据库
            var _jc_Def = ObjectConverter.Copy<Jc_DefInfo, R_DefModel>(item);
            _repository.UpdateDef(_jc_Def);
            //保存识别器限制进入、禁止进入信息  20171123
            SaveRestrictedperson(item.PointID, item.RestrictedpersonInfoList);

            //置下发初始化标记
            PointInitializes(item, olditem);

            //更新缓存
            UpdatePointDefineCacheByProperty(item);

            return Result;
        }

        private void UpdatePointDefineCacheByProperty(Jc_DefInfo Point)
        {
            Dictionary<string, object> paramater = new Dictionary<string, object>();
            paramater.Add("Areaid", Point.Areaid);
            paramater.Add("Sysid", Point.Sysid);
            paramater.Add("Activity", Point.Activity);
            paramater.Add("CreateUpdateTime", Point.CreateUpdateTime);
            paramater.Add("DeleteTime", Point.DeleteTime);
            paramater.Add("Fzh", Point.Fzh);
            paramater.Add("Kh", Point.Kh);
            paramater.Add("Dzh", Point.Dzh);
            paramater.Add("Devid", Point.Devid);
            paramater.Add("Wzid", Point.Wzid);
            paramater.Add("Csid", Point.Csid);
            paramater.Add("Point", Point.Point);
            paramater.Add("Jckz1", Point.Jckz1);
            paramater.Add("Jckz2", Point.Jckz2);
            paramater.Add("Jckz3", Point.Jckz3);
            paramater.Add("Z1", Point.Z1);
            paramater.Add("Z2", Point.Z2);
            paramater.Add("Z3", Point.Z3);
            paramater.Add("Z4", Point.Z4);
            paramater.Add("Z5", Point.Z5);
            paramater.Add("Z6", Point.Z6);
            paramater.Add("Z7", Point.Z7);
            paramater.Add("Z8", Point.Z8);
            paramater.Add("K1", Point.K1);
            paramater.Add("K2", Point.K2);
            paramater.Add("K3", Point.K3);
            paramater.Add("K4", Point.K4);
            paramater.Add("K5", Point.K5);
            paramater.Add("K6", Point.K6);
            paramater.Add("K7", Point.K7);
            paramater.Add("K8", Point.K8);
            paramater.Add("Bz1", Point.Bz1);
            paramater.Add("Bz2", Point.Bz2);
            paramater.Add("Bz3", Point.Bz3);
            paramater.Add("Bz4", Point.Bz4);
            paramater.Add("Bz5", Point.Bz5);
            paramater.Add("Bz6", Point.Bz6);
            paramater.Add("Bz7", Point.Bz7);
            paramater.Add("Bz8", Point.Bz8);
            paramater.Add("Bz9", Point.Bz9);
            paramater.Add("Bz10", Point.Bz10);
            paramater.Add("Bz11", Point.Bz11);
            paramater.Add("Bz12", Point.Bz12);
            paramater.Add("Bz18", Point.Bz18);
            //扩展字段更新
            paramater.Add("Wz", Point.Wz);
            paramater.Add("DevName", Point.DevName);
            paramater.Add("DevPropertyID", Point.DevPropertyID);
            paramater.Add("DevClassID", Point.DevClassID);
            paramater.Add("DevModelID", Point.DevModelID);
            paramater.Add("DevProperty", Point.DevProperty);
            paramater.Add("DevClass", Point.DevClass);
            paramater.Add("DevModel", Point.DevModel);
            paramater.Add("Unit", Point.Unit);
            paramater.Add("AreaName", Point.AreaName);
            paramater.Add("AreaLoc", Point.AreaLoc);
            paramater.Add("XCoordinate", Point.XCoordinate);
            paramater.Add("YCoordinate", Point.YCoordinate);
            //识别器类型更新  20171127
            paramater.Add("RecognizerTypeDesc", EnumHelper.GetEnumDescription((Recognizer)Point.Bz1));

            // 20171123
            paramater.Add("RestrictedpersonInfoList", Point.RestrictedpersonInfoList);

            //定义改变标记
            paramater.Add("PointEditState", Point.PointEditState);
            paramater.Add("DefIsInit", Point.DefIsInit);
            paramater.Add("kzchangeflag", Point.kzchangeflag);
            paramater.Add("ReDoDeal", Point.ReDoDeal);
            paramater.Add("Dormancyflag", Point.Dormancyflag);

            //修改标记
            paramater.Add("InfoState", Point.InfoState);


            RDefineCacheUpdatePropertiesRequest defineCacheUpdatePropertiesRequest =
                new RDefineCacheUpdatePropertiesRequest();
            defineCacheUpdatePropertiesRequest.PointID = Point.PointID;
            defineCacheUpdatePropertiesRequest.UpdateItems = paramater;
            _rPointDefineCacheService.UpdateRPointDefineInfo(defineCacheUpdatePropertiesRequest);

        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="jc_Defrequest"></param>
        /// <returns></returns>
        public BasicResponse UpdatePointDefines(PointDefinesUpdateRequest PointDefineRequest)
        {

            BasicResponse Result = new BasicResponse();
            List<Jc_DefInfo> items = PointDefineRequest.PointDefinesInfo;
            List<Jc_DefInfo> Jc_DefCaches = new List<Jc_DefInfo>();     //人员定位测点缓存           
            Jc_DefInfo olditem = null;

            Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest pointDefineCacheRequest = new Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest();
            pointDefineCacheRequest.Predicate = a => a.Activity == "1";
            var result = _rPointDefineCacheService.GetPointDefineCache(pointDefineCacheRequest);
            Jc_DefCaches = result.Data;

            foreach (Jc_DefInfo item in items)
            {

                olditem = Jc_DefCaches.Find(a => a.Point == item.Point && a.Activity == "1");

                //增加重复判断
                if (olditem == null)
                { //缓存中存在此测点
                    Result.Code = 1;
                    Result.Message = "当前更新列表中的数据在数据库中不存在！";
                    return Result;
                }
            }
            //向网关同步数据
            List<Jc_DefInfo> SendItemList = items;
            var resultSync = SynchronousDataToGateway(SendItemList);
            if (!resultSync)
            {
                Result.Code = 1;
                Result.Message = "向网关同步数据失败！";
                return Result;
            }
            TransactionsManager.BeginTransaction(() =>
            {
                foreach (Jc_DefInfo item in items)
                {
                    olditem = Jc_DefCaches.Find(a => a.Point == item.Point && a.Activity == "1");
                    //保存数据库
                    var _jc_Def = ObjectConverter.Copy<Jc_DefInfo, R_DefModel>(item);
                    _repository.UpdateDef(_jc_Def);
                    //保存识别器限制进入、禁止进入信息  20171123
                    SaveRestrictedperson(item.PointID, item.RestrictedpersonInfoList);

                    //置下发初始化标记
                    PointInitializes(item, olditem);

                    //更新缓存
                    UpdatePointDefineCacheByProperty(item);
                }
            });

            return Result;
        }
        /// <summary>
        /// 批量更新测点定义缓存
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse UpdatePointDefinesCache(PointDefinesUpdateRequest PointDefineRequest)
        {
            BasicResponse Result = new BasicResponse();
            List<Jc_DefInfo> items = PointDefineRequest.PointDefinesInfo;
            //批量更新到缓存
            RPointDefineCacheBatchUpdateRequest UpdatePointDefineCacheRequest = new RPointDefineCacheBatchUpdateRequest();
            UpdatePointDefineCacheRequest.PointDefineInfos = items;
            _rPointDefineCacheService.BatchRUpdatePointDefineCache(UpdatePointDefineCacheRequest);
            return Result;
        }
        /// <summary>
        /// 批量更新属性
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BasicResponse BatchUpdatePointDefineInfo(DefineCacheBatchUpdatePropertiesRequest request)
        {
            RDefineCacheBatchUpdatePropertiesRequest UpdatePointDefineCacheRequest = new RDefineCacheBatchUpdatePropertiesRequest();
            UpdatePointDefineCacheRequest.PointItems = request.PointItems;
            return _rPointDefineCacheService.BatchUpdatePointDefineInfo(UpdatePointDefineCacheRequest);
        }
        public BasicResponse AddUpdatePointDefineAndNetworkModuleCache(PointDefineAddNetworkModuleAddUpdateRequest PointDefineAddNetworkModuleRequest)
        {
            BasicResponse Result = new BasicResponse();
            Jc_DefInfo item = PointDefineAddNetworkModuleRequest.PointDefineInfo;
            Jc_MacInfo mac = PointDefineAddNetworkModuleRequest.NetworkModuleInfo;
            Jc_MacInfo oldmac = PointDefineAddNetworkModuleRequest.NetworkModuleInfoOld;
            Jc_DefInfo olditem = null;
            List<Jc_DefInfo> updateSonPointList = PointDefineAddNetworkModuleRequest.UpdateSonPointList;

            Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest pointDefineCacheRequest = new Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest();
            pointDefineCacheRequest.Predicate = a => a.Point == item.Point && a.Activity == "1";
            var result = _rPointDefineCacheService.GetPointDefineCache(pointDefineCacheRequest);

            if (result.Data != null && result.Data.Count > 0)
            {
                if (result.Data[0].Activity == "1")
                {
                    olditem = result.Data[0];
                }
            }

            //增加重复判断
            if (item.InfoState == InfoState.AddNew)
            {
                if (olditem != null)
                { //缓存中存在此测点
                    Result.Code = 1;
                    Result.Message = "当前添加的测点已存在！";
                    return Result;
                }
            }
            else
            {
                if (result.Data == null || result.Data.Count < 1)
                { //缓存中存在此测点
                    Result.Code = 1;
                    Result.Message = "当前更新的测点不存在！";
                    return Result;
                }
            }
            //向网关同步数据
            List<Jc_DefInfo> SendItemList = new List<Jc_DefInfo>();
            SendItemList.Add(item);
            if (updateSonPointList.Count > 0)
            {
                SendItemList.AddRange(updateSonPointList);
            }
            var resultSync = SynchronousDataToGateway(SendItemList);
            if (!resultSync)
            {
                Result.Code = 1;
                Result.Message = "向网关同步数据失败！";
                return Result;
            }
            TransactionsManager.BeginTransaction(() =>
            {


                //保存数据库
                if (item.InfoState == InfoState.AddNew)
                {
                    var _jc_Def = ObjectConverter.Copy<Jc_DefInfo, R_DefModel>(item);
                    var resultjc_Def = _repository.AddDef(_jc_Def);
                }
                else
                {
                    var _jc_Def = ObjectConverter.Copy<Jc_DefInfo, R_DefModel>(item);
                    _repository.UpdateDef(_jc_Def);
                }
                if (updateSonPointList.Count > 0)
                {
                    foreach (Jc_DefInfo upItem in updateSonPointList)
                    {
                        var _jc_Def = ObjectConverter.Copy<Jc_DefInfo, R_DefModel>(upItem);
                        _repository.UpdateDef(_jc_Def);
                    }
                }

                //保存新的Mac信息   
                if (mac != null)
                {
                    if (mac.InfoState == InfoState.AddNew)
                    {
                        NetworkModuleAddRequest jc_Macrequest = new NetworkModuleAddRequest();
                        jc_Macrequest.NetworkModuleInfo = mac;
                        var resultjc_Mac = _networkModuleService.AddNetworkModule(jc_Macrequest);
                    }
                    else
                    {
                        NetworkModuleUpdateRequest jc_Macrequest = new NetworkModuleUpdateRequest();
                        jc_Macrequest.NetworkModuleInfo = mac;
                        _networkModuleService.UpdateNetworkModule(jc_Macrequest);
                    }
                }

                //更新以前的Mac绑定信息               
                if (oldmac != null)
                {
                    if (oldmac.InfoState == InfoState.AddNew)
                    {
                        NetworkModuleAddRequest jc_Macrequest = new NetworkModuleAddRequest();
                        jc_Macrequest.NetworkModuleInfo = oldmac;
                        var resultjc_Mac = _networkModuleService.AddNetworkModule(jc_Macrequest);
                    }
                    else
                    {
                        NetworkModuleUpdateRequest jc_Macrequest = new NetworkModuleUpdateRequest();
                        jc_Macrequest.NetworkModuleInfo = oldmac;
                        _networkModuleService.UpdateNetworkModule(jc_Macrequest);
                    }
                }


                //置下发初始化标记
                if (item.InfoState == InfoState.AddNew)
                {
                    item.DefIsInit = true;
                }

                if (item.InfoState == InfoState.AddNew)
                {
                    //添加到缓存
                    RPointDefineCacheAddRequest AddPointDefineCacheRequest = new RPointDefineCacheAddRequest();
                    AddPointDefineCacheRequest.PointDefineInfo = item;
                    _rPointDefineCacheService.AddRPointDefineCache(AddPointDefineCacheRequest);
                }
                else
                {
                    //更新缓存
                    RPointDefineCacheUpdateRequest UpdatePointDefineCacheRequest = new RPointDefineCacheUpdateRequest();
                    UpdatePointDefineCacheRequest.PointDefineInfo = item;
                    _rPointDefineCacheService.UpdateRPointDefineCahce(UpdatePointDefineCacheRequest);
                }
                //更新子设备的休眠、检修状态
                if (updateSonPointList.Count > 0)
                {
                    foreach (Jc_DefInfo upItem in updateSonPointList)
                    {
                        UpdatePointDefineCacheByBz4(upItem);
                    }
                }

            });

            return Result;
        }
        private void UpdatePointDefineCacheByBz4(Jc_DefInfo Point)
        {
            Dictionary<string, object> paramater = new Dictionary<string, object>();
            //休眠、检修状态标记
            paramater.Add("Bz4", Point.Bz4);
            //定义改变标记           
            paramater.Add("DefIsInit", Point.DefIsInit);


            //修改标记
            paramater.Add("InfoState", Point.InfoState);

            RDefineCacheUpdatePropertiesRequest defineCacheUpdatePropertiesRequest = new RDefineCacheUpdatePropertiesRequest();
            defineCacheUpdatePropertiesRequest.PointID = Point.PointID;
            defineCacheUpdatePropertiesRequest.UpdateItems = paramater;
            _rPointDefineCacheService.UpdateRPointDefineInfo(defineCacheUpdatePropertiesRequest);
        }
        public BasicResponse DeletePointDefine(PointDefineDeleteRequest PointDefineRequest)
        {
            _repository.DeleteDef(PointDefineRequest.Id);
            var jc_Defresponse = new BasicResponse();
            return jc_Defresponse;
        }
        /// <summary>
        /// 向网关同步数据
        /// </summary>
        /// <param name="SendItemList"></param>
        /// <returns></returns>
        private bool SynchronousDataToGateway(List<Jc_DefInfo> SendItemList)
        {

            UpdateCacheDataRequest UpdateCache = new UpdateCacheDataRequest();
            List<DeviceInfo> UpdateCacheDataList = new List<DeviceInfo>();

            UpdateCacheDataList = ObjectConverter.CopyList<Jc_DefInfo, DeviceInfo>(SendItemList).ToList();
            foreach (DeviceInfo deviceInfo in UpdateCacheDataList)
            {
                deviceInfo.UniqueKey = deviceInfo.Point;
                if (deviceInfo.InfoState == InfoState.Modified && deviceInfo.Activity == "0")//向下发送时，将非活动点的数据状态置为删除状态
                {
                    deviceInfo.InfoState = InfoState.Delete;
                }
            }
            UpdateCache.DeviceList = UpdateCacheDataList;
            //调用RPC发送
            MasProtocol masProtocol = new MasProtocol(SystemType.Security, DirectionType.Down, ProtocolType.UpdateCacheDataRequest);
            masProtocol.Protocol = UpdateCache;
            var result = RpcService.Send<GatewayRpcResponse>(masProtocol, RequestType.BusinessRequest);
            if (result == null)
            {
                return false;
            }
            return result.IsSuccess;
            //return true;
        }

        public BasicResponse PointDefineSaveData()
        {
            BasicResponse Result = new BasicResponse();

            //修改，根据保存时，置的初始化标记来置初始化标记
            RPointDefineCacheGetAllRequest pointDefineCacheRequest = new RPointDefineCacheGetAllRequest();
            var result = _rPointDefineCacheService.GetAllRPointDefineCache(pointDefineCacheRequest);
            List<Jc_DefInfo> newItems = result.Data.Where(a => a.DefIsInit).ToList();
            //保存巡检时，如果设备置了休眠的标记，则将缓存的Bz4更新  20170705
            newItems.ForEach(a =>
            {
                if (a.Dormancyflag)
                {
                    a.Bz4 |= 0x02;
                    Dictionary<string, object> paramater = new Dictionary<string, object>();
                    paramater.Add("Bz4", a.Bz4);
                    RDefineCacheUpdatePropertiesRequest defineCacheUpdatePropertiesRequest = new RDefineCacheUpdatePropertiesRequest();
                    defineCacheUpdatePropertiesRequest.PointID = a.PointID;
                    defineCacheUpdatePropertiesRequest.UpdateItems = paramater;
                    _rPointDefineCacheService.UpdateRPointDefineInfo(defineCacheUpdatePropertiesRequest);
                }
            });
            IEnumerable<IGrouping<short, Jc_DefInfo>> groupFz = newItems.GroupBy(p => p.Fzh);
            //驱动预处理
            foreach (IGrouping<short, Jc_DefInfo> info in groupFz)
            {
                //调用驱动处理
                Drv_LoadDef(info.ToList<Jc_DefInfo>());
            }
            //批量更新到缓存
            result = _rPointDefineCacheService.GetAllRPointDefineCache(pointDefineCacheRequest);
            PointDefineCacheBatchUpdateRequest UpdatePointDefineCacheRequest = new PointDefineCacheBatchUpdateRequest();
            //保存巡检后将所有定义初始化判断标记置成false
            Dictionary<string, Dictionary<string, object>> updateItemsList = new Dictionary<string, Dictionary<string, object>>();
            Dictionary<string, object> updateItems = new Dictionary<string, object>();
            List<Jc_DefInfo> items = result.Data;
            foreach (Jc_DefInfo item in items)
            {
                //if (item.DefIsInit)//交叉控制口变化要置kzchangeflag，但不会下发初始化，加上此条件会导致kzchangeflag一直为true
                //{
                item.DefIsInit = false;
                item.kzchangeflag = false;
                item.Dormancyflag = false;
                //}
                updateItems = new Dictionary<string, object>();
                updateItems.Add("DefIsInit", false);
                updateItems.Add("kzchangeflag", false);
                updateItems.Add("Dormancyflag", false);

                updateItemsList.Add(item.PointID, updateItems);
            }
            RDefineCacheBatchUpdatePropertiesRequest cacheUpdatePropertiesRequest = new RDefineCacheBatchUpdatePropertiesRequest();
            cacheUpdatePropertiesRequest.PointItems = updateItemsList;
            _rPointDefineCacheService.BatchUpdatePointDefineInfo(cacheUpdatePropertiesRequest);
            //UpdatePointDefineCacheRequest.PointDefineInfos = items;
            // _pointDefineCacheService.BatchUpdatePointDefineCache(UpdatePointDefineCacheRequest);
            //删除缓存中的非活动点
            List<Jc_DefInfo> NonActivityList = result.Data.FindAll(a => a.Activity == "0" && a.InfoState == InfoState.Modified);
            RPointDefineCacheBatchDeleteRequest DeletePointDefineCacheRequest = new RPointDefineCacheBatchDeleteRequest();
            DeletePointDefineCacheRequest.PointDefineInfos = NonActivityList;
            _rPointDefineCacheService.BatchDeleteRPointDefineCache(DeletePointDefineCacheRequest);


            return Result;
        }

        private void Drv_LoadDef(List<Jc_DefInfo> lstPoints)
        {
            try
            {
                string logInfo = "";
                //2016.9.19 by 便于查看内存数据流动过程
                foreach (Jc_DefInfo def in lstPoints)
                {
                    logInfo += def.Point + "|";
                }
                LogHelper.Info("收到设备定义信息数据:" + logInfo);

                //Dictionary<string, object> defDic = new Dictionary<string, object>();


                //GroupPoint(lstPoints).Select(item => CommInterface.Drv_LoadMonitorItem(item.Key,item.Value));
                //2016.8.23 by
                //Dictionary<dynamic, Dictionary<string, object>> groupPoint = GroupPoint(lstPoints);
                //foreach (dynamic DLLObj in groupPoint.Keys)
                //{
                //    CommInterface.Drv_LoadMonitorItem(DLLObj, groupPoint[DLLObj]);
                //}

                //Dictionary<long, Dictionary<string, Jc_DefInfo>> gp = GroupPoint(lstPoints);
                int sysid = 0;
                List<Jc_DefInfo> defItems;
                IEnumerable<IGrouping<short, Jc_DefInfo>> defLists = lstPoints.GroupBy(a => a.Fzh);
                foreach (IGrouping<short, Jc_DefInfo> info in defLists)
                {
                    defItems = info.ToList();
                    if (defItems.Count > 0)
                    {
                        sysid = defItems[0].Sysid;
                        if (GlobleStaticVariable.driverHandle.DriverItems.ContainsKey(sysid))
                        {
                            DriverTransferInterface.Drv_Pretreatment(GlobleStaticVariable.driverHandle.DriverItems[sysid].DLLObj, defItems);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

        }

        /// <summary>
        /// 获取所有定义缓存数据
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetAllPointDefineCache()
        {
            BasicResponse<List<Jc_DefInfo>> Result = new BasicResponse<List<Jc_DefInfo>>();
            RPointDefineCacheGetAllRequest rpointDefineCacheRequest = new RPointDefineCacheGetAllRequest();
            var resultpersondef = _rPointDefineCacheService.GetAllRPointDefineCache(rpointDefineCacheRequest);
            Result.Data = resultpersondef.Data.FindAll(a => a.Activity == "1");
            return Result;
        }
        /// <summary>
        ///通过设备性质查找测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByDevpropertID(PointDefineGetByDevpropertIDRequest PointDefineRequest)
        {
            BasicResponse<List<Jc_DefInfo>> Result = new BasicResponse<List<Jc_DefInfo>>();
            Result.Data = new List<Jc_DefInfo>();
            //在人员定位系统中查找

            Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest rpointDefineCacheRequest = new Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest();
            rpointDefineCacheRequest.Predicate = a => a.DevPropertyID == PointDefineRequest.DevpropertID && a.Activity == "1";
            var rdefresult = _rPointDefineCacheService.GetPointDefineCache(rpointDefineCacheRequest);
            if (rdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(rdefresult.Data);
            }

            return Result;
        }

        /// <summary>
        /// 根据区域ID获取人员定位设备
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByAreaID(PointDefineGetByAreaIDRequest PointDefineRequest)
        {
            BasicResponse<List<Jc_DefInfo>> response = new BasicResponse<List<Jc_DefInfo>>();

            RPointDefineCacheGetByConditonRequest rdergetrequest = new RPointDefineCacheGetByConditonRequest();
            rdergetrequest.Predicate = r => r.Areaid == PointDefineRequest.AreaId;
            response.Data = _rPointDefineCacheService.GetPointDefineCache(rdergetrequest).Data;

            return response;
        }


        public BasicResponse<Jc_DefInfo> GetPointDefineCacheByPointID(PointDefineGetByPointIDRequest PointDefineRequest)
        {
            BasicResponse<Jc_DefInfo> response = new BasicResponse<Jc_DefInfo>();

            RPointDefineCacheGetByConditonRequest rdergetrequest = new RPointDefineCacheGetByConditonRequest();
            rdergetrequest.Predicate = r => r.PointID == PointDefineRequest.PointID;
            response.Data = _rPointDefineCacheService.GetPointDefineCache(rdergetrequest).Data.FirstOrDefault();

            return response;
        }

        public BasicResponse<Jc_DefInfo> GetPointDefineCacheByPoint(PointDefineGetByPointRequest PointDefineRequest)
        {
            BasicResponse<Jc_DefInfo> response = new BasicResponse<Jc_DefInfo>();

            RPointDefineCacheGetByConditonRequest rdergetrequest = new RPointDefineCacheGetByConditonRequest();
            rdergetrequest.Predicate = r => r.Point == PointDefineRequest.Point;
            response.Data = _rPointDefineCacheService.GetPointDefineCache(rdergetrequest).Data.FirstOrDefault();

            return response;
        }

        public BasicResponse OldPlsPointSync(OldPlsPointSyncRequest request)
        {
            try
            {
                lastSyncTime = DateTime.Now;

                var pointInfo = request.PointInfo;
                var point08 = pointInfo.Select("type=0 or type=8");
                var point9 = pointInfo.Select("type=9");

                //区域同步
                var res = _areaService.GetAllAreaCache(new AreaCacheGetAllRequest());
                var allAreaInfo = res.Data;
                bool areaChange = false;
                foreach (DataRow item in point9)
                {
                    var areaName = item["wz"].ToString();
                    var exist = allAreaInfo.Any(a => a.Areaname == areaName);
                    if (!exist)
                    {
                        var newArea = new AreaAddRequest
                        {
                            AreaInfo = new AreaInfo()
                            {
                                Areaid = IdHelper.CreateLongId().ToString(),
                                Areaname = areaName,
                                Activity = "1",
                                CreateUpdateTime = DateTime.Now
                            }
                        };
                        _areaService.AddArea(newArea);
                        areaChange = true;
                    }
                }

                if (areaChange)
                {
                    res = _areaService.GetAllAreaCache(new AreaCacheGetAllRequest());
                    allAreaInfo = res.Data;
                }

                var req4 = new RPointDefineCacheGetAllRequest();
                var res4 = _rPointDefineCacheService.GetAllRPointDefineCache(req4);
                var allJcdef = res4.Data.ToList();
                bool ifDefineChange = false;

                foreach (DataRow item in point08)
                {
                    var currentPoint = item["point"].ToString();

                    string areaName = null;
                    foreach (DataRow item2 in point9)
                    {
                        var exist = item2["jckz1"].ToString().Contains(currentPoint);
                        if (exist)
                        {
                            areaName = item2["wz"].ToString();
                            break;
                        }
                    }

                    string areaId = null;
                    if (areaName != null)
                    {
                        var matchArea = allAreaInfo.FirstOrDefault(a => a.Areaname == areaName);
                        if (matchArea != null)
                        {
                            areaId = matchArea.Areaid;
                        }
                    }

                    var ptdefType = item["type"].ToString();
                    string devName = ptdefType == "0" ? "人员定位分站" : "识别器";
                    var req2 = new DeviceDefineCacheGetByConditonRequest()
                    {
                        Predicate = a => a.Name == devName
                    };
                    var res2 = _deviceDefineCacheService.GetPointDefineCache(req2);
                    var devInfo = res2.Data[0];

                    //查询设备性质、种类、型号
                    var req6 = new DevicePropertyCacheGetByKeyRequest
                    {
                        LngEnumValue = devInfo.Type
                    };
                    var res6 = _devicePropertyCacheService.GetDevicePropertyByKey(req6);
                    var equProperty = res6.Data == null ? "" : res6.Data.StrEnumDisplay;

                    var req7 = new DeviceClassCacheGetByKeyRequest
                    {
                        LngEnumValue = devInfo.Bz3
                    };
                    var res7 = _deviceClassCacheService.GetDeviceClassByKey(req7);
                    var equKind = res7.Data == null ? "" : res7.Data.StrEnumDisplay;

                    var req8 = new DeviceTypeCacheGetByKeyRequest
                    {
                        LngEnumValue = devInfo.Bz4
                    };
                    var res8 = _deviceTypeCacheService.GetDeviceTypeByKey(req8);
                    var equModel = res8.Data == null ? "" : res8.Data.StrEnumDisplay;

                    var wz = item["wz"].ToString();
                    var req3 = new PositionGetByWzRequest
                    {
                        Wz = wz
                    };
                    var res3 = _positionService.GetPositionCacheByWz(req3);
                    string wzid;
                    if (res3.Data.Count == 0)
                    {
                        //Edit by  -人员定位同步位置信息应该更新缓存
                        Jc_WzInfo wzinfo = new Jc_WzInfo();

                        wzinfo.ID = IdHelper.CreateLongId().ToString();
                        var maxid = _positionService.GetMaxPositionId().Data;
                        wzinfo.WzID = (maxid + 1).ToString();
                        wzid = (maxid + 1).ToString();
                        wzinfo.Wz = wz;
                        wzinfo.CreateTime = DateTime.Now;

                        _positionService.AddPosition(new Request.Position.PositionAddRequest { PositionInfo = wzinfo });
                    }
                    else
                    {
                        wzid = res3.Data[0].WzID;
                    }

                    var pointId = IdHelper.CreateLongId().ToString();
                    Int16 state = Convert.ToInt16(item["state"]);
                    if (ptdefType == "8" && item["state"].ToString() == "1")
                    {
                        state = 21;
                    }
                    if (ptdefType == "8" && item["state"].ToString() == "0")
                    {
                        state = 20;
                    }

                    string pointSsz = "";
                    if (ptdefType == "8")
                    {
                        pointSsz = item["k1"] + "人";
                    }
                    else
                    {
                        pointSsz = EnumHelper.GetEnumDescription((DeviceDataState)state);
                    }

                    var newJcdef = new Jc_DefInfo();
                    newJcdef.ID = pointId;
                    newJcdef.PointID = pointId;
                    newJcdef.Areaid = areaId;
                    newJcdef.Sysid = 11;
                    newJcdef.Activity = "1";
                    newJcdef.CreateUpdateTime = DateTime.Now;
                    newJcdef.DeleteTime = new DateTime(1900, 1, 1);
                    //Fzh = Convert.ToInt16(item["fzh"]);
                    newJcdef.Kh = Convert.ToInt16(item["kh"]);
                    newJcdef.Dzh = 0;
                    newJcdef.Devid = devInfo.Devid;
                    newJcdef.Wzid = wzid;
                    newJcdef.Csid = 0;
                    newJcdef.Point = currentPoint;
                    newJcdef.Jckz1 = item["jckz1"].ToString();
                    newJcdef.Jckz2 = item["jckz2"].ToString();
                    newJcdef.K1 = Convert.ToInt32(item["k1"]);
                    newJcdef.K2 = Convert.ToInt32(item["k2"]);
                    newJcdef.K3 = Convert.ToInt32(item["k3"]);
                    newJcdef.K4 = Convert.ToInt32(item["k4"]);
                    newJcdef.K5 = Convert.ToInt32(item["k5"]);
                    newJcdef.K6 = Convert.ToInt32(item["k6"]);
                    newJcdef.K7 = Convert.ToInt32(item["k7"]);
                    newJcdef.K8 = Convert.ToInt32(item["k8"]);
                    //实时信息
                    //Ssz = EnumHelper.GetEnumDescription((DeviceDataState)state),                        
                    newJcdef.Ssz = pointSsz;
                    newJcdef.DttStateTime = DateTime.Now;
                    newJcdef.State = state;
                    newJcdef.DataState = state;
                    newJcdef.Alarm = Convert.ToInt16(item["alarm"]);
                    // Zts = Convert.ToDateTime(item["zts"]),
                    //扩展信息
                    newJcdef.Wz = wz;
                    newJcdef.AreaName = areaName;
                    newJcdef.DevName = devInfo.Name;
                    newJcdef.DevPropertyID = devInfo.Type;
                    newJcdef.DevClassID = devInfo.Bz3;
                    newJcdef.DevModelID = devInfo.Bz4;
                    newJcdef.DevProperty = equProperty;
                    newJcdef.DevClass = equKind;
                    newJcdef.DevModel = equModel;
                    newJcdef.Upflag = "1";

                    var fzh = Convert.ToInt16(item["fzh"]) + 512;
                    newJcdef.Fzh = (short)fzh;

                    if (!item.IsNull("zts"))
                        newJcdef.Zts = Convert.ToDateTime(item["zts"]);

                    var pointInfo2 = allJcdef.FirstOrDefault(a => a.Point == currentPoint && a.Activity == "1");
                    if (pointInfo2 == null)
                    {
                        var model = ObjectConverter.Copy<Jc_DefInfo, R_DefModel>(newJcdef);
                        _repository.AddDef(model);

                        var req5 = new RPointDefineCacheAddRequest
                        {
                            PointDefineInfo = newJcdef
                        };
                        _rPointDefineCacheService.AddRPointDefineCache(req5);
                        ifDefineChange = true;
                    }
                    else
                    {
                        newJcdef.ID = pointInfo2.ID;
                        newJcdef.PointID = pointInfo2.PointID;

                        bool ifChange = false;
                        //if (pointInfo2.AreaName != newJcdef.AreaName || pointInfo2.Fzh != newJcdef.Fzh ||
                        //    pointInfo2.Kh != newJcdef.Kh || pointInfo2.Dzh != newJcdef.Dzh ||
                        //    pointInfo2.DevName != newJcdef.DevName || pointInfo2.Wz != newJcdef.Wz || pointInfo2.Jckz1 != newJcdef.Jckz1 ||
                        //    pointInfo2.Jckz2 != newJcdef.Jckz2 || pointInfo2.K1 != newJcdef.K1 ||
                        //    pointInfo2.K2 != newJcdef.K2 || pointInfo2.K3 != newJcdef.K3 || pointInfo2.K4 != newJcdef.K4 ||
                        //    pointInfo2.K5 != newJcdef.K5 || pointInfo2.K6 != newJcdef.K6 || pointInfo2.K7 != newJcdef.K7 ||
                        //    pointInfo2.K8 != newJcdef.K8)
                        if (pointInfo2.AreaName != newJcdef.AreaName || pointInfo2.Fzh != newJcdef.Fzh ||
                            pointInfo2.Kh != newJcdef.Kh || pointInfo2.Dzh != newJcdef.Dzh ||
                            pointInfo2.DevName != newJcdef.DevName || pointInfo2.Wz != newJcdef.Wz || pointInfo2.Jckz1 != newJcdef.Jckz1 ||
                            pointInfo2.Jckz2 != newJcdef.Jckz2)
                        {
                            var model = ObjectConverter.Copy<Jc_DefInfo, R_DefModel>(newJcdef);
                            _repository.UpdateDef(model);

                            ifChange = true;
                            ifDefineChange = true;
                        }

                        if (pointInfo2.Ssz != newJcdef.Ssz || pointInfo2.DttStateTime != newJcdef.DttStateTime ||
                            pointInfo2.State != newJcdef.State || pointInfo2.Alarm != newJcdef.Alarm ||
                            pointInfo2.Zts != newJcdef.Zts)
                        {
                            ifChange = true;
                        }

                        if (ifChange)
                        {
                            var req9 = new RPointDefineCacheUpdateRequest
                            {
                                PointDefineInfo = newJcdef
                            };
                            _rPointDefineCacheService.UpdateRPointDefineCahce(req9);
                        }
                    }
                }

                //清理已删除测点
                var req = new RPointDefineCacheGetAllRequest();
                var allRpoint = _rPointDefineCacheService.GetAllRPointDefineCache(req).Data.ToList();
                for (int i = allRpoint.Count - 1; i >= 0; i--)
                {
                    var ifExist = point08.Any(a => a["point"].ToString() == allRpoint[i].Point);
                    if (!ifExist)
                    {
                        _repository.DeleteDef(allRpoint[i].PointID);
                        var req2 = new RPointDefineCacheDeleteRequest
                        {
                            PointDefineInfo = allRpoint[i]
                        };
                        _rPointDefineCacheService.DeleteRPointDefineCache(req2);
                        ifDefineChange = true;
                    }
                }

                if (ifDefineChange)
                {
                    var saveTime = DateTime.Now;
                    //保存定义更新时间
                    if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey("_DefUpdateTime"))
                    {
                        Basic.Framework.Data.PlatRuntime.Items["_DefUpdateTime"] = saveTime;
                    }
                    else
                    {
                        Basic.Framework.Data.PlatRuntime.Items.Add("_DefUpdateTime", saveTime);
                    }

                    //保存数据库
                    ConfigInfo tempConfig = new ConfigInfo();
                    tempConfig.Name = "defdatetime";
                    tempConfig.Text = saveTime.ToString("yyyy-MM-dd HH:mm:ss");
                    tempConfig.Upflag = "0";
                    ConfigCacheGetByKeyRequest configCacheRequest = new ConfigCacheGetByKeyRequest();
                    configCacheRequest.Name = "defdatetime";
                    var result = _configCacheService.GetConfigCacheByKey(configCacheRequest);
                    ConfigInfo tempConfigCache = result.Data;
                    if (tempConfig != null)
                    {
                        tempConfig.ID = tempConfigCache.ID;
                        tempConfig.InfoState = InfoState.Modified;
                        //更新数据库
                        var req2 = ObjectConverter.Copy<ConfigInfo, ConfigModel>(tempConfig);
                        _configRepository.UpdateConfig(req2);
                        //更新缓存
                        ConfigCacheUpdateRequest UpdateConfigCacheRequest = new ConfigCacheUpdateRequest();
                        UpdateConfigCacheRequest.ConfigInfo = tempConfig;
                        _configCacheService.UpdateConfigCahce(UpdateConfigCacheRequest);
                    }
                }

                return new BasicResponse();
            }
            catch (Exception e)
            {
                LogHelper.Error(e.ToString());
                return new BasicResponse()
                {
                    Code = 101,
                    Message = "同步失败"
                };
            }
        }

        public BasicResponse PlsPointSync(PlsPointSyncRequest request)
        {
            try
            {
                var pointInfoDt = new DataTable();
                pointInfoDt.Columns.Add("point");
                pointInfoDt.Columns.Add("fzh");
                pointInfoDt.Columns.Add("kh");
                pointInfoDt.Columns.Add("type");
                pointInfoDt.Columns.Add("wz");
                pointInfoDt.Columns.Add("state");
                pointInfoDt.Columns.Add("alarm");
                pointInfoDt.Columns.Add("zts");
                pointInfoDt.Columns.Add("jckz1");
                pointInfoDt.Columns.Add("jckz2");
                pointInfoDt.Columns.Add("k1");
                pointInfoDt.Columns.Add("k2");
                pointInfoDt.Columns.Add("k3");
                pointInfoDt.Columns.Add("k4");
                pointInfoDt.Columns.Add("k5");
                pointInfoDt.Columns.Add("k6");
                pointInfoDt.Columns.Add("k7");
                pointInfoDt.Columns.Add("k8");

                if (request == null || request.PointInfo == null)
                {
                    LogHelper.Error("人员定位子系统同步失败,没有找到测点对象！！");
                    return new BasicResponse()
                    {
                        Code = 101,
                        Message = "人员定位子系统同步失败,没有找到测点对象！！"
                    };
                }

                foreach (var item in request.PointInfo)
                {
                    //var info = item as Newtonsoft.Json.Linq.JObject;
                    //var pointPro = info.Property("point");
                    //var point = pointPro == null ? "" : pointPro.Value.ToString();
                    //var fzhPro = info.Property("fzh");
                    //var fzh = fzhPro == null ? "" : fzhPro.Value.ToString();
                    //var khPro = info.Property("kh");
                    //var kh = khPro == null ? "" : khPro.Value.ToString();
                    //var typePro = info.Property("type");
                    //var type = typePro == null ? "" : typePro.Value.ToString();
                    //var wzPro = info.Property("wz");
                    //var wz = wzPro == null ? "" : wzPro.Value.ToString();
                    //var statePro = info.Property("state");
                    //var state = statePro == null ? "" : statePro.Value.ToString();
                    //var alarmPro = info.Property("alarm");
                    //var alarm = alarmPro == null ? "" : alarmPro.Value.ToString();
                    //var ztsPro = info.Property("zts");
                    //var zts = ztsPro == null ? "" : ztsPro.Value.ToString();
                    //var jckz1Pro = info.Property("jckz1");
                    //var jckz1 = jckz1Pro == null ? "" : jckz1Pro.Value.ToString();
                    //var jckz2Pro = info.Property("jckz2");
                    //var jckz2 = jckz2Pro == null ? "" : jckz2Pro.Value.ToString();
                    //var k1Pro = info.Property("k1");
                    //var k1 = k1Pro == null ? "0" : k1Pro.Value.ToString();
                    //pointInfoDt.Rows.Add(point, fzh, kh, type, wz, state, alarm, zts, jckz1, jckz2, k1, "0", "0", "0", "0", "0", "0", "0");
                    pointInfoDt.Rows.Add(item.point, item.fzh, item.kh, item.type, item.wz, item.state, item.alarm, item.zts, item.jckz1, item.jckz2, item.k1, "0", "0", "0", "0", "0", "0", "0");

                }

                var req = new OldPlsPointSyncRequest
                {
                    PointInfo = pointInfoDt
                };
                var res = OldPlsPointSync(req);
                return res;
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


