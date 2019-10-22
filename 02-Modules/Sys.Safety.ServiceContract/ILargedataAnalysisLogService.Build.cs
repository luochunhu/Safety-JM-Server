using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.JC_Largedataanalysislog;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface ILargedataAnalysisLogService
    {
        BasicResponse<JC_LargedataAnalysisLogInfo> AddJC_Largedataanalysislog(LargedataAnalysisLogAddRequest largedataanalysislogrequest);
        BasicResponse<JC_LargedataAnalysisLogInfo> UpdateJC_Largedataanalysislog(LargedataAnalysisLogUpdateRequest largedataanalysislogrequest);
        BasicResponse DeleteJC_Largedataanalysislog(LargedataanalysislogDeleteRequest largedataanalysislogrequest);
        BasicResponse<List<JC_LargedataAnalysisLogInfo>> GetJC_LargedataanalysislogList(LargedataAnalysisLogGetListRequest largedataanalysislogrequest);
        BasicResponse<JC_LargedataAnalysisLogInfo> GetJC_LargedataanalysislogById(LargedataAnalysisLogGetRequest largedataanalysislogrequest);

        /// <summary>
        ///  获取分析模型的分析日志
        /// </summary>
        /// <param name="largedataAnalysisLogGetListRequest">获取分析模型的分析日志请求对象</param>
        /// <returns>分析模型的分析日志列表</returns>
        BasicResponse<List<JC_LargedataAnalysisLogInfo>> GetLargedataAnalysisLogListByAnalysisModelId(LargedataAnalysisLogGetByAnalysisModelIdRequest largedataAnalysisLogGetByAnalysisModelIdRequest);

        /// <summary>
        /// 获取最近一条分析模型的分析日志
        /// </summary>
        /// <param name="largedataAnalysisLogGetByAnalysisModelIdRequest">获取分析模型的分析日志请求对象</param>
        /// <returns>最近一条分析模型的分析日志</returns>
        BasicResponse<JC_LargedataAnalysisLogInfo> GetLargedataAnalysisLogLatestByAnalysisModelId(LargedataAnalysisLogGetByAnalysisModelIdRequest largedataAnalysisLogGetByAnalysisModelIdRequest);

        /// <summary>
        /// 查询分析日志列表
        /// </summary>
        /// <param name="LargedataAnalysisLogGetListByModelIdAndTimeRequest">模型Id和分析时间</param>
        /// <returns>分析日志列表</returns>
        BasicResponse<List<JC_LargedataAnalysisLogInfo>> GetLargedataAnalysisLogListByModelIdAndTime(LargedataAnalysisLogGetListByModelIdAndTimeRequest largedataAnalysisLogGetListByModelIdAndTimeRequest);
    }
}

