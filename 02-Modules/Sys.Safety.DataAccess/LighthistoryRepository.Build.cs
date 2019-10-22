using System.Collections.Generic;
using System.Linq;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.DataAccess
{
    public class LighthistoryRepository : RepositoryBase<LighthistoryModel>, ILighthistoryRepository
    {
        public LighthistoryModel AddLighthistory(LighthistoryModel lighthistoryModel)
        {
            return Insert(lighthistoryModel);
        }

        public void UpdateLighthistory(LighthistoryModel lighthistoryModel)
        {
            Update(lighthistoryModel);
        }

        public void DeleteLighthistory(string id)
        {
            Delete(id);
        }

        public IList<LighthistoryModel> GetLighthistoryList(int pageIndex, int pageSize, out int rowCount)
        {
            var lighthistoryModelLists = Datas;
            rowCount = lighthistoryModelLists.Count();
            return lighthistoryModelLists.OrderBy(a => a.Timer).Skip(pageIndex*pageSize).Take(pageSize).ToList();
        }

        public LighthistoryModel GetLighthistoryById(string id)
        {
            var lighthistoryModel = Datas.FirstOrDefault(c => c.ID == id);
            return lighthistoryModel;
        }
    }
}