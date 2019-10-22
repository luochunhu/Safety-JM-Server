using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sys.Safety.WebApi.Message
{
    public class MsgRuleController : Basic.Framework.Web.WebApi.BasicApiController, IMsgRuleService
    {
        IMsgRuleService msgRuleService = ServiceFactory.Create<IMsgRuleService>();

        [HttpPost]
        [Route("v1/MsgRule/AddMsgRule")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.MsgRuleInfo> AddMsgRule(Sys.Safety.Request.MsgRule.MsgRuleAddRequest msgRuleRequest)
        {
            return msgRuleService.AddMsgRule(msgRuleRequest);
        }

        [HttpPost]
        [Route("v1/MsgRule/UpdateMsgRule")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.MsgRuleInfo> UpdateMsgRule(Sys.Safety.Request.MsgRule.MsgRuleUpdateRequest msgRuleRequest)
        {
            return msgRuleService.UpdateMsgRule(msgRuleRequest);
        }

        [HttpPost]
        [Route("v1/MsgRule/DeleteMsgRule")]
        public Basic.Framework.Web.BasicResponse DeleteMsgRule(Sys.Safety.Request.MsgRule.MsgRuleDeleteRequest msgRuleRequest)
        {
            return msgRuleService.DeleteMsgRule(msgRuleRequest);
        }

        [HttpPost]
        [Route("v1/MsgRule/GetMsgRuleList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.MsgRuleInfo>> GetMsgRuleList(Sys.Safety.Request.MsgRule.MsgRuleGetListRequest msgRuleRequest)
        {
            return msgRuleService.GetMsgRuleList(msgRuleRequest);
        }

        [HttpPost]
        [Route("v1/MsgRule/GetMsgRuleById")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.MsgRuleInfo> GetMsgRuleById(Sys.Safety.Request.MsgRule.MsgRuleGetRequest msgRuleRequest)
        {
            return msgRuleService.GetMsgRuleById(msgRuleRequest);
        }

        [HttpPost]
        [Route("v1/MsgRule/BatchInsert")]
        public Basic.Framework.Web.BasicResponse<bool> BatchInsert(Sys.Safety.Request.MsgRule.MsgRuleBatchRequest msgRuleRequest)
        {
            return msgRuleService.BatchInsert(msgRuleRequest);
        }

        [HttpPost]
        [Route("v1/MsgRule/BatchDelete")]
        public Basic.Framework.Web.BasicResponse<bool> BatchDelete(Sys.Safety.Request.MsgRule.MsgRuleBatchRequest msgRuleRequest)
        {
            return msgRuleService.BatchDelete(msgRuleRequest);
        }

        [HttpPost]
        [Route("v1/MsgRule/GetAllMsgRule")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.MsgRuleInfo>> GetAllMsgRule(BasicRequest msgRuleRequest)
        {
            return msgRuleService.GetAllMsgRule(msgRuleRequest);
        }

        [HttpPost]
        [Route("v1/MsgRule/GetMsgRuleByDevIdOrPointId")]
        public BasicResponse<List<Sys.Safety.DataContract.MsgRuleInfo>> GetMsgRuleByDevIdOrPointId(Sys.Safety.Request.MsgRule.MasRuleGetByDevIdOrPointIdRequest msgRuleRequest)
        {
            return msgRuleService.GetMsgRuleByDevIdOrPointId(msgRuleRequest);
        }
    }
}
