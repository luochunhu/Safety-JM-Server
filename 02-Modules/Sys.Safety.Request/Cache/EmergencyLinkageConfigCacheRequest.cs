using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request
{
    public partial class EmergencyLinkageConfigCacheLoadRequest : Basic.Framework.Web.BasicRequest
    {

    }

    public partial class EmergencyLinkageConfigCacheAddRequest : Basic.Framework.Web.BasicRequest
    {
        public SysEmergencyLinkageInfo SysEmergencyLinkageInfo { get; set; }
    }

    public partial class EmergencyLinkageConfigCacheUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public SysEmergencyLinkageInfo SysEmergencyLinkageInfo { get; set; }
    }

    public partial class EmergencyLinkageConfigCacheDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public SysEmergencyLinkageInfo SysEmergencyLinkageInfo { get; set; }
    }
    
    public partial class EmergencyLinkageConfigCacheGetAllRequest : Basic.Framework.Web.BasicRequest 
    {

    }

    public partial class EmergencyLinkageConfigCacheGetByKeyRequest : Basic.Framework.Web.BasicRequest 
    {
        public string Id { get; set; }
    }

    public partial class EmergencyLinkageConfigCacheGetByConditonRequest : Basic.Framework.Web.BasicRequest 
    {
        public Func<SysEmergencyLinkageInfo, bool> Predicate { get; set; }
    }
}
