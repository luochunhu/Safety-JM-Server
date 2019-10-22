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
    public class ListdataremarkControllerProxy : BaseProxy, IListdataremarkService
    {
        public Basic.Framework.Web.BasicResponse<DataContract.ListdataremarkInfo> AddListdataremark(Sys.Safety.Request.Listdataremark.ListdataremarkAddRequest listdataremarkRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listdataremark/AddListdataremark?token=" + Token, JSONHelper.ToJSONString(listdataremarkRequest));
            return JSONHelper.ParseJSONString<BasicResponse<ListdataremarkInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.ListdataremarkInfo> UpdateListdataremark(Sys.Safety.Request.Listdataremark.ListdataremarkUpdateRequest listdataremarkRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listdataremark/UpdateListdataremark?token=" + Token, JSONHelper.ToJSONString(listdataremarkRequest));
            return JSONHelper.ParseJSONString<BasicResponse<ListdataremarkInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeleteListdataremark(Sys.Safety.Request.Listdataremark.ListdataremarkDeleteRequest listdataremarkRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listdataremark/DeleteListdataremark?token=" + Token, JSONHelper.ToJSONString(listdataremarkRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.ListdataremarkInfo>> GetListdataremarkList(Sys.Safety.Request.Listdataremark.ListdataremarkGetListRequest listdataremarkRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listdataremark/GetListdataremarkList?token=" + Token, JSONHelper.ToJSONString(listdataremarkRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<DataContract.ListdataremarkInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.ListdataremarkInfo> GetListdataremarkById(Sys.Safety.Request.Listdataremark.ListdataremarkGetRequest listdataremarkRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listdataremark/GetListdataremarkById?token=" + Token, JSONHelper.ToJSONString(listdataremarkRequest));
            return JSONHelper.ParseJSONString<BasicResponse<ListdataremarkInfo>>(responseStr);
        }
        
        public BasicResponse<ListdataremarkInfo> GetListdataremarkByTimeListDataId(Sys.Safety.Request.Listdataremark.GetListdataremarkByTimeListDataIdRequest listdataremarkRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listdataremark/GetListdataremarkByTimeListDataId?token=" + Token, JSONHelper.ToJSONString(listdataremarkRequest));
            return JSONHelper.ParseJSONString<BasicResponse<ListdataremarkInfo>>(responseStr);
        }


        public BasicResponse<ListdataremarkInfo> UpdateListdataremarkByTimeListDataId(Sys.Safety.Request.Listdataremark.ListdataremarkUpdateRequest listdataremarkRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listdataremark/UpdateListdataremarkByTimeListDataId?token=" + Token, JSONHelper.ToJSONString(listdataremarkRequest));
            return JSONHelper.ParseJSONString<BasicResponse<ListdataremarkInfo>>(responseStr);
        }
    }
}
