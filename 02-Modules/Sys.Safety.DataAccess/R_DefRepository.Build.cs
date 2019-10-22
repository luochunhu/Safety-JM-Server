using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class R_DefRepository : RepositoryBase<R_DefModel>, IR_DefRepository
    {

        public R_DefModel AddDef(R_DefModel defModel)
        {
            return base.Insert(defModel);
        }
        public void UpdateDef(R_DefModel defModel)
        {
            base.Update(defModel);
        }
        public void DeleteDef(string id)
        {
            //base.Delete(id);
            base.ExecuteNonQuery("global_DeleteRDef", id);
        }
        public IList<R_DefModel> GetDefList(int pageIndex, int pageSize, out int rowCount)
        {
            var defModelLists = base.Datas.ToList();
            rowCount = base.Datas.Count();
            return defModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public R_DefModel GetDefById(string id)
        {
            R_DefModel defModel = base.Datas.FirstOrDefault(c => c.PointID == id);
            return defModel;
        }
    }
}
