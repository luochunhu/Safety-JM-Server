using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Position;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Cache;

namespace Sys.Safety.ServiceContract
{
    public interface IPositionService
    {
        /// <summary>
        ///添加安装位置
        /// </summary>
        /// <param name="PositionRequest"></param>
        /// <returns></returns>
        BasicResponse AddPosition(PositionAddRequest PositionRequest);
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="PositionRequest"></param>
        /// <returns></returns>
        BasicResponse AddPositions(PositionsRequest PositionRequest);
        /// <summary>
        /// 安装位置更新
        /// </summary>
        /// <param name="PositionRequest"></param>
        /// <returns></returns>
        BasicResponse UpdatePosition(PositionUpdateRequest PositionRequest);
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="PositionRequest"></param>
        /// <returns></returns>
        BasicResponse UpdatePositions(PositionsRequest PositionRequest);
        BasicResponse DeletePosition(PositionDeleteRequest PositionRequest);
        BasicResponse<List<Jc_WzInfo>> GetPositionList(PositionGetListRequest PositionRequest);
        BasicResponse<List<Jc_WzInfo>> GetPositionList();
        BasicResponse<Jc_WzInfo> GetPositionById(PositionGetRequest PositionRequest);	
        /// <summary>
        /// 获取所有安装位置缓存
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<Jc_WzInfo>> GetAllPositionCache();
        /// <summary>
        /// 根据安装位置ID获取安装位置缓存
        /// </summary>
        /// <param name="positionCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_WzInfo>> GetPositionCacheByWzID(PositionGetByWzIDRequest PositionRequest);
        /// <summary>
        /// 根据安装位置获取安装位置缓存
        /// </summary>
        /// <param name="positionCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_WzInfo>> GetPositionCacheByWz(PositionGetByWzRequest PositionRequest);
        /// <summary>
        /// 获取缓存中当前最大的位置ID
        /// </summary>
        /// <returns></returns>
        BasicResponse<long> GetMaxPositionId();

        /// <summary>通过sql插入位置信息，并返回wzid
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse<string> AddPositionBySql(AddPositionBySqlRequest request);
    }
}

