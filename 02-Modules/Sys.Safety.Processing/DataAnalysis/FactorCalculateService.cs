using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Cache.BigDataAnalysis;
using Sys.Safety.ServiceContract;
using System.Linq;

namespace Sys.Safety.Processing.DataAnalysis
{
    public class FactorCalculateService
    {
        IJc_HourService hourService = ServiceFactory.Create<IJc_HourService>();

        #region 单例
        private volatile static FactorCalculateService _instance = null;
        private static readonly object lockHelper = new object();
        private FactorCalculateService() { }

        public static FactorCalculateService CreateService()
        {
            if (_instance == null)
            {
                lock (lockHelper)
                {
                    if (_instance == null)
                        _instance = new FactorCalculateService();
                }
            }
            return _instance;
        }
        #endregion

        /// <summary>
        /// 报警上限值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>报警上限值</returns>
        public FactorValueInfo AlarmUpperValue(string pointId)
        {
            FactorValueInfo factorValueInfo = new FactorValueInfo();
            Jc_DefInfo pointDefine = PointCache.Instance.Query(q => q.PointID == pointId, false).FirstOrDefault();
            if (pointDefine != null)
            {
                factorValueInfo.Value = pointDefine.Z2.ToString();
            }
            return factorValueInfo;

        }
        /// <summary>
        /// 报警下限值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>报警下限值</returns>
        public FactorValueInfo AlarmLowerValue(string pointId)
        {
            FactorValueInfo factorValueInfo = new FactorValueInfo();
            Jc_DefInfo pointDefine = PointCache.Instance.Query(q => q.PointID == pointId, false).FirstOrDefault();
            if (pointDefine != null)
            {
                factorValueInfo.Value = pointDefine.Z6.ToString();
            }
            return factorValueInfo;
        }
        /// <summary>
        /// 日平均值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>日平均值</returns>
        public FactorValueInfo DayAverageValue(string pointId)
        {
            FactorValueInfo factorValueInfo = new FactorValueInfo();
            var historyItem = HistoryDataCache.Instance.Query(q => q.PointId == pointId, false).FirstOrDefault();
            if (historyItem != null)
            {
                factorValueInfo.Value = historyItem.DayAverageValue.ToString();
            }
            return factorValueInfo;
        }
        /// <summary>
        /// 日最大值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>日最大值</returns>
        public FactorValueInfo DayMaxValue(string pointId)
        {
            FactorValueInfo factorValueInfo = new FactorValueInfo();
            var historyItem = HistoryDataCache.Instance.Query(q => q.PointId == pointId, false).FirstOrDefault();
            if (historyItem != null)
            {
                factorValueInfo.Value = historyItem.DayMaxValue.ToString();
            }
            return factorValueInfo;
        }
        /// <summary>
        /// 月平均值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>月平均值</returns>
        public FactorValueInfo MonthAverageValue(string pointId)
        {
            FactorValueInfo factorValueInfo = new FactorValueInfo();
            var historyItem = HistoryDataCache.Instance.Query(q => q.PointId == pointId, false).FirstOrDefault();
            if (historyItem != null)
            {
                factorValueInfo.Value = historyItem.MonthAverageValue.ToString();
            }
            return factorValueInfo;
        }

        /// <summary>
        /// 周平均值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>周平均值</returns>
        public FactorValueInfo WeekAverageValue(string pointId)
        {
            FactorValueInfo factorValueInfo = new FactorValueInfo();
            var historyItem = HistoryDataCache.Instance.Query(q => q.PointId == pointId, false).FirstOrDefault();
            if (historyItem != null)
            {
                factorValueInfo.Value = historyItem.WeekAverageValue.ToString();
            }
            return factorValueInfo;
        }
        /// <summary>
        /// 五分钟平均值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>五分钟平均值</returns>
        public FactorValueInfo FiveMinutesAverageValue(string pointId)
        {
            FactorValueInfo factorValueInfo = new FactorValueInfo();
            var historyItem = HistoryDataCache.Instance.Query(q => q.PointId == pointId, false).FirstOrDefault();
            if (historyItem != null)
            {
                factorValueInfo.Value = historyItem.FiveMinutesAverageValue.ToString();
            }
            return factorValueInfo;
        }
        /// <summary>
        /// 五分钟最大值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>五分钟最大值</returns>
        public FactorValueInfo FiveMinutesMaxValue(string pointId)
        {
            FactorValueInfo factorValueInfo = new FactorValueInfo();
            var historyItem = HistoryDataCache.Instance.Query(q => q.PointId == pointId, false).FirstOrDefault();
            if (historyItem != null)
            {
                factorValueInfo.Value = historyItem.FiveMinutesMaxValue.ToString();
            }
            return factorValueInfo;
        }

        /// <summary>
        /// 开关量/模拟量实时值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>开关量/模拟量实时值</returns>
        public FactorValueInfo OnOffRealtimeValue(string pointId)
        {
            FactorValueInfo factorValueInfo = new FactorValueInfo();
            Jc_DefInfo pointDefine = PointCache.Instance.Query(q => q.PointID == pointId, false).FirstOrDefault();
            if (pointDefine != null)
            {
                //(DevPropertyID: 1 模拟量 2 开关量)
                if (pointDefine.DevPropertyID == 1)
                { //模拟量
                    switch (pointDefine.State)
                    {
                        //case 5://红外遥控模拟数据时，也按正常处理  20171219
                        case 21:
                            factorValueInfo.Value = pointDefine.Ssz;
                            break;
                        default:
                            break;
                    }
                }
                else if (pointDefine.DevPropertyID == 2)
                { //开关量
                    switch (pointDefine.DataState)
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
            return factorValueInfo;
        }
        /// <summary>
        /// 断电上限值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>断电上限值</returns>
        public FactorValueInfo PowerOffUpperValue(string pointId)
        {
            FactorValueInfo factorValueInfo = new FactorValueInfo();
            Jc_DefInfo pointDefine = PointCache.Instance.Query(q => q.PointID == pointId, false).FirstOrDefault();
            if (pointDefine != null)
            {
                factorValueInfo.Value = pointDefine.Z3.ToString();
            }
            return factorValueInfo;
        }
        /// <summary>
        /// 断电下限值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>断电下限值</returns>
        public FactorValueInfo PowerOffLowerValue(string pointId)
        {
            FactorValueInfo factorValueInfo = new FactorValueInfo();
            Jc_DefInfo pointDefine = PointCache.Instance.Query(q => q.PointID == pointId, false).FirstOrDefault();
            if (pointDefine != null)
            {
                factorValueInfo.Value = pointDefine.Z7.ToString();
            }
            return factorValueInfo;
        }
        /// <summary>
        /// 预警上限值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>预警上限值</returns>
        public FactorValueInfo PrevAlarmUpperValue(string pointId)
        {
            FactorValueInfo factorValueInfo = new FactorValueInfo();
            Jc_DefInfo pointDefine = PointCache.Instance.Query(q => q.PointID == pointId, false).FirstOrDefault();
            if (pointDefine != null)
            {
                factorValueInfo.Value = pointDefine.Z1.ToString();
            }
            return factorValueInfo;
        }
        /// <summary>
        /// 预警下限值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>预警下限值</returns>
        public FactorValueInfo PrevAlarmLowerValue(string pointId)
        {
            FactorValueInfo factorValueInfo = new FactorValueInfo();
            Jc_DefInfo pointDefine = PointCache.Instance.Query(q => q.PointID == pointId, false).FirstOrDefault();
            if (pointDefine != null)
            {
                factorValueInfo.Value = pointDefine.Z5.ToString();
            }
            return factorValueInfo;
        }
        /// <summary>
        /// 复电上限值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>复电上限值</returns>
        public FactorValueInfo RowerOnUpperValue(string pointId)
        {
            FactorValueInfo factorValueInfo = new FactorValueInfo();
            Jc_DefInfo pointDefine = PointCache.Instance.Query(q => q.PointID == pointId, false).FirstOrDefault();
            if (pointDefine != null)
            {
                factorValueInfo.Value = pointDefine.Z4.ToString();
            }
            return factorValueInfo;
        }
        /// <summary>
        /// 复电下限值
        /// </summary>
        /// <param name="pointId">测点号</param>
        /// <returns>复电下限值</returns>
        public FactorValueInfo RowerOnLowerValue(string pointId)
        {
            FactorValueInfo factorValueInfo = new FactorValueInfo();
            Jc_DefInfo pointDefine = PointCache.Instance.Query(q => q.PointID == pointId, false).FirstOrDefault();
            if (pointDefine != null)
            {
                factorValueInfo.Value = pointDefine.Z8.ToString();
            }
            return factorValueInfo;
        }
    }
}
