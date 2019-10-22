using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.ServiceContract.Cache
{
    public interface IAnalysisTemplateAlarmLevelCacheService
    {
        BasicResponse LoadAnalysisTemplateAlarmLevelCache(AnalysisTemplateAlarmLevelCacheLoadRequest AnalysisTemplateAlarmLevelCacheRequest);

        BasicResponse AddAnalysisTemplateAlarmLevelCache(AnalysisTemplateAlarmLevelCacheAddRequest AnalysisTemplateAlarmLevelCacheRequest);

        BasicResponse DeleteAnalysisTemplateAlarmLevelCache(AnalysisTemplateAlarmLevelCacheDeleteRequest AnalysisTemplateAlarmLevelCacheRequest);

        BasicResponse UpdateAnalysisTemplateAlarmLevelCache(AnalysisTemplateAlarmLevelCacheUpdateRequest AnalysisTemplateAlarmLevelCacheRequest);

        BasicResponse<Jc_AnalysistemplatealarmlevelInfo> GetByIdAnalysisTemplateAlarmLevelCache(AnalysisTemplateAlarmLevelCacheGetByIdRequest AnalysisTemplateAlarmLevelCacheRequest);

        BasicResponse<Jc_AnalysistemplatealarmlevelInfo> GetByTemplateIdAnalysisTemplateAlarmLevelCache(AnalysisTemplateAlarmLevelCacheGetByTemplateIdRequest AnalysisTemplateAlarmLevelCacheRequest);

        BasicResponse<List<Jc_AnalysistemplatealarmlevelInfo>> GetAllAnalysisTemplateAlarmLevelCache();

        BasicResponse<List<Jc_AnalysistemplatealarmlevelInfo>> GetAnalysisTemplateAlarmLevelCache(AnalysisTemplateAlarmLevelCacheGetByConditionRequest AnalysisTemplateAlarmLevelCacheRequest);
    }
}
