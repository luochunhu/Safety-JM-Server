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
    public class MsgRuleControllerProxy : BaseProxy,IMsgRuleService
    {
        public Basic.Framework.Web.BasicResponse<DataContract.MsgRuleInfo> AddMsgRule(Sys.Safety.Request.MsgRule.MsgRuleAddRequest msgRuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/MsgRule/AddMsgRule?token=" + Token, JSONHelper.ToJSONString(msgRuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataContract.MsgRuleInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.MsgRuleInfo> UpdateMsgRule(Sys.Safety.Request.MsgRule.MsgRuleUpdateRequest msgRuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/MsgRule/UpdateMsgRule?token=" + Token, JSONHelper.ToJSONString(msgRuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataContract.MsgRuleInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeleteMsgRule(Sys.Safety.Request.MsgRule.MsgRuleDeleteRequest msgRuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/MsgRule/DeleteMsgRule?token=" + Token, JSONHelper.ToJSONString(msgRuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.MsgRuleInfo>> GetMsgRuleList(Sys.Safety.Request.MsgRule.MsgRuleGetListRequest msgRuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/MsgRule/GetMsgRuleList?token=" + Token, JSONHelper.ToJSONString(msgRuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<DataContract.MsgRuleInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.MsgRuleInfo> GetMsgRuleById(Sys.Safety.Request.MsgRule.MsgRuleGetRequest msgRuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/MsgRule/GetMsgRuleById?token=" + Token, JSONHelper.ToJSONString(msgRuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataContract.MsgRuleInfo>>(responseStr);
        }


        public Basic.Framework.Web.BasicResponse<bool> BatchInsert(Sys.Safety.Request.MsgRule.MsgRuleBatchRequest msgRuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/MsgRule/BatchInsert?token=" + Token, JSONHelper.ToJSONString(msgRuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse<bool>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<bool> BatchDelete(Sys.Safety.Request.MsgRule.MsgRuleBatchRequest msgRuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/MsgRule/BatchDelete?token=" + Token, JSONHelper.ToJSONString(msgRuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse<bool>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.MsgRuleInfo>> GetAllMsgRule(BasicRequest msgRuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/MsgRule/GetAllMsgRule?token=" + Token, JSONHelper.ToJSONString(msgRuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<DataContract.MsgRuleInfo>>>(responseStr);
        }

        public BasicResponse<List<DataContract.MsgRuleInfo>> GetMsgRuleByDevIdOrPointId(Sys.Safety.Request.MsgRule.MasRuleGetByDevIdOrPointIdRequest msgRuleRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/MsgRule/GetMsgRuleByDevIdOrPointId?token=" + Token, JSONHelper.ToJSONString(msgRuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<DataContract.MsgRuleInfo>>>(responseStr);
        }
    }
}
