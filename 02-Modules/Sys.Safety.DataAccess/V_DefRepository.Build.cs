using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class V_DefRepository : RepositoryBase<V_DefModel>, IV_DefRepository
    {

        public V_DefModel AddDef(V_DefModel defModel)
        {
            return base.Insert(defModel);
        }
        public void UpdateDef(V_DefModel defModel)
        {
            base.Update(defModel);
        }
        public void DeleteDef(string id)
        {
            base.Delete(id);
        }
        public IList<V_DefModel> GetDefList(int pageIndex, int pageSize, out int rowCount)
        {
            var defModelLists = base.Datas.ToList();
            rowCount = base.Datas.Count();
            return defModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public V_DefModel GetDefById(string id)
        {
            V_DefModel defModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return defModel;
        }
    }
}
