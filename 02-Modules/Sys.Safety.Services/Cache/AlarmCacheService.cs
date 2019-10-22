using Sys.Safety.ServiceContract.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Cache;
using Sys.Safety.Cache.Safety;
using System.Threading;
using Basic.Framework.Logging;

namespace Sys.Safety.Services.Cache
{
    /// <summary>
    /// 作者:
    /// 时间:2017-05-22
    /// 描述:报警缓存业务
    /// 修改记录
    /// 2017-05-23
    /// </summary>
    public class AlarmCacheService : IAlarmCacheService
    {
        private Thread cleanAlarmCacheThread;
        private TimeSpan delaytime;
        private bool cleanAlarmCacheThreadRunning;
        ISettingCacheService settingCacheService;

        public AlarmCacheService(ISettingCacheService _settingCacheService)
        {
            int alarmCacheClearTime = 5;
            settingCacheService = _settingCacheService;
            //读取报警缓存清除时间的配置  20170723
            SettingCacheGetByKeyRequest request = new SettingCacheGetByKeyRequest();
            request.StrKey = "AlarmCacheClearTime";
            var result = settingCacheService.GetSettingCacheByKey(request);
            if (result != null && result.Data != null)
            {
                int.TryParse(result.Data.StrValue, out alarmCacheClearTime);
            }
            if (alarmCacheClearTime < 5) {
                alarmCacheClearTime = 5;//最少5分钟 
            }
            delaytime = new TimeSpan(0, alarmCacheClearTime, 0);
            cleanAlarmCacheThreadRunning = false;
        }

        public BasicResponse AddAlarmCache(AlarmCacheAddRequest alarmCacheRequest)
        {
            AlarmCache.AlarmCacheInstance.AddItem(alarmCacheRequest.AlarmInfo);
            return new BasicResponse();
        }

        public BasicResponse BacthAddAlarmCache(AlarmCacheBatchAddRequest alarmCacheRequest)
        {
            AlarmCache.AlarmCacheInstance.AddItems(alarmCacheRequest.AlarmInfos);
            return new BasicResponse();
        }

        public BasicResponse UpdateAlarmCahce(AlarmCacheUpdateRequest alarmCacheRequest)
        {
            AlarmCache.AlarmCacheInstance.UpdateItem(alarmCacheRequest.AlarmInfo);
            return new BasicResponse();
        }

        public BasicResponse BatchUpdateAlarmCache(AlarmCacheBatchUpdateRequest alarmCacheRequest)
        {
            AlarmCache.AlarmCacheInstance.UpdateItems(alarmCacheRequest.AlarmInfos);
            return new BasicResponse();
        }

        public BasicResponse DeleteAllAlarmCache(AlarmCacheDeleteAllRequest alarmCacheRequest)
        {
            AlarmCache.AlarmCacheInstance.ClearUp();
            return new BasicResponse();
        }

        public BasicResponse<List<Jc_BInfo>> GetAlarmCache(AlarmCacheGetByConditonRequest alarmCacheRequest)
        {
            var alarmCache = AlarmCache.AlarmCacheInstance.Query(alarmCacheRequest.Predicate);
            var alarmCacheResponse = new BasicResponse<List<Jc_BInfo>>();
            alarmCacheResponse.Data = alarmCache;
            return alarmCacheResponse;
        }

        public BasicResponse<Jc_BInfo> GetAlarmCacheByKey(AlarmCacheGetByKeyRequest alarmCacheRequest)
        {
            var alarmCache = AlarmCache.AlarmCacheInstance.Query(alarm => alarm.ID == alarmCacheRequest.Id).FirstOrDefault();
            var alarmCacheResponse = new BasicResponse<Jc_BInfo>();
            alarmCacheResponse.Data = alarmCache;
            return alarmCacheResponse;
        }

        public BasicResponse<List<Jc_BInfo>> GetAllAlarmCache(AlarmCacheGetAllRequest alarmCacheRequest)
        {
            var alarmCache = AlarmCache.AlarmCacheInstance.Query();
            var alarmCacheResponse = new BasicResponse<List<Jc_BInfo>>();
            alarmCacheResponse.Data = alarmCache;
            return alarmCacheResponse;
        }

        public BasicResponse LoadAlarmCache(AlarmCacheLoadRequest alarmCacheRequest)
        {
            if (cleanAlarmCacheThreadRunning)
                return new BasicResponse();
            cleanAlarmCacheThreadRunning = true;

            if (cleanAlarmCacheThread == null || cleanAlarmCacheThread.ThreadState != ThreadState.Running)
            {
                cleanAlarmCacheThread = new Thread(CleanAlarmCache);
                cleanAlarmCacheThread.Start();
            }
            AlarmCache.AlarmCacheInstance.Load();
            return new BasicResponse();
        }

        public BasicResponse StopCleanAlarmCacheThread(AlarmCacheStopCleanRequest alarmCacheRequest)
        {
            LogHelper.Info("清理报警缓存信息正在停止！");
            cleanAlarmCacheThreadRunning = false;
            LogHelper.Info("清理报警缓存信息已停止！");
            return new BasicResponse();
        }

        private void CleanAlarmCache()
        {
            while (cleanAlarmCacheThreadRunning)
            {
                try
                {
                    //删除已结束,并超过5分钟的报警数据,Etime=1900-01-01 00:00:00 表示未结束   20170616
                    List<Jc_BInfo> deleteAlarmCaches = AlarmCache.AlarmCacheInstance.Query(alarm => DateTime.Now - alarm.Etime > delaytime && alarm.Etime != DateTime.Parse("1900-01-01 00:00:00"));
                    if (deleteAlarmCaches.Any())
                        AlarmCache.AlarmCacheInstance.DeleteItems(deleteAlarmCaches);
                }
                catch (Exception ex)
                {
                    LogHelper.Error("删除5分钟报警记录出错：" + "\r\n" + ex.Message);
                }
                Thread.Sleep(10000);
            }
            LogHelper.Info("清理报警缓存信息线程结束！");
        }


        public BasicResponse BatchDeleteAlarmCache(AlarmCacheBatchDeleteRequest alarmCacheRequest)
        {
            AlarmCache.AlarmCacheInstance.DeleteItems(alarmCacheRequest.AlarmInfos);
            return new BasicResponse();
        }


        public BasicResponse UpdateAlarmInfoProperties(AlarmCacheUpdatePropertiesRequest alarmCacheRequest)
        {
            AlarmCache.AlarmCacheInstance.UpdateAlarmInfoProperties(alarmCacheRequest.AlarmKey, alarmCacheRequest.UpdateItems);
            return new BasicResponse();
        }

    }
}
