using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.Request.Webmenu;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sys.Safety.WebApi
{
    public class WebmenuController : Basic.Framework.Web.WebApi.BasicApiController, IWebmenuService
    {
        private readonly IWebmenuService webmunuService;
        public WebmenuController()
        {
            webmunuService = ServiceFactory.Create<IWebmenuService>();
        }

        [HttpPost]
        [Route("v1/Webmenu/AddWebmenu")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.WebmenuInfo> AddWebmenu(Sys.Safety.Request.Webmenu.WebmenuAddRequest webmenurequest)
        {
            return webmunuService.AddWebmenu(webmenurequest);
        }

        [HttpPost]
        [Route("v1/Webmenu/UpdateWebmenu")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.WebmenuInfo> UpdateWebmenu(Sys.Safety.Request.Webmenu.WebmenuUpdateRequest webmenurequest)
        {
            return webmunuService.UpdateWebmenu(webmenurequest);
        }

        [HttpPost]
        [Route("v1/Webmenu/DeleteWebmenu")]
        public Basic.Framework.Web.BasicResponse DeleteWebmenu(Sys.Safety.Request.Webmenu.WebmenuDeleteRequest webmenurequest)
        {
            return webmunuService.DeleteWebmenu(webmenurequest);
        }

        [HttpPost]
        [Route("v1/Webmenu/GetWebmenuList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.WebmenuInfo>> GetWebmenuList(Sys.Safety.Request.Webmenu.WebmenuGetListRequest webmenurequest)
        {
            return webmunuService.GetWebmenuList(webmenurequest);
        }

        [HttpPost]
        [Route("v1/Webmenu/GetWebmenuById")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.WebmenuInfo> GetWebmenuById(Sys.Safety.Request.Webmenu.WebmenuGetRequest webmenurequest)
        {
            return webmunuService.GetWebmenuById(webmenurequest);
        }

        [HttpPost]
        [Route("v1/Webmenu/GetWebmenuListByUserCode")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.WebmenuInfo>> GetWebmenuListByUserCode(WebmunuGetListByUserCodeRequest webmenurequest)
        {
            return webmunuService.GetWebmenuListByUserCode(webmenurequest);
        }


        public Basic.Framework.Web.BasicResponse<bool> BatchInsertWebMenus(WebmenuBatchInsertRequest webmenurequest)
        {
            throw new NotImplementedException();
        }

        public Basic.Framework.Web.BasicResponse<bool> BatchDeleteWebMenus(WebmenuBatchDeleteRequest webmenurequest)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("v1/Webmenu/GetAllWebMenuInfos")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.WebmenuInfo>> GetAllWebMenuInfos(BasicRequest webmenurequest)
        {
            return webmunuService.GetAllWebMenuInfos(webmenurequest);
        }
    }
}
