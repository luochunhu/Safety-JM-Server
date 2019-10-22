using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.R_Call;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IB_CallService
    {
        BasicResponse<B_CallInfo> AddCall(B_CallAddRequest callRequest);
        BasicResponse<B_CallInfo> UpdateCall(B_CallUpdateRequest callRequest);
        BasicResponse DeleteCall(B_CallDeleteRequest callRequest);
        BasicResponse<List<B_CallInfo>> GetCallList(B_CallGetListRequest callRequest);
        BasicResponse<B_CallInfo> GetCallById(B_CallGetRequest callRequest);

        BasicResponse<List<B_CallInfo>> GetAll(BasicRequest callRequest);

        BasicResponse<List<B_CallInfo>> GetAllCache();

        /// <summary>
        /// 获取所有bcall（融合系统接口）
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<B_CallInfo>> GetFusionCache();

        BasicResponse<List<B_CallInfo>> GetBCallInfoByMasterID(BCallInfoGetByMasterIDRequest callRequest);

        /// <summary>结束指定的bcall
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse EndBcallByBcallInfoList(EndBcallByBcallInfoListRequest request);

        /// <summary>
        /// 结束bcall数据库
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse EndBcallDbByBcallInfoList(EndBcallDbByBcallInfoListRequest request);

        /// <summary>
        /// 删除2天前已结束的bcall
        /// </summary>
        /// <returns></returns>
        BasicResponse DeleteFinishedBcall();
    }
}

