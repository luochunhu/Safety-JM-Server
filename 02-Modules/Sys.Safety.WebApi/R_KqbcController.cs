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
    public class R_KqbcController : BasicApiController, IR_KqbcService
    {
        IR_KqbcService _R_KqbcService = ServiceFactory.Create<IR_KqbcService>();
               public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.R_KqbcInfo> AddKqbc(Sys.Safety.Request.Kqbc.R_KqbcAddRequest kqbcRequest)
        {
            throw new NotImplementedException();
        }

        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.R_KqbcInfo> UpdateKqbc(Sys.Safety.Request.Kqbc.R_KqbcUpdateRequest kqbcRequest)
        {
            throw new NotImplementedException();
        }

        public Basic.Framework.Web.BasicResponse DeleteKqbc(Sys.Safety.Request.Kqbc.R_KqbcDeleteRequest kqbcRequest)
        {
            throw new NotImplementedException();
        }

        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.R_KqbcInfo>> GetKqbcList(Sys.Safety.Request.Kqbc.R_KqbcGetListRequest kqbcRequest)
        {
            throw new NotImplementedException();
        }

        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.R_KqbcInfo>> GetAllKqbcList()
        {
            throw new NotImplementedException();
        }

        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.R_KqbcInfo> GetKqbcById(Sys.Safety.Request.Kqbc.R_KqbcGetRequest kqbcRequest)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        [Route("v1/R_Kqbc/GetAllKqbcCacheList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.R_KqbcInfo>> GetAllKqbcCacheList(RKqbcCacheGetAllRequest RKqbcCacheRequest)
        {
            return _R_KqbcService.GetAllKqbcCacheList(RKqbcCacheRequest);
        }
        [HttpPost]
        [Route("v1/R_Kqbc/GetDefaultKqbcCache")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.R_KqbcInfo> GetDefaultKqbcCache(RKqbcCacheGetByConditionRequest RKqbcCacheRequest)
        {
            return _R_KqbcService.GetDefaultKqbcCache(RKqbcCacheRequest);
        }
    }
}
