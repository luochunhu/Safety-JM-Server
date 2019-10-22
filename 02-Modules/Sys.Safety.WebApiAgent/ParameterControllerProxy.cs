using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using Basic.Framework.Common;
using Sys.Safety.Request.JC_Parameter;

namespace Sys.Safety.WebApiAgent
{
    public class ParameterControllerProxy : BaseProxy, IParameterService
    {


        public BasicResponse<JC_ParameterInfo> AddJC_Parameter(ParameterAddRequest jC_Parameterrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Parameter/AddJC_Parameter?token=" + Token, JSONHelper.ToJSONString(jC_Parameterrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_ParameterInfo>>(responseStr);
        }



        public BasicResponse<JC_ParameterInfo> UpdateJC_Parameter(ParameterUpdateRequest jC_Parameterrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Parameter/UpdateJC_Parameter?token=" + Token, JSONHelper.ToJSONString(jC_Parameterrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_ParameterInfo>>(responseStr);
        }

        public BasicResponse DeleteJC_Parameter(ParameterDeleteRequest jC_Parameterrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Parameter/DeleteJC_Parameter?token=" + Token, JSONHelper.ToJSONString(jC_Parameterrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }


        public BasicResponse<List<JC_ParameterInfo>> GetJC_ParameterList(ParameterGetListRequest jC_Parameterrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Parameter/GetJC_ParameterList?token=" + Token, JSONHelper.ToJSONString(jC_Parameterrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_ParameterInfo>>>(responseStr);
        }


        public Basic.Framework.Web.BasicResponse<JC_ParameterInfo> GetJC_ParameterById(ParameterGetRequest jC_Parameterrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Parameter/GetJC_ParameterById?token=" + Token, JSONHelper.ToJSONString(jC_Parameterrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_ParameterInfo>>(responseStr);
        }

    }
}
