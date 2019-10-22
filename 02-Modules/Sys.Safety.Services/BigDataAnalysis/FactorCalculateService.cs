using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.FactorValue;
using Sys.Safety.Request.Jc_Hour;
using Sys.Safety.ServiceContract;
using Sys.Safety.ServiceContract.Cache;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Services
{
    public class FactorCalculateService : IFactorCalculateService
    {
        IPointDefineCacheService pointDefineCacheService = ServiceFactory.Create<IPointDefineCacheService>();
        IJc_HourService hourService = ServiceFactory.Create<IJc_HourService>();

        /// <summary>
        /// 报警上限值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>报警上限值</returns>
        public BasicResponse<FactorValueInfo> AlarmUpperValue(FactorValueGetRequest factorValueGetRequest)
        {
            var pointDefineCacheResponse = pointDefineCacheService.PointDefineCacheByPointIdRequeest(
                new PointDefineCacheByPointIdRequeest() { PointID = factorValueGetRequest.PointId }
                );

            var factorValueResponse = new BasicResponse<FactorValueInfo>();

            FactorValueInfo factorValueInfo = new FactorValueInfo();

            if (pointDefineCacheResponse.Code == 100)
            {
                if (pointDefineCacheResponse.Data != null)
                {
                    factorValueInfo.Value = pointDefineCacheResponse.Data.Z2.ToString();
                }
            }
            factorValueResponse.Data = factorValueInfo;
            factorValueResponse.Code = pointDefineCacheResponse.Code;
            factorValueResponse.Message = pointDefineCacheResponse.Message;


            return factorValueResponse;

        }
        /// <summary>
        /// 报警下限值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>报警下限值</returns>
        public BasicResponse<FactorValueInfo> AlarmLowerValue(FactorValueGetRequest factorValueGetRequest)
        {
            var pointDefineCacheResponse = pointDefineCacheService.PointDefineCacheByPointIdRequeest(
                new PointDefineCacheByPointIdRequeest() { PointID = factorValueGetRequest.PointId }
                );

            var factorValueResponse = new BasicResponse<FactorValueInfo>();

            FactorValueInfo factorValueInfo = new FactorValueInfo();

            if (pointDefineCacheResponse.Code == 100)
            {
                if (pointDefineCacheResponse.Data != null)
                {
                    factorValueInfo.Value = pointDefineCacheResponse.Data.Z6.ToString();
                }
            }
            factorValueResponse.Data = factorValueInfo;
            factorValueResponse.Code = pointDefineCacheResponse.Code;
            factorValueResponse.Message = pointDefineCacheResponse.Message;


            return factorValueResponse;
        }
        /// <summary>
        /// 日平均值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>日平均值</returns>
        public BasicResponse<FactorValueInfo> DayAverageValue(FactorValueGetRequest factorValueGetRequest)
        {
            var hourResponse = hourService.GetDayAverageValueByPointId(
                new Jc_HourGetRequest() { PointId = factorValueGetRequest.PointId }
                );

            var factorValueResponse = new BasicResponse<FactorValueInfo>();

            FactorValueInfo factorValueInfo = new FactorValueInfo();

            if (hourResponse.Code == 100)
            {
                if (hourResponse.Data != null)
                {
                    decimal tryParseValue = 0.00M;
                    decimal.TryParse(hourResponse.Data.CountDataValue, out tryParseValue);
                    if (tryParseValue > 0)
                        factorValueInfo.Value = hourResponse.Data.CountDataValue;
                }
            }

            factorValueResponse.Data = factorValueInfo;
            factorValueResponse.Code = hourResponse.Code;
            factorValueResponse.Message = hourResponse.Message;

            return factorValueResponse;
        }
        /// <summary>
        /// 日最大值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>日最大值</returns>
        public BasicResponse<FactorValueInfo> DayMaxValue(FactorValueGetRequest factorValueGetRequest)
        {
            var hourResponse = hourService.GetDayMaxValueByPointId(
               new Jc_HourGetRequest() { PointId = factorValueGetRequest.PointId }
               );

            var factorValueResponse = new BasicResponse<FactorValueInfo>();

            FactorValueInfo factorValueInfo = new FactorValueInfo();

            if (hourResponse.Code == 100)
            {
                if (hourResponse.Data != null)
                {
                    decimal tryParseValue = 0.00M;
                    decimal.TryParse(hourResponse.Data.CountDataValue, out tryParseValue);
                    if (tryParseValue > 0)
                        factorValueInfo.Value = hourResponse.Data.CountDataValue;
                }
            }
            factorValueResponse.Data = factorValueInfo;
            factorValueResponse.Code = hourResponse.Code;
            factorValueResponse.Message = hourResponse.Message;

            return factorValueResponse;
        }
        /// <summary>
        /// 月平均值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>月平均值</returns>
        public BasicResponse<FactorValueInfo> MonthAverageValue(FactorValueGetRequest factorValueGetRequest)
        {
            var hourResponse = hourService.GetMonthAverageValueByPointId(
              new Jc_HourGetRequest() { PointId = factorValueGetRequest.PointId }
              );

            var factorValueResponse = new BasicResponse<FactorValueInfo>();

            FactorValueInfo factorValueInfo = new FactorValueInfo();

            if (hourResponse.Code == 100)
            {
                if (hourResponse.Data != null)
                {
                    decimal tryParseValue = 0.00M;
                    decimal.TryParse(hourResponse.Data.CountDataValue, out tryParseValue);
                    if (tryParseValue > 0)
                        factorValueInfo.Value = hourResponse.Data.CountDataValue;
                }
            }
            factorValueResponse.Data = factorValueInfo;
            factorValueResponse.Code = hourResponse.Code;
            factorValueResponse.Message = hourResponse.Message;

            return factorValueResponse;
        }

        /// <summary>
        /// 周平均值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>周平均值</returns>
        public BasicResponse<FactorValueInfo> WeekAverageValue(FactorValueGetRequest factorValueGetRequest)
        {
            var hourResponse = hourService.GetWeekAverageValueByPointId(
              new Jc_HourGetRequest() { PointId = factorValueGetRequest.PointId }
              );

            var factorValueResponse = new BasicResponse<FactorValueInfo>();

            FactorValueInfo factorValueInfo = new FactorValueInfo();

            if (hourResponse.Code == 100)
            {
                if (hourResponse.Data != null)
                {
                    decimal tryParseValue = 0.00M;
                    decimal.TryParse(hourResponse.Data.CountDataValue, out tryParseValue);
                    if (tryParseValue > 0)
                        factorValueInfo.Value = hourResponse.Data.CountDataValue;
                }
            }
            factorValueResponse.Data = factorValueInfo;
            factorValueResponse.Code = hourResponse.Code;
            factorValueResponse.Message = hourResponse.Message;

            return factorValueResponse;
        }
        /// <summary>
        /// 五分钟平均值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>五分钟平均值</returns>
        public BasicResponse<FactorValueInfo> FiveMinutesAverageValue(FactorValueGetRequest factorValueGetRequest)
        {
            var pointDefineCacheResponse = pointDefineCacheService.PointDefineCacheByPointIdRequeest(
              new PointDefineCacheByPointIdRequeest() { PointID = factorValueGetRequest.PointId }
              );

            var factorValueResponse = new BasicResponse<FactorValueInfo>();

            FactorValueInfo factorValueInfo = new FactorValueInfo();

            if (pointDefineCacheResponse.Code == 100)
            {
                try
                {
                    factorValueInfo.Value = (
                        pointDefineCacheResponse.Data.ClsFiveMinObj.m_nAllVal /
                        pointDefineCacheResponse.Data.ClsFiveMinObj.m_nAllCount).ToString();

                }
                catch
                {
                    factorValueInfo.Value = "0";
                }
            }
            factorValueResponse.Data = factorValueInfo;
            factorValueResponse.Code = pointDefineCacheResponse.Code;
            factorValueResponse.Message = pointDefineCacheResponse.Message;


            return factorValueResponse;
        }
        /// <summary>
        /// 五分钟最大值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>五分钟最大值</returns>
        public BasicResponse<FactorValueInfo> FiveMinutesMaxValue(FactorValueGetRequest factorValueGetRequest)
        {
            var pointDefineCacheResponse = pointDefineCacheService.PointDefineCacheByPointIdRequeest(
           new PointDefineCacheByPointIdRequeest() { PointID = factorValueGetRequest.PointId }
           );

            var factorValueResponse = new BasicResponse<FactorValueInfo>();

            FactorValueInfo factorValueInfo = new FactorValueInfo();

            if (pointDefineCacheResponse.Code == 100)
            {
                try
                {
                    factorValueInfo.Value = pointDefineCacheResponse.Data.ClsFiveMinObj.m_nMaxVal.ToString();
                }
                catch
                {
                    factorValueInfo.Value = "0";
                }
            }
            factorValueResponse.Data = factorValueInfo;
            factorValueResponse.Code = pointDefineCacheResponse.Code;
            factorValueResponse.Message = pointDefineCacheResponse.Message;


            return factorValueResponse;
        }

        /// <summary>
        /// 开关量/模拟量实时值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>开关量/模拟量实时值</returns>
        public BasicResponse<FactorValueInfo> OnOffRealtimeValue(FactorValueGetRequest factorValueGetRequest)
        {
            var pointDefineCacheResponse = pointDefineCacheService.PointDefineCacheByPointIdRequeest(
           new PointDefineCacheByPointIdRequeest() { PointID = factorValueGetRequest.PointId }
           );

            var factorValueResponse = new BasicResponse<FactorValueInfo>();

            FactorValueInfo factorValueInfo = new FactorValueInfo();

            if (pointDefineCacheResponse.Code == 100)
            {
                if (pointDefineCacheResponse.Data != null)
                {
                    //(DevPropertyID: 1 模拟量 2 开关量)
                    if (pointDefineCacheResponse.Data.DevPropertyID == 1)
                    { //模拟量
                        switch (pointDefineCacheResponse.Data.State)
                        {
                            case 21:
                                factorValueInfo.Value = pointDefineCacheResponse.Data.Ssz;
                                break;
                            default:
                                break;
                        }
                    }
                    else if (pointDefineCacheResponse.Data.DevPropertyID == 2)
                    { //开关量
                        switch (pointDefineCacheResponse.Data.DataState)
                        {
                            case 25:
                                factorValueInfo.Value = "0";
                                break;
                            case 26:
                                factorValueInfo.Value = "1";
                                break;
                            case 27:
                                factorValueInfo.Value = "2";
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            factorValueResponse.Data = factorValueInfo;
            factorValueResponse.Code = pointDefineCacheResponse.Code;
            factorValueResponse.Message = pointDefineCacheResponse.Message;


            return factorValueResponse;
        }
        /// <summary>
        /// 断电上限值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>断电上限值</returns>
        public BasicResponse<FactorValueInfo> PowerOffUpperValue(FactorValueGetRequest factorValueGetRequest)
        {
            var pointDefineCacheResponse = pointDefineCacheService.PointDefineCacheByPointIdRequeest(
              new PointDefineCacheByPointIdRequeest() { PointID = factorValueGetRequest.PointId }
              );

            var factorValueResponse = new BasicResponse<FactorValueInfo>();

            FactorValueInfo factorValueInfo = new FactorValueInfo();

            if (pointDefineCacheResponse.Code == 100)
            {
                if (pointDefineCacheResponse.Data != null)
                    factorValueInfo.Value = pointDefineCacheResponse.Data.Z3.ToString();

            }
            factorValueResponse.Data = factorValueInfo;
            factorValueResponse.Code = pointDefineCacheResponse.Code;
            factorValueResponse.Message = pointDefineCacheResponse.Message;


            return factorValueResponse;
        }
        /// <summary>
        /// 断电下限值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>断电下限值</returns>
        public BasicResponse<FactorValueInfo> PowerOffLowerValue(FactorValueGetRequest factorValueGetRequest)
        {
            var pointDefineCacheResponse = pointDefineCacheService.PointDefineCacheByPointIdRequeest(
            new PointDefineCacheByPointIdRequeest() { PointID = factorValueGetRequest.PointId }
            );

            var factorValueResponse = new BasicResponse<FactorValueInfo>();

            FactorValueInfo factorValueInfo = new FactorValueInfo();

            if (pointDefineCacheResponse.Code == 100)
            {
                if (pointDefineCacheResponse.Data != null)
                    factorValueInfo.Value = pointDefineCacheResponse.Data.Z7.ToString();

            }
            factorValueResponse.Data = factorValueInfo;
            factorValueResponse.Code = pointDefineCacheResponse.Code;
            factorValueResponse.Message = pointDefineCacheResponse.Message;


            return factorValueResponse;
        }
        /// <summary>
        /// 预警上限值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>预警上限值</returns>
        public BasicResponse<FactorValueInfo> PrevAlarmUpperValue(FactorValueGetRequest factorValueGetRequest)
        {
            var pointDefineCacheResponse = pointDefineCacheService.PointDefineCacheByPointIdRequeest(
            new PointDefineCacheByPointIdRequeest() { PointID = factorValueGetRequest.PointId }
            );

            var factorValueResponse = new BasicResponse<FactorValueInfo>();

            FactorValueInfo factorValueInfo = new FactorValueInfo();

            if (pointDefineCacheResponse.Code == 100)
            {
                if (pointDefineCacheResponse.Data != null)
                    factorValueInfo.Value = pointDefineCacheResponse.Data.Z1.ToString();

            }
            factorValueResponse.Data = factorValueInfo;
            factorValueResponse.Code = pointDefineCacheResponse.Code;
            factorValueResponse.Message = pointDefineCacheResponse.Message;


            return factorValueResponse;
        }
        /// <summary>
        /// 预警下限值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>预警下限值</returns>
        public BasicResponse<FactorValueInfo> PrevAlarmLowerValue(FactorValueGetRequest factorValueGetRequest)
        {
            var pointDefineCacheResponse = pointDefineCacheService.PointDefineCacheByPointIdRequeest(
           new PointDefineCacheByPointIdRequeest() { PointID = factorValueGetRequest.PointId }
           );

            var factorValueResponse = new BasicResponse<FactorValueInfo>();

            FactorValueInfo factorValueInfo = new FactorValueInfo();

            if (pointDefineCacheResponse.Code == 100)
            {
                if (pointDefineCacheResponse.Data != null)
                    factorValueInfo.Value = pointDefineCacheResponse.Data.Z5.ToString();

            }
            factorValueResponse.Data = factorValueInfo;
            factorValueResponse.Code = pointDefineCacheResponse.Code;
            factorValueResponse.Message = pointDefineCacheResponse.Message;


            return factorValueResponse;
        }
        /// <summary>
        /// 复电上限值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>复电上限值</returns>
        public BasicResponse<FactorValueInfo> RowerOnUpperValue(FactorValueGetRequest factorValueGetRequest)
        {
            var pointDefineCacheResponse = pointDefineCacheService.PointDefineCacheByPointIdRequeest(
            new PointDefineCacheByPointIdRequeest() { PointID = factorValueGetRequest.PointId }
            );

            var factorValueResponse = new BasicResponse<FactorValueInfo>();

            FactorValueInfo factorValueInfo = new FactorValueInfo();

            if (pointDefineCacheResponse.Code == 100)
            {
                if (pointDefineCacheResponse.Data != null)
                    factorValueInfo.Value = pointDefineCacheResponse.Data.Z4.ToString();

            }
            factorValueResponse.Data = factorValueInfo;
            factorValueResponse.Code = pointDefineCacheResponse.Code;
            factorValueResponse.Message = pointDefineCacheResponse.Message;


            return factorValueResponse;
        }
        /// <summary>
        /// 复电下限值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>复电下限值</returns>
        public BasicResponse<FactorValueInfo> RowerOnLowerValue(FactorValueGetRequest factorValueGetRequest)
        {
            var pointDefineCacheResponse = pointDefineCacheService.PointDefineCacheByPointIdRequeest(
            new PointDefineCacheByPointIdRequeest() { PointID = factorValueGetRequest.PointId }
            );

            var factorValueResponse = new BasicResponse<FactorValueInfo>();

            FactorValueInfo factorValueInfo = new FactorValueInfo();

            if (pointDefineCacheResponse.Code == 100)
            {
                if (pointDefineCacheResponse.Data != null)
                    factorValueInfo.Value = pointDefineCacheResponse.Data.Z8.ToString();

            }
            factorValueResponse.Data = factorValueInfo;
            factorValueResponse.Code = pointDefineCacheResponse.Code;
            factorValueResponse.Message = pointDefineCacheResponse.Message;


            return factorValueResponse;
        }


    }
}
