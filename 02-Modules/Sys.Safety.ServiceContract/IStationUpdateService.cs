using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.StationUpdate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.ServiceContract
{
    /// <summary>
    /// 分站远程升级接口
    /// </summary>
    public interface IStationUpdateService
    {
        BasicResponse LoadUpdateBuffer(LoadUpdateBufferRequest loadUpdateBufferRequest);

        BasicResponse UpdateStationItemForUser(UpdateOrderForUserRequest updateOrderRequest);


        BasicResponse UpdateStationItemForSys(UpdateOrderForSysRequest updateOrderRequest);

        BasicResponse<StationUpdateItem> GetStationItem(GetStationItemRequest getStationItemRequest);

        BasicResponse<List<StationUpdateItem>> GetAllStationItems(GetAllStationItemsRequest getAllStationItemsRequest);
    }
}
