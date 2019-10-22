using System.Collections.Generic;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.R_Preal;
using Sys.Safety.Request.PersonCache;

namespace Sys.Safety.ServiceContract
{
    public interface IR_PrealService
    {
        BasicResponse<R_PrealInfo> AddPreal(R_PrealAddRequest prealRequest);
        BasicResponse<R_PrealInfo> UpdatePreal(R_PrealUpdateRequest prealRequest);
        BasicResponse DeletePreal(R_PrealDeleteRequest prealRequest);
        BasicResponse<List<R_PrealInfo>> GetPrealList(R_PrealGetListRequest prealRequest);
        BasicResponse<R_PrealInfo> GetPrealById(R_PrealGetRequest prealRequest);
        /// <summary>
        /// 获取所有人员实时缓存
        /// </summary>
        /// <param name="prealRequest"></param>
        /// <returns></returns>
        BasicResponse<List<R_PrealInfo>> GetAllPrealCacheList(RPrealCacheGetAllRequest RealCacheRequest);
        /// <summary>
        /// 获取所有报警人员实时缓存
        /// </summary>
        /// <param name="prealRequest"></param>
        /// <returns></returns>
        BasicResponse<List<R_PrealInfo>> GetAllAlarmPrealCacheList();

        /// <summary>
        /// 老人员定位系统人员实时信息同步
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        BasicResponse OldPlsPersonRealSync(OldPlsPersonRealSyncRequest request);
    }
}

