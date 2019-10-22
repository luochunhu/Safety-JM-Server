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
    public class ListdataexControllerProxy : BaseProxy, IListdataexService
    {
        public Basic.Framework.Web.BasicResponse<DataContract.ListdataexInfo> AddListdataex(Sys.Safety.Request.Listdataex.ListdataexAddRequest listdataexrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listdataex/AddListdataex?token=" + Token, JSONHelper.ToJSONString(listdataexrequest));
            return JSONHelper.ParseJSONString <BasicResponse<ListdataexInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.ListdataexInfo> UpdateListdataex(Sys.Safety.Request.Listdataex.ListdataexUpdateRequest listdataexrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listdataex/UpdateListdataex?token=" + Token, JSONHelper.ToJSONString(listdataexrequest));
            return JSONHelper.ParseJSONString<BasicResponse<ListdataexInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeleteListdataex(Sys.Safety.Request.Listdataex.ListdataexDeleteRequest listdataexrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listdataex/DeleteListdataex?token=" + Token, JSONHelper.ToJSONString(listdataexrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.ListdataexInfo>> GetListdataexList(Sys.Safety.Request.Listdataex.ListdataexGetListRequest listdataexrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listdataex/GetListdataexList?token=" + Token, JSONHelper.ToJSONString(listdataexrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<DataContract.ListdataexInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.ListdataexInfo> GetListdataexById(Sys.Safety.Request.Listdataex.ListdataexGetRequest listdataexrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listdataex/GetListdataexById?token=" + Token, JSONHelper.ToJSONString(listdataexrequest));
            return JSONHelper.ParseJSONString<BasicResponse<ListdataexInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<IList<DataContract.ListdataexInfo>> GetListDataExEntity(Sys.Safety.Request.Listdataex.ListdataexGetBySqlRequest strHql)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listdataex/GetListDataExEntity?token=" + Token, JSONHelper.ToJSONString(strHql));
            return JSONHelper.ParseJSONString<BasicResponse<IList<DataContract.ListdataexInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse SaveListDataExInfo(Sys.Safety.Request.Listdataex.SaveListDataExInfoRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listdataex/SaveListDataExInfo?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
    }
}
