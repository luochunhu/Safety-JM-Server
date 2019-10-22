using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.StaionHistoryData;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.Request.StaionControlHistoryData;

namespace Sys.Safety.Services
{
    public partial class StaionHistoryDataService : IStaionHistoryDataService
    {
        private IStaionHistoryDataRepository _Repository;

        public StaionHistoryDataService(IStaionHistoryDataRepository _Repository)
        {
            this._Repository = _Repository;
        }
        public BasicResponse<StaionHistoryDataInfo> AddStaionHistoryData(StaionHistoryDataAddRequest staionHistoryDatarequest)
        {
            var _staionHistoryData = ObjectConverter.Copy<StaionHistoryDataInfo, StaionHistoryDataModel>(staionHistoryDatarequest.StaionHistoryDataInfo);
            var resultstaionHistoryData = _Repository.AddStaionHistoryData(_staionHistoryData);
            var staionHistoryDataresponse = new BasicResponse<StaionHistoryDataInfo>();
            staionHistoryDataresponse.Data = ObjectConverter.Copy<StaionHistoryDataModel, StaionHistoryDataInfo>(resultstaionHistoryData);
            return staionHistoryDataresponse;
        }
        public BasicResponse<StaionHistoryDataInfo> UpdateStaionHistoryData(StaionHistoryDataUpdateRequest staionHistoryDatarequest)
        {
            var _staionHistoryData = ObjectConverter.Copy<StaionHistoryDataInfo, StaionHistoryDataModel>(staionHistoryDatarequest.StaionHistoryDataInfo);
            _Repository.UpdateStaionHistoryData(_staionHistoryData);
            var staionHistoryDataresponse = new BasicResponse<StaionHistoryDataInfo>();
            staionHistoryDataresponse.Data = ObjectConverter.Copy<StaionHistoryDataModel, StaionHistoryDataInfo>(_staionHistoryData);
            return staionHistoryDataresponse;
        }
        public BasicResponse DeleteStaionHistoryData(StaionHistoryDataDeleteRequest staionHistoryDatarequest)
        {
            _Repository.DeleteStaionHistoryData(staionHistoryDatarequest.Id);
            var staionHistoryDataresponse = new BasicResponse();
            return staionHistoryDataresponse;
        }
        public BasicResponse<List<StaionHistoryDataInfo>> GetStaionHistoryDataList(StaionHistoryDataGetListRequest staionHistoryDatarequest)
        {
            var staionHistoryDataresponse = new BasicResponse<List<StaionHistoryDataInfo>>();
            staionHistoryDatarequest.PagerInfo.PageIndex = staionHistoryDatarequest.PagerInfo.PageIndex - 1;
            if (staionHistoryDatarequest.PagerInfo.PageIndex < 0)
            {
                staionHistoryDatarequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var staionHistoryDataModelLists = _Repository.GetStaionHistoryDataList(staionHistoryDatarequest.PagerInfo.PageIndex, staionHistoryDatarequest.PagerInfo.PageSize, out rowcount);
            var staionHistoryDataInfoLists = new List<StaionHistoryDataInfo>();
            foreach (var item in staionHistoryDataModelLists)
            {
                var StaionHistoryDataInfo = ObjectConverter.Copy<StaionHistoryDataModel, StaionHistoryDataInfo>(item);
                staionHistoryDataInfoLists.Add(StaionHistoryDataInfo);
            }
            staionHistoryDataresponse.Data = staionHistoryDataInfoLists;
            return staionHistoryDataresponse;
        }
        public BasicResponse<StaionHistoryDataInfo> GetStaionHistoryDataById(StaionHistoryDataGetRequest staionHistoryDatarequest)
        {
            var result = _Repository.GetStaionHistoryDataById(staionHistoryDatarequest.Id);
            var staionHistoryDataInfo = ObjectConverter.Copy<StaionHistoryDataModel, StaionHistoryDataInfo>(result);
            var staionHistoryDataresponse = new BasicResponse<StaionHistoryDataInfo>();
            staionHistoryDataresponse.Data = staionHistoryDataInfo;
            return staionHistoryDataresponse;
        }
        public BasicResponse<List<StaionHistoryDataInfo>> GetStaionHistoryDataByFzh(StaionHistoryDataGetByFzhRequest staionHistoryDatarequest)
        {
            var staionHistoryDataresponse = new BasicResponse<List<StaionHistoryDataInfo>>();
            var staionHistoryDataModelLists = _Repository.GetAllStaionHistoryData();
            var staionHistoryDataInfoLists = new List<StaionHistoryDataInfo>();
            foreach (var item in staionHistoryDataModelLists)
            {
                if (item.Fzh == staionHistoryDatarequest.Fzh)
                {
                    var StaionHistoryDataInfo = ObjectConverter.Copy<StaionHistoryDataModel, StaionHistoryDataInfo>(item);
                    staionHistoryDataInfoLists.Add(StaionHistoryDataInfo);
                }
            }
            staionHistoryDataresponse.Data = staionHistoryDataInfoLists;
            return staionHistoryDataresponse;
        }

        public BasicResponse<List<GetSubstationHistoryRealDataByFzhTimeResponse>> GetSubstationHistoryRealDataByFzhTime(
            GetSubstationHistoryRealDataByFzhTimeRequest request)
        {
            var dt = _Repository.QueryTable("global_StaionHistoryDataService_GetSubstationHistoryRealDataByFzhTime",
                request.Fzh, request.Time);
            var gsh = ObjectConverter.Copy<GetSubstationHistoryRealDataByFzhTimeResponse>(dt);
            return new BasicResponse<List<GetSubstationHistoryRealDataByFzhTimeResponse>>()
            {
                Data = gsh
            };
        }

        public BasicResponse InsertStationHistoryDataToDB(InsertStationHistoryDataRequest staionHistoryDatarequest)
        {
            var StationHistoryDataModels = ObjectConverter.CopyList<StaionHistoryDataInfo, StaionHistoryDataModel>(staionHistoryDatarequest.StationHistoryDataItems);
            _Repository.Insert(StationHistoryDataModels);
            return new BasicResponse();
        }
        /// <summary>
        /// 删除分站历史数据(根据测点和时间)  20170703
        /// </summary>
        /// <param name="staionHistoryDatarequest"></param>
        /// <returns></returns>
        public BasicResponse DeleteStationHistoryDataByPointAndTime(DeleteByPointAndTimeStationHistoryDataRequest staionHistoryDatarequest)
        {
            BasicResponse result = new BasicResponse();
            string Point=staionHistoryDatarequest.Point;
            string SaveTime=staionHistoryDatarequest.Time.ToString("yyyy-MM-dd HH:mm:ss");
           _Repository.ExecuteNonQuery("global_StationHistoryData_DeleteStationHistoryDataByPointAndTime", Point, SaveTime);          
            return new BasicResponse();
        }
    }
}


