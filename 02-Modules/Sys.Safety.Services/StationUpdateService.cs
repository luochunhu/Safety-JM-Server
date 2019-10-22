using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.Cache.Safety;
using Sys.Safety.DataContract;
using Sys.Safety.Request.StationUpdate;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Services
{
    public class StationUpdateService : IStationUpdateService
    {
        public BasicResponse LoadUpdateBuffer(LoadUpdateBufferRequest loadUpdateBufferRequest)
        {
            BasicResponse Result = new BasicResponse();
            try
            {
                StationUpdateCache.LoadUpdateBuffer(loadUpdateBufferRequest.updateBufferItems
                    , loadUpdateBufferRequest.updatetTypeid
                    , loadUpdateBufferRequest.updateHardVersion
                    , loadUpdateBufferRequest.updateFileVersion
                    , loadUpdateBufferRequest.updateMaxVersion
                    , loadUpdateBufferRequest.updateMinVersion
                    , loadUpdateBufferRequest.updateCount
                    , loadUpdateBufferRequest.crcValue);
            }
            catch(Exception ex)
            {
                Result.Code = 1;
                Result.Message = ex.Message;
            }
            return Result;
        }


        public BasicResponse UpdateStationItemForUser(UpdateOrderForUserRequest updateOrderRequest)
        {
            BasicResponse response = new BasicResponse();
            string errorMsg = "";
            StationUpdateCache.UpdateStationItemForUser(updateOrderRequest.order, updateOrderRequest.fzh, ref errorMsg);
            response.Message = errorMsg;
            return response;
        }
        public BasicResponse UpdateStationItemForSys(UpdateOrderForSysRequest updateOrderRequest)
        {
            BasicResponse response = new BasicResponse();
            string errorMsg = "";

            StationUpdateCache.UpdateStationItemForSys(updateOrderRequest.item, ref errorMsg);

            response.Message = errorMsg;
            return response;
        }

        public BasicResponse<StationUpdateItem> GetStationItem(GetStationItemRequest getStationItemRequest)
        {
            var result = StationUpdateCache.GetStationItem(getStationItemRequest.fzh);
            var stationItem = new BasicResponse<StationUpdateItem>();
            stationItem.Data = result;
            return stationItem;
        }

        public BasicResponse<List<StationUpdateItem>> GetAllStationItems(GetAllStationItemsRequest getAllStationItemsRequest)
        {
            var result = StationUpdateCache.GetAllStationItems();
            var stationItem = new BasicResponse<List<StationUpdateItem>>();
            stationItem.Data = result;
            return stationItem;
        }
    }
}
