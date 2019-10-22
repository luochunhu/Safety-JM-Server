using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.MsgUserRule;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Logging;

namespace Sys.Safety.Services
{
    public partial class MsgUserRuleService : IMsgUserRuleService
    {
        private IMsgUserRuleRepository _Repository;

        public MsgUserRuleService(IMsgUserRuleRepository _Repository)
        {
            this._Repository = _Repository;
        }
        public BasicResponse<MsgUserRuleInfo> AddMsgUserRule(MsgUserRuleAddRequest msgUserRuleRequest)
        {
            var _msgUserRule = ObjectConverter.Copy<MsgUserRuleInfo, MsgUserRuleModel>(msgUserRuleRequest.MsgUserRuleInfo);
            var resultmsgUserRule = _Repository.AddMsgUserRule(_msgUserRule);
            var msgUserRuleresponse = new BasicResponse<MsgUserRuleInfo>();
            msgUserRuleresponse.Data = ObjectConverter.Copy<MsgUserRuleModel, MsgUserRuleInfo>(resultmsgUserRule);
            return msgUserRuleresponse;
        }
        public BasicResponse<MsgUserRuleInfo> UpdateMsgUserRule(MsgUserRuleUpdateRequest msgUserRuleRequest)
        {
            var _msgUserRule = ObjectConverter.Copy<MsgUserRuleInfo, MsgUserRuleModel>(msgUserRuleRequest.MsgUserRuleInfo);
            _Repository.UpdateMsgUserRule(_msgUserRule);
            var msgUserRuleresponse = new BasicResponse<MsgUserRuleInfo>();
            msgUserRuleresponse.Data = ObjectConverter.Copy<MsgUserRuleModel, MsgUserRuleInfo>(_msgUserRule);
            return msgUserRuleresponse;
        }
        public BasicResponse DeleteMsgUserRule(MsgUserRuleDeleteRequest msgUserRuleRequest)
        {
            _Repository.DeleteMsgUserRule(msgUserRuleRequest.Id);
            var msgUserRuleresponse = new BasicResponse();
            return msgUserRuleresponse;
        }
        public BasicResponse<List<MsgUserRuleInfo>> GetMsgUserRuleList(MsgUserRuleGetListRequest msgUserRuleRequest)
        {
            var msgUserRuleresponse = new BasicResponse<List<MsgUserRuleInfo>>();
            msgUserRuleRequest.PagerInfo.PageIndex = msgUserRuleRequest.PagerInfo.PageIndex - 1;
            if (msgUserRuleRequest.PagerInfo.PageIndex < 0)
            {
                msgUserRuleRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var msgUserRuleModelLists = _Repository.GetMsgUserRuleList(msgUserRuleRequest.PagerInfo.PageIndex, msgUserRuleRequest.PagerInfo.PageSize, out rowcount);
            var msgUserRuleInfoLists = new List<MsgUserRuleInfo>();
            foreach (var item in msgUserRuleModelLists)
            {
                var MsgUserRuleInfo = ObjectConverter.Copy<MsgUserRuleModel, MsgUserRuleInfo>(item);
                msgUserRuleInfoLists.Add(MsgUserRuleInfo);
            }
            msgUserRuleresponse.Data = msgUserRuleInfoLists;
            return msgUserRuleresponse;
        }
        public BasicResponse<MsgUserRuleInfo> GetMsgUserRuleById(MsgUserRuleGetRequest msgUserRuleRequest)
        {
            var result = _Repository.GetMsgUserRuleById(msgUserRuleRequest.Id);
            var msgUserRuleInfo = ObjectConverter.Copy<MsgUserRuleModel, MsgUserRuleInfo>(result);
            var msgUserRuleresponse = new BasicResponse<MsgUserRuleInfo>();
            msgUserRuleresponse.Data = msgUserRuleInfo;
            return msgUserRuleresponse;
        }


        public BasicResponse<List<MsgUserRuleInfo>> GetAllMsgUserInfo(BasicRequest msgUserRuleRequest)
        {
            var result = _Repository.Datas.ToList();
            var msgUserRuleInfo = ObjectConverter.CopyList<MsgUserRuleModel, MsgUserRuleInfo>(result);
            var msgUserRuleresponse = new BasicResponse<List<MsgUserRuleInfo>>();
            msgUserRuleresponse.Data = msgUserRuleInfo.ToList();
            return msgUserRuleresponse;
        }


        public BasicResponse<bool> BatchInsert(MsgUserRuleBatchRequest msgUserRuleRequest)
        {
            var msgUserRuleresponse = new BasicResponse<bool>();
            try
            {
                var msgUserRuleModels = ObjectConverter.CopyList<MsgUserRuleInfo, MsgUserRuleModel>(msgUserRuleRequest.MsgUserRules);
                _Repository.Insert(msgUserRuleModels);

                msgUserRuleresponse.Data = true;
                return msgUserRuleresponse;
            }
            catch (Exception ex)
            {
                LogHelper.Error("批量保存用户短信规则出错！" + ex.Message + ex.StackTrace);
                msgUserRuleresponse.Data = false;
                return msgUserRuleresponse;
            }
        }


        public BasicResponse<MsgUserRuleInfo> GetMsgUserRuleByUserInfoAndRuleId(MsgUserRuleGetByUserInfoAndRuleIdRequest msgUserRuleRequest)
        {
            var result = _Repository.Datas.FirstOrDefault(ur => ur.UserName == msgUserRuleRequest.UserName &&
                ur.Phone == msgUserRuleRequest.Phone &&
                ur.RuleId == msgUserRuleRequest.RuleId);

            var msgUserRuleresponse = new BasicResponse<MsgUserRuleInfo>();
            if (result != null)
            {
                msgUserRuleresponse.Data = ObjectConverter.Copy<MsgUserRuleModel, MsgUserRuleInfo>(result);
            }
            return msgUserRuleresponse;
        }
    }
}


