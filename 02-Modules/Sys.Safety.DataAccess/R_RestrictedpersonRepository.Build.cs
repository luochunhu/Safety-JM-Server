using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class R_RestrictedpersonRepository : RepositoryBase<R_RestrictedpersonModel>, IR_RestrictedpersonRepository
    {

        public R_RestrictedpersonModel AddRestrictedperson(R_RestrictedpersonModel restrictedpersonModel)
        {
            return base.Insert(restrictedpersonModel);
        }
        public void UpdateRestrictedperson(R_RestrictedpersonModel restrictedpersonModel)
        {
            base.Update(restrictedpersonModel);
        }
        public void DeleteRestrictedperson(string id)
        {
            base.Delete(id);
        }
        public void DeleteRestrictedpersonByPointId(string PointId)
        {
            base.Delete(a => a.PointId == PointId);
        }
        public IList<R_RestrictedpersonModel> GetRestrictedpersonList(int pageIndex, int pageSize, out int rowCount)
        {
            var restrictedpersonModelLists = base.Datas.ToList();
            rowCount = base.Datas.Count();
            return restrictedpersonModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public R_RestrictedpersonModel GetRestrictedpersonById(string id)
        {
            R_RestrictedpersonModel restrictedpersonModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return restrictedpersonModel;
        }
    }
}
