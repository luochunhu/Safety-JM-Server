using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent
{
    public class RolewebmenuControllerProxy : BaseProxy, IRolewebmenuService
    {
        public Basic.Framework.Web.BasicResponse<DataContract.RolewebmenuInfo> AddRolewebmenu(Sys.Safety.Request.Rolewebmenu.RolewebmenuAddRequest rolewebmenurequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Rolewebmenu/AddRolewebmenu?token=" + Token, JSONHelper.ToJSONString(rolewebmenurequest));
            return JSONHelper.ParseJSONString<BasicResponse<RolewebmenuInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.RolewebmenuInfo> UpdateRolewebmenu(Sys.Safety.Request.Rolewebmenu.RolewebmenuUpdateRequest rolewebmenurequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Rolewebmenu/UpdateRolewebmenu?token=" + Token, JSONHelper.ToJSONString(rolewebmenurequest));
            return JSONHelper.ParseJSONString<BasicResponse<RolewebmenuInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeleteRolewebmenu(Sys.Safety.Request.Rolewebmenu.RolewebmenuDeleteRequest rolewebmenurequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Rolewebmenu/DeleteRolewebmenu?token=" + Token, JSONHelper.ToJSONString(rolewebmenurequest));
            return JSONHelper.ParseJSONString<BasicResponse<RolewebmenuInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.RolewebmenuInfo>> GetRolewebmenuList(Sys.Safety.Request.Rolewebmenu.RolewebmenuGetListRequest rolewebmenurequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Rolewebmenu/GetRolewebmenuList?token=" + Token, JSONHelper.ToJSONString(rolewebmenurequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<RolewebmenuInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.RolewebmenuInfo> GetRolewebmenuById(Sys.Safety.Request.Rolewebmenu.RolewebmenuGetRequest rolewebmenurequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Rolewebmenu/GetRolewebmenuById?token=" + Token, JSONHelper.ToJSONString(rolewebmenurequest));
            return JSONHelper.ParseJSONString<BasicResponse<RolewebmenuInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<bool> UpdateWebRoleByRoleMenuInfo(Sys.Safety.Request.Rolewebmenu.RoleWebMenuUpdateByRoleRequest rolewebmenurequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Rolewebmenu/UpdateWebRoleByRoleMenuInfo?token=" + Token, JSONHelper.ToJSONString(rolewebmenurequest));
            return JSONHelper.ParseJSONString<BasicResponse<bool>>(responseStr);
        }


        public BasicResponse<List<RolewebmenuInfo>> GetRolewebmenuInfoByRole(Sys.Safety.Request.Rolewebmenu.RolewebmenuGetByRoleRequest rolewebmenurequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Rolewebmenu/GetRolewebmenuInfoByRole?token=" + Token, JSONHelper.ToJSONString(rolewebmenurequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<RolewebmenuInfo>>>(responseStr);
        }
    }
}
