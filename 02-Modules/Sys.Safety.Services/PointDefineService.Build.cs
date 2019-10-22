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
using Sys.Safety.Request.PersonCache;
using Sys.Safety.Request.Area;
using Sys.Safety.Request.Position;
using System.Data.SqlClient;
using Sys.Safety.Enums;

namespace Sys.Safety.Services
{
    /// <summary>
    /// 测点定义服务
    /// </summary>
    public partial class PointDefineService : IPointDefineService
    {
        private IPointDefineRepository _repository;
        /// <summary>
        /// 测点定义缓存接口
        /// </summary>
        private IPointDefineCacheService _pointDefineCacheService;
        /// <summary>
        /// Jc_Mac表操作类
        /// </summary>
        private INetworkModuleService _networkModuleService;
        /// <summary>
        /// JC_MAC表缓存操作
        /// </summary>
        private INetworkModuleCacheService _networkModuleCacheService;
        /// <summary>
        /// 自动挂接设备缓存操作
        /// </summary>
        private IAutomaticArticulatedDeviceCacheService _automaticArticulatedDeviceCacheService;

        public PointDefineService(IPointDefineRepository _Repository, IPointDefineCacheService _PointDefineCacheService,
            INetworkModuleService _networkModuleService,
            INetworkModuleCacheService _networkModuleCacheService,
            IAutomaticArticulatedDeviceCacheService _automaticArticulatedDeviceCacheService)
        {
            this._repository = _Repository;
            this._pointDefineCacheService = _PointDefineCacheService;
            this._networkModuleService = _networkModuleService;
            this._networkModuleCacheService = _networkModuleCacheService;
            this._automaticArticulatedDeviceCacheService = _automaticArticulatedDeviceCacheService;
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

            PointDefineCacheGetByKeyRequest PointDefineCacheRequest = new PointDefineCacheGetByKeyRequest();
            PointDefineCacheRequest.Point = item.Point;
            PointDefineCacheRequest.IsQueryFromWriteCache = true;
            var result = _pointDefineCacheService.GetPointDefineCacheByKey(PointDefineCacheRequest);
            if (result.Data != null)
            {
                if (result.Data.Activity == "1")
                {
                    olditem = result.Data;
                }
            }

            //增加重复判断
            if (result.Data != null)
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
            var _jc_Def = ObjectConverter.Copy<Jc_DefInfo, Jc_DefModel>(item);
            var resultjc_Def = _repository.AddPointDefine(_jc_Def);

            //置下发初始化标记
            PointDefineCommon.PointInitializes(item, olditem);

            //添加到缓存
            PointDefineCacheAddRequest AddPointDefineCacheRequest = new PointDefineCacheAddRequest();
            AddPointDefineCacheRequest.PointDefineInfo = item;
            _pointDefineCacheService.AddPointDefineCache(AddPointDefineCacheRequest);

            return Result;
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

            PointDefineCacheGetAllRequest PointDefineCacheRequest = new PointDefineCacheGetAllRequest();
            PointDefineCacheRequest.IsQueryFromWriteCache = true;
            var result = _pointDefineCacheService.GetAllPointDefineCache(PointDefineCacheRequest);
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
                        var _jc_Def = ObjectConverter.Copy<Jc_DefInfo, Jc_DefModel>(item);
                        var resultjc_Def = _repository.AddPointDefine(_jc_Def);

                        //置下发初始化标记
                        PointDefineCommon.PointInitializes(item, olditem);
                    }


                    //添加到缓存
                    PointDefineCacheBatchAddRequest AddPointDefineCacheRequest = new PointDefineCacheBatchAddRequest();
                    AddPointDefineCacheRequest.PointDefineInfos = items;
                    _pointDefineCacheService.BacthAddPointDefineCache(AddPointDefineCacheRequest);

                });

            return Result;
        }

        /// <summary>
        /// 扩展属性实例化 //2017.6.9 by
        /// </summary>
        /// <param name="item"></param>
        private void AddExtension(Jc_DefInfo item)
        {
            item.NErrCount = 0;
            item.Fdstate = 0;
            item.BDisCharge = 0;
            item.NCtrlSate = 46;//馈电状态未知
            item.DeviceControlItems = new List<ControlItem>();
            item.SoleCodingChanels = new List<ControlItem>();
            item.GasThreeUnlockContro = 0;
            item.StationHisDataClear = 0;
            item.ModificationItems = new List<EditDeviceAddressItem>();
            item.HistoryControlState = 0;
            item.HistoryRealDataState = 0;
            item.RealTypeInfo = "";

            item.ClsAlarmObj = new AlarmProperty();

            item.ClsCommObj = new CommProperty((uint)item.Fzh);

            item.ClsCtrlObj = new List<ControlRemote>();

            item.ClsFiveMinObj = new FiveMinData();

            item.GradingAlarmItems = new List<GradingAlarmItem>();//2018.3.26 by 此处不加，则默认为null，会自动在开机时添加下发分级报警控制命令标记
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

            PointDefineCacheGetByKeyRequest PointDefineCacheRequest = new PointDefineCacheGetByKeyRequest();
            PointDefineCacheRequest.Point = item.Point;
            PointDefineCacheRequest.IsQueryFromWriteCache = true;
            var result = _pointDefineCacheService.GetPointDefineCacheByKey(PointDefineCacheRequest);
            if (result.Data != null)
            {
                if (result.Data.Activity == "1")
                {
                    olditem = result.Data;
                }
            }

            //增加重复判断            
            if (result.Data == null)
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
            var _jc_Def = ObjectConverter.Copy<Jc_DefInfo, Jc_DefModel>(item);
            _repository.UpdatePointDefine(_jc_Def);

            //置下发初始化标记
            PointDefineCommon.PointInitializes(item, olditem);

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
            paramater.Add("Bz13", Point.Bz13);
            paramater.Add("Bz14", Point.Bz14);
            paramater.Add("Bz15", Point.Bz15);
            paramater.Add("Bz16", Point.Bz16);
            paramater.Add("Bz17", Point.Bz17);
            paramater.Add("Bz18", Point.Bz18);
            paramater.Add("Bz19", Point.Bz19);
            paramater.Add("Bz20", Point.Bz20);
            paramater.Add("Remark", Point.Remark);
            paramater.Add("Upflag", Point.Upflag);
            paramater.Add("Addresstypeid", Point.Addresstypeid);
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

            //定义改变标记
            paramater.Add("PointEditState", Point.PointEditState);
            paramater.Add("DefIsInit", Point.DefIsInit);
            paramater.Add("kzchangeflag", Point.kzchangeflag);
            paramater.Add("ReDoDeal", Point.ReDoDeal);
            paramater.Add("Dormancyflag", Point.Dormancyflag);

            //修改标记
            paramater.Add("InfoState", Point.InfoState);

            DefineCacheUpdatePropertiesRequest defineCacheUpdatePropertiesRequest = new DefineCacheUpdatePropertiesRequest();
            defineCacheUpdatePropertiesRequest.PointID = Point.PointID;
            defineCacheUpdatePropertiesRequest.UpdateItems = paramater;
            _pointDefineCacheService.UpdatePointDefineInfo(defineCacheUpdatePropertiesRequest);
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
            List<Jc_DefInfo> Jc_DefCaches = new List<Jc_DefInfo>();
            Jc_DefInfo olditem = null;

            PointDefineCacheGetAllRequest PointDefineCacheRequest = new PointDefineCacheGetAllRequest();
            PointDefineCacheRequest.IsQueryFromWriteCache = true;
            var result = _pointDefineCacheService.GetAllPointDefineCache(PointDefineCacheRequest);
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
                    var _jc_Def = ObjectConverter.Copy<Jc_DefInfo, Jc_DefModel>(item);
                    var resultjc_Def = _repository.Update(_jc_Def);

                    //置下发初始化标记
                    PointDefineCommon.PointInitializes(item, olditem);

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
            PointDefineCacheBatchUpdateRequest UpdatePointDefineCacheRequest = new PointDefineCacheBatchUpdateRequest();
            UpdatePointDefineCacheRequest.PointDefineInfos = items;
            _pointDefineCacheService.BatchUpdatePointDefineCache(UpdatePointDefineCacheRequest);
            return Result;
        }
        /// <summary>
        /// 批量更新属性
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BasicResponse BatchUpdatePointDefineInfo(DefineCacheBatchUpdatePropertiesRequest request)
        {
            return _pointDefineCacheService.BatchUpdatePointDefineInfo(request);
        }
        /// <summary>
        /// 同时添加、更新定义及网络模拟绑定信息
        /// </summary>
        /// <param name="jc_DefAndjc_Macrequest"></param>
        /// <returns></returns>
        public BasicResponse AddUpdatePointDefineAndNetworkModuleCache(PointDefineAddNetworkModuleAddUpdateRequest PointDefineAddNetworkModuleRequest)
        {
            BasicResponse Result = new BasicResponse();
            Jc_DefInfo item = PointDefineAddNetworkModuleRequest.PointDefineInfo;
            Jc_MacInfo mac = PointDefineAddNetworkModuleRequest.NetworkModuleInfo;
            Jc_MacInfo oldmac = PointDefineAddNetworkModuleRequest.NetworkModuleInfoOld;
            Jc_DefInfo olditem = null;
            List<Jc_DefInfo> updateSonPointList = PointDefineAddNetworkModuleRequest.UpdateSonPointList;
            Jc_MacInfo switches = PointDefineAddNetworkModuleRequest.SwitchesInfo;
            Jc_MacInfo oldswitches = PointDefineAddNetworkModuleRequest.SwitchesInfoOld;

            PointDefineCacheGetByKeyRequest PointDefineCacheRequest = new PointDefineCacheGetByKeyRequest();
            PointDefineCacheRequest.Point = item.Point;
            var result = _pointDefineCacheService.GetPointDefineCacheByKey(PointDefineCacheRequest);
            if (result.Data != null)
            {
                if (result.Data.Activity == "1")
                {
                    olditem = result.Data;
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
                if (result.Data == null)
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
                    var _jc_Def = ObjectConverter.Copy<Jc_DefInfo, Jc_DefModel>(item);
                    var resultjc_Def = _repository.AddPointDefine(_jc_Def);
                }
                else
                {
                    var _jc_Def = ObjectConverter.Copy<Jc_DefInfo, Jc_DefModel>(item);
                    _repository.UpdatePointDefine(_jc_Def);
                }
                if (updateSonPointList.Count > 0)
                {
                    foreach (Jc_DefInfo upItem in updateSonPointList)
                    {
                        var _jc_Def = ObjectConverter.Copy<Jc_DefInfo, Jc_DefModel>(upItem);
                        _repository.UpdatePointDefine(_jc_Def);
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

                //保存新的交换机Mac信息   
                if (switches != null)
                {
                    if (switches.InfoState == InfoState.AddNew)
                    {
                        NetworkModuleAddRequest jc_Macrequest = new NetworkModuleAddRequest();
                        jc_Macrequest.NetworkModuleInfo = switches;
                        var resultjc_Mac = _networkModuleService.AddNetworkModule(jc_Macrequest);
                    }
                    else
                    {
                        NetworkModuleUpdateRequest jc_Macrequest = new NetworkModuleUpdateRequest();
                        jc_Macrequest.NetworkModuleInfo = switches;
                        _networkModuleService.UpdateNetworkModule(jc_Macrequest);
                    }
                }

                //更新以前的交换机Mac绑定信息               
                if (oldswitches != null)
                {
                    if (oldswitches.InfoState == InfoState.AddNew)
                    {
                        NetworkModuleAddRequest jc_Macrequest = new NetworkModuleAddRequest();
                        jc_Macrequest.NetworkModuleInfo = oldswitches;
                        var resultjc_Mac = _networkModuleService.AddNetworkModule(jc_Macrequest);
                    }
                    else
                    {
                        NetworkModuleUpdateRequest jc_Macrequest = new NetworkModuleUpdateRequest();
                        jc_Macrequest.NetworkModuleInfo = oldswitches;
                        _networkModuleService.UpdateNetworkModule(jc_Macrequest);
                    }
                }


                //置下发初始化标记
                PointDefineCommon.PointInitializes(item, olditem);

                //置传感器下发初始化标记
                foreach (Jc_DefInfo upItem in updateSonPointList)
                {
                    Jc_DefInfo olditem1 = new Jc_DefInfo();
                    PointDefineCacheGetByKeyRequest PointDefineCacheRequest1 = new PointDefineCacheGetByKeyRequest();
                    PointDefineCacheRequest1.Point = item.Point;
                    var result1 = _pointDefineCacheService.GetPointDefineCacheByKey(PointDefineCacheRequest1);
                    if (result1.Data != null)
                    {
                        if (result1.Data.Activity == "1")
                        {
                            olditem1 = result1.Data;
                            //置下发初始化标记
                            PointDefineCommon.PointInitializes(upItem, olditem1);
                        }
                    }

                }


                if (item.InfoState == InfoState.AddNew)
                {
                    //添加到缓存
                    PointDefineCacheAddRequest AddPointDefineCacheRequest = new PointDefineCacheAddRequest();
                    AddPointDefineCacheRequest.PointDefineInfo = item;
                    _pointDefineCacheService.AddPointDefineCache(AddPointDefineCacheRequest);
                }
                else
                {
                    //更新缓存
                    PointDefineCacheUpdateRequest UpdatePointDefineCacheRequest = new PointDefineCacheUpdateRequest();
                    UpdatePointDefineCacheRequest.PointDefineInfo = item;
                    _pointDefineCacheService.UpdatePointDefineCahce(UpdatePointDefineCacheRequest);
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
            paramater.Add("PointEditState", Point.PointEditState);
            paramater.Add("DefIsInit", Point.DefIsInit);
            paramater.Add("kzchangeflag", Point.kzchangeflag);
            paramater.Add("ReDoDeal", Point.ReDoDeal);
            paramater.Add("Dormancyflag", Point.Dormancyflag);

            //修改标记
            paramater.Add("InfoState", Point.InfoState);

            DefineCacheUpdatePropertiesRequest defineCacheUpdatePropertiesRequest = new DefineCacheUpdatePropertiesRequest();
            defineCacheUpdatePropertiesRequest.PointID = Point.PointID;
            defineCacheUpdatePropertiesRequest.UpdateItems = paramater;
            _pointDefineCacheService.UpdatePointDefineInfo(defineCacheUpdatePropertiesRequest);
        }
        public BasicResponse DeletePointDefine(PointDefineDeleteRequest PointDefineRequest)
        {
            _repository.DeletePointDefine(PointDefineRequest.Id);
            var jc_Defresponse = new BasicResponse();
            return jc_Defresponse;
        }
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineList(PointDefineGetListRequest PointDefineRequest)
        {
            var jc_Defresponse = new BasicResponse<List<Jc_DefInfo>>();
            PointDefineRequest.PagerInfo.PageIndex = PointDefineRequest.PagerInfo.PageIndex - 1;
            if (PointDefineRequest.PagerInfo.PageIndex < 0)
            {
                PointDefineRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jc_DefModelLists = _repository.GetPointDefineList(PointDefineRequest.PagerInfo.PageIndex, PointDefineRequest.PagerInfo.PageSize, out rowcount);
            var jc_DefInfoLists = new List<Jc_DefInfo>();
            foreach (var item in jc_DefModelLists)
            {
                var Jc_DefInfo = ObjectConverter.Copy<Jc_DefModel, Jc_DefInfo>(item);
                jc_DefInfoLists.Add(Jc_DefInfo);
            }
            jc_Defresponse.Data = jc_DefInfoLists;
            return jc_Defresponse;
        }
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineList()
        {
            var jc_Defresponse = new BasicResponse<List<Jc_DefInfo>>();
            var jc_DefModelLists = _repository.GetPointDefineList();
            var jc_DefInfoLists = new List<Jc_DefInfo>();
            foreach (var item in jc_DefModelLists)
            {
                var Jc_DefInfo = ObjectConverter.Copy<Jc_DefModel, Jc_DefInfo>(item);
                jc_DefInfoLists.Add(Jc_DefInfo);
            }
            jc_Defresponse.Data = jc_DefInfoLists;
            return jc_Defresponse;
        }
        public BasicResponse<Jc_DefInfo> GetPointDefineById(PointDefineGetRequest PointDefineRequest)
        {
            var result = _repository.GetPointDefineById(PointDefineRequest.Id);
            var jc_DefInfo = ObjectConverter.Copy<Jc_DefModel, Jc_DefInfo>(result);
            var jc_Defresponse = new BasicResponse<Jc_DefInfo>();
            jc_Defresponse.Data = jc_DefInfo;
            return jc_Defresponse;
        }
        /// <summary>
        /// 获取所有定义缓存数据
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetAllPointDefineCache()
        {
            BasicResponse<List<Jc_DefInfo>> Result = new BasicResponse<List<Jc_DefInfo>>();
            PointDefineCacheGetAllRequest pointDefineCacheRequest = new PointDefineCacheGetAllRequest();
            var result = _pointDefineCacheService.GetAllPointDefineCache(pointDefineCacheRequest);
            Result.Data = result.Data.FindAll(a => a.Activity == "1");
            return Result;
        }
        /// <summary>
        /// 根据测点号查找缓存信息
        /// </summary>
        /// <param name="jc_Defrequest"></param>
        /// <returns></returns>
        public BasicResponse<Jc_DefInfo> GetPointDefineCacheByPoint(PointDefineGetByPointRequest PointDefineRequest)
        {
            BasicResponse<Jc_DefInfo> Result = new BasicResponse<Jc_DefInfo>();
            PointDefineCacheGetByKeyRequest pointDefineCacheRequest = new PointDefineCacheGetByKeyRequest();
            pointDefineCacheRequest.Point = PointDefineRequest.Point;
            var result = _pointDefineCacheService.GetPointDefineCacheByKey(pointDefineCacheRequest);
            if (result.Data != null)
            {
                if (result.Data.Activity == "1")
                {
                    Result.Data = result.Data;
                }
            }
            return Result;
        }

        /// <summary>
        /// 根据测点名称/位置查询测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse<Jc_DefInfo> GetPointDefineCacheByWz(PointDefineGetByWzRequest PointDefineRequest)
        {
            BasicResponse<Jc_DefInfo> Result = new BasicResponse<Jc_DefInfo>();
            PointDefineCacheGetByConditonRequest pointDefineCacheRequest = new PointDefineCacheGetByConditonRequest();
            pointDefineCacheRequest.Predicate = a => a.Wz == PointDefineRequest.Wz && a.Activity == "1";
            var result = _pointDefineCacheService.GetPointDefineCache(pointDefineCacheRequest);
            if (result.Data.Count > 0)
            {
                Result.Data = result.Data[0];
            }
            else
            {
                Result.Code = 1;
                Result.Message = "未找到缓存对象";
                Result.Data = null;
            }
            return Result;
        }
        /// <summary>
        ///  根据MAC地址查询MAC地址对应的分站信息
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByMac(PointDefineGetByMacRequest PointDefineRequest)
        {
            BasicResponse<List<Jc_DefInfo>> Result = new BasicResponse<List<Jc_DefInfo>>();
            PointDefineCacheGetByConditonRequest pointDefineCacheRequest = new PointDefineCacheGetByConditonRequest();
            pointDefineCacheRequest.Predicate = a => a.Jckz1 == PointDefineRequest.Mac && a.Activity == "1";
            var result = _pointDefineCacheService.GetPointDefineCache(pointDefineCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        ///  查找COM下的所有分站
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByCOM(PointDefineGetByCOMRequest PointDefineRequest)
        {
            BasicResponse<List<Jc_DefInfo>> Result = new BasicResponse<List<Jc_DefInfo>>();
            PointDefineCacheGetByConditonRequest pointDefineCacheRequest = new PointDefineCacheGetByConditonRequest();
            pointDefineCacheRequest.Predicate = a => a.K3 == int.Parse(PointDefineRequest.COM) && a.Activity == "1";
            var result = _pointDefineCacheService.GetPointDefineCache(pointDefineCacheRequest);
            Result.Data = result.Data;
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
            PointDefineCacheGetByConditonRequest pointDefineCacheRequest = new PointDefineCacheGetByConditonRequest();
            pointDefineCacheRequest.Predicate = a => a.DevPropertyID == PointDefineRequest.DevpropertID && a.Activity == "1";
            var result = _pointDefineCacheService.GetPointDefineCache(pointDefineCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        ///通过设备种类查找测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByDevClassID(PointDefineGetByDevClassIDRequest PointDefineRequest)
        {
            BasicResponse<List<Jc_DefInfo>> Result = new BasicResponse<List<Jc_DefInfo>>();
            PointDefineCacheGetByConditonRequest pointDefineCacheRequest = new PointDefineCacheGetByConditonRequest();
            pointDefineCacheRequest.Predicate = a => a.DevClassID == PointDefineRequest.DevClassID && a.Activity == "1";
            var result = _pointDefineCacheService.GetPointDefineCache(pointDefineCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        ///通过设备型号查找测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByDevModelID(PointDefineGetByDevModelIDRequest PointDefineRequest)
        {
            BasicResponse<List<Jc_DefInfo>> Result = new BasicResponse<List<Jc_DefInfo>>();
            PointDefineCacheGetByConditonRequest pointDefineCacheRequest = new PointDefineCacheGetByConditonRequest();
            pointDefineCacheRequest.Predicate = a => a.DevModelID == PointDefineRequest.DevModelID && a.Activity == "1";
            var result = _pointDefineCacheService.GetPointDefineCache(pointDefineCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        ///通过设备类型查找测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByDevID(PointDefineGetByDevIDRequest PointDefineRequest)
        {
            BasicResponse<List<Jc_DefInfo>> Result = new BasicResponse<List<Jc_DefInfo>>();
            PointDefineCacheGetByConditonRequest pointDefineCacheRequest = new PointDefineCacheGetByConditonRequest();
            pointDefineCacheRequest.Predicate = a => a.Devid == PointDefineRequest.DevID && a.Activity == "1";
            var result = _pointDefineCacheService.GetPointDefineCache(pointDefineCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        ///根据分站号查找分站下的所有测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationID(PointDefineGetByStationIDRequest PointDefineRequest)
        {
            BasicResponse<List<Jc_DefInfo>> Result = new BasicResponse<List<Jc_DefInfo>>();
            PointDefineCacheGetByConditonRequest pointDefineCacheRequest = new PointDefineCacheGetByConditonRequest();
            pointDefineCacheRequest.Predicate = a => a.Fzh == PointDefineRequest.StationID && a.Activity == "1";
            var result = _pointDefineCacheService.GetPointDefineCache(pointDefineCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        ///根据分站号查找分站下的所有测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationPoint(PointDefineGetByStationPointRequest PointDefineRequest)
        {
            BasicResponse<List<Jc_DefInfo>> Result = new BasicResponse<List<Jc_DefInfo>>();

            PointDefineCacheGetByConditonRequest pointDefineCacheRequest = new PointDefineCacheGetByConditonRequest();
            pointDefineCacheRequest.Predicate = a => a.Point == PointDefineRequest.StationPoint & a.DevPropertyID == 0 && a.Activity == "1";
            var resultStation = _pointDefineCacheService.GetPointDefineCache(pointDefineCacheRequest);//先查找分站对应的分站号

            PointDefineCacheGetByConditonRequest pointDefineCacheStationRequest = new PointDefineCacheGetByConditonRequest();
            pointDefineCacheStationRequest.Predicate = a => a.Fzh == resultStation.Data[0].Fzh & a.DevPropertyID != 0 && a.Activity == "1";
            var result = _pointDefineCacheService.GetPointDefineCache(pointDefineCacheStationRequest);//再根据分站号查找分站下面的设备
            Result.Data = result.Data;

            return Result;
        }
        /// <summary>
        ///通过分站号、口号、设备性质查找测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationIDChannelIDDevPropertID(PointDefineGetByStationIDChannelIDDevPropertIDRequest PointDefineRequest)
        {
            BasicResponse<List<Jc_DefInfo>> Result = new BasicResponse<List<Jc_DefInfo>>();
            PointDefineCacheGetByConditonRequest pointDefineCacheRequest = new PointDefineCacheGetByConditonRequest();
            pointDefineCacheRequest.Predicate = a => a.Fzh == PointDefineRequest.StationID && a.Kh == PointDefineRequest.ChannelID
                && a.DevPropertyID == PointDefineRequest.DevPropertID && a.Activity == "1";
            var result = _pointDefineCacheService.GetPointDefineCache(pointDefineCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        ///通过分站号、设备性质 查找测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationIDDevPropertID(PointDefineGetByStationIDDevPropertIDRequest PointDefineRequest)
        {
            BasicResponse<List<Jc_DefInfo>> Result = new BasicResponse<List<Jc_DefInfo>>();
            PointDefineCacheGetByConditonRequest pointDefineCacheRequest = new PointDefineCacheGetByConditonRequest();
            pointDefineCacheRequest.Predicate = a => a.Fzh == PointDefineRequest.StationID
                && a.DevPropertyID == PointDefineRequest.DevPropertID && a.Activity == "1";
            var result = _pointDefineCacheService.GetPointDefineCache(pointDefineCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        ///通过分站号、通道号 地址号、设备性质 查找测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationIDChannelIDAddressIDDevPropertID(PointDefineGetByStationIDChannelIDAddressIDDevPropertIDRequest PointDefineRequest)
        {
            BasicResponse<List<Jc_DefInfo>> Result = new BasicResponse<List<Jc_DefInfo>>();
            PointDefineCacheGetByConditonRequest pointDefineCacheRequest = new PointDefineCacheGetByConditonRequest();
            pointDefineCacheRequest.Predicate = a => a.Fzh == PointDefineRequest.StationID
                 && a.Kh == PointDefineRequest.ChannelID
                  && a.Dzh == PointDefineRequest.AddressID
                && a.DevPropertyID == PointDefineRequest.DevPropertID && a.Activity == "1";
            var result = _pointDefineCacheService.GetPointDefineCache(pointDefineCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        ///通过分站号、通道号 查找测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStationIDChannelID(PointDefineGetByStationIDChannelIDRequest PointDefineRequest)
        {
            BasicResponse<List<Jc_DefInfo>> Result = new BasicResponse<List<Jc_DefInfo>>();
            PointDefineCacheGetByConditonRequest pointDefineCacheRequest = new PointDefineCacheGetByConditonRequest();
            pointDefineCacheRequest.Predicate = a => a.Fzh == PointDefineRequest.StationID
                 && a.Kh == PointDefineRequest.ChannelID && a.Activity == "1";
            var result = _pointDefineCacheService.GetPointDefineCache(pointDefineCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        /// 根据区域ID查找测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByAreaId(PointDefineGetByAreaIdRequest PointDefineRequest)
        {
            BasicResponse<List<Jc_DefInfo>> Result = new BasicResponse<List<Jc_DefInfo>>();
            PointDefineCacheGetByConditonRequest pointDefineCacheRequest = new PointDefineCacheGetByConditonRequest();
            pointDefineCacheRequest.Predicate = a => a.Areaid == PointDefineRequest.AreaId && a.Activity == "1";
            var result = _pointDefineCacheService.GetPointDefineCache(pointDefineCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        /// 根据地点类型获取测点信息
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByAddressTypeId(PointDefineGetByAddressTypeIdRequest PointDefineRequest)
        {
            BasicResponse<List<Jc_DefInfo>> Result = new BasicResponse<List<Jc_DefInfo>>();
            PointDefineCacheGetByConditonRequest pointDefineCacheRequest = new PointDefineCacheGetByConditonRequest();
            pointDefineCacheRequest.Predicate = a => a.Addresstypeid == PointDefineRequest.AddressTypeId && a.Activity == "1";
            var result = _pointDefineCacheService.GetPointDefineCache(pointDefineCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        ///通过区域编码 查找测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByAreaCode(PointDefineGetByAreaCodeRequest PointDefineRequest)
        {
            BasicResponse<List<Jc_DefInfo>> Result = new BasicResponse<List<Jc_DefInfo>>();
            PointDefineCacheGetByConditonRequest pointDefineCacheRequest = new PointDefineCacheGetByConditonRequest();
            pointDefineCacheRequest.Predicate = a => a.AreaLoc.IndexOf(PointDefineRequest.AreaCode) == 0 && a.Activity == "1";
            var result = _pointDefineCacheService.GetPointDefineCache(pointDefineCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        ///通过区域编码、设备性质 查找测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByAreaCodeDevPropertID(PointDefineGetByAreaCodeDevPropertIDRequest PointDefineRequest)
        {
            BasicResponse<List<Jc_DefInfo>> Result = new BasicResponse<List<Jc_DefInfo>>();
            PointDefineCacheGetByConditonRequest pointDefineCacheRequest = new PointDefineCacheGetByConditonRequest();
            pointDefineCacheRequest.Predicate = a => a.AreaLoc.IndexOf(PointDefineRequest.AreaCode) == 0 && a.DevPropertyID != PointDefineRequest.DevPropertID && a.Activity == "1";
            var result = _pointDefineCacheService.GetPointDefineCache(pointDefineCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        ///通过区域名称、测点号、安装位置等关键字查找分站设备
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStrKeywords(PointDefineGetByStrKeywordsRequest PointDefineRequest)
        {
            BasicResponse<List<Jc_DefInfo>> Result = new BasicResponse<List<Jc_DefInfo>>();
            PointDefineCacheGetByConditonRequest pointDefineCacheRequest = new PointDefineCacheGetByConditonRequest();
            string strKeywords = PointDefineRequest.StrKeywords;
            pointDefineCacheRequest.Predicate = a => (strKeywords == "" || a.Point.Contains(strKeywords) || a.AreaName.Contains(strKeywords) || a.Wz.Contains(strKeywords)
                    ) && a.Kh != 0 && a.Activity == "1";
            var result = _pointDefineCacheService.GetPointDefineCache(pointDefineCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        ///通过测点ID 查找测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByPointID(PointDefineGetByPointIDRequest PointDefineRequest)
        {
            BasicResponse<List<Jc_DefInfo>> Result = new BasicResponse<List<Jc_DefInfo>>();
            PointDefineCacheGetByConditonRequest pointDefineCacheRequest = new PointDefineCacheGetByConditonRequest();
            pointDefineCacheRequest.Predicate = a => a.PointID == PointDefineRequest.PointID && a.Activity == "1";
            var result = _pointDefineCacheService.GetPointDefineCache(pointDefineCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        /// 获取所有电池电量过低的传感器（目前只支持无线）
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheBySensorPowerAlarmValue(PointDefineGetBySensorPowerAlarmValueRequest PointDefineRequest)
        {
            BasicResponse<List<Jc_DefInfo>> Result = new BasicResponse<List<Jc_DefInfo>>();
            PointDefineCacheGetByConditonRequest pointDefineCacheRequest = new PointDefineCacheGetByConditonRequest();
            pointDefineCacheRequest.Predicate = a => a.Voltage != 0 && a.Voltage < PointDefineRequest.SensorPowerAlarmValue && a.Activity == "1" && a.Devid == "24";
            var result = _pointDefineCacheService.GetPointDefineCache(pointDefineCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        /// 获取所有欠压报警设备
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByUnderVoltageAlarmValue(PointDefineGetByUnderVoltageAlarmValueRequest PointDefineRequest)
        {
            BasicResponse<List<Jc_DefInfo>> Result = new BasicResponse<List<Jc_DefInfo>>();
            PointDefineCacheGetByConditonRequest pointDefineCacheRequest = new PointDefineCacheGetByConditonRequest();
            pointDefineCacheRequest.Predicate = a => a.State == (int)(DeviceRunState.UnderVoltageAlarm) && a.Activity == "1";
            var result = _pointDefineCacheService.GetPointDefineCache(pointDefineCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        /// 获取所有分级报警的设备
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByGradingAlarmLevel(PointDefineGetByGradingAlarmLevelRequest PointDefineRequest)
        {
            BasicResponse<List<Jc_DefInfo>> Result = new BasicResponse<List<Jc_DefInfo>>();
            PointDefineCacheGetByConditonRequest pointDefineCacheRequest = new PointDefineCacheGetByConditonRequest();
            pointDefineCacheRequest.Predicate = a => a.GradingAlarmLevel > 0 && a.Activity == "1";
            var result = _pointDefineCacheService.GetPointDefineCache(pointDefineCacheRequest);
            Result.Data = result.Data;
            return Result;
        }

        /// <summary>
        /// 获取网络通信的分站
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetNetworkCommunicationStation()
        {
            BasicResponse<List<Jc_DefInfo>> Result = new BasicResponse<List<Jc_DefInfo>>();
            PointDefineCacheGetByConditonRequest pointDefineCacheRequest = new PointDefineCacheGetByConditonRequest();
            pointDefineCacheRequest.Predicate = a => !string.IsNullOrEmpty(a.Jckz1) && a.Activity == "1";
            var result = _pointDefineCacheService.GetPointDefineCache(pointDefineCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        /// 获取串口通信的分站
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetCOMCommunicationStation()
        {
            BasicResponse<List<Jc_DefInfo>> Result = new BasicResponse<List<Jc_DefInfo>>();
            PointDefineCacheGetByConditonRequest pointDefineCacheRequest = new PointDefineCacheGetByConditonRequest();
            pointDefineCacheRequest.Predicate = a => string.IsNullOrEmpty(a.Jckz1) && a.K3 > 0 && a.Activity == "1";
            var result = _pointDefineCacheService.GetPointDefineCache(pointDefineCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        /// 获取未通信的分站
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetNonCommunicationStation()
        {
            BasicResponse<List<Jc_DefInfo>> Result = new BasicResponse<List<Jc_DefInfo>>();
            PointDefineCacheGetByConditonRequest pointDefineCacheRequest = new PointDefineCacheGetByConditonRequest();
            pointDefineCacheRequest.Predicate = a => a.DevPropertyID == 0 && string.IsNullOrEmpty(a.Jckz1) && string.IsNullOrEmpty(a.Jckz2) && a.K3 <= 0 && a.Activity == "1";
            var result = _pointDefineCacheService.GetPointDefineCache(pointDefineCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        /// 测点定义保存巡检操作
        /// </summary>
        /// <returns></returns>
        public BasicResponse PointDefineSaveData()
        {
            BasicResponse Result = new BasicResponse();

            //修改，根据保存时，置的初始化标记来置初始化标记
            PointDefineCacheGetAllRequest pointDefineCacheRequest = new PointDefineCacheGetAllRequest();
            pointDefineCacheRequest.IsQueryFromWriteCache = true;
            var result = _pointDefineCacheService.GetAllPointDefineCache(pointDefineCacheRequest);
            List<Jc_DefInfo> newItems = result.Data.Where(a => a.DefIsInit || a.kzchangeflag || a.Dormancyflag || a.ReDoDeal == 1).ToList();
            //保存巡检时，如果设备置了休眠的标记，则将缓存的Bz4更新  20170705
            newItems.ForEach(a =>
            {
                if (a.Dormancyflag)
                {
                    a.Bz4 |= 0x02;
                    Dictionary<string, object> paramater = new Dictionary<string, object>();
                    paramater.Add("Bz4", a.Bz4);
                    DefineCacheUpdatePropertiesRequest defineCacheUpdatePropertiesRequest = new DefineCacheUpdatePropertiesRequest();
                    defineCacheUpdatePropertiesRequest.PointID = a.PointID;
                    defineCacheUpdatePropertiesRequest.UpdateItems = paramater;
                    _pointDefineCacheService.UpdatePointDefineInfo(defineCacheUpdatePropertiesRequest);
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
            result = _pointDefineCacheService.GetAllPointDefineCache(pointDefineCacheRequest);
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
            DefineCacheBatchUpdatePropertiesRequest cacheUpdatePropertiesRequest = new DefineCacheBatchUpdatePropertiesRequest();
            cacheUpdatePropertiesRequest.PointItems = updateItemsList;
            _pointDefineCacheService.BatchUpdatePointDefineInfo(cacheUpdatePropertiesRequest);
            //UpdatePointDefineCacheRequest.PointDefineInfos = items;
            // _pointDefineCacheService.BatchUpdatePointDefineCache(UpdatePointDefineCacheRequest);
            //删除缓存中的非活动点
            List<Jc_DefInfo> NonActivityList = result.Data.FindAll(a => a.Activity == "0" && a.InfoState == InfoState.Modified);
            PointDefineCacheBatchDeleteRequest DeletePointDefineCacheRequest = new PointDefineCacheBatchDeleteRequest();
            DeletePointDefineCacheRequest.PointDefineInfos = NonActivityList;
            _pointDefineCacheService.BatchDeletePointDefineCache(DeletePointDefineCacheRequest);


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
        private Dictionary<long, Dictionary<string, Jc_DefInfo>> GroupPoint(List<Jc_DefInfo> lstPoints)
        {
            Dictionary<long, Dictionary<string, Jc_DefInfo>> ret = new Dictionary<long, Dictionary<string, Jc_DefInfo>>();
            Dictionary<string, Jc_DefInfo> ls_def = new Dictionary<string, Jc_DefInfo>();
            try
            {
                foreach (Jc_DefInfo def in lstPoints)
                {
                    if (ret.ContainsKey(def.Sysid))
                    {
                        ls_def = (Dictionary<string, Jc_DefInfo>)ret[def.Sysid];
                        ls_def.Add(def.Point, def);
                        ret[def.Sysid] = ls_def;
                    }
                    else
                    {
                        ls_def = new Dictionary<string, Jc_DefInfo>();
                        ls_def.Add(def.Point, def);
                        ret.Add(def.Sysid, ls_def);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("GroupPoint：" + ex.Message);
            }

            return ret;
        }

        /// <summary>
        /// 根据分站号获取电源箱地址号
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BasicResponse<List<string>> GetAllPowerBoxAddress(GetAllPowerBoxAddressRequest request)
        {
            var point = request.Fzh.PadLeft(3, '0') + "0000";
            var req = new PointDefineGetByPointRequest
            {
                Point = point
            };
            var res = GetPointDefineCacheByPoint(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            var model = res.Data;
            var retData = new List<string>();

            if (model.BatteryItems != null)
            {
                foreach (var item in model.BatteryItems)
                {
                    retData.Add(item.BatteryAddress);
                }
            }
            var ret = new BasicResponse<List<string>>
            {
                Data = retData
            };
            return ret;
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

        public BasicResponse<BatteryItem> GetSubstationBatteryInfo(GetSubstationBatteryInfoRequest request)
        {
            var req = new PointDefineCacheGetByConditonRequest
            {
                Predicate = a => a.Fzh.ToString() == request.Fzh && a.Activity == "1"
            };
            var res = _pointDefineCacheService.GetPointDefineCache(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            var battery = from m in res.Data[0].BatteryItems
                          where m.BatteryAddress == request.Address
                          select m;
            var ret = new BasicResponse<BatteryItem>
            {
                Data = battery.FirstOrDefault()
            };
            return ret;
        }

        public BasicResponse<GetSubstationAllPowerBoxInfoResponse> GetSubstationAllPowerBoxInfo(GetSubstationAllPowerBoxInfoRequest request)
        {
            var req = new PointDefineCacheGetByConditonRequest
            {
                Predicate = a => a.Fzh.ToString() == request.Fzh && a.Activity == "1"
            };
            var res = _pointDefineCacheService.GetPointDefineCache(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }

            var jcDefInfo = res.Data.FirstOrDefault();
            if (jcDefInfo != null)
            {
                var ret = new BasicResponse<GetSubstationAllPowerBoxInfoResponse>()
                {
                    Data = new GetSubstationAllPowerBoxInfoResponse()
                    {
                        PowerBoxInfo = jcDefInfo.BatteryItems,
                        PowerDateTime = jcDefInfo.PowerDateTime
                    }
                };
                return ret;
            }
            else
            {
                var ret = new BasicResponse<GetSubstationAllPowerBoxInfoResponse>()
                {
                    Data = new GetSubstationAllPowerBoxInfoResponse()
                    {
                        PowerBoxInfo = new List<BatteryItem>(),
                        PowerDateTime = new DateTime()
                    }
                };
                return ret;
            }
        }

        #region ----电源箱操作接口----

        /// <summary>
        /// 下发获取电源箱实时数据命令
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BasicResponse SendQueryBatteryRealDataRequest(SendDComReqest request)
        {
            try
            {
                List<BatteryControlItem> queryBatteryRealDataItems = request.queryBatteryRealDataItems;
                if (queryBatteryRealDataItems != null)
                {
                    foreach (BatteryControlItem item in queryBatteryRealDataItems)
                    {
                        int fzh = 0;
                        if (item.DevProID == 0)
                        {
                            if (int.TryParse(item.FzhOrMac, out fzh))
                            {
                                StationDControlRequest controlRequest = new StationDControlRequest();
                                controlRequest.controlItems = new List<StationControlItem>();
                                StationControlItem controlItem = new StationControlItem();
                                controlItem.fzh = (ushort)fzh;
                                controlItem.controlType = (byte)item.controlType;
                                controlRequest.controlItems.Add(controlItem);

                                SendStationDControl(controlRequest);
                            }
                        }
                        else if (item.DevProID == 16)
                        {
                            SendSwitchesQueryBatteryRealDataComm(item.FzhOrMac, item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("sendD出错，原因：" + ex.Message + "-" + ex.StackTrace);
            }
            return new BasicResponse();
        }

        /// <summary>
        /// 分站电源箱控制命令
        /// </summary>
        /// <param name="controlInfo"></param>
        /// <returns></returns>
        public BasicResponse SendStationDControl(StationDControlRequest request)
        {
            List<StationControlItem> stationControlItems = request.controlItems;
            foreach (StationControlItem item in stationControlItems)
            {
                Jc_DefInfo fzItem = GetStationByFzh(item.fzh);
                Dictionary<string, object> updateItems = new Dictionary<string, object>();
                if (fzItem != null)
                {
                    if (item.controlType == 0)//表示不进行任何操作,继续下发上次的D命令，否则在老分站下发0时，网关会自动判断为下发解除放电命令 2018.3.20 by
                    {
                        //如果分站已经切回交流，还是下发的放电命令，则取消
                        if (fzItem.State == (short)DeviceRunState.EquipmentAC && fzItem.BDisCharge == 2)
                        {
                            item.controlType = 0;
                        }
                        else
                        {
                            item.controlType = fzItem.BDisCharge;
                        }
                    }
                    fzItem.sendDTime = DateTime.Now;

                    //当前正在下发，用户请求终止
                    fzItem.ClsCommObj.NCommandbz |= 0x0002;
                    updateItems.Add("sendDTime", fzItem.sendDTime);
                    updateItems.Add("BDisCharge", item.controlType);
                    updateItems.Add("ClsCommObj", fzItem.ClsCommObj);

                    if (updateItems.Count > 0)
                    {
                        UpdatePointDefineInfo(fzItem.PointID, updateItems);
                    }
                }
            }
            return new BasicResponse();
        }

        /// <summary>
        /// 下发交换机电源箱控制命令
        /// </summary>
        /// <param name="mac"></param>
        /// <param name="controlType"></param>
        /// <returns></returns>
        public BasicResponse SendSwitchesDControl(SwitchesDControlRequest controlInfo)
        {

            List<SwichControlItem> swichControlItems = controlInfo.swichControlItems;
            foreach (SwichControlItem item in swichControlItems)
            {
                NetworkModuleCacheGetByKeyRequest networkModuleCacheGetByKeyRequest = new NetworkModuleCacheGetByKeyRequest();
                networkModuleCacheGetByKeyRequest.Mac = item.mac;
                List<Jc_MacInfo> macItems = CacheDataHelper.GetAllMacItems();
                NetworkModuleCacheUpdatePropertiesRequest networkModuleCacheUpdatePropertiesRequest = new NetworkModuleCacheUpdatePropertiesRequest();

                Dictionary<string, object> updateItems = new Dictionary<string, object>();
                var reslut = _networkModuleCacheService.GetNetworkModuleCacheByKey(networkModuleCacheGetByKeyRequest);
                if (reslut.IsSuccess && reslut != null && reslut.Data != null)
                {
                    Jc_MacInfo macInfo = reslut.Data;
                    NetworkModuleCacheUpdateNCommandRequest networkModuleCacheUpdateNCommandRequest = new NetworkModuleCacheUpdateNCommandRequest();
                    if (macInfo != null)
                    {
                        //if ((macInfo.NCommandbz & 0x01) == 0x01 && item.controlType == 0)
                        if (item.controlType == 1) //2017.814 by
                        {
                            //之前在动作，现在要取消 
                            //macInfo.NCommandbz &= (0xFFFF - 0x01);
                            macInfo.NCommandbz |= 0x01;
                            macInfo.BatteryControl = item.controlType;
                            macInfo.SendBatteryControlCount++;
                            macInfo.SendDtime = DateTime.Now;

                            updateItems.Add("NCommandbz", macInfo.NCommandbz);
                            updateItems.Add("BatteryControl", macInfo.BatteryControl);
                            updateItems.Add("SendBatteryControlCount", macInfo.NetID);
                            updateItems.Add("State", macInfo.SendBatteryControlCount);
                            updateItems.Add("SendDtime", macInfo.SendDtime);

                            networkModuleCacheUpdatePropertiesRequest.Mac = macInfo.MAC;
                            networkModuleCacheUpdatePropertiesRequest.UpdateItems = updateItems;
                            _networkModuleCacheService.UpdateNetworkInfo(networkModuleCacheUpdatePropertiesRequest);
                            //networkModuleCacheUpdateNCommandRequest.NetWorkModuleInfo = macInfo;
                            //_networkModuleCacheService.UpdateNetworkModuleNCommand(networkModuleCacheUpdateNCommandRequest);
                        }
                        //else if ((macInfo.NCommandbz & 0x01) == 0x00 && item.controlType == 1)
                        else if (item.controlType == 2)
                        {
                            //之前无动作，现在要执行 
                            macInfo.NCommandbz |= 0x01;
                            macInfo.BatteryControl = item.controlType;
                            macInfo.SendBatteryControlCount++;
                            macInfo.SendDtime = DateTime.Now;
                            //networkModuleCacheUpdateNCommandRequest.NetWorkModuleInfo = macInfo;
                            //_networkModuleCacheService.UpdateNetworkModuleNCommand(networkModuleCacheUpdateNCommandRequest);

                            updateItems.Add("NCommandbz", macInfo.NCommandbz);
                            updateItems.Add("BatteryControl", macInfo.BatteryControl);
                            updateItems.Add("SendBatteryControlCount", macInfo.NetID);
                            updateItems.Add("State", macInfo.SendBatteryControlCount);
                            updateItems.Add("SendDtime", macInfo.SendDtime);

                            networkModuleCacheUpdatePropertiesRequest.Mac = macInfo.MAC;
                            networkModuleCacheUpdatePropertiesRequest.UpdateItems = updateItems;
                            _networkModuleCacheService.UpdateNetworkInfo(networkModuleCacheUpdatePropertiesRequest);
                        }
                    }
                }
            }

            return new BasicResponse();
        }

        #endregion

        #region ----获取分站历史数据接口----

        /// <summary>
        /// 下发获取分站历史控制数据命令
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        public BasicResponse SendQueryHistoryControlRequest(GetHistoryControlRequest request)
        {
            List<StationControlItem> stationControlItems = request.controlItems;
            foreach (StationControlItem item in stationControlItems)
            {
                Jc_DefInfo fzItem = GetStationByFzh(item.fzh);
                Dictionary<string, object> updateItems = new Dictionary<string, object>();
                if (fzItem != null)
                {
                    //if ((fzItem.ClsCommObj.NCommandbz & 0x0200) == 0x0200 && item.controlType == 0)
                    if (item.controlType == 0)
                    {
                        //当前正在下发，用户请求终止
                        fzItem.ClsCommObj.NCommandbz &= (0xFFFF - 0x0200);
                        fzItem.HistoryControlState = 3;
                        fzItem.HistoryControlLegacyCount = 0;
                        updateItems.Add("ClsCommObj", fzItem.ClsCommObj);
                        updateItems.Add("HistoryControlState", fzItem.HistoryControlState);
                        updateItems.Add("HistoryControlLegacyCount", fzItem.HistoryControlLegacyCount);
                    }
                    //else if ((fzItem.ClsCommObj.NCommandbz & 0x0200) == 0x00 && item.controlType == 1)
                    else if (item.controlType == 1)
                    {
                        //当前未下发，用户请求下发
                        fzItem.ClsCommObj.NCommandbz |= 0x0200;
                        fzItem.HistoryControlState = 1;
                        fzItem.HistoryControlLegacyCount = -1;
                        updateItems.Add("ClsCommObj", fzItem.ClsCommObj);
                        updateItems.Add("HistoryControlState", fzItem.HistoryControlState);
                        updateItems.Add("HistoryControlLegacyCount", fzItem.HistoryControlLegacyCount);
                    }
                    else if (item.controlType == 2)
                    {
                        //还原为初始状态
                        fzItem.ClsCommObj.NCommandbz &= (0xFFFF - 0x0200);
                        fzItem.HistoryControlState = 0;
                        fzItem.HistoryControlLegacyCount = 0;
                        updateItems.Add("ClsCommObj", fzItem.ClsCommObj);
                        updateItems.Add("HistoryControlState", fzItem.HistoryControlState);
                        updateItems.Add("HistoryControlLegacyCount", fzItem.HistoryControlLegacyCount);
                    }
                    if (updateItems.Count > 0)
                    {
                        UpdatePointDefineInfo(fzItem.PointID, updateItems);
                    }
                }
            }
            return new BasicResponse();
        }

        /// <summary>
        /// 下发获取分站历史5分钟数据命令
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BasicResponse SendQueryHistoryRealDataRequest(HistoryRealDataRequest request)
        {
            List<StationControlItem> stationControlItems = request.controlItems;
            foreach (StationControlItem item in stationControlItems)
            {
                Jc_DefInfo fzItem = GetStationByFzh(item.fzh);
                Dictionary<string, object> updateItems = new Dictionary<string, object>();
                if (fzItem != null)
                {
                    //if ((fzItem.ClsCommObj.NCommandbz & 0x0400) == 0x0400 && item.controlType == 0)
                    if (item.controlType == 0)
                    {
                        //当前正在下发，用户请求终止
                        fzItem.ClsCommObj.NCommandbz &= (0xFFFF - 0x0400);
                        fzItem.HistoryRealDataState = 3;
                        fzItem.HistoryRealDataLegacyCount = 0;
                        updateItems.Add("ClsCommObj", fzItem.ClsCommObj);
                        updateItems.Add("HistoryRealDataState", fzItem.HistoryRealDataState);
                        updateItems.Add("HistoryRealDataLegacyCount", fzItem.HistoryRealDataLegacyCount);
                    }
                    //else if ((fzItem.ClsCommObj.NCommandbz & 0x0400) == 0x00 && item.controlType == 1)
                    else if (item.controlType == 1)
                    {
                        Random random = new Random();
                        //当前未下发，用户请求下发
                        fzItem.ClsCommObj.NCommandbz |= 0x0400;
                        fzItem.SerialNumber = (byte)random.Next(1, 255);
                        fzItem.HistoryRealDataState = 1;
                        fzItem.HistoryRealDataLegacyCount = -1;
                        updateItems.Add("SerialNumber", fzItem.SerialNumber);
                        updateItems.Add("ClsCommObj", fzItem.ClsCommObj);
                        updateItems.Add("HistoryRealDataState", fzItem.HistoryRealDataState);
                        updateItems.Add("HistoryRealDataLegacyCount", fzItem.HistoryRealDataLegacyCount);
                    }
                    else if (item.controlType == 2)
                    {
                        //还原为初始状态
                        fzItem.ClsCommObj.NCommandbz &= (0xFFFF - 0x0400);
                        fzItem.HistoryRealDataState = 0;
                        fzItem.HistoryRealDataLegacyCount = 0;
                        updateItems.Add("ClsCommObj", fzItem.ClsCommObj);
                        updateItems.Add("HistoryRealDataState", fzItem.HistoryRealDataState);
                        updateItems.Add("HistoryRealDataLegacyCount", fzItem.HistoryRealDataLegacyCount);
                    }
                    if (updateItems.Count > 0)
                    {
                        UpdatePointDefineInfo(fzItem.PointID, updateItems);
                    }
                }
            }
            return new BasicResponse();
        }

        #endregion

        /// <summary>
        /// 获取设备唯一编码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BasicResponse QueryDeviceInfoRequest(DeviceInfoRequest request)
        {
            List<DeviceInfoRequestItem> deviceInfoRequestItems = request.deviceInfoRequestItems;
            Jc_DefInfo fzItem;
            Dictionary<string, object> updateItems;
            foreach (DeviceInfoRequestItem item in deviceInfoRequestItems)
            {
                updateItems = new Dictionary<string, object>();
                fzItem = GetStationByFzh(item.Fzh);
                if (fzItem != null)
                {
                    //if ((fzItem.ClsCommObj.NCommandbz & 0x0100) == 0x0100 && item.controlType == 0)
                    List<int> getDeviceDetailDtataAddressLst = new List<int>();
                    PointDefineCacheGetByStationRequest pointDefineCacheRequest = new PointDefineCacheGetByStationRequest();
                    pointDefineCacheRequest.Station = (short)item.Fzh;
                    List<Jc_DefInfo> def = _pointDefineCacheService.GetPointDefineCacheByStation(pointDefineCacheRequest).Data;
                    if (def.Count > 0)
                    {
                        foreach (Jc_DefInfo temp in def)
                        {
                            if (temp.Kh > 0)
                            {
                                getDeviceDetailDtataAddressLst.Add(temp.Kh);
                            }
                        }
                    }
                    fzItem.GetDeviceDetailDtataAddressLst = getDeviceDetailDtataAddressLst;
                    if (item.controlType == 0)
                    {
                        //当前正在下发，用户请求终止
                        fzItem.ClsCommObj.NCommandbz &= (0xFFFF - 0x0100);
                        //fzItem.GetSoftwareVersions = item.GetSoftwareVersions;
                        //fzItem.GetHardwareVersions = item.GetHardwareVersions;
                        //fzItem.GetDeviceSoleCoding = item.GetDeviceSoleCoding;
                        updateItems.Add("ClsCommObj", fzItem.ClsCommObj);
                        updateItems.Add("GetDeviceDetailDtataAddressLst", fzItem.GetDeviceDetailDtataAddressLst);
                        //updateItems.Add("GetSoftwareVersions", fzItem.GetSoftwareVersions);
                        //updateItems.Add("GetHardwareVersions", fzItem.GetHardwareVersions);
                        //updateItems.Add("GetDeviceSoleCoding", fzItem.GetDeviceSoleCoding);
                    }
                    //else if ((fzItem.ClsCommObj.NCommandbz & 0x0100) == 0x00 && item.controlType == 1)
                    else if (item.controlType == 1)
                    {
                        //当前未下发，用户请求下发
                        fzItem.sendDTime = DateTime.Now;
                        fzItem.ClsCommObj.NCommandbz |= 0x0100;
                        //fzItem.GetSoftwareVersions = item.GetSoftwareVersions;
                        //fzItem.GetHardwareVersions = item.GetHardwareVersions;
                        //fzItem.GetDeviceSoleCoding = item.GetDeviceSoleCoding;
                        updateItems.Add("sendDTime", fzItem.sendDTime);
                        updateItems.Add("ClsCommObj", fzItem.ClsCommObj);
                        updateItems.Add("GetDeviceDetailDtataAddressLst", fzItem.GetDeviceDetailDtataAddressLst);
                        //updateItems.Add("GetSoftwareVersions", fzItem.GetSoftwareVersions);
                        //updateItems.Add("GetHardwareVersions", fzItem.GetHardwareVersions);
                        //updateItems.Add("GetDeviceSoleCoding", fzItem.GetDeviceSoleCoding);
                    }
                    if (updateItems.Count > 0)
                    {
                        UpdatePointDefineInfo(fzItem.PointID, updateItems);
                    }
                }
            }
            return new BasicResponse();
        }

        /// <summary>
        /// 修改设备地址号
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BasicResponse ModificationDeviceAdressRequest(DeviceAddressModificationRequest request)
        {
            List<DeviceAddressModificationItem> modificationItems = request.DeviceAddressModificationItems;
            Jc_DefInfo fzItem;
            Dictionary<string, object> updateItems;
            EditDeviceAddressItem editDeviceAddressItem;
            Random random = new Random();
            foreach (DeviceAddressModificationItem item in modificationItems)
            {
                fzItem = GetStationByFzh(item.fzh);
                if (fzItem != null)
                {
                    if (fzItem.ModificationItems == null)
                    {
                        fzItem.ModificationItems = new List<EditDeviceAddressItem>();
                    }
                    editDeviceAddressItem = new EditDeviceAddressItem();
                    editDeviceAddressItem.RandomCode = (byte)random.Next(255);
                    editDeviceAddressItem.DeviceAddressItems = item.DeviceAddressItem;

                    fzItem.ModificationItems.Add(editDeviceAddressItem);
                    fzItem.ClsCommObj.NCommandbz |= 0x0800;
                    updateItems = new Dictionary<string, object>();
                    updateItems.Add("ModificationItems", fzItem.ModificationItems);
                    updateItems.Add("ClsCommObj", fzItem.ClsCommObj);
                    UpdatePointDefineInfo(fzItem.PointID, updateItems);
                }
            }
            return new BasicResponse();
        }

        /// <summary>
        /// 查找所有自动挂接设备缓存
        /// </summary>
        /// <param name="AutomaticArticulatedDeviceCacheRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<AutomaticArticulatedDeviceInfo>> GetAllAutomaticArticulatedDeviceCache(AutomaticArticulatedDeviceCacheGetAllRequest AutomaticArticulatedDeviceCacheRequest)
        {
            var automaticArticulatedDeviceCacheResponse = _automaticArticulatedDeviceCacheService.GetAllAutomaticArticulatedDeviceCache(AutomaticArticulatedDeviceCacheRequest);
            return automaticArticulatedDeviceCacheResponse;
        }

        public BasicResponse<GetSubstationBasicInfoResponse> GetSubstationBasicInfo(GetSubstationBasicInfoRequest request)
        {
            var req = new PointDefineGetByPointRequest
            {
                Point = request.Fzh.PadLeft(3, '0') + "0000"
            };
            var res = GetPointDefineCacheByPoint(req);      //分站缓存信息
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }

            var req2 = new PointDefineCacheGetByStationRequest
            {
                Station = Convert.ToInt16(request.Fzh)
            };
            var res2 = _pointDefineCacheService.GetPointDefineCacheByStation(req2);     //下级设备缓存信息
            if (!res2.IsSuccess)
            {
                throw new Exception(res.Message);
            }

            Jc_DefInfo stationInfo = res.Data;
            GetSubstationBasicInfoResponse getSubstationBasicInfoResponse = null;
            if (stationInfo != null)
            {
                //组织下级设备基础信息
                var inferiorBasicInfo = new List<InferiorBasicInfo>();
                foreach (var item in res2.Data)     //组织传感器数据
                {
                    InferiorBasicInfo basicInfo = new InferiorBasicInfo();
                    basicInfo.PointOrAddr = item.Point;
                    basicInfo.Location = item.Wz;
                    basicInfo.OnlyCoding = item.Bz16;
                    basicInfo.Type = item.DevName;
                    //加载传感器详细信息
                    //SensorInfo sensorInfo = stationInfo.DeviceDetailDtatalstSensor.Find(a => a.SoleCoding == item.Bz13);
                    //if (sensorInfo != null)
                    //{
                    //    basicInfo.ProductionTime = sensorInfo.ProductionTime;
                    //    basicInfo.Voltage = sensorInfo.Voltage;
                    //    basicInfo.RestartNum = sensorInfo.RestartNum;
                    //    basicInfo.AlarmNum = sensorInfo.AlarmNum;
                    //}
                    DateTime productionTime = new DateTime();
                    DateTime.TryParse(item.Bz15, out productionTime);
                    basicInfo.ProductionTime = productionTime;
                    basicInfo.Voltage = item.Voltage.ToString("f1");
                    int restartNum = 0;
                    int.TryParse(item.Bz17, out restartNum);
                    basicInfo.RestartNum = restartNum;
                    int alarmNum = 0;
                    int.TryParse(item.Bz13, out alarmNum);
                    basicInfo.AlarmNum = alarmNum;
                    DateTime timeNow = new DateTime();
                    DateTime.TryParse(item.Bz14, out productionTime);
                    basicInfo.TimeNow = timeNow;
                    inferiorBasicInfo.Add(basicInfo);
                }
                getSubstationBasicInfoResponse = new GetSubstationBasicInfoResponse();
                getSubstationBasicInfoResponse.Location = stationInfo.Wz;
                getSubstationBasicInfoResponse.OnlyCoding = stationInfo.Bz16;
                getSubstationBasicInfoResponse.Type = stationInfo.DevName;
                //if (stationInfo.DeviceDetailDtatalstStation.Count > 0)
                //{
                //    getSubstationBasicInfoResponse.ProductionTime = stationInfo.DeviceDetailDtatalstStation[0].ProductionTime;
                //    getSubstationBasicInfoResponse.Voltage = stationInfo.DeviceDetailDtatalstStation[0].Voltage;
                //    getSubstationBasicInfoResponse.RestartNum = stationInfo.DeviceDetailDtatalstStation[0].RestartNum;
                //    getSubstationBasicInfoResponse.IP = stationInfo.DeviceDetailDtatalstStation[0].IP;
                //    getSubstationBasicInfoResponse.MAC = stationInfo.DeviceDetailDtatalstStation[0].MAC;
                //}
                DateTime productionTime1;
                DateTime.TryParse(stationInfo.Bz15, out productionTime1);
                getSubstationBasicInfoResponse.ProductionTime = productionTime1;
                getSubstationBasicInfoResponse.Voltage = stationInfo.Voltage.ToString("f1");
                int restartNum1 = 0;
                int.TryParse(stationInfo.Bz17, out restartNum1);
                getSubstationBasicInfoResponse.RestartNum = restartNum1;
                getSubstationBasicInfoResponse.IP = stationInfo.Bz13;
                getSubstationBasicInfoResponse.MAC = stationInfo.Bz12;
                getSubstationBasicInfoResponse.InferiorInfo = inferiorBasicInfo;
            }
            var ret = new BasicResponse<GetSubstationBasicInfoResponse>()
            {
                Data = getSubstationBasicInfoResponse
            };
            return ret;
        }

        /// <summary>
        /// 判断控制口是否被用作甲烷风电闭锁控制口或者模开口是否在风电闭锁中使用(true：表示未使用，false：表示已使用)
        /// </summary>
        /// <param name="request"></param>
        /// <returns>(true：表示未使用，false：表示已使用)</returns>
        public BasicResponse<bool> ControlPointLegal(PointDefineGetByPointIDRequest request)
        {
            BasicResponse<bool> response = new BasicResponse<bool>();
            Jc_DefInfo ControlPoint = null;
            BasicResponse<Jc_DefInfo> pointResponse = _pointDefineCacheService.PointDefineCacheByPointIdRequeest(new PointDefineCacheByPointIdRequeest() { PointID = request.PointID });
            if (null != pointResponse && pointResponse.Data != null)
                ControlPoint = pointResponse.Data;

            response.Data = true;
            if (null == ControlPoint)
            {
                return response;
            }
            try
            {
                BasicResponse<List<Jc_DefInfo>> tempStationResponse = GetPointDefineCacheByStationIDDevPropertID(new PointDefineGetByStationIDDevPropertIDRequest() { StationID = ControlPoint.Fzh, DevPropertID = 0 });
                if (tempStationResponse.Data == null)
                    throw new Exception(tempStationResponse.Message);
                IList<Jc_DefInfo> tempStation = tempStationResponse.Data;
                if (null != tempStation)
                {
                    if (tempStation.Count == 1)
                    {
                        if ((tempStation[0].Bz3 & 0x01) == 0x1 || ((tempStation[0].Bz3 >> 1) & 0x1) == 0x1)
                        {
                            if (tempStation[0].Bz9.Contains("&"))
                            {//新风电闭锁，直接判断Bz9中是否包含对应的控制量  20170621
                                //只判断甲烷风电闭锁控制口  20170919
                                string CH4WindBreakControlStr = tempStation[0].Bz9.Split('|')[2].Split('&')[0];
                                if (CH4WindBreakControlStr.Contains(ControlPoint.Point))
                                {
                                    response.Data = false;
                                    return response;
                                }
                            }
                            else if (tempStation[0].Bz10.Length > 0)
                            {
                                if (ControlPoint.DevPropertyID != 3)//模拟量和开关量直接在Bz9字段查找
                                {
                                    if (tempStation[0].Bz9.Contains(ControlPoint.Point))
                                    {
                                        response.Data = false;
                                        return response;
                                    }
                                }
                                else//控制量需要在Bz10字节中获取
                                {
                                    if (tempStation[0].Bz10.Contains(','))
                                    {
                                        string[] ControlChar = tempStation[0].Bz10.Split(',');
                                        if (ControlChar.Length == 33) //长度限制
                                        {
                                            ushort ControlChannel = 0;
                                            if (ControlChar[1] != "0")
                                            {
                                                ControlChannel = Convert.ToByte(ControlChar[1]);
                                            }
                                            if (ControlChar[32] != "0")
                                            {
                                                ControlChannel |= (ushort)(Convert.ToByte(ControlChar[32]) << 8);
                                            }
                                            if (ControlChannel > 0)
                                            {
                                                if ((ControlChannel >> (ControlPoint.Kh - 1) & 0x1) == 0x1)
                                                {
                                                    response.Data = false;
                                                    return response;
                                                }
                                            }

                                            //风电闭锁控制只判断甲烷风电闭锁不判断风电闭锁  20170919
                                            //ControlChannel = 0;
                                            //if (ControlChar[30] != "0")
                                            //{
                                            //    ControlChannel = Convert.ToByte(ControlChar[30]);
                                            //}
                                            //if (ControlChar[31] != "0")
                                            //{
                                            //    ControlChannel |= (ushort)(Convert.ToByte(ControlChar[31]) << 8);
                                            //}
                                            //if (ControlChannel > 0)
                                            //{
                                            //    if ((ControlChannel >> (ControlPoint.Kh - 1) & 0x1) == 0x1)
                                            //    {
                                            //        response.Data = false;
                                            //        return response;
                                            //    }
                                            //}
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return response;
        }
        /// <summary>
        /// 判断控制口是否被用作风电闭锁或者甲烷风电闭锁控制口或者模开口是否在风电闭锁中使用(true：表示未使用，false：表示已使用)
        /// </summary>
        /// <param name="request"></param>
        /// <returns>(true：表示未使用，false：表示已使用)</returns>
        public BasicResponse<bool> ControlPointLegalAll(PointDefineGetByPointIDRequest request)
        {
            BasicResponse<bool> response = new BasicResponse<bool>();
            Jc_DefInfo ControlPoint = null;
            BasicResponse<Jc_DefInfo> pointResponse = _pointDefineCacheService.PointDefineCacheByPointIdRequeest(new PointDefineCacheByPointIdRequeest() { PointID = request.PointID });
            if (null != pointResponse && pointResponse.Data != null)
                ControlPoint = pointResponse.Data;

            response.Data = true;
            if (null == ControlPoint)
            {
                return response;
            }
            try
            {
                BasicResponse<List<Jc_DefInfo>> tempStationResponse = GetPointDefineCacheByStationIDDevPropertID(new PointDefineGetByStationIDDevPropertIDRequest() { StationID = ControlPoint.Fzh, DevPropertID = 0 });
                if (tempStationResponse.Data == null)
                    throw new Exception(tempStationResponse.Message);
                IList<Jc_DefInfo> tempStation = tempStationResponse.Data;
                if (null != tempStation)
                {
                    if (tempStation.Count == 1)
                    {
                        if ((tempStation[0].Bz3 & 0x01) == 0x1 || ((tempStation[0].Bz3 >> 1) & 0x1) == 0x1)
                        {
                            if (tempStation[0].Bz9.Contains("&"))
                            {//新风电闭锁，直接判断Bz9中是否包含对应的控制量  20170621                                
                                if (tempStation[0].Bz9.Contains(ControlPoint.Point))
                                {
                                    response.Data = false;
                                    return response;
                                }
                            }
                            else if (tempStation[0].Bz10.Length > 0)
                            {
                                if (ControlPoint.DevPropertyID != 3)//模拟量和开关量直接在Bz9字段查找
                                {
                                    if (tempStation[0].Bz9.Contains(ControlPoint.Point))
                                    {
                                        response.Data = false;
                                        return response;
                                    }
                                }
                                else//控制量需要在Bz10字节中获取
                                {
                                    if (tempStation[0].Bz10.Contains(','))
                                    {
                                        string[] ControlChar = tempStation[0].Bz10.Split(',');
                                        if (ControlChar.Length == 33) //长度限制
                                        {
                                            ushort ControlChannel = 0;
                                            if (ControlChar[1] != "0")
                                            {
                                                ControlChannel = Convert.ToByte(ControlChar[1]);
                                            }
                                            if (ControlChar[32] != "0")
                                            {
                                                ControlChannel |= (ushort)(Convert.ToByte(ControlChar[32]) << 8);
                                            }
                                            if (ControlChannel > 0)
                                            {
                                                if ((ControlChannel >> (ControlPoint.Kh - 1) & 0x1) == 0x1)
                                                {
                                                    response.Data = false;
                                                    return response;
                                                }
                                            }

                                            ControlChannel = 0;
                                            if (ControlChar[30] != "0")
                                            {
                                                ControlChannel = Convert.ToByte(ControlChar[30]);
                                            }
                                            if (ControlChar[31] != "0")
                                            {
                                                ControlChannel |= (ushort)(Convert.ToByte(ControlChar[31]) << 8);
                                            }
                                            if (ControlChannel > 0)
                                            {
                                                if ((ControlChannel >> (ControlPoint.Kh - 1) & 0x1) == 0x1)
                                                {
                                                    response.Data = false;
                                                    return response;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return response;
        }
        #region ----辅助方法----

        /// <summary>
        /// 根据分站号获取分站结构体
        /// </summary>
        /// <param name="fzh"></param>
        /// <returns></returns>
        private Jc_DefInfo GetStationByFzh(ushort fzh)
        {
            Jc_DefInfo fzItem = null;

            var req = new PointDefineCacheGetByConditonRequest
            {
                Predicate = a => fzh == a.Fzh && 0 == a.Kh && 0 == a.Dzh && 0 == a.DevPropertyID && a.Activity == "1" && a.InfoState != InfoState.Delete
            };
            var res = _pointDefineCacheService.GetPointDefineCache(req);
            if (res.Data != null && res.IsSuccess)
            {
                fzItem = res.Data.FirstOrDefault();
            }

            return fzItem;
        }
        ///// <summary>
        ///// 下发分站获取电源箱数据命令
        ///// </summary>
        ///// <param name="fzh"></param>
        //private void SendStationQueryBatteryRealDataComm(ushort fzh, BatteryControlItem item)
        //{
        //    #region ----分站下发D命令----
        //    Jc_DefInfo fzItem = GetStationByFzh(fzh);
        //    Dictionary<string, object> updateItems = new Dictionary<string, object>();
        //    if (fzItem != null)
        //    {
        //        fzItem.sendDTime = DateTime.Now;
        //        //updateItems.Add("sendDTime", fzItem.sendDTime);
        //        //if ((fzItem.ClsCommObj.NCommandbz & 0x02) == 0x02 && item.controlType !=2)
        //        //{
        //        //    //之前在动作，现在要取消              
        //        //    fzItem.ClsCommObj.NCommandbz &= (0xFFFF - 0x02);
        //        //    updateItems.Add("ClsCommObj", fzItem.ClsCommObj);
        //        //}
        //        //else if ((fzItem.ClsCommObj.NCommandbz & 0x02) == 0x00 && item.controlType == 2)
        //        //{
        //        //    //之前无动作，现在要执行 
        //        //    fzItem.ClsCommObj.NCommandbz |= 0x02;
        //        //    updateItems.Add("ClsCommObj", fzItem.ClsCommObj);
        //        //}

        //        fzItem.ClsCommObj.NCommandbz |= 0x0002;
        //        updateItems.Add("sendDTime", fzItem.sendDTime);
        //        updateItems.Add("BDisCharge", item.controlType);
        //        updateItems.Add("ClsCommObj", fzItem.ClsCommObj);

        //        if (updateItems.Count > 0)
        //        {
        //            UpdatePointDefineInfo(fzItem.PointID, updateItems);
        //        }

        //    }

        //    #endregion
        //}

        private void SendSwitchesQueryBatteryRealDataComm(string mac, BatteryControlItem item)
        {
            NetworkModuleCacheGetByKeyRequest networkModuleCacheGetByKeyRequest = new NetworkModuleCacheGetByKeyRequest();
            networkModuleCacheGetByKeyRequest.Mac = mac;
            var reslut = _networkModuleCacheService.GetNetworkModuleCacheByKey(networkModuleCacheGetByKeyRequest);
            if (reslut.IsSuccess && reslut != null && reslut.Data != null)
            {
                Jc_MacInfo macInfo = reslut.Data;
                NetworkModuleCacheUpdateNCommandRequest networkModuleCacheUpdateNCommandRequest = new NetworkModuleCacheUpdateNCommandRequest();
                if (macInfo != null)
                {
                    //if ((macInfo.NCommandbz & 0x01) == 0x01 && item.controlType == 0)
                    //{
                    //    //之前在动作，现在要取消 
                    //    macInfo.NCommandbz &= (0xFFFF - 0x01);
                    //    macInfo.SendBatteryControlCount++;
                    //    networkModuleCacheUpdateNCommandRequest.NetWorkModuleInfo = macInfo;
                    //    _networkModuleCacheService.UpdateNetworkModuleNCommand(networkModuleCacheUpdateNCommandRequest);
                    //}
                    //else if ((macInfo.NCommandbz & 0x01) == 0x00 && item.controlType == 1)
                    //{
                    //    //之前无动作，现在要执行 
                    //    macInfo.NCommandbz |= 0x01;
                    //    macInfo.SendBatteryControlCount++;
                    //    networkModuleCacheUpdateNCommandRequest.NetWorkModuleInfo = macInfo;
                    //    _networkModuleCacheService.UpdateNetworkModuleNCommand(networkModuleCacheUpdateNCommandRequest);
                    //}
                    //之前无动作，现在要执行 
                    macInfo.NCommandbz |= 0x01;
                    macInfo.SendDtime = DateTime.Now;
                    macInfo.BatteryControl = 0;//刷新置0
                    macInfo.SendBatteryControlCount++;
                    networkModuleCacheUpdateNCommandRequest.NetWorkModuleInfo = macInfo;
                    _networkModuleCacheService.UpdateNetworkModuleNCommand(networkModuleCacheUpdateNCommandRequest);
                }
            }
        }

        private void UpdatePointDefineInfo(string pointID, Dictionary<string, object> updateItems)
        {
            DefineCacheUpdatePropertiesRequest defineCacheUpdatePropertiesRequest = new DefineCacheUpdatePropertiesRequest();
            defineCacheUpdatePropertiesRequest.PointID = pointID;
            defineCacheUpdatePropertiesRequest.UpdateItems = updateItems;
            _pointDefineCacheService.UpdatePointDefineInfo(defineCacheUpdatePropertiesRequest);
        }
        #endregion

        public BasicResponse<int> QueryRealLinkageInfoFromMonitor(GetRealLinkageInfoRequest request)
        {
            BasicResponse<int> response = new BasicResponse<int>();
            int realRecordID = request.recordId;
            try
            {
                if (Basic.Framework.Configuration.ConfigurationManager.FileConfiguration.GetString("EmergencyLinkageEnable", "") == "1")
                {
                    //1   连接字符串
                    string connectionString
                        = Basic.Framework.Configuration.ConfigurationManager.FileConfiguration.GetString("EmergencyLinkageDbConnectionString", "");
                    //2 实例化数据库连接
                    using (System.Data.SqlClient.SqlConnection connection = new SqlConnection(connectionString))
                    {
                        //定义执行SQL语句,可以为select查询,也可以为存储过程,我们要的只是返回的结果集.
                        string sql = "select * from RealdataYuan where RecordID >" + request.recordId + " and (RState='0' or RState='11') order by RecordID";

                        //强大的SqlDataAdapter 
                        System.Data.SqlClient.SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);

                        DataSet ds = new DataSet();
                        //Fill 方法会执行一系列操作 connection.open command.reader 等等
                        //反正到最后就把 sql语句执行一遍,然后把结果集插入到 ds 里.
                        adapter.Fill(ds);

                        DataTable dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            realRecordID = int.Parse(dt.Rows[0]["RecordID"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            response.Data = realRecordID;
            return response;
        }
    }
}


