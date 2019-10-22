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
    public class R_PBCacheService : IR_PBCacheService
    {
        private Thread cleanR_PBCacheThread;
        private TimeSpan delaytime;
        private bool cleanR_PBCacheThreadRunning;
        ISettingCacheService settingCacheService;

        public R_PBCacheService(ISettingCacheService _settingCacheService)
        {
            int R_PBCacheClearTime = 5;
            settingCacheService = _settingCacheService;
            //读取报警缓存清除时间的配置  20170723
            SettingCacheGetByKeyRequest request = new SettingCacheGetByKeyRequest();
            request.StrKey = "R_PBCacheClearTime";
            var result = settingCacheService.GetSettingCacheByKey(request);
            if (result != null && result.Data != null)
            {
                int.TryParse(result.Data.StrValue, out R_PBCacheClearTime);
            }
            if (R_PBCacheClearTime < 5) {
                R_PBCacheClearTime = 5;//最少5分钟 
            }
            delaytime = new TimeSpan(0, R_PBCacheClearTime, 0);
            cleanR_PBCacheThreadRunning = false;
        }

        public BasicResponse AddR_PBCache(R_PBCacheAddRequest R_PBCacheRequest)
        {
            R_PBCache.CahceInstance.AddItem(R_PBCacheRequest.R_PBInfo);
            return new BasicResponse();
        }

        public BasicResponse BacthAddR_PBCache(R_PBCacheBatchAddRequest R_PBCacheRequest)
        {
            R_PBCache.CahceInstance.AddItems(R_PBCacheRequest.R_PBInfos);
            return new BasicResponse();
        }

        public BasicResponse UpdateAlarmCahce(R_PBCacheUpdateRequest R_PBCacheRequest)
        {
            R_PBCache.CahceInstance.UpdateItem(R_PBCacheRequest.R_PBInfo);
            return new BasicResponse();
        }

        public BasicResponse BatchUpdateR_PBCache(R_PBCacheBatchUpdateRequest R_PBCacheRequest)
        {
            R_PBCache.CahceInstance.UpdateItems(R_PBCacheRequest.R_PBInfos);
            return new BasicResponse();
        }

        public BasicResponse DeleteAllR_PBCache(R_PBCacheDeleteAllRequest R_PBCacheRequest)
        {
            R_PBCache.CahceInstance.ClearUp();
            return new BasicResponse();
        }

        public BasicResponse<List<R_PbInfo>> GetR_PBCache(R_PBCacheGetByConditonRequest R_PBCacheRequest)
        {
            var result = R_PBCache.CahceInstance.Query(R_PBCacheRequest.Predicate);
            var R_PBCacheResponse = new BasicResponse<List<R_PbInfo>>();
            R_PBCacheResponse.Data = result;
            return R_PBCacheResponse;
        }

        public BasicResponse<R_PbInfo> GetR_PBCacheByKey(R_PBCacheGetByKeyRequest R_PBCacheRequest)
        {
            var result = R_PBCache.CahceInstance.Query(alarm => alarm.Id == R_PBCacheRequest.Id).FirstOrDefault();
            var R_PBCacheResponse = new BasicResponse<R_PbInfo>();
            R_PBCacheResponse.Data = result;
            return R_PBCacheResponse;
        }

        public BasicResponse<List<R_PbInfo>> GetAllR_PBCache(R_PBCacheGetAllRequest R_PBCacheRequest)
        {
            var result = R_PBCache.CahceInstance.Query();
            var R_PBCacheResponse = new BasicResponse<List<R_PbInfo>>();
            R_PBCacheResponse.Data = result;
            return R_PBCacheResponse;
        }

        public BasicResponse LoadR_PBCache(R_PBCacheLoadRequest R_PBCacheRequest)
        {
            if (cleanR_PBCacheThreadRunning)
                return new BasicResponse();
            cleanR_PBCacheThreadRunning = true;

            if (cleanR_PBCacheThread == null || cleanR_PBCacheThread.ThreadState != ThreadState.Running)
            {
                cleanR_PBCacheThread = new Thread(CleanR_PBCache);
                cleanR_PBCacheThread.Start();
            }
            R_PBCache.CahceInstance.Load();
            return new BasicResponse();
        }

        public BasicResponse StopCleanR_PBCacheThread(R_PBCacheStopCleanRequest R_PBCacheRequest)
        {
            LogHelper.Info("清理人员报警缓存信息正在停止！");
            cleanR_PBCacheThreadRunning = false;
            LogHelper.Info("清理人员报警缓存信息已停止！");
            return new BasicResponse();
        }

        private void CleanR_PBCache()
        {
            while (cleanR_PBCacheThreadRunning)
            {
                try
                {
                    //删除已结束,并超过5分钟的报警数据,Etime=1900-01-01 00:00:00 表示未结束   20170616
                    List<R_PbInfo> deleteR_PBCaches = R_PBCache.CahceInstance.Query(alarm => DateTime.Now - alarm.Endtime > delaytime && alarm.Endtime != DateTime.Parse("1900-01-01 00:00:00"));
                    if (deleteR_PBCaches.Any())
                        R_PBCache.CahceInstance.DeleteItems(deleteR_PBCaches);
                }
                catch (Exception ex)
                {
                    LogHelper.Error("删除5分钟报警记录出错：" + "\r\n" + ex.Message);
                }
                Thread.Sleep(10000);
            }
            LogHelper.Info("清理报警缓存信息线程结束！");
        }


        public BasicResponse BatchDeleteR_PBCache(R_PBCacheBatchDeleteRequest R_PBCacheRequest)
        {
            R_PBCache.CahceInstance.DeleteItems(R_PBCacheRequest.R_PBInfos);
            return new BasicResponse();
        }


        public BasicResponse UpdateAlarmInfoProperties(R_PBCacheUpdatePropertiesRequest R_PBCacheRequest)
        {
            R_PBCache.CahceInstance.UpdateAlarmInfoProperties(R_PBCacheRequest.R_PBKey, R_PBCacheRequest.UpdateItems);
            return new BasicResponse();
        }

    }
}
