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
    public partial class Frm_KglBar : RibbonForm
    {
        XYDiagram Diagram { get { return chart.Diagram as XYDiagram; } }
        AxisBase AxisX { get { return Diagram != null ? Diagram.AxisX : null; } }
        Series Series1 { get { return chart.Series[0]; } }

        public Frm_KglBar()
        {
            InitializeComponent();
        }

        protected void InitControls()
        {
            LoadSeries(Series1, "001D01:安装位置【设备类型】(0态:断线，1态：停，2态：开)", "001D01");

        }
        void LoadSeries(Series series, string name, string shortName)
        {
            int CodeLen = 3;
            LoadPoints(series, CodeLen);
            series.CrosshairLabelPattern = shortName + " : {V:F0}%";
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

                for (; stime <= etime; stime = stime.AddHours(1))
                {
                    double rate = double.Parse((int.Parse(RandCode(CodeLen)) % 100).ToString(), CultureInfo.InvariantCulture);
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

        private void Frm_KglBar_Load(object sender, EventArgs e)
        {
            InitControls();
        }
    }
}
