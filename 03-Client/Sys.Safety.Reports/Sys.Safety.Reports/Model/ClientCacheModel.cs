using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Basic.Framework.Service;
using Sys.Safety.Request.Listex;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Reports.Model
{
    public class ClientCacheModel
    {
        private static IDictionary<string, DataTable> dicCache = new Dictionary<string, DataTable>();
        // private static IListExService service = ServiceFactory.CreateService<IListExService>();
        private static readonly IListexService _service = ServiceFactory.Create<IListexService>();


        /// <summary>
        ///     得到元数据缓存
        /// </summary>
        /// <returns></returns>
        public static DataTable GetServerMetaData()
        {
            var ret = _service.GetServerMetaData();
            if (!ret.IsSuccess)
                throw new Exception(ret.Message);
            return ret.Data;
        }

        /// <summary>
        ///     得到元数据字段缓存
        /// </summary>
        /// <returns></returns>
        public static DataTable GetServerMetaDataFields()
        {
            var ret = _service.GetServerMetaDataFields();
            if (!ret.IsSuccess)
                throw new Exception(ret.Message);
            return ret.Data;
        }


        /// <summary>
        ///     得到元数据字段缓存
        /// </summary>
        /// <returns></returns>
        public static DataTable GetServerMetaData(int ID)
        {
            var request = new IdRequest
            {
                Id = ID
            };
            var ret = _service.GetServerMetaData(request);
            if (!ret.IsSuccess)
                throw new Exception(ret.Message);
            return ret.Data;
        }

        /// <summary>
        ///     更新元数据缓存
        /// </summary>
        /// <returns></returns>
        public static void UpdateMetaDataCache()
        {
            var ret = _service.UpdateMetaDataCache();
            if (!ret.IsSuccess)
                throw new Exception(ret.Message);
        }

        /// <summary>
        ///     得到请求库缓存
        /// </summary>
        /// <returns></returns>
        public static DataTable GetServerRequest()
        {
            var ret = _service.GetServerRequest();
            if (!ret.IsSuccess)
                throw new Exception(ret.Message);
            return ret.Data;
        }


        //判断是否有字段权限,该方法未实现
        public static bool IsHaveFieldRigth(int metadatafieldid)
        {
            var blnHaveRight = false;
            try
            {
                var request = new IdRequest
                {
                    Id = 0
                };
                var ret = _service.GetFieldRights(request);
                if (!ret.IsSuccess)
                    throw new Exception(ret.Message);
                var dt = ret.Data;
                blnHaveRight = dt.Select("metadatafieldid=" + metadatafieldid).Length > 0 ? true : false;
            }
            catch
            {
            }

            return blnHaveRight;
        }


        public static IDictionary GetClientParameters()
        {
            IDictionary content = new Hashtable();
            content.Add("Today", DateTimeManager.GetToday()); //今天
            content.Add("ThisWeek", DateTimeManager.GetThisWeek()); //本周
            content.Add("ThisMonth", DateTimeManager.GetThisMonth()); //本月
            content.Add("ThisSeason", DateTimeManager.GetThisSeason()); //本季度
            content.Add("ThisYear", DateTimeManager.GetThisYear()); //本年
            content.Add("ThisWeekToToday", DateTimeManager.GetThisWeekToToday()); //本周至今日
            content.Add("ThisMonthToToday", DateTimeManager.GetThisMonthToToday()); //本月至今日
            content.Add("ThisSeasonToToday", DateTimeManager.GetThisSeasonToToday()); //本季度至今日
            content.Add("ThisYearToToday", DateTimeManager.GetThisYearToToday()); //本年至今日
            content.Add("LastWeek", DateTimeManager.GetLastWeek()); //上周
            content.Add("LastMonth", DateTimeManager.GetLastMonth()); //上月
            content.Add("LastSeason", DateTimeManager.GetLastSeason()); //上季度
            content.Add("LastYear", DateTimeManager.GetLastYear()); //上年
            content.Add("LastestWeek", DateTimeManager.GetLastestWeek()); //最近一周
            content.Add("LastestMonth", DateTimeManager.GetLastestMonth()); //最近一月
            content.Add("LastestSeason", DateTimeManager.GetLastestSeason()); //最近一季度
            content.Add("LastestYear", DateTimeManager.GetLastestYear()); //最近一年
            return content;
        }
    }
}