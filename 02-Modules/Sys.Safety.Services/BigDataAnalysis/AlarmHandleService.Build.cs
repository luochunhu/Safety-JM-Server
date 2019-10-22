using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.AlarmHandle;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.ServiceContract.Cache;
using Sys.Safety.Request.Cache;
using Basic.Framework.Logging;

namespace Sys.Safety.Services
{
    public partial class AlarmHandleService : IAlarmHandleService
    {
        private IAlarmHandleRepository _Repository;
        private ILargeDataAnalysisConfigCacheService _largeDataAnalysisConfigCacheService;
        private ILargedataAnalysisConfigRepository _LargedataAnalysisConfigRepository;

        public AlarmHandleService(IAlarmHandleRepository _Repository, ILargeDataAnalysisConfigCacheService largeDataAnalysisConfigCacheService,ILargedataAnalysisConfigRepository largedataAnalysisConfigRepository)
        {
            this._Repository = _Repository;
            this._largeDataAnalysisConfigCacheService = largeDataAnalysisConfigCacheService;
            this._LargedataAnalysisConfigRepository = largedataAnalysisConfigRepository;
        }
        public BasicResponse<JC_AlarmHandleInfo> AddJC_AlarmHandle(AlarmHandleAddRequest jC_AlarmHandlerequest)
        {
            var _jC_AlarmHandle = ObjectConverter.Copy<JC_AlarmHandleInfo, JC_AlarmHandleModel>(jC_AlarmHandlerequest.JC_AlarmHandleInfo);
            var resultjC_AlarmHandle = _Repository.AddJC_AlarmHandle(_jC_AlarmHandle);
            var jC_AlarmHandleresponse = new BasicResponse<JC_AlarmHandleInfo>();
            jC_AlarmHandleresponse.Data = ObjectConverter.Copy<JC_AlarmHandleModel, JC_AlarmHandleInfo>(resultjC_AlarmHandle);
            return jC_AlarmHandleresponse;
        }
        public BasicResponse<JC_AlarmHandleInfo> UpdateJC_AlarmHandle(AlarmHandleUpdateRequest jC_AlarmHandlerequest)
        {
            var _jC_AlarmHandle = ObjectConverter.Copy<JC_AlarmHandleInfo, JC_AlarmHandleModel>(jC_AlarmHandlerequest.JC_AlarmHandleInfo);
            _Repository.UpdateJC_AlarmHandle(_jC_AlarmHandle);
            var jC_AlarmHandleresponse = new BasicResponse<JC_AlarmHandleInfo>();
            jC_AlarmHandleresponse.Data = ObjectConverter.Copy<JC_AlarmHandleModel, JC_AlarmHandleInfo>(_jC_AlarmHandle);
            return jC_AlarmHandleresponse;
        }
        public BasicResponse DeleteJC_AlarmHandle(AlarmHandleDeleteRequest jC_AlarmHandlerequest)
        {
            _Repository.DeleteJC_AlarmHandle(jC_AlarmHandlerequest.Id);
            var jC_AlarmHandleresponse = new BasicResponse();
            return jC_AlarmHandleresponse;
        }
        public BasicResponse<List<JC_AlarmHandleInfo>> GetJC_AlarmHandleList(AlarmHandleGetListRequest jC_AlarmHandlerequest)
        {
            var jC_AlarmHandleresponse = new BasicResponse<List<JC_AlarmHandleInfo>>();
            jC_AlarmHandlerequest.PagerInfo.PageIndex = jC_AlarmHandlerequest.PagerInfo.PageIndex - 1;
            if (jC_AlarmHandlerequest.PagerInfo.PageIndex < 0)
            {
                jC_AlarmHandlerequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jC_AlarmHandleModelLists = _Repository.GetJC_AlarmHandleList(jC_AlarmHandlerequest.PagerInfo.PageIndex, jC_AlarmHandlerequest.PagerInfo.PageSize, out rowcount);
            var jC_AlarmHandleInfoLists = new List<JC_AlarmHandleInfo>();
            foreach (var item in jC_AlarmHandleModelLists)
            {
                var JC_AlarmHandleInfo = ObjectConverter.Copy<JC_AlarmHandleModel, JC_AlarmHandleInfo>(item);
                jC_AlarmHandleInfoLists.Add(JC_AlarmHandleInfo);
            }
            jC_AlarmHandleresponse.Data = jC_AlarmHandleInfoLists;
            return jC_AlarmHandleresponse;
        }
        public BasicResponse<JC_AlarmHandleInfo> GetJC_AlarmHandleById(AlarmHandleGetRequest jC_AlarmHandlerequest)
        {
            var result = _Repository.GetJC_AlarmHandleById(jC_AlarmHandlerequest.Id);
            var jC_AlarmHandleInfo = ObjectConverter.Copy<JC_AlarmHandleModel, JC_AlarmHandleInfo>(result);
            var jC_AlarmHandleresponse = new BasicResponse<JC_AlarmHandleInfo>();
            jC_AlarmHandleresponse.Data = jC_AlarmHandleInfo;
            return jC_AlarmHandleresponse;
        }

        /// <summary>
        /// 获取未关闭的报警列表
        /// </summary>
        /// <param name="alarmHandleWithoutSearchConditionRequest">空条件请求</param>
        /// <returns>未关闭的报警列表</returns>
        public BasicResponse<List<JC_AlarmHandleInfo>> GetUnclosedAlarmList(AlarmHandleWithoutSearchConditionRequest alarmHandleWithoutSearchConditionRequest)
        {
            var response = new BasicResponse<List<JC_AlarmHandleInfo>>();
            List<JC_AlarmHandleModel> alarmHandleModelList = _Repository.GetUnclosedAlarmList();
            response.Data = ObjectConverter.CopyList<JC_AlarmHandleModel, JC_AlarmHandleInfo>(alarmHandleModelList).ToList();
            return response;
        }

        /// <summary>
        /// 获取和分析模型有关的未关闭报警
        /// </summary>
        /// <param name="alarmHandleGetByAnalysisModelIdRequest">模型Id为条件的请求</param>
        /// <returns>分析模型有关的未关闭报警</returns>
        public BasicResponse<JC_AlarmHandleInfo> GetUnclosedAlarmByAnalysisModelId(AlarmHandleGetByAnalysisModelIdRequest alarmHandleGetByAnalysisModelIdRequest)
        {
            var response = new BasicResponse<JC_AlarmHandleInfo>();
            JC_AlarmHandleModel alarmHandleModel = _Repository.GetUnclosedAlarmByAnalysisModelId(alarmHandleGetByAnalysisModelIdRequest.AnalysisModelId);
            response.Data = ObjectConverter.Copy<JC_AlarmHandleModel, JC_AlarmHandleInfo>(alarmHandleModel);
            return response;
        }

        /// <summary>
        /// 获取未处理的报警列表
        /// </summary>
        /// <param name="alarmHandleWithoutSearchConditionRequest">空条件请求</param>
        /// <returns>未处理的报警列表</returns>
        public BasicResponse<List<JC_AlarmHandleInfo>> GetUnhandledAlarmList(AlarmHandleWithoutSearchConditionRequest alarmHandleWithoutSearchConditionRequest)
        {
            var response = new BasicResponse<List<JC_AlarmHandleInfo>>();
            List<JC_AlarmHandleModel> alarmHandleModelList = _Repository.GetUnhandledAlarmList();
            response.Data = ObjectConverter.CopyList<JC_AlarmHandleModel, JC_AlarmHandleInfo>(alarmHandleModelList).ToList();
            return response;
        }

        /// <summary>
        /// 获取和分析模型有关的未处理报警
        /// </summary>
        /// <param name="alarmHandleGetByAnalysisModelIdRequest">模型Id为条件的请求</param>
        /// <returns>分析模型有关的未处理报警</returns>
        public BasicResponse<JC_AlarmHandleInfo> GetUnhandledAlarmByAnalysisModelId(AlarmHandleGetByAnalysisModelIdRequest alarmHandleGetByAnalysisModelIdRequest)
        {
            var response = new BasicResponse<JC_AlarmHandleInfo>();
            JC_AlarmHandleModel alarmHandleModel = _Repository.GetUnhandledAlarmByAnalysisModelId(alarmHandleGetByAnalysisModelIdRequest.AnalysisModelId);
            response.Data = ObjectConverter.Copy<JC_AlarmHandleModel, JC_AlarmHandleInfo>(alarmHandleModel);
            return response;
        }
        /// <summary>
        /// 批量更新报警信息
        /// </summary>
        /// <param name="alarmHandleUpdateListRequest">批量更新报警信息请求</param>
        /// <returns>更新后的报警信息列表</returns>
        public BasicResponse<List<JC_AlarmHandleInfo>> UpdateAlarmHandleList(AlarmHandleUpdateListRequest alarmHandleUpdateListRequest)
        {
            var response = new BasicResponse<List<JC_AlarmHandleInfo>>();
            if (alarmHandleUpdateListRequest.AlarmHandleInfoList != null && alarmHandleUpdateListRequest.AlarmHandleInfoList.Count > 0)
            {
                List<JC_AlarmHandleModel> alarmHandleModelList = ObjectConverter.CopyList<JC_AlarmHandleInfo, JC_AlarmHandleModel>(alarmHandleUpdateListRequest.AlarmHandleInfoList).ToList();
                _Repository.UpdateAlarmHandleList(alarmHandleModelList);
                response.Data = ObjectConverter.CopyList<JC_AlarmHandleModel, JC_AlarmHandleInfo>(alarmHandleModelList).ToList();
                return response;
            }
            else
            {
                response.Data = alarmHandleUpdateListRequest.AlarmHandleInfoList;
            }
            return response;
        }

        /// <summary>
        /// 根据开始时间和结束时间获取报警列表
        /// </summary>
        /// <param name="alarmHandelRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<JC_AlarmHandleInfo>> GetAlarmHandleByStimeAndEtime(AlarmHandelGetByStimeAndETime alarmHandelRequest)
        {
            try
            {
                var stime = alarmHandelRequest.Stime.Date;
                var etime = alarmHandelRequest.Etime.AddDays(1).AddSeconds(-1);

                int pageindex=alarmHandelRequest.PagerInfo.PageIndex;
                int pagesize=alarmHandelRequest.PagerInfo.PageSize;

                DateTime noneEndTime = new DateTime(1900, 1, 1, 0, 0, 0);
                BasicResponse<List<JC_AlarmHandleInfo>> alarmresponse = new BasicResponse<List<JC_AlarmHandleInfo>>();
                //获取逻辑报警数据：当结束时间等于1990-01-01 00:00:00时取开始时间<etime;反之取开始时间<etime且结束时间>stime
                var query=_Repository.Datas.Where(alarm => 
                    (alarm.StartTime < etime && alarm.EndTime > stime) || 
                    (alarm.StartTime < etime && alarm.EndTime.Equals(noneEndTime)))
                    .OrderBy(alarm => alarm.StartTime);

                int rowcount=query.Count();
                var responseList =query.Skip((pageindex-1) * pagesize).Take(pagesize).ToList();

                List<JC_AlarmHandleInfo> alarmHandleModelList = ObjectConverter.CopyList<JC_AlarmHandleModel, JC_AlarmHandleInfo>(responseList).ToList();

                //var modellistresponse = _largeDataAnalysisConfigCacheService.GetAllLargeDataAnalysisConfigCache(new LargeDataAnalysisConfigCacheGetAllRequest());
                //逻辑报警分析模型从数据库获取。
                var modellist = _LargedataAnalysisConfigRepository.Datas.ToList();

                if (modellist != null && modellist.Any())
                {
                    alarmHandleModelList.ForEach(alarmhandle =>
                    {
                        if (alarmhandle.EndTime == null || alarmhandle.EndTime.ToString("yyyy-MM-dd HH:mm:ss") == "1900-01-01 00:00:00") 
                        {
                            alarmhandle.EtimeDisplay = "-";
                        }
                        else 
                        {
                            alarmhandle.EtimeDisplay = alarmhandle.EndTime.ToString("yyyy-MM-dd HH:mm:ss");
                        }

                        var analysismodel = modellist.FirstOrDefault(modle => modle.Id == alarmhandle.AnalysisModelId);
                        if (analysismodel != null)
                        {
                            alarmhandle.AnalysisModelName = analysismodel.Name;
                        }
                    });
                }

                alarmresponse.Data = alarmHandleModelList;
                alarmresponse.PagerInfo.PageIndex = pageindex;
                alarmresponse.PagerInfo.PageSize = pagesize;
                alarmresponse.PagerInfo.RowCount = rowcount;
                return alarmresponse;
            }
            catch (Exception ex)
            {
                LogHelper.Error("根据时间获取逻辑报警失败：" + "\r\n" + ex.Message);
                return new BasicResponse<List<JC_AlarmHandleInfo>>();
            }
        }

        /// <summary>
        /// 根据条件获取未结束的报警处理记录
        /// </summary>
        /// <param name="alarmHandelRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<JC_AlarmHandleNoEndInfo>> GetAlarmHandleNoEndListByCondition(AlarmHandleNoEndListByCondition alarmHandelRequest)
        {
            var response = new BasicResponse<List<JC_AlarmHandleNoEndInfo>>();
            if (!alarmHandelRequest.EndTime.HasValue && !string.IsNullOrWhiteSpace(alarmHandelRequest.PersonId))
            {
                response.Code = -100;
                response.Message = "参数错误！";
                return response;
            }
            try
            {
                var jC_AlarmHandleModelLists = _Repository.GetAlarmHandleNoEndListByCondition(alarmHandelRequest.StartTime, alarmHandelRequest.EndTime.Value, alarmHandelRequest.PersonId);
                var jC_AlarmHandleInfoLists = ObjectConverter.CopyList<JC_AlarmHandleNoEndModel, JC_AlarmHandleNoEndInfo>(jC_AlarmHandleModelLists);
                response.Data = jC_AlarmHandleInfoLists.ToList();
            }
            catch (Exception ex)
            {
                LogHelper.Error("根据条件获取未结束的报警处理记录：" + "\r\n" + ex.Message);
                response.Code = -100;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }


        /// <summary>
        /// 关闭未关闭的报警处理记录
        /// </summary>
        /// <returns>BasicResponse 对象</returns>
        public BasicResponse CloseUnclosedAlarmHandle(BasicRequest request)
        {
            _Repository.ExecuteNonQuery("CloseUnclosedAlarmHandle");
            return new BasicResponse();  
        }

        /// <summary>
        /// 关闭和分析模型有关的未关闭的报警处理记录
        /// </summary>
        /// <returns>BasicResponse 对象</returns>
        public BasicResponse CloseUnclosedAlarmHandleByAnalysisModelId(AlarmHandleGetByAnalysisModelIdRequest getByAnalysisModelIdRequest)
        {
            List<JC_AlarmHandleModel> alarmHandleModelList = _Repository.GetUnclosedAlarmListByAnalysisModelId(getByAnalysisModelIdRequest.AnalysisModelId);
            if(alarmHandleModelList != null && alarmHandleModelList.Count > 0)
            {
                foreach (var alarmHandleModel in alarmHandleModelList)
                {
                    alarmHandleModel.EndTime = DateTime.Now;
                }
                _Repository.Update(alarmHandleModelList);
            }
            return new BasicResponse();
        }
    }
}


