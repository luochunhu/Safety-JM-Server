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
    public class R_PersoninfControllerProxy : BaseProxy, IR_PersoninfService
    {
        public Basic.Framework.Web.BasicResponse<DataContract.R_PersoninfInfo> AddPersoninf(Sys.Safety.Request.R_Personinf.R_PersoninfAddRequest personinfRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Personinf/AddPersoninf?token=" + Token, JSONHelper.ToJSONString(personinfRequest));
            return JSONHelper.ParseJSONString<BasicResponse<R_PersoninfInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.R_PersoninfInfo> UpdatePersoninf(Sys.Safety.Request.R_Personinf.R_PersoninfUpdateRequest personinfRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Personinf/UpdatePersoninf?token=" + Token, JSONHelper.ToJSONString(personinfRequest));
            return JSONHelper.ParseJSONString<BasicResponse<R_PersoninfInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeletePersoninf(Sys.Safety.Request.R_Personinf.R_PersoninfDeleteRequest personinfRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Personinf/DeletePersoninf?token=" + Token, JSONHelper.ToJSONString(personinfRequest));
            return JSONHelper.ParseJSONString<BasicResponse<R_PersoninfInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.R_PersoninfInfo>> GetPersoninfList(Sys.Safety.Request.R_Personinf.R_PersoninfGetListRequest personinfRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Personinf/GetPersoninfList?token=" + Token, JSONHelper.ToJSONString(personinfRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<R_PersoninfInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.R_PersoninfInfo> GetPersoninfById(Sys.Safety.Request.R_Personinf.R_PersoninfGetRequest personinfRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Personinf/GetPersoninfById?token=" + Token, JSONHelper.ToJSONString(personinfRequest));
            return JSONHelper.ParseJSONString<BasicResponse<R_PersoninfInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.R_PersoninfInfo>> GetAllPersonInfo(Basic.Framework.Web.BasicRequest personinfRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Personinf/GetAllPersonInfo?token=" + Token, JSONHelper.ToJSONString(personinfRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<R_PersoninfInfo>>>(responseStr);
        }

        public BasicResponse<List<R_PersoninfInfo>> GetAllPersonInfoCache(BasicRequest personinfRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Personinf/GetAllPersonInfoCache?token=" + Token, JSONHelper.ToJSONString(personinfRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<R_PersoninfInfo>>>(responseStr);
        }


        public BasicResponse<R_PersoninfInfo> GetPersoninfCache(Sys.Safety.Request.R_Personinf.R_PersoninfGetRequest personinfRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Personinf/GetPersoninfCache?token=" + Token, JSONHelper.ToJSONString(personinfRequest));
            return JSONHelper.ParseJSONString<BasicResponse<R_PersoninfInfo>>(responseStr);
        }


        public BasicResponse<List<R_PersoninfInfo>> GetAllDefinedPersonInfoCache(BasicRequest personinfRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Personinf/GetAllDefinedPersonInfoCache?token=" + Token, JSONHelper.ToJSONString(personinfRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<R_PersoninfInfo>>>(responseStr);
        }


        public BasicResponse<List<R_PersoninfInfo>> GetPersoninfCacheByBh(Sys.Safety.Request.R_Personinf.R_PersoninfGetByBhRequest personinfRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Personinf/GetPersoninfCacheByBh?token=" + Token, JSONHelper.ToJSONString(personinfRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<R_PersoninfInfo>>>(responseStr);
        }
    }
}
