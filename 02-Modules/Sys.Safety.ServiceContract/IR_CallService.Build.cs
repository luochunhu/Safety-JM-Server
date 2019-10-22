using System.Collections.Generic;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.R_Call;
using Sys.Safety.Request.PersonCache;

namespace Sys.Safety.ServiceContract
{
    public interface IR_CallService
    {
        BasicResponse<R_CallInfo> AddCall(R_CallAddRequest callRequest);
        BasicResponse<R_CallInfo> UpdateCall(R_CallUpdateRequest callRequest);
        BasicResponse DeleteCall(R_CallDeleteRequest callRequest);
        BasicResponse<List<R_CallInfo>> GetCallList(R_CallGetListRequest callRequest);
        BasicResponse<R_CallInfo> GetCallById(R_CallGetRequest callRequest);

        BasicResponse<List<R_CallInfo>> GetAllCall();

        BasicResponse BachUpdateAlarmInfoProperties(R_CallUpdateProperitesRequest request);
        /// <summary>
        /// 获取所有人员呼叫缓存
        /// </summary>
        /// <param name="RCallCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<R_CallInfo>> GetAllRCallCache(RCallCacheGetAllRequest RCallCacheRequest);
        /// <summary>
        /// 根据ID获取人员呼叫缓存
        /// </summary>
        /// <param name="RCallCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<R_CallInfo> GetByKeyRCallCache(RCallCacheGetByKeyRequest RCallCacheRequest);

        /// <summary>
        /// 根据参数查询呼叫
        /// </summary>
        /// <param name="RCallCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<R_CallInfo>> GetRCallInfoByMasterID(RCallInfoGetByMasterIDRequest RCallCacheRequest);

        /// <summary>结束指定的rcall
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse EndRcallByRcallInfoList(EndRcallByRcallInfoListEequest request);

        BasicResponse EndRcallDbByRcallInfoList(EndRcallDbByRcallInfoListEequest request);

        BasicResponse DeleteFinishedBcall();
    }
}

