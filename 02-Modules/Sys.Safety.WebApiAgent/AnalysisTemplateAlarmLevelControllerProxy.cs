using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent
{
    public class AnalysisTemplateAlarmLevelControllerProxy : BaseProxy, IJc_AnalysistemplatealarmlevelService
    {
        public Basic.Framework.Web.BasicResponse<DataContract.Jc_AnalysistemplatealarmlevelInfo> AddAnalysistemplatealarmlevel(Sys.Safety.Request.Analysistemplatealarmlevel.AnalysistemplatealarmlevelAddRequest analysistemplatealarmlevelRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalysisTemplateAlarmLevel/AddAnalysistemplatealarmlevel?token=" + Token, JSONHelper.ToJSONString(analysistemplatealarmlevelRequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_AnalysistemplatealarmlevelInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.Jc_AnalysistemplatealarmlevelInfo> UpdateAnalysistemplatealarmlevel(Sys.Safety.Request.Analysistemplatealarmlevel.AnalysistemplatealarmlevelUpdateRequest analysistemplatealarmlevelRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalysisTemplateAlarmLevel/UpdateAnalysistemplatealarmlevel?token=" + Token, JSONHelper.ToJSONString(analysistemplatealarmlevelRequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_AnalysistemplatealarmlevelInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeleteAnalysistemplatealarmlevel(Sys.Safety.Request.Analysistemplatealarmlevel.AnalysistemplatealarmlevelDeleteRequest analysistemplatealarmlevelRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalysisTemplateAlarmLevel/DeleteAnalysistemplatealarmlevel?token=" + Token, JSONHelper.ToJSONString(analysistemplatealarmlevelRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.Jc_AnalysistemplatealarmlevelInfo>> GetAnalysistemplatealarmlevelList(Sys.Safety.Request.Analysistemplatealarmlevel.AnalysistemplatealarmlevelGetListRequest analysistemplatealarmlevelRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalysisTemplateAlarmLevel/GetAnalysistemplatealarmlevelList?token=" + Token, JSONHelper.ToJSONString(analysistemplatealarmlevelRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_AnalysistemplatealarmlevelInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.Jc_AnalysistemplatealarmlevelInfo> GetAnalysistemplatealarmlevelById(Sys.Safety.Request.Analysistemplatealarmlevel.AnalysistemplatealarmlevelGetRequest analysistemplatealarmlevelRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalysisTemplateAlarmLevel/GetAnalysistemplatealarmlevelById?token=" + Token, JSONHelper.ToJSONString(analysistemplatealarmlevelRequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_AnalysistemplatealarmlevelInfo>>(responseStr);
        }

        public BasicResponse<Jc_AnalysistemplatealarmlevelInfo> GetAnalysistemplatealarmlevelByAnalysistemplateId(Sys.Safety.Request.Analysistemplatealarmlevel.AnalysistemplatealarmlevelGetByAnalysistemplateIdRequest analysistemplatealarmlevelRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalysisTemplateAlarmLevel/GetAnalysistemplatealarmlevelByAnalysistemplateId?token=" + Token, JSONHelper.ToJSONString(analysistemplatealarmlevelRequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_AnalysistemplatealarmlevelInfo>>(responseStr);
        }

        public BasicResponse<List<Jc_AnalysistemplatealarmlevelInfo>> GetAllAnalysistemplateAlarmLevelInfos()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AnalysisTemplateAlarmLevel/GetAllAnalysistemplateAlarmLevelInfos?token=" + Token, "");
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_AnalysistemplatealarmlevelInfo>>>(responseStr);
        }
    }
}
