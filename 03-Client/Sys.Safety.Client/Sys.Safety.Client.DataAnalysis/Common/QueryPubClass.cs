using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DevExpress.XtraCharts;
using System.Drawing;
using Basic.Framework.Logging;
using Basic.Framework.Service;

namespace Sys.Safety.Client.Chart
{
    /// <summary>
    /// 查询公共类
    /// </summary>
    public class QueryPubClass
    {
        /// <summary>
        /// 获取报警阈值
        /// </summary>
        /// <param name="CurrentDevid"></param>
        /// <returns></returns>
        public List<float> GetZFromTable(string devid)
        {
            List<float> Rvalue = new List<float>();
            //try
            //{
            //    Rvalue = ServiceFactory.CreateService<IChartService>().GetZFromTable(devid);
            //}
            //catch (Exception Ex)
            //{
            //    Basic.Framework.Utils.Log.LogHelper.Error("QueryPubClass_GetLcFromTable" + Ex.Message + Ex.StackTrace);
            //}
            return Rvalue;
        }
        /// <summary>
        /// 返回开关量的状态定义信息
        /// </summary>
        /// <param name="CurrentDevid"></param>
        /// <returns></returns>
        public List<string> getKglStateDev(string CurrentDevid)
        {
            List<string> rvalue = new List<string>();
            //try
            //{
            //    rvalue = ServiceFactory.CreateService<IChartService>().getKglStateDev(CurrentDevid);
            //}
            //catch (Exception Ex)
            //{
            //    Basic.Framework.Utils.Log.LogHelper.Error("QueryPubClass_getKglStateDev" + Ex.Message + Ex.StackTrace);
            //}
            return rvalue;
        }
        /// <summary>
        /// 获取当前曲线中的最大值作为量程高值
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ColumnName"></param>
        /// <returns></returns>
        public float getMaxBv(DataTable dt, string ColumnName)
        {
            float MaxValue = 0;
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(dr[ColumnName].ToString()))
                    {
                        if (float.Parse(dr[ColumnName].ToString()) > MaxValue)
                        {
                            MaxValue = float.Parse(dr[ColumnName].ToString());
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
               LogHelper.Error("QueryPubClass_getMaxBv" + ex.Message + ex.StackTrace);
            }
            return MaxValue;
        }
        /// <summary>
        /// 获取当前曲线中的最小值作为量程低值
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ColumnName"></param>
        /// <returns></returns>
        public float getMinBv(DataTable dt, string ColumnName)
        {
            float MinValue = 0;
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(dr[ColumnName].ToString()))
                    {
                        if (float.Parse(dr[ColumnName].ToString()) < MinValue)
                        {
                            MinValue = float.Parse(dr[ColumnName].ToString());
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.Error("QueryPubClass_getMinBv" + ex.Message + ex.StackTrace);
            }
            return MinValue;
        }
        /// <summary>
        /// 加载曲线颜色
        /// </summary>
        /// <param name="serie">当前曲线</param>
        /// <param name="ColorKey">颜色key值</param>
        public void SetChartColor(Series serie, string ColorKey)
        {
            //DataTable ChartSets = ServiceFactory.CreateService<IChartService>().getAllChartSet("");
            //for (int i = 0; i < ChartSets.Rows.Count; i++)
            //{
            //    if (ChartSets.Rows[i]["strKey"].ToString() == ColorKey)
            //    {
            //        serie.View.Color = Color.FromArgb(int.Parse(ChartSets.Rows[i]["strValue"].ToString()));
            //    }
            //}
        }
        /// <summary>
        /// 加载曲线背景颜色
        /// </summary>
        /// <param name="Diagram">曲线背景</param>
        /// <param name="ColorKey">颜色key值</param>
        public void SetChartBgColor(XYDiagram Diagram, string ColorKey)
        {

            //DataTable ChartSets = ServiceFactory.CreateService<IChartService>().getAllChartSet("");
            //for (int i = 0; i < ChartSets.Rows.Count; i++)
            //{
            //    if (ChartSets.Rows[i]["strKey"].ToString() == ColorKey)
            //    {
            //        Diagram.DefaultPane.BackColor = Color.FromArgb(int.Parse(ChartSets.Rows[i]["strValue"].ToString()));
            //        for (int j = 0; j < Diagram.Panes.Count; j++)
            //        {
            //            Diagram.Panes[j].BackColor = Color.FromArgb(int.Parse(ChartSets.Rows[i]["strValue"].ToString()));
            //        }
            //    }
            //}

        }
    }
}
