using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Analysistemplatealarmlevel;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IJc_AnalysistemplatealarmlevelService
    {
        BasicResponse<Jc_AnalysistemplatealarmlevelInfo> AddAnalysistemplatealarmlevel(AnalysistemplatealarmlevelAddRequest analysistemplatealarmlevelRequest);
        BasicResponse<Jc_AnalysistemplatealarmlevelInfo> UpdateAnalysistemplatealarmlevel(AnalysistemplatealarmlevelUpdateRequest analysistemplatealarmlevelRequest);
        BasicResponse DeleteAnalysistemplatealarmlevel(AnalysistemplatealarmlevelDeleteRequest analysistemplatealarmlevelRequest);
        BasicResponse<List<Jc_AnalysistemplatealarmlevelInfo>> GetAnalysistemplatealarmlevelList(AnalysistemplatealarmlevelGetListRequest analysistemplatealarmlevelRequest);
        BasicResponse<Jc_AnalysistemplatealarmlevelInfo> GetAnalysistemplatealarmlevelById(AnalysistemplatealarmlevelGetRequest analysistemplatealarmlevelRequest);

        BasicResponse<Jc_AnalysistemplatealarmlevelInfo> GetAnalysistemplatealarmlevelByAnalysistemplateId(AnalysistemplatealarmlevelGetByAnalysistemplateIdRequest analysistemplatealarmlevelRequest);

        BasicResponse<List<Jc_AnalysistemplatealarmlevelInfo>> GetAllAnalysistemplateAlarmLevelInfos();
    }
}

