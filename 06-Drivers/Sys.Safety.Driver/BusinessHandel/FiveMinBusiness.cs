using Basic.Framework.Common;
using Basic.Framework.Logging;
using Sys.DataCollection.Common.Protocols.Devices;
using Sys.Safety.DataContract;
using Sys.Safety.Enums;
using Sys.Safety.Request.DataToDb;
using Sys.Safety.Request.StaionHistoryData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Driver
{
    class FiveMinBusiness
    {
        #region 五分钟数据相关操作
        /// <summary>
        /// 五分钟记录入库
        /// </summary>
        /// <param name="analogInfo"></param>
        /// <param name="time"></param>
        public static void CreateFiviMinInfo(Jc_DefInfo analogInfo, DateTime time)
        {
            int allCount;
            decimal maxVal, minVal, allVal, avgVal;
            DateTime maxValTime, minValTime;

            lock (SafetyHelper.fiveMiniteDataProcLocker)
            {
                maxVal = analogInfo.ClsFiveMinObj.m_nMaxVal;
                minVal = analogInfo.ClsFiveMinObj.m_nMinVal;
                maxValTime = analogInfo.ClsFiveMinObj.m_nMaxValTime;
                minValTime = analogInfo.ClsFiveMinObj.m_nMinValTime;
                allVal = analogInfo.ClsFiveMinObj.m_nAllVal;
                allCount = analogInfo.ClsFiveMinObj.m_nAllCount;
            }

            Jc_MInfo jc_mrecord = new Jc_MInfo();
            jc_mrecord.ID = IdHelper.CreateLongId().ToString();
            jc_mrecord.Devid = analogInfo.Devid;
            jc_mrecord.PointId = analogInfo.PointID;
            jc_mrecord.Point = analogInfo.Point;
            jc_mrecord.Kh = analogInfo.Kh;
            jc_mrecord.Dzh = analogInfo.Dzh;
            jc_mrecord.Fzh = analogInfo.Fzh;

            //jc_mrecord.Sj = Convert.ToInt16(time.Hour.ToString().PadLeft(2, '0') + time.Minute.ToString().PadLeft(2, '0'));

            if (allCount > 0)
            {
                float sszValue = 0;
                avgVal = Math.Round(allVal / allCount, 2);
                jc_mrecord.Pjz = Convert.ToSingle(avgVal);
                if (float.TryParse(analogInfo.Ssz, out sszValue))
                {
                    jc_mrecord.Ssz = sszValue;
                }
                else
                {
                    jc_mrecord.Ssz = 0;
                }

                if (analogInfo.DataState == (short)DeviceDataState.EquipmentStateUnknow)
                {
                    jc_mrecord.Type = (short)DeviceDataState.EquipmentDown;
                    jc_mrecord.State = SafetyHelper.GetState(DeviceRunState.EquipmentDown, analogInfo.Bz4);
                }
                else
                {
                    jc_mrecord.Type = analogInfo.DataState;
                    jc_mrecord.State = SafetyHelper.GetState((DeviceRunState)analogInfo.State, analogInfo.Bz4);
                }
                jc_mrecord.Type = (short)analogInfo.ClsFiveMinObj.m_maxValueDataState;
            }
            else
            {
                jc_mrecord.Pjz = 0;
                jc_mrecord.Ssz = 0;
                if (analogInfo.DataState == (short)DeviceDataState.EquipmentStateUnknow)
                {
                    jc_mrecord.Type = (short)DeviceDataState.EquipmentDown;
                    jc_mrecord.State = SafetyHelper.GetState(DeviceRunState.EquipmentDown, analogInfo.Bz4);
                }
                else
                {
                    jc_mrecord.Type = analogInfo.DataState;
                    jc_mrecord.State = SafetyHelper.GetState((DeviceRunState)analogInfo.State, analogInfo.Bz4);
                }
            }

            if (time.Hour == 0 && time.Minute == 0)
            {
                jc_mrecord.Sj = 2355;
                jc_mrecord.Timer = new DateTime(time.AddDays(-1).Year, time.AddDays(-1).Month, time.AddDays(-1).Day, 23, 55, 00);
            }
            else if (time.Minute % 5 == 0)
            {
                if (time.Minute == 0)
                {
                    jc_mrecord.Sj = (short)((time.Hour - 1) * 100 + 55);
                }
                else
                {
                    jc_mrecord.Sj = (short)(time.Hour * 100 + time.Minute - 5);
                }
                jc_mrecord.Timer = DateTime.Parse(time.AddMinutes(-5).ToString("yyyy-MM-dd HH:mm:00"));
            }
            else
            {
               
                #region ----当前时间已经不是内存中五分钟对象的时间，将数据写到之前的五分钟数据里去----2017.11.13 by
                DateTime tempDate = maxValTime;
                tempDate = new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, tempDate.Hour, (maxValTime.Minute / 5) * 5, 0);
                LogHelper.Info("【" + jc_mrecord.Point + "】写五分钟时间判断异常，当前时间：" + DateTime.Now + ",将数据写到之前的五分钟数据里去" + tempDate);
                jc_mrecord.Sj = (short)(tempDate.Hour * 100 + tempDate.Minute);           
                jc_mrecord.Timer = DateTime.Parse(tempDate.ToString("yyyy-MM-dd HH:mm:00"));
                #endregion
            }

            jc_mrecord.Upflag = "0";
            jc_mrecord.Wzid = analogInfo.Wzid;
            jc_mrecord.Zdz = Convert.ToSingle(maxVal);
            jc_mrecord.Zdzs = (maxValTime.Year < 1900 ? Convert.ToDateTime("1900-01-01 00:00:00") : maxValTime);
            jc_mrecord.Zxz = Convert.ToSingle(minVal);
            jc_mrecord.Zxzs = (minValTime.Year < 1900 ? Convert.ToDateTime("1900-01-01 00:00:00") : minValTime);          
            jc_mrecord.InfoState = Basic.Framework.Web.InfoState.AddNew;
            //五分钟数据入库
            DataToDbAddRequest<Jc_MInfo> dataToDbAddRequest = new DataToDbAddRequest<Jc_MInfo>();
            dataToDbAddRequest.Item = jc_mrecord;
            SafetyHelper.fiveMinDataTodbService.AddItem(dataToDbAddRequest);

            if (analogInfo.DevClassID == 1 && maxVal > 40)
            {
                LogHelper.Debug("【" + analogInfo.Point + "】CreateFiviMinInfo:value=" + maxVal + ",State=" + analogInfo.State);
            }
        }

        #endregion

        #region ----历史五分钟----

        public static StaionHistoryDataInfo GetStaionHistoryData(int fzh, string ponit, DeviceHistoryRealDataItem item, DateTime time)
        {
            StaionHistoryDataInfo data = new StaionHistoryDataInfo();

            try
            {
                data.Id = IdHelper.CreateLongId().ToString();
                data.Fzh = fzh;
                data.Point = ponit;
                data.Kh = Convert.ToInt32(item.Channel);
                data.Dzh = Convert.ToInt32(item.Address);
                data.SaveTime = item.HistoryDate;
                data.State = (int)item.State;
                data.DataTime = time;
                data.GradingAlarmLevel = item.SeniorGradeAlarm;
                data.RealData = item.RealData;
                data.Voltage = item.Voltage;
                data.DeviceTypeCode = item.DeviceTypeCode;
                data.FeedBackState = item.FeedBackState;
                data.FeedState = item.FeedState;
            }
            catch (Exception ex)
            {
                LogHelper.Error("GetStaionHistoryData Error:" + ponit + "," + ex.Message);
            }

            return data;
        }
        /// <summary>
        /// 历史五分钟数据入库
        /// </summary>
        /// <param name="items"></param>
        public static void StaionHistoryDataToDB(List<StaionHistoryDataInfo> items)
        {
            //删除已有记录
            DeleteByPointAndTimeStationHistoryDataRequest deleteByPointAndTimeStationHistoryDataRequest = new DeleteByPointAndTimeStationHistoryDataRequest();
            foreach (StaionHistoryDataInfo item in items)
            {
                deleteByPointAndTimeStationHistoryDataRequest.Point = item.Point;
                deleteByPointAndTimeStationHistoryDataRequest.Time = item.SaveTime;
                SafetyHelper.staionHistoryDataService.DeleteStationHistoryDataByPointAndTime(deleteByPointAndTimeStationHistoryDataRequest);
            }
            //新增记录到数据库
            InsertStationHistoryDataRequest insertStationHistoryDataRequest = new InsertStationHistoryDataRequest();
            insertStationHistoryDataRequest.StationHistoryDataItems = items;
            SafetyHelper.staionHistoryDataService.InsertStationHistoryDataToDB(insertStationHistoryDataRequest);
        }

        #endregion

    }
}
