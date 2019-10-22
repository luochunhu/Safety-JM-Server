using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.WebApiAgent
{
    public class GascontentanalyzeconfigControllerProxy : BaseProxy, IGascontentanalyzeconfigService
    {
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.GascontentanalyzeconfigInfo> AddGascontentanalyzeconfig(Sys.Safety.Request.Gascontentanalyzeconfig.GascontentanalyzeconfigAddRequest gascontentanalyzeconfigRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/GasContentAnalyzeConfig/AddGascontentanalyzeconfig?token=" + Token, JSONHelper.ToJSONString(gascontentanalyzeconfigRequest));
            return JSONHelper.ParseJSONString<BasicResponse<GascontentanalyzeconfigInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.GascontentanalyzeconfigInfo> UpdateGascontentanalyzeconfig(Sys.Safety.Request.Gascontentanalyzeconfig.GascontentanalyzeconfigUpdateRequest gascontentanalyzeconfigRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/GasContentAnalyzeConfig/UpdateGascontentanalyzeconfig?token=" + Token, JSONHelper.ToJSONString(gascontentanalyzeconfigRequest));
            return JSONHelper.ParseJSONString<BasicResponse<GascontentanalyzeconfigInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeleteGascontentanalyzeconfig(Sys.Safety.Request.Gascontentanalyzeconfig.GascontentanalyzeconfigDeleteRequest gascontentanalyzeconfigRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/GasContentAnalyzeConfig/DeleteGascontentanalyzeconfig?token=" + Token, JSONHelper.ToJSONString(gascontentanalyzeconfigRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.GascontentanalyzeconfigInfo>> GetGascontentanalyzeconfigList(Sys.Safety.Request.Gascontentanalyzeconfig.GascontentanalyzeconfigGetListRequest gascontentanalyzeconfigRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/GasContentAnalyzeConfig/GetGascontentanalyzeconfigList?token=" + Token, JSONHelper.ToJSONString(gascontentanalyzeconfigRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Sys.Safety.DataContract.GascontentanalyzeconfigInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.GascontentanalyzeconfigInfo>> GetAllGascontentanalyzeconfigList()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/GasContentAnalyzeConfig/GetAllGascontentanalyzeconfigList?token=" + Token, "");
            return JSONHelper.ParseJSONString<BasicResponse<List<Sys.Safety.DataContract.GascontentanalyzeconfigInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.GascontentanalyzeconfigInfo> GetGascontentanalyzeconfigById(Sys.Safety.Request.Gascontentanalyzeconfig.GascontentanalyzeconfigGetRequest gascontentanalyzeconfigRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/GasContentAnalyzeConfig/GetGascontentanalyzeconfigById?token=" + Token, JSONHelper.ToJSONString(gascontentanalyzeconfigRequest));
            return JSONHelper.ParseJSONString<BasicResponse<Sys.Safety.DataContract.GascontentanalyzeconfigInfo>>(responseStr);
        }


        public BasicResponse<List<GascontentanalyzeconfigInfo>> GetAllGascontentanalyzeconfigListCache()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/GasContentAnalyzeConfig/GetAllGascontentanalyzeconfigListCache?token=" + Token, "");
            return JSONHelper.ParseJSONString<BasicResponse<List<GascontentanalyzeconfigInfo>>>(responseStr);
        }

        public BasicResponse<GascontentanalyzeconfigInfo> GetGascontentanalyzeconfigCacheById(Sys.Safety.Request.Gascontentanalyzeconfig.GascontentanalyzeconfigGetRequest gascontentanalyzeconfigRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/GasContentAnalyzeConfig/GetGascontentanalyzeconfigCacheById?token=" + Token, JSONHelper.ToJSONString(gascontentanalyzeconfigRequest));
            return JSONHelper.ParseJSONString<BasicResponse<Sys.Safety.DataContract.GascontentanalyzeconfigInfo>>(responseStr);
        }
    }
}
