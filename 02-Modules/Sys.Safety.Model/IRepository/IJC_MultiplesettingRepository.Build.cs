using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IJC_MultiplesettingRepository : IRepository<JC_MultiplesettingModel>
    {
        JC_MultiplesettingModel AddMultiplesetting(JC_MultiplesettingModel multiplesettingModel);
        void UpdateMultiplesetting(JC_MultiplesettingModel multiplesettingModel);
        void DeleteMultiplesetting(string id);
        IList<JC_MultiplesettingModel> GetMultiplesettingList(int pageIndex, int pageSize, out int rowCount);
        List<JC_MultiplesettingModel> GetAllMultiplesettingList();
        JC_MultiplesettingModel GetMultiplesettingById(string id);
    }
}
