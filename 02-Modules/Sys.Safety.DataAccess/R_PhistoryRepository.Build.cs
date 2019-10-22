using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class R_PhistoryRepository : RepositoryBase<R_PhistoryModel>, IR_PhistoryRepository
    {

        public R_PhistoryModel AddPhistory(R_PhistoryModel phistoryModel)
        {
            return base.Insert(phistoryModel);
        }
        public void UpdatePhistory(R_PhistoryModel phistoryModel)
        {
            base.Update(phistoryModel);
        }
        public void DeletePhistory(string id)
        {
            base.Delete(id);
        }
        public IList<R_PhistoryModel> GetPhistoryList(int pageIndex, int pageSize, out int rowCount)
        {
            var phistoryModelLists = base.Datas.ToList();
            rowCount = base.Datas.Count();
            return phistoryModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public R_PhistoryModel GetPhistoryById(string id)
        {
            R_PhistoryModel phistoryModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return phistoryModel;
        }


        public R_PhistoryModel GetPhistoryByPar(string pointid, string yid, DateTime rtime)
        {
            R_PhistoryModel phistoryModel = base.Datas.FirstOrDefault(c => c.Pointid == pointid && c.Yid == yid && c.Rtime == rtime);
            return phistoryModel;
        }
    }
}
