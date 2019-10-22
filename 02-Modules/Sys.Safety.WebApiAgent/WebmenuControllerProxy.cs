using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Webmenu;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent
{
    public class WebmenuControllerProxy : BaseProxy, IWebmenuService
    {
        public Basic.Framework.Web.BasicResponse<DataContract.WebmenuInfo> AddWebmenu(Sys.Safety.Request.Webmenu.WebmenuAddRequest webmenurequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Webmenu/AddWebmenu?token=" + Token, JSONHelper.ToJSONString(webmenurequest));
            return JSONHelper.ParseJSONString<BasicResponse<WebmenuInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.WebmenuInfo> UpdateWebmenu(Sys.Safety.Request.Webmenu.WebmenuUpdateRequest webmenurequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Webmenu/UpdateWebmenu?token=" + Token, JSONHelper.ToJSONString(webmenurequest));
            return JSONHelper.ParseJSONString<BasicResponse<WebmenuInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeleteWebmenu(Sys.Safety.Request.Webmenu.WebmenuDeleteRequest webmenurequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Webmenu/DeleteWebmenu?token=" + Token, JSONHelper.ToJSONString(webmenurequest));
            return JSONHelper.ParseJSONString<BasicResponse<StaionHistoryDataInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.WebmenuInfo>> GetWebmenuList(Sys.Safety.Request.Webmenu.WebmenuGetListRequest webmenurequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Webmenu/GetWebmenuList?token=" + Token, JSONHelper.ToJSONString(webmenurequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<WebmenuInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.WebmenuInfo> GetWebmenuById(Sys.Safety.Request.Webmenu.WebmenuGetRequest webmenurequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Webmenu/GetWebmenuById?token=" + Token, JSONHelper.ToJSONString(webmenurequest));
            return JSONHelper.ParseJSONString<BasicResponse<WebmenuInfo>>(responseStr);
        }

        public BasicResponse<List<WebmenuInfo>> GetWebmenuListByUserCode(WebmunuGetListByUserCodeRequest webmenurequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Webmenu/GetWebmenuListByUserCode?token=" + Token, JSONHelper.ToJSONString(webmenurequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<WebmenuInfo>>>(responseStr);
        }


        public BasicResponse<bool> BatchInsertWebMenus(WebmenuBatchInsertRequest webmenurequest)
        {
            throw new NotImplementedException();
        }

        public BasicResponse<bool> BatchDeleteWebMenus(WebmenuBatchDeleteRequest webmenurequest)
        {
            throw new NotImplementedException();
        }


        public BasicResponse<List<WebmenuInfo>> GetAllWebMenuInfos(BasicRequest webmenurequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Webmenu/GetAllWebMenuInfos?token=" + Token, JSONHelper.ToJSONString(webmenurequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<WebmenuInfo>>>(responseStr);
        }
    }
}
