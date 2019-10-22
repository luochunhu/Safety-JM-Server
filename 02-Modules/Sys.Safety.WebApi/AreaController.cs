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
    public class AreaController : Basic.Framework.Web.WebApi.BasicApiController, IAreaService
    {
        IAreaService areaService = ServiceFactory.Create<IAreaService>();

        
        [HttpPost]
        [Route("v1/Area/AddArea")]
        public BasicResponse<AreaInfo> AddArea(Sys.Safety.Request.Area.AreaAddRequest arearequest)
        {
            return areaService.AddArea(arearequest);
        }
        [HttpPost]
        [Route("v1/Area/UpdateArea")]
        public BasicResponse<AreaInfo> UpdateArea(Sys.Safety.Request.Area.AreaUpdateRequest arearequest)
        {
            return areaService.UpdateArea(arearequest);
        }
        [HttpPost]
        [Route("v1/Area/DeleteArea")]
        public BasicResponse DeleteArea(Sys.Safety.Request.Area.AreaDeleteRequest arearequest)
        {
            return areaService.DeleteArea(arearequest);
        }
        [HttpPost]
        [Route("v1/Area/GetAreaList")]
        public BasicResponse<List<AreaInfo>> GetAreaList(Sys.Safety.Request.Area.AreaGetListRequest arearequest)
        {
            return areaService.GetAllAreaList(arearequest);
        }
        [HttpPost]
        [Route("v1/Area/GetAreaById")]
        public BasicResponse<AreaInfo> GetAreaById(Sys.Safety.Request.Area.AreaGetRequest arearequest)
        {
            return areaService.GetAreaById(arearequest);
        }
        [HttpPost]
        [Route("v1/Area/GetAllAreaList")]
        public BasicResponse<List<AreaInfo>> GetAllAreaList(Sys.Safety.Request.Area.AreaGetListRequest arearequest)
        {
            return areaService.GetAllAreaList(arearequest);
        }

        [HttpPost]
        [Route("v1/Area/GetAllAreaCache")]
        public BasicResponse<List<AreaInfo>> GetAllAreaCache(Sys.Safety.Request.PersonCache.AreaCacheGetAllRequest arearequest)
        {
            return areaService.GetAllAreaCache(arearequest);
        }
    }
}
