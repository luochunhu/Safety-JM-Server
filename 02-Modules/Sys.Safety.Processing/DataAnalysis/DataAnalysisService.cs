using Basic.Framework.Common;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.JC_Largedataanalysislog;
using Sys.Safety.Cache.BigDataAnalysis;
using Sys.Safety.Processing.DataAnalysis.Notification;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Sys.Safety.Request.ManualCrossControl;

namespace Sys.Safety.Processing.DataAnalysis
{
    /// <summary>
    /// 作者:
    /// 时间:2017-05-26
    /// 描述:数据分析模块
    /// 逻辑描述:
    /// 
    /// 修改记录:
    /// </summary>
    public class DataAnalysisService
    {
        private volatile static DataAnalysisService _instance = null;
        private static readonly object lockHelper = new object();
        private static readonly object listLock = new object();
        private static readonly object threadPoolLock = new object();
        private static readonly object addLogLock = new object();
        public bool IsRunning = false;
        private bool isCallForStop = false;
        private DataAnalysisService() { }

        class PointAnalysisInfo
        {
            public string PointId { get; set; }
            public string Point { get; set; }

            public string Wz { get; set; }
        }

        public static DataAnalysisService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockHelper)
                    {
                        if (_instance == null)
                        {
                            _instance = new DataAnalysisService();
                            _instance.dataAnalysisNotificationService.RegisterAnalysisReulstHandlerEvent(_instance);
                        }
                    }
                }
                return _instance;
            }
        }
        /// <summary>
        /// 分析结果输出日志服务
        /// </summary>
        private ILargedataAnalysisLogService analysisLogService = ServiceFactory.Create<ILargedataAnalysisLogService>();
        /// <summary>
        /// 数据分析通知模块
        /// </summary>
        private DataAnalysisNotificationService dataAnalysisNotificationService = DataAnalysisNotificationService.Instance;
        /// <summary>
        /// 因子Lua计算服务
        /// </summary>
        private FactorLuaService factorLuaService = FactorLuaService.CreateService();
        /// <summary>
        /// 因子计算服务
        /// </summary>
        private FactorCalculateService factorCalculateService = FactorCalculateService.CreateService();
        /// <summary>
        /// 报警处理服务
        /// </summary>
        private IAlarmHandleService alarmHandleService = ServiceFactory.Create<IAlarmHandleService>();
        /// <summary>
        /// 线程列表, 每个分析模型启动一个线程实时分析。
        /// </summary>
        private Dictionary<string, Thread> threadPool = new Dictionary<string, Thread>();
        /// <summary>
        /// 表达式实时分析结果列表
        /// </summary>
        private List<ExpressionRealTimeResultInfo> expressionRealTimeResultList = new List<ExpressionRealTimeResultInfo>();
        public event DataAnalysisNotificationDelegate AnalysisReulstHandlerEvent;

        private ILargeDataAnalysisCacheClientService largeDataAnalysisCacheClientService = ServiceFactory.Create<ILargeDataAnalysisCacheClientService>();

        /// <summary>
        /// 本地日志自动清除线程
        /// </summary>
        private static Thread log4netClearThread;

        /// <summary>
        /// 开始数据分析服务.
        /// </summary>
        /// <returns></returns>
        public void Start()
        {
            LogHelper.SystemInfo("开始启动数据分析服务模块");
            if (IsRunning)
            {
                LogHelper.SystemInfo("数据分析服务模块已启动");
                return;
            }
            //加载分析模型
            AnalysisModelChangeNotification.Instance.Start();
            //加载测点缓存
            PointChangeNotification.Instance.Start();
            //加载分析模型输出配置
            OutPutConfigChangeNotification.Instance.Start();
            //加载模拟量历史数据
            HistoryDataCacheNotification.Instance.Start();
            HistoryDataHandler();//启动服务时关闭未关闭的报警处理记录.
            IsRunning = true;//运行状态标志位True
            threadPool.Clear();
            OpenThreadToDoWork();

            //增加本地日志自动清除功能  20170910
            log4netClearThread = new Thread(ClearLog4netLog);
            log4netClearThread.IsBackground = true;
            log4netClearThread.Start();

            LogHelper.SystemInfo("启动数据分析服务模块成功");
        }

        private void OpenThreadToDoWork()
        {
            IManualCrossControlService manualCrossControlService = ServiceFactory.Create<IManualCrossControlService>();
            //数据分析线程
            var startDataAnalysisThread = new Thread(new ThreadStart(() =>
            {
                do
                {
                    try
                    {
                        StartDataAnalysis();//开始数据分析
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(string.Format("分析出现错误! 错误消息:{0}", ex.StackTrace));
                    }
                    finally
                    {
                        Thread.Sleep(5000);
                    }
                } while (IsRunning);
            }));
            startDataAnalysisThread.IsBackground = true;
            startDataAnalysisThread.Start();

            //分析结果处理线程
            var asyncAnalysisHandlerThread = new Thread(new ThreadStart(() =>
            {
                do
                {
                    var analysisConfigInfoList = AnalysisModelCache.Instance.Query();
                    for (int i = 0; IsRunning && null != analysisConfigInfoList && i < analysisConfigInfoList.Count; i++)
                    {
                        if (AnalysisReulstHandlerEvent != null)
                        {
                            try
                            {
                                AnalysisReulstHandlerEvent(new AnalysisChangedEventArgs() { AnalysisConfig = analysisConfigInfoList[i] });
                            }
                            catch (Exception ex)
                            {
                                LogHelper.Error(string.Format("处理分析模型:{0}的输出出错. 错误消息:{1}", analysisConfigInfoList[i].Name, ex.StackTrace));
                            }
                        }
                        Thread.Sleep(100);
                    }
                    try
                    {
                        //判断，如果分级报警交叉控制中存在已删除的模型，则解除控制  20180919
                        //删除已经结束的分级报警传感器  20180919                    
                        List<Jc_JcsdkzInfo> updatejckzinfos = new List<Jc_JcsdkzInfo>();
                        List<Jc_JcsdkzInfo> manualCrossControlList = manualCrossControlService.GetAllManualCrossControl().Data.FindAll(a => a.Upflag != "0");
                        foreach (Jc_JcsdkzInfo manualCrossControl in manualCrossControlList)
                        {
                            bool isInanalysisConfigInfo = false;
                            if (analysisConfigInfoList.FindAll(a => a.Id == manualCrossControl.ZkPoint).Count > 0)
                            {
                                isInanalysisConfigInfo = true;
                            }
                            if (!isInanalysisConfigInfo)//如果模型已经删除，则解除传感器分级报警  20180919
                            {
                                manualCrossControl.Upflag = "0";
                                updatejckzinfos.Add(manualCrossControl);
                            }
                        }
                        if (updatejckzinfos.Count > 0)
                        {
                            ManualCrossControlsRequest updaterequest = new ManualCrossControlsRequest();
                            updaterequest.ManualCrossControlInfos = updatejckzinfos;
                            manualCrossControlService.UpdateManualCrossControls(updaterequest);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    Thread.Sleep(500);
                } while (IsRunning);

            }));
            asyncAnalysisHandlerThread.IsBackground = true;
            asyncAnalysisHandlerThread.Start();
        }

        /// <summary>
        /// 停止数据分析服务.
        /// </summary>
        public void Stop()
        {
            LogHelper.SystemInfo("开始停止数据分析服务模块");
            if (IsRunning)
            {
                isCallForStop = true;
            }
            LogHelper.SystemInfo("等待分析线程处理完成");
            while (threadPool.Count > 0)
            {
                isCallForStop = true;
                Thread.Sleep(500);//等500毫秒看看分析线程有没有退完.
            }
            HistoryDataHandler();//停止服务时关闭未关闭的报警处理记录.       
            IsRunning = false;
            isCallForStop = false;
            AnalysisModelChangeNotification.Instance.Stop();
            PointChangeNotification.Instance.Stop();
            OutPutConfigChangeNotification.Instance.Stop();
            HistoryDataCacheNotification.Instance.Stop();
            LogHelper.SystemInfo("停止数据分析服务模块成功");
        }

        /// <summary>
        /// 重启数据分析服务
        /// </summary>
        public void ReStart()
        {
            if (IsRunning)
                Stop();
            if (!IsRunning)
                Start();
        }

        private List<PointAnalysisInfo> GetPointByDevType(int devTypeId)
        {
            return PointCache.Instance.Query(q => q.DevClassID == devTypeId, false).Select(q => new PointAnalysisInfo() { Point = q.Point, PointId = q.PointID, Wz = q.Wz }).ToList();
        }

        public void RegisterAnalysisModelChangedEvent(AnalysisModelChangeNotification analysisModelChangeNotification)
        {
            analysisModelChangeNotification.AnalysisModelChangedEvent += DataAnalysisService_AnalysisModelChangedEvent;
        }

        #region 分析模型添加，修改，删除 处理
        private void DataAnalysisService_AnalysisModelChangedEvent(AnalysisChangedEventArgs args)
        {
            var changedAnalysisModel = args.AnalysisConfig;
            changedAnalysisModel.AnalysisResult = 0;
            changedAnalysisModel.AnalysisTime = null;
            changedAnalysisModel.ExpressionRealTimeResultList = new List<ExpressionRealTimeResultInfo>();
            changedAnalysisModel.AnalysisSuccessfulPointList.Clear();
            if (args.Action == "Add")
            {
                AnalysisModelCache.Instance.AddItem(changedAnalysisModel);
            }
            if (args.Action == "Update")
            {
                AnalysisModelCache.Instance.UpdateItem(changedAnalysisModel);
            }
            if (args.Action == "Delete")
            {
                AnalysisModelCache.Instance.DeleteItem(changedAnalysisModel);
            }
        }
        #endregion

        /// <summary>
        /// 开始数据分析
        /// </summary>
        private void StartDataAnalysis()
        {
            var analysisConfigInfoList = AnalysisModelCache.Instance.Query();
            for (int i = 0; IsRunning && null != analysisConfigInfoList && i < analysisConfigInfoList.Count; i++)
            {
                if (!analysisConfigInfoList[i].AnalysisTime.HasValue
                    || (DateTime.Now - analysisConfigInfoList[i].AnalysisTime.Value).TotalSeconds >= analysisConfigInfoList[i].AnalysisInterval)
                {
                    try
                    {
                        Monitor.Enter(threadPoolLock);
                        bool isPrevNotDone = threadPool.ContainsKey(analysisConfigInfoList[i].Id);
                        //如果前一次的分析还没有完成跳过这一次的分析.
                        if (isPrevNotDone)
                            continue;
                        //绑定类型的分析模型排队分析.
                        if (threadPool.Count > 0 && analysisConfigInfoList[i].AnalysisModelPointRecordInfoList.FirstOrDefault().DevTypeId >= 0)
                        {
                            Thread.Sleep(1);
                            i--;
                            continue;
                        }
                        threadPool.Add(analysisConfigInfoList[i].Id, CreateAnalysisThread());
                        Thread specifiedThread;
                        threadPool.TryGetValue(analysisConfigInfoList[i].Id, out specifiedThread);
                        if (specifiedThread != null)
                            specifiedThread.Start(analysisConfigInfoList[i]);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(string.Format("创建分析线程或是分析过程出错! 错误消息:{0}", ex.StackTrace));
                    }
                    finally
                    {
                        Monitor.Exit(threadPoolLock);
                    }
                }
            }
        }

        /// <summary>
        /// 创建分析线程
        /// </summary>
        /// <returns>分析线程</returns>
        private Thread CreateAnalysisThread()
        {           
            Thread newAnalysisThread = new Thread(new ParameterizedThreadStart((item) =>
            {
                JC_LargedataAnalysisConfigInfo toDoItem = item as JC_LargedataAnalysisConfigInfo;
                if (toDoItem == null)
                    return;
                if (isCallForStop)
                {
                    lock (threadPoolLock)
                    {
                        if (threadPool.ContainsKey(toDoItem.Id))
                        {
                            threadPool.Remove(toDoItem.Id);
                            AnalysisModelCache.Instance.DeleteItem(toDoItem);
                        }
                    }
                    return;
                }
                try
                {
                    List<ExpressionRealTimeResultInfo> result = DataAnalysis(toDoItem);
                    AnalysisResultHandler(toDoItem, result);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(string.Format("分析模型:({0})分析出错! 错误消息:{1}", toDoItem.Name, ex.StackTrace));
                }
                lock (threadPoolLock)
                {
                    if (threadPool.ContainsKey(toDoItem.Id))
                    {
                        threadPool.Remove(toDoItem.Id);
                    }
                }
            }));
            return newAnalysisThread;
        }

        /// <summary>
        /// 处理分析结果
        /// </summary>
        /// <param name="dataAnalysisInfo">实时分析模型</param>
        /// <param name="expressionRealTimeResultInfoList">实时表达式分析结果列表</param>
        private void AnalysisResultHandler(JC_LargedataAnalysisConfigInfo dataAnalysisInfo, List<ExpressionRealTimeResultInfo> expressionRealTimeResultInfoList)
        {
            //赋值实时表达式分析结果列表
            dataAnalysisInfo.ExpressionRealTimeResultList = expressionRealTimeResultInfoList;
            //模型分析成功
            //bool isAnalysisSuccessful = expressionRealTimeResultInfoList.Exists(q => q.AnalysisResult == 2 && q.ActualContinueTime >= q.ContinueTime);
            bool isAnalysisSuccessful = expressionRealTimeResultInfoList.Exists(q => q.AnalysisResult == 2 && ((q.MaxContinueTime == 0 && q.ActualContinueTime >= q.ContinueTime) || (q.MaxContinueTime > 0 && q.ActualContinueTime >= q.ContinueTime && q.ActualContinueTime <= q.MaxContinueTime)));
            //模型分析不成功
            bool isAnalysisFail = !expressionRealTimeResultInfoList.Exists(q => q.AnalysisResult == 2);

            //if (isAnalysisSuccessful || isAnalysisFail)
            //{
            if (isAnalysisSuccessful)
            {
                dataAnalysisInfo.AnalysisResult = 2;
                dataAnalysisInfo.AnalysisResultText = dataAnalysisInfo.TrueDescription;
                dataAnalysisInfo.AnalysisTime = expressionRealTimeResultInfoList.FirstOrDefault(q => q.AnalysisResult == 2 && q.ActualContinueTime >= q.ContinueTime).LastAnalysisTime;
            }
            else
            {
                dataAnalysisInfo.AnalysisResult = expressionRealTimeResultInfoList.Exists(q => q.AnalysisResult == 1) ? 1 : 0;
                dataAnalysisInfo.AnalysisResultText = dataAnalysisInfo.AnalysisResult == 1 ? dataAnalysisInfo.FalseDescription : "未知";
                dataAnalysisInfo.AnalysisTime = expressionRealTimeResultInfoList.LastOrDefault().LastAnalysisTime;
                dataAnalysisInfo.AnalysisSuccessfulPointList.Clear();
            }

            try
            {
                //更新服务端实时缓存.
                largeDataAnalysisCacheClientService.UpdateLargeDataAnalysisConfigCahce(new LargeDataAnalysisConfigCacheUpdateRequest() { LargeDataAnalysisConfigInfo = dataAnalysisInfo });
            }
            catch (Exception ex)
            {
                LogHelper.Error(string.Format("更新实时缓存出错:{0}", ex.StackTrace));
            }

            //当前分析结果和上一次分析结果不同时
            if (dataAnalysisInfo.AnalysisResult != dataAnalysisInfo.PrevAnalysisResult || DateTime.Now > Convert.ToDateTime(dataAnalysisInfo.PrevAnalysisTime.ToString("yyyy-MM-dd 23:59:59")))
            {
                try
                {
                    Monitor.Enter(addLogLock);
                    //写入分析日志
                    analysisLogService.AddJC_Largedataanalysislog(new LargedataAnalysisLogAddRequest()
                    {
                        JC_LargedataAnalysisLogInfo = new JC_LargedataAnalysisLogInfo()
                        {
                            Id = IdHelper.CreateLongId().ToString(),
                            AnalysisModelId = dataAnalysisInfo.Id,
                            AnalysisResult = dataAnalysisInfo.AnalysisResult,
                            AnalysisTime = dataAnalysisInfo.AnalysisTime.Value,
                            IsDeleted = Enums.Enums.DeleteState.No,
                            Name = dataAnalysisInfo.Name,
                            StatusDescription = dataAnalysisInfo.AnalysisResult == 2 ? dataAnalysisInfo.TrueDescription
                                : dataAnalysisInfo.AnalysisResult == 0 ? "分析结果未知" : dataAnalysisInfo.FalseDescription
                        }
                    });
                }
                catch (Exception ex)
                {
                    LogHelper.Error(string.Format("处理分析结果>保存分析日志出错:{0}", ex.StackTrace));
                }
                finally
                {
                    Monitor.Exit(addLogLock);
                }
            }

            //更新上一次的分析结果
            //dataAnalysisInfo.PrevAnalysisResult = dataAnalysisInfo.AnalysisResult;
            //dataAnalysisInfo.PrevAnalysisTime = dataAnalysisInfo.AnalysisTime.Value;
            //更新内部缓存
            AnalysisModelCache.Instance.UpdateItem(dataAnalysisInfo);
            //}
        }

        /// <summary>
        /// 单个表达式分析
        /// </summary>
        /// <param name="dataAnalysisInfo">分析模型</param>
        /// <param name="expConfigList">表达式配置列表</param>
        /// <param name="returnResult">表达式分析结果</param>
        /// <param name="analysisPointId">绑定类型时每个类型下的测点ID</param>
        private void ExpressionAnalysis(JC_LargedataAnalysisConfigInfo dataAnalysisInfo, IGrouping<string, JC_SetAnalysisModelPointRecordInfo> expConfigList, List<ExpressionRealTimeResultInfo> returnResult, PointAnalysisInfo pointAnalysisInfo = null)
        {
            List<AnalysisSuccessfulPointInfo> expressionUsedPointList = new List<AnalysisSuccessfulPointInfo>();

            var firstPointBindingInfo = expConfigList.FirstOrDefault();
            string expressionId = firstPointBindingInfo.ExpressionId;
            string expression = firstPointBindingInfo.Expresstion;
            string expressionText = firstPointBindingInfo.Expresstion;
            int continueTime = firstPointBindingInfo.ContinueTime;
            int maxcontinueTime = firstPointBindingInfo.MaxContinueTime;
            if (pointAnalysisInfo != null)
                expressionId = string.Format("{0}_{1}", expressionId, pointAnalysisInfo.PointId);
            //是否因子值为未知
            bool isUnknowFactorValue = false;
            //获取表达式参数因子值
            foreach (var pointMapping in expConfigList)
            {
                string expressionConfigId = pointMapping.ExpressionConfigId;
                string pointId = pointAnalysisInfo == null ? pointMapping.PointId : pointAnalysisInfo.PointId;
                string pointCode = pointAnalysisInfo == null ? pointMapping.Point : pointAnalysisInfo.Point;
                string wz = pointAnalysisInfo == null ? pointMapping.Wz : pointAnalysisInfo.Wz;
                string callMethod = pointMapping.CallMethodName;
                if (!expressionUsedPointList.Any(q => q.PointId == pointId))
                {
                    expressionUsedPointList.Add(new AnalysisSuccessfulPointInfo() { PointId = pointId, Point = pointCode, Wz = wz });
                }
                object[] factorValues = null;
                try
                {
                    factorLuaService.RegisterFunction(factorCalculateService, callMethod);
                    factorValues = factorLuaService.CallFunction(callMethod, pointId);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(string.Format("通过Lua调用因子({0})服务接口({1})出错:{2}", pointId + ">>" + pointMapping.FactorName, callMethod, ex.StackTrace));
                }
                if (!isUnknowFactorValue)
                {
                    object factorGetValue = (factorValues == null || factorValues.Length <= 0) ? "Unknow" : (factorValues[0] as FactorValueInfo).Value;
                    isUnknowFactorValue = (factorGetValue.ToString() == "Unknow" || factorGetValue.ToString() == "未知") ? true : false;
                    //if (isUnknowFactorValue)
                    //    break;//因为要替换表达式文本为测点号->因子格式所以这里取消跳出循环了.
                    //获取到的因子值替换表达式中的表达式参数配置ID.
                    expression = expression.Replace(expressionConfigId, factorGetValue.ToString());
                }
                expressionText = expressionText.Replace(expressionConfigId, string.Format("{0}[{1}]->{2}", pointCode, wz, pointMapping.FactorName));
            }
            //默认表达式计算结果为0-未知
            int expressionCalculateResult = 0;
            if (!isUnknowFactorValue)
            {
                //计算表达式.
                object[] expressionLuaCalculateResult = null;
                try
                {
                    //表达式计算结果要么true要么false
                    expressionLuaCalculateResult = factorLuaService.ExecuteLuaScript(expression);
                    expressionCalculateResult = bool.Parse(expressionLuaCalculateResult[0].ToString()) ? 2 : 1;
                }
                catch (Exception ex)
                {
                    LogHelper.Error(string.Format("Lua计算表达式出错!表达式为:{0}, 错误消息:{1}", expressionText, ex.StackTrace));
                    //计算出错，表达式计算结果为未知
                    expressionCalculateResult = 0;
                }
            }
            lock (listLock)
            {
                //记录表达式实时分析结果
                ExpressionRealTimeResultInfo expressionRealTimeResultInfo = expressionRealTimeResultList.FirstOrDefault(q => q.AnalysisModelId == dataAnalysisInfo.Id && q.ExpressionId == expressionId);
                string analysisResultText = string.Empty;
                switch (expressionCalculateResult)
                {
                    case 1:
                        analysisResultText = dataAnalysisInfo.FalseDescription;
                        break;
                    case 2:

                        //表达式分析成立时,判断持续时间是否大于最大持续时间;如果大于持续时间，则置为分析不成立
                        //bool ismaxcontinuetime = false;
                        //if (expressionRealTimeResultInfo != null)
                        //{
                        //    int actualContinueTime = Convert.ToInt32((DateTime.Now - expressionRealTimeResultInfo.FirstSuccessfulTime).TotalSeconds);
                        //    if (maxcontinueTime > 0 && maxcontinueTime < actualContinueTime)
                        //        ismaxcontinuetime = true;
                        //}

                        //if (ismaxcontinuetime)
                        //{
                        //    analysisResultText = dataAnalysisInfo.FalseDescription;
                        //    expressionCalculateResult = 1;
                        //}
                        //else
                        //{
                        //    analysisResultText = dataAnalysisInfo.TrueDescription;
                        //    foreach (var item in expressionUsedPointList)
                        //    {
                        //        if (!dataAnalysisInfo.AnalysisSuccessfulPointList.Any(q => q.PointId == item.PointId))
                        //            dataAnalysisInfo.AnalysisSuccessfulPointList.Add(item);
                        //    }
                        //}


                        analysisResultText = dataAnalysisInfo.TrueDescription;
                        foreach (var item in expressionUsedPointList)
                        {
                            if (!dataAnalysisInfo.AnalysisSuccessfulPointList.Any(q => q.PointId == item.PointId))
                                dataAnalysisInfo.AnalysisSuccessfulPointList.Add(item);
                        }
                        //dataAnalysisInfo.AnalysisSuccessfulPointList.AddRange(expressionUsedPointList.Except(dataAnalysisInfo.AnalysisSuccessfulPointList));
                        break;
                    default:
                        analysisResultText = "未知";
                        break;
                }
                if (expressionRealTimeResultInfo == null)
                {
                    expressionRealTimeResultInfo = new ExpressionRealTimeResultInfo()
                    {
                        AnalysisModelId = dataAnalysisInfo.Id,
                        AnalysisModelName = dataAnalysisInfo.Name,
                        ExpressionId = expressionId,
                        Expression = expression,
                        ExpressionText = expressionText,
                        AnalysisResult = expressionCalculateResult,
                        AnalysisResultText = analysisResultText,
                        FirstSuccessfulTime = expressionCalculateResult == 2 ? DateTime.Now : DateTime.MinValue,
                        LastAnalysisTime = DateTime.Now,
                        ContinueTime = continueTime,
                        MaxContinueTime = maxcontinueTime,
                        ActualContinueTime = 0
                    };
                    expressionRealTimeResultList.Add(expressionRealTimeResultInfo);
                }
                else
                {
                    expressionRealTimeResultInfo.AnalysisResult = expressionCalculateResult;
                    expressionRealTimeResultInfo.AnalysisResultText = analysisResultText;
                    expressionRealTimeResultInfo.LastAnalysisTime = DateTime.Now;
                    expressionRealTimeResultInfo.ExpressionText = expressionText;
                    if (expressionCalculateResult == 2 && expressionRealTimeResultInfo.FirstSuccessfulTime == DateTime.MinValue)
                        expressionRealTimeResultInfo.FirstSuccessfulTime = DateTime.Now;
                    if (expressionCalculateResult == 2)
                    {
                        expressionRealTimeResultInfo.ActualContinueTime = Convert.ToInt64((expressionRealTimeResultInfo.LastAnalysisTime - expressionRealTimeResultInfo.FirstSuccessfulTime).TotalSeconds);
                    }
                    else
                    {
                        expressionRealTimeResultInfo.FirstSuccessfulTime = DateTime.MinValue;
                        expressionRealTimeResultInfo.ActualContinueTime = 0;
                    }
                }
                returnResult.Add(ObjectConverter.DeepCopy(expressionRealTimeResultInfo));
            }
        }

        /// <summary>
        /// 单个分析模型的分析.
        /// </summary>
        /// <param name="dataAnalysisInfo">分析模型</param>
        /// <returns>表达式分析结果列表</returns>
        private List<ExpressionRealTimeResultInfo> DataAnalysis(JC_LargedataAnalysisConfigInfo dataAnalysisInfo)
        {
            List<ExpressionRealTimeResultInfo> returnResult = new List<ExpressionRealTimeResultInfo>();
            List<JC_SetAnalysisModelPointRecordInfo> pointMappingList = dataAnalysisInfo.AnalysisModelPointRecordInfoList;
            if (pointMappingList == null || pointMappingList.Count == 0)
                return returnResult;

            dataAnalysisInfo.AnalysisSuccessfulPointList.Clear();
            //group by ExpressionId
            IEnumerable<IGrouping<string, JC_SetAnalysisModelPointRecordInfo>> expressionGroup = pointMappingList.GroupBy(p => p.ExpressionId);
            foreach (IGrouping<string, JC_SetAnalysisModelPointRecordInfo> expressionParameterList in expressionGroup)
            {
                var firstPointBindingInfo = expressionParameterList.FirstOrDefault();
                int devTypeId = firstPointBindingInfo.DevTypeId;
                if (devTypeId > -1)
                {
                    //模型绑定类型的情况
                    List<PointAnalysisInfo> pointAnalysisInfoList = GetPointByDevType(devTypeId);
                    foreach (var item in pointAnalysisInfoList)
                    {
                        ExpressionAnalysis(dataAnalysisInfo, expressionParameterList, returnResult, item);
                        //绑定类型的情况，只要一个表达式成立就更新分析模型状态.

                        //计算结束时间 edit by  2018-02-02
                        bool isAnalysisSuccessful = returnResult.Any(q => q.AnalysisResult == 2 && ((q.MaxContinueTime == 0 && q.ActualContinueTime >= q.ContinueTime) || (q.MaxContinueTime > 0 && q.ActualContinueTime >= q.ContinueTime && q.ActualContinueTime <= q.MaxContinueTime)));
                        if (isAnalysisSuccessful)
                            AnalysisResultHandler(dataAnalysisInfo, returnResult);
                        if (isCallForStop)
                            break;
                    }
                }
                else
                {
                    //模型绑定测点的情况.
                    ExpressionAnalysis(dataAnalysisInfo, expressionParameterList, returnResult);
                }
            }
            return returnResult;
        }

        private bool HistoryDataHandler()
        {
            try
            {
                //关闭未关闭的报警处理记录
                BasicResponse response = alarmHandleService.CloseUnclosedAlarmHandle(new BasicRequest());
                return response.IsSuccess;
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(string.Format("关闭未关闭的报警处理记录出错, 错误消息:{0}", ex.StackTrace));
                return false;
            }
        }

        /// <summary>
        /// Log4net日志自动清除功能
        /// </summary>
        private void ClearLog4netLog()
        {
            string sondirsonDate = string.Empty;
            DateTime sondirsonDateTime = new DateTime();
            TimeSpan ts = new TimeSpan();
            string AutoClearLog4netLog = System.Configuration.ConfigurationManager.AppSettings["AutoClearLog4netLog"].ToString().ToLower();
            string Log4netFilePath = System.Configuration.ConfigurationManager.AppSettings["Log4netFilePath"].ToString();
            string ClearTimeLongAgo = System.Configuration.ConfigurationManager.AppSettings["ClearTimeLongAgo"].ToString();

            while (IsRunning)
            {
                try
                {
                    //清除同步debug日志 
                    if (AutoClearLog4netLog == "true")
                    {
                        if (Directory.Exists(Log4netFilePath))
                        {
                            DirectoryInfo dir = new DirectoryInfo(Log4netFilePath);
                            DirectoryInfo[] dirs = dir.GetDirectories();
                            foreach (DirectoryInfo sondir in dirs)
                            {
                                DirectoryInfo[] sondirsons = sondir.GetDirectories();
                                foreach (DirectoryInfo sondirson in sondirsons)
                                {
                                    sondirsonDate = sondirson.Name.Substring(0, 4) + "-" + sondirson.Name.Substring(4, 2) + "-" + sondirson.Name.Substring(6, 2);
                                    sondirsonDateTime = DateTime.Parse(sondirsonDate);
                                    ts = DateTime.Now - sondirsonDateTime;
                                    if (ts.TotalDays > int.Parse(ClearTimeLongAgo))
                                    {
                                        Directory.Delete(sondirson.FullName, true);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    LogHelper.Error("清除mysql同步日志失败,详细信息" + ex.Message + ex.StackTrace);
                }
                Thread.Sleep(3600000);//每小时执行一次
            }
        }
    }
}