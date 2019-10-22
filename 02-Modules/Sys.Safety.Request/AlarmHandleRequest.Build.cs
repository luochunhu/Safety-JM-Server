using Basic.Framework.Web;
using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.AlarmHandle
{
    public partial class AlarmHandleAddRequest : Basic.Framework.Web.BasicRequest
    {
        public JC_AlarmHandleInfo JC_AlarmHandleInfo { get; set; }      
    }

	public partial class AlarmHandleUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public JC_AlarmHandleInfo JC_AlarmHandleInfo { get; set; }      
    }

	public partial class AlarmHandleDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class AlarmHandleGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class AlarmHandleGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }

    public partial class AlarmHandleWithoutSearchConditionRequest : Basic.Framework.Web.BasicRequest{ }

    public partial class AlarmHandleGetByAnalysisModelIdRequest : Basic.Framework.Web.BasicRequest
    {
        public string AnalysisModelId { get; set; }
    }

    public partial class AlarmHandleUpdateListRequest : Basic.Framework.Web.BasicRequest
    {
        private List<JC_AlarmHandleInfo> alarmHandleInfoList = new List<JC_AlarmHandleInfo>();
        public List<JC_AlarmHandleInfo> AlarmHandleInfoList {
            get { return alarmHandleInfoList; }
            set { alarmHandleInfoList = value; }
        }
    }

    public partial class AlarmHandelGetByStimeAndETime : BasicRequest 
    {
        public DateTime Stime { get; set; }

        public DateTime Etime { get; set; }
    }

    public partial class AlarmHandleNoEndListByCondition : Basic.Framework.Web.BasicRequest
    {
        public string StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string PersonId { get; set; }
    }
}
