using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.ServiceContract.Cache
{
    /// <summary>
    /// 作者:
    /// 时间:2017-07-27
    /// 描述:倍数报警缓存业务RPC接口
    /// </summary>
    public interface IRatioAlarmCacheService
    {
        /// <summary>
        /// 添加报警缓存
        /// </summary>
        /// <param name="alarmCacheRequest"></param>
        /// <returns></returns>
        BasicResponse AddAlarmCache(RatioAlarmCacheAddRequest alarmCacheRequest);

        /// <summary>
        /// 批量添加报警缓存
        /// </summary>
        /// <param name="alarmCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BacthAddAlarmCache(RatioAlarmCacheBatchAddRequest alarmCacheRequest);

        /// <summary>
        /// 更新报警缓存
        /// </summary>
        /// <param name="alarmCacheRequest"></param>
        /// <returns></returns>
        BasicResponse UpdateAlarmCahce(RatioAlarmCacheUpdateRequest alarmCacheRequest);

        /// <summary>
        /// 批量更新报警缓存
        /// </summary>
        /// <param name="alarmCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchUpdateAlarmCache(RatioAlarmCacheBatchUpdateRequest alarmCacheRequest);

        /// <summary>
        /// 获取所有报警缓存
        /// </summary>
        /// <param name="alarmCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<JC_MbInfo>> GetAllAlarmCache(RatioAlarmCacheGetAllRequest alarmCacheRequest);

        /// <summary>
        /// 根据Key(Name)获取缓存
        /// </summary>
        /// <param name="alarmCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<JC_MbInfo> GetAlarmCacheByKey(RatioAlarmCacheGetByKeyRequest alarmCacheRequest);

        /// <summary>
        /// 根据开始时间获取
        /// </summary>
        /// <param name="alarmCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<JC_MbInfo>> GetAlarmCacheByStime(RatioAlarmCacheGetByStimeRequest alarmCacheRequest);

        /// <summary>
        /// 获取报警缓存
        /// </summary>
        /// <param name="alarmCacheRequest">条件</param>
        /// <returns></returns>
        BasicResponse<List<JC_MbInfo>> GetAlarmCache(RatioAlarmCacheGetByConditonRequest alarmCacheRequest);

        /// <summary>
        /// 批量删除报警信息
        /// </summary>
        /// <param name="alarmCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchDeleteAlarmCache(RatioAlarmCacheBatchDeleteRequest alarmCacheRequest);

        /// <summary>
        /// 更新报警缓存部分属性
        /// </summary>
        /// <param name="alarmCacheRequest"></param>
        /// <returns></returns>
        BasicResponse UpdateAlarmInfoProperties(RatioAlarmCacheUpdatePropertiesRequest alarmCacheRequest);


    }
}
