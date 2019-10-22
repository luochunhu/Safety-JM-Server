using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Basic.Framework.Service;
using Basic.Framework.Web.WebApi;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.WebApi
{
    public class GasContentAnalyzeConfigController : BasicApiController, IGascontentanalyzeconfigService
    {
        private IGascontentanalyzeconfigService _gascontentanalyzeconfigService = ServiceFactory.Create<IGascontentanalyzeconfigService>();

        [HttpPost]
        [Route("v1/GasContentAnalyzeConfig/AddGascontentanalyzeconfig")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.GascontentanalyzeconfigInfo> AddGascontentanalyzeconfig(Sys.Safety.Request.Gascontentanalyzeconfig.GascontentanalyzeconfigAddRequest gascontentanalyzeconfigRequest)
        {
            return _gascontentanalyzeconfigService.AddGascontentanalyzeconfig(gascontentanalyzeconfigRequest);
        }

        [HttpPost]
        [Route("v1/GasContentAnalyzeConfig/UpdateGascontentanalyzeconfig")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.GascontentanalyzeconfigInfo> UpdateGascontentanalyzeconfig(Sys.Safety.Request.Gascontentanalyzeconfig.GascontentanalyzeconfigUpdateRequest gascontentanalyzeconfigRequest)
        {
            return _gascontentanalyzeconfigService.UpdateGascontentanalyzeconfig(gascontentanalyzeconfigRequest);
        }

        [HttpPost]
        [Route("v1/GasContentAnalyzeConfig/DeleteGascontentanalyzeconfig")]
        public Basic.Framework.Web.BasicResponse DeleteGascontentanalyzeconfig(Sys.Safety.Request.Gascontentanalyzeconfig.GascontentanalyzeconfigDeleteRequest gascontentanalyzeconfigRequest)
        {
            return _gascontentanalyzeconfigService.DeleteGascontentanalyzeconfig(gascontentanalyzeconfigRequest);
        }

        [HttpPost]
        [Route("v1/GasContentAnalyzeConfig/GetGascontentanalyzeconfigList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.GascontentanalyzeconfigInfo>> GetGascontentanalyzeconfigList(Sys.Safety.Request.Gascontentanalyzeconfig.GascontentanalyzeconfigGetListRequest gascontentanalyzeconfigRequest)
        {
            return _gascontentanalyzeconfigService.GetGascontentanalyzeconfigList(gascontentanalyzeconfigRequest);
        }

        [HttpPost]
        [Route("v1/GasContentAnalyzeConfig/GetAllGascontentanalyzeconfigList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.GascontentanalyzeconfigInfo>> GetAllGascontentanalyzeconfigList()
        {
            return _gascontentanalyzeconfigService.GetAllGascontentanalyzeconfigList();
        }

        [HttpPost]
        [Route("v1/GasContentAnalyzeConfig/GetGascontentanalyzeconfigById")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.GascontentanalyzeconfigInfo> GetGascontentanalyzeconfigById(Sys.Safety.Request.Gascontentanalyzeconfig.GascontentanalyzeconfigGetRequest gascontentanalyzeconfigRequest)
        {
            return _gascontentanalyzeconfigService.GetGascontentanalyzeconfigById(gascontentanalyzeconfigRequest);
        }

        [HttpPost]
        [Route("v1/GasContentAnalyzeConfig/GetAllGascontentanalyzeconfigListCache")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.GascontentanalyzeconfigInfo>> GetAllGascontentanalyzeconfigListCache()
        {
            return _gascontentanalyzeconfigService.GetAllGascontentanalyzeconfigListCache();
        }

        [HttpPost]
        [Route("v1/GasContentAnalyzeConfig/GetGascontentanalyzeconfigCacheById")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.GascontentanalyzeconfigInfo> GetGascontentanalyzeconfigCacheById(Sys.Safety.Request.Gascontentanalyzeconfig.GascontentanalyzeconfigGetRequest gascontentanalyzeconfigRequest)
        {
            return _gascontentanalyzeconfigService.GetGascontentanalyzeconfigCacheById(gascontentanalyzeconfigRequest);
        }
    }
}
