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
    public class RPRealCache : KJ73NCacheManager<R_PrealInfo>
    {

        private readonly IR_PrealService rPrealService;

        private static volatile RPRealCache rPrealCahceInstance;
        /// <summary>
        /// 呼叫控制单例
        /// </summary>
        public static RPRealCache RPrealCahceInstance
        {
            get
            {
                if (rPrealCahceInstance == null)
                {
                    lock (obj)
                    {
                        if (rPrealCahceInstance == null)
                        {
                            rPrealCahceInstance = new RPRealCache();
                        }
                    }
                }
                return rPrealCahceInstance;
            }
        }

        private RPRealCache()
        {
            rPrealService = ServiceFactory.Create<IR_PrealService>();
        }

        public override void Load()
        {
           // throw new NotImplementedException();
        }

        protected override void AddEntityToCache(R_PrealInfo item)
        {
            if (_cache.Count(real => real.Id == item.Id) < 1)
                _cache.Add(item);
        }

        protected override void DeleteEntityFromCache(R_PrealInfo item)
        {
            var tempitem = _cache.FirstOrDefault(real => real.Id == item.Id);
            if (tempitem != null)
                _cache.Remove(tempitem);
        }

        protected override void UpdateEntityToCache(R_PrealInfo item)
        {
            int itemIndex = _cache.FindIndex(real => real.Id == item.Id);
            if (itemIndex >= 0)
            {
                _cache[itemIndex] = item;
            }
        }

        public void UpdateRealInfo(string Id, Dictionary<string, object> updateItems)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            _rwLocker.AcquireWriterLock(-1);
            sw.Stop();
            if (sw.ElapsedMilliseconds > 0)
            {
                LogHelper.Warn("UpdatePersonRealInfo Lock：" + sw.ElapsedMilliseconds);
            }
            sw.Restart();
            try
            {
                R_PrealInfo iteminfo = _cache.FirstOrDefault(real => real.Id == Id);
                if (iteminfo != null)
                {
                    iteminfo.CopyProperties(updateItems);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("部分更新人员实时缓存信息失败：" + ex.Message);
            }
            finally
            {
                _rwLocker.ReleaseWriterLock();
            }
            sw.Stop();
            if (sw.ElapsedMilliseconds > 0)
            {
                LogHelper.Warn("UpdatePersonRealInfo：" + sw.ElapsedMilliseconds);
            }
        }

        public void BatchUpdateRealInfo(Dictionary<string, Dictionary<string, object>> Items)
        {
            _rwLocker.AcquireWriterLock(-1);

            try
            {
                foreach (string Id in Items.Keys)
                {
                    R_PrealInfo iteminfo = _cache.FirstOrDefault(real => real.Id == Id);
                    if (iteminfo != null)
                    {
                        iteminfo.CopyProperties(Items[Id]);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("部分更新人员实时缓存信息失败：" + ex.Message);
            }
            finally
            {
                _rwLocker.ReleaseWriterLock();
            }
        }
    }
}
