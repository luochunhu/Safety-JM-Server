using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sys.Safety.Request.Runlog
{
    public partial class RunlogAddRequest : Basic.Framework.Web.BasicRequest
    {
        public RunlogInfo RunlogInfo { get; set; }      
    }

	public partial class RunlogUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public RunlogInfo RunlogInfo { get; set; }      
    }

	public partial class RunlogDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }
    public partial class RunlogDeleteByStimeEtimeRequest : Basic.Framework.Web.BasicRequest
    {
        public DateTime Stime { get; set; }
        public DateTime Etime { get; set; }
    }

    public partial class RunlogGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class RunlogGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
    public partial class RunlogGetByConditionsRequest : Basic.Framework.Web.BasicRequest
    {
        public DateTime dtStart { get; set; }
        public DateTime dtEnd { get; set; }
        public RunlogInfo.EnumLogLevel loglevel { get; set; }
        public string context { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
    }
    
}
