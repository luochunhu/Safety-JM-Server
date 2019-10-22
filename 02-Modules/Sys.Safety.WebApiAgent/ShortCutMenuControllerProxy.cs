using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using System.Collections.Generic;

namespace Sys.Safety.WebApiAgent
{
    public class ShortCutMenuControllerProxy : BaseProxy,IShortCutMenuService
    {
        public Basic.Framework.Web.BasicResponse<DataContract.ShortCutMenuInfo> AddShortCutMenu(Sys.Safety.Request.ShortCutMenu.ShortCutMenuAddRequest shortCutMenuRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ShortCutMenu/AddShortCutMenu?token=" + Token, JSONHelper.ToJSONString(shortCutMenuRequest));
            return JSONHelper.ParseJSONString<BasicResponse<ShortCutMenuInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.ShortCutMenuInfo> UpdateShortCutMenu(Sys.Safety.Request.ShortCutMenu.ShortCutMenuUpdateRequest shortCutMenuRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ShortCutMenu/AddShortCutMenu?token=" + Token, JSONHelper.ToJSONString(shortCutMenuRequest));
            return JSONHelper.ParseJSONString<BasicResponse<ShortCutMenuInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeleteShortCutMenu(Sys.Safety.Request.ShortCutMenu.ShortCutMenuDeleteRequest shortCutMenuRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ShortCutMenu/AddShortCutMenu?token=" + Token, JSONHelper.ToJSONString(shortCutMenuRequest));
            return JSONHelper.ParseJSONString<BasicResponse<ShortCutMenuInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.ShortCutMenuInfo>> GetShortCutMenuList(Sys.Safety.Request.ShortCutMenu.ShortCutMenuGetListRequest shortCutMenuRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ShortCutMenu/GetShortCutMenuList?token=" + Token, JSONHelper.ToJSONString(shortCutMenuRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<ShortCutMenuInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.ShortCutMenuInfo> GetShortCutMenuById(Sys.Safety.Request.ShortCutMenu.ShortCutMenuGetRequest shortCutMenuRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ShortCutMenu/GetShortCutMenuById?token=" + Token, JSONHelper.ToJSONString(shortCutMenuRequest));
            return JSONHelper.ParseJSONString<BasicResponse<ShortCutMenuInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<bool> DeleteShortCutMenuByUserId(Sys.Safety.Request.ShortCutMenu.ShortCutMenuUserRequest shortCutMenuRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ShortCutMenu/DeleteShortCutMenuByUserId?token=" + Token, JSONHelper.ToJSONString(shortCutMenuRequest));
            return JSONHelper.ParseJSONString<BasicResponse<bool>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<bool> BatchInsetShortCutMenu(Sys.Safety.Request.ShortCutMenu.ShortCutMenuBatchInsertRequest shortCutMenuRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ShortCutMenu/BatchInsetShortCutMenu?token=" + Token, JSONHelper.ToJSONString(shortCutMenuRequest));
            return JSONHelper.ParseJSONString<BasicResponse<bool>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.ShortCutMenuInfo>> GetShortCutMenuByUserId(Sys.Safety.Request.ShortCutMenu.ShortCutMenuUserRequest shortCutMenuRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ShortCutMenu/GetShortCutMenuByUserId?token=" + Token, JSONHelper.ToJSONString(shortCutMenuRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<ShortCutMenuInfo>>>(responseStr);
        }
    }
}
