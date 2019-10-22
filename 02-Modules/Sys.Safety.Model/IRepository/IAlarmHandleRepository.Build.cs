using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IAlarmHandleRepository : IRepository<JC_AlarmHandleModel>
    {
        JC_AlarmHandleModel AddJC_AlarmHandle(JC_AlarmHandleModel jC_AlarmHandleModel);
        void UpdateJC_AlarmHandle(JC_AlarmHandleModel jC_AlarmHandleModel);
        void DeleteJC_AlarmHandle(string id);
        IList<JC_AlarmHandleModel> GetJC_AlarmHandleList(int pageIndex, int pageSize, out int rowCount);
        JC_AlarmHandleModel GetJC_AlarmHandleById(string id);

        /// <summary>
        /// 获取未关闭的报警列表
        /// </summary>
        /// <returns>未关闭的报警列表</returns>
        List<JC_AlarmHandleModel> GetUnclosedAlarmList();
        /// <summary>
        /// 获取和分析模型有关的未关闭报警
        /// </summary>
        /// <param name="analysisModelId">分析模型Id</param>
        /// <returns>分析模型有关的未关闭报警</returns>
        JC_AlarmHandleModel GetUnclosedAlarmByAnalysisModelId(string analysisModelId);
        /// <summary>
        /// 获取和分析模型有关的未关闭报警列表
        /// </summary>
        /// <param name="analysisModelId">分析模型Id</param>
        /// <returns>分析模型有关的未关闭报警列表</returns>
        List<JC_AlarmHandleModel> GetUnclosedAlarmListByAnalysisModelId(string analysisModelId);
        /// <summary>
        /// 获取未处理的报警列表
        /// </summary>
        /// <returns>未处理的报警列表</returns>
        List<JC_AlarmHandleModel> GetUnhandledAlarmList();

        /// <summary>
        /// 获取和分析模型有关的未处理报警
        /// </summary>
        /// <param name="analysisModelId">分析模型Id</param>
        /// <returns>分析模型有关的未处理报警</returns>
        JC_AlarmHandleModel GetUnhandledAlarmByAnalysisModelId(string analysisModelId);
        /// <summary>
        /// 批量更新报警信息
        /// </summary>
        /// <param name="alarmHandleModelList">报警处理列表</param>
        void UpdateAlarmHandleList(IList<JC_AlarmHandleModel> alarmHandleModelList);

        /// <summary>
        /// 根据条件获取未结束的报警处理记录
        /// </summary>
        /// <param name="searchTime"></param>
        /// <param name="maxStartTime"></param>
        /// <returns></returns>
        List<JC_AlarmHandleNoEndModel> GetAlarmHandleNoEndListByCondition(string startTime, DateTime? endTime, string personId);
    }
}
