using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.AreaAlarm
{
    public partial class R_AreaAlarmAddRequest : Basic.Framework.Web.BasicRequest
    {
        public R_AreaAlarmInfo AreaAlarmInfo { get; set; }      
    }

	public partial class R_AreaAlarmUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public R_AreaAlarmInfo AreaAlarmInfo { get; set; }      
    }

	public partial class R_AreaAlarmDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class R_AreaAlarmGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class R_AreaAlarmGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}
