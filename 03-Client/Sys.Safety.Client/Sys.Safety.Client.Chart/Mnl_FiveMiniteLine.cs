﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using Basic.Framework.Logging;
using DashStyle = System.Drawing.Drawing2D.DashStyle;
using Sys.Safety.Request.Chart;
using DevExpress.XtraGrid.Views.Grid;

namespace Sys.Safety.Client.Chart
{
    public partial class Mnl_FiveMiniteLine : XtraForm
    {
        private bool _isScale; //鼠标左键是否按下

        private object _maxX, _minX, _maxY, _minY, _maxX1, _minX1, _maxY1, _minY1;
        //原图X轴和Y轴的最大和最小值以及第二坐标轴（如果有）X轴和Y轴的最大和最小值

        private Point _p0;
        private Point _p1;
        private object _x0, _x1, _y0, _y1;
        private object _x00, _y00, _x11, _y11;

        /// <summary>
        ///     当前设备类型ID
        /// </summary>
        private string CurrentDevid = "0";

        /// <summary>
        ///     当前测点ID
        /// </summary>
        private string CurrentPointID = "0";

        /// <summary>
        ///     当前设备位置ID
        /// </summary>
        private string CurrentWzid = "0";

        /// <summary>
        ///     设备类型ID列表
        /// </summary>
        public List<string> DevList = new List<string>();

        /// <summary>
        ///     曲线数据源
        /// </summary>
        private DataTable dt_line = new DataTable();

        private int IscheckDate;

        private bool IsInIframe = false;

        /// <summary>
        ///     记录鼠标上一次移动的时间
        /// </summary>
        private DateTime lastMouseMoveTime = DateTime.Now;

        /// <summary>
        ///     测点加载线程
        /// </summary>
        //public Thread m_LoadPointThread;

        /// <summary>
        ///     测点单位
        /// </summary>
        private string PointDw = "";

        /// <summary>
        ///     测点号
        /// </summary>
        private string PointID = "";

        /// <summary>
        ///     测点ID列表
        /// </summary>
        public List<string> PointIDList = new List<string>();

        private DateTime SzNameE;

        private DateTime SzNameS;
        private DateTime tempSelTime = new DateTime();

        /// <summary>
        ///     位置ID列表
        /// </summary>
        public List<string> WzList = new List<string>();

        /// <summary>
        /// 曲线颜色设置
        /// </summary>
        private DataTable chartSetting = new DataTable();

        /// <summary>
        /// 测点阈值
        /// </summary>
        private List<float> threshold = new List<float>();
        /// <summary>
        /// 是否支持放大
        /// </summary>
        private bool isZoomFlag = false;
        /// <summary>
        /// 是否选择测点自动查询
        /// </summary>
        private bool autoQuery = false;

        public Mnl_FiveMiniteLine()
        {
            LogHelper.Debug(DateTime.Now);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            InitializeComponent();
            LogHelper.Debug(DateTime.Now);
        }

        public Mnl_FiveMiniteLine(Dictionary<string, string> param)
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            InitializeComponent();
            if ((param != null) && (param.Count > 0))
                try
                {
                    if (param.ContainsKey("PointID") && !string.IsNullOrEmpty(param["PointID"]))
                        PointID = param["PointID"];
                }
                catch
                {
                    PointID = "";
                }
            else
                return;
        }
        public void Reload(Dictionary<string, string> param)
        {
            PointID = "";
            if ((param != null) && (param.Count > 0))
                try
                {
                    if (param.ContainsKey("PointID") && !string.IsNullOrEmpty(param["PointID"]))
                        PointID = param["PointID"];
                }
                catch
                {
                    PointID = "";
                }           
            object sender1 = null;
            var e1 = new EventArgs();
            Mnl_FiveMiniteLine_Load(sender1, e1);
        }
        
        /// <summary>
        ///     坐标系
        /// </summary>
        private XYDiagram Diagram
        {
            get { return chart.Diagram as XYDiagram; }
        }

        /// <summary>
        ///     X坐标
        /// </summary>
        private AxisBase AxisX
        {
            get { return Diagram != null ? Diagram.AxisX : null; }
        }

        /// <summary>
        ///     Y坐标
        /// </summary>
        private AxisBase AxisY
        {
            get { return Diagram != null ? Diagram.AxisY : null; }
        }

        /// <summary>
        ///     监测值曲线
        /// </summary>
        private Series Series1
        {
            get { return chart.Series[0]; }
        }

        /// <summary>
        ///     最大值曲线
        /// </summary>
        private Series Series2
        {
            get { return chart.Series[1]; }
        }

        /// <summary>
        ///     最小值曲线
        /// </summary>
        private Series Series3
        {
            get { return chart.Series[2]; }
        }

        /// <summary>
        ///     平均值曲线
        /// </summary>
        private Series Series4
        {
            get { return chart.Series[3]; }
        }

        /// <summary>
        ///     移动值曲线
        /// </summary>
        private Series Series5
        {
            get { return chart.Series[4]; }
        }


        /// <summary>
        ///     单击左键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DevUCChart_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (isZoomFlag)
                {
                    return;
                }
                if (e.Button == MouseButtons.Left)
                {
                    _p0 = e.Location;
                    SetLocationClientValue(_p0, ref _x0, ref _y0, ref _x00, ref _y00);
                    _isScale = true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_FiveMiniteLine_DevUCChart_MouseDown" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     将屏幕坐标转换为图表的坐标
        /// </summary>
        /// <param name="p"></param>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        private void SetLocationClientValue(Point p, ref object x0, ref object y0, ref object x1, ref object y1)
        {
            try
            {
                var diagram = Diagram;
                var coord = diagram.PointToDiagram(p);
                switch (coord.ArgumentScaleType)
                {
                    case ScaleType.DateTime:
                        x0 = coord.DateTimeArgument;
                        break;
                    case ScaleType.Numerical:
                        x0 = coord.NumericalArgument;
                        break;
                    case ScaleType.Qualitative:
                        x0 = coord.QualitativeArgument;
                        break;
                }
                switch (coord.ValueScaleType)
                {
                    case ScaleType.DateTime:
                        y0 = coord.DateTimeValue;
                        break;
                    case ScaleType.Numerical:
                        y0 = coord.NumericalValue;
                        break;
                }
                if (diagram.GetAllAxesX().Count > 1)
                {
                    var ax = coord.GetAxisValue(diagram.GetAllAxesX()[1]);
                    switch (coord.ArgumentScaleType)
                    {
                        case ScaleType.DateTime:
                            x1 = ax.DateTimeValue;
                            break;
                        case ScaleType.Numerical:
                            x1 = ax.NumericalValue;
                            break;
                        case ScaleType.Qualitative:
                            x1 = ax.QualitativeValue;
                            break;
                    }
                }

                if (diagram.GetAllAxesY().Count > 1)
                {
                    var ax = coord.GetAxisValue(diagram.GetAllAxesY()[1]);
                    switch (coord.ValueScaleType)
                    {
                        case ScaleType.DateTime:
                            y1 = ax.DateTimeValue;
                            break;
                        case ScaleType.Numerical:
                            y1 = ax.NumericalValue;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_FiveMiniteLine_SetLocationClientValue" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     拖动鼠标事件，绘制虚线矩形
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DevUCChart_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (isZoomFlag)
                {
                    return;
                }
                if (_isScale)
                {
                    var pE = new Point(e.Location.X - Location.X, e.Location.Y - Location.Y);
                    var g = Graphics.FromHwnd(chart.Handle);
                    var pen = new Pen(new SolidBrush(Color.Red));
                    pen.DashStyle = DashStyle.Dot;
                    g.DrawRectangle(pen, _p0.X, _p0.Y, e.Location.X - _p0.X, e.Location.Y - _p0.Y);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_FiveMiniteLine_DevUCChart_MouseMove" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     释放左键事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DevUCChart_MouseUp(object sender, MouseEventArgs e)
        {
            if (isZoomFlag)
            {
                return;
            }
            var diag = Diagram;

            try
            {
                if (e.Button == MouseButtons.Left) //右键框选查询密采
                {
                    if ((Series1.Name != "密采值") && (Series1.Points.Count > 0))
                    {
                        _p1 = e.Location;
                        SetLocationClientValue(_p1, ref _x1, ref _y1, ref _x11, ref _y11);

                        if ((_p1.X == _p0.X) && (_p1.Y == _p0.Y)) //没有拖动操作
                            return;
                        if ((_p1.X > _p0.X) && (_p1.Y > _p0.Y)) //右下拖动，显示密采数据
                        {
                            //diag.AxisX.WholeRange.SetMinMaxValues(_x0, _x1);
                            //diag.AxisX.VisualRange.SetMinMaxValues(_x0, _x1);
                            var wdf = new WaitDialogForm("正在加载数据...", "请等待...");
                            try
                            {
                                double MaxLC2 = 0;
                                double MinLC2 = 0;
                                DateTime SzNameS1 = new DateTime(), SzNameE1 = new DateTime();
                                if ((_x0.ToString().Length > 8) && (_x1.ToString().Length > 8))
                                {
                                    SzNameS1 = DateTime.Parse(_x0.ToString());
                                    SzNameE1 = DateTime.Parse(_x1.ToString());
                                }
                                else if (string.IsNullOrEmpty(_x0.ToString()) && string.IsNullOrEmpty(_x1.ToString()))
                                {
                                    SzNameS1 = DateTime.Parse(SzNameS.ToShortDateString());
                                    SzNameE1 = DateTime.Parse(SzNameE.ToShortDateString() + " 23:59:59");
                                }
                                else if (string.IsNullOrEmpty(_x1.ToString()))
                                {
                                    SzNameS1 = DateTime.Parse(_x0.ToString());
                                    SzNameE1 = DateTime.Parse(SzNameE.ToShortDateString() + " 23:59:59");
                                }
                                else if (string.IsNullOrEmpty(_x0.ToString()))
                                {
                                    SzNameS1 = DateTime.Parse(SzNameS.ToShortDateString());
                                    SzNameE1 = DateTime.Parse(_x1.ToString());
                                }


                                dt_line = InterfaceClass.McLineQueryClass_.GetMcData(SzNameS1, SzNameE1, true,
                                    CurrentPointID,
                                    CurrentDevid, CurrentWzid, "密采值", ref MaxLC2, ref MinLC2);


                                InitControls2(dt_line, "密采值");

                                var MaxValue = (float)MaxLC2 * 1.2f;

                                var tempList = InterfaceClass.QueryPubClass_.GetZFromTable(CurrentPointID);
                                foreach (var tempMax in tempList)
                                    if (MaxValue < tempMax) //表示当天没值
                                        MaxValue = tempMax;

                                //读取量程低
                                var MinValue = (float)MinLC2;
                                if (MinValue > 0)
                                    MinValue = 0.0f;
                                else
                                    foreach (var tempMin in tempList)
                                        if (MinValue > tempMin) //表示当天没值
                                            MinValue = tempMin;

                                AxisY.WholeRange.SetMinMaxValues(MinValue, MaxValue);
                                AxisY.VisualRange.SetMinMaxValues(MinValue, MaxValue);

                                var _minX = SzNameS1;
                                var _maxX = SzNameE1;


                                AxisX.WholeRange.SetMinMaxValues(_minX, _maxX);
                                AxisX.VisualRange.SetMinMaxValues(_minX, _maxX);

                                #region//动态添加、删除阈值线

                                var tempZ = PubOptClass.AddZSeries(dt_line,
                                    InterfaceClass.QueryPubClass_.GetZFromTable(CurrentPointID), null, null);
                                var isinchart = false;
                                if (tempZ.Count > 0)
                                {
                                    foreach (var serie in tempZ)
                                    {
                                        isinchart = false;
                                        for (var i = chart.Series.Count - 1; i >= 5; i--)
                                        {
                                            var chartserie = chart.Series[i];
                                            if (chartserie.Name == serie.Name) //如果已存在
                                            {
                                                //重新添加                               
                                                serie.CheckedInLegend = chartserie.CheckedInLegend;
                                                chart.Series[i].Points.Clear();
                                                chart.Series[i].Points.AddRange(serie.Points.ToArray());
                                                isinchart = true;
                                                break;
                                            }
                                        }
                                        if (!isinchart)
                                        {
                                            serie.ShowInLegend = false;
                                            chart.Series.Add(serie);
                                        }
                                    }
                                    //删除曲线中未定义的阈值线
                                    for (var i = chart.Series.Count - 1; i >= 5; i--)
                                    {
                                        var chartserie = chart.Series[i];
                                        isinchart = false;
                                        foreach (var serie in tempZ)
                                            if (chartserie.Name == serie.Name) //如果已存在
                                            {
                                                isinchart = true;
                                                break;
                                            }
                                        if (!isinchart)
                                            chart.Series.Remove(chartserie);
                                    }
                                }

                                #endregion

                            }
                            catch (Exception ex)
                            {
                                Basic.Framework.Logging.LogHelper.Error(ex);
                            }
                            if (wdf != null)
                                wdf.Close();
                            //double MaxValue = 0, MinValue = 0;
                            //if (diag.AxisY.WholeRange.MinValue is double)
                            //{
                            //    double __y1 = (double)_y1;
                            //    double __y0 = (double)_y0;
                            //    MaxValue = __y1 > __y0 ? __y1 : __y0;
                            //    MinValue = __y1 > __y0 ? __y0 : __y1;
                            //}
                            //diag.AxisY.WholeRange.SetMinMaxValues(MinValue, MaxValue);
                            //diag.AxisY.VisualRange.SetMinMaxValues(MinValue, MaxValue);

                            checkEdit3.Enabled = false;
                            checkEdit1.Enabled = false;
                            checkEdit2.Enabled = false;
                            checkEdit4.Enabled = false;
                            checkEdit5.Enabled = false;
                        }
                    }
                }
                else if (e.Button == MouseButtons.Right) //右键还原
                {
                    object sender1 = null;
                    var e1 = new EventArgs();
                    simpleButton1_Click(sender1, e1);

                    checkEdit3.Enabled = true;
                    checkEdit1.Enabled = true;
                    checkEdit2.Enabled = true;
                    checkEdit4.Enabled = true;
                    checkEdit5.Enabled = true;
                }
                _isScale = false;
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_FiveMiniteLine_DevUCChart_MouseUp" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     加载所有曲线数据
        /// </summary>
        protected void InitControls(DataTable dt)
        {
            
            //DevExpress.XtraCharts.LineSeriesView lineView1 = new DevExpress.XtraCharts.LineSeriesView();
            //lineView1.LineStyle.Thickness = 1;
            //Series1.View = lineView1;
            //DevExpress.XtraCharts.LineSeriesView lineView2 = new DevExpress.XtraCharts.LineSeriesView();
            //lineView2.LineStyle.Thickness = 1;
            //Series2.View = lineView2;
            InterfaceClass.QueryPubClass_.SetChartColor(Series1, "Chart_JczColor");
            InterfaceClass.QueryPubClass_.SetChartColor(Series2, "Chart_ZdzColor");

            LoadSeries(Series1, "监测值", "Av", dt);
            LoadSeries(Series2, "最大值", "Bv", dt);
            LoadSeries(Series3, "最小值", "Cv", dt);
            LoadSeries(Series4, "平均值", "Dv", dt);
            LoadSeries(Series5, "移动值", "Ev", dt);
        }

        /// <summary>
        ///     测点曲线加载
        /// </summary>
        /// <param name="series"></param>
        /// <param name="name"></param>
        /// <param name="shortName"></param>
        private void LoadSeries(Series series, string name, string shortName, DataTable dt)
        {
            var CodeLen = 3;
            LoadPoints(series, shortName, dt);
            //series.CrosshairLabelPattern = name + " : {V:F2}" + "%";
            series.Name = name;
        }

        /// <summary>
        ///     加载曲线测点的值
        /// </summary>
        /// <param name="series"></param>
        /// <param name="CodeLen"></param>
        private void LoadPoints(Series series, string shortName, DataTable dt)
        {
            try
            {
                if (series != null)
                {
                    series.Points.BeginUpdate();
                    series.Points.Clear();

                    //重新定义数据             

                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        var rate = double.Parse(dt.Rows[i][shortName].ToString(), CultureInfo.InvariantCulture);
                        series.Points.Add(new SeriesPoint(DateTime.Parse(dt.Rows[i]["Timer"].ToString()), rate));
                    }

                    series.Points.EndUpdate();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_FiveMiniteLine_LoadPoints" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     加载所有曲线数据
        /// </summary>
        protected void InitControls2(DataTable dt, string lineName)
        {
            //DevExpress.XtraCharts.StepLineSeriesView lineView1 = new DevExpress.XtraCharts.StepLineSeriesView();
            //lineView1.LineStyle.Thickness = 1;
            //Series1.View = lineView1;
            //DevExpress.XtraCharts.StepLineSeriesView lineView2 = new DevExpress.XtraCharts.StepLineSeriesView();
            //lineView2.LineStyle.Thickness = 1;
            //Series2.View = lineView2;
            InterfaceClass.QueryPubClass_.SetChartColor(Series1, "Chart_McColor");
            InterfaceClass.QueryPubClass_.SetChartColor(Series2, "Chart_YdzColor");

            LoadSeries2(Series1, lineName, "A", dt);
            LoadSeries2(Series2, "移动值", "B", dt);
            Series1.Visible = true;
            Series2.Visible = true;

            Series3.Visible = false;
            Series4.Visible = false;
            Series5.Visible = false;
        }

        /// <summary>
        ///     测点曲线加载
        /// </summary>
        /// <param name="series"></param>
        /// <param name="name"></param>
        /// <param name="shortName"></param>
        private void LoadSeries2(Series series, string name, string shortName, DataTable dt)
        {
            LoadPoints2(series, shortName, dt);
            //series.CrosshairLabelPattern = name + " : {V:F2}" + "%";
            series.Name = name;
        }

        /// <summary>
        ///     加载曲线测点的值
        /// </summary>
        /// <param name="series"></param>
        /// <param name="CodeLen"></param>
        private void LoadPoints2(Series series, string shortName, DataTable dt)
        {
            try
            {
                if (series != null)
                {
                    series.Points.BeginUpdate();
                    series.Points.Clear();

                    //重新定义数据             

                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        var rate = double.Parse(dt.Rows[i][shortName].ToString(), CultureInfo.InvariantCulture);
                        series.Points.Add(new SeriesPoint(DateTime.Parse(dt.Rows[i]["Timer"].ToString()), rate));
                    }

                    series.Points.EndUpdate();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_FiveMiniteLine_LoadPoints2" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     产生随机数(测试用)
        /// </summary>
        /// <param name="N"></param>
        /// <returns></returns>
        public string RandCode(int N)
        {
            char[] arrChar = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            var num = new StringBuilder();
            var rnd = new Random(Guid.NewGuid().GetHashCode());
            for (var i = 0; i < N; i++)
                num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());
            return num.ToString();
        }



        /// <summary>
        ///     窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Mnl_FiveMiniteLine_Load(object sender, EventArgs e)
        {
            try
            {
                LogHelper.Debug(DateTime.Now);
                //设置窗体高度和宽度
                Width = Convert.ToInt32(Screen.GetWorkingArea(this).Width * 0.9);
                Height = Convert.ToInt32(Screen.GetWorkingArea(this).Height * 0.9);
                Left = Convert.ToInt32(Screen.GetWorkingArea(this).Width * 0.1 / 2);
                Top = Convert.ToInt32(Screen.GetWorkingArea(this).Height * 0.1 / 2);

                //设置左边查询条件宽度
                layoutControlItem1.Width = 320;
                //layoutControlItem4.Width = 320;

                //初始化控件值
                dateEdit1.DateTime = DateTime.Now;
                dateEdit2.DateTime = DateTime.Now;
                Series1.Points.Clear();
                Series2.Points.Clear();
                Series3.Points.Clear();
                Series4.Points.Clear();
                Series5.Points.Clear();
                gridControl1.DataSource = null;
                

                #region//加载曲线颜色 

                InterfaceClass.QueryPubClass_.SetChartColor(Series2, "Chart_ZdzColor");
                InterfaceClass.QueryPubClass_.SetChartColor(Series3, "Chart_ZxzColor");
                InterfaceClass.QueryPubClass_.SetChartColor(Series4, "Chart_PjzColor");
                InterfaceClass.QueryPubClass_.SetChartColor(Series1, "Chart_JczColor");
                InterfaceClass.QueryPubClass_.SetChartColor(Series5, "Chart_YdzColor");
                InterfaceClass.QueryPubClass_.SetChartBgColor(Diagram, "Chart_BgColor");

                chartSetting = InterfaceClass.QueryPubClass_.GetChartColorSetting();

                #endregion

                //启动测点加载
                //m_LoadPointThread = new System.Threading.Thread(new System.Threading.ThreadStart(this.LoadPointList));
                //m_LoadPointThread.Priority = ThreadPriority.Normal;
                //m_LoadPointThread.Start();
                LoadPointList(true);
                LogHelper.Debug(DateTime.Now);
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_FiveMiniteLine_Mnl_FiveMiniteLine_Load" + ex.Message + ex.StackTrace);
            }
        }



        private void LoadPointList(bool loadData)
        {
            var wdf = new WaitDialogForm("正在加载数据...", "请等待...");
            try
            {
                //Thread.Sleep(500);


                //数据校验
                //var ts = dateEdit2.DateTime - dateEdit1.DateTime;
                //if ((ts.TotalDays > 7) || (ts.TotalDays < 0)) 
                //    if (IscheckDate == 2)
                //        dateEdit1.DateTime = dateEdit2.DateTime;
                //    else if (IscheckDate == 1)
                //        dateEdit2.DateTime = dateEdit1.DateTime;
                LoadPointSelList(dateEdit1.DateTime, dateEdit2.DateTime, loadData);
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_FiveMiniteLine_LoadPointList" + ex.Message + ex.StackTrace);
            }
            if (wdf != null)
                wdf.Close();
        }

        /// <summary>
        ///     曲线鼠标移动获取游标的值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chart_CustomDrawCrosshair(object sender, CustomDrawCrosshairEventArgs e)
        {
            try
            {
                var ShowText = "";
                string SelTime = "";
                DataRow[] drs = null;
                var index = 0;

                foreach (var element in e.CrosshairElements)
                {
                    var point = element.SeriesPoint;
                    //label1.Text = point.Argument.ToString();//显示要显示的文字
                    SelTime = DateTime.Parse(point.ArgumentSerializable).ToString("yyyy-MM-dd HH:mm:ss");
                    //if (index == 0)
                    //{
                    //    if (element.Series.Name != "密采值")
                    //    {
                    //        //大于300毫秒或有组件显示或隐藏才进行重绘
                    //        TimeSpan mouseMoveTimeStep = System.DateTime.Now - lastMouseMoveTime;
                    //        if (mouseMoveTimeStep.TotalMilliseconds >= 2000)
                    //        {
                    //            lastMouseMoveTime = System.DateTime.Now;

                    //            ChartGridDis(SelTime);

                    //            _isScale = false;
                    //        }
                    //    }
                    //}
                    if (element.Series.Name.Contains("移动值")
                        || element.Series.Name.Contains("监测值")
                        || element.Series.Name.Contains("最大值")
                        || element.Series.Name.Contains("最小值")
                        || element.Series.Name.Contains("平均值")
                        || element.Series.Name.Contains("密采值"))
                    {
                        if (element.Series.Name.Contains("密采值") || element.Series.Name.Contains("移动值"))
                        {
                            SelTime = DateTime.Parse(point.ArgumentSerializable).ToString("yyyy-MM-dd HH:mm:ss.fff");
                        }

                        drs = dt_line.Select("Timer='" + SelTime + "' ");
                        if (drs.Length > 0)
                            if ((drs[0]["type"].ToString() == "20") ||
                                (drs[0]["type"].ToString() == "22") ||
                                (drs[0]["type"].ToString() == "23") ||
                                (drs[0]["type"].ToString() == "33") ||
                                (drs[0]["type"].ToString() == "34") ||
                                (drs[0]["type"].ToString() == "46"))
                            {
                                if (!element.Series.Name.Contains("移动值"))
                                {
                                    ShowText = element.Series.Name + ":" + drs[0]["typetext"];
                                }
                                else
                                {
                                    if (point.Values[0].ToString() == "1E-05")
                                        ShowText = element.Series.Name + ":" + "未记录";
                                    else
                                        ShowText = element.Series.Name + ":" + point.Values[0].ToString("f2") + PointDw;
                                }
                            }
                            else
                            {
                                if (point.Values[0].ToString() == "1E-05")
                                    ShowText = element.Series.Name + ":" + "未记录";
                                else
                                    ShowText = element.Series.Name + ":" + point.Values[0].ToString("f2") + PointDw;
                            }
                    }
                    else
                    {
                        ShowText = element.Series.Name + ":" + point.Values[0].ToString("f2");
                    }
                    if (index == 0)
                        element.LabelElement.HeaderText = "时间:" + SelTime + "\n";


                    element.LabelElement.Text = ShowText; //显示要显示的文字   
                    index++;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_FiveMiniteLine_chart_CustomDrawCrosshair" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     监测值显示/隐藏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEdit3_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkEdit1.Checked && !checkEdit2.Checked && !checkEdit3.Checked && !checkEdit4.Checked && !checkEdit5.Checked)
            {
                XtraMessageBox.Show("请至少选择1条曲线进行查看！");
                checkEdit3.Checked = true;
                return;
            }
            if (checkEdit3.Checked)
                Series1.Visible = true;
            else
                Series1.Visible = false;
        }

        /// <summary>
        ///     最大值显示/隐藏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkEdit1.Checked && !checkEdit2.Checked && !checkEdit3.Checked && !checkEdit4.Checked && !checkEdit5.Checked)
            {
                XtraMessageBox.Show("请至少选择1条曲线进行查看！");
                checkEdit1.Checked = true;
                return;
            }
            if (checkEdit1.Checked)
                Series2.Visible = true;
            else
                Series2.Visible = false;
        }

        /// <summary>
        ///     最小值显示/隐藏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEdit2_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkEdit1.Checked && !checkEdit2.Checked && !checkEdit3.Checked && !checkEdit4.Checked && !checkEdit5.Checked)
            {
                XtraMessageBox.Show("请至少选择1条曲线进行查看！");
                checkEdit2.Checked = true;
                return;
            }
            if (checkEdit2.Checked)
                Series3.Visible = true;
            else
                Series3.Visible = false;
        }

        /// <summary>
        ///     平均值显示/隐藏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEdit4_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkEdit1.Checked && !checkEdit2.Checked && !checkEdit3.Checked && !checkEdit4.Checked && !checkEdit5.Checked)
            {
                XtraMessageBox.Show("请至少选择1条曲线进行查看！");
                checkEdit4.Checked = true;
                return;
            }
            if (checkEdit4.Checked)
                Series4.Visible = true;
            else
                Series4.Visible = false;
        }

        /// <summary>
        ///     移动值显示/隐藏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEdit5_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkEdit1.Checked && !checkEdit2.Checked && !checkEdit3.Checked && !checkEdit4.Checked && !checkEdit5.Checked)
            {
                XtraMessageBox.Show("请至少选择1条曲线进行查看！");
                checkEdit5.Checked = true;
                return;
            }
            if (checkEdit5.Checked)
                Series5.Visible = true;
            else
                Series5.Visible = false;
        }

        /// <summary>
        ///     打印曲线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                if (chart != null)
                    ChartPrint.chartPrint(chart, (float)(Width * 0.8));
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_FiveMiniteLine_simpleButton4_Click" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     导出图片
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="ext"></param>
        private void ExportToCore(string filename, string ext)
        {
            try
            {
                if (chart != null)
                {
                    var currentCursor = Cursor.Current;
                    Cursor.Current = Cursors.WaitCursor;
                    if (ext == "rtf")
                        chart.ExportToRtf(filename);
                    else if (ext == "pdf")
                        chart.ExportToPdf(filename);
                    else if (ext == "mht")
                        chart.ExportToMht(filename);
                    else if (ext == "html")
                        chart.ExportToHtml(filename);
                    else if (ext == "xls")
                        chart.ExportToXls(filename);
                    else if (ext == "xlsx")
                        chart.ExportToXlsx(filename);
                    else
                    {
                        ImageFormat currentImageFormat = null;
                        if (ext.ToLower().Contains("bmp"))
                            currentImageFormat = ImageFormat.Bmp;
                        else if (ext.ToLower().Contains("jpg"))
                            currentImageFormat = ImageFormat.Jpeg;
                        else if (ext.ToLower().Contains("png"))
                            currentImageFormat = ImageFormat.Png;
                        else if (ext.ToLower().Contains("gif"))
                            currentImageFormat = ImageFormat.Gif;
                        chart.ExportToImage(filename, currentImageFormat);
                    }
                    Cursor.Current = currentCursor;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_FiveMiniteLine_ExportToCore" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            var wdf = new WaitDialogForm("正在加载数据...", "请等待...");
            try
            {
                SzNameS = dateEdit1.DateTime;
                SzNameE = dateEdit2.DateTime;
                //数据校验
                var ts = SzNameE - SzNameS;
                if (ts.TotalDays > 7)
                {
                    if (wdf != null)
                        wdf.Close();
                    XtraMessageBox.Show("查询的最大天数为7天,请重新选择日期！");
                    return;
                }
                if (comboBoxEdit1.SelectedIndex < 0)
                {
                    if (wdf != null)
                        wdf.Close();
                    XtraMessageBox.Show("请选择测点！");

                    return;
                }


                CurrentDevid = DevList[comboBoxEdit1.SelectedIndex];
                CurrentPointID = PointIDList[comboBoxEdit1.SelectedIndex];
                PointID = CurrentPointID;
                CurrentWzid = WzList[comboBoxEdit1.SelectedIndex];

                bool isQueryByPoint = false;
                if (QueryByPoint.Checked)
                {
                    isQueryByPoint = true;
                }

                dt_line = InterfaceClass.FiveMiniteLineQueryClass_.getFiveMiniteLine(SzNameS, SzNameE, CurrentPointID,
                    CurrentDevid, CurrentWzid, isQueryByPoint);

                string zdzTime = "";
                var MaxValueNow = InterfaceClass.QueryPubClass_.getMaxBv(dt_line, "Bv", ref zdzTime);

                var MaxValue = MaxValueNow * 1.2f;

                threshold.Clear();
                threshold = InterfaceClass.QueryPubClass_.GetZFromTable(CurrentPointID);
                //var tempList = InterfaceClass.QueryPubClass_.GetZFromTable(CurrentPointID);
                foreach (var tempMax in threshold)
                    if (MaxValue < tempMax) //表示当天没值
                        MaxValue = tempMax;

                if (MaxValue < 0.01)
                { //如果无数据，则加个默认最大值  20170723
                    MaxValue = 1;
                }

                //读取量程低
                var MinValueNow = InterfaceClass.QueryPubClass_.getMinBv(dt_line, "Cv");
                var MinValue = MinValueNow;
                if (MinValue > 0)
                    MinValue = 0.0f;
                else
                    foreach (var tempMin in threshold)
                        if (MinValue > tempMin) //表示当天没值
                            MinValue = tempMin;

                MinValue = (float)MinValue - 0.01f;

                PointDw = InterfaceClass.FiveMiniteLineQueryClass_.getPointDw(CurrentDevid);


                AxisY.WholeRange.SetMinMaxValues(MinValue, MaxValue);
                AxisY.VisualRange.SetMinMaxValues(MinValue, MaxValue);

                var name = comboBoxEdit1.SelectedItem.ToString();
                var shortName = name.Substring(0, name.IndexOf('.'));

                float avgValue = InterfaceClass.QueryPubClass_.getAvgBv(dt_line, "Dv");

                chart.Titles[0].Text = comboBoxEdit1.SelectedItem.ToString() + "\r\n最大值：" + (((float)MaxValueNow).ToString() == "-9999" ? "-" : ((float)MaxValueNow).ToString("f2"))
                     + "，最大值时间：" + (zdzTime == "" ? "-" : zdzTime) + ",最小值：" + (((float)MinValueNow).ToString() == "9999" ? "-" : ((float)MinValueNow).ToString("f2"))
                     + "，平均值：" + (avgValue.ToString() == "-9999" ? "-" : avgValue.ToString("f2"));

                InitControls(dt_line);

                Series3.Visible = true;
                Series4.Visible = true;
                Series5.Visible = true;
                _minX = DateTime.Parse(SzNameS.ToShortDateString());
                _maxX = DateTime.Parse(SzNameE.ToShortDateString() + " 23:59:55");

                AxisX.WholeRange.SetMinMaxValues(_minX, _maxX);
                AxisX.VisualRange.SetMinMaxValues(_minX, _maxX);

                #region//动态添加、删除阈值线

                var tempZ = PubOptClass.AddZSeries(dt_line, InterfaceClass.QueryPubClass_.GetZFromTable(CurrentPointID),
                    null, null);
                var isinchart = false;
                if (tempZ.Count > 0)
                {
                    foreach (var serie in tempZ)
                    {
                        isinchart = false;
                        for (var i = chart.Series.Count - 1; i >= 5; i--)
                        {
                            var chartserie = chart.Series[i];
                            if (chartserie.Name == serie.Name) //如果已存在
                            {
                                //重新添加                               
                                serie.CheckedInLegend = chartserie.CheckedInLegend;
                                chart.Series[i].Points.Clear();
                                chart.Series[i].Points.AddRange(serie.Points.ToArray());
                                isinchart = true;
                                break;
                            }
                        }
                        if (!isinchart)
                        {
                            serie.ShowInLegend = false;
                            chart.Series.Add(serie);
                        }
                    }
                    //删除曲线中未定义的阈值线
                    for (var i = chart.Series.Count - 1; i >= 5; i--)
                    {
                        var chartserie = chart.Series[i];
                        isinchart = false;
                        foreach (var serie in tempZ)
                            if (chartserie.Name == serie.Name) //如果已存在
                            {
                                isinchart = true;
                                break;
                            }
                        if (!isinchart)
                            chart.Series.Remove(chartserie);
                    }
                }

                #endregion

                #region//设置曲线显示隐藏

                for (var i = chart.Series.Count - 1; i >= 0; i--)
                {
                    var chartserie = chart.Series[i];
                    switch (chartserie.Name)
                    {
                        case "监测值":
                            if (checkEdit3.Checked)
                                chartserie.Visible = true;
                            else
                                chartserie.Visible = false;
                            break;
                        case "最大值":
                            if (checkEdit1.Checked)
                                chartserie.Visible = true;
                            else
                                chartserie.Visible = false;
                            break;
                        case "最小值":
                            if (checkEdit2.Checked)
                                chartserie.Visible = true;
                            else
                                chartserie.Visible = false;
                            break;
                        case "平均值":
                            if (checkEdit4.Checked)
                                chartserie.Visible = true;
                            else
                                chartserie.Visible = false;
                            break;
                        case "移动值":
                            if (checkEdit5.Checked)
                                chartserie.Visible = true;
                            else
                                chartserie.Visible = false;
                            break;
                        case "预警阈值":
                        case "下限预警阈值":
                            if (checkEdit7.Checked)
                                chartserie.Visible = true;
                            else
                                chartserie.Visible = false;
                            break;
                        case "报警阈值":
                        case "下限报警阈值":
                            if (checkEdit8.Checked)
                                chartserie.Visible = true;
                            else
                                chartserie.Visible = false;
                            break;
                        case "断电阈值":
                        case "下限断电阈值":
                            if (checkEdit9.Checked)
                                chartserie.Visible = true;
                            else
                                chartserie.Visible = false;
                            break;
                        case "复电阈值":
                        case "下限复电阈值":
                            if (checkEdit10.Checked)
                                chartserie.Visible = true;
                            else
                                chartserie.Visible = false;
                            break;
                    }
                }

                #endregion


                //加载列表
                gridControl2.DataSource = dt_line;
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_FiveMiniteLine_simpleButton1_Click" + ex.Message + ex.StackTrace);
            }
            if (wdf != null)
                wdf.Close();
        }

        /// <summary>
        ///     结束日期选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dateEdit2_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                //IscheckDate = 2;

                //启动测点加载
                //m_LoadPointThread = new System.Threading.Thread(new System.Threading.ThreadStart(this.LoadPointList));
                //m_LoadPointThread.Priority = ThreadPriority.Normal;
                //m_LoadPointThread.Start();
                if (radioGroup1.SelectedIndex == 1)
                {//如果按已存储测点查询，则每次选择时间后，重新加载 
                    LoadPointList(false);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_FiveMiniteLine_dateEdit2_EditValueChanged" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     开始日期选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dateEdit1_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                //IscheckDate = 1;

                //启动测点加载
                //m_LoadPointThread = new System.Threading.Thread(new System.Threading.ThreadStart(this.LoadPointList));
                //m_LoadPointThread.Priority = ThreadPriority.Normal;
                //m_LoadPointThread.Start();

                if (radioGroup1.SelectedIndex == 1)
                {//如果按已存储测点查询，则每次选择时间后，重新加载 
                    LoadPointList(false);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_FiveMiniteLine_dateEdit1_EditValueChanged" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     加载测点选择列表
        /// </summary>
        /// <param name="stime"></param>
        /// <param name="etime"></param>
        private void LoadPointSelList(DateTime stime, DateTime etime,bool loadData)
        {
            try
            {
                var LoadPointStr = new List<string>();
                if (radioGroup1.SelectedIndex == 0)//按已定义测点查询
                {
                    LoadPointStr = InterfaceClass.queryConditions_.GetActivePointList(1, ref PointIDList,
                      ref DevList, ref WzList);
                }
                else//按已存储测点查询
                {
                    LoadPointStr = InterfaceClass.queryConditions_.GetPointList(stime, etime, 1, ref PointIDList,
                        ref DevList, ref WzList);
                }
                //comboBoxEdit1.BeginInvoke(new Action(() =>
                //   {
                comboBoxEdit1.Properties.Items.Clear();

                foreach (var PointStr in LoadPointStr)
                    comboBoxEdit1.Properties.Items.Add(PointStr);
                if (comboBoxEdit1.Properties.Items.Count > 0)
                {
                    comboBoxEdit1.SelectedIndex = 0;
                    comboBoxEdit1.Enabled = true;
                }
                else
                {
                    comboBoxEdit1.Enabled = false;
                    comboBoxEdit1.Text = "没有数据";
                }

                //加载曲线
                if (!string.IsNullOrEmpty(PointID))
                {
                    var isinlist = false;
                    //给combox赋初始值
                    for (var i = 0; i < PointIDList.Count; i++)
                    {
                        if (PointIDList[i].Contains(PointID))
                        {
                            comboBoxEdit1.SelectedIndex = i;
                            isinlist = true;
                            break;
                        }
                    }
                    if (loadData)
                    {
                        if (isinlist)
                        {
                            //调用查询
                            object sender1 = null;
                            var e1 = new EventArgs();
                            simpleButton1_Click(sender1, e1);
                        }
                    }
                }
                //}));
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_FiveMiniteLine_LoadPointSelList" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     曲线移动显示列表数据
        /// </summary>
        private void ChartGridDis(DateTime SelTime)
        {
            try
            {
                var DtRefresh = new DataTable();
                DtRefresh.Columns.Add("名称及类型", Type.GetType("System.String"));
                DtRefresh.Columns.Add("报警值", Type.GetType("System.String"));
                DtRefresh.Columns.Add("断电值", Type.GetType("System.String"));
                DtRefresh.Columns.Add("复电值", Type.GetType("System.String"));
                DtRefresh.Columns.Add("设备状态", Type.GetType("System.String"));
                DtRefresh.Columns.Add("断电范围", Type.GetType("System.String"));
                DtRefresh.Columns.Add("读值时刻", Type.GetType("System.String"));
                DtRefresh.Columns.Add("监测值", Type.GetType("System.String"));
                DtRefresh.Columns.Add("最大值", Type.GetType("System.String"));
                DtRefresh.Columns.Add("平均值", Type.GetType("System.String"));
                DtRefresh.Columns.Add("报警/解除", Type.GetType("System.String"));
                DtRefresh.Columns.Add("断电/复电", Type.GetType("System.String"));
                DtRefresh.Columns.Add("馈电状态", Type.GetType("System.String"));
                DtRefresh.Columns.Add("措施及时刻", Type.GetType("System.String"));

                var QueryStr = new string[14];
                var tempPointInf = new string[14];
                string SzTableName;
                DateTime QxDate, DtStart, DtEnd;

                for (var i = 6; i <= 13; i++)
                    QueryStr[i] = "";

                SzTableName = "KJ_StaFiveMinute" + SelTime.Year + SelTime.Month.ToString().PadLeft(2, '0') +
                              SelTime.Day.ToString().PadLeft(2, '0');
                tempPointInf = InterfaceClass.FiveMiniteLineQueryClass_.ShowPointInf(CurrentWzid, CurrentPointID);
                QueryStr[0] = tempPointInf[0];
                QueryStr[1] = tempPointInf[1];
                QueryStr[2] = tempPointInf[2];
                QueryStr[3] = tempPointInf[3];
                var szDate = SelTime.ToString();


                QxDate = Convert.ToDateTime(szDate);
                DtStart = Convert.ToDateTime(QxDate.ToShortDateString());

                DtEnd = Convert.ToDateTime(QxDate.ToShortDateString());

                var Ihour = QxDate.Hour;
                var SMin = QxDate.Minute;
                QueryStr[6] = QxDate.Hour + ":" + QxDate.Minute + ":" + QxDate.Second;
                QueryStr[5] = "";
                var Iminite = QxDate.Minute % 10;

                if (Iminite > 4)
                {
                    DtStart = DtStart.AddHours(Ihour);
                    var Ival = 5;
                    if (SMin / 10 != 0)
                        Ival = Ival + SMin / 10 * 10;
                    DtStart = DtStart.AddMinutes(Ival);

                    Ival = 9;
                    DtEnd = DtEnd.AddHours(Ihour);
                    if (SMin / 10 != 0)
                        Ival = Ival + SMin / 10 * 10;
                    DtEnd = DtEnd.AddMinutes(Ival);
                    DtEnd = DtEnd.AddSeconds(59);
                }
                else
                {
                    DtStart = DtStart.AddHours(Ihour);
                    var Ival = 0;
                    if (SMin / 10 != 0)
                        Ival = Ival + SMin / 10 * 10;
                    DtStart = DtStart.AddMinutes(Ival);

                    Ival = 4;
                    DtEnd = DtEnd.AddHours(Ihour);
                    if (SMin / 10 != 0)
                        Ival = Ival + SMin / 10 * 10;
                    DtEnd = DtEnd.AddMinutes(Ival);
                    DtEnd = DtEnd.AddSeconds(59);
                }
                tempPointInf = InterfaceClass.FiveMiniteLineQueryClass_.GetDataVale(QxDate, DtStart, CurrentPointID,
                    CurrentDevid, CurrentWzid);
                QueryStr[7] = tempPointInf[6];
                QueryStr[8] = tempPointInf[7];
                QueryStr[9] = tempPointInf[8];
                QueryStr[4] = tempPointInf[13]; //设备状态


                tempPointInf = InterfaceClass.FiveMiniteLineQueryClass_.GetValue(QxDate, DtStart, DtEnd, CurrentPointID,
                    CurrentDevid, CurrentWzid);
                if (QueryStr[7] == "未记录")
                {
                    QueryStr[5] = "";
                    QueryStr[10] = "解除";
                    QueryStr[11] = "复电";
                    QueryStr[12] = "正常";
                    QueryStr[13] = "无";
                }
                else
                {
                    QueryStr[5] = tempPointInf[4];
                    QueryStr[10] = tempPointInf[9];
                    QueryStr[11] = tempPointInf[10];
                    QueryStr[12] = tempPointInf[11];
                    QueryStr[13] = tempPointInf[12];
                }


                DtRefresh.Rows.Clear();
                var Dr = DtRefresh.NewRow();
                for (var i = 0; i <= 13; i++)
                    Dr[i] = QueryStr[i];
                DtRefresh.Rows.Add(Dr);
                gridControl1.DataSource = DtRefresh;
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_FiveMiniteLine_ChartGridDis" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     导出图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.FileName = "模拟量5分钟曲线.png";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    ExportToCore(saveFileDialog1.FileName, "png");
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_FiveMiniteLine_simpleButton2_Click" + ex.Message + ex.StackTrace);
            }
        }

        private void chart_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                _isScale = false;
                var diagram = Diagram;
                var _p0 = e.Location;
                var coord = diagram.PointToDiagram(_p0);
                var SelTime = coord.DateTimeArgument;
                if (chart.Series[0].Name == "密采值")
                {
                    if ((SelTime.Minute % 10 >= 0) && (SelTime.Minute % 10 < 5))
                        SelTime =
                            DateTime.Parse(SelTime.ToShortDateString() + " " + SelTime.Hour.ToString("00") + ":" +
                                           SelTime.Minute / 10 + "0:00");
                    else
                        SelTime =
                            DateTime.Parse(SelTime.ToShortDateString() + " " + SelTime.Hour.ToString("00") + ":" +
                                           SelTime.Minute / 10 + "5:00");
                }
                else
                {
                    if ((SelTime.Minute % 10 >= 3) && (SelTime.Minute % 10 < 8))
                    {
                        SelTime =
                            DateTime.Parse(SelTime.ToShortDateString() + " " + SelTime.Hour.ToString("00") + ":" +
                                           SelTime.Minute / 10 + "5:00");
                    }
                    else
                    {
                        if (SelTime.Minute % 10 < 3)
                            SelTime =
                                DateTime.Parse(SelTime.ToShortDateString() + " " + SelTime.Hour.ToString("00") + ":" +
                                               SelTime.Minute / 10 + "0:00");
                        else //>=8
                            SelTime =
                                DateTime.Parse(SelTime.ToShortDateString() + " " +
                                               SelTime.AddMinutes(2).Hour.ToString("00") + ":" +
                                               SelTime.AddMinutes(2).Minute / 10 + "0:00");
                    }
                }

                // 20170715
                if (SelTime.Year <= 1900) return;

                ChartGridDis(SelTime);
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_FiveMiniteLine_chart_MouseClick" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     预警值显示设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEdit7_CheckedChanged(object sender, EventArgs e)
        {
            for (var i = chart.Series.Count - 1; i >= 0; i--)
            {
                var chartserie = chart.Series[i];
                if (chartserie.Name.Contains("预警阈值")) //
                    chartserie.Visible = checkEdit7.Checked;
            }
        }

        /// <summary>
        ///     报警值显示设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEdit8_CheckedChanged(object sender, EventArgs e)
        {
            for (var i = chart.Series.Count - 1; i >= 0; i--)
            {
                var chartserie = chart.Series[i];
                if (chartserie.Name.Contains("报警阈值")) //
                    chartserie.Visible = checkEdit8.Checked;
            }
        }

        /// <summary>
        ///     断电值显示设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEdit9_CheckedChanged(object sender, EventArgs e)
        {
            for (var i = chart.Series.Count - 1; i >= 0; i--)
            {
                var chartserie = chart.Series[i];
                if (chartserie.Name.Contains("断电阈值")) //
                    chartserie.Visible = checkEdit9.Checked;
            }
        }

        /// <summary>
        ///     复电值显示设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEdit10_CheckedChanged(object sender, EventArgs e)
        {
            for (var i = chart.Series.Count - 1; i >= 0; i--)
            {
                var chartserie = chart.Series[i];
                if (chartserie.Name.Contains("复电阈值")) //
                    chartserie.Visible = checkEdit10.Checked;
            }
        }
        /// <summary>
        /// 已定义，已存储功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPointList(false);
        }

        /// <summary>
        /// 突出显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chart_CustomDrawSeriesPoint(object sender, CustomDrawSeriesPointEventArgs e)
        {
            var value = e.SeriesPoint.Values;

            var pointtimer = DateTime.Parse(e.SeriesPoint.ArgumentSerializable).ToString("yyyy-MM-dd HH:mm:ss");

            var pointvalue = dt_line.Select(string.Format("Timer= '" + pointtimer + "'"));
            if (pointvalue.Length > 0)
            {
                var type = pointvalue[0]["state"].ToString();
                //如果是标校数据,则设置对应点为标校颜色;否则判断是否为报警数据
                if (type == "24")
                {
                    if (chartSetting.Select("strKey='Chart_BxColor'").Length > 0)
                    {
                        Color bxColor = Color.FromArgb(int.Parse(chartSetting.Select("strKey='Chart_BxColor'")[0]["strValue"].ToString()));
                        e.SeriesDrawOptions.Color = bxColor;
                    }
                    else
                    {
                        e.SeriesDrawOptions.Color = Color.Yellow;
                    }
                }
                //该point对应类型不为-1
                else if (pointvalue[0]["type"].ToString() != "-1")
                {
                    if (threshold.Count > 5)
                    {
                        //上限报警和下限报警
                        if ((threshold[1] != (float)0 && value[0] >= threshold[1]) ||
                            (threshold[5] != (float)0 && value[0] <= threshold[5]))
                        {
                            if (chartSetting.Select("strKey='Chart_BJColor'").Length > 0)
                            {
                                Color bjColor = Color.FromArgb(int.Parse(chartSetting.Select("strKey='Chart_BJColor'")[0]["strValue"].ToString()));
                                e.SeriesDrawOptions.Color = bjColor;
                            }
                            else
                            {
                                e.SeriesDrawOptions.Color = Color.Red;
                            }
                        }
                    }
                }
            }
        }

        private void checkEdit6_CheckedChanged(object sender, EventArgs e)
        {
            List<string> seriesNameList = new List<string>();
            for (int i = 0; i < chart.Series.Count; i++)
            {
                seriesNameList.Add(chart.Series[i].Name);
            }

            if (checkEdit6.Checked)
            {
                if (seriesNameList.Contains("最大值"))
                {
                    LineSeriesView lineview1 = (LineSeriesView)chart.Series["最大值"].View;
                    lineview1.MarkerVisibility = DefaultBoolean.True;
                    lineview1.LineMarkerOptions.Size = 10;
                }

                if (seriesNameList.Contains("最小值"))
                {
                    LineSeriesView lineview2 = (LineSeriesView)chart.Series["最小值"].View;
                    lineview2.MarkerVisibility = DefaultBoolean.True;
                    lineview2.LineMarkerOptions.Size = 10;
                }

                if (seriesNameList.Contains("平均值"))
                {
                    LineSeriesView lineview3 = (LineSeriesView)chart.Series["平均值"].View;
                    lineview3.MarkerVisibility = DefaultBoolean.True;
                    lineview3.LineMarkerOptions.Size = 10;
                }

                if (seriesNameList.Contains("监测值"))
                {
                    LineSeriesView lineview4 = (LineSeriesView)chart.Series["监测值"].View;
                    lineview4.MarkerVisibility = DefaultBoolean.True;
                    lineview4.LineMarkerOptions.Size = 10;
                }

                if (seriesNameList.Contains("移动值"))
                {
                    LineSeriesView lineview5 = (LineSeriesView)chart.Series["移动值"].View;
                    lineview5.MarkerVisibility = DefaultBoolean.True;
                    lineview5.LineMarkerOptions.Size = 10;
                }

                if (seriesNameList.Contains("密采值"))
                {
                    LineSeriesView lineview6 = (LineSeriesView)chart.Series["密采值"].View;
                    lineview6.MarkerVisibility = DefaultBoolean.True;
                    lineview6.LineMarkerOptions.Size = 10;
                }
            }
            else
            {
                if (seriesNameList.Contains("最大值"))
                {
                    LineSeriesView lineview1 = (LineSeriesView)chart.Series["最大值"].View;
                    lineview1.MarkerVisibility = DefaultBoolean.False;
                }

                if (seriesNameList.Contains("最小值"))
                {
                    LineSeriesView lineview2 = (LineSeriesView)chart.Series["最小值"].View;
                    lineview2.MarkerVisibility = DefaultBoolean.False;
                }

                if (seriesNameList.Contains("平均值"))
                {
                    LineSeriesView lineview3 = (LineSeriesView)chart.Series["平均值"].View;
                    lineview3.MarkerVisibility = DefaultBoolean.False;
                }

                if (seriesNameList.Contains("监测值"))
                {
                    LineSeriesView lineview4 = (LineSeriesView)chart.Series["监测值"].View;
                    lineview4.MarkerVisibility = DefaultBoolean.False;
                }

                if (seriesNameList.Contains("移动值"))
                {
                    LineSeriesView lineview5 = (LineSeriesView)chart.Series["移动值"].View;
                    lineview5.MarkerVisibility = DefaultBoolean.False;
                }

                if (seriesNameList.Contains("密采值"))
                {
                    LineSeriesView lineview5 = (LineSeriesView)chart.Series["密采值"].View;
                    lineview5.MarkerVisibility = DefaultBoolean.False;
                }
            }
        }
        /// <summary>
        /// 支持放大
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEdit11_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit11.Checked)
            {
                isZoomFlag = true;
                Diagram.EnableAxisXScrolling = true;
                Diagram.EnableAxisXZooming = true;
            }
            else
            {
                isZoomFlag = false;
                Diagram.EnableAxisXScrolling = false;
                Diagram.EnableAxisXZooming = false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEdit12_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit12.Checked)
            {
                autoQuery = true;
            }
            else
            {
                autoQuery = false;
            }
        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (autoQuery)
            {
                //调用查询
                object sender1 = null;
                var e1 = new EventArgs();
                simpleButton1_Click(sender1, e1);
            }
        }

        private void gridView2_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            try
            {
                var view = sender as GridView;
                if (e.Column.FieldName == "Av")
                {
                    var aa = view.GetRowCellDisplayText(e.RowHandle, view.Columns["Av"]);
                    if (aa.Contains("0.00001"))
                    {
                        e.Appearance.ForeColor = Color.Black;
                        view.SetRowCellValue(e.RowHandle, e.Column, "未记录");
                    }                   
                    else
                    {
                        e.Appearance.ForeColor = Color.Black;
                    }
                }
                if (e.Column.FieldName == "Bv")
                {
                    var aa = view.GetRowCellDisplayText(e.RowHandle, view.Columns["Bv"]);
                    if (aa.Contains("0.00001"))
                    {
                        e.Appearance.ForeColor = Color.Black;
                        view.SetRowCellValue(e.RowHandle, e.Column, "未记录");
                    }
                    else
                    {
                        e.Appearance.ForeColor = Color.Black;
                    }
                }
                if (e.Column.FieldName == "Cv")
                {
                    var aa = view.GetRowCellDisplayText(e.RowHandle, view.Columns["Cv"]);
                    if (aa.Contains("0.00001"))
                    {
                        e.Appearance.ForeColor = Color.Black;
                        view.SetRowCellValue(e.RowHandle, e.Column, "未记录");
                    }
                    else
                    {
                        e.Appearance.ForeColor = Color.Black;
                    }
                }
                if (e.Column.FieldName == "Dv")
                {
                    var aa = view.GetRowCellDisplayText(e.RowHandle, view.Columns["Dv"]);
                    if (aa.Contains("0.00001"))
                    {
                        e.Appearance.ForeColor = Color.Black;
                        view.SetRowCellValue(e.RowHandle, e.Column, "未记录");
                    }
                    else
                    {
                        e.Appearance.ForeColor = Color.Black;
                    }
                }                
            }
            catch (Exception ex)
            {
                LogHelper.Error("gridView2_RowCellStyle" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 前一天
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            checkOhterTime(-1);
        }
        /// <summary>
        /// 后一天
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            checkOhterTime(1);
        }
        /// <summary>
        /// 前一天，后一天查询
        /// </summary>
        /// <param name="days"></param>
        private void checkOhterTime(int days)
        {
            dateEdit1.DateTime = dateEdit1.DateTime.AddDays(days);
            dateEdit2.DateTime = dateEdit2.DateTime.AddDays(days);
            simpleButton1_Click(new object(), new EventArgs());
        }
    }
}