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
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.StaionHistoryData;

namespace Sys.Safety.WebApiAgent
{
    public class StaionHistoryDataControllerProxy : BaseProxy, IStaionHistoryDataService
    {
        //public BasicResponse<List<Jc_WzInfo>> GetPositionCacheByWz(PositionGetByWzRequest PositionRequest)
        //{
        //    var responseStr = HttpClientHelper.Post(Webapi + "/v1/Position/GetPositionCacheByWz?token=" + Token, JSONHelper.ToJSONString(PositionRequest));
        //    return JSONHelper.ParseJSONString<BasicResponse<List<Jc_WzInfo>>>(responseStr);
        //}
        public BasicResponse<StaionHistoryDataInfo> AddStaionHistoryData(Sys.Safety.Request.StaionHistoryData.StaionHistoryDataAddRequest staionHistoryDatarequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/StaionHistoryData/AddStaionHistoryData?token=" + Token, JSONHelper.ToJSONString(staionHistoryDatarequest));
            return JSONHelper.ParseJSONString<BasicResponse<StaionHistoryDataInfo>>(responseStr);
        }

        public BasicResponse<StaionHistoryDataInfo> UpdateStaionHistoryData(Sys.Safety.Request.StaionHistoryData.StaionHistoryDataUpdateRequest staionHistoryDatarequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/StaionHistoryData/UpdateStaionHistoryData?token=" + Token, JSONHelper.ToJSONString(staionHistoryDatarequest));
            return JSONHelper.ParseJSONString<BasicResponse<StaionHistoryDataInfo>>(responseStr);
        }

        public BasicResponse DeleteStaionHistoryData(Sys.Safety.Request.StaionHistoryData.StaionHistoryDataDeleteRequest staionHistoryDatarequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/StaionHistoryData/DeleteStaionHistoryData?token=" + Token, JSONHelper.ToJSONString(staionHistoryDatarequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse<List<StaionHistoryDataInfo>> GetStaionHistoryDataList(Sys.Safety.Request.StaionHistoryData.StaionHistoryDataGetListRequest staionHistoryDatarequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/StaionHistoryData/GetStaionHistoryDataList?token=" + Token, JSONHelper.ToJSONString(staionHistoryDatarequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<StaionHistoryDataInfo>>>(responseStr);
        }


        public BasicResponse<StaionHistoryDataInfo> GetStaionHistoryDataById(Sys.Safety.Request.StaionHistoryData.StaionHistoryDataGetRequest staionHistoryDatarequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/StaionHistoryData/GetStaionHistoryDataById?token=" + Token, JSONHelper.ToJSONString(staionHistoryDatarequest));
            return JSONHelper.ParseJSONString<BasicResponse<StaionHistoryDataInfo>>(responseStr);
        }

        public BasicResponse<List<StaionHistoryDataInfo>> GetStaionHistoryDataByFzh(Sys.Safety.Request.StaionHistoryData.StaionHistoryDataGetByFzhRequest staionHistoryDatarequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/StaionHistoryData/GetStaionHistoryDataByFzh?token=" + Token, JSONHelper.ToJSONString(staionHistoryDatarequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<StaionHistoryDataInfo>>>(responseStr);
        }


        public BasicResponse<List<Sys.Safety.Request.StaionHistoryData.GetSubstationHistoryRealDataByFzhTimeResponse>> GetSubstationHistoryRealDataByFzhTime(Sys.Safety.Request.StaionHistoryData.GetSubstationHistoryRealDataByFzhTimeRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/StaionHistoryData/GetSubstationHistoryRealDataByFzhTime?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<List<Sys.Safety.Request.StaionHistoryData.GetSubstationHistoryRealDataByFzhTimeResponse>>>(responseStr);
        }
        public BasicResponse InsertStationHistoryDataToDB(Sys.Safety.Request.StaionHistoryData.InsertStationHistoryDataRequest staionHistoryDatarequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/StaionHistoryData/InsertStationHistoryDataToDB?token=" + Token, JSONHelper.ToJSONString(staionHistoryDatarequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
        public BasicResponse DeleteStationHistoryDataByPointAndTime(DeleteByPointAndTimeStationHistoryDataRequest staionHistoryDatarequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/StaionHistoryData/DeleteStationHistoryDataByPointAndTime?token=" + Token, JSONHelper.ToJSONString(staionHistoryDatarequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
    }
}
