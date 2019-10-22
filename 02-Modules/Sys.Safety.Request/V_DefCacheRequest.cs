using Basic.Framework.Web;
using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Cache
{
    public partial class V_DefCacheInsertRequest : BasicRequest
    {
        public V_DefInfo V_DefInfo { get; set; }
    }

    public partial class V_DefCacheBatchInsertRequest : BasicRequest
    {
        public List<V_DefInfo> V_DefInfos { get; set; }
    }

    public partial class V_DefCacheDeleteRequest : BasicRequest
    {
        public V_DefInfo V_DefInfo { get; set; }
    }

    public partial class V_DefCacheBatchDeleteRequest : BasicRequest
    {
        public List<V_DefInfo> V_DefInfos { get; set; }
    }

    public partial class V_DefCacheGetByIdRequest : BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class V_DefCacheGetAllRequest : BasicRequest
    {

    }

    public partial class V_DefCacheGetByConditionRequest : BasicRequest
    {
        public Func<V_DefInfo, bool> predicate { get; set; }
    }
}
