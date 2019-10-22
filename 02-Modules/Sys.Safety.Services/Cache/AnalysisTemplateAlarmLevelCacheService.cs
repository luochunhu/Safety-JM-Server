using Basic.Framework.Web;
using Sys.Safety.Cache.Safety;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Services.Cache
{
    public class AnalysisTemplateAlarmLevelCacheService : IAnalysisTemplateAlarmLevelCacheService
    {
        public Basic.Framework.Web.BasicResponse LoadAnalysisTemplateAlarmLevelCache(Sys.Safety.Request.Cache.AnalysisTemplateAlarmLevelCacheLoadRequest AnalysisTemplateAlarmLevelCacheRequest)
        {
            AnalysisTemplateAlarmLevelCache.Instance.Load();
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse AddAnalysisTemplateAlarmLevelCache(Sys.Safety.Request.Cache.AnalysisTemplateAlarmLevelCacheAddRequest AnalysisTemplateAlarmLevelCacheRequest)
        {
            AnalysisTemplateAlarmLevelCache.Instance.AddItem(AnalysisTemplateAlarmLevelCacheRequest.AnalysistemplatealarmlevelInfo);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse DeleteAnalysisTemplateAlarmLevelCache(Sys.Safety.Request.Cache.AnalysisTemplateAlarmLevelCacheDeleteRequest AnalysisTemplateAlarmLevelCacheRequest)
        {
            AnalysisTemplateAlarmLevelCache.Instance.DeleteItem(AnalysisTemplateAlarmLevelCacheRequest.AnalysistemplatealarmlevelInfo);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse UpdateAnalysisTemplateAlarmLevelCache(Sys.Safety.Request.Cache.AnalysisTemplateAlarmLevelCacheUpdateRequest AnalysisTemplateAlarmLevelCacheRequest)
        {
            AnalysisTemplateAlarmLevelCache.Instance.UpdateItem(AnalysisTemplateAlarmLevelCacheRequest.AnalysistemplatealarmlevelInfo);
            return new BasicResponse();
        }


        public BasicResponse<DataContract.Jc_AnalysistemplatealarmlevelInfo> GetByIdAnalysisTemplateAlarmLevelCache(Sys.Safety.Request.Cache.AnalysisTemplateAlarmLevelCacheGetByIdRequest AnalysisTemplateAlarmLevelCacheRequest)
        {
            var info = AnalysisTemplateAlarmLevelCache.Instance.Query(o => o.Id == AnalysisTemplateAlarmLevelCacheRequest.Id).FirstOrDefault();

            BasicResponse<Jc_AnalysistemplatealarmlevelInfo> response = new BasicResponse<Jc_AnalysistemplatealarmlevelInfo>();
            response.Data = info;
            return response;
        }

        public BasicResponse<DataContract.Jc_AnalysistemplatealarmlevelInfo> GetByTemplateIdAnalysisTemplateAlarmLevelCache(Sys.Safety.Request.Cache.AnalysisTemplateAlarmLevelCacheGetByTemplateIdRequest AnalysisTemplateAlarmLevelCacheRequest)
        {
            var info = AnalysisTemplateAlarmLevelCache.Instance.Query(o => o.AnalysisModelId == AnalysisTemplateAlarmLevelCacheRequest.TemplateId).FirstOrDefault();
            BasicResponse<Jc_AnalysistemplatealarmlevelInfo> response = new BasicResponse<Jc_AnalysistemplatealarmlevelInfo>();
            response.Data = info;
            return response;
        }

        public BasicResponse<List<DataContract.Jc_AnalysistemplatealarmlevelInfo>> GetAllAnalysisTemplateAlarmLevelCache()
        {
            var infos = AnalysisTemplateAlarmLevelCache.Instance.Query();
            BasicResponse<List<Jc_AnalysistemplatealarmlevelInfo>> response = new BasicResponse<List<Jc_AnalysistemplatealarmlevelInfo>>();
            response.Data = infos;
            return response;
        }

        public BasicResponse<List<DataContract.Jc_AnalysistemplatealarmlevelInfo>> GetAnalysisTemplateAlarmLevelCache(Sys.Safety.Request.Cache.AnalysisTemplateAlarmLevelCacheGetByConditionRequest AnalysisTemplateAlarmLevelCacheRequest)
        {
            var infos = AnalysisTemplateAlarmLevelCache.Instance.Query(AnalysisTemplateAlarmLevelCacheRequest.Predicate);
            BasicResponse<List<Jc_AnalysistemplatealarmlevelInfo>> response = new BasicResponse<List<Jc_AnalysistemplatealarmlevelInfo>>();
            response.Data = infos;
            return response;
        }
    }
}
