using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Basic.Framework.Service;
using Basic.Framework.Web.WebApi;
using Sys.Safety.ServiceContract;
using Sys.Safety.ServiceContract.Cache;

namespace Sys.Safety.WebApi
{
    public class GasContentController : BasicApiController, IGasContentService
    {
        private IGasContentAlarmCacheService _gasContentAlarmCacheService = ServiceFactory.Create<IGasContentAlarmCacheService>();

        [HttpPost]
        [Route("v1/GasContent/GetAllGasContentAlarmCache")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.Custom.GasContentAlarmInfo>> GetAllGasContentAlarmCache()
        {
            return _gasContentAlarmCacheService.GetAllCache();
        }
    }
}
