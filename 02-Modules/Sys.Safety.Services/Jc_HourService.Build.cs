using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Jc_Hour;
using Basic.Framework.Common;
using Basic.Framework.Web;
using System.Data;

namespace Sys.Safety.Services
{
    public partial class Jc_HourService : IJc_HourService
    {
        private IJc_HourRepository _Repository;

        public Jc_HourService(IJc_HourRepository _Repository)
        {
            this._Repository = _Repository;
        }
        public BasicResponse<Jc_HourInfo> AddJc_Hour(Jc_HourAddRequest jc_Hourrequest)
        {
            var _jc_Hour = ObjectConverter.Copy<Jc_HourInfo, Jc_HourModel>(jc_Hourrequest.Jc_HourInfo);
            var resultjc_Hour = _Repository.AddJc_Hour(_jc_Hour);
            var jc_Hourresponse = new BasicResponse<Jc_HourInfo>();
            jc_Hourresponse.Data = ObjectConverter.Copy<Jc_HourModel, Jc_HourInfo>(resultjc_Hour);
            return jc_Hourresponse;
        }
        public BasicResponse<Jc_HourInfo> UpdateJc_Hour(Jc_HourUpdateRequest jc_Hourrequest)
        {
            var _jc_Hour = ObjectConverter.Copy<Jc_HourInfo, Jc_HourModel>(jc_Hourrequest.Jc_HourInfo);
            _Repository.UpdateJc_Hour(_jc_Hour);
            var jc_Hourresponse = new BasicResponse<Jc_HourInfo>();
            jc_Hourresponse.Data = ObjectConverter.Copy<Jc_HourModel, Jc_HourInfo>(_jc_Hour);
            return jc_Hourresponse;
        }
        public BasicResponse DeleteJc_Hour(Jc_HourDeleteRequest jc_Hourrequest)
        {
            _Repository.DeleteJc_Hour(jc_Hourrequest.Id);
            var jc_Hourresponse = new BasicResponse();
            return jc_Hourresponse;
        }
        public BasicResponse<List<Jc_HourInfo>> GetJc_HourList(Jc_HourGetListRequest jc_Hourrequest)
        {
            var jc_Hourresponse = new BasicResponse<List<Jc_HourInfo>>();
            jc_Hourrequest.PagerInfo.PageIndex = jc_Hourrequest.PagerInfo.PageIndex - 1;
            if (jc_Hourrequest.PagerInfo.PageIndex < 0)
            {
                jc_Hourrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jc_HourModelLists = _Repository.GetJc_HourList(jc_Hourrequest.PagerInfo.PageIndex, jc_Hourrequest.PagerInfo.PageSize, out rowcount);
            var jc_HourInfoLists = new List<Jc_HourInfo>();
            foreach (var item in jc_HourModelLists)
            {
                var Jc_HourInfo = ObjectConverter.Copy<Jc_HourModel, Jc_HourInfo>(item);
                jc_HourInfoLists.Add(Jc_HourInfo);
            }
            jc_Hourresponse.Data = jc_HourInfoLists;
            return jc_Hourresponse;
        }
        public BasicResponse<Jc_HourInfo> GetJc_HourById(Jc_HourGetRequest jc_Hourrequest)
        {
            var result = _Repository.GetJc_HourById(jc_Hourrequest.Id);
            var jc_HourInfo = ObjectConverter.Copy<Jc_HourModel, Jc_HourInfo>(result);
            var jc_Hourresponse = new BasicResponse<Jc_HourInfo>();
            jc_Hourresponse.Data = jc_HourInfo;
            return jc_Hourresponse;
        }
        /// <summary>
        /// 查询日最大值
        /// </summary>
        /// <param name="jc_Hourrequest"></param>
        /// <returns>CountDataValue</returns>
        public BasicResponse<Jc_HourInfo> GetDayMaxValueByPointId(Jc_HourGetRequest jc_Hourrequest)
        {
            Jc_HourInfo listResult = new Jc_HourInfo();
            var jc_Hourresponse = new BasicResponse<Jc_HourInfo>();
            try
            {
                string parm0 = string.Format("select PointID, zdz, pjz, Timer from KJ_Hour{0}", DateTime.Now.ToString("yyyyMM"));
                if (DateTime.Now.AddDays(-1).ToString("yyyyMM") != DateTime.Now.ToString("yyyyMM"))
                {
                    //过去一天存在跨月的情况
                    string beforeYearMonth = DateTime.Now.AddDays(-1).ToString("yyyyMM");
                    DataTable dtExists = _Repository.QueryTable("dataAnalysis_QueryTableExists", _Repository.DatebaseName, string.Format("JC_HOUR{0}", beforeYearMonth));
                    if (dtExists != null && dtExists.Rows.Count > 0)
                    {
                        //上月的表也存在，需要联合表查询.
                        parm0 = string.Concat(parm0, string.Format(" union all select PointID, zdz, pjz, Timer from JC_HOUR{0}", beforeYearMonth));
                    }
                }
                DataTable dataTable = _Repository.QueryTable("global_FactorCalculateService_DayMaxValue", parm0, jc_Hourrequest.PointId);
                jc_Hourresponse.Code = 100;
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    listResult.CountDataValue = dataTable.Rows[0][0].ToString();
                }
                else
                {
                    listResult.CountDataValue = "0";
                }

            }
            catch (Exception ex)
            {
                jc_Hourresponse.Code = 1;
                jc_Hourresponse.Message = ex.Message;
            }

            jc_Hourresponse.Data = listResult;
            return jc_Hourresponse;
        }

        /// <summary>
        /// 查询日平均值
        /// </summary>
        /// <param name="jc_Hourrequest"></param>
        /// <returns>CountDataValue</returns>
        public BasicResponse<Jc_HourInfo> GetDayAverageValueByPointId(Jc_HourGetRequest jc_Hourrequest)
        {
            Jc_HourInfo listResult = new Jc_HourInfo();
            var jc_Hourresponse = new BasicResponse<Jc_HourInfo>();
            try
            {
                string parm0 = string.Format("select PointID, zdz, pjz, Timer from KJ_Hour{0}", DateTime.Now.ToString("yyyyMM"));
                if (DateTime.Now.AddDays(-1).ToString("yyyyMM") != DateTime.Now.ToString("yyyyMM"))
                {
                    //过去一天存在跨月的情况
                    string beforeYearMonth = DateTime.Now.AddDays(-1).ToString("yyyyMM");
                    DataTable dtExists = _Repository.QueryTable("dataAnalysis_QueryTableExists", _Repository.DatebaseName, string.Format("KJ_Hour{0}", beforeYearMonth));
                    if (dtExists != null && dtExists.Rows.Count > 0)
                    {
                        //上月的表也存在，需要联合表查询.
                        parm0 = string.Concat(parm0, string.Format(" union all select PointID, zdz, pjz, Timer from KJ_Hour{0}", beforeYearMonth));
                    }
                }
                DataTable dataTable = _Repository.QueryTable("global_FactorCalculateService_DayAverageValue", parm0, jc_Hourrequest.PointId);
                jc_Hourresponse.Code = 100;
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    listResult.CountDataValue = dataTable.Rows[0][0].ToString();
                }
                else
                {
                    listResult.CountDataValue = "0";
                }

            }
            catch (Exception ex)
            {
                jc_Hourresponse.Code = 1;
                jc_Hourresponse.Message = ex.Message;
            }

            jc_Hourresponse.Data = listResult;
            return jc_Hourresponse;
        }

        /// <summary>
        /// 查询月平均值
        /// </summary>
        /// <param name="jc_Hourrequest"></param>
        /// <returns>CountDataValue</returns>
        public BasicResponse<Jc_HourInfo> GetMonthAverageValueByPointId(Jc_HourGetRequest jc_Hourrequest)
        {
            Jc_HourInfo listResult = new Jc_HourInfo();
            var jc_Hourresponse = new BasicResponse<Jc_HourInfo>();
            try
            {

                DataTable dataTable = _Repository.QueryTable("global_FactorCalculateService_MonthAverageValue", jc_Hourrequest.PointId);
                jc_Hourresponse.Code = 100;
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    listResult.CountDataValue = dataTable.Rows[0][0].ToString();
                }
                else
                {
                    listResult.CountDataValue = "0";
                }

            }
            catch (Exception ex)
            {
                jc_Hourresponse.Code = 1;
                jc_Hourresponse.Message = ex.Message;
            }

            jc_Hourresponse.Data = listResult;
            return jc_Hourresponse;
        }

        /// <summary>
        /// 查询周平均值
        /// </summary>
        /// <param name="jc_Hourrequest"></param>
        /// <returns></returns>
        public BasicResponse<Jc_HourInfo> GetWeekAverageValueByPointId(Jc_HourGetRequest jc_Hourrequest)
        {
            Jc_HourInfo hourInfo = new Jc_HourInfo();
            var jc_Hourresponse = new BasicResponse<Jc_HourInfo>();
            try
            {
                //查询周平均数据.
                DataTable dataTable = _Repository.QueryTable("dataAnalysis_WeekAverageValue", jc_Hourrequest.PointId);
                jc_Hourresponse.Code = 100;
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    hourInfo.CountDataValue = dataTable.Rows[0][0].ToString();
                }
                else
                {
                    hourInfo.CountDataValue = "0";
                }
            }
            catch (Exception ex)
            {
                jc_Hourresponse.Code = 1;
                jc_Hourresponse.Message = ex.Message;
            }

            jc_Hourresponse.Data = hourInfo;
            return jc_Hourresponse;
        }

        /// <summary>
        /// 获取所有模拟量历史数据(如月平均值、周平均值、日最大值、日平均值、5分钟最大值、5分钟平均值)
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<DataAnalysisHistoryDataInfo>> GetDataAnalysisHistoryData(BasicRequest request)
        {
            var response = new BasicResponse<List<DataAnalysisHistoryDataInfo>>();

            string parm0 = string.Format("select PointID, zdz, pjz, Timer from KJ_Hour{0}", DateTime.Now.ToString("yyyyMM"));
            if (DateTime.Now.AddDays(-1).ToString("yyyyMM") != DateTime.Now.ToString("yyyyMM"))
            {
                //过去一天存在跨月的情况
                string beforeYearMonth = DateTime.Now.AddDays(-1).ToString("yyyyMM");
                DataTable dtExists = _Repository.QueryTable("dataAnalysis_QueryTableExists", _Repository.DatebaseName, string.Format("KJ_Hour{0}", beforeYearMonth));
                if (dtExists != null && dtExists.Rows.Count > 0)
                {
                    //上月的表也存在，需要联合表查询.
                    parm0 = string.Concat(parm0, string.Format(" union all select PointID, zdz, pjz, Timer from KJ_Hour{0}", beforeYearMonth));
                }
            }
            DateTime now = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:00"));
            DateTime start = now.AddMinutes(-5 - now.Minute % 5);
            string parm1 = start.ToString("yyyyMMdd");
            string parm2 = start.ToString("yyyy-MM-dd HH:mm:ss");
            string parm3 = start.AddMinutes(5).ToString("yyyy-MM-dd HH:mm:ss");
            DataTable dtResult = _Repository.QueryTable("dataAnalysis_GetPointHistoryData", parm0, parm1, parm2, parm3);
            if (dtResult != null && dtResult.Rows.Count > 0)
                response.Data = ObjectConverter.Copy<DataAnalysisHistoryDataInfo>(dtResult);
            else
                response.Data = new List<DataAnalysisHistoryDataInfo>();
            return response;
        }
    }
}


