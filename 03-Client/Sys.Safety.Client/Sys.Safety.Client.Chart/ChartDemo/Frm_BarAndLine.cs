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
using System.Globalization;

namespace Sys.Safety.Client.Chart
{
    public partial class Frm_BarAndLine : RibbonForm
    {

        XYDiagram Diagram { get { return chart.Diagram as XYDiagram; } }
        AxisBase AxisX { get { return Diagram != null ? Diagram.AxisX : null; } }
        Series Series1 { get { return chart.Series[0]; } }
        Series Series2 { get { return chart.Series[1]; } }
        Series Series3 { get { return chart.Series[2]; } }

        public Frm_BarAndLine()
        {
            InitializeComponent();
        }
        protected void InitControls()
        {
            LoadSeries(Series1, "最大值", "最大值");
            LoadSeries(Series2, "平均值", "平均值");
            LoadSeries(Series3, "最小值", "最小值");

            //TimeSpan offset = (DateTime)AxisX.VisualRange.MaxValue - (DateTime)AxisX.VisualRange.MinValue;
            //offset = new TimeSpan(offset.Ticks >> 2);
            //AxisX.VisualRange.SetMinMaxValues((DateTime)AxisX.VisualRange.MinValue + offset, (DateTime)AxisX.VisualRange.MaxValue - offset);
        }
        protected void InitControls1(string Time)
        {
            LoadPointSeries1(chartControl1.Series[0], Time, "最大值", "最大值");
            LoadPointSeries(chartControl1.Series[1], Time, "平均值", "平均值");
            LoadPointSeries(chartControl1.Series[2], Time, "最小值", "最小值");

            //TimeSpan offset = (DateTime)AxisX.VisualRange.MaxValue - (DateTime)AxisX.VisualRange.MinValue;
            //offset = new TimeSpan(offset.Ticks >> 2);
            //AxisX.VisualRange.SetMinMaxValues((DateTime)AxisX.VisualRange.MinValue + offset, (DateTime)AxisX.VisualRange.MaxValue - offset);
        }
        void LoadPointSeries(Series series, string Time, string name, string shortName)
        {
            int CodeLen = 3;
            LoadPoints_(series, Time, CodeLen);
            series.CrosshairLabelPattern = shortName + " : {V:F2}";
            series.Name = name;
        }
        void LoadPointSeries1(Series series, string Time, string name, string shortName)
        {
            int CodeLen = 4;
            LoadPoints_(series, Time, CodeLen);
            series.CrosshairLabelPattern = shortName + " : {V:F2}";
            series.Name = name;
        }
        void LoadPoints_(Series series, string Time, int CodeLen)
        {
            if (series != null)
            {
                series.Points.BeginUpdate();
                series.Points.Clear();

                //重新定义数据
                DateTime stime = DateTime.Parse(DateTime.Parse(Time).ToShortDateString());
                DateTime etime = DateTime.Parse(DateTime.Parse(Time).ToShortDateString() + " 23:59:59");

                for (; stime <= etime; stime = stime.AddMinutes(5))
                {
                    double rate = double.Parse((int.Parse(RandCode(CodeLen)) / 100.0).ToString(), CultureInfo.InvariantCulture);
                    series.Points.Add(new SeriesPoint(stime, rate));
                }

                series.Points.EndUpdate();
            }
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
                DateTime stime = DateTime.Parse("2015-06-01");
                DateTime etime = DateTime.Parse("2015-06-30 23:59:59");

                for (; stime <= etime; stime = stime.AddDays(1))
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

        private void Frm_Histogram_Load(object sender, EventArgs e)
        {
            InitControls();
        }

        private void chart_MouseClick(object sender, MouseEventArgs e)
        {

            ChartHitInfo hitInfo = chart.CalcHitInfo(e.Location);
            if (hitInfo.SeriesPoint != null)
            {
                //MessageBox.Show(hitInfo.SeriesPoint.ValuesSerializable.ToString() + "," + hitInfo.SeriesPoint.Argument.ToString());
                InitControls1(hitInfo.SeriesPoint.Argument.ToString());
            }
        }


    }
}
