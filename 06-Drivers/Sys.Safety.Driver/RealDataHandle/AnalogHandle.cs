using Basic.Framework.Common;
using Basic.Framework.Logging;
using Sys.DataCollection.Common.Protocols;
using Sys.Safety.DataContract;
using Sys.Safety.Enums;
using Sys.Safety.Request.Cache;
using System;
using System.Collections.Generic;

namespace Sys.Safety.Driver.RealDataHandle
{
    /// <summary>
    /// 作者：
    /// 时间：2017-05-31
    /// 描述：模拟量实时数据处理
    /// 修改记录
    /// 2017-05-31
    /// </summary>
    public class AnalogHandle : PointHandle
    {

        /// <summary>
        /// 用于生成五分钟以及报警断电时的最大值，最小值。
        /// </summary>
        /// <param name="Type">Type=1五分钟，=2报警最大，最小值，=3断电最大最小值，=4报警断电平均值</param>
        private void HandleMaxMinData(byte Type, decimal value)
        {
            if (PointDefineInfo.DevClassID == 1 && value > 40)
            {
                LogHelper.Debug("【" + PointDefineInfo.Point + "】HandleMaxMinData:value=" + value + ",type=" + Type + ",State=" + PointDefineInfo.State);
            }
            if (Type == 1)
            {
                #region 处理五分钟的平均值(设备状态不正常五分钟数据不处理)
                if (!SafetyHelper.isAbnormalState((DeviceRunState)((int)RealDataItem.State)))
                {
                    //todo:原则上此次要加锁，必免五分钟入库时的冲突问题

                    if (PointDefineInfo.ClsFiveMinObj.m_nAllCount == 0)
                    {
                        PointDefineInfo.ClsFiveMinObj.m_nAllCount = 1;
                        PointDefineInfo.ClsFiveMinObj.m_nMaxVal = value;
                        PointDefineInfo.ClsFiveMinObj.m_nMinVal = value;
                        PointDefineInfo.ClsFiveMinObj.m_nAllVal = value;
                        PointDefineInfo.ClsFiveMinObj.m_nMaxValTime = CreatedTime;
                        PointDefineInfo.ClsFiveMinObj.m_nMinValTime = CreatedTime;
                        PointDefineInfo.ClsFiveMinObj.m_maxValueDataState = PointDefineInfo.DataState;//---------
                        PointDefineInfo.ClsFiveMinObj.m_maxValueSBState = PointDefineInfo.State;//-------------
                    }
                    else
                    {
                        PointDefineInfo.ClsFiveMinObj.m_nAllCount++;
                        PointDefineInfo.ClsFiveMinObj.m_nAllVal += value;
                        // 处理五分钟的最大值/最小值/最大时/最小时
                        if (PointDefineInfo.Ssz != value.ToString())
                        {
                            if (value > PointDefineInfo.ClsFiveMinObj.m_nMaxVal)
                            {
                                PointDefineInfo.ClsFiveMinObj.m_nMaxVal = value;
                                PointDefineInfo.ClsFiveMinObj.m_nMaxValTime = CreatedTime;
                                PointDefineInfo.ClsFiveMinObj.m_maxValueDataState = PointDefineInfo.DataState;//---------------
                                PointDefineInfo.ClsFiveMinObj.m_maxValueSBState = PointDefineInfo.State;//---------
                            }
                            if (value < PointDefineInfo.ClsFiveMinObj.m_nMinVal)
                            {
                                PointDefineInfo.ClsFiveMinObj.m_nMinVal = value;
                                PointDefineInfo.ClsFiveMinObj.m_nMinValTime = CreatedTime;
                            }
                        }
                    }
                }
                #endregion
            }
            else if (Type == 2)
            {
                #region 处理报警状态下的最大值/最大时/最小值/最小值时
                if (AlarmBusiness.JudgeAlarmIsAlarmState(PointDefineInfo.Alarm, DeviceDataState.DataHighAlarm) ||
               AlarmBusiness.JudgeAlarmIsAlarmState(PointDefineInfo.Alarm, DeviceDataState.DataLowAlarm))
                {
                    if (value >= PointDefineInfo.ClsAlarmObj.NAlarmMaxVal)
                    {
                        PointDefineInfo.ClsAlarmObj.NAlarmMaxVal = value;
                        PointDefineInfo.ClsAlarmObj.DttAlarmMaxValTime = CreatedTime;
                    }
                    if (value <= PointDefineInfo.ClsAlarmObj.NAlarmMinVal)
                    {
                        PointDefineInfo.ClsAlarmObj.NAlarmMinVal = value;
                        PointDefineInfo.ClsAlarmObj.DttAlarmMinValTime = CreatedTime;
                    }
                }
                #endregion
            }
            else if (Type == 3)
            {
                #region 处理断电状态下的最大值/最大时/最小值/最小值时
                //if ((PointDefineInfo.Alarm & 0x04) == 0x04 || (PointDefineInfo.Alarm & 0x40) == 0x40)
                if (AlarmBusiness.JudgeAlarmIsAlarmState(PointDefineInfo.Alarm, DeviceDataState.DataHighAlarmPowerOFF) ||
               AlarmBusiness.JudgeAlarmIsAlarmState(PointDefineInfo.Alarm, DeviceDataState.DataLowPower))
                {
                    if (value > PointDefineInfo.ClsAlarmObj.NPowerOffMaxVal)
                    {
                        PointDefineInfo.ClsAlarmObj.NPowerOffMaxVal = value;
                        PointDefineInfo.ClsAlarmObj.DttPowerOffMaxValTime = CreatedTime;
                    }
                    if (value < PointDefineInfo.ClsAlarmObj.NPowerOffMinVal)
                    {
                        PointDefineInfo.ClsAlarmObj.NPowerOffMinVal = value;
                        PointDefineInfo.ClsAlarmObj.DttPowerOffMinValTime = CreatedTime;
                    }
                }
                #endregion
            }
            else if (Type == 4)
            {
                #region 报警断电下的平均值
                //if ((PointDefineInfo.Alarm & 0x02) == 0x02 || (PointDefineInfo.Alarm & 0x20) == 0x20)
                if (AlarmBusiness.JudgeAlarmIsAlarmState(PointDefineInfo.Alarm, DeviceDataState.DataHighAlarm) ||
                    AlarmBusiness.JudgeAlarmIsAlarmState(PointDefineInfo.Alarm, DeviceDataState.DataLowAlarm))
                {
                    PointDefineInfo.ClsAlarmObj.NAlarmAllCount++;
                    PointDefineInfo.ClsAlarmObj.NAlarmAllVal += value;
                }
                if (AlarmBusiness.JudgeAlarmIsAlarmState(PointDefineInfo.Alarm, DeviceDataState.DataHighAlarmPowerOFF) ||
                   AlarmBusiness.JudgeAlarmIsAlarmState(PointDefineInfo.Alarm, DeviceDataState.DataLowPower))
                {
                    PointDefineInfo.ClsAlarmObj.NPowerOffAllCount++;
                    PointDefineInfo.ClsAlarmObj.NPowerOffAllVal += value;
                }
                #endregion
            }
        }
        protected override Dictionary<string, object> DataHandle()
        {
            decimal value = 0;
            decimal lsvalue = 0;
            Dictionary<string, object> updateItems = new Dictionary<string, object>();

            //#region ----判断是否写五分钟记录，并进行处理 2017.7.26 by----
            //if (PointDefineInfo.DoFiveMinData && PointDefineInfo.ClsFiveMinObj.m_nAllCount > 0) //2017.10.13 by  增加PointDefineInfo.ClsFiveMinObj.m_nAllCount > 0条件，以避免分站突然中断，过断时间再恢复正常后，五分钟内存数据还是以前的（分站中断m_nAllCount会置0，重新计算五分钟数据）
            //{
            //    FiveMinBusiness.CreateFiviMinInfo(PointDefineInfo, DateTime.Now);
            //    PointDefineInfo.ClsFiveMinObj.m_nAllCount = 0; //重新计数
            //    updateItems.Add("DoFiveMinData", false);
            //}
            //#endregion
            //if (SafetyHelper.pointTimeTemp.ContainsKey(PointDefineInfo.Point))
            //{
            //    if (SafetyHelper.pointTimeTemp[PointDefineInfo.Point].ToString("yyyy-MM-dd HH:mm:ss") == CreatedTime.ToString("yyyy-MM-dd HH:mm:ss"))
            //    {
            //        return null;//上一次时间与当前时间一样的数据，不处理 
            //    }
            //}

            //if (SafetyHelper.pointTimeTemp.ContainsKey(PointDefineInfo.Point))
            //{
            //    SafetyHelper.pointTimeTemp[PointDefineInfo.Point] = CreatedTime;
            //}
            //else
            //{
            //    SafetyHelper.pointTimeTemp.Add(PointDefineInfo.Point, CreatedTime);
            //}

            float Voltage = 0;
            float.TryParse(RealDataItem.Voltage, out Voltage);
            if (PointDefineInfo.Voltage != Voltage)
            {
                PointDefineInfo.Voltage = Voltage;//2018.4.8 by
                updateItems.Add("Voltage", PointDefineInfo.Voltage);
            }
            //判断电量过低,并写运行记录  20180830
            if (PointDefineInfo.Devid == "24")
            {
                if (PointDefineInfo.Voltage < SafetyHelper.SensorPowerAlarmValue)
                {
                    if ((CreatedTime - PointDefineInfo.DttSensorPowerAlarmTime).TotalMinutes > 60)//每小时写一次
                    {
                        //写电量过低运行记录
                        SafetyHelper.CreateRunLogInfo(PointDefineInfo, CreatedTime, (short)DeviceDataState.SensorPowerAlarm, (short)DeviceRunState.SensorPowerAlarm, PointDefineInfo.Ssz);
                        //更新内存的时间
                        Dictionary<string, object> updateItems1 = new Dictionary<string, object>();
                        updateItems1.Add("DttSensorPowerAlarmTime", CreatedTime);
                        SafetyHelper.UpdatePointDefineInfoByProperties(PointDefineInfo.PointID, updateItems1);
                    }
                }
            }

            if (decimal.TryParse(RealDataItem.RealData, out value))
            {
                //模拟量数值变动才处理
                bool bValuechangeflg = false;
                if (decimal.TryParse(PointDefineInfo.Ssz, out lsvalue))
                {
                    //20190119:

                    if (lsvalue != value)
                    {
                        if (IsImpDevice(PointDefineInfo))
                        {
                            //重要设备类型，变值存储
                            bValuechangeflg = true;
                            ChangeRateStore(ref value);//判断数据是否超出量程范围
                        }
                        else
                        {
                            //普通设备类型，判断变化系数
                            bValuechangeflg = ChangeRateStore(ref value);
                        }
                    }
                }
                bool isGradingAlarmLevelChgFlg = false;
                if (PointDefineInfo.GradingAlarmLevel != RealDataItem.SeniorGradeAlarm)
                {
                    isGradingAlarmLevelChgFlg = true;
                }
                //判断如果中心站未设置分级报警，则不显示分级报警
                if (string.IsNullOrEmpty(PointDefineInfo.Bz10) || PointDefineInfo.Bz10 == "0")
                {
                    RealDataItem.SeniorGradeAlarm = 0;//中心站未启用，则置为0
                }
                //计算五分钟
                if ((DeviceRunState)RealDataItem.State != DeviceRunState.EquipmentStart) //2017.10.13 by  开机中只写JC_R和JC_MC
                {
                    HandleMaxMinData(1, value);
                    if (bValuechangeflg || isGradingAlarmLevelChgFlg || PointDefineInfo.State != (short)RealDataItem.State || PointDefineInfo.ReDoDeal == 2)
                    {

                        PointDefineInfo.ReDoDeal = 0;
                        PointDefineInfo.Ssz = RealDataItem.RealData;
                        PointDefineInfo.DttStateTime = DateTime.Now;
                        PointDefineInfo.GradingAlarmLevel = RealDataItem.SeniorGradeAlarm;
                        // 测点运行记录
                        PointRunRecord(DeviceDataState.EquipmentCommOK, (DeviceRunState)RealDataItem.State);
                        HandleMaxMinData(2, value);//报警的最大最小值及时间
                        HandleMaxMinData(3, value);//断电的最大最小值及时间
                    }
                    HandleMaxMinData(4, value);//处理报警断电中的平均值
                }
                else
                {
                    if (bValuechangeflg || isGradingAlarmLevelChgFlg || PointDefineInfo.State != (short)RealDataItem.State || PointDefineInfo.ReDoDeal == 2)
                    {
                        //if (KJ73NHelper.isAbnormalDataState((DeviceDataState)PointDefineInfo.DataState)) //设备状态变动后，现在是正常，结束之前的异常数据报警与控制
                        {
                            //2017.11.9 by 任何状态切换到开机中   都应该把之前的所有报警、控制状态取消掉,不然从正常的上限断电、异常断线  突然切换到开机中   结束不到报警和控制了

                            //结束之前的报警(暂时不解除控制，后面单独把断线和故障一起解除)
                            AlarmBusiness.EndAnalogAlarm(PointDefineInfo, CreatedTime, false);
                            //解除断线/异常（上溢负漂）交叉控制(首次进来处理，数据状态为未知，根据数据状态无法解除控制，所以此处只能单独强制结束这两种异常控制)
                            List<Jc_JcsdkzInfo> jckzItems = new List<Jc_JcsdkzInfo>();
                            jckzItems.AddRange(ControlBus.GetManualCrossControlFromCache(PointDefineInfo.Point, ControlType.ControlErro));
                            jckzItems.AddRange(ControlBus.GetManualCrossControlFromCache(PointDefineInfo.Point, ControlType.ControlLineDown));
                            jckzItems.AddRange(ControlBus.GetManualCrossControlFromCache(PointDefineInfo.Point, ControlType.ControlPowerOff));
                            if (jckzItems.Count > 0)
                            {
                                jckzItems.ForEach(a =>
                                {
                                    a.InfoState = Basic.Framework.Web.InfoState.Delete;
                                });
                                ControlBus.OperationManualCrossControl(jckzItems);
                            }
                        }
                        if (bValuechangeflg) { LogHelper.Info("bValuechangeflg写开机记录"); }
                        if (PointDefineInfo.State != (short)RealDataItem.State) { LogHelper.Info("State写开机记录"); }
                        if (PointDefineInfo.ReDoDeal == 2) { LogHelper.Info("ReDoDeal写开机记录"); }

                        PointDefineInfo.Alarm = 0;//2018.3.16 by
                        PointDefineInfo.ReDoDeal = 0;
                        PointDefineInfo.Ssz = RealDataItem.RealData;
                        PointDefineInfo.DataState = (short)DeviceDataState.EquipmentStart;
                        PointDefineInfo.State = (short)RealDataItem.State;
                        PointDefineInfo.DttStateTime = DateTime.Now;
                        PointDefineInfo.GradingAlarmLevel = RealDataItem.SeniorGradeAlarm;
                        //写运行正常日志
                        SafetyHelper.CreateRunLogInfo(PointDefineInfo, CreatedTime, (short)DeviceDataState.EquipmentCommOK, (short)DeviceRunState.EquipmentStart, PointDefineInfo.Ssz);
                        //写密采记录
                        SafetyHelper.CreateDentisyCollInfo(PointDefineInfo, CreatedTime, DeviceDataState.EquipmentCommOK, DeviceRunState.EquipmentStart);
                    }
                }
                updateItems.Add("ReDoDeal", PointDefineInfo.ReDoDeal);
                updateItems.Add("ClsFiveMinObj", PointDefineInfo.ClsFiveMinObj);
                updateItems.Add("ClsAlarmObj", PointDefineInfo.ClsAlarmObj);
                updateItems.Add("Ssz", PointDefineInfo.Ssz);
              
                updateItems.Add("Zts", PointDefineInfo.Zts);
                //updateItems.Add("DttStateTime", PointDefineInfo.DttStateTime);
                updateItems.Add("Alarm", PointDefineInfo.Alarm);
                updateItems.Add("DataState", PointDefineInfo.DataState);
                updateItems.Add("State", PointDefineInfo.State);
                updateItems.Add("DttRunStateTime", PointDefineInfo.DttRunStateTime);
                updateItems.Add("CalibrationNum", PointDefineInfo.CalibrationNum);
                updateItems.Add("GradingAlarmLevel", PointDefineInfo.GradingAlarmLevel);
            }
            else
            {
                //设备传的数据非数字  
                if ((PointDefineInfo.State != (short)RealDataItem.State) || PointDefineInfo.ReDoDeal == 2)
                {
                    PointDefineInfo.ReDoDeal = 0;
                    PointDefineInfo.Ssz = RealDataItem.RealData;
                    PointDefineInfo.DttStateTime = DateTime.Now;
                    PointDefineInfo.GradingAlarmLevel = RealDataItem.SeniorGradeAlarm;
                    //状态或值发生变化，写记录
                    PointRunRecord(DeviceDataState.EquipmentDown, (DeviceRunState)RealDataItem.State);
                    updateItems.Add("ReDoDeal", PointDefineInfo.ReDoDeal);
                    updateItems.Add("ClsFiveMinObj", PointDefineInfo.ClsFiveMinObj);
                    updateItems.Add("ClsAlarmObj", PointDefineInfo.ClsAlarmObj);
                    updateItems.Add("Ssz", PointDefineInfo.Ssz);                    
                    updateItems.Add("Zts", PointDefineInfo.Zts);
                    //updateItems.Add("DttStateTime", PointDefineInfo.DttStateTime);
                    updateItems.Add("Alarm", PointDefineInfo.Alarm);
                    updateItems.Add("StrMeasure", PointDefineInfo.StrMeasure);
                    updateItems.Add("DataState", PointDefineInfo.DataState);
                    updateItems.Add("State", PointDefineInfo.State);
                    updateItems.Add("DttRunStateTime", PointDefineInfo.DttRunStateTime);
                    updateItems.Add("CalibrationNum", PointDefineInfo.CalibrationNum);
                    updateItems.Add("GradingAlarmLevel", PointDefineInfo.GradingAlarmLevel);
                }
            }


            //if (updateItems.Count > 0)
            //{
            //    KJ73NHelper.UpdatePointDefineInfoByProperties(PointDefineInfo.PointID, updateItems);
            //}
            if (!updateItems.ContainsKey("DttStateTime"))
            {
                updateItems.Add("DttStateTime", CreatedTime);
            }

            return updateItems;
        }
        protected override void PointRunRecord(DeviceDataState dataState, DeviceRunState runState)
        {
            bool highAlarm = true;//是否设置上限报警 
            bool lowAlarm = true;  //是否设置下限报警 
            bool isWriteRunLog = false;
            DateTime time = CreatedTime;
            Jc_BInfo alarmInfo;
            List<Jc_JcsdkzInfo> jckzItems;
            List<Jc_JcsdkzInfo> myJckzItems;
            List<Jc_JcsdkzInfo> updateJckzItems;

            if (runState == DeviceRunState.EquipmentAdjusting && PointDefineInfo.State != (short)runState)
            {
                PointDefineInfo.CalibrationNum = IdHelper.CreateLongId().ToString();  //2017.10.13 by 状态切换为标校时，更新此编码
            }
            int index = 0;
            //判断是否设置有上下限报警
            if ((PointDefineInfo.Z1 == 0) && (PointDefineInfo.Z2 == 0) && (PointDefineInfo.Z3 == 0) && (PointDefineInfo.Z4 == 0))
            {
                highAlarm = false;
            }
            if ((PointDefineInfo.Z5 == 0) && (PointDefineInfo.Z6 == 0) && (PointDefineInfo.Z7 == 0) && (PointDefineInfo.Z8 == 0))
            {
                lowAlarm = false;
            }

            if ((PointDefineInfo.Bz4 & 0x02) != 0x02)
            {
                if (SafetyHelper.isAbnormalState(runState))
                {
                    #region ----设备状态异常处理---
                    //获取异常设备状态下的数据状态
                    dataState = SafetyHelper.GetDataStateByAbnormalState(runState);
                    if (PointDefineInfo.DataState != (short)dataState || PointDefineInfo.State != (short)runState)  //状态变化才写记录
                    {
                        //写密采记录
                        SafetyHelper.CreateDentisyCollInfo(PointDefineInfo, time, dataState, runState);
                        //添加运行记录
                        SafetyHelper.CreateRunLogInfo(PointDefineInfo, time, (short)dataState, (short)runState, EnumHelper.GetEnumDescription(dataState));
                    }
                    //更新实时值
                    PointDefineInfo.Ssz = EnumHelper.GetEnumDescription(dataState);                  
                    PointDefineInfo.Zts = time;

                    #endregion
                }
                else
                {
                    #region ----设备状态正常处理---
                   

                    dataState = DeviceDataState.EquipmentCommOK;
                    PointDefineInfo.Alarm &= (0xFF - 0x80);//清除高位的断线标记 2017.7.5 by
                    isWriteRunLog |= JudgeDataHighPreAlarm(dataState, runState, highAlarm);
                    isWriteRunLog |= JudgeDataHighAlarm(dataState, runState, highAlarm);
                    isWriteRunLog |= JudgeDataHighAlarmPowerOFF(dataState, runState, highAlarm);
                    isWriteRunLog |= JudgeDataLowPreAlarm(dataState, runState, lowAlarm);
                    isWriteRunLog |= JudgeDataLowAlarm(dataState, runState, lowAlarm);
                    isWriteRunLog |= JudgeDataLowPower(dataState, runState, lowAlarm);
                    //重新处理DataState的状态
                    dataState = GetDataStateByAlarm(PointDefineInfo.Alarm);
                    //数据状态没有改变时的运行记录（数据状态在预警、报警、断电中写）
                    if (PointDefineInfo.State != (int)runState && !isWriteRunLog)
                    {
                        //写运行正常日志
                        SafetyHelper.CreateRunLogInfo(PointDefineInfo, time, (short)dataState, (short)runState, PointDefineInfo.Ssz);
                    }
                    //更新测点实时值
                    PointDefineInfo.Zts = time;
                    //写密采记录
                    SafetyHelper.CreateDentisyCollInfo(PointDefineInfo, time, dataState, runState);
                    #endregion
                }
                #region ----原处理----

                //switch (runState)
                //{
                //    case DeviceRunState.EquipmentDown:
                //    case DeviceRunState.EquipmentHeadDown:
                //    case DeviceRunState.EquipmentStateUnknow://todo 上溢   负漂   类型不匹配 通讯误码
                //        // 断线/设备中断/状态未知
                //        //dataState = DeviceDataState.EquipmentDown;
                //        dataState = (DeviceDataState)runState;
                //        //写密采记录
                //        KJ73NHelper.CreateDentisyCollInfo(PointDefineInfo, time, dataState, runState);
                //        //添加运行记录
                //        KJ73NHelper.CreateRunLogInfo(PointDefineInfo, time, (short)DeviceDataState.EquipmentDown, (short)runState, EnumHelper.GetEnumDescription(dataState));
                //        //更新实时值
                //        PointDefineInfo.Ssz = EnumHelper.GetEnumDescription(dataState); 
                //        PointDefineInfo.Zts = time;
                //        break;
                //    case DeviceRunState.EquipmentStart:
                //    case DeviceRunState.EquipmentAdjusting:
                //    case DeviceRunState.EquipmentInfrareding:
                //    case DeviceRunState.EquipmentCommOK:
                //        #region 判断正常/上限预警/上限报警/上限断电/上限恢复/下限预警/下限报警/下限断电/下限恢复
                //        dataState = DeviceDataState.EquipmentCommOK;
                //        isWriteRunLog |= JudgeDataHighPreAlarm(dataState, runState, highAlarm);
                //        isWriteRunLog |= JudgeDataHighAlarm(dataState, runState, highAlarm);
                //        isWriteRunLog |= JudgeDataHighAlarmPowerOFF(dataState, runState, highAlarm);
                //        isWriteRunLog |= JudgeDataLowPreAlarm(dataState, runState, lowAlarm);
                //        isWriteRunLog |= JudgeDataLowAlarm(dataState, runState, lowAlarm);
                //        isWriteRunLog |= JudgeDataLowPower(dataState, runState, lowAlarm);
                //        //重新处理DataState的状态
                //        dataState = GetDataStateByAlarm(PointDefineInfo.Alarm);
                //        //数据状态没有改变时的运行记录（数据状态在预警、报警、断电中写）
                //        if (PointDefineInfo.State != (int)runState && !isWriteRunLog)
                //        {
                //            //写运行正常日志
                //            KJ73NHelper.CreateRunLogInfo(PointDefineInfo, time, (short)dataState, (short)runState, PointDefineInfo.Ssz);
                //        }
                //        //更新测点实时值
                //        PointDefineInfo.Zts = time;
                //        //写密采记录
                //        KJ73NHelper.CreateDentisyCollInfo(PointDefineInfo, time, dataState, runState);
                //        #endregion
                //        break;
                //    case DeviceRunState.EquipmentOverrange:
                //        //上溢
                //        dataState = DeviceDataState.EquipmentOverrange;
                //        KJ73NHelper.CreateDentisyCollInfo(PointDefineInfo, time, dataState, runState);
                //        PointDefineInfo.Ssz = EnumHelper.GetEnumDescription(dataState);
                //        PointDefineInfo.Zts = time;
                //        KJ73NHelper.CreateRunLogInfo(PointDefineInfo, time, (short)dataState, (short)runState, EnumHelper.GetEnumDescription(dataState));
                //        break;
                //    case DeviceRunState.EquipmentUnderrange:
                //        //负漂
                //        dataState = DeviceDataState.EquipmentUnderrange;
                //        KJ73NHelper.CreateDentisyCollInfo(PointDefineInfo, time, dataState, runState);
                //        PointDefineInfo.Ssz = EnumHelper.GetEnumDescription(dataState);
                //        PointDefineInfo.Zts = time;
                //        KJ73NHelper.CreateRunLogInfo(PointDefineInfo, time, (short)dataState, (short)runState, EnumHelper.GetEnumDescription(dataState));
                //        break;
                //    case DeviceRunState.EquipmentBiterror:
                //        // 通讯误码
                //        if (PointDefineInfo.State != (short)runState)
                //        {
                //            KJ73NHelper.CreateRunLogInfo(PointDefineInfo, time, (short)DeviceDataState.EquipmentDown, (short)runState, EnumHelper.GetEnumDescription(dataState));
                //        }
                //        PointDefineInfo.Zts = time;
                //        break;
                //}

                #endregion
                //上一次状态和本次通讯状态不等时
                if (PointDefineInfo.DataState != (short)dataState)
                {
                    //不包括误码
                    if (SafetyHelper.isAbnormalDataState((DeviceDataState)PointDefineInfo.DataState)) //设备状态变动后，现在是正常，结束之前的异常数据报警与控制
                    {
                        //结束之前的异常报警(暂时不解除报警，后面单独把断线和故障一起解除)
                        AlarmBusiness.EndAnalogAlarm(PointDefineInfo, time, false);
                        //解除断线/异常（上溢负漂）交叉控制(首次进来处理，数据状态为未知，根据数据状态无法解除控制，所以此处只能单独强制结束这两种异常控制)
                        jckzItems = new List<Jc_JcsdkzInfo>();
                        jckzItems.AddRange(ControlBus.GetManualCrossControlFromCache(PointDefineInfo.Point, ControlType.ControlErro));
                        jckzItems.AddRange(ControlBus.GetManualCrossControlFromCache(PointDefineInfo.Point, ControlType.ControlLineDown));
                        if (jckzItems.Count > 0)
                        {
                            jckzItems.ForEach(a =>
                            {
                                a.InfoState = Basic.Framework.Web.InfoState.Delete;
                            });
                            ControlBus.OperationManualCrossControl(jckzItems);
                        }
                    }

                    if (SafetyHelper.isAbnormalDataState(dataState)) //设备状态变动后，现在是异常，之前是正常，结束之前的正常数据报警与控制
                    {
                        jckzItems = new List<Jc_JcsdkzInfo>();
                        myJckzItems = new List<Jc_JcsdkzInfo>();
                        updateJckzItems = new List<Jc_JcsdkzInfo>();
                        //结束之前的正常数据状态报警（此处不删除报警，待后面单独取出来与要下发的报警一起处理）
                        AlarmBusiness.EndAnalogAlarm(PointDefineInfo, time, false);
                        //解除之前的正常数据状态断电控制
                        jckzItems = ControlBus.GetManualCrossControlFromCache(PointDefineInfo.Point, ControlType.ControlPowerOff);
                        if (jckzItems.Count > 0)
                        {
                            jckzItems.ForEach(a =>
                            {
                                a.InfoState = Basic.Framework.Web.InfoState.Delete;
                            });
                        }
                        //生成当前断线交叉控制/异常（上溢负漂）交叉控制
                        ControlType controlType = SafetyHelper.GetControlTypeByAbnormalState(runState);
                        if (controlType != ControlType.NoControl)
                        {
                            myJckzItems = ControlBus.GetAddManualCrossControlFromDefine(PointDefineInfo, controlType);
                            myJckzItems.ForEach(jckz =>
                            {
                                jckz.InfoState = Basic.Framework.Web.InfoState.AddNew;
                            });
                        }
                        //添加到链表统一处理
                        if (jckzItems.Count > 0)
                        {
                            updateJckzItems.AddRange(jckzItems);
                        }
                        if (myJckzItems.Count > 0)
                        {
                            updateJckzItems.AddRange(myJckzItems);
                        }
                        if (updateJckzItems.Count > 0)
                        {
                            ControlBus.OperationManualCrossControl(updateJckzItems);
                        }

                        //生成新报警
                        PointDefineInfo.Alarm = AlarmBusiness.dataDeiviceError;
                        alarmInfo = AlarmBusiness.CreateAlarmInfo(PointDefineInfo, time, dataState, runState, 1);
                        AlarmBusiness.UpdateAlarmInfo(alarmInfo, 1, false);
                        PointDefineInfo.DttRunStateTime = time;
                        //PointDefineInfo.Alarm = 0;
                    }
                }
                if (PointDefineInfo.DataState != (short)dataState || PointDefineInfo.State != (short)runState)
                {
                    PointDefineInfo.State = (short)runState;
                    PointDefineInfo.DataState = (short)dataState;
                    PointDefineInfo.Zts = CreatedTime;//增加状态变化时间赋值  20181113
                }
            }
        }
        public override bool PretreatmentHandle(Jc_DefInfo pointDefineInfo)
        {
            Dictionary<string, object> updateItems = new Dictionary<string, object>();
            DateTime time = DateTime.Now;
            List<Jc_DefInfo> analogItems = new List<Jc_DefInfo>();

            if (pointDefineInfo.ReDoDeal == 1)
            {
                pointDefineInfo.ReDoDeal = 2;
                updateItems.Add("ReDoDeal", pointDefineInfo.ReDoDeal);
            }

            analogItems.Add(pointDefineInfo);
            //模拟量删除
            if (pointDefineInfo.InfoState == Basic.Framework.Web.InfoState.Delete || pointDefineInfo.Activity == "0")
            {
                AlarmBusiness.BatchEndAnalogAlarm(analogItems, time, true);
            }
            //模拟量新增
            else if (pointDefineInfo.InfoState == Basic.Framework.Web.InfoState.AddNew)
            {
                pointDefineInfo.State = (short)DeviceRunState.EquipmentStateUnknow;
                pointDefineInfo.DataState = (short)DeviceRunState.EquipmentStateUnknow;
                //设备新增，默认设备类型匹配
                pointDefineInfo.BCommDevTypeMatching = true;
                updateItems.Add("BCommDevTypeMatching", pointDefineInfo.BCommDevTypeMatching);
            }
            //模拟量修改
            else if (pointDefineInfo.InfoState == Basic.Framework.Web.InfoState.Modified)
            {
                //控制口变化处理
                if (pointDefineInfo.kzchangeflag)
                {
                    AnalogControlChange(pointDefineInfo);
                }
                //休眠处理
                if ((pointDefineInfo.Bz4 & 0x02) == 0x02)
                {
                    #region ----设备休眠----
                    //结束报警
                    AlarmBusiness.EndAnalogAlarm(pointDefineInfo, time, true);
                    updateItems.Add("ClsAlarmObj", PointDefineInfo.ClsAlarmObj);
                    //updateItems.Add("Alarm", PointDefineInfo.Alarm);
                    updateItems.Add("Alarm", 0);
                    //写R表记录
                    SafetyHelper.CreateRunLogInfo(pointDefineInfo, time, PointDefineInfo.DataState, (short)DeviceRunState.EquipmentSleep, pointDefineInfo.Ssz);
                    //模拟量休眠
                    pointDefineInfo.DataState = (short)DeviceDataState.EquipmentSleep;
                    pointDefineInfo.State = (short)DeviceRunState.EquipmentSleep;
                    #endregion
                }

                updateItems.Add("Ssz", pointDefineInfo.Ssz);
                updateItems.Add("State", pointDefineInfo.State);
                updateItems.Add("DataState", pointDefineInfo.DataState);
            }
            if (updateItems.Count > 0)
            {
                SafetyHelper.UpdatePointDefineInfoByProperties(pointDefineInfo.PointID, updateItems);
            }
            return pointDefineInfo.DefIsInit;
        }

        #region 业务方法
        /// <summary>
        /// 根据报警状态获取设备当前的数据状态
        /// </summary>
        /// <param name="alarm"></param>
        /// <returns></returns>
        private DeviceDataState GetDataStateByAlarm(int alarm)
        {
            DeviceDataState dataState = DeviceDataState.EquipmentStateUnknow;
            if (alarm == 0)
            {
                dataState = DeviceDataState.EquipmentCommOK;
            }
            else if ((alarm & 0x04) == 0x04)
            {
                dataState = DeviceDataState.DataHighAlarmPowerOFF;
            }
            else if ((alarm & 0x02) == 0x02)
            {
                dataState = DeviceDataState.DataHighAlarm;
            }
            else if ((alarm & 0x01) == 0x01)
            {
                dataState = DeviceDataState.DataHighPreAlarm;
            }
            else if ((alarm & 0x40) == 0x40)
            {
                dataState = DeviceDataState.DataLowPower;
            }
            else if ((alarm & 0x20) == 0x20)
            {
                dataState = DeviceDataState.DataLowAlarm;
            }
            else if ((alarm & 0x10) == 0x10)
            {
                dataState = DeviceDataState.DataLowPreAlarm;
            }
            else if ((alarm & 0x80) == 0x80)
            {
                dataState = DeviceDataState.EquipmentDown;
            }
            return dataState;
        }

        /// <summary>
        /// 判断上限预警
        /// </summary>
        /// <param name="dataState"></param>
        /// <param name="runState"></param>
        /// <param name="isHaveHighAlarm"></param>
        private bool JudgeDataHighPreAlarm(DeviceDataState dataState, DeviceRunState runState, bool isHaveHighAlarm)
        {
            bool isWriteRunLog = false;
            try
            {
                DateTime time = CreatedTime;
                decimal value = 0, DefAlarmValue = 0;
                if (decimal.TryParse(PointDefineInfo.Ssz, out value))
                {
                    if (decimal.TryParse(PointDefineInfo.Z1.ToString(), out DefAlarmValue) && isHaveHighAlarm && value >= DefAlarmValue && DefAlarmValue != 0)
                    {
                        //if ((PointDefineInfo.Alarm & 0x01) != 0x01)
                        if (!AlarmBusiness.JudgeAlarmIsAlarmState(PointDefineInfo.Alarm, DeviceDataState.DataHighPreAlarm))
                        {
                            //上限预警
                            //PointDefineInfo.Alarm |= 0x01;
                            PointDefineInfo.Alarm |= AlarmBusiness.dataHighPreAlarmInt;
                            SafetyHelper.CreateRunLogInfo(PointDefineInfo, time, (short)DeviceDataState.DataHighPreAlarm, (short)runState, PointDefineInfo.Ssz);
                            isWriteRunLog = true;
                        }
                    }
                    else
                    {
                        //if ((PointDefineInfo.Alarm & 0x01) == 0x01)
                        if (AlarmBusiness.JudgeAlarmIsAlarmState(PointDefineInfo.Alarm, DeviceDataState.DataHighPreAlarm))
                        {
                            //上限预警解除
                            //PointDefineInfo.Alarm &= 0xFE;
                            PointDefineInfo.Alarm &= (short)(0xFF - AlarmBusiness.dataHighPreAlarmInt);
                            SafetyHelper.CreateRunLogInfo(PointDefineInfo, time, (short)DeviceDataState.DataHighPreAlarmRemove, (short)runState, PointDefineInfo.Ssz);
                            isWriteRunLog = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return isWriteRunLog;
        }

        /// <summary>
        /// 判断上限报警
        /// </summary>
        /// <param name="dataState"></param>
        /// <param name="runState"></param>
        /// <param name="isHaveHighAlarm"></param>
        private bool JudgeDataHighAlarm(DeviceDataState dataState, DeviceRunState runState, bool isHaveHighAlarm)
        {
            bool isWriteRunLog = false;
            DateTime time = CreatedTime;
            decimal value = 0, DefAlarmValue = 0;
            if (decimal.TryParse(PointDefineInfo.Ssz, out value))
            {
                if (decimal.TryParse(PointDefineInfo.Z2.ToString(), out DefAlarmValue) && isHaveHighAlarm && value >= DefAlarmValue && DefAlarmValue != 0)
                {
                    //if ((PointDefineInfo.Alarm & 0x02) != 0x02)
                    if (!AlarmBusiness.JudgeAlarmIsAlarmState(PointDefineInfo.Alarm, DeviceDataState.DataHighAlarm))
                    {
                        #region 需要先判断是否存在下限报警（已删除）
                        //if ((PointDefineInfo.Alarm & 0x20) == 0x20)
                        //{
                        //    //下限报警解除
                        //    PointDefineInfo.Alarm &= 0xDF;
                        //    //并更新报警结束时间
                        //    if (PointDefineInfo.ClsAlarmObj.NAlarmAllCount <= 0)
                        //    {
                        //        PointDefineInfo.ClsAlarmObj.NAlarmAllCount = 1;
                        //    }
                        //    // 下限报警解除记录
                        //    KJ73NHelper.CreateRunLogInfo(PointDefineInfo, time, (short)DeviceDataState.DataLowAlarmRemove, (short)runState, PointDefineInfo.Ssz);
                        //    isWriteRunLog = true;
                        //    // 下限报警结束记录
                        //    KJ73NHelper.EndAlarmInfo(PointDefineInfo.Point,
                        //        PointDefineInfo.ClsAlarmObj.DttAlarmTime,
                        //        (short)DeviceDataState.DataLowAlarm,
                        //        Convert.ToSingle(PointDefineInfo.ClsAlarmObj.NAlarmMaxVal),
                        //        PointDefineInfo.ClsAlarmObj.DttAlarmMaxValTime,
                        //        time,
                        //        Convert.ToSingle(Math.Round(PointDefineInfo.ClsAlarmObj.NAlarmAllVal / PointDefineInfo.ClsAlarmObj.NAlarmAllCount, 2)));
                        //    // 清除下限报警临时缓存记录 2017.6.25 by
                        //    KJ73NHelper.DdClearByArrPoint(PointDefineInfo, DeviceDataState.DataLowAlarm);

                        //    PointDefineInfo.ClsAlarmObj.NAlarmAllCount = 0;

                        //    // 控制解除 复电
                        //    KJ73NHelper.GetAnalogControlPort(PointDefineInfo, DeviceDataState.DataLowAlarm, PointDefineInfo.ClsAlarmObj.DttAlarmTime, PointDefineInfo.Point, 2);
                        //}
                        #endregion

                        //生成上限报警记录
                        //PointDefineInfo.Alarm |= 0x02;
                        PointDefineInfo.Alarm |= AlarmBusiness.dataHighAlarmInt;
                        PointDefineInfo.ClsAlarmObj.NAlarmMaxVal = value;
                        PointDefineInfo.ClsAlarmObj.NAlarmAllVal = value;
                        PointDefineInfo.ClsAlarmObj.NAlarmMinVal = value;
                        PointDefineInfo.ClsAlarmObj.NAlarmAllCount = 1;
                        PointDefineInfo.ClsAlarmObj.DttAlarmMinValTime = time;
                        PointDefineInfo.ClsAlarmObj.DttAlarmMaxValTime = time;
                        PointDefineInfo.ClsAlarmObj.DttHighAlarmTime = time;
                        // 写报警运行记录
                        SafetyHelper.CreateRunLogInfo(PointDefineInfo, time, (short)DeviceDataState.DataHighAlarm, (short)runState, PointDefineInfo.Ssz);
                        isWriteRunLog = true;
                        // 写报警记录
                        Jc_BInfo alarm = AlarmBusiness.CreateAlarmInfo(PointDefineInfo, time, DeviceDataState.DataHighAlarm, runState, 1);
                        AlarmBusiness.UpdateAlarmInfo(alarm, 1, false);
                    }
                }
                else
                {
                    #region 解除报警
                    //if ((PointDefineInfo.Alarm & 0x02) == 0x02)
                    if (AlarmBusiness.JudgeAlarmIsAlarmState(PointDefineInfo.Alarm, DeviceDataState.DataHighAlarm))
                    {
                        //上限报警解除
                        //PointDefineInfo.Alarm &= 0xFD;
                        PointDefineInfo.Alarm &= (short)(0xFF - AlarmBusiness.dataHighAlarmInt);
                        // 写报警解除运行记录
                        SafetyHelper.CreateRunLogInfo(PointDefineInfo, time, (short)DeviceDataState.DataHighAlarmRemove, (short)runState, PointDefineInfo.Ssz);
                        isWriteRunLog = true;
                        //并更新报警结束时间
                        if (PointDefineInfo.ClsAlarmObj.NAlarmAllCount <= 0)
                        {
                            PointDefineInfo.ClsAlarmObj.NAlarmAllCount = 1;
                        }
                        //写报警结束记录
                        AlarmBusiness.EndAlarmInfo(PointDefineInfo.Point,
                            PointDefineInfo.ClsAlarmObj.DttHighAlarmTime,
                            (short)DeviceDataState.DataHighAlarm,
                            Convert.ToSingle(PointDefineInfo.ClsAlarmObj.NAlarmMaxVal),
                            PointDefineInfo.ClsAlarmObj.DttAlarmMaxValTime,
                            time,
                            Convert.ToSingle(Math.Round(PointDefineInfo.ClsAlarmObj.NAlarmAllVal / SafetyHelper.GetCumulativeCount(PointDefineInfo.ClsAlarmObj.NAlarmAllCount), 2)),
                            Convert.ToSingle(PointDefineInfo.ClsAlarmObj.NAlarmMinVal),
                            PointDefineInfo.ClsAlarmObj.DttAlarmMinValTime
                            );

                        PointDefineInfo.ClsAlarmObj.NAlarmAllCount = 0;
                    }
                    #endregion
                }
            }
            return isWriteRunLog;
        }

        /// <summary>
        /// 判断上限断电
        /// </summary>
        /// <param name="dataState"></param>
        /// <param name="runState"></param>
        /// <param name="isHaveHighAlarm"></param>
        private bool JudgeDataHighAlarmPowerOFF(DeviceDataState dataState, DeviceRunState runState, bool isHaveHighAlarm)
        {
            bool isWriteRunLog = false;
            try
            {
                bool Flag = false;
                DateTime time = CreatedTime;
                decimal value = 0, DefAlarmValue = 0;
                List<Jc_JcsdkzInfo> jckzItems;
                List<Jc_JcsdkzInfo> oldJckzItems;
                List<Jc_JcsdkzInfo> updateJckzItems;
                if (decimal.TryParse(PointDefineInfo.Ssz, out value))
                {
                    if (decimal.TryParse(PointDefineInfo.Z3.ToString(), out DefAlarmValue) && isHaveHighAlarm && DefAlarmValue != 0)
                    {
                        if (value >= DefAlarmValue)
                        {
                            #region 需要先判断是否存在下限断电 （已删除）
                            //if ((PointDefineInfo.Alarm & 0x40) == 0x40)
                            //{
                            //    //下限复电
                            //    PointDefineInfo.Alarm &= 0xBF;
                            //    //并更新断电结束时间
                            //    if (PointDefineInfo.ClsAlarmObj.NPowerOffAllCount == 0)
                            //    {
                            //        PointDefineInfo.ClsAlarmObj.NPowerOffAllCount = 1;
                            //    }
                            //    // 下限断电解除运行记录
                            //    KJ73NHelper.CreateRunLogInfo(PointDefineInfo, time, (short)DeviceDataState.DataLowPowerRemove, (short)runState, PointDefineInfo.Ssz);
                            //    isWriteRunLog = true;
                            //    // 下限断电结束记录
                            //    KJ73NHelper.EndAlarmInfo(PointDefineInfo.Point,
                            //        PointDefineInfo.ClsAlarmObj.DttPowerOffTime,
                            //        (short)DeviceDataState.DataLowPower,
                            //        Convert.ToSingle(PointDefineInfo.ClsAlarmObj.NPowerOffMaxVal),
                            //        PointDefineInfo.ClsAlarmObj.DttPowerOffMaxValTime,
                            //        time,
                            //        Convert.ToSingle(Math.Round(PointDefineInfo.ClsAlarmObj.NPowerOffAllVal / PointDefineInfo.ClsAlarmObj.NPowerOffAllCount, 2)));
                            //    // 清除下限报警临时缓存记录 2017.6.25 by
                            //    KJ73NHelper.DdClearByArrPoint(PointDefineInfo, DeviceDataState.DataLowPower);

                            //    PointDefineInfo.ClsAlarmObj.NPowerOffAllCount = 0;
                            //    // 控制解除 复电
                            //    KJ73NHelper.GetAnalogControlPort(PointDefineInfo, DeviceDataState.DataLowPower, PointDefineInfo.ClsAlarmObj.DttPowerOffTime, PointDefineInfo.Point, 2);

                            //    // 处理交叉控制复电
                            //    KJ73NHelper.DeleteManualCrossControl(PointDefineInfo.Point, ControlType.ControlPowerOff);
                            //}
                            #endregion

                            #region 处理断电信息
                            //if ((PointDefineInfo.Alarm & 0x04) != 0x04)
                            if (!AlarmBusiness.JudgeAlarmIsAlarmState(PointDefineInfo.Alarm, DeviceDataState.DataHighAlarmPowerOFF))
                            {
                                //上限断电
                                //PointDefineInfo.Alarm |= 0x04;
                                PointDefineInfo.Alarm |= AlarmBusiness.dataHighAlarmPowerOFFInt;
                                PointDefineInfo.ClsAlarmObj.NPowerOffAllVal = value;
                                PointDefineInfo.ClsAlarmObj.NPowerOffMaxVal = value;
                                PointDefineInfo.ClsAlarmObj.NPowerOffMinVal = value;
                                PointDefineInfo.ClsAlarmObj.NPowerOffAllCount = 1;
                                PointDefineInfo.ClsAlarmObj.DttPowerOffMaxValTime = time;
                                PointDefineInfo.ClsAlarmObj.DttPowerOffMinValTime = time;
                                PointDefineInfo.ClsAlarmObj.DttHighPowerOffTime = time;
                                // 写断电运行记录
                                SafetyHelper.CreateRunLogInfo(PointDefineInfo, time, (short)DeviceDataState.DataHighAlarmPowerOFF, (short)runState, PointDefineInfo.Ssz);
                                isWriteRunLog = true;
                                // 写断电开始记录
                                Jc_BInfo alarm = AlarmBusiness.CreateAlarmInfo(PointDefineInfo, time, DeviceDataState.DataHighAlarmPowerOFF, runState, 1);
                                AlarmBusiness.UpdateAlarmInfo(alarm, 1, false);
                                // 添加交叉控制断电
                                updateJckzItems = new List<Jc_JcsdkzInfo>();
                                //删除本控制口已有的断电控制
                                oldJckzItems = ControlBus.GetManualCrossControlFromCache(PointDefineInfo.Point, ControlType.ControlPowerOff);
                                if (oldJckzItems.Count > 0)
                                {
                                    oldJckzItems.ForEach(a =>
                                    {
                                        a.InfoState = Basic.Framework.Web.InfoState.Delete;
                                    });
                                    updateJckzItems.AddRange(oldJckzItems);
                                }
                                //生成本控制口新增的断电控制
                                jckzItems = ControlBus.GetAddManualCrossControlFromDefine(PointDefineInfo, ControlType.ControlPowerOff);
                                if (jckzItems.Count > 0)
                                {
                                    jckzItems.ForEach(a =>
                                    {
                                        a.InfoState = Basic.Framework.Web.InfoState.AddNew;
                                    });
                                    updateJckzItems.AddRange(jckzItems);
                                }

                                if (updateJckzItems.Count > 0)
                                {
                                    ControlBus.OperationManualCrossControl(updateJckzItems);
                                }
                            }
                            #endregion
                        }
                        if (decimal.TryParse(PointDefineInfo.Z4.ToString(), out DefAlarmValue))
                        {
                            if (isHaveHighAlarm)
                            {
                                if (value < DefAlarmValue)
                                {
                                    Flag = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        Flag = true;
                    }

                    if (Flag)
                    {
                        //if ((PointDefineInfo.Alarm & 0x04) == 0x04)
                        if (AlarmBusiness.JudgeAlarmIsAlarmState(PointDefineInfo.Alarm, DeviceDataState.DataHighAlarmPowerOFF))
                        {
                            //上限复电
                            //PointDefineInfo.Alarm &= 0xFB;
                            PointDefineInfo.Alarm &= (short)(0xFF - AlarmBusiness.dataHighAlarmPowerOFFInt);
                            //并更新断电结束时间
                            if (PointDefineInfo.ClsAlarmObj.NPowerOffAllCount <= 0)
                            {
                                PointDefineInfo.ClsAlarmObj.NPowerOffAllCount = 1;
                            }
                            // 断电解除运行记录
                            SafetyHelper.CreateRunLogInfo(PointDefineInfo, time, (short)DeviceDataState.DataHighAlarmPowerOFFRemove, (short)runState, PointDefineInfo.Ssz);
                            isWriteRunLog = true;
                            // 断电结束记录
                            AlarmBusiness.EndAlarmInfo(PointDefineInfo.Point,
                                PointDefineInfo.ClsAlarmObj.DttHighPowerOffTime,
                                (short)DeviceDataState.DataHighAlarmPowerOFF,
                                Convert.ToSingle(PointDefineInfo.ClsAlarmObj.NPowerOffMaxVal),
                                PointDefineInfo.ClsAlarmObj.DttPowerOffMaxValTime,
                                time,
                                Convert.ToSingle(Math.Round(PointDefineInfo.ClsAlarmObj.NPowerOffAllVal / SafetyHelper.GetCumulativeCount(PointDefineInfo.ClsAlarmObj.NPowerOffAllCount), 2)),
                                Convert.ToSingle(PointDefineInfo.ClsAlarmObj.NPowerOffMinVal),
                                PointDefineInfo.ClsAlarmObj.DttPowerOffMinValTime
                                );

                            PointDefineInfo.ClsAlarmObj.NPowerOffAllCount = 0;

                            //交叉控制解除
                            jckzItems = ControlBus.GetManualCrossControlFromCache(PointDefineInfo.Point, ControlType.ControlPowerOff);
                            if (jckzItems.Count > 0)
                            {
                                jckzItems.ForEach(a =>
                                {
                                    a.InfoState = Basic.Framework.Web.InfoState.Delete;
                                });
                                ControlBus.OperationManualCrossControl(jckzItems);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("JudgeDataHighAlarmPowerOFF Error:" + ex.Message);
            }

            return isWriteRunLog;
        }
        /// <summary>
        /// 判断下限预警
        /// </summary>
        /// <param name="dataState"></param>
        /// <param name="runState"></param>
        /// <param name="isHaveLowAlarm"></param>
        private bool JudgeDataLowPreAlarm(DeviceDataState dataState, DeviceRunState runState, bool isHaveLowAlarm)
        {
            bool isWriteRunLog = false;
            DateTime time = CreatedTime;
            decimal value = 0, DefAlarmValue = 0;
            if (decimal.TryParse(PointDefineInfo.Ssz, out value))
            {
                if (decimal.TryParse(PointDefineInfo.Z5.ToString(), out DefAlarmValue) && isHaveLowAlarm && value <= DefAlarmValue && DefAlarmValue != 0)
                {
                    //if ((PointDefineInfo.Alarm & 0x10) != 0x10)
                    if (!AlarmBusiness.JudgeAlarmIsAlarmState(PointDefineInfo.Alarm, DeviceDataState.DataLowPreAlarm))
                    {
                        //下限预警
                        //PointDefineInfo.Alarm |= 0x10;
                        PointDefineInfo.Alarm |= AlarmBusiness.dataLowPreAlarmInt;
                        //下限预警记录
                        SafetyHelper.CreateRunLogInfo(PointDefineInfo, time, (short)DeviceDataState.DataLowPreAlarm, (short)runState, PointDefineInfo.Ssz);
                        isWriteRunLog = true;
                    }
                }
                else
                {
                    //if ((PointDefineInfo.Alarm & 0x10) == 0x10)
                    if (AlarmBusiness.JudgeAlarmIsAlarmState(PointDefineInfo.Alarm, DeviceDataState.DataLowPreAlarm))
                    {
                        //上限预警解除
                        //PointDefineInfo.Alarm &= 0xEF;
                        PointDefineInfo.Alarm &= (short)(0xFF - AlarmBusiness.dataLowPreAlarmInt);
                        // 下限预警解除记录
                        SafetyHelper.CreateRunLogInfo(PointDefineInfo, time, (short)DeviceDataState.DataLowPreAlarmRemove, (short)runState, PointDefineInfo.Ssz);
                        isWriteRunLog = true;
                    }
                }
            }
            return isWriteRunLog;
        }

        /// <summary>
        /// 判断下限报警
        /// </summary>
        /// <param name="dataState"></param>
        /// <param name="runState"></param>
        /// <param name="isHaveLowAlarm"></param>
        private bool JudgeDataLowAlarm(DeviceDataState dataState, DeviceRunState runState, bool isHaveLowAlarm)
        {
            bool isWriteRunLog = false;
            bool Flag = false;
            DateTime time = CreatedTime;    //从基类中取得
            decimal value = 0, DefAlarmValue = 0;
            if (decimal.TryParse(PointDefineInfo.Ssz, out value))
            {
                if (decimal.TryParse(PointDefineInfo.Z6.ToString(), out DefAlarmValue) && isHaveLowAlarm && value <= DefAlarmValue && DefAlarmValue != 0)
                {

                    //if ((PointDefineInfo.Alarm & 0x20) != 0x20)
                    if (!AlarmBusiness.JudgeAlarmIsAlarmState(PointDefineInfo.Alarm, DeviceDataState.DataLowAlarm))
                    {
                        // 下限报警
                        //PointDefineInfo.Alarm |= 0x20;
                        PointDefineInfo.Alarm |= AlarmBusiness.dataLowAlarmInt;
                        PointDefineInfo.ClsAlarmObj.NAlarmMaxVal = value;
                        PointDefineInfo.ClsAlarmObj.NAlarmAllVal = value;
                        PointDefineInfo.ClsAlarmObj.NAlarmMinVal = value;
                        PointDefineInfo.ClsAlarmObj.NAlarmAllCount = 1;
                        PointDefineInfo.ClsAlarmObj.DttAlarmMinValTime = time;
                        PointDefineInfo.ClsAlarmObj.DttAlarmMaxValTime = time;
                        PointDefineInfo.ClsAlarmObj.DttLowAlarmTime = time;
                        //生成下限报警记录
                        Jc_BInfo alam = AlarmBusiness.CreateAlarmInfo(PointDefineInfo, time, DeviceDataState.DataLowAlarm, runState, 1);
                        AlarmBusiness.UpdateAlarmInfo(alam, 1, false);
                        // 下限报警运行记录
                        SafetyHelper.CreateRunLogInfo(PointDefineInfo, time, (short)DeviceDataState.DataLowAlarm, (short)runState, PointDefineInfo.Ssz);
                        isWriteRunLog = true;
                    }
                    else
                    {
                        dataState = DeviceDataState.DataLowAlarm;
                    }

                }
                else
                {
                    //if ((PointDefineInfo.Alarm & 0x20) == 0x20)
                    if (AlarmBusiness.JudgeAlarmIsAlarmState(PointDefineInfo.Alarm, DeviceDataState.DataLowAlarm))
                    {
                        //下限报警解除;
                        //PointDefineInfo.Alarm &= 0xDF;
                        PointDefineInfo.Alarm &= (short)(0xFF - AlarmBusiness.dataLowAlarmInt);
                        if (PointDefineInfo.ClsAlarmObj.NAlarmAllCount <= 0)
                        {
                            PointDefineInfo.ClsAlarmObj.NAlarmAllCount = 1;
                        }
                        // 下限报警解除记录
                        SafetyHelper.CreateRunLogInfo(PointDefineInfo, time, (short)DeviceDataState.DataLowAlarmRemove, (short)runState, PointDefineInfo.Ssz);
                        isWriteRunLog = true;
                        //下限报警结束记录
                        AlarmBusiness.EndAlarmInfo(PointDefineInfo.Point,
                            PointDefineInfo.ClsAlarmObj.DttLowAlarmTime,
                            (short)DeviceDataState.DataLowAlarm,
                            Convert.ToSingle(PointDefineInfo.ClsAlarmObj.NAlarmMaxVal),
                            PointDefineInfo.ClsAlarmObj.DttAlarmMaxValTime,
                            time,
                            Convert.ToSingle(Math.Round(PointDefineInfo.ClsAlarmObj.NAlarmAllVal / SafetyHelper.GetCumulativeCount(PointDefineInfo.ClsAlarmObj.NAlarmAllCount), 2)),
                            Convert.ToSingle(PointDefineInfo.ClsAlarmObj.NAlarmMinVal),
                            PointDefineInfo.ClsAlarmObj.DttAlarmMinValTime
                            );

                        PointDefineInfo.ClsAlarmObj.NAlarmAllCount = 0;
                    }
                }
            }
            return isWriteRunLog;
        }

        /// <summary>
        /// 判断下限断电
        /// </summary>
        /// <param name="dataState"></param>
        /// <param name="runState"></param>
        /// <param name="isHaveLowAlarm"></param>
        private bool JudgeDataLowPower(DeviceDataState dataState, DeviceRunState runState, bool isHaveLowAlarm)
        {
            bool isWriteRunLog = false;
            bool Flag = false;
            DateTime time = CreatedTime;
            decimal value = 0, DefAlarmValue = 0;
            List<Jc_JcsdkzInfo> jckzItems;
            List<Jc_JcsdkzInfo> oldJckzItems;
            List<Jc_JcsdkzInfo> updateJckzItems;
            if (decimal.TryParse(PointDefineInfo.Ssz, out value))
            {
                if (decimal.TryParse(PointDefineInfo.Z7.ToString(), out DefAlarmValue) && isHaveLowAlarm && DefAlarmValue != 0)
                {
                    if (value <= DefAlarmValue)
                    {
                        //if ((PointDefineInfo.Alarm & 0x40) != 0x40)
                        if (!AlarmBusiness.JudgeAlarmIsAlarmState(PointDefineInfo.Alarm, DeviceDataState.DataLowPower))
                        {
                            // 下限断电
                            //PointDefineInfo.Alarm |= 0x40;
                            PointDefineInfo.Alarm |= AlarmBusiness.dataLowPowerInt;
                            PointDefineInfo.ClsAlarmObj.NPowerOffAllVal = value;
                            PointDefineInfo.ClsAlarmObj.NPowerOffMaxVal = value;
                            PointDefineInfo.ClsAlarmObj.NPowerOffMinVal = value;
                            PointDefineInfo.ClsAlarmObj.NPowerOffAllCount = 1;
                            PointDefineInfo.ClsAlarmObj.DttPowerOffMaxValTime = time;
                            PointDefineInfo.ClsAlarmObj.DttPowerOffMinValTime = time;
                            PointDefineInfo.ClsAlarmObj.DttLowPowerOffTime = time;
                            Jc_BInfo alarm = AlarmBusiness.CreateAlarmInfo(PointDefineInfo, time, DeviceDataState.DataLowPower, runState, 1);

                            PointDefineInfo.ClsAlarmObj.DttAlarmID = Convert.ToInt64(alarm.ID);
                            AlarmBusiness.UpdateAlarmInfo(alarm, 1, false);
                            // 下限断电记录
                            SafetyHelper.CreateRunLogInfo(PointDefineInfo, time, (short)DeviceDataState.DataLowPower, (short)runState, PointDefineInfo.Ssz);
                            isWriteRunLog = true;
                            //下限交叉控制
                            updateJckzItems = new List<Jc_JcsdkzInfo>();
                            //删除已有的断电记录
                            oldJckzItems = ControlBus.GetManualCrossControlFromCache(PointDefineInfo.Point, ControlType.ControlPowerOff);
                            if (oldJckzItems.Count > 0)
                            {
                                oldJckzItems.ForEach(a =>
                                {
                                    a.InfoState = Basic.Framework.Web.InfoState.Delete;
                                });
                                updateJckzItems.AddRange(oldJckzItems);
                            }
                            //生成新增的断电记录
                            jckzItems = ControlBus.GetAddManualCrossControlFromDefine(PointDefineInfo, ControlType.ControlPowerOff);
                            if (jckzItems.Count > 0)
                            {
                                jckzItems.ForEach(a =>
                                {
                                    a.InfoState = Basic.Framework.Web.InfoState.AddNew;
                                });
                                updateJckzItems.AddRange(jckzItems);
                            }
                            if (updateJckzItems.Count > 0)
                            {
                                ControlBus.OperationManualCrossControl(jckzItems);
                            }
                        }
                    }
                    else if (decimal.TryParse(PointDefineInfo.Z8.ToString(), out DefAlarmValue))
                    {
                        if (isHaveLowAlarm)
                        {
                            if (value > DefAlarmValue)
                            {
                                Flag = true;
                            }
                        }
                    }
                }
                else
                {
                    Flag = true;
                }

                if (Flag)
                {
                    //if ((PointDefineInfo.Alarm & 0x40) == 0x40)
                    if (AlarmBusiness.JudgeAlarmIsAlarmState(PointDefineInfo.Alarm, DeviceDataState.DataLowPower))
                    {
                        //下限复电
                        //PointDefineInfo.Alarm &= 0xBF;
                        PointDefineInfo.Alarm &= (short)(0xFF - AlarmBusiness.dataLowPowerInt);
                        //并更新断电结束时间
                        if (PointDefineInfo.ClsAlarmObj.NPowerOffAllCount <= 0)
                        {
                            PointDefineInfo.ClsAlarmObj.NPowerOffAllCount = 1;
                        }
                        // 下限断电解除运行记录
                        SafetyHelper.CreateRunLogInfo(PointDefineInfo, time, (short)DeviceDataState.DataLowPowerRemove, (short)runState, PointDefineInfo.Ssz);
                        isWriteRunLog = true;
                        // 下限断电结束记录
                        AlarmBusiness.EndAlarmInfo(PointDefineInfo.Point,
                            PointDefineInfo.ClsAlarmObj.DttLowPowerOffTime,
                            (short)DeviceDataState.DataLowPower,
                            Convert.ToSingle(PointDefineInfo.ClsAlarmObj.NPowerOffMaxVal),
                            PointDefineInfo.ClsAlarmObj.DttPowerOffMaxValTime,
                            time,
                            Convert.ToSingle(Math.Round(PointDefineInfo.ClsAlarmObj.NPowerOffAllVal / SafetyHelper.GetCumulativeCount(PointDefineInfo.ClsAlarmObj.NPowerOffAllCount), 2)),
                            Convert.ToSingle(PointDefineInfo.ClsAlarmObj.NPowerOffMinVal),
                            PointDefineInfo.ClsAlarmObj.DttPowerOffMinValTime
                            );

                        PointDefineInfo.ClsAlarmObj.NPowerOffAllCount = 0;

                        //交叉控制解除
                        jckzItems = ControlBus.GetManualCrossControlFromCache(PointDefineInfo.Point, ControlType.ControlPowerOff);
                        if (jckzItems.Count > 0)
                        {
                            jckzItems.ForEach(a =>
                            {
                                a.InfoState = Basic.Framework.Web.InfoState.Delete;
                            });
                            ControlBus.OperationManualCrossControl(jckzItems);
                        }
                    }
                }
            }
            return isWriteRunLog;
        }

        /// <summary>
        /// 模拟量控制口变化操作
        /// </summary>
        /// <param name="pointDefineItem"></param>
        private void AnalogControlChange(Jc_DefInfo pointDefineItem)
        {
            Jc_BInfo alarmInfo;
            if (SafetyHelper.isAbnormalDataState((DeviceDataState)pointDefineItem.DataState))
            {
                // 处理交叉控制
                ControlType controlType = SafetyHelper.GetControlTypeByAbnormalState((DeviceRunState)pointDefineItem.State);
                if (controlType != ControlType.NoControl)
                {
                    ControlBus.DoControlChange(pointDefineItem, controlType);
                }
                // 修改报警记录的控制口
                alarmInfo = AlarmBusiness.CreateControlChangeAlarmInfo(pointDefineItem, (DeviceDataState)pointDefineItem.DataState, pointDefineItem.DttRunStateTime);
                AlarmBusiness.UpdateAlarmInfo(alarmInfo, 6, false);
            }
            else if (pointDefineItem.Alarm > 0)
            {
                //if ((pointDefineItem.Alarm & 0x02) == 0x02)
                if (AlarmBusiness.JudgeAlarmIsAlarmState(pointDefineItem.Alarm, DeviceDataState.DataHighAlarm))
                {
                    // 修改控制口
                    alarmInfo = AlarmBusiness.CreateControlChangeAlarmInfo(pointDefineItem, DeviceDataState.DataHighAlarm, pointDefineItem.ClsAlarmObj.DttHighAlarmTime);
                    AlarmBusiness.UpdateAlarmInfo(alarmInfo, 6, false);
                }
                //if ((pointDefineItem.Alarm & 0x04) == 0x04)
                if (AlarmBusiness.JudgeAlarmIsAlarmState(pointDefineItem.Alarm, DeviceDataState.DataHighAlarmPowerOFF))
                {
                    //处理交叉控制
                    ControlBus.DoControlChange(pointDefineItem, ControlType.ControlPowerOff);
                    // 修改控制口
                    alarmInfo = AlarmBusiness.CreateControlChangeAlarmInfo(pointDefineItem, DeviceDataState.DataHighAlarmPowerOFF, pointDefineItem.ClsAlarmObj.DttHighPowerOffTime);
                    AlarmBusiness.UpdateAlarmInfo(alarmInfo, 6, false);
                }
                //if ((pointDefineItem.Alarm & 0x20) == 0x20)
                if (AlarmBusiness.JudgeAlarmIsAlarmState(pointDefineItem.Alarm, DeviceDataState.DataLowAlarm))
                {
                    // 修改控制口
                    alarmInfo = AlarmBusiness.CreateControlChangeAlarmInfo(pointDefineItem, DeviceDataState.DataLowAlarm, pointDefineItem.ClsAlarmObj.DttHighAlarmTime);
                    AlarmBusiness.UpdateAlarmInfo(alarmInfo, 6, false);
                }
                //if ((pointDefineItem.Alarm & 0x40) == 0x40)
                if (AlarmBusiness.JudgeAlarmIsAlarmState(pointDefineItem.Alarm, DeviceDataState.DataLowPower))
                {
                    //处理交叉控制
                    ControlBus.DoControlChange(pointDefineItem, ControlType.ControlPowerOff);
                    // 修改控制口
                    alarmInfo = AlarmBusiness.CreateControlChangeAlarmInfo(pointDefineItem, DeviceDataState.DataLowPower, pointDefineItem.ClsAlarmObj.DttHighPowerOffTime);
                    AlarmBusiness.UpdateAlarmInfo(alarmInfo, 6, false);
                }
            }
        }
        #endregion

        /// <summary>
        /// 是否是重要传感器
        /// </summary>
        /// <param name="devName"></param>
        /// <returns></returns>
        private bool IsImpDevice(Jc_DefInfo def)
        {
            bool flag = false;

            //if (devName.Contains("甲烷")) { flag = true; }
            //else if (devName.Contains("瓦斯")) { flag = true; }
            //else if (devName.Contains("一氧化碳")) { flag = true; }
            //else if (devName.Contains("二氧化碳")) { flag = true; }
            //else if (devName.Contains("低沼")) { flag = true; }

            if (def.DevClassID == 32
                || def.DevClassID == 64
                || def.DevClassID == 128

                )//甲烷、CO、粉尘、流量 2017.12.19 by
            {
                flag = true;
            }

            return flag;
        }

        /// <summary>
        /// 是否满足变值存储条件
        /// </summary>
        /// <returns></returns>
        protected bool ChangeRateStore(ref decimal value)
        {
            bool flag = true;
            try
            {
                if (DevItems != null)
                {
                    Jc_DevInfo item = DevItems.Find(a => a.Devid == PointDefineInfo.Devid);
                    if (item != null)
                    {
                        //20190119--todo,加入低于量程判断
                        if (value > item.LC) value = item.LC;
                        decimal ChangeRate = 0.02m;
                        if (item.Name.Contains("风速"))
                        {
                            ChangeRate = item.LC2 * 0.03M;
                        }
                        else
                        {
                            if (item.LC2 > 0) //如果有中间量程
                            {
                                if (value < item.LC2)
                                {
                                    ChangeRate = item.LC2 * 0.01M;
                                }
                                else
                                {
                                    ChangeRate = item.LC * 0.01M;
                                }
                            }
                            else
                            {
                                ChangeRate = item.LC * 0.01M;
                            }

                            if (value > PointDefineInfo.ClsFiveMinObj.m_nMaxVal || value < PointDefineInfo.ClsFiveMinObj.m_nMinVal || Math.Abs(value - Convert.ToDecimal(PointDefineInfo.Ssz)) >= ChangeRate)
                            {
                                flag = true;
                            }
                            else
                            {
                                flag = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info("ChangeRateStore:" + ex.Message);
            }

            return flag;
        }



    }
}
