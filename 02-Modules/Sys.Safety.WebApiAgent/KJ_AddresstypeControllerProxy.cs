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
    public class KJ_AddresstypeControllerProxy : BaseProxy, IKJ_AddresstypeService
    {

        public BasicResponse<KJ_AddresstypeInfo> AddKJ_Addresstype(Request.KJ_Addresstype.KJ_AddresstypeAddRequest kJ_AddresstypeRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/KJ_Addresstype/AddKJ_Addresstype?token=" + Token, JSONHelper.ToJSONString(kJ_AddresstypeRequest));
            return JSONHelper.ParseJSONString<BasicResponse<KJ_AddresstypeInfo>>(responseStr);
        }

        public BasicResponse<KJ_AddresstypeInfo> UpdateKJ_Addresstype(Request.KJ_Addresstype.KJ_AddresstypeUpdateRequest kJ_AddresstypeRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/KJ_Addresstype/UpdateKJ_Addresstype?token=" + Token, JSONHelper.ToJSONString(kJ_AddresstypeRequest));
            return JSONHelper.ParseJSONString<BasicResponse<KJ_AddresstypeInfo>>(responseStr);
        }

        public BasicResponse DeleteKJ_Addresstype(Request.KJ_Addresstype.KJ_AddresstypeDeleteRequest kJ_AddresstypeRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/KJ_Addresstype/DeleteKJ_Addresstype?token=" + Token, JSONHelper.ToJSONString(kJ_AddresstypeRequest));
            return JSONHelper.ParseJSONString<BasicResponse<KJ_AddresstypeInfo>>(responseStr);
        }

        public BasicResponse<List<KJ_AddresstypeInfo>> GetKJ_AddresstypeList(Request.KJ_Addresstype.KJ_AddresstypeGetListRequest kJ_AddresstypeRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/KJ_Addresstype/GetKJ_AddresstypeList?token=" + Token, JSONHelper.ToJSONString(kJ_AddresstypeRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<KJ_AddresstypeInfo>>>(responseStr);
        }

        public BasicResponse<KJ_AddresstypeInfo> GetKJ_AddresstypeById(Request.KJ_Addresstype.KJ_AddresstypeGetRequest kJ_AddresstypeRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/KJ_Addresstype/GetKJ_AddresstypeById?token=" + Token, JSONHelper.ToJSONString(kJ_AddresstypeRequest));
            return JSONHelper.ParseJSONString<BasicResponse<KJ_AddresstypeInfo>>(responseStr);
        }
    }
}
