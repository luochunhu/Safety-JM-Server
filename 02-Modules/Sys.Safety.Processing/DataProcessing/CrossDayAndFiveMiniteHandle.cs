using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;

using Sys.Safety.Interface;
using Sys.Safety.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using Sys.Safety.Request.Jc_B;
using Basic.Framework.Common;
using Sys.Safety.Model;
using Sys.Safety.Request.Cache;
using Sys.Safety.ServiceContract.Cache;
using Sys.Safety.Request.DataToDb;
using Sys.Safety.ServiceContract.DataToDb;
using Basic.Framework.Web;

namespace Sys.Safety.Processing.DataProcessing
{
    /// <summary>
    /// 跨天和五分钟数据处理
    /// </summary>
    public class CrossDayAndFiveMiniteHandle
    {
        /// <summary>
        /// 当前是否正在进行跨天处理（跨在处理时，数据接收线程不接收数据）
        /// </summary>
        public static bool isCrossDay = false;
        IAlarmRecordRepository _Repository = ServiceFactory.Create<IAlarmRecordRepository>();
        IAlarmCacheService _alarmCacheService = ServiceFactory.Create<IAlarmCacheService>();

        #region----私有变量----
        /// <summary>
        /// 线程运行标记（=false 线程结束，默认=true）
        /// </summary>
        private bool isStop = true;
        /// <summary>
        /// 跨天和五分钟处理线程
        /// </summary>
        private Thread runThread;
        #endregion


        public CrossDayAndFiveMiniteHandle()
        {
            CreatDayTable(DateTime.Now);
            runThread = new Thread(CrossDayAndFiveMinite);
        }
        /// <summary>
        /// 启动跨天和五分钟处理线程
        /// </summary>
        public void Start()
        {
            LogHelper.Info("启动跨天五分钟处理模块");
            isStop = false;
            if (runThread.ThreadState != ThreadState.Running)
            {
                runThread.Start();
            }
        }
        /// <summary>
        /// 停止启动跨天和五分钟处理线程
        /// </summary>
        public void Stop()
        {
            isStop = true;
            LogHelper.Info("开始停止跨天五分钟处理模块");
        }

        #region ---私有方法----
        /// <summary>
        /// 跨天和五分钟处理方法
        /// </summary>
        private void CrossDayAndFiveMinite()
        {
            DateTime fiveMiniteProcMinite = DateTime.Now;
            DateTime nowTime = DateTime.Now;
            List<Jc_DefInfo> pointDefineItems;
            List<Jc_DevInfo> pointDevItems;
            Jc_DevInfo pointDevItem;
            IDriver driverObj = null;
            int totalcount=0;

            bool doFiveMinData = false; //是否要处理五分钟数据  2017.9.15 by

            for (; ; )
            {
                try
                {
                    doFiveMinData = false;

                    if (isStop)
                    {
                        LogHelper.Info("停止数据处理模块【跨天和五分钟处理方法处理完成】！");
                        break;
                    }

                    if (DateTime.Now.Day != nowTime.Day)
                    {
                        //2017.12.18 by 异常处理跨天(手动修改电脑时间)
                        CrossDay(new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 23, 59, 59),
                            new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0));
                    }

                    nowTime = DateTime.Now;

                    //if (nowTime.Hour == 0 && nowTime.Minute == 0)
                    //{
                    //    doFiveMinData = false;  //刚跨天00：00：00 满足写记录条件，但不应该写记录
                    //}
                    //else  if (nowTime.Hour == 23 && nowTime.Minute == 59 && nowTime.Second > 40 && nowTime.Minute != fiveMiniteProcMinite.Minute) //2017.9.15 by
                    //{
                    //    doFiveMinData = true;
                    //}
                    //else 
                    if ((nowTime.Minute % 5 == 0) && (nowTime.Minute != fiveMiniteProcMinite.Minute))
                    {
                        doFiveMinData = true;
                    }

                    //if ((nowTime.Minute % 5 == 0) && (nowTime.Minute != fiveMiniteProcMinite.Minute))
                    if (doFiveMinData)
                    {
                        fiveMiniteProcMinite = nowTime;
                        #region ----五分钟数据处理----
                        pointDefineItems = CacheDataHelper.GetKJPointDefineItems();
                        pointDefineItems = pointDefineItems.Where(a => a.DevPropertyID == (int)DeviceProperty.Substation).ToList();
                        pointDevItems = CacheDataHelper.GetAllDevItems();

                        foreach (Jc_DefInfo station in pointDefineItems)
                        {
                            pointDevItem = pointDevItems.FirstOrDefault(a => a.Devid == station.Devid);
                            if (pointDevItem != null)
                            {
                                if (GlobleStaticVariable.driverHandle.DriverItems.ContainsKey(pointDevItem.Sysid))
                                {
                                    driverObj = GlobleStaticVariable.driverHandle.DriverItems[pointDevItem.Sysid].DLLObj;
                                    DriverTransferInterface.Drv_FiveMinPro(driverObj, station.Fzh, nowTime);
                                }
                            }
                        }

                        #endregion
                    }
                    if (nowTime.Hour == 23 && nowTime.Minute == 59 && nowTime.Second > 50)
                    {
                        isCrossDay = true;
                        #region ----跨天处理----
                        //判断数据处理线程数据是否处理完成,处理完成才继续进行跨天处理  20170703
                        while (DataProcHandle.Instance.GetArriveDataCount() > 0)
                        {
                            Thread.Sleep(200);
                        }
                        //处理时已跨天  输出日志  20170703
                        if (DateTime.Now.Day != nowTime.Day)
                        {
                            Basic.Framework.Logging.LogHelper.Error("处理跨天数据时,时间已到第二天!");
                        }

                        //DateTime nowTime = time;
                        //nowTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 23, 59, 59);

                        //DateTime tomorrowTime = nowTime.AddDays(1);
                        //tomorrowTime = new DateTime(tomorrowTime.Year, tomorrowTime.Month, tomorrowTime.Day, 0, 0, 0);
                        CrossDay( new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 23, 59, 59),
                            new DateTime(nowTime.AddDays(1).Year, nowTime.AddDays(1).Month, nowTime.AddDays(1).Day, 0, 0, 0)); //2017.12.18 by  增加手动修改电脑日期处理时增加

                        while (true)
                        {
                            Thread.Sleep(200);
                            if (DateTime.Now.Hour != 23)
                            {
                                CreatDayTable(DateTime.Now);
                                break;
                            }
                        }

                        #endregion
                        isCrossDay = false;
                        nowTime = DateTime.Now; //正常跨天处理完成，将时间赋值到第二天
                    }
                    totalcount++;
                    if (totalcount >= 20)//每4秒记算一次馈电信息
                    {
                        totalcount = 0;
                        CheckAlarmFeeding();
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error("CrossDayAndFiveMinite Error:" + ex.Message);
                }
                Thread.Sleep(200);
            }
        }
        /// <summary>
        /// 定时，处理馈电记录,每2秒处理一次
        /// </summary>
        private void CheckAlarmFeeding()
        {
            //控制口不为空，并且upflag=0 为主控，控制量取断电失败和复电失败
            Jc_KdInfo kdInfo;
            DataTable dtpointcur = new DataTable();
            DataTable dtcontrolcur = new DataTable();
            DateTime etime = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            DateTime stime = Convert.ToDateTime(DateTime.Now.AddDays(-1.0f).ToShortDateString());
            DateTime dtimecur = new DateTime();
            string kzklst = "";
            string oldkdid = "", tempkdid = "", Curkdid = ""; ;
            int feedingtime = 60;//关于复电失败的只计算60秒以内的。
            byte upflag = 0;

            for (; stime <= etime; stime = stime.AddDays(1.0f))
            {
                dtpointcur = _Repository.GetAlarmFeedingList("KJ_DataAlarm" + stime.ToString("yyyyMM"), stime.ToString(), stime.AddDays(1.0f).ToShortDateString());
                if (dtpointcur != null)//找到当天控制口未空，并且upflag=0的记录
                {
                    for (int i = 0; i < dtpointcur.Rows.Count; i++)
                    {
                        dtimecur = DateTime.Parse(dtpointcur.Rows[i]["etime"].ToString());
                        kzklst = dtpointcur.Rows[i]["kzk"].ToString().Replace("|", "','");
                        oldkdid = dtpointcur.Rows[i]["kdid"].ToString();                      
                        upflag = 0;
                        if (dtimecur.Year == 1900)
                            dtcontrolcur = _Repository.GetAlarmFeedingControlList("KJ_DataAlarm" + stime.ToString("yyyyMM"),
                                dtpointcur.Rows[i]["stime"].ToString(), "", kzklst, false);
                        else
                        {
                            dtcontrolcur = _Repository.GetAlarmFeedingControlList("KJ_DataAlarm" + stime.ToString("yyyyMM"),
                                dtpointcur.Rows[i]["stime"].ToString(), dtpointcur.Rows[i]["etime"].ToString(), kzklst);
                            if ((DateTime.Now - dtimecur).TotalSeconds >= feedingtime) upflag = 1;
                        }
                        Curkdid = oldkdid;
                        for (int j = 0; j < dtcontrolcur.Rows.Count; j++)
                        {
                            tempkdid = dtcontrolcur.Rows[j]["id"].ToString();
                            if(!Curkdid.Contains(tempkdid))
                            {
                                if (Curkdid == "")
                                    Curkdid = tempkdid;
                                else
                                    Curkdid += "," + tempkdid;
                                ///更新馈电信息表
                                kdInfo = GetNewJCKDInfo(dtpointcur.Rows[i]["id"].ToString(), tempkdid, DateTime.Now);
                                addJCKDRecordToDB(kdInfo);
                            }
                        }
                        //以主控的ID，更新kdid和upflag
                        if (Curkdid != oldkdid || upflag == 1)
                        {
                            Jc_BInfo alarmInfo = new Jc_BInfo();
                            alarmInfo.Upflag = upflag.ToString();
                            alarmInfo.Kdid = Curkdid;
                            alarmInfo.ID = dtpointcur.Rows[i]["id"].ToString();
                            alarmInfo.Stime = (DateTime)(dtpointcur.Rows[i]["stime"]);

                            Dictionary<string, object> updateItems = new Dictionary<string, object>();
                            updateItems.Add("Upflag", alarmInfo.Upflag);
                            updateItems.Add("Kdid", alarmInfo.Kdid);

                            AlarmRecordUpdateProperitesRequest updateRequest = new AlarmRecordUpdateProperitesRequest()
                            {
                                AlarmInfo = alarmInfo,
                                UpdateItems = updateItems
                            };

                            UpdateAlarm(updateRequest);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 馈电信息表记录入库
        /// </summary>
        /// <param name="JCKD"></param>
        public static void addJCKDRecordToDB(Jc_KdInfo JCKD)
        {
            IInsertToDbService<Jc_KdInfo> kdDataTodbService= ServiceFactory.Create<IInsertToDbService<Jc_KdInfo>>();
            DataToDbAddRequest<Jc_KdInfo> dataToDbAddRequest = new DataToDbAddRequest<Jc_KdInfo>();
            dataToDbAddRequest.Item = JCKD;
            kdDataTodbService.AddItem(dataToDbAddRequest);
        }
        /// <summary>
        /// 生成馈电记录结构体
        /// </summary>
        /// <param name="BJID">报警ID</param>
        /// <param name="KDID">馈电ID</param>
        /// <param name="stime">报警开始时间</param>
        /// <returns></returns>
        private   Jc_KdInfo GetNewJCKDInfo(string BJID, string KDID, DateTime stime)
        {
            Jc_KdInfo jk = new Jc_KdInfo();
            jk.ID = IdHelper.CreateLongId().ToString();
            jk.BJID = BJID;
            jk.KDID = KDID;
            jk.Timer = stime;
            jk.InfoState = InfoState.AddNew;
            return jk;
        }
        private void UpdateAlarm(AlarmRecordUpdateProperitesRequest AlarmRecordRequest)
        {
            List<Jc_BInfo> alarmInfos = new List<Jc_BInfo>();
            string alarmDate = AlarmRecordRequest.AlarmInfo.Stime.ToString("yyyyMM");
            alarmInfos.Add(AlarmRecordRequest.AlarmInfo);

            var alarmModels = ObjectConverter.CopyList<Jc_BInfo, Jc_BModel>(alarmInfos);
            List<string> updateItems = GetUpdateColumns(AlarmRecordRequest.UpdateItems);
            updateItems.Add("ID");

            string[] updateColumns = updateItems.ToArray();

            if (updateColumns.Length > 0)
            {
                if (_Repository.BulkUpdate("KJ_DataAlarm" + alarmDate, alarmModels, BuildDataColumn(updateColumns), "ID"))
                {                   
                    //更新缓存
                    var updateRequest = new AlarmCacheUpdatePropertiesRequest
                    {
                        AlarmKey = AlarmRecordRequest.AlarmInfo.ID,
                        UpdateItems = AlarmRecordRequest.UpdateItems
                    };
                    _alarmCacheService.UpdateAlarmInfoProperties(updateRequest);                  
                }
            }
        }
        private List<string> GetUpdateColumns(Dictionary<string, object> updateItems)
        {
            List<string> updatecolumns = new List<string>();
            foreach (var item in updateItems)
            {
                if (!string.IsNullOrEmpty(item.Key))
                {
                    updatecolumns.Add(item.Key);
                }
            }

            return updatecolumns;
            //return updatecolumns.ToArray();
        }
        private DataColumn[] BuildDataColumn(string[] columnsName)
        {
            DataColumn[] dataColumns = new DataColumn[columnsName.Count()];
            for (int i = 0; i < columnsName.Count(); i++)
            {
                dataColumns[i] = new DataColumn();
                dataColumns[i].ColumnName = columnsName[i];
            }
            return dataColumns;
        }
        /// <summary>
        /// 创建日表（昨天、今天、明天）
        /// </summary>
        /// <param name="DataTime"></param>
        private void CreatDayTable(DateTime DataTime)
        {
            try
            {
                IPointDefineRepository pointDefineRepository = ServiceFactory.Create<IPointDefineRepository>();
                //跨天建表修改,由于存在月表,需要创建上个月及下个月的月表  20170703
                pointDefineRepository.ExecuteNonQuery("CreateDateTable", DataTime.AddMonths(-1).ToString("yyyyMM") + "01");//默认创建一次上个月一号的表,通过此方法来创建上月月表
                pointDefineRepository.ExecuteNonQuery("CreateDateTable", DataTime.AddMonths(1).ToString("yyyyMM") + "01");//默认创建一次下个月一号的表,通过此方法来创建下月月表

                pointDefineRepository.ExecuteNonQuery("CreateDateTable", DataTime.AddDays(-1).ToString("yyyyMMdd"));
                pointDefineRepository.ExecuteNonQuery("CreateDateTable", DataTime.ToString("yyyyMMdd"));
                pointDefineRepository.ExecuteNonQuery("CreateDateTable", DataTime.AddDays(+1).ToString("yyyyMMdd"));
            }
            catch (Exception ex)
            {
                LogHelper.Error("CreatDayTable Error:" + ex.Message);
            }
        }
        /// <summary>
        /// 跨天处理
        /// </summary>
        private void CrossDay(DateTime nowTime, DateTime tomorrowTime)
        {
            try
            {
                //DateTime nowTime = time;
                //nowTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 23, 59, 59);

                //DateTime tomorrowTime = nowTime.AddDays(1);
                //tomorrowTime = new DateTime(tomorrowTime.Year, tomorrowTime.Month, tomorrowTime.Day, 0, 0, 0);

                List<Jc_DefInfo> pointDefineItems;
                List<Jc_DevInfo> pointDevItems;
                Jc_DevInfo pointDevItem;
                IDriver driverObj = null;

                pointDefineItems = CacheDataHelper.GetAllSystemPointDefineItems();
                pointDefineItems = pointDefineItems.Where(a => a.DevPropertyID == (int)DeviceProperty.Substation).ToList();
                pointDevItems = CacheDataHelper.GetAllDevItems();

                foreach (Jc_DefInfo station in pointDefineItems)
                {
                    pointDevItem = pointDevItems.FirstOrDefault(a => a.Devid == station.Devid);
                    if (pointDevItem != null)
                    {
                        if (GlobleStaticVariable.driverHandle.DriverItems.ContainsKey(pointDevItem.Sysid))
                        {
                            driverObj = GlobleStaticVariable.driverHandle.DriverItems[pointDevItem.Sysid].DLLObj;
                            DriverTransferInterface.Drv_CrossDayPro(driverObj, station.Fzh, nowTime, tomorrowTime);
                        }
                    }
                }
                //人员定位报警表跨天处理  20171206
                KJ237CacheHelper.Drv_CrossDayPro(nowTime, tomorrowTime);
            }
            catch (Exception ex)
            {
                LogHelper.Error("CrossDay Error:" + ex.Message);
            }
        }

        #endregion
    }
}
