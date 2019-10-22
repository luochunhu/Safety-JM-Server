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
using Sys.Safety.DataContract.CommunicateExtend;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.Config;
using Sys.Safety.Request.PointDefine;
using Sys.Safety.Request.StationUpdate;

namespace Sys.Safety.WebApiAgent
{
    public class StationUpdateControllerProxy : BaseProxy, IStationUpdateService
    {
        public BasicResponse LoadUpdateBuffer(LoadUpdateBufferRequest loadUpdateBufferRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/StationUpdate/LoadUpdateBuffer?token=" + Token, JSONHelper.ToJSONString(loadUpdateBufferRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }


        public BasicResponse UpdateStationItemForUser(UpdateOrderForUserRequest updateOrderRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/StationUpdate/UpdateStationItemForUser?token=" + Token, JSONHelper.ToJSONString(updateOrderRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse UpdateStationItemForSys(UpdateOrderForSysRequest updateOrderRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/StationUpdate/UpdateStationItemForSys?token=" + Token, JSONHelper.ToJSONString(updateOrderRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse<StationUpdateItem> GetStationItem(GetStationItemRequest getStationItemRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/StationUpdate/GetStationItem?token=" + Token, JSONHelper.ToJSONString(getStationItemRequest));
            return JSONHelper.ParseJSONString<BasicResponse<StationUpdateItem>>(responseStr);
        }

        public BasicResponse<List<StationUpdateItem>> GetAllStationItems(GetAllStationItemsRequest getAllStationItemsRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/StationUpdate/GetAllStationItems?token=" + Token, JSONHelper.ToJSONString(getAllStationItemsRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<StationUpdateItem>>>(responseStr);
        }
    }
}
