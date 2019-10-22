using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.R_Call
{
    public partial class B_CallAddRequest : Basic.Framework.Web.BasicRequest
    {
        public B_CallInfo CallInfo { get; set; }
    }

    public partial class B_CallUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public B_CallInfo CallInfo { get; set; }
    }
    public partial class B_CallMonitorRequest : Basic.Framework.Web.BasicRequest
    {
        public B_CallInfo CallInfo { get; set; }
    }

    public partial class B_CallDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class B_CallGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class B_CallGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
        public string Id { get; set; }
    }

    public partial class BCallInfoGetByMasterIDRequest : Basic.Framework.Web.BasicRequest
    {
        public string MasterId { get; set; }

        public int CallType { get; set; }
    }

    public partial class EndBcallByBcallInfoListRequest : Basic.Framework.Web.BasicRequest
    {
        public List<B_CallInfo> Info { get; set; }
    }

    public partial class EndBcallDbByBcallInfoListRequest : Basic.Framework.Web.BasicRequest
    {
        public List<B_CallInfo> Info { get; set; }
    }

}
