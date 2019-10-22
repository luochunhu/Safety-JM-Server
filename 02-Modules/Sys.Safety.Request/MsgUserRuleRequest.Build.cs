using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.MsgUserRule
{
    public partial class MsgUserRuleAddRequest : Basic.Framework.Web.BasicRequest
    {
        public MsgUserRuleInfo MsgUserRuleInfo { get; set; }
    }

    public partial class MsgUserRuleUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public MsgUserRuleInfo MsgUserRuleInfo { get; set; }
    }

    public partial class MsgUserRuleDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class MsgUserRuleGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class MsgUserRuleGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
        public string Id { get; set; }
    }

    public partial class MsgUserRuleBatchRequest : Basic.Framework.Web.BasicRequest
    {
        public List<MsgUserRuleInfo> MsgUserRules { get; set; }
    }

    public partial class MsgUserRuleGetByUserInfoAndRuleIdRequest : Basic.Framework.Web.BasicRequest 
    {
        public string UserName { get; set; }

        public string Phone { get; set; }

        public string RuleId { get; set; }
    }
}
