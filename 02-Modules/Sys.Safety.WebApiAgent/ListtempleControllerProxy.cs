using System;
using System.Collections.Generic;
using System.Data;
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
    public class ListtempleControllerProxy : BaseProxy, IListtempleService
    {
        public Basic.Framework.Web.BasicResponse<DataContract.ListtempleInfo> AddListtemple(Sys.Safety.Request.Listtemple.ListtempleAddRequest listtemplerequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listtemple/AddListtemple?token=" + Token, JSONHelper.ToJSONString(listtemplerequest));
            return JSONHelper.ParseJSONString<BasicResponse<ListtempleInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.ListtempleInfo> UpdateListtemple(Sys.Safety.Request.Listtemple.ListtempleUpdateRequest listtemplerequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listtemple/UpdateListtemple?token=" + Token, JSONHelper.ToJSONString(listtemplerequest));
            return JSONHelper.ParseJSONString<BasicResponse<ListtempleInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeleteListtemple(Sys.Safety.Request.Listtemple.ListtempleDeleteRequest listtemplerequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listtemple/DeleteListtemple?token=" + Token, JSONHelper.ToJSONString(listtemplerequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.ListtempleInfo>> GetListtempleList(Sys.Safety.Request.Listtemple.ListtempleGetListRequest listtemplerequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listtemple/GetListtempleList?token=" + Token, JSONHelper.ToJSONString(listtemplerequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<DataContract.ListtempleInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.ListtempleInfo> GetListtempleById(Sys.Safety.Request.Listtemple.ListtempleGetRequest listtemplerequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listtemple/GetListtempleById?token=" + Token, JSONHelper.ToJSONString(listtemplerequest));
            return JSONHelper.ParseJSONString<BasicResponse<ListtempleInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse SaveListTempleInfo(Sys.Safety.Request.Listtemple.SaveListTempleInfoRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listtemple/SaveListTempleInfo?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.ListtempleInfo> GetListtempleByListDataID(Sys.Safety.Request.Listex.IdRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listtemple/GetListtempleByListDataID?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<ListtempleInfo>>(responseStr);
        }


        public BasicResponse<System.Data.DataTable> GetNameFromListDataExListEx(Sys.Safety.Request.Listex.IdRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listtemple/GetNameFromListDataExListEx?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responseStr);
        }
    }
}
