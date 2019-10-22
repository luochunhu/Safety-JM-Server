using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class R_PrealRepository : RepositoryBase<R_PrealModel>, IR_PrealRepository
    {

        public R_PrealModel AddPreal(R_PrealModel prealModel)
        {
            return base.Insert(prealModel);
        }
        public void UpdatePreal(R_PrealModel prealModel)
        {
            base.Update(prealModel);
        }
        public void DeletePreal(string id)
        {
            base.Delete(id);
        }
        public IList<R_PrealModel> GetPrealList(int pageIndex, int pageSize, out int rowCount)
        {
            var prealModelLists = base.Datas.ToList();
            rowCount = base.Datas.Count();
            return prealModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public R_PrealModel GetPrealById(string id)
        {
            R_PrealModel prealModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return prealModel;
        }
    }
}
