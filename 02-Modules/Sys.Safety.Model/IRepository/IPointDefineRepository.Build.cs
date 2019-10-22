using System.Collections.Generic;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IPointDefineRepository : IRepository<Jc_DefModel>
    {
        Jc_DefModel AddPointDefine(Jc_DefModel PointDefineModel);
        void UpdatePointDefine(Jc_DefModel PointDefineModel);
        void DeletePointDefine(string id);
        IList<Jc_DefModel> GetPointDefineList(int pageIndex, int pageSize, out int rowCount);
        List<Jc_DefModel> GetPointDefineList();
        Jc_DefModel GetPointDefineById(string id);

        IList<Jc_DefModel> GetPointDefineByPointId(string pointId);
    }
}