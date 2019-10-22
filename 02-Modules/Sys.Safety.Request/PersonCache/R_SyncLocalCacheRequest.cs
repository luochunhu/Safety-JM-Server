using Basic.Framework.Web;
using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.PersonCache
{
    public partial class R_SyncLocalCacheInsertRequest : BasicRequest
    {
        public R_SyncLocalInfo SyncLocal{get;set;}
    }

    public partial class R_SyncLocalCacheBatchInsertRequest : BasicRequest
    {
        public List<R_SyncLocalInfo> SyncLocals { get; set; }
    }

    public partial class R_SyncLocalCacheDeleteRequest : BasicRequest
    {
        public R_SyncLocalInfo SyncLocal { get; set; }
    }

    public partial class R_SyncLocalCacheBatchDeleteRequest : BasicRequest
    {
        public List<R_SyncLocalInfo> SyncLocals { get; set; }
    }

    public partial class R_SyncLocalCacheGetByIdRequest : BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class R_SyncLocalCacheGetAllRequest : BasicRequest
    {
        
    }

    public partial class R_SyncLocalCacheGetByConditionRequest : BasicRequest
    {
        public Expression<Func<R_SyncLocalInfo, bool>> predicate { get; set; }
    }
}
