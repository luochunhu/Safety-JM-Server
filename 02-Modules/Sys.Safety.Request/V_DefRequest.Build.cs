using Basic.Framework.Rpc;
using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Def
{
    public partial class DefAddRequest : Basic.Framework.Web.BasicRequest
    {
        public V_DefInfo DefInfo { get; set; }
    }

    public partial class DefUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public V_DefInfo DefInfo { get; set; }
    }

    public partial class DefDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class DefGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class DefGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
        public string Id { get; set; }
    }

    public partial class DefGetAllRequest : Basic.Framework.Web.BasicRequest
    {
    }

    public partial class DefIPRequest : Basic.Framework.Web.BasicRequest 
    {
        public string IPAddress { get; set; }
    }
}
