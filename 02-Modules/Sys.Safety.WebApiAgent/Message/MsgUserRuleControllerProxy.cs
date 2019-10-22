using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent.Message
{
    public class MsgUserRuleControllerProxy:BaseProxy, IMsgUserRuleService
    {
        public Basic.Framework.Web.BasicResponse<DataContract.MsgUserRuleInfo> AddMsgUserRule(Sys.Safety.Request.MsgUserRule.MsgUserRuleAddRequest msgUserRuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/MsgUserRule/AddMsgRule?token=" + Token, JSONHelper.ToJSONString(msgUserRuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataContract.MsgUserRuleInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.MsgUserRuleInfo> UpdateMsgUserRule(Sys.Safety.Request.MsgUserRule.MsgUserRuleUpdateRequest msgUserRuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/MsgUserRule/UpdateMsgUserRule?token=" + Token, JSONHelper.ToJSONString(msgUserRuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataContract.MsgUserRuleInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeleteMsgUserRule(Sys.Safety.Request.MsgUserRule.MsgUserRuleDeleteRequest msgUserRuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/MsgUserRule/DeleteMsgUserRule?token=" + Token, JSONHelper.ToJSONString(msgUserRuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.MsgUserRuleInfo>> GetMsgUserRuleList(Sys.Safety.Request.MsgUserRule.MsgUserRuleGetListRequest msgUserRuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/MsgUserRule/GetMsgUserRuleList?token=" + Token, JSONHelper.ToJSONString(msgUserRuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<DataContract.MsgUserRuleInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.MsgUserRuleInfo> GetMsgUserRuleById(Sys.Safety.Request.MsgUserRule.MsgUserRuleGetRequest msgUserRuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/MsgUserRule/GetMsgUserRuleById?token=" + Token, JSONHelper.ToJSONString(msgUserRuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataContract.MsgUserRuleInfo>>(responseStr);
        }


        public Basic.Framework.Web.BasicResponse<List<DataContract.MsgUserRuleInfo>> GetAllMsgUserInfo(Basic.Framework.Web.BasicRequest msgUserRuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/MsgUserRule/GetAllMsgUserInfo?token=" + Token, JSONHelper.ToJSONString(msgUserRuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<DataContract.MsgUserRuleInfo>>>(responseStr);
        }


        public BasicResponse<bool> BatchInsert(Sys.Safety.Request.MsgUserRule.MsgUserRuleBatchRequest msgUserRuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/MsgUserRule/BatchInsert?token=" + Token, JSONHelper.ToJSONString(msgUserRuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse<bool>>(responseStr);
        }


        public BasicResponse<DataContract.MsgUserRuleInfo> GetMsgUserRuleByUserInfoAndRuleId(Sys.Safety.Request.MsgUserRule.MsgUserRuleGetByUserInfoAndRuleIdRequest msgUserRuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/MsgUserRule/GetMsgUserRuleByUserInfoAndRuleId?token=" + Token, JSONHelper.ToJSONString(msgUserRuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataContract.MsgUserRuleInfo>>(responseStr);
        }
    }
}
