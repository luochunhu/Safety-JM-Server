using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.StaionHistoryData;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IStaionHistoryDataService
    {
        BasicResponse<StaionHistoryDataInfo> AddStaionHistoryData(StaionHistoryDataAddRequest staionHistoryDatarequest);
        BasicResponse<StaionHistoryDataInfo> UpdateStaionHistoryData(StaionHistoryDataUpdateRequest staionHistoryDatarequest);
        BasicResponse DeleteStaionHistoryData(StaionHistoryDataDeleteRequest staionHistoryDatarequest);
        BasicResponse<List<StaionHistoryDataInfo>> GetStaionHistoryDataList(StaionHistoryDataGetListRequest staionHistoryDatarequest);
        BasicResponse<StaionHistoryDataInfo> GetStaionHistoryDataById(StaionHistoryDataGetRequest staionHistoryDatarequest);
        BasicResponse<List<StaionHistoryDataInfo>> GetStaionHistoryDataByFzh(StaionHistoryDataGetByFzhRequest staionHistoryDatarequest);

        /// <summary>
        /// 根据分站号和时间获取分站的5分钟历史数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse<List<GetSubstationHistoryRealDataByFzhTimeResponse>> GetSubstationHistoryRealDataByFzhTime(
            GetSubstationHistoryRealDataByFzhTimeRequest request);

        BasicResponse InsertStationHistoryDataToDB(InsertStationHistoryDataRequest staionHistoryDatarequest);
        /// <summary>
        /// 删除分站历史数据(根据测点和时间)  20170703
        /// </summary>
        /// <param name="staionHistoryDatarequest"></param>
        /// <returns></returns>
        BasicResponse DeleteStationHistoryDataByPointAndTime(DeleteByPointAndTimeStationHistoryDataRequest staionHistoryDatarequest);
    }
}

