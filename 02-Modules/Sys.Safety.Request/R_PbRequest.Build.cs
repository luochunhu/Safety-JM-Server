using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Pb
{
    public partial class R_PbAddRequest : Basic.Framework.Web.BasicRequest
    {
        public R_PbInfo PbInfo { get; set; }      
    }

	public partial class R_PbUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public R_PbInfo PbInfo { get; set; }      
    }
    public partial class R_PBBatchUpateRequest : Basic.Framework.Web.BasicRequest
    {
        public List<R_PbInfo> PbInfoList { get; set; }
    }

	public partial class R_PbDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class R_PbGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class R_PbGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }

    public partial class R_PbGetByParRequest : Basic.Framework.Web.BasicRequest
    {
        public string yid;
        public int datastate;
        public string Sysflag;
    }
}
