using Basic.Framework.Common;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.DataCollection.Common.Protocols;
using Sys.DataCollection.Common.Protocols.Devices;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.CommunicateExtend;

using Sys.Safety.Model;
using Sys.Safety.Enums;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.DataToDb;
using Sys.Safety.Request.Config;
using Sys.Safety.Request.ManualCrossControl;
using Sys.Safety.Request.StaionControlHistoryData;
using Sys.Safety.Request.StaionHistoryData;
using Sys.Safety.Request.StationUpdate;
using Sys.Safety.ServiceContract;
using Sys.Safety.ServiceContract.Cache;
using Sys.Safety.ServiceContract.DataToDb;
using System;
using System.Collections.Generic;
using System.Linq;
using Sys.Safety.Request.NetworkModule;

namespace Sys.Safety.Driver
{
    /// <summary>
    /// 作者：
    /// 时间：2017-06-01
    /// 描述：驱动配置
    /// 修改记录
    /// 2017-06-01
    /// </summary>
    public class SafetyHelper
    {
        #region ----服务接口定义----

        public static IManualCrossControlService manualControlService;
        public static IManualCrossControlCacheService manualControlCacheService;
        public static IInsertToDbService<Jc_McInfo> mcDataInsertToDbService;
        public static IInsertToDbService<Jc_MInfo> fiveMinDataTodbService;
        public static IInsertToDbService<Jc_KdInfo> kdDataTodbService;
        public static IInsertToDbService<Jc_RInfo> jcRTodbService;
        public static IInsertToDbService<Jc_BInfo> alarmTodbService;

        public static IPointDefineRepository pointDefineRepository;
        public static IAlarmRecordRepository alarmRecordRepository;

        public static IRunLogCacheService runLogCacheService;
        public static IAlarmCacheService alarmCacheService;
        public static IDeviceDefineCacheService deviceDefineCacheService;
        public static IPointDefineCacheService pointDefineCacheService;
        public static IAutomaticArticulatedDeviceCacheService automaticArticulatedDeviceCacheService;
        public static IStaionHistoryDataService staionHistoryDataService;
        public static IStaionControlHistoryDataService staionControlHistoryDataService;
        public static ISettingCacheService settingCacheService;
        public static IConfigService configService;
        /// <summary>
        /// 网络模块缓存RPC接口
        /// </summary>
        private static INetworkModuleCacheService networkModuleCacheService;

        private static IStationUpdateService stationUpdateService;
        #endregion

        /// <summary>
        /// 五分钟数据处理锁
        /// </summary>
        public static object fiveMiniteDataProcLocker;
        /// <summary>
        /// 系统可挂接最大分站数
        /// </summary>
        public static int NmaxStationNum;
        /// <summary>
        /// 电量过低报警值
        /// </summary>
        public static float SensorPowerAlarmValue = 20;
        ///// <summary>
        ///// 测点时间重复处理临时变量
        ///// </summary>
        //public static Dictionary<string, DateTime> pointTimeTemp = new Dictionary<string, DateTime>();


        /// <summary>
        /// 静态构造函数：初始化静态属性（在所有静态成员之前有.net内部调用）
        /// </summary>
        static SafetyHelper()
        {
            manualControlService = ServiceFactory.Create<IManualCrossControlService>();
            manualControlCacheService = ServiceFactory.Create<IManualCrossControlCacheService>();
            mcDataInsertToDbService = ServiceFactory.Create<IInsertToDbService<Jc_McInfo>>();
            fiveMinDataTodbService = ServiceFactory.Create<IInsertToDbService<Jc_MInfo>>();
            kdDataTodbService = ServiceFactory.Create<IInsertToDbService<Jc_KdInfo>>();
            jcRTodbService = ServiceFactory.Create<IInsertToDbService<Jc_RInfo>>();
            alarmTodbService = ServiceFactory.Create<IInsertToDbService<Jc_BInfo>>();

            pointDefineRepository = ServiceFactory.Create<IPointDefineRepository>();
            alarmRecordRepository = ServiceFactory.Create<IAlarmRecordRepository>();

            runLogCacheService = ServiceFactory.Create<IRunLogCacheService>();
            alarmCacheService = ServiceFactory.Create<IAlarmCacheService>();
            deviceDefineCacheService = ServiceFactory.Create<IDeviceDefineCacheService>();
            pointDefineCacheService = ServiceFactory.Create<IPointDefineCacheService>();
            automaticArticulatedDeviceCacheService = ServiceFactory.Create<IAutomaticArticulatedDeviceCacheService>();
            staionHistoryDataService = ServiceFactory.Create<IStaionHistoryDataService>();
            staionControlHistoryDataService = ServiceFactory.Create<IStaionControlHistoryDataService>();
            settingCacheService = ServiceFactory.Create<ISettingCacheService>();
            configService = ServiceFactory.Create<IConfigService>();

            networkModuleCacheService = ServiceFactory.Create<INetworkModuleCacheService>();

            stationUpdateService = ServiceFactory.Create<IStationUpdateService>();

            fiveMiniteDataProcLocker = new object();
            NmaxStationNum = 255;
            //读取电量报警阈值
            SettingCacheGetByKeyRequest request = new SettingCacheGetByKeyRequest();
            request.StrKey = "SensorPowerAlarmValue";
            var result = settingCacheService.GetSettingCacheByKey(request);
            if (result != null && result.Data != null)
            {
                float.TryParse(result.Data.StrValue, out SensorPowerAlarmValue);
                if (SensorPowerAlarmValue < 1)
                {
                    SensorPowerAlarmValue = 20;
                }
            }
        }

        #region 运行记录相关操作
        /// <summary>
        /// 写运行记录
        /// </summary>
        /// <param name="defInfo"></param>
        /// <param name="time"></param>
        public static void WriteRunLog(Jc_DefInfo defInfo, DateTime time)
        {
            bool isWriteNormalRrecord = true;
            #region 写R记录
            if (defInfo.DataState == (short)DeviceDataState.EquipmentStateUnknow)
            {
                CreateRunLogInfo(defInfo, time, (short)DeviceDataState.EquipmentDown, (int)DeviceDataState.EquipmentDown, "断线");
            }
            else
            {
                //if ((defInfo.Alarm & 0x02) == 0x02)
                if (AlarmBusiness.JudgeAlarmIsAlarmState(defInfo.Alarm, DeviceDataState.DataHighAlarm))
                {
                    isWriteNormalRrecord = false;
                    //上限报警
                    CreateRunLogInfo(defInfo, time, 10, defInfo.State, defInfo.Ssz);
                }
                //if ((defInfo.Alarm & 0x20) == 0x20)
                if (AlarmBusiness.JudgeAlarmIsAlarmState(defInfo.Alarm, DeviceDataState.DataLowAlarm))
                {
                    isWriteNormalRrecord = false;
                    //下限报警
                    CreateRunLogInfo(defInfo, time, 16, defInfo.State, defInfo.Ssz);
                }
                //if ((defInfo.Alarm & 0x04) == 0x04)
                if (AlarmBusiness.JudgeAlarmIsAlarmState(defInfo.Alarm, DeviceDataState.DataHighAlarmPowerOFF))
                {
                    isWriteNormalRrecord = false;
                    //上限断电
                    CreateRunLogInfo(defInfo, time, 12, defInfo.State, defInfo.Ssz);
                }
                //if ((defInfo.Alarm & 0x40) == 0x40)
                if (AlarmBusiness.JudgeAlarmIsAlarmState(defInfo.Alarm, DeviceDataState.DataLowPower))
                {
                    isWriteNormalRrecord = false;
                    //下限断电
                    CreateRunLogInfo(defInfo, time, 18, defInfo.State, defInfo.Ssz);
                }

                if (isWriteNormalRrecord)
                {
                    //未写任何报警记录，则补写一条运行记录
                    if (defInfo.State == (short)DeviceRunState.EquipmentStateUnknow)
                    {
                        SafetyHelper.CreateRunLogInfo(defInfo, time, (short)DeviceDataState.EquipmentDown, (short)DeviceRunState.EquipmentDown, "断线");
                    }
                    else
                    {
                        SafetyHelper.CreateRunLogInfo(defInfo, time, defInfo.DataState, defInfo.State, defInfo.Ssz);
                    }
                    //CreateRunLogInfo(defInfo, time, defInfo.DataState, defInfo.State, defInfo.Ssz);
                }
            }
            #endregion
        }

        /// <summary>
        /// 生成运行记录 并保存到数据库及缓存
        /// </summary>
        /// <param name="def">测点信息</param>
        /// <param name="time">数据采集时间</param>
        /// <param name="dataState">数据状态</param>
        /// <param name="runState">设备状态</param>
        /// <param name="realValue">实时值</param>
        public static void CreateRunLogInfo(Jc_DefInfo pointInfo, DateTime time, short dataState, short runState, string realValue, string remark = "")
        {
            try
            {
                Jc_RInfo runlogInfo = new Jc_RInfo();
                runlogInfo.ID = IdHelper.CreateLongId().ToString();
                runlogInfo.Devid = pointInfo.Devid;
                runlogInfo.PointID = pointInfo.PointID;
                runlogInfo.Fzh = pointInfo.Fzh;
                runlogInfo.Kh = pointInfo.Kh;
                runlogInfo.Dzh = pointInfo.Dzh;
                runlogInfo.Point = pointInfo.Point;
                runlogInfo.Upflag = pointInfo.Upflag;
                runlogInfo.Wzid = pointInfo.Wzid;

                runlogInfo.Val = realValue;
                runlogInfo.Timer = time;
                runlogInfo.Type = dataState;
                runlogInfo.State = GetState((DeviceRunState)runState, pointInfo.Bz4);

                runlogInfo.Remark = remark;

                runlogInfo.InfoState = InfoState.AddNew;
                //添加运行记录至缓存
                RunLogCacheAddRequest runLogCacheAddRequest = new RunLogCacheAddRequest();
                runLogCacheAddRequest.RunLogInfo = runlogInfo;
                runLogCacheService.AddRunLogCache(runLogCacheAddRequest);
                //添加运行记录至数据库
                DataToDbAddRequest<Jc_RInfo> dataToDbAddRequest = new DataToDbAddRequest<Jc_RInfo>();
                dataToDbAddRequest.Item = runlogInfo;
                jcRTodbService.AddItem(dataToDbAddRequest);
            }
            catch (Exception ex)
            {
                LogHelper.Error("CreateRunLogInfo Error【" + pointInfo.Point + "】" + ex.Message);
            }
        }

        #endregion

        #region ----密采操作相关----

        /// <summary>
        /// 密采记录相关操作
        /// </summary>
        /// <param name="andloInfo"></param>
        /// <param name="time"></param>
        /// <param name="datastate"></param>
        /// <param name="runstate"></param>
        public static void CreateDentisyCollInfo(Jc_DefInfo analogInfo, DateTime time, DeviceDataState datastate, DeviceRunState runstate)
        {
            try
            {
                Jc_McInfo densityColl = new Jc_McInfo();
                densityColl.PointID = analogInfo.PointID;
                densityColl.ID = IdHelper.CreateLongId().ToString();
                densityColl.Point = analogInfo.Point;
                densityColl.Devid = analogInfo.Devid;
                densityColl.Fzh = analogInfo.Fzh;
                densityColl.Kh = analogInfo.Kh;
                densityColl.Dzh = analogInfo.Dzh;
                densityColl.Timer = time;
                densityColl.Type = (short)datastate;
                densityColl.State = GetState(runstate, analogInfo.Bz4);
                densityColl.Voltage = analogInfo.Voltage;  //2018.2.27 by 密采记录加入保存当时的电压
                if (isAbnormalState(runstate))
                {
                    densityColl.Ssz = 0;
                }
                else
                {
                    densityColl.Ssz = float.Parse(analogInfo.Ssz);
                }
                densityColl.Upflag = "0";
                densityColl.Wzid = analogInfo.Wzid;

                if (runstate == DeviceRunState.EquipmentAdjusting)
                {
                    densityColl.Bz5 = analogInfo.CalibrationNum; //2017.10.13 by 标校状态下，写入标校标识
                }

                densityColl.Bz1 = analogInfo.GradingAlarmLevel;//分级报警等级

                densityColl.InfoState = InfoState.AddNew;
                //密采记录入库
                DataToDbAddRequest<Jc_McInfo> dataToDbAddRequest = new DataToDbAddRequest<Jc_McInfo>();
                dataToDbAddRequest.Item = densityColl;
                mcDataInsertToDbService.AddItem(dataToDbAddRequest);
            }
            catch (Exception ex)
            {
                LogHelper.Error("CreateDentisyCollInfo Error【" + analogInfo.Point + "】" + ex.Message);
            }
        }

        #endregion

        #region 测点定义相关操作

        /// <summary>
        /// 根据分站号获取测点定义缓存
        /// </summary>
        /// <param name="stationID">分站号</param>
        /// <returns></returns>
        public static List<Jc_DefInfo> GetPointDefinesByStationID(short stationID)
        {
            try
            {
                PointDefineCacheGetByStationRequest pointDefineCacheGetByStationRequest = new PointDefineCacheGetByStationRequest();
                pointDefineCacheGetByStationRequest.Station = stationID;
                pointDefineCacheGetByStationRequest.IsQueryFromWriteCache = true;
                var defResponse = pointDefineCacheService.GetPointDefineCacheByStation(pointDefineCacheGetByStationRequest);
                if (defResponse != null)
                {
                    return defResponse.Data;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("GetPointDefinesByStationID Error【stationID = " + stationID + "】" + ex.Message);
            }
            return new List<Jc_DefInfo>();
        }

        /// <summary>
        /// 根据测点号point查找结构体
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Jc_DefInfo GetPointDefinesByPoint(string point)
        {
            try
            {
                PointDefineCacheGetByKeyRequest pointDefineCacheGetByKeyRequest = new PointDefineCacheGetByKeyRequest();
                pointDefineCacheGetByKeyRequest.Point = point;
                pointDefineCacheGetByKeyRequest.IsQueryFromWriteCache = true;
                var defResponse = pointDefineCacheService.GetPointDefineCacheByKey(pointDefineCacheGetByKeyRequest);
                if (defResponse.IsSuccess && defResponse != null)
                {
                    return defResponse.Data;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("GetPointDefinesByPoint Error【point = " + point + "】" + ex.Message);
            }
            return null;
        }

        /// <summary>
        /// 更新JC_DEF的部分属性
        /// </summary>
        /// <param name="point">分站测点号</param>
        /// <param name="paramater">key = 属性名称(区分大小写),value = 值</param>
        public static void UpdatePointDefineInfoByProperties(string pointID, Dictionary<string, object> paramater)
        {
            try
            {
                DefineCacheUpdatePropertiesRequest defineCacheUpdatePropertiesRequest = new DefineCacheUpdatePropertiesRequest();
                defineCacheUpdatePropertiesRequest.PointID = pointID;
                defineCacheUpdatePropertiesRequest.UpdateItems = paramater;
                pointDefineCacheService.UpdatePointDefineInfo(defineCacheUpdatePropertiesRequest);
            }
            catch (Exception ex)
            {
                LogHelper.Error("UpdatePointDefineInfoByProperties Error【pointID = " + pointID + "】" + ex.Message);
            }
        }

        /// <summary>
        /// 批量 更新JC_DEF的部分属性
        /// </summary>
        /// <param name="pointItems"></param>
        public static void BatchUpdatePointDefineInfoByProperties(Dictionary<string, Dictionary<string, object>> pointItems)
        {
            //added by  20170719
            DefineCacheBatchUpdatePropertiesRequest request = new DefineCacheBatchUpdatePropertiesRequest();
            request.PointItems = pointItems;

            pointDefineCacheService.BatchUpdatePointDefineInfo(request);
        }

        /// <summary>
        /// 根据设备信息获取开关量、控制量所显示的实时值
        /// </summary>
        /// <param name="datastate"></param>
        /// <param name="devID"></param>
        /// <returns></returns>
        public static string GetDerailSszByDataState(DeviceDataState datastate, string devID)
        {
            string realValue = "断线";

            Jc_DevInfo devInfo;
            IDeviceDefineCacheService deviceDefineCacheService = ServiceFactory.Create<IDeviceDefineCacheService>();
            DeviceDefineCacheGetByKeyRequest deviceDefineCacheGetByKeyRequest = new DeviceDefineCacheGetByKeyRequest();
            deviceDefineCacheGetByKeyRequest.Devid = devID;
            var result = deviceDefineCacheService.GetPointDefineCacheByKey(deviceDefineCacheGetByKeyRequest);
            if (result != null && result.IsSuccess && result.Data != null)
            {
                devInfo = result.Data;
                switch (datastate)
                {
                    case DeviceDataState.DataDerailState0:
                    case DeviceDataState.DataControlState0:
                        realValue = devInfo.Xs1;
                        break;
                    case DeviceDataState.DataDerailState1:
                    case DeviceDataState.DataControlState1:
                        realValue = devInfo.Xs2;
                        break;
                    case DeviceDataState.DataDerailState2:
                        realValue = devInfo.Xs3;
                        break;
                }
            }
            return realValue;
        }

        /// <summary>
        /// 更新设备唯一编码  bz13、bz14到内存和数据库
        /// </summary>
        /// <param name="def"></param>
        public static void UpdateSoleCodings(List<Jc_DefInfo> defItems)
        {
            try
            {
                // 调用接口更新设备唯一编码到数内存
                Dictionary<string, object> updateItems;
                defItems.ForEach(def =>
                {
                    updateItems = new Dictionary<string, object>();
                    updateItems.Add("Bz16", def.Bz16);
                    updateItems.Add("Bz14", def.Bz14);
                    updateItems.Add("Bz15", def.Bz15);
                    updateItems.Add("BatteryItems", def.BatteryItems);
                    UpdatePointDefineInfoByProperties(def.PointID, updateItems);
                });

                // 调用接口更新设备唯一编码到数据库
                System.Data.DataColumn[] cols = new System.Data.DataColumn[2];
                cols[0] = new System.Data.DataColumn("Bz16");
                cols[1] = new System.Data.DataColumn("Bz14");
                cols[1] = new System.Data.DataColumn("Bz15");
                var _jc_Def = ObjectConverter.CopyList<Jc_DefInfo, Jc_DefModel>(defItems);
                pointDefineRepository.BulkUpdate("KJ_DeviceDefInfo", _jc_Def, cols, "ID");
            }
            catch (Exception ex)
            {
                LogHelper.Error("UpdateSoleCodings Error:" + ex.Message);
            }
        }
        /// <summary>
        /// 更新分站的基础信息(更新数据库)
        /// </summary>
        /// <param name="defItems"></param>
        public static void UpdateStationDetailData(List<Jc_DefInfo> defItems)
        {
            try
            {
                //// 调用接口更新设备唯一编码到数内存
                //Dictionary<string, object> updateItems;
                //defItems.ForEach(def =>
                //{
                //    updateItems = new Dictionary<string, object>();
                //    updateItems.Add("Voltage", def.Voltage);//电量
                //    updateItems.Add("Bz16", def.Bz16);//唯一编码
                //    updateItems.Add("Bz17", def.Bz17);//重启次数
                //    updateItems.Add("Bz15", def.Bz15);//出厂日期
                //    updateItems.Add("Bz18", def.Bz18);//当前时间
                //    //updateItems.Add("Bz13", def.Bz13);//IP
                //    //updateItems.Add("Bz12", def.Bz12);//MAC
                //    UpdatePointDefineInfoByProperties(def.PointID, updateItems);
                //});

                // 调用接口更新设备唯一编码到数据库
                System.Data.DataColumn[] cols = new System.Data.DataColumn[4];
                cols[0] = new System.Data.DataColumn("Voltage");
                cols[1] = new System.Data.DataColumn("Bz17");
                cols[2] = new System.Data.DataColumn("Bz15");
                cols[3] = new System.Data.DataColumn("Bz18");
                var _jc_Def = ObjectConverter.CopyList<Jc_DefInfo, Jc_DefModel>(defItems);
                pointDefineRepository.BulkUpdate("KJ_DeviceDefInfo", _jc_Def, cols, "ID");
            }
            catch (Exception ex)
            {
                LogHelper.Error("UpdateStationDetailData Error:" + ex.Message);
            }
        }
        /// <summary>
        /// 更新传感器的基础信息(更新数据库)
        /// </summary>
        /// <param name="defItems"></param>
        public static void UpdateSensorDetailData(List<Jc_DefInfo> defItems)
        {
            try
            {
                //// 调用接口更新设备唯一编码到数内存
                //Dictionary<string, object> updateItems;
                //defItems.ForEach(def =>
                //{
                //    updateItems = new Dictionary<string, object>();
                //    updateItems.Add("Voltage", def.Voltage);//电压
                //    updateItems.Add("Bz16", def.Bz16);//唯一编码
                //    updateItems.Add("Bz17", def.Bz17);//重启次数
                //    updateItems.Add("Bz15", def.Bz15);//出厂日期                  
                //    updateItems.Add("Bz13", def.Bz13);//报警次数                    
                //    UpdatePointDefineInfoByProperties(def.PointID, updateItems);
                //});

                // 调用接口更新设备唯一编码到数据库
                System.Data.DataColumn[] cols = new System.Data.DataColumn[5];
                cols[0] = new System.Data.DataColumn("Voltage");
                cols[1] = new System.Data.DataColumn("Bz17");
                cols[2] = new System.Data.DataColumn("Bz15");
                cols[3] = new System.Data.DataColumn("Bz13");
                cols[4] = new System.Data.DataColumn("Bz14");
                var _jc_Def = ObjectConverter.CopyList<Jc_DefInfo, Jc_DefModel>(defItems);
                pointDefineRepository.BulkUpdate("KJ_DeviceDefInfo", _jc_Def, cols, "ID");
            }
            catch (Exception ex)
            {
                LogHelper.Error("UpdateSensorDetailData Error:" + ex.Message);
            }
        }

        /// <summary>
        /// 根据devModelID取dev信息
        /// </summary>
        /// <param name="devModelID"></param>
        /// <returns></returns>
        public static Jc_DevInfo GetDeviceInfoByDevModelID(int devModelID)
        {
            Jc_DevInfo devInfo = null;
            try
            {
                DeviceDefineCacheGetByConditonRequest deviceDefineCacheGetByConditonRequest = new DeviceDefineCacheGetByConditonRequest();
                deviceDefineCacheGetByConditonRequest.Predicate = a => a.Bz4 == devModelID;
                var result = deviceDefineCacheService.GetPointDefineCache(deviceDefineCacheGetByConditonRequest);
                if (result.Data != null && result.IsSuccess)
                {
                    devInfo = result.Data.First();
                }
            }
            catch
            {
            }
            return devInfo;
        }

        /// <summary>
        /// 根据设备性质查询dev信息（dev.type）
        /// </summary>
        /// <param name="devType"></param>
        /// <returns></returns>
        public static List<Jc_DevInfo> GetDeviceInfoByDevType(int devType)
        {
            List<Jc_DevInfo> devItems = null;
            try
            {
                DeviceDefineCacheGetByConditonRequest deviceDefineCacheGetByConditonRequest = new DeviceDefineCacheGetByConditonRequest();
                deviceDefineCacheGetByConditonRequest.Predicate = a => a.Type == devType;
                var result = deviceDefineCacheService.GetPointDefineCache(deviceDefineCacheGetByConditonRequest);
                if (result.Data != null && result.IsSuccess)
                {
                    devItems = result.Data;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("GetDeviceInfoByDevType Error" + ex.Message);
            }
            return devItems;
        }


        ///// <summary>
        ///// 批量更新Def缓存
        ///// </summary>
        ///// <param name="defItems"></param>
        ///// <param name="updateType">1 更新整个内存，2 更新控制相关字段，3 更新初始化相关字段 ，4 更新中断次数 ，5 更新实时值相关字段</param>
        //public static void BatchUpdatePointDefineInfo(List<Jc_DefInfo> defItems, int updateType)
        //{
        //    defItems.ForEach(point => UpdatePointDefineInfo(point, updateType));
        //}
        #endregion

        #region 设备中断处理
        /// <summary>
        /// 设备中断处理
        /// </summary>
        /// <param name="stationID">分站号</param>
        /// <param name="time">中断时间</param>
        /// <param name="state">中断状态</param>
        /// <param name="updateValues">分站需要更新的其他参数</param>
        public static Dictionary<string, object> DeviceInterruptPro(short stationID, DateTime time, DeviceRunState state, Dictionary<string, object> updateValues = null)
        {
            string strLog = "";//日志
            Dictionary<string, object> updateItems = new Dictionary<string, object>();
            try
            {
                DateTime dttStateTime = time;
                List<Jc_DefInfo> pointDefineItems = GetPointDefinesByStationID(stationID);
                Jc_DefInfo defItem = new Jc_DefInfo();
                Jc_DefInfo stationInfo = pointDefineItems.FirstOrDefault(a => a.DevPropertyID == (int)ItemDevProperty.Substation);

                if (stationInfo != null)
                {
                    if (stationInfo.Fzh >= 0 && stationInfo.Fzh < NmaxStationNum)
                    {
                        //更新分站电压值
                        if (updateValues != null)
                        {
                            if (updateValues.ContainsKey("Voltage"))
                            {
                                updateItems.Add("Voltage", updateValues["Voltage"]);
                            }
                        }
                        updateItems.Add("DttStateTime", dttStateTime);
                        //分站不为休眠且状态改变
                        if (((stationInfo.Bz4 & 0x02) != 0x02) && (stationInfo.State != (short)state))
                        {
                            #region 通道设备中断
                            if (state == DeviceRunState.EquipmentBiterror || state == DeviceRunState.EquipmentInterrupted)
                            {
                                //更新通道报警记录及实时值
                                if (pointDefineItems.Count > 1)
                                {
                                    //2018.8.24 by 分站中断结束报警，否则后面的 Alarm 赋值成0 ，正恢复正常无法解除之前的报警
                                    AlarmBusiness.BatchEndAnalogAlarm(pointDefineItems.Where(a => a.DevPropertyID == (int)ItemDevProperty.Analog).ToList(), time, false);
                                    AlarmBusiness.BatchEndDerailAlarm(pointDefineItems.Where(a => a.DevPropertyID == (int)ItemDevProperty.Derail).ToList(), time, false);
                                    AlarmBusiness.BatchEndControlAlarm(pointDefineItems.Where(a => a.DevPropertyID == (int)ItemDevProperty.Control).ToList(), time);

                                    Dictionary<string, Dictionary<string, object>> UpdateItemsList = new Dictionary<string, Dictionary<string, object>>();
                                    Dictionary<string, object> updateDeviceItems = null;
                                    foreach (Jc_DefInfo def in pointDefineItems)
                                    {
                                        if (def.DevPropertyID == (int)ItemDevProperty.Substation)
                                        {
                                            continue;   //分站不在此处处理，在后面进行处理
                                        }
                                        updateDeviceItems = new Dictionary<string, object>();
                                        updateDeviceItems.Add("Ssz", "");
                                        //updateDeviceItems.Add("Alarm",0);
                                        updateDeviceItems.Add("DttStateTime", dttStateTime);
                                        updateDeviceItems.Add("Voltage", 0);
                                        updateDeviceItems.Add("State", (short)DeviceRunState.EquipmentStateUnknow);
                                        updateDeviceItems.Add("DataState", (short)DeviceDataState.EquipmentStateUnknow);
                                        updateDeviceItems.Add("NCtrlSate", (int)ControlState.DataPowerUnKnowm);
                                        updateDeviceItems.Add("Zts", time);
                                        updateDeviceItems.Add("BCommDevTypeMatching", true); // 2017.7.23 by 此处置设备类型匹配，通讯正常后才会继续判断设备类型不匹配
                                        //2018.8.28 by 屏蔽下三行代码，以保证5分钟内分站通讯中断，之前记录的5分钟数据无法记录下来的问题
                                        //updateDeviceItems.Add("DoFiveMinData", false); //2017.7.26 by 
                                        //def.ClsFiveMinObj.m_nAllCount = 0;
                                        //updateDeviceItems.Add("ClsFiveMinObj", def.ClsFiveMinObj); //2017.7.26 by 分站中断，清楚当前的五分钟数据
                                        UpdateItemsList.Add(def.PointID, updateDeviceItems);

                                        //增加写密采和运行记录的功能
                                        SafetyHelper.CreateDentisyCollInfo(def, time, DeviceDataState.EquipmentStateUnknow, DeviceRunState.EquipmentStateUnknow);
                                        CreateRunLogInfo(def, time, (short)DeviceDataState.EquipmentStateUnknow, (short)DeviceRunState.EquipmentStateUnknow,
                                            EnumHelper.GetEnumDescription(DeviceDataState.EquipmentStateUnknow));
                                    }
                                    if (UpdateItemsList.Count > 0)
                                    {
                                        SafetyHelper.BatchUpdatePointDefineInfoByProperties(UpdateItemsList);
                                    }
                                }
                                //写分站密采记录
                                SafetyHelper.CreateDentisyCollInfo(stationInfo, time, (DeviceDataState)state, state);
                            }
                            #endregion

                            #region 写运行记录和实时记录

                            //正常
                            if (state == DeviceRunState.EquipmentAC ||
                                state == DeviceRunState.EquipmentIniting ||
                                state == DeviceRunState.EquipmentInfrareding)
                            {
                                // 写分站上一状态结束记录
                                AlarmBusiness.EndAlarmInfo(stationInfo, stationInfo.DataState, stationInfo.DttRunStateTime, time);
                                //更新分站实时值等内存信息
                                updateItems.Add("Alarm", 0);
                                updateItems.Add("Zts", time);
                                updateItems.Add("State", (short)state);
                                updateItems.Add("DataState", (short)state);
                                updateItems.Add("Ssz", EnumHelper.GetEnumDescription(state));
                                updateItems.Add("DttRunStateTime", time);
                                LogHelper.Info("3 更新分站" + stationInfo.Point + "状态,state=" + state + ",ssz=" + EnumHelper.GetEnumDescription(state));                                
                            }
                            //异常
                            else
                            {
                                //结束分站上一异常状态报警 2017.7.4 by
                                AlarmBusiness.EndSubstationAlarm(stationInfo, time);
                                // 更新分站实时值
                                if (state == DeviceRunState.EquipmentBiterror || state == DeviceRunState.EquipmentInterrupted)
                                {
                                    if (updateItems.ContainsKey("Voltage"))
                                    {
                                        updateItems["Voltage"] = 0;
                                    }
                                    else
                                    {
                                        updateItems.Add("Voltage", 0);
                                    }
                                }
                                if (state == DeviceRunState.EquipmentInterrupted)
                                {
                                    stationInfo.ClsCommObj.NCommandbz |= CommunicationCommandValue.Comm_QueryRealDataRequest;//置下发F命令标记
                                    if (updateItems.ContainsKey("ClsCommObj"))
                                    {
                                        updateItems["ClsCommObj"] = stationInfo.ClsCommObj;
                                    }
                                    else
                                    {
                                        updateItems.Add("ClsCommObj", stationInfo.ClsCommObj);
                                    }
                                }
                                updateItems.Add("Alarm", 1);
                                updateItems.Add("Zts", time);
                                updateItems.Add("State", (short)state);
                                updateItems.Add("DataState", (short)state);
                                updateItems.Add("Ssz", EnumHelper.GetEnumDescription(state));
                                updateItems.Add("DttRunStateTime", time);
                                LogHelper.Info("4 更新分站" + stationInfo.Point + "状态,state=" + state + ",ssz=" + EnumHelper.GetEnumDescription(state));
                                //写当前异常状态开始记录
                                AlarmBusiness.UpdateAlarmInfo(AlarmBusiness.CreateAlarmInfo(stationInfo, time, (DeviceDataState)state, (DeviceRunState)state, 0, EnumHelper.GetEnumDescription(state), 1), 1, false);
                            }
                            CreateRunLogInfo(stationInfo, time, (short)state, (short)state, EnumHelper.GetEnumDescription(state));
                            #endregion
                        }
                        if (updateItems.Count > 0)
                        {
                            UpdatePointDefineInfoByProperties(stationInfo.PointID, updateItems);
                        }
                    }
                }
                else
                {
                    LogHelper.Info("DeviceInterruptPro,未找到分站信息。" + (pointDefineItems.Count > 0 ? pointDefineItems[0].Fzh + "-" + pointDefineItems[0].Kh + "-" + pointDefineItems[0].Dzh : "Count=0"));
                }
            }
            catch (Exception ex)
            {
                strLog = ex.Message + "DeviceInterruptPro ";
                LogHelper.Error(ex);
            }
            return null;
        }
        #endregion

        #region ----未定义设备处理----

        /// <summary>
        /// 未定义设备处理
        /// </summary>
        /// <param name="fzh"></param>
        /// <param name="item"></param>
        public static AutomaticArticulatedDeviceInfo DoUnknownPoint(short fzh, RealDataItem item, DateTime createTime)
        {
            AutomaticArticulatedDeviceInfo automaticArticulatedDeviceInfo = new AutomaticArticulatedDeviceInfo();
            try
            {
                //判断该设备是否已在未定义设备缓存中
                short kh = 0;
                short dzh = 0;
                if (!short.TryParse(item.Channel, out kh)) { return null; }
                if (!short.TryParse(item.Address, out dzh)) { return null; }

                //AutomaticArticulatedDeviceCacheGetByConditionRequest automaticArticulatedDeviceCacheGetByConditionRequest = new AutomaticArticulatedDeviceCacheGetByConditionRequest();
                //automaticArticulatedDeviceCacheGetByConditionRequest.Pridicate = a => a.DeviceOnlyCode == item.SoleCoding;
                //var result = automaticArticulatedDeviceCacheService.GetAutomaticArticulatedDeviceCache(automaticArticulatedDeviceCacheGetByConditionRequest);
                //if (result.IsSuccess)
                {
                    //if (result.Data == null || result.Data.Count == 0)
                    {
                        if (item.SoleCoding == null)
                        {
                            WriteLogInfo(fzh.ToString() + "-" + kh.ToString() + "-" + dzh.ToString() + " 未定义，未上传DeviceOnlyCode");
                            return null; ;
                        }
                        if (item.SoleCoding.Trim() == "")
                        {
                            WriteLogInfo(fzh.ToString() + "-" + kh.ToString() + "-" + dzh.ToString() + " 未定义，未上传DeviceOnlyCode");
                            return null; ;
                        }

                        //设备不在未定义缓存中,新增
                        AutomaticArticulatedDeviceCacheAddRequest automaticArticulatedDeviceCacheAddRequest = new AutomaticArticulatedDeviceCacheAddRequest();
                        automaticArticulatedDeviceCacheAddRequest.AutomaticArticulatedDeviceInfo = new AutomaticArticulatedDeviceInfo();
                        automaticArticulatedDeviceCacheAddRequest.AutomaticArticulatedDeviceInfo.ID = IdHelper.CreateGuidId();
                        automaticArticulatedDeviceCacheAddRequest.AutomaticArticulatedDeviceInfo.StationNumber = fzh;
                        automaticArticulatedDeviceCacheAddRequest.AutomaticArticulatedDeviceInfo.ChanelNumber = kh;
                        automaticArticulatedDeviceCacheAddRequest.AutomaticArticulatedDeviceInfo.AddressNumber = dzh;
                        automaticArticulatedDeviceCacheAddRequest.AutomaticArticulatedDeviceInfo.BranchNumber = item.BranchNumber;
                        automaticArticulatedDeviceCacheAddRequest.AutomaticArticulatedDeviceInfo.DeviceModel = item.DeviceTypeCode;
                        automaticArticulatedDeviceCacheAddRequest.AutomaticArticulatedDeviceInfo.Value = item.RealData;
                        automaticArticulatedDeviceCacheAddRequest.AutomaticArticulatedDeviceInfo.DeviceOnlyCode = item.SoleCoding;
                        automaticArticulatedDeviceCacheAddRequest.AutomaticArticulatedDeviceInfo.ReciveTime = createTime;

                        automaticArticulatedDeviceCacheService.AddAutomaticArticulatedDeviceCache(automaticArticulatedDeviceCacheAddRequest);

                        automaticArticulatedDeviceInfo = automaticArticulatedDeviceCacheAddRequest.AutomaticArticulatedDeviceInfo;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("DoUnknownPoint Error【fzh = " + fzh + "】" + ex.Message + ex.StackTrace);
            }
            return automaticArticulatedDeviceInfo;
        }
        public static AutomaticArticulatedDeviceInfo AddUknownPoint(short fzh, RealDataItem item, DateTime createTime)
        {
            AutomaticArticulatedDeviceInfo automaticArticulatedDeviceInfo = new AutomaticArticulatedDeviceInfo();
            try
            {
                //判断该设备是否已在未定义设备缓存中
                short kh = 0;
                short dzh = 0;
                if (!short.TryParse(item.Channel, out kh)) { return null; }
                if (!short.TryParse(item.Address, out dzh)) { return null; }

                if (item.DeviceTypeCode==0)
                {
                    Basic.Framework.Logging.LogHelper.Info(fzh.ToString() + "-" + kh.ToString() + "-" + dzh.ToString() + " 未定义，上传设备型号编码为0，无法进行识别！");
                    return automaticArticulatedDeviceInfo;
                }

                if (item.SoleCoding == null || item.SoleCoding.Trim() == "" || item.SoleCoding.Trim() == "0")
                {
                    WriteLogInfo(fzh.ToString() + "-" + kh.ToString() + "-" + dzh.ToString() + " 未定义，未上传DeviceOnlyCode,系统按规则自动生成。");
                    item.SoleCoding = CreateSoleCoding(fzh.ToString(), item.BranchNumber.ToString(), kh.ToString()); //2017.12.15 by 没有唯一编码的自动创建一个特殊唯一编码
                }

                AutomaticArticulatedDeviceCacheGetByConditionRequest automaticArticulatedDeviceCacheGetByConditionRequest = new AutomaticArticulatedDeviceCacheGetByConditionRequest();
                automaticArticulatedDeviceCacheGetByConditionRequest.Pridicate = a => a.DeviceOnlyCode == item.SoleCoding;
                var result = automaticArticulatedDeviceCacheService.GetAutomaticArticulatedDeviceCache(automaticArticulatedDeviceCacheGetByConditionRequest);
                if (result.IsSuccess)
                {
                    if (result.Data == null || result.Data.Count == 0)
                    {
                        //设备不在未定义缓存中,新增
                        AutomaticArticulatedDeviceCacheAddRequest automaticArticulatedDeviceCacheAddRequest = new AutomaticArticulatedDeviceCacheAddRequest();
                        automaticArticulatedDeviceCacheAddRequest.AutomaticArticulatedDeviceInfo = new AutomaticArticulatedDeviceInfo();
                        automaticArticulatedDeviceCacheAddRequest.AutomaticArticulatedDeviceInfo.ID = IdHelper.CreateGuidId();
                        automaticArticulatedDeviceCacheAddRequest.AutomaticArticulatedDeviceInfo.StationNumber = fzh;
                        automaticArticulatedDeviceCacheAddRequest.AutomaticArticulatedDeviceInfo.ChanelNumber = kh;
                        automaticArticulatedDeviceCacheAddRequest.AutomaticArticulatedDeviceInfo.AddressNumber = dzh;
                        automaticArticulatedDeviceCacheAddRequest.AutomaticArticulatedDeviceInfo.BranchNumber = item.BranchNumber;
                        automaticArticulatedDeviceCacheAddRequest.AutomaticArticulatedDeviceInfo.DeviceModel = item.DeviceTypeCode;
                        automaticArticulatedDeviceCacheAddRequest.AutomaticArticulatedDeviceInfo.Value = item.RealData;
                        automaticArticulatedDeviceCacheAddRequest.AutomaticArticulatedDeviceInfo.DeviceOnlyCode = item.SoleCoding;
                        automaticArticulatedDeviceCacheAddRequest.AutomaticArticulatedDeviceInfo.ReciveTime = createTime;

                        automaticArticulatedDeviceCacheService.AddAutomaticArticulatedDeviceCache(automaticArticulatedDeviceCacheAddRequest);

                        automaticArticulatedDeviceInfo = automaticArticulatedDeviceCacheAddRequest.AutomaticArticulatedDeviceInfo;
                    }
                    else//更新自动挂接缓存的实时值
                    {
                        AutomaticArticulatedDeviceCacheUpdateRequest automaticArticulatedDeviceCacheUpRequest = new AutomaticArticulatedDeviceCacheUpdateRequest();
                        automaticArticulatedDeviceCacheUpRequest.AutomaticArticulatedDeviceInfo = result.Data[0];
                        automaticArticulatedDeviceCacheUpRequest.AutomaticArticulatedDeviceInfo.Value = item.RealData;
                        automaticArticulatedDeviceCacheService.UpdateAutomaticArticulatedDeviceCache(automaticArticulatedDeviceCacheUpRequest);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("AddUknownPoint Error【fzh = " + fzh + "】" + ex.Message + ex.StackTrace);
            }
            return automaticArticulatedDeviceInfo;
        }
        private static string CreateSoleCoding(string fzh, string branch, string kh)
        {
            //3 + 分站号3 + 01 + 01 + 分支号2 + 口号2
            return "3" + fzh.PadLeft(3, '0') + "01" + "01" + branch.PadLeft(2, '0') + kh.PadLeft(2, '0');
        }
        public static AutomaticArticulatedDeviceInfo DoUnknownPointNew(short fzh, SensorInfo item, DateTime createTime)
        {
            AutomaticArticulatedDeviceInfo automaticArticulatedDeviceInfo = new AutomaticArticulatedDeviceInfo();
            try
            {
                //判断该设备是否已在未定义设备缓存中
                short kh = 0;
                short dzh = 0;
                if (!short.TryParse(item.Channel, out kh)) { return null; }
                if (!short.TryParse(item.Address, out dzh)) { return null; }

                //AutomaticArticulatedDeviceCacheGetByConditionRequest automaticArticulatedDeviceCacheGetByConditionRequest = new AutomaticArticulatedDeviceCacheGetByConditionRequest();
                //automaticArticulatedDeviceCacheGetByConditionRequest.Pridicate = a => a.DeviceOnlyCode == item.SoleCoding;
                //var result = automaticArticulatedDeviceCacheService.GetAutomaticArticulatedDeviceCache(automaticArticulatedDeviceCacheGetByConditionRequest);
                //if (result.IsSuccess)
                {
                    //if (result.Data == null || result.Data.Count == 0)
                    {
                        if (item.SoleCoding == null)
                        {
                            WriteLogInfo(fzh.ToString() + "-" + kh.ToString() + "-" + dzh.ToString() + " 未定义，未上传DeviceOnlyCode");
                            return null; ;
                        }
                        if (item.SoleCoding.Trim() == "")
                        {
                            WriteLogInfo(fzh.ToString() + "-" + kh.ToString() + "-" + dzh.ToString() + " 未定义，未上传DeviceOnlyCode");
                            return null; ;
                        }
                        //设备不在未定义缓存中,新增
                        AutomaticArticulatedDeviceCacheAddRequest automaticArticulatedDeviceCacheAddRequest = new AutomaticArticulatedDeviceCacheAddRequest();
                        automaticArticulatedDeviceCacheAddRequest.AutomaticArticulatedDeviceInfo = new AutomaticArticulatedDeviceInfo();
                        automaticArticulatedDeviceCacheAddRequest.AutomaticArticulatedDeviceInfo.ID = IdHelper.CreateGuidId();
                        automaticArticulatedDeviceCacheAddRequest.AutomaticArticulatedDeviceInfo.StationNumber = fzh;
                        automaticArticulatedDeviceCacheAddRequest.AutomaticArticulatedDeviceInfo.ChanelNumber = kh;
                        automaticArticulatedDeviceCacheAddRequest.AutomaticArticulatedDeviceInfo.AddressNumber = dzh;
                        automaticArticulatedDeviceCacheAddRequest.AutomaticArticulatedDeviceInfo.BranchNumber = item.BranchNumber;
                        int deviceTypeCode = (byte)(Int32.Parse(item.SoleCoding) >> 24);//设备型号
                        automaticArticulatedDeviceCacheAddRequest.AutomaticArticulatedDeviceInfo.DeviceModel = deviceTypeCode;
                        automaticArticulatedDeviceCacheAddRequest.AutomaticArticulatedDeviceInfo.Value = "";//没有实时值
                        automaticArticulatedDeviceCacheAddRequest.AutomaticArticulatedDeviceInfo.DeviceOnlyCode = item.SoleCoding;
                        automaticArticulatedDeviceCacheAddRequest.AutomaticArticulatedDeviceInfo.ReciveTime = createTime;

                        automaticArticulatedDeviceCacheService.AddAutomaticArticulatedDeviceCache(automaticArticulatedDeviceCacheAddRequest);

                        automaticArticulatedDeviceInfo = automaticArticulatedDeviceCacheAddRequest.AutomaticArticulatedDeviceInfo;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("DoUnknownPoint Error【fzh = " + fzh + "】" + ex.Message);
            }
            return automaticArticulatedDeviceInfo;
        }
        /// <summary>
        /// 查询所有未定义测点
        /// </summary>
        /// <returns></returns>
        public static List<AutomaticArticulatedDeviceInfo> GetAllUnDefinePoint()
        {
            List<AutomaticArticulatedDeviceInfo> unDefineItems = new List<AutomaticArticulatedDeviceInfo>();
            AutomaticArticulatedDeviceCacheGetByConditionRequest automaticArticulatedDeviceCacheGetByConditionRequest = new AutomaticArticulatedDeviceCacheGetByConditionRequest();
            automaticArticulatedDeviceCacheGetByConditionRequest.Pridicate = a => 1 == 1;
            var result = automaticArticulatedDeviceCacheService.GetAutomaticArticulatedDeviceCache(automaticArticulatedDeviceCacheGetByConditionRequest);
            if (result.IsSuccess)
            {
                unDefineItems = result.Data;
            }
            return unDefineItems;
        }
        #endregion

        #region ----设备唯一编码处理----

        /// <summary>
        /// 更新分站唯一编码确认链表
        /// </summary>
        /// <param name="fzh"></param>
        /// <param name="chanels"></param>
        public static void UpdateDeviceOnlyCodeConfirmToStation(string stationPoint, List<short> chanels)
        {
            try
            {
                Jc_DefInfo station = SafetyHelper.GetPointDefinesByPoint(stationPoint);
                //if (station.SoleCodingChanels == null)
                {
                    station.SoleCodingChanels = new List<ControlItem>();
                }
                bool SoleCodingChanelsChange = false;
                ControlItem controlItem;
                Dictionary<string, object> updateItems = new Dictionary<string, object>();
                if (chanels.Count > 0)
                {
                    foreach (short chanel in chanels)
                    {
                        //if (station.SoleCodingChanels.FindIndex(a => a.Channel == chanel) < 0)
                        {
                            SoleCodingChanelsChange = true;
                            controlItem = new ControlItem();
                            controlItem.Channel = chanel;
                            controlItem.ControlType = 1;
                            station.SoleCodingChanels.Add(controlItem);
                        }
                    }
                    if (SoleCodingChanelsChange)
                    {
                        //下发F命令
                        station.ClsCommObj.NCommandbz |= CommunicationCommandValue.Comm_QueryRealDataRequest;
                        //根据新接口更新缓存         
                        updateItems.Add("SoleCodingChanels", station.SoleCodingChanels);
                        updateItems.Add("ClsCommObj", station.ClsCommObj);
                    }
                }
                else
                {
                    //不需要确认，不再下发F命令
                    station.ClsCommObj.NCommandbz &= (ushort)(0xFFFF - CommunicationCommandValue.Comm_QueryRealDataRequest);
                    updateItems.Add("SoleCodingChanels", station.SoleCodingChanels);
                    updateItems.Add("ClsCommObj", station.ClsCommObj);
                }
                if (updateItems.Count > 0)
                {
                    UpdatePointDefineInfoByProperties(station.PointID, updateItems);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("UpdateDeviceOnlyCodeConfirmToStation Error【" + stationPoint + "】" + ex.Message);
            }
        }
        /// <summary>
        /// 从未定义缓存移除未定义设备
        /// </summary>
        /// <param name="unDefineItems"></param>
        public static void RemoveUnDefineItems(List<AutomaticArticulatedDeviceInfo> unDefineItems)
        {
            AutomaticArticulatedDeviceCacheDeleteRequest automaticArticulatedDeviceCacheDeleteRequest;
            unDefineItems.ForEach(item =>
            {
                automaticArticulatedDeviceCacheDeleteRequest = new AutomaticArticulatedDeviceCacheDeleteRequest();
                automaticArticulatedDeviceCacheDeleteRequest.AutomaticArticulatedDeviceInfo = item;
                automaticArticulatedDeviceCacheService.DeleteAutomaticArticulatedDeviceCache(automaticArticulatedDeviceCacheDeleteRequest);
            });

        }
        #endregion

        #region ----辅助处理方法----
        /// <summary>
        /// 或取网关回发的设备信息及实时值
        /// </summary>
        /// <param name="devModelID"></param>
        /// <param name="ssz"></param>
        /// <returns></returns>
        public static string GetRealTypeInfoStr(int devModelID, string ssz)
        {
            string realTypeInfo = "";

            Jc_DevInfo devInfo = SafetyHelper.GetDeviceInfoByDevModelID(devModelID);
            if (devInfo != null)
            {
                realTypeInfo = devInfo.Name + "【" + ssz + "】";
            }
            //else
            //{
            //    LogHelper.Error("设备回发类型编码" + PointDefineInfo.Point + "----" + devModelID + "，但数据库中未找到对应设备！");
            //}

            return realTypeInfo;
        }
        public static void WriteLogInfo(string logInfo)
        {
            LogHelper.Info(logInfo);
        }

        /// <summary>
        /// 综合用户设置的状态与设备本身的状态，获取要写入数据库的状态(现在bz4暂时不用，值为0)
        /// </summary>
        /// <param name="state">设备本身状态</param>
        /// <param name="bz4">用户定义的状态</param>
        /// <returns></returns>
        public static short GetState(DeviceRunState state, int bz4)
        {
            short myState = (short)state;

            if ((bz4 & 0x02) == 0x02)
            {
                myState = (short)DeviceRunState.EquipmentSleep;
            }
            else if ((bz4 & 0x04) == 0x04)
            {
                myState = (short)DeviceRunState.EquipmentDebugging;
            }
            else if ((bz4 & 0x08) == 0x08)
            {
                myState = (short)DeviceRunState.EquipmentAdjusting;
            }

            return myState;
        }

        /// <summary>
        /// 获取开关量此状态显示值与量澡报警
        /// </summary>
        /// <param name="def"></param>
        /// <param name="datastate"></param>
        /// <param name="Ssz">显示值</param>
        /// <param name="isAlarm">是否报警</param>
        public static bool GetDerailShowInfo(Jc_DefInfo def, DeviceDataState datastate, ref string Ssz, ref int isAlarm)
        {
            bool flag = false;

            flag = true;
            Ssz = "";
            isAlarm = 0;
            //开关量实时值及报警状态判断
            switch (datastate)
            {
                case DeviceDataState.DataDerailState0:
                    Ssz = def.Bz6;
                    if ((def.K8 & 0x01) == 0x01)
                    {
                        isAlarm = 1;
                    }
                    break;
                case DeviceDataState.DataDerailState1:
                    Ssz = def.Bz7;
                    if ((def.K8 & 0x02) == 0x02)
                    {
                        isAlarm = 1;
                    }
                    break;
                case DeviceDataState.DataDerailState2:
                    Ssz = def.Bz8;
                    if ((def.K8 & 0x04) == 0x04)
                    {
                        isAlarm = 1;
                    }
                    break;
                default:
                    isAlarm = 1;
                    break;
            }
            //开关量逻辑报警判断
            isAlarm = CheckDerailLogicRelationState(isAlarm, def);
            return flag;
        }

        /// <summary>
        /// 判断开关量逻辑关联测点状态
        /// </summary>
        /// <param name="pointAlarmState">当前开关量报警状态</param>
        private static int CheckDerailLogicRelationState(int pointAlarmState, Jc_DefInfo def)
        {
            //逻辑关联处理 与或（1,2）认为此开关量有关联测点
            if (def.K4 == 1 || def.K4 == 2)
            {
                Jc_DefInfo pointItem;
                if (def.Fzh > 0 && def.Fzh < SafetyHelper.NmaxStationNum && def.K5 > 0)
                {
                    PointDefineCacheGetByConditonRequest stationquest = new PointDefineCacheGetByConditonRequest
                    {
                        IsQueryFromWriteCache = true,
                        Predicate = point => point.Fzh == def.Fzh && point.Kh == def.K5 && point.Dzh == def.K6 && point.DevPropertyID == (int)DeviceProperty.Derail
                    };
                    var relationresponse = pointDefineCacheService.GetPointDefineCache(stationquest);
                    //如果此开关量有关联测点,则判断关联测点的报警状态
                    if (relationresponse != null && relationresponse.IsSuccess && relationresponse.Data != null && relationresponse.Data.Any())
                    {
                        pointItem = relationresponse.Data.FirstOrDefault();
                        int relationalarm = 0;
                        if ((pointItem.DataState == (short)DeviceDataState.DataDerailState0) && ((pointItem.K8 & 0x01) == 0x01) ||
                            (pointItem.DataState == (short)DeviceDataState.DataDerailState1) && ((pointItem.K8 & 0x02) == 0x02) ||
                            (pointItem.DataState == (short)DeviceDataState.DataDerailState2) && ((pointItem.K8 & 0x04) == 0x04))
                            relationalarm = 1;

                        //当前开关量与关联开关量与或运算,判断当前开关量是否报警
                        if (def.K4 == 2)
                        {
                            pointAlarmState |= relationalarm;
                        }
                        else if (def.K4 == 1)
                        {
                            pointAlarmState &= relationalarm;
                        }
                    }
                }
            }
            //PointDefineInfo.Alarm = (short)pointAlarmState;
            return pointAlarmState;
        }

        /// <summary>
        /// 获取控制量的显示及报警信息
        /// </summary>
        /// <param name="def"></param>
        /// <param name="datastate"></param>
        /// <param name="Ssz"></param>
        /// <param name="isAlarm"></param>
        /// <returns></returns>
        public static bool GetControlShowInfo(Jc_DefInfo def, DeviceDataState datastate, ref string Ssz, ref int isAlarm)
        {
            bool flag = false;

            flag = true;
            Ssz = "断线";
            isAlarm = 0;
            switch (datastate)
            {
                case DeviceDataState.DataControlState0:
                    Ssz = def.Bz6;
                    if ((def.K8 & 0x01) == 0x01)
                    {
                        isAlarm = 1;
                    }
                    break;
                case DeviceDataState.DataControlState1:
                    Ssz = def.Bz7;
                    if ((def.K8 & 0x02) == 0x02)
                    {
                        isAlarm = 1;
                    }
                    break;
                default:
                    isAlarm = 1;
                    break;
            }

            return flag;
        }

        public static decimal GetCumulativeCount(decimal cumulativeCount)
        {
            return (cumulativeCount == 0 ? 1 : cumulativeCount);
        }

        /// <summary>
        /// 获取所有配置信息
        /// </summary>
        /// <returns></returns>
        public static List<SettingInfo> GetAllSetting()
        {
            SettingCacheGetAllRequest settingCacheGetAllRequest = new SettingCacheGetAllRequest();

            var result = settingCacheService.GetAllSettingCache(settingCacheGetAllRequest);
            if (result.Data != null && result.IsSuccess)
            {
                return result.Data;
            }

            return null;
        }

        /// <summary>
        /// 根据主键获取配置信息
        /// </summary>
        /// <param name="StrKey"></param>
        /// <returns></returns>
        public static SettingInfo GetSettingByKeyStr(string StrKey)
        {
            SettingCacheGetByKeyRequest settingCacheGetByKeyRequest = new SettingCacheGetByKeyRequest();
            settingCacheGetByKeyRequest.StrKey = StrKey;
            var result = settingCacheService.GetSettingCacheByKey(settingCacheGetByKeyRequest);
            if (result.Data != null && result.IsSuccess)
            {
                return result.Data;
            }

            return null;
        }
        /// <summary>
        /// 当前设备状态是否是异常状态(异常 true ,不是异常 false)
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static bool isAbnormalState(DeviceRunState state)
        {
            bool flag = false;

            if (state == DeviceRunState.EquipmentDown ||
                state == DeviceRunState.EquipmentHeadDown ||
                state == DeviceRunState.EquipmentOverrange ||
                state == DeviceRunState.EquipmentUnderrange ||
                state == DeviceRunState.EquipmentBiterror ||
                state == DeviceRunState.EquipmentInterrupted ||
                state == DeviceRunState.EquipmentStateUnknow ||
                state == DeviceRunState.EquipmentTypeError)
            {
                flag = true;
            }

            return flag;
        }

        /// <summary>
        /// 根据异常设备状态获取设备数据状态(头子断线、类型为匹配、通讯误码、设备中断、设备未知、设备断线 = 断线；上溢 = 上溢；浮漂 = 浮漂)
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static DeviceDataState GetDataStateByAbnormalState(DeviceRunState state)
        {
            DeviceDataState datastate = DeviceDataState.EquipmentDown;

            switch (state)
            {
                case DeviceRunState.EquipmentHeadDown:
                case DeviceRunState.EquipmentTypeError:
                case DeviceRunState.EquipmentBiterror:
                case DeviceRunState.EquipmentInterrupted:
                case DeviceRunState.EquipmentStateUnknow:
                case DeviceRunState.EquipmentDown:
                    datastate = DeviceDataState.EquipmentDown;
                    break;
                case DeviceRunState.EquipmentOverrange:
                    datastate = DeviceDataState.EquipmentOverrange;
                    break;
                case DeviceRunState.EquipmentUnderrange:
                    datastate = DeviceDataState.EquipmentUnderrange;
                    break;
            }

            return datastate;
        }

        /// <summary>
        /// 根据异常设备类型，判断当前异常类型数据何种交叉控制，返回 ControlType.NoControl 说明此设备异常无控制，此处只会返回故障控制和断线控制（断线、通讯中断、头子断线、通讯误码、类型不匹配进行断线控制，上溢、浮漂当做故障控制）
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static ControlType GetControlTypeByAbnormalState(DeviceRunState state)
        {
            ControlType controlType = ControlType.NoControl;

            switch (state)
            {
                //case DeviceRunState.EquipmentTypeError:
                case DeviceRunState.EquipmentBiterror:
                case DeviceRunState.EquipmentHeadDown:
                case DeviceRunState.EquipmentInterrupted:
                case DeviceRunState.EquipmentDown:
                    //断线控制
                    controlType = ControlType.ControlLineDown;
                    break;
                case DeviceRunState.EquipmentOverrange:
                case DeviceRunState.EquipmentUnderrange:
                    //故障控制
                    controlType = ControlType.ControlErro;
                    break;
            }

            return controlType;
        }

        /// <summary>
        /// 当前数据状态是否是异常状态(异常 true ,不是异常 false)
        /// </summary>
        /// <param name="datastate"></param>
        /// <returns></returns>
        public static bool isAbnormalDataState(DeviceDataState datastate)
        {
            bool flag = false;

            if (datastate == DeviceDataState.EquipmentDown ||
                datastate == DeviceDataState.EquipmentOverrange ||
                datastate == DeviceDataState.EquipmentUnderrange ||
                datastate == DeviceDataState.EquipmentStateUnknow) //若首次进行数据处理(数据状态为默认值-未知)，而之前数据库里已有异常控制，同样需要先解除控制。2017.7.5 by
            {
                flag = true;
            }

            return flag;
        }

        #endregion

        #region ----远程升级相关----

        public static StationUpdateItem GetStationItem(int fzh)
        {
            GetStationItemRequest getStationItemRequest = new GetStationItemRequest();
            getStationItemRequest.fzh = fzh;
            var result = stationUpdateService.GetStationItem(getStationItemRequest);
            if (result != null && result.IsSuccess)
            {
                return result.Data;
            }
            return null;
        }

        #endregion

        #region ----JC_DEF----
        ///// <summary>
        ///// 更新设备的下发初始化标记
        ///// </summary>
        //public static void UpdateJC_DEF_Bz19(Jc_DefInfo def)
        //{
        //    List<Jc_DefInfo> defItems = new List<Jc_DefInfo>();
        //    defItems.Add(def);
        //    // 调用接口更新设备唯一编码到数据库
        //    System.Data.DataColumn[] cols = new System.Data.DataColumn[1];
        //    cols[0] = new System.Data.DataColumn("Bz19");
        //    var _jc_Def = ObjectConverter.CopyList<Jc_DefInfo, Jc_DefModel>(defItems);
        //    pointDefineRepository.BulkUpdate("KJ_DeviceDefInfo", _jc_Def, cols, "ID");
        //}
        /// <summary>
        /// 更新分站的Crc
        /// </summary>
        /// <param name="station"></param>
        public static void UpdateStation_Crc(Jc_DefInfo station)
        {
            List<Jc_DefInfo> defItems = new List<Jc_DefInfo>();
            defItems.Add(station);
            // 调用接口更新设备唯一编码到数据库
            System.Data.DataColumn[] cols = new System.Data.DataColumn[1];
            cols[0] = new System.Data.DataColumn("Bz19");
            var _jc_Def = ObjectConverter.CopyList<Jc_DefInfo, Jc_DefModel>(defItems);
            pointDefineRepository.BulkUpdate("KJ_DeviceDefInfo", _jc_Def, cols, "ID");
        }
        #endregion

        public static Jc_MacInfo GetMacItemByMac(string mac)
        {
            Jc_MacInfo item = null;

            List<Jc_MacInfo> macItems = new List<Jc_MacInfo>();

            var defResponse = networkModuleCacheService.GetAllNetworkModuleCache(new NetworkModuleCacheGetAllRequest());
            if (defResponse.IsSuccess)
            {
                macItems = defResponse.Data;
                item = macItems.FirstOrDefault(a => a.MAC == mac);
            }

            return item;
        }

        public static void UpdateNetworkModeuleCacheByPropertys(string mac, Dictionary<string, object> updateItems)
        {

            NetworkModuleCacheUpdatePropertiesRequest networkModuleCacheUpdatePropertiesRequest = new NetworkModuleCacheUpdatePropertiesRequest();
            networkModuleCacheUpdatePropertiesRequest.Mac = mac;
            networkModuleCacheUpdatePropertiesRequest.UpdateItems = updateItems;
            networkModuleCacheService.UpdateNetworkInfo(networkModuleCacheUpdatePropertiesRequest);
        }
    }
}
