using System;
using System.Collections.Generic;
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
using Sys.Safety.Request.Listex;
using Sys.Safety.ServiceContract.KJ237Cache;
using Sys.Safety.Request.PersonCache;
using Sys.Safety.Request.R_Restrictedperson;
using Sys.Safety.Enums;
using Sys.Safety.Request.UndefinedDef;
using System.Data;

namespace Sys.Safety.Services
{
    /// <summary>
    /// 测点定义服务(广播系统)
    /// </summary>
    public partial class BroadCastPointDefineService : IBroadCastPointDefineService
    {
        private IB_DefRepository _repository;
        /// <summary>
        /// 测点定义缓存接口
        /// </summary>
        private IB_DefCacheService _b_DefCacheService;
        /// <summary>
        /// 分区缓存 
        /// </summary>
        private IAreaCacheService _areaCacheService;
        /// <summary>
        /// 分区服务
        /// </summary>
        private IAreaRepository _areaRepository;
        /// <summary>
        /// 位置缓存
        /// </summary>
        private IPositionCacheService _positionCacheService;
        /// <summary>
        /// 位置服务
        /// </summary>
        private IPositionService _positionService;
        /// <summary>
        /// 配置服务
        /// </summary>
        private IConfigRepository _configRepository;
        /// <summary>
        /// 配置缓存
        /// </summary>
        private IConfigCacheService _configCacheService;
        /// <summary>
        /// 设备类型缓存服务
        /// </summary>
        private IDeviceDefineCacheService _devCacheService;

        public BroadCastPointDefineService(IB_DefRepository _Bepository,
            IB_DefCacheService _B_DefCacheService,
            IAreaCacheService _AreaCacheService,
            IAreaRepository _AreaRepository,
            IPositionCacheService _PositionCacheService,
            IPositionService _PositionService,
            IConfigCacheService _ConfigCacheService,
            IConfigRepository _ConfigRepository,
            IDeviceDefineCacheService DevCacheService)
        {
            this._repository = _Bepository;
            this._b_DefCacheService = _B_DefCacheService;
            this._areaCacheService = _AreaCacheService;
            this._areaRepository = _AreaRepository;
            this._positionCacheService = _PositionCacheService;
            this._positionService = _PositionService;
            this._configCacheService = _ConfigCacheService;
            this._configRepository = _ConfigRepository;
            this._devCacheService = DevCacheService;
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

            Jc_DefInfo olditem = null;

            B_DefCacheGetByConditionRequest bDefCacheRequest = new B_DefCacheGetByConditionRequest();
            bDefCacheRequest.predicate = a => a.Point == item.Point && a.Activity == "1";
            var result = _b_DefCacheService.Get(bDefCacheRequest);
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
            var _jc_Def = ObjectConverter.Copy<Jc_DefInfo, B_DefModel>(item);
            var resultjc_Def = _repository.AddDef(_jc_Def);

            //置初始化标记及休眠处理
            PointInitializes(item, olditem);

            //添加到缓存
            B_DefCacheInsertRequest addbDefCacheRequest = new B_DefCacheInsertRequest();
            addbDefCacheRequest.B_DefInfo = item;
            _b_DefCacheService.Insert(addbDefCacheRequest);

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

            B_DefCacheGetByConditionRequest bDefCacheRequest = new B_DefCacheGetByConditionRequest();
            bDefCacheRequest.predicate = a => a.Activity == "1";
            var result = _b_DefCacheService.Get(bDefCacheRequest);
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

                        //保存数据库
                        var _jc_Def = ObjectConverter.Copy<Jc_DefInfo, B_DefModel>(item);
                        var resultjc_Def = _repository.AddDef(_jc_Def);

                        //置下发初始化标记
                        PointInitializes(item, olditem);
                    }
                    //添加到缓存
                    B_DefCacheBatchInsertRequest batchInsertbDefCacheRequest = new B_DefCacheBatchInsertRequest();
                    batchInsertbDefCacheRequest.B_DefInfos = items;
                    _b_DefCacheService.BatchInsert(batchInsertbDefCacheRequest);

                });
            return Result;
        }


        private void PointInitializes(Jc_DefInfo item, Jc_DefInfo olditem)
        {
            //置是否需要下发初始化标记
            //if (item.InfoState == InfoState.AddNew || item.Activity == "0")
            //{
            //    item.DefIsInit = true;//新增加测点，都发初始化
            //}
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

            B_DefCacheGetByConditionRequest bDefCacheRequest = new B_DefCacheGetByConditionRequest();
            bDefCacheRequest.predicate = a => a.Point == item.Point && a.Activity == "1";
            var result = _b_DefCacheService.Get(bDefCacheRequest);
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
            var _jc_Def = ObjectConverter.Copy<Jc_DefInfo, B_DefModel>(item);
            _repository.UpdateDef(_jc_Def);

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
            paramater.Add("Bz13", Point.Bz13);//条件呼叫转移号码
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

            //定义改变标记
            paramater.Add("PointEditState", Point.PointEditState);
            paramater.Add("DefIsInit", Point.DefIsInit);
            paramater.Add("kzchangeflag", Point.kzchangeflag);
            paramater.Add("ReDoDeal", Point.ReDoDeal);
            paramater.Add("Dormancyflag", Point.Dormancyflag);

            //修改标记
            paramater.Add("InfoState", Point.InfoState);


            UpdatePropertiesRequest bDefCacheRequest = new UpdatePropertiesRequest();
            bDefCacheRequest.PointID = Point.PointID;
            bDefCacheRequest.UpdateItems = paramater;
            _b_DefCacheService.UpdateInfo(bDefCacheRequest);

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

            B_DefCacheGetByConditionRequest bDefCacheRequest = new B_DefCacheGetByConditionRequest();
            bDefCacheRequest.predicate = a => a.Activity == "1";
            var result = _b_DefCacheService.Get(bDefCacheRequest);
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
                    var _jc_Def = ObjectConverter.Copy<Jc_DefInfo, B_DefModel>(item);
                    _repository.UpdateDef(_jc_Def);

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
            B_DefCacheBatchUpdateRequest UpdatePointDefineCacheRequest = new B_DefCacheBatchUpdateRequest();
            UpdatePointDefineCacheRequest.B_DefInfos = items;
            _b_DefCacheService.BatchUpdate(UpdatePointDefineCacheRequest);
            return Result;
        }
        /// <summary>
        /// 批量更新属性
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BasicResponse BatchUpdatePointDefineInfo(DefineCacheBatchUpdatePropertiesRequest request)
        {
            BatchUpdatePropertiesRequest UpdatePointDefineCacheRequest = new BatchUpdatePropertiesRequest();
            UpdatePointDefineCacheRequest.PointItems = request.PointItems;
            return _b_DefCacheService.BatchUpdateInfo(UpdatePointDefineCacheRequest);
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

            UpdatePropertiesRequest defineCacheUpdatePropertiesRequest = new UpdatePropertiesRequest();
            defineCacheUpdatePropertiesRequest.PointID = Point.PointID;
            defineCacheUpdatePropertiesRequest.UpdateItems = paramater;
            _b_DefCacheService.UpdateInfo(defineCacheUpdatePropertiesRequest);
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
            foreach (Jc_DefInfo deviceInfo in SendItemList)
            {
                TerminalControlRequest terminalControlRequest = new TerminalControlRequest();
                //查找分区标识
                AreaCacheGetByKeyRequest AreaCacheRequest = new AreaCacheGetByKeyRequest();
                AreaCacheRequest.Areaid = deviceInfo.Areaid;
                AreaInfo areaInfo = _areaCacheService.GetByKeyAreaCache(AreaCacheRequest).Data;
                if (areaInfo != null)
                {
                    terminalControlRequest.zoneId = areaInfo.Areadescribe;//分区标识
                }
                else
                {
                    Basic.Framework.Logging.LogHelper.Error("未找到广播设备对应的分区信息，设备标识：" + deviceInfo.Point);
                    return false;
                }
                terminalControlRequest.termDN = deviceInfo.Point;//终端号码
                terminalControlRequest.type = deviceInfo.Bz6;//终端类型
                terminalControlRequest.name = deviceInfo.Wz;//终端名称
                terminalControlRequest.record = deviceInfo.Bz7;//录音使能是否启用
                terminalControlRequest.auth = deviceInfo.Bz8;//注册鉴权是否启用
                terminalControlRequest.password = deviceInfo.Bz9;//注册鉴权密码
                terminalControlRequest.pa = deviceInfo.Bz11;//广播使能是否启用
                terminalControlRequest.cfuDN = deviceInfo.Bz12;//始终呼叫转移号码
                terminalControlRequest.cfxDN = deviceInfo.Bz13;//条件呼叫转移号码
                terminalControlRequest.InfoState = deviceInfo.InfoState;
                //调用RPC发送
                MasProtocol masProtocol = new MasProtocol(SystemType.Broadcast, DirectionType.Down, ProtocolType.TerminalControlRequest);
                masProtocol.Protocol = terminalControlRequest;
                TerminalControlResponse result = RpcService.Send<TerminalControlResponse>(masProtocol, RequestType.BusinessRequest);

                if (result == null && result.retCode != "0")
                {
                    Basic.Framework.Logging.LogHelper.Error("向网关同步广播设备信息失败！,设备标识：" + deviceInfo.Point);
                    return false;
                }

            }
            return true;
        }

        public BasicResponse PointDefineSaveData()
        {
            BasicResponse Result = new BasicResponse();

            //修改，根据保存时，置的初始化标记来置初始化标记
            B_DefCacheGetAllRequest pointDefineCacheRequest = new B_DefCacheGetAllRequest();
            var result = _b_DefCacheService.GetAll(pointDefineCacheRequest);
            List<Jc_DefInfo> newItems = result.Data.Where(a => a.DefIsInit).ToList();
            //保存巡检时，如果设备置了休眠的标记，则将缓存的Bz4更新  20170705
            newItems.ForEach(a =>
            {
                if (a.Dormancyflag)
                {
                    a.Bz4 |= 0x02;
                    Dictionary<string, object> paramater = new Dictionary<string, object>();
                    paramater.Add("Bz4", a.Bz4);
                    UpdatePropertiesRequest defineCacheUpdatePropertiesRequest = new UpdatePropertiesRequest();
                    defineCacheUpdatePropertiesRequest.PointID = a.PointID;
                    defineCacheUpdatePropertiesRequest.UpdateItems = paramater;
                    _b_DefCacheService.UpdateInfo(defineCacheUpdatePropertiesRequest);
                }
            });
            IEnumerable<IGrouping<short, Jc_DefInfo>> groupFz = newItems.GroupBy(p => p.Fzh);

            ////驱动预处理
            //foreach (IGrouping<short, Jc_DefInfo> info in groupFz)
            //{
            //    //调用驱动处理
            //    Drv_LoadDef(info.ToList<Jc_DefInfo>());
            //}

            ////批量更新到缓存
            //result = _rPointDefineCacheService.GetAllRPointDefineCache(pointDefineCacheRequest);
            //PointDefineCacheBatchUpdateRequest UpdatePointDefineCacheRequest = new PointDefineCacheBatchUpdateRequest();
            ////保存巡检后将所有定义初始化判断标记置成false
            //Dictionary<string, Dictionary<string, object>> updateItemsList = new Dictionary<string, Dictionary<string, object>>();
            //Dictionary<string, object> updateItems = new Dictionary<string, object>();
            //List<Jc_DefInfo> items = result.Data;
            //foreach (Jc_DefInfo item in items)
            //{
            //    //if (item.DefIsInit)//交叉控制口变化要置kzchangeflag，但不会下发初始化，加上此条件会导致kzchangeflag一直为true
            //    //{
            //    item.DefIsInit = false;
            //    item.kzchangeflag = false;
            //    item.Dormancyflag = false;
            //    //}
            //    updateItems = new Dictionary<string, object>();
            //    updateItems.Add("DefIsInit", false);
            //    updateItems.Add("kzchangeflag", false);
            //    updateItems.Add("Dormancyflag", false);

            //    updateItemsList.Add(item.PointID, updateItems);
            //}
            //RDefineCacheBatchUpdatePropertiesRequest cacheUpdatePropertiesRequest = new RDefineCacheBatchUpdatePropertiesRequest();
            //cacheUpdatePropertiesRequest.PointItems = updateItemsList;
            //_rPointDefineCacheService.BatchUpdatePointDefineInfo(cacheUpdatePropertiesRequest);

            //删除缓存中的非活动点
            List<Jc_DefInfo> NonActivityList = result.Data.FindAll(a => a.Activity == "0" && a.InfoState == InfoState.Modified);
            B_DefCacheBatchDeleteRequest DeletePointDefineCacheRequest = new B_DefCacheBatchDeleteRequest();
            DeletePointDefineCacheRequest.B_DefInfos = NonActivityList;
            _b_DefCacheService.BatchDelete(DeletePointDefineCacheRequest);


            return Result;
        }
        /// <summary>
        /// 广播系统不需要调用驱动预处理
        /// </summary>
        /// <param name="lstPoints"></param>
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
            B_DefCacheGetAllRequest bDefCacheRequest = new B_DefCacheGetAllRequest();
            var resultpersondef = _b_DefCacheService.GetAll(bDefCacheRequest);
            Result.Data = resultpersondef.Data.FindAll(a => a.Activity == "1");
            return Result;
        }

        /// <summary>
        /// 根据区域ID获取设备
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByAreaID(PointDefineGetByAreaIDRequest PointDefineRequest)
        {
            BasicResponse<List<Jc_DefInfo>> response = new BasicResponse<List<Jc_DefInfo>>();

            B_DefCacheGetByConditionRequest rdergetrequest = new B_DefCacheGetByConditionRequest();
            rdergetrequest.predicate = r => r.Areaid == PointDefineRequest.AreaId;
            response.Data = _b_DefCacheService.Get(rdergetrequest).Data;

            return response;
        }


        public BasicResponse<Jc_DefInfo> GetPointDefineCacheByPointID(PointDefineGetByPointIDRequest PointDefineRequest)
        {
            BasicResponse<Jc_DefInfo> response = new BasicResponse<Jc_DefInfo>();

            B_DefCacheGetByConditionRequest rdergetrequest = new B_DefCacheGetByConditionRequest();
            rdergetrequest.predicate = r => r.PointID == PointDefineRequest.PointID;
            response.Data = _b_DefCacheService.Get(rdergetrequest).Data.FirstOrDefault();

            return response;
        }

        /// <summary>
        /// 同步老广播系统测点定义
        /// </summary>
        /// <returns></returns>
        public BasicResponse<bool> SynchronousPoint(SynchronousPointRequest PointDefineRequest)
        {
            BasicResponse<bool> response = new BasicResponse<bool>();
            try
            {

                bool isupdate = false;

                if (PointDefineRequest.Points != null)
                {
                    var points = _b_DefCacheService.Get(new B_DefCacheGetByConditionRequest
                    {
                        predicate = b => b.Sysid
                            == (int)SystemEnum.Broadcast
                    }).Data;
                    bool isupdatedb = false;//是否更新数据库，如果只有状态发生改变，则只更新缓存数据；反之需要更新数据库
                    foreach (DataRow row in PointDefineRequest.Points.Rows)
                    {
                        var point = row["AddNum"].ToString();
                        //如果存在则比较更新测点信息；如果不存在则插入测点信息
                        if (points.FirstOrDefault(p => p.Point == point) != null)
                        {
                            Dictionary<string, object> paramater = new Dictionary<string, object>();
                            var definfo = points.FirstOrDefault(p => p.Point == point);
                            //比较位置是否一致
                            if (definfo.Wz != row["Wz"].ToString())
                            {
                                isupdatedb = true;
                                definfo.Wzid = GetPositionId(row["Wz"].ToString());
                                definfo.Wz = row["Wz"].ToString();
                                paramater.Add("Wzid", GetPositionId(row["Wz"].ToString()));
                                paramater.Add("Wz", row["Wz"].ToString());
                            }
                            //比较区域是否一致
                            // 20180606
                            //var zoneId = row["ZoneId"];
                            if (row.Table.Columns.Contains("ZoneId") && row["ZoneId"] != null && row["ZoneId"].ToString() != "")
                            {
                                var req = new AreaCacheGetByConditionRequest();
                                req.Predicate = a => a.Areaid == definfo.Areaid;
                                var res = _areaCacheService.GetAreaCache(req);
                                var areaInfo = res.Data[0];
                                if (areaInfo.Areaname != row["Groupname"].ToString() || areaInfo.Areadescribe != row["ZoneId"].ToString())
                                {
                                    isupdatedb = true;
                                    definfo.Areaid = GetAreaId(row["Groupname"].ToString(), row["ZoneId"].ToString());
                                    definfo.AreaName = row["Groupname"].ToString();
                                    paramater.Add("Areaid", GetAreaId(row["Groupname"].ToString(), row["ZoneId"].ToString()));
                                    paramater.Add("AreaName", row["Groupname"].ToString());
                                    paramater.Add("Areadescribe", row["ZoneId"].ToString());
                                }
                            }
                            else
                            {
                                if (definfo.AreaName != row["Groupname"].ToString())
                                {
                                    isupdatedb = true;
                                    definfo.Areaid = GetAreaId(row["Groupname"].ToString());
                                    definfo.AreaName = row["Groupname"].ToString();
                                    paramater.Add("Areaid", GetAreaId(row["Groupname"].ToString()));
                                    paramater.Add("AreaName", row["Groupname"].ToString());
                                }
                            }

                            //数据状态与设备状态不做比较，直接更新
                            var state = row["State"].ToString();
                            if (state == "0")
                            {
                                definfo.DataState = 3;
                                definfo.State = 3;

                                paramater.Add("DataState", 3);
                                paramater.Add("State", 3);
                                paramater.Add("Ssz", EnumHelper.GetEnumDescription(DeviceDataState.EquipmentAC));
                            }
                            else if (state == "1")
                            {
                                definfo.DataState = 49;
                                definfo.State = 3;

                                paramater.Add("DataState", 49);
                                paramater.Add("State", 3);
                                paramater.Add("Ssz", EnumHelper.GetEnumDescription(DeviceDataState.BroadCastInCall));
                            }
                            else if (state == "2")
                            {
                                definfo.DataState = 0;
                                definfo.State = 0;

                                paramater.Add("DataState", 0);
                                paramater.Add("State", 0);
                                paramater.Add("Ssz", EnumHelper.GetEnumDescription(DeviceDataState.EquipmentInterrupted));
                            }
                            //比较Mac是否一致
                            if (definfo.Jckz1 != row["Mac"].ToString())
                            {
                                isupdatedb = true;
                                definfo.Jckz1 = row["Mac"].ToString();
                                paramater.Add("Jckz1", row["Mac"].ToString());
                            }
                            //比较IP是否一致
                            if (definfo.Jckz2 != row["IP"].ToString())
                            {
                                isupdatedb = true;
                                definfo.Jckz2 = row["IP"].ToString();
                                paramater.Add("Jckz2", row["IP"].ToString());
                            }
                            //比较设备类型是否一致
                            if (definfo.Bz6 != row["Type"].ToString())
                            {
                                isupdatedb = true;
                                definfo.Bz6 = row["Type"].ToString();
                                paramater.Add("Bz6", row["Type"].ToString());
                            }

                            //更新数据库
                            if (isupdatedb)
                            {
                                isupdate = true;
                                var _jc_Def = ObjectConverter.Copy<Jc_DefInfo, B_DefModel>(definfo);
                                _repository.UpdateDef(_jc_Def);
                            }

                            //更新数据更新时间，每次调用都更新时间  20180422
                            paramater.Add("DttStateTime", DateTime.Now);
                            //更新缓存
                            if (paramater.Count > 0)
                            {

                                UpdatePropertiesRequest updaterequest = new UpdatePropertiesRequest();
                                updaterequest.PointID = definfo.PointID;
                                updaterequest.UpdateItems = paramater;
                                _b_DefCacheService.UpdateInfo(updaterequest);
                            }
                        }
                        else
                        {
                            var bdev = _devCacheService.GetPointDefineCache(new DeviceDefineCacheGetByConditonRequest
                            {
                                Predicate = dev => dev.Name == "广播主机"
                            }).Data.FirstOrDefault();

                            if (bdev != null)
                            {
                                Jc_DefInfo bdefinfo = new Jc_DefInfo();
                                bdefinfo.ID = bdefinfo.PointID = IdHelper.CreateLongId().ToString();
                                bdefinfo.Wzid = GetPositionId(row["Wz"].ToString());
                                bdefinfo.Wz = row["Wz"].ToString();

                                // 20180606
                                //var zoneId = row["ZoneId"];
                                if (row.Table.Columns.Contains("ZoneId") && row["ZoneId"] != null && row["ZoneId"].ToString() != "")
                                {
                                    bdefinfo.Areaid = GetAreaId(row["Groupname"].ToString(), row["ZoneId"].ToString());
                                    bdefinfo.AreaName = row["Groupname"].ToString();
                                }
                                else
                                {
                                    bdefinfo.Areaid = GetAreaId(row["Groupname"].ToString());
                                    bdefinfo.AreaName = row["Groupname"].ToString();
                                }

                                bdefinfo.Point = point;
                                bdefinfo.Upflag = "1";//子系统同步测点
                                bdefinfo.Jckz1 = row["Mac"].ToString();
                                bdefinfo.Jckz2 = row["IP"].ToString();
                                bdefinfo.Bz6 = row["Type"].ToString();
                                bdefinfo.CreateUpdateTime = DateTime.Now;
                                bdefinfo.DeleteTime = DateTime.Parse("1900-01-01 00:00:00");
                                bdefinfo.Activity = "1";
                                bdefinfo.InfoState = InfoState.AddNew;
                                bdefinfo.DevPropertyID = 0;
                                bdefinfo.DevProperty = bdev.DevProperty;
                                bdefinfo.Devid = bdev.Devid;
                                bdefinfo.DevName = bdev.Name;
                                bdefinfo.Sysid = (int)SystemEnum.Broadcast;
                                bdefinfo.DttStateTime = DateTime.Now;

                                var bdefinfos = _b_DefCacheService.Get(new B_DefCacheGetByConditionRequest
                                {
                                    predicate = b => b.Sysid
                                        == (int)SystemEnum.Broadcast
                                }).Data;

                                //if (bdefinfos.Count == 0)
                                //{
                                //    bdefinfo.Fzh = 256;
                                //}
                                //else
                                //{
                                //    int maxstationnum = bdefinfos.Max(o => o.Fzh);
                                //    bdefinfo.Fzh = (short)(maxstationnum + 1);
                                //}

                                // 20180605
                                //生成分站号
                                var req = new B_DefCacheGetAllRequest();
                                var res = _b_DefCacheService.GetAll(req);
                                var broadcastPoints = res.Data;
                                short fzh = 0;
                                for (int i = 257; i < 513; i++)
                                {
                                    var exist = broadcastPoints.Any(a => a.Fzh == i);
                                    if (!exist)
                                    {
                                        fzh = (short)i;
                                        break;
                                    }
                                }

                                //var fzh = Convert.ToInt32(point) + 256;
                                if (fzh == 0)
                                {
                                    var ret = new BasicResponse<bool>();
                                    ret.Code = 101;
                                    ret.Data = false;
                                    ret.Message = "广播设备对应分站号已满。";
                                    return ret;
                                }
                                bdefinfo.Fzh = fzh;

                                isupdate = true;
                                //添加数据库
                                var _jc_Def = ObjectConverter.Copy<Jc_DefInfo, B_DefModel>(bdefinfo);
                                var resultjc_Def = _repository.AddDef(_jc_Def);
                                //更新缓存
                                B_DefCacheInsertRequest addbDefCacheRequest = new B_DefCacheInsertRequest();
                                addbDefCacheRequest.B_DefInfo = bdefinfo;
                                _b_DefCacheService.Insert(addbDefCacheRequest);
                            }
                        }
                    }

                    //判断同步数据是否有删除的测点信息，如果有则同步删除
                    for (int i = points.Count - 1; i >= 0; i--)
                    {
                        var pointrow = PointDefineRequest.Points.Select("AddNum = '" + points[i].Point + "'");
                        if (pointrow.Length == 0)
                        {
                            isupdate = true;
                            //删除数据库
                            _repository.Delete(points[i].ID);
                            //删除缓存
                            _b_DefCacheService.Delete(new B_DefCacheDeleteRequest { B_DefInfo = points[i] });
                        }
                    }
                }

                if (isupdate)
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

                response.Data = true;
                return response;
            }
            catch (Exception ex)
            {
                LogHelper.Error("广播同步定义信息出错！" + ex.Message);
                response.Data = true;
                return response;
            }
        }

        public BasicResponse BroadcastSysPointSync(BroadcastSysPointSyncRequest request)
        {
            try
            {
                if (request.Points == null)
                {
                    return new BasicResponse();
                }

                var pointInfoDt = new DataTable();
                pointInfoDt.Columns.Add("AddNum");
                pointInfoDt.Columns.Add("Wz");
                pointInfoDt.Columns.Add("Groupname");
                pointInfoDt.Columns.Add("ZoneId");
                pointInfoDt.Columns.Add("State");
                pointInfoDt.Columns.Add("Mac");
                pointInfoDt.Columns.Add("IP");
                pointInfoDt.Columns.Add("Type");

                if (request == null || request.Points == null)
                {
                    LogHelper.Error("广播通信子系统同步失败,没有找到测点对象！！");
                    return new BasicResponse()
                    {
                        Code = 101,
                        Message = "广播通信子系统同步失败,没有找到测点对象！！"
                    };
                }

                foreach (var item in request.Points)
                {
                    //var info = item as Newtonsoft.Json.Linq.JObject;
                    //var addNumPro = info.Property("AddNum");
                    //var addNum = addNumPro == null ? "" : addNumPro.Value.ToString();
                    //var wzPro = info.Property("Wz");
                    //var wz = wzPro == null ? "" : wzPro.Value.ToString();
                    //var groupnamePro = info.Property("Groupname");
                    //var groupname = groupnamePro == null ? "" : groupnamePro.Value.ToString();
                    //var zoneIdPro = info.Property("ZoneId");
                    //var zoneId = zoneIdPro == null ? "" : zoneIdPro.Value.ToString();
                    //var statePro = info.Property("State");
                    //var state = statePro == null ? "" : statePro.Value.ToString();
                    //var macPro = info.Property("Mac");
                    //var mac = macPro == null ? "" : macPro.Value.ToString();
                    //var ipPro = info.Property("IP");
                    //var ip = ipPro == null ? "" : ipPro.Value.ToString();
                    //var typePro = info.Property("Type");
                    //var type = typePro == null ? "" : typePro.Value.ToString();
                    //pointInfoDt.Rows.Add(addNum, wz, groupname, zoneId, state, mac, ip, type);

                    pointInfoDt.Rows.Add(item.AddNum, item.Wz, item.Groupname, item.ZoneId, item.State, item.Mac, item.IP, item.Type);
                }

                var req = new SynchronousPointRequest
                {
                    Points = pointInfoDt
                };
                var res = SynchronousPoint(req);
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

        /// <summary>
        /// 获取位置ID，如果不存在则添加位置
        /// </summary>
        /// <param name="psition"></param>
        /// <returns></returns>
        private string GetPositionId(string position)
        {
            if (string.IsNullOrEmpty(position))
                return "0";

            string wzid = string.Empty;
            var positioninfo = _positionCacheService.GetPositionCache(new PositionCacheGetByConditionRequest { Predicate = w => w.Wz == position }).Data.FirstOrDefault();

            if (positioninfo == null)
            {
                Jc_WzInfo wzinfo = new Jc_WzInfo();

                wzinfo.ID = IdHelper.CreateLongId().ToString();
                var maxid = _positionService.GetMaxPositionId().Data;
                wzid = (maxid + 1).ToString();
                wzinfo.WzID = (maxid + 1).ToString();
                //var id = IdHelper.CreateLongId().ToString();
                //wzid = id;
                //wzinfo.WzID = id;
                wzinfo.Wz = position;
                wzinfo.CreateTime = DateTime.Now;

                _positionService.AddPosition(new Request.Position.PositionAddRequest { PositionInfo = wzinfo });
            }
            else
            {
                wzid = positioninfo.WzID;
            }

            return wzid;
        }

        /// <summary>
        /// 获取区域ID，如果不存在则添加区域
        /// </summary>
        /// <param name="areaname"></param>
        /// <param name="areadescribe"></param>
        /// <returns></returns>
        private string GetAreaId(string areaname, string areadescribe = null)
        {
            if (string.IsNullOrEmpty(areaname))
                return "0";

            string areaid = string.Empty;

            // 20180606
            AreaInfo areainfo;
            if (areadescribe == null)
            {
                areainfo = _areaCacheService.GetAreaCache(new AreaCacheGetByConditionRequest { Predicate = a => a.Areaname == areaname }).Data.FirstOrDefault();
            }
            else
            {
                areainfo = _areaCacheService.GetAreaCache(new AreaCacheGetByConditionRequest { Predicate = a => a.Areaname == areaname && a.Areadescribe == areadescribe }).Data.FirstOrDefault();
            }
            if (areainfo == null)
            {
                AreaInfo area = new AreaInfo();
                areaid = IdHelper.CreateLongId().ToString();
                area.Areaid = areaid;
                area.Areaname = areaname;
                area.Areadescribe = areadescribe;
                area.Activity = "1";
                area.CreateUpdateTime = DateTime.Now;
                //更新数据库
                var _area = ObjectConverter.Copy<AreaInfo, AreaModel>(area);
                var resultarea = _areaRepository.AddArea(_area);
                //更新区域缓存
                AreaCacheAddRequest AreaCacheAddRequest = new AreaCacheAddRequest();
                AreaCacheAddRequest.AreaInfo = ObjectConverter.Copy<AreaModel, AreaInfo>(resultarea);
                _areaCacheService.AddAreaCache(AreaCacheAddRequest);
            }
            else
            {
                areaid = areainfo.Areaid;
            }
            return areaid;
        }
    }
}


