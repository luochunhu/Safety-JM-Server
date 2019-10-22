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
    /// 描述:测点定义缓存业务RPC接口
    /// 修改记录
    /// 2017-05-23
    /// 2017-05-26
    /// </summary>
    public interface IPointDefineCacheService
    {
        /// <summary>
        /// 加载测点定义缓存
        /// </summary>
        /// <param name="pointDefineCacheRequest"></param>
        /// <returns></returns>
        BasicResponse LoadPointDefineCache(PointDefineCacheLoadRequest pointDefineCacheRequest);

        /// <summary>
        /// 添加测点定义缓存
        /// </summary>
        /// <param name="pointDefineCacheRequest"></param>
        /// <returns></returns>
        BasicResponse AddPointDefineCache(PointDefineCacheAddRequest pointDefineCacheRequest);

        /// <summary>
        /// 批量添加测点定义缓存
        /// </summary>
        /// <param name="pointDefineCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BacthAddPointDefineCache(PointDefineCacheBatchAddRequest pointDefineCacheRequest);

        /// <summary>
        /// 更新测点定义缓存
        /// </summary>
        /// <param name="pointDefineCacheRequest"></param>
        /// <returns></returns>
        BasicResponse UpdatePointDefineCahce(PointDefineCacheUpdateRequest pointDefineCacheRequest);

        /// <summary>
        /// 批量更新测点定义缓存
        /// </summary>
        /// <param name="pointDefineCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchUpdatePointDefineCache(PointDefineCacheBatchUpdateRequest pointDefineCacheRequest);

        /// <summary>
        /// 删除测点定义缓存
        /// </summary>
        /// <param name="pointDefineCacheRequest"></param>
        /// <returns></returns>
        BasicResponse DeletePointDefineCache(PointDefineCacheDeleteRequest pointDefineCacheRequest);

        /// <summary>
        /// 批量删除测点定义缓存
        /// </summary>
        /// <param name="pointDefineCacheRequest"></param>
        /// <returns></returns>
        BasicResponse BatchDeletePointDefineCache(PointDefineCacheBatchDeleteRequest pointDefineCacheRequest);

        /// <summary>
        /// 获取所有测点定义缓存
        /// </summary>
        /// <param name="pointDefineCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetAllPointDefineCache(PointDefineCacheGetAllRequest pointDefineCacheRequest);

        /// <summary>
        /// 根据Key(Point)获取测点定义缓存
        /// </summary>
        /// <param name="pointDefineCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<Jc_DefInfo> GetPointDefineCacheByKey(PointDefineCacheGetByKeyRequest pointDefineCacheRequest);

        /// <summary>
        /// 根据测点编号获取测点定义缓存
        /// </summary>
        /// <param name="pointDefineCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<Jc_DefInfo> PointDefineCacheByPointIdRequeest(PointDefineCacheByPointIdRequeest pointDefineCacheRequest);

        /// <summary>
        /// 获取测点定义缓存
        /// </summary>
        /// <param name="pointDefineCacheRequest">条件</param>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetPointDefineCache(PointDefineCacheGetByConditonRequest pointDefineCacheRequest);

        /// <summary>
        /// 根据分站号获取测点定义缓存
        /// </summary>
        /// <param name="pointDefineCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByStation(PointDefineCacheGetByStationRequest pointDefineCacheRequest);

        /// <summary>
        /// 更新测点定义缓存控制信息
        /// </summary>
        /// <returns></returns>
        BasicResponse UpdatePointDefineControl(PointDefineCacheUpdateControlReqest pointDefineCacheRequest);

        /// <summary>
        /// 更新测点定义缓存分站信息
        /// </summary>
        /// <returns></returns>
        BasicResponse UpdatePointDefineStationFlag(PointDefineCacheUpdateStationFlatReqest pointDefineCacheRequest);

        /// <summary>
        /// 更新测点定义缓存通讯次数
        /// </summary>
        /// <returns></returns>
        BasicResponse UpdatePointDefineCommunicateTimes(PointDefineCacheUpdateCommTimesReqest pointDefineCacheRequest);

        /// <summary>
        /// 更新测点定义缓存唯一编码
        /// </summary>
        /// <returns></returns>
        BasicResponse UpdatePointDefineUniqueCode(PointDefineCacheUpdateUniqueCodeReqest pointDefineCacheRequest);

        /// <summary>
        /// 更新测点定义缓存初始化信息
        /// </summary>
        /// <returns></returns>
        BasicResponse UpdatePointDefineInitInfo(PointDefineCacheUpdateInitInfoReqest pointDefineCacheRequest);

        /// <summary>
        /// 更新测点定义缓存错误次数
        /// </summary>
        /// <returns></returns>
        BasicResponse UpdatePointDefineErrorCount(PointDefineCacheUpdateErrorCountReqest pointDefineCacheRequest);

        /// <summary>
        /// 更新测点定义缓存实时信息
        /// </summary>
        /// <param name="pointDefineCacheRequest"></param>
        /// <returns></returns>
        BasicResponse UpdatePointDefineRealValue(PointDefineCacheUpdateRealValueReqest pointDefineCacheRequest);

        /// <summary>
        /// 更新测点定义修改设备地址链表
        /// </summary>
        /// <param name="pointDefineCacheRequest"></param>
        /// <returns></returns>
        BasicResponse UpdateModifyItems(PointDefineCacheUpdateRequest pointDefineCacheRequest);

        /// <summary>
        /// 部分更新测点信息缓存
        /// </summary>
        /// <param name="pointDefineCacheRequest"></param>
        /// <returns></returns>
        BasicResponse UpdatePointDefineInfo(DefineCacheUpdatePropertiesRequest pointDefineCacheRequest);

        /// <summary>
        /// 批量更新测点缓存字段
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse BatchUpdatePointDefineInfo(DefineCacheBatchUpdatePropertiesRequest request);

        void Stop();
    }
}
