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
    public class RPointDefineCache : KJ73NCacheManager<Jc_DefInfo>
    {
        private readonly IR_DefService pointDefineService;

        /// <summary>
        /// 更新缓存队列计数器
        /// </summary>
        public long pointCacheUpCount { get; set; }

        private static volatile RPointDefineCache _instance;
        /// <summary>
        /// 测点定义单例
        /// </summary>
        public static RPointDefineCache GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new RPointDefineCache();
                        }
                    }
                }
                return _instance;
            }
        }

        private RPointDefineCache()
        {
            pointDefineService = ServiceFactory.Create<IR_DefService>();
        }

        public override void Load()
        {
            var respose = pointDefineService.GetAllDefInfo();
            if (respose.Data != null && respose.IsSuccess)
            {
                _cache.Clear();
                var data = respose.Data;
                data.ForEach(item => { item.Ssz = ""; });
                _cache = respose.Data.FindAll(a => a.Activity == "1").OrderBy(a => a.Point).ToList();
            }
        }

        protected override void AddEntityToCache(Jc_DefInfo item)
        {
            if (_cache.Count(pointdefine => pointdefine.PointID == item.PointID) < 1)
                _cache.Add(item);
        }

        protected override void DeleteEntityFromCache(Jc_DefInfo item)
        {
            var tempitem = _cache.FirstOrDefault(devdefine => devdefine.PointID == item.PointID);
            if (tempitem != null)
                _cache.Remove(tempitem);
        }

        protected override void UpdateEntityToCache(Jc_DefInfo item)
        {
            int itemIndex = _cache.FindIndex(pointdefine => pointdefine.PointID == item.PointID);
            if (itemIndex >= 0)
            {
                _cache[itemIndex] = item;
                pointCacheUpCount++;
            }
        }

        public void UpdatePointInfo(string PointKey, Dictionary<string, object> updateItems)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            _rwLocker.AcquireWriterLock(-1);
            sw.Stop();
            if (sw.ElapsedMilliseconds > 0)
            {
                LogHelper.Warn("UpdatePointInfo Lock：" + sw.ElapsedMilliseconds);
            }
            sw.Restart();
            try
            {
                Jc_DefInfo iteminfo = _cache.FirstOrDefault(pointdefine => pointdefine.PointID == PointKey);
                if (iteminfo != null)
                {
                    iteminfo.CopyProperties(updateItems);
                    pointCacheUpCount++;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("部分更新测点定义缓存信息失败：" + ex.Message);
            }
            finally
            {
                _rwLocker.ReleaseWriterLock();
            }
            sw.Stop();
            if (sw.ElapsedMilliseconds > 0)
            {
                LogHelper.Warn("UpdatePointInfo：" + sw.ElapsedMilliseconds);
            }
        }

        public void BatchUpdatePointInfo(Dictionary<string, Dictionary<string, object>> pointItems)
        {
            _rwLocker.AcquireWriterLock(-1);

            try
            {
                foreach (string pointId in pointItems.Keys)
                {
                    Jc_DefInfo iteminfo = _cache.FirstOrDefault(pointdefine => pointdefine.PointID == pointId);
                    if (iteminfo != null)
                    {
                        iteminfo.CopyProperties(pointItems[pointId]);
                        pointCacheUpCount++;
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
