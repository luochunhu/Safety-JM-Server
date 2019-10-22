using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Jc_Show
{
    public partial class Jc_ShowAddRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_ShowInfo Jc_ShowInfo { get; set; }
    }

    public partial class Jc_ShowUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_ShowInfo Jc_ShowInfo { get; set; }
    }

    public partial class Jc_ShowDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class Jc_ShowGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class Jc_ShowGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
        public string Id { get; set; }
    }


    public partial class SaveCustomPagePointsRequest : Basic.Framework.Web.BasicRequest
    {
        public int? Page { get; set; }
        public List<Jc_ShowInfo> Jc_ShowInfoList { get; set; }
    }
}
