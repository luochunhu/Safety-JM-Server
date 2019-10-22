using Sys.DataCollection.Common.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract.Cache;
using Basic.Framework.Service;
using Sys.Safety.Request.Cache;
using Sys.Safety.Enums;
using Basic.Framework.Common;

using Sys.Safety.Model;
using Basic.Framework.Web;
using Basic.Framework.Logging;

namespace Sys.Safety.Driver.RealDataHandle
{
    /// <summary>
    /// 作者：
    /// 时间：2017-05-31
    /// 描述：开关量实时数据处理
    /// 修改记录
    /// 2017-05-31
    /// </summary>
    public class DerailHandle : PointHandle
    {
        private readonly IAlarmCacheService alarmCacheService;

        public DerailHandle()
        {
            alarmCacheService = ServiceFactory.Create<IAlarmCacheService>();
        }

        protected override Dictionary<string, object> DataHandle()
        {
            Dictionary<string, object> updateItems = new Dictionary<string, object>();
            //DeviceDefineCacheGetByKeyRequest request = new DeviceDefineCacheGetByKeyRequest
            //{
            //    Devid = PointDefineInfo.Devid
            //};
            //var response = DeviceDefineCacheService.GetPointDefineCacheByKey(request);

            //if (response != null && response.IsSuccess && response.Data != null)
            //{
            //Jc_DevInfo deviceDefine = response.Data;
            DeviceDataState datastate;

            //开关量实时值(0态、1态、2态)对应的显示值从设备定义缓存获取
            if (RealDataItem.RealData == "1")
            {
                //PointDefineInfo.Ssz = deviceDefine.Xs2;
                datastate = DeviceDataState.DataDerailState1;
            }
            else if (RealDataItem.RealData == "2")
            {
                //PointDefineInfo.Ssz = deviceDefine.Xs3;
                datastate = DeviceDataState.DataDerailState2;
            }
            else
            {
                //PointDefineInfo.Ssz = deviceDefine.Xs1;
                datastate = DeviceDataState.DataDerailState0;
            }

            if ((int)RealDataItem.State == (int)DeviceRunState.EquipmentCommOK ||
                (int)RealDataItem.State == (int)DeviceRunState.EquipmentInfrareding ||//红外遥控
                (int)RealDataItem.State == (int)DeviceRunState.EquipmentStart ||//开机
               (int)RealDataItem.State == (int)DeviceRunState.EquipmentAdjusting)//标校
            {
                //if (PointDefineInfo.State != (short)RealDataItem.State || PointDefineInfo.DataState != (short)datastate || PointDefineInfo.ReDoDeal == 2) 开关量每次都要进行数据处理 2017.7.12 by
                {
                    PointRunRecord(datastate, (DeviceRunState)((int)RealDataItem.State));
                    PointDefineInfo.ReDoDeal = 0;
                    PointDefineInfo.DttStateTime = DateTime.Now;
                    updateItems.Add("Alarm", PointDefineInfo.Alarm);
                    updateItems.Add("ReDoDeal", PointDefineInfo.ReDoDeal);
                    updateItems.Add("DataState", PointDefineInfo.DataState);
                    updateItems.Add("State", PointDefineInfo.State);
                    //updateItems.Add("DttStateTime", PointDefineInfo.DttStateTime);
                    updateItems.Add("DttStateTime", CreatedTime);
                    updateItems.Add("Zts", PointDefineInfo.Zts);
                    updateItems.Add("DttRunStateTime", PointDefineInfo.DttRunStateTime);
                    updateItems.Add("Ssz", PointDefineInfo.Ssz);
                    updateItems.Add("Voltage", RealDataItem.Voltage);//增加电压赋值
                }
            }
            else
            {
                //误码和传感器自己传输的断线都当做是0态
                //if (PointDefineInfo.DataState != (short)DeviceDataState.DataDerailState0 || PointDefineInfo.ReDoDeal == 2) 开关量每次都要进行数据处理 2017.7.12 by
                {
                    PointRunRecord(DeviceDataState.DataDerailState0, (DeviceRunState)((int)RealDataItem.State));
                    PointDefineInfo.ReDoDeal = 0;
                    PointDefineInfo.DttStateTime = DateTime.Now;
                    updateItems.Add("Alarm", PointDefineInfo.Alarm);
                    updateItems.Add("ReDoDeal", PointDefineInfo.ReDoDeal);
                    updateItems.Add("DataState", PointDefineInfo.DataState);
                    //updateItems.Add("DttStateTime", PointDefineInfo.DttStateTime);
                    updateItems.Add("DttStateTime",CreatedTime);
                    updateItems.Add("State", PointDefineInfo.State);
                    updateItems.Add("Zts", PointDefineInfo.Zts);
                    updateItems.Add("DttRunStateTime", PointDefineInfo.DttRunStateTime);
                    updateItems.Add("Ssz", PointDefineInfo.Ssz);
                    updateItems.Add("Voltage", RealDataItem.Voltage);//增加电压赋值
                }
            }
            return updateItems;
            //更新实时值
            //if (updateItems.Count > 0)//hdw1
            //{
            //    KJ73NHelper.UpdatePointDefineInfoByProperties(PointDefineInfo.PointID, updateItems);
            //}
            //}
        }

        protected override void PointRunRecord(DeviceDataState dataState, DeviceRunState runState)
        {
            //设备状态不为休眠则处理数据
            if ((PointDefineInfo.Bz4 & 0x02) != 0x02)
            {
                string ssz = "";
                int isAlarm = 0;

                if (SafetyHelper.GetDerailShowInfo(PointDefineInfo, dataState, ref ssz, ref isAlarm))
                {
                    ////PointDefineInfo.Ssz = ssz;
                    //if (PointDefineInfo.Alarm != (short)isAlarm)
                    //{
                    //    //更新JC_B表的isAlarm标记 2017.7.12 by
                    //    UpdateJCBIsAlram(PointDefineInfo, isAlarm);
                    //    //更新缓存的报警标记
                    //    PointDefineInfo.Alarm = (short)isAlarm;
                    //}
                    //运行记录保存至缓存及数据库
                    if (PointDefineInfo.DataState != (short)dataState || PointDefineInfo.State != (short)runState)  //设备状态变化或数据状态变化才写记录
                    {
                        PointDefineInfo.Zts = CreatedTime;
                        SafetyHelper.CreateRunLogInfo(PointDefineInfo, CreatedTime, (short)dataState, (short)runState, ssz);
                    }
                    //状态变化
                    if (PointDefineInfo.DataState != (short)dataState)
                    {
                        PointDefineInfo.Alarm = (short)isAlarm;
                        CheckDerailAlarmInfo(dataState, runState);
                        //处理开关量交叉控制信息
                        if (runState != DeviceRunState.EquipmentTypeError)  //2017.9.22 by 类型不匹配，不下发交叉控制
                        {
                            CheckDerailControlInfo(dataState, runState);
                        }
                    }
                    else
                    {
                        if (PointDefineInfo.Alarm != (short)isAlarm)
                        {
                            //更新JC_B表的isAlarm标记 2017.7.12 by
                            UpdateJCBIsAlram(PointDefineInfo, isAlarm);
                            //更新缓存的报警标记
                            PointDefineInfo.Alarm = (short)isAlarm;
                        }
                    }
                    ////处理开关量交叉控制信息
                    //CheckDerailControlInfo(dataState);
                    //更新实时值
                    PointDefineInfo.Ssz = ssz;
                    PointDefineInfo.DataState = (short)dataState;
                    PointDefineInfo.State = (short)runState;
                    //PointDefineInfo.Zts = CreatedTime;
                }
            }
        }

        public override bool PretreatmentHandle(Jc_DefInfo pointDefineInfo)
        {
            Dictionary<string, object> updateItems = new Dictionary<string, object>();
            if (pointDefineInfo.ReDoDeal == 1)
            {
                pointDefineInfo.ReDoDeal = 2;
                updateItems.Add("ReDoDeal", pointDefineInfo.ReDoDeal);
            }
            pointDefineInfo.BCommDevTypeMatching = true;
            DateTime time = DateTime.Now;
           
            //开关量删除
            if (pointDefineInfo.InfoState == Basic.Framework.Web.InfoState.Delete || pointDefineInfo.Activity == "0")
            {
                AlarmBusiness.EndDerailAlarm(pointDefineInfo, time, true);
            }
            //开关量新增
            else if (pointDefineInfo.InfoState == Basic.Framework.Web.InfoState.AddNew)
            {
                pointDefineInfo.State = (short)DeviceRunState.EquipmentStateUnknow;
                pointDefineInfo.DataState = (short)DeviceRunState.EquipmentStateUnknow;
                //设备新增，默认设备类型匹配
                pointDefineInfo.BCommDevTypeMatching = true;
                updateItems.Add("BCommDevTypeMatching", pointDefineInfo.BCommDevTypeMatching);
            }
            //开关量修改
            else if (pointDefineInfo.InfoState == Basic.Framework.Web.InfoState.Modified)
            {
                //控制口变化处理
                if (pointDefineInfo.kzchangeflag)
                {
                    DerailControlChange(pointDefineInfo);
                }
                //休眠处理
                if ((pointDefineInfo.Bz4 & 0x02) == 0x02)
                {
                    #region ----设备休眠----
                    //结束报警
                    AlarmBusiness.EndDerailAlarm(pointDefineInfo, time, true);
                    updateItems.Add("ClsAlarmObj", PointDefineInfo.ClsAlarmObj);
                    updateItems.Add("Alarm", PointDefineInfo.Alarm);
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


        /// <summary>
        /// 判断测点状态变化,处理相应的报警信息
        /// </summary>
        /// <param name="dataState"></param>
        /// <param name="runState"></param>
        private void CheckDerailAlarmInfo(DeviceDataState dataState, DeviceRunState runState)
        {
            //if (PointDefineInfo.DataState != (short)dataState)
            {
                //Jc_BInfo alarmInfo;

                //AlarmCacheGetByConditonRequest getbyconditonrequest = new AlarmCacheGetByConditonRequest
                //{
                //    Predicate = a => a.Point == PointDefineInfo.Point && a.Stime == PointDefineInfo.DttRunStateTime && a.Type == PointDefineInfo.DataState
                //};
                //var getbyconditonresponse = alarmCacheService.GetAlarmCache(getbyconditonrequest);
                ////结束旧记录
                //if (getbyconditonresponse != null
                //    && getbyconditonresponse.IsSuccess
                //    && getbyconditonresponse.Data != null)
                //{
                //    alarmInfo = getbyconditonresponse.Data.FirstOrDefault();
                //    if (alarmInfo != null)
                //    {
                //        //alarmInfo.Stime = PointDefineInfo.DttRunStateTime;
                //        alarmInfo.Etime = CreatedTime;
                //        alarmInfo.InfoState = Basic.Framework.Web.InfoState.Modified;
                //        //ControlBus.GetDerailControlPort(PointDefineInfo, dataState, CreatedTime, null, 2);
                //        AlarmBusiness.InsertOrUpdateAlarmInfo(alarmInfo);

                       
                //    }
                //}
                AlarmBusiness.EndDerailAlarm(PointDefineInfo, CreatedTime, false);//此处不处理交叉控制，后面统一处理
                //生成新记录
                //Jc_BInfo alarmInfo = new Jc_BInfo();
                Jc_BInfo alarmInfo = AlarmBusiness.CreateAlarmInfo(PointDefineInfo, CreatedTime, dataState, runState, 0);
                alarmInfo.InfoState = Basic.Framework.Web.InfoState.AddNew;
                //alarmInfo.Cs = string.Empty;
                //alarmInfo.ID = IdHelper.CreateLongId().ToString();
                //alarmInfo.PointID = PointDefineInfo.PointID;
                //alarmInfo.Devid = PointDefineInfo.Devid;
                //alarmInfo.Fzh = PointDefineInfo.Fzh;
                //alarmInfo.Kh = PointDefineInfo.Kh;
                //alarmInfo.Dzh = PointDefineInfo.Dzh;
                //alarmInfo.Kzk =  ControlBus.GetDerailControlPort(PointDefineInfo, dataState);
                //alarmInfo.Point = PointDefineInfo.Point;
                //alarmInfo.Ssz = PointDefineInfo.Ssz;
                //alarmInfo.Stime = CreatedTime;
                //alarmInfo.Etime = new DateTime(1900, 1, 1, 0, 0, 0);
                //alarmInfo.Isalarm = PointDefineInfo.Alarm;
                //alarmInfo.Type = (short)dataState;
                //alarmInfo.State = (short)runState;
                //alarmInfo.Upflag = "0";
                //alarmInfo.Wzid = PointDefineInfo.Wzid;
                //alarmInfo.InfoState = Basic.Framework.Web.InfoState.AddNew;

                AlarmBusiness.InsertOrUpdateAlarmInfo(alarmInfo,true);

                PointDefineInfo.DttRunStateTime = CreatedTime;
                ////清除开关量本状态控制的断电信息的内存复电链表
                //KJ73NHelper.FdClearByArrPoint(PointDefineInfo, dataState);

                ////开关量断电，复电链表处理
                //DealDerailFDList((DeviceDataState)PointDefineInfo.DataState, dataState, alarmInfo.ID);
            }
        }

        /// <summary>
        /// 处理开关量交叉控制信息
        /// </summary>
        private void CheckDerailControlInfo(DeviceDataState datestate, DeviceRunState state)
        {
            ControlType controlType = ControlType.NoControl;
            if (state != DeviceRunState.EquipmentTypeError)
            {
                if (datestate == DeviceDataState.DataDerailState0)
                {
                    controlType = ControlType.ControlState0;
                }
                else if (datestate == DeviceDataState.DataDerailState1)
                {
                    controlType = ControlType.ControlState1;
                }
                else if (datestate == DeviceDataState.DataDerailState2)
                {
                    controlType = ControlType.ControlState2;
                }
            }
            ControlBus.DoControlChange(PointDefineInfo, controlType);
        }
        /// <summary>
        /// 开关量控制口修改处理
        /// </summary>
        /// <param name="pointDefineItem"></param>
        private void DerailControlChange(Jc_DefInfo pointDefineItem)
        {
            if (pointDefineItem.DataState == (short)DeviceDataState.DataDerailState0 ||
                    pointDefineItem.DataState == (short)DeviceDataState.DataDerailState1 ||
                    pointDefineItem.DataState == (short)DeviceDataState.DataDerailState2)
            {
                // 处理交叉控制
                CheckDerailControlInfo((DeviceDataState)pointDefineItem.DataState, (DeviceRunState)pointDefineItem.State);
                //更新JC_B的kzk字段
                Jc_BInfo JC_B = AlarmBusiness.CreateControlChangeAlarmInfo(pointDefineItem, (DeviceDataState)pointDefineItem.DataState, pointDefineItem.DttRunStateTime);
                AlarmBusiness.UpdateAlarmInfo(JC_B, 6,false);              
            }
        }

        /// <summary>
        /// 更新JC_B表的isalarm标记
        /// </summary>
        /// <param name="def"></param>
        /// <param name="isAlarm"></param>
        private static void UpdateJCBIsAlram(Jc_DefInfo def, int isAlarm)
        {
            try
            {
                IAlarmCacheService alarmCacheService = ServiceFactory.Create<IAlarmCacheService>();
                AlarmCacheGetByConditonRequest alarmCacheGetByConditonRequest = new AlarmCacheGetByConditonRequest();
                List<Jc_BInfo> alarmItems;
                alarmCacheGetByConditonRequest.Predicate = p => p.Stime == def.DttRunStateTime && p.Point == def.Point && p.Type == def.DataState;

                if (alarmCacheGetByConditonRequest.Predicate != null)
                {
                    var alarmResponse = alarmCacheService.GetAlarmCache(alarmCacheGetByConditonRequest);
                    if (alarmResponse.Data != null && alarmResponse.IsSuccess)
                    {
                        alarmItems = alarmResponse.Data;
                        if (alarmItems.Count > 0)
                        {
                            alarmItems.ForEach(a =>
                            {
                                a.Isalarm = (short)isAlarm;
                                a.InfoState = InfoState.Modified;
                            });
                            BatchUpdateAlarmInfo(alarmItems);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("UpdateJCBIsAlram Error:" + ex.Message);
            }
        }
        
        private static void BatchUpdateAlarmInfo(List<Jc_BInfo> alarmItems)
        {
            IAlarmCacheService alarmCacheService = ServiceFactory.Create<IAlarmCacheService>();
            AlarmCacheUpdatePropertiesRequest alarmCacheUpdatePropertiesRequest = new AlarmCacheUpdatePropertiesRequest();
            Dictionary<string, object> updateItems = new Dictionary<string, object>();
            foreach (Jc_BInfo alarmInfo in alarmItems)
            {
                updateItems = new Dictionary<string, object>();
                updateItems.Add("Isalarm", alarmInfo.Isalarm);
                //更新到缓存
                alarmCacheUpdatePropertiesRequest.AlarmKey = alarmInfo.ID;
                alarmCacheUpdatePropertiesRequest.UpdateItems = updateItems;
                alarmCacheService.UpdateAlarmInfoProperties(alarmCacheUpdatePropertiesRequest);
            }
            //更新到数据库
            IAlarmRecordRepository alarmRecordRepository = ServiceFactory.Create<IAlarmRecordRepository>();
            System.Data.DataColumn[] cols = new System.Data.DataColumn[updateItems.Count];
            for (int i = 0; i < updateItems.Count; i++)
            {
                cols[i] = new System.Data.DataColumn(updateItems.Keys.ToList()[i]);
            }
            List<Jc_BModel> alarmModel;
            alarmItems.ForEach(item =>
            {
                alarmModel = new List<Jc_BModel>();
                alarmModel.Add(ObjectConverter.Copy<Jc_BInfo, Jc_BModel>(item));
                alarmRecordRepository.BulkUpdate("KJ_DataAlarm" + item.Stime.ToString("yyyyMM"), alarmModel, cols, "ID");
            });
        }
    }
}
