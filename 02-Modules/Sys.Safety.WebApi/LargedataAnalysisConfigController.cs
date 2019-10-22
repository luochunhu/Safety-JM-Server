using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.JC_Largedataanalysisconfig;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sys.Safety.WebApi
{
    public class LargedataAnalysisConfigController : Basic.Framework.Web.WebApi.BasicApiController, ILargedataAnalysisConfigService
    {
        static LargedataAnalysisConfigController()
        {

        }
        ILargedataAnalysisConfigService _largedataAnalysisConfigService = ServiceFactory.Create<ILargedataAnalysisConfigService>();

        
        [HttpPost]
        [Route("v1/LargedataAnalysisConfig/AddLargeDataAnalysisConfig")]
        public BasicResponse<JC_LargedataAnalysisConfigInfo> AddLargeDataAnalysisConfig(LargedataAnalysisConfigAddRequest jc_LargedataAnalysisConfigRequest)
        {
            return _largedataAnalysisConfigService.AddLargeDataAnalysisConfig(jc_LargedataAnalysisConfigRequest);
        }
        [HttpPost]
        [Route("v1/LargedataAnalysisConfig/UpdateLargeDataAnalysisConfig")]
        public BasicResponse<JC_LargedataAnalysisConfigInfo> UpdateLargeDataAnalysisConfig(LargedataAnalysisConfigUpdateRequest jc_LargedataAnalysisConfigRequest)
        {
            return _largedataAnalysisConfigService.UpdateLargeDataAnalysisConfig(jc_LargedataAnalysisConfigRequest);
        }
        [HttpPost]
        [Route("v1/LargedataAnalysisConfig/DeleteLargeDataAnalysisConfig")]
        public BasicResponse DeleteLargeDataAnalysisConfig(LargedataAnalysisConfigDeleteRequest jc_LargedataAnalysisConfigRequest)
        {
            return _largedataAnalysisConfigService.DeleteLargeDataAnalysisConfig(jc_LargedataAnalysisConfigRequest);
        }
        [HttpPost]
        [Route("v1/LargedataAnalysisConfig/GetLargeDataAnalysisConfigList")]
        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigList(LargedataAnalysisConfigGetListRequest jc_LargedataAnalysisConfigRequest)
        {
            return _largedataAnalysisConfigService.GetLargeDataAnalysisConfigList(jc_LargedataAnalysisConfigRequest);
        }
        [HttpPost]
        [Route("v1/LargedataAnalysisConfig/GetLargeDataAnalysisConfigById")]
        public BasicResponse<JC_LargedataAnalysisConfigInfo> GetLargeDataAnalysisConfigById(LargedataAnalysisConfigGetRequest jc_LargedataAnalysisConfigRequest)
        {
            return _largedataAnalysisConfigService.GetLargeDataAnalysisConfigById(jc_LargedataAnalysisConfigRequest);
        }

        [HttpPost]
        [Route("v1/LargedataAnalysisConfig/GetAllLargeDataAnalysisConfigList")]
        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetAllLargeDataAnalysisConfigList(LargedataAnalysisConfigGetListRequest jc_LargedataAnalysisConfigRequest)
        {
            return _largedataAnalysisConfigService.GetAllLargeDataAnalysisConfigList(jc_LargedataAnalysisConfigRequest);
        }

        [HttpPost]
        [Route("v1/LargedataAnalysisConfig/GetAllEnabledLargeDataAnalysisConfigWithDetail")]
        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetAllEnabledLargeDataAnalysisConfigWithDetail(LargedataAnalysisConfigGetListRequest jc_LargedataAnalysisConfigRequest)
        {
            return _largedataAnalysisConfigService.GetAllLargeDataAnalysisConfigList(jc_LargedataAnalysisConfigRequest);
        }

        [HttpPost]
        [Route("v1/LargedataAnalysisConfig/GetLargeDataAnalysisConfigListByTempleteId")]
        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigListByTempleteId(LargedataAnalysisConfigGetRequest jc_LargedataAnalysisConfigRequest)
        {
            return _largedataAnalysisConfigService.GetLargeDataAnalysisConfigListByTempleteId(jc_LargedataAnalysisConfigRequest);
        }

        [HttpPost]
        [Route("v1/LargedataAnalysisConfig/GetLargedataAnalysisConfigDetailById")]
        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargedataAnalysisConfigDetailById(LargedataAnalysisConfigGetRequest jc_LargedataAnalysisConfigRsequest)
        {
            return _largedataAnalysisConfigService.GetLargedataAnalysisConfigDetailById(jc_LargedataAnalysisConfigRsequest);
        }

        [HttpPost]
        [Route("v1/LargedataAnalysisConfig/GetLargeDataAnalysisConfigListByName")]
        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigListByName(LargedataAnalysisConfigGetListByNameRequest jc_LargedataAnalysisConfigRequest)
        {
            return _largedataAnalysisConfigService.GetLargeDataAnalysisConfigListByName(jc_LargedataAnalysisConfigRequest);
        }

        [HttpPost]
        [Route("v1/LargedataAnalysisConfig/GetLargeDataAnalysisConfigWithoutAlarmConfigList")]
        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigWithoutAlarmConfigList(LargedataAnalysisConfigGetListRequest jc_LargedataAnalysisConfigRequest)
        {
            return _largedataAnalysisConfigService.GetLargeDataAnalysisConfigWithoutAlarmConfigList(jc_LargedataAnalysisConfigRequest);
        }

        [HttpPost]
        [Route("v1/LargedataAnalysisConfig/GetLargeDataAnalysisConfigWithRegionOutage")]
        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigWithRegionOutage(LargedataAnalysisConfigGetListWithRegionOutageRequest largedataAnalysisConfigGetListWithRegionOutageRequest)
        {
            return _largedataAnalysisConfigService.GetLargeDataAnalysisConfigWithRegionOutage(largedataAnalysisConfigGetListWithRegionOutageRequest);
        }
        [HttpPost]
        [Route("v1/LargedataAnalysisConfig/GetLargeDataAnalysisConfigWithoutRegionOutage")]
        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigWithoutRegionOutage(LargedataAnalysisConfigGetListRequest largedataAnalysisConfigGetListRequest)
        {
            return _largedataAnalysisConfigService.GetLargeDataAnalysisConfigWithoutRegionOutage(largedataAnalysisConfigGetListRequest);
        }

        [HttpPost]
        [Route("v1/LargedataAnalysisConfig/GetLargeDataAnalysisConfigListForCurve")]
        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigListForCurve(LargeDataAnalysisConfigListForCurveRequest largeDataAnalysisConfigListForCurveRequest)
        {
            return _largedataAnalysisConfigService.GetLargeDataAnalysisConfigListForCurve(largeDataAnalysisConfigListForCurveRequest);
        }

        [HttpPost]
        [Route("v1/LargedataAnalysisConfig/GetLargeDataAnalysisConfigWithRegionOutagePage")]
        public BasicResponse<List<JC_LargedataAnalysisConfigInfo>> GetLargeDataAnalysisConfigWithRegionOutagePage(LargedataAnalysisConfigGetListWithRegionOutageRequest largedataAnalysisConfigGetListWithRegionOutageRequest)
        {
            return _largedataAnalysisConfigService.GetLargeDataAnalysisConfigWithRegionOutagePage(largedataAnalysisConfigGetListWithRegionOutageRequest);
        }
    }
}
