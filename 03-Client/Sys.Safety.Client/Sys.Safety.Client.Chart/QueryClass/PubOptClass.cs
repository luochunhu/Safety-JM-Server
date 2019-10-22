using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraCharts;
using System.Drawing;
using System.Data;

namespace Sys.Safety.Client.Chart
{
    public class PubOptClass
    {
        /// <summary>
        /// 添加曲线阈值线
        /// </summary>
        public static List<Series> AddZSeries(DataTable data_line, List<float> Points,
            DevExpress.XtraCharts.XYDiagramPane tempxyDiagram, DevExpress.XtraCharts.SecondaryAxisY tempsecondaryAxisY)
        {
            List<Series> Rvalue = new List<Series>();
            for (int i = 0; i < Points.Count; i++)
            {
                if (Points[i] > 0)
                {
                    DevExpress.XtraCharts.LineSeriesView lineSeriesViewZ1 = new DevExpress.XtraCharts.LineSeriesView();
                    lineSeriesViewZ1.LineStyle.DashStyle = DashStyle.Dot;
                    lineSeriesViewZ1.LineMarkerOptions.Size = 1;
                    Series SeriesZ1 = new Series();
                    if (tempxyDiagram != null)
                    {
                        lineSeriesViewZ1.Pane = tempxyDiagram;

                    }
                    if (tempsecondaryAxisY != null)
                    {
                        lineSeriesViewZ1.AxisY = tempsecondaryAxisY;

                    }

                    SeriesZ1.View = lineSeriesViewZ1;

                    switch (i)
                    {
                        case 0:
                            SeriesZ1.Name = "预警阈值";
                            //lineSeriesViewZ1.Color = Color.DarkOrange;
                            InterfaceClass.QueryPubClass_.SetChartColor(SeriesZ1, "Chart_WarnColor");//通过配置设置阈值  20171218
                            break;
                        case 1:
                            SeriesZ1.Name = "报警阈值";
                            //lineSeriesViewZ1.Color = Color.Red;
                             InterfaceClass.QueryPubClass_.SetChartColor(SeriesZ1, "Chart_AlarmColor");//通过配置设置阈值  20171218
                            break;
                        case 2:
                            SeriesZ1.Name = "断电阈值";
                            //lineSeriesViewZ1.Color = Color.DarkRed;
                             InterfaceClass.QueryPubClass_.SetChartColor(SeriesZ1, "Chart_PowerOffColor");//通过配置设置阈值  20171218
                            break;
                        case 3:
                            SeriesZ1.Name = "复电阈值";
                            //lineSeriesViewZ1.Color = Color.Green;
                            InterfaceClass.QueryPubClass_.SetChartColor(SeriesZ1, "Chart_PowerOnColor");//通过配置设置阈值  20171218
                            break;
                        case 4:
                            SeriesZ1.Name = "下限预警阈值";
                            //lineSeriesViewZ1.Color = Color.DarkOrange;
                            InterfaceClass.QueryPubClass_.SetChartColor(SeriesZ1, "Chart_WarnColor");//通过配置设置阈值  20171218
                            break;
                        case 5:
                            SeriesZ1.Name = "下限报警阈值";
                            //lineSeriesViewZ1.Color = Color.Red;
                            InterfaceClass.QueryPubClass_.SetChartColor(SeriesZ1, "Chart_AlarmColor");//通过配置设置阈值  20171218
                            break;
                        case 6:
                            SeriesZ1.Name = "下限断电阈值";
                            //lineSeriesViewZ1.Color = Color.DarkRed;
                            InterfaceClass.QueryPubClass_.SetChartColor(SeriesZ1, "Chart_PowerOffColor");//通过配置设置阈值  20171218
                            break;
                        case 7:
                            SeriesZ1.Name = "下限复电阈值";
                            //lineSeriesViewZ1.Color = Color.Green;
                             InterfaceClass.QueryPubClass_.SetChartColor(SeriesZ1, "Chart_PowerOnColor");//通过配置设置阈值  20171218
                            break;
                    }
                  

                    if (SeriesZ1 != null)
                    {
                        SeriesZ1.Points.BeginUpdate();
                        SeriesZ1.Points.Clear();
                        for (int j = 0; j < data_line.Rows.Count; j++)
                        {
                            //重新定义数据 
                            SeriesZ1.Points.Add(new SeriesPoint(DateTime.Parse(data_line.Rows[j]["Timer"].ToString()), (double)Points[i]));
                        }
                        SeriesZ1.Points.EndUpdate();

                        Rvalue.Add(SeriesZ1);
                    }
                }
            }
            return Rvalue;
        }
    }
}
