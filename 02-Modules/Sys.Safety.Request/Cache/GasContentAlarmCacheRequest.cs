using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract.Custom;

namespace Sys.Safety.Request.Cache
{
    public class AddCacheRequest : BasicRequest
    {
        public GasContentAlarmInfo Info { get; set; }
    }

    public class DeleteCacheRequest : BasicRequest
    {
        public GasContentAlarmInfo Info { get; set; }
    }

    public class DeleteCachesRequest : BasicRequest
    {
        public List<GasContentAlarmInfo> Infos { get; set; }
    }

    public class UpdateCacheRequest : BasicRequest
    {
        public GasContentAlarmInfo Info { get; set; }
    }

    public class GetCacheByConditionRequest : BasicRequest
    {
        public Func<GasContentAlarmInfo, bool> Condition { get; set; }
    }
}
