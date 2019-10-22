using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.NetworkModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.ServiceContract.Cache
{
    /// <summary>
    /// 作者:
    /// 时间:2017-05-22
    /// 描述:网络模块缓存业务RPC接口
    /// 修改记录
    /// 2017-05-23
    /// </summary>
    public interface INetworkModuleCacheService
    {
        /// <summary>
        /// 加载网络模块缓存
        /// </summary>
        /// <param name="networkModuleCacheRequest"></param>
        /// <returns></returns>
        BasicResponse LoadNetworkModuleCache(NetworkModuleCacheLoadRequest networkModuleCacheRequest);

        /// <summary>
        /// 添加网络模块缓存
        /// </summary>
        /// <param name="networkModuleCacheRequest"></param>
        /// <returns></returns>
        BasicResponse AddNetworkModuleCache(NetworkModuleCacheAddRequest networkModuleCacheRequest);

        /// <summary>
        /// 批量添加网络模块缓存
        /// </summary>
        /// <param name="networkModuleCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BacthAddNetworkModuleCache(NetworkModuleCacheBatchAddRequest networkModuleCacheRequest);

        /// <summary>
        /// 更新网络模块缓存
        /// </summary>
        /// <param name="networkModuleCacheRequest"></param>
        /// <returns></returns>
        BasicResponse UpdateNetworkModuleCahce(NetworkModuleCacheUpdateRequest networkModuleCacheRequest);

        /// <summary>
        /// 批量更新网络模块缓存
        /// </summary>
        /// <param name="networkModuleCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchUpdateNetworkModuleCache(NetworkModuleCacheBatchUpdateRequest networkModuleCacheRequest);

        /// <summary>
        /// 删除网络模块缓存
        /// </summary>
        /// <param name="networkModuleCacheRequest"></param>
        /// <returns></returns>
        BasicResponse DeleteNetworkModuleCache(NetworkModuleCacheDeleteRequest networkModuleCacheRequest);

        /// <summary>
        /// 获取所有网络模块缓存
        /// </summary>
        /// <param name="networkModuleCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_MacInfo>> GetAllNetworkModuleCache(NetworkModuleCacheGetAllRequest networkModuleCacheRequest);

        /// <summary>
        /// 根据Key(Point)获取网络模块缓存
        /// </summary>
        /// <param name="networkModuleCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<Jc_MacInfo> GetNetworkModuleCacheByKey(NetworkModuleCacheGetByKeyRequest networkModuleCacheRequest);

        /// <summary>
        /// 获取网络模块缓存
        /// </summary>
        /// <param name="networkModuleCacheRequest">条件</param>
        /// <returns></returns>
        BasicResponse<List<Jc_MacInfo>> GetNetworkModuleCache(NetworkModuleCacheGetByConditonRequest networkModuleCacheRequest);

        /// <summary>
        /// 更新网络模块放电状态
        /// </summary>
        /// <param name="networkModuleCacheRequest"></param>
        /// <returns></returns>
        BasicResponse UpdateNetworkModuleFdState(NetworkModuleCacheUpdateFdStateRequest networkModuleCacheRequest);

        /// <summary>
        /// 更新网络模块命令下发标记
        /// </summary>
        /// <param name="networkModuleCacheRequest"></param>
        /// <returns></returns>
        BasicResponse UpdateNetworkModuleNCommand(NetworkModuleCacheUpdateNCommandRequest networkModuleCacheRequest);
        /// <summary>
        /// 更新网络模块指定属性
        /// </summary>
        /// <param name="pointDefineCacheRequest"></param>
        /// <returns></returns>
        BasicResponse UpdateNetworkInfo(NetworkModuleCacheUpdatePropertiesRequest pointDefineCacheRequest);


        BasicResponse TestAlarm(TestAlarmRequest testAlarmRequest);
    }
}
