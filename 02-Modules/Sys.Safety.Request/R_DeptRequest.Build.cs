using Basic.Framework.Rpc;
using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.R_Dept
{
    public partial class R_DeptAddRequest : Basic.Framework.Web.BasicRequest
    {
        public R_DeptInfo DeptInfo { get; set; }
    }

    public partial class R_DeptUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public R_DeptInfo DeptInfo { get; set; }
    }

    public partial class R_DeptDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class R_DeptGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class R_DeptGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
        public string Id { get; set; }
    }
}
