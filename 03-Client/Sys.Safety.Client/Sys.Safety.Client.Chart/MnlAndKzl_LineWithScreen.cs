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
using System.Drawing;

namespace Sys.Safety.Client.Chart
{
    public partial class MnlAndKzl_LineWithScreen : XtraForm
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
        ///     测点1 当前测点ID
        /// </summary>
        private string CurrentPointID1 = "0";

        /// <summary>
        ///     测点2 当前测点ID
        /// </summary>
        private string CurrentPointID2 = "0";

        /// <summary>
        ///     测点1 当前位置ID
        /// </summary>
        private string CurrentWzid1 = "0";

        /// <summary>
        ///     测点2 当前位置ID
        /// </summary>
        private string CurrentWzid2 = "0";

        /// <summary>
        ///     设备类型ID列表
        /// </summary>
        public List<string> DevList = new List<string>();

        /// <summary>
        ///     设备类型ID列表
        /// </summary>
        public List<string> DevList1 = new List<string>();

        /// <summary>
        ///     曲线数据源
        /// </summary>
        private DataTable dt_line = new DataTable();

        private bool IsInIframe = false;

        /// <summary>
        ///     曲线数据源
        /// </summary>
        private DataTable Kgl_dt_line = new DataTable();

        /// <summary>
        ///     是否计算未知状态
        /// </summary>
        private bool kglztjsfs = true;


        /// <summary>
        ///     测点加载线程
        /// </summary>
        //public Thread m_LoadPointThread;

        /// <summary>
        ///     测点1 测点单位
        /// </summary>
        private string PointDw1 = "";

        /// <summary>
        ///     测点ID列表
        /// </summary>
        public List<string> PointIDList = new List<string>();

        /// <summary>
        ///     测点ID列表
        /// </summary>
        public List<string> PointIDList1 = new List<string>();

        /// <summary>
        ///     测点号列表
        /// </summary>
        private readonly string[] PointIDs = new string[2] {"", ""};

        private string pointName1 = "";
        private readonly string pointName2 = "";
        private string pointName3 = "";

        /// <summary>
        ///     安装位置ID列表
        /// </summary>
        public List<string> WzList = new List<string>();

        /// <summary>
        ///     安装位置ID列表
        /// </summary>
        public List<string> WzList1 = new List<string>();

        /// <summary>
        /// 曲线颜色设置
        /// </summary>
        private DataTable chartSetting = new DataTable();

        /// <summary>
        /// 测点阈值
        /// </summary>
        private List<float> threshold = new List<float>();

        public MnlAndKzl_LineWithScreen()
        {
            InitializeComponent();
        }

        public MnlAndKzl_LineWithScreen(Dictionary<string, string> param)
        {
            InitializeComponent();
            if ((param != null) && (param.Count > 0))
                try
                {
                    var temppoints = param["PointID"].Split('|');
                    for (var i = 0; i < temppoints.Length; i++)
                        if (!string.IsNullOrEmpty(temppoints[i]))
                            PointIDs[i] = temppoints[i];
                }
                catch
                {
                }
            else
                return;
        }
        public void Reload(Dictionary<string, string> param)
        {
            if ((param != null) && (param.Count > 0))
                try
                {
                    var temppoints = param["PointID"].Split('|');
                    for (var i = 0; i < temppoints.Length; i++)
                        if (!string.IsNullOrEmpty(temppoints[i]))
                            PointIDs[i] = temppoints[i];
                }
                catch
                {
                }
            object sender1 = null;
            var e1 = new EventArgs();
            Frm_Mnl_LineWithScreen_Load(sender1, e1);
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
                LogHelper.Error("MnlAndKzl_LineWithScreen_LoadPoints" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     加载所有曲线数据
        /// </summary>
        protected void InitControls1(DataTable dt, string name)
        {
            LoadSeries1(Series2_1, name, "C", dt);
        }

        /// <summary>
        ///     测点曲线加载
        /// </summary>
        /// <param name="series"></param>
        /// <param name="name"></param>
        /// <param name="shortName"></param>
        private void LoadSeries1(Series series, string name, string shortName, DataTable dt)
        {
            var CodeLen = 3;
            LoadPoints1(series, shortName, dt);
            //series.CrosshairLabelPattern = name + " : {V:F2}" + "%";
            series.Name = name;
        }

        /// <summary>
        ///     加载曲线测点的值
        /// </summary>
        /// <param name="series"></param>
        /// <param name="CodeLen"></param>
        private void LoadPoints1(Series series, string shortName, DataTable dt)
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

        private void Frm_Mnl_LineWithScreen_Load(object sender, EventArgs e)
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
               

                #region//加载曲线颜色 

                InterfaceClass.QueryPubClass_.SetChartColor(Series1_1, "Chart_ZdzColor");
                InterfaceClass.QueryPubClass_.SetChartColor(Series2_1, "Chart_KglColor");
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
                LogHelper.Error("MnlAndKzl_LineWithScreen_Frm_Mnl_LineWithScreen_Load" + ex.Message + ex.StackTrace);
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
                //启动测点加载
                //m_LoadPointThread = new System.Threading.Thread(new System.Threading.ThreadStart(this.LoadPointList));
                //m_LoadPointThread.Priority = ThreadPriority.Normal;
                //m_LoadPointThread.Start();

                if (radioGroup1.SelectedIndex == 1)
                {//如果按已存储测点查询，则每次选择时间后，重新加载 
                    LoadPointList(false,0);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("MnlAndKzl_LineWithScreen_dateEdit1_EditValueChanged" + ex.Message + ex.StackTrace);
            }
        }

        private void dateEdit2_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                //启动测点加载
                //m_LoadPointThread = new System.Threading.Thread(new System.Threading.ThreadStart(this.LoadPointList));
                //m_LoadPointThread.Priority = ThreadPriority.Normal;
                //m_LoadPointThread.Start();

                if (radioGroup1.SelectedIndex == 1)
                {//如果按已存储测点查询，则每次选择时间后，重新加载 
                    LoadPointList(false,0);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("MnlAndKzl_LineWithScreen_dateEdit2_EditValueChanged" + ex.Message + ex.StackTrace);
            }
        }

        private void LoadPointList(bool loadData,int state = 0)
        {
            var wdf = new WaitDialogForm("正在加载数据...", "请等待...");
            try
            {
                //var ts = dateEdit2.DateTime - dateEdit1.DateTime;
                //if ((ts.TotalDays > 7) || (ts.TotalDays < 0))
                //    if (state == 2)
                //        dateEdit1.DateTime = dateEdit2.DateTime;
                //    else if (state == 1)
                //        dateEdit2.DateTime = dateEdit1.DateTime;

                LoadPointSelList1(dateEdit1.DateTime, dateEdit2.DateTime, loadData);
                //LoadPointSelList2(dateEdit1.DateTime, dateEdit1.DateTime);
            }
            catch (Exception ex)
            {
                LogHelper.Error("MnlAndKzl_LineWithScreen_dateEdit1_EditValueChanged" + ex.Message + ex.StackTrace);
            }
            if (wdf != null)
                wdf.Close();
        }

        private void LoadPointList2(bool loadData)
        {
            var wdf = new WaitDialogForm("正在加载数据...", "请等待...");
            try
            {
                //Thread.Sleep(500);


                LoadPointSelList2(CurrentPointID1, dateEdit1.DateTime, dateEdit2.DateTime,loadData);
            }
            catch (Exception ex)
            {
                LogHelper.Error("MnlAndKzl_LineWithScreen_dateEdit1_EditValueChanged" + ex.Message + ex.StackTrace);
            }
            if (wdf != null)
                wdf.Close();
        }

        /// <summary>
        ///     加载测点选择列表 曲线1
        /// </summary>
        /// <param name="stime"></param>
        /// <param name="etime"></param>
        private void LoadPointSelList1(DateTime stime, DateTime etime,bool loadData)
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
                LoadPointSer(loadData);
                //}));
            }
            catch (Exception ex)
            {
                LogHelper.Error("MnlAndKzl_LineWithScreen_LoadPointSelList" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     加载测点选择列表 曲线2
        /// </summary>
        /// <param name="stime"></param>
        /// <param name="etime"></param>
        private void LoadPointSelList2(string PointID, DateTime stime, DateTime etime, bool loadData)
        {
            try
            {
                var LoadPointStr = new List<string>();
                LoadPointStr = InterfaceClass.queryConditions_.GetPointKzList(stime, etime, PointID, ref PointIDList1,
                    ref DevList1, ref WzList1);
                //comboBoxEdit2.BeginInvoke(new Action(() =>
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
                LoadPointSer(loadData);
                //}));
            }
            catch (Exception ex)
            {
                LogHelper.Error("MnlAndKzl_LineWithScreen_LoadPointSelList" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        ///     加载并查询
        /// </summary>
        private void LoadPointSer(bool loadData)
        {
            try
            {
                //加载曲线
                var isLoad = false;
                //给combox赋初始值
                //comboBoxEdit1.BeginInvoke(new Action(() =>
                //{
                for (var i = 0; i < PointIDList.Count; i++)
                {
                    if (PointIDs[0] != "")
                        if (PointIDList[i].Contains(PointIDs[0]))
                        {
                            comboBoxEdit1.SelectedIndex = i;
                            isLoad = true;
                        }
                    if (PointIDs[1] != "")
                        if (PointIDList[i].Contains(PointIDs[1]))
                        {
                            comboBoxEdit1.SelectedIndex = i;
                            isLoad = true;
                        }
                }
                //}));
                //comboBoxEdit2.BeginInvoke(new Action(() =>
                //{
                for (var i = 0; i < PointIDList1.Count; i++)
                {
                    if (PointIDs[0] != "")
                        if (PointIDList1[i].Contains(PointIDs[0]))
                        {
                            comboBoxEdit2.SelectedIndex = i;
                            isLoad = true;
                        }
                    if (PointIDs[1] != "")
                        if (PointIDList1[i].Contains(PointIDs[1]))
                        {
                            comboBoxEdit2.SelectedIndex = i;
                            isLoad = true;
                        }
                }
                //}));
                if (loadData)
                {
                    if (isLoad)
                    {
                        //调用查询
                        object sender1 = null;
                        var e1 = new EventArgs();
                        simpleButton7_Click(sender1, e1);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("MnlAndKzl_LineWithScreen_LoadPointStr" + ex.Message + ex.StackTrace);
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
                var SzNameS = Convert.ToDateTime(dateEdit1.DateTime.ToShortDateString());
                var SzNameE = Convert.ToDateTime(dateEdit2.DateTime.ToShortDateString() + " 23:59:59");
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
                if (comboBoxEdit2.SelectedIndex < 0)
                {
                    if (wdf != null)
                        wdf.Close();
                    XtraMessageBox.Show("请选择被控区域！");
                    return;
                }


                CurrentPointID1 = PointIDList[comboBoxEdit1.SelectedIndex];
                CurrentDevid1 = DevList[comboBoxEdit1.SelectedIndex];
                CurrentWzid1 = WzList[comboBoxEdit1.SelectedIndex];

                CurrentPointID2 = PointIDList1[comboBoxEdit2.SelectedIndex];
                CurrentDevid2 = DevList1[comboBoxEdit2.SelectedIndex];
                CurrentWzid2 = WzList1[comboBoxEdit2.SelectedIndex];

                PointIDs[0] = CurrentPointID1;
                PointIDs[1] = CurrentPointID2;

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

                //测点1
                dt_line = InterfaceClass.FiveMiniteLineQueryClass_.getFiveMiniteLine(SzNameS, SzNameE, CurrentPointID1,
                    CurrentDevid1, CurrentWzid1);
                var MaxValue = InterfaceClass.QueryPubClass_.getMaxBv(dt_line, "Bv")*1.2f;

                threshold.Clear();
                threshold = InterfaceClass.QueryPubClass_.GetZFromTable(CurrentPointID1);
                //var tempList = InterfaceClass.QueryPubClass_.GetZFromTable(CurrentPointID1);
                foreach (var tempMax in threshold)
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
                    foreach (var tempMin in threshold)
                        if (MinValue > tempMin) //表示当天没值
                            MinValue = tempMin;

                MinValue = (float)MinValue - 0.01f;

                PointDw1 = InterfaceClass.FiveMiniteLineQueryClass_.getPointDw(CurrentDevid1);

                AxisY1.WholeRange.SetMinMaxValues(MinValue, MaxValue);
                AxisY1.VisualRange.SetMinMaxValues(MinValue, MaxValue);
                var name = comboBoxEdit1.SelectedItem.ToString();
                var shortName = name.Substring(0, name.IndexOf('.'));
                pointName1 = shortName;
                InitControls(Series1_1, dt_line, shortName, ValueType, bindColumn);
                //测点2
                Kgl_dt_line = InterfaceClass.KglStateLineQueryClass_.getKzlLineDt(SzNameS, SzNameE, CurrentPointID2,
                    CurrentDevid2, CurrentWzid2);

                //List<string> pointDev = InterfaceClass.QueryPubClass_.getKglStateDev(CurrentDevid2);
                var stateName = "1态:断电失败/复电失败," + "0态:正常";

                name = comboBoxEdit2.SelectedItem + "(" + stateName + ")";
                shortName =
                    comboBoxEdit2.SelectedItem.ToString()
                        .Substring(0, comboBoxEdit2.SelectedItem.ToString().IndexOf('.')) + "-馈电状态";


                InitControls1(Kgl_dt_line, shortName);


                chart.Titles[0].Text = comboBoxEdit1.SelectedItem + "," + name;
                var _minX = DateTime.Parse(SzNameS.ToShortDateString());
                var _maxX = DateTime.Parse(SzNameE.ToShortDateString() + " 23:59:59");

                //添加阈值线
                var tempZ = PubOptClass.AddZSeries(dt_line, InterfaceClass.QueryPubClass_.GetZFromTable(CurrentPointID1),
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
                LogHelper.Error("MnlAndKzl_LineWithScreen_simpleButton7_Click" + ex.Message + ex.StackTrace);
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
                var value = "";
                string SelTime = "";
                var KglSelTime = "";
                DataRow[] drs = null;
                var SelTimeNow = new DateTime();
                DataRow[] drs1 = null;
                var index = 0;
                foreach (var element in e.CrosshairElements)
                {
                    ShowText = "";
                    var point = element.SeriesPoint;
                    //label1.Text = point.Argument.ToString();//显示要显示的文字
                    SelTime = DateTime.Parse(point.ArgumentSerializable).ToString("yyyy-MM-dd HH:mm:ss");
                    foreach (var element1 in e.CrosshairAxisLabelElements)
                        SelTimeNow = DateTime.Parse(element1.AxisValue.ToString());

                    if (element.Series.Name.Contains("移动值")
                        || element.Series.Name.Contains("监测值")
                        || element.Series.Name.Contains("最大值")
                        || element.Series.Name.Contains("最小值")
                        || element.Series.Name.Contains("平均值")
                        || element.Series.Name.Contains("馈电状态"))
                    {
                        if (element.Series.Name.Contains(pointName1))
                        {
                            drs1 = dt_line.Select("Timer='" + SelTime + "' ");
                            if (drs1.Length > 0)
                                if ((drs1[0]["type"].ToString() == "20") ||
                                    (drs1[0]["type"].ToString() == "22") ||
                                    (drs1[0]["type"].ToString() == "23") ||
                                    (drs1[0]["type"].ToString() == "33") ||
                                    (drs1[0]["type"].ToString() == "34"))
                                {
                                    if (!element.Series.Name.Contains("移动值"))
                                    {
                                        //ShowText += SelTime + "\n";
                                        ShowText += element.Series.Name + ":" + drs1[0]["typetext"];
                                    }
                                    else
                                    {
                                        //ShowText += SelTime + "\n";
                                        if (point.Values[0].ToString() == "1E-05")
                                            ShowText += element.Series.Name + ":" + "未记录\n";
                                        else
                                            ShowText += element.Series.Name + ":" + point.Values[0].ToString("f2") +
                                                        PointDw1 + "\n";
                                    }
                                }
                                else
                                {
                                    //ShowText += SelTime + "\n";
                                    if (point.Values[0].ToString() == "1E-05")
                                        ShowText += element.Series.Name + ":" + "未记录\n";
                                    else
                                        ShowText += element.Series.Name + ":" + point.Values[0].ToString("f2") +
                                                    PointDw1 + "\n";
                                }
                        }
                        else if (element.Series.Name.Contains(pointName2))
                        {
                            SelTime = DateTime.Parse(point.ArgumentSerializable).ToString("yyyy-MM-dd HH:mm:ss");
                            drs = Kgl_dt_line.Select("sTimer='" + SelTime + "' ");
                            if (drs.Length > 0)
                            {
                                if (drs[0]["C"].ToString() == "0.00001")
                                    value = "未记录";
                                else
                                    value = drs[0]["stateName"].ToString();
                                ShowText += "" + element.Series.Name + ":\n";
                                ShowText += "起止时刻：" + DateTime.Parse(drs[0]["sTimer"].ToString()).ToLongTimeString()
                                            + "-" + DateTime.Parse(drs[0]["eTimer"].ToString()).ToLongTimeString() +
                                            "\n";
                                ShowText += "状态：" + value + "\n";
                                //ShowText += "馈电状态：" + drs[0]["D"].ToString() + "\n";
                                //ShowText += "处理措施：" + drs[0]["E"].ToString();
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
                LogHelper.Error("MnlAndKzl_LineWithScreen_chart_CustomDrawCrosshair" + ex.Message + ex.StackTrace);
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                if (chart != null)
                    ChartPrint.chartPrint(chart, (float)(Width * 0.8));
            }
            catch (Exception ex)
            {
                LogHelper.Error("MnlAndKzl_LineWithScreen_simpleButton4_Click" + ex.Message + ex.StackTrace);
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
                LogHelper.Error("MnlAndKzl_LineWithScreen_ExportToCore" + ex.Message + ex.StackTrace);
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
                saveFileDialog1.FileName = "模拟量开关量同屏曲线.png";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    ExportToCore(saveFileDialog1.FileName, "png");
            }
            catch (Exception ex)
            {
                LogHelper.Error("MnlAndKzl_LineWithScreen_simpleButton2_Click" + ex.Message + ex.StackTrace);
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

                #region//加载曲线颜色 

                InterfaceClass.QueryPubClass_.SetChartColor(Series1_1, "Chart_JczColor");

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

                #region//加载曲线颜色 

                InterfaceClass.QueryPubClass_.SetChartColor(Series1_1, "Chart_ZdzColor");

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

                #region//加载曲线颜色 

                InterfaceClass.QueryPubClass_.SetChartColor(Series1_1, "Chart_ZxzColor");

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

                #region//加载曲线颜色 

                InterfaceClass.QueryPubClass_.SetChartColor(Series1_1, "Chart_PjzColor");

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

                #region//加载曲线颜色 

                InterfaceClass.QueryPubClass_.SetChartColor(Series1_1, "Chart_YdzColor");

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
        ///     加载模拟量所有被控区域测点 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if ((comboBoxEdit1.SelectedIndex >= 0) && (PointIDList.Count > 0))
            {
                CurrentPointID1 = PointIDList[comboBoxEdit1.SelectedIndex];
                //启动测点加载
                //m_LoadPointThread = new System.Threading.Thread(new System.Threading.ThreadStart(this.LoadPointList2));
                //m_LoadPointThread.Priority = ThreadPriority.Normal;
                //m_LoadPointThread.Start();
                LoadPointList2(false);
            }
            else
            {
                comboBoxEdit2.Properties.Items.Clear();
                comboBoxEdit2.Enabled = false;
                comboBoxEdit2.Text = "没有数据";
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
                string seriesname1 = seriesNameList.Find(o => o.Contains("最大值"));
                if (seriesname1 != null)
                {
                    LineSeriesView lineview1 = (LineSeriesView)chart.Series[seriesname1].View;
                    lineview1.MarkerVisibility = DefaultBoolean.True;
                    lineview1.LineMarkerOptions.Size = 10;
                }

                string seriesname2 = seriesNameList.Find(o => o.Contains("最小值"));
                if (seriesname2 != null)
                {
                    LineSeriesView lineview2 = (LineSeriesView)chart.Series[seriesname2].View;
                    lineview2.MarkerVisibility = DefaultBoolean.True;
                    lineview2.LineMarkerOptions.Size = 10;
                }

                string seriesname3 = seriesNameList.Find(o => o.Contains("平均值"));
                if (seriesname3 != null)
                {
                    LineSeriesView lineview3 = (LineSeriesView)chart.Series[seriesname3].View;
                    lineview3.MarkerVisibility = DefaultBoolean.True;
                    lineview3.LineMarkerOptions.Size = 10;
                }

                string seriesname4 = seriesNameList.Find(o => o.Contains("监测值"));
                if (seriesname4 != null)
                {
                    LineSeriesView lineview4 = (LineSeriesView)chart.Series[seriesname4].View;
                    lineview4.MarkerVisibility = DefaultBoolean.True;
                    lineview4.LineMarkerOptions.Size = 10;
                }

                string seriesname5 = seriesNameList.Find(o => o.Contains("移动值"));
                if (seriesname5 != null)
                {
                    LineSeriesView lineview5 = (LineSeriesView)chart.Series[seriesname5].View;
                    lineview5.MarkerVisibility = DefaultBoolean.True;
                    lineview5.LineMarkerOptions.Size = 10;
                }
            }
            else
            {
                string seriesname1 = seriesNameList.Find(o => o.Contains("最大值"));
                if (seriesname1 != null)
                {
                    LineSeriesView lineview1 = (LineSeriesView)chart.Series[seriesname1].View;
                    lineview1.MarkerVisibility = DefaultBoolean.False;
                }

                string seriesname2 = seriesNameList.Find(o => o.Contains("最小值"));
                if (seriesname2 != null)
                {
                    LineSeriesView lineview2 = (LineSeriesView)chart.Series["最小值"].View;
                    lineview2.MarkerVisibility = DefaultBoolean.False;
                }

                string seriesname3 = seriesNameList.Find(o => o.Contains("平均值"));
                if (seriesname3 != null)
                {
                    LineSeriesView lineview3 = (LineSeriesView)chart.Series[seriesname3].View;
                    lineview3.MarkerVisibility = DefaultBoolean.False;
                }

                string seriesname4 = seriesNameList.Find(o => o.Contains("监测值"));
                if (seriesname4 != null)
                {
                    LineSeriesView lineview4 = (LineSeriesView)chart.Series[seriesname4].View;
                    lineview4.MarkerVisibility = DefaultBoolean.False;
                }

                string seriesname5 = seriesNameList.Find(o => o.Contains("移动值"));
                if (seriesname5 != null)
                {
                    LineSeriesView lineview5 = (LineSeriesView)chart.Series[seriesname5].View;
                    lineview5.MarkerVisibility = DefaultBoolean.False;
                }
            }
        }

        private void chart_CustomDrawSeriesPoint(object sender, CustomDrawSeriesPointEventArgs e)
        {
            //只对测点1做突出显示
            if (e.Series.Name != Series1_1.Name)
                return;

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