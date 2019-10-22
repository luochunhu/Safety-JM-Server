using System.Collections.Generic;
using System.Linq;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.DataAccess
{
    public class PointDefineRepository : RepositoryBase<Jc_DefModel>, IPointDefineRepository
    {
        public Jc_DefModel AddPointDefine(Jc_DefModel jc_DefModel)
        {
            return Insert(jc_DefModel);
        }        

        public void UpdatePointDefine(Jc_DefModel jc_DefModel)
        {
            Update(jc_DefModel); 
        }

        public void DeletePointDefine(string id)
        {
            Delete(id);
        }

        public IList<Jc_DefModel> GetPointDefineList(int pageIndex, int pageSize, out int rowCount)
        {
            var jc_DefModelLists = Datas;
            rowCount = jc_DefModelLists.Count();
            return jc_DefModelLists.OrderBy(p => p.Point).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }

        public List<Jc_DefModel> GetPointDefineList()
        {
            var jc_DefModelLists = Datas.ToList();            
            return jc_DefModelLists;
        }

        public Jc_DefModel GetPointDefineById(string id)
        {
            var jc_DefModel = Datas.FirstOrDefault(c => c.ID == id);
            return jc_DefModel;
        }

        public IList<Jc_DefModel> GetPointDefineByPointId(string pointId)
        {
            var ret = from m in Datas
                where m.PointID == pointId
                select m;
            return ret.ToList();
        }
    }
}