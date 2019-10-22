using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.StaionControlHistoryData;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class StaionControlHistoryDataService : IStaionControlHistoryDataService
    {
        private IStaionControlHistoryDataRepository _Repository;

        public StaionControlHistoryDataService(IStaionControlHistoryDataRepository _Repository)
        {
            this._Repository = _Repository;
        }
        public BasicResponse<StaionControlHistoryDataInfo> AddStaionControlHistoryData(StaionControlHistoryDataAddRequest staionControlHistoryDatarequest)
        {
            var _staionControlHistoryData = ObjectConverter.Copy<StaionControlHistoryDataInfo, StaionControlHistoryDataModel>(staionControlHistoryDatarequest.StaionControlHistoryDataInfo);
            var resultstaionControlHistoryData = _Repository.AddStaionControlHistoryData(_staionControlHistoryData);
            var staionControlHistoryDataresponse = new BasicResponse<StaionControlHistoryDataInfo>();
            staionControlHistoryDataresponse.Data = ObjectConverter.Copy<StaionControlHistoryDataModel, StaionControlHistoryDataInfo>(resultstaionControlHistoryData);
            return staionControlHistoryDataresponse;
        }
        public BasicResponse<StaionControlHistoryDataInfo> UpdateStaionControlHistoryData(StaionControlHistoryDataUpdateRequest staionControlHistoryDatarequest)
        {
            var _staionControlHistoryData = ObjectConverter.Copy<StaionControlHistoryDataInfo, StaionControlHistoryDataModel>(staionControlHistoryDatarequest.StaionControlHistoryDataInfo);
            _Repository.UpdateStaionControlHistoryData(_staionControlHistoryData);
            var staionControlHistoryDataresponse = new BasicResponse<StaionControlHistoryDataInfo>();
            staionControlHistoryDataresponse.Data = ObjectConverter.Copy<StaionControlHistoryDataModel, StaionControlHistoryDataInfo>(_staionControlHistoryData);
            return staionControlHistoryDataresponse;
        }
        public BasicResponse DeleteStaionControlHistoryData(StaionControlHistoryDataDeleteRequest staionControlHistoryDatarequest)
        {
            _Repository.DeleteStaionControlHistoryData(staionControlHistoryDatarequest.Id);
            var staionControlHistoryDataresponse = new BasicResponse();
            return staionControlHistoryDataresponse;
        }
        public BasicResponse<List<StaionControlHistoryDataInfo>> GetStaionControlHistoryDataList(StaionControlHistoryDataGetListRequest staionControlHistoryDatarequest)
        {
            var staionControlHistoryDataresponse = new BasicResponse<List<StaionControlHistoryDataInfo>>();
            staionControlHistoryDatarequest.PagerInfo.PageIndex = staionControlHistoryDatarequest.PagerInfo.PageIndex - 1;
            if (staionControlHistoryDatarequest.PagerInfo.PageIndex < 0)
            {
                staionControlHistoryDatarequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var staionControlHistoryDataModelLists = _Repository.GetStaionControlHistoryDataList(staionControlHistoryDatarequest.PagerInfo.PageIndex, staionControlHistoryDatarequest.PagerInfo.PageSize, out rowcount);
            var staionControlHistoryDataInfoLists = new List<StaionControlHistoryDataInfo>();
            foreach (var item in staionControlHistoryDataModelLists)
            {
                var StaionControlHistoryDataInfo = ObjectConverter.Copy<StaionControlHistoryDataModel, StaionControlHistoryDataInfo>(item);
                staionControlHistoryDataInfoLists.Add(StaionControlHistoryDataInfo);
            }
            staionControlHistoryDataresponse.Data = staionControlHistoryDataInfoLists;
            return staionControlHistoryDataresponse;
        }
        public BasicResponse<StaionControlHistoryDataInfo> GetStaionControlHistoryDataById(StaionControlHistoryDataGetRequest staionControlHistoryDatarequest)
        {
            var result = _Repository.GetStaionControlHistoryDataById(staionControlHistoryDatarequest.Id);
            var staionControlHistoryDataInfo = ObjectConverter.Copy<StaionControlHistoryDataModel, StaionControlHistoryDataInfo>(result);
            var staionControlHistoryDataresponse = new BasicResponse<StaionControlHistoryDataInfo>();
            staionControlHistoryDataresponse.Data = staionControlHistoryDataInfo;
            return staionControlHistoryDataresponse;
        }


        public BasicResponse<List<StaionControlHistoryDataInfo>> GetStaionControlHistoryDataByFzh(StaionControlHistoryDataGetByFzhRequest staionControlHistoryDatarequest)
        {
            var staionControlHistoryDataresponse = new BasicResponse<List<StaionControlHistoryDataInfo>>();

            var staionControlHistoryDataModelLists = _Repository.GetStaionControlHistoryDataByFzh(staionControlHistoryDatarequest.Fzh);
            var staionControlHistoryDataInfoLists = new List<StaionControlHistoryDataInfo>();
            foreach (var item in staionControlHistoryDataModelLists)
            {
                var StaionControlHistoryDataInfo = ObjectConverter.Copy<StaionControlHistoryDataModel, StaionControlHistoryDataInfo>(item);
                staionControlHistoryDataInfoLists.Add(StaionControlHistoryDataInfo);
            }

            staionControlHistoryDataresponse.Data = staionControlHistoryDataInfoLists;
            return staionControlHistoryDataresponse;
        }

        public BasicResponse InsertStationControlHistoryDataToDB(StationControlHistoryDataToDBRequest staionControlHistoryDatarequest)
        {
            var StationControlHistoryModels = ObjectConverter.CopyList<StaionControlHistoryDataInfo, StaionControlHistoryDataModel>(staionControlHistoryDatarequest.StaionControlHistoryDataItems);
            _Repository.Insert(StationControlHistoryModels);
            return new BasicResponse();
        }

        public BasicResponse<List<GetStaionControlHistoryDataByByFzhTimeResponse>> GetStaionControlHistoryDataByByFzhTime(GetStaionControlHistoryDataByByFzhTimeRequest request)
        {
            var dt = _Repository.QueryTable("global_StaionControlHistoryDataService_GetStaionControlHistoryDataByByFzhTime",
                request.Fzh, request.Time);
            var gsh = ObjectConverter.Copy<GetStaionControlHistoryDataByByFzhTimeResponse>(dt);

            //ControlDevice转换为2进制字符串
            foreach (var item in gsh)
            {
                item.ControlDeviceConvert = Convert.ToString(item.ControlDevice, 2).PadLeft(16, '0');
            }
            return new BasicResponse<List<GetStaionControlHistoryDataByByFzhTimeResponse>>()
            {
                Data = gsh
            };
        }

        /// <summary>
        /// 删除分站控制历史数据(根据测点和时间)  20170703
        /// </summary>
        /// <param name="staionHistoryDatarequest"></param>
        /// <returns></returns>
        public BasicResponse DeleteStaionControlHistoryDataByPointAndTime(DeleteByPointAndTimeStaionControlHistoryDataRequest request)
        {
            BasicResponse result = new BasicResponse();
            string Point = request.Point;
            string SaveTime = request.Time.ToString("yyyy-MM-dd HH:mm:ss");
            _Repository.ExecuteNonQuery("global_Staioncontrolhistorydata_DeleteStaioncontrolhistorydataByPointAndTime", Point, SaveTime);
            return new BasicResponse();
        }
    }
}


