using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.StaionControlHistoryData;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IStaionControlHistoryDataService
    {
        BasicResponse<StaionControlHistoryDataInfo> AddStaionControlHistoryData(StaionControlHistoryDataAddRequest staionControlHistoryDatarequest);
        BasicResponse<StaionControlHistoryDataInfo> UpdateStaionControlHistoryData(StaionControlHistoryDataUpdateRequest staionControlHistoryDatarequest);
        BasicResponse DeleteStaionControlHistoryData(StaionControlHistoryDataDeleteRequest staionControlHistoryDatarequest);
        BasicResponse<List<StaionControlHistoryDataInfo>> GetStaionControlHistoryDataList(StaionControlHistoryDataGetListRequest staionControlHistoryDatarequest);
        BasicResponse<StaionControlHistoryDataInfo> GetStaionControlHistoryDataById(StaionControlHistoryDataGetRequest staionControlHistoryDatarequest);

        BasicResponse<List<StaionControlHistoryDataInfo>> GetStaionControlHistoryDataByFzh(StaionControlHistoryDataGetByFzhRequest staionControlHistoryDatarequest);
        BasicResponse InsertStationControlHistoryDataToDB(StationControlHistoryDataToDBRequest staionControlHistoryDatarequest);

        /// <summary>
        /// 根据分站号和时间获取分站的控制历史数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse<List<GetStaionControlHistoryDataByByFzhTimeResponse>> GetStaionControlHistoryDataByByFzhTime(
            GetStaionControlHistoryDataByByFzhTimeRequest request);
        /// <summary>
        /// 删除分站控制历史数据(根据测点和时间)  20170703
        /// </summary>
        /// <param name="staionHistoryDatarequest"></param>
        /// <returns></returns>
        BasicResponse DeleteStaionControlHistoryDataByPointAndTime(DeleteByPointAndTimeStaionControlHistoryDataRequest request);
    }
}

