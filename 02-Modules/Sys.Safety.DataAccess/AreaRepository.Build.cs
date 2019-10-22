using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class AreaRepository : RepositoryBase<AreaModel>, IAreaRepository
    {

        public AreaModel AddArea(AreaModel areaModel)
        {
            return base.Insert(areaModel);
        }
        public void UpdateArea(AreaModel areaModel)
        {
            base.Update(areaModel);
        }
        public void DeleteArea(string id)
        {
            base.Delete(id);
        }
        public IList<AreaModel> GetAreaList(int pageIndex, int pageSize, out int rowCount)
        {
            var areaModelLists = base.Datas;
            rowCount = areaModelLists.Count();
            return areaModelLists.OrderBy(p => p.CreateUpdateTime).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public AreaModel GetAreaById(string id)
        {
            AreaModel areaModel = base.Datas.FirstOrDefault(c => c.Areaid == id);
            return areaModel;
        }


        public IList<AreaModel> GetAreaList()
        {
            var areaModelLists = base.Datas;
            return areaModelLists.ToList();
        }
    }
}
