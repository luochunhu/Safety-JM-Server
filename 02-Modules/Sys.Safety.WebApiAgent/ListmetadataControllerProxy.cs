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
    public class ListmetadataControllerProxy : BaseProxy, IListmetadataService
    {
        public Basic.Framework.Web.BasicResponse<DataContract.ListmetadataInfo> AddListmetadata(Sys.Safety.Request.Listmetadata.ListmetadataAddRequest listmetadatarequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listmetadata/AddListmetadata?token=" + Token, JSONHelper.ToJSONString(listmetadatarequest));
            return JSONHelper.ParseJSONString<BasicResponse<ListmetadataInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.ListmetadataInfo> UpdateListmetadata(Sys.Safety.Request.Listmetadata.ListmetadataUpdateRequest listmetadatarequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listmetadata/UpdateListmetadata?token=" + Token, JSONHelper.ToJSONString(listmetadatarequest));
            return JSONHelper.ParseJSONString<BasicResponse<ListmetadataInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeleteListmetadata(Sys.Safety.Request.Listmetadata.ListmetadataDeleteRequest listmetadatarequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listmetadata/DeleteListmetadata?token=" + Token, JSONHelper.ToJSONString(listmetadatarequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.ListmetadataInfo>> GetListmetadataList(Sys.Safety.Request.Listmetadata.ListmetadataGetListRequest listmetadatarequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listmetadata/GetListmetadataList?token=" + Token, JSONHelper.ToJSONString(listmetadatarequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<DataContract.ListmetadataInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.ListmetadataInfo> GetListmetadataById(Sys.Safety.Request.Listmetadata.ListmetadataGetRequest listmetadatarequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listmetadata/GetListmetadataById?token=" + Token, JSONHelper.ToJSONString(listmetadatarequest));
            return JSONHelper.ParseJSONString<BasicResponse<ListmetadataInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse SaveListMetaDataExInfo(Sys.Safety.Request.Listmetadata.SaveListMetaDataExInfoRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listmetadata/SaveListMetaDataExInfo?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
    }
}
