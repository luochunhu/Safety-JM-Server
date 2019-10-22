using Basic.Framework.Web;
using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Cache
{
    public partial class B_DefCacheLoadRequest : Basic.Framework.Web.BasicRequest
    {
    }
    public partial class B_DefCacheInsertRequest : BasicRequest
    {
        public Jc_DefInfo B_DefInfo { get; set; }
    }

    public partial class B_DefCacheBatchInsertRequest : BasicRequest
    {
        public List<Jc_DefInfo> B_DefInfos { get; set; }
    }

    public partial class B_DefCacheUpdateRequest : BasicRequest
    {
        public Jc_DefInfo B_DefInfo { get; set; }
    }

    public partial class B_DefCacheBatchUpdateRequest : BasicRequest
    {
        public List<Jc_DefInfo> B_DefInfos { get; set; }
    }

    public partial class B_DefCacheDeleteRequest : BasicRequest
    {
        public Jc_DefInfo B_DefInfo { get; set; }
    }

    public partial class B_DefCacheBatchDeleteRequest : BasicRequest
    {
        public List<Jc_DefInfo> B_DefInfos { get; set; }
    }

    public partial class B_DefCacheGetByIdRequest : BasicRequest
    {
        public string PointId { get; set; }
    }

    public partial class B_DefCacheGetAllRequest : BasicRequest
    {

    }

    public partial class B_DefCacheGetByConditionRequest : BasicRequest
    {
        public Func<Jc_DefInfo, bool> predicate { get; set; }
    }

    /// <summary>
    /// 部分更新测点信息RPC
    /// </summary>
    public partial class UpdatePropertiesRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// Key
        /// </summary>
        public string PointID { get; set; }
        /// <summary>
        /// 更新信息
        /// </summary>
        public Dictionary<string, object> UpdateItems { get; set; }
    }

    /// <summary>
    /// 批量 部分更新测点信息请求
    /// </summary>
    public class BatchUpdatePropertiesRequest : Basic.Framework.Web.BasicRequest
    {
        public Dictionary<string, Dictionary<string, object>> PointItems { get; set; }
    }
}
