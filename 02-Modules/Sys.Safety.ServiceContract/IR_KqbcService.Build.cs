using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Kqbc;
using Sys.Safety.DataContract;
using Sys.Safety.Request.PersonCache;

namespace Sys.Safety.ServiceContract
{
    public interface IR_KqbcService
    {
        BasicResponse<R_KqbcInfo> AddKqbc(R_KqbcAddRequest kqbcRequest);
        BasicResponse<R_KqbcInfo> UpdateKqbc(R_KqbcUpdateRequest kqbcRequest);
        BasicResponse DeleteKqbc(R_KqbcDeleteRequest kqbcRequest);
        BasicResponse<List<R_KqbcInfo>> GetKqbcList(R_KqbcGetListRequest kqbcRequest);
        BasicResponse<List<R_KqbcInfo>> GetAllKqbcList();
        BasicResponse<R_KqbcInfo> GetKqbcById(R_KqbcGetRequest kqbcRequest);
        /// <summary>
        /// 获取所有考勤班次缓存列表
        /// </summary>
        /// <param name="RKqbcCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<R_KqbcInfo>> GetAllKqbcCacheList(RKqbcCacheGetAllRequest RKqbcCacheRequest);
        /// <summary>
        /// 获取默认班次
        /// </summary>
        /// <param name="RKqbcCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<R_KqbcInfo> GetDefaultKqbcCache(RKqbcCacheGetByConditionRequest RKqbcCacheRequest);
    }
}

