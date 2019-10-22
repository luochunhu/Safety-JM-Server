using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.MsgRule;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Logging;

namespace Sys.Safety.Services
{
    public partial class MsgRuleService : IMsgRuleService
    {
        private IMsgRuleRepository _Repository;

        public MsgRuleService(IMsgRuleRepository _Repository)
        {
            this._Repository = _Repository;
        }
        public BasicResponse<MsgRuleInfo> AddMsgRule(MsgRuleAddRequest msgRuleRequest)
        {
            var _msgRule = ObjectConverter.Copy<MsgRuleInfo, MsgRuleModel>(msgRuleRequest.MsgRuleInfo);
            var resultmsgRule = _Repository.AddMsgRule(_msgRule);
            var msgRuleresponse = new BasicResponse<MsgRuleInfo>();
            msgRuleresponse.Data = ObjectConverter.Copy<MsgRuleModel, MsgRuleInfo>(resultmsgRule);
            return msgRuleresponse;
        }
        public BasicResponse<MsgRuleInfo> UpdateMsgRule(MsgRuleUpdateRequest msgRuleRequest)
        {
            var _msgRule = ObjectConverter.Copy<MsgRuleInfo, MsgRuleModel>(msgRuleRequest.MsgRuleInfo);
            _Repository.UpdateMsgRule(_msgRule);
            var msgRuleresponse = new BasicResponse<MsgRuleInfo>();
            msgRuleresponse.Data = ObjectConverter.Copy<MsgRuleModel, MsgRuleInfo>(_msgRule);
            return msgRuleresponse;
        }
        public BasicResponse DeleteMsgRule(MsgRuleDeleteRequest msgRuleRequest)
        {
            _Repository.DeleteMsgRule(msgRuleRequest.Id);
            var msgRuleresponse = new BasicResponse();
            return msgRuleresponse;
        }
        public BasicResponse<List<MsgRuleInfo>> GetMsgRuleList(MsgRuleGetListRequest msgRuleRequest)
        {
            var msgRuleresponse = new BasicResponse<List<MsgRuleInfo>>();
            msgRuleRequest.PagerInfo.PageIndex = msgRuleRequest.PagerInfo.PageIndex - 1;
            if (msgRuleRequest.PagerInfo.PageIndex < 0)
            {
                msgRuleRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var msgRuleModelLists = _Repository.GetMsgRuleList(msgRuleRequest.PagerInfo.PageIndex, msgRuleRequest.PagerInfo.PageSize, out rowcount);
            var msgRuleInfoLists = new List<MsgRuleInfo>();
            foreach (var item in msgRuleModelLists)
            {
                var MsgRuleInfo = ObjectConverter.Copy<MsgRuleModel, MsgRuleInfo>(item);
                msgRuleInfoLists.Add(MsgRuleInfo);
            }
            msgRuleresponse.Data = msgRuleInfoLists;
            return msgRuleresponse;
        }
        public BasicResponse<MsgRuleInfo> GetMsgRuleById(MsgRuleGetRequest msgRuleRequest)
        {
            var result = _Repository.GetMsgRuleById(msgRuleRequest.Id);
            var msgRuleInfo = ObjectConverter.Copy<MsgRuleModel, MsgRuleInfo>(result);
            var msgRuleresponse = new BasicResponse<MsgRuleInfo>();
            msgRuleresponse.Data = msgRuleInfo;
            return msgRuleresponse;
        }


        public BasicResponse<bool> BatchInsert(MsgRuleBatchRequest msgRuleRequest)
        {
            var msgRuleresponse = new BasicResponse<bool>();
            try
            {
                var msgRuleModels = ObjectConverter.CopyList<MsgRuleInfo, MsgRuleModel>(msgRuleRequest.MsgRuleInfos);
                _Repository.Insert(msgRuleModels);

                msgRuleresponse.Data = true;
                return msgRuleresponse;
            }
            catch (Exception ex)
            {
                LogHelper.Error("批量保存短信规则出错！" + ex.Message + ex.StackTrace);
                msgRuleresponse.Data = false;
                return msgRuleresponse;
            }
        }

        public BasicResponse<bool> BatchDelete(MsgRuleBatchRequest msgRuleRequest)
        {
            var msgRuleresponse = new BasicResponse<bool>();
            try
            {
                var msgRuleModels = ObjectConverter.CopyList<MsgRuleInfo, MsgRuleModel>(msgRuleRequest.MsgRuleInfos);
                _Repository.Delete(msgRuleModels);

                msgRuleresponse.Data = true;
                return msgRuleresponse;
            }
            catch (Exception ex)
            {
                LogHelper.Error("批量删除短信规则出错！" + ex.Message + ex.StackTrace);
                msgRuleresponse.Data = false;
                return msgRuleresponse;
            }
        }


        public BasicResponse<List<MsgRuleInfo>> GetAllMsgRule(BasicRequest msgRuleRequest)
        {
            var msgRuleModelLists = _Repository.GetAllMsgRule();
            var MsgRuleInfos = ObjectConverter.CopyList<MsgRuleModel, MsgRuleInfo>(msgRuleModelLists);

            var msgRuleresponse =new BasicResponse<List<MsgRuleInfo>>();
            msgRuleresponse.Data = MsgRuleInfos.ToList();
            return msgRuleresponse;
        }


        public BasicResponse<List<MsgRuleInfo>> GetMsgRuleByDevIdOrPointId(MasRuleGetByDevIdOrPointIdRequest msgRuleRequest)
        {
            var msgRuleresponse = new BasicResponse<List<MsgRuleInfo>>();
            if (msgRuleRequest.DevIdOrPointId != 0)
            {
                var msgRuleModelLists = _Repository.Datas.Where(rule => rule.DevId == msgRuleRequest.DevIdOrPointId || rule.PointId == msgRuleRequest.DevIdOrPointId).ToList();
                var MsgRuleInfos = ObjectConverter.CopyList<MsgRuleModel, MsgRuleInfo>(msgRuleModelLists);
                msgRuleresponse.Data = MsgRuleInfos.ToList();
                return msgRuleresponse;
            }
            return msgRuleresponse;
        }
    }
}


