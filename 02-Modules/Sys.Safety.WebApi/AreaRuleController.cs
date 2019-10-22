using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.AlarmHandle;
using Basic.Framework.Service;
using System.Web.Http;

namespace Sys.Safety.WebApi
{
    public class AreaRuleController : Basic.Framework.Web.WebApi.BasicApiController, IAreaRuleService
    {
        IAreaRuleService alarmHandleService = ServiceFactory.Create<IAreaRuleService>();

        [HttpPost]
        [Route("v1/AreaRule/AddAreaRule")]
        public BasicResponse<AreaRuleInfo> AddAreaRule(Sys.Safety.Request.Area.AreaRuleAddRequest areaRuleRequest)
        {
            return alarmHandleService.AddAreaRule(areaRuleRequest);
        }
        [HttpPost]
        [Route("v1/AreaRule/UpdateAreaRule")]
        public BasicResponse<AreaRuleInfo> UpdateAreaRule(Sys.Safety.Request.Area.AreaRuleUpdateRequest areaRuleRequest)
        {
            return alarmHandleService.UpdateAreaRule(areaRuleRequest);
        }
        [HttpPost]
        [Route("v1/AreaRule/DeleteAreaRule")]
        public BasicResponse DeleteAreaRule(Sys.Safety.Request.Area.AreaRuleDeleteRequest areaRuleRequest)
        {
            return alarmHandleService.DeleteAreaRule(areaRuleRequest);
        }
        [HttpPost]
        [Route("v1/AreaRule/GetAreaRuleList")]
        public BasicResponse<List<AreaRuleInfo>> GetAreaRuleList(Sys.Safety.Request.Area.AreaRuleGetListRequest areaRuleRequest)
        {
            return alarmHandleService.GetAreaRuleList(areaRuleRequest);
        }
        [HttpPost]
        [Route("v1/AreaRule/GetAreaRuleById")]
        public BasicResponse<AreaRuleInfo> GetAreaRuleById(Sys.Safety.Request.Area.AreaRuleGetRequest areaRuleRequest)
        {
            return alarmHandleService.GetAreaRuleById(areaRuleRequest);
        }
        [HttpPost]
        [Route("v1/AreaRule/GetAreaRuleListByAreaID")]
        public BasicResponse<List<AreaRuleInfo>> GetAreaRuleListByAreaID(Sys.Safety.Request.Area.GetAreaRuleListByAreaIDRequest areaRuleRequest)
        {
            return alarmHandleService.GetAreaRuleListByAreaID(areaRuleRequest);
        }

        [HttpPost]
        [Route("v1/AreaRule/DeleteAreaRuleByAreaID")]
        public BasicResponse DeleteAreaRuleByAreaID(Sys.Safety.Request.Area.AreaRuleDeleteRequest areaRuleRequest)
        {
            return alarmHandleService.DeleteAreaRuleByAreaID(areaRuleRequest);
        }
    }
}
