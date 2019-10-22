using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract.Custom;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.WebApiAgent
{
    public class GasContentControllerProxy : BaseProxy,IGasContentService
    {
        public Basic.Framework.Web.BasicResponse<List<DataContract.Custom.GasContentAlarmInfo>> GetAllGasContentAlarmCache()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/GasContent/GetAllGasContentAlarmCache?token=" + Token, "");
            return JSONHelper.ParseJSONString<BasicResponse<List<GasContentAlarmInfo>>>(responseStr);
        }
    }
}
