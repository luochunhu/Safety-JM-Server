using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using Basic.Framework.Logging;
using System.Drawing;
using DevExpress.XtraGrid.Views.Grid;

namespace Sys.Safety.Client.Chart
{
    public partial class Mnl_DayZdzLine : XtraForm
    {
        private object _maxX, _minX, _maxY, _minY;

        /// <summary>
        ///     当前设备类型ID
        /// </summary>
        private string CurrentDevid = "0";

        /// <summary>
        ///     测点ID
        /// </summary>
        private string CurrentPointID = "";

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
        ///     测点加载线程
        /// </summary>
        //public Thread m_LoadPointThread;

        /// <summary>
        ///     测点单位
        /// </summary>
        private string PointDw = "";

        /// <summary>
        ///     测点ID(传入参数)
        /// </summary>
        private string PointID = "";

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

        public Mnl_DayZdzLine()
        {
            InitializeComponent();
        }

        public Mnl_DayZdzLine(Dictionary<string, string> param)
        {
            InitializeComponent();
            if ((param != null) && (param.Count > 0))
                try
                {
                    if (!string.IsNullOrEmpty(param["PointID"]))
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
            Mnl_DayZdzLine_Load(sender1, e1);
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
        ///     小时最大值
        /// </summary>
        private Series Series1
        {
            get { return chart.Series[0]; }
        }
        /// <summary>
        ///     小时最小值
        /// </summary>
        private Series Series2
        {
            get { return chart.Series[1]; }
        }
        /// <summary>
        ///     小时平均值
        /// </summary>
        private Series Series3
        {
            get { return chart.Series[2]; }
        }

        /// <summary>
        ///     加载所有曲线数据
        /// </summary>
        protected void InitControls(DataTable dt)
        {
            LoadSeries(Series1, "小时最大值", "HourMax", dt);
            LoadSeries(Series2, "小时最小值", "HourMin", dt);
            LoadSeries(Series3, "小时平均值", "HourAvg", dt);
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
                LogHelper.Error("Mnl_DayZdzLine_LoadPoints" + ex.Message + ex.StackTrace);
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
        private void Mnl_DayZdzLine_Load(object sender, EventArgs e)
        {
            try
            {
                //设置窗体高度和宽度
                Width = Convert.ToInt32(Screen.GetWorkingArea(this).Width * 0.9);
                Height = Convert.ToInt32(Screen.GetWorkingArea(this).Height * 0.9);
                Left = Convert.ToInt32(Screen.GetWorkingArea(this).Width * 0.1 / 2);
                Top = Convert.ToInt32(Screen.GetWorkingArea(this).Height * 0.1 / 2);

                //设置左边查询条件宽度
                layoutControlItem1.Width = 320;


                //初始化控件值
                dateEdit1.DateTime = DateTime.Now;
                dateEdit2.DateTime = DateTime.Now;
                Series1.Points.Clear();
                Series2.Points.Clear();
                Series3.Points.Clear();
               

                #region//加载曲线颜色 

                InterfaceClass.QueryPubClass_.SetChartColor(Series1, "Chart_ZdzColor");
                InterfaceClass.QueryPubClass_.SetChartColor(Series2, "Chart_ZxzColor");
                InterfaceClass.QueryPubClass_.SetChartColor(Series3, "Chart_PjzColor");
                InterfaceClass.QueryPubClass_.SetChartBgColor(Diagram, "Chart_BgColor");

                chartSetting = InterfaceClass.QueryPubClass_.GetChartColorSetting();

                #endregion

                //启动测点加载
                //m_LoadPointThread = new System.Threading.Thread(new System.Threading.ThreadStart(this.LoadPointList));
                //m_LoadPointThread.Priority = ThreadPriority.Normal;
                //m_LoadPointThread.Start();
                LoadPointList(true);
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_DayZdzLine_Mnl_DayZdzLine_Load" + ex.Message + ex.StackTrace);
            }
        }

        private void LoadPointList(bool loadData)
        {
            var wdf = new WaitDialogForm("正在加载数据...", "请等待...");
            try
            {
                Thread.Sleep(500);


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
                LogHelper.Error("Mnl_DayZdzLine_LoadPointList" + ex.Message + ex.StackTrace);
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
                var SelTime = new DateTime();
                DataRow[] drs = null;
                var index = 0;

                foreach (var element in e.CrosshairElements)
                {
                    var point = element.SeriesPoint;
                    //label1.Text = point.Argument.ToString();//显示要显示的文字
                    SelTime = DateTime.Parse(point.ArgumentSerializable);
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
                    if (element.Series.Name.Contains("最大值") || element.Series.Name.Contains("最小值") || element.Series.Name.Contains("平均值"))
                    {
                        drs = dt_line.Select("Timer='" + SelTime.ToString("yyyy-MM-dd HH:mm") + ":00' ");
                        if (drs.Length > 0)
                            if (point.Values[0].ToString() == "1E-05")
                                ShowText = element.Series.Name + ":" + "未记录";
                            else
                                ShowText = element.Series.Name + ":" + point.Values[0].ToString("f2") + PointDw;
                    }
                    else
                    {
                        ShowText = element.Series.Name + ":" + point.Values[0].ToString("f2");
                    }
                    if (index == 0)
                        element.LabelElement.HeaderText = "时间:" + SelTime.ToString("yyyy-MM-dd HH:mm") + "\n";


                    element.LabelElement.Text = ShowText; //显示要显示的文字   
                    index++;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_DayZdzLine_chart_CustomDrawCrosshair" + ex.Message + ex.StackTrace);
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
                LogHelper.Error("Mnl_DayZdzLine_simpleButton4_Click" + ex.Message + ex.StackTrace);
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
                LogHelper.Error("Mnl_DayZdzLine_ExportToCore" + ex.Message + ex.StackTrace);
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
                if (ts.TotalDays > 31)
                {
                    if (wdf != null)
                        wdf.Close();
                    XtraMessageBox.Show("查询的最大天数为31天,请重新选择日期！");
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
                PointID = CurrentPointID;
                CurrentDevid = DevList[comboBoxEdit1.SelectedIndex];
                CurrentWzid = WzList[comboBoxEdit1.SelectedIndex];


                dt_line = InterfaceClass.FiveMiniteLineQueryClass_.getMonthLine(SzNameS, SzNameE, CurrentPointID,
                    CurrentDevid, CurrentWzid);

                // 20170627
                DataView dataView = dt_line.DefaultView;
                dataView.Sort = "Timer asc";
                dt_line = dataView.ToTable();
                              
                var MaxValueNow = InterfaceClass.QueryPubClass_.getMaxBv(dt_line, "HourMax");

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
                var MinValueNow = InterfaceClass.QueryPubClass_.getMinBv(dt_line, "HourMin");
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

                float avgValue = InterfaceClass.QueryPubClass_.getAvgBv(dt_line, "HourAvg");

                chart.Titles[0].Text = comboBoxEdit1.SelectedItem.ToString() + "，最大值：" + (((float)MaxValueNow).ToString() == "-9999" ? "-" : ((float)MaxValueNow).ToString("f2"))
                    + "，最小值：" + (((float)MinValueNow).ToString() == "9999" ? "-" : ((float)MinValueNow).ToString("f2"))
                    + "，平均值：" + (avgValue.ToString() == "-9999" ? "-" : avgValue.ToString("f2"));
                InitControls(dt_line);


                _minX = DateTime.Parse(SzNameS.ToShortDateString());
                _maxX = DateTime.Parse(SzNameE.ToShortDateString() + " 23:59:59");

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
                        for (var i = chart.Series.Count - 1; i >= 3; i--)
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
                    for (var i = chart.Series.Count - 1; i >= 3; i--)
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
                LogHelper.Error("Mnl_DayZdzLine_simpleButton1_Click" + ex.Message + ex.StackTrace);
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
                LogHelper.Error("Mnl_DayZdzLine_dateEdit2_EditValueChanged" + ex.Message + ex.StackTrace);
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
                LogHelper.Error("Mnl_DayZdzLine_dateEdit1_EditValueChanged" + ex.Message + ex.StackTrace);
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
                LogHelper.Error("Mnl_DayZdzLine_LoadPointSelList" + ex.Message + ex.StackTrace);
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
                saveFileDialog1.FileName = "模拟量月曲线.png";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    ExportToCore(saveFileDialog1.FileName, "png");
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_DayZdzLine_simpleButton2_Click" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     最大值显示/隐藏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEdit3_CheckedChanged(object sender, EventArgs e)
        {
            if ( !checkEdit2.Checked && !checkEdit3.Checked && !checkEdit4.Checked )
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
        ///     最小值显示/隐藏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEdit2_CheckedChanged(object sender, EventArgs e)
        {
            if ( !checkEdit2.Checked && !checkEdit3.Checked && !checkEdit4.Checked )
            {
                XtraMessageBox.Show("请至少选择1条曲线进行查看！");
                checkEdit2.Checked = true;
                return;
            }
            if (checkEdit2.Checked)
                Series2.Visible = true;
            else
                Series2.Visible = false;
        }

        /// <summary>
        ///     平均值显示/隐藏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEdit4_CheckedChanged(object sender, EventArgs e)
        {
            if ( !checkEdit2.Checked && !checkEdit3.Checked && !checkEdit4.Checked  )
            {
                XtraMessageBox.Show("请至少选择1条曲线进行查看！");
                checkEdit4.Checked = true;
                return;
            }
            if (checkEdit4.Checked)
                Series3.Visible = true;
            else
                Series3.Visible = false;
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

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            List<string> seriesNameList = new List<string>();
            for (int i = 0; i < chart.Series.Count; i++)
            {
                seriesNameList.Add(chart.Series[i].Name);
            }

            if (checkEdit1.Checked)
            {
                if (seriesNameList.Contains("小时最大值"))
                {
                    LineSeriesView lineview1 = (LineSeriesView)chart.Series["小时最大值"].View;
                    lineview1.MarkerVisibility = DefaultBoolean.True;
                    lineview1.LineMarkerOptions.Size = 10;
                }
                if (seriesNameList.Contains("小时最小值"))
                {
                    LineSeriesView lineview1 = (LineSeriesView)chart.Series["小时最小值"].View;
                    lineview1.MarkerVisibility = DefaultBoolean.True;
                    lineview1.LineMarkerOptions.Size = 10;
                }
                if (seriesNameList.Contains("小时平均值"))
                {
                    LineSeriesView lineview1 = (LineSeriesView)chart.Series["小时平均值"].View;
                    lineview1.MarkerVisibility = DefaultBoolean.True;
                    lineview1.LineMarkerOptions.Size = 10;
                }
            }
            else
            {
                if (seriesNameList.Contains("小时最大值"))
                {
                    LineSeriesView lineview1 = (LineSeriesView)chart.Series["小时最大值"].View;
                    lineview1.MarkerVisibility = DefaultBoolean.False;
                }
                if (seriesNameList.Contains("小时最小值"))
                {
                    LineSeriesView lineview1 = (LineSeriesView)chart.Series["小时最小值"].View;
                    lineview1.MarkerVisibility = DefaultBoolean.False;
                }
                if (seriesNameList.Contains("小时平均值"))
                {
                    LineSeriesView lineview1 = (LineSeriesView)chart.Series["小时平均值"].View;
                    lineview1.MarkerVisibility = DefaultBoolean.False;
                }
            }
        }

        private void chart_CustomDrawSeriesPoint(object sender, CustomDrawSeriesPointEventArgs e)
        {
            var value = e.SeriesPoint.Values;

            var pointtimer = DateTime.Parse(e.SeriesPoint.ArgumentSerializable).ToString("yyyy-MM-dd HH:mm:ss");

            var pointvalue = dt_line.Select(string.Format("Timer= '" + pointtimer + "'"));
            if (pointvalue.Length > 0)
            {
                if (threshold.Count > 5)
                {
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

        private void gridView2_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            try
            {
                var view = sender as GridView;
                if (e.Column.FieldName == "HourMax")
                {
                    var aa = view.GetRowCellDisplayText(e.RowHandle, view.Columns["HourMax"]);
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
                if (e.Column.FieldName == "HourMin")
                {
                    var aa = view.GetRowCellDisplayText(e.RowHandle, view.Columns["HourMin"]);
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
                if (e.Column.FieldName == "HourAvg")
                {
                    var aa = view.GetRowCellDisplayText(e.RowHandle, view.Columns["HourAvg"]);
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
    }
}