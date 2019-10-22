using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Basic.Framework.Service;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.WebApi
{
    public class ListcommandexController : Basic.Framework.Web.WebApi.BasicApiController, IListcommandexService
    {
        readonly IListcommandexService _listcommandexService = ServiceFactory.Create<IListcommandexService>();

        [HttpPost]
        [Route("v1/Listcommandex/AddListcommandex")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ListcommandexInfo> AddListcommandex(Sys.Safety.Request.Listcommandex.ListcommandexAddRequest listcommandexrequest)
        {
            return _listcommandexService.AddListcommandex(listcommandexrequest);
        }

        [HttpPost]
        [Route("v1/Listcommandex/UpdateListcommandex")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ListcommandexInfo> UpdateListcommandex(Sys.Safety.Request.Listcommandex.ListcommandexUpdateRequest listcommandexrequest)
        {
            return _listcommandexService.UpdateListcommandex(listcommandexrequest);
        }

        [HttpPost]
        [Route("v1/Listcommandex/DeleteListcommandex")]
        public Basic.Framework.Web.BasicResponse DeleteListcommandex(Sys.Safety.Request.Listcommandex.ListcommandexDeleteRequest listcommandexrequest)
        {
            return _listcommandexService.DeleteListcommandex(listcommandexrequest);
        }

        [HttpPost]
        [Route("v1/Listcommandex/GetListcommandexList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.ListcommandexInfo>> GetListcommandexList(Sys.Safety.Request.Listcommandex.ListcommandexGetListRequest listcommandexrequest)
        {
            return _listcommandexService.GetListcommandexList(listcommandexrequest);
        }

        [HttpPost]
        [Route("v1/Listcommandex/GetListcommandexById")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ListcommandexInfo> GetListcommandexById(Sys.Safety.Request.Listcommandex.ListcommandexGetRequest listcommandexrequest)
        {
            return _listcommandexService.GetListcommandexById(listcommandexrequest);
        }

        [HttpPost]
        [Route("v1/Listcommandex/SaveListCommandInfo")]
        public Basic.Framework.Web.BasicResponse SaveListCommandInfo(Sys.Safety.Request.Listcommandex.SaveListCommandInfoRequest request)
        {
            return _listcommandexService.SaveListCommandInfo(request);
        }
    }
}
