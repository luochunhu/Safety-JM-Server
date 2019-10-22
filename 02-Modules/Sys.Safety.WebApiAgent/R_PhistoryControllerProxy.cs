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
    public class R_PhistoryControllerProxy : BaseProxy, IR_PhistoryService
    {

        public BasicResponse<R_PhistoryInfo> AddPhistory(Sys.Safety.Request.R_Phistory.R_PhistoryAddRequest phistoryRequest)
        {
            throw new NotImplementedException();
        }

        public BasicResponse<R_PhistoryInfo> UpdatePhistory(Sys.Safety.Request.R_Phistory.R_PhistoryUpdateRequest phistoryRequest)
        {
            throw new NotImplementedException();
        }

        public BasicResponse DeletePhistory(Sys.Safety.Request.R_Phistory.R_PhistoryDeleteRequest phistoryRequest)
        {
            throw new NotImplementedException();
        }

        public BasicResponse<List<R_PhistoryInfo>> GetPhistoryList(Sys.Safety.Request.R_Phistory.R_PhistoryGetListRequest phistoryRequest)
        {
            throw new NotImplementedException();
        }

        public BasicResponse<R_PhistoryInfo> GetPhistoryById(Sys.Safety.Request.R_Phistory.R_PhistoryGetRequest phistoryRequest)
        {
            throw new NotImplementedException();
        }

        public BasicResponse<R_PhistoryInfo> GetPhistoryByPar(Sys.Safety.Request.R_Phistory.R_PhistoryGetByParRequest phistoryRequest)
        {
            throw new NotImplementedException();
        }

        public BasicResponse<R_PhistoryInfo> GetPersonLastR_Phistory(Sys.Safety.Request.R_Phistory.R_PhistoryGetLastByYidRequest request)
        {
            throw new NotImplementedException();
        }

        public BasicResponse<List<R_PhistoryInfo>> GetPersonR_PhistoryByTimer(Sys.Safety.Request.R_Phistory.R_PhistoryGetLastByTimerRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/R_Phistory/GetPersonR_PhistoryByTimer?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<List<R_PhistoryInfo>>>(responseStr);
        }
    }
}
