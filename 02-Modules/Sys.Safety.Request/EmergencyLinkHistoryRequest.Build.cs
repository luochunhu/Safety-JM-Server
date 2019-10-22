using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.EmergencyLinkHistory
{
    public partial class EmergencyLinkHistoryAddRequest : Basic.Framework.Web.BasicRequest
    {
        public EmergencyLinkHistoryInfo EmergencyLinkHistoryInfo { get; set; }
    }

    public partial class EmergencyLinkHistoryUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public EmergencyLinkHistoryInfo EmergencyLinkHistoryInfo { get; set; }
    }

    public partial class EmergencyLinkHistoryDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class EmergencyLinkHistoryGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class EmergencyLinkHistoryGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
        public string Id { get; set; }
    }

    public partial class EmergencyLinkHistoryGetByEmergencyRequest : Basic.Framework.Web.BasicRequest
    {
        public string EmergencyId { get; set; }
    }

    public partial class BatchAddEmergencyLinkHistoryRequest : Basic.Framework.Web.BasicRequest
    {
        public List<EmergencyLinkHistoryInfo> LisEmergencyLinkHistoryInfo { get; set; }
    }

    public partial class EndAllRequest: Basic.Framework.Web.BasicRequest
    {
        public DateTime EndTime { get; set; }
    }

    public partial class EndByLinkageIdRequest: Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }

        public DateTime EndTime { get; set; }
    }

    public partial class AddEmergencyLinkHistoryAndAssRequest : Basic.Framework.Web.BasicRequest
    {
        public EmergencyLinkHistoryInfo EmergencyLinkHistoryInfo { get; set; }

        public List<EmergencyLinkageHistoryMasterPointAssInfo> LinkageHistoryMasterPointAssInfoList { get;
            set;
        }
    }
}
