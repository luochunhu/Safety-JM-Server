using Basic.Framework.Service;
using Basic.Framework.Web.WebApi;
using Sys.Safety.Request.Def;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sys.Safety.WebApi
{
    public class KJ_AddresstyperuleController : BasicApiController, IKJ_AddresstyperuleService
    {
        IKJ_AddresstyperuleService kJ_AddresstyperuleService = ServiceFactory.Create<IKJ_AddresstyperuleService>();

        [HttpPost]
        [Route("v1/KJ_Addresstyperule/AddKJ_Addresstyperule")]
        public Basic.Framework.Web.BasicResponse<DataContract.KJ_AddresstyperuleInfo> AddKJ_Addresstyperule(Request.KJ_Addresstyperule.KJ_AddresstyperuleAddRequest kJ_AddresstyperuleRequest)
        {
            return kJ_AddresstyperuleService.AddKJ_Addresstyperule(kJ_AddresstyperuleRequest);
        }
        [HttpPost]
        [Route("v1/KJ_Addresstyperule/UpdateKJ_Addresstyperule")]
        public Basic.Framework.Web.BasicResponse<DataContract.KJ_AddresstyperuleInfo> UpdateKJ_Addresstyperule(Request.KJ_Addresstyperule.KJ_AddresstyperuleUpdateRequest kJ_AddresstyperuleRequest)
        {
            return kJ_AddresstyperuleService.UpdateKJ_Addresstyperule(kJ_AddresstyperuleRequest);
        }
        [HttpPost]
        [Route("v1/KJ_Addresstyperule/DeleteKJ_Addresstyperule")]
        public Basic.Framework.Web.BasicResponse DeleteKJ_Addresstyperule(Request.KJ_Addresstyperule.KJ_AddresstyperuleDeleteRequest kJ_AddresstyperuleRequest)
        {
            return kJ_AddresstyperuleService.DeleteKJ_Addresstyperule(kJ_AddresstyperuleRequest);
        }
        [HttpPost]
        [Route("v1/KJ_Addresstyperule/GetKJ_AddresstyperuleList")]
        public Basic.Framework.Web.BasicResponse<List<DataContract.KJ_AddresstyperuleInfo>> GetKJ_AddresstyperuleList(Request.KJ_Addresstyperule.KJ_AddresstyperuleGetListRequest kJ_AddresstyperuleRequest)
        {
            return kJ_AddresstyperuleService.GetKJ_AddresstyperuleList(kJ_AddresstyperuleRequest);
        }
        [HttpPost]
        [Route("v1/KJ_Addresstyperule/GetKJ_AddresstyperuleById")]
        public Basic.Framework.Web.BasicResponse<DataContract.KJ_AddresstyperuleInfo> GetKJ_AddresstyperuleById(Request.KJ_Addresstyperule.KJ_AddresstyperuleGetRequest kJ_AddresstyperuleRequest)
        {
            return kJ_AddresstyperuleService.GetKJ_AddresstyperuleById(kJ_AddresstyperuleRequest);
        }

        [HttpPost]
        [Route("v1/KJ_Addresstyperule/GetKJ_AddresstyperuleListByAddressTypeId")]
        public Basic.Framework.Web.BasicResponse<List<DataContract.KJ_AddresstyperuleInfo>> GetKJ_AddresstyperuleListByAddressTypeId(Request.KJ_Addresstyperule.KJ_AddresstyperuleGetListRequest kJ_AddresstyperuleRequest)
        {
            return kJ_AddresstyperuleService.GetKJ_AddresstyperuleListByAddressTypeId(kJ_AddresstyperuleRequest);
        }

        [HttpPost]
        [Route("v1/KJ_Addresstyperule/DeleteKJ_AddresstyperuleByAddressTypeId")]
        public Basic.Framework.Web.BasicResponse DeleteKJ_AddresstyperuleByAddressTypeId(Request.KJ_Addresstyperule.KJ_AddresstyperuleDeleteByAddressTypeIdRequest kJ_AddresstyperuleRequest)
        {
            return kJ_AddresstyperuleService.DeleteKJ_AddresstyperuleByAddressTypeId(kJ_AddresstyperuleRequest);
        }
    }
}
