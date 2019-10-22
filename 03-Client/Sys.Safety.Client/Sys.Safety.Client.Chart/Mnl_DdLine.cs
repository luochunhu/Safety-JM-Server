using System;
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

namespace Sys.Safety.Client.Chart
{
    public partial class Mnl_DdLine : XtraForm
    {
        private bool _isScale; //鼠标左键是否按下

        private object _maxX, _minX, _maxY, _minY, _maxX1, _minX1, _maxY1, _minY1;
            //原图X轴和Y轴的最大和最小值以及第二坐标轴（如果有）X轴和Y轴的最大和最小值

        private Point _p0 = new Point();
        private Point _p1 = new Point();

        ///// <summary>
        /////     传入测点ID（参数）
        ///// </summary>
        //private string _PointID = "";

        private object _x0, _x1, _y0, _y1;
        private object _x00, _y00, _x11, _y11;

        /// <summary>
        ///     当前设备类型ID
        /// </summary>
        private string CurrentDevid = "0";

        /// <summary>
        ///     当前测点编号ID
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

        private int IscheckDate;

        private bool IsInIframe = false;

        /// <summary>
        ///     是否计算未知状态
        /// </summary>
        private readonly bool kglztjsfs = true;

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
        ///     测点编号ID列表
        /// </summary>
        public List<string> PointIDList = new List<string>();

        private DateTime SzNameE;

        private DateTime SzNameS;

        /// <summary>
        ///     安装位置ID列表
        /// </summary>
        public List<string> WzList = new List<string>();

        public Mnl_DdLine()
        {
            InitializeComponent();
        }

        public Mnl_DdLine(Dictionary<string, string> param)
        {
            InitializeComponent();
            if ((param != null) && (param.Count > 0))
                try
                {
                    if (!string.IsNullOrEmpty(param["PointID"]))
                        //_PointID = param["PointID"];
                        CurrentPointID = param["PointID"];
                }
                catch
                {
                    //_PointID = "";
                    CurrentPointID = "";
                }
            else
                return;
        }
        public void Reload(Dictionary<string, string> param)
        {
            //_PointID = "";
            CurrentPointID = "";
            if ((param != null) && (param.Count > 0))
                try
                {
                    if (param.ContainsKey("PointID") && !string.IsNullOrEmpty(param["PointID"]))
                        //_PointID = param["PointID"];
                        CurrentPointID = param["PointID"];
                }
                catch
                {
                    //_PointID = "";
                    CurrentPointID = "";
                }
            object sender1 = null;
            var e1 = new EventArgs();
            Mnl_DdLine_Load(sender1, e1);
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
        ///     单击左键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DevUCChart_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                //if (e.Button == MouseButtons.Left)
                //{
                //    _p0 = e.Location;
                //    SetLocationClientValue(_p0, ref  _x0, ref  _y0, ref _x00, ref _y00);
                //    _isScale = true;
                //}
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateLine_DevUCChart_MouseDown" + ex.Message + ex.StackTrace);
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
                LogHelper.Error("Kgl_StateLine_SetLocationClientValue" + ex.Message + ex.StackTrace);
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
                //if (_isScale)
                //{
                //    Point pE = new Point(e.Location.X - this.Location.X, e.Location.Y - this.Location.Y);
                //    Graphics g = Graphics.FromHwnd(this.chart.Handle);
                //    Pen pen = new Pen(new SolidBrush(Color.Red));
                //    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                //    g.DrawRectangle(pen, _p0.X, _p0.Y, e.Location.X - _p0.X, e.Location.Y - _p0.Y);
                //}
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateLine_DevUCChart_MouseMove" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     释放左键事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DevUCChart_MouseUp(object sender, MouseEventArgs e)
        {
            var diag = Diagram;
            try
            {
                //if (e.Button == MouseButtons.Left)//右键框选查询密采
                //{
                //    _p1 = e.Location;
                //    SetLocationClientValue(_p1, ref  _x1, ref  _y1, ref _x11, ref _y11);

                //    if (_p1.X == _p0.X && _p1.Y == _p0.Y)//没有拖动操作
                //    {
                //        return;
                //    }
                //    if (_p1.X > _p0.X && _p1.Y > _p0.Y)//右下拖动，显示密采数据
                //    {
                //        DateTime SzNameS1 = new DateTime(), SzNameE1 = new DateTime();
                //        if (_x0.ToString().Length > 8 && _x1.ToString().Length > 8)
                //        {
                //            SzNameS1 = DateTime.Parse(_x0.ToString());
                //            SzNameE1 = DateTime.Parse(_x1.ToString());
                //        }
                //        else if (string.IsNullOrEmpty(_x0.ToString()) && string.IsNullOrEmpty(_x1.ToString()))
                //        {
                //            SzNameS1 = DateTime.Parse(SzNameS.ToShortDateString());
                //            SzNameE1 = DateTime.Parse(SzNameS.ToShortDateString() + " 23:59:59");
                //        }
                //        else if (string.IsNullOrEmpty(_x1.ToString()))
                //        {
                //            SzNameS1 = DateTime.Parse(_x0.ToString());
                //            SzNameE1 = DateTime.Parse(SzNameS.ToShortDateString() + " 23:59:59");
                //        }
                //        else if (string.IsNullOrEmpty(_x0.ToString()))
                //        {
                //            SzNameS1 = DateTime.Parse(SzNameS.ToShortDateString());
                //            SzNameE1 = DateTime.Parse(_x1.ToString());
                //        }
                //        diag.AxisX.WholeRange.SetMinMaxValues(SzNameS1, SzNameE1);
                //        diag.AxisX.VisualRange.SetMinMaxValues(SzNameS1, SzNameE1);
                //    }
                //}
                //else if (e.Button == MouseButtons.Right)//右键还原
                //{
                //    object sender1 = null;
                //    EventArgs e1 = new EventArgs();
                //    simpleButton1_Click(sender1, e1);
                //}
                //_isScale = false;
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateLine_DevUCChart_MouseUp" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     加载所有曲线数据
        /// </summary>
        protected void InitControls(DataTable dt, string name)
        {
            LoadSeries(Series1, name, "C", dt);
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
                        series.Points.Add(new SeriesPoint(DateTime.Parse(dt.Rows[i]["sTimer"].ToString()), rate));
                    }

                    series.Points.EndUpdate();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateLine_LoadPoints" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     产生随机数(测试用)
        /// </summary>
        /// <param name="N"></param>
        /// <returns></returns>
        public string RandCode(int N)
        {
            char[] arrChar = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
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
        private void Mnl_DdLine_Load(object sender, EventArgs e)
        {
            try
            {
                //设置窗体高度和宽度
                Width = Convert.ToInt32(Screen.GetWorkingArea(this).Width*0.9);
                Height = Convert.ToInt32(Screen.GetWorkingArea(this).Height*0.9);
                Left = Convert.ToInt32(Screen.GetWorkingArea(this).Width*0.1/2);
                Top = Convert.ToInt32(Screen.GetWorkingArea(this).Height*0.1/2);

                //设置左边查询条件宽度
                layoutControlItem1.Width = 320;

                //初始化控件值
                dateEdit1.DateTime = DateTime.Now;
                dateEdit2.DateTime = DateTime.Now;
                Series1.Points.Clear();               
                gridControl1.DataSource = null;

                #region//加载曲线颜色                 

                InterfaceClass.QueryPubClass_.SetChartColor(Series1, "Chart_KglColor");
                InterfaceClass.QueryPubClass_.SetChartBgColor(Diagram, "Chart_BgColor");

                #endregion

                LoadPointList(true);
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateLine_Kgl_StateLine_Load" + ex.Message + ex.StackTrace);
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
                var value = "";
                DataRow[] drs = null;
                string SelTime = "";               

                foreach (var element in e.CrosshairElements)
                {
                    var point = element.SeriesPoint;
                    //foreach (CrosshairAxisLabelElement element1 in e.CrosshairAxisLabelElements)
                    //{
                    //    SelTimeNow = DateTime.Parse(element1.AxisValue.ToString());
                    //    //大于300毫秒或有组件显示或隐藏才进行重绘
                    //    TimeSpan mouseMoveTimeStep = System.DateTime.Now - lastMouseMoveTime;
                    //    if (mouseMoveTimeStep.TotalMilliseconds >= 2000)
                    //    {
                    //        lastMouseMoveTime = System.DateTime.Now;

                    //        ChartGridDis(SelTimeNow);
                    //        _isScale = false;
                    //    }
                    //}
                    //label1.Text = point.Argument.ToString();//显示要显示的文字
                    SelTime = DateTime.Parse(point.ArgumentSerializable).ToString("yyyy-MM-dd HH:mm:ss");
                    drs = dt_line.Select("sTimer='" + SelTime + "' ");
                    if (drs.Length > 0)
                    {
                        if (drs[0]["C"].ToString() == "0.00001")
                            value = "未记录";
                        else
                            value = drs[0]["stateName"].ToString();
                        ShowText += "测点号：" + element.Series.Name + "\n";
                        ShowText += "起止时刻：" + DateTime.Parse(drs[0]["sTimer"].ToString()).ToLongTimeString()
                                    + "-" + DateTime.Parse(drs[0]["eTimer"].ToString()).ToLongTimeString() + "\n";
                        ShowText += "状态：" + value + "\n";
                        //ShowText += "馈电状态：" + drs[0]["D"].ToString() + "\n";
                        //ShowText += "处理措施：" + drs[0]["E"].ToString();
                    }
                    element.LabelElement.Text = ShowText; //显示要显示的文字
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateLine_chart_CustomDrawCrosshair" + ex.Message + ex.StackTrace);
            }
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
                LogHelper.Error("Kgl_StateLine_simpleButton4_Click" + ex.Message + ex.StackTrace);
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
                LogHelper.Error("Kgl_StateLine_ExportToCore" + ex.Message + ex.StackTrace);
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
                if (ts.TotalDays > 30)
                {
                    if (wdf != null)
                        wdf.Close();
                    XtraMessageBox.Show("查询的最大天数为30天,请重新选择日期！");
                    return;
                }
                if (comboBoxEdit1.SelectedIndex < 0)
                {
                    if (wdf != null)
                        wdf.Close();
                    XtraMessageBox.Show("请选择测点！");
                    return;
                }


                CurrentPointID = PointIDList[comboBoxEdit1.SelectedIndex];
                //_PointID = CurrentPointID;
                CurrentDevid = DevList[comboBoxEdit1.SelectedIndex];
                CurrentWzid = WzList[comboBoxEdit1.SelectedIndex];


                dt_line = new DataTable();

                //循环查找所有天的数据
                //for (DateTime NTime = SzNameS; NTime <= SzNameE; NTime = NTime.AddDays(1))
                //{
                var tempDt = InterfaceClass.KglStateLineQueryClass_.getMnlBjDdLineDt(SzNameS, SzNameE, CurrentPointID,
                    CurrentDevid, CurrentWzid, "2");
                if (dt_line.Columns.Count < 1)
                    dt_line = tempDt.Clone();
                foreach (DataRow dr in tempDt.Rows)
                    dt_line.Rows.Add(dr.ItemArray);
                //}
                //List<string> pointDev = InterfaceClass.QueryPubClass_.getKglStateDev(CurrentDevid);
                var stateName = "1态:断电," + "0态:正常/断电解除";

                var name = comboBoxEdit1.SelectedItem + "(" + stateName + ")";
                var shortName = comboBoxEdit1.SelectedItem.ToString()
                    .Substring(0, comboBoxEdit1.SelectedItem.ToString().IndexOf("."));

                chart.Titles[0].Text = name;
                InitControls(dt_line, shortName);


                _minX = DateTime.Parse(SzNameS.ToShortDateString());
                _maxX = DateTime.Parse(SzNameE.ToShortDateString() + " 23:59:59");

                AxisX.WholeRange.SetMinMaxValues(_minX, _maxX);
                AxisX.VisualRange.SetMinMaxValues(_minX, _maxX);
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateLine_simpleButton1_Click" + ex.Message + ex.StackTrace);
            }
            if (wdf != null)
                wdf.Close();
        }

        private void LoadPointList(bool loadData)
        {
            var wdf = new WaitDialogForm("正在加载数据...", "请等待...");
            try
            {
                ////数据校验
                //var ts = dateEdit2.DateTime - dateEdit1.DateTime;
                //if ((ts.TotalDays > 31) || (ts.TotalDays < 0))
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
                LogHelper.Error("Kgl_StateChg_dateEdit2_EditValueChanged" + ex.Message + ex.StackTrace);
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
                LogHelper.Error("Kgl_StateChg_dateEdit1_EditValueChanged" + ex.Message + ex.StackTrace);
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
                //comboBoxEdit1.BeginInvoke(new Action(() =>
                //{
                comboBoxEdit1.Properties.Items.Clear();
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
                //}));
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
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateChg_LoadPointSelList" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     曲线移动显示列表数据
        /// </summary>
        private void ChartGridDis(DateTime SelTime)
        {
            try
            {
                var QueryStr = new string[11];
                var tempPointInf = new string[11];
                var DtRefresh = new DataTable();
                DtRefresh.Columns.Add("名称及类型", Type.GetType("System.String"));
                DtRefresh.Columns.Add("报警及断电状态", Type.GetType("System.String"));
                DtRefresh.Columns.Add("读值时区", Type.GetType("System.String"));
                DtRefresh.Columns.Add("读值时刻", Type.GetType("System.String"));
                DtRefresh.Columns.Add("开机效率", Type.GetType("System.String"));
                DtRefresh.Columns.Add("开机时间", Type.GetType("System.String"));
                DtRefresh.Columns.Add("开停次数", Type.GetType("System.String"));
                DtRefresh.Columns.Add("报警/解除", Type.GetType("System.String"));
                DtRefresh.Columns.Add("断电/复电", Type.GetType("System.String"));
                DtRefresh.Columns.Add("馈电状态", Type.GetType("System.String"));
                DtRefresh.Columns.Add("措施及时刻", Type.GetType("System.String"));

                QueryStr[0] =
                    comboBoxEdit1.SelectedItem.ToString()
                        .Substring(comboBoxEdit1.SelectedItem.ToString().IndexOf(".") + 1);
                QueryStr[2] = SelTime.ToShortDateString() + " " + SelTime.Hour + ":00" + "~" + (SelTime.Hour + 1) +
                              ":00";
                QueryStr[3] = SelTime.ToLongTimeString();
                var Dt1 = new DateTime();
                var dt2 = new DateTime();
                Dt1 = DateTime.Parse(SelTime.ToLongDateString());
                dt2 = DateTime.Parse(SelTime.ToLongDateString());
                Dt1 = Dt1.AddHours(SelTime.Hour);
                dt2 = dt2.AddHours(SelTime.Hour + 1);

                tempPointInf = InterfaceClass.KglStateLineQueryClass_.GetDgView(SelTime, CurrentPointID, CurrentDevid,
                    CurrentWzid, kglztjsfs);
                QueryStr[1] = tempPointInf[1];
                QueryStr[7] = tempPointInf[7];
                QueryStr[8] = tempPointInf[8];
                QueryStr[9] = tempPointInf[9];
                QueryStr[10] = tempPointInf[10];

                tempPointInf = InterfaceClass.KglStateLineQueryClass_.GetKjThings(Dt1, dt2, CurrentPointID, CurrentDevid,
                    CurrentWzid, kglztjsfs);
                QueryStr[4] = tempPointInf[4];
                QueryStr[5] = tempPointInf[5];
                QueryStr[6] = tempPointInf[6];


                DtRefresh.Clear();
                var Dr = DtRefresh.NewRow();
                for (var i = 0; i < 11; i++)
                    Dr[i] = QueryStr[i];
                DtRefresh.Rows.Add(Dr);
                gridControl1.DataSource = DtRefresh;
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateLine_ChartGridDis" + ex.Message + ex.StackTrace);
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
                saveFileDialog1.FileName = "模拟量断电曲线图.png";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    ExportToCore(saveFileDialog1.FileName, "png");
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateLine_simpleButton2_Click" + ex.Message + ex.StackTrace);
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

                ChartGridDis(SelTime);
            }
            catch (Exception ex)
            {
                LogHelper.Error("Kgl_StateLine_chart_MouseClick" + ex.Message + ex.StackTrace);
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

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            checkOhterTime(-1);
        }

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