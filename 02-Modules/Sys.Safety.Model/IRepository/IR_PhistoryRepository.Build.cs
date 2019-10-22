using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IR_PhistoryRepository : IRepository<R_PhistoryModel>
    {
        R_PhistoryModel AddPhistory(R_PhistoryModel phistoryModel);
        void UpdatePhistory(R_PhistoryModel phistoryModel);
        void DeletePhistory(string id);
        IList<R_PhistoryModel> GetPhistoryList(int pageIndex, int pageSize, out int rowCount);
        R_PhistoryModel GetPhistoryById(string id);

        R_PhistoryModel GetPhistoryByPar(string pointid, string yid, DateTime rtime);
    }
}
