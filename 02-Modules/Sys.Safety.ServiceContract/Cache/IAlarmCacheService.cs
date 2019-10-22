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
    /// 时间:2017-05-22
    /// 描述:报警缓存业务RPC接口
    /// 修改记录
    /// 2017-05-23
    /// </summary>
    public interface IAlarmCacheService
    {
        /// <summary>
        /// 加载报警缓存
        /// </summary>
        /// <param name="alarmCacheRequest"></param>
        /// <returns></returns>
        BasicResponse LoadAlarmCache(AlarmCacheLoadRequest alarmCacheRequest);

        /// <summary>
        /// 添加报警缓存
        /// </summary>
        /// <param name="alarmCacheRequest"></param>
        /// <returns></returns>
        BasicResponse AddAlarmCache(AlarmCacheAddRequest alarmCacheRequest);

        /// <summary>
        /// 批量添加报警缓存
        /// </summary>
        /// <param name="alarmCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BacthAddAlarmCache(AlarmCacheBatchAddRequest alarmCacheRequest);

        /// <summary>
        /// 更新报警缓存
        /// </summary>
        /// <param name="alarmCacheRequest"></param>
        /// <returns></returns>
        BasicResponse UpdateAlarmCahce(AlarmCacheUpdateRequest alarmCacheRequest);

        /// <summary>
        /// 批量更新报警缓存
        /// </summary>
        /// <param name="alarmCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchUpdateAlarmCache(AlarmCacheBatchUpdateRequest alarmCacheRequest);

        /// <summary>
        /// 删除所有报警缓存
        /// </summary>
        /// <param name="alarmCacheRequest"></param>
        /// <returns></returns>
        BasicResponse DeleteAllAlarmCache(AlarmCacheDeleteAllRequest alarmCacheRequest);

        /// <summary>
        /// 获取所有报警缓存
        /// </summary>
        /// <param name="alarmCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_BInfo>> GetAllAlarmCache(AlarmCacheGetAllRequest alarmCacheRequest);

        /// <summary>
        /// 根据Key(Name)获取缓存
        /// </summary>
        /// <param name="alarmCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<Jc_BInfo> GetAlarmCacheByKey(AlarmCacheGetByKeyRequest alarmCacheRequest);

        /// <summary>
        /// 获取报警缓存
        /// </summary>
        /// <param name="alarmCacheRequest">条件</param>
        /// <returns></returns>
        BasicResponse<List<Jc_BInfo>> GetAlarmCache(AlarmCacheGetByConditonRequest alarmCacheRequest);

        /// <summary>
        /// 停止清除报警数据缓存线程
        /// </summary>
        /// <param name="alarmCacheRequest"></param>
        /// <returns></returns>
        BasicResponse StopCleanAlarmCacheThread(AlarmCacheStopCleanRequest alarmCacheRequest);

        /// <summary>
        /// 批量删除报警信息
        /// </summary>
        /// <param name="alarmCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchDeleteAlarmCache(AlarmCacheBatchDeleteRequest alarmCacheRequest);

        /// <summary>
        /// 更新报警缓存部分属性
        /// </summary>
        /// <param name="alarmCacheRequest"></param>
        /// <returns></returns>
        BasicResponse UpdateAlarmInfoProperties(AlarmCacheUpdatePropertiesRequest alarmCacheRequest);

    }
}
