using Basic.Framework.Web;
using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Cache
{
    public class AnalysisTemplateAlarmLevelCacheLoadRequest : BasicRequest
    {

    }

    public class AnalysisTemplateAlarmLevelCacheAddRequest : BasicRequest
    {
        public Jc_AnalysistemplatealarmlevelInfo AnalysistemplatealarmlevelInfo { get; set; }
    }

    public class AnalysisTemplateAlarmLevelCacheUpdateRequest : BasicRequest
    {
        public Jc_AnalysistemplatealarmlevelInfo AnalysistemplatealarmlevelInfo { get; set; }
    }

    public class AnalysisTemplateAlarmLevelCacheDeleteRequest : BasicRequest
    {
        public Jc_AnalysistemplatealarmlevelInfo AnalysistemplatealarmlevelInfo { get; set; }
    }

    public class AnalysisTemplateAlarmLevelCacheGetByIdRequest : BasicRequest 
    {
        public string Id { get; set; }
    }

    public class AnalysisTemplateAlarmLevelCacheGetByTemplateIdRequest : BasicRequest
    {
        public string TemplateId { get; set; }
    }

    public class AnalysisTemplateAlarmLevelCacheGetByConditionRequest : BasicRequest
    {
        public Func<Jc_AnalysistemplatealarmlevelInfo, bool> Predicate { get; set; }
    }
}
