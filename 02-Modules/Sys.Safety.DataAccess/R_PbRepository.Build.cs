using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class R_PbRepository : RepositoryBase<R_PbModel>, IR_PbRepository
    {

        public R_PbModel AddPb(R_PbModel pbModel)
        {
            return base.Insert(pbModel);
        }
        public void UpdatePb(R_PbModel pbModel)
        {
            base.Update(pbModel);
        }
        public void DeletePb(string id)
        {
            base.Delete(id);
        }
        public IList<R_PbModel> GetPbList(int pageIndex, int pageSize, out int rowCount)
        {
            var pbModelLists = base.Datas.ToList();
            rowCount = base.Datas.Count();
            return pbModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public R_PbModel GetPbById(string id)
        {
            R_PbModel pbModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return pbModel;
        }


        public void BachUpdatePb(List<R_PbModel> pbModels)
        {
            base.Update(pbModels);
        }
    }
}
