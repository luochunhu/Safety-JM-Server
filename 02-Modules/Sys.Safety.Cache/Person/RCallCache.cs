using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.Cache.Safety;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Cache.Person
{
    public class RCallCache : KJ73NCacheManager<R_CallInfo>
    {
        private readonly IR_CallService rCallService;

        private static volatile RCallCache rCallCahceInstance;
        /// <summary>
        /// 呼叫控制单例
        /// </summary>
        public static RCallCache RCallCahceInstance
        {
            get
            {
                if (rCallCahceInstance == null)
                {
                    lock (obj)
                    {
                        if (rCallCahceInstance == null)
                        {
                            rCallCahceInstance = new RCallCache();
                        }
                    }
                }
                return rCallCahceInstance;
            }
        }

        private RCallCache()
        {
            rCallService = ServiceFactory.Create<IR_CallService>();
        }

        public override void Load()
        {
            var respose = rCallService.GetAllCall();
            if (respose.Data != null && respose.IsSuccess)
            {
                _cache.Clear();
                _cache = respose.Data;
            }
        }

        protected override void AddEntityToCache(R_CallInfo item)
        {
            if (_cache.Count(call => call.Id == item.Id) < 1)
                _cache.Add(item);
        }

        protected override void DeleteEntityFromCache(R_CallInfo item)
        {
            var tempitem = _cache.FirstOrDefault(call => call.Id == item.Id);
            if (tempitem != null)
                _cache.Remove(tempitem);
        }

        protected override void UpdateEntityToCache(R_CallInfo item)
        {
            int itemIndex = _cache.FindIndex(call => call.Id == item.Id);
            if (itemIndex >= 0)
            {
                _cache[itemIndex] = item;
            }
        }

        public void BachUpdateAlarmInfoProperties(Dictionary<string, Dictionary<string, object>>  updateItems)
        {
            _rwLocker.AcquireWriterLock(-1);
            try
            {
                foreach (string id in updateItems.Keys)
                {
                    R_CallInfo iteminfo = _cache.FirstOrDefault(pointdefine => pointdefine.Id == id);
                    if (iteminfo != null)
                    {
                        iteminfo.CopyProperties(updateItems[id]);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("部分更新R_CallInfo缓存信息失败：" + ex.Message);
            }
            finally
            {
                _rwLocker.ReleaseWriterLock();
            }
        }
    }
}
