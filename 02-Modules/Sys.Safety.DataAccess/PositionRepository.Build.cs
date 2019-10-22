using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class PositionRepository : RepositoryBase<Jc_WzModel>, IPositionRepository
    {

        public Jc_WzModel AddPosition(Jc_WzModel PositionModel)
        {
            return base.Insert(PositionModel);
        }
        public void UpdatePosition(Jc_WzModel PositionModel)
        {
            base.Update(PositionModel);
        }
        public void DeletePosition(string id)
        {
            base.Delete(id);
        }
        public IList<Jc_WzModel> GetPositionList(int pageIndex, int pageSize, out int rowCount)
        {
            var jc_WzModelLists = base.Datas;
            rowCount = jc_WzModelLists.Count();
            return jc_WzModelLists.OrderBy(p => p.WzID).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public List<Jc_WzModel> GetPositionList()
        {
            var jc_WzModelLists = base.Datas.ToList();            
            return jc_WzModelLists;
        }
        public Jc_WzModel GetPositionById(string id)
        {
            Jc_WzModel jc_WzModel = base.Datas.FirstOrDefault(c => c.WzID == id);
            return jc_WzModel;
        }
    }
}
