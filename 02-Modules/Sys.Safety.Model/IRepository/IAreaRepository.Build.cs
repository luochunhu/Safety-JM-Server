using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IAreaRepository : IRepository<AreaModel>
    {
        AreaModel AddArea(AreaModel areaModel);
        void UpdateArea(AreaModel areaModel);
        void DeleteArea(string id);
        IList<AreaModel> GetAreaList(int pageIndex, int pageSize, out int rowCount);
        AreaModel GetAreaById(string id);

        IList<AreaModel> GetAreaList();
    }
}
