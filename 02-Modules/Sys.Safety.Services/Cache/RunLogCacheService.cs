using Sys.Safety.ServiceContract.Cache;
using System.Collections.Generic;
using System.Linq;
using Basic.Framework.Web;
using Sys.Safety.Request.Cache;
using Sys.Safety.Cache.Safety;
using Sys.Safety.DataContract;
using System.Threading;
using System;
using Basic.Framework.Logging;

namespace Sys.Safety.Services.Cache
{
    /// <summary>
    /// 作者:
    /// 时间:2017-05-24
    /// 描述:运行日志业务
    /// 修改记录
    /// 2017-05-24
    /// </summary>
    public class RunLogCacheService : IRunLogCacheService
    {
        private Thread cleaRunLogThread;
        private int _delayCounter;
        private bool isRunning;

        public RunLogCacheService()
        {
            _delayCounter = 1000;//运行记录中保留数量
            isRunning = true;

            if (cleaRunLogThread == null || cleaRunLogThread.ThreadState != ThreadState.Running)
            {
                cleaRunLogThread = new Thread(CleanRunLog);
                cleaRunLogThread.Start();
            }
        }

        public BasicResponse AddRunLogCache(RunLogCacheAddRequest runLogCacheRequest)
        {
            RunLogCache.RunLogCahceInstance.AddItem(runLogCacheRequest.RunLogInfo);
            return new BasicResponse();
        }

        public BasicResponse BatchAddRunLogCache(RunLogCacheBatchAddRequest runLogCacheRequest)
        {
            RunLogCache.RunLogCahceInstance.AddItems(runLogCacheRequest.RunLogInfos);
            return new BasicResponse();
        }

        public BasicResponse BatchUpdateRunLogCache(RunLogCacheBatchUpdateRequest runLogCacheRequest)
        {
            RunLogCache.RunLogCahceInstance.UpdateItems(runLogCacheRequest.RunLogInfos);
            return new BasicResponse();
        }

        public BasicResponse<List<Jc_RInfo>> GetAllRunLogCache(RunLogCacheGetAllRequest runLogCacheRequest)
        {
            var runLogCache = RunLogCache.RunLogCahceInstance.Query();
            var runLogCacheResponse = new BasicResponse<List<Jc_RInfo>>();
            runLogCacheResponse.Data = runLogCache;
            return runLogCacheResponse;
        }

        public BasicResponse<List<Jc_RInfo>> GetRunLogCache(RunLogCacheGetByConditionRequest runLogCacheRequest)
        {
            var runLogCache = RunLogCache.RunLogCahceInstance.Query(runLogCacheRequest.Pridicate);
            var runLogCacheResponse = new BasicResponse<List<Jc_RInfo>>();
            runLogCacheResponse.Data = runLogCache;
            return runLogCacheResponse;
        }

        public BasicResponse<Jc_RInfo> GetRunLogCacheByKey(RunLogCacheGetByKeyRequest runLogCacheRequest)
        {
            var runLogCache = RunLogCache.RunLogCahceInstance.Query(runlog => runlog.ID == runLogCacheRequest.RunLogId).FirstOrDefault();
            var runLogCacheResponse = new BasicResponse<Jc_RInfo>();
            runLogCacheResponse.Data = runLogCache;
            return runLogCacheResponse;
        }

        public BasicResponse UpdateRunLogCache(RunLogCacheUpdateRequest runLogCacheRequest)
        {
            RunLogCache.RunLogCahceInstance.UpdateItem(runLogCacheRequest.RunLogInfo);
            return new BasicResponse();
        }

        public BasicResponse StopCleanRunlogCacheThread(RunLogCacheStopCleanRunLogRequest alarmCacheRequest)
        {
            LogHelper.Info("清理运行记录缓存信息正在停止！");
            isRunning = false;
            LogHelper.Info("清理运行记录缓存信息已结束！");
            return new BasicResponse();
        }

        private void CleanRunLog()
        {
            while (isRunning)
            {
                try
                {
                    int count = RunLogCache.RunLogCahceInstance.Query().Count() - _delayCounter;
                    if (count > 0)
                    {
                        //清除运行记录：先取出需要清除的记录，再删除
                        var cleanItems = RunLogCache.RunLogCahceInstance.Query().OrderBy(runlog => runlog.Counter).Take(count).ToList();
                        if (cleanItems.Any())
                            RunLogCache.RunLogCahceInstance.DeleteItems(cleanItems);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error("清除运行记录出错:" + "\r\n" + ex.Message);
                }
                Thread.Sleep(10000);
            }
            LogHelper.Info("清理运行记录缓存信息线程结束！");
        }


        public BasicResponse BatchDeleteRunLogCache(RunLogBatchDeleteRequest runLogCacheRequest)
        {
            RunLogCache.RunLogCahceInstance.DeleteItems(runLogCacheRequest.RunLogInfos);
            return new BasicResponse();
        }
    }
}
