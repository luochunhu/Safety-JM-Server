using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class R_CallRepository : RepositoryBase<R_CallModel>, IR_CallRepository
    {

        public R_CallModel AddCall(R_CallModel callModel)
        {
            return base.Insert(callModel);
        }
        public void UpdateCall(R_CallModel callModel)
        {
            base.Update(callModel);
        }
        public void DeleteCall(string id)
        {
            base.Delete(id);
        }
        public IList<R_CallModel> GetCallList(int pageIndex, int pageSize, out int rowCount)
        {
            var callModelLists = base.Datas.ToList();
            rowCount = base.Datas.Count();
            return callModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public R_CallModel GetCallById(string id)
        {
            R_CallModel callModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return callModel;
        }
    }
}
