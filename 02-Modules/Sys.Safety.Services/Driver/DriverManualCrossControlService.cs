using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.CommunicateExtend;
using Sys.Safety.Enums;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.Driver;
using Sys.Safety.ServiceContract.Cache;
using Sys.Safety.ServiceContract.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sys.Safety.Services.Driver
{
    /// 作者：
    /// 时间：2017-06-03
    /// 描述：驱动手动交叉控制接口
    /// 修改记录
    /// 2017-06-03
    /// </summary>
    public class DriverManualCrossControlService : IDriverManualCrossControlService
    {
        private readonly IManualCrossControlCacheService manualCrossControlCacheService;
        private readonly IPointDefineCacheService poingDefineCacheService;

        public DriverManualCrossControlService()
        {
            manualCrossControlCacheService = ServiceFactory.Create<IManualCrossControlCacheService>();
            poingDefineCacheService = ServiceFactory.Create<IPointDefineCacheService>();
        }
        /// <summary>
        /// 重新加载分站控制信息
        /// </summary>
        /// <param name="reLoadRequest"></param>
        public void ReLoad(DriverManualCrossControlReLoadRequest reLoadRequest)
        {
            ManualCrossControlCacheGetAllRequest manualCrossControlGetAllRequest = new ManualCrossControlCacheGetAllRequest();
            PointDefineCacheGetByConditonRequest pointDefineStationRequest = new PointDefineCacheGetByConditonRequest();
            //分站信息是否有更新
            List<string> StationControlChangeList = new List<string>();
            //获取所有分站
            pointDefineStationRequest.Predicate = point => point.DevPropertyID == (int)DeviceProperty.Substation;
            var stationsResponse = poingDefineCacheService.GetPointDefineCache(pointDefineStationRequest);
            List<Jc_DefInfo> stationPointInsos = new List<Jc_DefInfo>();
            if (stationsResponse != null && stationsResponse.IsSuccess && stationsResponse.Data != null)
            {
                stationPointInsos = stationsResponse.Data;
            }

            //获取所有手动交叉控制缓存
            var manualCrossControlResponse = manualCrossControlCacheService.GetAllManualCrossControlCache(manualCrossControlGetAllRequest);
            List<Jc_JcsdkzInfo> manualCrossControlInfos = new List<Jc_JcsdkzInfo>();
            if (manualCrossControlResponse != null && manualCrossControlResponse.IsSuccess && manualCrossControlResponse.Data.Any())
            {
                manualCrossControlInfos = manualCrossControlResponse.Data;
            }

            #region ----加载控制列表----
            uint oldControlInt = 0;
            uint newControlInt = 0;
            bool updateFlag = false;    //分站结构体是否需要更新
            List<Jc_DefInfo> updateStations = new List<Jc_DefInfo>();
            foreach (Jc_DefInfo station in stationPointInsos)
            {
                if (station.DeviceControlItems == null)
                    station.DeviceControlItems = new List<ControlItem>();

                updateFlag = false;
                oldControlInt = GetControlInt(station.DeviceControlItems);
                station.DeviceControlItems.Clear();
                var stationDischargeControl = manualCrossControlInfos.FirstOrDefault(m => m.Type == (int)ControlType.ControlDisCharge && short.Parse(m.Bkpoint.Substring(0, 3)) == station.Fzh);
                #region ----------------放电命令判断----------------

                if (stationDischargeControl == null && station.BDisCharge == 2)
                {
                    //==null 表示不放电，若此时状态是放电，则更新放电状态
                    updateFlag = true;
                    station.BDisCharge = 1;
                    station.realControlCount++;
                    station.ClsCommObj.NCommandbz |= 0x80; // todo  此处走0x0002
                }
                else if (stationDischargeControl != null && station.BDisCharge != 2)
                {
                    //不为null表示要放电，若此时状态是不放电，则更新放电状态
                    updateFlag = true;
                    station.BDisCharge = 2;
                    station.realControlCount++;
                    station.ClsCommObj.NCommandbz |= 0x80;
                }

                #endregion
                #region ----------------三分封电闭锁强制解锁命令判断----------------

                var sationGasThreeUnlockControl = manualCrossControlInfos.FirstOrDefault(m => m.Type == (int)ControlType.GasThreeUnlockControl && short.Parse(m.Bkpoint.Substring(0, 3)) == station.Fzh);
                if (sationGasThreeUnlockControl == null && station.GasThreeUnlockContro == 1)
                {
                    //交叉控制表中已删除，但内存还是解锁状态
                    updateFlag = true;
                    station.GasThreeUnlockContro = 0;
                    station.realControlCount++;
                    station.ClsCommObj.NCommandbz |= 0x80;

                }
                else if (sationGasThreeUnlockControl != null && station.GasThreeUnlockContro == 0)
                {
                    //交叉控制表中已增加，但内存是不解锁状态
                    updateFlag = true;
                    station.GasThreeUnlockContro = 1;
                    station.realControlCount++;
                    station.ClsCommObj.NCommandbz |= 0x80;
                }

                #endregion
                #region ----------------手动交叉控制命令判断（Type < 50 表示需要直接加载到分站控制链表的控制命令）----------------
                var stationcontrolInfos = manualCrossControlInfos.Where(m => short.Parse(m.Bkpoint.Substring(0, 3)) == station.Fzh && m.Type <= 50 && m.Type != 12).ToList();//type=12 是大数据分析的传感器分级报警控制，在下面单独处理
                foreach (Jc_JcsdkzInfo jckzItem in stationcontrolInfos)
                {
                    int controlType = 1;
                    if (jckzItem.Type == (short)ControlType.RemoveLocalControl)
                    {
                        controlType = 2;//表示强制解控
                    }
                    //重新根据JC_JCKZ表重新生成控制列表
                    ControlItem controlItem = new ControlItem
                    {
                        Channel = short.Parse(jckzItem.Bkpoint.Substring(4, 2)),
                        ControlType = controlType
                    };
                    station.DeviceControlItems.Add(controlItem);
                }
                newControlInt = GetControlInt(station.DeviceControlItems);
                if (oldControlInt != newControlInt)
                {
                    //本次控制字与之前控制字不一样，更新内存下发控制标记
                    updateFlag = true;
                    station.realControlCount++;
                    station.ClsCommObj.NCommandbz |= 0x80;
                }
                #endregion
                #region ----------------分级报警控制列表---------------------
                List<GradingAlarmItem> oldControls = station.GradingAlarmItems;
                List<GradingAlarmItem> newControls = GetGradeControls(station.Fzh, manualCrossControlInfos);
                if (CompareGradeControls(oldControls, newControls) == false)
                {
                    updateFlag = true;
                    station.GradingAlarmCount++;
                    station.GradingAlarmItems = newControls;
                    station.GradingAlarmTime = DateTime.Now;
                    station.ClsCommObj.NCommandbz |= 0x0010;
                }
                #endregion
                #region ----------------清除分站历史数据命令判断----------------

                var stationHisDataClearControl = manualCrossControlInfos.FirstOrDefault(m => m.Type == (int)ControlType.StationHisDataClear && short.Parse(m.Bkpoint.Substring(0, 3)) == station.Fzh);
                if (stationHisDataClearControl == null && station.StationHisDataClear == 1)
                {
                    //交叉控制表中已删除，但内存还是解锁状态
                    updateFlag = true;
                    station.StationHisDataClear = 0;
                    station.realControlCount++;
                    station.ClsCommObj.NCommandbz |= 0x80;

                }
                else if (stationHisDataClearControl != null && station.StationHisDataClear == 0)
                {
                    //交叉控制表中已增加，但内存是不解锁状态
                    updateFlag = true;
                    station.StationHisDataClear = 1;
                    station.realControlCount++;
                    station.ClsCommObj.NCommandbz |= 0x80;
                }

                #endregion

                if (updateFlag)
                {
                    updateStations.Add(station);
                }

            }
            if (updateStations.Count > 0)
            {
                //更新分站内存
                PointDefineCacheUpdateControlReqest updateControlRequest = new PointDefineCacheUpdateControlReqest();
                updateStations.ForEach(updateItem =>
                {
                    updateControlRequest.PointDefineInfo = updateItem;
                    poingDefineCacheService.UpdatePointDefineControl(updateControlRequest);
                });
            }
            #endregion
        }

        /// <summary>
        /// key = 口号，value = 报警等级
        /// </summary>
        /// <returns></returns>
        private List<GradingAlarmItem> GetGradeControls(int fzh, List<Jc_JcsdkzInfo> manualCrossControlInfos)
        {
            List<GradingAlarmItem> items = new List<GradingAlarmItem>();
            GradingAlarmItem item;

            int index, kh, grade;
            List<Jc_JcsdkzInfo> myControls = GetJckzInfo(fzh, manualCrossControlInfos);
            foreach (Jc_JcsdkzInfo info in myControls)
            {
                kh = Convert.ToInt32(info.Bkpoint.Substring(4, 2));
                grade = Convert.ToInt32(info.Upflag);

                if (grade != 0)
                {
                    grade = 5 - grade;
                }

                index = items.FindIndex(a => a.kh == kh);
                if (index > 0)
                {
                    if (items[index].grade > grade)
                    {
                        items[index].grade = grade;
                    }
                }
                else
                {
                    item = new GradingAlarmItem();
                    item.kh = kh;
                    item.grade = grade;
                    items.Add(item);
                }
            }

            myControls.ForEach(a =>
            {
                if (a.Upflag != "0")
                {
                    a.Upflag = (5 - Convert.ToInt32(a.Upflag)).ToString();
                }
            });

            return items;
        }

        private bool CompareGradeControls(List<GradingAlarmItem> oldControls, List<GradingAlarmItem> newControls)
        {
            bool flag = true;
            GradingAlarmItem newControl;
            if (oldControls == null)
            {
                flag = false;
            }
            else if (oldControls.Count != newControls.Count)
            {
                flag = false;
            }
            else
            {
                foreach (GradingAlarmItem item in oldControls)
                {
                    newControl = newControls.FirstOrDefault(a => a.kh == item.kh && a.grade == item.grade);
                    if (newControl == null)
                    {
                        flag = false;
                        break;
                    }
                }
            }
            return flag;
        }

        private List<Jc_JcsdkzInfo> GetJckzInfo(int fzh, List<Jc_JcsdkzInfo> manualCrossControlInfos)
        {
            List<Jc_JcsdkzInfo> myControls = new List<Jc_JcsdkzInfo>();

            myControls = manualCrossControlInfos.Where(a => (a.Type == 12) && (a.Bkpoint.Substring(0, 3) == fzh.ToString().PadLeft(3, '0'))).ToList();

            return myControls;
        }

        private uint GetControlInt(List<ControlItem> controlItems)
        {
            uint controlInt = 0;

            if (controlItems != null)
            {
                foreach (ControlItem item in controlItems)
                {
                    controlInt |= (uint)(item.ControlType << item.Channel);
                }
            }

            return controlInt;
        }

        /// <summary>
        /// 分站是否有变化
        /// </summary>
        /// <param name="stationList"></param>
        /// <param name="key"></param>
        private void UpdateStationControlChangeList(List<string> stationList, string key)
        {
            if (!stationList.Contains(key))
            {
                stationList.Add(key);
            }
        }

        /// <summary>
        /// 更新有变化的分站缓存
        /// </summary>
        /// <param name="stationPoints"></param>
        /// <param name="stationList"></param>
        private void UpdatePointDefineControlInfo(List<Jc_DefInfo> stationPoints, List<string> stationList)
        {
            List<Jc_DefInfo> updateStations = new List<Jc_DefInfo>();

            stationPoints.ForEach(station =>
            {
                if (stationList.Contains(station.Point))
                {
                    station.sendIniCount++;
                    updateStations.Add(station);
                }
            });

            if (updateStations.Any())
            {
                //更新分站控制信息至测点定义缓存
                PointDefineCacheUpdateControlReqest updateControlRequest = new PointDefineCacheUpdateControlReqest();
                updateStations.ForEach(updateItem =>
                {
                    updateControlRequest.PointDefineInfo = updateItem;
                    poingDefineCacheService.UpdatePointDefineControl(updateControlRequest);
                });
            }
        }
    }
}
