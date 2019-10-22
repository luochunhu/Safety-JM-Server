using Basic.Framework.Web;
using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sys.Safety.Request.PersonCache;

namespace Sys.Safety.ServiceContract.KJ237Cache
{
    public interface IRPointDefineCacheService
    {
        /// <summary>
        /// 加载测点定义缓存
        /// </summary>
        /// <param name="pointDefineCacheRequest"></param>
        /// <returns></returns>
        BasicResponse LoadRPointDefineCache(RPointDefineCacheLoadRequest pointDefineCacheRequest);

        /// <summary>
        /// 添加测点定义缓存
        /// </summary>
        /// <param name="pointDefineCacheRequest"></param>
        /// <returns></returns>
        BasicResponse AddRPointDefineCache(RPointDefineCacheAddRequest pointDefineCacheRequest);

        /// <summary>
        /// 批量添加测点定义缓存
        /// </summary>
        /// <param name="pointDefineCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BacthAddRPointDefineCache(RPointDefineCacheBatchAddRequest pointDefineCacheRequest);

        /// <summary>
        /// 更新测点定义缓存
        /// </summary>
        /// <param name="pointDefineCacheRequest"></param>
        /// <returns></returns>
        BasicResponse UpdateRPointDefineCahce(RPointDefineCacheUpdateRequest pointDefineCacheRequest);

        /// <summary>
        /// 批量更新测点定义缓存
        /// </summary>
        /// <param name="pointDefineCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchRUpdatePointDefineCache(RPointDefineCacheBatchUpdateRequest pointDefineCacheRequest);

        /// <summary>
        /// 删除测点定义缓存
        /// </summary>
        /// <param name="pointDefineCacheRequest"></param>
        /// <returns></returns>
        BasicResponse DeleteRPointDefineCache(RPointDefineCacheDeleteRequest pointDefineCacheRequest);

        /// <summary>
        /// 批量删除测点定义缓存
        /// </summary>
        /// <param name="pointDefineCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchDeleteRPointDefineCache(RPointDefineCacheBatchDeleteRequest pointDefineCacheRequest);

        /// <summary>
        /// 获取所有测点定义缓存
        /// </summary>
        /// <param name="pointDefineCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetAllRPointDefineCache(RPointDefineCacheGetAllRequest pointDefineCacheRequest);

        /// <summary>
        /// 根据测点ID获取测点定义缓存
        /// </summary>
        /// <param name="pointDefineCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<Jc_DefInfo> PointDefineCacheByPointIdRequeest(RPointDefineCacheByPointIdRequeest pointDefineCacheRequest);

        /// <summary>
        /// 获取测点定义缓存
        /// </summary>
        /// <param name="pointDefineCacheRequest">条件</param>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetPointDefineCache(RPointDefineCacheGetByConditonRequest pointDefineCacheRequest);

        /// <summary>
        /// 部分更新测点信息缓存
        /// </summary>
        /// <param name="pointDefineCacheRequest"></param>
        /// <returns></returns>
        BasicResponse UpdateRPointDefineInfo(RDefineCacheUpdatePropertiesRequest pointDefineCacheRequest);

        /// <summary>
        /// 批量更新测点缓存字段
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse BatchUpdatePointDefineInfo(RDefineCacheBatchUpdatePropertiesRequest request);

    }
}
