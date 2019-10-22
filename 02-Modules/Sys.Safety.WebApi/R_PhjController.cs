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
    public class R_PhjController : BasicApiController, IR_PhjService
    {
        IR_PhjService _R_PhjService = ServiceFactory.Create<IR_PhjService>();
        
             public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.R_PhjInfo> AddPhj(Sys.Safety.Request.Phj.PhjAddRequest phjRequest)
        {
            throw new NotImplementedException();
        }

        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.R_PhjInfo> UpdatePhj(Sys.Safety.Request.Phj.PhjUpdateRequest phjRequest)
        {
            throw new NotImplementedException();
        }

        public Basic.Framework.Web.BasicResponse DeletePhj(Sys.Safety.Request.Phj.PhjDeleteRequest phjRequest)
        {
            throw new NotImplementedException();
        }

        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.R_PhjInfo>> GetPhjList(Sys.Safety.Request.Phj.PhjGetListRequest phjRequest)
        {
            throw new NotImplementedException();
        }

        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.R_PhjInfo> GetPhjById(Sys.Safety.Request.Phj.PhjGetRequest phjRequest)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        [Route("v1/R_Phj/AddPhjToDB")]
        public Basic.Framework.Web.BasicResponse AddPhjToDB(Sys.Safety.Request.Phj.PhjAddRequest phjRequest)
        {
            return _R_PhjService.AddPhjToDB(phjRequest);
        }

        [HttpPost]
        [Route("v1/R_Phj/GetPhjAlarmedList")]
        public Basic.Framework.Web.BasicResponse<List<string>> GetPhjAlarmedList()
        {
            return _R_PhjService.GetPhjAlarmedList();
        }
    }
}
