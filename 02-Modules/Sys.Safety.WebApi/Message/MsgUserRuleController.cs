using Basic.Framework.Service;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sys.Safety.WebApi.Message
{
    public class MsgUserRuleController : Basic.Framework.Web.WebApi.BasicApiController, IMsgUserRuleService
    {
        IMsgUserRuleService msgUserRuleService = ServiceFactory.Create<IMsgUserRuleService>();

        [HttpPost]
        [Route("v1/MsgUserRule/AddMsgUserRule")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.MsgUserRuleInfo> AddMsgUserRule(Sys.Safety.Request.MsgUserRule.MsgUserRuleAddRequest msgUserRuleRequest)
        {
            return msgUserRuleService.AddMsgUserRule(msgUserRuleRequest);
        }

        [HttpPost]
        [Route("v1/MsgUserRule/UpdateMsgUserRule")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.MsgUserRuleInfo> UpdateMsgUserRule(Sys.Safety.Request.MsgUserRule.MsgUserRuleUpdateRequest msgUserRuleRequest)
        {
            return msgUserRuleService.UpdateMsgUserRule(msgUserRuleRequest);
        }

        [HttpPost]
        [Route("v1/MsgUserRule/DeleteMsgUserRule")]
        public Basic.Framework.Web.BasicResponse DeleteMsgUserRule(Sys.Safety.Request.MsgUserRule.MsgUserRuleDeleteRequest msgUserRuleRequest)
        {
            return msgUserRuleService.DeleteMsgUserRule(msgUserRuleRequest);
        }

        [HttpPost]
        [Route("v1/MsgUserRule/GetMsgUserRuleList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.MsgUserRuleInfo>> GetMsgUserRuleList(Sys.Safety.Request.MsgUserRule.MsgUserRuleGetListRequest msgUserRuleRequest)
        {
            return msgUserRuleService.GetMsgUserRuleList(msgUserRuleRequest);
        }

        [HttpPost]
        [Route("v1/MsgUserRule/GetMsgUserRuleById")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.MsgUserRuleInfo> GetMsgUserRuleById(Sys.Safety.Request.MsgUserRule.MsgUserRuleGetRequest msgUserRuleRequest)
        {
            return msgUserRuleService.GetMsgUserRuleById(msgUserRuleRequest);
        }

        [HttpPost]
        [Route("v1/MsgUserRule/GetAllMsgUserInfo")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.MsgUserRuleInfo>> GetAllMsgUserInfo(Basic.Framework.Web.BasicRequest msgUserRuleRequest)
        {
            return msgUserRuleService.GetAllMsgUserInfo(msgUserRuleRequest);
        }

        [HttpPost]
        [Route("v1/MsgUserRule/BatchInsert")]
        public Basic.Framework.Web.BasicResponse<bool> BatchInsert(Sys.Safety.Request.MsgUserRule.MsgUserRuleBatchRequest msgUserRuleRequest)
        {
            return msgUserRuleService.BatchInsert(msgUserRuleRequest);
        }

        [HttpPost]
        [Route("v1/MsgUserRule/GetMsgUserRuleByUserInfoAndRuleId")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.MsgUserRuleInfo> GetMsgUserRuleByUserInfoAndRuleId(Sys.Safety.Request.MsgUserRule.MsgUserRuleGetByUserInfoAndRuleIdRequest msgUserRuleRequest)
        {
            return msgUserRuleService.GetMsgUserRuleByUserInfoAndRuleId(msgUserRuleRequest);
        }
    }
}
