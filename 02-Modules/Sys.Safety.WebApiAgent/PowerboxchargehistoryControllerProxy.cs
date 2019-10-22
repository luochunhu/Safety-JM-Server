using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Position;
using Basic.Framework.Common;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.Request.Cache;

namespace Sys.Safety.WebApiAgent
{
    public class PowerboxchargehistoryControllerProxy : BaseProxy, IPowerboxchargehistoryService
    {
        public BasicResponse<PowerboxchargehistoryInfo> AddPowerboxchargehistory(Sys.Safety.Request.Powerboxchargehistory.PowerboxchargehistoryAddRequest powerboxchargehistoryRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Powerboxchargehistory/Add?token=" + Token, JSONHelper.ToJSONString(powerboxchargehistoryRequest));
            return JSONHelper.ParseJSONString<BasicResponse<PowerboxchargehistoryInfo>>(responseStr);
        }

        public BasicResponse<PowerboxchargehistoryInfo> UpdatePowerboxchargehistory(Sys.Safety.Request.Powerboxchargehistory.PowerboxchargehistoryUpdateRequest powerboxchargehistoryRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Powerboxchargehistory/Update?token=" + Token, JSONHelper.ToJSONString(powerboxchargehistoryRequest));
            return JSONHelper.ParseJSONString<BasicResponse<PowerboxchargehistoryInfo>>(responseStr);
        }

        public BasicResponse DeletePowerboxchargehistory(Sys.Safety.Request.Powerboxchargehistory.PowerboxchargehistoryDeleteRequest powerboxchargehistoryRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Powerboxchargehistory/Delete?token=" + Token, JSONHelper.ToJSONString(powerboxchargehistoryRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse<List<PowerboxchargehistoryInfo>> GetPowerboxchargehistoryList(Sys.Safety.Request.Powerboxchargehistory.PowerboxchargehistoryGetListRequest powerboxchargehistoryRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Powerboxchargehistory/GetList?token=" + Token, JSONHelper.ToJSONString(powerboxchargehistoryRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<PowerboxchargehistoryInfo>>>(responseStr);
        }

        public BasicResponse<PowerboxchargehistoryInfo> GetPowerboxchargehistoryById(Sys.Safety.Request.Powerboxchargehistory.PowerboxchargehistoryGetRequest powerboxchargehistoryRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Powerboxchargehistory/GetById?token=" + Token, JSONHelper.ToJSONString(powerboxchargehistoryRequest));
            return JSONHelper.ParseJSONString<BasicResponse<PowerboxchargehistoryInfo>>(responseStr);
        }



        public BasicResponse<List<PowerboxchargehistoryInfo>> GetPowerboxchargehistoryByFzhOrMac(Sys.Safety.Request.Powerboxchargehistory.PowerboxchargehistoryGetByFzhOrMacRequest powerboxchargehistoryRequest)
        {
           var responseStr = HttpClientHelper.Post(Webapi + "/v1/Powerboxchargehistory/GetPowerboxchargehistoryByFzhOrMac?token=" + Token, JSONHelper.ToJSONString(powerboxchargehistoryRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<PowerboxchargehistoryInfo>>>(responseStr);
        }


        public BasicResponse<List<PowerboxchargehistoryInfo>> GetPowerboxchargehistoryByStime(Request.Powerboxchargehistory.PowerboxchargehistoryGetByStimeRequest powerboxchargehistoryRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Powerboxchargehistory/GetPowerboxchargehistoryByStime?token=" + Token, JSONHelper.ToJSONString(powerboxchargehistoryRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<PowerboxchargehistoryInfo>>>(responseStr);
        }
    }
}
