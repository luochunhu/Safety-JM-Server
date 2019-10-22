using Basic.Framework.Web;
using Sys.Safety.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.ServiceContract
{
    public interface ILargeDataAnalysisLastChangedService
    {
        /// <summary>
        /// 获取分析模型最后修改的时间
        /// </summary>
        /// <param name="lastChangedRequest">request</param>
        /// <returns>最后修改的时间(yyyy-MM-dd HH:mm:ss)</returns>
        BasicResponse<string> GetAnalysisModelLastChangedTime(LargeDataAnalysisLastChangedRequest lastChangedRequest);

        /// <summary>
        /// 获取测点最后修改时间
        /// </summary>
        /// <param name="lastChangedRequest">request</param>
        /// <returns>最后修改的时间(yyyy-MM-dd HH:mm:ss)</returns>
        BasicResponse<string> GetPointDefineLastChangedTime(LargeDataAnalysisLastChangedRequest lastChangedRequest);

        /// <summary>
        /// 获取区域断电最后修改时间
        /// </summary>
        /// <param name="lastChangedRequest">request</param>
        /// <returns>最后修改的时间(yyyy-MM-dd HH:mm:ss)</returns>
        BasicResponse<string> GetRegionOutageLastChangedTime(LargeDataAnalysisLastChangedRequest lastChangedRequest);

        /// <summary>
        /// 获取应急联动最后修改时间
        /// </summary>
        /// <param name="lastChangedRequest">request</param>
        /// <returns>最后修改的时间(yyyy-MM-dd HH:mm:ss)</returns>
        BasicResponse<string> GetEmergencyLinkageLastChangedTime(LargeDataAnalysisLastChangedRequest lastChangedRequest);

        /// <summary>
        /// 获取报警通知最后修改时间
        /// </summary>
        /// <param name="lastChangedRequest">request</param>
        /// <returns>最后修改的时间(yyyy-MM-dd HH:mm:ss)</returns>
        BasicResponse<string> GetAlarmNotificationLastChangedTime(LargeDataAnalysisLastChangedRequest lastChangedRequest);
    }
}
