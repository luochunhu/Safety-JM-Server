using Basic.Framework.Service;
using Basic.Framework.Web.WebApi;
using Sys.Safety.Request.Def;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sys.Safety.WebApi
{
    public class KJ_AddresstypeController : BasicApiController, IKJ_AddresstypeService
    {
        IKJ_AddresstypeService kJ_AddresstypeService = ServiceFactory.Create<IKJ_AddresstypeService>();
       
        [HttpPost]
        [Route("v1/KJ_Addresstype/AddKJ_Addresstype")]
        public Basic.Framework.Web.BasicResponse<DataContract.KJ_AddresstypeInfo> AddKJ_Addresstype(Request.KJ_Addresstype.KJ_AddresstypeAddRequest kJ_AddresstypeRequest)
        {
           return kJ_AddresstypeService.AddKJ_Addresstype(kJ_AddresstypeRequest);
        }
        [HttpPost]
        [Route("v1/KJ_Addresstype/UpdateKJ_Addresstype")]
        public Basic.Framework.Web.BasicResponse<DataContract.KJ_AddresstypeInfo> UpdateKJ_Addresstype(Request.KJ_Addresstype.KJ_AddresstypeUpdateRequest kJ_AddresstypeRequest)
        {
            return kJ_AddresstypeService.UpdateKJ_Addresstype(kJ_AddresstypeRequest);
        }
        [HttpPost]
        [Route("v1/KJ_Addresstype/DeleteKJ_Addresstype")]
        public Basic.Framework.Web.BasicResponse DeleteKJ_Addresstype(Request.KJ_Addresstype.KJ_AddresstypeDeleteRequest kJ_AddresstypeRequest)
        {
            return kJ_AddresstypeService.DeleteKJ_Addresstype(kJ_AddresstypeRequest);
        }
        [HttpPost]
        [Route("v1/KJ_Addresstype/GetKJ_AddresstypeList")]
        public Basic.Framework.Web.BasicResponse<List<DataContract.KJ_AddresstypeInfo>> GetKJ_AddresstypeList(Request.KJ_Addresstype.KJ_AddresstypeGetListRequest kJ_AddresstypeRequest)
        {
            return kJ_AddresstypeService.GetKJ_AddresstypeList(kJ_AddresstypeRequest);
        }
        [HttpPost]
        [Route("v1/KJ_Addresstype/GetKJ_AddresstypeById")]
        public Basic.Framework.Web.BasicResponse<DataContract.KJ_AddresstypeInfo> GetKJ_AddresstypeById(Request.KJ_Addresstype.KJ_AddresstypeGetRequest kJ_AddresstypeRequest)
        {
            return kJ_AddresstypeService.GetKJ_AddresstypeById(kJ_AddresstypeRequest);
        }
    }
}
