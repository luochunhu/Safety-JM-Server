using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.MsgRule;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IMsgRuleService
    {
        BasicResponse<MsgRuleInfo> AddMsgRule(MsgRuleAddRequest msgRuleRequest);
        BasicResponse<MsgRuleInfo> UpdateMsgRule(MsgRuleUpdateRequest msgRuleRequest);
        BasicResponse DeleteMsgRule(MsgRuleDeleteRequest msgRuleRequest);
        BasicResponse<List<MsgRuleInfo>> GetMsgRuleList(MsgRuleGetListRequest msgRuleRequest);
        BasicResponse<MsgRuleInfo> GetMsgRuleById(MsgRuleGetRequest msgRuleRequest);

        BasicResponse<bool> BatchInsert(MsgRuleBatchRequest msgRuleRequest);

        BasicResponse<bool> BatchDelete(MsgRuleBatchRequest msgRuleRequest);

        BasicResponse<List<MsgRuleInfo>> GetAllMsgRule(BasicRequest msgRuleRequest);

        BasicResponse<List<MsgRuleInfo>> GetMsgRuleByDevIdOrPointId(MasRuleGetByDevIdOrPointIdRequest msgRuleRequest);
    }
}

