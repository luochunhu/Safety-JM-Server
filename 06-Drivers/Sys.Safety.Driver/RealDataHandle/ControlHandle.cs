using System;
using System.Collections.Generic;
using Sys.Safety.Enums;
using Sys.DataCollection.Common.Protocols;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Cache;
using Basic.Framework.Logging;

namespace Sys.Safety.Driver.RealDataHandle
{
    /// <summary>
    /// 作者：
    /// 时间：2017-05-31
    /// 描述：控制量实时数据处理
    /// 修改记录
    /// 2017-05-31
    /// </summary>
    public class ControlHandle : PointHandle
    {
        protected Dictionary<string, object> UpDateItemS = new Dictionary<string, object>();
        protected override Dictionary<string, object> DataHandle()
        {            
            DeviceDataState datastate = DeviceDataState.DataControlState0;
            //控制状态
            if (!SafetyHelper.isAbnormalState((DeviceRunState)RealDataItem.State))
            {
                if (RealDataItem.State == ItemState.EquipmentControlDown)
                {
                    datastate = DeviceDataState.EquipmentControlDown;
                }
                else
                {
                    datastate = RealDataItem.RealData == "0" ? DeviceDataState.DataControlState0 : DeviceDataState.DataControlState1;
                }
               

                if (PointDefineInfo.State != (short)RealDataItem.State || PointDefineInfo.DataState != (short)datastate || PointDefineInfo.ReDoDeal == 2)
                {
                    PointDefineInfo.ReDoDeal = 0;
                    PointRunRecord(datastate, (DeviceRunState)((int)RealDataItem.State));
                }
            }
            else if (PointDefineInfo.State != (short)DeviceRunState.EquipmentControlDown || PointDefineInfo.ReDoDeal ==2)
            {
                PointDefineInfo.ReDoDeal = 0;
                PointRunRecord(DeviceDataState.EquipmentControlDown, DeviceRunState.EquipmentControlDown);
            }

            //处理馈电
            #region ----处理馈电----
            if (IsHaveFeedPoint(PointDefineInfo))
            {
                #region ---- 自带馈电状态 ----
                //控制馈电状态
                if (RealDataItem.FeedState == "1")
                {
                    if (PointDefineInfo.NCtrlSate != (int)ControlState.DataPowerOffSuccess)
                    {
                        //断电成功
                        StatiskdPointRunRecord(DeviceDataState.DataPowerOffSuc, (DeviceRunState)RealDataItem.State);
                    }

                }
                //断电失败
                else if (RealDataItem.FeedState == "2")
                {
                    if (PointDefineInfo.NCtrlSate != (int)ControlState.DataPowerOffFail)
                    {
                        //断电失败
                        StatiskdPointRunRecord(DeviceDataState.DataPowerOffFail, (DeviceRunState)RealDataItem.State);
                    }
                }
                //复电成功
                else if (RealDataItem.FeedState == "3")
                {
                    if (PointDefineInfo.NCtrlSate != (int)ControlState.DataPowerOnSuccess)
                    {
                        //复电成功
                        StatiskdPointRunRecord(DeviceDataState.DataPowerOnSuc, (DeviceRunState)RealDataItem.State);
                    }
                }
                //复电失败
                else if (RealDataItem.FeedState == "4")
                {
                    if (PointDefineInfo.NCtrlSate != (int)ControlState.DataPowerOnFail)
                    {
                        //复电失败     
                        StatiskdPointRunRecord(DeviceDataState.DataPowerOnFail, (DeviceRunState)RealDataItem.State);
                    }
                }
                #endregion
            }
            #endregion

            //2018.1.13 by
            if (!UpDateItemS.ContainsKey("DttStateTime"))
            {
                UpDateItemS.Add("DttStateTime", CreatedTime);
            }

            return UpDateItemS;
        }

        public override bool PretreatmentHandle(Jc_DefInfo pointDefineInfo)
        {
            Dictionary<string, object> updateItems = new Dictionary<string, object>();
            DateTime time = DateTime.Now;
            if (pointDefineInfo.ReDoDeal == 1)
            {
                pointDefineInfo.ReDoDeal = 2;
                updateItems.Add("ReDoDeal", pointDefineInfo.ReDoDeal);
            }
            //控制量删除
            if (pointDefineInfo.InfoState == Basic.Framework.Web.InfoState.Delete || pointDefineInfo.Activity == "0")
            {
                AlarmBusiness.EndAlarmInfo(pointDefineInfo, (short)pointDefineInfo.NCtrlSate, pointDefineInfo.DttkdStrtime, time);
            }
            //控制量新增
            else if (pointDefineInfo.InfoState == Basic.Framework.Web.InfoState.AddNew)
            {
                pointDefineInfo.Ssz = "";
                pointDefineInfo.State = (short)DeviceRunState.EquipmentStateUnknow;
                pointDefineInfo.DataState = (short)DeviceRunState.EquipmentStateUnknow;
                //设备新增，默认设备类型匹配
                pointDefineInfo.BCommDevTypeMatching = true;
                updateItems.Add("BCommDevTypeMatching", pointDefineInfo.BCommDevTypeMatching);
            }
            //控制量修改
            else if (pointDefineInfo.InfoState == Basic.Framework.Web.InfoState.Modified && pointDefineInfo.DefIsInit)
            {
                //休眠处理
                if ((pointDefineInfo.Bz4 & 0x02) == 0x02)
                {
                    #region ----设备休眠----
                    //结束报警
                    AlarmBusiness.EndAlarmInfo(pointDefineInfo, (short)pointDefineInfo.NCtrlSate, pointDefineInfo.DttkdStrtime, time);
                    //写R表记录
                    SafetyHelper.CreateRunLogInfo(pointDefineInfo, time, PointDefineInfo.DataState, (short)DeviceRunState.EquipmentSleep, pointDefineInfo.Ssz);
                    //模拟量休眠
                    pointDefineInfo.DataState = (short)DeviceDataState.EquipmentSleep;
                    pointDefineInfo.State = (short)DeviceRunState.EquipmentSleep;
                    #endregion
                }
            }
            updateItems.Add("Ssz", pointDefineInfo.Ssz);
            updateItems.Add("State", pointDefineInfo.State);
            updateItems.Add("DataState", pointDefineInfo.DataState);
            if (updateItems.Count > 0)
            {
                SafetyHelper.UpdatePointDefineInfoByProperties(pointDefineInfo.PointID, updateItems);
            }
            return pointDefineInfo.DefIsInit;
        }

        protected override void PointRunRecord(DeviceDataState dataState, DeviceRunState runState)
        {
            //设备状态不为休眠则处理数据
            if ((PointDefineInfo.Bz4 & 0x2) != 0x2)
            {
                string ssz = "";
                int isAlarm = 0;
                if (SafetyHelper.GetControlShowInfo(PointDefineInfo, dataState, ref ssz, ref isAlarm))
                {
                    PointDefineInfo.Ssz = ssz;
                    PointDefineInfo.Alarm = (short)isAlarm;

                    //运行记录保存至缓存及数据库
                    if (PointDefineInfo.DataState != (short)dataState || PointDefineInfo.State != (short)runState)
                    {
                        string remark = "";
                        long SoleCoding = 0;
                        long.TryParse(RealDataItem.SoleCoding, out SoleCoding);
                        if (SoleCoding == 0x10101010)//0x10101010
                        {
                            remark = "上电闭锁";
                        }
                        SafetyHelper.CreateRunLogInfo(PointDefineInfo, CreatedTime, (short)dataState, (short)runState, PointDefineInfo.Ssz, remark);
                    }
                    //更新实时值
                    PointDefineInfo.DataState = (short)dataState;
                    PointDefineInfo.State = (short)runState;
                    PointDefineInfo.Zts = CreatedTime;
                    PointDefineInfo.DttStateTime = DateTime.Now;
                    //Dictionary<string, object> updateItems = new Dictionary<string, object>();hdw1
                    UpDateItemS.Add("ReDoDeal", PointDefineInfo.ReDoDeal);
                    UpDateItemS.Add("Ssz", PointDefineInfo.Ssz);
                    UpDateItemS.Add("Alarm", PointDefineInfo.Alarm);
                    UpDateItemS.Add("DataState", PointDefineInfo.DataState);
                    //UpDateItemS.Add("DttStateTime", PointDefineInfo.DttStateTime);
                    UpDateItemS.Add("DttStateTime", CreatedTime);
                    UpDateItemS.Add("State", PointDefineInfo.State);
                    UpDateItemS.Add("Zts", PointDefineInfo.Zts);

                    //
                    //KJ73NHelper.UpdatePointDefineInfoByProperties(PointDefineInfo.PointID, updateItems);hdw1
                }
            }
        }

        protected void StatiskdPointRunRecord(DeviceDataState dataState, DeviceRunState runState)
        {
            #region 馈电量处理
            Jc_BInfo JC_B;
            if ((PointDefineInfo.Bz4 & 0x02) != 0x02)
            {
                #region 运行记录

                SafetyHelper.CreateRunLogInfo(PointDefineInfo, CreatedTime, (short)dataState, (short)runState, EnumHelper.GetEnumDescription(dataState));

                #endregion

                if (dataState == DeviceDataState.DataPowerOnFail || dataState == DeviceDataState.DataPowerOffFail)
                {
                    JC_B = AlarmBusiness.CreateAlarmInfo(PointDefineInfo, CreatedTime, dataState, runState, 0);

                    //if (dataState == DeviceDataState.DataPowerOnFail)
                    //{
                    //    KJ73NHelper.DealFdStart(PointDefineInfo, CreatedTime, JC_B.ID);
                    //    //PointDefineInfo.Sckdid = Convert.ToInt64(JC_B.ID); //todo  何用？
                    //}
                    //else if (dataState == DeviceDataState.DataPowerOffFail)
                    //{
                    //    KJ73NHelper.DealDdStart(PointDefineInfo, CreatedTime, JC_B.ID);
                    //    //PointDefineInfo.Sckdid = Convert.ToInt64(JC_B.ID);
                    //}
                    AlarmBusiness.UpdateAlarmInfo(JC_B, 1,false);
                }
                if (PointDefineInfo.NCtrlSate == (int)ControlState.DataPowerOnFail || PointDefineInfo.NCtrlSate == (int)ControlState.DataPowerOffFail)
                {
                    //List<Jc_DefInfo> controlItems = new List<Jc_DefInfo>();
                    //controlItems.Add(PointDefineInfo);
                    //KJ73NHelper.BatchEndControlAlarm(controlItems, CreatedTime);
                    // EndAlarmInfo(defInfo, (short)defInfo.NCtrlSate, defInfo.DttkdStrtime, time);
                    AlarmBusiness.EndAlarmInfo(PointDefineInfo, (short)PointDefineInfo.NCtrlSate, PointDefineInfo.DttkdStrtime, CreatedTime);
                }
            }

            if (dataState == DeviceDataState.DataPowerOnFail)
            {
                PointDefineInfo.NCtrlSate = (int)ControlState.DataPowerOnFail;
            }
            else if (dataState == DeviceDataState.DataPowerOffFail)
            {
                PointDefineInfo.NCtrlSate = (int)ControlState.DataPowerOffFail;
            }
            else if (dataState == DeviceDataState.DataPowerOffSuc)
            {
                PointDefineInfo.NCtrlSate = (int)ControlState.DataPowerOffSuccess;
                //清除断电失败链表
                //DdClear(PointDefineInfo); 2017.6.22 by 屏蔽
            }
            else if (dataState == DeviceDataState.DataPowerOnSuc)
            {
                PointDefineInfo.NCtrlSate = (int)ControlState.DataPowerOnSuccess;
                //清除复电失败链表
                //FdClear(PointDefineInfo); 2017.6.22 by 屏蔽
            }
            PointDefineInfo.DttkdStrtime = CreatedTime;
            #endregion
            //Dictionary<string, object> updateItems = new Dictionary<string, object>();hdw1
            UpDateItemS.Add("NCtrlSate", PointDefineInfo.NCtrlSate);
            UpDateItemS.Add("DttkdStrtime", PointDefineInfo.DttkdStrtime);
            PointDefineInfo.DttStateTime = DateTime.Now;
            if (!UpDateItemS.ContainsKey("DttStateTime"))
            {
                //UpDateItemS.Add("DttStateTime", PointDefineInfo.DttStateTime);
                UpDateItemS.Add("DttStateTime", CreatedTime);
            }
            //updateItems.Add("DkdStrtime", PointDefineInfo.DkdStrtime);
            //updateItems.Add("Sckdid", PointDefineInfo.Sckdid);
            //KJ73NHelper.UpdatePointDefineInfoByProperties(PointDefineInfo.PointID, updateItems);hdw1
        }

        ///// <summary>
        ///// 处理断电失败开始
        ///// </summary>
        ///// <param name="item" >控制量对象</param>
        ///// <param name="time" >开始时间</param>
        //private void DealDdStart(Jc_DefInfo item, DateTime time, string id)
        //{
        //    #region 写断电异常开始
        //    List<AbnormalFeedItem> ilist = null;
        //    Jc_BInfo JC_B;
        //    try
        //    {
        //        if (AbnormalFeedClass.kzkdlist.Count > 0)
        //        {
        //            ilist = AbnormalFeedClass.kzkdlist.FindAll(delegate(AbnormalFeedItem it) { return it.ControlPoint == item.Point; });
        //            if (ilist != null && ilist.Count > 0)
        //            {
        //                foreach (AbnormalFeedItem abitem in ilist)
        //                {
        //                    if (!abitem.HaveWrite)
        //                    {
        //                        abitem.HaveWrite = true;
        //                        #region 修改运行记录中的馈电异常标志
        //                        try
        //                        {

        //                            JC_B = new Jc_BInfo();
        //                            JC_B.Stime = abitem.TriggerTime;
        //                            JC_B.Type = (short)abitem.TriggerState;
        //                            JC_B.Point = abitem.arrPoint;
        //                            JC_B.PointID = item.PointID;
        //                            JC_B.Kdid = "," + id;

        //                            KJ73NHelper.UpdateAlarmInfo(JC_B, 3);
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            LogHelper.Info("DealDdStart Error:" + ex.Message);
        //                        }
        //                        #endregion
        //                    }
        //                }
        //                for (int i = 0; i < AbnormalFeedClass.kzkdlist.Count; i++)
        //                {
        //                    if (AbnormalFeedClass.kzkdlist[i].HaveWrite)
        //                    {
        //                        AbnormalFeedClass.kzkdlist.RemoveAt(i);
        //                        i--;
        //                    }
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Info(ex);
        //    }
        //    #endregion
        //}

        ///// <summary>
        ///// 处理复电失败开始
        ///// </summary>
        ///// <param name="item" >控制量对象</param >
        ///// <param name="time" >开始时间</param>
        //private void DealFdStart(Jc_DefInfo item, DateTime time, string id)
        //{
        //    #region 写复电失败开始
        //    List<AbnormalFeedItem> ilist = null;
        //    Jc_BInfo JC_B;
        //    try
        //    {
        //        if (AbnormalFeedClass.jkkdlist.Count > 0)
        //        {
        //            ilist = AbnormalFeedClass.jkkdlist.FindAll(delegate(AbnormalFeedItem it) { return it.ControlPoint == item.Point; });
        //            if (ilist != null && ilist.Count > 0)
        //            {
        //                foreach (AbnormalFeedItem abitem in ilist)
        //                {
        //                    if (!abitem.HaveWrite)
        //                    {
        //                        abitem.HaveWrite = true;
        //                        try
        //                        {
        //                            JC_B = new Jc_BInfo();
        //                            JC_B.Stime = abitem.TriggerTime;
        //                            JC_B.Type = (short)abitem.TriggerState;
        //                            JC_B.Point = abitem.arrPoint;
        //                            JC_B.Kdid = "," + id;
        //                            KJ73NHelper.UpdateAlarmInfo(JC_B, 3);
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            LogHelper.Info(ex);
        //                        }
        //                    }
        //                }
        //                for (int i = 0; i < AbnormalFeedClass.jkkdlist.Count; i++)
        //                {
        //                    if (AbnormalFeedClass.jkkdlist[i].HaveWrite)
        //                    {
        //                        AbnormalFeedClass.jkkdlist.RemoveAt(i);
        //                        i--;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Info(ex);
        //    }
        //    #endregion
        //}
        ///// <summary>
        ///// 清除复电主控点
        ///// </summary>
        ///// <param name="item"></param>
        //private void FdClear(Jc_DefInfo item)
        //{
        //    try
        //    {
        //        for (int i = 0; i < AbnormalFeedClass.jkkdlist.Count; i++)
        //        {
        //            if (AbnormalFeedClass.jkkdlist[i].ControlPoint == item.Point)
        //            {
        //                AbnormalFeedClass.jkkdlist.RemoveAt(i);
        //                i--;
        //            }
        //        }
        //    }
        //    catch
        //    { }
        //}
        ///// <summary>
        ///// 清除断电主控点
        ///// </summary>
        ///// <param name="item"></param>
        //private void DdClear(Jc_DefInfo item)
        //{
        //    try
        //    {
        //        for (int i = 0; i < AbnormalFeedClass.kzkdlist.Count; i++)
        //        {
        //            if (AbnormalFeedClass.kzkdlist[i].ControlPoint == item.Point)
        //            {
        //                AbnormalFeedClass.kzkdlist.RemoveAt(i);
        //                i--;
        //            }
        //        }
        //    }
        //    catch
        //    { }
        //}

        /// <summary>
        /// 判断测点是否关联馈电
        /// </summary>
        /// <param name="def"></param>
        /// <returns></returns>
        private bool IsHaveFeedPoint(Jc_DefInfo def)
        {
            bool flag = false;
            if (def.DevPropertyID != (int)DeviceProperty.Control)
            {
                //只有控制量才关联有馈电
                return false;
            }
            int fzh = def.K1;
            int kh = def.K2;
            int dzh = def.K4;
            string point = fzh.ToString().PadLeft(3, '0') + "D" + kh.ToString().PadLeft(2, '0') + dzh;
            if (SafetyHelper.GetPointDefinesByPoint(point) != null)
            {
                flag = true;
            }

            return flag;
        }
    }
}
