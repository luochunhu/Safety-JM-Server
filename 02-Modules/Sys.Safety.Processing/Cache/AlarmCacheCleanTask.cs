using Basic.Framework.JobSchedule;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Cache;
using Sys.Safety.ServiceContract.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Processing.Cache
{    /// <summary>
    /// 作者:
    /// 时间:2017-06-21
    /// 描述:报警缓存线程
    /// 修改记录
    /// 2017-06-21
    /// </summary>
    public class AlarmCacheCleanTask : BasicTask
    {
        protected static readonly object obj = new object();

        private static volatile AlarmCacheCleanTask _instance;
        public static AlarmCacheCleanTask Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new AlarmCacheCleanTask(10000);
                        }
                    }
                }
                return _instance;
            }
        }

        private readonly IAlarmCacheService alarmCacheService;

        private TimeSpan delaytime;

        private AlarmCacheCleanTask(int intreval)
            : base("清除已结束报警缓存任务", intreval)
        {
            alarmCacheService = ServiceFactory.Create<IAlarmCacheService>();
            delaytime = new TimeSpan(0, 5, 0);
        }

        protected override void DoWork()
        {
            //删除已结束,并超过5分钟的报警数据,Etime=1900-01-01 00:00:00 表示未结束   20170616
            AlarmCacheGetByConditonRequest request = new AlarmCacheGetByConditonRequest
            {
                Predicate = alarm => DateTime.Now - alarm.Etime > delaytime && alarm.Etime != DateTime.Parse("1900-01-01 00:00:00")
            };
            var alarmRespnse = alarmCacheService.GetAlarmCache(request);
            if (alarmRespnse.IsSuccess && alarmRespnse.Data != null && alarmRespnse.Data.Any())
            {
                AlarmCacheBatchDeleteRequest deleteRequest = new AlarmCacheBatchDeleteRequest
                {
                    AlarmInfos = alarmRespnse.Data
                };
                alarmCacheService.BatchDeleteAlarmCache(deleteRequest);
            }

            base.DoWork();
        }
    }
}
