using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.NetworkModule;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sys.Safety.Cache.Safety
{
    /// <summary>
    /// 作者:
    /// 时间:2017-05-24
    /// 描述:网络模块缓存
    /// 修改记录
    /// 2017-05-24
    /// </summary>
    public class NetworkModuleCache : KJ73NCacheManager<Jc_MacInfo>
    {
        private readonly INetworkModuleService networkModuleService;
        private readonly IPositionService positionService;

        private static volatile NetworkModuleCache networModuleCahceInstance;
        /// <summary>
        /// 网络模块单例
        /// </summary>
        public static NetworkModuleCache NetworModuleCahceInstance
        {
            get
            {
                if (networModuleCahceInstance == null)
                {
                    lock (obj)
                    {
                        if (networModuleCahceInstance == null)
                        {
                            networModuleCahceInstance = new NetworkModuleCache();
                        }
                    }
                }
                return networModuleCahceInstance;
            }
        }

        private NetworkModuleCache()
        {
            networkModuleService = ServiceFactory.Create<INetworkModuleService>();
            positionService = ServiceFactory.Create<IPositionService>();
        }

        public override void Load()
        {
            //try
            //{
                var respose = networkModuleService.GetNetworkModuleList();
                //List<Jc_WzInfo> PositionList = positionService.GetPositionList().Data;
                List<Jc_MacInfo> NetworkModuleList = respose.Data;
                //foreach (Jc_MacInfo NetworkModule in NetworkModuleList)
                //{
                //    var result = PositionList.Find(a => a.WzID == NetworkModule.Wzid);
                //    if (result != null)
                //    {
                //        NetworkModule.Wz = result.Wz;//赋值安装位置
                //    }
                //    else
                //    {
                //        NetworkModule.Wz = null;
                //    }
                //}
                if (respose.Data != null && respose.IsSuccess)
                {
                    _cache.Clear();
                    _cache = NetworkModuleList;
                }
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.Error(string.Format("{0}:加载缓存失败！" + "\r\n" + ex.Message, CacheName));
            //}
        }

        protected override void AddEntityToCache(Jc_MacInfo item)
        {
            if (_cache.Count(networkModule => networkModule.MAC == item.MAC) < 1)
                _cache.Add(item);
        }

        protected override void DeleteEntityFromCache(Jc_MacInfo item)
        {
            var tempitem = _cache.FirstOrDefault(networkModule => networkModule.MAC == item.MAC);
            if (tempitem != null)
                _cache.Remove(tempitem);
        }

        protected override void UpdateEntityToCache(Jc_MacInfo item)
        {
            int itemIndex = _cache.FindIndex(networkModule => networkModule.MAC == item.MAC);
            if (itemIndex >= 0)
            {
                _cache[itemIndex] = item;
            }
        }

        public void UpdateNCommand(Jc_MacInfo item)
        {
            int itemIndex = _cache.FindIndex(mac => mac.MAC == item.MAC);
            //if (itemIndex > 0)
            if (itemIndex >= 0) //2017.11.2 by
            {
                _cache[itemIndex].NCommandbz = item.NCommandbz;
                _cache[itemIndex].SendDtime = item.SendDtime;
                _cache[itemIndex].BatteryControl = item.BatteryControl;//赋值命令类型
                _cache[itemIndex].SendBatteryControlCount = item.SendBatteryControlCount;
            }
        }

        public void UpdateFdState(Jc_MacInfo item)
        {
            int itemIndex = _cache.FindIndex(mac => mac.MAC == item.MAC);
            //if (itemIndex > 0)
            if (itemIndex >= 0) //2017.11.2 by
            {
                _cache[itemIndex].Fdstate = item.Fdstate;
                _cache[itemIndex].SendBatteryControlCount = item.SendBatteryControlCount;
            }
        }
        /// <summary>
        /// 更新部分属性
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="updateItems"></param>
        public void UpdateNetworkInfo(string Key, Dictionary<string, object> updateItems)
        {
            _rwLocker.AcquireWriterLock(-1);
            try
            {
                Jc_MacInfo iteminfo = _cache.FirstOrDefault(Network => Network.MAC == Key);
                if (iteminfo != null)
                {
                    iteminfo.CopyProperties(updateItems);                  
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("部分更新MAC缓存信息失败：" + ex.Message);
            }
            finally 
            {
                _rwLocker.ReleaseWriterLock();
            }
        }

        public void TestAlarm(List<Jc_MacInfo> macItems,int testFlag)
        {
            _rwLocker.AcquireWriterLock(-1);
            try
            {
                if (macItems.Count == 0)
                {
                    _cache.ForEach(a =>
                    {
                        a.NCommandbz |= 2;
                        a.sendAlarmCount = 3;
                        a.testAlarmFlag = testFlag;
                    });
                }
                else
                {
                    foreach (Jc_MacInfo item in macItems)
                    {
                        int itemIndex = _cache.FindIndex(mac => mac.MAC == item.MAC);

                        if (itemIndex >= 0)
                        {
                            _cache[itemIndex].NCommandbz |= 0x02;
                            _cache[itemIndex].sendAlarmCount = 3;
                            _cache[itemIndex].testAlarmFlag = testFlag;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("部分更新MAC缓存信息失败：" + ex.Message);
            }
            finally
            {
                _rwLocker.ReleaseWriterLock();
            }
        }
    }
}
