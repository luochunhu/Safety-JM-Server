using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.ShortCutMenu
{
    public partial class ShortCutMenuAddRequest : Basic.Framework.Web.BasicRequest
    {
        public ShortCutMenuInfo ShortCutMenuInfo { get; set; }
    }

    public partial class ShortCutMenuUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public ShortCutMenuInfo ShortCutMenuInfo { get; set; }
    }

    public partial class ShortCutMenuDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class ShortCutMenuGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class ShortCutMenuGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
        public string Id { get; set; }
    }

    public partial class ShortCutMenuUserRequest : Basic.Framework.Web.BasicRequest
    {
        public string UserId { get; set; }
        
        //public string UserCode { get; set; }
    }

    public partial class ShortCutMenuBatchInsertRequest : Basic.Framework.Web.BasicRequest
    {
        public List<ShortCutMenuInfo> ShortCutMenuInfos { get; set; }
    }
}
