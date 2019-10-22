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
    public class R_UndefinedDefController : BasicApiController, IR_UndefinedDefService
    {
        IR_UndefinedDefService _presonInfoService = ServiceFactory.Create<IR_UndefinedDefService>();

        
        

        [HttpPost]
        [Route("v1/R_UndefinedDef/GetAllRUndefinedDefCache")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.R_UndefinedDefInfo>> GetAllRUndefinedDefCache(RUndefinedDefCacheGetAllRequest UndefinedDefRequest)
        {
            return _presonInfoService.GetAllRUndefinedDefCache(UndefinedDefRequest);
        }

        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.R_UndefinedDefInfo> AddUndefinedDef(Sys.Safety.Request.UndefinedDef.R_UndefinedDefAddRequest undefinedDefRequest)
        {
            throw new NotImplementedException();
        }

        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.R_UndefinedDefInfo> UpdateUndefinedDef(Sys.Safety.Request.UndefinedDef.R_UndefinedDefUpdateRequest undefinedDefRequest)
        {
            throw new NotImplementedException();
        }

        public Basic.Framework.Web.BasicResponse DeleteUndefinedDef(Sys.Safety.Request.UndefinedDef.R_UndefinedDefDeleteRequest undefinedDefRequest)
        {
            throw new NotImplementedException();
        }

        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.R_UndefinedDefInfo>> GetUndefinedDefList(Sys.Safety.Request.UndefinedDef.R_UndefinedDefGetListRequest undefinedDefRequest)
        {
            throw new NotImplementedException();
        }

        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.R_UndefinedDefInfo> GetUndefinedDefById(Sys.Safety.Request.UndefinedDef.R_UndefinedDefGetRequest undefinedDefRequest)
        {
            throw new NotImplementedException();
        }

        
    }
}
