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
    
    public interface IR_PBCacheService
    {
        /// <summary>
        /// 加载报警缓存
        /// </summary>
        /// <param name="R_PBCacheRequest"></param>
        /// <returns></returns>
        BasicResponse LoadR_PBCache(R_PBCacheLoadRequest R_PBCacheRequest);

        /// <summary>
        /// 添加报警缓存
        /// </summary>
        /// <param name="R_PBCacheRequest"></param>
        /// <returns></returns>
        BasicResponse AddR_PBCache(R_PBCacheAddRequest R_PBCacheRequest);

        /// <summary>
        /// 批量添加报警缓存
        /// </summary>
        /// <param name="R_PBCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BacthAddR_PBCache(R_PBCacheBatchAddRequest R_PBCacheRequest);

        /// <summary>
        /// 更新报警缓存
        /// </summary>
        /// <param name="R_PBCacheRequest"></param>
        /// <returns></returns>
        BasicResponse UpdateAlarmCahce(R_PBCacheUpdateRequest R_PBCacheRequest);

        /// <summary>
        /// 批量更新报警缓存
        /// </summary>
        /// <param name="R_PBCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchUpdateR_PBCache(R_PBCacheBatchUpdateRequest R_PBCacheRequest);

        /// <summary>
        /// 删除所有报警缓存
        /// </summary>
        /// <param name="R_PBCacheRequest"></param>
        /// <returns></returns>
        BasicResponse DeleteAllR_PBCache(R_PBCacheDeleteAllRequest R_PBCacheRequest);

        /// <summary>
        /// 获取所有报警缓存
        /// </summary>
        /// <param name="R_PBCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<R_PbInfo>> GetAllR_PBCache(R_PBCacheGetAllRequest R_PBCacheRequest);

        /// <summary>
        /// 根据Key(Name)获取缓存
        /// </summary>
        /// <param name="R_PBCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<R_PbInfo> GetR_PBCacheByKey(R_PBCacheGetByKeyRequest R_PBCacheRequest);

        /// <summary>
        /// 获取报警缓存
        /// </summary>
        /// <param name="R_PBCacheRequest">条件</param>
        /// <returns></returns>
        BasicResponse<List<R_PbInfo>> GetR_PBCache(R_PBCacheGetByConditonRequest R_PBCacheRequest);

        /// <summary>
        /// 停止清除报警数据缓存线程
        /// </summary>
        /// <param name="R_PBCacheRequest"></param>
        /// <returns></returns>
        BasicResponse StopCleanR_PBCacheThread(R_PBCacheStopCleanRequest R_PBCacheRequest);

        /// <summary>
        /// 批量删除报警信息
        /// </summary>
        /// <param name="R_PBCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchDeleteR_PBCache(R_PBCacheBatchDeleteRequest R_PBCacheRequest);

        /// <summary>
        /// 更新报警缓存部分属性
        /// </summary>
        /// <param name="R_PBCacheRequest"></param>
        /// <returns></returns>
        BasicResponse UpdateAlarmInfoProperties(R_PBCacheUpdatePropertiesRequest R_PBCacheRequest);

    }
}
