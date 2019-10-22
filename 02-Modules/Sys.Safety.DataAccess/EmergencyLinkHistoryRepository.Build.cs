using System;
using System.Collections.Generic;
using System.Linq;
using Basic.Framework.Data;
using Sys.Safety.DataContract;
using Sys.Safety.Model;

namespace Sys.Safety.DataAccess
{
    public class EmergencyLinkHistoryRepository : RepositoryBase<EmergencyLinkHistoryModel>,
        IEmergencyLinkHistoryRepository
    {
        public EmergencyLinkHistoryModel AddEmergencyLinkHistory(EmergencyLinkHistoryModel emergencyLinkHistoryModel)
        {
            return Insert(emergencyLinkHistoryModel);
        }

        public void UpdateEmergencyLinkHistory(EmergencyLinkHistoryModel emergencyLinkHistoryModel)
        {
            Update(emergencyLinkHistoryModel);
        }

        public void DeleteEmergencyLinkHistory(string id)
        {
            Delete(id);
        }

        public IList<EmergencyLinkHistoryModel> GetEmergencyLinkHistoryList(int pageIndex, int pageSize,
            out int rowCount)
        {
            var emergencyLinkHistoryModelLists = Datas.ToList();
            rowCount = Datas.Count();
            return emergencyLinkHistoryModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }

        public EmergencyLinkHistoryModel GetEmergencyLinkHistoryById(string id)
        {
            var emergencyLinkHistoryModel = Datas.FirstOrDefault(c => c.Id == id);
            return emergencyLinkHistoryModel;
        }

        public IList<EmergencyLinkHistoryModel> GetNotEndEmergencyLinkHistory()
        {
            var time1900 = new DateTime(1900, 1, 1);
            var res = Datas.Where(a => a.EndTime == time1900).ToList();
            return res;
        }

        public EmergencyLinkHistoryModel GetLastLinkHistoryByLinkageId(string id)
        {
            var res = Datas.Where(a => a.SysEmergencyLinkageId == id).OrderByDescending(a => a.StartTime)
                .FirstOrDefault();
            return res;
        }

        public IList<EmergencyLinkHistoryModel> GetNotEndEmergencyLinkHistoryByLinkageId(string id)
        {
            var time1900 = new DateTime(1900, 1, 1);
            var res = Datas.Where(a => a.SysEmergencyLinkageId == id && a.EndTime == time1900).ToList();
            return res;
        }
    }
}