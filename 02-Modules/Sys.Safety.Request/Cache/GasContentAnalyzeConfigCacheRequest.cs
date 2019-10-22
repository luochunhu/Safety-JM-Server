using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;

namespace Sys.Safety.Request.Cache
{
    public class GasContentAnalyzeConfigAddCacheRequest : BasicRequest
    {
        public GascontentanalyzeconfigInfo Info { get; set; }
    }

    public class GasContentAnalyzeConfigDeleteCacheRequest : BasicRequest
    {
        public GascontentanalyzeconfigInfo Info { get; set; }
    }

    public class GasContentAnalyzeConfigDeleteCachesRequest : BasicRequest
    {
        public List<GascontentanalyzeconfigInfo> Infos { get; set; }
    }

    public class GasContentAnalyzeConfigUpdateCacheRequest : BasicRequest
    {
        public GascontentanalyzeconfigInfo Info { get; set; }
    }

    public class GasContentAnalyzeConfigGetCacheByConditionRequest : BasicRequest
    {
        public Func<GascontentanalyzeconfigInfo, bool> Condition { get; set; }
    }
}
