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
    public class FactorControllerProxy : BaseProxy, IFactorService
    {


        public Basic.Framework.Web.BasicResponse<DataContract.JC_FactorInfo> AddJC_Factor(Sys.Safety.Request.JC_Factor.FactorAddRequest jC_Factorrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Factor/AddJC_Factor?token=" + Token, JSONHelper.ToJSONString(jC_Factorrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_FactorInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.JC_FactorInfo> UpdateJC_Factor(Sys.Safety.Request.JC_Factor.FactorUpdateRequest jC_Factorrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Factor/UpdateJC_Factor?token=" + Token, JSONHelper.ToJSONString(jC_Factorrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_FactorInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeleteJC_Factor(Sys.Safety.Request.JC_Factor.FactorDeleteRequest jC_Factorrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Factor/DeleteJC_Factor?token=" + Token, JSONHelper.ToJSONString(jC_Factorrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }


        public Basic.Framework.Web.BasicResponse<List<DataContract.JC_FactorInfo>> GetJC_FactorList(Sys.Safety.Request.JC_Factor.FactorGetListRequest jC_Factorrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Factor/GetJC_FactorList?token=" + Token, JSONHelper.ToJSONString(jC_Factorrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<DataContract.JC_FactorInfo>>>(responseStr);
        }
        public Basic.Framework.Web.BasicResponse<DataContract.JC_FactorInfo> GetJC_FactorById(Sys.Safety.Request.JC_Factor.FactorGetRequest jC_Factorrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Factor/GetJC_FactorById?token=" + Token, JSONHelper.ToJSONString(jC_Factorrequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_FactorInfo>>(responseStr);
        }
    }
}
