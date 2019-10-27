using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using Basic.Framework.Common;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.ServiceContract.Chart;
using ItemCheckEventArgs = DevExpress.XtraEditors.Controls.ItemCheckEventArgs;

namespace Sys.Safety.Client.Chart
{
    public partial class Mnl_SSZChart : XtraForm
    {
        private IChartService _chartService = ServiceFactory.Create<IChartService>();

        /// <summary>
        ///     是否启动标记
        /// </summary>
        private bool? inProcess = null;

        public bool isRun;
        public bool isSuspended;

        /// <summary>
        ///     刷新线程
        /// </summary>
        public Thread m_RefThread;

        /// <summary>
        ///     当前最大值
        /// </summary>
        private double MaxValue;

        /// <summary>
        ///     当前最大值
        /// </summary>
        private double MinValue;

        /// <summary>
        ///     当前测点号
        /// </summary>
        private  List<string> points = new List<string>();

        /// <summary>
        ///     实时曲线值
        /// </summary>
        private List<double> values = new List<double>();



        public Mnl_SSZChart()
        {
            InitializeComponent();

           
        }

        public Mnl_SSZChart(List<string> pointlist)
        {
            InitializeComponent();

           
            //bug修正: 20170316
            lock (this)
            {
                points = pointlist;
            }
        }

        public Mnl_SSZChart(Dictionary<string, string> param)
        {
            InitializeComponent();

            

            //bug修正: 20170316
            if ((param != null) && (param.Count > 0))
            {
                //var jc_defList = ServiceFactory.CreateService<IChartService>().QueryAllPointCache().ToList();
                var res = _chartService.QueryAllPointCache();
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                var jc_defList = res.Data.ToList();

                var temppointIDs = param["PointID"].Split('|');
                foreach (var pointID in temppointIDs)
                    if (!string.IsNullOrEmpty(pointID))
                    {
                        var tempPoint = jc_defList.FindAll(a => a.PointID == pointID);
                        lock (this)
                        {
                            if (tempPoint.Count > 0)
                                points.Add(tempPoint[0].Point);
                        }
                    }
            }
        }
        public void Reload(Dictionary<string, string> param)
        {
          

            //bug修正: 20170316
            if ((param != null) && (param.Count > 0))
            {
                //var jc_defList = ServiceFactory.CreateService<IChartService>().QueryAllPointCache().ToList();
                var res = _chartService.QueryAllPointCache();
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                var jc_defList = res.Data.ToList();

                points.Clear();
                var temppointIDs = param["PointID"].Split('|');
                foreach (var pointID in temppointIDs)
                    if (!string.IsNullOrEmpty(pointID))
                    {
                        var tempPoint = jc_defList.FindAll(a => a.PointID == pointID);
                        lock (this)
                        {
                            if (tempPoint.Count > 0)
                                points.Add(tempPoint[0].Point);
                        }
                    }
            }
            object sender1 = null;
            var e1 = new EventArgs();
            Mnl_SSZChart_Load(sender1, e1);
        }
        

        private int _TimeInterval = 2;
        /// <summary>
        ///     曲线显示区域（分为单位）
        /// </summary>
        private int TimeInterval
        {
            get { return Convert.ToInt32(_TimeInterval); }
            set { _TimeInterval = value; }
        }

        /// <summary>
        ///     实时曲线
        /// </summary>
        private Series Series1
        {
            get { return chartControl.Series.Count > 0 ? chartControl.Series[0] : null; }
        }

        private Series Series2
        {
            get { return chartControl.Series.Count > 0 ? chartControl.Series[1] : null; }
        }

        private Series Series3
        {
            get { return chartControl.Series.Count > 0 ? chartControl.Series[2] : null; }
        }

        private Series Series4
        {
            get { return chartControl.Series.Count > 0 ? chartControl.Series[3] : null; }
        }

        private Series Series5
        {
            get { return chartControl.Series.Count > 0 ? chartControl.Series[4] : null; }
        }

        private Series Series6
        {
            get { return chartControl.Series.Count > 0 ? chartControl.Series[5] : null; }
        }


        /// <summary>
        ///     实时曲线Regression
        /// </summary>
        private RegressionLine Regression1
        {
            get { return GetRegressionLine(Series1); }
        }

        private RegressionLine Regression2
        {
            get { return GetRegressionLine(Series2); }
        }

        private RegressionLine Regression3
        {
            get { return GetRegressionLine(Series3); }
        }

        private RegressionLine Regression4
        {
            get { return GetRegressionLine(Series4); }
        }
        private RegressionLine Regression5
        {
            get { return GetRegressionLine(Series5); }
        }
        private RegressionLine Regression6
        {
            get { return GetRegressionLine(Series6); }
        }


        /// <summary>
        ///     获取RegressionLine
        /// </summary>
        /// <param name="series"></param>
        /// <returns></returns>
        private static RegressionLine GetRegressionLine(Series series)
        {
            if (series != null)
            {
                var swiftPlotView = series.View as SwiftPlotSeriesView;
                if (swiftPlotView != null)
                    foreach (Indicator indicator in swiftPlotView.Indicators)
                    {
                        var regressionLine = indicator as RegressionLine;
                        if (regressionLine != null)
                            return regressionLine;
                    }
            }
            return null;
        }

        /// <summary>
        ///     暂停/继续
        /// </summary>
        private void SetPauseResumeButtonText()
        {
            btnPauseResume.Text = isSuspended ? "继续" : "暂停";
        }

        /// <summary>
        ///     获取测点实时值
        /// </summary>
        /// <returns></returns>
        private List<double> CalculateNextValue()
        {
            var rvalue = new List<double>();

            try
            {
                var tempoint = "";

                //bug修正: 20170316
                lock (this)
                {
                    foreach (var point in points)
                        if (point.Length > 0)
                            if (point.Contains('.'))
                                tempoint += point.Substring(0, point.IndexOf(".")) + "|";
                            else tempoint += point + "|";
                }

                //if (tempoint.Contains("|"))
                //{
                //    tempoint = tempoint.Substring(0, tempoint.Length - 1).Replace("|", "','");
                //}
                //return value + (random.NextDouble() * 10.0 - 5.0);
                //string strSql = @"select ssz from jc_def where point in ('" + tempoint + "')";
                //DataTable dt = ServiceFactory.CreateService<IChartService>().GetDataTableBySQL(strSql);
                //从缓存中读取实时值

                //var dt = TypeConvert.ToDataTable(ServiceFactory.CreateService<IChartService>().QueryAllPointCache());
                var res = _chartService.QueryAllPointCache();
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }

                var dt = ObjectConverter.ToDataTable(res.Data);
                dt.DefaultView.Sort = "point ASC";

                dt = dt.DefaultView.ToTable();
                double value = 0;
                for (var i = 0; i < dt.Rows.Count; i++)
                    if (tempoint.Contains(dt.Rows[i]["point"].ToString()))
                    {
                        value = 0;
                        if (double.TryParse(dt.Rows[i]["ssz"].ToString(), out value))
                        {
                            if (value <= 0)
                                value = 0;
                        }
                        else
                        {
                            value = 0;
                        }
                        rvalue.Add(value);
                    }
                dt.Clear();
                dt.Dispose();
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_SSZChart_CalculateNextValue" + ex.Message + ex.StackTrace);
            }
            return rvalue;
        }

        /// <summary>
        ///     更新实时值
        /// </summary>
        private void UpdateValues()
        {
            values = CalculateNextValue();
        }

        /// <summary>
        ///     启动刷新
        /// </summary>
        protected void DoShow()
        {
            isRun = true;
        }

        /// <summary>
        ///     启动停止刷新
        /// </summary>
        protected void DoHide()
        {
            isRun = false;
        }

        /// <summary>
        ///     实时刷新方法
        /// </summary>
        private void RefClass()
        {
            var argument = DateTime.Now;
            var pointsToUpdates = new SeriesPoint[6];
            DateTime minDate;
            var pointsToRemoveCount = 0;
            SwiftPlotDiagram diagram;

            while (isRun)
            {
                try
                {
                    if (isSuspended)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }

                    if (Series1 == null)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }


                    argument = DateTime.Now;
                    pointsToUpdates = new SeriesPoint[6];
                    pointsToRemoveCount = 0;

                    UpdateValues();

                    for (var i = 0; i < points.Count; i++)
                    {
                        if (values != null && values.Count > i)
                        {
                            pointsToUpdates[i] = new SeriesPoint(argument, values[i]);
                            if (values[i] > MaxValue)
                                MaxValue = values[i];
                        }
                    }
                    for (var i = 0; i < points.Count; i++)
                    {
                        if (values != null && values.Count > i)
                        {
                            pointsToUpdates[i] = new SeriesPoint(argument, values[i]);
                            if (values[i] < MinValue)
                                MinValue = values[i];
                        }
                    }
                    if (points.Count == 0)
                    {
                        MaxValue = 1;
                        MinValue = 0;
                    }

                    argument = argument.AddSeconds(1);

                    minDate = argument.AddSeconds(-TimeInterval * 60);
                    if (chartControl.InvokeRequired)
                    {
                        chartControl.BeginInvoke(new Action(() =>
                        {
                            foreach (SeriesPoint point in Series1.Points)
                                if (point.DateTimeArgument < minDate)
                                    pointsToRemoveCount++;
                            if (pointsToRemoveCount < Series1.Points.Count)
                                pointsToRemoveCount--;

                            if (pointsToUpdates[0] != null)
                            {
                                AddPoints(Series1, pointsToUpdates[0]);
                                lock (this)
                                {
                                    Series1.Name = points.Count > 0 ? points[0] : "";
                                }
                                if (pointsToRemoveCount > 0)
                                    Series1.Points.RemoveRange(0, pointsToRemoveCount);
                            }
                            else
                            {
                                Series1.Name = "曲线1(未添加)";
                                AddPoints(Series1, new SeriesPoint(argument, 0));
                            }
                            if (pointsToUpdates[1] != null)
                            {
                                AddPoints(Series2, pointsToUpdates[1]);
                                lock (this)
                                {
                                    Series2.Name = points.Count > 1 ? points[1] : "";
                                }
                                if (pointsToRemoveCount > 0)
                                    Series2.Points.RemoveRange(0, pointsToRemoveCount);
                            }
                            else
                            {
                                Series2.Name = "曲线2(未添加)";
                                AddPoints(Series2, new SeriesPoint(argument, 0));
                            }
                            if (pointsToUpdates[2] != null)
                            {
                                AddPoints(Series3, pointsToUpdates[2]);
                                lock (this)
                                {
                                    Series3.Name = points.Count > 2 ? points[2] : "";
                                }
                                if (pointsToRemoveCount > 0)
                                    Series3.Points.RemoveRange(0, pointsToRemoveCount);
                            }
                            else
                            {
                                Series3.Name = "曲线3(未添加)";
                                AddPoints(Series3, new SeriesPoint(argument, 0));
                            }
                            if (pointsToUpdates[3] != null)
                            {
                                AddPoints(Series4, pointsToUpdates[3]);
                                lock (this)
                                {
                                    Series4.Name = points.Count > 3 ? points[3] : "";
                                }
                                if (pointsToRemoveCount > 0)
                                    Series4.Points.RemoveRange(0, pointsToRemoveCount);
                            }
                            else
                            {
                                Series4.Name = "曲线4(未添加)";
                                AddPoints(Series4, new SeriesPoint(argument, 0));
                            }
                            if (pointsToUpdates[4] != null)
                            {
                                AddPoints(Series5, pointsToUpdates[4]);
                                lock (this)
                                {
                                    Series5.Name = points.Count > 4 ? points[4] : "";
                                }
                                if (pointsToRemoveCount > 0)
                                    Series5.Points.RemoveRange(0, pointsToRemoveCount);
                            }
                            else
                            {
                                Series5.Name = "曲线5(未添加)";
                                AddPoints(Series5, new SeriesPoint(argument, 0));
                            }
                            if (pointsToUpdates[5] != null)
                            {
                                AddPoints(Series6, pointsToUpdates[5]);
                                lock (this)
                                {
                                    Series6.Name = points.Count > 5 ? points[5] : "";
                                }
                                if (pointsToRemoveCount > 0)
                                    Series6.Points.RemoveRange(0, pointsToRemoveCount);
                            }
                            else
                            {
                                Series6.Name = "曲线6(未添加)";
                                AddPoints(Series6, new SeriesPoint(argument, 0));
                            }

                            diagram = chartControl.Diagram as SwiftPlotDiagram;
                            if ((diagram != null) &&
                                ((diagram.AxisX.DateTimeScaleOptions.MeasureUnit == DateTimeMeasureUnit.Millisecond) ||
                                 (diagram.AxisX.DateTimeScaleOptions.ScaleMode == ScaleMode.Continuous)))
                            {
                                diagram.AxisX.WholeRange.SetMinMaxValues(minDate, argument);
                                diagram.AxisY.WholeRange.SetMinMaxValues(MinValue, MaxValue);
                                diagram.AxisY.VisualRange.SetMinMaxValues(MinValue, MaxValue);
                            }
                        }));
                    }

                }
                catch (Exception ex)
                {
                    LogHelper.Error("Mnl_SSZChart_timer_Tick" + ex.Message + ex.StackTrace);
                }
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        ///     将点加到曲线上
        /// </summary>
        /// <param name="series"></param>
        /// <param name="pointsToUpdate"></param>
        private void AddPoints(Series series, SeriesPoint pointsToUpdate)
        {
            if (series.View is SwiftPlotSeriesViewBase)
                series.Points.Add(pointsToUpdate);
        }

        /// <summary>
        ///     暂停/继续按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPauseResume_Click(object sender, EventArgs e)
        {
            isSuspended = !isSuspended;
            SetPauseResumeButtonText();
        }

        /// <summary>
        ///     窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Mnl_SSZChart_Load(object sender, EventArgs e)
        {
            try
            {
                Regression1.Visible = false;
                Regression2.Visible = false;
                Regression3.Visible = false;
                Regression4.Visible = false;
                Regression5.Visible = false;
                Regression6.Visible = false;

                Series1.Points.Clear();
                Series2.Points.Clear();
                Series3.Points.Clear();
                Series4.Points.Clear();
                Series5.Points.Clear();
                Series6.Points.Clear();
               
                values.Clear();

                //设置窗体高度和宽度
                Width = Convert.ToInt32(Screen.GetWorkingArea(this).Width * 0.8);
                Height = Convert.ToInt32(Screen.GetWorkingArea(this).Height * 0.7);
                Left = Convert.ToInt32(Screen.GetWorkingArea(this).Width * 0.3 / 2);
                Top = Convert.ToInt32(Screen.GetWorkingArea(this).Height * 0.3 / 2);
                //加载测点
                var LoadPointStr = new List<string>();
                checkedListBoxControl1.Items.Clear();
                LoadPointStr = InterfaceClass.queryConditions_.GetPointList();
                foreach (var PointStr in LoadPointStr)
                    checkedListBoxControl1.Items.Add(PointStr);
                if (checkedListBoxControl1.Items.Count > 0)
                    checkedListBoxControl1.SelectedIndex = -1;

                //bug修正: 20170316
                lock (this)
                {
                    for (var i = 0; i < checkedListBoxControl1.Items.Count; i++)
                    {
                        var temppoint = checkedListBoxControl1.Items[i].Value.ToString()
                            .Substring(0, checkedListBoxControl1.Items[i].Value.ToString().IndexOf('.'));
                        if (points.Contains(temppoint))
                            checkedListBoxControl1.Items[i].CheckState = CheckState.Checked;
                    }
                }

                spinEdit1.Value = TimeInterval;

                //启动刷新线程
                if (!isRun) {//判断线程不重复开启  20171016
                    m_RefThread = new Thread(RefClass);
                    m_RefThread.IsBackground = true;
                    m_RefThread.Priority = ThreadPriority.Normal;
                    m_RefThread.Start();
                }
                isRun = true;

                
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_SSZChart_Mnl_SSZChart_Load" + ex.Message + ex.StackTrace);
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
                if (chartControl != null)
                    ChartPrint.chartPrint(chartControl, (float)(Width * 0.8));
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_SSZChart_simpleButton4_Click" + ex.Message + ex.StackTrace);
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
                saveFileDialog1.FileName = "模拟量实时曲线.png";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    ExportToCore(saveFileDialog1.FileName, "png");
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_SSZChart_simpleButton2_Click" + ex.Message + ex.StackTrace);
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
                if (chartControl != null)
                {
                    var currentCursor = Cursor.Current;
                    Cursor.Current = Cursors.WaitCursor;
                    if (ext == "rtf")
                        chartControl.ExportToRtf(filename);
                    else if (ext == "pdf")
                        chartControl.ExportToPdf(filename);
                    else if (ext == "mht")
                        chartControl.ExportToMht(filename);
                    else if (ext == "html")
                        chartControl.ExportToHtml(filename);
                    else if (ext == "xls")
                        chartControl.ExportToXls(filename);
                    else if (ext == "xlsx")
                        chartControl.ExportToXlsx(filename);
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
                        chartControl.ExportToImage(filename, currentImageFormat);
                    }
                    Cursor.Current = currentCursor;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_SSZChart_ExportToCore" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     测点列表选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkedListBoxControl1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                isSuspended = true;
                SetPauseResumeButtonText();

                //bug修正: 20170316
                lock (this)
                {
                    points.Clear();
                    for (var i = 0; i < checkedListBoxControl1.Items.Count; i++)
                        if (checkedListBoxControl1.Items[i].CheckState == CheckState.Checked)
                        {
                            var tempoint = checkedListBoxControl1.Items[i].Value.ToString();
                            if (!points.Contains(tempoint))
                            {
                                if (points.Count == 6)
                                {
                                    XtraMessageBox.Show("最多只支持6个设备的实时曲线显示！");
                                    checkedListBoxControl1.Items[i].CheckState = CheckState.Unchecked;
                                    break;
                                }
                                points.Add(tempoint);
                            }
                        }
                }

                Series1.Points.Clear();
                Series2.Points.Clear();
                Series3.Points.Clear();
                Series4.Points.Clear();
                Series5.Points.Clear();
                Series6.Points.Clear();
                isSuspended = false;
                SetPauseResumeButtonText();
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_SSZChart_checkedListBoxControl1_ItemCheck" + ex.Message + ex.StackTrace);
            }
        }

        private void Mnl_SSZChart_FormClosed(object sender, FormClosedEventArgs e)
        {
            isRun = false;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            TimeInterval = (int)spinEdit1.Value;
        }
    }
}