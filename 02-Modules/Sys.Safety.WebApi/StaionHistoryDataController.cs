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

namespace Sys.Safety.WebApi
{
    public class StaionHistoryDataController : Basic.Framework.Web.WebApi.BasicApiController, IStaionHistoryDataService
    {
        IStaionHistoryDataService _StaionHistoryDataService = ServiceFactory.Create<IStaionHistoryDataService>();
        static StaionHistoryDataController()
        {

        }
      
        [HttpPost]
        [Route("v1/StaionHistoryData/AddStaionHistoryData")]
        public BasicResponse<StaionHistoryDataInfo> AddStaionHistoryData(Sys.Safety.Request.StaionHistoryData.StaionHistoryDataAddRequest staionHistoryDatarequest)
        {
            return _StaionHistoryDataService.AddStaionHistoryData(staionHistoryDatarequest);
        }
        [HttpPost]
        [Route("v1/StaionHistoryData/UpdateStaionHistoryData")]
        public BasicResponse<StaionHistoryDataInfo> UpdateStaionHistoryData(Sys.Safety.Request.StaionHistoryData.StaionHistoryDataUpdateRequest staionHistoryDatarequest)
        {
            return _StaionHistoryDataService.UpdateStaionHistoryData(staionHistoryDatarequest);
        }
        [HttpPost]
        [Route("v1/StaionHistoryData/DeleteStaionHistoryData")]
        public BasicResponse DeleteStaionHistoryData(Sys.Safety.Request.StaionHistoryData.StaionHistoryDataDeleteRequest staionHistoryDatarequest)
        {
            return _StaionHistoryDataService.DeleteStaionHistoryData(staionHistoryDatarequest);
        }
        [HttpPost]
        [Route("v1/StaionHistoryData/GetStaionHistoryDataList")]
        public BasicResponse<List<StaionHistoryDataInfo>> GetStaionHistoryDataList(Sys.Safety.Request.StaionHistoryData.StaionHistoryDataGetListRequest staionHistoryDatarequest)
        {
            return _StaionHistoryDataService.GetStaionHistoryDataList(staionHistoryDatarequest);
        }
        [HttpPost]
        [Route("v1/StaionHistoryData/GetStaionHistoryDataById")]
        public BasicResponse<StaionHistoryDataInfo> GetStaionHistoryDataById(Sys.Safety.Request.StaionHistoryData.StaionHistoryDataGetRequest staionHistoryDatarequest)
        {
            return _StaionHistoryDataService.GetStaionHistoryDataById(staionHistoryDatarequest);
        }
        [HttpPost]
        [Route("v1/StaionHistoryData/GetStaionHistoryDataByFzh")]
        public BasicResponse<List<StaionHistoryDataInfo>> GetStaionHistoryDataByFzh(StaionHistoryDataGetByFzhRequest staionHistoryDatarequest)
        {
            return _StaionHistoryDataService.GetStaionHistoryDataByFzh(staionHistoryDatarequest);
        }

        [HttpPost]
        [Route("v1/StaionHistoryData/GetSubstationHistoryRealDataByFzhTime")]
        public BasicResponse<List<GetSubstationHistoryRealDataByFzhTimeResponse>> GetSubstationHistoryRealDataByFzhTime(GetSubstationHistoryRealDataByFzhTimeRequest request)
        {
            return _StaionHistoryDataService.GetSubstationHistoryRealDataByFzhTime(request);
        }

        [HttpPost]
        [Route("v1/StaionHistoryData/InsertStationHistoryDataToDB")]
        public BasicResponse InsertStationHistoryDataToDB(InsertStationHistoryDataRequest staionHistoryDatarequest)
        {
            return _StaionHistoryDataService.InsertStationHistoryDataToDB(staionHistoryDatarequest);
        }

        [HttpPost]
        [Route("v1/StaionHistoryData/DeleteStationHistoryDataByPointAndTime")]
        public BasicResponse DeleteStationHistoryDataByPointAndTime(DeleteByPointAndTimeStationHistoryDataRequest request)
        {
            return _StaionHistoryDataService.DeleteStationHistoryDataByPointAndTime(request);
        }
    }
}
