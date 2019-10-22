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

namespace Sys.Safety.Services
{
    /// <summary>
    /// 测点定义服务(所有系统)
    /// </summary>
    public partial class AllSystemPointDefineService : IAllSystemPointDefineService
    {
        private IPointDefineRepository _repository;
        private IR_DefRepository _r_defrepository;
        /// <summary>
        /// 测点定义缓存接口(监控系统)
        /// </summary>
        private IPointDefineCacheService _pointDefineCacheService;
        /// <summary>
        /// 测点定义缓存接口(人员定位)
        /// </summary>
        private IRPointDefineCacheService _rPointDefineCacheService;
        /// <summary>
        /// 测点定义缓存接口(广播系统)
        /// </summary>
        private IB_DefCacheService _B_DefCacheService;
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

        private IDeviceDefineCacheService _deviceDefineCacheService;


        public AllSystemPointDefineService(IPointDefineRepository _Repository, IR_DefRepository _R_Defrepository,
            IPointDefineCacheService _PointDefineCacheService,
            IRPointDefineCacheService _RPointDefineCacheService,
            INetworkModuleService _networkModuleService,
            INetworkModuleCacheService _networkModuleCacheService,
            IAutomaticArticulatedDeviceCacheService _automaticArticulatedDeviceCacheService,
            IDeviceDefineCacheService deviceDefineCacheService,
            IB_DefCacheService _B_DefCacheService)
        {
            this._repository = _Repository;
            this._r_defrepository = _R_Defrepository;
            this._pointDefineCacheService = _PointDefineCacheService;
            this._rPointDefineCacheService = _RPointDefineCacheService;
            this._networkModuleService = _networkModuleService;
            this._networkModuleCacheService = _networkModuleCacheService;
            this._automaticArticulatedDeviceCacheService = _automaticArticulatedDeviceCacheService;
            this._deviceDefineCacheService = deviceDefineCacheService;
            this._B_DefCacheService = _B_DefCacheService;
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
            //人员定位系统数据追加
            RPointDefineCacheGetAllRequest rpointDefineCacheRequest = new RPointDefineCacheGetAllRequest();
            var resultpersondef = _rPointDefineCacheService.GetAllRPointDefineCache(rpointDefineCacheRequest);
            Result.Data.AddRange(resultpersondef.Data.FindAll(a => a.Activity == "1"));

            //广播系统数据追加
            B_DefCacheGetAllRequest bDefCacheRequest = new B_DefCacheGetAllRequest();
            var resultb_def = _B_DefCacheService.GetAll(bDefCacheRequest);
            Result.Data.AddRange(resultb_def.Data.FindAll(a => a.Activity == "1"));
            return Result;
        }
        /// <summary>
        /// 根据条件查询缓存
        /// </summary>
        /// <param name="pointDefineCacheRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCache(PointDefineCacheGetByConditonRequest pointDefineCacheRequest)
        {
            BasicResponse<List<Jc_DefInfo>> Result = new BasicResponse<List<Jc_DefInfo>>();
            PointDefineCacheGetByConditonRequest pointDefineCacheRequestJc = new PointDefineCacheGetByConditonRequest();
            pointDefineCacheRequestJc.Predicate = pointDefineCacheRequest.Predicate;
            var result = _pointDefineCacheService.GetPointDefineCache(pointDefineCacheRequestJc);
            Result.Data = result.Data.FindAll(a => a.Activity == "1");
            //人员定位系统数据追加
            RPointDefineCacheGetByConditonRequest rpointDefineCacheRequest = new RPointDefineCacheGetByConditonRequest();
            rpointDefineCacheRequest.Predicate = pointDefineCacheRequest.Predicate;
            var resultpersondef = _rPointDefineCacheService.GetPointDefineCache(rpointDefineCacheRequest);
            Result.Data.AddRange(resultpersondef.Data.FindAll(a => a.Activity == "1"));
            //广播系统数据追加
            B_DefCacheGetByConditionRequest bDefCacheRequest = new B_DefCacheGetByConditionRequest();
            bDefCacheRequest.predicate = pointDefineCacheRequest.Predicate;
            var resultb_def = _B_DefCacheService.Get(bDefCacheRequest);
            Result.Data.AddRange(resultb_def.Data.FindAll(a => a.Activity == "1"));
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
            //在人员定位系统中查找
            if (result.Data == null)
            {
                Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest rpointDefineCacheRequest = new Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest();
                rpointDefineCacheRequest.Predicate = a => a.Point == PointDefineRequest.Point && a.Activity == "1";
                var rdefresult = _rPointDefineCacheService.GetPointDefineCache(rpointDefineCacheRequest);
                if (rdefresult.Data.Count > 0)
                {
                    Result.Data = rdefresult.Data[0];
                }
            }
            //在广播系统中查找
            if (result.Data == null)
            {
                B_DefCacheGetByConditionRequest bDefCacheRequest = new B_DefCacheGetByConditionRequest();
                bDefCacheRequest.predicate = a => a.Point == PointDefineRequest.Point && a.Activity == "1";
                var bdefresult = _B_DefCacheService.Get(bDefCacheRequest);
                if (bdefresult.Data.Count > 0)
                {
                    Result.Data = bdefresult.Data[0];
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

            //在人员定位系统中查找
            if (Result.Data == null)
            {
                Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest rpointDefineCacheRequest = new Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest();
                rpointDefineCacheRequest.Predicate = a => a.Wz == PointDefineRequest.Wz && a.Activity == "1";
                var rdefresult = _rPointDefineCacheService.GetPointDefineCache(rpointDefineCacheRequest);
                if (rdefresult.Data.Count > 0)
                {
                    Result.Data = rdefresult.Data[0];
                }
            }
            //在广播系统中查找
            if (result.Data == null)
            {
                B_DefCacheGetByConditionRequest bDefCacheRequest = new B_DefCacheGetByConditionRequest();
                bDefCacheRequest.predicate = a => a.Wz == PointDefineRequest.Wz && a.Activity == "1";
                var bdefresult = _B_DefCacheService.Get(bDefCacheRequest);
                if (bdefresult.Data.Count > 0)
                {
                    Result.Data = bdefresult.Data[0];
                }
            }
            if (Result.Data == null)
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

            //在人员定位系统中查找

            Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest rpointDefineCacheRequest = new Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest();
            rpointDefineCacheRequest.Predicate = a => a.Jckz1 == PointDefineRequest.Mac && a.Activity == "1";
            var rdefresult = _rPointDefineCacheService.GetPointDefineCache(rpointDefineCacheRequest);
            if (rdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(rdefresult.Data);
            }
            //在广播系统中查找
            B_DefCacheGetByConditionRequest bDefCacheRequest = new B_DefCacheGetByConditionRequest();
            bDefCacheRequest.predicate = a => a.Jckz1 == PointDefineRequest.Mac && a.Activity == "1";
            var bdefresult = _B_DefCacheService.Get(bDefCacheRequest);
            if (bdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(bdefresult.Data);
            }


            return Result;
        }
        /// <summary>
        ///  根据MAC地址查询MAC地址对应的分站信息
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheBySwitch(PointDefineGetByMacRequest PointDefineRequest)
        {
            BasicResponse<List<Jc_DefInfo>> Result = new BasicResponse<List<Jc_DefInfo>>();
            PointDefineCacheGetByConditonRequest pointDefineCacheRequest = new PointDefineCacheGetByConditonRequest();
            pointDefineCacheRequest.Predicate = a => a.Bz12 == PointDefineRequest.Mac && a.Activity == "1";
            var result = _pointDefineCacheService.GetPointDefineCache(pointDefineCacheRequest);
            Result.Data = result.Data;

            //在人员定位系统中查找

            Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest rpointDefineCacheRequest = new Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest();
            rpointDefineCacheRequest.Predicate = a => a.Bz12 == PointDefineRequest.Mac && a.Activity == "1";
            var rdefresult = _rPointDefineCacheService.GetPointDefineCache(rpointDefineCacheRequest);
            if (rdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(rdefresult.Data);
            }
            //在广播系统中查找
            B_DefCacheGetByConditionRequest bDefCacheRequest = new B_DefCacheGetByConditionRequest();
            bDefCacheRequest.predicate = a => a.Bz12 == PointDefineRequest.Mac && a.Activity == "1";
            var bdefresult = _B_DefCacheService.Get(bDefCacheRequest);
            if (bdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(bdefresult.Data);
            }


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
            //在人员定位系统中查找

            Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest rpointDefineCacheRequest = new Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest();
            rpointDefineCacheRequest.Predicate = a => a.K3 == int.Parse(PointDefineRequest.COM) && a.Activity == "1";
            var rdefresult = _rPointDefineCacheService.GetPointDefineCache(rpointDefineCacheRequest);
            if (rdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(rdefresult.Data);
            }
            //在广播系统中查找
            B_DefCacheGetByConditionRequest bDefCacheRequest = new B_DefCacheGetByConditionRequest();
            bDefCacheRequest.predicate = a => a.K3 == int.Parse(PointDefineRequest.COM) && a.Activity == "1";
            var bdefresult = _B_DefCacheService.Get(bDefCacheRequest);
            if (bdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(bdefresult.Data);
            }

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
            //在人员定位系统中查找

            Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest rpointDefineCacheRequest = new Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest();
            rpointDefineCacheRequest.Predicate = a => a.DevPropertyID == PointDefineRequest.DevpropertID && a.Activity == "1";
            var rdefresult = _rPointDefineCacheService.GetPointDefineCache(rpointDefineCacheRequest);
            if (rdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(rdefresult.Data);
            }
            //在广播系统中查找
            B_DefCacheGetByConditionRequest bDefCacheRequest = new B_DefCacheGetByConditionRequest();
            bDefCacheRequest.predicate = a => a.DevPropertyID == PointDefineRequest.DevpropertID && a.Activity == "1";
            var bdefresult = _B_DefCacheService.Get(bDefCacheRequest);
            if (bdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(bdefresult.Data);
            }

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
            //在人员定位系统中查找

            Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest rpointDefineCacheRequest = new Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest();
            rpointDefineCacheRequest.Predicate = a => a.DevClassID == PointDefineRequest.DevClassID && a.Activity == "1";
            var rdefresult = _rPointDefineCacheService.GetPointDefineCache(rpointDefineCacheRequest);
            if (rdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(rdefresult.Data);
            }
            //在广播系统中查找
            B_DefCacheGetByConditionRequest bDefCacheRequest = new B_DefCacheGetByConditionRequest();
            bDefCacheRequest.predicate = a => a.DevClassID == PointDefineRequest.DevClassID && a.Activity == "1";
            var bdefresult = _B_DefCacheService.Get(bDefCacheRequest);
            if (bdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(bdefresult.Data);
            }

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
            //在人员定位系统中查找

            Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest rpointDefineCacheRequest = new Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest();
            rpointDefineCacheRequest.Predicate = a => a.DevModelID == PointDefineRequest.DevModelID && a.Activity == "1";
            var rdefresult = _rPointDefineCacheService.GetPointDefineCache(rpointDefineCacheRequest);
            if (rdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(rdefresult.Data);
            }
            //在广播系统中查找
            B_DefCacheGetByConditionRequest bDefCacheRequest = new B_DefCacheGetByConditionRequest();
            bDefCacheRequest.predicate = a => a.DevModelID == PointDefineRequest.DevModelID && a.Activity == "1";
            var bdefresult = _B_DefCacheService.Get(bDefCacheRequest);
            if (bdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(bdefresult.Data);
            }

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
            //在人员定位系统中查找

            Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest rpointDefineCacheRequest = new Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest();
            rpointDefineCacheRequest.Predicate = a => a.Devid == PointDefineRequest.DevID && a.Activity == "1";
            var rdefresult = _rPointDefineCacheService.GetPointDefineCache(rpointDefineCacheRequest);
            if (rdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(rdefresult.Data);
            }
            //在广播系统中查找
            B_DefCacheGetByConditionRequest bDefCacheRequest = new B_DefCacheGetByConditionRequest();
            bDefCacheRequest.predicate = a => a.Devid == PointDefineRequest.DevID && a.Activity == "1";
            var bdefresult = _B_DefCacheService.Get(bDefCacheRequest);
            if (bdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(bdefresult.Data);
            }

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
            //在人员定位系统中查找

            Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest rpointDefineCacheRequest = new Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest();
            rpointDefineCacheRequest.Predicate = a => a.Fzh == PointDefineRequest.StationID && a.Activity == "1";
            var rdefresult = _rPointDefineCacheService.GetPointDefineCache(rpointDefineCacheRequest);
            if (rdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(rdefresult.Data);
            }
            //在广播系统中查找
            B_DefCacheGetByConditionRequest bDefCacheRequest = new B_DefCacheGetByConditionRequest();
            bDefCacheRequest.predicate = a => a.Fzh == PointDefineRequest.StationID && a.Activity == "1";
            var bdefresult = _B_DefCacheService.Get(bDefCacheRequest);
            if (bdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(bdefresult.Data);
            }

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
            Result.Data = new List<Jc_DefInfo>();

            if (resultStation.Data.Count > 0)
            {
                PointDefineCacheGetByConditonRequest pointDefineCacheStationRequest = new PointDefineCacheGetByConditonRequest();
                pointDefineCacheStationRequest.Predicate = a => a.Fzh == resultStation.Data[0].Fzh & a.DevPropertyID != 0 && a.Activity == "1";
                var result = _pointDefineCacheService.GetPointDefineCache(pointDefineCacheStationRequest);//再根据分站号查找分站下面的设备
                Result.Data = result.Data;
            }
            //在人员定位系统中查找

            Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest rpointDefineCacheRequest = new Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest();
            rpointDefineCacheRequest.Predicate = a => a.Point == PointDefineRequest.StationPoint & a.DevPropertyID == 0 && a.Activity == "1";
            resultStation = _rPointDefineCacheService.GetPointDefineCache(rpointDefineCacheRequest);

            if (resultStation.Data.Count > 0)
            {
                Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest rpointDefineCacheRequest1 = new Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest();
                rpointDefineCacheRequest1.Predicate = a => a.Fzh == resultStation.Data[0].Fzh && a.Activity == "1";
                var rdefresult = _rPointDefineCacheService.GetPointDefineCache(rpointDefineCacheRequest1);
                if (rdefresult.Data.Count > 0)
                {
                    Result.Data.AddRange(rdefresult.Data);
                }
            }

            //在广播系统中查找
            B_DefCacheGetByConditionRequest bDefCacheRequest = new B_DefCacheGetByConditionRequest();
            bDefCacheRequest.predicate = a => a.Point == PointDefineRequest.StationPoint & a.DevPropertyID == 0 && a.Activity == "1";
            resultStation = _B_DefCacheService.Get(bDefCacheRequest);

            if (resultStation.Data.Count > 0)
            {
                B_DefCacheGetByConditionRequest bDefCacheRequest1 = new B_DefCacheGetByConditionRequest();
                bDefCacheRequest1.predicate = a => a.Fzh == resultStation.Data[0].Fzh && a.Activity == "1";
                var bdefresult = _B_DefCacheService.Get(bDefCacheRequest1);
                if (bdefresult.Data.Count > 0)
                {
                    Result.Data.AddRange(bdefresult.Data);
                }
            }


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
            //在人员定位系统中查找

            Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest rpointDefineCacheRequest = new Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest();
            rpointDefineCacheRequest.Predicate = a => a.Fzh == PointDefineRequest.StationID && a.Kh == PointDefineRequest.ChannelID
            && a.DevPropertyID == PointDefineRequest.DevPropertID && a.Activity == "1";
            var rdefresult = _rPointDefineCacheService.GetPointDefineCache(rpointDefineCacheRequest);
            if (rdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(rdefresult.Data);
            }
            //在广播系统中查找
            B_DefCacheGetByConditionRequest bDefCacheRequest = new B_DefCacheGetByConditionRequest();
            bDefCacheRequest.predicate = a => a.Fzh == PointDefineRequest.StationID && a.Kh == PointDefineRequest.ChannelID
            && a.DevPropertyID == PointDefineRequest.DevPropertID && a.Activity == "1";
            var bdefresult = _B_DefCacheService.Get(bDefCacheRequest);
            if (bdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(bdefresult.Data);
            }

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
            //在人员定位系统中查找

            Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest rpointDefineCacheRequest = new Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest();
            rpointDefineCacheRequest.Predicate = a => a.Fzh == PointDefineRequest.StationID
            && a.DevPropertyID == PointDefineRequest.DevPropertID && a.Activity == "1";
            var rdefresult = _rPointDefineCacheService.GetPointDefineCache(rpointDefineCacheRequest);
            if (rdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(rdefresult.Data);
            }
            //在广播系统中查找
            B_DefCacheGetByConditionRequest bDefCacheRequest = new B_DefCacheGetByConditionRequest();
            bDefCacheRequest.predicate = a => a.Fzh == PointDefineRequest.StationID
            && a.DevPropertyID == PointDefineRequest.DevPropertID && a.Activity == "1";
            var bdefresult = _B_DefCacheService.Get(bDefCacheRequest);
            if (bdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(bdefresult.Data);
            }

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
            //在人员定位系统中查找

            Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest rpointDefineCacheRequest = new Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest();
            rpointDefineCacheRequest.Predicate = a => a.Fzh == PointDefineRequest.StationID
             && a.Kh == PointDefineRequest.ChannelID
              && a.Dzh == PointDefineRequest.AddressID
            && a.DevPropertyID == PointDefineRequest.DevPropertID && a.Activity == "1";
            var rdefresult = _rPointDefineCacheService.GetPointDefineCache(rpointDefineCacheRequest);
            if (rdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(rdefresult.Data);
            }
            //在广播系统中查找
            B_DefCacheGetByConditionRequest bDefCacheRequest = new B_DefCacheGetByConditionRequest();
            bDefCacheRequest.predicate = a => a.Fzh == PointDefineRequest.StationID
             && a.Kh == PointDefineRequest.ChannelID
              && a.Dzh == PointDefineRequest.AddressID
            && a.DevPropertyID == PointDefineRequest.DevPropertID && a.Activity == "1";
            var bdefresult = _B_DefCacheService.Get(bDefCacheRequest);
            if (bdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(bdefresult.Data);
            }

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
            //在人员定位系统中查找

            Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest rpointDefineCacheRequest = new Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest();
            rpointDefineCacheRequest.Predicate = a => a.Fzh == PointDefineRequest.StationID
             && a.Kh == PointDefineRequest.ChannelID && a.Activity == "1";
            var rdefresult = _rPointDefineCacheService.GetPointDefineCache(rpointDefineCacheRequest);
            if (rdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(rdefresult.Data);
            }
            //在广播系统中查找
            B_DefCacheGetByConditionRequest bDefCacheRequest = new B_DefCacheGetByConditionRequest();
            bDefCacheRequest.predicate = a => a.Fzh == PointDefineRequest.StationID
             && a.Kh == PointDefineRequest.ChannelID && a.Activity == "1";
            var bdefresult = _B_DefCacheService.Get(bDefCacheRequest);
            if (bdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(bdefresult.Data);
            }

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
            //在人员定位系统中查找

            Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest rpointDefineCacheRequest = new Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest();
            rpointDefineCacheRequest.Predicate = a => a.Areaid == PointDefineRequest.AreaId && a.Activity == "1";
            var rdefresult = _rPointDefineCacheService.GetPointDefineCache(rpointDefineCacheRequest);
            if (rdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(rdefresult.Data);
            }
            //在广播系统中查找
            B_DefCacheGetByConditionRequest bDefCacheRequest = new B_DefCacheGetByConditionRequest();
            bDefCacheRequest.predicate = a => a.Areaid == PointDefineRequest.AreaId && a.Activity == "1";
            var bdefresult = _B_DefCacheService.Get(bDefCacheRequest);
            if (bdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(bdefresult.Data);
            }

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
            //在人员定位系统中查找

            Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest rpointDefineCacheRequest = new Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest();
            rpointDefineCacheRequest.Predicate = a => a.AreaLoc.IndexOf(PointDefineRequest.AreaCode) == 0 && a.Activity == "1";
            var rdefresult = _rPointDefineCacheService.GetPointDefineCache(rpointDefineCacheRequest);
            if (rdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(rdefresult.Data);
            }
            //在广播系统中查找
            B_DefCacheGetByConditionRequest bDefCacheRequest = new B_DefCacheGetByConditionRequest();
            bDefCacheRequest.predicate = a => a.AreaLoc.IndexOf(PointDefineRequest.AreaCode) == 0 && a.Activity == "1";
            var bdefresult = _B_DefCacheService.Get(bDefCacheRequest);
            if (bdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(bdefresult.Data);
            }

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
            //在人员定位系统中查找

            Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest rpointDefineCacheRequest = new Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest();
            rpointDefineCacheRequest.Predicate = a => a.AreaLoc.IndexOf(PointDefineRequest.AreaCode) == 0 && a.DevPropertyID != PointDefineRequest.DevPropertID && a.Activity == "1";
            var rdefresult = _rPointDefineCacheService.GetPointDefineCache(rpointDefineCacheRequest);
            if (rdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(rdefresult.Data);
            }
            //在广播系统中查找
            B_DefCacheGetByConditionRequest bDefCacheRequest = new B_DefCacheGetByConditionRequest();
            bDefCacheRequest.predicate = a => a.AreaLoc.IndexOf(PointDefineRequest.AreaCode) == 0 && a.DevPropertyID != PointDefineRequest.DevPropertID && a.Activity == "1";
            var bdefresult = _B_DefCacheService.Get(bDefCacheRequest);
            if (bdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(bdefresult.Data);
            }

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
            //在人员定位系统中查找

            Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest rpointDefineCacheRequest = new Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest();
            rpointDefineCacheRequest.Predicate = a => (strKeywords == "" || a.Point.Contains(strKeywords) || a.AreaName.Contains(strKeywords) || a.Wz.Contains(strKeywords)
                ) && a.Kh != 0 && a.Activity == "1";
            var rdefresult = _rPointDefineCacheService.GetPointDefineCache(rpointDefineCacheRequest);
            if (rdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(rdefresult.Data);
            }
            //在广播系统中查找
            B_DefCacheGetByConditionRequest bDefCacheRequest = new B_DefCacheGetByConditionRequest();
            bDefCacheRequest.predicate = a => (strKeywords == "" || a.Point.Contains(strKeywords) || a.AreaName.Contains(strKeywords) || a.Wz.Contains(strKeywords)
                ) && a.Kh != 0 && a.Activity == "1";
            var bdefresult = _B_DefCacheService.Get(bDefCacheRequest);
            if (bdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(bdefresult.Data);
            }

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
            //在人员定位系统中查找

            Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest rpointDefineCacheRequest = new Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest();
            rpointDefineCacheRequest.Predicate = a => a.PointID == PointDefineRequest.PointID && a.Activity == "1";
            var rdefresult = _rPointDefineCacheService.GetPointDefineCache(rpointDefineCacheRequest);
            if (rdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(rdefresult.Data);
            }
            //在广播系统中查找
            B_DefCacheGetByConditionRequest bDefCacheRequest = new B_DefCacheGetByConditionRequest();
            bDefCacheRequest.predicate = a => a.PointID == PointDefineRequest.PointID && a.Activity == "1";
            var bdefresult = _B_DefCacheService.Get(bDefCacheRequest);
            if (bdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(bdefresult.Data);
            }

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
            //在人员定位系统中查找

            Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest rpointDefineCacheRequest = new Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest();
            rpointDefineCacheRequest.Predicate = a => !string.IsNullOrEmpty(a.Jckz1) && a.Activity == "1";
            var rdefresult = _rPointDefineCacheService.GetPointDefineCache(rpointDefineCacheRequest);
            if (rdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(rdefresult.Data);
            }
            //在广播系统中查找
            B_DefCacheGetByConditionRequest bDefCacheRequest = new B_DefCacheGetByConditionRequest();
            bDefCacheRequest.predicate = a => !string.IsNullOrEmpty(a.Jckz1) && a.Activity == "1";
            var bdefresult = _B_DefCacheService.Get(bDefCacheRequest);
            if (bdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(bdefresult.Data);
            }

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
            //在人员定位系统中查找

            Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest rpointDefineCacheRequest = new Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest();
            rpointDefineCacheRequest.Predicate = a => string.IsNullOrEmpty(a.Jckz1) && a.K3 > 0 && a.Activity == "1";
            var rdefresult = _rPointDefineCacheService.GetPointDefineCache(rpointDefineCacheRequest);
            if (rdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(rdefresult.Data);
            }
            //在广播系统中查找
            B_DefCacheGetByConditionRequest bDefCacheRequest = new B_DefCacheGetByConditionRequest();
            bDefCacheRequest.predicate = a => string.IsNullOrEmpty(a.Jckz1) && a.K3 > 0 && a.Activity == "1";
            var bdefresult = _B_DefCacheService.Get(bDefCacheRequest);
            if (bdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(bdefresult.Data);
            }

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
            //在人员定位系统中查找

            Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest rpointDefineCacheRequest = new Sys.Safety.Request.PersonCache.RPointDefineCacheGetByConditonRequest();
            rpointDefineCacheRequest.Predicate = a => a.DevPropertyID == 0 && string.IsNullOrEmpty(a.Jckz1) && string.IsNullOrEmpty(a.Jckz2) && a.K3 <= 0 && a.Activity == "1";
            var rdefresult = _rPointDefineCacheService.GetPointDefineCache(rpointDefineCacheRequest);
            if (rdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(rdefresult.Data);
            }
            //在广播系统中查找
            B_DefCacheGetByConditionRequest bDefCacheRequest = new B_DefCacheGetByConditionRequest();
            bDefCacheRequest.predicate = a => a.DevPropertyID == 0 && string.IsNullOrEmpty(a.Jckz1) && string.IsNullOrEmpty(a.Jckz2) && a.K3 <= 0 && a.Activity == "1";
            var bdefresult = _B_DefCacheService.Get(bDefCacheRequest);
            if (bdefresult.Data.Count > 0)
            {
                Result.Data.AddRange(bdefresult.Data);
            }

            return Result;
        }
    }
}


