using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IPositionRepository : IRepository<Jc_WzModel>
    {
        Jc_WzModel AddPosition(Jc_WzModel PositionModel);
        void UpdatePosition(Jc_WzModel PositionModel);
        void DeletePosition(string id);
        IList<Jc_WzModel> GetPositionList(int pageIndex, int pageSize, out int rowCount);
        List<Jc_WzModel> GetPositionList();
        Jc_WzModel GetPositionById(string id);
    }
}
