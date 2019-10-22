using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.B_Def
{
    public partial class B_DefAddRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_DefInfo DefInfo { get; set; }
    }

    public partial class B_DefUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_DefInfo DefInfo { get; set; }
    }

    public partial class B_DefDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class B_DefGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class B_DefGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
        public string Id { get; set; }
    }
}
