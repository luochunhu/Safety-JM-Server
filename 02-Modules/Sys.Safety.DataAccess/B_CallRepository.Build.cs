using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class B_CallRepository : RepositoryBase<B_CallModel>, IB_CallRepository
    {

        public B_CallModel AddCall(B_CallModel callModel)
        {
            return base.Insert(callModel);
        }
        public void UpdateCall(B_CallModel callModel)
        {
            base.Update(callModel);
        }
        public void DeleteCall(string id)
        {
            base.Delete(id);
        }
        public IList<B_CallModel> GetCallList(int pageIndex, int pageSize, out int rowCount)
        {
            var callModelLists = base.Datas.ToList();
            rowCount = base.Datas.Count();
            return callModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public B_CallModel GetCallById(string id)
        {
            B_CallModel callModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return callModel;
        }

        public IList<B_CallModel> GetAllCall()
        {
            return Datas.ToList();
        }
    }
}
