using Basic.Framework.Common;
using Basic.Framework.Logging;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Enums;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.DataToDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Driver
{
    class AlarmBusiness
    {
        #region 报警数据相关操作

        public static void EndAlarmInfo(Jc_DefInfo pointInfo, short dataState, DateTime sTime, DateTime eTime)
        {
            bool updateSsz = false;
            Jc_BInfo endAlarmInfo = new Jc_BInfo();
            endAlarmInfo.Stime = sTime;
            endAlarmInfo.Type = dataState;
            endAlarmInfo.State = SafetyHelper.GetState((DeviceRunState)pointInfo.State, pointInfo.Bz4); //设备休眠时，要更新其state状态
            endAlarmInfo.Point = pointInfo.Point;
            endAlarmInfo.Etime = eTime;

            if (pointInfo.DevPropertyID == (int)DeviceProperty.Derail)
            {
                endAlarmInfo.Ssz = pointInfo.Ssz;
                updateSsz = true;
            }

            UpdateAlarmInfo(endAlarmInfo, 2, updateSsz);
        }
        public static void EndAlarmInfo(string point, DateTime stime, short dataState, double maxValue, DateTime maxValueTime, DateTime endTime, double avgValue,double minValue,DateTime minValueTime)
        {
            Jc_BInfo endAlarmInfo = new Jc_BInfo();
            endAlarmInfo.Point = point;
            endAlarmInfo.Stime = stime;
            endAlarmInfo.Type = dataState;
            endAlarmInfo.Zdz = maxValue;
            endAlarmInfo.Zdzs = maxValueTime;
            endAlarmInfo.Etime = endTime;
            endAlarmInfo.Pjz = avgValue;
            endAlarmInfo.Bz4 = minValue.ToString("F2");
            endAlarmInfo.Bz5 = minValueTime.ToString();
            UpdateAlarmInfo(endAlarmInfo, 4,false);
        }

        #region 结束报警记录

        /// <summary>
        /// 结束分站报警记录
        /// </summary>
        /// <param name="substationItem">分站测点</param>
        ///  <param name="time"></param>
        public static void EndSubstationAlarm(Jc_DefInfo substationItem, DateTime time)
        {
            //写上一状态结束记录
            if (substationItem != null && (substationItem.DataState == (short)DeviceDataState.EquipmentDC ||
                substationItem.DataState == (short)DeviceDataState.EquipmentBiterror ||
                substationItem.DataState == (short)DeviceDataState.EquipmentInterrupted))
            {
                EndAlarmInfo(substationItem, substationItem.DataState, substationItem.DttRunStateTime, time);
            }
        }

        /// <summary>
        /// 批量结束模拟量报警记录（休眠不处理）
        /// </summary>
        /// <param name="analogItems">模拟量测点列表（同一分站下的测点）</param>
        /// <param name="time"></param>
        /// <param name="isEndConrol">是否在结束报警记录的同时结束当前交叉控制</param>
        public static void BatchEndAnalogAlarm(List<Jc_DefInfo> analogItems, DateTime time, bool isEndConrol)
        {
            DateTime endtime;
            if (analogItems != null)
            {
                foreach (Jc_DefInfo defInfo in analogItems)
                {
                    if ((defInfo.Bz4 & 0x2) != 0x2) //设备状态不为休眠
                    {
                        endtime = defInfo.DttStateTime > new DateTime(2000, 1, 1) ? defInfo.DttStateTime : time; //2017.12.22 by 取最后一次通讯时间为报警结束时间
                        EndAnalogAlarm(defInfo, endtime, isEndConrol);
                    }
                }
            }
        }

        /// <summary>
        /// 单个结束模拟量报警记录（休眠也要处理）
        /// </summary>
        /// <param name="analogItems">模拟量测点列表（同一分站下的测点）</param>
        /// <param name="time"></param>
        /// <param name="isEndConrol">是否在结束报警记录的同时结束当前交叉控制</param>
        public static void EndAnalogAlarm(Jc_DefInfo defInfo, DateTime time, bool isEndConrol)
        {
            try
            {
                if (SafetyHelper.isAbnormalDataState((DeviceDataState)defInfo.DataState))
                {
                    //写今天模拟量断线|上溢|负漂结束记录
                    EndAlarmInfo(defInfo, defInfo.DataState, defInfo.DttRunStateTime, time);

                    //解除断线/异常（上溢负漂）交叉控制 
                    if (isEndConrol && (defInfo.DataState == (short)DeviceDataState.EquipmentOverrange || defInfo.DataState == (short)DeviceDataState.EquipmentUnderrange))
                    {
                        ControlBus.DeleteManualCrossControlByZkpoint(defInfo.Point, ControlType.ControlErro);
                    }
                    else if (isEndConrol && defInfo.DataState == (short)DeviceDataState.EquipmentDown)
                    {
                        ControlBus.DeleteManualCrossControlByZkpoint(defInfo.Point, ControlType.ControlLineDown);
                    }
                }
                else if (defInfo.Alarm != 0)
                {
                    #region 生成模拟量报警结束|断电结束运行记录结构体
                    //今天上限报警结束
                    if ((defInfo.Alarm & 0x02) == 0x02)
                    {
                        EndAlarmInfo(defInfo.Point,
                            defInfo.ClsAlarmObj.DttHighAlarmTime,
                            (short)DeviceDataState.DataHighAlarm,
                            Convert.ToSingle(defInfo.ClsAlarmObj.NAlarmMaxVal),
                            defInfo.ClsAlarmObj.DttAlarmMaxValTime,
                            time,
                            Convert.ToSingle(Math.Round(defInfo.ClsAlarmObj.NAlarmAllVal / SafetyHelper.GetCumulativeCount(defInfo.ClsAlarmObj.NAlarmAllCount), 2)),
                            Convert.ToSingle(defInfo.ClsAlarmObj.NAlarmMinVal),
                            defInfo.ClsAlarmObj.DttAlarmMinValTime);
                        defInfo.ClsAlarmObj.NAlarmAllCount = 0;
                    }
                    //今天下限报警结束
                    else if ((defInfo.Alarm & 0x20) == 0x20)
                    {
                        EndAlarmInfo(defInfo.Point,
                            defInfo.ClsAlarmObj.DttLowAlarmTime,
                            (short)DeviceDataState.DataLowAlarm,
                            Convert.ToSingle(defInfo.ClsAlarmObj.NAlarmMaxVal),
                            defInfo.ClsAlarmObj.DttAlarmMaxValTime,
                            time,
                            Convert.ToSingle(Math.Round(defInfo.ClsAlarmObj.NAlarmAllVal / SafetyHelper.GetCumulativeCount(defInfo.ClsAlarmObj.NAlarmAllCount), 2)),
                            Convert.ToSingle(defInfo.ClsAlarmObj.NAlarmMinVal),
                            defInfo.ClsAlarmObj.DttAlarmMinValTime
                            );
                        defInfo.ClsAlarmObj.NAlarmAllCount = 0;
                    }

                    if ((defInfo.Alarm & 0x04) == 0x04)
                    {
                        //今天上限断电结束
                        EndAlarmInfo(defInfo.Point,
                            defInfo.ClsAlarmObj.DttHighPowerOffTime,
                            (short)DeviceDataState.DataHighAlarmPowerOFF,
                            Convert.ToSingle(defInfo.ClsAlarmObj.NPowerOffMaxVal),
                            defInfo.ClsAlarmObj.DttPowerOffMaxValTime,
                            time,
                            Convert.ToSingle(Math.Round(defInfo.ClsAlarmObj.NPowerOffAllVal / SafetyHelper.GetCumulativeCount(defInfo.ClsAlarmObj.NPowerOffAllCount), 2)),
                            Convert.ToSingle(defInfo.ClsAlarmObj.NPowerOffMinVal),
                            defInfo.ClsAlarmObj.DttPowerOffMinValTime
                            );
                        defInfo.ClsAlarmObj.NPowerOffAllCount = 0;
                    }
                    //今天下限断电结束
                    else if ((defInfo.Alarm & 0x40) == 0x40)
                    {
                        EndAlarmInfo(defInfo.Point,
                            defInfo.ClsAlarmObj.DttLowPowerOffTime,
                            (short)DeviceDataState.DataLowPower,
                            Convert.ToSingle(defInfo.ClsAlarmObj.NPowerOffMaxVal),
                            defInfo.ClsAlarmObj.DttPowerOffMaxValTime,
                            time,
                            Convert.ToSingle(Math.Round(defInfo.ClsAlarmObj.NPowerOffAllVal / SafetyHelper.GetCumulativeCount(defInfo.ClsAlarmObj.NPowerOffAllCount), 2)),
                            Convert.ToSingle(defInfo.ClsAlarmObj.NPowerOffMinVal),
                            defInfo.ClsAlarmObj.DttPowerOffMinValTime
                            );
                        defInfo.ClsAlarmObj.NPowerOffAllCount = 0;
                    }

                    #endregion

                    //解除断电交叉控制
                    if (isEndConrol)
                    {
                        if ((defInfo.Alarm & 0x04) == 0x04 || (defInfo.Alarm & 0x40) == 0x40)
                        {
                            ControlBus.DeleteManualCrossControlByZkpoint(defInfo.Point, ControlType.ControlPowerOff);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("EndAnalogAlarm Error【" + defInfo.Point + "】" + ex.Message);
            }
        }

        /// <summary>
        /// 批量结束开关量报警记录（休眠不处理）
        /// </summary>
        /// <param name="derailItems"></param>
        /// <param name="time"></param>
        /// <param name="isEndConrol">是否在结束报警记录的同时结束当前交叉控制</param>
        public static void BatchEndDerailAlarm(List<Jc_DefInfo> derailItems, DateTime time, bool isEndConrol)
        {
            DateTime endtime;
            if (derailItems != null)
            {
                foreach (Jc_DefInfo defInfo in derailItems)
                {
                    if ((defInfo.Bz4 & 0x2) != 0x02)
                    {
                        endtime = defInfo.DttStateTime > new DateTime(2000, 1, 1) ? defInfo.DttStateTime : time; //2017.12.22 by 取最后一次通讯时间为报警结束时间
                        EndDerailAlarm(defInfo, endtime, isEndConrol);
                    }
                }
            }
        }
        /// <summary>
        /// 单个结束开关量报警记录（休眠也要处理）
        /// </summary>
        /// <param name="derailItems"></param>
        /// <param name="time"></param>
        /// <param name="isEndConrol">是否在结束报警记录的同时结束当前交叉控制</param>
        public static void EndDerailAlarm(Jc_DefInfo defInfo, DateTime time, bool isEndConrol)
        {
            //if (defInfo.DataState == (short)DeviceDataState.DataDerailState0 ||
            //                defInfo.DataState == (short)DeviceDataState.DataDerailState1 ||
            //                defInfo.DataState == (short)DeviceDataState.DataDerailState2)
            {
                //写上一状态结束记录
                EndAlarmInfo(defInfo, defInfo.DataState, defInfo.DttRunStateTime, time);
                //解除交叉控制
                if (isEndConrol)
                {
                    ControlType controlType = ControlType.NoControl;
                    if (defInfo.DataState == (short)DeviceDataState.DataDerailState0)
                    {
                        controlType = ControlType.ControlState0;
                    }
                    else if (defInfo.DataState == (short)DeviceDataState.DataDerailState1)
                    {
                        controlType = ControlType.ControlState1;
                    }
                    else if (defInfo.DataState == (short)DeviceDataState.DataDerailState2)
                    {
                        controlType = ControlType.ControlState2;
                    }

                    ControlBus.DeleteManualCrossControlByZkpoint(defInfo.Point, controlType);
                }
            }
        }

        /// <summary>
        /// 结束控制量报警记录
        /// </summary>
        /// <param name="controlItems"></param>
        /// <param name="time"></param>
        public static void BatchEndControlAlarm(List<Jc_DefInfo> controlItems, DateTime time)
        {
            DateTime endtime;
            if (controlItems != null)
            {
                foreach (Jc_DefInfo defInfo in controlItems)
                {
                    if ((defInfo.Bz4 & 0x2) != 0x02)
                    {
                        //写今天馈电结束
                        if (defInfo.NCtrlSate == (int)ControlState.DataPowerOnFail || defInfo.NCtrlSate == (int)ControlState.DataPowerOffFail)
                        {
                            endtime = defInfo.DttStateTime > new DateTime(2000, 1, 1) ? defInfo.DttStateTime : time; //2017.12.22 by 取最后一次通讯时间为报警结束时间
                            EndAlarmInfo(defInfo, (short)defInfo.NCtrlSate, defInfo.DttkdStrtime, endtime);
                        }
                    }
                }
            }
        }
        #endregion

        #region 结束报警记录并生成新报警记录

        /// <summary>
        /// 结束分站当前报警，生成新报警记录
        /// </summary>
        /// <param name="substationItem"></param>
        /// <param name="endTime">当前报警结束时间</param>
        /// <param name="reStartTime">新报警开始时间</param>
        public static void ReStartSubstationAlarm(Jc_DefInfo substationItem, DateTime endTime, DateTime reStartTime)
        {
            try
            {
                if (substationItem != null)
                {
                    Jc_BInfo alarmInfo;
                    short tempDataState = substationItem.DataState;
                    short tempState = SafetyHelper.GetState((DeviceRunState)substationItem.State, substationItem.Bz4);
                    if ((substationItem.Bz4 & 0x2) != 0x2) //休眠不进行跨天处理
                    {
                        ////todo 加入头子断线等处理
                        //if (substationItem.DataState == (short)DeviceDataState.EquipmentDC ||
                        //    substationItem.DataState == (short)DeviceDataState.EquipmentBiterror ||
                        //    substationItem.DataState == (short)DeviceDataState.EquipmentInterrupted)
                        if (SafetyHelper.isAbnormalState((DeviceRunState)substationItem.State) || substationItem.DataState == (short)DeviceDataState.EquipmentDC)
                        {
                            //写上一状态结束记录
                            EndAlarmInfo(substationItem, substationItem.DataState, substationItem.DttRunStateTime, endTime);
                            //写当前异常状态开始记录
                            alarmInfo = new Jc_BInfo();
                            alarmInfo.Cs = "";
                            alarmInfo.ID = IdHelper.CreateLongId().ToString();
                            alarmInfo.PointID = substationItem.PointID;
                            alarmInfo.Devid = substationItem.Devid;
                            alarmInfo.Fzh = substationItem.Fzh;
                            alarmInfo.Kh = substationItem.Kh;
                            alarmInfo.Dzh = substationItem.Dzh;
                            alarmInfo.Kzk = "";
                            alarmInfo.Point = substationItem.Point;
                            alarmInfo.Ssz = substationItem.Ssz;
                            alarmInfo.Stime = reStartTime;
                            alarmInfo.Etime = new DateTime(1900, 1, 1, 0, 0, 0);
                            alarmInfo.Isalarm = 0;
                            if ((DeviceRunState)substationItem.State == DeviceRunState.EquipmentStateUnknow)
                            {
                                tempDataState = (short)DeviceDataState.EquipmentInterrupted;
                                tempState = SafetyHelper.GetState(DeviceRunState.EquipmentInterrupted, substationItem.Bz4);
                            }
                            alarmInfo.Type = tempDataState;
                            alarmInfo.State = tempState;

                            alarmInfo.Upflag = "0";
                            alarmInfo.Wzid = substationItem.Wzid;

                            UpdateAlarmInfo(alarmInfo, 1, false);
                        }

                        //补运行记录
                        //KJ73NHelper.CreateRunLogInfo(substationItem, reStartTime, substationItem.DataState, substationItem.State, EnumHelper.GetEnumDescription((DeviceDataState)substationItem.DataState));
                        if (substationItem.State == (short)DeviceRunState.EquipmentStateUnknow)
                        {
                            SafetyHelper.CreateRunLogInfo(substationItem, reStartTime, (short)DeviceDataState.EquipmentInterrupted, (short)DeviceRunState.EquipmentInterrupted, "通讯中断");
                        }
                        else
                        {
                            SafetyHelper.CreateRunLogInfo(substationItem, reStartTime, tempDataState, tempState, EnumHelper.GetEnumDescription((DeviceDataState)substationItem.DataState));
                        }

                        //更新测点信息
                        substationItem.DttRunStateTime = reStartTime;
                        Dictionary<string, object> updateItems = new Dictionary<string, object>();
                        updateItems.Add("DttRunStateTime", substationItem.DttRunStateTime);
                        SafetyHelper.UpdatePointDefineInfoByProperties(substationItem.PointID, updateItems);
                        //UpdatePointDefineInfo(substationItem, 5);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("ReStartSubstationAlarm Error【" + substationItem.Point + "】" + ex.Message);
            }
        }

        /// <summary>
        /// 结束控制量当前报警，生成新报警记录
        /// </summary>
        /// <param name="controlItems"></param>
        /// <param name="endTime">当前报警结束时间</param>
        /// <param name="reStartTime">新报警开始时间</param>
        public static void ReStartControlAlarm(List<Jc_DefInfo> controlItems, DateTime endTime, DateTime reStartTime)
        {
            if (controlItems != null && controlItems.Count > 0)
            {
                Dictionary<string, object> updateItems;
                foreach (Jc_DefInfo defInfo in controlItems)
                {
                    if ((defInfo.Bz4 & 0x2) != 0x2)
                    {
                        try
                        {
                            updateItems = new Dictionary<string, object>();
                            // 写今天馈电结束
                            if ((defInfo.NCtrlSate == (int)ControlState.DataPowerOnFail) || (defInfo.NCtrlSate == (int)ControlState.DataPowerOffFail))
                            {
                                EndAlarmInfo(defInfo, (short)defInfo.NCtrlSate, defInfo.DttkdStrtime, reStartTime.AddSeconds(-1));
                            }
                            // 写明天馈电开始
                            if (defInfo.NCtrlSate == (int)ControlState.DataPowerOnFail)
                            {
                                defInfo.DttkdStrtime = reStartTime;
                                //defInfo.Ssz = EnumHelper.GetEnumDescription(DeviceDataState.DataPowerOnFail);
                                //defInfo.DataState = (short)DeviceDataState.DataPowerOnFail;
                                UpdateAlarmInfo(CreateAlarmInfo(defInfo, reStartTime, (DeviceDataState)defInfo.NCtrlSate, (DeviceRunState)defInfo.State, 0), 1, false);
                            }
                            else if (defInfo.NCtrlSate == (int)ControlState.DataPowerOffFail)
                            {
                                defInfo.DttkdStrtime = reStartTime;
                                //defInfo.Ssz = EnumHelper.GetEnumDescription(DeviceDataState.DataPowerOffFail);
                                //defInfo.DataState = (short)DeviceDataState.DataPowerOffFail;
                                UpdateAlarmInfo(CreateAlarmInfo(defInfo, reStartTime, (DeviceDataState)defInfo.NCtrlSate, (DeviceRunState)defInfo.State, 0), 1, false);
                            }
                            //KJ73NHelper.CreateRunLogInfo(defInfo, reStartTime, defInfo.DataState, defInfo.State, defInfo.Ssz);
                            updateItems.Add("DttkdStrtime", defInfo.DttkdStrtime);
                            SafetyHelper.UpdatePointDefineInfoByProperties(defInfo.PointID, updateItems);
                            //跨天补写运行记录表
                            if (defInfo.State == (short)DeviceRunState.EquipmentStateUnknow)
                            {
                                SafetyHelper.CreateRunLogInfo(defInfo, reStartTime, (short)DeviceDataState.EquipmentDown, (short)DeviceRunState.EquipmentDown, "断线");
                            }
                            else
                            {
                                SafetyHelper.CreateRunLogInfo(defInfo, reStartTime, defInfo.DataState, defInfo.State, defInfo.Ssz);
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error("ReStartControlAlarm Error【" + defInfo.Point + "】" + ex.Message);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 结束开关量当前报警，生成新报警记录
        /// </summary>
        /// <param name="derailItems"></param>
        /// <param name="endTime"></param>
        /// <param name="reStartTime"></param>
        public static void ReStartDerailAlarm(List<Jc_DefInfo> derailItems, DateTime endTime, DateTime reStartTime)
        {
            if (derailItems != null && derailItems.Count > 0)
            {
                //List<Jc_DefInfo> updateDefItems = new List<Jc_DefInfo>();
                Dictionary<string, object> updateItems;
                foreach (Jc_DefInfo defInfo in derailItems)
                {
                    if ((defInfo.Bz4 & 0x2) != 0x2)
                    {
                        try
                        {
                            if (defInfo.DataState == (int)DeviceDataState.DataDerailState0 ||
                                defInfo.DataState == (int)DeviceDataState.DataDerailState1 ||
                                defInfo.DataState == (int)DeviceDataState.DataDerailState2)
                            {
                                //写上一状态结束记录
                                EndAlarmInfo(defInfo, defInfo.DataState, defInfo.DttRunStateTime, endTime);
                                UpdateAlarmInfo(CreateAlarmInfo(defInfo, reStartTime, (DeviceDataState)defInfo.DataState, (DeviceRunState)defInfo.State, 1), 1, false);
                                defInfo.DttRunStateTime = reStartTime;
                                updateItems = new Dictionary<string, object>();
                                updateItems.Add("DttRunStateTime", defInfo.DttRunStateTime);
                                SafetyHelper.UpdatePointDefineInfoByProperties(defInfo.PointID, updateItems);
                            }
                            //补运行记录
                            if ((DeviceRunState)defInfo.State == DeviceRunState.EquipmentStateUnknow)
                            {
                                SafetyHelper.CreateRunLogInfo(defInfo, reStartTime, (short)DeviceDataState.DataDerailState0, (short)DeviceRunState.DataDerailState0, "断线");
                            }
                            else
                            {
                                SafetyHelper.CreateRunLogInfo(defInfo, reStartTime, defInfo.DataState, defInfo.State, defInfo.Ssz);
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error("ReStartDerailAlarm Error【" + defInfo.Point + "】"+ex.Message);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 结束模拟量当前报警，生成新报警记录
        /// </summary>
        /// <param name="analogItems"></param>
        /// <param name="endTime">当前报警结束时间</param>
        /// <param name="reStartTime">新报警开始时间</param>
        public static void ReStartAnalogAlarm(List<Jc_DefInfo> analogItems, DateTime endTime, DateTime reStartTime)
        {
            if (analogItems != null && analogItems.Count > 0)
            {
                List<Jc_DefInfo> updateDefItems = new List<Jc_DefInfo>();
                decimal value = 0;
                Dictionary<string, object> updateItems;
                foreach (Jc_DefInfo defInfo in analogItems)
                {
                    if ((defInfo.Bz4 & 0x2) != 0x2)
                    {
                        try
                        {
                            updateItems = new Dictionary<string, object>();
                            if (!decimal.TryParse(defInfo.Ssz, out value))
                            {
                                value = 0;
                            }

                            #region 写密采数据
                            Jc_McInfo densityColl = new Jc_McInfo();
                            densityColl.ID = IdHelper.CreateLongId().ToString();
                            densityColl.Point = defInfo.Point;
                            densityColl.PointID = defInfo.PointID;
                            densityColl.Devid = defInfo.Devid;
                            densityColl.Fzh = defInfo.Fzh;
                            densityColl.Kh = defInfo.Kh;
                            densityColl.Dzh = defInfo.Dzh;
                            densityColl.Timer = reStartTime;
                            if (defInfo.DataState == (int)DeviceDataState.EquipmentStateUnknow)
                            {
                                densityColl.Type = (int)DeviceDataState.EquipmentDown;
                                densityColl.State = SafetyHelper.GetState(DeviceRunState.EquipmentDown, defInfo.Bz4);
                            }
                            else
                            {
                                densityColl.Type = defInfo.DataState;
                                densityColl.State = SafetyHelper.GetState((DeviceRunState)defInfo.State, defInfo.Bz4);
                            }
                            //if (defInfo.DataState == (short)DeviceDataState.EquipmentDown ||
                            //    defInfo.DataState == (short)DeviceDataState.EquipmentUnderrange ||
                            //    defInfo.DataState == (short)DeviceDataState.EquipmentOverrange)
                            if (SafetyHelper.isAbnormalDataState((DeviceDataState)defInfo.DataState))
                            {
                                densityColl.Ssz = 0;
                            }
                            else
                            {
                                densityColl.Ssz = float.Parse(value.ToString());
                            }
                            densityColl.Upflag = "0";
                            densityColl.Wzid = defInfo.Wzid;
                            densityColl.Voltage = defInfo.Voltage;
                            densityColl.InfoState = Basic.Framework.Web.InfoState.AddNew;
                            //密采记录入库
                            DataToDbAddRequest<Jc_McInfo> dataToDbAddRequest = new DataToDbAddRequest<Jc_McInfo>();
                            dataToDbAddRequest.Item = densityColl;
                            SafetyHelper.mcDataInsertToDbService.AddItem(dataToDbAddRequest);

                            #endregion

                            #region 写模拟量运行记录
                            if (defInfo.DataState == (short)DeviceDataState.EquipmentDown ||
                                defInfo.DataState == (short)DeviceDataState.EquipmentOverrange ||
                                defInfo.DataState == (short)DeviceDataState.EquipmentUnderrange)
                            {
                                RestartAnalogAlarmInfo(defInfo, defInfo.DttRunStateTime, (DeviceDataState)defInfo.DataState, endTime, reStartTime);
                                UpdateAlarmPointInfo(defInfo, value, reStartTime, 0);
                            }
                            else if (defInfo.Alarm != 0)
                            {
                                #region 生成模拟量报警结束|断电结束运行记录结构体
                                // 报警
                                if ((defInfo.Alarm & 0x02) == 0x02)
                                {
                                    //今天上限报警结束
                                    RestartAnalogAlarmInfo(defInfo, defInfo.ClsAlarmObj.DttHighAlarmTime, DeviceDataState.DataHighAlarm, endTime, reStartTime);
                                    UpdateAlarmPointInfo(defInfo, value, reStartTime, 1);
                                }
                                else if ((defInfo.Alarm & 0x20) == 0x20)
                                {
                                    //今天下限报警结束
                                    RestartAnalogAlarmInfo(defInfo, defInfo.ClsAlarmObj.DttLowAlarmTime, DeviceDataState.DataLowAlarm, endTime, reStartTime);
                                    UpdateAlarmPointInfo(defInfo, value, reStartTime, 3);
                                }
                                // 断电
                                if ((defInfo.Alarm & 0x04) == 0x04)
                                {
                                    //今天上限断电结束
                                    RestartAnalogAlarmInfo(defInfo, defInfo.ClsAlarmObj.DttHighPowerOffTime, DeviceDataState.DataHighAlarmPowerOFF, endTime, reStartTime);
                                    UpdateAlarmPointInfo(defInfo, value, reStartTime, 2);
                                }
                                else if ((defInfo.Alarm & 0x40) == 0x40)
                                {
                                    // 今天下限断电结束
                                    RestartAnalogAlarmInfo(defInfo, defInfo.ClsAlarmObj.DttLowPowerOffTime, DeviceDataState.DataLowPower, endTime, reStartTime);
                                    UpdateAlarmPointInfo(defInfo, value, reStartTime, 4);
                                }
                                #endregion
                            }
                            #endregion
                            updateItems.Add("DttRunStateTime", defInfo.DttRunStateTime);
                            updateItems.Add("ClsAlarmObj", defInfo.ClsAlarmObj);

                            SafetyHelper.UpdatePointDefineInfoByProperties(defInfo.PointID, updateItems);

                            //跨天补写运行记录
                            SafetyHelper.WriteRunLog(defInfo, reStartTime);

                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error("ReStartAnalogAlarm Error【" + defInfo.Point + "】"+ex.Message);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 结束模拟量报警并生成新报警
        /// </summary>
        /// <param name="defInfo"></param>
        /// <param name="sTime"></param>
        /// <param name="dataState"></param>
        /// <param name="endTime"></param>
        /// <param name="reStartTime"></param>
        private static void RestartAnalogAlarmInfo(Jc_DefInfo defInfo, DateTime sTime, DeviceDataState dataState, DateTime endTime, DateTime reStartTime)
        {
            try
            {
                //结束当前报警
                Jc_BInfo alarminfo = new Jc_BInfo();
                alarminfo.Stime = sTime;
                alarminfo.Type = (short)dataState;
                alarminfo.Point = defInfo.Point;
                alarminfo.Etime = endTime;

                if (dataState == DeviceDataState.EquipmentOverrange || dataState == DeviceDataState.EquipmentUnderrange || dataState == DeviceDataState.EquipmentDown)
                {
                    //此状态下没有最大值、最大值时、平均值
                    alarminfo.Zdz = 0;
                    alarminfo.Zdzs = new DateTime(1900, 1, 1);
                    alarminfo.Pjz = 0;
                    alarminfo.Bz4 = "0";
                    alarminfo.Bz5 = "1900-1-1";
                }
                else if (dataState == DeviceDataState.DataHighAlarm || dataState == DeviceDataState.DataLowAlarm)
                {
                    alarminfo.Zdz = Convert.ToSingle(defInfo.ClsAlarmObj.NAlarmMaxVal);
                    alarminfo.Zdzs = defInfo.ClsAlarmObj.DttAlarmMaxValTime;
                    alarminfo.Pjz = defInfo.ClsAlarmObj.NAlarmAllCount == 0 ? 0 : Convert.ToSingle(Math.Round(defInfo.ClsAlarmObj.NAlarmAllVal / defInfo.ClsAlarmObj.NAlarmAllCount, 2));
                    alarminfo.Bz4 = Convert.ToSingle(defInfo.ClsAlarmObj.NAlarmMinVal).ToString();
                    alarminfo.Bz5 = defInfo.ClsAlarmObj.DttAlarmMinValTime.ToString();
                }
                else if (dataState == DeviceDataState.DataHighAlarmPowerOFF || dataState == DeviceDataState.DataLowPower)
                {
                    alarminfo.Zdz = Convert.ToSingle(defInfo.ClsAlarmObj.NPowerOffMaxVal);
                    alarminfo.Zdzs = defInfo.ClsAlarmObj.DttPowerOffMaxValTime;
                    alarminfo.Pjz = defInfo.ClsAlarmObj.NPowerOffAllCount == 0 ? 0 : Convert.ToSingle(Math.Round(defInfo.ClsAlarmObj.NPowerOffAllVal / defInfo.ClsAlarmObj.NPowerOffAllCount, 2));
                    alarminfo.Bz4 = Convert.ToSingle(defInfo.ClsAlarmObj.NPowerOffMinVal).ToString("F2");
                    alarminfo.Bz5 = defInfo.ClsAlarmObj.DttPowerOffMinValTime.ToString();
                }
                UpdateAlarmInfo(alarminfo, 4, false);

                // 新JC_B开始
                UpdateAlarmInfo(CreateAlarmInfo(defInfo, reStartTime, dataState, (DeviceRunState)defInfo.State, 1), 1, false);
            }
            catch (Exception ex)
            {
                LogHelper.Error("RestartAnalogAlarmInfo Error【" + defInfo.Point + "】" + ex.Message);
            }
        }

        /// <summary>
        /// 结束当前报警，生成新报警后，更新测点信息
        /// </summary>
        /// <param name="defInfo"></param>
        /// <param name="value"></param>
        /// <param name="reStartTime"></param>
        /// <param name="type">0更新运行时间,1更新上限报警运行时间,2更新上限断电运行时间，3更新下限报警运行时间，4更新下限断电运行时间</param>
        /// <returns></returns>
        private static void UpdateAlarmPointInfo(Jc_DefInfo defInfo, decimal value, DateTime reStartTime, int type)
        {

            if (type == 0)
            {
                //更新运行时间
                defInfo.DttRunStateTime = reStartTime;
            }
            else if (type == 1)
            {
                //更新上限报警运行时间
                defInfo.ClsAlarmObj.NAlarmAllCount = 1;
                defInfo.ClsAlarmObj.NAlarmAllVal = value;
                defInfo.ClsAlarmObj.DttAlarmMaxValTime = reStartTime;
                defInfo.ClsAlarmObj.NAlarmMaxVal = value;
                defInfo.ClsAlarmObj.DttAlarmMinValTime = reStartTime;
                defInfo.ClsAlarmObj.NAlarmMinVal = value;
                defInfo.ClsAlarmObj.DttHighAlarmTime = reStartTime;
            }
            else if (type == 2)
            {
                //更新上限断电运行时间
                defInfo.ClsAlarmObj.NPowerOffAllCount = 1;
                defInfo.ClsAlarmObj.NPowerOffAllVal = value;
                defInfo.ClsAlarmObj.DttPowerOffMaxValTime = reStartTime;
                defInfo.ClsAlarmObj.NPowerOffMaxVal = value;
                defInfo.ClsAlarmObj.DttPowerOffMinValTime = reStartTime;
                defInfo.ClsAlarmObj.NPowerOffMinVal = value;
                defInfo.ClsAlarmObj.DttHighPowerOffTime = reStartTime;
            }
            else if (type == 3)
            {
                //更新下限报警运行时间
                defInfo.ClsAlarmObj.NAlarmAllCount = 1;
                defInfo.ClsAlarmObj.NAlarmAllVal = value;
                defInfo.ClsAlarmObj.DttAlarmMaxValTime = reStartTime;
                defInfo.ClsAlarmObj.NAlarmMaxVal = value;
                defInfo.ClsAlarmObj.DttAlarmMinValTime = reStartTime;
                defInfo.ClsAlarmObj.NAlarmMinVal = value;
                defInfo.ClsAlarmObj.DttLowAlarmTime = reStartTime;
            }
            else if (type == 4)
            { 
                //更新下限报警运行时间
                defInfo.ClsAlarmObj.NAlarmAllCount = 1;
                defInfo.ClsAlarmObj.NAlarmAllVal = value;
                defInfo.ClsAlarmObj.DttAlarmMaxValTime = reStartTime;
                defInfo.ClsAlarmObj.NAlarmMaxVal = value;
                defInfo.ClsAlarmObj.DttAlarmMinValTime = reStartTime;
                defInfo.ClsAlarmObj.NAlarmMinVal = value;
                defInfo.ClsAlarmObj.DttLowPowerOffTime = reStartTime;
            }

        }
        #endregion

        /// <summary>
        /// 更新JC_B记录 (1 更新最大值时间为1900-1-1 00:00:00; 2 更新结束时间，并写馈电记录；3 更新Kdid，并写馈电记录；4 更新最大值、平均值、结束时间；6 更新kzk ; 7 更新isalarm报警标记)
        /// </summary>
        /// <param name="alarmInfo">JC_B</param>
        /// <param name="updateType">1 更新最大值时间为1900-1-1 00:00:00; 2 更新结束时间，并写馈电记录；4 更新最大值、平均值、结束时间；6 更新kzk ; 7 更新isalarm报警标记</param>
        ///  <param name="updateSsz">是否更新实时值（只开关量要更新）</param>
        public static void UpdateAlarmInfo(Jc_BInfo alarmInfo, int updateType,bool updateSsz)
        {
            List<Jc_BInfo> updateAlarmItems = new List<Jc_BInfo>();
            if (updateType == 1)
            {
                alarmInfo.Zdzs = new DateTime(1900, 1, 1, 0, 0, 0);
                alarmInfo.InfoState = Basic.Framework.Web.InfoState.AddNew;
                updateAlarmItems.Add(alarmInfo);
            }
            else
            {
                AlarmCacheGetByConditonRequest alarmCacheGetByConditonRequest = new AlarmCacheGetByConditonRequest();
                //alarmCacheGetByConditonRequest.Predicate = p => p.Stime == alarmInfo.Stime && p.Point == alarmInfo.Point && p.Type == alarmInfo.Type;
                //var alarmResponse = SafetyHelper.alarmCacheService.GetAlarmCache(alarmCacheGetByConditonRequest);                
                alarmCacheGetByConditonRequest.Predicate = p => p.Point == alarmInfo.Point &&  p.Etime < new DateTime(2000, 1, 1); //p.Type == alarmInfo.Type &&
                var alarmResponse = SafetyHelper.alarmCacheService.GetAlarmCache(alarmCacheGetByConditonRequest);
                if (alarmResponse != null && alarmResponse.IsSuccess && alarmResponse.Data != null)
                {
                    List<Jc_BInfo> alarmInfoItems = alarmResponse.Data;
                    foreach (Jc_BInfo alarm in alarmInfoItems)
                    {
                        if (updateType == 2)
                        {
                            alarm.Etime = alarmInfo.Etime;
                        }
                        else if (updateType == 4)
                        {
                            alarm.Pjz = alarmInfo.Pjz;
                            alarm.Zdz = alarmInfo.Zdz;
                            alarm.Zdzs = alarmInfo.Zdzs;
                            alarm.Bz4 = alarmInfo.Bz4;
                            alarm.Bz5 = alarmInfo.Bz5;
                            alarm.Etime = alarmInfo.Etime;
                        }
                        else if (updateType == 6)
                        {
                            alarm.Kzk = alarmInfo.Kzk;
                        }
                        else if (updateType == 7)
                        {
                            alarm.Isalarm = alarmInfo.Isalarm;
                        }

                        //alarm.Ssz = alarmInfo.Ssz;

                        alarm.InfoState = Basic.Framework.Web.InfoState.Modified;
                        updateAlarmItems.Add(alarm);
                    }
                }
            }

            //报警记录更新至缓存及数据库
            if (updateAlarmItems != null && updateAlarmItems.Any())
            {
                updateAlarmItems.ForEach(alarm => InsertOrUpdateAlarmInfo(alarm, updateSsz));
            }
        }

        /// <summary>
        /// 保存或更新报警信息至数据库及缓存
        /// </summary>
        /// <param name="alarmInfo"></param>
        /// <param name="updateSsz">是否更新实时值（只开关量要更新）</param>
        public static void InsertOrUpdateAlarmInfo(Jc_BInfo alarmInfo, bool updateSsz)
        {
            if (alarmInfo.InfoState == Basic.Framework.Web.InfoState.Modified)
            {
                UpdateAlarmInfo(alarmInfo, updateSsz);
            }
            else if (alarmInfo.InfoState == Basic.Framework.Web.InfoState.AddNew)
            {
                InsertAlarmInfo(alarmInfo);
            }
            else
            {
                LogHelper.Error("保存或更新报警信息至数据库及缓存失败，原因：InfoState = " + alarmInfo.InfoState);
            }
        }

        /// <summary>
        /// 插入新记录
        /// </summary>
        /// <param name="alarmInfo"></param>
        private static void InsertAlarmInfo(Jc_BInfo alarmInfo)
        {
            //添加记录入缓存
            alarmInfo.Zdzs = new DateTime(1900, 1, 1, 0, 0, 0);
            AlarmCacheAddRequest addRequest = new AlarmCacheAddRequest();
            addRequest.AlarmInfo = alarmInfo;
            SafetyHelper.alarmCacheService.AddAlarmCache(addRequest);
         
            //添加报警信息至数据库
            DataToDbAddRequest<Jc_BInfo> dataToDbAddRequest = new DataToDbAddRequest<Jc_BInfo>();
            dataToDbAddRequest.Item = alarmInfo;
            SafetyHelper.alarmTodbService.AddItem(dataToDbAddRequest);
        }
        /// <summary>
        /// 更新除kdid和upflag的其他字段
        /// </summary>
        /// <param name="alarmInfo"></param>
        /// <param name="updateSsz">是否更新实时值（只开关量要更新）</param>
        private static void UpdateAlarmInfo(Jc_BInfo alarmInfo,bool updateSsz)
        {
            Dictionary<string, object> updateItems = new Dictionary<string, object>();
            updateItems.Add("State", alarmInfo.State);
            updateItems.Add("Etime", alarmInfo.Etime);
            updateItems.Add("Zdz", alarmInfo.Zdz);
            updateItems.Add("Pjz", alarmInfo.Pjz);
            updateItems.Add("Zdzs", alarmInfo.Zdzs);
            updateItems.Add("Kzk", alarmInfo.Kzk);
            updateItems.Add("Isalarm", alarmInfo.Isalarm);
            updateItems.Add("Bz4", alarmInfo.Bz4);
            updateItems.Add("Bz5", alarmInfo.Bz5);
            if (updateSsz)
            {
                //特殊处理（只有开关量和控制量会赋值ssz，其他类型的均不改变ssz） 2017.7.11 by
                updateItems.Add("Ssz", alarmInfo.Ssz);
            }
            //更新到缓存
            AlarmCacheUpdatePropertiesRequest alarmCacheUpdatePropertiesRequest = new AlarmCacheUpdatePropertiesRequest();
            alarmCacheUpdatePropertiesRequest.AlarmKey = alarmInfo.ID;
            alarmCacheUpdatePropertiesRequest.UpdateItems = updateItems;
            SafetyHelper.alarmCacheService.UpdateAlarmInfoProperties(alarmCacheUpdatePropertiesRequest);
            
            //更新到数据库    
            DataToDbAddRequest<Jc_BInfo> dataToDbAddRequest = new DataToDbAddRequest<Jc_BInfo>();
            dataToDbAddRequest.Item = alarmInfo;
            SafetyHelper.alarmTodbService.AddItem(dataToDbAddRequest);
        }
        /// <summary>
        /// 根据设备内存信息，生成报警开始结构
        /// </summary>
        /// <param name="defInfo">设备内存信息</param>
        /// <param name="sTime">报警开始时间</param>
        /// <param name="writeTyep">0不写、 1断电、2复电</param>
        /// <returns></returns>
        public static Jc_BInfo CreateAlarmInfo(Jc_DefInfo defInfo, DateTime sTime, DeviceDataState dataState, DeviceRunState state, int writeTyep,string showValue="",short alarm=-1)
        {
            Jc_BInfo alarmInfo = new Jc_BInfo();
            string controlport = string.Empty;
            string ssz = "";
            int isAlarm = 0;
            //模拟量和开关量获取控制口
            if (defInfo.DevPropertyID == (int)DeviceProperty.Analog)
            {
                controlport = ControlBus.GetAnalogControlPort(defInfo, dataState);
            }
            else if (defInfo.DevPropertyID == (int)DeviceProperty.Derail)
            {
                controlport = ControlBus.GetDerailControlPort(defInfo, dataState);
            }

            alarmInfo.Cs = "";
            alarmInfo.PointID = defInfo.PointID;
            alarmInfo.ID = IdHelper.CreateLongId().ToString();
            alarmInfo.Devid = defInfo.Devid;
            alarmInfo.Fzh = defInfo.Fzh;
            alarmInfo.Kh = defInfo.Kh;
            alarmInfo.Dzh = defInfo.Dzh;
            alarmInfo.Kzk = controlport;
            alarmInfo.Point = defInfo.Point;
            if (defInfo.DevPropertyID == (int)DeviceProperty.Analog)
            {
                alarmInfo.Ssz = defInfo.Ssz;
                alarmInfo.Isalarm = (byte)defInfo.Alarm;
            }
            else if (defInfo.DevPropertyID == (int)DeviceProperty.Derail)
            {
                SafetyHelper.GetDerailShowInfo(defInfo, dataState, ref ssz, ref isAlarm);
                alarmInfo.Ssz = ssz;
                alarmInfo.Isalarm = (short)isAlarm;
            }
            else if (defInfo.DevPropertyID == (int)DeviceProperty.Control)
            {
                alarmInfo.Ssz = EnumHelper.GetEnumDescription(dataState);
                //控制量只有馈电异常报警写B表
                alarmInfo.Isalarm = 1;
            }
            else
            {
                //只有分站会进此处理
                alarmInfo.Ssz = showValue;
                alarmInfo.Isalarm = alarm;
            }
            alarmInfo.Stime = sTime;
            alarmInfo.Etime = new DateTime(1900, 1, 1, 0, 0, 0);
            alarmInfo.Type = (short)dataState;
            alarmInfo.State = SafetyHelper.GetState((DeviceRunState)state, defInfo.Bz4);
            alarmInfo.Upflag = "0";
            alarmInfo.Wzid = defInfo.Wzid;

            return alarmInfo;
        }

        /// <summary>
        /// 控制口变化生成报警信息
        /// </summary>
        /// <param name="def"></param>
        /// <param name="datastate"></param>
        /// <param name="stime"></param>
        /// <returns></returns>
        public static Jc_BInfo CreateControlChangeAlarmInfo(Jc_DefInfo def, DeviceDataState datastate, DateTime stime)
        {
            Jc_BInfo JC_B = new Jc_BInfo();
            string ssz = "";
            int isAlarm = 0;//无用，传参数需要
            if (def.DevPropertyID == (int)DeviceProperty.Analog)
            {
                JC_B.Kzk = ControlBus.GetAnalogControlPort(def, datastate);
            }
            else if (def.DevPropertyID == (int)DeviceProperty.Derail)
            {
                SafetyHelper.GetDerailShowInfo(def, datastate, ref ssz, ref isAlarm);
                JC_B.Ssz = ssz;
                JC_B.Kzk = ControlBus.GetDerailControlPort(def, datastate);
            }
            JC_B.Point = def.Point;
            JC_B.Stime = stime;
            JC_B.Type = (short)datastate;
            return JC_B;
        }
        #endregion

        #region ----辅助方法----
        /// <summary>
        /// 上限预警 0x01
        /// </summary>
        public static short dataHighPreAlarmInt = 0x01;
        /// <summary>
        /// 上限报警 0x02
        /// </summary>
        public static short dataHighAlarmInt = 0x02;
        /// <summary>
        /// 上限断电 0x04
        /// </summary>
        public static short dataHighAlarmPowerOFFInt = 0x04;
        /// <summary>
        /// 下限预警 0x10
        /// </summary>
        public static short dataLowPreAlarmInt = 0x10;
        /// <summary>
        /// 下限报警 0x20
        /// </summary>
        public static short dataLowAlarmInt = 0x20;
        /// <summary>
        /// 下限断电 0x40
        /// </summary>
        public static short dataLowPowerInt = 0x40;
        /// <summary>
        /// 设备异常
        /// </summary>
        public static short dataDeiviceError = 0x80;

        /// <summary>
        /// 判断当前报警字是否包含此报警类型（true 包含，false 不包含）
        /// </summary>
        /// <param name="alarm"></param>
        /// <param name="alarmState"></param>
        /// <returns></returns>
        public static bool JudgeAlarmIsAlarmState(int alarm, DeviceDataState alarmState)
        {
            bool defaultFlag = false;
            if ((alarm & 0x01) == 0x01 && alarmState == DeviceDataState.DataHighPreAlarm)
            {
                return true;
            }
            if ((alarm & 0x02) == 0x02 && alarmState == DeviceDataState.DataHighAlarm)
            {
                return true;
            }
            if ((alarm & 0x04) == 0x04 && alarmState == DeviceDataState.DataHighAlarmPowerOFF)
            {
                return true;
            }

            if ((alarm & 0x10) == 0x10 && alarmState == DeviceDataState.DataLowPreAlarm)
            {
                return true;
            }
            if ((alarm & 0x20) == 0x20 && alarmState == DeviceDataState.DataLowAlarm)
            {
                return true;
            }
            if ((alarm & 0x40) == 0x40 && alarmState == DeviceDataState.DataLowPower)
            {
                return true;
            }

            return defaultFlag;
        }

        #endregion
    }
}
