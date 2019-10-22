using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Sys.Safety.Model;

namespace Sys.Safety.DataAccess
{
    public partial class KJ_AddresstypeRepository : RepositoryBase<KJ_AddresstypeModel>, IKJ_AddresstypeRepository
    {

        public KJ_AddresstypeModel AddKJ_Addresstype(KJ_AddresstypeModel kJ_AddresstypeModel)
        {
            return base.Insert(kJ_AddresstypeModel);
        }
        public void UpdateKJ_Addresstype(KJ_AddresstypeModel kJ_AddresstypeModel)
        {
            base.Update(kJ_AddresstypeModel);
        }
        public void DeleteKJ_Addresstype(string id)
        {
            base.Delete(id);
        }
        public IList<KJ_AddresstypeModel> GetKJ_AddresstypeList(int pageIndex, int pageSize, out int rowCount)
        {
            rowCount = base.Datas.Count();
            return base.Datas.ToList();
        }
        public KJ_AddresstypeModel GetKJ_AddresstypeById(string id)
        {
            KJ_AddresstypeModel kJ_AddresstypeModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return kJ_AddresstypeModel;
        }
    }
}
