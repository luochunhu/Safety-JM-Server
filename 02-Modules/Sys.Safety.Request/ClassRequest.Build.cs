using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Class
{
    public partial class ClassListAddRequest : Basic.Framework.Web.BasicRequest
    {
        public List<ClassInfo> ClassInfoList { get; set; }
    }

    public partial class ClassAddRequest : Basic.Framework.Web.BasicRequest
    {
        public ClassInfo ClassInfo { get; set; }
    }

    public partial class ClassUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public ClassInfo ClassInfo { get; set; }
    }

    public partial class ClassDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class ClassGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class ClassGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
        public string Id { get; set; }
    }

    public partial class ClassCodeRequest : Basic.Framework.Web.BasicRequest
    {
        public string Code { get; set; }
    }
    public partial class GetClassByStrNameRequest : Basic.Framework.Web.BasicRequest
    {
        public string StrName { get; set; }
    }

    public partial class SaveClassByConditionRequest : Basic.Framework.Web.BasicRequest
    {
        public int? State { get; set; }
        public ClassInfo ClassInfo { get; set; }
    }
}
