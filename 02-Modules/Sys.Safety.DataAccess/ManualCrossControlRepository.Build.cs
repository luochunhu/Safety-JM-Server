using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class ManualCrossControlRepository : RepositoryBase<Jc_JcsdkzModel>, IManualCrossControlRepository
    {

        public Jc_JcsdkzModel AddManualCrossControl(Jc_JcsdkzModel ManualCrossControlModel)
        {
            return base.Insert(ManualCrossControlModel);
        }
        public void UpdateManualCrossControl(Jc_JcsdkzModel ManualCrossControlModel)
        {
            base.Update(ManualCrossControlModel);
        }
        public void DeleteManualCrossControl(string id)
        {
            base.Delete(id);
        }
        public IList<Jc_JcsdkzModel> GetManualCrossControlList(int pageIndex, int pageSize, out int rowCount)
        {
            var jc_JcsdkzModelLists = base.Datas;
            rowCount = jc_JcsdkzModelLists.Count();
            return jc_JcsdkzModelLists.OrderBy(p => p.ZkPoint).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public List<Jc_JcsdkzModel> GetManualCrossControlList()
        {
            var jc_JcsdkzModelLists = base.Datas.ToList();            
            return jc_JcsdkzModelLists;
        }
        public Jc_JcsdkzModel GetManualCrossControlById(string id)
        {
            Jc_JcsdkzModel jc_JcsdkzModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return jc_JcsdkzModel;
        }

        public void DelteManualCrossControlFromDB()
        {
            base.ExecuteNonQuery("global_DeleteOthreManualCrossControl");
        }
    }
}
