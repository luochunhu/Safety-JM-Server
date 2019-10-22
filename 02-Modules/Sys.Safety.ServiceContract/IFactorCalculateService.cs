using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.FactorValue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.ServiceContract
{
    public interface IFactorCalculateService
    {
    
        /// <summary>
        /// 五分钟最大值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>五分钟最大值</returns>
        BasicResponse<FactorValueInfo> FiveMinutesMaxValue(FactorValueGetRequest factorValueGetRequest);
        /// <summary>
        /// 五分钟平均值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>五分钟平均值</returns>
        BasicResponse<FactorValueInfo> FiveMinutesAverageValue(FactorValueGetRequest factorValueGetRequest);
        /// <summary>
        /// 日最大值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>日最大值</returns>
        BasicResponse<FactorValueInfo> DayMaxValue(FactorValueGetRequest factorValueGetRequest);
        /// <summary>
        /// 日平均值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>日平均值</returns>
        BasicResponse<FactorValueInfo> DayAverageValue(FactorValueGetRequest factorValueGetRequest);
        /// <summary>
        /// 月平均值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>月平均值</returns>
        BasicResponse<FactorValueInfo> MonthAverageValue(FactorValueGetRequest factorValueGetRequest);
        /// <summary>
        /// 周平均值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>周平均值</returns>
        BasicResponse<FactorValueInfo> WeekAverageValue(FactorValueGetRequest factorValueGetRequest);
        /// <summary>
        /// 预警上限值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>预警上限值</returns>
        BasicResponse<FactorValueInfo> PrevAlarmUpperValue(FactorValueGetRequest factorValueGetRequest);
        /// <summary>
        /// 预警下限值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>预警下限值</returns>
        BasicResponse<FactorValueInfo> PrevAlarmLowerValue(FactorValueGetRequest factorValueGetRequest);
        /// <summary>
        /// 报警上限值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>报警上限值</returns>
        BasicResponse<FactorValueInfo> AlarmUpperValue(FactorValueGetRequest factorValueGetRequest);
        /// <summary>
        /// 报警下限值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>报警下限值</returns>
        BasicResponse<FactorValueInfo> AlarmLowerValue(FactorValueGetRequest factorValueGetRequest);
        /// <summary>
        /// 断电上限值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>断电上限值</returns>
        BasicResponse<FactorValueInfo> PowerOffUpperValue(FactorValueGetRequest factorValueGetRequest);
        /// <summary>
        /// 断电下限值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>断电下限值</returns>
        BasicResponse<FactorValueInfo> PowerOffLowerValue(FactorValueGetRequest factorValueGetRequest);
        /// <summary>
        /// 复电上限值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>复电上限值</returns>
        BasicResponse<FactorValueInfo> RowerOnUpperValue(FactorValueGetRequest factorValueGetRequest);
        /// <summary>
        /// 复电下限值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>复电下限值</returns>
        BasicResponse<FactorValueInfo> RowerOnLowerValue(FactorValueGetRequest factorValueGetRequest);
        /// <summary>
        /// 开关量/模拟量实时值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>开关量/模拟量实时值</returns>
        BasicResponse<FactorValueInfo> OnOffRealtimeValue(FactorValueGetRequest factorValueGetRequest);
    }
}
