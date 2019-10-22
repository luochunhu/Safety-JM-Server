using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Basic.Framework.Common;
using Basic.Framework.Data;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;

using Sys.Safety.Model;
using Sys.Safety.Request.Cache;
using Sys.Safety.ServiceContract.Cache;
using Sys.Safety.Enums;
using Sys.Safety.DataContract.CommunicateExtend;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request.NetworkModule;
using Sys.Safety.Request.PointDefine;

namespace Sys.Safety.Processing.Statistics
{
    /// <summary>
    /// 放电时直流状态下每5分钟消耗的电量统计  20180125
    /// </summary>
    public static class ElectricityStatistics
    {
        private static DateTime _lastRunTime = new DateTime();

        private static Thread _handleThread;

        private static readonly IPointDefineCacheService PointDefineCacheService = ServiceFactory.Create<IPointDefineCacheService>();
        private static readonly IPointDefineService pointDefinService = ServiceFactory.Create<IPointDefineService>();
        private static readonly INetworkModuleCacheService networkModuleService = ServiceFactory.Create<INetworkModuleCacheService>();

        private static bool _isRun;
        /// <summary>
        /// 测点耗电量统计临时缓存
        /// </summary>
        private static List<PointElectricityInfo> pointElectricityInfoList = new List<PointElectricityInfo>();


        public static void Start()
        {
            LogHelper.Info("【ElectricityStatistics】充放电电量统计线程开启。");

            _isRun = true;
            if (_handleThread == null || (_handleThread != null && !_handleThread.IsAlive))
            {
                _handleThread = new Thread(HandleThreadFun);
                _handleThread.Start();
            }
        }

        /// <summary>
        ///     停止模块  20170413
        /// </summary>
        public static void Stop()
        {
            LogHelper.Info("【ElectricityStatistics】充放电电量统计线程结束。");
            _isRun = false;
            while (true)
            {
                if (_isRun) break;
                Thread.Sleep(1000);
            }
        }

        private static void HandleThreadFun()
        {
            while (_isRun)
            {
                try
                {
                    #region 计算交换机电源箱信息
                    NetworkModuleCacheGetByConditonRequest networkModuleCacheRequest = new NetworkModuleCacheGetByConditonRequest();
                    networkModuleCacheRequest.Predicate = a => a.Bz4 == "1";
                    List<Jc_MacInfo> macList = networkModuleService.GetNetworkModuleCache(networkModuleCacheRequest).Data;//所有挂接了智能电源箱的设备
                    foreach (Jc_MacInfo mac in macList)
                    {
                        if (mac.State == (short)DeviceDataState.EquipmentDC)//交换机进入直流状态，表示电源箱已处于放电状态
                        {
                            List<PointElectricityInfo> pointElectricityInfo = pointElectricityInfoList.FindAll(a => a.PointId == mac.MAC);
                            if (pointElectricityInfo.Count < 1)//表示第一次放电，则记录放电的开始时间
                            {
                                sendD(16, mac.MAC);  //下发获取电源箱数据命令
                                Thread.Sleep(10000);

                                PointElectricityInfo tempPointElectricityInfo = new PointElectricityInfo();
                                tempPointElectricityInfo.StartTime = DateTime.Now;
                                tempPointElectricityInfo.EndTime = DateTime.Parse("1900-01-01 00:00:00");
                                if (mac.BatteryItems != null)
                                {
                                    foreach (BatteryItem battery in mac.BatteryItems)
                                    {
                                        tempPointElectricityInfo.Channel = battery.Channel;
                                        tempPointElectricityInfo.PowerConsumption = battery.PowerPackVOL;
                                        if (pointElectricityInfoList.FindAll(a => a.Channel == battery.Channel && a.PointId == mac.MAC).Count < 1)
                                        {
                                            pointElectricityInfoList.Add(tempPointElectricityInfo);
                                        }
                                    }
                                }                                

                            }
                            else//表示之前已经添加了第一次放电的时间，判断放电是否达到了5分钟
                            {
                                if (pointElectricityInfo[0].EndTime != DateTime.Parse("1900-01-01 00:00:00"))
                                {
                                    continue;//如果之前已经计算过，则不用重复计算
                                }
                                TimeSpan ts = DateTime.Now - pointElectricityInfo[0].StartTime;
                                if (ts.TotalMinutes >= 5)//到5分钟时，计算当前5分钟消耗的电量，并存储
                                {
                                    sendD(16, mac.MAC);  //下发获取电源箱数据命令
                                    Thread.Sleep(10000);

                                    Dictionary<string, object> updateItems = new Dictionary<string, object>();
                                    List<BatteryPowerConsumption> batteryPowerConsumptions = new List<BatteryPowerConsumption>();
                                    for (int i = 0; i < mac.BatteryItems.Count; i++)
                                    {
                                        int powerConsumption = pointElectricityInfo[i].PowerConsumption - mac.BatteryItems[i].PowerPackVOL;//得到当前5分钟的电量衰减量
                                        //将5分钟电量衰减参数更新到实时缓存                                        
                                        BatteryPowerConsumption batteryPowerConsumption = new BatteryPowerConsumption();
                                        batteryPowerConsumption.Channel = pointElectricityInfo[i].Channel;
                                        batteryPowerConsumption.PowerConsumption = powerConsumption;
                                        batteryPowerConsumptions.Add(batteryPowerConsumption);
                                    }
                                    updateItems.Add("BatteryPowerConsumptions", batteryPowerConsumptions);
                                    UpdateNetWorkInfoByProperties(mac.MAC, updateItems);

                                    //更新临时缓存的结束时间，后续不用再重复计算
                                    foreach (PointElectricityInfo temp in pointElectricityInfo)
                                    {
                                        temp.EndTime = DateTime.Now;
                                    }
                                }
                            }
                        }
                        else//交换机状态改变，重新计算
                        {
                            //清除临时缓存，并重新计算
                            foreach (PointElectricityInfo temp in pointElectricityInfoList)
                            {
                                if (temp.PointId == mac.MAC)
                                {
                                    pointElectricityInfoList.Remove(temp);
                                }
                            }
                        }
                    }
                    #endregion

                    #region 计算分站电源箱信息
                    PointDefineCacheGetByConditonRequest pointDefineCacheRequest = new PointDefineCacheGetByConditonRequest();
                    pointDefineCacheRequest.Predicate = a => a.DevPropertyID == 0 && (a.Bz3 & 0x8) == 0x8;
                    List<Jc_DefInfo> stationList = PointDefineCacheService.GetPointDefineCache(pointDefineCacheRequest).Data;
                    foreach (Jc_DefInfo station in stationList)
                    {
                        if (station.DataState == (short)DeviceDataState.EquipmentDC)//分站进入直流状态，表示电源箱已处于放电状态
                        {
                            List<PointElectricityInfo> pointElectricityInfo = pointElectricityInfoList.FindAll(a => a.PointId == station.PointID);
                            if (pointElectricityInfo.Count < 1)//表示第一次放电，则记录放电的开始时间
                            {
                                sendD(0, station.Fzh.ToString());//下发获取电源箱命令
                                Thread.Sleep(10000);

                                PointElectricityInfo tempPointElectricityInfo = new PointElectricityInfo();
                                tempPointElectricityInfo.PointId = station.PointID;
                                tempPointElectricityInfo.StartTime = DateTime.Now;
                                tempPointElectricityInfo.EndTime = DateTime.Parse("1900-01-01 00:00:00");
                                if (station.BatteryItems != null)
                                {
                                    foreach (BatteryItem battery in station.BatteryItems)
                                    {
                                        tempPointElectricityInfo.Channel = battery.Channel;
                                        tempPointElectricityInfo.PowerConsumption = battery.PowerPackVOL;
                                        if (pointElectricityInfoList.FindAll(a => a.Channel == battery.Channel && a.PointId == station.PointID).Count < 1)
                                        {
                                            pointElectricityInfoList.Add(tempPointElectricityInfo);
                                        }
                                    }
                                }
                                else
                                {
                                    sendD(0, station.Fzh.ToString());
                                }

                            }
                            else//表示之前已经添加了第一次放电的时间，判断放电是否达到了5分钟
                            {
                                if (pointElectricityInfo[0].EndTime != DateTime.Parse("1900-01-01 00:00:00"))
                                {
                                    continue;//如果之前已经计算过，则不用重复计算
                                }
                                TimeSpan ts = DateTime.Now - pointElectricityInfo[0].StartTime;
                                if (ts.TotalMinutes >= 5)//到5分钟时，计算当前5分钟消耗的电量，并存储
                                {
                                    sendD(0, station.Fzh.ToString());//下发获取电源箱命令
                                    Thread.Sleep(10000);

                                    Dictionary<string, object> updateItems = new Dictionary<string, object>();
                                    List<BatteryPowerConsumption> batteryPowerConsumptions = new List<BatteryPowerConsumption>();
                                    for (int i = 0; i < station.BatteryItems.Count; i++)
                                    {
                                        int powerConsumption = pointElectricityInfo[i].PowerConsumption - station.BatteryItems[i].PowerPackVOL;//得到当前5分钟的电量衰减量
                                        //将5分钟电量衰减参数更新到实时缓存                                        
                                        BatteryPowerConsumption batteryPowerConsumption = new BatteryPowerConsumption();
                                        batteryPowerConsumption.Channel = pointElectricityInfo[i].Channel;
                                        batteryPowerConsumption.PowerConsumption = powerConsumption;
                                        batteryPowerConsumptions.Add(batteryPowerConsumption);
                                    }
                                    updateItems.Add("BatteryPowerConsumptions", batteryPowerConsumptions);
                                    UpdatePointDefineInfoByProperties(station.PointID, updateItems);

                                    //更新临时缓存的结束时间，后续不用再重复计算
                                    foreach (PointElectricityInfo temp in pointElectricityInfo)
                                    {
                                        temp.EndTime = DateTime.Now;
                                    }
                                }
                            }
                        }
                        else//分站状态改变，重新计算
                        {
                            //清除临时缓存，并重新计算
                            foreach (PointElectricityInfo temp in pointElectricityInfoList)
                            {
                                if (temp.PointId == station.PointID)
                                {
                                    pointElectricityInfoList.Remove(temp);
                                }
                            }
                        }
                    }
                    #endregion
                }
                catch (Exception e)
                {
                    LogHelper.Error(e.ToString());
                }

                Thread.Sleep(5000);
            }
            _isRun = true;
            LogHelper.Info("【ElectricityStatistics】充放电电量统计线程结束成功。");
        }

        /// <summary>
        /// 获取电源箱状态
        /// </summary>
        /// <param name="m">设备类型</param>
        /// <param name="fzhormac">分站传分站号，交换机传MAC</param>
        public static void sendD(int m, string fzhormac)
        {
            SendDComReqest sendDComReqest = new SendDComReqest
            {
                queryBatteryRealDataItems = new List<BatteryControlItem>()
                {
                    new BatteryControlItem()
                    {
                        DevProID = m,
                        FzhOrMac = fzhormac,
                        controlType = 0
                    }
                }
            };

            var res = pointDefinService.SendQueryBatteryRealDataRequest(sendDComReqest);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
        }
        public static void UpdatePointDefineInfoByProperties(string pointID, Dictionary<string, object> paramater)
        {
            try
            {
                DefineCacheUpdatePropertiesRequest defineCacheUpdatePropertiesRequest = new DefineCacheUpdatePropertiesRequest();
                defineCacheUpdatePropertiesRequest.PointID = pointID;
                defineCacheUpdatePropertiesRequest.UpdateItems = paramater;
                PointDefineCacheService.UpdatePointDefineInfo(defineCacheUpdatePropertiesRequest);
            }
            catch (Exception ex)
            {
                LogHelper.Error("ElectricityStatistics-UpdatePointDefineInfoByProperties Error【pointID = " + pointID + "】" + ex.Message);
            }
        }
        public static void UpdateNetWorkInfoByProperties(string pointID, Dictionary<string, object> paramater)
        {
            try
            {
                NetworkModuleCacheUpdatePropertiesRequest pointDefineCacheRequest = new NetworkModuleCacheUpdatePropertiesRequest();
                pointDefineCacheRequest.Mac = pointID;
                pointDefineCacheRequest.UpdateItems = paramater;
                networkModuleService.UpdateNetworkInfo(pointDefineCacheRequest);
            }
            catch (Exception ex)
            {
                LogHelper.Error("ElectricityStatistics-UpdateNetWorkInfoByProperties Error【Mac = " + pointID + "】" + ex.Message);
            }
        }
    }

    public class PointElectricityInfo
    {
        /// <summary>
        /// 测点Id
        /// </summary>
        public string PointId { get; set; }
        /// <summary>
        /// 电源箱地址号
        /// </summary>
        public string Channel { get; set; }
        /// <summary>
        /// 统计开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 统计结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 耗电量
        /// </summary>
        public byte PowerConsumption { get; set; }
    }
}