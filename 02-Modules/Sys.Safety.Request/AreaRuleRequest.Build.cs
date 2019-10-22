using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Area
{
    public partial class AreaRuleAddRequest : Basic.Framework.Web.BasicRequest
    {
        public AreaRuleInfo AreaRuleInfo { get; set; }      
    }

	public partial class AreaRuleUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public AreaRuleInfo AreaRuleInfo { get; set; }      
    }

	public partial class AreaRuleDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class AreaRuleGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class AreaRuleGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }

    public partial class GetAreaRuleListByAreaIDRequest : Basic.Framework.Web.BasicRequest
    {
        public string areaID{ get; set; }
    }
}
