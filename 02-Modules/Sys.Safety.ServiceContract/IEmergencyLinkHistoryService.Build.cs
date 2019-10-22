using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.EmergencyLinkHistory;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Listex;

namespace Sys.Safety.ServiceContract
{
    public interface IEmergencyLinkHistoryService
    {
        BasicResponse<EmergencyLinkHistoryInfo> AddEmergencyLinkHistory(EmergencyLinkHistoryAddRequest emergencyLinkHistoryRequest);
        BasicResponse<EmergencyLinkHistoryInfo> UpdateEmergencyLinkHistory(EmergencyLinkHistoryUpdateRequest emergencyLinkHistoryRequest);
        BasicResponse DeleteEmergencyLinkHistory(EmergencyLinkHistoryDeleteRequest emergencyLinkHistoryRequest);
        BasicResponse<List<EmergencyLinkHistoryInfo>> GetEmergencyLinkHistoryList(EmergencyLinkHistoryGetListRequest emergencyLinkHistoryRequest);
        BasicResponse<EmergencyLinkHistoryInfo> GetEmergencyLinkHistoryById(EmergencyLinkHistoryGetRequest emergencyLinkHistoryRequest);

        BasicResponse<EmergencyLinkHistoryInfo> GetEmergencyLinkHistoryByEmergency(EmergencyLinkHistoryGetByEmergencyRequest emergencyLinkHistoryRequest);

        /// <summary>批量增加历史记录
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse BatchAddEmergencyLinkHistory(BatchAddEmergencyLinkHistoryRequest request);

        /// <summary>结束所有联动
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse EndAll(EndAllRequest request);

        /// <summary>根据联动id结束所有联动
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse EndByLinkageId(EndByLinkageIdRequest request);

        /// <summary>根据应急联动id获取最后一次未结束的历史应急联动主控信息
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse<IList<EmergencyLinkageHistoryMasterPointAssInfo>>
            GetNotEndLastLinkageHistoryMasterPointByLinkageId(LongIdRequest request);

        /// <summary>获取所有已删除但未结束应急联动id
        /// 
        /// </summary>
        /// <returns></returns>
        BasicResponse<IList<SysEmergencyLinkageInfo>> GetDeleteButNotEndLinkageIds();

        /// <summary>新增应急联动历史记录及关联历史主控测点
        /// 
        /// </summary>
        /// <param name="requet"></param>
        /// <returns></returns>
        BasicResponse AddEmergencyLinkHistoryAndAss(AddEmergencyLinkHistoryAndAssRequest request);
    }
}

