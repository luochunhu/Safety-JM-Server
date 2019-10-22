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
    public partial class Frm_KglStateChg : RibbonForm
    {
        XYDiagram Diagram { get { return chartControl1.Diagram as XYDiagram; } }
        AxisBase AxisX { get { return Diagram != null ? Diagram.AxisX : null; } }
        Series Series1 { get { return chartControl1.Series[0]; } }
        Series Series2 { get { return chartControl1.Series[1]; } }
        public Frm_KglStateChg()
        {
            InitializeComponent();
        }

        protected void InitControls()
        {
            LoadSeries(Series1, "开百分比", "开百分比");
            LoadSeries(Series2, "断线百分比", "断线百分比");
            LoadDataTable();
            LoadStateBar();
            LoadDetailList();
        }
        /// <summary>
        /// 加载明细列表
        /// </summary>
        void LoadDetailList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Timer", typeof(string));
            dt.Columns.Add("State", typeof(string));

            object[] obj = new object[dt.Columns.Count];
            obj[0] = "2015-06-09 00:00:00";
            obj[1] = "2态(开)";
            dt.Rows.Add(obj);
            obj = new object[dt.Columns.Count];
            obj[0] = "2015-06-09 00:00:00";
            obj[1] = "2态(开)";
            dt.Rows.Add(obj);
            obj = new object[dt.Columns.Count];
            obj[0] = "2015-06-09 00:00:00";
            obj[1] = "1态(停)";
            dt.Rows.Add(obj);
            obj = new object[dt.Columns.Count];
            obj[0] = "2015-06-09 00:00:00";
            obj[1] = "1态(停)";
            dt.Rows.Add(obj);
            obj = new object[dt.Columns.Count];
            obj[0] = "2015-06-09 00:00:00";
            obj[1] = "0态(断线)";       
            dt.Rows.Add(obj);
            obj = new object[dt.Columns.Count];
            obj[0] = "2015-06-09 00:00:00";
            obj[1] = "未知";
            dt.Rows.Add(obj);
            gridControl2.DataSource = dt;
        }
        /// <summary>
        /// 加载状态趋势柱形图
        /// </summary>
        void LoadStateBar()
        {
            LoadBar(chart.Series[0], "2态", DateTime.Parse(DateTime.Now.ToShortDateString() + " 00:00:00"),
                DateTime.Parse(DateTime.Now.ToShortDateString() + " 05:00:00"));
            LoadBar(chart.Series[1], "1态", DateTime.Parse(DateTime.Now.ToShortDateString() + " 05:00:01"),
                DateTime.Parse(DateTime.Now.ToShortDateString() + " 12:00:05"));
            LoadBar(chart.Series[2], "0态", DateTime.Parse(DateTime.Now.ToShortDateString() + " 12:00:06"),
              DateTime.Parse(DateTime.Now.ToShortDateString() + " 18:00:00"));
            LoadBar(chart.Series[3], "其它", DateTime.Parse(DateTime.Now.ToShortDateString() + " 18:00:01"),
              DateTime.Parse(DateTime.Now.ToShortDateString() + " 23:59:59"));
        }
        void LoadBar(Series series, string Argument, DateTime stime, DateTime etime)
        {
            series.Points.BeginUpdate();
            series.Points.Clear();
            object[] dt = new object[2];
            dt[0] = (object)stime;
            dt[1] = (object)etime;
            series.Points.Add(new SeriesPoint(Argument, dt));
            series.Points.EndUpdate();          
        }
        /// <summary>
        /// 加载状态统计信息表格
        /// </summary>
        void LoadDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Columns1");
            dt.Columns.Add("Columns2");
            dt.Columns.Add("Columns3");
            dt.Columns.Add("Columns4");
            dt.Columns.Add("Columns5");
            dt.Columns.Add("Columns6");
            dt.Columns.Add("Columns7");
            dt.Columns.Add("Columns8");
            dt.Columns.Add("Columns9");
            object[] obj = new object[dt.Columns.Count];
            obj[0] = "";
            obj[1] = "0点";
            obj[2] = "1点";
            obj[3] = "2点";
            obj[4] = "3点";
            obj[5] = "4点";
            obj[6] = "5点";
            obj[7] = "6点";
            obj[8] = "7点";
            dt.Rows.Add(obj);
            obj = new object[dt.Columns.Count];
            obj[0] = "开（断线）时间";
            obj[1] = "0:0(0:0)";
            obj[2] = "0:0(0:0)";
            obj[3] = "0:0(0:0)";
            obj[4] = "0:0(0:0)";
            obj[5] = "0:0(0:0)";
            obj[6] = "0:0(0:0)";
            obj[7] = "0:0(0:0)";
            obj[8] = "0:0(0:0)";
            dt.Rows.Add(obj);
            obj = new object[dt.Columns.Count];
            obj[0] = "开（断线）次数";
            obj[1] = "0(0)";
            obj[2] = "0(0)";
            obj[3] = "0(0)";
            obj[4] = "0(0)";
            obj[5] = "0(0)";
            obj[6] = "0(0)";
            obj[7] = "0(0)";
            obj[8] = "0(0)";
            dt.Rows.Add(obj);
            obj = new object[dt.Columns.Count];
            obj[0] = "";
            obj[1] = "8点";
            obj[2] = "9点";
            obj[3] = "10点";
            obj[4] = "11点";
            obj[5] = "12点";
            obj[6] = "13点";
            obj[7] = "14点";
            obj[8] = "15点";
            dt.Rows.Add(obj);
            obj = new object[dt.Columns.Count];
            obj[0] = "开（断线）时间";
            obj[1] = "0:0(0:0)";
            obj[2] = "0:0(0:0)";
            obj[3] = "0:0(0:0)";
            obj[4] = "0:0(0:0)";
            obj[5] = "0:0(0:0)";
            obj[6] = "0:0(0:0)";
            obj[7] = "0:0(0:0)";
            obj[8] = "0:0(0:0)";
            dt.Rows.Add(obj);
            obj = new object[dt.Columns.Count];
            obj[0] = "开（断线）次数";
            obj[1] = "0(0)";
            obj[2] = "0(0)";
            obj[3] = "0(0)";
            obj[4] = "0(0)";
            obj[5] = "0(0)";
            obj[6] = "0(0)";
            obj[7] = "0(0)";
            obj[8] = "0(0)";
            dt.Rows.Add(obj);
            obj = new object[dt.Columns.Count];
            obj[0] = "";
            obj[1] = "16点";
            obj[2] = "17点";
            obj[3] = "18点";
            obj[4] = "19点";
            obj[5] = "20点";
            obj[6] = "21点";
            obj[7] = "22点";
            obj[8] = "23点";
            dt.Rows.Add(obj);
            obj = new object[dt.Columns.Count];
            obj[0] = "开（断线）时间";
            obj[1] = "0:0(0:0)";
            obj[2] = "0:0(0:0)";
            obj[3] = "0:0(0:0)";
            obj[4] = "0:0(0:0)";
            obj[5] = "0:0(0:0)";
            obj[6] = "0:0(0:0)";
            obj[7] = "0:0(0:0)";
            obj[8] = "0:0(0:0)";
            dt.Rows.Add(obj);
            obj = new object[dt.Columns.Count];
            obj[0] = "开（断线）次数";
            obj[1] = "0(0)";
            obj[2] = "0(0)";
            obj[3] = "0(0)";
            obj[4] = "0(0)";
            obj[5] = "0(0)";
            obj[6] = "0(0)";
            obj[7] = "0(0)";
            obj[8] = "0(0)";
            dt.Rows.Add(obj);
            gridControl1.DataSource = dt;
        }
        /// <summary>
        /// 加载柱状图
        /// </summary>
        /// <param name="series"></param>
        /// <param name="name"></param>
        /// <param name="shortName"></param>
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

        private void Frm_KglStateChg_Load(object sender, EventArgs e)
        {
            InitControls();
        }

        private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            if (e.RowHandle >= 0)
            {
                string category = view.GetRowCellDisplayText(e.RowHandle, view.Columns["Columns2"]);
                if (category.Contains("点"))
                {
                    e.Appearance.BackColor = Color.Silver;                    
                    e.Appearance.ForeColor = Color.Black;
                }
            }
        }

        private void gridView2_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            if (e.Column.FieldName == "State")
            {
                string aa = view.GetRowCellDisplayText(e.RowHandle, view.Columns["State"]);
                if (aa.Contains("2"))
                {
                    e.Appearance.ForeColor = Color.Green;
                }
                else if (aa.Contains("1"))
                {
                    e.Appearance.ForeColor = Color.Red;
                }
                else if (aa.Contains("0"))
                {
                    e.Appearance.ForeColor = Color.Black;
                }
                else
                {
                    e.Appearance.ForeColor = Color.Silver;
                }
            }
        }
    }
}
