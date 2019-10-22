using Basic.Framework.Service;
using Basic.Framework.Web.WebApi;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sys.Safety.WebApi
{
    public class ShortCutMenuController : BasicApiController, IShortCutMenuService
    {
        IShortCutMenuService shortCutMenuService = ServiceFactory.Create<IShortCutMenuService>();

        [HttpPost]
        [Route("v1/ShortCutMenu/AddShortCutMenu")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ShortCutMenuInfo> AddShortCutMenu(Sys.Safety.Request.ShortCutMenu.ShortCutMenuAddRequest shortCutMenuRequest)
        {
            return shortCutMenuService.AddShortCutMenu(shortCutMenuRequest);
        }

        [HttpPost]
        [Route("v1/ShortCutMenu/UpdateShortCutMenu")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ShortCutMenuInfo> UpdateShortCutMenu(Sys.Safety.Request.ShortCutMenu.ShortCutMenuUpdateRequest shortCutMenuRequest)
        {
            return shortCutMenuService.UpdateShortCutMenu(shortCutMenuRequest);
        }

        [HttpPost]
        [Route("v1/ShortCutMenu/DeleteShortCutMenu")]
        public Basic.Framework.Web.BasicResponse DeleteShortCutMenu(Sys.Safety.Request.ShortCutMenu.ShortCutMenuDeleteRequest shortCutMenuRequest)
        {
            return shortCutMenuService.DeleteShortCutMenu(shortCutMenuRequest);
        }

        [HttpPost]
        [Route("v1/ShortCutMenu/GetShortCutMenuList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.ShortCutMenuInfo>> GetShortCutMenuList(Sys.Safety.Request.ShortCutMenu.ShortCutMenuGetListRequest shortCutMenuRequest)
        {
            return shortCutMenuService.GetShortCutMenuList(shortCutMenuRequest);
        }

        [HttpPost]
        [Route("v1/ShortCutMenu/GetShortCutMenuById")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ShortCutMenuInfo> GetShortCutMenuById(Sys.Safety.Request.ShortCutMenu.ShortCutMenuGetRequest shortCutMenuRequest)
        {
            return shortCutMenuService.GetShortCutMenuById(shortCutMenuRequest);
        }

        [HttpPost]
        [Route("v1/ShortCutMenu/DeleteShortCutMenuByUserId")]
        public Basic.Framework.Web.BasicResponse<bool> DeleteShortCutMenuByUserId(Sys.Safety.Request.ShortCutMenu.ShortCutMenuUserRequest shortCutMenuRequest)
        {
            return shortCutMenuService.DeleteShortCutMenuByUserId(shortCutMenuRequest);
        }

        [HttpPost]
        [Route("v1/ShortCutMenu/BatchInsetShortCutMenu")]
        public Basic.Framework.Web.BasicResponse<bool> BatchInsetShortCutMenu(Sys.Safety.Request.ShortCutMenu.ShortCutMenuBatchInsertRequest shortCutMenuRequest)
        {
            return shortCutMenuService.BatchInsetShortCutMenu(shortCutMenuRequest);
        }

        [HttpPost]
        [Route("v1/ShortCutMenu/GetShortCutMenuByUserId")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.ShortCutMenuInfo>> GetShortCutMenuByUserId(Sys.Safety.Request.ShortCutMenu.ShortCutMenuUserRequest shortCutMenuRequest)
        {
            return shortCutMenuService.GetShortCutMenuByUserId(shortCutMenuRequest);
        }
    }
}
