using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.SyncLocal
{
    public partial class R_SyncLocalAddRequest : Basic.Framework.Web.BasicRequest
    {
        public R_SyncLocalInfo SyncLocalInfo { get; set; }      
    }

    public partial class R_SyncLocalUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public R_SyncLocalInfo SyncLocalInfo { get; set; }      
    }

    public partial class R_SyncLocalDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class R_SyncLocalGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class R_SyncLocalGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}
