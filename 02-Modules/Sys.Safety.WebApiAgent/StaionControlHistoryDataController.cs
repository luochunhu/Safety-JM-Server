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
using Sys.Safety.Request.StaionControlHistoryData;

namespace Sys.Safety.WebApiAgent
{
    public class StaionControlHistoryDataControllerProxy : BaseProxy, IStaionControlHistoryDataService
    {
        //public BasicResponse<List<Jc_WzInfo>> GetPositionCacheByWz(PositionGetByWzRequest PositionRequest)
        //{
        //    var responseStr = HttpClientHelper.Post(Webapi + "/v1/Position/GetPositionCacheByWz?token=" + Token, JSONHelper.ToJSONString(PositionRequest));
        //    return JSONHelper.ParseJSONString<BasicResponse<List<Jc_WzInfo>>>(responseStr);
        //}

        public BasicResponse<StaionControlHistoryDataInfo> AddStaionControlHistoryData(Sys.Safety.Request.StaionControlHistoryData.StaionControlHistoryDataAddRequest staionControlHistoryDatarequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/StaionControlHistoryData/AddStaionControlHistoryData?token=" + Token, JSONHelper.ToJSONString(staionControlHistoryDatarequest));
            return JSONHelper.ParseJSONString<BasicResponse<StaionControlHistoryDataInfo>>(responseStr);
        }

        public BasicResponse<StaionControlHistoryDataInfo> UpdateStaionControlHistoryData(Sys.Safety.Request.StaionControlHistoryData.StaionControlHistoryDataUpdateRequest staionControlHistoryDatarequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/StaionControlHistoryData/UpdateStaionControlHistoryData?token=" + Token, JSONHelper.ToJSONString(staionControlHistoryDatarequest));
            return JSONHelper.ParseJSONString<BasicResponse<StaionControlHistoryDataInfo>>(responseStr);
        }

        public BasicResponse DeleteStaionControlHistoryData(Sys.Safety.Request.StaionControlHistoryData.StaionControlHistoryDataDeleteRequest staionControlHistoryDatarequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/StaionControlHistoryData/DeleteStaionControlHistoryData?token=" + Token, JSONHelper.ToJSONString(staionControlHistoryDatarequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse<List<StaionControlHistoryDataInfo>> GetStaionControlHistoryDataList(Sys.Safety.Request.StaionControlHistoryData.StaionControlHistoryDataGetListRequest staionControlHistoryDatarequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/StaionControlHistoryData/GetStaionControlHistoryDataList?token=" + Token, JSONHelper.ToJSONString(staionControlHistoryDatarequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<StaionControlHistoryDataInfo>>>(responseStr);
        }

        public BasicResponse<StaionControlHistoryDataInfo> GetStaionControlHistoryDataById(Sys.Safety.Request.StaionControlHistoryData.StaionControlHistoryDataGetRequest staionControlHistoryDatarequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/StaionControlHistoryData/GetStaionControlHistoryDataById?token=" + Token, JSONHelper.ToJSONString(staionControlHistoryDatarequest));
            return JSONHelper.ParseJSONString<BasicResponse<StaionControlHistoryDataInfo>>(responseStr);
        }

        public BasicResponse<List<StaionControlHistoryDataInfo>> GetStaionControlHistoryDataByFzh(Sys.Safety.Request.StaionControlHistoryData.StaionControlHistoryDataGetByFzhRequest staionControlHistoryDatarequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/StaionControlHistoryData/GetStaionControlHistoryDataByFzh?token=" + Token, JSONHelper.ToJSONString(staionControlHistoryDatarequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<StaionControlHistoryDataInfo>>>(responseStr);
        }


        public BasicResponse InsertStationControlHistoryDataToDB(Sys.Safety.Request.StaionControlHistoryData.StationControlHistoryDataToDBRequest staionControlHistoryDatarequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/StaionControlHistoryData/InsertStationControlHistoryDataToDB?token=" + Token, JSONHelper.ToJSONString(staionControlHistoryDatarequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse<List<Sys.Safety.Request.StaionControlHistoryData.GetStaionControlHistoryDataByByFzhTimeResponse>> GetStaionControlHistoryDataByByFzhTime(Sys.Safety.Request.StaionControlHistoryData.GetStaionControlHistoryDataByByFzhTimeRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/StaionControlHistoryData/GetStaionControlHistoryDataByByFzhTime?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<List<Sys.Safety.Request.StaionControlHistoryData.GetStaionControlHistoryDataByByFzhTimeResponse>>>(responseStr);
        }

        public BasicResponse DeleteStaionControlHistoryDataByPointAndTime(DeleteByPointAndTimeStaionControlHistoryDataRequest staionControlHistoryDatarequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/StaionControlHistoryData/DeleteStaionControlHistoryDataByPointAndTime?token=" + Token, JSONHelper.ToJSONString(staionControlHistoryDatarequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
    }
}
