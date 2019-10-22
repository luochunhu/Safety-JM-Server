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
    public class MetadatafieldsControllerProxy : BaseProxy, IMetadatafieldsService
    {
        public Basic.Framework.Web.BasicResponse<DataContract.MetadatafieldsInfo> AddMetadatafields(Sys.Safety.Request.Metadatafields.MetadatafieldsAddRequest metadatafieldsrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Metadatafields/AddMetadatafields?token=" + Token, JSONHelper.ToJSONString(metadatafieldsrequest));
            return JSONHelper.ParseJSONString<BasicResponse<MetadatafieldsInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.MetadatafieldsInfo> UpdateMetadatafields(Sys.Safety.Request.Metadatafields.MetadatafieldsUpdateRequest metadatafieldsrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Metadatafields/UpdateMetadatafields?token=" + Token, JSONHelper.ToJSONString(metadatafieldsrequest));
            return JSONHelper.ParseJSONString<BasicResponse<MetadatafieldsInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeleteMetadatafields(Sys.Safety.Request.Metadatafields.MetadatafieldsDeleteRequest metadatafieldsrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Metadatafields/DeleteMetadatafields?token=" + Token, JSONHelper.ToJSONString(metadatafieldsrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.MetadatafieldsInfo>> GetMetadatafieldsList(Sys.Safety.Request.Metadatafields.MetadatafieldsGetListRequest metadatafieldsrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Metadatafields/GetMetadatafieldsList?token=" + Token, JSONHelper.ToJSONString(metadatafieldsrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<DataContract.MetadatafieldsInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.MetadatafieldsInfo> GetMetadatafieldsById(Sys.Safety.Request.Metadatafields.MetadatafieldsGetRequest metadatafieldsrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Metadatafields/GetMetadatafieldsById?token=" + Token, JSONHelper.ToJSONString(metadatafieldsrequest));
            return JSONHelper.ParseJSONString<BasicResponse<MetadatafieldsInfo>>(responseStr);
        }
    }
}
