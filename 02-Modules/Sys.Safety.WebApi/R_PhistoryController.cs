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

namespace Sys.Safety.WebApi
{
    public class R_PhistoryController : BasicApiController, IR_PhistoryService
    {
        IR_PhistoryService _R_PhistoryService = ServiceFactory.Create<IR_PhistoryService>();

        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.R_PhistoryInfo> AddPhistory(Sys.Safety.Request.R_Phistory.R_PhistoryAddRequest phistoryRequest)
        {
            throw new NotImplementedException();
        }

        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.R_PhistoryInfo> UpdatePhistory(Sys.Safety.Request.R_Phistory.R_PhistoryUpdateRequest phistoryRequest)
        {
            throw new NotImplementedException();
        }

        public Basic.Framework.Web.BasicResponse DeletePhistory(Sys.Safety.Request.R_Phistory.R_PhistoryDeleteRequest phistoryRequest)
        {
            throw new NotImplementedException();
        }

        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.R_PhistoryInfo>> GetPhistoryList(Sys.Safety.Request.R_Phistory.R_PhistoryGetListRequest phistoryRequest)
        {
            throw new NotImplementedException();
        }

        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.R_PhistoryInfo> GetPhistoryById(Sys.Safety.Request.R_Phistory.R_PhistoryGetRequest phistoryRequest)
        {
            throw new NotImplementedException();
        }

        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.R_PhistoryInfo> GetPhistoryByPar(Sys.Safety.Request.R_Phistory.R_PhistoryGetByParRequest phistoryRequest)
        {
            throw new NotImplementedException();
        }

        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.R_PhistoryInfo> GetPersonLastR_Phistory(Sys.Safety.Request.R_Phistory.R_PhistoryGetLastByYidRequest request)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        [Route("v1/R_Phistory/GetPersonR_PhistoryByTimer")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.R_PhistoryInfo>> GetPersonR_PhistoryByTimer(Sys.Safety.Request.R_Phistory.R_PhistoryGetLastByTimerRequest request)
        {
           return _R_PhistoryService.GetPersonR_PhistoryByTimer(request);
        }
    }
}
