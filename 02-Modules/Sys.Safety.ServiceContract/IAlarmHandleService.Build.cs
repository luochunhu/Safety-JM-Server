using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.AlarmHandle;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IAlarmHandleService
    {
        BasicResponse<JC_AlarmHandleInfo> AddJC_AlarmHandle(AlarmHandleAddRequest jC_AlarmHandlerequest);
        BasicResponse<JC_AlarmHandleInfo> UpdateJC_AlarmHandle(AlarmHandleUpdateRequest jC_AlarmHandlerequest);
        BasicResponse DeleteJC_AlarmHandle(AlarmHandleDeleteRequest jC_AlarmHandlerequest);
        BasicResponse<List<JC_AlarmHandleInfo>> GetJC_AlarmHandleList(AlarmHandleGetListRequest jC_AlarmHandlerequest);
        BasicResponse<JC_AlarmHandleInfo> GetJC_AlarmHandleById(AlarmHandleGetRequest jC_AlarmHandlerequest);
        /// <summary>
        /// 获取未关闭的报警列表
        /// </summary>
        /// <param name="alarmHandleWithoutSearchConditionRequest">空条件请求</param>
        /// <returns>未关闭的报警列表</returns>
        BasicResponse<List<JC_AlarmHandleInfo>> GetUnclosedAlarmList(AlarmHandleWithoutSearchConditionRequest alarmHandleWithoutSearchConditionRequest);
        /// <summary>
        /// 获取和分析模型有关的未关闭报警
        /// </summary>
        /// <param name="alarmHandleGetByAnalysisModelIdRequest">模型Id为条件的请求</param>
        /// <returns>分析模型有关的未关闭报警</returns>
        BasicResponse<JC_AlarmHandleInfo> GetUnclosedAlarmByAnalysisModelId(AlarmHandleGetByAnalysisModelIdRequest alarmHandleGetByAnalysisModelIdRequest);
        /// <summary>
        /// 获取未处理的报警列表
        /// </summary>
        /// <param name="alarmHandleWithoutSearchConditionRequest">空条件请求</param>
        /// <returns>未处理的报警列表</returns>
        BasicResponse<List<JC_AlarmHandleInfo>> GetUnhandledAlarmList(AlarmHandleWithoutSearchConditionRequest alarmHandleWithoutSearchConditionRequest);

        /// <summary>
        /// 获取和分析模型有关的未处理报警
        /// </summary>
        /// <param name="alarmHandleGetByAnalysisModelIdRequest">模型Id为条件的请求</param>
        /// <returns>分析模型有关的未处理报警</returns>
        BasicResponse<JC_AlarmHandleInfo> GetUnhandledAlarmByAnalysisModelId(AlarmHandleGetByAnalysisModelIdRequest alarmHandleGetByAnalysisModelIdRequest);
        /// <summary>
        /// 批量更新报警信息
        /// </summary>
        /// <param name="alarmHandleUpdateListRequest">批量更新报警信息请求</param>
        /// <returns>更新后的报警信息列表</returns>
        BasicResponse<List<JC_AlarmHandleInfo>> UpdateAlarmHandleList(AlarmHandleUpdateListRequest alarmHandleUpdateListRequest);

        /// <summary>
        /// 根据开始时间和结束时间获取报警列表
        /// </summary>
        /// <param name="alarmHandelRequest"></param>
        /// <returns></returns>
        BasicResponse<List<JC_AlarmHandleInfo>> GetAlarmHandleByStimeAndEtime(AlarmHandelGetByStimeAndETime alarmHandelRequest);

        /// <summary>
        /// 根据条件获取未结束的报警处理记录
        /// </summary>
        /// <param name="alarmHandelRequest"></param>
        /// <returns></returns>
        BasicResponse<List<JC_AlarmHandleNoEndInfo>> GetAlarmHandleNoEndListByCondition(AlarmHandleNoEndListByCondition alarmHandelRequest);

        /// <summary>
        /// 关闭未关闭的报警处理记录
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <returns>BasicResponse 对象</returns>
        BasicResponse CloseUnclosedAlarmHandle(BasicRequest request);
        /// <summary>
        /// 关闭和分析模型有关的未关闭的报警处理记录
        /// </summary>
        /// <param name="getByAnalysisModelIdRequest"></param>
        /// <returns></returns>
        BasicResponse CloseUnclosedAlarmHandleByAnalysisModelId(AlarmHandleGetByAnalysisModelIdRequest getByAnalysisModelIdRequest);
    }
}

