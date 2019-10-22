using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IRightRepository : IRepository<RightModel>
    {
        RightModel AddRight(RightModel rightModel);
        void UpdateRight(RightModel rightModel);
        void DeleteRight(string id);
        IList<RightModel> GetRightList(int pageIndex, int pageSize, out int rowCount);
        List<RightModel> GetRightList();

        RightModel GetRightById(string id);
    }
}
