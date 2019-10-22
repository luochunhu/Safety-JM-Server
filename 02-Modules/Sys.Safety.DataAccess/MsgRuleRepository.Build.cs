using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class MsgRuleRepository : RepositoryBase<MsgRuleModel>, IMsgRuleRepository
    {

        public MsgRuleModel AddMsgRule(MsgRuleModel msgRuleModel)
        {
            return base.Insert(msgRuleModel);
        }
        public void UpdateMsgRule(MsgRuleModel msgRuleModel)
        {
            base.Update(msgRuleModel);
        }
        public void DeleteMsgRule(string id)
        {
            base.Delete(id);
        }
        public IList<MsgRuleModel> GetMsgRuleList(int pageIndex, int pageSize, out int rowCount)
        {
            //var msgRuleModelLists = base.Datas.ToList();
            rowCount = base.Datas.Count();
            return base.Datas.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public MsgRuleModel GetMsgRuleById(string id)
        {
            MsgRuleModel msgRuleModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return msgRuleModel;
        }

        public IList<MsgRuleModel> GetAllMsgRule()
        {
            return base.Datas.ToList();
        }
    }
}
