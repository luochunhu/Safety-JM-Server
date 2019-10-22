using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.AlarmHandle;
using Basic.Framework.Web.WebApi.Proxy;
using Basic.Framework.Common;

namespace Sys.Safety.WebApiAgent
{
    public class AreaRuleControllerProxy : BaseProxy, IAreaRuleService
    {
        public BasicResponse<AreaRuleInfo> AddAreaRule(Sys.Safety.Request.Area.AreaRuleAddRequest areaRuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AreaRule/AddAreaRule?token=" + Token, JSONHelper.ToJSONString(areaRuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse<AreaRuleInfo>>(responseStr);
        }

        public BasicResponse<AreaRuleInfo> UpdateAreaRule(Sys.Safety.Request.Area.AreaRuleUpdateRequest areaRuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AreaRule/UpdateAreaRule?token=" + Token, JSONHelper.ToJSONString(areaRuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse<AreaRuleInfo>>(responseStr);
        }

        public BasicResponse DeleteAreaRule(Sys.Safety.Request.Area.AreaRuleDeleteRequest areaRuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AreaRule/DeleteAreaRule?token=" + Token, JSONHelper.ToJSONString(areaRuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse<AreaRuleInfo>>(responseStr);
        }

        public BasicResponse<List<AreaRuleInfo>> GetAreaRuleList(Sys.Safety.Request.Area.AreaRuleGetListRequest areaRuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AreaRule/GetAreaRuleList?token=" + Token, JSONHelper.ToJSONString(areaRuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<AreaRuleInfo>>>(responseStr);
        }

        public BasicResponse<AreaRuleInfo> GetAreaRuleById(Sys.Safety.Request.Area.AreaRuleGetRequest areaRuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AreaRule/GetAreaRuleById?token=" + Token, JSONHelper.ToJSONString(areaRuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse<AreaRuleInfo>>(responseStr);
        }

        public BasicResponse<List<AreaRuleInfo>> GetAreaRuleListByAreaID(Sys.Safety.Request.Area.GetAreaRuleListByAreaIDRequest areaRuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AreaRule/GetAreaRuleListByAreaID?token=" + Token, JSONHelper.ToJSONString(areaRuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<AreaRuleInfo>>>(responseStr);
        }


        public BasicResponse DeleteAreaRuleByAreaID(Sys.Safety.Request.Area.AreaRuleDeleteRequest areaRuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AreaRule/DeleteAreaRuleByAreaID?token=" + Token, JSONHelper.ToJSONString(areaRuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<AreaRuleInfo>>>(responseStr);
        }
    }
}
