using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Sys.Safety.Model
{
    public interface IKJ_AddresstyperuleRepository : IRepository<KJ_AddresstyperuleModel>
    {
                KJ_AddresstyperuleModel AddKJ_Addresstyperule(KJ_AddresstyperuleModel kJ_AddresstyperuleModel);
		        void UpdateKJ_Addresstyperule(KJ_AddresstyperuleModel kJ_AddresstyperuleModel);
	            void DeleteKJ_Addresstyperule(string id);
                void DeleteKJ_AddresstyperuleByAddressTypeId(string addressTypeId);
		        IList<KJ_AddresstyperuleModel> GetKJ_AddresstyperuleList(int pageIndex, int pageSize, out int rowCount);
                IList<KJ_AddresstyperuleModel> GetKJ_AddresstyperuleListByAddressTypeId(string addresstypeId);
				KJ_AddresstyperuleModel GetKJ_AddresstyperuleById(string id);
    }
}
