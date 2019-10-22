using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using DevExpress.XtraCharts;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.Request.Chart;
using Sys.Safety.Request.Listex;
using Sys.Safety.ServiceContract.Chart;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Request.PointDefine;

namespace Sys.Safety.Client.Chart
{
    /// <summary>
    ///     查询公共类
    /// </summary>
    public class QueryPubClass
    {
        private readonly IChartService _chartService = ServiceFactory.Create<IChartService>();

        private IPointDefineService pointDefineService = ServiceFactory.Create<IPointDefineService>();

        /// <summary>
        ///     获取报警阈值
        /// </summary>
        /// <param name="CurrentDevid"></param>
        /// <returns></returns>
        public List<float> GetZFromTable(string PointId)
        {
            var Rvalue = new List<float>();
            try
            {
                //Rvalue = ServiceFactory.CreateService<IChartService>().GetZFromTable(devid);
                var req = new PointIdRequest
                {
                    PointId = PointId
                };
                var res = _chartService.GetZFromTable(req);
                if (!res.IsSuccess)
                    throw new Exception(res.Message);
                Rvalue = res.Data;
            }
            catch (Exception Ex)
            {
                LogHelper.Error("QueryPubClass_GetLcFromTable" + Ex.Message + Ex.StackTrace);
            }
            return Rvalue;
        }

        /// <summary>
        ///     返回开关量的状态定义信息
        /// </summary>
        /// <param name="CurrentDevid"></param>
        /// <returns></returns>
        public List<string> getKglStateDev(string PointId)
        {
            var rvalue = new List<string>();
            try
            {
                //rvalue = ServiceFactory.CreateService<IChartService>().getKglStateDev(CurrentDevid);
                var req = new PointIdRequest
                {
                    PointId = PointId
                };
                var res = _chartService.GetKglStateDev(req);
                rvalue = res.Data;
            }
            catch (Exception Ex)
            {
                LogHelper.Error("QueryPubClass_getKglStateDev" + Ex.Message + Ex.StackTrace);
            }
            return rvalue;
        }

        /// <summary>
        ///     获取当前曲线中的最大值作为量程高值
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ColumnName"></param>
        /// <returns></returns>
        public float getMaxBv(DataTable dt, string ColumnName)
        {
            float MaxValue = -9999;
            try
            {
                foreach (DataRow dr in dt.Rows)
                    if (!string.IsNullOrEmpty(dr[ColumnName].ToString()))
                        if (float.Parse(dr[ColumnName].ToString()) > MaxValue  && dr[ColumnName].ToString() != "0.00001")
                            MaxValue = float.Parse(dr[ColumnName].ToString());
            }
            catch (Exception ex)
            {
                LogHelper.Error("QueryPubClass_getMaxBv" + ex.Message + ex.StackTrace);
            }
            return MaxValue;
        }

        /// <summary>
        ///     获取当前曲线的平均值
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ColumnName"></param>
        /// <returns></returns>
        public float getAvgBv(DataTable dt, string ColumnName)
        {
            float countValue = 0;
            int countIndex = 0;
            float avgValue = -9999;
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(dr[ColumnName].ToString()))
                    {
                        if (dr[ColumnName].ToString() != "0.00001")
                        {
                            countValue += float.Parse(dr[ColumnName].ToString());
                            countIndex++;
                        }
                    }
                }
                if (countIndex > 0)
                {
                    avgValue = countValue / countIndex;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("QueryPubClass_getAvgBv" + ex.Message + ex.StackTrace);
            }
            return avgValue;
        }

        /// <summary>
        ///     获取当前曲线中的最小值作为量程低值
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ColumnName"></param>
        /// <returns></returns>
        public float getMinBv(DataTable dt, string ColumnName)
        {
            float MinValue = 9999;
            try
            {
                foreach (DataRow dr in dt.Rows)
                    if (!string.IsNullOrEmpty(dr[ColumnName].ToString()))
                        if (float.Parse(dr[ColumnName].ToString()) < MinValue && dr[ColumnName].ToString() != "0.00001")
                            MinValue = float.Parse(dr[ColumnName].ToString());
            }
            catch (Exception ex)
            {
                LogHelper.Error("QueryPubClass_getMinBv" + ex.Message + ex.StackTrace);
            }
            return MinValue;
        }

        /// <summary>
        ///     加载曲线颜色
        /// </summary>
        /// <param name="serie">当前曲线</param>
        /// <param name="ColorKey">颜色key值</param>
        public void SetChartColor(Series serie, string ColorKey)
        {
            //DataTable ChartSets = ServiceFactory.CreateService<IChartService>().getAllChartSet("");
            var req = new GetAllChartSetRequest
            {
                StrKey = ""
            };
            var res = _chartService.GetAllChartSet(req);
            if (!res.IsSuccess)
                throw new Exception(res.Message);
            var ChartSets = res.Data;
            for (var i = 0; i < ChartSets.Rows.Count; i++)
                if (ChartSets.Rows[i]["strKey"].ToString() == ColorKey)
                    serie.View.Color = Color.FromArgb(int.Parse(ChartSets.Rows[i]["strValue"].ToString()));
        }

        /// <summary>
        ///     加载曲线背景颜色
        /// </summary>
        /// <param name="Diagram">曲线背景</param>
        /// <param name="ColorKey">颜色key值</param>
        public void SetChartBgColor(XYDiagram Diagram, string ColorKey)
        {
            //DataTable ChartSets = ServiceFactory.CreateService<IChartService>().getAllChartSet("");
            var req = new GetAllChartSetRequest
            {
                StrKey = ""
            };
            var res = _chartService.GetAllChartSet(req);
            if (!res.IsSuccess)
                throw new Exception(res.Message);
            var ChartSets = res.Data;

            for (var i = 0; i < ChartSets.Rows.Count; i++)
                if (ChartSets.Rows[i]["strKey"].ToString() == ColorKey)
                {
                    Diagram.DefaultPane.BackColor = Color.FromArgb(int.Parse(ChartSets.Rows[i]["strValue"].ToString()));
                    for (var j = 0; j < Diagram.Panes.Count; j++)
                        Diagram.Panes[j].BackColor = Color.FromArgb(int.Parse(ChartSets.Rows[i]["strValue"].ToString()));
                }
        }

        /// <summary>
        ///     加载曲线背景颜色
        /// </summary>
        /// <param name="Diagram">曲线背景</param>
        /// <param name="ColorKey">颜色key值</param>
        public void SetBigDataChartBgColor(SwiftPlotDiagram Diagram, string ColorKey)
        {
            //DataTable ChartSets = ServiceFactory.CreateService<IChartService>().getAllChartSet("");
            var req = new GetAllChartSetRequest
            {
                StrKey = ""
            };
            var res = _chartService.GetAllChartSet(req);
            if (!res.IsSuccess)
                throw new Exception(res.Message);
            var ChartSets = res.Data;

            for (var i = 0; i < ChartSets.Rows.Count; i++)
                if (ChartSets.Rows[i]["strKey"].ToString() == ColorKey)
                {
                    Diagram.DefaultPane.BackColor = Color.FromArgb(int.Parse(ChartSets.Rows[i]["strValue"].ToString()));
                    for (var j = 0; j < Diagram.Panes.Count; j++)
                        Diagram.Panes[j].BackColor = Color.FromArgb(int.Parse(ChartSets.Rows[i]["strValue"].ToString()));
                }
        }

        public DataTable GetChartColorSetting(string strKey = "")
        {
            var req = new GetAllChartSetRequest
            {
                StrKey = string.IsNullOrEmpty(strKey) ? "" : strKey
            };
            var res = _chartService.GetAllChartSet(req);
            if (!res.IsSuccess)
            {
                LogHelper.Info("获取曲线颜色配置出错：" + res.Message);
                return null;
            }
            return res.Data;
        }

        /// <summary>
        /// 判断测点是否绑定控制
        /// </summary>
        /// <param name="Point"></param>
        /// <returns></returns>
        public bool GetPointIsBindControl(string point)
        {
            bool rvalue = false;
            PointDefineGetByPointRequest PointDefineRequest = new PointDefineGetByPointRequest();
            PointDefineRequest.Point = point;
            Jc_DefInfo def = pointDefineService.GetPointDefineCacheByPoint(PointDefineRequest).Data;
            if (def != null && def.DevPropertyID == 1
                && (def.K1 > 0
                || def.K2 > 0
                || def.K3 > 0
                || def.K4 > 0
                || def.K5 > 0
                || def.K6 > 0
                || def.K7 > 0))
            {//模拟量控制口判断
                rvalue = true;
            }
            if (def != null && def.DevPropertyID == 2
                && (def.K1 > 0
                || def.K2 > 0
                || def.K3 > 0))
            {//模拟量控制口判断
                rvalue = true;
            }
            return rvalue;
        }
    }
}