using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Jc_Hour;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IJc_HourService
    {
        BasicResponse<Jc_HourInfo> AddJc_Hour(Jc_HourAddRequest jc_Hourrequest);
        BasicResponse<Jc_HourInfo> UpdateJc_Hour(Jc_HourUpdateRequest jc_Hourrequest);
        BasicResponse DeleteJc_Hour(Jc_HourDeleteRequest jc_Hourrequest);
        BasicResponse<List<Jc_HourInfo>> GetJc_HourList(Jc_HourGetListRequest jc_Hourrequest);
        BasicResponse<Jc_HourInfo> GetJc_HourById(Jc_HourGetRequest jc_Hourrequest);
        /// <summary>
        /// 查询日最大值
        /// </summary>
        /// <param name="jc_Hourrequest"></param>
        /// <returns></returns>
        BasicResponse<Jc_HourInfo> GetDayMaxValueByPointId(Jc_HourGetRequest jc_Hourrequest);
        /// <summary>
        /// 查询日平均值
        /// </summary>
        /// <param name="jc_Hourrequest"></param>
        /// <returns></returns>
        BasicResponse<Jc_HourInfo> GetDayAverageValueByPointId(Jc_HourGetRequest jc_Hourrequest);
        /// <summary>
        /// 查询月平均值
        /// </summary>
        /// <param name="jc_Hourrequest"></param>
        /// <returns></returns>
        BasicResponse<Jc_HourInfo> GetMonthAverageValueByPointId(Jc_HourGetRequest jc_Hourrequest);

        /// <summary>
        /// 查询周平均值
        /// </summary>
        /// <param name="jc_Hourrequest"></param>
        /// <returns></returns>
        BasicResponse<Jc_HourInfo> GetWeekAverageValueByPointId(Jc_HourGetRequest jc_Hourrequest);

        /// <summary>
        /// 获取所有模拟量历史数据(如月平均值、周平均值、日最大值、日平均值、5分钟最大值、5分钟平均值)
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<DataAnalysisHistoryDataInfo>> GetDataAnalysisHistoryData(BasicRequest request);
    }
}

