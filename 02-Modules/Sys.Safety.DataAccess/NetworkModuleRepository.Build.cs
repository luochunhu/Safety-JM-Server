using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class NetworkModuleRepository : RepositoryBase<Jc_MacModel>, INetworkModuleRepository
    {

        public Jc_MacModel AddNetworkModule(Jc_MacModel NetworkModuleModel)
        {
            return base.Insert(NetworkModuleModel);
        }
        public void UpdateNetworkModule(Jc_MacModel NetworkModuleModel)
        {
            base.Update(NetworkModuleModel);
        }
        public void DeleteNetworkModule(string id)
        {
            base.Delete(id);
        }
        public IList<Jc_MacModel> GetNetworkModuleList(int pageIndex, int pageSize, out int rowCount)
        {
            var jc_MacModelLists = base.Datas;
            rowCount = jc_MacModelLists.Count();
            return jc_MacModelLists.OrderBy(p => p.ID).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public List<Jc_MacModel> GetNetworkModuleList()
        {
            var jc_MacModelLists = base.Datas.ToList();           
            return jc_MacModelLists;
        }
        public Jc_MacModel GetNetworkModuleById(string id)
        {
            Jc_MacModel jc_MacModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return jc_MacModel;
        }
    }
}
