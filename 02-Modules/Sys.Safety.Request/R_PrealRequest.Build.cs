using Basic.Framework.Rpc;
using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.R_Preal
{
    public partial class R_PrealAddRequest : Basic.Framework.Web.BasicRequest
    {
        public R_PrealInfo PrealInfo { get; set; }      
    }

    public partial class R_PrealUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public R_PrealInfo PrealInfo { get; set; }      
    }

    public partial class R_PrealDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class R_PrealGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class R_PrealGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}
