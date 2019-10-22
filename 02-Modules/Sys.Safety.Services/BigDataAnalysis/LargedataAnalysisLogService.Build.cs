using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.JC_Largedataanalysislog;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class LargedataAnalysisLogService : ILargedataAnalysisLogService
    {
        private ILargedataAnalysisLogRepository _Repository;

        public LargedataAnalysisLogService(ILargedataAnalysisLogRepository _Repository)
        {
            this._Repository = _Repository;
        }
        public BasicResponse<JC_LargedataAnalysisLogInfo> AddJC_Largedataanalysislog(LargedataAnalysisLogAddRequest largedataanalysislogrequest)
        {
            var _jC_Largedataanalysislog = ObjectConverter.Copy<JC_LargedataAnalysisLogInfo, JC_LargedataanalysislogModel>(largedataanalysislogrequest.JC_LargedataAnalysisLogInfo);
            var resultjC_Largedataanalysislog = _Repository.AddJC_Largedataanalysislog(_jC_Largedataanalysislog);
            var jC_Largedataanalysislogresponse = new BasicResponse<JC_LargedataAnalysisLogInfo>();
            jC_Largedataanalysislogresponse.Data = ObjectConverter.Copy<JC_LargedataanalysislogModel, JC_LargedataAnalysisLogInfo>(resultjC_Largedataanalysislog);
            return jC_Largedataanalysislogresponse;
        }
        public BasicResponse<JC_LargedataAnalysisLogInfo> UpdateJC_Largedataanalysislog(LargedataAnalysisLogUpdateRequest largedataanalysislogrequest)
        {
            var _jC_Largedataanalysislog = ObjectConverter.Copy<JC_LargedataAnalysisLogInfo, JC_LargedataanalysislogModel>(largedataanalysislogrequest.JC_LargedataAnalysisLogInfo);
            _Repository.UpdateJC_Largedataanalysislog(_jC_Largedataanalysislog);
            var jC_Largedataanalysislogresponse = new BasicResponse<JC_LargedataAnalysisLogInfo>();
            jC_Largedataanalysislogresponse.Data = ObjectConverter.Copy<JC_LargedataanalysislogModel, JC_LargedataAnalysisLogInfo>(_jC_Largedataanalysislog);
            return jC_Largedataanalysislogresponse;
        }
        public BasicResponse DeleteJC_Largedataanalysislog(LargedataanalysislogDeleteRequest largedataanalysislogrequest)
        {
            JC_LargedataanalysislogModel analysisLogModel = _Repository.GetJC_LargedataanalysislogById(largedataanalysislogrequest.Id);
            if (analysisLogModel != null) {
                analysisLogModel.IsDeleted = Enums.Enums.DeleteState.Yes;
                _Repository.UpdateJC_Largedataanalysislog(analysisLogModel);
            }
            var jC_Largedataanalysislogresponse = new BasicResponse();
            return jC_Largedataanalysislogresponse;
        }
        public BasicResponse<List<JC_LargedataAnalysisLogInfo>> GetJC_LargedataanalysislogList(LargedataAnalysisLogGetListRequest largedataanalysislogrequest)
        {
            var jC_Largedataanalysislogresponse = new BasicResponse<List<JC_LargedataAnalysisLogInfo>>();
            largedataanalysislogrequest.PagerInfo.PageIndex = largedataanalysislogrequest.PagerInfo.PageIndex - 1;
            if (largedataanalysislogrequest.PagerInfo.PageIndex < 0)
            {
                largedataanalysislogrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jC_LargedataanalysislogModelLists = _Repository.GetJC_LargedataanalysislogList(largedataanalysislogrequest.PagerInfo.PageIndex, largedataanalysislogrequest.PagerInfo.PageSize, out rowcount);
            var jC_LargedataanalysislogInfoLists = new List<JC_LargedataAnalysisLogInfo>();
            foreach (var item in jC_LargedataanalysislogModelLists)
            {
                var JC_LargedataAnalysisLogInfo = ObjectConverter.Copy<JC_LargedataanalysislogModel, JC_LargedataAnalysisLogInfo>(item);
                jC_LargedataanalysislogInfoLists.Add(JC_LargedataAnalysisLogInfo);
            }
            jC_Largedataanalysislogresponse.Data = jC_LargedataanalysislogInfoLists;
            return jC_Largedataanalysislogresponse;
        }
        public BasicResponse<JC_LargedataAnalysisLogInfo> GetJC_LargedataanalysislogById(LargedataAnalysisLogGetRequest largedataanalysislogrequest)
        {
            var result = _Repository.GetJC_LargedataanalysislogById(largedataanalysislogrequest.Id);
            var jC_LargedataanalysislogInfo = ObjectConverter.Copy<JC_LargedataanalysislogModel, JC_LargedataAnalysisLogInfo>(result);
            var jC_Largedataanalysislogresponse = new BasicResponse<JC_LargedataAnalysisLogInfo>();
            jC_Largedataanalysislogresponse.Data = jC_LargedataanalysislogInfo;
            return jC_Largedataanalysislogresponse;
        }

        public BasicResponse<List<JC_LargedataAnalysisLogInfo>> GetLargedataAnalysisLogListByAnalysisModelId(LargedataAnalysisLogGetByAnalysisModelIdRequest largedataAnalysisLogGetByAnalysisModelIdRequest)
        {
            var result = _Repository.GetLargedataAnalysisLogListByAnalysisModelId(largedataAnalysisLogGetByAnalysisModelIdRequest.AnalysisModelId);
            var response = new BasicResponse<List<JC_LargedataAnalysisLogInfo>>();
            if(result != null)
                response.Data = ObjectConverter.CopyList<JC_LargedataanalysislogModel, JC_LargedataAnalysisLogInfo>(result).ToList();
            return response;
        }

        public BasicResponse<List<JC_LargedataAnalysisLogInfo>> GetLargedataAnalysisLogListByModelIdAndTime(LargedataAnalysisLogGetListByModelIdAndTimeRequest largedataAnalysisLogGetListByModelIdAndTimeRequest)
        {
            var result = _Repository.GetLargedataAnalysisLogListByModelIdAndTime(largedataAnalysisLogGetListByModelIdAndTimeRequest.AnalysisModelId, largedataAnalysisLogGetListByModelIdAndTimeRequest.AnalysisDate);
            var response = new BasicResponse<List<JC_LargedataAnalysisLogInfo>>();
            if (result != null)
                response.Data = ObjectConverter.CopyList<JC_LargedataanalysislogModel, JC_LargedataAnalysisLogInfo>(result).ToList();
            return response;
        }

        /// <summary>
        /// 获取最近一条分析模型的分析日志
        /// </summary>
        /// <param name="largedataAnalysisLogGetByAnalysisModelIdRequest">获取分析模型的分析日志请求对象</param>
        /// <returns>最近一条分析模型的分析日志</returns>
        public BasicResponse<JC_LargedataAnalysisLogInfo> GetLargedataAnalysisLogLatestByAnalysisModelId(LargedataAnalysisLogGetByAnalysisModelIdRequest largedataAnalysisLogGetByAnalysisModelIdRequest)
        {
            var result = _Repository.GetLargedataAnalysisLogLatestByAnalysisModelId(largedataAnalysisLogGetByAnalysisModelIdRequest.AnalysisModelId);
            var response = new BasicResponse<JC_LargedataAnalysisLogInfo>();
            if (result != null)
                response.Data = ObjectConverter.Copy<JC_LargedataanalysislogModel, JC_LargedataAnalysisLogInfo>(result);
            return response;
        }
    }
}


