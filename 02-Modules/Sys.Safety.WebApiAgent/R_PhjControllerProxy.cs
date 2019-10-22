using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.Request.PersonCache;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent
{
    public class R_PhjControllerProxy : BaseProxy, IR_PhjService
    {
      
        public BasicResponse<R_PhjInfo> AddPhj(Sys.Safety.Request.Phj.PhjAddRequest phjRequest)
        {
            throw new NotImplementedException();
        }

        public BasicResponse<R_PhjInfo> UpdatePhj(Sys.Safety.Request.Phj.PhjUpdateRequest phjRequest)
        {
            throw new NotImplementedException();
        }

        public BasicResponse DeletePhj(Sys.Safety.Request.Phj.PhjDeleteRequest phjRequest)
        {
            throw new NotImplementedException();
        }

        public BasicResponse<List<R_PhjInfo>> GetPhjList(Sys.Safety.Request.Phj.PhjGetListRequest phjRequest)
        {
            throw new NotImplementedException();
        }

        public BasicResponse<R_PhjInfo> GetPhjById(Sys.Safety.Request.Phj.PhjGetRequest phjRequest)
        {
            throw new NotImplementedException();
        }

        public BasicResponse AddPhjToDB(Sys.Safety.Request.Phj.PhjAddRequest phjRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Phj/AddPhjToDB?token=" + Token, JSONHelper.ToJSONString(phjRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }


        public BasicResponse<List<string>> GetPhjAlarmedList()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Phj/GetPhjAlarmedList?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<string>>>(responseStr);
        }
    }
}
