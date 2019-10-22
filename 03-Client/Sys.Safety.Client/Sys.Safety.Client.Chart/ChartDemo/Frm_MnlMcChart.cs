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
using System.Globalization;

namespace Sys.Safety.Client.Chart
{
    public partial class Frm_MnlMcChart : RibbonForm
    {
        Series Series { get { return chart.Series.Count > 0 ? chart.Series[0] : null; } }
        XYDiagram Diagram { get { return chart.Diagram as XYDiagram; } }
        public ChartControl ChartControl { get { return chart; } }

        public Frm_MnlMcChart()
        {
            InitializeComponent();
        }
        protected void InitControls()
        {
            using (WaitDialogForm dlg = new WaitDialogForm("Please Wait", "Loading Data...", new Size(200, 50), ParentForm))
            {
                LoadSeries(Series, "001A01:安装位置【设备类型】(单位：%)", "001A01");               
            }
            Diagram.AxisX.NumericScaleOptions.ScaleMode = ScaleMode.Automatic;
            CrosshairFreePosition crosshairPosition = new CrosshairFreePosition();
            crosshairPosition.DockTarget = ((XYDiagram2D)ChartControl.Diagram).DefaultPane;
            crosshairPosition.DockCorner = DockCorner.LeftTop;
            ChartControl.CrosshairOptions.CommonLabelPosition = crosshairPosition;
            foreach (Series series in chart.Series)
                series.CrosshairLabelPattern = "{A} : {V:F2}";
        }
        void LoadSeries(Series series, string name, string shortName)
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

                for (; stime <= etime; stime = stime.AddSeconds(1))
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
        private void Frm_MnlMcChart_Load(object sender, EventArgs e)
        {
            InitControls();
        }
    }
}
