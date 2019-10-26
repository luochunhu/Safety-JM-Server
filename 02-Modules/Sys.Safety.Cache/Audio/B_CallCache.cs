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

namespace Sys.Safety.Cache.Audio
{
    public class B_CallCache : KJ73NCacheManager<B_CallInfo>
    {
        private readonly IB_CallService BCallService;

        private static volatile B_CallCache _Instance;
        /// <summary>
        /// 呼叫控制单例
        /// </summary>
        public static B_CallCache Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (obj)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new B_CallCache();
                        }
                    }
                }
                return _Instance;
            }
        }

        private B_CallCache()
        {
            BCallService = ServiceFactory.Create<IB_CallService>();
        }

        public override void Load()
        {
            var respose = BCallService.GetAll(new Basic.Framework.Web.BasicRequest());
            if (respose.Data != null && respose.IsSuccess)
            {
                _cache.Clear();
                _cache = respose.Data;
            }
        }

        protected override void AddEntityToCache(B_CallInfo item)
        {
            if (_cache.Count(call => call.Id == item.Id) < 1)
                _cache.Add(item);
        }

        protected override void DeleteEntityFromCache(B_CallInfo item)
        {
            var tempitem = _cache.FirstOrDefault(call => call.Id == item.Id);
            if (tempitem != null)
                _cache.Remove(tempitem);
        }

        protected override void UpdateEntityToCache(B_CallInfo item)
        {
            int itemIndex = _cache.FindIndex(call => call.Id == item.Id);
            if (itemIndex >= 0)
            {
                _cache[itemIndex] = item;
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="callId"></param>
        ///// <param name="callerDN">主叫</param>
        ///// <param name="calledDN">被叫</param>
        ///// <returns></returns>
        //public B_CallInfo GetCallInfoByCallInfo(string callId, string callerDN, string calledDN)
        //{
        //    B_CallInfo item = null;

        //    try
        //    {
        //        foreach (B_CallInfo info in _cache)
        //        {
        //            if (info.CallPointList != null)
        //            {
        //                if (info.CallPointList.FirstOrDefault(a => a.CallId == callId && a.AgentPointId == callerDN && a.CalledPointId == calledDN) != null)
        //                {
        //                    item = info;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Error("获取B_CallInfo缓存出错：" + ex.Message);
        //    }

        //    return item;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointItems"></param>
        public void BatchUpdatePointInfo(Dictionary<string, Dictionary<string, object>> pointItems)
        {
            _rwLocker.AcquireWriterLock(-1);

            try
            {
                foreach (string id in pointItems.Keys)
                {
                    B_CallInfo iteminfo = _cache.FirstOrDefault(a => a.Id == id);
                    if (iteminfo != null)
                    {
                        iteminfo.CopyProperties(pointItems[id]);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("批量部分更新测点定义缓存信息失败：" + ex.Message);
            }
            finally
            {
                _rwLocker.ReleaseWriterLock();
            }
        }
    }
}
