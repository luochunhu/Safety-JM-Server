using System.Collections.Generic;
using System.Linq;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.DataAccess
{
    public class DeviceKoriyasuRepository : RepositoryBase<Jc_DefwbModel>, IDeviceKoriyasuRepository
    {
        public Jc_DefwbModel AddDeviceKoriyasu(Jc_DefwbModel jc_DefwbModel)
        {
            return Insert(jc_DefwbModel);
        }

        public void UpdateDeviceKoriyasu(Jc_DefwbModel jc_DefwbModel)
        {
            Update(jc_DefwbModel);
        }

        public void DeleteDeviceKoriyasu(string id)
        {
            Delete(id);
        }

        public IList<Jc_DefwbModel> GetDeviceKoriyasuList(int pageIndex, int pageSize, out int rowCount)
        {
            var jc_DefwbModelLists = Datas;
            rowCount = jc_DefwbModelLists.Count();
            return jc_DefwbModelLists.Skip(pageIndex*pageSize).Take(pageSize).ToList();
        }

        public Jc_DefwbModel GetDeviceKoriyasuById(string id)
        {
            var jc_DefwbModel = Datas.FirstOrDefault(c => c.ID == id);
            return jc_DefwbModel;
        }
    }
}