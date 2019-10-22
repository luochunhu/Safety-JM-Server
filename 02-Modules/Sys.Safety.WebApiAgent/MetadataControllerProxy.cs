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
    public class MetadataControllerProxy : BaseProxy, IMetadataService
    {
        public Basic.Framework.Web.BasicResponse<DataContract.MetadataInfo> AddMetadata(Sys.Safety.Request.Metadata.MetadataAddRequest metadatarequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Metadata/AddMetadata?token=" + Token, JSONHelper.ToJSONString(metadatarequest));
            return JSONHelper.ParseJSONString<BasicResponse<MetadataInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.MetadataInfo> UpdateMetadata(Sys.Safety.Request.Metadata.MetadataUpdateRequest metadatarequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Metadata/UpdateMetadata?token=" + Token, JSONHelper.ToJSONString(metadatarequest));
            return JSONHelper.ParseJSONString<BasicResponse<MetadataInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeleteMetadata(Sys.Safety.Request.Metadata.MetadataDeleteRequest metadatarequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Metadata/DeleteMetadata?token=" + Token, JSONHelper.ToJSONString(metadatarequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.MetadataInfo>> GetMetadataList(Sys.Safety.Request.Metadata.MetadataGetListRequest metadatarequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Metadata/GetMetadataList?token=" + Token, JSONHelper.ToJSONString(metadatarequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<DataContract.MetadataInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.MetadataInfo> GetMetadataById(Sys.Safety.Request.Metadata.MetadataGetRequest metadatarequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Metadata/GetMetadataById?token=" + Token, JSONHelper.ToJSONString(metadatarequest));
            return JSONHelper.ParseJSONString<BasicResponse<MetadataInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse ImportMetadata(Sys.Safety.Request.Metadata.ImportMetadataRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Metadata/ImportMetadata?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
    }
}
