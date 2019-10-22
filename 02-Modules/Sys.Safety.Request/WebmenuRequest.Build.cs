using Basic.Framework.Web;
using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Webmenu
{
    public partial class WebmenuAddRequest : Basic.Framework.Web.BasicRequest
    {
        public WebmenuInfo WebmenuInfo { get; set; }
    }

    public partial class WebmenuUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public WebmenuInfo WebmenuInfo { get; set; }
    }

    public partial class WebmenuDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class WebmenuGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class WebmenuGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
        public string Id { get; set; }
    }

    public partial class WebmunuGetListByUserCodeRequest : BasicRequest 
    {
        public string UserCode { get; set; }
    }

    public partial class WebmenuBatchInsertRequest : BasicRequest 
    {
        public List<WebmenuInfo> WebMenuInfos { get; set; }
    }

    public partial class WebmenuBatchDeleteRequest : BasicRequest
    {
        public List<WebmenuInfo> WebMenuInfos { get; set; }
    }
}
