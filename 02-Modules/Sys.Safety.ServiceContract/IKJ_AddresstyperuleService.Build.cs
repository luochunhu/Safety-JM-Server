using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.KJ_Addresstyperule;

namespace Sys.Safety.ServiceContract
{
    public interface IKJ_AddresstyperuleService
    {
        BasicResponse<KJ_AddresstyperuleInfo> AddKJ_Addresstyperule(KJ_AddresstyperuleAddRequest kJ_AddresstyperuleRequest);
        BasicResponse<KJ_AddresstyperuleInfo> UpdateKJ_Addresstyperule(KJ_AddresstyperuleUpdateRequest kJ_AddresstyperuleRequest);
        BasicResponse DeleteKJ_Addresstyperule(KJ_AddresstyperuleDeleteRequest kJ_AddresstyperuleRequest);
        BasicResponse DeleteKJ_AddresstyperuleByAddressTypeId(KJ_AddresstyperuleDeleteByAddressTypeIdRequest kJ_AddresstyperuleRequest);
        BasicResponse<List<KJ_AddresstyperuleInfo>> GetKJ_AddresstyperuleList(KJ_AddresstyperuleGetListRequest kJ_AddresstyperuleRequest);
        BasicResponse<KJ_AddresstyperuleInfo> GetKJ_AddresstyperuleById(KJ_AddresstyperuleGetRequest kJ_AddresstyperuleRequest);

        BasicResponse<List<KJ_AddresstyperuleInfo>> GetKJ_AddresstyperuleListByAddressTypeId(KJ_AddresstyperuleGetListRequest kJ_AddresstyperuleRequest);
    }
}

