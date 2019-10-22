using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraCharts;
using DevExpress.Utils;

using System.IO;
using System.Xml;
using System.Globalization;

namespace Sys.Safety.Client.Chart
{
    public partial class Frm_LineChart : RibbonForm
    {
        XYDiagram Diagram { get { return chart.Diagram as XYDiagram; } }
        AxisBase AxisX { get { return Diagram != null ? Diagram.AxisX : null; } }
        Series Series1 { get { return chart.Series[0]; } }
        Series Series2 { get { return chart.Series[1]; } }
        Series Series3 { get { return chart.Series[2]; } }
        Series Series4 { get { return chart.Series[3]; } }
        Series Series5 { get { return chart.Series[4]; } }

        public Frm_LineChart()
        {
            InitializeComponent();
        }

        protected void InitControls()
        {
            LoadSeries(Series1, "监测值", "监测值");
            LoadSeries1(Series2, "最大值", "最大值");
            LoadSeries1(Series3, "最小值", "最小值");
            LoadSeries(Series4, "平均值", "平均值");
            LoadSeries(Series5, "移动值", "移动值");
            //TimeSpan offset = (DateTime)AxisX.VisualRange.MaxValue - (DateTime)AxisX.VisualRange.MinValue;
            //offset = new TimeSpan(offset.Ticks >> 2);
            //AxisX.VisualRange.SetMinMaxValues((DateTime)AxisX.VisualRange.MinValue + offset, (DateTime)AxisX.VisualRange.MaxValue - offset);
        }
        void LoadSeries(Series series, string name, string shortName)
        {
            int CodeLen = 3;
            LoadPoints(series, CodeLen);
            series.CrosshairLabelPattern = shortName + " : {V:F2}";
            series.Name = name;
        }
        void LoadSeries1(Series series, string name, string shortName)
        {
            int CodeLen = 4;
            LoadPoints(series, CodeLen);
            series.CrosshairLabelPattern = shortName + " : {V:F2}";
            series.Name = name;
        }
        void LoadPoints(Series series, int CodeLen)
        {
            if (series != null)
            {
                series.Points.BeginUpdate();
                series.Points.Clear();

                //重新定义数据
                DateTime stime = DateTime.Parse(DateTime.Now.ToShortDateString());
                DateTime etime = DateTime.Parse(DateTime.Now.ToShortDateString() + " 23:59:59");

                for (; stime <= etime; stime = stime.AddMinutes(5))
                {
                    double rate = double.Parse((int.Parse(RandCode(CodeLen)) / 100.0).ToString(), CultureInfo.InvariantCulture);
                    series.Points.Add(new SeriesPoint(stime, rate));
                }

                series.Points.EndUpdate();
            }
        }
        public string RandCode(int N)
        {
            char[] arrChar = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            StringBuilder num = new StringBuilder();
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < N; i++)
            {
                num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());
            }
            return num.ToString();
        }

        private void Frm_Coordinates_Load(object sender, EventArgs e)
        {
            InitControls();
        }
        private void chart_CustomDrawCrosshair(object sender, CustomDrawCrosshairEventArgs e)
        {
            foreach (CrosshairElement element in e.CrosshairElements)
            {
                SeriesPoint point = element.SeriesPoint;
                label1.Text = point.Argument.ToString();//显示要显示的文字
            }
        }

    }
}
