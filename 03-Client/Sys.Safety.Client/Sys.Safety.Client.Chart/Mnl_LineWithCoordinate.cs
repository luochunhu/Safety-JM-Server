using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using Basic.Framework.Logging;

namespace Sys.Safety.Client.Chart
{
    public partial class Mnl_LineWithCoordinate : XtraForm
    {
        /// <summary>
        ///     测点1 当前设备类型ID
        /// </summary>
        private string CurrentDevid1 = "0";

        /// <summary>
        ///     测点2 当前设备类型ID
        /// </summary>
        private string CurrentDevid2 = "0";

        /// <summary>
        ///     测点3 当前设备类型ID
        /// </summary>
        private string CurrentDevid3 = "0";

        /// <summary>
        ///     测点1 当前测点ID
        /// </summary>
        private string CurrentPointID1 = "0";

        /// <summary>
        ///     测点2 当前测点ID
        /// </summary>
        private string CurrentPointID2 = "0";

        /// <summary>
        ///     测点3 当前测点ID
        /// </summary>
        private string CurrentPointID3 = "0";

        /// <summary>
        ///     测点1 当前位置ID
        /// </summary>
        private string CurrentWzid1 = "0";

        /// <summary>
        ///     测点2 当前位置ID
        /// </summary>
        private string CurrentWzid2 = "0";

        /// <summary>
        ///     测点3 当前位置ID
        /// </summary>
        private string CurrentWzid3 = "0";

        /// <summary>
        ///     设备类型ID列表
        /// </summary>
        public List<string> DevList = new List<string>();

        /// <summary>
        ///     曲线1数据源
        /// </summary>
        private DataTable dt_line = new DataTable();

        /// <summary>
        ///     曲线2数据源
        /// </summary>
        private DataTable dt_line1 = new DataTable();

        /// <summary>
        ///     曲线3数据源
        /// </summary>
        private DataTable dt_line2 = new DataTable();

        private int IscheckDate;


        private bool IsInIframe = false;

        /// <summary>
        ///     记录鼠标上一次移动的时间
        /// </summary>
        private DateTime lastMouseMoveTime = DateTime.Now;

        /// <summary>
        ///     测点加载列表
        /// </summary>
        private List<string> LoadPointStr = new List<string>();

        /// <summary>
        ///     测点加载线程
        /// </summary>
        //public Thread m_LoadPointThread;

        //tempsecondaryAxisY1 = secondaryAxisY1;
        //tempsecondaryAxisY2 = secondaryAxisY2;

        /// <summary>
        ///     测点1 测点单位
        /// </summary>
        private string PointDw1 = "";

        /// <summary>
        ///     测点2 测点单位
        /// </summary>
        private string PointDw2 = "";

        /// <summary>
        ///     测点3 测点单位
        /// </summary>
        private string PointDw3 = "";

        /// <summary>
        ///     测点ID列表
        /// </summary>
        public List<string> PointIDList = new List<string>();

        /// <summary>
        ///     测点名称
        /// </summary>
        private string pointName1 = "", pointName2 = "", pointName3 = "";

        private SecondaryAxisY tempsecondaryAxisY1;
        private SecondaryAxisY tempsecondaryAxisY2;

        /// <summary>
        ///     安装位置ID列表
        /// </summary>
        public List<string> WzList = new List<string>();

        public Mnl_LineWithCoordinate()
        {
            InitializeComponent();
        }

        public Mnl_LineWithCoordinate(Dictionary<string, string> param)
        {
            InitializeComponent();
        }
        public void Reload(Dictionary<string, string> param)
        {            
            object sender1 = null;
            var e1 = new EventArgs();
            Frm_Mnl_LineWithCoordinate_Load(sender1, e1);
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
        ///     Y1坐标
        /// </summary>
        private AxisBase AxisY1
        {
            get { return Diagram != null ? Diagram.AxisY : null; }
        }

        /// <summary>
        ///     Y2坐标
        /// </summary>
        private AxisBase AxisY2
        {
            get { return Diagram != null ? Diagram.SecondaryAxesY[0] : null; }
        }

        /// <summary>
        ///     Y3坐标
        /// </summary>
        private AxisBase AxisY3
        {
            get { return Diagram != null ? Diagram.SecondaryAxesY[1] : null; }
        }

        /// <summary>
        ///     测点1 监测值曲线
        /// </summary>
        private Series Series1_1
        {
            get { return chart.Series[0]; }
        }

        /// <summary>
        ///     测点2 监测值曲线
        /// </summary>
        private Series Series2_1
        {
            get { return chart.Series[1]; }
        }

        /// <summary>
        ///     测点3 监测值曲线
        /// </summary>
        private Series Series3_1
        {
            get { return chart.Series[2]; }
        }

        /// <summary>
        ///     加载所有曲线数据
        /// </summary>
        protected void InitControls(Series Series1, DataTable dt, string shortName, string ValueType, string bindColumn)
        {
            LoadSeries(Series1, shortName + "-" + ValueType, bindColumn, dt);
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
                LogHelper.Error("Mnl_LineWithCoordinate_LoadPoints" + ex.Message + ex.StackTrace);
            }
        }

        private void Frm_Mnl_LineWithCoordinate_Load(object sender, EventArgs e)
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
                Series1_1.Points.Clear();
                Series2_1.Points.Clear();
                Series3_1.Points.Clear();               
                gridControl1.DataSource = null;
              

                #region//加载曲线颜色  //暂时取消  20170425                               

                //InterfaceClass.QueryPubClass_.SetChartColor(Series1_1, "Chart_ZdzColor");
                //InterfaceClass.QueryPubClass_.SetChartColor(Series2_1, "Chart_ZdzColor");
                //InterfaceClass.QueryPubClass_.SetChartColor(Series3_1, "Chart_ZdzColor");               
                //InterfaceClass.QueryPubClass_.SetChartBgColor(Diagram, "Chart_BgColor");             

                #endregion

                //启动测点加载
                //m_LoadPointThread = new System.Threading.Thread(new System.Threading.ThreadStart(this.LoadPointList));
                //m_LoadPointThread.Priority = ThreadPriority.Normal;
                //m_LoadPointThread.Start();
                LoadPointList();
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_LineWithCoordinate_Frm_Mnl_LineWithCoordinate_Load" + ex.Message + ex.StackTrace);
            }
        }

        private void LoadPointList()
        {
            var wdf = new WaitDialogForm("正在加载数据...", "请等待...");
            try
            {
                //Thread.Sleep(500);


                ////数据校验
                //var ts = dateEdit2.DateTime - dateEdit1.DateTime;

                ////bug修正_20170325
                //if ((ts.TotalDays > 7) || (ts.TotalDays < 0))
                //    if (IscheckDate == 2)
                //        dateEdit1.DateTime = dateEdit2.DateTime;
                //    else if (IscheckDate == 1)
                //        dateEdit2.DateTime = dateEdit1.DateTime;
                LoadPointSelList1(dateEdit1.DateTime, dateEdit2.DateTime);
                LoadPointSelList2(dateEdit1.DateTime, dateEdit2.DateTime);
                LoadPointSelList3(dateEdit1.DateTime, dateEdit2.DateTime);
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_LineWithScreen_LoadPointList" + ex.Message + ex.StackTrace);
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
                    LoadPointList();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_LineWithCoordinate_dateEdit2_EditValueChanged" + ex.Message + ex.StackTrace);
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
                    LoadPointList();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_LineWithCoordinate_dateEdit1_EditValueChanged" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     加载测点选择列表 曲线1
        /// </summary>
        /// <param name="stime"></param>
        /// <param name="etime"></param>
        private void LoadPointSelList1(DateTime stime, DateTime etime)
        {
            try
            {
                LoadPointStr = new List<string>();
                if (LoadPointStr.Count < 1)
                {
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
                }
                // comboBoxEdit1.BeginInvoke(new Action(() =>
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
                //}));
                if (!string.IsNullOrEmpty(CurrentPointID1))
                {
                    //var isinlist = false;
                    //给combox赋初始值
                    for (var i = 0; i < PointIDList.Count; i++)
                        if (PointIDList[i].Contains(CurrentPointID1))
                        {
                            comboBoxEdit1.SelectedIndex = i;
                            //isinlist = true;
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_LineWithCoordinate_LoadPointSelList" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     加载测点选择列表 曲线2
        /// </summary>
        /// <param name="stime"></param>
        /// <param name="etime"></param>
        private void LoadPointSelList2(DateTime stime, DateTime etime)
        {
            try
            {
                if (LoadPointStr.Count < 1)
                {
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
                }
                //   comboBoxEdit2.BeginInvoke(new Action(() =>
                //{
                comboBoxEdit2.Properties.Items.Clear();

                foreach (var PointStr in LoadPointStr)
                    comboBoxEdit2.Properties.Items.Add(PointStr);
                if (comboBoxEdit2.Properties.Items.Count > 0)
                {
                    comboBoxEdit2.SelectedIndex = 0;
                    comboBoxEdit2.Enabled = true;
                }
                else
                {
                    comboBoxEdit2.Enabled = false;
                    comboBoxEdit2.Text = "没有数据";
                }
                //}));
                if (!string.IsNullOrEmpty(CurrentPointID2))
                {
                    //var isinlist = false;
                    //给combox赋初始值
                    for (var i = 0; i < PointIDList.Count; i++)
                        if (PointIDList[i].Contains(CurrentPointID2))
                        {
                            comboBoxEdit2.SelectedIndex = i;
                            //isinlist = true;
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_LineWithCoordinate_LoadPointSelList" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     加载测点选择列表 曲线3
        /// </summary>
        /// <param name="stime"></param>
        /// <param name="etime"></param>
        private void LoadPointSelList3(DateTime stime, DateTime etime)
        {
            try
            {
                if (LoadPointStr.Count < 1)
                {
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
                }
                //   comboBoxEdit3.BeginInvoke(new Action(() =>
                //{
                comboBoxEdit3.Properties.Items.Clear();

                foreach (var PointStr in LoadPointStr)
                    comboBoxEdit3.Properties.Items.Add(PointStr);
                if (comboBoxEdit3.Properties.Items.Count > 0)
                {
                    comboBoxEdit3.SelectedIndex = 0;
                    comboBoxEdit3.Enabled = true;
                }
                else
                {
                    comboBoxEdit3.Enabled = false;
                    comboBoxEdit3.Text = "没有数据";
                }
                //}));
                if (!string.IsNullOrEmpty(CurrentPointID3))
                {
                    //var isinlist = false;
                    //给combox赋初始值
                    for (var i = 0; i < PointIDList.Count; i++)
                        if (PointIDList[i].Contains(CurrentPointID3))
                        {
                            comboBoxEdit3.SelectedIndex = i;
                            //isinlist = true;
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_LineWithCoordinate_LoadPointSelList" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton7_Click(object sender, EventArgs e)
        {
            var wdf = new WaitDialogForm("正在加载数据...", "请等待...");
            try
            {
                //每次查询删除所有并重新添加阈值线
                for (var i = chart.Series.Count - 1; i >= 3; i--)
                    chart.Series.RemoveAt(i);

                var SzNameS = dateEdit1.DateTime;
                var SzNameE = dateEdit2.DateTime;

                var _minX = DateTime.Parse(SzNameS.ToShortDateString());
                var _maxX = DateTime.Parse(SzNameE.ToShortDateString() + " 23:59:59");
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
                    XtraMessageBox.Show("请选择测点1！");
                    return;
                }
                if (comboBoxEdit2.SelectedIndex < 0)
                {
                    if (wdf != null)
                        wdf.Close();
                    XtraMessageBox.Show("请选择测点2！");
                    return;
                }
                if (comboBoxEdit3.SelectedIndex < 0)
                {
                    if (wdf != null)
                        wdf.Close();
                    XtraMessageBox.Show("请选择测点3！");
                    return;
                }

                CurrentPointID1 = PointIDList[comboBoxEdit1.SelectedIndex];
                CurrentDevid1 = DevList[comboBoxEdit1.SelectedIndex];
                CurrentWzid1 = WzList[comboBoxEdit1.SelectedIndex];

                CurrentPointID2 = PointIDList[comboBoxEdit2.SelectedIndex];
                CurrentDevid2 = DevList[comboBoxEdit2.SelectedIndex];
                CurrentWzid2 = WzList[comboBoxEdit2.SelectedIndex];

                CurrentPointID3 = PointIDList[comboBoxEdit3.SelectedIndex];
                CurrentDevid3 = DevList[comboBoxEdit3.SelectedIndex];
                CurrentWzid3 = WzList[comboBoxEdit3.SelectedIndex];

                var ValueType = "";
                var bindColumn = "";
                if (checkEdit2.Checked)
                {
                    ValueType = "监测值";
                    bindColumn = "Av";
                }
                if (checkEdit3.Checked)
                {
                    ValueType = "最大值";
                    bindColumn = "Bv";
                }
                if (checkEdit4.Checked)
                {
                    ValueType = "最小值";
                    bindColumn = "Cv";
                }
                if (checkEdit5.Checked)
                {
                    ValueType = "平均值";
                    bindColumn = "Dv";
                }
                if (checkEdit7.Checked)
                {
                    ValueType = "移动值";
                    bindColumn = "Ev";
                }
                if (bindColumn == "")
                {
                    for (var i = chart.Series.Count - 1; i >= 0; i--)
                        chart.Series[i].Points.Clear();
                    if (wdf != null)
                        wdf.Close();
                    XtraMessageBox.Show("请选择至少一条曲线！");
                    return;
                }

                //测点1
                dt_line = InterfaceClass.FiveMiniteLineQueryClass_.getFiveMiniteLine(SzNameS, SzNameE, CurrentPointID1,
                    CurrentDevid1, CurrentWzid1);
                var tempList = InterfaceClass.QueryPubClass_.GetZFromTable(CurrentPointID1);

                var MaxValue = InterfaceClass.QueryPubClass_.getMaxBv(dt_line, "Bv")*1.2f;
                foreach (var tempMax in tempList)
                    if (MaxValue < tempMax) //表示当天没值
                        MaxValue = tempMax;
                if (MaxValue < 0.01)
                { //如果无数据，则加个默认最大值  20170723
                    MaxValue = 1;
                }

                //读取量程低
                var MinValue = InterfaceClass.QueryPubClass_.getMinBv(dt_line, "Cv");
                if (MinValue > 0)
                    MinValue = 0.0f;
                else
                    foreach (var tempMin in tempList)
                        if (MinValue > tempMin) //表示当天没值
                            MinValue = tempMin;
                PointDw1 = InterfaceClass.FiveMiniteLineQueryClass_.getPointDw(CurrentDevid1);

                AxisY1.WholeRange.SetMinMaxValues(MinValue, MaxValue);
                AxisY1.VisualRange.SetMinMaxValues(MinValue, MaxValue);
                var name = comboBoxEdit1.SelectedItem.ToString();
                var shortName = name.Substring(0, 7) + "[测点1]";
                pointName1 = shortName;
                InitControls(Series1_1, dt_line, shortName, ValueType, bindColumn);
                //添加阈值线
                var tempZ = PubOptClass.AddZSeries(dt_line, InterfaceClass.QueryPubClass_.GetZFromTable(CurrentPointID1),
                    null, null);
                var isinchart = false;
                if (tempZ.Count > 0)
                    foreach (var serie in tempZ)
                    {
                        isinchart = false;
                        for (var i = chart.Series.Count - 1; i >= 0; i--)
                        {
                            var chartserie = chart.Series[i];
                            if (chartserie.Name == "测点1" + serie.Name) //如果已存在
                            {
                                //重新添加                               
                                serie.CheckedInLegend = chartserie.CheckedInLegend;
                                serie.Name = "测点1" + serie.Name;
                                chart.Series[i].Points.Clear();
                                chart.Series[i].Points.AddRange(serie.Points.ToArray());

                                isinchart = true;
                                break;
                            }
                        }
                        if (!isinchart)
                        {
                            serie.ShowInLegend = false;
                            serie.Name = "测点1" + serie.Name;
                            chart.Series.Add(serie);
                        }
                    }
                //测点2
                dt_line1 = InterfaceClass.FiveMiniteLineQueryClass_.getFiveMiniteLine(SzNameS, SzNameE, CurrentPointID2,
                    CurrentDevid2, CurrentWzid2);
                tempList = InterfaceClass.QueryPubClass_.GetZFromTable(CurrentPointID2);
                MaxValue = InterfaceClass.QueryPubClass_.getMaxBv(dt_line1, "Bv")*1.2f;
                foreach (var tempMax in tempList)
                    if (MaxValue < tempMax) //表示当天没值
                        MaxValue = tempMax;
                if (MaxValue < 0.01)
                { //如果无数据，则加个默认最大值  20170723
                    MaxValue = 1;
                }

                //读取量程低
                MinValue = InterfaceClass.QueryPubClass_.getMinBv(dt_line1, "Cv");
                if (MinValue > 0)
                    MinValue = 0.0f;
                else
                    foreach (var tempMin in tempList)
                        if (MinValue > tempMin) //表示当天没值
                            MinValue = tempMin;

                MinValue = (float)MinValue - 0.01f;

                PointDw2 = InterfaceClass.FiveMiniteLineQueryClass_.getPointDw(CurrentDevid2);

                AxisY2.WholeRange.SetMinMaxValues(MinValue, MaxValue);
                AxisY2.VisualRange.SetMinMaxValues(MinValue, MaxValue);
                name = comboBoxEdit2.SelectedItem.ToString();
                shortName = name.Substring(0, 7) + "[测点2]";
                pointName2 = shortName;
                InitControls(Series2_1, dt_line1, shortName, ValueType, bindColumn);
                //添加阈值线
                tempZ = PubOptClass.AddZSeries(dt_line1, InterfaceClass.QueryPubClass_.GetZFromTable(CurrentPointID2),
                    null, tempsecondaryAxisY1);
                isinchart = false;
                if (tempZ.Count > 0)
                    foreach (var serie in tempZ)
                    {
                        isinchart = false;
                        for (var i = chart.Series.Count - 1; i >= 0; i--)
                        {
                            var chartserie = chart.Series[i];
                            if (chartserie.Name == "测点2" + serie.Name) //如果已存在
                            {
                                //重新添加                               
                                serie.CheckedInLegend = chartserie.CheckedInLegend;
                                serie.Name = "测点2" + serie.Name;
                                chart.Series[i].Points.Clear();
                                chart.Series[i].Points.AddRange(serie.Points.ToArray());

                                isinchart = true;
                                break;
                            }
                        }
                        if (!isinchart)
                        {
                            serie.ShowInLegend = false;
                            serie.Name = "测点2" + serie.Name;
                            chart.Series.Add(serie);
                        }
                    }
                //测点3
                dt_line2 = InterfaceClass.FiveMiniteLineQueryClass_.getFiveMiniteLine(SzNameS, SzNameE, CurrentPointID3,
                    CurrentDevid3, CurrentWzid3);
                tempList = InterfaceClass.QueryPubClass_.GetZFromTable(CurrentPointID3);
                MaxValue = InterfaceClass.QueryPubClass_.getMaxBv(dt_line2, "Bv")*1.2f;
                foreach (var tempMax in tempList)
                    if (MaxValue < tempMax) //表示当天没值
                        MaxValue = tempMax;
                if (MaxValue < 0.01)
                { //如果无数据，则加个默认最大值  20170723
                    MaxValue = 1;
                }

                //读取量程低
                MinValue = InterfaceClass.QueryPubClass_.getMinBv(dt_line2, "Cv");
                if (MinValue > 0)
                    MinValue = 0.0f;
                else
                    foreach (var tempMin in tempList)
                        if (MinValue > tempMin) //表示当天没值
                            MinValue = tempMin;
                PointDw3 = InterfaceClass.FiveMiniteLineQueryClass_.getPointDw(CurrentDevid3);

                AxisY3.WholeRange.SetMinMaxValues(MinValue, MaxValue);
                AxisY3.VisualRange.SetMinMaxValues(MinValue, MaxValue);
                name = comboBoxEdit3.SelectedItem.ToString();
                shortName = name.Substring(0, 7) + "[测点3]";
                pointName3 = shortName;
                InitControls(Series3_1, dt_line2, shortName, ValueType, bindColumn);

                chart.Titles[0].Text = comboBoxEdit1.SelectedItem + "," + comboBoxEdit2.SelectedItem + "," +
                                       comboBoxEdit3.SelectedItem;

                AxisX.WholeRange.SetMinMaxValues(_minX, _maxX);
                AxisX.VisualRange.SetMinMaxValues(_minX, _maxX);

                //添加阈值线
                tempZ = PubOptClass.AddZSeries(dt_line2, InterfaceClass.QueryPubClass_.GetZFromTable(CurrentPointID3),
                    null, tempsecondaryAxisY2);
                isinchart = false;
                if (tempZ.Count > 0)
                    foreach (var serie in tempZ)
                    {
                        isinchart = false;
                        for (var i = chart.Series.Count - 1; i >= 0; i--)
                        {
                            var chartserie = chart.Series[i];
                            if (chartserie.Name == "测点3" + serie.Name) //如果已存在
                            {
                                //重新添加                               
                                serie.CheckedInLegend = chartserie.CheckedInLegend;
                                serie.Name = "测点3" + serie.Name;
                                chart.Series[i].Points.Clear();
                                chart.Series[i].Points.AddRange(serie.Points.ToArray());

                                isinchart = true;
                                break;
                            }
                        }
                        if (!isinchart)
                        {
                            serie.ShowInLegend = false;
                            serie.Name = "测点3" + serie.Name;
                            chart.Series.Add(serie);
                        }
                    }

                #region//设置曲线显示隐藏

                for (var i = chart.Series.Count - 1; i >= 0; i--)
                {
                    var chartserie = chart.Series[i];
                    switch (chartserie.Name.Substring(3))
                    {
                        case "预警阈值":
                        case "下限预警阈值":
                            if (checkEdit11.Checked)
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
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_LineWithCoordinate_simpleButton7_Click" + ex.Message + ex.StackTrace);
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
                DataRow[] drs1 = null;
                DataRow[] drs2 = null;
                var index = 0;
                foreach (var element in e.CrosshairElements)
                {
                    var point = element.SeriesPoint;
                    //label1.Text = point.Argument.ToString();//显示要显示的文字
                    SelTime = DateTime.Parse(point.ArgumentSerializable).ToString("yyyy-MM-dd HH:mm:ss");
                    //if (index == 0)
                    //{
                    //    //大于300毫秒或有组件显示或隐藏才进行重绘
                    //    TimeSpan mouseMoveTimeStep = System.DateTime.Now - lastMouseMoveTime;
                    //    if (mouseMoveTimeStep.TotalMilliseconds >= 2000)
                    //    {
                    //        lastMouseMoveTime = System.DateTime.Now;
                    //        ChartGridDis(SelTime);
                    //    }
                    //}

                    if (element.Series.Name.Contains("移动值")
                        || element.Series.Name.Contains("监测值")
                        || element.Series.Name.Contains("最大值")
                        || element.Series.Name.Contains("最小值")
                        || element.Series.Name.Contains("平均值")
                    )
                    {
                        if (element.Series.Name.Contains(pointName1))
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
                                    if (!element.Series.Name.Contains("移动值"))
                                    {
                                        ShowText = element.Series.Name + ":" + drs[0]["typetext"];
                                    }
                                    else
                                    {
                                        if (point.Values[0].ToString() == "1E-05")
                                            ShowText = element.Series.Name + ":" + "未记录";
                                        else
                                            ShowText = element.Series.Name + ":" + point.Values[0].ToString("f2") +
                                                       PointDw1;
                                    }
                                }
                                else
                                {
                                    if (point.Values[0].ToString() == "1E-05")
                                        ShowText = element.Series.Name + ":" + "未记录";
                                    else
                                        ShowText = element.Series.Name + ":" + point.Values[0].ToString("f2") + PointDw1;
                                }
                        }
                        if (element.Series.Name.Contains(pointName2))
                        {
                            drs1 = dt_line1.Select("Timer='" + SelTime + "' ");
                            if (drs1.Length > 0)
                                if ((drs1[0]["type"].ToString() == "20") ||
                                    (drs1[0]["type"].ToString() == "22") ||
                                    (drs1[0]["type"].ToString() == "23") ||
                                    (drs1[0]["type"].ToString() == "33") ||
                                    (drs1[0]["type"].ToString() == "34"))
                                {
                                    if (!element.Series.Name.Contains("移动值"))
                                    {
                                        ShowText = element.Series.Name + ":" + drs1[0]["typetext"];
                                    }
                                    else
                                    {
                                        if (point.Values[0].ToString() == "1E-05")
                                            ShowText = element.Series.Name + ":" + "未记录";
                                        else
                                            ShowText = element.Series.Name + ":" + point.Values[0].ToString("f2") +
                                                       PointDw2;
                                    }
                                }
                                else
                                {
                                    if (point.Values[0].ToString() == "1E-05")
                                        ShowText = element.Series.Name + ":" + "未记录";
                                    else
                                        ShowText = element.Series.Name + ":" + point.Values[0].ToString("f2") + PointDw2;
                                }
                        }
                        if (element.Series.Name.Contains(pointName3))
                        {
                            drs2 = dt_line2.Select("Timer='" + SelTime + "' ");
                            if (drs2.Length > 0)
                                if ((drs2[0]["type"].ToString() == "20") ||
                                    (drs2[0]["type"].ToString() == "22") ||
                                    (drs2[0]["type"].ToString() == "23") ||
                                    (drs2[0]["type"].ToString() == "33") ||
                                    (drs2[0]["type"].ToString() == "34"))
                                {
                                    if (!element.Series.Name.Contains("移动值"))
                                    {
                                        ShowText = element.Series.Name + ":" + drs2[0]["typetext"];
                                    }
                                    else
                                    {
                                        if (point.Values[0].ToString() == "1E-05")
                                            ShowText = element.Series.Name + ":" + "未记录";
                                        else
                                            ShowText = element.Series.Name + ":" + point.Values[0].ToString("f2") +
                                                       PointDw3;
                                    }
                                }
                                else
                                {
                                    if (point.Values[0].ToString() == "1E-05")
                                        ShowText = element.Series.Name + ":" + "未记录";
                                    else
                                        ShowText = element.Series.Name + ":" + point.Values[0].ToString("f2") + PointDw3;
                                }
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
                LogHelper.Error("Mnl_LineWithCoordinate_chart_CustomDrawCrosshair" + ex.Message + ex.StackTrace);
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
                DtRefresh.Rows.Clear();
                string szDate;
                DataRow Dr;
                int Ihour, SMin, Iminite;

                #region//测点1

                for (var i = 6; i <= 12; i++)
                    QueryStr[i] = "";
                SzTableName = "KJ_StaFiveMinute" + SelTime.Year + SelTime.Month.ToString().PadLeft(2, '0') +
                              SelTime.Day.ToString().PadLeft(2, '0');
                tempPointInf = InterfaceClass.FiveMiniteLineQueryClass_.ShowPointInf(CurrentWzid1, CurrentPointID1);
                QueryStr[0] = tempPointInf[0];
                QueryStr[1] = tempPointInf[1];
                QueryStr[2] = tempPointInf[2];
                QueryStr[3] = tempPointInf[3];
                szDate = SelTime.ToString();


                QxDate = Convert.ToDateTime(szDate);
                DtStart = Convert.ToDateTime(QxDate.ToShortDateString());

                DtEnd = Convert.ToDateTime(QxDate.ToShortDateString());

                Ihour = QxDate.Hour;
                SMin = QxDate.Minute;
                QueryStr[6] = QxDate.Hour + ":" + QxDate.Minute + ":" + QxDate.Second;
                QueryStr[5] = "";
                Iminite = QxDate.Minute%10;

                if (Iminite > 4)
                {
                    DtStart = DtStart.AddHours(Ihour);
                    var Ival = 5;
                    if (SMin/10 != 0)
                        Ival = Ival + SMin/10*10;
                    DtStart = DtStart.AddMinutes(Ival);

                    Ival = 9;
                    DtEnd = DtEnd.AddHours(Ihour);
                    if (SMin/10 != 0)
                        Ival = Ival + SMin/10*10;
                    DtEnd = DtEnd.AddMinutes(Ival);
                    DtEnd = DtEnd.AddSeconds(59);
                }
                else
                {
                    DtStart = DtStart.AddHours(Ihour);
                    var Ival = 0;
                    if (SMin/10 != 0)
                        Ival = Ival + SMin/10*10;
                    DtStart = DtStart.AddMinutes(Ival);

                    Ival = 4;
                    DtEnd = DtEnd.AddHours(Ihour);
                    if (SMin/10 != 0)
                        Ival = Ival + SMin/10*10;
                    DtEnd = DtEnd.AddMinutes(Ival);
                    DtEnd = DtEnd.AddSeconds(59);
                }
                tempPointInf = InterfaceClass.FiveMiniteLineQueryClass_.GetDataVale(QxDate, DtStart, CurrentPointID1,
                    CurrentDevid1, CurrentWzid1);
                QueryStr[7] = tempPointInf[6];
                QueryStr[8] = tempPointInf[7];
                QueryStr[9] = tempPointInf[8];
                QueryStr[4] = tempPointInf[13]; //设备状态

                tempPointInf = InterfaceClass.FiveMiniteLineQueryClass_.GetValue(QxDate, DtStart, DtEnd, CurrentPointID1,
                    CurrentDevid1, CurrentWzid1);
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


                Dr = DtRefresh.NewRow();
                for (var i = 0; i <= 13; i++)
                    Dr[i] = QueryStr[i];
                DtRefresh.Rows.Add(Dr);

                #endregion

                #region//测点2

                for (var i = 6; i <= 12; i++)
                    QueryStr[i] = "";
                SzTableName = "KJ_StaFiveMinute" + SelTime.Year + SelTime.Month.ToString().PadLeft(2, '0') +
                              SelTime.Day.ToString().PadLeft(2, '0');
                tempPointInf = InterfaceClass.FiveMiniteLineQueryClass_.ShowPointInf(CurrentWzid2, CurrentPointID2);
                QueryStr[0] = tempPointInf[0];
                QueryStr[1] = tempPointInf[1];
                QueryStr[2] = tempPointInf[2];
                QueryStr[3] = tempPointInf[3];
                szDate = SelTime.ToString();


                QxDate = Convert.ToDateTime(szDate);
                DtStart = Convert.ToDateTime(QxDate.ToShortDateString());

                DtEnd = Convert.ToDateTime(QxDate.ToShortDateString());

                Ihour = QxDate.Hour;
                SMin = QxDate.Minute;
                QueryStr[6] = QxDate.Hour + ":" + QxDate.Minute + ":" + QxDate.Second;
                QueryStr[5] = "";
                Iminite = QxDate.Minute%10;

                if (Iminite > 4)
                {
                    DtStart = DtStart.AddHours(Ihour);
                    var Ival = 5;
                    if (SMin/10 != 0)
                        Ival = Ival + SMin/10*10;
                    DtStart = DtStart.AddMinutes(Ival);

                    Ival = 9;
                    DtEnd = DtEnd.AddHours(Ihour);
                    if (SMin/10 != 0)
                        Ival = Ival + SMin/10*10;
                    DtEnd = DtEnd.AddMinutes(Ival);
                    DtEnd = DtEnd.AddSeconds(59);
                }
                else
                {
                    DtStart = DtStart.AddHours(Ihour);
                    var Ival = 0;
                    if (SMin/10 != 0)
                        Ival = Ival + SMin/10*10;
                    DtStart = DtStart.AddMinutes(Ival);

                    Ival = 4;
                    DtEnd = DtEnd.AddHours(Ihour);
                    if (SMin/10 != 0)
                        Ival = Ival + SMin/10*10;
                    DtEnd = DtEnd.AddMinutes(Ival);
                    DtEnd = DtEnd.AddSeconds(59);
                }
                tempPointInf = InterfaceClass.FiveMiniteLineQueryClass_.GetDataVale(QxDate, DtStart, CurrentPointID2,
                    CurrentDevid2, CurrentWzid2);
                QueryStr[7] = tempPointInf[6];
                QueryStr[8] = tempPointInf[7];
                QueryStr[9] = tempPointInf[8];
                QueryStr[4] = tempPointInf[13]; //设备状态

                tempPointInf = InterfaceClass.FiveMiniteLineQueryClass_.GetValue(QxDate, DtStart, DtEnd, CurrentPointID2,
                    CurrentDevid2, CurrentWzid2);
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


                Dr = DtRefresh.NewRow();
                for (var i = 0; i <= 13; i++)
                    Dr[i] = QueryStr[i];
                DtRefresh.Rows.Add(Dr);

                #endregion

                #region//测点3

                for (var i = 6; i <= 12; i++)
                    QueryStr[i] = "";
                SzTableName = "KJ_StaFiveMinute" + SelTime.Year + SelTime.Month.ToString().PadLeft(2, '0') +
                              SelTime.Day.ToString().PadLeft(2, '0');
                tempPointInf = InterfaceClass.FiveMiniteLineQueryClass_.ShowPointInf(CurrentWzid3, CurrentPointID3);
                QueryStr[0] = tempPointInf[0];
                QueryStr[1] = tempPointInf[1];
                QueryStr[2] = tempPointInf[2];
                QueryStr[3] = tempPointInf[3];
                szDate = SelTime.ToString();


                QxDate = Convert.ToDateTime(szDate);
                DtStart = Convert.ToDateTime(QxDate.ToShortDateString());

                DtEnd = Convert.ToDateTime(QxDate.ToShortDateString());

                Ihour = QxDate.Hour;
                SMin = QxDate.Minute;
                QueryStr[6] = QxDate.Hour + ":" + QxDate.Minute + ":" + QxDate.Second;
                QueryStr[5] = "";
                Iminite = QxDate.Minute%10;

                if (Iminite > 4)
                {
                    DtStart = DtStart.AddHours(Ihour);
                    var Ival = 5;
                    if (SMin/10 != 0)
                        Ival = Ival + SMin/10*10;
                    DtStart = DtStart.AddMinutes(Ival);

                    Ival = 9;
                    DtEnd = DtEnd.AddHours(Ihour);
                    if (SMin/10 != 0)
                        Ival = Ival + SMin/10*10;
                    DtEnd = DtEnd.AddMinutes(Ival);
                    DtEnd = DtEnd.AddSeconds(59);
                }
                else
                {
                    DtStart = DtStart.AddHours(Ihour);
                    var Ival = 0;
                    if (SMin/10 != 0)
                        Ival = Ival + SMin/10*10;
                    DtStart = DtStart.AddMinutes(Ival);

                    Ival = 4;
                    DtEnd = DtEnd.AddHours(Ihour);
                    if (SMin/10 != 0)
                        Ival = Ival + SMin/10*10;
                    DtEnd = DtEnd.AddMinutes(Ival);
                    DtEnd = DtEnd.AddSeconds(59);
                }
                tempPointInf = InterfaceClass.FiveMiniteLineQueryClass_.GetDataVale(QxDate, DtStart, CurrentPointID3,
                    CurrentDevid3, CurrentWzid3);
                QueryStr[7] = tempPointInf[6];
                QueryStr[8] = tempPointInf[7];
                QueryStr[9] = tempPointInf[8];
                QueryStr[4] = tempPointInf[13]; //设备状态

                tempPointInf = InterfaceClass.FiveMiniteLineQueryClass_.GetValue(QxDate, DtStart, DtEnd, CurrentPointID3,
                    CurrentDevid3, CurrentWzid3);
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


                Dr = DtRefresh.NewRow();
                for (var i = 0; i <= 13; i++)
                    Dr[i] = QueryStr[i];
                DtRefresh.Rows.Add(Dr);

                #endregion

                gridControl1.DataSource = DtRefresh;
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_LineWithCoordinate_ChartGridDis" + ex.Message + ex.StackTrace);
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
                LogHelper.Error("Mnl_LineWithCoordinate_simpleButton4_Click" + ex.Message + ex.StackTrace);
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
                saveFileDialog1.FileName = "模拟量多点同坐标曲线.png";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    ExportToCore(saveFileDialog1.FileName, "png");
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_LineWithCoordinate_simpleButton4_Click" + ex.Message + ex.StackTrace);
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

        private void chart_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                //_isScale = false;
                var diagram = Diagram;
                var _p0 = e.Location;
                var coord = diagram.PointToDiagram(_p0);
                var SelTime = coord.DateTimeArgument;
                if ((SelTime.Minute%10 >= 3) && (SelTime.Minute%10 < 8))
                {
                    SelTime =
                        DateTime.Parse(SelTime.ToShortDateString() + " " + SelTime.Hour.ToString("00") + ":" +
                                       SelTime.Minute/10 + "5:00");
                }
                else
                {
                    if (SelTime.Minute%10 < 3)
                        SelTime =
                            DateTime.Parse(SelTime.ToShortDateString() + " " + SelTime.Hour.ToString("00") + ":" +
                                           SelTime.Minute/10 + "0:00");
                    else //>=8
                        SelTime =
                            DateTime.Parse(SelTime.ToShortDateString() + " " + SelTime.AddMinutes(2).Hour.ToString("00") +
                                           ":" + SelTime.AddMinutes(2).Minute/10 + "0:00");
                }
                ChartGridDis(SelTime);
            }
            catch (Exception ex)
            {
                LogHelper.Error("Mnl_LineWithCoordinate_chart_MouseClick" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     监测值选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEdit2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit2.Checked)
            {
                checkEdit3.Checked = false;
                checkEdit4.Checked = false;
                checkEdit5.Checked = false;
                checkEdit7.Checked = false;

                #region//加载曲线颜色  //暂时取消  20170425

                //InterfaceClass.QueryPubClass_.SetChartColor(Series1_1, "Chart_JczColor");
                //InterfaceClass.QueryPubClass_.SetChartColor(Series2_1, "Chart_JczColor");
                //InterfaceClass.QueryPubClass_.SetChartColor(Series3_1, "Chart_JczColor");                

                #endregion
            }
            object sender1 = null;
            var e1 = new EventArgs();
            simpleButton7_Click(sender1, e1);
        }

        /// <summary>
        ///     最大值选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEdit3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit3.Checked)
            {
                checkEdit2.Checked = false;
                checkEdit4.Checked = false;
                checkEdit5.Checked = false;
                checkEdit7.Checked = false;

                #region//加载曲线颜色  //暂时取消  20170425

                //InterfaceClass.QueryPubClass_.SetChartColor(Series1_1, "Chart_ZdzColor");
                //InterfaceClass.QueryPubClass_.SetChartColor(Series2_1, "Chart_ZdzColor");
                //InterfaceClass.QueryPubClass_.SetChartColor(Series3_1, "Chart_ZdzColor");     

                #endregion
            }
            object sender1 = null;
            var e1 = new EventArgs();
            simpleButton7_Click(sender1, e1);
        }

        /// <summary>
        ///     最小值选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEdit4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit4.Checked)
            {
                checkEdit3.Checked = false;
                checkEdit2.Checked = false;
                checkEdit5.Checked = false;
                checkEdit7.Checked = false;

                #region//加载曲线颜色  //暂时取消  20170425

                //InterfaceClass.QueryPubClass_.SetChartColor(Series1_1, "Chart_ZxzColor");
                //InterfaceClass.QueryPubClass_.SetChartColor(Series2_1, "Chart_ZxzColor");
                //InterfaceClass.QueryPubClass_.SetChartColor(Series3_1, "Chart_ZxzColor");  

                #endregion
            }
            object sender1 = null;
            var e1 = new EventArgs();
            simpleButton7_Click(sender1, e1);
        }

        /// <summary>
        ///     平均值选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEdit5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit5.Checked)
            {
                checkEdit3.Checked = false;
                checkEdit4.Checked = false;
                checkEdit2.Checked = false;
                checkEdit7.Checked = false;

                #region//加载曲线颜色  //暂时取消  20170425

                //InterfaceClass.QueryPubClass_.SetChartColor(Series1_1, "Chart_PjzColor");
                //InterfaceClass.QueryPubClass_.SetChartColor(Series2_1, "Chart_PjzColor");
                //InterfaceClass.QueryPubClass_.SetChartColor(Series3_1, "Chart_PjzColor");  

                #endregion
            }
            object sender1 = null;
            var e1 = new EventArgs();
            simpleButton7_Click(sender1, e1);
        }

        /// <summary>
        ///     移动值选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEdit7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit7.Checked)
            {
                checkEdit3.Checked = false;
                checkEdit4.Checked = false;
                checkEdit5.Checked = false;
                checkEdit2.Checked = false;

                #region//加载曲线颜色  //暂时取消  20170425

                //InterfaceClass.QueryPubClass_.SetChartColor(Series1_1, "Chart_YdzColor");
                //InterfaceClass.QueryPubClass_.SetChartColor(Series2_1, "Chart_YdzColor");
                //InterfaceClass.QueryPubClass_.SetChartColor(Series3_1, "Chart_YdzColor");  

                #endregion
            }
            object sender1 = null;
            var e1 = new EventArgs();
            simpleButton7_Click(sender1, e1);
        }

        /// <summary>
        ///     预警值显示设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEdit11_CheckedChanged(object sender, EventArgs e)
        {
            for (var i = chart.Series.Count - 1; i >= 0; i--)
            {
                var chartserie = chart.Series[i];
                if (chartserie.Name.Contains("预警阈值")) //
                    chartserie.Visible = checkEdit11.Checked;
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
            LoadPointList();
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
            simpleButton7_Click(new object(), new EventArgs());
        }
    }
}