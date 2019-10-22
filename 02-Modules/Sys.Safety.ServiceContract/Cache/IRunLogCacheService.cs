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
    /// 描述:运行记录缓存业务RPC接口
    /// 修改记录
    /// 2017-05-23
    /// </summary>
    public interface IRunLogCacheService
    {
        /// <summary>
        /// 添加运行日志缓存
        /// </summary>
        /// <param name="runLogCacheRequest"></param>
        /// <returns></returns>
        BasicResponse AddRunLogCache(RunLogCacheAddRequest runLogCacheRequest);

        /// <summary>
        /// 批量添加运行日志缓存
        /// </summary>
        /// <param name="runLogCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchAddRunLogCache(RunLogCacheBatchAddRequest runLogCacheRequest);

        /// <summary>
        /// 更新运行日志缓存
        /// </summary>
        /// <param name="runLogCacheRequest"></param>
        /// <returns></returns>
        BasicResponse UpdateRunLogCache(RunLogCacheUpdateRequest runLogCacheRequest);

        /// <summary>
        /// 批量更新运行日志缓存
        /// </summary>
        /// <param name="runLogCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchUpdateRunLogCache(RunLogCacheBatchUpdateRequest runLogCacheRequest);

        /// <summary>
        /// 获取所有运行日志缓存
        /// </summary>
        /// <param name="runLogCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_RInfo>> GetAllRunLogCache(RunLogCacheGetAllRequest runLogCacheRequest);

        /// <summary>
        /// 根据Key获取运行日志缓存
        /// </summary>
        /// <param name="runLogCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<Jc_RInfo> GetRunLogCacheByKey(RunLogCacheGetByKeyRequest runLogCacheRequest);

        /// <summary>
        /// 根据条件获取运行日志缓存
        /// </summary>
        /// <param name="runLogCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_RInfo>> GetRunLogCache(RunLogCacheGetByConditionRequest runLogCacheRequest);

        /// <summary>
        /// 停止清除运行日志线程
        /// </summary>
        /// <param name="runLogCacheRequest"></param>
        /// <returns></returns>
        BasicResponse StopCleanRunlogCacheThread(RunLogCacheStopCleanRunLogRequest runLogCacheRequest);

        BasicResponse BatchDeleteRunLogCache(RunLogBatchDeleteRequest runLogCacheRequest);
    }
}
