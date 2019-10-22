using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.MsgRule
{
    public partial class MsgRuleAddRequest : Basic.Framework.Web.BasicRequest
    {
        public MsgRuleInfo MsgRuleInfo { get; set; }      
    }

	public partial class MsgRuleUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public MsgRuleInfo MsgRuleInfo { get; set; }      
    }

	public partial class MsgRuleDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class MsgRuleGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class MsgRuleGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }


    public partial class MsgRuleBatchRequest : Basic.Framework.Web.BasicRequest 
    {
        public List<MsgRuleInfo> MsgRuleInfos { get; set; }
    }

    public partial class MasRuleGetByDevIdOrPointIdRequest : Basic.Framework.Web.BasicRequest 
    {
        public long DevIdOrPointId { get; set; }
    }
}
