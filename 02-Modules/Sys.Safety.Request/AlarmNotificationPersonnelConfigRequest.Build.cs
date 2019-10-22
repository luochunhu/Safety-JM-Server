using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.AlarmNotificationPersonnelConfig
{
    public partial class AlarmNotificationPersonnelConfigAddRequest : Basic.Framework.Web.BasicRequest
    {
        public List<JC_AlarmNotificationPersonnelInfo> JC_AlarmNotificationPersonnelInfoList { get; set; }
        public JC_AlarmNotificationPersonnelConfigInfo JC_AlarmNotificationPersonnelConfigInfo { get; set; }
    }

    public partial class AlarmNotificationPersonnelConfigListAddRequest : Basic.Framework.Web.BasicRequest
    {
        public List<JC_AlarmNotificationPersonnelConfigInfo> JC_AlarmNotificationPersonnelConfigListInfo { get; set; }
        public List<JC_AlarmNotificationPersonnelInfo> JC_AlarmNotificationPersonnelInfoList { get; set; }
    }

    public partial class AlarmNotificationPersonnelConfigUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public JC_AlarmNotificationPersonnelConfigInfo JC_AlarmNotificationPersonnelConfigInfo { get; set; }
        public List<JC_AlarmNotificationPersonnelInfo> JC_AlarmNotificationPersonnelInfoList { get; set; }
    }

    public partial class AlarmNotificationPersonnelConfigDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
        public List<string> ids { get; set; } 
    }

    public partial class AlarmNotificationPersonnelConfigGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class AlarmNotificationPersonnelConfigGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
        public string AnalysisModeName { get; set; }
        public string Id { get; set; }
        public string AnalysisModelId { get; set; }
    }

    public partial class GetAlarmNotificationByAnalysisModelIdRequest : Basic.Framework.Web.BasicRequest
    {
        public string AnalysisModelId { get; set; }
    }

    public partial class GetAllAlarmNotificationRequest : Basic.Framework.Web.BasicRequest
    {
    }
}
