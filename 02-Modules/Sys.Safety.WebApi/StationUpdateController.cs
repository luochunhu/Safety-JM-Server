using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Position;
using Basic.Framework.Service;
using System.Web.Http;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.Config;
using Sys.Safety.Request.PointDefine;
using Sys.Safety.Request.StationUpdate;

namespace Sys.Safety.WebApi
{
    public class StationUpdateController : Basic.Framework.Web.WebApi.BasicApiController, IStationUpdateService
    {
        IStationUpdateService _StationUpdateService = ServiceFactory.Create<IStationUpdateService>();   
        static StationUpdateController()
        {

        } 

        [HttpPost]
        [Route("v1/StationUpdate/LoadUpdateBuffer")]
        public BasicResponse LoadUpdateBuffer(LoadUpdateBufferRequest loadUpdateBufferRequest)
        {
            return _StationUpdateService.LoadUpdateBuffer(loadUpdateBufferRequest);
        }

        [HttpPost]
        [Route("v1/StationUpdate/UpdateStationItemForUser")]
        public BasicResponse UpdateStationItemForUser(UpdateOrderForUserRequest updateOrderRequest)
        {
            return _StationUpdateService.UpdateStationItemForUser(updateOrderRequest);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="getStationItemRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/StationUpdate/GetStationItem")]
        public BasicResponse<StationUpdateItem> GetStationItem(GetStationItemRequest getStationItemRequest)
        {
            return _StationUpdateService.GetStationItem(getStationItemRequest);
        }

        [HttpPost]
        [Route("v1/StationUpdate/UpdateStationItemForSys")]
        public BasicResponse UpdateStationItemForSys(UpdateOrderForSysRequest updateOrderRequest)
        {
            return _StationUpdateService.UpdateStationItemForSys(updateOrderRequest);
        }
        [HttpPost]
        [Route("v1/StationUpdate/GetAllStationItems")]
        public BasicResponse<List<StationUpdateItem>> GetAllStationItems(GetAllStationItemsRequest getAllStationItemsRequest)
        {
            return _StationUpdateService.GetAllStationItems(getAllStationItemsRequest);
        }
    }
}
