using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface INetworkModuleRepository : IRepository<Jc_MacModel>
    {
        Jc_MacModel AddNetworkModule(Jc_MacModel NetworkModuleModel);
        void UpdateNetworkModule(Jc_MacModel NetworkModuleModel);
        void DeleteNetworkModule(string id);
        IList<Jc_MacModel> GetNetworkModuleList(int pageIndex, int pageSize, out int rowCount);
        List<Jc_MacModel> GetNetworkModuleList();
        Jc_MacModel GetNetworkModuleById(string id);
    }
}
