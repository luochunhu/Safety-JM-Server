using System.Collections.Generic;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IDeviceKoriyasuRepository : IRepository<Jc_DefwbModel>
    {
        Jc_DefwbModel AddDeviceKoriyasu(Jc_DefwbModel jc_DefwbModel);
        void UpdateDeviceKoriyasu(Jc_DefwbModel jc_DefwbModel);
        void DeleteDeviceKoriyasu(string id);
        IList<Jc_DefwbModel> GetDeviceKoriyasuList(int pageIndex, int pageSize, out int rowCount);
        Jc_DefwbModel GetDeviceKoriyasuById(string id);
    }
}