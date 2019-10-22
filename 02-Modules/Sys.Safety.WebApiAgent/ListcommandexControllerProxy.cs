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
    public class ListcommandexControllerProxy : BaseProxy, IListcommandexService
    {
        public Basic.Framework.Web.BasicResponse<DataContract.ListcommandexInfo> AddListcommandex(Sys.Safety.Request.Listcommandex.ListcommandexAddRequest listcommandexrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listcommandex/AddListcommandex?token=" + Token, JSONHelper.ToJSONString(listcommandexrequest));
            return JSONHelper.ParseJSONString<BasicResponse<ListcommandexInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.ListcommandexInfo> UpdateListcommandex(Sys.Safety.Request.Listcommandex.ListcommandexUpdateRequest listcommandexrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listcommandex/UpdateListcommandex?token=" + Token, JSONHelper.ToJSONString(listcommandexrequest));
            return JSONHelper.ParseJSONString<BasicResponse<ListcommandexInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeleteListcommandex(Sys.Safety.Request.Listcommandex.ListcommandexDeleteRequest listcommandexrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listcommandex/DeleteListcommandex?token=" + Token, JSONHelper.ToJSONString(listcommandexrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.ListcommandexInfo>> GetListcommandexList(Sys.Safety.Request.Listcommandex.ListcommandexGetListRequest listcommandexrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listcommandex/GetListcommandexList?token=" + Token, JSONHelper.ToJSONString(listcommandexrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<DataContract.ListcommandexInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.ListcommandexInfo> GetListcommandexById(Sys.Safety.Request.Listcommandex.ListcommandexGetRequest listcommandexrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listcommandex/GetListcommandexById?token=" + Token, JSONHelper.ToJSONString(listcommandexrequest));
            return JSONHelper.ParseJSONString<BasicResponse<ListcommandexInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse SaveListCommandInfo(Sys.Safety.Request.Listcommandex.SaveListCommandInfoRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listcommandex/SaveListCommandInfo?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
    }
}
