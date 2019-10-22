using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent
{
    public class KJ_AddresstyperuleControllerProxy : BaseProxy, IKJ_AddresstyperuleService
    {      
        public BasicResponse<KJ_AddresstyperuleInfo> AddKJ_Addresstyperule(Request.KJ_Addresstyperule.KJ_AddresstyperuleAddRequest kJ_AddresstyperuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/KJ_Addresstyperule/AddKJ_Addresstyperule?token=" + Token, JSONHelper.ToJSONString(kJ_AddresstyperuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse<KJ_AddresstyperuleInfo>>(responseStr);
        }

        public BasicResponse<KJ_AddresstyperuleInfo> UpdateKJ_Addresstyperule(Request.KJ_Addresstyperule.KJ_AddresstyperuleUpdateRequest kJ_AddresstyperuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/KJ_Addresstyperule/UpdateKJ_Addresstyperule?token=" + Token, JSONHelper.ToJSONString(kJ_AddresstyperuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse<KJ_AddresstyperuleInfo>>(responseStr);
        }

        public BasicResponse DeleteKJ_Addresstyperule(Request.KJ_Addresstyperule.KJ_AddresstyperuleDeleteRequest kJ_AddresstyperuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/KJ_Addresstyperule/DeleteKJ_Addresstyperule?token=" + Token, JSONHelper.ToJSONString(kJ_AddresstyperuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse<KJ_AddresstyperuleInfo>>(responseStr);
        }

        public BasicResponse<List<KJ_AddresstyperuleInfo>> GetKJ_AddresstyperuleList(Request.KJ_Addresstyperule.KJ_AddresstyperuleGetListRequest kJ_AddresstyperuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/KJ_Addresstyperule/GetKJ_AddresstyperuleList?token=" + Token, JSONHelper.ToJSONString(kJ_AddresstyperuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<KJ_AddresstyperuleInfo>>>(responseStr);
        }

        public BasicResponse<KJ_AddresstyperuleInfo> GetKJ_AddresstyperuleById(Request.KJ_Addresstyperule.KJ_AddresstyperuleGetRequest kJ_AddresstyperuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/KJ_Addresstyperule/GetKJ_AddresstyperuleById?token=" + Token, JSONHelper.ToJSONString(kJ_AddresstyperuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse<KJ_AddresstyperuleInfo>>(responseStr);
        }

        public BasicResponse<List<KJ_AddresstyperuleInfo>> GetKJ_AddresstyperuleListByAddressTypeId(Request.KJ_Addresstyperule.KJ_AddresstyperuleGetListRequest kJ_AddresstyperuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/KJ_Addresstyperule/GetKJ_AddresstyperuleListByAddressTypeId?token=" + Token, JSONHelper.ToJSONString(kJ_AddresstyperuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<KJ_AddresstyperuleInfo>>>(responseStr);
        }


        public BasicResponse DeleteKJ_AddresstyperuleByAddressTypeId(Request.KJ_Addresstyperule.KJ_AddresstyperuleDeleteByAddressTypeIdRequest kJ_AddresstyperuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/KJ_Addresstyperule/DeleteKJ_AddresstyperuleByAddressTypeId?token=" + Token, JSONHelper.ToJSONString(kJ_AddresstyperuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
    }
}
