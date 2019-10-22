using Basic.Framework.Logging;
using Sys.DataCollection.Common.Protocols;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.CommunicateExtend;
using Sys.Safety.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sys.Safety.Driver.RealDataHandle
{
    /// <summary>
    /// 作者：
    /// 时间：2017-05-31
    /// 描述：分站实时数据处理
    /// 修改记录
    /// 2017-05-31
    /// </summary>
    public class StationHandle : PointHandle
    {
        protected override Dictionary<string, object> DataHandle()
        {
            Dictionary<string, object> updateItems = null;
            if (RealDataItem.State != ItemState.EquipmentInterrupted)   //通讯中断不在此处处理，由下发线程判断延时后置中断 2017.7，21 by
            {
                float voltage = 0;               
                float.TryParse(RealDataItem.Voltage, out voltage);              
                if (voltage != PointDefineInfo.Voltage)
                {
                    updateItems = new Dictionary<string, object>();
                    updateItems.Add("Voltage", voltage);                  
                }
                if ((DeviceRunState)RealDataItem.State == DeviceRunState.EquipmentInterrupted)
                {
                    //2017.10.13 by 增加接收到分站通讯中断日志输出，便于分析数据
                    LogHelper.Info("接收到分站【" + PointDefineInfo.Point + "】通讯中断数据,CreatedTime=" + CreatedTime.ToString("yyyy-MM-dd HH:mm:ss") + ";NowTime=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                SafetyHelper.DeviceInterruptPro(PointDefineInfo.Fzh, CreatedTime, (DeviceRunState)RealDataItem.State, updateItems);
            }

            return null; //分站相关数据更新在DeviceInterruptPro处理，返回null后，前端不再处理其实时值相关信息 2017.7.21 by
        }
        protected override void PointRunRecord(DeviceDataState dataState, DeviceRunState runState)
        {

        }

        public override bool PretreatmentHandle(Jc_DefInfo pointDefineInfo)
        {
            pointDefineInfo.BCommDevTypeMatching = true;
            DateTime time = DateTime.Now;
            //分站删除
            if (pointDefineInfo.InfoState == Basic.Framework.Web.InfoState.Delete || pointDefineInfo.Activity == "0")
            {
                DeleteStationProc(pointDefineInfo, time);
            }
            //分站新增
            else if (pointDefineInfo.InfoState == Basic.Framework.Web.InfoState.AddNew)
            {
                AddStationProc(pointDefineInfo, time);
            }
            else if (pointDefineInfo.InfoState == Basic.Framework.Web.InfoState.Modified)
            {
                ModifiedStationProc(pointDefineInfo, time);
            }

            if ((pointDefineInfo.Bz3 & 0x01) == 0x00)
            {
                //当前没有封电闭锁,解除封电闭锁交叉控制
                DeleteGasThreeUnlockControl(pointDefineInfo);
            }

            return pointDefineInfo.DefIsInit;
        }

        /// <summary>
        /// 解除封电闭锁交叉控制 2017.7.24 by
        /// </summary>
        private static void DeleteGasThreeUnlockControl(Jc_DefInfo pointDefineInfo)
        {
            ControlBus.DeleteManualCrossControlByBkpoint(pointDefineInfo.Point, ControlType.GasThreeUnlockControl);
        }

        /// <summary>
        /// 分站设备删除处理
        /// </summary>
        /// <param name="stationInfo"></param>
        /// <param name="time"></param>
        private static void DeleteStationProc(Jc_DefInfo stationInfo, DateTime time)
        {
            AlarmBusiness.EndSubstationAlarm(stationInfo, time);

            //处理下级设备 
            List<Jc_DefInfo> pointDefineItems = SafetyHelper.GetPointDefinesByStationID(stationInfo.Fzh);
            AlarmBusiness.BatchEndAnalogAlarm(pointDefineItems.Where(a => a.DevPropertyID == (int)ItemDevProperty.Analog).ToList(), time, true);
            AlarmBusiness.BatchEndDerailAlarm(pointDefineItems.Where(a => a.DevPropertyID == (int)ItemDevProperty.Derail).ToList(), time, true);
            AlarmBusiness.BatchEndControlAlarm(pointDefineItems.Where(a => a.DevPropertyID == (int)ItemDevProperty.Control).ToList(), time);
        }

        /// <summary>
        /// 分站设备新增处理
        /// </summary>
        /// <param name="stationInfo"></param>
        /// <param name="time"></param>
        private static void AddStationProc(Jc_DefInfo stationInfo, DateTime time)
        {
            stationInfo.ClsCommObj = new CommProperty((uint)stationInfo.Fzh);
            stationInfo.ClsAlarmObj = new AlarmProperty();
            stationInfo.ClsCommObj.BCommTest = false;
            stationInfo.ClsCommObj.BCommuOK = false;
            stationInfo.DttkdStrtime = DateTime.Now;
            stationInfo.DttRunStateTime = DateTime.Now;
            stationInfo.NCtrlSate = (int)ControlState.DataPowerOnSuccess;
            stationInfo.State = (short)DeviceDataState.EquipmentStateUnknow;
            stationInfo.DataState = (short)DeviceDataState.EquipmentStateUnknow;
        }

        /// <summary>
        /// 分站设备修改处理
        /// </summary>
        /// <param name="stationInfo"></param>
        /// <param name="time"></param>
        private static void ModifiedStationProc(Jc_DefInfo stationInfo, DateTime time)
        {
            Dictionary<string, object> updateItems = new Dictionary<string, object>();
             if (stationInfo.ClsCommObj == null)
            {
                stationInfo.ClsCommObj = new CommProperty((uint)stationInfo.Fzh);
                stationInfo.ClsCommObj.BCommTest = false;
                stationInfo.ClsCommObj.BCommuOK = false;
                stationInfo.ClsCommObj.BInit = true;
                stationInfo.ClsCommObj.BNet_Change = true;
                stationInfo.ClsCommObj.BSendControlCommand = false;
            }

            if (!stationInfo.DefIsInit)
            {
                if ((stationInfo.Bz4 & 0x02) == 0x02)
                {
                    //结束报警
                    AlarmBusiness.EndSubstationAlarm(stationInfo, time);
                    //写R表记录
                    SafetyHelper.CreateRunLogInfo(stationInfo, time, stationInfo.DataState, (short)DeviceRunState.EquipmentSleep, stationInfo.Ssz);
                    //模拟量休眠
                    stationInfo.DataState = (short)DeviceDataState.EquipmentSleep;
                    stationInfo.State = (short)DeviceRunState.EquipmentSleep;

                }
            }
            if (updateItems.Count > 0)
            {
                SafetyHelper.UpdatePointDefineInfoByProperties(stationInfo.PointID, updateItems);
            }
        }
    }
}
