using System.Collections.Generic;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.R_Phistory;

namespace Sys.Safety.ServiceContract
{
    public interface IR_PhistoryService
    {
        BasicResponse<R_PhistoryInfo> AddPhistory(R_PhistoryAddRequest phistoryRequest);
        BasicResponse<R_PhistoryInfo> UpdatePhistory(R_PhistoryUpdateRequest phistoryRequest);
        BasicResponse DeletePhistory(R_PhistoryDeleteRequest phistoryRequest);
        BasicResponse<List<R_PhistoryInfo>> GetPhistoryList(R_PhistoryGetListRequest phistoryRequest);
        BasicResponse<R_PhistoryInfo> GetPhistoryById(R_PhistoryGetRequest phistoryRequest);

        BasicResponse<R_PhistoryInfo> GetPhistoryByPar(R_PhistoryGetByParRequest phistoryRequest);
        /// <summary>
        /// 获取人员最后一条轨迹记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse<R_PhistoryInfo> GetPersonLastR_Phistory(R_PhistoryGetLastByYidRequest request);
        /// <summary>
        /// 根据存储时间查询轨迹记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse<List<R_PhistoryInfo>> GetPersonR_PhistoryByTimer(R_PhistoryGetLastByTimerRequest request);
    }
}

