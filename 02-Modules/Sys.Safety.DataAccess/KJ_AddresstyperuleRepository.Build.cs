using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Sys.Safety.Model;

namespace Sys.Safety.DataAccess
{
    public partial class KJ_AddresstyperuleRepository : RepositoryBase<KJ_AddresstyperuleModel>, IKJ_AddresstyperuleRepository
    {

        public KJ_AddresstyperuleModel AddKJ_Addresstyperule(KJ_AddresstyperuleModel kJ_AddresstyperuleModel)
        {
            return base.Insert(kJ_AddresstyperuleModel);
        }
        public void UpdateKJ_Addresstyperule(KJ_AddresstyperuleModel kJ_AddresstyperuleModel)
        {
            base.Update(kJ_AddresstyperuleModel);
        }
        public void DeleteKJ_Addresstyperule(string id)
        {
            base.Delete(id);
        }

        public void DeleteKJ_AddresstyperuleByAddressTypeId(string addressTypeId)
        {
            base.Delete(a => a.Addresstypeid == addressTypeId);
        }
        public IList<KJ_AddresstyperuleModel> GetKJ_AddresstyperuleList(int pageIndex, int pageSize, out int rowCount)
        {
            rowCount = base.Datas.Count();
            return base.Datas.ToList();
        }

        public IList<KJ_AddresstyperuleModel> GetKJ_AddresstyperuleListByAddressTypeId(string addresstypeId)
        {
            return base.Datas.Where(a => a.Addresstypeid == addresstypeId).ToList();
        }
        public KJ_AddresstyperuleModel GetKJ_AddresstyperuleById(string id)
        {
            KJ_AddresstyperuleModel kJ_AddresstyperuleModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return kJ_AddresstyperuleModel;
        }
    }
}
