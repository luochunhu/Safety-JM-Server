using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.JC_Largedataanalysisconfig;
using Sys.Safety.Request.JC_Largedataanalysislog;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sys.Safety.WebApi
{
    public class LargedataAnalysisLogController: Basic.Framework.Web.WebApi.BasicApiController, ILargedataAnalysisLogService
    {
        static LargedataAnalysisLogController()
        {

        }
        ILargedataAnalysisLogService _largedataAnalysisLogService = ServiceFactory.Create<ILargedataAnalysisLogService>();

        [HttpPost]
        [Route("v1/LargedataAnalysisLog/AddJC_Largedataanalysislog")]
        public BasicResponse<JC_LargedataAnalysisLogInfo> AddJC_Largedataanalysislog(LargedataAnalysisLogAddRequest jC_Largedataanalysislogrequest)
        {
            return _largedataAnalysisLogService.AddJC_Largedataanalysislog(jC_Largedataanalysislogrequest);
        }
        [HttpPost]
        [Route("v1/LargedataAnalysisLog/UpdateJC_Largedataanalysislog")]
        public BasicResponse<JC_LargedataAnalysisLogInfo> UpdateJC_Largedataanalysislog(LargedataAnalysisLogUpdateRequest jC_Largedataanalysislogrequest)
        {
            return _largedataAnalysisLogService.UpdateJC_Largedataanalysislog(jC_Largedataanalysislogrequest);
        }
        [HttpPost]
        [Route("v1/LargedataAnalysisLog/DeleteJC_Largedataanalysislog")]
        public BasicResponse DeleteJC_Largedataanalysislog(LargedataanalysislogDeleteRequest jC_Largedataanalysislogrequest)
        {
            return _largedataAnalysisLogService.DeleteJC_Largedataanalysislog(jC_Largedataanalysislogrequest);
        }
        [HttpPost]
        [Route("v1/LargedataAnalysisLog/GetJC_LargedataanalysislogList")]
        public BasicResponse<List<JC_LargedataAnalysisLogInfo>> GetJC_LargedataanalysislogList(LargedataAnalysisLogGetListRequest jC_Largedataanalysislogrequest)
        {
            return _largedataAnalysisLogService.GetJC_LargedataanalysislogList(jC_Largedataanalysislogrequest);
        }
        [HttpPost]
        [Route("v1/LargedataAnalysisLog/GetJC_LargedataanalysislogById")]
        public BasicResponse<JC_LargedataAnalysisLogInfo> GetJC_LargedataanalysislogById(LargedataAnalysisLogGetRequest jC_Largedataanalysislogrequest)
        {
            return _largedataAnalysisLogService.GetJC_LargedataanalysislogById(jC_Largedataanalysislogrequest);
        }
        [HttpPost]
        [Route("v1/LargedataAnalysisLog/GetLargedataAnalysisLogListByAnalysisModelId")]
        public BasicResponse<List<JC_LargedataAnalysisLogInfo>> GetLargedataAnalysisLogListByAnalysisModelId(LargedataAnalysisLogGetByAnalysisModelIdRequest largedataAnalysisLogGetByAnalysisModelIdRequest)
        {
            return _largedataAnalysisLogService.GetLargedataAnalysisLogListByAnalysisModelId(largedataAnalysisLogGetByAnalysisModelIdRequest);
        }
        [HttpPost]
        [Route("v1/LargedataAnalysisLog/GetLargedataAnalysisLogListByModelIdAndTime")]
        public BasicResponse<List<JC_LargedataAnalysisLogInfo>> GetLargedataAnalysisLogListByModelIdAndTime(LargedataAnalysisLogGetListByModelIdAndTimeRequest largedataAnalysisLogGetListByModelIdAndTimeRequest)
        {
            return _largedataAnalysisLogService.GetLargedataAnalysisLogListByModelIdAndTime(largedataAnalysisLogGetListByModelIdAndTimeRequest);
        }
        [HttpPost]
        [Route("v1/LargedataAnalysisLog/GetLargedataAnalysisLogLatestByAnalysisModelId")]
        public BasicResponse<JC_LargedataAnalysisLogInfo> GetLargedataAnalysisLogLatestByAnalysisModelId(LargedataAnalysisLogGetByAnalysisModelIdRequest largedataAnalysisLogGetByAnalysisModelIdRequest)
        {
            return _largedataAnalysisLogService.GetLargedataAnalysisLogLatestByAnalysisModelId(largedataAnalysisLogGetByAnalysisModelIdRequest);
        }
    }
}
