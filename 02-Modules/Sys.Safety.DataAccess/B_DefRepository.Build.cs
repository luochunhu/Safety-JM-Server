using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class B_DefRepository : RepositoryBase<B_DefModel>, IB_DefRepository
    {

        public B_DefModel AddDef(B_DefModel defModel)
        {
            return base.Insert(defModel);
        }
        public void UpdateDef(B_DefModel defModel)
        {
            base.Update(defModel);
        }
        public void DeleteDef(string id)
        {
            base.Delete(id);
        }
        public IList<B_DefModel> GetDefList(int pageIndex, int pageSize, out int rowCount)
        {
            var defModelLists = base.Datas.ToList();
            rowCount = base.Datas.Count();
            return defModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public B_DefModel GetDefById(string id)
        {
            B_DefModel defModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return defModel;
        }
    }
}
