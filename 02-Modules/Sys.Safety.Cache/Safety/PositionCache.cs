using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Position;
using Sys.Safety.ServiceContract;
using System;
using System.Linq;

namespace Sys.Safety.Cache.Safety
{
    /// <summary>
    /// 作者:
    /// 时间:2017-05-24
    /// 描述:安装位置缓存
    /// 修改记录
    /// 2017-05-24
    /// </summary>
    public class PositionCache : KJ73NCacheManager<Jc_WzInfo>
    {
        private readonly IPositionService positionService;

        private static volatile PositionCache positionCahceInstance;
        /// <summary>
        /// 安装位置单例
        /// </summary>
        public static PositionCache PositionCahceInstance
        {
            get
            {
                if (positionCahceInstance == null)
                {
                    lock (obj)
                    {
                        if (positionCahceInstance == null)
                        {
                            positionCahceInstance = new PositionCache();
                        }
                    }
                }
                return positionCahceInstance;
            }
        }

        private PositionCache()
        {
            positionService = ServiceFactory.Create<IPositionService>();
        }

        public override void Load()
        {
            //try
            //{
                var respose = positionService.GetPositionList();
                if (respose.Data != null && respose.IsSuccess)
                {
                    _cache.Clear();
                    _cache = respose.Data;
                }
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.Error(string.Format("{0}:加载缓存失败！" + "\r\n" + ex.Message, CacheName));
            //}
        }

        protected override void AddEntityToCache(Jc_WzInfo item)
        {
            if (_cache.Count(position => position.WzID == item.WzID) < 1)
                _cache.Add(item);
        }

        protected override void DeleteEntityFromCache(Jc_WzInfo item)
        {
            var tempitem = _cache.FirstOrDefault(position => position.WzID == item.WzID);
            if (tempitem != null)
                _cache.Remove(tempitem);
        }

        protected override void UpdateEntityToCache(Jc_WzInfo item)
        {
            int itemIndex = _cache.FindIndex(position => position.WzID == item.WzID);
            if (itemIndex >= 0)
            {
                _cache[itemIndex] = item;
            }
        }
    }
}
