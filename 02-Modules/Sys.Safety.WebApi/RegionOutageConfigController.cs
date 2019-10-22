using Basic.Framework.Service;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.RegionOutageConfig;

namespace Sys.Safety.WebApi
{
    public class RegionOutageConfigController : Basic.Framework.Web.WebApi.BasicApiController, IRegionOutageConfigService
    {
        static RegionOutageConfigController()
        {

        }
        IRegionOutageConfigService _regionOutageConfigService = ServiceFactory.Create<IRegionOutageConfigService>();

       
        [HttpPost]
        [Route("v1/RegionOutageConfig/AddJC_RegionOutageConfigList")]
        public BasicResponse<List<JC_RegionOutageConfigInfo>> AddJC_RegionOutageConfigList(RegionOutageConfigListAddRequest jC_RegionOutageConfigListAddRequest)
        {
            return _regionOutageConfigService.AddJC_RegionOutageConfigList(jC_RegionOutageConfigListAddRequest);
        }
        [HttpPost]
        [Route("v1/RegionOutageConfig/AddJC_Regionoutageconfig")]
        public BasicResponse<JC_RegionOutageConfigInfo> AddJC_Regionoutageconfig(RegionOutageConfigAddRequest jC_Regionoutageconfigrequest)
        {
            return _regionOutageConfigService.AddJC_Regionoutageconfig(jC_Regionoutageconfigrequest);
        }
        [HttpPost]
        [Route("v1/RegionOutageConfig/UpdateJC_Regionoutageconfig")]
        public BasicResponse<JC_RegionOutageConfigInfo> UpdateJC_Regionoutageconfig(RegionOutageConfigUpdateRequest jC_Regionoutageconfigrequest)
        {
            return _regionOutageConfigService.UpdateJC_Regionoutageconfig(jC_Regionoutageconfigrequest);
        }
        [HttpPost]
        [Route("v1/RegionOutageConfig/DeleteJC_Regionoutageconfig")]
        public BasicResponse DeleteJC_Regionoutageconfig(RegionoutageconfigDeleteRequest jC_Regionoutageconfigrequest)
        {
            return _regionOutageConfigService.DeleteJC_Regionoutageconfig(jC_Regionoutageconfigrequest);
        }
        [HttpPost]
        [Route("v1/RegionOutageConfig/DeleteJC_RegionoutageconfigByAnalysisModelId")]
        public BasicResponse DeleteJC_RegionoutageconfigByAnalysisModelId(RegionoutageconfigDeleteByAnalysisModelIdRequest jC_Regionoutageconfigrequest)
        {
            return _regionOutageConfigService.DeleteJC_RegionoutageconfigByAnalysisModelId(jC_Regionoutageconfigrequest);
        }
        [HttpPost]
        [Route("v1/RegionOutageConfig/GetJC_RegionoutageconfigList")]
        public BasicResponse<List<JC_RegionOutageConfigInfo>> GetJC_RegionoutageconfigList(RegionOutageConfigGetListRequest jC_Regionoutageconfigrequest)
        {
            return _regionOutageConfigService.GetJC_RegionoutageconfigList(jC_Regionoutageconfigrequest);
        }
        [HttpPost]
        [Route("v1/RegionOutageConfig/GetJC_RegionoutageconfigById")]
        public BasicResponse<JC_RegionOutageConfigInfo> GetJC_RegionoutageconfigById(RegionOutageConfigGetRequest jC_Regionoutageconfigrequest)
        {
            return _regionOutageConfigService.GetJC_RegionoutageconfigById(jC_Regionoutageconfigrequest);
        }
        [HttpPost]
        [Route("v1/RegionOutageConfig/GetRegionOutageConfigListByAnalysisModelId")]
        public BasicResponse<List<JC_RegionOutageConfigInfo>> GetRegionOutageConfigListByAnalysisModelId(RegionOutageConfigGetListRequest regionOutageConfigGetListRequest)
        {
            return _regionOutageConfigService.GetRegionOutageConfigListByAnalysisModelId(regionOutageConfigGetListRequest);
        }
        [HttpPost]
        [Route("v1/RegionOutageConfig/NoReleaseControlForAnalysysModelAndPoint")]
        public BasicResponse NoReleaseControlForAnalysysModelAndPoint(ReleaseControlCheckRequest releaseControlCheckRequest)
        {
            return _regionOutageConfigService.NoReleaseControlForAnalysysModelAndPoint(releaseControlCheckRequest);
        }
        [HttpPost]
        [Route("v1/RegionOutageConfig/HasRegionOutageForAnalysisModel")]
        public BasicResponse<bool> HasRegionOutageForAnalysisModel(GetByAnalysisModelIdRequest getByAnalysisModelIdRequest)
        {
            return _regionOutageConfigService.HasRegionOutageForAnalysisModel(getByAnalysisModelIdRequest);
        }

        [HttpPost]
        [Route("v1/RegionOutageConfig/GetRegionOutageConfigAllList")]
        public BasicResponse<List<JC_RegionOutageConfigInfo>> GetRegionOutageConfigAllList(GetAllRegionOutageConfigRequest getAllRegionOutageConfigRequest)
        {
            return _regionOutageConfigService.GetRegionOutageConfigAllList(getAllRegionOutageConfigRequest);
        }
    }
}
