using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Basic.Framework.Logging;
using DashStyle = System.Drawing.Drawing2D.DashStyle;
using Sys.Safety.ServiceContract;
using Basic.Framework.Service;
using Sys.Safety.Request.DeviceDefine;
using Sys.Safety.DataContract;
using System.Diagnostics;

namespace Sys.Safety.Client.Chart
{
    public partial class Mnl_McData : XtraForm
    {
        Color tbColor = Color.Blue;


        IDeviceDefineService deviceDefineService;

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
        ///     当前位置ID
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

        private bool IsInIframe = false;


        /// <summary>
        ///     测点加载线程
        /// </summary>
        //public Thread m_LoadPointThread;

        /// <summary>
        ///     测点单位
        /// </summary>
        private string PointDw = "";

        ///// <summary>
        /////     测点号
        ///// </summary>
        //private string PointID = "";

        /// <summary>
        ///     测点ID列表
        /// </summary>
        public List<string> PointIDList = new List<string>();

        private DateTime SzNameE;

        private DateTime SzNameS;

        /// <summary>
        ///     安装位置ID列表
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
        /// 是否选择测点自动查询
        /// </summary>
        private bool autoQuery = false;
        public Mnl_McData()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            InitializeComponent();
        }

        public Mnl_McData(Dictionary<string, string> param)
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            InitializeComponent();
            if ((param != null) && (param.Count > 0))
                try
                {
                    if (param.ContainsKey("PointID") && !string.IsNullOrEmpty(param["PointID"]))
                        //PointID = param["PointID"];
                        CurrentPointID = param["PointID"];
                }
                catch
                {
                    //PointID = "";
                    CurrentPointID = "";
                }
            else
                return;
        }
        public void Reload(Dictionary<string, string> param)
        {
            //PointID = "";
            CurrentPointID = "";
            if ((param != null) && (param.Count > 0))
                try
                {
                    if (param.ContainsKey("PointID") && !string.IsNullOrEmpty(param["PointID"]))
                        //PointID = param["PointID"];
                        CurrentPointID = param["PointID"];
                }
                catch
                {
                    //PointID = "";
                    CurrentPointID = "";
                }
            object sender1 = null;
            var e1 = new EventArgs();
            Frm_MnlMcChart_Load(sender1, e1);
        }


        /// <summary>
        ///     坐标系
        /// </summary>
        private SwiftPlotDiagram Diagram
        {
            get { return chart.Diagram as SwiftPlotDiagram; }
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
        ///     密采值
        /// </summary>
        private Series Series1
        {
            get { return chart.Series[0]; }
        }

        /// <summary>
        ///     移动值
        /// </summary>
        private Series Series2
        {
            get { return chart.Series[1]; }
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
                if (e.Button == MouseButtons.Left)
                {
                    // 20170627
                    //_p0 = e.Location;
                    //SetLocationClientValue(_p0, ref _x0, ref _y0, ref _x00, ref _y00);
                    //_isScale = true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_McLine_DevUCChart_MouseDown" + ex.Message + ex.StackTrace);
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
                LogHelper.Error("Mnl_McLine_SetLocationClientValue" + ex.Message + ex.StackTrace);
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
                LogHelper.Error("Mnl_McLine_DevUCChart_MouseMove" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     释放左键事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DevUCChart_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                var diag = Diagram;
                if (e.Button == MouseButtons.Left) //右键框选查询密采
                {
                    // 20170627
                    //if (Series1.Points.Count > 0)
                    //{
                    //    _p1 = e.Location;
                    //    SetLocationClientValue(_p1, ref _x1, ref _y1, ref _x11, ref _y11);

                    //    if ((_p1.X == _p0.X) && (_p1.Y == _p0.Y)) //没有拖动操作
                    //        return;
                    //    if ((_p1.X > _p0.X) && (_p1.Y > _p0.Y)) //右下拖动，显示密采数据
                    //    {
                    //        if (string.IsNullOrEmpty(_x0.ToString()))
                    //            _x0 = SzNameS;
                    //        if (string.IsNullOrEmpty(_x1.ToString()))
                    //            _x1 = SzNameE;

                    //        diag.AxisX.WholeRange.SetMinMaxValues(_x0, _x1);
                    //        diag.AxisX.VisualRange.SetMinMaxValues(_x0, _x1);
                    //        SzNameS = DateTime.Parse(_x0.ToString());
                    //        SzNameE = DateTime.Parse(_x1.ToString());
                    //    }
                    //}
                }
                else if (e.Button == MouseButtons.Right) //右键还原
                {
                    object sender1 = null;
                    var e1 = new EventArgs();
                    simpleButton1_Click(sender1, e1);
                }
                _isScale = false;
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_McLine_DevUCChart_MouseUp" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     加载所有曲线数据
        /// </summary>
        protected void InitControls(DataTable dt, string lineName)
        {
            LoadSeries(Series1, lineName, "A", dt);
            LoadSeries(Series2, "移动值", "B", dt);
        }

        /// <summary>
        ///     测点曲线加载
        /// </summary>
        /// <param name="series"></param>
        /// <param name="name"></param>
        /// <param name="shortName"></param>
        private void LoadSeries(Series series, string name, string shortName, DataTable dt)
        {
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
                LogHelper.Error("Mnl_McLine_LoadPoints" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm_MnlMcChart_Load(object sender, EventArgs e)
        {
            try
            {
                deviceDefineService = ServiceFactory.Create<IDeviceDefineService>();
                //设置窗体高度和宽度
                Width = Convert.ToInt32(Screen.GetWorkingArea(this).Width * 0.9);
                Height = Convert.ToInt32(Screen.GetWorkingArea(this).Height * 0.9);
                Left = Convert.ToInt32(Screen.GetWorkingArea(this).Width * 0.1 / 2);
                Top = Convert.ToInt32(Screen.GetWorkingArea(this).Height * 0.1 / 2);

                //设置左边查询条件宽度
                layoutControlItem1.Width = 320;

                //初始化控件值
                DateTime timenow = DateTime.Now;
                dateEdit1.DateTime = DateTime.Now.AddHours(-4);
                dateEdit2.DateTime = DateTime.Now;
                #region//加载曲线颜色

                InterfaceClass.QueryPubClass_.SetChartColor(Series1, "Chart_McColor");
                InterfaceClass.QueryPubClass_.SetChartColor(Series2, "Chart_YdzColor");
                InterfaceClass.QueryPubClass_.SetBigDataChartBgColor(Diagram, "Chart_BgColor");

                chartSetting = InterfaceClass.QueryPubClass_.GetChartColorSetting();

                #endregion
                Series1.Points.Clear();
                Series2.Points.Clear();
                gridControl2.DataSource = null;


                dateEdit1.Properties.VistaDisplayMode = DefaultBoolean.True;
                dateEdit1.Properties.VistaEditTime = DefaultBoolean.True;
                this.dateEdit1.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
                this.dateEdit1.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                this.dateEdit1.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
                this.dateEdit1.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                this.dateEdit1.Properties.Mask.EditMask = "yyyy-MM-dd HH:mm:ss";

                dateEdit2.Properties.VistaDisplayMode = DefaultBoolean.True;
                dateEdit2.Properties.VistaEditTime = DefaultBoolean.True;
                this.dateEdit2.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
                this.dateEdit2.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                this.dateEdit2.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
                this.dateEdit2.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                this.dateEdit2.Properties.Mask.EditMask = "yyyy-MM-dd HH:mm:ss";

                LoadPointList(true);
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_McLine_Frm_MnlMcChart_Load" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     日期选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dateEdit1_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
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
                LogHelper.Error("Mnl_McLine_dateEdit1_EditValueChanged" + ex.Message + ex.StackTrace);
            }
        }

        private void LoadPointList(bool loadData)
        {
            var wdf = new WaitDialogForm("正在加载数据...", "请等待...");
            try
            {
                //Thread.Sleep(500);


                LoadPointSelList(dateEdit1.DateTime, dateEdit1.DateTime, loadData);
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_McLine_dateEdit1_EditValueChanged" + ex.Message + ex.StackTrace);
            }
            if (wdf != null)
                wdf.Close();
        }

        /// <summary>
        ///     加载测点选择列表
        /// </summary>
        /// <param name="stime"></param>
        /// <param name="etime"></param>
        private void LoadPointSelList(DateTime stime, DateTime etime, bool loadData)
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
                //{
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
                if (!string.IsNullOrEmpty(CurrentPointID))
                {
                    var isinlist = false;
                    //给combox赋初始值
                    for (var i = 0; i < PointIDList.Count; i++)
                    {
                        if (PointIDList[i].Contains(CurrentPointID))
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
                LogHelper.Error("Mnl_McLine_LoadPointSelList" + ex.Message + ex.StackTrace);
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
                //SzNameS = DateTime.Parse(dateEdit1.DateTime.ToShortDateString());
                //SzNameE = DateTime.Parse(dateEdit1.DateTime.ToShortDateString() + " 23:59:59");
                SzNameS = dateEdit1.DateTime;
                SzNameE = dateEdit2.DateTime;

                if (SzNameS > SzNameE)
                {
                    if (wdf != null)
                        wdf.Close();
                    XtraMessageBox.Show("开始时间不能大于结束时间！");
                    return;
                }
                if (SzNameE > DateTime.Now)
                {
                    SzNameE = DateTime.Now;
                }
                if ((int)((SzNameE - SzNameS).TotalSeconds) > 4 * 60 * 60)
                {
                    if (wdf != null)
                        wdf.Close();
                    XtraMessageBox.Show("本查询最多支持查询4小时数据，请重新选择起止时间！");
                    return;
                }

                if (comboBoxEdit1.SelectedIndex < 0)
                {
                    if (wdf != null)
                        wdf.Close();
                    XtraMessageBox.Show("请选择测点！");
                    return;
                }

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();

                double MaxLC2 = 0;
                double MinLC2 = 0;

                CurrentPointID = PointIDList[comboBoxEdit1.SelectedIndex];
                CurrentDevid = DevList[comboBoxEdit1.SelectedIndex];
                CurrentWzid = WzList[comboBoxEdit1.SelectedIndex];
                var TimeTick = "";
                TimeTick = comboBoxEdit2.SelectedItem.ToString();

                bool isQueryByPoint = false;
                if (QueryByPoint.Checked)
                {
                    isQueryByPoint = true;
                }

                dt_line = InterfaceClass.McLineQueryClass_.GetMcData(SzNameS, SzNameE, false, CurrentPointID,
                    CurrentDevid, CurrentWzid, TimeTick, ref MaxLC2, ref MinLC2, isQueryByPoint);

                stopwatch.Stop();
                LogHelper.Debug("查询数据" + dt_line.Rows.Count + "，耗时：" + stopwatch.ElapsedMilliseconds);

                //if (checkEdit2.Checked)
                //{
                //    stopwatch.Restart();
                //    if (dt_line.Rows.Count > 0)
                //    {
                //        DeviceDefineGetByDevIdRequest deviceDefineGetByDevIdRequest = new DeviceDefineGetByDevIdRequest();
                //        //deviceDefineGetByDevIdRequest.DevId = dt_line.Rows[0]["type"].ToString();
                //        deviceDefineGetByDevIdRequest.DevId = CurrentDevid;
                //        var result = deviceDefineService.GetDeviceDefineCacheByDevId(deviceDefineGetByDevIdRequest);
                //        if (result != null && result.IsSuccess)
                //        {
                //            dt_line = DataFilter(dt_line, result.Data);
                //        }
                //    }
                //    stopwatch.Stop();
                //    LogHelper.Debug("数据筛选" + dt_line.Rows.Count + "，耗时：" + stopwatch.ElapsedMilliseconds);
                //}


                stopwatch.Restart();

                // 20170627
                DataView dataView = dt_line.DefaultView;
                dataView.Sort = "Timer asc";
                dt_line = dataView.ToTable();

                var MaxValue = (float)MaxLC2 * 1.2f;

                threshold.Clear();
                threshold = InterfaceClass.QueryPubClass_.GetZFromTable(CurrentPointID);
                //var tempList = InterfaceClass.QueryPubClass_.GetZFromTable(CurrentPointID);
                foreach (var tempMax in threshold)
                    if (MaxValue < tempMax) //表示当天没值
                        MaxValue = tempMax;

                //读取量程低
                var MinValue = (float)MinLC2;
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


                //_minX = DateTime.Parse(SzNameS.ToShortDateString());
                //_maxX = DateTime.Parse(SzNameE.ToShortDateString() + " 23:59:59");



                var name = comboBoxEdit1.SelectedItem.ToString();
                var shortName = name.Substring(0, name.IndexOf('.'));

                chart.Titles[0].Text = comboBoxEdit1.SelectedItem.ToString() + "，最大值：" + (((float)MaxLC2).ToString() == "-9999" ? "-" : ((float)MaxLC2).ToString())
                    + "，最小值：" + (((float)MinLC2).ToString() == "9999" ? "-" : ((float)MinLC2).ToString());
                InitControls(dt_line, TimeTick);

                //var _minX = DateTime.Parse(SzNameS.ToShortDateString());
                //var _maxX = DateTime.Parse(SzNameS.ToShortDateString() + " 23:59:59");
                var _minX = dateEdit1.DateTime;
                var _maxX = dateEdit2.DateTime;

                //添加阈值线

                var tempZ = PubOptClass.AddZSeries(dt_line, InterfaceClass.QueryPubClass_.GetZFromTable(CurrentPointID),
                    null, null);
                var isinchart = false;
                if (tempZ.Count > 0)
                {
                    foreach (var serie in tempZ)
                    {
                        isinchart = false;
                        for (var i = chart.Series.Count - 1; i >= 0; i--)
                        {
                            var chartserie = chart.Series[i];
                            if (chartserie.Name == serie.Name)
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
                    for (var i = chart.Series.Count - 1; i >= 2; i--)
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

                AxisX.WholeRange.SetMinMaxValues(_minX, _maxX);
                AxisX.VisualRange.SetMinMaxValues(_minX, _maxX);

                #region//设置曲线显示隐藏

                for (var i = chart.Series.Count - 1; i >= 0; i--)
                {
                    var chartserie = chart.Series[i];
                    switch (chartserie.Name)
                    {
                        case "预警阈值":
                        case "下限预警阈值":
                            if (checkEdit13.Checked)
                                chartserie.Visible = true;
                            else
                                chartserie.Visible = false;
                            break;
                        case "报警阈值":
                        case "下限报警阈值":
                            if (checkEdit12.Checked)
                                chartserie.Visible = true;
                            else
                                chartserie.Visible = false;
                            break;
                        case "断电阈值":
                        case "下限断电阈值":
                            if (checkEdit11.Checked)
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

                //进行密采曲线补点处理
                //dt_line.DefaultView.Sort = "Timer desc";
                //dt_line = dt_line.DefaultView.ToTable();
                string point = comboBoxEdit1.SelectedItem.ToString().Substring(0, comboBoxEdit1.SelectedItem.ToString().IndexOf('.'));
                string wz = comboBoxEdit1.SelectedItem.ToString().Substring(comboBoxEdit1.SelectedItem.ToString().IndexOf('.')+1, comboBoxEdit1.SelectedItem.ToString().IndexOf('[')-
                    comboBoxEdit1.SelectedItem.ToString().IndexOf('.')-1);
                string devName = comboBoxEdit1.SelectedItem.ToString().Substring(comboBoxEdit1.SelectedItem.ToString().IndexOf('[') + 1, comboBoxEdit1.SelectedItem.ToString().LastIndexOf(']')
                    - comboBoxEdit1.SelectedItem.ToString().IndexOf('[')-1);
                DataTable tempdt = new DataTable();
                tempdt.Columns.Add("A");
                tempdt.Columns.Add("B");
                tempdt.Columns.Add("Timer");
                tempdt.Columns.Add("state");
                tempdt.Columns.Add("statetext");
                tempdt.Columns.Add("type");
                tempdt.Columns.Add("typetext");
                tempdt.Columns.Add("Point");
                tempdt.Columns.Add("Wz");
                tempdt.Columns.Add("DevName");
                DataTable tempDt1 = dt_line.Clone();
                tempDt1 = InterfaceClass.McLineQueryClass_.GetMcData(DateTime.Parse(SzNameS.ToShortDateString()), SzNameE, false, CurrentPointID,
                    CurrentDevid, CurrentWzid, TimeTick, ref MaxLC2, ref MinLC2, isQueryByPoint);
                for (DateTime stime = SzNameS; stime <= SzNameE; stime = stime.AddSeconds(2))
                {
                    DataRow[] dr = dt_line.Select("Timer<='" + stime.ToString("yyyy-MM-dd HH:mm:ss") + "'", "Timer desc");
                    if (dr.Length < 1)
                    {
                        dr = tempDt1.Select("Timer<='" + stime.ToString("yyyy-MM-dd HH:mm:ss") + "'", "Timer desc");
                    }
                    if (dr.Length > 0)
                    {
                        if (dr[0]["state"].ToString() == "21")
                        {
                            object[] obj = new object[tempdt.Columns.Count];
                            obj[0] = dr[0][0].ToString() + PointDw;
                            obj[1] = dr[0][1].ToString();
                            obj[2] = stime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                            obj[3] = dr[0][3].ToString();
                            obj[4] = dr[0][4].ToString();
                            obj[5] = dr[0][5].ToString();
                            obj[6] = dr[0][6].ToString();
                            obj[7] = point;
                            obj[8] = wz;
                            obj[9] = devName;
                            tempdt.Rows.Add(obj);
                        }
                        else
                        {
                            DataRow[] dr1 = tempdt.Select("Timer='" + dr[0]["Timer"].ToString() + "'");
                            if (dr1.Length < 1)
                            {
                                object[] obj = new object[tempdt.Columns.Count];
                                obj[0] = dr[0][0].ToString();
                                obj[1] = dr[0][1].ToString();
                                obj[2] = dr[0][2].ToString();
                                obj[3] = dr[0][3].ToString();
                                obj[4] = dr[0][4].ToString();
                                obj[5] = dr[0][5].ToString();
                                obj[6] = dr[0][6].ToString();
                                obj[7] = point;
                                obj[8] = wz;
                                obj[9] = devName;
                                tempdt.Rows.Add(obj);
                            }
                        }
                    }
                }
                //加载列表
                gridControl2.DataSource = tempdt;

                stopwatch.Stop();
                LogHelper.Info("曲线加载" + dt_line.Rows.Count + "，耗时：" + stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_McLine_simpleButton1_Click" + ex.Message + ex.StackTrace);
            }
            if (wdf != null)
                wdf.Close();
        }

        private DataTable DataFilter(DataTable oldDt, Jc_DevInfo dev)
        {
            DataTable newDt = new DataTable();
            double maxValue = 0;
            double minValue = 0;
            newDt = oldDt.Clone();
            double lastValue = 0;
            double tempValue = 0;
            //dtR.Columns.Add("A");
            //dtR.Columns.Add("B");
            //dtR.Columns.Add("Timer");
            //dtR.Columns.Add("state");
            //dtR.Columns.Add("statetext");
            //dtR.Columns.Add("type");
            //dtR.Columns.Add("typetext");
            //foreach (DataRow dr in oldDt.Rows)
            double ChangeRate = 0.01;
            if (dev.Bz3 == 104 || dev.Bz3 == 96 || dev.Bz3 == 48)//风速、氧气、温度按5%变化进行过滤  20171222
            {
                ChangeRate = 0.05;
            }
            if (oldDt.Rows.Count > 0)
            {
                if (double.TryParse(oldDt.Rows[0]["A"].ToString(), out tempValue))
                {
                    maxValue = tempValue;
                    minValue = tempValue;
                }
            }
            DataRow dr;
            for (int i = 0; i < oldDt.Rows.Count; i++)
            {
                dr = oldDt.Rows[i];

                if (double.TryParse(dr["A"].ToString(), out tempValue) && (dr["state"].ToString() == "21") && (i != oldDt.Rows.Count - 1))//正常数据才处理，其他数据直接显示
                {
                    if (ChangeRateStore(ChangeRate, tempValue, dev, ref  maxValue, ref  minValue, ref lastValue))
                    {
                        newDt.Rows.Add(dr["A"], dr["B"], dr["Timer"], dr["state"], dr["statetext"], dr["type"], dr["typetext"]);
                    }
                }
                else
                {
                    lastValue = 0.00001;
                    newDt.Rows.Add(dr["A"], dr["B"], dr["Timer"], dr["state"], dr["statetext"], dr["type"], dr["typetext"]);
                }
            }

            return newDt;
        }
        protected bool ChangeRateStore(double ChangeRate, double value, Jc_DevInfo DevItems, ref  double maxValue, ref double minValue, ref  double lastValue)
        {
            bool flag = true;
            try
            {
                if (DevItems != null)
                {
                    //double ChangeRate = 0.02;
                    if (DevItems.LC2 > 0) //如果有中间量程
                    {
                        if (value < DevItems.LC2)
                        {
                            ChangeRate = DevItems.LC2 * ChangeRate;
                        }
                        else
                        {
                            ChangeRate = DevItems.LC * ChangeRate;
                        }
                    }
                    else
                    {
                        ChangeRate = DevItems.LC * ChangeRate;
                    }

                    if (value > maxValue)
                    {
                        flag = true;
                        maxValue = value;
                        lastValue = value;
                    }
                    else if (value < minValue)
                    {
                        flag = true;
                        minValue = value;
                        lastValue = value;
                    }
                    else if (Math.Abs(value - lastValue) >= ChangeRate)
                    {
                        flag = true;
                        lastValue = value;
                    }
                    else
                    {
                        flag = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info("ChangeRateStore:" + ex.Message);
            }

            return flag;
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
                LogHelper.Error("Mnl_McLine_simpleButton4_Click" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     导出图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "模拟量密采曲线.png";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                ExportToCore(saveFileDialog1.FileName, "png");
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
                LogHelper.Error("Mnl_McLine_ExportToCore" + ex.Message + ex.StackTrace);
            }
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
                    SelTime = DateTime.Parse(point.ArgumentSerializable).ToString("yyyy-MM-dd HH:mm:ss.fff");
                    if (element.Series.Name.Contains("移动值")
                        || element.Series.Name.Contains("密采值")
                    )
                    {
                        drs = dt_line.Select("Timer='" + SelTime + "' ");
                        if (drs.Length > 0)
                            if ((drs[0]["type"].ToString() == "20") ||
                                 (drs[0]["type"].ToString() == "22") ||
                                 (drs[0]["type"].ToString() == "23") ||
                                 (drs[0]["type"].ToString() == "33") ||
                                 (drs[0]["type"].ToString() == "34") ||
                                (drs[0]["type"].ToString() == "46"))
                            {
                                ShowText = element.Series.Name + ":" + drs[0]["typetext"];
                            }
                            else if (drs[0]["state"].ToString() == "28")
                            {
                                ShowText = element.Series.Name + ":" + drs[0]["statetext"];
                            }
                            else
                            {
                                if (point.Values[0].ToString() == "1E-05")
                                    ShowText = element.Series.Name + ":" + "系统退出";
                                else if (point.Values[0].ToString() == "2E-05")
                                    ShowText = element.Series.Name + ":" + "分站中断";
                                else if (point.Values[0].ToString() == "3E-05")
                                    ShowText = element.Series.Name + ":" + "断线";
                                else if (point.Values[0].ToString() == "4E-05")
                                    ShowText = element.Series.Name + ":" + "上溢";
                                else if (point.Values[0].ToString() == "5E-05")
                                    ShowText = element.Series.Name + ":" + "负漂";
                                else if (point.Values[0].ToString() == "6E-05")
                                    ShowText = element.Series.Name + ":" + "头子断线";
                                else if (point.Values[0].ToString() == "7E-05")
                                    ShowText = element.Series.Name + ":" + "类型有误";
                                else if (point.Values[0].ToString() == "8E-05")
                                    ShowText = element.Series.Name + ":" + "开机";
                                else
                                {
                                    if (drs[0]["state"].ToString() == "42" || drs[0]["state"].ToString() == "24")//线性突变、标效
                                    {
                                        ShowText = drs[0]["typetext"] + "\n";
                                        ShowText += drs[0]["statetext"] + "\n";
                                    }
                                    else
                                    {
                                        ShowText = drs[0]["typetext"] + "\n";
                                    }
                                    ShowText += element.Series.Name + ":" + point.Values[0].ToString("f2") + PointDw;
                                }
                            }
                    }
                    else
                    {
                        ShowText = element.Series.Name + ":" + point.Values[0].ToString("f2") + PointDw;
                    }
                    if (index == 0)
                        element.LabelElement.HeaderText = "时间:" + SelTime + "\n";


                    element.LabelElement.Text = ShowText; //显示要显示的文字   

                    index++;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_McLine_chart_CustomDrawCrosshair" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     将值转换成汉字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView2_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                var view = sender as GridView;
                if (e.Column.FieldName == "A")
                {
                    var aa = view.GetRowCellDisplayText(e.RowHandle, view.Columns["A"]);
                    if (aa.Contains("0.00001"))
                    {
                        e.Appearance.ForeColor = Color.Red;
                        view.SetRowCellValue(e.RowHandle, e.Column, "系统退出");
                    }
                    else if (aa.Contains("0.00002"))
                    {
                        e.Appearance.ForeColor = Color.Red;
                        view.SetRowCellValue(e.RowHandle, e.Column, "分站中断");
                    }
                    else if (aa.Contains("0.00003"))
                    {
                        e.Appearance.ForeColor = Color.Red;
                        view.SetRowCellValue(e.RowHandle, e.Column, "断线");
                    }
                    else if (aa.Contains("0.00004"))
                    {
                        e.Appearance.ForeColor = Color.Red;
                        view.SetRowCellValue(e.RowHandle, e.Column, "上溢");
                    }
                    else if (aa.Contains("0.00005"))
                    {
                        e.Appearance.ForeColor = Color.Red;
                        view.SetRowCellValue(e.RowHandle, e.Column, "负漂");
                    }
                    else if (aa.Contains("0.00006"))
                    {
                        e.Appearance.ForeColor = Color.Red;
                        view.SetRowCellValue(e.RowHandle, e.Column, "头子断线");
                    }
                    else if (aa.Contains("0.00007"))
                    {
                        e.Appearance.ForeColor = Color.Red;
                        view.SetRowCellValue(e.RowHandle, e.Column, "类型有误");
                    }
                    else if (aa.Contains("0.00008"))
                    {
                        e.Appearance.ForeColor = Color.Red;
                        view.SetRowCellValue(e.RowHandle, e.Column, "未知");
                    }
                    else
                    {
                        e.Appearance.ForeColor = Color.Black;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_McLine_gridView2_RowCellStyle" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     预警值显示设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEdit13_CheckedChanged(object sender, EventArgs e)
        {
            for (var i = chart.Series.Count - 1; i >= 0; i--)
            {
                var chartserie = chart.Series[i];
                if (chartserie.Name.Contains("预警阈值")) //
                    chartserie.Visible = checkEdit13.Checked;
            }
        }

        /// <summary>
        ///     报警值显示设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEdit12_CheckedChanged(object sender, EventArgs e)
        {
            for (var i = chart.Series.Count - 1; i >= 0; i--)
            {
                var chartserie = chart.Series[i];
                if (chartserie.Name.Contains("报警阈值")) //
                    chartserie.Visible = checkEdit12.Checked;
            }
        }

        /// <summary>
        ///     断电值显示设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEdit11_CheckedChanged(object sender, EventArgs e)
        {
            for (var i = chart.Series.Count - 1; i >= 0; i--)
            {
                var chartserie = chart.Series[i];
                if (chartserie.Name.Contains("断电阈值")) //
                    chartserie.Visible = checkEdit11.Checked;
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


        #region ----查询前一天、后一天记录 2017.7.30 by----

        private void btn_pre_Click(object sender, EventArgs e)
        {
            checkOhterTime(-1);
        }
        private void btn_next_Click(object sender, EventArgs e)
        {
            checkOhterTime(1);
        }

        private void checkOhterTime(int days)
        {
            dateEdit1.DateTime = dateEdit1.DateTime.AddDays(days);
            dateEdit2.DateTime = dateEdit2.DateTime.AddDays(days);
            simpleButton1_Click(new object(), new EventArgs());
        }

        #endregion

        private void chart_CustomDrawSeriesPoint(object sender, CustomDrawSeriesPointEventArgs e)
        {
            var value = e.SeriesPoint.Values;

            var pointtimer = DateTime.Parse(e.SeriesPoint.ArgumentSerializable).ToString("yyyy-MM-dd HH:mm:ss.fff");

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
                else if (type == "42")
                {
                    if (chartSetting.Select("strKey='Chart_PseudoColor'").Length > 0)
                    {
                        Color bxColor = Color.FromArgb(int.Parse(chartSetting.Select("strKey='Chart_PseudoColor'")[0]["strValue"].ToString()));
                        e.SeriesDrawOptions.Color = bxColor;
                    }
                    else
                    {
                        e.SeriesDrawOptions.Color = Color.Blue;
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

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit1.Checked)
            {
                LineSeriesView lineview1 = (LineSeriesView)Series1.View;
                lineview1.MarkerVisibility = DefaultBoolean.True;
                lineview1.LineMarkerOptions.Size = 10;

                LineSeriesView lineview2 = (LineSeriesView)Series2.View;
                lineview2.MarkerVisibility = DefaultBoolean.True;
                lineview2.LineMarkerOptions.Size = 10;
            }
            else
            {
                LineSeriesView lineview1 = (LineSeriesView)Series1.View;
                lineview1.MarkerVisibility = DefaultBoolean.False;

                LineSeriesView lineview2 = (LineSeriesView)Series2.View;
                lineview2.MarkerVisibility = DefaultBoolean.False;
            }
        }

        private void checkEdit3_CheckedChanged(object sender, EventArgs e)
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

        private void dateEdit2_EditValueChanged(object sender, EventArgs e)
        {
            try
            {

                if (radioGroup1.SelectedIndex == 1)
                {//如果按已存储测点查询，则每次选择时间后，重新加载 
                    LoadPointList(false);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_McLine_dateEdit1_EditValueChanged" + ex.Message + ex.StackTrace);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter =
                    "Excel文件(*.xls)|*.xls|PDF文件(*.pdf)|*.pdf|TXT文件(*.txt)|*.txt|CSV文件(*.csv)|*.csv|HTML文件(*.html)|*.html";
                saveFileDialog.Title = "保存文件";
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.FileName = Text;


                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ////列表方式
                    gridControl2.ExportToXls(saveFileDialog.FileName, false);

                    if (DialogResult.Yes ==
                        MessageBox.Show("是否立即打开此文件?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                        Process.Start(saveFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message + ex.StackTrace);
            }
        }
    }
}