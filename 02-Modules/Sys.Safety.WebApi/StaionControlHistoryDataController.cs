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
using Sys.Safety.Request.StaionHistoryData;
using Sys.Safety.Request.StaionControlHistoryData;

namespace Sys.Safety.WebApi
{
    public class StaionControlHistoryDataController : Basic.Framework.Web.WebApi.BasicApiController, IStaionControlHistoryDataService
    {
        IStaionControlHistoryDataService _StaionControlHistoryDataService = ServiceFactory.Create<IStaionControlHistoryDataService>();
        static StaionControlHistoryDataController()
        {

        }
        [HttpPost]
        [Route("v1/StaionControlHistoryData/AddStaionControlHistoryData")]
        public BasicResponse<StaionControlHistoryDataInfo> AddStaionControlHistoryData(Sys.Safety.Request.StaionControlHistoryData.StaionControlHistoryDataAddRequest staionControlHistoryDatarequest)
        {
            return _StaionControlHistoryDataService.AddStaionControlHistoryData(staionControlHistoryDatarequest);
        }
        [HttpPost]
        [Route("v1/StaionControlHistoryData/UpdateStaionControlHistoryData")]
        public BasicResponse<StaionControlHistoryDataInfo> UpdateStaionControlHistoryData(Sys.Safety.Request.StaionControlHistoryData.StaionControlHistoryDataUpdateRequest staionControlHistoryDatarequest)
        {
            return _StaionControlHistoryDataService.UpdateStaionControlHistoryData(staionControlHistoryDatarequest);
        }
        [HttpPost]
        [Route("v1/StaionControlHistoryData/DeleteStaionControlHistoryData")]
        public BasicResponse DeleteStaionControlHistoryData(Sys.Safety.Request.StaionControlHistoryData.StaionControlHistoryDataDeleteRequest staionControlHistoryDatarequest)
        {
            return _StaionControlHistoryDataService.DeleteStaionControlHistoryData(staionControlHistoryDatarequest);
        }
        [HttpPost]
        [Route("v1/StaionControlHistoryData/GetStaionControlHistoryDataList")]
        public BasicResponse<List<StaionControlHistoryDataInfo>> GetStaionControlHistoryDataList(Sys.Safety.Request.StaionControlHistoryData.StaionControlHistoryDataGetListRequest staionControlHistoryDatarequest)
        {
            return _StaionControlHistoryDataService.GetStaionControlHistoryDataList(staionControlHistoryDatarequest);
        }
        [HttpPost]
        [Route("v1/StaionControlHistoryData/GetStaionControlHistoryDataById")]
        public BasicResponse<StaionControlHistoryDataInfo> GetStaionControlHistoryDataById(Sys.Safety.Request.StaionControlHistoryData.StaionControlHistoryDataGetRequest staionControlHistoryDatarequest)
        {
            return _StaionControlHistoryDataService.GetStaionControlHistoryDataById(staionControlHistoryDatarequest);
        }
        [HttpPost]
        [Route("v1/StaionControlHistoryData/GetStaionControlHistoryDataByFzh")]
        public BasicResponse<List<StaionControlHistoryDataInfo>> GetStaionControlHistoryDataByFzh(Sys.Safety.Request.StaionControlHistoryData.StaionControlHistoryDataGetByFzhRequest staionControlHistoryDatarequest)
        {
            return _StaionControlHistoryDataService.GetStaionControlHistoryDataByFzh(staionControlHistoryDatarequest);
        }

        [HttpPost]
        [Route("v1/StaionControlHistoryData/InsertStationControlHistoryDataToDB")]
        public BasicResponse InsertStationControlHistoryDataToDB(Sys.Safety.Request.StaionControlHistoryData.StationControlHistoryDataToDBRequest staionControlHistoryDatarequest)
        {
            return _StaionControlHistoryDataService.InsertStationControlHistoryDataToDB(staionControlHistoryDatarequest);
        }

        [HttpPost]
        [Route("v1/StaionControlHistoryData/GetStaionControlHistoryDataByByFzhTime")]
        public BasicResponse<List<Sys.Safety.Request.StaionControlHistoryData.GetStaionControlHistoryDataByByFzhTimeResponse>> GetStaionControlHistoryDataByByFzhTime(Sys.Safety.Request.StaionControlHistoryData.GetStaionControlHistoryDataByByFzhTimeRequest request)
        {
            return _StaionControlHistoryDataService.GetStaionControlHistoryDataByByFzhTime(request);
        }
        [HttpPost]
        [Route("v1/StaionControlHistoryData/DeleteStaionControlHistoryDataByPointAndTime")]
        public BasicResponse DeleteStaionControlHistoryDataByPointAndTime(DeleteByPointAndTimeStaionControlHistoryDataRequest request)
        {
            return _StaionControlHistoryDataService.DeleteStaionControlHistoryDataByPointAndTime(request);
        }
    }
}
