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
    public class ListdisplayexControllerProxy : BaseProxy, IListdisplayexService
    {
        public Basic.Framework.Web.BasicResponse<DataContract.ListdisplayexInfo> AddListdisplayex(Sys.Safety.Request.Listdisplayex.ListdisplayexAddRequest listdisplayexrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listdisplayex/AddListdisplayex?token=" + Token, JSONHelper.ToJSONString(listdisplayexrequest));
            return JSONHelper.ParseJSONString<BasicResponse<ListdisplayexInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.ListdisplayexInfo> UpdateListdisplayex(Sys.Safety.Request.Listdisplayex.ListdisplayexUpdateRequest listdisplayexrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listdisplayex/UpdateListdisplayex?token=" + Token, JSONHelper.ToJSONString(listdisplayexrequest));
            return JSONHelper.ParseJSONString<BasicResponse<ListdisplayexInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeleteListdisplayex(Sys.Safety.Request.Listdisplayex.ListdisplayexDeleteRequest listdisplayexrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listdisplayex/DeleteListdisplayex?token=" + Token, JSONHelper.ToJSONString(listdisplayexrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.ListdisplayexInfo>> GetListdisplayexList(Sys.Safety.Request.Listdisplayex.ListdisplayexGetListRequest listdisplayexrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listdisplayex/GetListdisplayexList?token=" + Token, JSONHelper.ToJSONString(listdisplayexrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<DataContract.ListdisplayexInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.ListdisplayexInfo> GetListdisplayexById(Sys.Safety.Request.Listdisplayex.ListdisplayexGetRequest listdisplayexrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listdisplayex/GetListdisplayexById?token=" + Token, JSONHelper.ToJSONString(listdisplayexrequest));
            return JSONHelper.ParseJSONString<BasicResponse<ListdisplayexInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse SaveListDisplayExInfo(Sys.Safety.Request.Listdisplayex.SaveListDisplayExInfoRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listdisplayex/SaveListDisplayExInfo?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
    }
}
