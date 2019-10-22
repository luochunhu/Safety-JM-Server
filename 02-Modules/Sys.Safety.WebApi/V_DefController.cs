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
    public class V_DefController : BasicApiController, IV_DefService
    {
        IV_DefService _vdeftService = ServiceFactory.Create<IV_DefService>();

        [HttpPost]
        [Route("v1/V_Def/AddDef")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.V_DefInfo> AddDef(Sys.Safety.Request.Def.DefAddRequest defRequest)
        {
            return _vdeftService.AddDef(defRequest);
        }

        [HttpPost]
        [Route("v1/V_Def/UpdateDef")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.V_DefInfo> UpdateDef(Sys.Safety.Request.Def.DefUpdateRequest defRequest)
        {
            return _vdeftService.UpdateDef(defRequest);
        }

        [HttpPost]
        [Route("v1/V_Def/DeleteDef")]
        public Basic.Framework.Web.BasicResponse DeleteDef(Sys.Safety.Request.Def.DefDeleteRequest defRequest)
        {
            return _vdeftService.DeleteDef(defRequest);
        }

        [HttpPost]
        [Route("v1/V_Def/GetDefList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.V_DefInfo>> GetDefList(Sys.Safety.Request.Def.DefGetListRequest defRequest)
        {
            return _vdeftService.GetDefList(defRequest);
        }

        [HttpPost]
        [Route("v1/V_Def/GetDefById")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.V_DefInfo> GetDefById(Sys.Safety.Request.Def.DefGetRequest defRequest)
        {
            return _vdeftService.GetDefById(defRequest);
        }

        [HttpPost]
        [Route("v1/V_Def/GetAllDef")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.V_DefInfo>> GetAllDef(DefGetAllRequest defRequest)
        {
            return _vdeftService.GetAllDef(defRequest);
        }

        [HttpPost]
        [Route("v1/V_Def/GetAllVideoDefCache")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.V_DefInfo>> GetAllVideoDefCache()
        {
            return _vdeftService.GetAllVideoDefCache();
        }

        [HttpPost]
        [Route("v1/V_Def/GetDefByIP")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.V_DefInfo> GetDefByIP(DefIPRequest defRequest)
        {
            return _vdeftService.GetDefByIP(defRequest);
        }
    }
}
