using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IEmergencyLinkHistoryRepository : IRepository<EmergencyLinkHistoryModel>
    {
        EmergencyLinkHistoryModel AddEmergencyLinkHistory(EmergencyLinkHistoryModel emergencyLinkHistoryModel);
        void UpdateEmergencyLinkHistory(EmergencyLinkHistoryModel emergencyLinkHistoryModel);
        void DeleteEmergencyLinkHistory(string id);
        IList<EmergencyLinkHistoryModel> GetEmergencyLinkHistoryList(int pageIndex, int pageSize, out int rowCount);
        EmergencyLinkHistoryModel GetEmergencyLinkHistoryById(string id);

        /// <summary>
        /// 获取所有未结束的记录
        /// </summary>
        /// <returns></returns>
        IList<EmergencyLinkHistoryModel> GetNotEndEmergencyLinkHistory();

        /// <summary>根据应急联动id获取未结束的记录
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IList<EmergencyLinkHistoryModel> GetNotEndEmergencyLinkHistoryByLinkageId(string id);

        /// <summary>
        /// 根据应急联动id获取最后一条历史记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        EmergencyLinkHistoryModel GetLastLinkHistoryByLinkageId(string id);

        
    }
}
