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
    public class ListdatalayountControllerProxy : BaseProxy, IListdatalayountService
    {
        public Basic.Framework.Web.BasicResponse<DataContract.ListdatalayountInfo> AddListdatalayount(Sys.Safety.Request.Listdatalayount.ListdatalayountAddRequest listdatalayountrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listdatalayount/AddListdatalayount?token=" + Token, JSONHelper.ToJSONString(listdatalayountrequest));
            return JSONHelper.ParseJSONString<BasicResponse<ListdatalayountInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.ListdatalayountInfo> UpdateListdatalayount(Sys.Safety.Request.Listdatalayount.ListdatalayountUpdateRequest listdatalayountrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listdatalayount/UpdateListdatalayount?token=" + Token, JSONHelper.ToJSONString(listdatalayountrequest));
            return JSONHelper.ParseJSONString<BasicResponse<ListdatalayountInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeleteListdatalayount(Sys.Safety.Request.Listdatalayount.ListdatalayountDeleteRequest listdatalayountrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listdatalayount/DeleteListdatalayount?token=" + Token, JSONHelper.ToJSONString(listdatalayountrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.ListdatalayountInfo>> GetListdatalayountList(Sys.Safety.Request.Listdatalayount.ListdatalayountGetListRequest listdatalayountrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listdatalayount/GetListdatalayountList?token=" + Token, JSONHelper.ToJSONString(listdatalayountrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<DataContract.ListdatalayountInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.ListdatalayountInfo> GetListdatalayountById(Sys.Safety.Request.Listdatalayount.ListdatalayountGetRequest listdatalayountrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listdatalayount/GetListdatalayountById?token=" + Token, JSONHelper.ToJSONString(listdatalayountrequest));
            return JSONHelper.ParseJSONString<BasicResponse<ListdatalayountInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse SaveListDataLayountInfo(Sys.Safety.Request.Listdatalayount.SaveListDataLayountInfoRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listdatalayount/SaveListDataLayountInfo?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }


        public BasicResponse DeleteListdatalayountByTimeListDataId(Sys.Safety.Request.Listdatalayount.DeleteListdatalayountByTimeListDataIdRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listdatalayount/DeleteListdatalayountByTimeListDataId?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }


        public BasicResponse<IList<ListdatalayountInfo>> GetListdatalayountByListDataId(Sys.Safety.Request.Listdatalayount.GetListdatalayountByListDataIdRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listdatalayount/GetListdatalayountByListDataId?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<IList<ListdatalayountInfo>>>(responseStr);
        }


        public BasicResponse<ListdatalayountInfo> GetListdatalayountByListDataIdArrangeName(Sys.Safety.Request.Listdatalayount.GetListdatalayountByListDataIdArrangeTimeRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listdatalayount/GetListdatalayountByListDataIdArrangeTime?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<ListdatalayountInfo>>(responseStr);
        }
    }
}
