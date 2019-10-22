using Basic.Framework.Common;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;

using Sys.Safety.Model;
using Sys.Safety.Enums;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.Jc_Hour;
using Sys.Safety.Request.JC_Multiplesetting;
using Sys.Safety.ServiceContract;
using Sys.Safety.ServiceContract.Cache;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sys.Safety.Processing.DataProcessing
{
    /// <summary>
    /// 倍率报警 by 2017.7.26 by
    /// </summary>
  public  class RatioAlarmProcHandle
    {
        #region ----变量定义----
        
        private static IJC_MbRepository mbRepository = null;
        private static IRatioAlarmCacheService ratioAlarmCacheService = null;
        private static volatile RatioAlarmProcHandle getInstance;
        protected static readonly object obj = new object();

        /// <summary>
        /// 倍率报警基准值 key = point,value = 周平均值
        /// </summary>
        private Dictionary<string, double> BasisValueItems = new Dictionary<string, double>();

        /// <summary>
        /// 是否退出线程标记
        /// </summary>
        bool isStop = false;

        private Thread thread = null;

        /// <summary>
        /// 单位秒，线程休息时间
        /// </summary>
        int timeSpan = 20;
        #endregion

        #region ----对外接口----

        public static RatioAlarmProcHandle Instance
        {
            get
            {
                if (getInstance == null)
                {
                    lock (obj)
                    {
                        if (getInstance == null)
                        {
                            getInstance = new RatioAlarmProcHandle();
                        }
                    }
                }
                return getInstance;
            }
        }
        public bool Start()
        {
            mbRepository = ServiceFactory.Create<IJC_MbRepository>();
            ratioAlarmCacheService = ServiceFactory.Create<IRatioAlarmCacheService>();

            EndAllAlarmBySql();

            isStop = false;
            if (thread == null)
            {
                thread = new Thread(DataProc);
            }
            thread.Start();
            return true;
        }

        public bool Stop()
        {
            isStop = true;
            EndAllAlarmByCache(DateTime.Now);
            return true;
        }

        #endregion

        #region ----保存缓存操作（增、删、改、查）----

        private void StartAlarms(List<JC_MbInfo> alarmInfoItems)
        {
            //入库
            string[] colNameItems = { "Id", "PointID", "fzh", "kh", "dzh", "devid", "wzid", "point", "type", "bstj", "bsz", "stime", "etime", "ssz", "pjz" };
            System.Data.DataColumn[] cols = new System.Data.DataColumn[colNameItems.Length];
            for (int i = 0; i < cols.Length; i++)
            {
                cols[i] = new System.Data.DataColumn(colNameItems[i]);
            }
            alarmInfoItems.ForEach(item =>
            {
                JC_MbModel _jcmb = ObjectConverter.Copy<JC_MbInfo, JC_MbModel>(item);
                mbRepository.AddItemBySql(_jcmb);
            });
            //添加内存
            RatioAlarmCacheBatchAddRequest ratioAlarmCacheBatchAddRequest = new RatioAlarmCacheBatchAddRequest();
            ratioAlarmCacheBatchAddRequest.AlarmInfos = alarmInfoItems;
            ratioAlarmCacheService.BacthAddAlarmCache(ratioAlarmCacheBatchAddRequest);
        }
        private void EndAlarmItems(List<JC_MbInfo> alarmInfoItems)
        {
            //入库
            string[] colNameItems = { "zdz", "zdzs", "etime" };
            System.Data.DataColumn[] cols = new System.Data.DataColumn[colNameItems.Length];
            for (int i = 0; i < cols.Length; i++)
            {
                cols[i] = new System.Data.DataColumn(colNameItems[i]);
            }
            alarmInfoItems.ForEach(item =>
            {
                JC_MbModel _jcmb = ObjectConverter.Copy<JC_MbInfo, JC_MbModel>(item);
                mbRepository.UpdateItemBySql(_jcmb);
            });

            //删除内存
            RatioAlarmCacheBatchDeleteRequest ratioAlarmCacheBatchDeleteRequest = new RatioAlarmCacheBatchDeleteRequest();
            ratioAlarmCacheBatchDeleteRequest.AlarmInfos  =alarmInfoItems;
            ratioAlarmCacheService.BatchDeleteAlarmCache(ratioAlarmCacheBatchDeleteRequest);
        }
        private void UpdateAlarm(List< JC_MbInfo> alarmInfoItems)
        {
            //更新内存
            RatioAlarmCacheBatchUpdateRequest ratioAlarmCacheBatchUpdateRequest = new RatioAlarmCacheBatchUpdateRequest();
            ratioAlarmCacheBatchUpdateRequest.AlarmInfos = alarmInfoItems;
            ratioAlarmCacheService.BatchUpdateAlarmCache(ratioAlarmCacheBatchUpdateRequest);
        }

        /// <summary>
        /// 从内存中获取到该测点已有的报警信息
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private JC_MbInfo GetAlarmInfo(string point)
        {
            JC_MbInfo alarmInfo = null;

            try
            {
                List<JC_MbInfo> alarmInfoItems = null;
                RatioAlarmCacheGetByConditonRequest ratioAlarmCacheGetByConditonRequest = new RatioAlarmCacheGetByConditonRequest();
                ratioAlarmCacheGetByConditonRequest.Predicate = a => a.Point == point;
                var result = ratioAlarmCacheService.GetAlarmCache(ratioAlarmCacheGetByConditonRequest);
                if (result.Data != null && result.IsSuccess)
                {
                    alarmInfoItems = result.Data;
                    if (alarmInfoItems.Count == 0)
                    {
                        alarmInfo = null;
                    }
                    else if (alarmInfoItems.Count > 1)
                    {
                        alarmInfo = alarmInfoItems[0];
                        LogHelper.Error("RatioAlarmProcHandle GetAlarmInfo 发现异常，" + point + "同时存在多个倍数报警记录！");
                    }
                    else
                    {
                        alarmInfo = alarmInfoItems[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("RatioAlarmProcHandle GetAlarmInfo Error" + ex.Message);
            }

            return alarmInfo;
        }
        #endregion

        #region ----处理倍率报警定义信息操作----
        
        /// <summary>
        /// 获取倍数报警配置信息
        /// </summary>
        /// <returns></returns>
        private List<JC_MultiplesettingInfo> GetAllMultiplesettingInfo()
        {
            List<JC_MultiplesettingInfo> MultiplesettingItems = null;
            try
            {
                IJC_MultiplesettingService multiplesettingService = ServiceFactory.Create<IJC_MultiplesettingService>();
                JC_MultiplesettingGetListRequest multiplesettingGetListRequest = new JC_MultiplesettingGetListRequest();
                var reslut = multiplesettingService.GetMultiplesettingList(multiplesettingGetListRequest);
                if (reslut.Data != null && reslut.IsSuccess)
                {
                    MultiplesettingItems = reslut.Data;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("RatioAlarmProcHandle GetMultiplesettingInfo Error" + ex.Message);
            }
            return MultiplesettingItems;
        }

        /// <summary>
        /// 将数据库存储的文本转换成实体
        /// </summary>
        /// <param name="ratioAlarmConditions">0，0.2，5|0.2，0.3，3|0.3，0.4，2.5 ...</param>
        /// <returns></returns>
        private List<RatioAlarmConditions> GetRatioAlarmConditionsFromStr(JC_MultiplesettingInfo multiplesettingInfo)
        {
            List<RatioAlarmConditions> ratioAlarmConditionItems = null;
            try
            {
                RatioAlarmConditions ratioAlarmConditions = null;
                string[] strItems = multiplesettingInfo.MultipleText.Split('|');
                string[] items;
                if (strItems.Length == 10)
                {
                    ratioAlarmConditionItems = new List<RatioAlarmConditions>();
                    for (int i = 0; i < strItems.Length; i++)
                    {
                        items = strItems[i].Split(',');
                        if (items.Length == 3)
                        {
                            ratioAlarmConditions = new RatioAlarmConditions();
                            try
                            {
                                ratioAlarmConditions.minValue = Convert.ToDouble(items[0]);
                                ratioAlarmConditions.maxValue = Convert.ToDouble(items[1]);
                                ratioAlarmConditions.ratioValue = Convert.ToDouble(items[2]);
                                ratioAlarmConditions.alarmTyepe = i + 1;
                                if (ratioAlarmConditions.ratioValue != 0)
                                {
                                    ratioAlarmConditions.isExsit = true;
                                }
                                else
                                {
                                    ratioAlarmConditions.isExsit = false;
                                }
                            }
                            catch
                            {
                                ratioAlarmConditions.isExsit = false;
                            }
                        }
                        else
                        {
                            //定义的倍数报警条件存储有误，默认此逻辑不生效
                            ratioAlarmConditions = new RatioAlarmConditions();
                            ratioAlarmConditions.isExsit = false;

                            LogHelper.Error(multiplesettingInfo.Devid + "定义的倍数报警条件存储有误，存在条件" + strItems[i]);
                        }
                        ratioAlarmConditionItems.Add(ratioAlarmConditions);
                    }
                }
                else
                {
                    LogHelper.Error(multiplesettingInfo.Devid + "定义的倍数报警条件存储有误，共" + strItems.Length + "条");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(multiplesettingInfo.Devid + "定义的倍数报警条件有误，" + ex.Message);
            }
            return ratioAlarmConditionItems;
        }

        #endregion
                
        #region ----取计算基数相关操作----
        
        /// <summary>
        /// 重载所有测点分析基础值（周平均值）
        /// </summary>
        /// <param name="pointDefineItems"></param>
        private void ReloadBasicValues(List<Jc_DefInfo> pointDefineItems)
        {
            try
            {
                Stopwatch test = new Stopwatch();
                test.Start();

                DateTime nowTime = DateTime.Now;
                DateTime etime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 0, 0, 0);
                DateTime stime = etime.AddDays(-7);

                List<string> pointItems = new List<string>();
                foreach (Jc_DefInfo def in pointDefineItems)
                {
                    pointItems.Add(def.Point);
                }

                DataTable dt = GetHourData(stime, etime, pointItems);
                DataRow[] dr;
                int count = 0;
                double values = 0;
                double baseValue = 0;
                double tempValue = 0;
                foreach (Jc_DefInfo def in pointDefineItems)
                {
                    dr = dt.Select("point = '" + def.Point + "'");
                    if (dr.Length > 0)
                    {
                        count = 0;
                        values = 0;
                        baseValue = 0;
                        for (int x = 0; x < dr.Length; x++)
                        {
                            tempValue =  Convert.ToDouble(dr[x]["pjz"]);
                            if (tempValue > 0)
                            {
                                count++;
                                values += tempValue;
                            }
                        }
                        if (count > 0)
                        {
                            baseValue = Math.Round(values / count, 3);
                        }
                    }
                    else
                    {
                        baseValue = 0;
                    }

                    if (BasisValueItems.ContainsKey(def.Point))
                    {
                        BasisValueItems[def.Point] = baseValue;
                    }
                    else
                    {
                        BasisValueItems.Add(def.Point, baseValue);
                    }
                }
                test.Stop();
                LogHelper.Info("ReloadBasicValues 耗时" + test.ElapsedMilliseconds + ",数据量：" + dt.Rows.Count);
            }
            catch (Exception ex)
            {
                LogHelper.Error("RatioAlarmProcHandle ReloadBasicValues Error" + ex.Message);
            }
        }
        /// <summary>
        /// 计算单个测点的分析基础值
        /// </summary>
        /// <param name="pointid"></param>
        /// <returns></returns>
        private Jc_HourInfo GetHourInfoByPointID(string pointid)
        {
            Jc_HourInfo hourInfo = null;
            try
            {
                IJc_HourService jc_HourService = ServiceFactory.Create<IJc_HourService>();
                Jc_HourGetRequest jc_HourGetRequest = new Jc_HourGetRequest();
                jc_HourGetRequest.PointId = pointid;
                var result = jc_HourService.GetWeekAverageValueByPointId(jc_HourGetRequest);
                if (result.Data != null && result.IsSuccess)
                {
                    hourInfo = result.Data;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("RatioAlarmProcHandle GetBasicValues Error" + ex.Message);
            }
            return hourInfo;
        }

        /// <summary>
        /// 从内存中获取当前测点的分析基数（周平均值）
        /// </summary>
        /// <param name="def"></param>
        /// <returns></returns>
        private double GetBasicValueByDef(Jc_DefInfo def)
        {
            double basicValue = 0;
            if (BasisValueItems.ContainsKey(def.Point))
            {
                basicValue = BasisValueItems[def.Point];
            }
            else
            {
                Jc_HourInfo hourInfo = GetHourInfoByPointID(def.PointID);
                if (hourInfo == null)
                {
                    basicValue = 0;
                }
                else
                {
                    basicValue = hourInfo.Pjz;
                }
                BasisValueItems.Add(def.Point, basicValue);
            }
            return basicValue;
        }

        private DataTable GetHourData(DateTime stime, DateTime etime, List<string> pointItems)
        {
            DataTable dt = new DataTable();
            try
            {
                 IJc_HourRepository repository = ServiceFactory.Create<IJc_HourRepository>();
                if (stime.Month == etime.Month)
                {
                  dt =  repository.GetJC_HourDataByTimer(stime, etime, pointItems);
                }
                else
                {
                    DateTime endTime = new DateTime(etime.Year, etime.Month, 1, 23, 59, 59).AddDays(-1);
                    DateTime startTime = new DateTime(etime.Year, etime.Month, 1, 0, 0, 0) ;
                    DataTable dt1 = repository.GetJC_HourDataByTimer(stime, endTime, pointItems);
                    DataTable dt2 = repository.GetJC_HourDataByTimer(startTime, etime, pointItems);
                    //添加DataTable1的数据
                    object[] obj = new object[dt1.Columns.Count];
                    dt = dt1.Clone();
                    if (dt1 != null)
                    {
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            dt1.Rows[i].ItemArray.CopyTo(obj, 0);
                            dt.Rows.Add(obj);
                        }
                    }
                    if (dt2 != null)
                    {
                        //添加DataTable2的数据
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            dt2.Rows[i].ItemArray.CopyTo(obj, 0);
                            dt.Rows.Add(obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("RatioAlarmProcHandle GetHourData Error" + ex.Message);
            }
            return dt;
        }

        #endregion

        #region ----业务相关操作----
       
        private void DataProc()
        {
            List<Jc_DefInfo> pointDefineItems = null;
            SettingInfo settingInfo = null;
            string settingTime = "";    //上次加载倍率报警配置时间
            List<JC_MultiplesettingInfo> multiplesettingInfotems = null;
            JC_MultiplesettingInfo multiplesettingInfotem = null;
            List<RatioAlarmConditions> ratioAlarmConditionItems = null;
            RatioAlarmConditions ratioAlarmConditionItem = null;
            double sszValue = 0;
            double basicValue = 0;
            int alarmType = 0;
            JC_MbInfo ratioAlarm = null;
            DateTime getBasicValueDate = DateTime.Now.AddDays(-1);  //上次获取周平均值的时间
            List<JC_MbInfo> alarm_AddItems = new List<JC_MbInfo>();
            List<JC_MbInfo> alarm_DelItems = new List<JC_MbInfo>();
            List<JC_MbInfo> alarm_UpdateItems = new List<JC_MbInfo>();
            
            DateTime lastProcData = DateTime.Now;   //最后一次处理数据的日期
            DateTime nowTime = DateTime.Now;    //本次处理时间
            for (; ; )
            {
                try
                {
                    nowTime = DateTime.Now;
                    if (isStop) { break; }
                    //获取定义信息
                    pointDefineItems = CacheDataHelper.GetKJPointDefineItems();
                    
                    //判断是否重载倍率报警定义
                    settingInfo = CacheDataHelper.GetSettingByKeyStr("MultipleSettingUpdateTime");
                    if (settingInfo != null)
                    {
                        if (settingTime != settingInfo.StrValue)
                        {
                            //重新加载倍率报警定义信息
                            multiplesettingInfotems = GetAllMultiplesettingInfo();
                            settingTime = settingInfo.StrValue;
                        }
                    }
                    else
                    {
                        Thread.Sleep(timeSpan * 1000);
                        continue;
                    }

                    //筛选模拟量
                    pointDefineItems = pointDefineItems.Where(a => (a.DevPropertyID == (int)DeviceProperty.Analog)
                        && (multiplesettingInfotems.FirstOrDefault(b => b.Devid == a.Devid) != null)).ToList();

                    if (pointDefineItems.Count == 0) { Thread.Sleep(timeSpan * 1000); continue; }


                    //重载周平均值
                    if (getBasicValueDate.Date != nowTime.Date)
                    {
                        ReloadBasicValues(pointDefineItems);
                        getBasicValueDate = nowTime;
                    }

                    if (pointDefineItems == null) { Thread.Sleep(timeSpan * 1000); continue; }
                    if (pointDefineItems.Count == 0) { Thread.Sleep(timeSpan * 1000); continue; }

                   

                    alarm_AddItems = new List<JC_MbInfo>();
                    alarm_DelItems = new List<JC_MbInfo>();
                    alarm_UpdateItems = new List<JC_MbInfo>();

                    if (lastProcData.Date != nowTime.Date)
                    {
                        //结束报警重新生成
                        CrossDay(pointDefineItems, multiplesettingInfotems, new DateTime(lastProcData.Year, lastProcData.Month, lastProcData.Day, 23, 59, 59), new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 0, 0, 0));
                        lastProcData = nowTime;
                    }

                    foreach (Jc_DefInfo def in pointDefineItems)
                    {
                        if (isStop) { break; }
                        if (double.TryParse(def.Ssz, out sszValue))
                        {
                            multiplesettingInfotem = multiplesettingInfotems.FirstOrDefault(a => a.Devid == def.Devid);
                            if (multiplesettingInfotem == null) { continue; }//未定义该设备类型的倍率报警
                            ratioAlarmConditionItems = GetRatioAlarmConditionsFromStr(multiplesettingInfotem);  //解析倍率报警配置
                            if (ratioAlarmConditionItems == null) { continue; }//提取倍率报警定义出错，不继续计算
                            basicValue = GetBasicValueByDef(def);   //获取周平均值
                            if (basicValue == 0) { continue; }  //没有基础值，不计算
                            //计算报警等级
                            alarmType = JudgeAlarmType(sszValue, ratioAlarmConditionItems, basicValue);
                            if (alarmType != 0)
                            {
                                ratioAlarmConditionItem = ratioAlarmConditionItems[alarmType - 1];  //获取当前报警的倍率信息配置
                            }
                            ratioAlarm = GetAlarmInfo(def.Point);
                            #region ----报警处理----

                            if (ratioAlarm == null)
                            {
                                if (alarmType > 0)
                                {
                                    //之前没有报警 现在有报警（新增报警）
                                    ratioAlarm = new JC_MbInfo();
                                    ratioAlarm.Id = IdHelper.CreateLongId().ToString();
                                    ratioAlarm.Point = def.Point;
                                    ratioAlarm.PointID = def.PointID;
                                    ratioAlarm.Fzh = def.Fzh.ToString();
                                    ratioAlarm.Kh = def.Kh.ToString();
                                    ratioAlarm.Dzh = def.Dzh.ToString();
                                    ratioAlarm.Devid = def.Devid;
                                    ratioAlarm.Wzid = def.Wzid;
                                    ratioAlarm.Type = alarmType.ToString();
                                    ratioAlarm.Bstj = "[" + ratioAlarmConditionItem.minValue + "," + ratioAlarmConditionItem.maxValue + "," + ratioAlarmConditionItem.ratioValue + "]";
                                    ratioAlarm.Bsz = ratioAlarmConditionItem.ratioValue;
                                    ratioAlarm.Stime = nowTime;
                                    ratioAlarm.Ssz = Convert.ToDouble(def.Ssz);
                                    ratioAlarm.Pjz = basicValue;
                                    ratioAlarm.Zdz = sszValue;
                                    ratioAlarm.Zdzs = nowTime;

                                    alarm_AddItems.Add(ratioAlarm); 
                                }
                            }
                            else
                            {
                                if (alarmType == 0)
                                {
                                    //之前有报警 现在没有报警(结束报警)
                                    ratioAlarm.Etime = nowTime;
                                    alarm_DelItems.Add(ratioAlarm);
                                }
                                else
                                {
                                    if (alarmType.ToString() != ratioAlarm.Type)
                                    {
                                        //之前有报警 报警切换（结束旧报警，生成新报警）
                                        ratioAlarm.Etime = nowTime;
                                        alarm_DelItems.Add(ratioAlarm);

                                        //生成新报警
                                        ratioAlarm = new JC_MbInfo();
                                        ratioAlarm.Id = IdHelper.CreateLongId().ToString();
                                        ratioAlarm.Point = def.Point;
                                        ratioAlarm.PointID = def.PointID;
                                        ratioAlarm.Fzh = def.Fzh.ToString();
                                        ratioAlarm.Kh = def.Kh.ToString();
                                        ratioAlarm.Dzh = def.Dzh.ToString();
                                        ratioAlarm.Devid = def.Devid;
                                        ratioAlarm.Wzid = def.Wzid;
                                        ratioAlarm.Type = alarmType.ToString();
                                        ratioAlarm.Bstj = "[" + ratioAlarmConditionItem.minValue + "," + ratioAlarmConditionItem.maxValue + "," + ratioAlarmConditionItem.ratioValue + "]";
                                        ratioAlarm.Bsz = ratioAlarmConditionItem.ratioValue;
                                        ratioAlarm.Stime = nowTime;
                                        ratioAlarm.Ssz = Convert.ToDouble(def.Ssz);
                                        ratioAlarm.Pjz = basicValue;
                                        ratioAlarm.Zdz = sszValue;
                                        ratioAlarm.Zdzs = nowTime;

                                        alarm_AddItems.Add(ratioAlarm);
                                    }
                                    else
                                    {
                                        //报警状态持续，更新报警期间最大值
                                        if (sszValue > ratioAlarm.Zdz)
                                        {
                                            ratioAlarm.Zdz = sszValue;
                                            ratioAlarm.Zdzs = nowTime;
                                            //更新报警
                                            alarm_UpdateItems.Add(ratioAlarm);
                                        }
                                    }
                                }
                            }

                            #endregion
                        }
                    }
                    if (alarm_DelItems.Count > 0)
                    {
                        EndAlarmItems(alarm_DelItems);
                    }
                    if (alarm_AddItems.Count > 0)
                    {
                        StartAlarms(alarm_AddItems);
                    }
                    if (alarm_UpdateItems.Count > 0)
                    {
                        UpdateAlarm(alarm_UpdateItems);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error("RatioAlarmProcHandle DataProc Error" + ex.Message);
                }
                Thread.Sleep(timeSpan * 1000);
            }
           
        }

        /// <summary>
        /// 当前报警等级分析
        /// </summary>
        /// <param name="ssz">当前实时值</param>
        /// <param name="ratioAlarmConditionItems">已定义的判断条件</param>
        /// <param name="basicValue">分析基数（周平均值）</param>
        /// <returns></returns>
        private int JudgeAlarmType(double ssz, List<RatioAlarmConditions> ratioAlarmConditionItems, double basicValue)
        {
            int alarmType = 0;

            try
            {
                RatioAlarmConditions ratioAlarmConditions = null;
                for (int i = ratioAlarmConditionItems.Count - 1; i >= 0; i--)
                {
                    ratioAlarmConditions = ratioAlarmConditionItems[i];
                    if (!ratioAlarmConditions.isExsit) { continue; }

                    if (basicValue >= ratioAlarmConditions.minValue && basicValue <= ratioAlarmConditions.maxValue)
                    {
                        if (ssz >= basicValue * ratioAlarmConditions.ratioValue)
                        {
                            alarmType = i + 1;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("RatioAlarmProcHandle GetAlarmType Error" + ex.Message);
            }

            return alarmType;
        }

        /// <summary>
        /// 结束所有内存报警，在线程stop前执行此操作
        /// </summary>
        private void EndAllAlarmByCache(DateTime etime)
        {
            List<JC_MbInfo> alarmItems = new List<JC_MbInfo>();
            RatioAlarmCacheGetAllRequest ratioAlarmCacheGetAllRequest = new RatioAlarmCacheGetAllRequest();
            var result = ratioAlarmCacheService.GetAllAlarmCache(ratioAlarmCacheGetAllRequest);
            if (result.Data != null && result.IsSuccess)
            {
                alarmItems = result.Data;
                alarmItems.ForEach(a =>
                {
                    a.Etime = etime;
                });
                EndAlarmItems(result.Data);
            }
        }
        /// <summary>
        /// 结束所有数据库报警，在线程start前执行此操作
        /// </summary>
        private void EndAllAlarmBySql()
        {
            try
            {
                mbRepository.ClearDbNoEndAlarmBySql();
            }
            catch (Exception ex)
            {
                LogHelper.Error("RatioAlarmProcHandle EndAllAlarmBySql Error"+ex.Message);
            }
        }

        /// <summary>
        /// 跨天处理，结束旧报警，生成新报警
        /// </summary>
        private void CrossDay(List<Jc_DefInfo> pointItems,List<JC_MultiplesettingInfo> multiplesettingInfotems,DateTime etime ,DateTime stime)
        {
            JC_MbInfo jcmb = null;
            JC_MbInfo tempJcmb = null;
            double basicValue = 0;
            double ssz = 0;
            List<JC_MbInfo> delItems = new List<JC_MbInfo>();
            List<JC_MbInfo> addItems = new List<JC_MbInfo>();
            JC_MultiplesettingInfo multiplesettingInfotem;
             RatioAlarmConditions ratioAlarmConditionItem;
             List<RatioAlarmConditions> ratioAlarmConditionItems = null;
             foreach (Jc_DefInfo def in pointItems)
             {
                 jcmb = GetAlarmInfo(def.Point);
                 if (jcmb != null)
                 {
                     jcmb.Etime = etime;
                     delItems.Add(jcmb);

                     multiplesettingInfotem = multiplesettingInfotems.FirstOrDefault(a => a.Devid == def.Devid);
                     if (multiplesettingInfotem == null) { continue; }//未定义该设备类型的倍率报警
                     ratioAlarmConditionItems = GetRatioAlarmConditionsFromStr(multiplesettingInfotem);
                     if (ratioAlarmConditionItems == null) { continue; }//提取倍率报警定义出错，不继续计算
                     basicValue = GetBasicValueByDef(def);
                     if (basicValue == 0) { continue; }  //没有基础值，不计算
                     ratioAlarmConditionItem = ratioAlarmConditionItems[Convert.ToInt32(jcmb.Type) - 1];
                     double.TryParse(def.Ssz, out ssz);
                     tempJcmb = JCMB_Copy(jcmb, ratioAlarmConditionItem, stime, ssz, basicValue);
                    
                     addItems.Add(tempJcmb);
                 }
             }
            EndAlarmItems(delItems);
            StartAlarms(addItems);
        }

        private JC_MbInfo JCMB_Copy(JC_MbInfo jcmb, RatioAlarmConditions ratioAlarmConditionItem, DateTime stime, double ssz, double basicValue)
        {
            JC_MbInfo newJcmb = new JC_MbInfo();

            newJcmb.Id = IdHelper.CreateLongId().ToString();
            newJcmb.Point = jcmb.Point;
            newJcmb.PointID = jcmb.PointID;
            newJcmb.Fzh = jcmb.Fzh;
            newJcmb.Kh = jcmb.Kh;
            newJcmb.Dzh = jcmb.Dzh;
            newJcmb.Devid = jcmb.Devid;
            newJcmb.Wzid = jcmb.Wzid;
            newJcmb.Type = jcmb.Type;
            newJcmb.Bstj = "[" + ratioAlarmConditionItem.minValue + "," + ratioAlarmConditionItem.maxValue + "," + ratioAlarmConditionItem.ratioValue + "]";
            newJcmb.Bsz = ratioAlarmConditionItem.ratioValue;
            newJcmb.Stime = stime;
            newJcmb.Etime = new DateTime(1900, 1, 1, 0, 0, 0);
            newJcmb.Ssz = ssz;
            newJcmb.Zdz = ssz;
            newJcmb.Pjz = basicValue;
            newJcmb.Zdzs = stime;
            return newJcmb;
        }
        #endregion
    }

    /// <summary>
    /// 报警配置类
    /// </summary>
    public class RatioAlarmConditions
    {
        /// <summary>
        /// 是否是可用条件
        /// </summary>
        public bool isExsit;
        /// <summary>
        /// 阀值上限
        /// </summary>
        public double maxValue;
        /// <summary>
        /// 阀值下限
        /// </summary>
        public double minValue;
        /// <summary>
        /// 倍率
        /// </summary>
        public double ratioValue;
        /// <summary>
        /// 报警等级
        /// </summary>
        public int alarmTyepe;
    }
}
