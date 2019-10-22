using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class JC_MultiplesettingRepository : RepositoryBase<JC_MultiplesettingModel>, IJC_MultiplesettingRepository
    {

        public JC_MultiplesettingModel AddMultiplesetting(JC_MultiplesettingModel multiplesettingModel)
        {
            return base.Insert(multiplesettingModel);
        }
        public void UpdateMultiplesetting(JC_MultiplesettingModel multiplesettingModel)
        {
            base.Update(multiplesettingModel);
        }
        public void DeleteMultiplesetting(string id)
        {
            base.Delete(id);
        }
        public IList<JC_MultiplesettingModel> GetMultiplesettingList(int pageIndex, int pageSize, out int rowCount)
        {
            var multiplesettingModelLists = base.Datas;
            rowCount = multiplesettingModelLists.Count();
            return multiplesettingModelLists.OrderBy(p => p.Id).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public List<JC_MultiplesettingModel> GetAllMultiplesettingList()
        {
            var multiplesettingModelLists = base.Datas;            
            return multiplesettingModelLists.ToList();
        }
        public JC_MultiplesettingModel GetMultiplesettingById(string id)
        {
            JC_MultiplesettingModel multiplesettingModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return multiplesettingModel;
        }
    }
}
