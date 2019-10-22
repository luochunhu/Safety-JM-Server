using Basic.Framework.Service;
using Basic.Framework.Web.WebApi;
using Sys.Safety.Request.PersonCache;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Basic.Framework.Web;
using Sys.Safety.DataContract;

namespace Sys.Safety.WebApi
{
    public class R_PrealController : BasicApiController, IR_PrealService
    {
        IR_PrealService _presonInfoService = ServiceFactory.Create<IR_PrealService>();

        [HttpPost]
        [Route("v1/R_Preal/AddPreal")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.R_PrealInfo> AddPreal(Sys.Safety.Request.R_Preal.R_PrealAddRequest PrealRequest)
        {
            return _presonInfoService.AddPreal(PrealRequest);
        }

        [HttpPost]
        [Route("v1/R_Preal/UpdatePreal")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.R_PrealInfo> UpdatePreal(Sys.Safety.Request.R_Preal.R_PrealUpdateRequest PrealRequest)
        {
            return _presonInfoService.UpdatePreal(PrealRequest);
        }

        [HttpPost]
        [Route("v1/R_Preal/DeletePreal")]
        public Basic.Framework.Web.BasicResponse DeletePreal(Sys.Safety.Request.R_Preal.R_PrealDeleteRequest PrealRequest)
        {
            return _presonInfoService.DeletePreal(PrealRequest);
        }

        [HttpPost]
        [Route("v1/R_Preal/GetPrealList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.R_PrealInfo>> GetPrealList(Sys.Safety.Request.R_Preal.R_PrealGetListRequest PrealRequest)
        {
            return _presonInfoService.GetPrealList(PrealRequest);
        }

        [HttpPost]
        [Route("v1/R_Preal/GetPrealById")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.R_PrealInfo> GetPrealById(Sys.Safety.Request.R_Preal.R_PrealGetRequest PrealRequest)
        {
            return _presonInfoService.GetPrealById(PrealRequest);
        }
        

        [HttpPost]
        [Route("v1/R_Preal/GetAllPrealCacheList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.R_PrealInfo>> GetAllPrealCacheList(RPrealCacheGetAllRequest PrealRequest)
        {
            return _presonInfoService.GetAllPrealCacheList(PrealRequest);
        }       

        [HttpPost]
        [Route("v1/R_Preal/GetAllAlarmPrealCacheList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.R_PrealInfo>> GetAllAlarmPrealCacheList()
        {
            return _presonInfoService.GetAllAlarmPrealCacheList();
        }

        [HttpPost]
        [Route("v1/R_Preal/OldPlsPersonRealSync")]
        public BasicResponse OldPlsPersonRealSync(OldPlsPersonRealSyncRequest request)
        {
            return _presonInfoService.OldPlsPersonRealSync(request);
        }
    }
}
