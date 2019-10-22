using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class RightRepository : RepositoryBase<RightModel>, IRightRepository
    {

        public RightModel AddRight(RightModel rightModel)
        {
            return base.Insert(rightModel);
        }
        public void UpdateRight(RightModel rightModel)
        {
            base.Update(rightModel);
        }
        public void DeleteRight(string id)
        {
            base.Delete(id);
        }
        public IList<RightModel> GetRightList(int pageIndex, int pageSize, out int rowCount)
        {
            var rightModelLists = base.Datas.ToList();
            rowCount = rightModelLists.Count();
            return rightModelLists.OrderBy(p => p.RightID).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public List<RightModel> GetRightList()
        {
            var rightModelLists = base.Datas.ToList();            
            return rightModelLists;
        }
        public RightModel GetRightById(string id)
        {
            RightModel rightModel = base.Datas.FirstOrDefault(c => c.RightID == id);
            return rightModel;
        }
    }
}
