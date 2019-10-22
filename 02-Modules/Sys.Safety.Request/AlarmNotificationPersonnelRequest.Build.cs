using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.AlarmNotificationPersonnel
{
    public partial class AlarmNotificationPersonnelAddRequest : Basic.Framework.Web.BasicRequest
    {
        public JC_AlarmNotificationPersonnelInfo JC_AlarmNotificationPersonnelInfo { get; set; }
        public string AnalysisModelId { get; set; } 
        public List<JC_AlarmNotificationPersonnelInfo> JC_AlarmNotificationPersonnelInfoList { get; set; }     
    }

	public partial class AlarmNotificationPersonnelUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public JC_AlarmNotificationPersonnelInfo JC_AlarmNotificationPersonnelInfo { get; set; }      
    }

	public partial class AlarmNotificationPersonnelDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class AlarmNotificationPersonnelGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class AlarmNotificationPersonnelGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }

    public partial class AlarmNotificationPersonnelGetListByAlarmConfigIdRequest : Basic.Framework.Web.BasicRequest
    {
        public string AlarmConfigId { get; set; }
        public string AnalysisModelId { get; set; }
    }
}
