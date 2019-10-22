using Basic.Framework.JobSchedule;
using Basic.Framework.Service;
using Sys.Safety.Request.Cache;
using Sys.Safety.ServiceContract.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Processing.Cache
{
    public class RunLogCacheCleanTask : BasicTask
    {
        protected static readonly object obj = new object();

        private static volatile RunLogCacheCleanTask _instance;

        public static RunLogCacheCleanTask Instance 
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new RunLogCacheCleanTask(10000);
                        }
                    }
                }
                return _instance;
            }
        }

        private readonly IRunLogCacheService runLogCacheService;

        private int _delayCounter;

        private RunLogCacheCleanTask(int intreval)
            : base("清除运行记录缓存任务", intreval)
        {
            runLogCacheService = ServiceFactory.Create<IRunLogCacheService>();
            _delayCounter = 20;
        }

        protected override void DoWork()
        {
            var runlogResponse = runLogCacheService.GetAllRunLogCache(new RunLogCacheGetAllRequest());
            if (runlogResponse.IsSuccess && runlogResponse.Data != null)
            {
                var runlogItems=runlogResponse.Data;

                var count = runlogItems.Count - _delayCounter;
                if (count > 0) 
                {
                    var cleanItems = runlogItems.OrderBy(runlog => runlog.Counter).Take(count).ToList();
                    RunLogBatchDeleteRequest deleteRequest = new RunLogBatchDeleteRequest
                    {
                        RunLogInfos = cleanItems
                    };
                    runLogCacheService.BatchDeleteRunLogCache(deleteRequest);
                }
            }
            base.DoWork();
        }
    }
}
