using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Jc_R;
using Sys.Safety.ServiceContract;
using Sys.Safety.WebApiAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiProxy
{
    public class Jc_RControllerProxy : BaseProxy, IJc_RService
    {
        public BasicResponse<DataContract.Jc_RInfo> AddJc_R(Jc_RAddRequest jc_Rrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/JC_R/AddJc_R?token=" + Token, JSONHelper.ToJSONString(jc_Rrequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_RInfo>>(responsestr);
        }

        public BasicResponse<DataContract.Jc_RInfo> UpdateJc_R(Jc_RUpdateRequest jc_Rrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/JC_R/UpdateJc_R?token=" + Token, JSONHelper.ToJSONString(jc_Rrequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_RInfo>>(responsestr);
        }

        public BasicResponse DeleteJc_R(Jc_RDeleteRequest jc_Rrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/JC_R/DeleteJc_R?token=" + Token, JSONHelper.ToJSONString(jc_Rrequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_RInfo>>(responsestr);
        }

        public BasicResponse<List<DataContract.Jc_RInfo>> GetJc_RList(Jc_RGetListRequest jc_Rrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/JC_R/GetJc_RList?token=" + Token, JSONHelper.ToJSONString(jc_Rrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_RInfo>>>(responsestr);
        }

        public BasicResponse<DataContract.Jc_RInfo> GetJc_RById(Jc_RGetRequest jc_Rrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/JC_R/GetJc_RById?token=" + Token, JSONHelper.ToJSONString(jc_Rrequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_RInfo>>(responsestr);
        }

        public BasicResponse<DataContract.Jc_RInfo> GetJc_RByDataAndId(Jc_RGetByDateAndIdRequest jc_Rrequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/JC_R/GetJc_RByDataAndId?token=" + Token, JSONHelper.ToJSONString(jc_Rrequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_RInfo>>(responsestr);
        }
    }
}
