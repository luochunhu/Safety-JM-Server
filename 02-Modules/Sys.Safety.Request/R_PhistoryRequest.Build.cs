using Basic.Framework.Rpc;
using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.R_Phistory
{
    public partial class R_PhistoryAddRequest : Basic.Framework.Web.BasicRequest
    {
        public R_PhistoryInfo PhistoryInfo { get; set; }      
    }

	public partial class R_PhistoryUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public R_PhistoryInfo PhistoryInfo { get; set; }      
    }

	public partial class R_PhistoryDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class R_PhistoryGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class R_PhistoryGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }

    public partial class R_PhistoryGetByParRequest : Basic.Framework.Web.BasicRequest
    {
        public string yid { get; set; }
        public DateTime rtime { get; set; }
        public string pointid { get; set; }
    }

    public partial class R_PhistoryGetLastByYidRequest : Basic.Framework.Web.BasicRequest
    {
        public string yid { get; set; }
    }

    public partial class R_PhistoryGetLastByTimerRequest : Basic.Framework.Web.BasicRequest
    {
        public DateTime Timer { get; set; }
    }
}
