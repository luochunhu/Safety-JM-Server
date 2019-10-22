using System.Collections.Generic;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.Request.R_Phistory;
using System;

namespace Sys.Safety.Services
{
    public partial class R_PhistoryService : IR_PhistoryService
    {
        private IR_PhistoryRepository _Repository;

        public R_PhistoryService(IR_PhistoryRepository _Repository)
        {
            this._Repository = _Repository;
        }
        public BasicResponse<R_PhistoryInfo> AddPhistory(R_PhistoryAddRequest phistoryRequest)
        {
            var _phistory = ObjectConverter.Copy<R_PhistoryInfo, R_PhistoryModel>(phistoryRequest.PhistoryInfo);
            var resultphistory = _Repository.AddPhistory(_phistory);
            var phistoryresponse = new BasicResponse<R_PhistoryInfo>();
            phistoryresponse.Data = ObjectConverter.Copy<R_PhistoryModel, R_PhistoryInfo>(resultphistory);
            return phistoryresponse;
        }
        public BasicResponse<R_PhistoryInfo> UpdatePhistory(R_PhistoryUpdateRequest phistoryRequest)
        {
            var _phistory = ObjectConverter.Copy<R_PhistoryInfo, R_PhistoryModel>(phistoryRequest.PhistoryInfo);
            _Repository.UpdatePhistory(_phistory);
            var phistoryresponse = new BasicResponse<R_PhistoryInfo>();
            phistoryresponse.Data = ObjectConverter.Copy<R_PhistoryModel, R_PhistoryInfo>(_phistory);
            return phistoryresponse;
        }
        public BasicResponse DeletePhistory(R_PhistoryDeleteRequest phistoryRequest)
        {
            _Repository.DeletePhistory(phistoryRequest.Id);
            var phistoryresponse = new BasicResponse();
            return phistoryresponse;
        }
        public BasicResponse<List<R_PhistoryInfo>> GetPhistoryList(R_PhistoryGetListRequest phistoryRequest)
        {
            var phistoryresponse = new BasicResponse<List<R_PhistoryInfo>>();
            phistoryRequest.PagerInfo.PageIndex = phistoryRequest.PagerInfo.PageIndex - 1;
            if (phistoryRequest.PagerInfo.PageIndex < 0)
            {
                phistoryRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var phistoryModelLists = _Repository.GetPhistoryList(phistoryRequest.PagerInfo.PageIndex, phistoryRequest.PagerInfo.PageSize, out rowcount);
            var phistoryInfoLists = new List<R_PhistoryInfo>();
            foreach (var item in phistoryModelLists)
            {
                var PhistoryInfo = ObjectConverter.Copy<R_PhistoryModel, R_PhistoryInfo>(item);
                phistoryInfoLists.Add(PhistoryInfo);
            }
            phistoryresponse.Data = phistoryInfoLists;
            return phistoryresponse;
        }
        public BasicResponse<R_PhistoryInfo> GetPhistoryById(R_PhistoryGetRequest phistoryRequest)
        {
            var result = _Repository.GetPhistoryById(phistoryRequest.Id);
            var phistoryInfo = ObjectConverter.Copy<R_PhistoryModel, R_PhistoryInfo>(result);
            var phistoryresponse = new BasicResponse<R_PhistoryInfo>();
            phistoryresponse.Data = phistoryInfo;
            return phistoryresponse;
        }



        public BasicResponse<R_PhistoryInfo> GetPhistoryByPar(R_PhistoryGetByParRequest request)
        {
            var result = _Repository.QueryTable("global_R_PhistoryService_QueryR_PhistoryByPar", DateTime.Now.ToString("yyyyMMdd"), request.yid,request.pointid,request.rtime);
            var resultToList = ObjectConverter.Copy<R_PhistoryInfo>(result);
            R_PhistoryInfo phistoryInfo = null;
            if (resultToList.Count > 0)
            {
                phistoryInfo = ObjectConverter.Copy<R_PhistoryInfo>(result)[0];
            }
            var phistoryresponse = new BasicResponse<R_PhistoryInfo>();
            phistoryresponse.Data = phistoryInfo;
            return phistoryresponse;
        }
        /// <summary>
        /// 获取人员最后一条轨迹记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BasicResponse<R_PhistoryInfo> GetPersonLastR_Phistory(R_PhistoryGetLastByYidRequest request)
        {
            var result = _Repository.QueryTable("global_R_PhistoryService_QueryR_PhistoryFromDB", DateTime.Now.ToString("yyyyMMdd"), request.yid);
            var resultToList = ObjectConverter.Copy<R_PhistoryInfo>(result);
            R_PhistoryInfo phistoryInfo = null;
            if (resultToList.Count > 0)
            {
                phistoryInfo = ObjectConverter.Copy<R_PhistoryInfo>(result)[0];
            }
            var phistoryresponse = new BasicResponse<R_PhistoryInfo>();
            phistoryresponse.Data = phistoryInfo;
            return phistoryresponse;
        }
        /// <summary>
        /// 根据存储时间查询轨迹记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BasicResponse<List<R_PhistoryInfo>> GetPersonR_PhistoryByTimer(R_PhistoryGetLastByTimerRequest request)
        {
            var result = _Repository.QueryTable("global_R_PhistoryService_QueryR_PhistoryByTimer", DateTime.Now.ToString("yyyyMMdd"), request.Timer);
            List<R_PhistoryInfo> R_PhistoryInfoList = new List<R_PhistoryInfo>();
            for (int i = 0; i < result.Rows.Count; i++)
            {
                R_PhistoryInfo tempInfo = new R_PhistoryInfo();
                tempInfo.Id = result.Rows[i]["id"].ToString();
                tempInfo.Bh = result.Rows[i]["bh"].ToString();
                tempInfo.Name = result.Rows[i]["name"].ToString();
                tempInfo.Rtime =DateTime.Parse( result.Rows[i]["rtime"].ToString());
                tempInfo.Timer = DateTime.Parse(result.Rows[i]["timer"].ToString());
                tempInfo.Flag = result.Rows[i]["flag"].ToString();
                R_PhistoryInfoList.Add(tempInfo);
            }
            var phistoryresponse = new BasicResponse<List<R_PhistoryInfo>>();
            phistoryresponse.Data = R_PhistoryInfoList;
            return phistoryresponse;
        }
    }
}


