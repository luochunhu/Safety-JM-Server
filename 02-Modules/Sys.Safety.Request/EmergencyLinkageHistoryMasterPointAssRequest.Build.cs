using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.EmergencyLinkageHistoryMasterPointAss
{
    public partial class EmergencyLinkageHistoryMasterPointAssAddRequest : Basic.Framework.Web.BasicRequest
    {
        public EmergencyLinkageHistoryMasterPointAssInfo EmergencyLinkageHistoryMasterPointAssInfo { get; set; }      
    }

	public partial class EmergencyLinkageHistoryMasterPointAssUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public EmergencyLinkageHistoryMasterPointAssInfo EmergencyLinkageHistoryMasterPointAssInfo { get; set; }      
    }

	public partial class EmergencyLinkageHistoryMasterPointAssDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class EmergencyLinkageHistoryMasterPointAssGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class EmergencyLinkageHistoryMasterPointAssGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}
