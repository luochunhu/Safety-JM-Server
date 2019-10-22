using Sys.Safety.ServiceContract.Cache;
using System.Collections.Generic;
using System.Linq;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Cache;
using Sys.Safety.Cache.Safety;
using Sys.Safety.Request.NetworkModule;

namespace Sys.Safety.Services.Cache
{
    /// <summary>
    /// 作者:
    /// 时间:2017-05-24
    /// 描述:网络模块缓存业务
    /// 修改记录
    /// 2017-05-24
    /// </summary>
    public class NetworkModuleCacheService : INetworkModuleCacheService
    {
        public BasicResponse AddNetworkModuleCache(NetworkModuleCacheAddRequest networkModuleCacheRequest)
        {
            NetworkModuleCache.NetworModuleCahceInstance.AddItem(networkModuleCacheRequest.NetworkModuleInfo);
            return new BasicResponse();
        }

        public BasicResponse BacthAddNetworkModuleCache(NetworkModuleCacheBatchAddRequest networkModuleCacheRequest)
        {
            NetworkModuleCache.NetworModuleCahceInstance.AddItems(networkModuleCacheRequest.NetworkModuleInfos);
            return new BasicResponse();
        }

        public BasicResponse BatchUpdateNetworkModuleCache(NetworkModuleCacheBatchUpdateRequest networkModuleCacheRequest)
        {
            
            NetworkModuleCache.NetworModuleCahceInstance.UpdateItems(networkModuleCacheRequest.NetworkModuleInfos);
            return new BasicResponse();
        }

        public BasicResponse DeleteNetworkModuleCache(NetworkModuleCacheDeleteRequest networkModuleCacheRequest)
        {
            NetworkModuleCache.NetworModuleCahceInstance.DeleteItem(networkModuleCacheRequest.NetworkModuleInfo);
            return new BasicResponse();
        }

        public BasicResponse<List<Jc_MacInfo>> GetAllNetworkModuleCache(NetworkModuleCacheGetAllRequest networkModuleCacheRequest)
        {
            var networkModuleCache = NetworkModuleCache.NetworModuleCahceInstance.Query();
            var networkModuleCacheResponse = new BasicResponse<List<Jc_MacInfo>>();
            networkModuleCacheResponse.Data = networkModuleCache;
            return networkModuleCacheResponse;
        }

        public BasicResponse<List<Jc_MacInfo>> GetNetworkModuleCache(NetworkModuleCacheGetByConditonRequest networkModuleCacheRequest)
        {
            var networkModuleCache = NetworkModuleCache.NetworModuleCahceInstance.Query(networkModuleCacheRequest.Predicate);
            var networkModuleCacheResponse = new BasicResponse<List<Jc_MacInfo>>();
            networkModuleCacheResponse.Data = networkModuleCache;
            return networkModuleCacheResponse;
        }

        public BasicResponse<Jc_MacInfo> GetNetworkModuleCacheByKey(NetworkModuleCacheGetByKeyRequest networkModuleCacheRequest)
        {
            var networkModuleCache = NetworkModuleCache.NetworModuleCahceInstance.Query(networkModule => networkModule.MAC == networkModuleCacheRequest.Mac).FirstOrDefault();
            var networkModuleCacheResponse = new BasicResponse<Jc_MacInfo>();
            networkModuleCacheResponse.Data = networkModuleCache;
            return networkModuleCacheResponse;
        }

        public BasicResponse LoadNetworkModuleCache(NetworkModuleCacheLoadRequest networkModuleCacheRequest)
        {
            NetworkModuleCache.NetworModuleCahceInstance.Load();
            //加载设备定义基本信息之后，加载设备定义拓展属性
            var networkModuleList = NetworkModuleCache.NetworModuleCahceInstance.Query();

            if (networkModuleList.Any())
            {
                //位置信息
                var positionList = PositionCache.PositionCahceInstance.Query();
                networkModuleList.ForEach(nwModule =>
                {
                    var position = positionList.FirstOrDefault(p => p.WzID == nwModule.Wzid);
                    nwModule.Wz = position == null ? null : position.Wz;
                    nwModule.NetID = 0;//第一次加载，将连接号全部清零  20170822
                    nwModule.State = 0;//第一次加载，将状态全部清零  20170822
                });
                NetworkModuleCache.NetworModuleCahceInstance.UpdateItems(networkModuleList);
            }
            return new BasicResponse();
        }

        public BasicResponse UpdateNetworkModuleCahce(NetworkModuleCacheUpdateRequest networkModuleCacheRequest)
        {
            NetworkModuleCache.NetworModuleCahceInstance.UpdateItem(networkModuleCacheRequest.NetworkModuleInfo);
            return new BasicResponse();
        }


        public BasicResponse UpdateNetworkModuleFdState(NetworkModuleCacheUpdateFdStateRequest networkModuleCacheRequest)
        {
            NetworkModuleCache.NetworModuleCahceInstance.UpdateFdState(networkModuleCacheRequest.NetWorkModuleInfo);
            return new BasicResponse();
        }

        public BasicResponse UpdateNetworkModuleNCommand(NetworkModuleCacheUpdateNCommandRequest networkModuleCacheRequest)
        {
            NetworkModuleCache.NetworModuleCahceInstance.UpdateNCommand(networkModuleCacheRequest.NetWorkModuleInfo);
            return new BasicResponse();
        }

        public BasicResponse UpdateNetworkInfo(NetworkModuleCacheUpdatePropertiesRequest pointDefineCacheRequest)
        {
            NetworkModuleCache.NetworModuleCahceInstance.UpdateNetworkInfo(pointDefineCacheRequest.Mac, pointDefineCacheRequest.UpdateItems);
            return new BasicResponse();
        }


        public BasicResponse TestAlarm(TestAlarmRequest testAlarmRequest)
        {
            NetworkModuleCache.NetworModuleCahceInstance.TestAlarm(testAlarmRequest.macItems, testAlarmRequest.testAlarmFlag);
            return new BasicResponse();
        }
    }
}
